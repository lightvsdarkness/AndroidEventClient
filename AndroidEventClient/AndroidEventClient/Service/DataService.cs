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
    /// Класс сервиса работы с сервером (основная часть)
    /// </summary>
    [Service]
    [IntentFilter(new String[] { DataService.AECServiceIntent })]
    public partial class DataService : Android.App.Service
    {
        //Идентифкаторы сообщений для синхронизации запуска сервиса
        public const string AECServiceIntent = "AECService";
        public const string AECServiceStartedIntent = "AECServiceStarted";
        public const string AECServiceBoundIntent = "AECServiceBound";

        //Объект связи приложений с сервисом
        DataServiceBinder _binder = null;

        //Кэш иконок
        PhotoCache _iconsCache = null;
        //Кэш полных описаний событий
        EventFullCache _eventsFullCache = null;

        bool downloading = false;
        WorkingInetInteractDB check1 = new WorkingInetInteractDB();
        List<EventShort> cacheEvents;
        List<Bitmap> cachePhotos;

        /// <summary>
        /// Обработчик создания сервиса
        /// </summary>
        public override void OnCreate()
        {
            //Инициализируем базу данных
            CreateDB();

            //Инициализируем класс обмена с сервером
            UserAccess.Init();
            //Подключаем обработчик ошибок
            UserAccess.OnError += UserAccess_OnError;

            //Инициализируем кэш иконок
            _iconsCache = new PhotoCache(this, 1000, 10000000, 20000);
            //Инициализируем кэш полных описаний событий
            _eventsFullCache = new EventFullCache(this, 200, 1000000, 5000);

            //Выводим информационное сообщение
            Toast.MakeText(this, "Сервис AEC запущен", ToastLength.Long).Show();
        }

        /// <summary>
        /// Обработчик запуска сервиса
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            //Создаём сообщение
            Intent serviceStartedIntent = new Intent(DataService.AECServiceStartedIntent);
            //Отсылаем приложению извещение о готовности сервиса
            SendOrderedBroadcast(serviceStartedIntent, null);

            //Возвращаем признак перезапуска сервиса в случае его остановки
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Обработчик события привязки приложения к сервису
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Android.Content.Intent intent)
        {
            //Если объект привязки ещё не создан
            if (_binder == null)
            { 
                //Создаём объект привязки
                _binder = new DataServiceBinder(this);
            }

            //Начинаем сессию с сервером (пока тут для отладки)
            Login("Emeri", "Emeri", new Captcha());

            //Возвращаем првязку
            return _binder;
        }

        /// <summary>
        /// Обработчик отключения всех привязок
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override bool OnUnbind(Intent intent)
        {
            //Зaвершаем сессию с сервером (пока тут для отладки)
            Logout();

            //Возвращаем признак несохранения интента
            return false;
        }

        /// <summary>
        /// Обработчик завершения сервиса
        /// </summary>
        public override void OnDestroy()
        {
            //Освобождаем ресурсы обмена с сервером
            UserAccess.Destroy();

            //Вызываем метод базового класса
            base.OnDestroy();
        }

        //Обработчик ошибок при обмене с сервером
        void UserAccess_OnError(string errorDescription)
        {
            //Создаём объект вызова кода в основном потоке
            var myHandler = new Handler();
            //Вызываем код в основном потоке
            myHandler.Post(() =>
            {
                //Выводим информационное сообщение
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
                var notification = noti.SetTicker("Сегодня у Вас в планах:")
                    .SetContentTitle("Сегодня у Вас в планах")
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
    /// Объект привязки сервиса к приложению
    /// </summary>
    public class DataServiceBinder : Binder
    {
        /// <summary>
        /// Объект сервиса
        /// </summary>
        public DataService Service { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service"></param>
        public DataServiceBinder(DataService service)
        {
            //Сохраняем ссылку на сервис
            Service = service;
        }
    }
}