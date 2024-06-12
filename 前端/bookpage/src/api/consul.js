import request from '@/utils/request'

export function GetConsul() {
  return request({
    url: '/Consul/Consul/GetSonsul',
    method: 'get'
  })
}

// 重试策略
export function TestRetry(id) {
  return request({
    url: '/Consul/Polly/TestRetry',
    method: 'post',
    params: {
      id
    }
  })
}

// 回退策略
export function TestFallback(id) {
  return request({
    url: '/Consul/Polly/TestFallback',
    method: 'post',
    params: {
      id
    }
  })
}

// 超时策略
export function TestTimeOut(id) {
  return request({
    url: '/Consul/Polly/TestTimeOut',
    method: 'post',
    params: {
      id
    }
  })
}

// 组合策略
export function TestWrapPolicy(id) {
  return request({
    url: '/Consul/Polly/TestWrapPolicy',
    method: 'post',
    params: {
      id
    }
  })
}

// 熔断降级
export function TestWrapPolicCircuitBreaker(id) {
  return request({
    url: '/Consul/Polly/TestWrapPolicCircuitBreaker',
    method: 'post',
    params: {
      id
    }
  })
}
