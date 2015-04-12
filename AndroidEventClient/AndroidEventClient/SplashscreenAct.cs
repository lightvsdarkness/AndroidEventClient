using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using AEC.Fragments;
using AEC.Service;

namespace AEC
{
    [Activity(Label = "Куда сходить", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.PAThemeSplash", NoHistory = true)]
    public class SplashscreenAct : Activity
    {
        //bool isBound = false;
        //DataServiceBinder binder;
        //Intent demoServiceIntent;

        //Объект обработчика извещения о старте сервиса
        ServiceStartedReceiver _serviceStartedReceiver;
        //Объект обработчика извещения о привязке к сервису
        ServiceBoundReceiver _serviceBoundReceiver;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Splashscreen);
        }

        protected override void OnStart()
        {
            base.OnStart();

            //Создаём объект обработчика извещения о старте сервиса
            _serviceStartedReceiver = new ServiceStartedReceiver(this);
            //Создаём фильтр сообщений для обработчика
            IntentFilter intentFilter = new IntentFilter(DataService.AECServiceStartedIntent) { Priority = (int)IntentFilterPriority.HighPriority };
            //Регистрируем обработчик
            RegisterReceiver(_serviceStartedReceiver, intentFilter);

            //Создаём объект обработчика извещения о привязке к сервису
            _serviceBoundReceiver = new ServiceBoundReceiver(this);
            //Создаём фильтр сообщений для обработчика
            intentFilter = new IntentFilter(DataService.AECServiceBoundIntent) { Priority = (int)IntentFilterPriority.HighPriority };
            //Регистрируем обработчик
            RegisterReceiver(_serviceBoundReceiver, intentFilter);

            //Запускаем сервис
            if (StartService(new Intent(this, typeof(DataService))) == null)
            {
                //Если сервис не удалось запустить
                //Выводим информационное сообщение
                Toast.MakeText(this, "Не удалось запустить сервис", ToastLength.Long).Show();
                //Завершаем активити
                Finish();
                //Выходим
                return;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }
    }

    /// <summary>
    /// Класс обработчика извещения о старте сервиса
    /// </summary>
    class ServiceStartedReceiver : BroadcastReceiver
    {
        Context activityContext;
        public ServiceStartedReceiver(Context activityContext)
        {
            this.activityContext = activityContext;
        }
        public override void OnReceive(Context context, Android.Content.Intent intent)
        {
            //Удаляем извещение
            InvokeAbortBroadcast();

            //Формируем ссылку на объект приложения
            AECApplication application = Application.Context as AECApplication;

            //Создаём объект соединения с сервисом
            ServiceConnection serviceConnection = new ServiceConnection(application);

            //Создаём сообщение
            Intent bindServiceIntent = new Intent(DataService.AECServiceIntent);
            //Подключаемся к сервису
            application.BindService(bindServiceIntent, serviceConnection, Android.Content.Bind.AutoCreate);
        }
    }

    /// <summary>
    /// Класс обработчика извещения о привязке к сервису
    /// </summary>
    class ServiceBoundReceiver : BroadcastReceiver
    {
        Context activityContext;
        public ServiceBoundReceiver(Context activityContext)
        {
            this.activityContext = activityContext;
        }
        public override void OnReceive(Context context, Android.Content.Intent intent)
        {
            //Удаляем извещение
            InvokeAbortBroadcast();

            //Запускаем основную Activity
            context.StartActivity(typeof(DrawerAct));
        }
    }
}

