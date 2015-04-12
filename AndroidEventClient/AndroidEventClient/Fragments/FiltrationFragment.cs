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
using System.Collections.Generic;
using System.Linq;
using AEC.Service;

namespace AEC    {
	public class FiltrationFragment : Fragment    {

        private Fragment _lastFragment;
        private FlyOutContainer _flyOut;
        public string FragmentLaunched = "zero";
        //private Android.Support.V7.Widget.SearchView _searchView;
        //Spinner для FiltrCategories
        public Spinner navFilSpinner;
        public bool navFilSpinnerClientUsing = false;
        //Current cash
        public List<long> idFiltrationNavList;
        public List<string> textFiltrationNavList;
        public View filtrationFragmentView { get; set; }
        public ArrayAdapter navigationFiltrationAdapter { get; set; }

        //private void FiltrationFragment() { SetRetainInstance(true); }
        
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			//base.OnCreateView (savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Filtration, container, false);
            filtrationFragmentView = view;
            FiltrationNavList.PutInCache(0, DataService.whatVersionCatID, GetString(Resource.String.All_categories));

            //if (FragmentLaunched == "main")
            //{
            //    try {
            //        FragmentManager.BeginTransaction()
            //           .Remove(_lastFragment);
            //        _lastFragment.Dispose();
            //    }
            //    catch (Exception e) { var k = e.Message; }
            //}

            //Первоначальная загрузка
            _flyOut = view.FindViewById<FlyOutContainer>(Resource.Id.FlyOutContainer);
            var menuButton = view.FindViewById(Resource.Id.MenuButton);
			menuButton.Click += (sender, e) =>
            {
                _flyOut.AnimatedOpened = !_flyOut.AnimatedOpened;
			};

            //FRAGMENTS BEGINS
            //При открытии используется 
            var categorieslistfragment = new FiltrCategoriesListFrag();
            _lastFragment = categorieslistfragment;
            FragmentLaunched = "categorieslistfragment";
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.filtration_content_frame, categorieslistfragment, FragmentLaunched)
                .SetTransition(FragmentTransit.FragmentFade)
                .Commit();

            //Используем фрагмент ..., если открываем профиль аккаунта - ProfileFragments
            var filterControlTextView = view.FindViewById<TextView>(Resource.Id.textView5);
            filterControlTextView.Click += (sender, args) =>
		    {
                //var profileFragment = new ProfileFragment();
                //FragmentManager.BeginTransaction()
                //    .Remove(_lastFragment)
                //    .Add(Resource.Id.filtration_content_frame, profileFragment)
                //    .SetTransition(FragmentTransit.FragmentFade)
                //    .Commit();

                //_lastFragment.Dispose();
                //_lastFragment = profileFragment;

                //_flyOut.ExternalyClosed();
		    };

            //Используем фрагмент ..., если открываем категории - CategoriesListFragments
            var filterCategoryTextView = view.FindViewById<TextView>(Resource.Id.FilterCategory);
            filterCategoryTextView.Click += (sender, args) =>
            {
                var eventsListFragment2 = new FiltrCategoriesListFrag();
                FragmentManager.BeginTransaction()
                    .Remove(_lastFragment)
                    .Add(Resource.Id.filtration_content_frame, eventsListFragment2)
                    //.AddToBackStack("categorieslistfragment")
                    .SetTransition(FragmentTransit.FragmentFade)
                    .Commit();

                _lastFragment.Dispose();
                _lastFragment = eventsListFragment2;

                _flyOut.ExternalyClosed();
            };
            //FRAGMENTS ENDS

            //Spinner filterSpinner = view.FindViewById<Spinner>(Resource.Id.FilterSpinner);
            //filterSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(filterSpinner_ItemSelected);
            //var filterAdapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.Filter_array, Android.Resource.Layout.SimpleSpinnerItem);
            //filterAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //filterSpinner.Adapter = filterAdapter;

            //Навигация по уровням иерархии фильтруемого образа
            //Обработчик кликов по пунктам меню
            navFilSpinner = view.FindViewById<Spinner>(Resource.Id.NavFilSpinner);
            navFilSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(navFilSpinner_ItemSelected);

