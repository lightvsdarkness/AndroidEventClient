using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using AEC.Fragments;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using AEC.Service;
using System.Threading.Tasks;
using Android.Graphics;

namespace AEC
{
    [Activity(Label = "Куда сходить", Icon = "@drawable/Icon", Theme = "@style/Theme.PATheme")]                  //, ParentActivity = typeof(DrawerActivity)
    [MetaData("android.support.PARENT_ACTIVITY", Value = "aec.DrawerAct")]      //aec.LogInAct
    public class EventFullAct : Activity       //ActionBarActivity
    {
        bool iWillGoFirstTime = true;
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.OneEvent, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.FindItem(Resource.Id.action_websearch).SetVisible(true);
            return base.OnPrepareOptionsMenu(menu);
        }

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

        //Асинхронный метод загрузки данных
        protected async void Load(Int64 eventId)
        {
            //Результат загрузки
            bool result = true;

            //Полное описание события
            EventFullGet currEvent = null;

            //Запускаем загрузку полного описания события
            result &= await Task.FromResult<bool>((Application.Context as AECApplication).Service.GetEventFull(eventId, out currEvent, false));
            
            //Если ошибка при загрузке
            if (!result)
            {
                //Дальше ничего не делаем
                return;
            }

            SetContentView(Resource.Layout.EventFullDescription);
            ActionBar.Title = GetString(Resource.String.FullEventName);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            var fullEventNameText = FindViewById<TextView>(Resource.Id.FullEventName);
            fullEventNameText.Text = currEvent.Name;

            var fullEventDateText = FindViewById<TextView>(Resource.Id.FullEventDate);
            fullEventDateText.Text = currEvent.Date.ToString("dd.MM.yy");

            var fullEventOrganizerText = FindViewById<TextView>(Resource.Id.FullEventOrganizer);
            fullEventOrganizerText.Text = currEvent.Organizers[0];

            var fullEventAddressText = FindViewById<TextView>(Resource.Id.FullEventAddress);
            if (currEvent.Addresses != null && currEvent.Addresses.Count() != 0)
            {
                foreach (string address in currEvent.Addresses)
                {
                    fullEventAddressText.Text = address;       //currEvent.Addresses[address]
                }
            }

            var fullEventDescriptionText = FindViewById<TextView>(Resource.Id.FullEventDescription);
            //fullEventDescriptionText.DataDetectorTypes = UIDataDetectorType.Link;

            fullEventDescriptionText.TextFormatted = Html.FromHtml(currEvent.Description);
            fullEventDescriptionText.MovementMethod = LinkMovementMethod.Instance;

            Button buttonIllgo = FindViewById<Button>(Resource.Id.IllgoButton);
            buttonIllgo.Click += (o, e) =>
            {
                //StartActivity(typeof(MainMenuActivity));
                if (iWillGoFirstTime)
                {
                    if (WorkingWithAccount.SavingVisitEvent(eventId))
                    {
                        Toast.MakeText(this, GetString(Resource.String.InYourPlansAdded) + currEvent.Name, ToastLength.Long).Show();
                        buttonIllgo.Text = GetString(Resource.String.IllgoSecond);
                        iWillGoFirstTime = false;

                    }
                }
                else
                {
                    //Открыть Календарь с добавленным событием

                }

            };

            //Если для события не задана картинка
            if (!currEvent.PrimaryPhotoId.HasValue)
            {
                //Дальше ничего не делаем
                return;
            }

            //Картинка
            Bitmap image = null;

            //Запускаем загрузку картинки
            result &= await Task.FromResult<bool>((Application.Context as AECApplication).Service.GetIcon(currEvent.PrimaryPhotoId.Value, out image, false));

            //Если ошибка при загрузке
            if (!result)
            {
                //Дальше ничего не делаем
                return;
            }

            var imageEventImage = FindViewById<ImageView>(Resource.Id.ImageEvent);
            imageEventImage.SetImageBitmap(image);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Int64 eventId = Intent.GetLongExtra("Event", 0);

            //Загружаем и отображаем данные
            Load(eventId);
            //TO DO
            //Main photo with only eventId
            //List<Android.Graphics.Bitmap> q = check1.GetEventsPhotos(check1.GetEventShort(eventId));
            //var image = q[0];

        }
    }
}

