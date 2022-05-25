using ServerLib.Utill;
using Microsoft.AspNetCore.Http;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dto.User;
using System;
using System.Net;

namespace API.Di
{
    /// <summary>
    /// 플레이어 정보 종속성 주입
    /// </summary>
    public class PlayerDi
    {
        public HttpContext _httpContext { get; private set; }

        public MysqlDbContext _mysqlDbContext;

        public UserDataDto UserData { get; private set; } = null;

        public PlayerDi(IHttpContextAccessor httpContextAccessor, MysqlDbContext mysqlDbContext)
        {
            _httpContext = httpContextAccessor.HttpContext;

            var playerId = PlayerDataUtill.GetPlayerId(_httpContext.Request.Headers);
            if(playerId == null)
            {
                throw new BadHttpRequestException("플레이어가 존재하지 않습니다.", (int) HttpStatusCode.Unauthorized);
            }

            UserData = mysqlDbContext.Find<UserDataDto>(playerId);
            if (UserData == null)
            {
                throw new BadHttpRequestException("유저를 생성 후 해주세요.", (int) HttpStatusCode.Unauthorized);
            }
        }
    }
}
