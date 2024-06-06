// 消费者业务逻辑编写

using Book.Model;
using Book.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;


var factory = new RabbitMQ.Client.ConnectionFactory();
factory.HostName = "localhost";
factory.UserName = "guest";
factory.Password = "guest";

var serviceProvider = new ServiceCollection()
    .AddScoped<Book.DataAccess.BooksContext>()
    .AddScoped<Book.Repository.DB_Book>()
    .AddScoped<Book.Repository.Redis_Book>()
   .AddScoped<IBookService, BookSerivceImp>()
   .BuildServiceProvider();


// 连接rabbitmq
using (var connection = factory.CreateConnection())
{
    // 打开通信
    using (var channel = connection.CreateModel())
    {
        // 定义队列
        string queueName = "book-add";
        var queue = channel.QueueDeclare(queueName, false, false, false, null);

        // 同一时刻服务器只发送一条消息给消费者
        channel.BasicQos(0, 1, false);

        // 定义消费者
        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);

        // 消费者接收到消息事件后的处理
        consumer.Received += (sender, e) =>
        {
            try
            {
                // 将消息转换为期望的格式
                var jsonStr = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("收到消息" + jsonStr);

                //var book = System.Text.Json.JsonSerializer.Deserialize<Book.Model.TempBooks>(jsonStr); // 反序列化


                if(e.RoutingKey == "book-add") // 图书添加的业务
                {
                    Console.WriteLine("开始处理数据");
                    // 获取传过来的图书数据 然后执行我的图书相关的服务
                    var book = System.Text.Json.JsonSerializer.Deserialize<Book.Model.Book>(jsonStr);

                    if(book == null)
                    {
                        throw new Exception("数据为空");
                    }
                    else
                    {
                        var bookservice = serviceProvider.GetService<IBookService>();
                        if (bookservice.InstallBook(book))
                        {
                            Console.WriteLine("数据处理成功");
                            channel.BasicAck(e.DeliveryTag, false);
                        }
                    }
                }


                //// 处理收到的消息
                //if (book != null)
                //{
                //    Console.WriteLine("开始处理消息");
                //    bool processed = false;// 标志数据处理完成
                   

                  
                //    //processed = true; // 这里是标识
                   
                //    if (processed)
                //    {
                //        // 数据处理后，向消息队列返回确认状态
                //        channel.BasicAck(e.DeliveryTag, false);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("消费者角色业务发生错误" + ex.Message);
            }
        };
        string consumerTag = "book-consumer";

        // 启动消费者程序，监听消息
        channel.BasicConsume(queueName, false, consumerTag, false, false, null, consumer);

        Console.WriteLine("按任意键结束程序");
        Console.ReadLine();
    }
}
