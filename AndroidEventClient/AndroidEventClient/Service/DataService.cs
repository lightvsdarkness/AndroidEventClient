using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace AEC.Service
{
    /// <summary>
    /// ����� ������� ������ � �������� (�������� �����)
    /// </summary>
    [Service]
    [IntentFilter(new String[] { DataService.AECServiceIntent })]
    public partial class DataService : Android.App.Service
    {
        //������������� ��������� ��� ������������� ������� �������
        public const string AECServiceIntent = "AECService";
        public const string AECServiceStartedIntent = "AECServiceStarted";
        public const string AECServiceBoundIntent = "AECServiceBound";

        //������ ����� ���������� � ��������
        DataServiceBinder _binder = null;

        //��� ������
        PhotoCache _iconsCache = null;
        //��� ������ �������� �������
        EventFullCache _eventsFullCache = null;

        bool downloading = false;
        WorkingInetInteractDB check1 = new WorkingInetInteractDB();
        List<EventShort> cacheEvents;
        List<Bitmap> cachePhotos;

        /// <summary>
        /// ���������� �������� �������
        /// </summary>
        public override void OnCreate()
        {
            //�������������� ���� ������
            CreateDB();

            //�������������� ����� ������ � ��������
            UserAccess.Init();
            //���������� ���������� ������
            UserAccess.OnError += UserAccess_OnError;

            //�������������� ��� ������
            _iconsCache = new PhotoCache(this, 1000, 10000000, 20000);
            //�������������� ��� ������ �������� �������
            _eventsFullCache = new EventFullCache(this, 200, 1000000, 5000);

            //������� �������������� ���������
            Toast.MakeText(this, "������ AEC �������", ToastLength.Long).Show();
        }

        /// <summary>
        /// ���������� ������� �������
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            //������ ���������
            Intent serviceStartedIntent = new Intent(DataService.AECServiceStartedIntent);
            //�������� ���������� ��������� � ���������� �������
            SendOrderedBroadcast(serviceStartedIntent, null);

            //���������� ������� ����������� ������� � ������ ��� ���������
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// ���������� ������� �������� ���������� � �������
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Android.Content.Intent intent)
        {
            //���� ������ �������� ��� �� ������
            if (_binder == null)
            { 
                //������ ������ ��������
                _binder = new DataServiceBinder(this);
            }

            //�������� ������ � �������� (���� ��� ��� �������)
            Login("Emeri", "Emeri", new Captcha());

            //���������� �������
            return _binder;
        }

        /// <summary>
        /// ���������� ���������� ���� ��������
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override bool OnUnbind(Intent intent)
        {
            //�a������� ������ � �������� (���� ��� ��� �������)
            Logout();

            //���������� ������� ������������ �������
            return false;
        }

        /// <summary>
        /// ���������� ���������� �������
        /// </summary>
        public override void OnDestroy()
        {
            //����������� ������� ������ � ��������
            UserAccess.Destroy();

            //�������� ����� �������� ������
            base.OnDestroy();
        }

        //���������� ������ ��� ������ � ��������
        void UserAccess_OnError(string errorDescription)
        {
            //������ ������ ������ ���� � �������� ������
            var myHandler = new Handler();
            //�������� ��� � �������� ������
            myHandler.Post(() =>
            {
                //������� �������������� ���������
                Toast.MakeText(this, errorDescription, ToastLength.Long).Show();
            });
        }

        public void DownloadWork()
        {
            Thread secondThread = new Thread(() =>
            { //Log.Debug("DemoService", "Doing work");   Thread.Sleep(5000); Log.Debug("DemoService", "Work complete");  StopSelf();
                downloading = true;

                //BROADCAST
                var stocksIntent = new Intent(WorkingInetInteractDB.aecActivityIntent);

                SendOrderedBroadcast(stocksIntent, null);

                Thread thirdThread = new Thread(() =>
                {
                    Thread.Sleep(50);
                    downloading = false;
                }
                );
                thirdThread.Start();
            }
            );
            secondThread.Start();
        }

        public List<EventShort> GetCacheEvents()
        {
            return cacheEvents;
        }
        public List<Bitmap> GetCachePhotos()
        {
            return cachePhotos;
        }
        public CategoryShort[] GetCategoryDataService(Int64 whatVersionCatID)
        {
            CategoryShort[] NewCategories = check1.GetCategoryData(whatVersionCatID);
            return NewCategories;
        }



        public void CreateDB()
        {
            //var myHandler = new Handler();
            //myHandler.Post(() =>
            //{
                WorkingInetAndSQL.CreateDBIfNeed("profile.sqlite");
                WorkingInetAndSQL.CreateDBIfNeed("events.sqlite");
                WorkingInetAndSQL.CreateDBIfNeed("chosenstuff.sqlite");
        //    });
        }
        public void DownloadWorkOnUIThread()
        {
            //RunOnUiThread(() =>
            //{ }
            //);
            var myHandler = new Handler ();
            myHandler.Post(() => {
                //Toast.MakeText (this, "Message from demo service", ToastLength.Long).Show();
                
                //var newEventsData = check1.GetEventsData(200).ToList<EventShort>();
                //List<Bitmap> photos = check1.GetEventsPhotos(newEventsData.ToArray());

                cacheEvents = check1.GetEventsData(200).ToList<EventShort>();
                cachePhotos = check1.GetEventsPhotos(cacheEvents.ToArray());
            });
        }

        public void NotificateToday(string nameOfEvent)
        {

            Thread secondThread = new Thread(() =>
            {
                var nMgr = (NotificationManager)GetSystemService(NotificationService);
                var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(DrawerAct)), 0);
                Notification.Builder noti = new Notification.Builder(this);
                var notification = noti.SetTicker("������� � ��� � ������:")
                    .SetContentTitle("������� � ��� � ������")
                    .SetContentText(nameOfEvent)
                    .SetSmallIcon(Resource.Drawable.service_itstime)
                    .Build();

                nMgr.Notify(0, notification);

            }
            );
            secondThread.Start();
        }
    }

    /// <summary>
    /// ������ �������� ������� � ����������
    /// </summary>
    public class DataServiceBinder : Binder
    {
        /// <summary>
        /// ������ �������
        /// </summary>
        public DataService Service { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="service"></param>
        public DataServiceBinder(DataService service)
        {
            //��������� ������ �� ������
            Service = service;
        }
    }
}