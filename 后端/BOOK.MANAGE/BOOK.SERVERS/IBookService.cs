using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public interface IBookService
    {
        
        public IEnumerable<BOOK.MODEL.Book> GetBook();// 获取全部图书
        public BOOK.MODEL.Book GetById(int id);
        public bool Edit(BOOK.MODEL.Book book);
        public bool InstallBook(BOOK.MODEL.Book book);
        public bool DelBook(BOOK.MODEL.Book book);
    }
}
