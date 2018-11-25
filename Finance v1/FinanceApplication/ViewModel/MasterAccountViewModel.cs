using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using FinanceApplication.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinanceApplication.ViewModel
{
    class MasterAccountViewModel : INotifyPropertyChanged
    {
        // public ObservableCollection<Master> masterLogList { get; set; }
        public MasterAccountModel masterAccountModel;
        public ObservableCollection<AccountCmbList> cmbData = new ObservableCollection<AccountCmbList>();
        FinanceApplicationModel financeModel;

        #region Private Fields

        private ObservableCollection<Master> masterLogList;

        private int start = 0;

        private int itemCount = 15;

        private string searchID = string.Empty;

        private bool ascending = true;

        private int totalItems = 0;

        private ICommand firstCommand;

        private ICommand previousCommand;

        private ICommand nextCommand;

        private ICommand lastCommand;

        #endregion

        #region Paging

        public int Start { get { return start + 1; } }

        /// <summary>
        /// Gets the index of the last item in the products list.
        /// </summary>
        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }
        /// <summary>
        /// The number of total items in the data store.
        /// </summary>
        public int TotalItems { get { return totalItems; } }

        /// <summary>
        /// Gets the command for moving to the first page of products.
        /// </summary>
        public ICommand FirstCommand
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new RelayCommand
                    (
                        param =>
                        {
                            start = 0;
                            GetMasterLog();
                        },
                        param =>
                        {
                            return start - itemCount >= 0 ? true : false;
                        }
                    );
                }

                return firstCommand;
            }

        }

        /// <summary>
        /// Gets the command for moving to the previous page of products.
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand
                    (
                        param =>
                        {
                            start -= itemCount;
                            GetMasterLog();
                        },
                        param =>
                        {
                            return start - itemCount >= 0 ? true : false;
                        }
                    );
                }

                return previousCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the next page of products.
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand
                    (
                        param =>
                        {
                            start += itemCount;
                            GetMasterLog();
                        },
                        param =>
                        {
                            return start + itemCount < totalItems ? true : false;
                        }
                    );
                }

                return nextCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the last page of products.
        /// </summary>
        public ICommand LastCommand
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new RelayCommand
                    (
                        param =>
                        {
                            start = (totalItems / itemCount - 1) * itemCount;
                            start += totalItems % itemCount == 0 ? 0 : itemCount;
                            GetMasterLog();
                        },
                        param =>
                        {
                            return start + itemCount < totalItems ? true : false;
                        }
                    );
                }

                return lastCommand;
            }

        }
        /// <summary>
        /// Sorts the list of products.
        /// </summary>
        /// <param name="sortColumn">The column or member that is the basis for sorting.</param>
        /// <param name="ascending">Set to true if the sort</param>
        //public void Sort(string sortColumn, bool ascending)
        //{
        //    this.sortColumn = sortColumn;
        //    this.ascending = ascending;

        //    GetAccountsLog();
        //}

        #endregion

        public ObservableCollection<Master> MasterLogList
        {
            get
            {
                return masterLogList;
            }
            private set
            {
                if (object.ReferenceEquals(masterLogList, value) != true)
                {
                    masterLogList = value;
                    NotifyPropertyChanged("MasterLogList");
                }
            }
        }
        public MasterAccountViewModel()
        {
            masterAccountModel = new MasterAccountModel();
            int openingBalanceFromDB = masterAccountModel.GetOpeningBalance();
            OpeningBalance = openingBalanceFromDB;
            GetMasterLog();
            //financeModel = new FinanceApplicationModel();
            // masterLogList = new ObservableCollection<Master>(financeModel.GetMasterLog());
            EntryDate = DateTime.Today;
            ExpenseAmount = 0;
        }

        public void GetMasterLog()
        {
            if (financeModel == null)
            {
                financeModel = new FinanceApplicationModel();
            }
            MasterLogList = financeModel.GetMasterLog(start, itemCount, ascending, out totalItems);
            // userList = new ObservableCollection<User>(userCollectionModel.GetUserList(), start, itemCount, sortColumn, ascending, out totalItems);
            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }

        #region Properties
        private DateTime entryDate;
        public DateTime EntryDate
        {
            get
            {
                return entryDate;
            }
            set
            {
                entryDate = value;
                OnPropertyChanged("EntryDate");
            }
        }
        private Int64? openingBalance;
        public Int64? OpeningBalance
        {
            get { return openingBalance; }
            set
            {
                if (value.HasValue)
                {
                    openingBalance = value;
                }
                OnPropertyChanged("OpeningBalance");
            }
        }
        private Int64? closingBalance;
        public Int64? ClosingBalance
        {
            get { return closingBalance; }
            set
            {
                if (closingBalance != value)
                {
                    if (value.HasValue)
                    {
                        closingBalance = value;
                        OnPropertyChanged(("ClosingBalance"));
                    }
                }
            }
        }
        private Int64? expenseAmount;
        public Int64? ExpenseAmount
        {
            get { return expenseAmount; }
            set
            {
                if (value.HasValue)
                {
                    expenseAmount = value;
                }
                OnPropertyChanged(("ExpenseAmount"));
            }
        }
        private Int64? incomeAmount;
        public Int64? IncomeAmount
        {
            get { return incomeAmount; }
            set
            {
                if (value.HasValue)
                {
                    incomeAmount = value;
                }
                OnPropertyChanged(("IncomeAmount"));
                if (ExpenseAmount > 0 || IncomeAmount > 0)
                {
                    UpdateTotalAmount();
                }
            }
        }
        #endregion

        #region Commands
        private RelayCommand submitCommand;
        public RelayCommand SubmitCommand
        {
            get
            {
                if (submitCommand == null)
                    submitCommand = new RelayCommand(AddMasterEntry);
                return submitCommand;
            }
            private set
            {
                submitCommand = value;
            }
        }
        private RelayCommand resetCommand;
        public RelayCommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                    resetCommand = new RelayCommand(ClearFields);
                return resetCommand;
            }
            private set
            {
                resetCommand = value;
            }
        }
        #endregion

        #region Methods

        void AddMasterEntry(object parameter)
        {
            Master accountFields = new Master()
            {
                OpeningBalance = openingBalance,
                EntryDate = entryDate,
                IncomeAmount = incomeAmount,
                ExpenseAmount = expenseAmount,
                ClosingBalance = closingBalance
            };

            if (OpeningBalance < 0 || ClosingBalance <= 0 || EntryDate == null || ExpenseAmount <= 0)
            {
                MessageBox.Show("Fill in all the mandatory fields");

            }
            else
            {
                masterAccountModel.AddMasterEntry(accountFields);
            }

        }
        void UpdateTotalAmount()
        {
            Int64? tempClosingBalance = ExpenseAmount - IncomeAmount;
            ClosingBalance = OpeningBalance + tempClosingBalance;
        }
        void ClearFields(object parameter)
        {
            ExpenseAmount = 0;
            ClosingBalance = 0;
            IncomeAmount = 0;
            EntryDate = DateTime.Today;
            int openingBalanceFromDB = masterAccountModel.GetOpeningBalance();
            OpeningBalance = openingBalanceFromDB;
            GetMasterLog();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                handler(this, new PropertyChangedEventArgs("HasError"));
            }
        }
        /// <summary>
        /// Notifies subscribers of changed properties.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
