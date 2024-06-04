using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.DB
{
    public class RedisContext
    {
        /// <summary>
        /// 链接redis数据库
        /// </summary>
        /// <returns></returns>
        public StackExchange.Redis.ConnectionMultiplexer GetConnection()
        {
            string connectionString = "localhost:6379,password=123456,abortConnect=false";
            var connection = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            return connection;
        }
    }
}
