using Book.DataAccess;
using DbException = Book.Model.Exception.DbException;

namespace Book.Repository
{
    public class DB_Book
    {
        #region 通过属性进行注入
        //private static readonly DB_Book m_BookService = new DB_Book();
        //private Book.DataAccess.BooksContext Ctx
        //{
        //    get
        //    {
        //        return new Book.DataAccess.BooksContext();
        //    }
        //}

        //public static DB_Book Instance
        //{
        //    get { return m_BookService; }
        //}
        #endregion

        #region 依赖注入
        private readonly Book.DataAccess.BooksContext Ctx;

        public DB_Book(BooksContext context)
        {
            this.Ctx = context;
        }
        #endregion

        #region 获取全部图书信息
        public IEnumerable<Book.Model.Book> GetBook()
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
        public object GetById(int id)
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
        public bool Edit(Book.Model.Book book)
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
        public bool InstallBook(Book.Model.Book book)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    // 由于ISBN编码具有唯一性 需要把他当作主键来看待 所以需要对ISBN进行查重
                    Book.Model.Book db_book = Ctx.Books.Where(c => c.ISBN == book.ISBN).FirstOrDefault();
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
        public bool DelBook(int id)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    var db_book = Ctx.Books.Where(c => c.Id == id).FirstOrDefault();
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

        #region 获取单个图书的库存
        public int GetOutInventory(int id)
        {
            try
            {
                int result = Ctx.Borroweds.Where(c => c.BID == id).Count();
                return result;
            }
            catch
            {
                throw new Exception();
            }

        }
        #endregion
    }
}
