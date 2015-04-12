using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using AEC.Service;

namespace AEC
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { WorkingInetInteractDB.aecCalendarIntent }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class EventNotificationReceiver : BroadcastReceiver
    {
        public EventNotificationReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var nMgr = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var notification = new Notification(Resource.Drawable.service_itstime, "Сегодня у Вас в планах");
            var pendingIntent = PendingIntent.GetActivity(context, 0, new Intent(context, typeof(DrawerAct)), 0);
            notification.SetLatestEventInfo(context, "Сегодня у Вас в планах", "nameOfEvent", pendingIntent);
            nMgr.Notify(0, notification);
        }
    }
}

