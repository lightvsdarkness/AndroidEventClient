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
    class FiltrCategoriesListFrag : Fragment    {
        ListView _listView;
        public CategoriesAdapter2 standardCategoriesAdapter;
        //private Android.Support.V7.Widget.SearchView _searchView;

        public int stackPos = 0;
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)                //���������� �� OnCreateOptionsMenu � Activity: ���������� void � ��������� 2 ���������
        {
            //menuInflater.Inflate(Resource.Menu.OneEvent, menu);
            base.OnCreateOptionsMenu(menu, menuInflater);
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)      {

            var view = inflater.Inflate(Resource.Layout.Events, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.FoundedEventsList);             // Set our View from the Events layout resource
            SetHasOptionsMenu(true);

            var myActivity = (DrawerAct)this.Activity;

            ////�������� ������� ������ �� ����������

            //������ ������ �������� ��������� ������
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //������ � ������������� ������ ��������� ��������� ������
                DataContext = new CategoriesListContext() 
                {
                    RootId = DataService.whatVersionRootCatID,
                    ParentId = DataService.whatVersionCatID 
                }
            };
            //������������ ������� ��� ������
            _listView.Adapter = standardCategoriesAdapter;
            _listView.ItemClick += OnListItemClick;

            return view;
        }

        //���� ������� �� ��������� ��� ��������� �� �� �������������
        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)             //override
        {
            //var listView = sender as ListView;
            var t = standardCategoriesAdapter[e.Position];
            //Toast.MakeText(Activity, t.Name, ToastLength.Short).Show();
            NavigateDownHierarchyCat(t.Id, t.Name);
        }

        //protected void OnListItemClick(ListView l, View v, int position, long id) {}
       //��� ������ ��������� ��������� ������ ���������
        public void NavigateDownHierarchyCat(long selectedCategoryId, string selectedCategoryName)
        {
            var filtrationFragment = (FiltrationFragment)FragmentManager.FindFragmentByTag("filtrationfragment");
            FiltrationNavList.PutInCache(0, selectedCategoryId, selectedCategoryName);

            var myActivity = (DrawerAct)this.Activity;

            //������ ������ �������� ��������� ������ ��� ����� �������� ������
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //������ � ������������� ������ ��������� ��������� ������
                DataContext = new CategoriesListContext()
                {
                    RootId = DataService.whatVersionRootCatID,
                    ParentId = selectedCategoryId
                }
            };
            _listView.Adapter = standardCategoriesAdapter;

            //���������� ��� �������� navFilSpinner ����� �������
            FiltrationNavList.GetFromCache(0, -1, out filtrationFragment.idFiltrationNavList, out filtrationFragment.textFiltrationNavList);

            var navFilAdapter = new ArrayAdapter(filtrationFragment.Activity, Android.Resource.Layout.SimpleSpinnerItem, filtrationFragment.textFiltrationNavList);
            navFilAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            filtrationFragment.navFilSpinnerClientUsing = false;
            filtrationFragment.navFilSpinner.Adapter = navFilAdapter;
            filtrationFragment.navFilSpinner.SetSelection(filtrationFragment.textFiltrationNavList.Count - 1);

            //�������� �� ��, ���� �� ������������
            if (standardCategoriesAdapter.cashitems.FirstOrDefault() != null)
            {
                //���� ����, ������������� ������� ��� ������
                //_listView.Adapter = standardCategoriesAdapter;            //_listView.RefreshDrawableState();
            }
            else
            {
                //�.�. cashitems ������ ��� ������ �� ���� �����, �� ���� �������� ����������. ���� ������, ����� ��-���� ����������� LoadItems
                //Toast.MakeText(Activity, selectedCategoryName + "\n��������� �� �������� ������������", ToastLength.Short).Show();
            }
        }

        public void NavigateUpHierarchyCat(int whereUp)
        {
            var filtrationFragment = (FiltrationFragment)FragmentManager.FindFragmentByTag("filtrationfragment");
            FiltrationNavList.GetFromCache(0, whereUp, out filtrationFragment.idFiltrationNavList, out filtrationFragment.textFiltrationNavList);
            //standardCategoriesAdapter.NavigateFilCategories(whereUp, this.FragmentManager);

            //������ ������ �������� ��������� ������ ��� ����� �������� ������
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //������ � ������������� ������ ��������� ��������� ������
                DataContext = new CategoriesListContext()
                {
                    RootId = DataService.whatVersionRootCatID,
                    ParentId = filtrationFragment.idFiltrationNavList[whereUp]
                }
            };
            _listView.Adapter = standardCategoriesAdapter;
            var navFilAdapter = new ArrayAdapter(filtrationFragment.Activity, Android.Resource.Layout.SimpleSpinnerItem, filtrationFragment.textFiltrationNavList);
            navFilAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            filtrationFragment.navFilSpinnerClientUsing = false;
            filtrationFragment.navFilSpinner.Adapter = navFilAdapter;
            filtrationFragment.navFilSpinner.SetSelection(filtrationFragment.textFiltrationNavList.Count - 1);
        }

    }


    /// <summary>
    /// ����� ��������� ������ ���������
    /// </summary>
    public class CategoriesListContext
    {
        /// <summary>
        /// �������� �������� ������
        /// </summary>
        public long RootId;

        /// <summary>
        /// ������������ �������� ������
        /// </summary>
        public long ParentId;
    }

    /// <summary>
    /// ������� ��������� ������ ���������
    /// </summary>
    public class CategoriesAdapter2 : DataAdapter<CategoryShort, CategoriesListContext, Bitmap>
    {
        //�������� ������ ��������
        readonly Activity _context;

        //����������� ����� ���
        public List<CategoryShort> cashitems = new List<CategoryShort>();

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="context"></param>
        public CategoriesAdapter2(Activity context)
        {
            //��������� ��������
            _context = context;

        }

        /// <summary>
        /// ���������������� ����� �������� ��������� - ���������
        /// </summary>
        /// <param name="position">��������� ������� ����������� ��������� � ��������� ������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="items">������������ ������ ���������</param>
        /// <returns>������� ���������� ��������</returns>
        public override bool LoadItems(int position, int count, out List<CategoryShort> items)
        {
            //����� ��������� �������� ��������
            items = null;

            //��������� ��������
            bool result = true;

            //���������� ������ �������
            CategoryShort[] categories;

            //�������� ������� �� �������
            result &= Service.GetCategoriesList(DataContext.ParentId, DataContext.RootId, position, count, out categories);
            //���� ������
            if (!result)
            {
                //������� � �������
                return false;
            }

            //���������� ��������� ��� ������ ���������
            items = categories.ToList();

            cashitems = items;

            //���������� ����� ��������
            return true;
        }

        /// <summary>
        /// ���������������� ����� �������� ������ ��������
        /// </summary>
        /// <param name="item"></param>
        /// <returns>������, ��������� ����������� ������</returns>
        public override bool LoadItem(CategoryShort item, out Bitmap extras)
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
        /// <param name="loadItemExtras">���������</param>
        /// <returns></returns>
        public override View GetInitialView(CategoryShort item, View convertView, ViewGroup parent, out bool loadItemExtras)
        {
            //�������������� �������� ���������
            loadItemExtras = true;

            //�������������� ��������� ����������� ��� ������ �����
            View itemView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CategoriesList, null);

            //���������� ������ ��������
            itemView.FindViewById<TextView>(Resource.Id.CategoryName).Text = item.Name;

            //���������� �������� ����
            itemView.FindViewById<ImageView>(Resource.Id.ImageCategory).SetImageResource(Resource.Drawable.noimage);

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
                    itemView.FindViewById<ImageView>(Resource.Id.ImageCategory).SetImageBitmap(photo);
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
        public override void FormFinalView(CategoryShort item, Bitmap extras, View itemView)
        {
            //��������� ������ �� ������ ����
            Bitmap photo = extras;        //extras as Bitmap;

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
                photo = Android.Graphics.BitmapFactory.DecodeResource(res, Resource.Drawable.noimage, options);
            }
            itemView.FindViewById<ImageView>(Resource.Id.ImageCategory).SetImageBitmap(photo);
        }
    }

}
