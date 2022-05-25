using API.Di;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class BaseController : Controller
    {
        public TotalDi _totalDi { get; set; }

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
