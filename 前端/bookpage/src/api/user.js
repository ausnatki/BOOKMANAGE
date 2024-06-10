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

export function GetInfoById(id) {
  return request({
    url: '/Book/SysUser/GetInfoById',
    method: 'post',
    params: { id }
  })
}

export function EditInfo(user) {
  return request({
    url: '/Book/SysUser/EditInfo',
    method: 'post',
    data: user
  })
}

export function GetAll() {
  return request({
    url: '/Book/SysUser/GetAllSysUser',
    method: 'get'
  })
}

export function ChangeState(UID) {
  return request({
    url: '/Book/SysUser/ChangeState',
    method: 'post',
    params: { UID }
  })
}
