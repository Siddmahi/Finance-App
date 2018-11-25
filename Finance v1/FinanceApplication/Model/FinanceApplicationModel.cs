using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FinanceApplication.Model;
using FinanceApplication.Data;
using System.Collections.ObjectModel;

namespace FinanceApplication.Model
{
    class FinanceApplicationModel
    {
        List<User> User { get; set; }
        List<CollectionEntry> SelectedUser { get; set; }

        public FinanceApplicationModel()
        {
            //User = DatabaseLayer.GetEmployeeFromDatabase();
        }

        public List<User> GetUserList()
        {
            return User = DatabaseLayer.GetUserList();
        }
        public List<Account> GetAccountLog()
        {
            return DatabaseLayer.GetAccountLog();
        }

        public double GetAccountBalance()
        {
            double accountBalance= DatabaseLayer.CheckAccountBalance();
            return accountBalance;
        }

        public ObservableCollection<Master> GetMasterLog(int start, int itemCount, bool ascending, out int totalItems)
        {
            List<Master> masterLogList = DatabaseLayer.GetMasterLog();
            ObservableCollection<Master> sortedMasterLogList = new ObservableCollection<Master>();

            sortedMasterLogList = new ObservableCollection<Master>
                           (
                               from p in masterLogList
                               orderby p.EntryDate descending
                               select p
                           );


            totalItems = sortedMasterLogList.Count;
            sortedMasterLogList = ascending ? sortedMasterLogList : new ObservableCollection<Master>(sortedMasterLogList.Reverse());

            ObservableCollection<Master> filteredProducts = new ObservableCollection<Master>();

            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                filteredProducts.Add(sortedMasterLogList[i]);
            }

            return filteredProducts;
            //return DatabaseLayer.GetMasterLog();
        }

        public ObservableCollection<Account> GetAccountsLog(int start, int itemCount, bool ascending, out int totalItems)
        {
            List<Account> accountsLogList = DatabaseLayer.GetAccountLog();
            ObservableCollection<Account> sortedAccountsLogList = new ObservableCollection<Account>();

            sortedAccountsLogList = new ObservableCollection<Account>
                           (
                               from p in accountsLogList
                               orderby p.EntryDate descending
                               select p
                           );


            totalItems = sortedAccountsLogList.Count;
            sortedAccountsLogList = ascending ? sortedAccountsLogList : new ObservableCollection<Account>(sortedAccountsLogList.Reverse());

            ObservableCollection<Account> filteredProducts = new ObservableCollection<Account>();

            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                filteredProducts.Add(sortedAccountsLogList[i]);
            }

            return filteredProducts;

        }

        public void UpdateAmountInUserLog(CollectionEntry dailyCollectionAmt)
        {
            dailyCollectionAmt.UpdatedAmt = dailyCollectionAmt.BalanceAmt - dailyCollectionAmt.CurrentAmt;
            DatabaseLayer.UpdateAmountInUserLog(dailyCollectionAmt);
        }

        public void AddEmployee(User user)
        {
            DatabaseLayer.InsertUser(user);
        }
        public void AddAccountEntry(Account accountFields)
        {
            DatabaseLayer.InsertAccountEntry(accountFields);
        }

    }



}
