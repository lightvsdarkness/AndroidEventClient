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
using Android.Content.Res;
using AEC.Service;

namespace AEC.Service
{
    public class WorkingInetInteractDB
    {
        public const string aecServiceIntent = "aecServiceIntent";
        public const string aecActivityIntent = "aecActivityIntent";
        public const string aecSplashscreenIntent = "aecSplashscreenIntent";
        public const string aecCalendarIntent = "aecCalendarIntent";

        public string TestSyncSessionID;

        //Context context = Android.App.Application.Context;        public Android.Content.Res.Resources res = context.Resources;
        public static Android.Content.Res.Resources res
        {
            get
            {
                Context context = Android.App.Application.Context;
                return context.Resources;
            }
        }

	    //Singletone stuff
	    protected static WorkingInetInteractDB m_MyObject = null;
	    public static WorkingInetInteractDB GetObject()
	    {
		    if (m_MyObject == null)
		    {
		    m_MyObject = new WorkingInetInteractDB();
		    }
		    return m_MyObject;
	    }

        //��������, �. �������� ������� ��������� ������� �� ���������� ������ GetEventsDataInternal
        public static EventShort[] ListCurrentEvents
        {
            get;
            set;
        }

        //�����, �. ����������


	    //Checking Inet
	    public bool WeHaveInetData { get; set; }                //���� �� ��� ������ ������
        public bool CheckServerDataPresent(bool hardwareOn)
        {
            WeHaveInetData = false;
            //TO DO
            //if (hardwareOn == true)
            //{
            //    using  (UserClientExchange clientTestingInet = new UserClientExchange())
            //        { 
            //            try {
            //                TestSyncSessionID = clientTestingInet.Login("Emeri", "Emeri", new Captcha());
            //                WeHaveInetData = true;
            //                clientTestingInet.Logout(TestSyncSessionID);
            //                return true;
            //                }
            //            catch (Exception e) { var k = e.Message; }
            //        }
            //}
            //else { return false; }
            return false;
        }

