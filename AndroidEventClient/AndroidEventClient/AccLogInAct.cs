using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Support.V7;
using Android.Support.V7.App;
using Android.Support.V4;
using Android.Support.V4.App;
using Android.Support.V4.View;
//using Android.Support.V7.Widget;
using SQLite;
using System.IO;



namespace AEC 
{

    [Activity(Label = "Авторизация", MainLauncher = false, Icon = "@drawable/Icon", Theme = "@style/Theme.PATheme")]      //Theme = "@android:style/Theme.NoTitleBar"
    [MetaData("android.support.PARENT_ACTIVITY", Value = "aec.DrawerAct")]
    public class AccLogInAct : Activity
    {
        //public Android.Support.V7.Widget.SearchView _searchView;
        //private Android.Support.V4.Widget.DrawerLayout _drawer;
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
            //this.requestWindowFeature(Window.FEATURE_NO_TITLE);
            ActionBar.Title = GetString(Resource.String.login);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.AccLogIn);

            //TO DO
            //WorkingInetAndSQL.CreateDBIfNeed("profile.sqlite");
            //WorkingInetAndSQL.CreateDBIfNeed("events.sqlite");

            var emailEditText = FindViewById<EditText>(Resource.Id.EmailEditText);
            //TO DO
            //emailEditText.Text = WorkingWithAccount.GetTestAccount();
            emailEditText.Text = "TO DO";
            var passwordEditText = FindViewById<EditText>(Resource.Id.PasswordEditText);
            //passwordEditText.Text = Convert.ToString(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

            var buttonCantAccessAccount = FindViewById<Button>(Resource.Id.ForgotPassword);
            buttonCantAccessAccount.Click += delegate { StartActivity(typeof(AccCantAccessAct)); };

            var buttonReg = FindViewById<Button>(Resource.Id.RegisterButton);
			buttonReg.Click += delegate { StartActivity(typeof(AccRegisterAct)); };

            var buttonSign = FindViewById<Button>(Resource.Id.SignInButton);
            buttonSign.Click += delegate {
                ISharedPreferences prefAcc = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                //ISharedPreferences prefs = Context.GetSharedPreferences;
                ISharedPreferencesEditor editor = prefAcc.Edit();
                editor.PutString("Account", "Emeri");
                editor.Apply();

                Toast.MakeText(this, "Вы уже авторизованы", ToastLength.Short).Show();
                //var intentMainMenu = new Intent(this, typeof(MainMenuActivity));
                //intentMainMenu.PutExtra("Account", "Emeri");
                //intentMainMenu.PutExtra("Password", passwordEditText.Text);
                //StartActivity(intentMainMenu);
            };

        }

        //static string defaultDBPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //static string destinationPath = Path.Combine(defaultDBPath, "db.sqlite");
        //public void CreateDBIfNeed() {
        //    //Check if DB has already been extracted
                //ASYNC CREATION OF TABLE
                //var conn = new SQLiteAsyncConnection(destinationPath);
                //conn.CreateTableAsync<Account>().ContinueWith(t =>
                //{
                //    Account testAccount = new Account { Email = "Emeri " };//, Password = "Emeri" 
                //    conn.InsertAsync(testAccount).ContinueWith(t2 =>
                //    {
                //        //string.Console.WriteLine("Test Account: {0}", testAccount.ID); 
                //    });                  
                //});
		//}



    }


}

