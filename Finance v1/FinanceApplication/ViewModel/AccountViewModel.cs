using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using FinanceApplication.Data;
using System.Windows.Input;
using System.Runtime.InteropServices;
using FinanceApplication.Model;
using RSA.Common.Utilities;

namespace FinanceApplication.ViewModel
{
    class AccountViewModel : INotifyPropertyChanged
    {
        //  public ObservableCollection<Account> accountLogList { get; set; }
        FinanceApplicationModel financeModel;

        #region Private Fields

        private ObservableCollection<Account> accountLogList;

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
                            GetAccountsLog();
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
                            GetAccountsLog();
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
                            GetAccountsLog();
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
                            GetAccountsLog();
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

        public ObservableCollection<Account> AccountLogList
        {
            get
            {
                return accountLogList;
            }
            private set
            {
                if (object.ReferenceEquals(accountLogList, value) != true)
                {
                    accountLogList = value;
                    NotifyPropertyChanged("AccountLogList");
                }
            }
        }

        public AccountViewModel()
        {
            financeModel = new FinanceApplicationModel();
            int openingBalanceFromDB = GetOpeningBalance();
            OpeningBalance = openingBalanceFromDB;
            EntryDate = DateTime.Today;
            GetAccountsLog();
            AmountType.Add(new AccountCmbList() { Amount = "Add" });
            AmountType.Add(new AccountCmbList() { Amount = "Sub" });
            //AmountType.Add(new AccountCmbList() { Month = 4, Year = 2013 });
        }

        public void GetAccountsLog()
        {

            if (financeModel == null)
            {
                financeModel = new FinanceApplicationModel();
            }
            AccountLogList = financeModel.GetAccountsLog(start, itemCount, ascending, out totalItems);
            // userList = new ObservableCollection<User>(userCollectionModel.GetUserList(), start, itemCount, sortColumn, ascending, out totalItems);
            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }

        #region DbCalls
        public int GetOpeningBalance()
        {
            return DatabaseLayer.GetAccountOpeningBalance();
        }
        public void AddMasterEntry(Master masterEntryFields)
        {
            DatabaseLayer.InsertMasterEntry(masterEntryFields);
        }
        #endregion


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
                closingBalance = value;
                OnPropertyChanged("ClosingBalance");
                //this.OnPropertyChanged(new PropertyChangedEventArgs("ClosingBalance"));
            }
        }
        private Int64? amount;
        public Int64? Amount
        {
            get { return amount; }
            set
            {
                if (value.HasValue)
                {
                    amount = value;
                }
                OnPropertyChanged(("Amount"));
                if (Amount > 0)
                {
                    if (SelectedItem.Amount == "Add")
                    {
                        ClosingBalance = OpeningBalance + Amount;
                    }
                    else
                    {
                        ClosingBalance = OpeningBalance - Amount;
                    }
                }
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private ObservableCollection<AccountCmbList> amountType = new ObservableCollection<AccountCmbList>();
        public ObservableCollection<AccountCmbList> AmountType
        {
            get { return amountType; }
            set { amountType = value; OnPropertyChanged("AmountType"); }
        }
        private AccountCmbList selectedItem = new AccountCmbList();
        public AccountCmbList SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        #endregion

        #region Commands
        private RelayCommand submitCommand;
        public RelayCommand SubmitCommand
        {
            get
            {
                if (submitCommand == null)
                    submitCommand = new RelayCommand(AddAccountEntry);
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

        void AddAccountEntry(object parameter)
        {
            Account accountFields = new Account()
            {
                StartingBalance = openingBalance,
                EntryDate = entryDate,
                Amount = amount,
                AmountType = SelectedItem.Amount,
                Description = description,
                ClosingBalance = closingBalance
            };

            if (openingBalance < 0 || closingBalance <= 0 || closingBalance == null || amount <= 0 || amount == null
                || openingBalance == null || SelectedItem.Amount == null || AmountType == null || description.IsVoid())
            {
                MessageBox.Show("Fill in all the mandatory fields");
            }
            else
            {
                if (financeModel == null)
                {
                    financeModel = new FinanceApplicationModel();
                }
                financeModel.AddAccountEntry(accountFields);
                ClearFields();
            }

        }
        //void UpdateTotalAmount()
        //{
        //    if (SelectedItem.AddAmount == "Add")
        //    {
        //        ClosingBalance = OpeningBalance + Amount;
        //    }
        //    else
        //    {
        //        ClosingBalance = OpeningBalance - Amount;
        //    }
        //    OnPropertyChanged("ClosingBalance");
        //}
        void ClearFields([Optional]object parameter)
        {
            Description = "";
            ClosingBalance = 0;
            Amount = 0;
            EntryDate = DateTime.Today;
            int openingBalanceFromDB = GetOpeningBalance();
            OpeningBalance = openingBalanceFromDB;
            GetAccountsLog();
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