            //Первоначальное составление списка элементов в меню navFilSpinner
            FiltrationNavList.GetFromCache(0, -1, out idFiltrationNavList, out textFiltrationNavList);
            var navFilAdapter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleSpinnerItem, textFiltrationNavList);
            navFilAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            navigationFiltrationAdapter = navFilAdapter;
            navFilSpinner.Adapter = navigationFiltrationAdapter;
            //result &= Service.GetRootCategory??
            //navFilSpinner.Adapter.RegisterDataSetObserver();

            return view;
		}

        //public void setLabelVisibility(bool hideOrShow) {
        //    var hidingLabelText = filtrationFragmentView.FindViewById<TextView>(Resource.Id.SecondText);
        //    if (hideOrShow) { hidingLabelText.Visibility = ViewStates.Visible; }
        //    else { hidingLabelText.Visibility = ViewStates.Invisible; }
        //}

        //Описание обработчика для navFilSpinner
        private void navFilSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            //string spinnerUsed = (GetString(Resource.String.Filter_chosen) + " {0}");
            //string toast = string.Format(spinnerUsed, spinner.GetItemAtPosition(e.Position)); Toast.MakeText(Activity, toast, ToastLength.Long).Show();

            //Чтобы избавиться от бесконечного цикла (при создании spinner выбирается первый элемент в нём)
            int firstTimeFilNav = -1;
            if (!navFilSpinnerClientUsing) {
                firstTimeFilNav = e.Position;
                navFilSpinnerClientUsing = true;
            }
            //spinner.RefreshDrawableState();
            if (e.Position == firstTimeFilNav) {
                }
            else {
                var categorieslistFragment = (FiltrCategoriesListFrag)FragmentManager.FindFragmentByTag("categorieslistfragment");
                //Переход на выбранный уровень иерархии
                categorieslistFragment.NavigateUpHierarchyCat(e.Position);
            }
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait) {
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape) {
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater) {
        //    MenuInflater.Inflate(Resource.Menu.ActionBarMain, menu);
            base.OnCreateOptionsMenu(menu, menuInflater);

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

        }

        public override void OnResume() {
            base.OnResume();
            ((DrawerAct)this.Activity).MySuperInvalidateOptionsMenu();
        }

	}

    //Кэш для элементов управления FiltrationFragment
    public class FiltrationNavList {
        //Кэш для категорий событий
        private static List<long> idCatFiltrationNavList = new List<long>();
        private static List<string> catFiltrationNavList = new List<string>();
        //Кэш для организаторов
        private static List<long> idOrgFiltrationNavList = new List<long>();
        private static List<string> orgFiltrationNavList = new List<string>();

        /// <summary>
        /// Поместить информацию, о том, что смотрел пользователь при навигации фильтров
        /// </summary>
        /// <param name="type">Какого типа фильтрация была использована. 0 - по категориям</param>
        /// <param name="id">id пункта фильтрации</param>
        /// <param name="name">Название пункта фильтрации</param>
        /// <returns></returns>
        public static bool PutInCache(short type, long id, string name) {

            switch (type)
            {
                case 0: {
                    idCatFiltrationNavList.Add(id);
                    catFiltrationNavList.Add(name);

                    break;
                }
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Получить информацию, о том, что смотрел пользователь при навигации фильтров.
        /// type = 0 - фильтр по категории
        /// </summary>
        /// <param name="type">Какого типа фильтрация интересует. 0 - по категориям</param>
        /// <param name="level">По какой уровень выбрать данные из кэша (что не выбрано, то удаляется). Если -1, то всё выбирается</param>
        /// <param name="someIDFiltrationNavList">id пункта фильтрации</param>
        /// <param name="someFiltrationNavList">Название пункта фильтрации</param>
        /// <returns></returns>
        public static bool GetFromCache(int type, int level, out List<long> someIDFiltrationNavList, out List<string> someFiltrationNavList)
        {
            //Задаём начальные выходные значения
            someIDFiltrationNavList = null;
            someFiltrationNavList = null;
            //В зависимости от типа фильтрации, выдаем, что смотрел пользователь по каждому типу
            switch (type)
            {
                case 0:
                    {
                        someIDFiltrationNavList = new List<long>();
                        someFiltrationNavList = new List<string>();

                        if (level == -1) {

                        }
                        else {
                            idCatFiltrationNavList = idCatFiltrationNavList.Take(level + 1).ToList();
                            catFiltrationNavList = catFiltrationNavList.Take(level + 1).ToList();
                        }

                        someIDFiltrationNavList = idCatFiltrationNavList;
                        someFiltrationNavList = catFiltrationNavList;

                        break;
                    }
                default:
                    break;
            }
            return true;
        }

    }

}


