using BOOK.MODEL;
using BOOK.Repository;
using BOOK.SERVERS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("GetAllSysUser")]
        public BOOK.MODEL.ApiResp GetAllSysUser()
        {
            var result = new BOOK.MODEL.ApiResp();
            var userlist = _userService.GetAllUserInfo();
            if (userlist == null)
            {
                result.Code = 500;
                result.Msg = "获取失败";
                result.Data = null;
                result.Result = false;
                return result;
            }
            else
            {
                result.Code = 20000;
                result.Data = userlist;
                result.Result = true;
                result.Msg = "查找成功";
                return result;
            }
        }

        [HttpPost("ChangeState")]
        public BOOK.MODEL.ApiResp ChangeState(int UID)
        {
            var result = new BOOK.MODEL.ApiResp();
            if(UID == 0) 
            {
                result.Code = 500;
                result.Msg = "传值错误，修改失败";
                result.Result = false;
                return result;
            }
            if (_userService.ChangeState(UID))
            {
                result.Code = 20000;
                result.Msg = "修改成功";
                result.Result = true;
                return result;
            }
            else
            {
                result.Code = 500;
                result.Msg = "服务器错误，修改失败";
                result.Result = false;
                return result;
            }
        }

        [HttpPost("Enroll")]

        public ActionResult<object> Enroll([FromBody] SysUser user)
        {
            if (user.UserName == null) { return StatusCode(400, new { code = 50008, message = "注册失败" }); }

            if (_userService.Enroll(user))
            {
                return new { code = 20000 };
            }
            else
            {
                return StatusCode(400, new { code = 50008, message = "注册失败" });
            }
        }
    }
}
