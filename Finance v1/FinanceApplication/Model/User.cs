using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FinanceApplication.Model
{
    class User : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public Int64? InitialAmt { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DueDate { get; set; }
        public Int64? CurrentAmt { get; set; }
        public Int64? InterestRate { get; set; }

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
