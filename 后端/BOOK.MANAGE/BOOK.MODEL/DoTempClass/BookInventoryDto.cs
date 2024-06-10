using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOK.MODEL.DoTempClass
{
    public class BookInventoryDto
    {
        public int Id { get; set; }
        public string? BookName {  get; set; }
        public string? ISBN {  get; set; }
        public bool? State {  get; set; } // 这里对应的应该是isdelete
        public int Inventory { get; set; } // 总库存
        public int InStock {  get; set; } // 在库
        public int LoanedOut {  get; set; } // 借出

    }
}
