using Book.Repository;
using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class BorrwoedServiceImp:IBorrowedService
    {
        private readonly DB_Borrwoed _dbBorrwoed;
        private readonly Redis_Borrowed redis_borrowed;

        public BorrwoedServiceImp(DB_Borrwoed dbBorrwoed, Redis_Borrowed redis_borrowed)
        {
            _dbBorrwoed = dbBorrwoed;
            this.redis_borrowed = redis_borrowed;
        }

        #region 借阅
        public bool BorrowBook(Book.Model.Borrowed borrowed)
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
                    if (borrowed.BID == 0 || borrowed.UID == 0)
                        throw new Exception("传值错误");

                    // 首先对数据库进行相关的操作
                    var result = _dbBorrwoed.IsBorrwoed(borrowed);
                    redis_borrowed.Install(result);
                    return true;
                }
                catch (Exception ex)
                {
                    // 这里可以选择是否重新抛出异常或者记录日志
                    Console.WriteLine($"借书失败: {ex.Message}");
                    throw;  // 重新抛出异常以触发 Polly 的重试
                }
            });
        }
        #endregion

        #region 用户归还
        public bool Repiad(Book.Model.Borrowed borrowed)
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
                    // 如果查询出来有相应的值 就更新我的redis数据库
                    if (_dbBorrwoed.repiad(borrowed))
                    {
                        borrowed.State = true;
                        redis_borrowed.UpdataBorrowed(borrowed);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // 这里可以选择是否重新抛出异常或者记录日志
                    Console.WriteLine($"还书失败: {ex.Message}");
                    throw;  // 重新抛出异常以触发 Polly 的重试
                }
            });
        }
        #endregion
    }
}
