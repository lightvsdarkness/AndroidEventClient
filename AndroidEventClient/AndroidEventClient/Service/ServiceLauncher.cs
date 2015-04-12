using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AEC.Service
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    class ServiceLauncher : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //���� ������� ���������� ��������
            if (intent.Action == Intent.ActionBootCompleted)
            {
                //��������� ������
                context.ApplicationContext.StartService(new Intent(context, typeof(DataService)));
            }
        }
    }
}