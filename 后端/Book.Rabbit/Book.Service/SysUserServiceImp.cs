using Book.Repository;
using Polly.Retry;
using Polly;
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
            // 定义一个带有重试策略的 Polly 策略，最多重试 3 次，每次重试间隔 2 秒
            RetryPolicy retryPolicy = Policy
                .Handle<Exception>()  // 捕捉所有异常
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(2),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        // 这里可以添加日志记录或其他处理逻辑
                        Console.WriteLine($"重试次数: {retryCount}，异常: {exception.Message}");
                    });

            return retryPolicy.Execute(() =>
            {
                try
                {
                    if (db_SysUser.EditUserInfo(user))
                    {
                        //throw new Exception();
                        redis_SysUser.UpdataSysUser(user);
                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // 这里可以选择是否重新抛出异常或者记录日志
                    Console.WriteLine($"编辑用户信息失败: {ex.Message}");
                    throw;  // 重新抛出异常以触发 Polly 的重试
                }
            });
        }
        #endregion
    }
}
