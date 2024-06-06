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
    [Authorize]
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
    }
}
