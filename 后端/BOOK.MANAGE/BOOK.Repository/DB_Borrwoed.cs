using BOOK.DB;
using BOOK.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    public class DB_Borrwoed
    {
        #region 依赖注入
        private readonly BOOK.DB.BooksContext Ctx;

        public DB_Borrwoed(BooksContext context)
        {
            this.Ctx = context;
        }
        #endregion

        #region 用户借阅图书
        public bool Borrwoed(int BID, int UID)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    // 判断传过来的id是否都合法
                    var book = Ctx.Books.Where(c => c.Id == BID).FirstOrDefault();
                    var sysuer = Ctx.SysUsers.Where(c => c.Id == UID).FirstOrDefault();
                    if (book == null || sysuer == null)
                    {
                        return false; // 数据不合法
                    }
                    else
                    {
                        var borrowed = new BOOK.MODEL.Borrowed();
                        borrowed.BID = BID;
                        borrowed.UID = UID;
                        borrowed.BorrowedTime = DateTime.Now;
                        borrowed.State = false;// 借出状态
                        Ctx.Borroweds.Add(borrowed);
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

        #region 判断用户是否借阅当前图书
        public bool IsBorrowed(int BID,int UID)
        {
            try
            {
                var result = Ctx.Borroweds.Where(c=>c.UID== UID&&c.BID == BID).FirstOrDefault();
                if (result == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
