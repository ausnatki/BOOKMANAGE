using BOOK.Repository;
using BOOK.SERVERS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BorrowedController : ControllerBase
    {
        private IBorrowedService borrowedService;

        public BorrowedController(IBorrowedService borrowedService)
        {
            this.borrowedService = borrowedService;
        }

        #region 读者借阅
        [HttpPost("BorrowedBook")]
        public BOOK.MODEL.ApiResp BorrowedBook(int BID, int UID)
        {
            var result = new BOOK.MODEL.ApiResp();
            // 传值错误时返回错误结果
            if (BID == 0|| UID == 0)
            {
                result.Code = 500;
                result.Msg = "传值错误";
                result.Result = false;
                return result;
            }
            if(borrowedService.BorrowBook(BID, UID)) 
            {
                result.Code = 20000;
                result.Result = true;
                result.Msg = "借阅成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Result = false;
                result.Msg = "借阅失败";
                return result;
            }
        }
        #endregion

        #region 判断用户是否借阅指定图书
        [HttpPost("IsBorrowed")]
        public BOOK.MODEL.ApiResp IsBorrowed(int BID, int UID)
        {
            var result = new BOOK.MODEL.ApiResp();
            // 传值错误时返回错误结果
            if (BID == 0 || UID == 0)
            {
                result.Code = 500;
                result.Msg = "传值错误";
                result.Result = false;
                return result;
            }

            result.Code = 20000;
            result.Result =  borrowedService.IsBorrowed(BID, UID);
            return result;
        }
        #endregion

        #region 获取当前用户的所有借阅信息
        [HttpGet("GetBorrowed")]
        public BOOK.MODEL.ApiResp GetBorrowed(int UID)
        {
            var result = new BOOK.MODEL.ApiResp();
            if(UID == 0)
            {
                result.Code = 500;
                result.Data = null;
                result.Result = false;
                result.Msg = "传值错误查询失败";
                return result;
            }

            var borrowedlist = borrowedService.GetBorrowed(UID);
            result.Data = borrowedlist;
            result.Result = true;
            result.Msg = "查询成功";
            result.Code = 20000;
            return result;
        }
        #endregion

        #region 归还
        [HttpPost("Repiad")]
        public BOOK.MODEL.ApiResp Repiad([FromBody] BOOK.MODEL.Borrowed borrowed)
        {
            var result = new BOOK.MODEL.ApiResp();
            if(borrowed.Id == 0)
            {
                result.Code = 500;
                result.Msg = "传值错误，归还失败";
                result.Result = false;
                return result;
            }

            if (borrowedService.Repiad(borrowed))
            {
                result.Code = 20000;
                result.Msg = "归还成功";
                result.Result = true;
                return result;
            }
            else
            {
                result.Code = 500;
                result.Msg = "程序错误，归还失败";
                result.Result = false;
                return result;
            }
        }
        #endregion

        #region 获取所有借阅信息

        [HttpGet("GetAllList")]
        public BOOK.MODEL.ApiResp GetAllList()
        {
            var result = new BOOK.MODEL.ApiResp();
            try 
            {
                result.Code = 20000;
                result.Data = borrowedService.GetAllList();
                result.Result = true;
                result.Msg = "查找成功";
                return result;
            } 
            catch 
            {
                result.Code = 500;
                result.Result= false;
                result.Msg = "服务器错误，查找失败";
                return result;
            }
        }
        #endregion
    }
}
