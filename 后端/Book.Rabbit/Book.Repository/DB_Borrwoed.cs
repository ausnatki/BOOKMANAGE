using Book.DataAccess;
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
        public bool IsBorrwoed(Book.Model.Borrowed borrowed)
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
                        return false; // 数据不合法
                    }
                    else
                    {
                        Ctx.Borroweds.Add(borrowed);
                        Ctx.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
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
