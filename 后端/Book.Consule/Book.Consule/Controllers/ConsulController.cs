using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Book.Consule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsulController : ControllerBase
    {
        private Microsoft.Extensions.Configuration.IConfiguration m_configuration { get; }

        private readonly ILogger<ConsulController> _logger;

        public ConsulController(ILogger<ConsulController> logger, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;
            m_configuration = configuration;
        }

        [HttpGet("GetSonsul")]
        public IActionResult GetSonsul()
        {
            var list = new List<Consul.AgentService>();

            try
            {
                // 1. 初始化consul服务信息
                var client = new Consul.ConsulClient(opt =>
                {
                    opt.Address = new Uri(m_configuration["ConsulConfig:ConsulServer"]);
                    opt.Datacenter = m_configuration["ConsulConfig:DataCenter"];
                });

                // 2. 获取服务
                var services = client.Agent.Services().Result;
                foreach (var item in services.Response)
                {
                    var service = item.Value;
                    list.Add(service);
                }
                return Ok(new { code = 20000, data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
