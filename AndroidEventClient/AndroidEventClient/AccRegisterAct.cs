using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;

namespace AEC
{
    [Activity(Label = "Регистрация", MainLauncher = false, Icon = "@drawable/Icon", Theme = "@style/Theme.PATheme")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "aec.AccLogInAct")]
	public class AccRegisterAct : Activity
    {
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:      //Id = 16908332
                    {
                        NavUtils.NavigateUpFromSameTask(this);
                        return true;
                    }

            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //ActionBar.Title = GetString(Resource.String.login);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.AccRegister);

            var FIOText = FindViewById<EditText>(Resource.Id.FIOTextR);
            var emailText = FindViewById<EditText>(Resource.Id.EmailTextR);
            var passwordText = FindViewById<EditText>(Resource.Id.PasswordTextR);
            var groupText = FindViewById<EditText>(Resource.Id.GroupTextR);
            var unitText = FindViewById<EditText>(Resource.Id.UnitTextR);

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
            //emailText.Text = string.Format("Emeri");
            //passwordText.Text = string.Format("Emeri");

            Spinner spinner = FindViewById<Spinner>(Resource.Id.Gender_spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Gender_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

			Button RegisterAccountButton = FindViewById<Button>(Resource.Id.RegisterAccount);
 
			    RegisterAccountButton.Click += delegate
                {
                    if (emailText.Text.Length > 4 || passwordText.Text.Length > 5)      //FIOText.Text.Length > 5 || 
                    {
                        RegisterAccountButton.Text = GetString(Resource.String.SuccessfulRegistration);             //string.Format("Вы зарегистрировались. Добро пожаловать!");

                        String textGender = spinner.SelectedItem.ToString();  //getSelectedItem().toString();
                        //В базу и на сервер сразу:
                        //TO DO
                        //WorkingWithAccount.RegisterAccount(FIOText.Text, emailText.Text, emailText.Text, passwordText.Text, textGender, groupText.Text, unitText.Text);
                    }
                    else
                    {
                        //if (FIOText.Text.Length <= 5) { Toast.MakeText(this, "Длина ФИО меньше 5 знаков", ToastLength.Long).Show(); }
                        if (emailText.Text.Length <= 4) { Toast.MakeText(this, "Пожалуйста, предоставьте другой электронный адрес", ToastLength.Long).Show(); }
                        else { Toast.MakeText(this, "Не могу принять пароль длиной меньше 6 знаков", ToastLength.Long).Show(); }
                    }
                };

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            //string spinnerUsed = (GetString(Resource.String.Gender_chosen) + " {0}");
            //string toast = string.Format(spinnerUsed, filterSpinner.GetItemAtPosition(e.Position)); Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
    }
}

