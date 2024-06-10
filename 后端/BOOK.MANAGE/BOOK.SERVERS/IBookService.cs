using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public interface IBookService
    {
        
        public IEnumerable<BOOK.MODEL.Book> GetBook();// 获取全部图书
        public object GetById(int id);
        public bool Edit(BOOK.MODEL.Book book);
        public bool InstallBook(BOOK.MODEL.Book book);
        public bool DelBook(int id);
        public int GetOutInventory(int id);
        public bool ChangeState(int BID);
        public IEnumerable<BOOK.MODEL.DoTempClass.BookInventoryDto> GetAllBookAdmin();
        public bool AddInventory(int BID, int Cnt);
        public IEnumerable<BOOK.MODEL.DoTempClass.BorroweByBidDto> GetBorrowedByBid(int BID);
    }
}
