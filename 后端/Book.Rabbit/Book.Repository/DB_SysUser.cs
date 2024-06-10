using Book.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Book.Repository
{
    public class DB_SysUser
    {
        private readonly Book.DataAccess.BooksContext Ctx;

        public DB_SysUser(BooksContext ctx)
        {
            Ctx = ctx;
        }

        #region 修改用户信息
        public bool EditUserInfo(Book.Model.SysUser user)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    var tuser = Ctx.SysUsers.Where(c => c.Id == user.Id).FirstOrDefault();
                    if (tuser == null) return false;
                    tuser.UserName = user.UserName;
                    tuser.Password = user.Password;
                    tuser.Email = user.Email;
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
