using BOOK.DB;
using BOOK.MODEL.Exception;
using BOOK.Repository;
using System.Reflection.Metadata.Ecma335;

namespace BOOK.SERVERS
{
    public class BookSerivceImp : IBookService
    {
        private readonly BOOK.Repository.DB_Book dB_Book;
        private readonly BOOK.Repository.Redis_Book redis_Book;

        public BookSerivceImp(DB_Book dB_Book, Redis_Book redis_Book)
        {
            this.dB_Book = dB_Book;
            this.redis_Book = redis_Book;
        }

        #region 获取全部图书信息
        public IEnumerable<BOOK.MODEL.Book> GetBook()
        {
            var booklist = new List<BOOK.MODEL.Book>();
            try 
            {
                // 先执行redis中的查询操作然后通过判断redis中是否拥有数据 如果没有数据的话就需要通过数据库来进行相关数据库的查询
                var redisBook = redis_Book.GetBooks();
                if (redisBook.Count() > 0)
                {
                    booklist = redisBook.ToList();
                    return booklist;
                }
                else
                {
                    // 执行通过数据库里面进项查询
                    // 并且将数据库里面查询的数据添加到redis中去
                    var dbBook = dB_Book.GetBook();
                    if(dbBook!=null)
                    {
                        foreach(var t in dbBook)
                        {
                            redis_Book.UpdateBook(t); // 更新图书信息
                        }
                    }
                }
                return booklist;
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
                object book = redisBook;

                if (redisBook == null)
                {
                    var dbBook = dB_Book.GetById(id);
                    book = dbBook;
                }
                return book;
            } 
            catch 
            {
                return null;
            }
        }
        #endregion

        #region 修改图书信息
        public bool Edit(BOOK.MODEL.Book book)
        {
            try 
            {
                bool result = dB_Book.Edit(book);
                if (result)
                {
                    redis_Book.UpdateBook(book);
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
        public bool InstallBook(BOOK.MODEL.Book book)
        {
            try 
            {
                var reuslt = dB_Book.InstallBook(book);
                if (reuslt)
                {
                    redis_Book.NewBook(book);
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
        public bool DelBook(BOOK.MODEL.Book book) 
        {
            try 
            {
                var reuslt = dB_Book.DelBook(book);
                if (reuslt)
                {
                    redis_Book.DeleteBook(book.Id);
                }
                return true;
            } 
            catch
            {
                return false ;
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
