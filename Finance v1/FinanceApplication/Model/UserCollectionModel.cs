using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceApplication.Data;
using System.Collections.ObjectModel;
using RSA.Common.Utilities;

namespace FinanceApplication.Model
{
    class UserCollectionModel
    {
        public int GetOpeningBalance()
        {
            return DatabaseLayer.GetAccountOpeningBalance();
        }
        public Int64? GetCollectionAmount(string date)
        {
            return DatabaseLayer.GetCollectionAmount(date);
        }
        public List<CollectionEntry> GetCollectionLog()
        {
            return DatabaseLayer.GetCollectionLog();
        }
        public ObservableCollection<CollectionEntry> GetCollectionsLog(int start, int itemCount, bool ascending,string userID, string entryDate, out int totalItems)
        {
            List<CollectionEntry> collectionList = DatabaseLayer.GetCollectionLog();
            ObservableCollection<CollectionEntry> sortedCollectionList = new ObservableCollection<CollectionEntry>();
            
            if (entryDate.IsVoid())
            {
                if (userID.IsVoid())
                {
                    sortedCollectionList = new ObservableCollection<CollectionEntry>
                               (
                                   from p in collectionList
                                   orderby p.EntryDate descending
                                   select p
                               );
                }
                else
                {
                    sortedCollectionList = new ObservableCollection<CollectionEntry>
                                   (
                                       from p in collectionList
                                       where p.ID == userID
                                       orderby p.EntryDate descending
                                       select p
                                   );
                }
            }
            else
            {
                DateTime selectedDate = DateTime.ParseExact(entryDate, "yyyy-MM-dd H:mm:ss", null);
                sortedCollectionList = new ObservableCollection<CollectionEntry>
                          (
                              from p in collectionList
                              where p.EntryDate == selectedDate
                              orderby p.EntryDate descending
                              select p
                          );
            }


            totalItems = sortedCollectionList.Count;
            sortedCollectionList = ascending ? sortedCollectionList : new ObservableCollection<CollectionEntry>(sortedCollectionList.Reverse());

            ObservableCollection<CollectionEntry> filteredProducts = new ObservableCollection<CollectionEntry>();

            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                filteredProducts.Add(sortedCollectionList[i]);
            }

            return filteredProducts;

        }
        public ObservableCollection<User> GetUserList(int start, int itemCount, string sortColumn, bool ascending, string searchID, out int totalItems)
        {
            List<User> usersList = DatabaseLayer.GetUserList();
            ObservableCollection<User> sortedUserList = new ObservableCollection<User>();
            if (searchID != string.Empty)
            {
                sortedUserList = new ObservableCollection<User>
                           (
                               from p in usersList
                               where p.ID == searchID
                               orderby p.ID
                               select p
                           );
            }
            else
            {
                sortedUserList = new ObservableCollection<User>
                (
                    from p in usersList
                    orderby p.ID
                    select p
                );
            }

            totalItems = sortedUserList.Count;
            sortedUserList = ascending ? sortedUserList : new ObservableCollection<User>(sortedUserList.Reverse());

            ObservableCollection<User> filteredProducts = new ObservableCollection<User>();

            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                filteredProducts.Add(sortedUserList[i]);
            }

            return filteredProducts;

        }
        public List<User> GetUserBasedOnID(string userID)
        {
            return DatabaseLayer.GetUserBasedOnID(userID);
        }
        //public List<CollectionEntry> GetUserCollectionList(string userID)
        //{
        //    return DatabaseLayer.GetUserCollectionList(userID);
        //}


        public void DailyCollectionEntry(CollectionEntry dailyCollectionAmt)
        {
            dailyCollectionAmt.UpdatedAmt = dailyCollectionAmt.BalanceAmt - dailyCollectionAmt.CurrentAmt;
            //  DatabaseLayer.UpdateAmountInUserLog(dailyCollectionAmt);
            DatabaseLayer.DailyCollectionEntry(dailyCollectionAmt);
        }

        public string CheckEntryForSameDate(string UserID, string EntryDate)
        {
            string user = DatabaseLayer.CheckEntryForSameDate(UserID, EntryDate);
            return user;
        }

        public double CheckCollectionEntryForSameDate(string EntryDate)
        {
            double collectionAmount = DatabaseLayer.CheckCollectionEntryForSameDate(EntryDate);
            return collectionAmount;
        }

        public void AccountCollectionEntry(CollectionEntry dailyCollectionAmt)
        {
            dailyCollectionAmt.UpdatedAmt = dailyCollectionAmt.BalanceAmt - dailyCollectionAmt.CurrentAmt;
            //  DatabaseLayer.UpdateAmountInUserLog(dailyCollectionAmt);
            DatabaseLayer.AccountCollectionEntry(dailyCollectionAmt);
        }
    }
}
