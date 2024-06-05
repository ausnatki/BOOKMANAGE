import request from '@/utils/request'

export function AddBook(data) {
  return request({
    url: '/Book/Book/AddBook',
    method: 'post',
    data
  })
}

