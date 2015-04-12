//using System;
//using System.Threading;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Util;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;


//namespace AEC
//{
//    [Service]
//    [IntentFilter(new String[] { "aecs" })]
//    public class ServiceScheduling : IntentService
//    {
//        IBinder binder;
//        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
//        {
//            string nameOfEvent = "Название мероприятия";
//            NotificateToday(nameOfEvent);

//            return StartCommandResult.Sticky;
//        }
//        protected override void OnHandleIntent(Intent intent)
//        {
//            //var stockSymbols = new List<string>() { "AMZN", "FB", "GOOG", "AAPL", "MSFT", "IBM" };
//            //stocks = UpdateStocks(stockSymbols);
//            //var stocksIntent = new Intent(StocksUpdatedAction);
//            //SendOrderedBroadcast(stocksIntent, null);
//        }

//        public override IBinder OnBind(Intent intent)
//        {
//            binder = new ServiceSchedulingBinder(this);
//            return binder;
//        }
//        public void NotificateToday(string nameOfEvent)
//        {


//            Thread secondThread = new Thread(() =>
//            {
//                var nMgr = (NotificationManager)GetSystemService(NotificationService);
//                var notification = new Notification(Resource.Drawable.service_itstime, "Сегодня у Вас в планах:");
//                var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(DrawerAct)), 0);
//                notification.SetLatestEventInfo(this, "Сегодня у Вас в планах", nameOfEvent, pendingIntent);
//                nMgr.Notify(0, notification);

//            }
//            );
//            secondThread.Start();
//        }
//        public override void OnDestroy()
//        {
//            base.OnDestroy();

//        }
//    }

//    public class ServiceSchedulingBinder : Binder
//    {
//        ServiceScheduling service;

//        public ServiceSchedulingBinder(ServiceScheduling service)
//        {
//            this.service = service;
//        }

//        public ServiceScheduling GetStockService()
//        {
//            return service;
//        }
//    }
//}