﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.AuthServer.Models
{
    public class SysUser_Role
    {
        public int Id { get; set; }
        public int UID { get; set; }
        public int RID { get; set; }
        public SysUser? sysUser { get; set; }
        public Role? Role { get; set; }
    }
}
