using BOOK.DB;
using BOOK.MODEL.Exception;
using BOOK.Repository;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace BOOK.SERVERS
{
    public class BookSerivceImp : IBookService
    {
        private readonly BOOK.Repository.DB_Book dB_Book;
        private readonly BOOK.Repository.Redis_Book redis_Book;

        public BookSerivceImp(DB_Book dB_Book, Redis_Book redis_Book)
        {
            this.dB_Book = dB_Book;
            this.redis_Book = redis_Book;
        }

        #region 获取全部图书信息
        public IEnumerable<BOOK.MODEL.Book> GetBook()
        {
            var booklist = new List<BOOK.MODEL.Book>();
            try
            {
                // 先执行redis中的查询操作然后通过判断redis中是否拥有数据 如果没有数据的话就需要通过数据库来进行相关数据库的查询
                var redisBook = redis_Book.GetBooks();
                if (redisBook.Count() > 0)
                {
                    booklist = redisBook.ToList();
                    return booklist;
                }
                else
                {
                    // 执行通过数据库里面进项查询
                    // 并且将数据库里面查询的数据添加到redis中去
                    var dbBook = dB_Book.GetBook();
                    if (dbBook != null)
                    {
                        foreach (var t in dbBook)
                        {
                            redis_Book.UpdateBook(t); // 更新图书信息
                        }
                        booklist = dbBook.ToList();
                        return booklist;
                    }
                }
                return booklist;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取单本书的信息
        public object GetById(int id)
        {
            try
            {

                // 首先从redis里面查询数据 如果没有查找到就从数据库里面查询
                var redisBook = redis_Book.GetBooksById(id);
                object book = redisBook;

                if (redisBook == null)
                {
                    var dbBook = dB_Book.GetById(id);
                    book = dbBook;
                }
                return book;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 修改图书信息
        public bool Edit(BOOK.MODEL.Book book)
        {
            try
            {
                bool result = dB_Book.Edit(book);
                if (result)
                {
                    redis_Book.UpdateBook(book);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 添加图书
        public bool InstallBook(BOOK.MODEL.Book book)
        {
            //try 
            //{
            //    var reuslt = dB_Book.InstallBook(book);
            //    if (reuslt)
            //    {
            //        redis_Book.NewBook(book);
            //    }
            //    return true;
            //} 
            //catch 
            //{
            //    return false;
            //}


            var reuslt = new BOOK.MODEL.ApiResp();

            // 连接rabbitmq

            // 创建一个rabbitmq对象
            var factory = new RabbitMQ.Client.ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "guest";
            factory.Password = "guest";

            // 1.连接rabbitmq
            using (var connection = factory.CreateConnection())
            {
                // 2. 创建信道
                using (var channel = connection.CreateModel())
                {
                    // 3. 创建交换器
                    string exchangeName = "exchange-direct";
                    string exchageType = "direct";
                    channel.ExchangeDeclare(exchangeName, exchageType, false, true, null);

                    // 4.创建消息队列
                    string queueName = "book-add";
                    string routigKey = "book-add";
                    var queue = channel.QueueDeclare(queueName, false, false, false, null);

                    // 5.交换器和消息队列进行绑定
                    channel.QueueBind(queueName, exchangeName, routigKey, null);

                    try
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        // 将图书序列化，生成要发送的消息
                        var bookJson = System.Text.Json.JsonSerializer.Serialize(book);
                        var body = System.Text.Encoding.UTF8.GetBytes(bookJson);

                        // 向Broker中的Exchange发送消息
                        channel.BasicPublish(exchangeName, routigKey, false, properties, body);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        //reuslt.Result = false;
                        //reuslt.Msg = "发送消息失败" + ex.Message;
                        return false;
                    }
                }

            }
        }


        #endregion

        #region 删除图书
        public bool DelBook(int id)
        {
            try
            {
                var reuslt = dB_Book.DelBook(id);
                if (reuslt)
                {
                    redis_Book.DeleteBook(id);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取单本书的库存
        public int GetOutInventory(int id)
        {
            try
            {
                // 直接查询借阅表
                var result = dB_Book.GetOutInventory(id);
                return result;
            }
            catch
            {
                throw new DbException("数据库错误");
            }
        }
        #endregion

        #region 获取图书库存界面的列表
        public IEnumerable<BOOK.MODEL.DoTempClass.BookInventoryDto> GetAllBookAdmin() 
        {
            try 
            {
                var userlist = dB_Book.GetAllBookAdmin();
                return userlist;
            }
            catch 
            {
                return null;
            }
        }
        #endregion

        #region 修改图书状态（isdel）
        public bool ChangeState(int BID)
        {
            try 
            {
                if(dB_Book.ChangeState(BID)) return true;
                else return false;  
            }
            catch 
            {
                return false;
            }
        }

        #endregion

        #region 添加库存
        public bool AddInventory(int BID,int Cnt)
        {
            try 
            {
                return dB_Book.AddInventory(BID, Cnt);

            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取单本图书的借阅情况
        public IEnumerable<BOOK.MODEL.DoTempClass.BorroweByBidDto> GetBorrowedByBid(int BID)
        {
            try 
            {
                var userlist = dB_Book.GetBorroweByBid(BID);
                return userlist;
            }
            catch
            {
                throw new Exception();
            }
        }
        #endregion
    }
}

