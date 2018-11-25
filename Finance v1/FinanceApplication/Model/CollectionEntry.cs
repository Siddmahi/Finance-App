using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FinanceApplication.Model
{
    class CollectionEntry : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public Int64? CurrentAmt { get; set; }
        public DateTime EntryDate { get; set; }
        public Int64? BalanceAmt { get; set; }
        public Int64? UpdatedAmt { get; set; }

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
