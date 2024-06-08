import request from '@/utils/request'

export function GetBorrowed(UID) {
  return request({
    url: '/Book/Borrowed/GetBorrowed',
    method: 'get',
    params: { UID }
  })
}

export function Repiad(data) {
  return request({
    url: '/Book/Borrowed/Repiad',
    method: 'post',
    data
  })
}
