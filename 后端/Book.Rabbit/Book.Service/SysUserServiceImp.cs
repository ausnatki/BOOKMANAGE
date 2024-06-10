using Book.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class SysUserServiceImp:ISysUserService
    {

        private readonly Book.Repository.Redis_SysUser redis_SysUser;
        private readonly Book.Repository.DB_SysUser db_SysUser;

        public SysUserServiceImp(Redis_SysUser redis_SysUser, DB_SysUser db_SysUser)
        {
            this.redis_SysUser = redis_SysUser;
            this.db_SysUser = db_SysUser;
        }

        #region 修改用户信息
        public bool EditUserInfo(Book.Model.SysUser user)
        {
            try
            {
                if(db_SysUser.EditUserInfo(user))
                {
                    redis_SysUser.UpdataSysUser(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false ;
            }
        }
        #endregion
    }
}
