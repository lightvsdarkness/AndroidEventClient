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
using Android.Support.V7.App;

namespace AEC.Service
{
    /// <summary>
    /// Класс соединения сервиса и Activity
    /// </summary>
    public class ServiceConnection : Java.Lang.Object, IServiceConnection
    {
        //Объект нашего приложения
        AECApplication _application = null;
        //Объект привязки
        DataServiceBinder _serviceBinder = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="activity"></param>
        public ServiceConnection(AECApplication application)
        {
            //Сохраняем приложение
            _application = application;
        }

        /// <summary>
        /// Обработчик события соединения задачи и сервиса
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            //Формируем ссылку на объект привязки
            _serviceBinder = service as DataServiceBinder;
            //Если ссылка существует
            if (_serviceBinder != null)
            {
                //Сохраняем ссылку на сервис в объекте приложения
                _application.Service = _serviceBinder.Service;

                //Создаём сообщение
                Intent serviceBoundIntent = new Intent(DataService.AECServiceBoundIntent);
                //Отсылаем приложению извещение о готовности сервиса
                _application.SendOrderedBroadcast(serviceBoundIntent, null);
            }
        }

        /// <summary>
        /// Обработчик события отключения задачи от сервиса
        /// </summary>
        /// <param name="name"></param>
        public void OnServiceDisconnected(ComponentName name)
        {
            //Очищаем ссылку на сервис в объекте приложения
            _application.Service = null;
        }
    }
}