using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading.Tasks;

namespace AEC.Service
{
    /// <summary>
    /// Класс сервиса работы с сервером (часть интерфейса работы с данными)
    /// </summary>
    public partial class DataService : Android.App.Service
    {
        //Версия приложения
        static private Int64 currentVersionRootCatID = 30;     //30 = Экономфак ЮФУ
        static private Int64 currentVersionCatID = 33;         //33 = Темы категорий Экономфака ЮФУ (чтобы исключить альтернативные иерархии категорий)
        //Доступ к версии приложения
        static public Int64 whatVersionRootCatID { get { return currentVersionRootCatID; } }
        static public Int64 whatVersionCatID { get { return currentVersionCatID; } }

        //Идентификатор текущей сессии доступа к серверу
        public string SessionId = null;

        /// <summary>
        /// Начать сеанс доступа к серверу
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public bool Login(string login, string password, Captcha captcha)
        {
            //Результат операции
            bool result = true;

            //Входим в систему
            result &= UserAccess.Execute<string>(() => UserAccess.Client.Login("admin", "admin", captcha), out SessionId);

            //Возвращаем результат
            return result;
        }

        /// <summary>
        /// Завершить сеанс доступа к серверу
        /// </summary>
        public bool Logout()
        {
            //Результат операции
            bool result = true;

            //Выходим из системы
            result &= UserAccess.Execute(() => UserAccess.Client.Logout(SessionId));

            //Возвращаем результат
            return result;
        }

        /// <summary>
        /// Получение списка сообщений
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public bool GetEventsList(int pos, int count, out EventShort[] events)
        {
            //Задаём начальные выходные значения
            events = null;

            //Результат операции
            bool result = true;

            //Получаем события с сервера
            result &= UserAccess.Execute<EventShort[]>(() => UserAccess.Client.GetEventsList(SessionId, pos, count), out events);
            //Если ошибка
            if (!result)
            {
                //Выходим с ошибкой
                return false;
            }

            //Выходим с успехом
            return true;
        }

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="rootId"></param>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public bool GetCategoriesList(long parentId, long rootId, int pos, int count, out CategoryShort[] categories)
        {
            //Задаём начальные выходные значения
            categories = null;

            //Результат операции
            bool result = true;

            //Получаем события с сервера
            result &= UserAccess.Execute<CategoryShort[]>(() => UserAccess.Client.GetCategoriesListForCurrentAccount(SessionId, parentId, rootId, pos, count), out categories);
            //Если ошибка
            if (!result)
            {
                //Выходим с ошибкой
                return false;
            }

            //Выходим с успехом
            return true;
        }

        /// <summary>
        /// Получить данные о категории по ID категории
        /// </summary>
        /// <param name="whatVersionRootCatID">ID родительской категории искомой категории</param>
        /// <param name="whatVersionCatID">ID искомой категории</param>
        /// <returns></returns>
        public bool GetRootCategory(Int64 categoryId, out CategoryShort[] categories)
        {
            Int64 parentId = 30;
            Int64 rootId = whatVersionRootCatID;
            int pos = 1;
            int count = 100;

            //Задаём начальные выходные значения
            categories = null;

            //Результат операции
            bool result = true;

            //Получаем события с сервера
            result &= UserAccess.Execute<CategoryShort[]>(() => UserAccess.Client.GetCategoriesListForCurrentAccount(SessionId, parentId, rootId, pos, count), out categories);
                        //Если ошибка
            if (!result)
            {
                //Выходим с ошибкой
                return false;
            }

            //Выходим с успехом
            return true;
        }

        /// <summary>
        /// Получить фото по его Id
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="photo"></param>
        /// <param name="onlyInCache">Признак поиска только в кэше</param>
        /// <returns></returns>
        public bool GetIcon(long photoId, out Bitmap photo, bool onlyInCache)
        {
            //Задаём начальные выходные значения    
            photo = null;

            //Фото с сервера
            Photo serverPhoto;

            //Получаем фото с кэша
            if (!_iconsCache.Get(photoId, out serverPhoto, onlyInCache))
            {
                //Если не получилось
                //Выходим с неуспехом, передавая картинку по умолчанию

                return false;
            }

            //Преобразуем фото к удобному виду и возвращаем его
            photo = Android.Graphics.BitmapFactory.DecodeByteArray(serverPhoto.Data, 0, serverPhoto.Data.Length);

            //Выходим с успехом
            return true;
        }

        /// <summary>
        /// Получить полное описание события по его Id
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="photo"></param>
        /// <param name="onlyInCache">Признак поиска только в кэше</param>
        /// <returns></returns>
        public bool GetEventFull(long eventId, out EventFullGet eventFull, bool onlyInCache)
        {
            //Задаём начальные выходные значения
            eventFull = null;

            //Получаем фото с кэша
            if (!_eventsFullCache.Get(eventId, out eventFull, onlyInCache))
            {
                //Если не получилось
                //Выходим с неуспехом
                return false;
            }

            //Выходим с успехом
            return true;
        }
    }
}