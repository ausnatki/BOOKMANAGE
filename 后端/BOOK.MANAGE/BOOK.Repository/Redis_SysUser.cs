using BOOK.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    public class Redis_SysUser
    {
        private string m_SysUserHashName = "SysUserHash";

        public StackExchange.Redis.ConnectionMultiplexer GetConnection()
        {
            string connectionString = "localhost:6379,password=123456,abortConnect=false";
            var connection = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            return connection;
        }

        #region 更新的业务逻辑
        public bool UpdataSysUser(SysUser sysUser)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase();
                    this.RefreshExpiredTime(db);// 设置缓存时间
                    var n = db.HashSet(m_SysUserHashName, sysUser.Id, System.Text.Json.JsonSerializer.Serialize(sysUser));
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
        public bool Install(SysUser sysUser)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase(); // 获取连接数据
                    this.RefreshExpiredTime(db);

                    // 获取redis中所有的数据然后再将里面的数据加1就变成了我的id
                    var key = db.HashKeys(m_SysUserHashName);
                    int count = key.Length;
                    sysUser.Id = count + 1;
                    var n = db.HashSet(m_SysUserHashName, sysUser.Id, System.Text.Json.JsonSerializer.Serialize<SysUser>(sysUser));// 添加到redis
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取用户的信息
        public BOOK.MODEL.SysUser GetInfoById(int id)
        {
            // 获取redis连接
            using (var connection = GetConnection())
            {
                // 获取redis的数据连接
                var db = connection.GetDatabase();
                this.RefreshExpiredTime(db);

                // 获取 Redis 中存储的所有 SysUser 对象的键
                var keys = db.HashKeys(m_SysUserHashName);

                // 遍历键，查找匹配的 SysUser 对象
                foreach (var key in keys)
                {
                    var userJson = db.HashGet(m_SysUserHashName, key);
                    if (!string.IsNullOrEmpty(userJson))
                    {
                        var user = System.Text.Json.JsonSerializer.Deserialize<BOOK.MODEL.SysUser>(userJson);
                        if (user != null && user.Id == id) // 假设 SysUser 类中有 Id 属性
                        {
                            return user;
                        }
                    }
                }
            }

            // 如果未找到匹配的用户，返回 null 或者抛出异常
            return null; // 或者 throw new Exception("User not found");
        }
        #endregion



        /// <summary>
        /// 设置的缓存时间
        /// </summary>
        /// <param name="db"></param>
        public void RefreshExpiredTime(StackExchange.Redis.IDatabase db)
        {
            db.KeyExpire(m_SysUserHashName, new TimeSpan(0, 5, 0));// 将过期缓存设置为5分钟的时限
        }
    }
}
