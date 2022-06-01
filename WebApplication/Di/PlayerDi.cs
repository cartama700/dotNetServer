using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;
using ServerLib.Utill;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Di
{
    /// <summary>
    /// 플레이어 정보 종속성 주입
    /// </summary>
    public class PlayerDi
    {
        private readonly HttpContext _httpContext;

        private readonly MysqlDbContext _mysqlDbContext;

        public readonly UserDataDto UserData = null;

        /// <summary>
        /// 마지막 슬롯 번호
        /// </summary>
        private ushort _lastSlotNum = 0;

        public PlayerDi(IHttpContextAccessor httpContextAccessor, MysqlDbContext mysqlDbContext)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _mysqlDbContext = mysqlDbContext;

            var playerId = PlayerDataUtill.GetPlayerId(_httpContext.Request.Headers);
            if (playerId == null)
            {
                throw new BadHttpRequestException("플레이어가 존재하지 않습니다.", (int)HttpStatusCode.Unauthorized);
            }

            UserData = _mysqlDbContext.Find<UserDataDto>(playerId);
            if (UserData == null)
            {
                throw new BadHttpRequestException("유저를 생성 후 해주세요.", (int)HttpStatusCode.Unauthorized);
            }
        }

        /// <summary>
        /// 마지막 슬롯번호를 가져온 후 프로퍼티에 넣기 
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public async Task<ushort> GetLastSlotAsync(bool isUse = false)
        {
            if (_lastSlotNum == 0)
            {
                _lastSlotNum = await _mysqlDbContext.UserItemDtos
                       .Where(x => x.PlayerId == UserData.PlayerId)
                       .OrderByDescending(x => x.Slot)
                       .Select(x => (ushort)(x.Slot + 1))
                       .SingleOrDefaultAsync();
            }

            return isUse ? _lastSlotNum++ : _lastSlotNum;
        }
    }
}
