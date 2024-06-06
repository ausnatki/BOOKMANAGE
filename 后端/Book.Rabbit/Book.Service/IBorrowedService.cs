using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public interface IBorrowedService
    {
        public bool BorrowBook(Book.Model.Borrowed borrowed);
    }
}
