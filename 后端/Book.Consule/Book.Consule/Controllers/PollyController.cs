using Consul;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Polly;
using Polly.CircuitBreaker;
using Polly.CircuitBreaker;
using Polly.Wrap;

namespace Book.Consule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PollyController : ControllerBase
    {
        private Microsoft.Extensions.Configuration.IConfiguration m_configuration { get; }

        private readonly ILogger<ConsulController> _logger;

        public PollyController(ILogger<ConsulController> logger, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;
            m_configuration = configuration;
        }

        /// <summary>
        /// 重试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost("TestRetry")]
        public apiResult TestRetry([FromQuery] string id)
        {
            apiResult r = new apiResult();
            try
            {
                var result = Policy<string>.Handle<Exception>().Retry(3).Execute(() => CallWebAPI(id));

                r.Code = 20000;
                r.Data = result;
                r.Result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.Msg = ex.Message;
                return r;
            }

        }

        /// <summary>
        /// 回退策略实现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost("TestFallback")]
        public apiResult TestFallback([FromQuery] string id)
        {
            apiResult r = new apiResult();
            try
            {
                var result = Policy<string>.Handle<Exception>().Fallback(() =>
                {
                    return "空值";
                }).Execute(() => CallWebAPI(id));
                r.Code = 20000;
                r.Data = result;
                r.Result = true;
                return r;
            }
            catch
            {
                r.Result = false;
                r.Msg = "回退策略失败";
                return r;
            }
        }

        /// <summary>
        /// 超时策略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost("TestTimeOut")]
        public apiResult TestTimeOut([FromQuery] string id)
        {
            apiResult apiResult = new apiResult();
            try
            {
                var result = Polly.Policy.Timeout(10, Polly.Timeout.TimeoutStrategy.Pessimistic).Execute(() => CallWebAPI(id)); ;
                apiResult.Code = 20000;
                apiResult.Data = result;
                apiResult.Result = true;
                apiResult.Msg = "超时策略完成";
                return apiResult;

            }
            catch
            {
                apiResult.Msg = "超时策略失败";
                return apiResult;
            }


        }

        /// <summary>
        /// 组合策略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpPost("TestWrapPolicy")]
        public apiResult TestWrapPolicy([FromQuery] string id)
        {
            apiResult r = new apiResult();
            try
            {
                var retry = Policy<string>.Handle<Exception>().Retry(3);

                var fallback = Policy<string>.Handle<Exception>().Fallback(() =>
                {
                    return "空值";
                });

                var result = Polly.Policy.Wrap(fallback, retry).Execute(() => CallWebAPI(id));
                r.Code = 20000;
                r.Data = result;
                r.Result = true;
                r.Msg = "组合策略完成";
                return r;
            }
            catch
            {
                r.Msg = "组合策略失败";
                r.Result = false;
                return r;
            }
        }



        /// <summary>
        /// 熔断降级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("TestWrapPolicCircuitBreaker")]
        public apiResult TestWrapPolicCircuitBreaker([FromQuery] string id)
        {
            apiResult r = new apiResult();

            try
            {
                var result = PolicyService.Instance.WrapPolicy.Execute(() => CallWebAPI(id));

                // 获取断路器策略
                var circuitBreakerPolicy = PolicyService.Instance.WrapPolicy.GetPolicy<CircuitBreakerPolicy<string>>();

                if (circuitBreakerPolicy != null)
                {
                    // 获取断路器的状态
                    var circuitState = circuitBreakerPolicy.CircuitState;

                    // 根据断路器状态返回不同的值
                    switch (circuitState)
                    {
                        case CircuitState.Closed:
                            r.Msg = "断路器：关闭状态";
                            break;
                        case CircuitState.Open:
                            r.Msg = "断路器：开启状态";
                            break;
                        case CircuitState.HalfOpen:
                            r.Msg = "断路器：半开启状态";
                            break;
                    }
                }

                r.Code = 20000;
                r.Result = true;
            }
            catch
            {
                r.Result = false;
                r.Msg = "熔断策略失败";
            }

            return r;
        }




        /// <summary>
        /// 定义业务逻辑请求
        /// </summary>
        /// <returns></returns>
        [HttpPost("CallWebAPI")]
        public string CallWebAPI(string id)
        {
            try
            {
                // 1. 初始化consul服务信息
                var consulClient = new Consul.ConsulClient(opt =>
                {
                    opt.Address = new Uri(m_configuration["ConsulConfig:ConsulServer"]);
                    opt.Datacenter = m_configuration["ConsulConfig:DataCenter"];
                });

                // 2.获取服务
                var services = consulClient.Agent.Services().Result;
                var serviceItem = services.Response.FirstOrDefault(c => c.Key == id);
                if (serviceItem.Value != null)
                {
                    var url = $"https://{serviceItem.Value.Address}:{serviceItem.Value.Port}/api/Books/GetAll";
                    var client = new System.Net.WebClient();

                    var result = client.DownloadString(url);
                    return result;
                }
                return string.Empty;
            }
            catch (Exception ex) // 捕获所有异常
            {
                // 这里可以记录异常信息，然后将异常抛出
                // 可以使用日志库记录异常信息，比如 Serilog、NLog 等
                // 记录异常后，可以选择将异常重新抛出，或者返回一个默认值或错误信息
                throw; // 将异常抛出
            }
        }

    }
}
