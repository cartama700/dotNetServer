using API.Di;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// 기본적인 베이스 컨트롤
    /// </summary>
    public class BaseController : Controller
    {
        public TotalDi _totalDi { get; set; }

        /// <summary>
        /// 요청 들어온 플레이어 아이디
        /// </summary>
        public long _playerId { get; set; }

        public BaseController(
            TotalDi totalDi
        )
        {
            _totalDi = totalDi;
            _playerId = totalDi._playerDi.UserData.PlayerId;
        }
    }
}
