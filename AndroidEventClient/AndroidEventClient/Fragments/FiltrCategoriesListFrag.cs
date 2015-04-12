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
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)                //Отличается от OnCreateOptionsMenu в Activity: возвращает void и принимает 2 параметра
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

            ////Получаем краткие данные по категориям

            //Создаём объект адаптера получения данных
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //Создаём и устанавливаем объект контекста получения данных
                DataContext = new CategoriesListContext() 
                {
                    RootId = DataService.whatVersionRootCatID,
                    ParentId = DataService.whatVersionCatID 
                }
            };
            //Устаналиваем адаптер для списка
            _listView.Adapter = standardCategoriesAdapter;
            _listView.ItemClick += OnListItemClick;

            return view;
        }

        //Юзер кликает на категории для навигации по ее подкатегориям
        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)             //override
        {
            //var listView = sender as ListView;
            var t = standardCategoriesAdapter[e.Position];
            //Toast.MakeText(Activity, t.Name, ToastLength.Short).Show();
            NavigateDownHierarchyCat(t.Id, t.Name);
        }

        //protected void OnListItemClick(ListView l, View v, int position, long id) {}
       //При выборе категории обновляем список категорий
        public void NavigateDownHierarchyCat(long selectedCategoryId, string selectedCategoryName)
        {
            var filtrationFragment = (FiltrationFragment)FragmentManager.FindFragmentByTag("filtrationfragment");
            FiltrationNavList.PutInCache(0, selectedCategoryId, selectedCategoryName);

            var myActivity = (DrawerAct)this.Activity;

            //Создаём объект адаптера получения данных для новой загрузки данных
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //Создаём и устанавливаем объект контекста получения данных
                DataContext = new CategoriesListContext()
                {
                    RootId = DataService.whatVersionRootCatID,
                    ParentId = selectedCategoryId
                }
            };
            _listView.Adapter = standardCategoriesAdapter;

            //Напоследок даём спиннеру navFilSpinner новый адаптер
            FiltrationNavList.GetFromCache(0, -1, out filtrationFragment.idFiltrationNavList, out filtrationFragment.textFiltrationNavList);

            var navFilAdapter = new ArrayAdapter(filtrationFragment.Activity, Android.Resource.Layout.SimpleSpinnerItem, filtrationFragment.textFiltrationNavList);
            navFilAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            filtrationFragment.navFilSpinnerClientUsing = false;
            filtrationFragment.navFilSpinner.Adapter = navFilAdapter;
            filtrationFragment.navFilSpinner.SetSelection(filtrationFragment.textFiltrationNavList.Count - 1);

            //Проверка на то, есть ли подкатегории
            if (standardCategoriesAdapter.cashitems.FirstOrDefault() != null)
            {
                //Если есть, устанавливаем адаптер для списка
                //_listView.Adapter = standardCategoriesAdapter;            //_listView.RefreshDrawableState();
            }
            else
            {
                //Т.е. cashitems всегда еще пустой на этом этапе, от идеи пришлось отказаться. Надо узнать, когда всё-таки выполняется LoadItems
                //Toast.MakeText(Activity, selectedCategoryName + "\nКатегория не содержит подкатегорий", ToastLength.Short).Show();
            }
        }

        public void NavigateUpHierarchyCat(int whereUp)
        {
            var filtrationFragment = (FiltrationFragment)FragmentManager.FindFragmentByTag("filtrationfragment");
            FiltrationNavList.GetFromCache(0, whereUp, out filtrationFragment.idFiltrationNavList, out filtrationFragment.textFiltrationNavList);
            //standardCategoriesAdapter.NavigateFilCategories(whereUp, this.FragmentManager);

            //Создаём объект адаптера получения данных для новой загрузки данных
            standardCategoriesAdapter = new CategoriesAdapter2(Activity)
            {
                //Создаём и устанавливаем объект контекста получения данных
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
    /// Класс контекста списка категорий
    /// </summary>
    public class CategoriesListContext
    {
        /// <summary>
        /// Корневой элемента списка
        /// </summary>
        public long RootId;

        /// <summary>
        /// Родительский элемента списка
        /// </summary>
        public long ParentId;
    }

    /// <summary>
    /// Адаптер получения списка категорий
    /// </summary>
    public class CategoriesAdapter2 : DataAdapter<CategoryShort, CategoriesListContext, Bitmap>
    {
        //Контекст работы адаптера
        readonly Activity _context;

        //Добавляется некий кэш
        public List<CategoryShort> cashitems = new List<CategoryShort>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public CategoriesAdapter2(Activity context)
        {
            //Сохраняем контекст
            _context = context;

        }

        /// <summary>
        /// Переопределяемый метод загрузки элементов - категорий
        /// </summary>
        /// <param name="position">Начальная позиция загружаемых элементов в источнике данных</param>
        /// <param name="count">Количество загружаемых элементов</param>
        /// <param name="items">Возвращаемый список элементов</param>
        /// <returns>Признак успешности загрузки</returns>
        public override bool LoadItems(int position, int count, out List<CategoryShort> items)
        {
            //Задаём начальные выходные значения
            items = null;

            //Результат операции
            bool result = true;

            //Получаемый массив событий
            CategoryShort[] categories;

            //Получаем события от сервиса
            result &= Service.GetCategoriesList(DataContext.ParentId, DataContext.RootId, position, count, out categories);
            //Если ошибка
            if (!result)
            {
                //Выходим с ошибкой
                return false;
            }

            //Возвращаем категории как список элементов
            items = categories.ToList();

            cashitems = items;

            //Возвращаем успех загрузки
            return true;
        }

        /// <summary>
        /// Переопределяемый метод загрузки одного элемента
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Объект, содержащиё загруженные данные</returns>
        public override bool LoadItem(CategoryShort item, out Bitmap extras)
        {
            //Задаём начальные выходные значения
            extras = null;

            //Результат операции
            bool result = true;

            //Если у события есть фото
            if (item.PrimaryPhotoId != null)
            {
                //Получаемый объект фото
                Bitmap photo;

                //Получаем картинку события от сервиса
                result &= Service.GetIcon(item.PrimaryPhotoId.Value, out photo, false);
                //Если ошибка
                if (!result)
                {
                    //Выходим с ошибкой
                    return false;
                }

                //Возвращаем фото как дополнительные данные
                extras = photo;
            }

            //Возвращаем успех загрузки
            return true;
        }

        /// <summary>
        /// Переопределяемый метод формирования начального вида элемента
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <param name="loadItemExtras">Запускать</param>
        /// <returns></returns>
        public override View GetInitialView(CategoryShort item, View convertView, ViewGroup parent, out bool loadItemExtras)
        {
            //Инициализируем выходные параметры
            loadItemExtras = true;

            //Переиспользуем компонент отображения или создаём новый
            View itemView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CategoriesList, null);

            //Отображаем данные элемента
            itemView.FindViewById<TextView>(Resource.Id.CategoryName).Text = item.Name;

            //Отображаем заглушку фото
            itemView.FindViewById<ImageView>(Resource.Id.ImageCategory).SetImageResource(Resource.Drawable.noimage);

            //Если у события есть фото
            if (item.PrimaryPhotoId != null)
            {
                //Получаемый объект фото
                Bitmap photo;

                //Пытаемся получить картинку от сервиса из кэша
                if (Service.GetIcon(item.PrimaryPhotoId.Value, out photo, true))
                {
                    //Если получилось
                    //Отображаем фото
                    itemView.FindViewById<ImageView>(Resource.Id.ImageCategory).SetImageBitmap(photo);
                    //Сбрасываем признак необходимости дозагрузки элемента
                    loadItemExtras = false;
                }
            }

            //Возвращаем компонент
            return itemView;
        }

        /// <summary>
        /// Переопределяемый метод формирования окончательного вида элемента
        /// </summary>
        /// <param name="item"></param>
        /// <param name="extras">Объект, возвращаемый методом LoadItem</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override void FormFinalView(CategoryShort item, Bitmap extras, View itemView)
        {
            //Формируем ссылку на объект фото
            Bitmap photo = extras;        //extras as Bitmap;

            //Если фото существует
            if (photo != null)
            {
                //Отображаем загруженное фото
            }
            else
            {
                //Отображаем картинку по умолчанию
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
