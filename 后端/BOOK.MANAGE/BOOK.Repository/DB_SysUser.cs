using BOOK.DB;
using BOOK.MODEL.DoTempClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        #region 获取所有的用户信息
        public List<SysUserDto> GetAllSysUser()
        {
            try
            {
                var userlist = Ctx.User_role
                                  .Where(c => c.RID != 1 && c.RID != 2)
                                  .Select(c => new SysUserDto
                                  {
                                      Id = c.sysUser.Id,
                                      UserName = c.sysUser.UserName,
                                      LoginName = c.sysUser.LoginName,
                                      Password = c.sysUser.Password,
                                      Email = c.sysUser.Email,
                                      Role = c.RID == 4 ? false : true
                                  })
                                  .ToList();
                return userlist;
            }
            catch (Exception ex)
            {
                // 记录错误日志或采取其他错误处理措施
                // 这里简单地抛出异常或返回一个空列表
                // 您可以根据需求进行更复杂的错误处理
                return null;
            }
        }


        #endregion

        #region 修改用户的身份
        public bool ChangeState(int UID)
        {
            using(var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    // 首先查找我的用户身份
                    var user = Ctx.User_role.Where(c => c.UID == UID).FirstOrDefault();
                    if (user == null) return false;
                    user.RID = user.RID == 4 ? 3 : 4;
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
