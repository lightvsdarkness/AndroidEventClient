using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace AEC.Fragments
{
    class ProfileFragment : Fragment    {
        private bool firstTimeDoesntCount = true;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Profile, container, false);
            SetHasOptionsMenu(true);

            // Get our button from the layout resource,
            // and attach an event to it
            var refreshProfileButton = view.FindViewById<Button>(Resource.Id.RefreshProfileButton);

            refreshProfileButton.Click += delegate {
                //refreshProfileButton.Text = string.Format("Ваши пользовательские данные обновлены!\nОбновить еще раз?");
                string toast = string.Format((GetString(Resource.String.ProfileSaved) + " {0}")); Toast.MakeText(Activity, toast, ToastLength.Long).Show();
            };

            var emailText = view.FindViewById<EditText>(Resource.Id.EmailText);
            var passwordText = view.FindViewById<EditText>(Resource.Id.PasswordText);
            var fIOText = view.FindViewById<EditText>(Resource.Id.FIOText);
            var groupText = view.FindViewById<EditText>(Resource.Id.GroupText);
            var unitText = view.FindViewById<EditText>(Resource.Id.UnitText);

            groupText.Text = GetString(Resource.String.SFEDU_students);
            groupText.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    groupText.Text = GetString(Resource.String.SFEDU_students);
                    e.Handled = true;
                }
            };

            //Достать из базы
            fIOText.Text = string.Format("Emeri");
            emailText.Text = string.Format("Emeri");
            passwordText.Text = string.Format("******");

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.Gender_spinner_profile);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.Gender_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            return view;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (firstTimeDoesntCount)
            { firstTimeDoesntCount = false; }
            else
            {
                //Обновить пол в базе

            }
        }

        public override void OnResume()
        {
            base.OnResume();
            firstTimeDoesntCount = true;
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }

    }

 
}