using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL
{
  [Table("TB_Role")]
  public class Role
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool State {  get; set; }
    public List<SysUser_Role>? user_roles { get; set; }
  }
}
