using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using AEC;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using AEC.Service;

namespace AEC.Fragments
{
    class MyEventsFragment : Fragment   {
        ListView _listView;
        public MyEventsAdapter myEventsAdapter;
        private Android.Support.V7.Widget.SearchView _searchView;
        byte allevents = 100;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)                //Отличается от OnCreateOptionsMenu в Activity: возвращает void и принимает 2 параметра
        {
            base.OnCreateOptionsMenu(menu, menuInflater);

            var item = menu.FindItem(Resource.Id.action_search);
            MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(myEventsAdapter));
            var searchItem = MenuItemCompat.GetActionView(item);
            _searchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();

            if (_searchView != null)
            {
                try
                {
                    _searchView.QueryTextChange += (s, e) => myEventsAdapter.Filter.InvokeFilter(e.NewText);
                    _searchView.QueryTextSubmit += (s, e) =>
                    {
                        Toast.MakeText(Activity, "Searched for: " + e.Query, ToastLength.Short).Show();             //Do something fancy when search button on keyboard is pressed
                        //e.Handled = true;
                    };
                }
                catch (Exception e) { var a = e; throw; }
            }
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.Events, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.FoundedEventsList);             // Set our View from the Events layout resource

            //Получаем краткие данные по событиям
            var check1 = new WorkingInetInteractDB();
            var newEventsData = check1.GetEventsData(allevents).ToList<EventShort>();

            myEventsAdapter = new MyEventsAdapter(Activity, newEventsData);
            _listView.Adapter = myEventsAdapter;
            _listView.ItemClick += OnListItemClick;

            SetHasOptionsMenu(true);
            return view;
        }

        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)             //override
        {
            //var listView = sender as ListView;
            var t = myEventsAdapter[e.Position];

            var intentEvent = new Intent(Activity, typeof(EventFullAct));
            intentEvent.PutExtra("Event", t.Id);
            StartActivity(intentEvent);

            //// Check what fragment is shown, replace if needed.
            //var details = FragmentManager.FindFragmentById<DetailsFragment>(Resource.Id.EventFullAct);
            //if (details == null || details.ShownIndex != index)
            //{
            //    // Make new fragment to show this selection.
            //    details = DetailsFragment.NewInstance(index);

            //    // Execute a transaction, replacing any existing
            //    // fragment with this one inside the frame.
            //    var ft = FragmentManager.BeginTransaction();
            //    ft.Replace(Resource.Id.details, details);
            //    ft.SetTransition(FragmentTransit.FragmentFade);
            //    ft.Commit();
            //}
        }

        public override void OnResume()
        {
            base.OnResume();
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }
    }


    public class MyEventsAdapter : BaseAdapter<EventShort>, IFilterable
    {
        readonly Activity _context;
        //EventShort[] _originalData; EventShort[] _items;
        List<EventShort> _originalData; List<EventShort> _items;
        //readonly Photo[] _photos;
        List<Bitmap> _photos = new List<Bitmap>();

        public MyEventsAdapter(Activity context, IEnumerable<EventShort> items) //: base()           Раньше передавался и массив с фотками - , Photo[] photos
        {
            _context = context;
            _items = items.OrderBy(s => s.Name).ToList();                           //Когда было наоборот, массив, было - .ToArray()
            var check1 = new WorkingInetInteractDB();
            _photos = check1.GetEventsPhotos(_items.ToArray());
            Filter = new SuggestionsFilter(this);                                   //Добавляем в конструктор фильтр
        }
        //SUPER FILTER BEGINS
        //Filter filter; public string[] AllItems; public string[] MatchItems;
        public Filter Filter { get; private set; }              //Другой вариант - public Filter Filter { get { return filter; } }

        class SuggestionsFilter : Filter
        {
            MyEventsAdapter a;
            public SuggestionsFilter(MyEventsAdapter adapter) //: base()
            {
                a = adapter;
            }

            protected override Filter.FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
            {
                FilterResults returnObj = new FilterResults();
                var results = new List<EventShort>();

                if (a._originalData == null)
                    a._originalData = a._items;
                if (constraint == null) return returnObj;

                if (a._originalData != null && a._originalData.Any())
                {
                    // Compare constraint to all names lowercased. If they are contained they are added to results.
                    results.AddRange(
                        a._originalData.Where(
                            item => item.Name.ToLower().Contains(constraint.ToString())));
                }

                // Nasty piece of .NET to Java wrapping, be careful with this!
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }
            protected override void PublishResults(Java.Lang.ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    a._items = values.ToArray<Java.Lang.Object>().Select(r => r.ToNetObject<EventShort>()).ToList();

                a.NotifyDataSetChanged();

                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }
        }
        //SUPER FILTER ENDS



        public override long GetItemId(int position)
        {
            return position;
        }
        public override EventShort this[int position]
        {
            get { return _items[position]; }
        }
        public override int Count
        {
            get { return _items.Count; }               //Было .Length для массива
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            //var image = _photos[position];

            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.EventsList, null);

            ////Для строк-мероприятий в ListView прописываем источники данных
            view.FindViewById<TextView>(Resource.Id.TextEventName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.FullEventDate).Text = item.Date.ToString("dd.MM.yy");
            view.FindViewById<TextView>(Resource.Id.TextOrganizer).Text = item.OrganizerName;
            //view.FindViewById<TextView>(Resource.Id.TextEventDescription).Text = item.OrganizerName;

            //Если фотка есть, то обрабатываем:
            if (_photos[position] != null)
            {
                view.FindViewById<ImageView>(Resource.Id.ImageEvent).SetImageBitmap(_photos[position]);
            }
            return view;
        }


    }
}