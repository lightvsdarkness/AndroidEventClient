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

namespace AEC.Fragments  {
    class EventsListFragment : Fragment     {
        public static string Account = "Account";
        public static string Password = "Password";

        //��������� ����������� ������ �������
        ListView _listView;
        //������� ������ �������
        public EventsAdapter2 _adapter = null;
        private Android.Support.V7.Widget.SearchView _searchView;
        byte allevents = 100;
        //public EventsListFragment(IntPtr a, JniHandleOwnership b) : base(a, b)
        //{ }
        //public EventsListFragment() : base()
        //{ }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)                //���������� �� OnCreateOptionsMenu � Activity: ���������� void � ��������� 2 ���������
        {
            //menuInflater.Inflate(Resource.Menu.OneEvent, menu);
            base.OnCreateOptionsMenu(menu, menuInflater);

            //TO DO
            //var item = menu.FindItem(Resource.Id.action_search);
            //MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(standardEventsAdapter));
            //var searchItem = MenuItemCompat.GetActionView(item);
            //_searchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();

            //if (_searchView != null)
            //{ 
            //try
            //{
            //    _searchView.QueryTextChange += (s, e) => standardEventsAdapter.Filter.InvokeFilter(e.NewText);
            //    _searchView.QueryTextSubmit += (s, e) =>
            //    {
            //        Toast.MakeText(Activity, "Searched for: " + e.Query, ToastLength.Short).Show();             //Do something fancy when search button on keyboard is pressed
            //        //e.Handled = true;
            //    };
            //}
            //catch (Exception e) { var a = e; throw; }
            //}
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)      
        {

            var acc = Arguments.GetString(Account);
            var pass = Arguments.GetString(Password);

            var view = inflater.Inflate(Resource.Layout.Events, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.FoundedEventsList);             // Set our View from the Events layout resource. ���������� ������ listview ��� �����.

            var myActivity = (DrawerAct)this.Activity;

            EventShort[] newEventsData = new EventShort[1] { new EventShort() { Name = "TO DO" }};
            List<Bitmap> newPhotosData = new List<Bitmap>();

            //������ ������� ������ �������
            _adapter = new EventsAdapter2(Activity);
            //������������� ������� ������ �������
            _listView.Adapter = _adapter;
            _listView.ItemClick += OnListItemClick;

        //    string yy = "AEC.assets." + "WordList.txt";
        //    List<string> lines = new List<string>();
        //    //Stream seedDataStream = Assets.Open(@"WordList.txt");
        //    using (Stream seedDataStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(yy))
        //    {
        //        using (StreamReader reader = new StreamReader(seedDataStream))
        //        {
        //            string line;
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                lines.Add(line);
        //            }
        //        }
        //    }
        //    string[] wordlist = lines.ToArray();

            SetHasOptionsMenu(true);
            return view;
        }

        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)             //override
        {
            //var listView = sender as ListView;
            var t = _adapter[e.Position];
            //Toast.MakeText(Activity, t.Name, ToastLength.Short).Show();

            var intentEvent = new Intent(Activity, typeof(EventFullAct));
            intentEvent.PutExtra("Event", t.Id);
            StartActivity(intentEvent);
            Console.WriteLine("Clicked on " + t.Name);

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


            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context); 
            //ISharedPreferences prefs = Context.GetSharedPreferences;
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutLong("eventID", t.Id);
            editor.Apply();
        }

        public override void OnResume()
        {
            base.OnResume();
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }

        public override void OnDestroy()
        {
            //���������� ������� ������ �������
            _listView.Adapter = null;
            //������� ������
            _adapter = null;

            base.OnDestroy();
        }
    }

    //Adapter for Events ListView
    public class EventsAdapter : BaseAdapter<EventShort>, IFilterable
    {
        readonly Activity _context;
        //EventShort[] _originalData; EventShort[] _items;
        List<EventShort> _originalData; List<EventShort> _items; 
        //readonly Photo[] _photos;
        List<Bitmap> _photos = new List<Bitmap>();

        //������������ �������� ���������� ������ �������, � ����������� ����� ������ ����, ��������������� �������� �� �������
        public EventsAdapter(Activity context, IEnumerable<EventShort> items, List<Bitmap> photos) //: base()           ������ ����������� � ������ � ������� - , Photo[] photos
        {
            _context = context;
            _items = items.OrderBy(s => s.Name).ToList();                           //����� ���� ��������, ������, ���� - .ToArray()
            var check1 = new WorkingInetInteractDB();
            _photos = photos;                                               //check1.GetEventsPhotos(_items.ToArray());
            Filter = new SuggestionsFilter(this);                                   //��������� � ����������� ������
        }
        //SUPER FILTER BEGINS
        //Filter filter; public string[] AllItems; public string[] MatchItems;
        public Filter Filter { get; private set; }              //������ ������� - public Filter Filter { get { return filter; } }

        class SuggestionsFilter : Filter
        {
            EventsAdapter a;
            public SuggestionsFilter(EventsAdapter adapter) //: base()
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
                
                
                //if (constraint != null)
                //{
                //    var searchFor = constraint.ToString();

                //    var matchList = new List<string>();

                //    var matches = from i in a.AllItems
                //                  where i.IndexOf(searchFor) >= 0
                //                  select i;

                //    foreach (var match in matches)
                //    {
                //        matchList.Add(match);
                //    }

                //    a.MatchItems = matchList.ToArray();

                //    Java.Lang.Object[] matchObjects;
                //    matchObjects = new Java.Lang.Object[matchList.Count];
                //    for (int i = 0; i < matchList.Count; i++)
                //    {
                //        matchObjects[i] = new Java.Lang.String(matchList[i]);
                //    }

                //    returnObj.Values = matchObjects;
                //    returnObj.Count = matchList.Count;
                //}
                //return returnObj;
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
            get { return _items.Count; }               //���� .Length ��� �������
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            //var image = _photos[position];

            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.EventsList, null);

            ////��� �����-����������� � ListView ����������� ��������� ������
            view.FindViewById<TextView>(Resource.Id.TextEventName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.FullEventDate).Text = item.Date.ToString("dd.MM.yy");
            view.FindViewById<TextView>(Resource.Id.TextOrganizer).Text = item.OrganizerName;
            //view.FindViewById<TextView>(Resource.Id.TextEventDescription).Text = item.OrganizerName;
            
            //���� ����� ����, �� ������������:
            //TO DO
            //if (_photos[position] != null)
            //{
            //    view.FindViewById<ImageView>(Resource.Id.ImageEvent).SetImageBitmap(_photos[position]);
            //}
            return view;
        }
    }

    /// <summary>
    /// ������� ��������� ������ �������
    /// </summary>
    public class EventsAdapter2 : DataAdapter<EventShort, EventShort, Bitmap>
    {
        //�������� ������ ��������
        readonly Activity _context;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="context"></param>
        public EventsAdapter2(Activity context)
        {
            //��������� ��������
            _context = context;
        }

        /// <summary>
        /// ���������������� ����� �������� ��������� ������
        /// </summary>
        /// <param name="position">��������� ������� ����������� ��������� � ��������� ������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="items">������������ ������ ���������</param>
        /// <returns>������� ���������� ��������</returns>
        public override bool LoadItems(int position, int count, out List<EventShort> items)
        {
            //����� ��������� �������� ��������
            items = null;

            //��������� ��������
            bool result = true;

            //���������� ������ �������
            EventShort[] events;

            //�������� ������� �� �������
            result &= Service.GetEventsList(position, count, out events);
            //���� ������
            if (!result)
            {
                //������� � �������
                return false;
            }

            //���������� ������� ��� ������ ���������
            items = events.ToList();

            //���������� ����� ��������
            return true;
        }

        /// <summary>
        /// ���������������� ����� �������� ������ ��������
        /// </summary>
        /// <param name="item"></param>
        /// <returns>������, ���������� ����������� ������</returns>
        public override bool LoadItem(EventShort item, out Bitmap extras)
        {
            //����� ��������� �������� ��������
            extras = null;

            //��������� ��������
            bool result = true;

            //���� � ������� ���� ����
            if (item.PrimaryPhotoId != null)
            {
                //���������� ������ ����
                Bitmap photo;

                //�������� �������� ������� �� �������
                result &= Service.GetIcon(item.PrimaryPhotoId.Value, out photo, false);
                //���� ������
                if (!result)
                {
                    //������� � �������
                    return false;
                }

                //���������� ���� ��� �������������� ������
                extras = photo;
            }

            //���������� ����� ��������
            return true;
        }

        /// <summary>
        /// ���������������� ����� ������������ ���������� ���� ��������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetInitialView(EventShort item, View convertView, ViewGroup parent, out bool loadItemExtras)
        {
            //�������������� �������� ���������
            loadItemExtras = true;

            //�������������� ��������� ����������� ��� ������ �����
            View itemView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.EventsList, null);

            //���������� ������ ��������
            itemView.FindViewById<TextView>(Resource.Id.TextEventName).Text = item.Name;
            itemView.FindViewById<TextView>(Resource.Id.FullEventDate).Text = item.Date.ToString("dd.MM.yy");
            itemView.FindViewById<TextView>(Resource.Id.TextOrganizer).Text = item.OrganizerName;

            //���������� �������� ����
            itemView.FindViewById<ImageView>(Resource.Id.ImageEvent).SetImageBitmap(null);

            //���� � ������� ���� ����
            if (item.PrimaryPhotoId != null)
            {
                //���������� ������ ����
                Bitmap photo;

                //�������� �������� �������� �� ������� �� ����
                if (Service.GetIcon(item.PrimaryPhotoId.Value, out photo, true))
                {
                    //���� ����������
                    //���������� ����
                    itemView.FindViewById<ImageView>(Resource.Id.ImageEvent).SetImageBitmap(photo);
                    //���������� ������� ������������� ���������� ��������
                    loadItemExtras = false;
                }
            }

            //���������� ���������
            return itemView;
        }

        /// <summary>
        /// ���������������� ����� ������������ �������������� ���� ��������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="extras">������, ������������ ������� LoadItem</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override void FormFinalView(EventShort item, Bitmap extras, View itemView)
        {
            //��������� ������ �� ������ ����
            Bitmap photo = extras as Bitmap;

            //���� ���� ����������
            if (photo != null)
            {
                //���������� ����������� ����
            }
            else
            {
                //���������� �������� �� ���������
                var options = new Android.Graphics.BitmapFactory.Options
                {
                    InJustDecodeBounds = false,
                };
                Context context = Android.App.Application.Context;
                Android.Content.Res.Resources res = context.Resources;
                photo = Android.Graphics.BitmapFactory.DecodeResource(res, Resource.Drawable.Leo, options);
            }
            //������������� ���� � ���������
            itemView.FindViewById<ImageView>(Resource.Id.ImageEvent).SetImageBitmap(photo);
        }
    }
}
