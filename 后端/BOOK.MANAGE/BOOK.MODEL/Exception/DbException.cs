using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL.Exception
{
    public class DbException : ApplicationException
    {
        public DbException(string message) : base(message)
        { }
    }

}
