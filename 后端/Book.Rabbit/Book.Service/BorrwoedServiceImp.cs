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
                bool result = _dbBorrwoed.IsBorrwoed(borrowed);
                if (result)
                {
                    borrowed.Book = null;
                    redis_borrowed.Install(borrowed);
                    return true;
                }
                else
                {
                    throw new Exception("数据库添加失败");
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
