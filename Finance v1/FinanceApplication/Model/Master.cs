using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceApplication.Model
{
    class Master
    {
        public Int64? OpeningBalance { get; set; }
        public DateTime EntryDate { get; set; }
        public Int64? IncomeAmount { get; set; }
        public Int64? ExpenseAmount { get; set; }
        public Int64? ClosingBalance { get; set; }

        public string Description { get; set; }
    }
}
