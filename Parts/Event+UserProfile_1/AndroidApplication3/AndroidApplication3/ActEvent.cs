using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidApplication3
{
    [Activity(Label = "AndroidApplication3", MainLauncher = true, Icon = "@drawable/icon")]
    public class ActEvent : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the Event layout resource
            SetContentView(Resource.Layout.Event);

            // Get our button from the layout resource,
            // and attach an event to it
            Button UserProfileButton = FindViewById<Button>(Resource.Id.UserProfileButton);

            UserProfileButton.Click += (sender, e) =>
            {
                var IntentUserProfile = new Intent(this, typeof(ActUserProfile));
                IntentUserProfile.PutExtra("FirstData", "Data from FirstActivity");
                StartActivity(IntentUserProfile);
            };
        }
    }
}

