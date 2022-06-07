using Grpc.Core;
using System.Collections.Generic;
using ChattingGrpc;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using ServerLib.Database.Mysql.Context;
using System.Threading.Tasks;
using ServerLib.Database.Mysql.Dto.User;
using API.Grpc.Model;

namespace API.Grpc
{
    public class ChatRoomService
    {
        private readonly MysqlDbContext _mysqlDbContext;

        public static SortedDictionary<long, ChattingUserModel> _chattingUserModelDict { get; set; } = new SortedDictionary<long, ChattingUserModel>();

        public static SortedDictionary<int, SortedSet<long>> _chattingRoomDict { get; set; } = new SortedDictionary<int, SortedSet<long>>();

        public ChatRoomService(
                MysqlDbContext mysqlDbContext
            )
        {
            _mysqlDbContext = mysqlDbContext;
        }

        /// <summary>
        /// 참가 가능한 룸 가져오기
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public async Task<int> GetRoomIdAsync(long playerId)
        {
            var roomId = _chattingRoomDict.FirstOrDefault(x => x.Value.Count < 10).Key;

            //유효한 유저인지 확인
            var userDataDto = await _mysqlDbContext.FindAsync<UserDataDto>(playerId);
            if (userDataDto == null)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, ""));
            }

            if (roomId == 0)
            {
                roomId = 1;
                _chattingRoomDict.Add(roomId, new SortedSet<long>());
            }

            _chattingRoomDict[roomId].Add(playerId);

            return roomId;
        }

        /// <summary>
        /// 룸에 참가
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="responseStream"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public async Task JoinRoomAsync(long playerId, IAsyncStreamWriter<ChatRes> responseStream)
        {
            var isRoomJoin = false;
            int roomId = 0;
            foreach (var (key,value) in _chattingRoomDict)
            {
                if(value.FirstOrDefault(x => x == playerId) != 0)
                {
                    isRoomJoin = true;
                    roomId = key;
                }
            }

            //유효하지 않은 룸
            if(!isRoomJoin)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, ""));
            }

            //캐쉬에 유저 정보 넣기
            if (!_chattingUserModelDict.TryGetValue(playerId, out var chattingUserModel))
            {
                var userDataDto = await _mysqlDbContext.FindAsync<UserDataDto>(playerId);
                if (userDataDto == null)
                {
                    throw new RpcException(new Status(StatusCode.PermissionDenied, ""));
                }

                chattingUserModel = new ChattingUserModel
                {
                    PlayerId = userDataDto.PlayerId,
                    Name = userDataDto.PlayerName,
                    RoomId = roomId,
                    Stream = responseStream
                };

                _chattingUserModelDict.Add(playerId, chattingUserModel);
            }
        }

        /// <summary>
        /// 룸에서 나가기 및 정보 삭제
        /// </summary>
        /// <param name="playerId"></param>
        /// <exception cref="RpcException"></exception>
        public void LeaveRoom(long playerId)
        {
            if (!_chattingUserModelDict.TryGetValue(playerId, out var chattingUserModel))
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, ""));
            }

            _chattingRoomDict[chattingUserModel.RoomId].Remove(playerId);

            _chattingUserModelDict.Remove(playerId);
        }

        public async Task BroadcastMessageAsync(ChatReq req)
        {
            var playerId = req.PlayerId;
            if (!_chattingUserModelDict.TryGetValue(playerId, out var chattingUserModel))
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, ""));
            }

            var roomId = chattingUserModel.RoomId;

            var targetPlayerIds = _chattingRoomDict[chattingUserModel.RoomId].Where(x => x != playerId).ToList();
            var targetChattingUserModelList = _chattingUserModelDict.Where(x => x.Key != playerId).Select(x => x.Value).ToList();

            foreach (var targetChattingUserModel in targetChattingUserModelList)
            {                
                await chattingUserModel.Stream.WriteAsync(new ChatRes
                {
                    PlayerId = playerId,
                    PlayerName = chattingUserModel.Name,
                    Message = req.Message,
                    Type = req.Type
                });
            }            
        }
    }
}
