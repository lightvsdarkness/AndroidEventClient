using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidApplication3
{
    [Activity(Label = "AndroidApplication3", MainLauncher = false, Icon = "@drawable/icon")]
    public class ActUserProfile : Activity
    {
       // int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.UserProfile);

            // Get our button from the layout resource,
            // and attach an event to it



            Button RefreshProfileButton = FindViewById<Button>(Resource.Id.RefreshProfileButton);

            RefreshProfileButton.Click += delegate { RefreshProfileButton.Text = string.Format("Ваши пользовательские данные обновлены!\nОбновить еще раз?"); };
        }
    }
}

