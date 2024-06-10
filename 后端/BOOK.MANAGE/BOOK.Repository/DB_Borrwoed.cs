using BOOK.DB;
using BOOK.MODEL;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.Repository
{
    public class DB_Borrwoed
    {
        #region 依赖注入
        private readonly BOOK.DB.BooksContext Ctx;

        public DB_Borrwoed(BooksContext context)
        {
            this.Ctx = context;
        }
        #endregion

        #region 用户借阅图书
        public bool Borrwoed(int BID, int UID)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {
                try
                {
                    // 判断传过来的id是否都合法
                    var book = Ctx.Books.Where(c => c.Id == BID).FirstOrDefault();
                    var sysuer = Ctx.SysUsers.Where(c => c.Id == UID).FirstOrDefault();
                    if (book == null || sysuer == null)
                    {
                        return false; // 数据不合法
                    }
                    else
                    {
                        var borrowed = new BOOK.MODEL.Borrowed();
                        borrowed.BID = BID;
                        borrowed.UID = UID;
                        borrowed.BorrowedTime = DateTime.Now;
                        borrowed.State = false;// 借出状态
                        Ctx.Borroweds.Add(borrowed);
                        transaction.Commit();
                        return true;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        #endregion

        #region 判断用户是否借阅当前图书
        public bool IsBorrowed(int BID, int UID)
        {
            try
            {
                var result = Ctx.Borroweds.Where(c => c.UID == UID && c.BID == BID).FirstOrDefault();
                if (result == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 用户借阅列表
        public IEnumerable<BOOK.MODEL.Borrowed> Borrowed_User(int UID)
        {
            try
            {
                var user = Ctx.SysUsers.Where(c => c.Id == UID).FirstOrDefault();
                if (user == null) return Enumerable.Empty<BOOK.MODEL.Borrowed>(); // 返回一个空的Borrowed集合，而不是null  

                // 用户验证完成，然后查询当前用户的借阅列表  
                IEnumerable<BOOK.MODEL.Borrowed> borrowedList = Ctx.Borroweds
                    .Where(c => c.UID == UID)
                    .Select(c => new BOOK.MODEL.Borrowed // 直接创建Borrowed对象  
                    {
                        Id = c.Id,
                        UID = c.UID,
                        BID = c.BID,
                        BorrowedTime = c.BorrowedTime,
                        State = c.State,
                        SysUser = c.SysUser != null ? new BOOK.MODEL.SysUser // 如果SysUser不为null，则创建一个SysUser对象  
                        {
                            
                            Id = c.SysUser.Id, // 假设SysUser有Id属性  
                            UserName = c.SysUser.UserName
                        } : null,
                        Book = c.Book != null ? new BOOK.MODEL.Book // 如果Book不为null，则创建一个Book对象  
                        {
                           
                            Id = c.Book.Id, // 假设Book有Id属性  
                            BookName = c.Book.BookName,
                            Author = c.Book.Author
                        } : null
                    }).ToList();

                return borrowedList;
            }
            catch
            {
                return Enumerable.Empty<BOOK.MODEL.Borrowed>(); // 返回一个空的Borrowed集合，而不是null  
            }
        }
        #endregion

        #region 用户归还
        public bool repiad(BOOK.MODEL.Borrowed borrowed)
        {
            using (var transaction = Ctx.Database.BeginTransaction())
            {

                try
                {
                    var borrow = Ctx.Borroweds.Where(c => c.Id == borrowed.Id).FirstOrDefault();
                    if (borrow == null) return false;
                    borrow.State = true;
                    Ctx.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region 获取所有借阅信息（管理员）
        public IEnumerable<BOOK.MODEL.Borrowed> GetAllList()
        {
            // 用户验证完成，然后查询当前用户的借阅列表  
            IEnumerable<BOOK.MODEL.Borrowed> borrowedList = Ctx.Borroweds
                .Select(c => new BOOK.MODEL.Borrowed // 直接创建Borrowed对象  
                {
                    Id = c.Id,
                    UID = c.UID,
                    BID = c.BID,
                    BorrowedTime = c.BorrowedTime,
                    State = c.State,
                    SysUser = c.SysUser != null ? new BOOK.MODEL.SysUser // 如果SysUser不为null，则创建一个SysUser对象  
                    {
                        Id = c.SysUser.Id, // 假设SysUser有Id属性  
                        UserName = c.SysUser.UserName
                    } : null,
                    Book = c.Book != null ? new BOOK.MODEL.Book // 如果Book不为null，则创建一个Book对象  
                    {
                        Id = c.Book.Id, // 假设Book有Id属性  
                        BookName = c.Book.BookName,
                        Author = c.Book.Author
                    } : null
                }).ToList();

            return borrowedList;
        }
        #endregion

    }
}
