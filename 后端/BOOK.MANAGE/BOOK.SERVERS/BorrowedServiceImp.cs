using BOOK.MODEL;
using BOOK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public class BorrowedServiceImp:IBorrowedService
    {
        private readonly DB_Borrwoed _dbBorrwoed;
        private readonly Redis_Borrowed redis_borrowed;

        public BorrowedServiceImp(DB_Borrwoed dbBorrwoed, Redis_Borrowed redis_borrowed)
        {
            _dbBorrwoed = dbBorrwoed;
            this.redis_borrowed = redis_borrowed;
        }

        #region 借阅
        public bool BorrowBook(int BID, int UID)
        {

            var reuslt = new BOOK.MODEL.ApiResp();

            #region 初始化borrowed类
            var borrowed = new BOOK.MODEL.Borrowed();
            borrowed.BID = BID;
            borrowed.UID = UID;
            borrowed.State = false;
            borrowed.BorrowedTime = DateTime.Now;
            #endregion

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
                    string queueName = "borrowed-add";
                    string routigKey = "borrowed-add";
                    var queue = channel.QueueDeclare(queueName, false, false, false, null);

                    // 5.交换器和消息队列进行绑定
                    channel.QueueBind(queueName, exchangeName, routigKey, null);

                    try
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        // 将图书序列化，生成要发送的消息
                        var borrowedJson = System.Text.Json.JsonSerializer.Serialize(borrowed);
                        var body = System.Text.Encoding.UTF8.GetBytes(borrowedJson);

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

        #region 判断用户是否借阅了当前书籍
        public bool IsBorrowed(int BID,int UID)
        {
            try 
            {
                if(_dbBorrwoed.IsBorrowed(BID, UID))
                {
                    return true;
                }
                else
                {
                    return false;
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
            var borrowedlist = new List<BOOK.MODEL.Borrowed>();
            try
            {
               var redisborrowed = redis_borrowed.GetBorrowed(UID);
                if (redisborrowed.Count() > 0)
                {
                    redisborrowed = redisborrowed.ToList();
                    return redisborrowed;
                }
                else
                {
                    // 如果redis中里面没有查询到相关的数据就从数据库里面查询数据
                    var dbBorrowed = _dbBorrwoed.Borrowed_User(UID);
                    if (dbBorrowed != null)
                    {
                        foreach(var t in dbBorrowed)
                        {
                            redis_borrowed.UpdataBorrowed(t);
                        }
                        borrowedlist = dbBorrowed.ToList();
                        return borrowedlist;
                    }
                }
                return borrowedlist;
            } 
            catch
            {
                return null;
            }
        }
        #endregion

        #region 归还
        public bool Repiad(BOOK.MODEL.Borrowed borrowed)
        {
            //try 
            //{

            //    // 如果查询出来有相应的值 就更新我的redis数据库
            //    if (_dbBorrwoed.repiad(borrowed))
            //    {
            //        borrowed.State = true;
            //        redis_borrowed.UpdataBorrowed(borrowed);    
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}

            //var reuslt = new BOOK.MODEL.ApiResp();

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
                    string queueName = "borrow-repaid";
                    string routigKey = "borrow-repaid";
                    var queue = channel.QueueDeclare(queueName, false, false, false, null);

                    // 5.交换器和消息队列进行绑定
                    channel.QueueBind(queueName, exchangeName, routigKey, null);

                    try
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        // 将图书序列化，生成要发送的消息
                        var bookJson = System.Text.Json.JsonSerializer.Serialize(borrowed);
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

        #region 获取所有借阅信息（管理员）
        public IEnumerable<BOOK.MODEL.Borrowed> GetAllList()
        {
            try
            {
                return _dbBorrwoed.GetAllList();
            } 
            catch
            {
                throw new Exception();
            }
        }
        #endregion
    }
}
