using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V7.App;
//using Android.Support.V7.Widget;
using flyoutmenu;
using AEC.Fragments;


namespace AEC    {
    [Activity(Label = "MainMenu", Icon = "@drawable/Icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@style/Theme.PATheme")]
    //, ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize
	public class MainMenuAct : ActionBarActivity    {
        private Fragment _lastFragment;
        private FlyOutContainer _flyOut;
        public string FragmentLaunched = "zero";
        //private Android.Support.V7.Widget.SearchView _searchView;        //dfdfds
        
        protected override void OnCreate (Bundle bundle)    {
			base.OnCreate (bundle);
            string text = Intent.GetStringExtra("Account") ?? "Emeri";
            string workMode = Intent.GetStringExtra("Password") ?? "Data not available";
            //TO DO
            //if (workMode == "q") { WorkingInetAndSQL.DeleteRowsInDBIfNeed("events.sqlite"); }; 

            FragmentLaunched = "main";
            try
            {
                SetContentView(Resource.Layout.MainOld);
            }
            catch (Exception e) { var a = e; throw; }

            SupportActionBar.SetDisplayShowHomeEnabled(true);

            if (FragmentLaunched == "main")
            {
                try
                {
                    FragmentManager.BeginTransaction()
                       .Remove(_lastFragment);
                    _lastFragment.Dispose();
                }
                catch (Exception e) { var k = e.Message; }
            }

            //Первоначальная загрузка
            _flyOut = FindViewById<FlyOutContainer>(Resource.Id.FlyOutContainer);

            //ГЛАВНОЕ МЕНЮ контейнера
            var menuButton = FindViewById (Resource.Id.MenuButton);
			menuButton.Click += (sender, e) =>
            {
                _flyOut.AnimatedOpened = !_flyOut.AnimatedOpened;
			};

            // mess with fragments begin //
            var eventsListFragment = new EventsListFragment();
            _lastFragment = eventsListFragment;

            var arguments = new Bundle();
            arguments.PutString(EventsListFragment.Account, "Emeri");
            arguments.PutString(EventsListFragment.Password, "Emeri");
            eventsListFragment.Arguments = arguments;

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.content_frame, eventsListFragment)
                .Commit();

            //Избавляемся от фрагмента, если открываем профиль аккаунта - ProfileFragments
            var profileTextView = FindViewById<TextView>(Resource.Id.textView6);
		    profileTextView.Click += (sender, args) =>
		    {
                var profileFragment = new ProfileFragment();

                FragmentManager.BeginTransaction()
                    .Remove(_lastFragment)
                    .Add(Resource.Id.content_frame, profileFragment)
                    .Commit();

                _lastFragment.Dispose();
		        _lastFragment = profileFragment;

                _flyOut.ExternalyClosed();
		    };

            //Избавляемся от фрагмента, если открываем события - EventListFragments
            var eventsListTextView = FindViewById<TextView>(Resource.Id.textView1);
            eventsListTextView.Click += (sender, args) =>
            {
                var eventsListFragment2 = new EventsListFragment();

                FragmentManager.BeginTransaction()
                    .Remove(_lastFragment)
                    .Add(Resource.Id.content_frame, eventsListFragment2)
                    .Commit();

                _lastFragment.Dispose();
                _lastFragment = eventsListFragment2;

                _flyOut.ExternalyClosed();
            };

		}

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
            }
        }


        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.ActionBarMain, menu);
        //    //base.OnCreateOptionsMenu(menu, menuInflater);

        //    var item = menu.FindItem(Resource.Id.action_search);
        //    MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(AEC.Fragments.standardEventsAdapter));
        //    var searchItem = MenuItemCompat.GetActionView(item);
        //    _searchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();                              //Android.Support.V7.Widget.
        //    _searchView.QueryTextChange += (s, e) => standardEventsAdapter.Filter.InvokeFilter(e.NewText);

        //    _searchView.QueryTextSubmit += (s, e) =>
        //    {
        //        //TODO: Do something fancy when search button on keyboard is pressed
        //        Toast.MakeText(Activity, "Searched for: " + e.Query, ToastLength.Short).Show();
        //        e.Handled = true;
        //    };
        //    return true;

        //}

	}
}


