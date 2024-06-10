using BOOK.SERVERS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserController : ControllerBase
    {
        private BOOK.SERVERS.ISysUserService _userService;

        public SysUserController(ISysUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("GetInfoById")]
        public BOOK.MODEL.ApiResp GetInfoById(int id)
        {
            var result = new BOOK.MODEL.ApiResp();
            if(id == 0) 
            {
                result.Code = 500;
                result.Data = null;
                result.Result = false;
                result.Msg = "查找失败，传值错误";
                return result;
            }else
            {
                var user = _userService.GetUserInfo(id);
                if(user == null )
                {
                    result.Code = 500;
                    result.Data = null;
                    result.Result = false;
                    result.Msg = "查找失败，服务器错误";
                    return result;
                }
                else
                {
                    result.Code = 20000;
                    result.Data = user;
                    result.Result = true;
                    result.Msg = "查找成功";
                    return result;
                }
            }
        }

        [HttpPost("EditInfo")]
        public BOOK.MODEL.ApiResp EditInfo([FromBody]BOOK.MODEL.SysUser user) 
        {
            var result = new BOOK.MODEL.ApiResp();
            if(user.Id == 0) 
            {
                result.Code = 500;
                result.Data = user;
                result.Result = false;
                result.Msg = "传值错误，修改错误";
                return result;
            }
            else
            {
                if (_userService.EditUserInfo(user))
                {
                    result.Code = 20000;
                    result.Data = user;
                    result.Result = true;
                    result.Msg = " 用户修改成功";
                    return result;
                }
                else
                {
                    result.Code = 500;
                    result.Data = user;
                    result.Result = false;
                    result.Msg = "用户修改失败";
                    return result;
                }
            }
        }
    }
}
