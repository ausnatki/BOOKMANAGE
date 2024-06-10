using BOOK.MODEL;
using BOOK.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public class SysUserServiceImp : ISysUserService
    {
        private readonly BOOK.Repository.DB_SysUser dB_SysUser;
        private readonly BOOK.Repository.Redis_SysUser redis_SysUser;

        public SysUserServiceImp(DB_SysUser dB_SysUser, Redis_SysUser redis_SysUser)
        {
            this.dB_SysUser = dB_SysUser;
            this.redis_SysUser = redis_SysUser;
        }

        #region 修改用户信息
        public bool EditUserInfo(SysUser user)
        {

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
                    string queueName = "user-edit";
                    string routigKey = "user-edit";
                    var queue = channel.QueueDeclare(queueName, false, false, false, null);

                    // 5.交换器和消息队列进行绑定
                    channel.QueueBind(queueName, exchangeName, routigKey, null);

                    try
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        // 将图书序列化，生成要发送的消息
                        var bookJson = System.Text.Json.JsonSerializer.Serialize(user);
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

        #region 获取用户信息
        public BOOK.MODEL.SysUser GetUserInfo(int Id)
        {
            try 
            {
                var user = redis_SysUser.GetInfoById(Id);
                if (user != null) return user;
                else
                {
                    user = dB_SysUser.GetInfoByID(Id);
                    if (user == null) return null;
                    return user;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取所有列表信息
        public List<BOOK.MODEL.DoTempClass.SysUserDto> GetAllUserInfo()
        {
            try 
            {
                var userlist = dB_SysUser.GetAllSysUser();
                return userlist;
            }
            catch 
            {
                return null;
            }
        }
        #endregion

        #region 更改用户状态
        public bool ChangeState(int UId)
        {
          
            try
            {
                if(dB_SysUser.ChangeState(UId)) return true;
                return false;
            }
            catch 
            {
                return false;
            }
        }
        #endregion
    }
}
