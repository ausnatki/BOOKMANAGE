using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model
{
    [Table("TB_SysUser")]
    public class SysUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string UserName { get; set; }
        public List<Borrowed>? borroweds { get; set; }
        public List<SysUser_Role>? user_roles { get; set; }
    }
}
