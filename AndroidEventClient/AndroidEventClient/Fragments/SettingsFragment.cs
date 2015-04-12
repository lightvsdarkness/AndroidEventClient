using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace AEC.Fragments
{
    class SettingsFragment : Fragment   {
        int firstTimeDoesntCount = 0;
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            //var colorDrawable = new ColorDrawable(Color.White);
            //activity.ActionBar.SetBackgroundDrawable(colorDrawable);
            var titleId = activity.Resources.GetIdentifier("ActionBar", "id", "aec");
            activity.ActionBar.Title = GetString(Resource.String.Settings);
            activity.ActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.ActionBar.SetHomeButtonEnabled(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Settings, container, false);
            SetHasOptionsMenu(true);

            var refreshSettingsButton = view.FindViewById<Button>(Resource.Id.RefreshSettingsButton);
            refreshSettingsButton.Click += delegate
            {
                //refreshSettingsButton.Text = string.Format("Настройки обновлены!\nОбновить еще раз?");
                Toast.MakeText(this.Activity, Resource.String.SettingsSaved, ToastLength.Long);
            };

            RadioButton only_my_cityRadio = view.FindViewById<RadioButton>(Resource.Id.Only_my_city);
            RadioButton other_cities_regionRadio = view.FindViewById<RadioButton>(Resource.Id.Other_cities_region);
            only_my_cityRadio.Click += RadioButtonClick;
            other_cities_regionRadio.Click += RadioButtonClick;


            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.City_spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.Cities_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            //LoginLogoutButton
            var loginLogoutButton = view.FindViewById<TextView>(Resource.Id.LoginLogoutButton);
            loginLogoutButton.Click += delegate
            {
                //Login или Logout
                if (((DrawerAct)this.Activity).authentication)
                {
                    ((DrawerAct)this.Activity).authentication = false;
                    loginLogoutButton.Text = string.Format("LogIn");
                }
                else
                {
                    ((DrawerAct)this.Activity).authentication = true;
                    var intent = new Intent(Activity, typeof(AccLogInAct));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                }
                ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
            };

            return view;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toast.MakeText(Activity, rb.Text, ToastLength.Short).Show();
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (firstTimeDoesntCount == 0)
                { firstTimeDoesntCount++;}
            else { 
                //GetString(Resource.String.City_chosen)
                string spinnerUsed = (GetString(Resource.String.City_chosen) + " {0}");
                string toast = string.Format(spinnerUsed, spinner.GetItemAtPosition(e.Position)); Toast.MakeText(Activity, toast, ToastLength.Short).Show();
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }
    }
}