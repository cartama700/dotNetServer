using System;
using System.IO;
using System.Threading.Tasks;
using ChattingGrpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace API.Grpc
{
    public class ChatService : ChattingProto.ChattingProtoBase
    {
        private readonly ILogger<ChatService> _logger;

        private readonly ChatRoomService _chatRoomService;

        public ChatService(
                ILogger<ChatService> logger,
                ChatRoomService chatRoomService
            )
        {
            _logger = logger;
            _chatRoomService = chatRoomService;
        }

        public override async Task<JoinChatRes> JoinChat(JoinChatReq req, ServerCallContext serverCallContext)
        {
            return new JoinChatRes
            {
                RoomId = await _chatRoomService.GetRoomIdAsync(req.PlayerId)
            };
        }

        public override async Task SendMessageToChatRoom(IAsyncStreamReader<ChatReq> requestStream, IServerStreamWriter<ChatRes> responseStream, ServerCallContext context)
        {
            var playerId = requestStream.Current.PlayerId;
            _logger.LogInformation($"connected PlayerId: {playerId}");

            if (!await requestStream.MoveNext())
            {
                return;
            }

            await _chatRoomService.JoinRoomAsync(playerId, responseStream);

            try
            {
                while (await requestStream.MoveNext())
                {
                    if (!string.IsNullOrEmpty(requestStream.Current.Message))
                    {
                        if (string.Equals(requestStream.Current.Message, "qw!", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        await _chatRoomService.BroadcastMessageAsync(requestStream.Current);
                    }
                }
            }
            catch (IOException)
            {
                _chatRoomService.LeaveRoom(playerId);
                _logger.LogInformation($"Connection for {playerId} was aborted.");
            }

        }
    }
}
