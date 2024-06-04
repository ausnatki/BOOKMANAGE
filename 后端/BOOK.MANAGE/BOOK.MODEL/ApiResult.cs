using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL
{
    public class ApiResp
    {
        public int Code {  get; set; }
        public bool Result { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }
}
