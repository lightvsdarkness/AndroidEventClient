using System.IO;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using SQLite;
//using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Android;
using Android.Content;
using AEC.Service;

namespace AEC.Service
{
    public class WorkingWithAccount
    {
        public static string accountDBPath = Path.Combine(WorkingInetAndSQL.destinationPath, "profile.sqlite");
        public static bool SavingVisitEvent(Int64 serverEventID)
        {
            string usedDB = "chosenstuff.sqlite";
            Int64 accountID = 1;

            bool visitEventSaved = false;
            bool visitEventAlreadyInDB = false;

            if (false)              //ѕроверка, что такое меропри€тие есть на сервере
            { }
            else
            {
                //ѕроверка, нет ли уже такой записи
                using (var checkingEventExist = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedDB)))
                {
                    var query = checkingEventExist.Table<ChosenEvents>().Where(v => v.EventForVisit == serverEventID);
                    if (query.FirstOrDefault() != null)
                    {
                        visitEventAlreadyInDB = true;
                        return visitEventAlreadyInDB;
                    }
                }
                //≈сли данное событие отсутствует в базе данных, то:
                if (!visitEventAlreadyInDB)
                {
                    var curEvent = new ChosenEvents { AccountID = accountID, EventForVisit = serverEventID };      // 
                    //SYNCHRONOUS is Simpler
                    using (var db = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedDB)))
                    {
                        db.CreateTable<ChosenEvents>();
                        db.Insert(curEvent);
                        visitEventSaved = true;
                    }

                }
            }

            return visitEventSaved;
        }
        public static void GettingVisitEvents(Int64 currAccountID)
        {
        }

        //TEST
        static public string GetTestAccount()
        {
            string testAccountDBPath = Path.Combine(WorkingInetAndSQL.destinationPath, "profile.sqlite");
            //var testAccount = new AccountProfile { Email = "Emeri", Password = "Emeri", PreferenceID = 2 };
            using (var db = new SQLite.SQLiteConnection(testAccountDBPath))
            {
                var stock = db.Get<AccountProfile>(1);
                return Convert.ToString(stock.Email);
            }
            //string Ee = "empty";
            //try
            //{
            //var query = conn.Table<Account>().Where(v => v.Email.StartsWith("E"));
            //query.ToListAsync().ContinueWith(t3 =>
            //{
            //    foreach (var testAccount in t3.Result) Ee = testAccount.Email; //Console.WriteLine("Stock: " + testAccount.Email);
            //});
            //return Ee;  
            //}
            //catch (Exception e) { var k = e.Message; }
            //return Ee;  
        }

        //REAL
        static public string GetAccount()
        {
            
            //var testAccount = new AccountProfile { Email = "Emeri", Password = "Emeri", PreferenceID = 2 };
            using (var db = new SQLite.SQLiteConnection(accountDBPath))
            {
                var stock = db.Get<AccountProfile>(1);
                return Convert.ToString(stock.Email);
            }
            //string Ee = "empty";
            //try
            //{
            //var query = conn.Table<Account>().Where(v => v.Email.StartsWith("E"));
            //query.ToListAsync().ContinueWith(t3 =>
            //{
            //    foreach (var testAccount in t3.Result) Ee = testAccount.Email; //Console.WriteLine("Stock: " + testAccount.Email);
            //});
            //return Ee;  
            //}
            //catch (Exception e) { var k = e.Message; }
            //return Ee;  
        }

        public static bool SaveAccountToDB(string alias, string email, string login, string password, string gender, string group, string unit)
        {
            bool savedAcc = false;
            var newAccount = new AccountProfile { FIO = alias, Email = email, Password = password, Gender = gender, Group = group, Unit = unit, PreferenceID = 2 };
            using (var db = new SQLite.SQLiteConnection(accountDBPath))
            {
                db.CreateTable<AccountProfile>();
                db.Insert(newAccount);
            }

            savedAcc = true;
            return savedAcc;
        }
    
        //
        //RegisterNewAccount(DTO.AccountPut account, DTO.Captcha captcha);
        public static bool RegisterAccount(string alias, string email, string login, string password, string gender, string group, string unit)        // List<string> registeringAcc
        {
            bool registeredAcc = false;
            AccountPut newAcc = new AccountPut();
            newAcc.Alias = alias;
            newAcc.EMail = email;
            newAcc.Login = login;
            newAcc.Password = password;
            newAcc.Avatar = new byte[1];
            newAcc.Id = 100000;
            //newAcc.IdSpecified = false;

            //TO DO
            //using (var client = new UserClientExchange())
            //{
            //    client.RegisterNewAccount(newAcc, new Captcha());
            //}

            //private byte[] avatarField;
            //private long idField;
            //private bool idFieldSpecified;

            registeredAcc = true;
            SaveAccountToDB(alias, email, login, password, gender, group, unit);
            return registeredAcc;
        }

    }
    public class AccountService
    {
        public void CalendarSync(Int64 eventServerID)
        {

        }
    }

    public class FilterWorks
    {
        public static bool UpdateFilter(int filterNumber, long filteringCategoryID, bool addOrSubstract)
        {
            bool updatedFilt = false;
            if (addOrSubstract)             //
            {
                var updEventsFilter = new EventsFilters { FilterNumber = filterNumber, FilteringCategoryID = filteringCategoryID };      // 
                using (var db = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, "choosenstuff.sqlite"), true))
                {
                    db.CreateTable<EventsFilters>();
                    db.Insert(updEventsFilter);
                }
                updatedFilt = true;
            }
            else             //
            {

                updatedFilt = true;
            }
            return updatedFilt;
        }
    }
}