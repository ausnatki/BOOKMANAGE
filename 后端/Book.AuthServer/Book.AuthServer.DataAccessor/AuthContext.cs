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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-VCSEMTQ\\KKKMSSQLSERVER;Database=BOOKMANAGE;Trusted_Connection=True;");


            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book.AuthServer.Models.SysUser>();
            modelBuilder.Entity<Book.AuthServer.Models.SysUser_Role>();
            modelBuilder.Entity<Book.AuthServer.Models.Role>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book.AuthServer.Models.SysUser> Users { get; set; }
        public DbSet<Book.AuthServer.Models.SysUser_Role> User_Roles { get; set; }
        public DbSet<Book.AuthServer.Models.Role> Roles { get; set; }
    }
}
