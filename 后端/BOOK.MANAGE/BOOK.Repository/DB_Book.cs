using BOOK.DB;
using BOOK.MODEL.Exception;
using System.Data.Common;
using DbException = BOOK.MODEL.Exception.DbException;

namespace BOOK.Repository
{
    public class DB_Book
    {
        private readonly BOOK.DB.BooksContext Ctx;

        public DB_Book(BooksContext ctx)
        {
            Ctx = ctx;
        }

        #region 获取全部图书信息
        public IEnumerable<BOOK.MODEL.Book> GetBook()
        {
            try 
            {
                var listbook = Ctx.Books.ToList();
                return listbook;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取单本书的信息
        public BOOK.MODEL.Book GetById(int id) 
        {
            try
            {
                var dbbook = Ctx.Books.Where(c => c.Id == id).FirstOrDefault();
                if (dbbook == null) throw new DbException("数据库没有相关数据");
                return dbbook;
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
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    var db_book = Ctx.Books.Where(c => c.Id == book.Id).FirstOrDefault();
                    if (db_book == null) throw new DbException("查找的值为空");

                    db_book.ISBN = book.ISBN; // ISBN
                    db_book.Press = book.Press;// 出版社
                    db_book.Inventory = book.Inventory; // 库存
                    db_book.Author = book.Author; // 作者
                    db_book.Image = book.Image;// 照片
                    db_book.BookName = book.BookName;

                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback(); return false;
                }
            }
        }
        #endregion

        #region 添加图书
        public bool InstallBook(BOOK.MODEL.Book book)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    // 由于ISBN编码具有唯一性 需要把他当作主键来看待 所以需要对ISBN进行查重
                    BOOK.MODEL.Book db_book = Ctx.Books.Where(c => c.ISBN == book.ISBN).FirstOrDefault();
                    if (db_book != null) throw new DbException("ISBN错误，数据库已有相同数据");
                    book.AddTime = DateTime.Now;
                    Ctx.Add(book);
                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        #endregion

        #region 删除图书
        public bool DelBook(BOOK.MODEL.Book book)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    var db_book = Ctx.Books.Where(c => c.Id == book.Id).FirstOrDefault();
                    if (db_book == null) throw new DbException("查找的数据为空");
                    Ctx.Books.Remove(db_book);
                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        #endregion

    }
}
