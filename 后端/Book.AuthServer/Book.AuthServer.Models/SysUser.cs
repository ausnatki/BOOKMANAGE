using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.AuthServer.Models
{
    public class SysUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string UserName { get; set; }
        public List<SysUser_Role>? user_roles { get; set; }
    }
}
