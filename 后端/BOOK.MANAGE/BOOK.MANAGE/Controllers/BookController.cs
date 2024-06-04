using BOOK.SERVERS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;

        public BookController(BookSerivceImp bookService)
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
            if (book != null)
            {
                result.Code = 20000;
                result.Data = book;
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
        public BOOK.MODEL.ApiResp DelBook([FromBody] BOOK.MODEL.Book book)
        {
            var result = new BOOK.MODEL.ApiResp();
            if (_bookService.DelBook(book))
            {
                result.Code = 20000;
                result.Data = book;
                result.Result = true;
                result.Msg = "删除成功";
                return result;
            }
            else
            {
                result.Code = 500;
                result.Data = book;
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
    }
}
