using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FinanceApplication.Model;
using System.Collections.ObjectModel;
using System.Windows;
using RSA.Common.Utilities;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace FinanceApplication.ViewModel
{
    class UserCollectionViewModel : INotifyPropertyChanged
    {
        // public ObservableCollection<User> userList { get; set; }
        UserCollectionModel userCollectionModel;



        #region Private Fields

        private ObservableCollection<User> userList;

        private int start = 0;

        private int itemCount = 10;

        private string sortColumn = "ID";

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
                            GetUserLogGrid();
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
                            GetUserLogGrid();
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
                            GetUserLogGrid();
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
                            GetUserLogGrid();
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
        public void Sort(string sortColumn, bool ascending)
        {
            this.sortColumn = sortColumn;
            this.ascending = ascending;

            GetUserLogGrid();
        }

        #endregion

        public ObservableCollection<User> UserList
        {
            get
            {
                return userList;
            }
            private set
            {
                if (object.ReferenceEquals(userList, value) != true)
                {
                    userList = value;
                    NotifyPropertyChanged("UserList");
                }
            }
        }


        public UserCollectionViewModel()
        {
            if (userCollectionModel == null)
            {
                userCollectionModel = new UserCollectionModel();
            }
            GetUserLogGrid();
            int openingBalanceFromDB = userCollectionModel.GetOpeningBalance();
            OpeningAccountBalance = openingBalanceFromDB;
            EntryDate = DateTime.Today;
        }

        private void GetUserLogGrid()
        {
            if (userCollectionModel == null)
            {
                userCollectionModel = new UserCollectionModel();
            }
            UserList = userCollectionModel.GetUserList(start, itemCount, sortColumn, ascending,searchID, out totalItems);
            // userList = new ObservableCollection<User>(userCollectionModel.GetUserList(), start, itemCount, sortColumn, ascending, out totalItems);
            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }

        #region Commands

        private RelayCommand searchCommmand;
        public RelayCommand SearchCommmand
        {
            get
            {
                if (searchCommmand == null)
                {
                    searchCommmand = new RelayCommand(GetUserBasedOnID);
                }
                return searchCommmand;
            }
            private set
            {
                searchCommmand = value;
            }
        }

        private RelayCommand submitDailyAmountCommand;
        public RelayCommand SubmitDailyAmountCommand
        {
            get
            {
                if (submitDailyAmountCommand == null)
                    submitDailyAmountCommand = new RelayCommand(DailyAmountLog);
                return submitDailyAmountCommand;
            }
            private set
            {
                submitDailyAmountCommand = value;
            }
        }

        private RelayCommand resetEntryCommand;
        public RelayCommand ResetEntryCommand
        {
            get
            {
                if (resetEntryCommand == null)
                    resetEntryCommand = new RelayCommand(ClearField);
                return resetEntryCommand;
            }
            private set
            {
                resetEntryCommand = value;
            }
        }

        #endregion

        #region properties

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                if (selectedUser != null)
                {
                    SelectedUserName = selectedUser.UserName;
                    SelectedUserPendingAmt = selectedUser.CurrentAmt;
                    SearchUserID = selectedUser.ID;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser"));
            }
        }

        private Int64? openingAccountBalance;
        public Int64? OpeningAccountBalance
        {
            get { return openingAccountBalance; }
            set
            {

                openingAccountBalance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OpeningAccountBalance"));
            }
        }
        private string selectedUserName;
        public string SelectedUserName
        {
            get { return selectedUserName; }
            set
            {
                selectedUserName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUserName"));
                //RaisePropertyChanged("id");
            }
        }
        private Int64? selectedUserPendingAmt;
        public Int64? SelectedUserPendingAmt
        {
            get { return selectedUserPendingAmt; }
            set
            {
                selectedUserPendingAmt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUserPendingAmt"));
            }
        }
        private Int64? collectionAmt;
        public Int64? CollectionAmt
        {
            get { return collectionAmt; }
            set
            {
                collectionAmt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CollectionAmt"));
            }
        }
        private string searchUserID;
        public string SearchUserID
        {
            get { return searchUserID; }
            set
            {
                searchUserID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchUserID"));
                //RaisePropertyChanged("id");
            }
        }
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
                this.OnPropertyChanged(new PropertyChangedEventArgs("EntryDate"));
            }
        }

        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
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

        #region Methods

        void DailyAmountLog(object parameter)
       {
            CollectionEntry dailyCollectionAmt = new CollectionEntry()
            {
                ID = SearchUserID,
                UserName = SelectedUserName,
                EntryDate = EntryDate,
                BalanceAmt = SelectedUserPendingAmt,
                CurrentAmt = CollectionAmt
            };

            if (SearchUserID == null || SelectedUserName == null || EntryDate == null || SelectedUserPendingAmt.HasValue == false
                || CollectionAmt.HasValue == false || SelectedUserName.IsVoid() || CollectionAmt<=0 || SelectedUserPendingAmt <= 0)
            {
                MessageBox.Show("Fill in all the mandatory fields");
            }
            else
            {
                if(userCollectionModel==null)
                {
                    userCollectionModel = new UserCollectionModel();
                }
                string selectedDate = (string.Format("{0:yyyy-MM-dd H:mm:ss}", dailyCollectionAmt.EntryDate));
                string userEntryExist = userCollectionModel.CheckEntryForSameDate(dailyCollectionAmt.ID, selectedDate);
                if(userEntryExist.IsVoid())
                {
                    userCollectionModel.DailyCollectionEntry(dailyCollectionAmt);
                    SelectedUserPendingAmt = SelectedUserPendingAmt - CollectionAmt;
                    ClearField();
                }
                else
                {
                    MessageBox.Show("Amount is already entered for the user");
                }
       
            }
        }

        void ClearField([Optional]object parameter)
        {
            if (userCollectionModel == null)
            {
                userCollectionModel = new UserCollectionModel();
            }
            UserList = userCollectionModel.GetUserList(start, itemCount, sortColumn, ascending, searchID, out totalItems);
            SearchUserID = "";
            SelectedUserName = "";
            SelectedUserPendingAmt = 0;
            CollectionAmt = 0;
            EntryDate = DateTime.Today;
        }
        void GetUserBasedOnID(object parameter)
        {
            if (userCollectionModel == null)
            {
                userCollectionModel = new UserCollectionModel();
            }
            ObservableCollection<User> userList = new ObservableCollection<User>();
            if (SearchUserID.IsEmpty() == false)
            {
                var searchUserList = userCollectionModel.GetUserBasedOnID(SearchUserID);
                if (searchUserList.IsEmpty() == false)
                {
                    SelectedUserName = searchUserList[0].UserName;
                    SelectedUserPendingAmt = (Int64)searchUserList[0].CurrentAmt;
                    //UserList = UserList.Where(a=>a.ID=SearchUserID).ToList();
                    UserList = userCollectionModel.GetUserList(start, itemCount, sortColumn, ascending, SearchUserID, out totalItems);
                    NotifyPropertyChanged("Start");
                    NotifyPropertyChanged("End");
                    NotifyPropertyChanged("TotalItems");
                }
                else
                {
                    MessageBox.Show("User ID is not present");
                }
            }
            else
                MessageBox.Show("Enter User ID to search");
        }

        #endregion
    }
}
