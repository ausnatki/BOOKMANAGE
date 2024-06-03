using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL
{
  [Table("TB_Book")]
  public class Book
  {
    public int Id { get; set; } 
    public string BookName { get; set; } // 书名
    public string ISBN { get; set; } // ISBN编码
    public DateTime AddTime { get; set; } // 入库时间
    public string Author {  get; set; } // 作者
    public string Image {  get; set; } // 图书图片
    public List<Borrowed>? borrowed { get; set; }
  }
}
