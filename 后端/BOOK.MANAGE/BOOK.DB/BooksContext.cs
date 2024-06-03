using BOOK.MODEL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace BOOK.DB
{
  public class BooksContext : DbContext
  {
    //public BooksContext()
    //{
    //}

    // 数据库配置文件
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=DESKTOP-VCSEMTQ\\KKKMSSQLSERVER;Database=BOOKMANAGE;Trusted_Connection=True;");


      base.OnConfiguring(optionsBuilder);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.Entity<CQIE.Books.Models.Book>().ToTable("TB_Book");
      //modelBuilder.Entity<CQIE.Books.Models.BookInventory>().ToTable("TB_BookInventory");
      modelBuilder.Entity<BOOK.MODEL.Book>(entity =>
      {
        entity.HasKey(c => c.Id);
      });

      modelBuilder.Entity<BOOK.MODEL.Borrowed>(entity =>
      {
        entity.HasKey(c => c.Id);
        entity.HasOne(c => c.Book).WithMany(c => c.borrowed).HasForeignKey(c => c.BID);
        entity.HasOne(c=>c.SysUser).WithMany(c=>c.borroweds).HasForeignKey(c => c.UID);
      });

      modelBuilder.Entity<BOOK.MODEL.Role>(entity =>
      {
        entity.HasKey(c => c.Id);
      });

      modelBuilder.Entity<BOOK.MODEL.SysUser>(entity =>
      {
        entity.HasKey(c => c.Id);
      });

      modelBuilder.Entity<BOOK.MODEL.SysUser_Role>(entity =>
      {
        entity.HasKey(c => c.Id);
        entity.HasOne(c => c.sysUser).WithMany(c => c.user_roles).HasForeignKey(c => c.UID);
        entity.HasOne(c => c.Role).WithMany(c => c.user_roles).HasForeignKey(c => c.RID);
      });
      
      base.OnModelCreating(modelBuilder);
    }

    //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.Book> Books { get; set; }
    //public Microsoft.EntityFrameworkCore.DbSet<CQIE.Books.Models.BookInventory> BooksInventory { get; set; }

    public Microsoft.EntityFrameworkCore.DbSet<BOOK.MODEL.Book> Books { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<BOOK.MODEL.Borrowed> Borroweds { get; set;}
    public Microsoft.EntityFrameworkCore.DbSet<BOOK.MODEL.Role> Roles { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<BOOK.MODEL.SysUser> SysUsers { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<BOOK.MODEL.SysUser_Role> User_role { get; set; }
  }
}
