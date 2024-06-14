using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.SERVERS
{
    public interface IBorrowedService
    {
        public bool BorrowBook(int BID, int UID);

        public bool IsBorrowed(int BID, int UID);

        public IEnumerable<object> GetBorrowed(int UID);

        public bool Repiad(BOOK.MODEL.Borrowed borrowed);

        public bool Renewal(BOOK.MODEL.Borrowed borrowed);

        public IEnumerable<BOOK.MODEL.Borrowed> GetAllList();

        public IEnumerable<BOOK.MODEL.Borrowed> GetAllAudit();

        public bool AuditSuccess(int BID);
    }
}
