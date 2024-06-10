using BOOK.DB;
using BOOK.MODEL.Exception;
using System.Data.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Validation;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
                var listbook = Ctx.Books.Where(c=>c.IsDel == false).OrderByDescending(c=>c.Id).ToList();
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

        #region 获取图书库存的信息(管理员）
        public IEnumerable<BOOK.MODEL.DoTempClass.BookInventoryDto> GetAllBookAdmin()
        {
            try
            {
                var booklist = Ctx.Books.Select(c => new BOOK.MODEL.DoTempClass.BookInventoryDto
                {
                    Id = c.Id,
                    BookName = c.BookName,
                    ISBN = c.ISBN,
                    State = c.IsDel,
                    Inventory = c.Inventory,
                    LoanedOut = c.borrowed.Count(o => o.BID == c.Id && !o.State),
                    InStock = c.Inventory - c.borrowed.Count(o => o.BID == c.Id && !o.State)
                }).ToList();

                return booklist;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return Enumerable.Empty<BOOK.MODEL.DoTempClass.BookInventoryDto>();
            }
        }

        #endregion

        #region 修改图书状态 （isdel）（管理员）
        public bool ChangeState(int BID)
        {
            using(var transaction = Ctx.Database.BeginTransaction())
            {
                try 
                {
                    var book = Ctx.Books.Where(c=>c.Id == BID).FirstOrDefault();
                    if (book == null) return false;
                    book.IsDel = (bool)book.IsDel ? false : true;
                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
        }
        #endregion

        #region 具体图书的借阅信息(管理员)
        public IEnumerable<BOOK.MODEL.DoTempClass.BorroweByBidDto> GetBorroweByBid(int BID) 
        {
            var borrowlist = Ctx.Borroweds.Where(c=>c.BID == BID).Select(c => new BOOK.MODEL.DoTempClass.BorroweByBidDto
            {

                BookName = c.Book.BookName,
                BorroweDate = c.BorrowedTime,
                State = c.State,
                UserName = c.SysUser.UserName,
                Email = c.SysUser.Email
            }).ToList();
            return borrowlist;
        }
        #endregion

        #region 添加库存（管理员）
        public bool AddInventory(int BID,int Cnt)
        {
            using(var transaction = Ctx.Database.BeginTransaction())
            {
                try 
                {
                    var book =Ctx.Books.Where(c=>c.Id==BID).FirstOrDefault();
                    if (book == null) return false;
                    book.Inventory += Cnt;
                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
        }
        #endregion


    }
}
