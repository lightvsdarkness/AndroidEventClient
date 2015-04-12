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
    public class WorkingInetAndSQL               //Класс к. сам решает, использовать БД или Инет
    {
        public static bool isSDPresent { get { return Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted; } }
        public static string destinationPath
        {
            get
            {
                if (WorkingInetAndSQL.isSDPresent)                                                                                       //Если есть external storage
                {
                    var directoryname = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "AEC");       //or External storage
                    Directory.CreateDirectory(directoryname);
                    return directoryname;           //Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/AEC/";
                }             
                else
                { return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); }           //or Environment.SpecialFolder.ApplicationData
            }
        }


        public static void CreateDBIfNeed(string copyingDB)
        {
            //Check if DB has already been extracted
            string filePath = destinationPath + "/" + copyingDB;
            if (!File.Exists(filePath))              //(!File.Exists(destinationPath))
            {
                string assetSource = "AEC.assets." + copyingDB;
                using (Stream source = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(assetSource))
                {
                    using (var destinationFilePath = System.IO.File.Create(Path.Combine(destinationPath, copyingDB)))
                    {
                        source.CopyTo(destinationFilePath);
                    }
                }
            }
            //ASYNC CREATION OF TABLE
            //var conn = new SQLiteAsyncConnection(destinationPath);
            //conn.CreateTableAsync<Account>().ContinueWith(t =>
            //{
            //    Account testAccount = new Account { Email = "Emeri " };//, Password = "Emeri" 
            //    conn.InsertAsync(testAccount).ContinueWith(t2 =>
            //    {
            //        //string.Console.WriteLine("Test Account: {0}", testAccount.ID); 
            //    });                  
            //});
        }
        public static void DeleteRowsInDBIfNeed(string deletedDB)
        {
            //Check if DB has already been extracted
            if (File.Exists(destinationPath))
            {
                using (var conn = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, deletedDB)))
                {
                    //var query = conn.Table<EventsShort>().Where(v => v.EventID.Equals(0));
                    for (int i = 1; i < 100; i++)
                    {
                        var rowcount = conn.Delete<EventsShort>(i);
                    }
                }
                
            }
        }

        //GET TEST ACCOUNT METHOD
        static public string GetTestAccount()
        {
            string testAccountDBPath = Path.Combine(destinationPath, "profile.sqlite");
            //var testAccount = new AccountProfile { Email = "Emeri", Password = "Emeri", PreferenceID = 2 };
            using (var db = new SQLite.SQLiteConnection(testAccountDBPath))                // former destinationPath
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
    }

    public class SavingEvents
    {
        static bool IsNullOrDefault<T>(T value)
        {
            return object.Equals(value, default(T));
        }

        //SAVING STUFF
        public static bool SavingShortEvents(string usedEventsDB, EventShort[] dwEvents)
        {
            bool eventSaved = false;
            for (int i = 0; i < dwEvents.Length; i++)
            {
                bool eventAlreadyInDB = false;
                eventSaved = false;
                if (IsNullOrDefault(dwEvents[i].Id) || IsNullOrDefault(dwEvents[i].Name) || IsNullOrDefault(dwEvents[i].Date) || IsNullOrDefault(dwEvents[i].OrganizerName)) { }       // || IsNullOrDefault(dwEvents[i].PrimaryPhotoId)
                else
                {
                    //Проверка, нет ли уже такой записи
                    using (var checkingEventExist = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedEventsDB), true))
                    {
                        var idOfEvent = dwEvents[i].Id;
                        var query = checkingEventExist.Table<EventsShort>().Where(v => v.EventServerID == idOfEvent);
                        //foreach (var checkedEvent in query) Console.WriteLine("We have information about Event with EventServerID: " + checkedEvent.EventServerID);
                        if (query.FirstOrDefault() != null) { eventAlreadyInDB = true; return eventAlreadyInDB; }
                    }

                    //Если данное событие отсутствует в базе данных, то:
                    if (!eventAlreadyInDB)
                    {
                        EventsShort curEvent;
                        if (dwEvents[i].PrimaryPhotoId.HasValue)
                        {
                            curEvent = new EventsShort { EventServerID = dwEvents[i].Id, EventName = dwEvents[i].Name, EventDateTime = dwEvents[i].Date, OrganizerName = dwEvents[i].OrganizerName, PhotoID = dwEvents[i].PrimaryPhotoId.Value };      // 
                        }
                        else
                        {
                            curEvent = new EventsShort { EventServerID = dwEvents[i].Id, EventName = dwEvents[i].Name, EventDateTime = dwEvents[i].Date, OrganizerName = dwEvents[i].OrganizerName };
                        }
                        //SYNCHRONOUS is Simpler
                        using (var db = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedEventsDB), true))
                        {
                            db.CreateTable<EventsShort>();
                            db.Insert(curEvent);
                            eventSaved = true;
                        }
                        //ASYNCHRONOUS is Cooler
                        //var db = new SQLite.SQLiteAsyncConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedEventsDB));
                        //db.CreateTableAsync<EventsShort>().ContinueWith (t => {db.InsertAsync(curEvent);});
                        //eventSaved = true;
                    }
                }

            }
            
            return eventSaved;
        }

    public static bool SavingFullEvents(EventShort[] dw)
        {
            bool saved = false;
            //string usedEventsDB = "events.sqlite";
            return saved;
        }

    public static bool SavingPhotos(Photo[] dw)
        {
            bool saved = false;
            //string usedEventsDB = "events.sqlite";
            return saved;
        }
    public static bool SavingCategory(CategoryShort[] categoryForSaving)
    {
        bool categorySaved = false;
        bool categoryAlreadyInDB = false;
        string usedDB = "chosenstuff.sqlite";

        for (int i = 0; i < categoryForSaving.Length; i++)
        {
            if (categoryForSaving[i] != null && categoryForSaving[i].Name != null)
            { 
                //Проверка, нет ли уже такой записи
                using (var checkingEventExist = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedDB), true))
                {
                    var idOfCategory = categoryForSaving[i].Id;
                    var query = checkingEventExist.Table<CatShort>().Where(v => v.CatID == idOfCategory);
                    if (query.FirstOrDefault() != null) { categoryAlreadyInDB = true; return categoryAlreadyInDB; }
                }

                //Если данное событие отсутствует в базе данных, то:
                if (!categoryAlreadyInDB)
                {
                    CatShort curCat;
                    curCat = new CatShort { ServerID = categoryForSaving[i].Id, Name = categoryForSaving[i].Name };

                    //SYNCHRONOUS is Simpler
                    using (var db = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedDB), true))
                    {
                        //db.CreateTable<CatShort>();
                        db.Insert(curCat);
                        categorySaved = true;
                    }
                }
            }
        }
        return categorySaved;
    }
    }

    //CLASSES FOR DATABASE ORM
    //PROFILES CLASSES
     public class AccountProfile {
        [PrimaryKey, AutoIncrement]
        public Int64 AccountID { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Group { get; set; }
        public string Unit { get; set; }
            //[ForeignKey(typeof(PreferenceProfile))]
        public int PreferenceID { get; set; }
     }
    public class PreferenceProfile {
        [PrimaryKey, AutoIncrement] //, SQLiteNetExtensions.Attributes.ForeignKey(typeof(AccountProfile))
        public int PreferenceID { get; set; }
        public string GPSUsage { get; set; }
        public string MobileInternetUsage { get; set; }
        public string WiFiUsage { get; set; }
        public string NotificationArea { get; set; }

    }
    //EVENTS CLASSES
    public class EventsShort {
        [PrimaryKey, AutoIncrement,]
        public int EventID { get; set; }
        public Int64 EventServerID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateTime { get; set; }
        public string OrganizerName { get; set; }
        //[ForeignKey(typeof(PreferenceProfile))]
        public Int64? PhotoID { get; set; }
    }

    public class EventsFull {

    }

    //CHOOSEN STUFF CLASSES
    public class CatShort {
        [PrimaryKey, AutoIncrement,]
        public Int64 CatID { get; set; }
        public Int64 ServerID { get; set; }
        public string Name { get; set; }
    }

    public class ChosenEvents
    {
        [PrimaryKey, AutoIncrement]
        public Int64 RecordID { get; set; }
        public Int64 AccountID { get; set; }
        public Int64 EventForVisit { get; set; }
    }

    public class EventsFilters
    {
        [PrimaryKey, AutoIncrement,]
        public Int64 RecordID { get; set; }
        public int FilterNumber { get; set; }
        public Int64 FilteringCategoryID { get; set; }
    }
}