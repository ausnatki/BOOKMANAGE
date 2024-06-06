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
    }
}
