using System;

namespace Book.AuthServer
{
    public static class ConsuleConfiguration
    {
        private static bool isRegistered = false;
        public static void RegisterConsul(this IServiceCollection services, IConfiguration configuration)
        {
            // 初始化consul的服务信息
            var consulClient = new Consul.ConsulClient(opt =>
            {
                opt.Address = new Uri(configuration["ConsulConfig:ConsulServer"]);
                opt.Datacenter = configuration["ConsulConfig:DataCenter"];
            });

            // 从配置文件中读取配置信息
            var apiId = configuration["ConsulConfig:APIID"];
            var ip = configuration["ConsulConfig:APIAddress"];
            var port = int.Parse(configuration["ConsulConfig:APIPort"]);
            var name = configuration["ConsulConfig:APIName"];
            var healthCheck = configuration["ConsulConfig:APICheckAddress"];

            // 定义服务注册信息
            var info = new Consul.AgentServiceRegistration()
            {
                ID = apiId,
                Name = name,
                Address = ip,
                Port = port,
                Tags = new string[] { "api/Books", "api/HealthCheck" },
                TaggedAddresses = new Dictionary<string, Consul.ServiceTaggedAddress>(),

                Check = new Consul.AgentServiceCheck() // 注册健康检查方式
                {
                    Interval = TimeSpan.FromSeconds(10),// 每隔10秒检查一次
                    HTTP = $"https://{ip}:{port}/{healthCheck}",// 健康检查地址
                    Timeout = TimeSpan.FromSeconds(5),// 超时时长5秒
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10) // 超过10秒后，webapi还不能连接到consul时，consul则注销本服务
                }
            };

            // 相关api
            info.TaggedAddresses.Add("api/Books", new Consul.ServiceTaggedAddress()
            {
                Address = "api/Books",
                Port = port,
            });

            info.TaggedAddresses.Add("api/HealthCheck", new Consul.ServiceTaggedAddress()
            {
                Address = "api/HealthCheck",
                Port = port,
            });

            // 尝试注册服务
            consulClient.Agent.ServiceRegister(info).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Console.WriteLine("服务注册成功");
                    isRegistered = true;
                }
                else
                {
                    Console.WriteLine("服务注册失败：" + task.Exception.InnerException.Message);
                }
            });

        }
    }
}
