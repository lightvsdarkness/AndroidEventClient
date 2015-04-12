using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AEC.Service
{
    /// <summary>
    /// Класс кэша полных описаний событий
    /// </summary>
    public class EventFullCache : SingleCacheBase<EventFullGet>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Объект сервиса получения данных</param>
        /// <param name="maxCacheCount">Максимально допустимое количество объектов в кэше (0 - проверка не осуществляется)</param>
        /// <param name="maxCacheSize">Максимально допустимый общий размер кэша (0 - проверка не осуществляется)</param>
        /// <param name="maxObjectSize">Максимально допустимый размер одного объекта (0 - проверка не осуществляется)</param>
        public EventFullCache(DataService service, int maxCacheCount = 0, int maxCacheSize = 0, int maxObjectSize = 0) :
            base(service, maxCacheCount, maxCacheSize, maxObjectSize)
        {
        }

        /// <summary>
        /// Переопределяемый метод получения объекта из БД
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Возвращаемый объект</param>
        /// <returns>Признак успешного нахождени в БД</returns>
        protected override bool GetFromDB(Int64 id, out EventFullGet obj)
        {
            //Инициализируем выходные параметры
            obj = null;

            //По умолчанию возвращаем неуспех
            return false;
        }

        /// <summary>
        /// Сохранить объект в БД
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Сохраняемый объект</param>
        protected override void SaveToDB(Int64 id, ref EventFullGet obj)
        {
        }

        /// <summary>
        /// Переопределяемый метод получения объекта с сервера
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Возвращаемый объект</param>
        /// <returns>Признак успешного нахождени на сервере</returns>
        protected override bool GetFromServer(Int64 id, out EventFullGet obj)
        {
            //Получаем фото с сервера
            return UserAccess.Execute<EventFullGet>(() => UserAccess.Client.GetEventFullDescription(_service.SessionId, id), out obj);
        }

        /// <summary>
        /// Переопределяемый метод получения приблизительного размера объекта 
        /// </summary>
        /// <param name="obj">Оцениваемый объект</param>
        /// <returns>Приблизительный размер</returns>
        protected override int GetObjectSize(ref EventFullGet obj)
        {
            //Возвращаем размер
            return obj.Description.Length;
        }
    }
}