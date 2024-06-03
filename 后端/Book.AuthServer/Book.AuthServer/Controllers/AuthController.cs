using Book.AuthServer.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
        private readonly ILoginService _loginService;

        public AuthController(LoginServiceImp loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("GetToken")]
        public ActionResult<object> GetToken([FromBody] UserInfo userInfo)
        {
           return _loginService.GetJwt(userInfo.username, userInfo.password);
        }


        [HttpGet("info")]
        public ActionResult<object> GetUserInfo([FromQuery] string token)
        { 
            return _loginService.GetInfoByName(token);
        }
        public class UserInfo
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
