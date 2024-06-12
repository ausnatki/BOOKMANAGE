using Book.Model;
using Book.Service;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

var serviceProvider = new ServiceCollection()
    .AddTransient<Book.DataAccess.BooksContext>()
    .AddTransient<Book.Repository.DB_Book>()
    .AddTransient<Book.Repository.Redis_Book>()
    .AddTransient<Book.Repository.DB_Borrwoed>()
    .AddTransient<Book.Repository.Redis_Borrowed>()
    .AddTransient<Book.Repository.DB_SysUser>()
    .AddTransient<Book.Repository.Redis_SysUser>()
    .AddTransient<IBorrowedService, BorrwoedServiceImp>()
    .AddTransient<IBookService, BookSerivceImp>()
    .AddTransient<ISysUserService, SysUserServiceImp>()
    .BuildServiceProvider();

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        string queueName = "book-add";
        string queue_Borrowed = "borrowed-add";
        string queue_Repaid = "borrow-repaid";
        string queue_EditUser = "user-edit";

        channel.QueueDeclare(queueName, false, false, false, null);
        channel.QueueDeclare(queue_Borrowed, false, false, false, null);
        channel.QueueDeclare(queue_Repaid, false, false, false, null);
        channel.QueueDeclare(queue_EditUser, false, false, false, null);

        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            try
            {
                int retryCount = 0;
                if (e.BasicProperties.Headers != null && e.BasicProperties.Headers.ContainsKey("x-retry-count"))
                {
                    retryCount = Convert.ToInt32(e.BasicProperties.Headers["x-retry-count"]);
                }

                int maxRetryCount = 3;
                if (retryCount >= maxRetryCount)
                {
                    Console.WriteLine("消息重试次数已达上限，丢弃消息");
                    channel.BasicReject(e.DeliveryTag, false);
                    return;
                }

                var jsonStr = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("收到消息: " + jsonStr);

                if (e.RoutingKey == "book-add")
                {
                    Console.WriteLine("开始处理数据");
                    var book = System.Text.Json.JsonSerializer.Deserialize<Book.Model.Book>(jsonStr);

                    if (book == null) throw new Exception("数据为空");

                    var bookService = serviceProvider.GetService<IBookService>();
                    if (bookService.InstallBook(book))
                    {
                        Console.WriteLine("数据处理成功");
                        channel.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        throw new Exception("数据错误");
                    }
                }
                else if (e.RoutingKey == "borrowed-add")
                {
                    Console.WriteLine("开始处理数据");
                    var borrowed = System.Text.Json.JsonSerializer.Deserialize<Book.Model.Borrowed>(jsonStr);

                    if (borrowed == null) throw new Exception("数据为空");

                    var borrowedService = serviceProvider.GetService<IBorrowedService>();
                    if (borrowedService.BorrowBook(borrowed))
                    {
                        Console.WriteLine("数据处理成功");
                        channel.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        throw new Exception("数据错误");
                    }
                }
                else if (e.RoutingKey == "borrow-repaid")
                {
                    Console.WriteLine("开始处理数据");
                    var borrowed = System.Text.Json.JsonSerializer.Deserialize<Book.Model.Borrowed>(jsonStr);

                    if (borrowed == null) throw new Exception("数据为空");

                    var borrowedService = serviceProvider.GetService<IBorrowedService>();
                    if (borrowedService.Repiad(borrowed))
                    {
                        Console.WriteLine("数据处理成功");
                        channel.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        throw new Exception("数据错误");
                    }
                }
                else if (e.RoutingKey == "user-edit")
                {
                    Console.WriteLine("开始处理数据");
                    var sysUser = System.Text.Json.JsonSerializer.Deserialize<Book.Model.SysUser>(jsonStr);

                    if (sysUser == null) throw new Exception("数据为空");

                    var sysUserService = serviceProvider.GetService<ISysUserService>();
                    if (sysUserService.EditUserInfo(sysUser))
                    {
                        Console.WriteLine("数据处理成功");
                        channel.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        throw new Exception("数据错误");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("消费者角色业务发生错误: " + ex.Message);

                int retryCount = 0;
                if (e.BasicProperties.Headers != null && e.BasicProperties.Headers.ContainsKey("x-retry-count"))
                {
                    retryCount = Convert.ToInt32(e.BasicProperties.Headers["x-retry-count"]);
                }

                retryCount++;
                if (retryCount >= 3)
                {
                    Console.WriteLine("消息重试次数已达上限，丢弃消息");
                    channel.BasicReject(e.DeliveryTag, false);
                }
                else
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Headers = properties.Headers ?? new Dictionary<string, object>();
                    properties.Headers["x-retry-count"] = retryCount;

                    channel.BasicPublish(exchange: e.Exchange, routingKey: e.RoutingKey, basicProperties: properties, body: e.Body);
                    channel.BasicAck(e.DeliveryTag, false);
                }
            }
        };

        string addConsumerTag = "add-book-consumer";
        string borrowedTag = "add-borrowed-consumer";
        string borrowedRepaidTag = "repaid-borrowed-consumer";
        string sysuserEditTag = "edit-user-consumer";

        channel.BasicConsume(queueName, false, addConsumerTag, false, false, null, consumer);
        channel.BasicConsume(queue_Borrowed, false, borrowedTag, false, false, null, consumer);
        channel.BasicConsume(queue_Repaid, false, borrowedRepaidTag, false, false, null, consumer);
        channel.BasicConsume(queue_EditUser, false, sysuserEditTag, false, false, null, consumer);

        Console.WriteLine("按任意键结束程序");
        Console.ReadLine();
    }
}
