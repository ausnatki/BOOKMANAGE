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

export function BorrowedBook(BID, UID) {
  return request({
    url: 'Book/Borrowed/BorrowedBook',
    method: 'post',
    params: {
      BID, UID
    }
  })
}

export function IsBorrowed(BID, UID) {
  return request({
    url: '/Book/Borrowed/IsBorrowed',
    method: 'post',
    params: {
      BID, UID
    }
  })
}

export function GetInventory() {
  return request({
    url: '/Book/Book/GetAllBookAdmin',
    method: 'get'

  })
}

export function ChangeState(BID) {
  return request({
    url: '/Book/Book/ChangeState',
    method: 'post',
    params: { BID }
  })
}

export function GetBorrowByBid(BID) {
  return request({
    url: '/Book/Book/GetBorrowByBid',
    method: 'get',
    params: { BID }
  })
}

export function AddInventory(BID, Cnt) {
  return request({
    url: '/Book/Book/AddInventory',
    method: 'post',
    params: { BID, Cnt }
  })
}
