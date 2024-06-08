using BOOK.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    public class Redis_Borrowed
    {
        private string m_BorrowedHashName = "BorrowedHash";

        public StackExchange.Redis.ConnectionMultiplexer GetConnection()
        {
            string connectionString = "localhost:6379,password=123456,abortConnect=false";
            var connection = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            return connection;
        }

        #region 更新的业务逻辑
        public bool UpdataBorrowed(BOOK.MODEL.Borrowed borrowed)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase();
                    this.RefreshExpiredTime(db);// 设置缓存时间
                    var n = db.HashSet(m_BorrowedHashName, borrowed.Id, System.Text.Json.JsonSerializer.Serialize(borrowed));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 添加的逻辑
        public bool Install(int BID, int UID)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var borrowed = new BOOK.MODEL.Borrowed();
                    var db = connection.GetDatabase(); // 获取连接数据
                    this.RefreshExpiredTime(db);

                    // 获取redis中所有的数据然后再将里面的数据加1就变成了我的id
                    var key = db.HashKeys(m_BorrowedHashName);
                    int count = key.Length;
                    borrowed.Id = count + 1;
                    borrowed.UID = UID;
                    borrowed.BID = BID;
                    borrowed.State = false;// 标记为未还
                    var n = db.HashSet(m_BorrowedHashName, borrowed.Id, System.Text.Json.JsonSerializer.Serialize(borrowed));// 添加到redis
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取当前用户的借阅列表
        public IEnumerable<object> GetBorrowed(int UID)
        {
            var borrwoedlist = new List<BOOK.MODEL.Borrowed>();

            // 获取redis连接
            using(var connection = GetConnection())
            {
                // 获取redis的数据连接
                var db = connection.GetDatabase();

                this.RefreshExpiredTime(db);// 设置缓存时间

                // 获取键值对名字
                var keys = db.HashKeys(m_BorrowedHashName);

                foreach(var key in keys)
                {
                    var json = db.HashGet(m_BorrowedHashName,key);

                    if (!string.IsNullOrEmpty(json))
                    {
                        var borrowed = System.Text.Json.JsonSerializer.Deserialize<BOOK.MODEL.Borrowed>(json);
                        if(borrowed != null)
                        {
                            borrwoedlist.Add(borrowed);
                        }
                    }
                }
            }
            return borrwoedlist;
        }
        #endregion

        /// <summary>
        /// 设置的缓存时间
        /// </summary>
        /// <param name="db"></param>
        public void RefreshExpiredTime(StackExchange.Redis.IDatabase db)
        {
            db.KeyExpire(m_BorrowedHashName, new TimeSpan(0, 5, 0));// 将过期缓存设置为5分钟的时限
        }
    }
}
