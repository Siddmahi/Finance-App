using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FinanceApplication.Model;
using RSA.Common.Utilities;
using System.Runtime.InteropServices;

namespace FinanceApplication.ViewModel
{
    class FinanceApplicationMainWindowViewModel : INotifyPropertyChanged
    {
        // private User _userFields;
        FinanceApplicationModel financeModel;
        public ObservableCollection<User> _userFieldsList { get; set; }
        ObservableCollection<object> _children;
        UserCollectionModel userModel;

        public FinanceApplicationMainWindowViewModel()
        {
            _children = new ObservableCollection<object>();
            _children.Add(new UserCollectionViewModel());
            _children.Add(new ViewCollectionLogViewModel());
            _children.Add(new AccountViewModel());
            _children.Add(new MasterAccountViewModel());
            financeModel = new FinanceApplicationModel();
            // _userFieldsList = new ObservableCollection<User>(financeModel.GetUserList());
            dateOfJoining = DateTime.Today;

        }
        public ObservableCollection<object> Children { get { return _children; } }

        #region Commands
        private RelayCommand addUserCommand;
        public RelayCommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                    addUserCommand = new RelayCommand(AddUser);
                return addUserCommand;
            }
            private set
            {
                addUserCommand = value;
            }
        }

        private RelayCommand deleteUserCommand;
        public RelayCommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                    deleteUserCommand = new RelayCommand(DeleteUser);
                return deleteUserCommand;
            }
            private set
            {
                deleteUserCommand = value;
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
        private DateTime dueDate;
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }

        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("id"));
            }
        }

        private string _userName;
        public string userName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("userName"));
            }
        }

        private string _address;
        public string address
        {
            get { return _address; }
            set
            {
                _address = value;
                //  RaisePropertyChanged("Address");
                this.OnPropertyChanged(new PropertyChangedEventArgs("address"));
            }
        }

        private string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("mobile"));
                //  RaisePropertyChanged("Mobile");
            }
        }

        private Int64? _initialAmt;
        public Int64? initialAmt
        {
            get { return _initialAmt; }
            set
            {
                if (value.HasValue)
                {
                    _initialAmt = value;
                }
                this.OnPropertyChanged(new PropertyChangedEventArgs("initialAmt"));
                // currentAmt = initialAmt;
            }
        }
        private Int64? _interestAmt;
        public Int64? interestAmt
        {
            get { return _interestAmt; }
            set
            {
                if (value.HasValue)
                {
                    _interestAmt = value;
                }
                this.OnPropertyChanged(new PropertyChangedEventArgs("initialAmt"));
                currentAmt = initialAmt + _interestAmt;
            }
        }
        private DateTime _dateOfJoining;
        public DateTime dateOfJoining
        {
            get
            {
                return _dateOfJoining;
            }
            set
            {

                _dateOfJoining = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("dateOfJoining"));
                //  RaisePropertyChanged("DateOfJoining");
            }
        }
        private Int64? _currentAmt;
        public Int64? currentAmt
        {
            get { return _currentAmt; }
            set
            {
                _currentAmt = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("currentAmt"));
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

        #endregion

        #region Methods

        void AddUser(object parameter)
        {
            User userFields = new User()
            {
                ID = id,
                UserName = userName,
                Address = address,
                Mobile = mobile,
                InitialAmt = initialAmt,
                DateOfJoining = dateOfJoining,
                InterestRate = interestAmt,
                CurrentAmt = currentAmt
            };

            if (id == null || userName == null || address == null || mobile == null || initialAmt == null || dateOfJoining == null || currentAmt == null)
            {
                MessageBox.Show("Fill in all the mandatory fields");
            }
            else
            {
                userModel = new UserCollectionModel();
                userFields.DueDate = userFields.DateOfJoining.AddDays(100);
                List<User> userIDAvailable = userModel.GetUserBasedOnID(id);
                double accountBalance = financeModel.GetAccountBalance();
                if (accountBalance > currentAmt)
                {
                    if (userIDAvailable.IsEmpty())
                    {
                        financeModel.AddEmployee(userFields);
                        MessageBox.Show("Data Saved Successfully.");
                        ClearField();
                    }
                    else
                    {
                        MessageBox.Show("User ID is already present");
                    }
                }
                else
                {
                    MessageBox.Show("Account Balance is low and maximum amount is " + accountBalance);
                }
            }

        }

        void DeleteUser(object parameter)
        {

        }

        void ClearField([Optional]object parameter)
        {
            id = string.Empty;
            userName = string.Empty;
            address = string.Empty;
            mobile = string.Empty;
            initialAmt = 0;
            dateOfJoining = DateTime.Now;
            interestAmt = 0;
            currentAmt = 0;
        }

        #endregion

    }
}
