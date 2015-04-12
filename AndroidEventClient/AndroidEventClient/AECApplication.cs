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
using AEC.Service;

namespace AEC
{
    /// <summary>
    /// Класс приложения
    /// </summary>
    [Application]
    public class AECApplication : Application
    {
        //Объект соединения с сервисом
        public ServiceConnection ServiceConnection { get; set; }
        /// <summary>
        /// Объект сервиса
        /// </summary>
        public DataService Service { get; set; }

        /// <summary>
        /// Base constructor which must be implemented if it is to successfully inherit from the Application
        /// class.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transfer"></param>
        public AECApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            // do any initialisation you want here (for example initialising properties)
        }

        public override void OnCreate()
        {
            base.OnCreate();
            //app init ...
        }
    }
}