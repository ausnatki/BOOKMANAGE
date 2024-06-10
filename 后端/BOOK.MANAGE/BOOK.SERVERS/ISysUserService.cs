using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public interface ISysUserService
    {
        public bool EditUserInfo(BOOK.MODEL.SysUser user);

        public BOOK.MODEL.SysUser GetUserInfo(int Id);
    }
}
