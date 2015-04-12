using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoDroid.TimesSquare;

namespace AEC.Fragments
{
    class CalendarFragment : Fragment   {
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            //var surfaceOrientation = activity.WindowManager.DefaultDisplay.Rotation;
            //surfaceOrientation = SurfaceOrientation.Rotation0;                // Portrait;
            //surfaceOrientation = SurfaceOrientation.Rotation90;
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)
        {
            if (menu.FindItem(Resource.Id.action_websearch) !=null )
            { menu.FindItem(Resource.Id.action_websearch).SetVisible(false); }
            base.OnCreateOptionsMenu(menu, menuInflater);
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            if (menu.FindItem(Resource.Id.action_websearch) != null)
            { menu.FindItem(Resource.Id.action_websearch).SetVisible(false); }
            base.OnPrepareOptionsMenu(menu);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Calendar, container, false);
            SetHasOptionsMenu(true);

            var nextYear = DateTime.Now.AddYears(1);
            var dates = new List<DateTime>();
            for (int i = 0; i < 4; i++)
            {
                dates.Add(DateTime.Now.AddDays(3 * i));
            }

            var calendar = view.FindViewById<CalendarPickerView>(Resource.Id.calendar_view);
            calendar.Init(DateTime.Now, nextYear)
                .InMode(CalendarPickerView.SelectionMode.Multi)
                .WithSelectedDates(dates)
                ;
            //.WithHighlightedDate(DateTime.Now.AddDays(2))


            // Get our button from the layout resource,
            // and attach an event to it
            //var refreshProfileButton = view.FindViewById<Button>(Resource.Id.RefreshProfileButton);

            //refreshProfileButton.Click += delegate { refreshProfileButton.Text = string.Format("Ваши пользовательские данные обновлены!\nОбновить еще раз?"); };

            //var emailText = view.FindViewById<EditText>(Resource.Id.EmailText);
            //var passwordText = view.FindViewById<EditText>(Resource.Id.PasswordText);
            //emailText.Text = string.Format("Emeri");
            //passwordText.Text = string.Format("Emeri");


            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }
    }

}