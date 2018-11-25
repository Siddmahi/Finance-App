using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FinanceApplication.Model
{
    class AccountCmbList
    {
        private string amount;
        public string Amount
        {
            get { return amount; }
            set
            {
                if (amount != value)
                {
                    amount = value;
                    NotifyOfPropertyChange("Amount");
                }
            }
        }
        //public string AddExpense
        //{
        //    get { return addExpense; }
        //    set
        //    {
        //        if (addExpense != value)
        //        {
        //            addExpense = value;
        //            NotifyOfPropertyChange("AddExpense");
        //        }
        //    }
        //}
        //public override string ToString()
        //{
        //    return $"{AddAmount}{AddExpense}";
        //}
        public override string ToString()
        {
            return String.Format("{0}", Amount);
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyOfPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
