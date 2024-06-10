using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL.DoTempClass
{
    public class BorroweByBidDto
    {
        public string? BookName {  get; set; } // 借阅图书名
        public DateTime BorroweDate { get; set; } // 借阅时间
        public bool State {  get; set; }// 是否归还
        public string? UserName { get; set; }// 用户姓名
        public string? Email {  get; set; }// 充当联系地址
    }
}
