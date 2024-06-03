import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/Values/login',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/api/Values/info',
    method: 'get',
    params: { token }
  })
}

export function logout() {
  return request({
    url: '/api/Values/logout',
    method: 'post'
  })
}
