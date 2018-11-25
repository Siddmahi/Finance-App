using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceApplication.Model;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.Common;
using RSA.Common.Utilities;


namespace FinanceApplication.Data
{
    class DatabaseLayer
    {

        public static List<User> GetUserList()
        {
            List<User> Employee = new List<User>();
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                DataTable dt = new DataTable();
                string query = "select ID,UserName,Address,Mobile,InitialAmount,DateOfJoining,DueDate,CurrentAmount from [MasterDB].[dbo].[USER_MAINTENANCE]";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                da.Dispose();
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new User()
                    {
                        ID = (string)HandleNulls.CheckNullString(row["ID"]),
                        UserName = (string)HandleNulls.CheckNullString(row["UserName"]),
                        Address = (string)HandleNulls.CheckNullString(row["Address"]),
                        Mobile = (string)HandleNulls.CheckNullString(row["Mobile"]),
                        InitialAmt = (int)HandleNulls.CheckNullInt(row["InitialAmount"]),
                        DateOfJoining = (DateTime)row["DateOfJoining"],
                        DueDate = (DateTime)row["DueDate"],
                        CurrentAmt = (int)HandleNulls.CheckNullInt(row["CurrentAmount"]),
                    };
                    Employee.Add(obj);
                }
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetUserList");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetUserList");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return Employee;
        }

        public static List<Account> GetAccountLog()
        {
            List<Account> accountLogList = new List<Account>();
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                DataTable dt = new DataTable();
                string query = "SELECT STARTING_BALANCE,ENTRY_DATE,INCOME_AMOUNT,EXPENSE_AMOUNT,DESCRIPTION,CLOSING_BALANCE  FROM MasterDB.dbo.ACCOUNTLOG_ENTRY";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                //(erow.SNO == null) ? string.Empty : erow.SNO.ToString()
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new Account()
                    {
                        StartingBalance = (int)HandleNulls.CheckNullInt(row["STARTING_BALANCE"]),
                        EntryDate = (DateTime)row["ENTRY_DATE"],
                        //OutstandingAmt = (int)row["OUTSTANDING_AMOUNT"],
                        AmountGiven = (int)HandleNulls.CheckNullInt(row["EXPENSE_AMOUNT"]),
                        Description = (string)HandleNulls.CheckNullString(row["DESCRIPTION"]),
                        CollectionAmt = (int)HandleNulls.CheckNullInt(row["INCOME_AMOUNT"]),
                        ClosingBalance = (int)HandleNulls.CheckNullInt(row["CLOSING_BALANCE"]),
                    };
                    accountLogList.Add(obj);
                }
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetAccountLog");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetAccountLog");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return accountLogList;
        }

        public static List<Master> GetMasterLog()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            List<Master> masterLogList = new List<Master>();
            try
            {
                DataTable dt = new DataTable();
                string query = "SELECT STARTING_BALANCE,ENTRY_DATE,INCOME_AMOUNT,EXPENSE_AMOUNT,DESCRIPTION,CLOSING_BALANCE  FROM MasterDB.dbo.MASTERLOG_ENTRY";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                da.Dispose();
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new Master()
                    {
                        OpeningBalance = (int)HandleNulls.CheckNullInt(row["STARTING_BALANCE"]),
                        EntryDate = (DateTime)HandleNulls.CheckNullDateTime(row["ENTRY_DATE"]),
                        Description = (string)HandleNulls.CheckNullString(row["DESCRIPTION"]),
                        IncomeAmount = (int)HandleNulls.CheckNullInt(row["INCOME_AMOUNT"]),
                        ExpenseAmount = (int)HandleNulls.CheckNullInt(row["EXPENSE_AMOUNT"]),
                        ClosingBalance = (int)HandleNulls.CheckNullInt(row["CLOSING_BALANCE"]),
                    };
                    masterLogList.Add(obj);
                }

            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetMasterLog");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetMasterLog");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return masterLogList;
        }

        public static List<CollectionEntry> GetCollectionLog()
        {
            List<CollectionEntry> collectionLogList = new List<CollectionEntry>();
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                DataTable dt = new DataTable();
                string query = "SELECT USER_ID,ENTRY_DATE,BALANCE_AMOUNT,AMOUNT,UPDATED_AMOUNT,NAME from [MasterDB].[dbo].[DAILY_AMOUNT_LOG]";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dt);
                conn.Close();
                da.Dispose();
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new CollectionEntry()
                    {
                        ID = (string)row["USER_ID"],
                        EntryDate = (DateTime)row["ENTRY_DATE"],
                        BalanceAmt = (int)row["BALANCE_AMOUNT"],
                        CurrentAmt = (int)row["AMOUNT"],
                        UpdatedAmt = (int)row["UPDATED_AMOUNT"],
                        UserName = (string)row["NAME"],
                    };
                    collectionLogList.Add(obj);
                }

            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetCollectionLog");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetCollectionLog");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return collectionLogList;
        }

        public static List<User> GetUserBasedOnID(string UserID)
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            List<User> selectedUser = new List<User>();
            try
            {
                DataTable dt = new DataTable();
                string getUser = "select UserName,CurrentAmount from [MasterDB].[dbo].[USER_MAINTENANCE] WHERE ID ={0} ";
                string query = string.Format(getUser, UserID);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                da.Dispose();
                foreach (DataRow row in dt.Rows)
                {
                    var obj = new User()
                    {
                        UserName = (string)HandleNulls.CheckNullString(row["UserName"]),
                        CurrentAmt = (int)HandleNulls.CheckNullInt(row["CurrentAmount"]),
                    };
                    selectedUser.Add(obj);
                }
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetUserBasedOnID");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetUserBasedOnID");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return selectedUser;

        }
        public static double CheckAccountBalance()
        {
            double collectionAmount = 0;
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                string query = "SELECT TOP 1 CLOSING_BALANCE FROM ACCOUNTLOG_ENTRY ORDER BY sg_key DESC ";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    collectionAmount = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                }

            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception CheckEntryForSameDate");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception CheckEntryForSameDate");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return collectionAmount;
        }
        public static string CheckEntryForSameDate(string UserID,string EntryDate)
        {
            string userEntry = string.Empty;
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                string constant = "SELECT USER_ID,ENTRY_DATE FROM [MasterDB].[dbo].[DAILY_AMOUNT_LOG]  WHERE USER_ID ='{0}' AND ENTRY_DATE='{1}' ";
                string query = string.Format(constant, UserID, EntryDate);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    userEntry = (string)rdr["USER_ID"];
                }
               
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception CheckEntryForSameDate");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception CheckEntryForSameDate");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return userEntry;
        }

        public static double CheckCollectionEntryForSameDate(string EntryDate)
        {
            double collectionAmount=0;
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                string constant = "SELECT SUM(INCOME_AMOUNT) as CollectionAmount  FROM [MasterDB].[dbo].[ACCOUNTLOG_ENTRY]  WHERE ENTRY_DATE='{0}' AND DESCRIPTION='Daily Collection' ";
                string query = string.Format(constant, EntryDate);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    collectionAmount = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                    //collectionAmount = (Int64)rdr["CollectionAmount"];
                }

            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception CheckCollectionEntryForSameDate");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception CheckCollectionEntryForSameDate");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return collectionAmount;
        }

        public static int GetMasterAccountOpeningBalance()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            int bal = 0;
            try
            {
                string query = "SELECT TOP 1 CLOSING_BALANCE FROM [MASTERLOG_ENTRY] ORDER BY sg_key DESC";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    bal = rdr.GetInt32(0);
                }
               
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetMasterAccountOpeningBalance");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetMasterAccountOpeningBalance");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return bal;

        }

        public static int GetAccountOpeningBalance()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            int bal = 0;
            try
            {
                string query = "SELECT TOP 1 CLOSING_BALANCE FROM ACCOUNTLOG_ENTRY ORDER BY sg_key DESC";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    bal = rdr.GetInt32(0);
                }
                conn.Close();
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetAccountOpeningBalance");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetAccountOpeningBalance");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return bal;

        }

        public static int GetCollectionAmount(string date)
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            int bal = 0;
            try
            {
                string query = "SELECT SUM(AMOUNT) FROM [MasterDB].[dbo].[DAILY_AMOUNT_LOG]  where ENTRY_DATE='{0}'";
                //select SUM(AMOUNT) FROM [MasterDB].[dbo].[DAILY_AMOUNT_LOG] where ENTRY_DATE='2017-01-20'
                string commandText = string.Format(query, date);

                conn.Open();
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    bal = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                    // bal = HandleNulls.CheckNullInt(rdr.GetInt32(0));
                }
                conn.Close();
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetCollectionAmount");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetCollectionAmount");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return bal;
        }

        public static List<CollectionEntry> GetUserCollectionList(string UserID)
        {
            List<CollectionEntry> userCollectionList = new List<CollectionEntry>();
            string conStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                DataTable dt = new DataTable();
                string getUser = "SELECT USER_ID,ENTRY_DATE,BALANCE_AMOUNT,AMOUNT,UPDATED_AMOUNT,NAME from [MasterDB].[dbo].[DAILY_AMOUNT_LOG] WHERE USER_ID = {0} ORDER BY ENTRY_DATE DESC ";
                string commandText = string.Format(getUser, UserID);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                foreach (DataRow row in dt.Rows)
                {
                    var obj = new CollectionEntry()
                    {
                        ID = (string)row["USER_ID"],
                        EntryDate = (DateTime)row["ENTRY_DATE"],
                        BalanceAmt = (int)row["BALANCE_AMOUNT"],
                        CurrentAmt = (int)row["AMOUNT"],
                        UpdatedAmt = (int)row["UPDATED_AMOUNT"],
                        UserName = (string)row["NAME"],
                    };
                    userCollectionList.Add(obj);
                }

            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception GetUserCollectionList");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception GetUserCollectionList");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                conn.Close();
            }
            return userCollectionList;
        }

        internal static void InsertMasterEntry(Master masterFields)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection con = new SqlConnection(connStr);
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("MASTER_LOG_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OPENING_BALANCE", masterFields.OpeningBalance);
                    cmd.Parameters.AddWithValue("@CLOSING_BALANCE", masterFields.ClosingBalance);
                    cmd.Parameters.AddWithValue("@INCOME_AMOUNT", masterFields.IncomeAmount);
                    cmd.Parameters.AddWithValue("@EXPENSE_AMOUNT", masterFields.ExpenseAmount);
                    cmd.Parameters.AddWithValue("@ENTRY_DATE", masterFields.EntryDate);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data Saved Successfully.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error!! Check the mandatory fields");
            }
            finally
            {
                con.Close();
            }
        }

        internal static void InsertAccountEntry(Account accountFields)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection con = new SqlConnection(connStr);
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("ACCOUNT_LOG_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OPENING_BALANCE", accountFields.StartingBalance);
                    cmd.Parameters.AddWithValue("@CLOSING_BALANCE", accountFields.ClosingBalance);
                    cmd.Parameters.AddWithValue("@AMOUNT_TYPE", accountFields.AmountType);
                    cmd.Parameters.AddWithValue("@AMOUNT", accountFields.Amount);
                    cmd.Parameters.AddWithValue("@DESCRIPTION", accountFields.Description);
                    cmd.Parameters.AddWithValue("@ENTRY_DATE", accountFields.EntryDate);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data Saved Successfully.");
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception InsertAccountEntry");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception InsertAccountEntry");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                con.Close();
            }
        } 
        internal static void UpdateAmountInUserLog(CollectionEntry dailyCollectionAmt)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection con = new SqlConnection(connStr);
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USER_DAILY_AMOUNT_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", dailyCollectionAmt.ID);
                    cmd.Parameters.AddWithValue("@CURRENT_AMOUNT", dailyCollectionAmt.UpdatedAmt);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                MessageBox.Show("Data Saved Successfully.");
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception UpdateAmountInUserLog");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception UpdateAmountInUserLog");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                con.Close();
            }
        }

        internal static void DailyCollectionEntry(CollectionEntry dailyCollectionAmt)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection con = new SqlConnection(connStr);
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DAILY_AMOUNT_LOG_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", dailyCollectionAmt.ID);
                    cmd.Parameters.AddWithValue("@NAME", dailyCollectionAmt.UserName);
                    cmd.Parameters.AddWithValue("@ENTRY_DATE", dailyCollectionAmt.EntryDate);
                    cmd.Parameters.AddWithValue("@BALANCE_AMOUNT", dailyCollectionAmt.BalanceAmt);
                    cmd.Parameters.AddWithValue("@AMOUNT", dailyCollectionAmt.CurrentAmt);
                    cmd.Parameters.AddWithValue("@UPDATED_AMOUNT", dailyCollectionAmt.UpdatedAmt);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                MessageBox.Show("Data Saved Successfully.");
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception DailyCollectionEntry");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception DailyCollectionEntry");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                con.Close();
            }
        }

        internal static void AccountCollectionEntry(CollectionEntry dailyCollectionAmt)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            SqlConnection con = new SqlConnection(connStr);
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ACCOUNT_COLLECTION_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ENTRY_DATE", dailyCollectionAmt.EntryDate);
                    cmd.Parameters.AddWithValue("@COLLECTION_AMOUNT", dailyCollectionAmt.CurrentAmt);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                MessageBox.Show("Data Saved Successfully.");
            }
            catch (DbException dbException)
            {
                MessageBox.Show("Error!! DB Exception AccountCollectionEntry");
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!! General Exception AccountCollectionEntry");
                ExceptionHandler.HandleException(exception);
            }
            finally
            {
                con.Close();
            }
        }
        internal static void InsertUser(User user)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                SqlConnection con = new SqlConnection(connStr);
                con.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT_USER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", user.ID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Mobile", user.Mobile);
                    cmd.Parameters.AddWithValue("@InitialAmt", user.InitialAmt);
                    cmd.Parameters.AddWithValue("@DateOfJoining", user.DateOfJoining);
                    cmd.Parameters.AddWithValue("@DueDate", user.DueDate);
                    cmd.Parameters.AddWithValue("@InterestRate", user.InterestRate);
                    cmd.Parameters.AddWithValue("@CurrentAmt", user.CurrentAmt);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (DbException dbException)
            {
                ExceptionHandler.HandleException(dbException);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }

        internal static void UpdateUser(User employee)
        {
            try
            {

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        internal static void DeleteUser(User employee)
        {
            try
            {

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


    }
}
