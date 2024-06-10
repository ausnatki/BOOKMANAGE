using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public interface ISysUserService
    {
        public bool EditUserInfo(Book.Model.SysUser user);
    }
}