        //���������� ����� ��� ��������� ��������� �������
        private EventShort[] GetEventsDataInternal(int whatevents)
	    {
            EventShort[] NewEvents = new EventShort[1] { new EventShort() };
            //TO DO
            //using (var client = new UserClientExchange())
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        try
            //        {
            //            var syncSessionId = client.Login("Emeri", "Emeri", new Captcha());
            //            NewEvents = client.GetEventsList(syncSessionId, 0, true, whatevents, true);
            //            SavingEvents.SavingShortEvents("events.sqlite", NewEvents);                                   //�������� ��� ���������� � �� ������ ��������� �������� �������� �������
            //            client.Logout(syncSessionId);



            //            return NewEvents;
            //        }
            //        catch (Exception r)
            //        {
            //            var l = r.Message;
            //            //� ������� �������� ��� ���������!
            //        }
            //    }

            //}
            WeHaveInetData = false;
            if (!WeHaveInetData)                //���� ������ �� ������� ��� - ������� ��� ���� �� ��������� ��
            {             
                string usedEventsDB = "events.sqlite";
                List<EventShort> fittingEvents = new List<EventShort>();
                using (var eventFromDB = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedEventsDB), true))
                {
                    var query = eventFromDB.Query<EventsShort>("SELECT * FROM EventsShort WHERE EventDateTime > ?", new DateTime(2014, 04, 01).Ticks);                  //"SELECT * FROM EventsShort WHERE EventDateTime > ? AND EventDateTime < ?", new DateTime(2013, 01, 01, 00, 00, 00), new DateTime(2020, 01, 01, 00, 00, 00));
                    if (query.FirstOrDefault() != null)
                    { 
                        List<EventsShort> queryList = query.ToList<EventsShort>();
                        foreach(EventsShort currEvent in queryList)
                        {
                            EventShort requestedEventShort = new EventShort();
                            requestedEventShort.Id = currEvent.EventServerID;
                            if (currEvent.EventName != null) { requestedEventShort.Name = currEvent.EventName; }
                            else { requestedEventShort.Name = "UFO"; }
                            if (currEvent.EventDateTime != null) { requestedEventShort.Date = currEvent.EventDateTime; }
                            else { requestedEventShort.Date = DateTime.Today; }
                            if (currEvent.OrganizerName != null) { requestedEventShort.OrganizerName = currEvent.OrganizerName; }
                            else { requestedEventShort.OrganizerName = "Universum"; }
                            if (currEvent.PhotoID != null) { requestedEventShort.PrimaryPhotoId = currEvent.PhotoID; }
                            fittingEvents.Add(requestedEventShort);
                        }
                    }
                    else
                    {
                        EventShort emptyItem = new EventShort();
                        emptyItem.Id = 1001;
                        emptyItem.Name = "������� �� �������";
                        emptyItem.Date = DateTime.Today;
                        emptyItem.OrganizerName = "Universum";
                        //emptyItem.PrimaryPhotoId = 1;
                        fittingEvents.Add(emptyItem);
                    }
                    NewEvents = fittingEvents.ToArray();
                }
            }
            return NewEvents;
	    }

        //���������� ����� ��� ��������� ����
        private List<Android.Graphics.Bitmap> GetEventsPhotosInternal(EventShort[] NewEvents)             //it was Photo[]
        {
            List<Android.Graphics.Bitmap> newPhotos = new List<Android.Graphics.Bitmap>();
            if (CheckServerDataPresent(true))                           //���� ������ �� ������� ����
                {
                    //TO DO
                    //using (var client = new UserClientExchange())
                    //{
                    //    var syncSessionId = client.Login("Emeri", "Emeri", new Captcha());
                    //    Photo[] serverPhotos = new Photo[NewEvents.Length];

                    //    //���� �� ���� ��������
                    //    for (int i = 0; i < NewEvents.Length; i += 1)
                    //    {
                    //        //��������� ���������� � ������
                    //        if (NewEvents[i].PrimaryPhotoId.HasValue)          //���� � ������� ���� ����   NewEvents[i]. || 
                    //        {
                    //            try
                    //            {
                    //                for (int howManyTimesTryDownload = 0; howManyTimesTryDownload < 3; howManyTimesTryDownload++)
                    //                {
                    //                    //���������, ��� �� � ��� ��� ����
                    //                    string filename = "EventMainPhoto_" + NewEvents[i].PrimaryPhotoId;
                    //                    if (!File.Exists(Path.Combine(WorkingInetAndSQL.destinationPath, filename)))         //���� ���� ��� �� �����, �� ��������� ��������� ���� � �������� � List
                    //                    {
                    //                        serverPhotos[i] = client.GetPhoto(syncSessionId, NewEvents[i].PrimaryPhotoId.Value, true);

                    //                        //��������� Bitmap � List
                    //                        Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(serverPhotos[i].Data, 0, serverPhotos[i].Data.Length);
                    //                        newPhotos.Add(bmp);
                    //                        //��������� ����� �� ����
                    //                        using (var stream = new BufferedStream(File.OpenWrite(Path.Combine(WorkingInetAndSQL.destinationPath, filename))))
                    //                            bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
                    //                    }
                    //                    else                         //���� �� ��� ��� ���� �� �����, ������ ����� � �����
                    //                    {
                    //                        Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(Path.Combine(WorkingInetAndSQL.destinationPath, filename));
                    //                        newPhotos.Add(bmp);
                    //                    }

                    //                    howManyTimesTryDownload = 3;
                    //                }
                    //            }
                    //            catch (Exception e)
                    //            {
                    //                var k = e.Message;
                    //                //� ������� �������� ��� ���������!
                    //            }




                    //        }
                    //        else                                        //���� ���� ���, ���������� ����������� ���� ��������� 
                    //        {
                    //            var options = new Android.Graphics.BitmapFactory.Options
                    //            {
                    //                InJustDecodeBounds = false,
                    //            };
                    //            Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeResource(res, Resource.Drawable.Leo, options);
                    //            newPhotos.Add(bmp);
                    //        }

                    //    }

                    //    client.Logout(syncSessionId);
                    //}

                }
                else                          //���� ������ �� ������� ���
                {
                    //���� �� ���� ��������
                    for (int i = 0; i < NewEvents.Length; i += 1)
                    {
                        //��������� ���������� � ������
                        if (NewEvents[i].PrimaryPhotoId.HasValue)          //���� � ������� ���� ����
                        {
                            string filename = "EventMainPhoto_" + NewEvents[i].PrimaryPhotoId;
                            if (File.Exists(Path.Combine(WorkingInetAndSQL.destinationPath, filename)))         //���� ��� ��� ���� �� �����, ������ ����� � �����
                            {
                                Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(Path.Combine(WorkingInetAndSQL.destinationPath, filename));
                                newPhotos.Add(bmp);
                            }
                            else                //���� � ������� ������ ���� ����, �� �� ����� ���
                            {
                                var options = new Android.Graphics.BitmapFactory.Options
                                {
                                    InJustDecodeBounds = false,
                                };
                                Context context = Android.App.Application.Context; Android.Content.Res.Resources res = context.Resources;
                                Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeResource(res, Resource.Drawable.Leo, options);
                                newPhotos.Add(bmp);
                            }
                        }

                        else                    //���� � ������� ���� ���
                        {
                        var options = new Android.Graphics.BitmapFactory.Options
                        {
                            InJustDecodeBounds = false,
                        };
                        Context context = Android.App.Application.Context; Android.Content.Res.Resources res = context.Resources;
                        Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeResource(res, Resource.Drawable.Leo, options);
                        newPhotos.Add(bmp);
                        }
                    }
                }
            return newPhotos;
        }
        private EventFullGet GetEventFullDataInternal(Int64 eventId)             //, out DTO.EventFullGet eventDescription
        {
            return new EventFullGet();

            //TO DO

            //using (var client = new UserClientExchange())
            //{
            //    EventFullGet eventFullData = new EventFullGet();
            //    try
            //    {
            //        for (int howManyTimesTryDownload = 0; howManyTimesTryDownload < 3; howManyTimesTryDownload++)
            //        {
            //            var syncSessionId = client.Login("Emeri", "Emeri", new Captcha());
            //            eventFullData = client.GetEventFullDescription(syncSessionId, eventId, true);             //string sessionId, Int64 eventId, out DTO.EventFullGet eventDescription
            //            client.Logout(syncSessionId);

            //            howManyTimesTryDownload = 3;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        var k = e.Message;
            //        //� ������� �������� ��� ���������!
            //    }

            //    return eventFullData;
            //}
        }
        //���������
        private CategoryShort[] GetCategoryDataInternal(Int64 categoryParentId) {
            //�������� ������� ������ �� ����������
            CategoryShort[] NewCategories = new CategoryShort[1] { new CategoryShort() };
            //TO DO
            //using (var client = new UserClientExchange())
            //{
            //    try
            //    {
            //        for (int howManyTimesTryDownload = 0; howManyTimesTryDownload < 3; howManyTimesTryDownload++)
            //        {
            //            var sessionId = client.Login("Emeri", "Emeri", new Captcha());
            //            NewCategories = client.GetCategoriesListForCurrentAccount(sessionId, categoryParentId, true, 30, true, 1, false, 50, true);
            //            client.Logout(sessionId);
            //            if (NewCategories != null)
            //            { bool savedOrNot = SavingEvents.SavingCategory(NewCategories); }

            //            howManyTimesTryDownload = 3;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        var k = e.Message;
            //        //� ������� �������� ��� ���������!
            //    }
            //}
            return NewCategories;
        }
        private CategoryShort[] GetVersionByCategoryInternal(Int64 rootVersionCategoryID, Int64 versionCategoryID) {
            //�������� ������� ������ �� ����������
            CategoryShort[] RootCategories = new CategoryShort[] {};
            CategoryShort[] versionCategory = new CategoryShort[1] { new CategoryShort() };
            if (WeHaveInetData)
            { }
            else { }
            //TO DO
            //using (var client = new UserClientExchange())
            //{
            //    try
            //    {
            //        for (int howManyTimesTryDownload = 0; howManyTimesTryDownload < 3; howManyTimesTryDownload++)
            //        {
            //            var sessionId = client.Login("Emeri", "Emeri", new Captcha());
            //            RootCategories = client.GetCategoriesListForCurrentAccount(sessionId, rootVersionCategoryID, true, rootVersionCategoryID, true, 1, false, 50, true);
            //            client.Logout(sessionId);

            //            versionCategory = RootCategories.Where(v => v.Id == versionCategoryID).ToArray();
            //            if (versionCategory != null)
            //            { bool savedOrNot = SavingEvents.SavingCategory(versionCategory); }

            //            howManyTimesTryDownload = 3;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        var k = e.Message;
            //        //� ������� �������� ��� ���������!
            //    }
            //}
            return versionCategory;
        }
        private string GetCategoryNameFromDBInternal(Int64 categoryId)
        {
            string usedDB = "chosenstuff.sqlite";
            CatShort[] NewCategory;
            using (var checkingCatExist = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedDB), true))
            {
                var query = checkingCatExist.Table<CatShort>().Where(v => v.ServerID == categoryId);
                if (query.FirstOrDefault() != null)
                    { NewCategory = query.ToArray<CatShort>(); }
                else
                {
                    NewCategory = new CatShort[1];
                    string qu = res.GetString(Resource.String.Unknown_category);
                    NewCategory[0] = new CatShort();
                    NewCategory[0].Name = res.GetString(Resource.String.Unknown_category);
;                }
                //List<EventsShort> queryList = query.ToList<EventsShort>();
            }

            return NewCategory[0].Name;
        }

        //���� ����� ��� ��������� ������ ���� �� �������, ���� ��� ��������� ID
        private EventShort[] GetEventShortInternal(Int64 ourEventServerID)
        {
            EventShort[] NewEvents = new EventShort[] {};
            EventShort[] resultingEvent = new EventShort[1] { new EventShort() };

            if (!CheckServerDataPresent(true))           	//���� ������ �� ������� ���� - ������ �� �����
            {
                return resultingEvent;

                //TO DO
                //using (var client = new UserClientExchange())
                //{
                //    try
                //    {
                //        for (int howManyTimesTryDownload = 0; howManyTimesTryDownload < 3; howManyTimesTryDownload++)
                //        {
                //            var syncSessionId = client.Login("Emeri", "Emeri", new Captcha());
                //            NewEvents = client.GetEventsList(syncSessionId, 0, true, 100, true);      //������ �������
                //            SavingEvents.SavingShortEvents("events.sqlite", NewEvents);                                   //�������� ��� ���������� � �� ������ ��������� �������� �������� �������
                //            client.Logout(syncSessionId);

                //            howManyTimesTryDownload = 3;
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        var k = e.Message;
                //        //� ������� �������� ��� ���������!
                //    }

                //    resultingEvent = NewEvents.Where(v => v.Id == ourEventServerID).ToArray();
                //    return resultingEvent;
                //}
            }
            else                                              //���� ������ �� ������� ��� - ������ 
            {
                string usedEventsDB = "events.sqlite";
                using (var eventFromDB = new SQLite.SQLiteConnection(Path.Combine(WorkingInetAndSQL.destinationPath, usedEventsDB)))
                {
                    var query = eventFromDB.Table<EventsShort>().Where(v => v.EventServerID == ourEventServerID);
                    if (query.FirstOrDefault() != null)             //���� ���� � ��
                    {
                        EventShort requestedEventShort = new EventShort();
                        requestedEventShort.Id = query.FirstOrDefault().EventServerID;
                        requestedEventShort.Name = query.FirstOrDefault().EventName;
                        requestedEventShort.Date = query.FirstOrDefault().EventDateTime;
                        requestedEventShort.OrganizerName = query.FirstOrDefault().OrganizerName;
                        requestedEventShort.PrimaryPhotoId = query.FirstOrDefault().PhotoID;

                        resultingEvent[0] = requestedEventShort;
                        return resultingEvent;
                    }
                    else
                    {
                        return resultingEvent;
                    }
                }
            }
        }

        //�������� ���������� ������
        public EventShort[] GetEventsData(int whatevents)
	    {
	        WorkingInetInteractDB a = GetObject();
            //�������� �������

            return a.GetEventsDataInternal(whatevents);
	    }
        public List<Android.Graphics.Bitmap> GetEventsPhotos(EventShort[] NewEvents)
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetEventsPhotosInternal(NewEvents);
        }
        public EventFullGet GetEventFullData(Int64 eventId)             //, out DTO.EventFullGet eventDescription
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetEventFullDataInternal(eventId);
        }
        public EventShort[] GetEventShort(Int64 ourEventServerID)             //, out DTO.EventFullGet eventDescription
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetEventShortInternal(ourEventServerID);
        }
        
        //���������
        public CategoryShort[] GetCategoryData(Int64 categoryParentId)             //
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetCategoryDataInternal(categoryParentId);
        }
        public CategoryShort[] GetVersionByCategory(Int64 rootVersionCategoryID, Int64 versionCategoryID)             //
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetVersionByCategoryInternal(rootVersionCategoryID, versionCategoryID);
        }
        public string GetCategoryNameFromDB(Int64 categoryId)             //
        {
            WorkingInetInteractDB a = GetObject();
            return a.GetCategoryNameFromDBInternal(categoryId);
        }
        
    }
}