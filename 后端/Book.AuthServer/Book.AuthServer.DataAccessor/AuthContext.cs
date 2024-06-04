using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Book.AuthServer.DataAccessor
{
    public class AuthContext:DbContext
    {

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book.AuthServer.Models.SysUser>().ToTable("TB_SysUser");
            modelBuilder.Entity<Book.AuthServer.Models.SysUser_Role>().ToTable("TB_SysUser_Role");
            modelBuilder.Entity<Book.AuthServer.Models.Role>().ToTable("TB_Role");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book.AuthServer.Models.SysUser> Users { get; set; }
        public DbSet<Book.AuthServer.Models.SysUser_Role> User_Roles { get; set; }
        public DbSet<Book.AuthServer.Models.Role> Roles { get; set; }
    }
}
