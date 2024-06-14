using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL
{
  [Table("TB_Borrowed")]
  public class Borrowed
  {
    public int Id {  get; set; }
    public int UID {  get; set; }
    public int BID {  get; set; }
    public DateTime BorrowedTime {  get; set; }
    public DateTime RepaidTime { get; set; }
    public bool State { get; set; } //  «∑ÒπÈªπ
    public bool IsAudit {  get; set; } //  «∑Ò…Û∫À
    public SysUser? SysUser { get; set; }
    public Book? Book { get; set; }

  }
}
