using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using RSA.Common.Utilities;
using System.Windows.Input;
using System.Runtime.InteropServices;
using FinanceApplication.Model;

namespace FinanceApplication.ViewModel
{
    class ViewCollectionLogViewModel : INotifyPropertyChanged
    {

        UserCollectionModel userCollectionModel;

        #region Private Fields

        private ObservableCollection<CollectionEntry> collectionLogList;

        private int start = 0;

        private int itemCount = 15;

        private string sortColumn = "ID";

        private string selectedDate = string.Empty;

        private string userID = string.Empty;

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
                            GetCollectionLogs();
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
                            GetCollectionLogs();
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
                            GetCollectionLogs();
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
                            GetCollectionLogs();
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

            GetCollectionLogs();
        }

        #endregion

        public ObservableCollection<CollectionEntry> CollectionLogList
        {
            get
            {
                return collectionLogList;
            }
            private set
            {
                if (object.ReferenceEquals(collectionLogList, value) != true)
                {
                    collectionLogList = value;
                    NotifyPropertyChanged("CollectionLogList");
                }
            }
        }
        public ViewCollectionLogViewModel()
        {
            userCollectionModel = new UserCollectionModel();
            GetCollectionLogs();

            CollectionEntryDate = DateTime.Today;
        }
        #region Properties
        private DateTime collectionEntryDate;
        public DateTime CollectionEntryDate
        {
            get
            {
                return collectionEntryDate;
            }
            set
            {
                collectionEntryDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("CollectionEntryDate"));
                //OnPropertyChanged("CollectionEntryDate");
            }
        }
        private Int64? totalCollection;
        public Int64? TotalCollection
        {
            get { return totalCollection; }
            set
            {
                //if (value.HasValue)
                //{
                totalCollection = value;
                //  }
                this.OnPropertyChanged(new PropertyChangedEventArgs("TotalCollection"));
                // OnPropertyChanged("TotalCollection");
            }
        }
        private string searchID;
        public string SearchID
        {
            get { return searchID; }
            set
            {
                searchID = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SearchID"));
                //OnPropertyChanged("SearchUserID");
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
                    submitCommand = new RelayCommand(SubmitAccountCollectionAmount);
                return submitCommand;
            }
            private set
            {
                submitCommand = value;
            }
        }
        private RelayCommand resetFieldsCommand;
        public RelayCommand ResetFieldsCommand
        {
            get
            {
                if (resetFieldsCommand == null)
                    resetFieldsCommand = new RelayCommand(ClearFieldEntries);
                return resetFieldsCommand;
            }
            private set
            {
                resetFieldsCommand = value;
            }
        }
        private RelayCommand searchCommmand;
        public RelayCommand SearchCommmand
        {
            get
            {
                if (searchCommmand == null)
                {
                    searchCommmand = new RelayCommand(GetUserCollection);
                }
                return searchCommmand;
            }
            private set
            {
                searchCommmand = value;
            }
        }
        private RelayCommand getCollectionCommmand;
        public RelayCommand GetCollectionCommmand
        {
            get
            {
                if (getCollectionCommmand == null)
                    getCollectionCommmand = new RelayCommand(GetCollection);
                return getCollectionCommmand;
            }
            private set
            {
                getCollectionCommmand = value;
            }
        }
        #endregion

        #region Methods

        public void GetCollectionLogs()
        {
            //CollectionLogList = new ObservableCollection<CollectionEntry>(userCollectionModel.GetCollectionLog());
            if (userCollectionModel == null)
            {
                userCollectionModel = new UserCollectionModel();
            }
            CollectionLogList = userCollectionModel.GetCollectionsLog(start, itemCount, ascending, userID, selectedDate, out totalItems);
            // userList = new ObservableCollection<User>(userCollectionModel.GetUserList(), start, itemCount, sortColumn, ascending, out totalItems);
            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }


        void SubmitAccountCollectionAmount(object parameter)
        {

            CollectionEntry dailyCollectionAmt = new CollectionEntry()
            {
                EntryDate = CollectionEntryDate,
                CurrentAmt = TotalCollection
            };

            if (TotalCollection <= 0)
            {
                MessageBox.Show("Amount should be more than 0");
            }
            else
            {
                //userCollectionModel.AccountCollectionEntry(dailyCollectionAmt);
                //ClearFieldEntries();

                string selectedDate = (string.Format("{0:yyyy-MM-dd H:mm:ss}", CollectionEntryDate));
                double collectionAmount = userCollectionModel.CheckCollectionEntryForSameDate(selectedDate);
                if (collectionAmount == 0)
                {
                    userCollectionModel.AccountCollectionEntry(dailyCollectionAmt);
                    ClearFieldEntries();
                }
                else
                {
                    if (TotalCollection != collectionAmount)
                    {
                        long? updatedCollectionAmt = TotalCollection - Convert.ToInt64(collectionAmount);
                        CollectionEntry updatedCollection = new CollectionEntry()
                        {
                            EntryDate = CollectionEntryDate,
                            CurrentAmt = updatedCollectionAmt
                        };
                        MessageBox.Show("Collection Entered is " + updatedCollectionAmt);
                        userCollectionModel.AccountCollectionEntry(updatedCollection);
                        ClearFieldEntries();
                    }
                    else
                    {
                        MessageBox.Show("Amount is already entered for the date");
                    }

                }
            }


        }

        void GetCollection(object parameter)
        {
            if (CollectionEntryDate == null)
            {
                MessageBox.Show("Select Date");
            }
            else
            {
                selectedDate = (string.Format("{0:yyyy-MM-dd H:mm:ss}", collectionEntryDate));
                TotalCollection = userCollectionModel.GetCollectionAmount(selectedDate);
                if (TotalCollection > 0)
                {
                    //double collectionAmount = userCollectionModel.CheckCollectionEntryForSameDate(selectedDate);
                    //if (collectionAmount > 0)
                    //{
                    //    TotalCollection = TotalCollection - Convert.ToInt64(collectionAmount);
                    //}
                    CollectionLogList = userCollectionModel.GetCollectionsLog(start, itemCount, ascending, userID, selectedDate, out totalItems);
                    NotifyPropertyChanged("Start");
                    NotifyPropertyChanged("End");
                    NotifyPropertyChanged("TotalItems");
                }
                else
                {
                    MessageBox.Show("No data present for the selected date");
                }
            }

        }
        void GetUserCollection(object parameter)
        {
            if (SearchID.IsEmpty() == false)
            {
                //collectionLogList = new ObservableCollection<CollectionEntry>(userCollectionModel.GetUserCollectionList(SearchID));
                CollectionLogList = userCollectionModel.GetCollectionsLog(start, itemCount, ascending, SearchID, selectedDate, out totalItems);
                NotifyPropertyChanged("Start");
                NotifyPropertyChanged("End");
                NotifyPropertyChanged("TotalItems");
                if (CollectionLogList.IsEmpty() == false)
                {
                    this.OnPropertyChanged(new PropertyChangedEventArgs("collectionLogList"));
                }
                else
                {
                    MessageBox.Show("User ID is not present");
                }
            }
            else
                MessageBox.Show("Enter User ID to search");
        }

        public void ClearFieldEntries([Optional] object parameter)
        {
            TotalCollection = 0;
            CollectionEntryDate = DateTime.Today;
            SearchID = string.Empty;
            selectedDate = string.Empty;
            GetCollectionLogs();
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
    }

}
