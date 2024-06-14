import request from '@/utils/request'
import { export_json_to_excel } from '@/vendor/Export2Excel'

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

export function GetAll() {
  return request({
    url: '/Book/Borrowed/GetAllList',
    method: 'get'
  })
}

export function GetAllAudit() {
  return request({
    url: '/Book/Borrowed/GetAllAudit',
    method: 'get'
  })
}

export function Audit(BID) {
  return request({
    url: '/Book/Borrowed/AuditSuccess',
    method: 'post',
    params: { BID }
  })
}

export function Renewal(data) {
  return request({
    url: '/Book/Borrowed/Renewal',
    method: 'post',
    data
  })
}
