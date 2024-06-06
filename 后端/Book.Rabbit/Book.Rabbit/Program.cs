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
    .AddTransient<Book.DataAccess.BooksContext>()
    .AddTransient<Book.Repository.DB_Book>()
    .AddTransient<Book.Repository.Redis_Book>()
    .AddTransient<Book.Repository.DB_Borrwoed>()
    .AddTransient<Book.Repository.Redis_Borrowed>()
    .AddTransient<IBorrowedService,BorrwoedServiceImp>()
   .AddTransient<IBookService, BookSerivceImp>()
   .BuildServiceProvider();


// 连接rabbitmq
using (var connection = factory.CreateConnection())
{
    // 打开通信
    using (var channel = connection.CreateModel())
    {
        // 定义队列
        string queueName = "book-add";
        string queue_Borrowed = "borrowed-add";
        var queue = channel.QueueDeclare(queueName, false, false, false, null);
        var queue_borrowed = channel.QueueDeclare(queue_Borrowed, false, false, false, null);
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
                else if(e.RoutingKey =="borrowed-add") // 图书添加业务
                {
                    Console.WriteLine("开始处理数据");
                    // 获取传过来的图书数据 然后执行我的图书相关的服务
                    var borrowed  = System.Text.Json.JsonSerializer.Deserialize<Book.Model.Borrowed>(jsonStr);

                    if (borrowed == null)
                    {
                        throw new Exception("数据为空");
                    }
                    else
                    {
                        var borrowedService = serviceProvider.GetService<IBorrowedService>();
                        if (borrowedService.BorrowBook(borrowed))
                        {
                            Console.WriteLine("数据处理成功");
                            channel.BasicAck(e.DeliveryTag, false);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("消费者角色业务发生错误" + ex.Message);
            }
        };
        string addConsumerTag = "add-book-consumer";
        string borrowedTag = "add-borrowed-consumer"; 
        // 启动消费者程序，监听消息
        channel.BasicConsume(queueName, false, addConsumerTag, false, false, null, consumer);
        channel.BasicConsume(queue_Borrowed, false, borrowedTag, false, false, null, consumer);
        Console.WriteLine("按任意键结束程序");
        Console.ReadLine();
    }
}
