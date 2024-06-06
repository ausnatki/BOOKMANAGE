using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess
{
        public class BooksContext : DbContext
        {
        public BooksContext()
        {
        }

        // 数据库配置文件
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-VCSEMTQ\\KKKMSSQLSERVER;Database=BookMANAGE;Trusted_Connection=True;");


                base.OnConfiguring(optionsBuilder);
            }
            public BooksContext(DbContextOptions<BooksContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder ModelBuilder)
            {
                //ModelBuilder.Entity<CQIE.Books.Models.Book>().ToTable("TB_Book");
                //ModelBuilder.Entity<CQIE.Books.Models.BookInventory>().ToTable("TB_BookInventory");
                ModelBuilder.Entity<Book.Model.Book>(entity =>
                {
                    entity.HasKey(c => c.Id);
                });

                ModelBuilder.Entity<Book.Model.Borrowed>(entity =>
                {
                    entity.HasKey(c => c.Id);
                    entity.HasOne(c => c.Book).WithMany(c => c.borrowed).HasForeignKey(c => c.BID);
                    entity.HasOne(c => c.SysUser).WithMany(c => c.borroweds).HasForeignKey(c => c.UID);
                });

                ModelBuilder.Entity<Book.Model.Role>(entity =>
                {
                    entity.HasKey(c => c.Id);
                });

                ModelBuilder.Entity<Book.Model.SysUser>(entity =>
                {
                    entity.HasKey(c => c.Id);
                });

                ModelBuilder.Entity<Book.Model.SysUser_Role>(entity =>
                {
                    entity.HasKey(c => c.Id);
                    entity.HasOne(c => c.sysUser).WithMany(c => c.user_roles).HasForeignKey(c => c.UID);
                    entity.HasOne(c => c.Role).WithMany(c => c.user_roles).HasForeignKey(c => c.RID);
                });

                base.OnModelCreating(ModelBuilder);
            }

            //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.Book> Books { get; set; }
            //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.BookInventory> BooksInventory { get; set; }

            public Microsoft.EntityFrameworkCore.DbSet<Book.Model.Book> Books { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Book.Model.Borrowed> Borroweds { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Book.Model.Role> Roles { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Book.Model.SysUser> SysUsers { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Book.Model.SysUser_Role> User_role { get; set; }
        }
    
}
