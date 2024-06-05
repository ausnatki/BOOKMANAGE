import request from '@/utils/request'

export function AddBook(data) {
  return request({
    url: '/Book/Book/AddBook',
    method: 'post',
    data
  })
}

export function GetBook() {
  return request({
    url: '/Book/Book/GetBook',
    method: 'get'
  })
}

export function GetById(id) {
  return request({
    url: '/Book/Book/GetById',
    method: 'get',
    params: { id }
  })
}
