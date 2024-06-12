using Book.Repository;
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
            try
            {
                if (borrowed.BID == 0 || borrowed.UID == 0) throw new Exception("传值错误");
                // 首先对数据库进行相关的操作
                var result = _dbBorrwoed.IsBorrwoed(borrowed);
                    redis_borrowed.Install(result);
                    return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 用户归还
        public bool Repiad(Book.Model.Borrowed borrowed)
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
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
