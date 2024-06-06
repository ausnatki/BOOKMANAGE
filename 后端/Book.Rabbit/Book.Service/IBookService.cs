using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public interface IBookService
    {

        public IEnumerable<Book.Model.Book> GetBook();// 获取全部图书
        public object GetById(int id);
        public bool Edit(Book.Model.Book book);
        public bool InstallBook(Book.Model.Book book);
        public bool DelBook(int id);
        public int GetOutInventory(int id);
    }
}
