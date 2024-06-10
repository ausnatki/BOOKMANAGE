using BOOK.Repository;
using BOOK.SERVERS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics.CodeAnalysis;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookController : ControllerBase
    {

        private readonly BOOK.SERVERS.IBookService _bookService;

        public BookController(BOOK.SERVERS.IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("GetBook")]
        public BOOK.MODEL.ApiResp GetBook()
        {
            var result = new BOOK.MODEL.ApiResp();
            var booklist = _bookService.GetBook();
            if (booklist != null)
            {
                result.Code = 20000;
                result.Data = booklist;
                result.Msg = "查询成功";
                result.Result = true;
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = null;
                result.Msg = "查询失败";
                result.Result = false;
                return result;
            }
        }

        [HttpGet("GetById")]
        public BOOK.MODEL.ApiResp GetById(int id)
        {
            var result = new BOOK.MODEL.ApiResp();
            var book = _bookService.GetById(id);
            var OutInventory = _bookService.GetOutInventory(id);
            if (book != null)
            {
                result.Code = 20000;
                result.Data = new { book ,OutInventory};
                result.Result = true;
                result.Msg = "查找成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = null;
                result.Result = false;
                result.Msg = "查询失败";
                return result;
            }
        }

        [HttpPost("DelBook")]
        public BOOK.MODEL.ApiResp DelBook(int id)
        {
            var result = new BOOK.MODEL.ApiResp();
            if (_bookService.DelBook(id))
            {
                result.Code = 20000;
                result.Data = id;
                result.Result = true;
                result.Msg = "删除成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = id;
                result.Result = false;
                result.Msg = "删除失败";
                return result;
            }
        }

        [HttpPost("AddBook")]
        public BOOK.MODEL.ApiResp AddBook([FromBody] BOOK.MODEL.Book book)
        {
            var result = new BOOK.MODEL.ApiResp();

            if (_bookService.InstallBook(book))
            {
                result.Code = 20000;
                result.Data = book;
                result.Result = true;
                result.Msg = "添加成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = book;
                result.Result = false;
                result.Msg = "添加失败";
                return result;
            }
        }

        [HttpPost("EditBook")]
        public BOOK.MODEL.ApiResp EditBook([FromBody] BOOK.MODEL.Book book)
        {
            var result = new BOOK.MODEL.ApiResp();
            if (_bookService.Edit(book))
            {
                result.Data = book;
                result.Result = true;
                result.Code = 20000;
                result.Msg = "修改成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = book;
                result.Result = false;
                result.Msg = "修改失败";
                return result;
            }
        }




        /// <summary>
        /// 下面事管理员操作的相关图书 业务
        /// </summary>
        /// <returns></returns>
        

        [HttpGet("GetAllBookAdmin")]
        public BOOK.MODEL.ApiResp GetAllBookAdmin()
        {
            var result = new BOOK.MODEL.ApiResp();
            var booklist = _bookService.GetAllBookAdmin();
            if (booklist == null)
            {
                result.Code = 500;
                result.Msg = "查询失败";
                result.Result = false;
                return result;
            }
            else
            {
                result.Code = 20000;
                result.Data = booklist;
                result.Result = true;
                result.Msg = "查询成功";
                return result;
            }
        }

        [HttpGet("GetBorrowByBid")]
        public BOOK.MODEL.ApiResp GetBorrowByBid(int BID)
        {
            var result = new BOOK.MODEL.ApiResp();
            try 
            {
                
                result.Data = _bookService.GetBorrowedByBid(BID);
                result.Code = 20000;
                result.Msg = "查找成功";
                result.Result = true;
                return result;
            } 
            catch 
            {
                result.Code = 500;
                result.Result = false;
                result.Msg = "发生错误查找失败";
                return result;
            }
        }

        [HttpPost("ChangeState")]
        public BOOK.MODEL.ApiResp ChangeState(int BID)
        {
            var result = new BOOK.MODEL.ApiResp();
            if(BID == 0)
            {
                result.Code = 500;
                result.Msg = "改变失败";
                result.Result = false;
                return result;
            }
            if (_bookService.ChangeState(BID))
            {
                result.Code = 20000;
                result.Result = true;
                result.Msg = "改变成功";
                result.Result = true;
                return result;
            }
            else
            {
                result.Code = 500;
                result.Msg = "改变失败";
                result.Result = false;
                return result;
            }

        }

        [HttpPost("AddInventory")]
        public BOOK.MODEL.ApiResp AddInventory(int BID,int Cnt) 
        {
            var result = new BOOK.MODEL.ApiResp();
            if (BID == 0) 
            {
                result.Code = 500;
                result.Msg = "添加失败";
                result.Result = false;
                return result;
            }

            if(_bookService.AddInventory(BID, Cnt))
            {
                result.Code = 20000;
                result.Msg = "添加成功";
                result.Result = true;
                return result;
            }
            else
            {
                result.Code = 500;
                result.Msg = "添加失败";
                result.Result = true;
                return result;
            }

        }
    }
}
