using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using AEC.Fragments;
using System.Collections.Generic;
using AEC.Service;

namespace AEC
{
    [Activity(Label = "Куда сходить", MainLauncher = false, Icon = "@drawable/Icon", Theme = "@style/Theme.PATheme",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class DrawerAct : ActionBarActivity       //Activity
    {
        DownloadedReceiver downloadedReceiver;
        Intent wIIDBServiceIntent;

        Fragment[] fragments = new Fragment[5];              //List<Fragment>[] fragments = new List<Fragment>[5];
        //private Fragment _lastFragment;
        private DrawerLayout _drawer;
        private MyActionBarDrawerToggle _drawerToggle;
        private ListView _drawerList;
        public IMenu drawerActivityMenu { get; set; }
        //FOR THE ACTIONBAR
        private int actionBarState;
        public bool authentication { get; set; }
        private bool drawerOpen;
        
        private string _drawerTitle;
        private string _title;
        private string[] _menuTitles;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            wIIDBServiceIntent = new Intent(WorkingInetInteractDB.aecServiceIntent);
            downloadedReceiver = new DownloadedReceiver();

            authentication = true;
            
            SetContentView(Resource.Layout.MainDrawer);
            _title = _drawerTitle = Title;
            _menuTitles = Resources.GetStringArray(Resource.Array.MenuArray);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _drawerList = FindViewById<ListView>(Resource.Id.left_drawer);

            _drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
            
            _drawerList.Adapter = new ArrayAdapter<string>(this, Resource.Layout.DrawerListItem, _menuTitles);
            _drawerList.ItemClick += (sender, args) => SelectItem(args.Position);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            //DrawerToggle is the animation that happens with the indicator next to the ActionBar icon. You can choose not to use this.
            _drawerToggle = new MyActionBarDrawerToggle(this, _drawer, Resource.Drawable.ic_drawer_light, Resource.String.DrawerOpen, Resource.String.DrawerClose);


            _drawerToggle.DrawerClosed += delegate      //You can alternatively use _drawer.DrawerClosed here
            {
                ActionBar.Title = _title;
                string visibleFragmentTag = FragmentManager.FindFragmentById(Resource.Id.content_frame).Tag;
                if (visibleFragmentTag == "settingsfragment")
                { ActionBar.Title = GetString(Resource.String.Preferences); }
                drawerOpen = false;
                InvalidateOptionsMenu();
            };

            _drawerToggle.DrawerOpened += delegate      //You can alternatively use _drawer.DrawerOpened here
            {
                ActionBar.Title = _drawerTitle;
                drawerOpen = true;
                InvalidateOptionsMenu();
            };
            _drawer.SetDrawerListener(_drawerToggle);

            //При запуске приложения открывается нулевой элемент из switch'а для Navigation Drawer
            if (null == savedInstanceState)
            {
                SelectItem(0);
            }

        }

        protected override void OnStart()
        {
            base.OnStart();

            //ACTIVITY RECEIVER
            var intentFilter = new IntentFilter(WorkingInetInteractDB.aecActivityIntent) { Priority = (int)IntentFilterPriority.HighPriority };
            RegisterReceiver(downloadedReceiver, intentFilter);

            ScheduleUpdates();
        }
        protected override void OnResume()
        {
            base.OnResume();
            //if (isBound)
            //{
            //    RunOnUiThread(() =>
            //    {
            //        binder.GetServiceInetInteractDB().DownloadWork();
            //    }
            //    );
            //}
        }
        void ScheduleUpdates()
        {
            if (!IsAlarmSet())
            {
                var alarm = (AlarmManager)GetSystemService(Context.AlarmService);

                var pendingServiceIntent = PendingIntent.GetService(this, 0, wIIDBServiceIntent, PendingIntentFlags.CancelCurrent);
                alarm.SetRepeating(AlarmType.Rtc, 0, 35000, pendingServiceIntent);
                //alarm.SetRepeating (AlarmType.Rtc, 0, AlarmManager.IntervalHalfHour, pendingServiceIntent);
            }
            else
            {
                //Console.WriteLine("alarm already set");
            }
        }
        bool IsAlarmSet()
        {
            return PendingIntent.GetBroadcast(this, 0, wIIDBServiceIntent, PendingIntentFlags.NoCreate) != null;
        }
        class DownloadedReceiver : BroadcastReceiver        //ServiceInetInteractDB сообщает нам, что он скачал что-то
        {
            public override void OnReceive(Context context, Android.Content.Intent intent)
            {
                //((DrawerAct)context).UpdateStuff();
                InvokeAbortBroadcast();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();


        }

        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (keyCode == Keycode.Back && FragmentManager.BackStackEntryCount > 1)        //Button Back and we have some fragments in Backstack
            {
                FragmentManager.PopBackStackImmediate();
                string visibleFragmentTag = FragmentManager.FindFragmentById(Resource.Id.content_frame).Tag;
                if (visibleFragmentTag == "settingsfragment")
                { ActionBar.Title = GetString(Resource.String.Preferences); }
                else
                {
                    var fragmentTagMenuArray = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray);
                    int index = Array.IndexOf(fragmentTagMenuArray, visibleFragmentTag);
                    _drawerList.SetItemChecked(index, true);

                    ActionBar.Title = Resources.GetStringArray(Resource.Array.MenuArray)[index];               //ActionBar.Title = _drawerList.CheckedItemPosition.ToString();
                    InvalidateOptionsMenu();
                }
                return true;
            }
            else { return base.OnKeyDown(keyCode, e); }
        }

        //Switch для обработки выбора пунктов Navigation Drawer
        private void SelectItem(int position)
        {
            switch (position)
            {
                //Мероприятия
                case 0:
                    {
                        if (fragments[position] == null)
                        {
                            var fragment = new EventsListFragment();
                            fragments[position] = fragment;
                            //_lastFragment = fragment;
                            var arguments = new Bundle();
                            arguments.PutString(EventsListFragment.Account, "Emeri"); arguments.PutString(EventsListFragment.Password, "Emeri");
                            fragment.Arguments = arguments;
                            FragmentManager.BeginTransaction()
                                .Replace(Resource.Id.content_frame, fragment, "eventfragment")
                                .AddToBackStack("eventfragment")
                                .SetTransition(FragmentTransit.FragmentFade).Commit();
                         }
                         else if (fragments[position].IsVisible) {}
                         else
                         {
                             //var xyz = fragments[position].Tag;
                             var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                             ChangeFragment(fragmentTag, position);
                         }

                        break;
                    }
                //Календарь
                case 1:
                    {
                        if (fragments[position] == null)
                        {
                            var fragment = new CalendarFragment();
                            fragments[position] = fragment;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }
                        else if (fragments[position].IsVisible) { }
                        else
                        {
                            //var xyz = fragments[position].Tag;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }

                        break;
                    }
                //Мои события
                case 2:
                    {
                        if (fragments[position] == null)
                        {
                            var fragment = new MyEventsFragment();
                            fragments[position] = fragment;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }
                        else if (fragments[position].IsVisible) { }
                        else
                        {
                            //var xyz = fragments[position].Tag;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }

                        break;
                    }
                //Фильтрация
                case 3:
                    {
                        if (fragments[position] == null)
                        {
                            var fragment = new FiltrationFragment();
                            fragments[position] = fragment;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }
                        else if (fragments[position].IsVisible) { }
                        else
                        {
                            //var xyz = fragments[position].Tag;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }

                        break;
                    }
                //Профиль
                case 4:
                    {
                        if (fragments[position] == null)
                        {
                            var fragment = new ProfileFragment();
                            fragments[position] = fragment;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }
                        else if (fragments[position].IsVisible) { }
                        else
                        {
                            //var xyz = fragments[position].Tag;
                            var fragmentTag = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray)[position];
                            ChangeFragment(fragmentTag, position);
                        }

                        break;
                    }

                default:
                    break;
            }

            _drawerList.SetItemChecked(position, true);
            //ActionBar.Title = _title = _menuTitles[position];
            _title = _menuTitles[position];
            _drawer.CloseDrawer(_drawerList);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionBarMain, menu);
            drawerActivityMenu = menu;
            //if (actionBarState == -1)
            //{
            //    menu.FindItem(Resource.Id.action_search).SetEnabled(false);
            //    //if (menu.FindItem(Resource.Id.action_websearch) != null)
            //    //{ 
            //    //    menu.FindItem(Resource.Id.action_websearch).SetEnabled(false);
            //    //}
            //}
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            //var drawerOpen = _drawer.IsDrawerOpen(Resource.Id.left_drawer); drawerOpen = _drawer.IsDrawerVisible(Resource.Id.left_drawer);
            //MySuperOnPrepareOptionsMenu(menu, drawerOpen);

            if (drawerOpen)
            {
                menu.FindItem(Resource.Id.action_search).SetEnabled(false);
                menu.FindItem(Resource.Id.action_settings).SetEnabled(false);
                menu.FindItem(Resource.Id.action_sort).SetEnabled(false);
                menu.FindItem(Resource.Id.action_filter).SetEnabled(false);
                if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else
                { menu.FindItem(Resource.Id.action_login).SetVisible(true); menu.FindItem(Resource.Id.action_login).SetEnabled(false);  }
            }
            else
            {
                switch (actionBarState)          //Enforced State of ActionBar
                {
                    case 0:                     //EventsList state
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(true);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(true);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(true);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    case 1:                     //Calendar state
                        {

                            menu.FindItem(Resource.Id.action_search).SetVisible(false);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(false);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(false);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    case 2:                     //MyEvents state
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(true);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(true);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(true);

                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    case 3:                     //Filtration state
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(false);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(false);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(true);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    case 4:                     //Profile state
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(false);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(false);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(false);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    case 5:                     //Settings state
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(false);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(false);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(false);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(false);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                            break;
                        }
                    default:
                        {
                            menu.FindItem(Resource.Id.action_search).SetVisible(false);
                            menu.FindItem(Resource.Id.action_settings).SetVisible(true);
                            menu.FindItem(Resource.Id.action_sort).SetVisible(false);
                            menu.FindItem(Resource.Id.action_filter).SetVisible(false);
                            if (authentication) { menu.FindItem(Resource.Id.action_login).SetVisible(false); } else { menu.FindItem(Resource.Id.action_login).SetVisible(true); }
                        }
                        break;
                }
            }

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_drawerToggle.OnOptionsItemSelected(item))
                return true;

            switch (item.ItemId)
            {
                case Resource.Id.action_websearch:
                    {
                        var intent = new Intent(Intent.ActionWebSearch);
                        intent.PutExtra(SearchManager.Query, ActionBar.Title);

                        if ((intent.ResolveActivity(PackageManager)) != null)
                            StartActivity(intent);
                        else
                            Toast.MakeText(this, Resource.String.app_not_available, ToastLength.Long).Show();
                        return true;
                    }
                case Resource.Id.action_login:
                    {
                        var intent = new Intent(this, typeof(AccLogInAct));
                        intent.AddFlags(ActivityFlags.ClearTop);
                        StartActivity(intent);
                        return true;
                    }
                case Resource.Id.action_settings:
                    {
                        //Для deselect'а в NavDrawer'е
                        string visibleFragmentTag = FragmentManager.FindFragmentById(Resource.Id.content_frame).Tag;
                        var fragmentTagMenuArray = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray);
                        int index = Array.IndexOf(fragmentTagMenuArray, visibleFragmentTag);
                        _drawerList.SetItemChecked(index, false);

                        var settingsFragmentTag = "settingsfragment";
                        var fragment = new SettingsFragment();
                        FragmentManager.BeginTransaction()
                            .Replace(Resource.Id.content_frame, fragment, settingsFragmentTag)
                            .AddToBackStack(settingsFragmentTag)
                            .SetTransition(FragmentTransit.FragmentFade)
                            .Commit();

                        //actionBarState = Array.IndexOf(fragmentTagMenuArray, settingsFragmentTag);
                        //InvalidateOptionsMenu();
                        return true;
                    }
                    
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            if (featureId == (int)WindowFeatures.ActionBar && menu != null)
            {
                try
                {
                    var menuBuilder = JNIEnv.GetObjectClass(menu.Handle);
                    var setOptionalIconsVisibleMethod = JNIEnv.GetMethodID(menuBuilder, "setOptionalIconsVisible",
                        "(Z)V");
                    JNIEnv.CallVoidMethod(menu.Handle, setOptionalIconsVisibleMethod, new[] { new JValue(true) });
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short);
                    //Log.Error("MainActivity", "Something went wrong calling setOptionalIconsVisible(Z), exception: {0}",
                    //    e.Message);
                }
            }

            return base.OnMenuOpened(featureId, menu);
        }

        public void ChangeFragment (string nameoffragment, int position)
        {
            FragmentManager.BeginTransaction()
            .Replace(Resource.Id.content_frame, fragments[position], nameoffragment)
            .AddToBackStack(nameoffragment)
            .SetTransition(FragmentTransit.FragmentFade).Commit();
        }
        //
        public void MySuperInvalidateOptionsMenu()      //int whatFragmentShown
        {
            string visibleFragmentTag = FragmentManager.FindFragmentById(Resource.Id.content_frame).Tag;
            var fragmentTagMenuArray = Resources.GetStringArray(Resource.Array.FragmentTagMenuArray);
            int index = Array.IndexOf(fragmentTagMenuArray, visibleFragmentTag);
            actionBarState = index;
            InvalidateOptionsMenu();
        }
        protected override void OnDestroy()
        {
            UnregisterReceiver(downloadedReceiver);
            base.OnDestroy();
        }
    }
}

