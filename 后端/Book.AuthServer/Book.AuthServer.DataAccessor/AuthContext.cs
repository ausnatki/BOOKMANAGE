using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Book.AuthServer.DataAccessor
{
    public class AuthContext:DbContext
    {

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CQIE.Books.Models.Book>().ToTable("TB_Book");
            //modelBuilder.Entity<CQIE.Books.Models.BookInventory>().ToTable("TB_BookInventory");
         

            modelBuilder.Entity<Book.AuthServer.Models.Role>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<Book.AuthServer.Models.SysUser>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<Book.AuthServer.Models.SysUser_Role>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.sysUser).WithMany(c => c.user_roles).HasForeignKey(c => c.UID);
                entity.HasOne(c => c.Role).WithMany(c => c.user_roles).HasForeignKey(c => c.RID);
            });

            base.OnModelCreating(modelBuilder);
        }

        //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.Book> Books { get; set; }
        //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.BookInventory> BooksInventory { get; set; }


        public Microsoft.EntityFrameworkCore.DbSet<Book.AuthServer.Models.Role> Roles { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Book.AuthServer.Models.SysUser> SysUsers { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Book.AuthServer.Models.SysUser_Role> User_role { get; set; }
    }
}
