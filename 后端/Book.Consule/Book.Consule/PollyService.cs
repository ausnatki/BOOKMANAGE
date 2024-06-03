using Polly;

namespace Book.Consule
{
    public class PolicyService
    {
        private static readonly PolicyService m_Instance = new PolicyService();
        private PolicyService() { }
        public static PolicyService Instance { get { return m_Instance; } }
        private Polly.Wrap.PolicyWrap<string> m_WrapPolicy;

        public Polly.Wrap.PolicyWrap<string> WrapPolicy
        {
            get
            {
                if (m_WrapPolicy == null)
                {

                    var fallbackPolicy = Policy<string>.Handle<Exception>()
                        .Fallback(() =>
                        {
                            return "[]";
                        });

                    var circuitBreakerPolicy = Policy<string>.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(30),
                                        onBreak: (ex, timespan) =>
                                        {
                                            // 在断路器开启时执行的动作
                                        },
                                        onReset: () =>
                                        {
                                            // 在断路器重置时执行的动作
                                        },
                                        onHalfOpen: () =>
                                        {
                                            // 在断路器半开启时执行的动作
                                        });


                    m_WrapPolicy = Policy.Wrap(fallbackPolicy, circuitBreakerPolicy);
                }

                return m_WrapPolicy;
            }
        }
    }
}
