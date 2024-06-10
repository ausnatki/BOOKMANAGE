using BOOK.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    
    public class DB_SysUser
    {
        private readonly BOOK.DB.BooksContext Ctx;

        public DB_SysUser(BooksContext ctx)
        {
            Ctx = ctx;
        }

        #region 获取单个用户的信息
        public BOOK.MODEL.SysUser GetInfoByID(int id)
        {
            try 
            {
                var user = Ctx.SysUsers.Where(c=>c.Id == id).FirstOrDefault();
                return user;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
