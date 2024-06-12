using Book.DataAccess;
using Book.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Book.Repository
{
    public class DB_Borrwoed
    {
        #region 依赖注入
        private readonly Book.DataAccess.BooksContext Ctx;

        public DB_Borrwoed(BooksContext context)
        {
            this.Ctx = context;
        }
        #endregion

        #region 用户借阅图书
        public Book.Model.Borrowed IsBorrwoed(Book.Model.Borrowed borrowed)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    borrowed.Book = null;
                    // 判断传过来的id是否都合法
                    var book = Ctx.Books.Where(c => c.Id == borrowed.BID).FirstOrDefault();
                    var sysuer = Ctx.SysUsers.Where(c => c.Id == borrowed.UID).FirstOrDefault();
                    if (book == null || sysuer == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        Ctx.Borroweds.Add(borrowed);
                        Ctx.SaveChanges();
                        transaction.Commit();
                       // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                        Book.Model.Borrowed reuslt = Ctx.Borroweds.OrderByDescending(c => c.Id)
                             .Select(c => new Borrowed // 直接创建Borrowed对象  
                             {
                                 Id = c.Id,
                                 UID = c.UID,
                                 BID = c.BID,
                                 BorrowedTime = c.BorrowedTime,
                                 State = c.State,
                                 SysUser = c.SysUser != null ? new SysUser // 如果SysUser不为null，则创建一个SysUser对象  
                                 {

                                     Id = c.SysUser.Id, // 假设SysUser有Id属性  
                                     UserName = c.SysUser.UserName
                                 } : null,
                                 Book = c.Book != null ? new Model.Book // 如果Book不为null，则创建一个Book对象  
                                 {

                                     Id = c.Book.Id, // 假设Book有Id属性  
                                     BookName = c.Book.BookName,
                                     Author = c.Book.Author
                                 } : null
                             }).FirstOrDefault();
                        // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                        return reuslt;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception();
                }
            }
        }
        #endregion

        #region 用户归还
        public bool repiad(Book.Model.Borrowed borrowed)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {

                try
                {
                    var borrow = Ctx.Borroweds.Where(c => c.Id == borrowed.Id).FirstOrDefault();
                    if (borrow == null) return false;
                    borrow.State = true;
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
