using Book.Model.Exception;
using Book.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class BookSerivceImp : IBookService
    {
        private readonly Book.Repository.DB_Book dB_Book;
        private readonly Book.Repository.Redis_Book redis_Book;

        public BookSerivceImp(DB_Book dB_Book, Redis_Book redis_Book)
        {
            this.dB_Book = dB_Book;
            this.redis_Book = redis_Book;
        }

        #region 获取全部图书信息
        public IEnumerable<Book.Model.Book> GetBook()
        {
            var Booklist = new List<Book.Model.Book>();
            try
            {
                // 先执行redis中的查询操作然后通过判断redis中是否拥有数据 如果没有数据的话就需要通过数据库来进行相关数据库的查询
                var redisBook = redis_Book.GetBooks();
                if (redisBook.Count() > 0)
                {
                    Booklist = redisBook.ToList();
                    return Booklist;
                }
                else
                {
                    // 执行通过数据库里面进项查询
                    // 并且将数据库里面查询的数据添加到redis中去
                    var dbBook = dB_Book.GetBook();
                    if (dbBook != null)
                    {
                        foreach (var t in dbBook)
                        {
                            redis_Book.UpdateBook(t); // 更新图书信息
                        }
                    }
                }
                return Booklist;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取单本书的信息
        public object GetById(int id)
        {
            try
            {

                // 首先从redis里面查询数据 如果没有查找到就从数据库里面查询
                var redisBook = redis_Book.GetBooksById(id);
                object Book = redisBook;

                if (redisBook == null)
                {
                    var dbBook = dB_Book.GetById(id);
                    Book = dbBook;
                }
                return Book;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 修改图书信息
        public bool Edit(Book.Model.Book Book)
        {
            try
            {
                bool result = dB_Book.Edit(Book);
                if (result)
                {
                    redis_Book.UpdateBook(Book);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 添加图书
        public bool InstallBook(Book.Model.Book Book)
        {
            try
            {
                var reuslt = dB_Book.InstallBook(Book);
                if (reuslt)
                {
                    redis_Book.NewBook(Book);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 删除图书
        public bool DelBook(int id)
        {
            try
            {
                var reuslt = dB_Book.DelBook(id);
                if (reuslt)
                {
                    redis_Book.DeleteBook(id);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取单本书的库存
        public int GetOutInventory(int id)
        {
            try
            {
                // 直接查询借阅表
                var result = dB_Book.GetOutInventory(id);
                return result;
            }
            catch
            {
                throw new DbException("数据库错误");
            }
        }
        #endregion
    }
}
