using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceApplication.Model
{
    class Account
    {
        public Int64? StartingBalance { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DueDate { get; set; }
        public Int64? OutstandingAmt { get; set; }
        public Int64? AmountGiven { get; set; }
        public Int64? Amount { get; set; }

        public string AmountType { get; set; }
        public string Description { get; set; }
        public Int64? CollectionAmt { get; set; }
        public Int64? ClosingBalance { get; set; }
    }
}
