using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceApplication.Data;

namespace FinanceApplication.Model
{
    class MasterAccountModel
    {
        public int GetOpeningBalance()
        {
            return DatabaseLayer.GetMasterAccountOpeningBalance();
        }
        public void AddMasterEntry(Master masterEntryFields)
        {
            DatabaseLayer.InsertMasterEntry(masterEntryFields);
        }
    }
}
