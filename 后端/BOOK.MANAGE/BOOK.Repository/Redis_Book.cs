﻿using BOOK.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    public class Redis_Book
    {
        private string m_BookHashSetName = "BookHashSet";

        public StackExchange.Redis.ConnectionMultiplexer GetConnection()
        {
            string connectionString = "localhost:6379,password=123456,abortConnect=false";
            var connection = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            return connection;
        }

        /// <summary>
        /// 获取所有图书
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BOOK.MODEL.Book> GetBooks()
        {
            var bookList = new List<BOOK.MODEL.Book>();
            // 获取redis的链接 然后创建一个临时空间
            using (var connction = GetConnection())
            {
                // 获取redis的数据链接
                var db = connction.GetDatabase();

                this.RefreshExpiredTime(db);// 设置缓存时间
                // 获取键值对的名字 这里相当于数据库
                var keys = db.HashKeys(m_BookHashSetName);
                foreach (var key in keys)
                {
                    // 这里获取数据库的键值对的具体数据 这里相当于数据库中的表
                    var json = db.HashGet(m_BookHashSetName, key);
                    // 不为空就添加数据
                    if (!string.IsNullOrEmpty(json))
                    {
                        // 序列化一个图书数据
                        // 注意这里面的序列化的字段名称要跟我的models层里面的一摸一样才行
                        var book = System.Text.Json.JsonSerializer.Deserialize<BOOK.MODEL.Book>(json);
                        if (book != null)
                        {
                            bookList.Add(book);
                        }
                    }
                }
            }
            return bookList;
        }

        /// <summary>
        /// 获取单本图书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetBooksById(int id)
        {
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase();
                this.RefreshExpiredTime(db);// 设置缓存时间
                var json = db.HashGet(m_BookHashSetName, id);
                if (!string.IsNullOrEmpty(json))
                {
                    var book = System.Text.Json.JsonSerializer.Deserialize<BOOK.MODEL.Book>(json);
                    return book;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过id删除图书信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBook(int id)
        {
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase();
                this.RefreshExpiredTime(db);// 设置缓存时间
                return db.HashDelete(m_BookHashSetName, id);
            }
        }

        /// <summary>
        /// 添加新图书
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool NewBook(BOOK.MODEL.Book book)
        {
            using (var connection = GetConnection())
            {


                var db = connection.GetDatabase();
                this.RefreshExpiredTime(db);// 设置缓存时间
                // 首先获取redies中的所有数据 然后得到我的总体长度
                var keys = db.HashKeys(m_BookHashSetName);
                //int count = keys.Length;
                //book.Id = count+1;
                var n = db.HashSet(m_BookHashSetName, book.Id, System.Text.Json.JsonSerializer.Serialize(book));
                return true;
            }
        }

        /// <summary>
        /// 更新图书
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool UpdateBook(BOOK.MODEL.Book book)
        {
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase();
                this.RefreshExpiredTime(db);// 设置缓存时间
                var n = db.HashSet(m_BookHashSetName, book.Id, System.Text.Json.JsonSerializer.Serialize(book));
                return true;
            }
        }

        public void RefreshExpiredTime(StackExchange.Redis.IDatabase db)
        {
            db.KeyExpire(m_BookHashSetName, new TimeSpan(0, 5, 0));// 将过期缓存设置为10秒的时限
        }
    }
}
