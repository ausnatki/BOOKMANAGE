import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/Auth/Auth/GetToken',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/Auth/Auth/info',
    method: 'get',
    params: { token }
  })
}

export function logout() {
  return request({
    url: '/webapiconsul/Login/logout',
    method: 'post'
  })
}
