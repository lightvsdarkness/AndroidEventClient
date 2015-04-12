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
using System.Collections;

namespace AEC.Service
{
    /// <summary>
    /// Базовый класс кэша для одиночных объектов
    /// </summary>
    public abstract class SingleCacheBase<T> 
    {
        //Минимальное количество объектов в кэше
        protected const int MIN_CACHE_COUNT = 256;

        //Объект сервиса
        protected DataService _service;
        //Максимально допустимое количество объектов в кэше
        protected int _maxCacheCount = 0;
        //Максимально допустимый общий размер кэша
        protected int _maxCacheSize = 0;
        //Максимально допустимый размер одного объекта
        protected int _maxObjectSize = 0;
        //Текущий размер кэша
        protected int _currentCacheSize = 0;

        //Словарь поиска объектов по id и их хранения (int - индекс в списке истории объектов)
        protected Dictionary<Int64, CachedObject> _objects = null;
        //Список истории объектов
        protected IndexedList<Int64> _objectsHistory = null;

        #region Переопределяемые методы

        /// <summary>
        /// Переопределяемый метод получения объекта из БД
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Возвращаемый объект</param>
        /// <returns>Признак успешного нахождени в БД</returns>
        protected virtual bool GetFromDB(Int64 id, out T obj)
        {
            //Инициализируем выходные параметры
            obj = default(T);

            //По умолчанию возвращаем неуспех
            return false;
        }

        /// <summary>
        /// Сохранить объект в БД
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Сохраняемый объект</param>
        protected virtual void SaveToDB(Int64 id, ref T obj)
        {
        }

        /// <summary>
        /// Переопределяемый метод получения объекта с сервера
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Возвращаемый объект</param>
        /// <returns>Признак успешного нахождени на сервере</returns>
        protected abstract bool GetFromServer(Int64 id, out T obj);

        /// <summary>
        /// Переопределяемый метод получения приблизительного размера объекта 
        /// </summary>
        /// <param name="obj">Оцениваемый объект</param>
        /// <returns>Приблизительный размер</returns>
        protected virtual int GetObjectSize(ref T obj)
        {
            //Возвращаем пустышку
            return 0;
        }

        #endregion Переопределяемые методы

        #region Свойства

        /// <summary>
        /// Текущее количество объектов в кэше
        /// </summary>
        public int Count
        {
            get
            {
                //Возвращаем количество объектов
                return _objectsHistory.Count;
            }
        }

        /// <summary>
        /// Текущий размер кэша
        /// </summary>
        public int Size
        {
            get
            {
                //Возвращаем текущий размер
                return _currentCacheSize;
            }
        }

        #endregion Свойства

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Объект сервиса получения данных</param>
        /// <param name="maxCacheCount">Максимально допустимое количество объектов в кэше (0 - проверка не осуществляется)</param>
        /// <param name="maxCacheSize">Максимально допустимый общий размер кэша (0 - проверка не осуществляется)</param>
        /// <param name="maxObjectSize">Максимально допустимый размер одного объекта (0 - проверка не осуществляется)</param>
        public SingleCacheBase(DataService service, int maxCacheCount = 0, int maxCacheSize = 0, int maxObjectSize = 0)
        {
            //Сохраняем параметры инициализации
            _service = service;
            _maxCacheCount = maxCacheCount;
            _maxCacheSize = maxCacheSize;
            _maxObjectSize = maxObjectSize;

            //Очищаем кэш
            Clear();
        }

        /// <summary>
        /// Очистить кэш
        /// </summary>
        public void Clear()
        {
            //Обнуляем текущий размер кэша
            _currentCacheSize = 0;

            //Создаём словарь поиска объектов по id и их хранения
            _objects = new Dictionary<Int64, CachedObject>(MIN_CACHE_COUNT);
            //Создаём список истории объектов
            _objectsHistory = new IndexedList<Int64>(MIN_CACHE_COUNT);
        }

        /// <summary>
        /// Метод получения объекта по его id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="obj">Возвращаемый объект</param>
        /// <param name="onlyInCache">Признак поиска только в кэше</param>
        /// <returns>Признак успешного нахождени</returns>
        public bool Get(Int64 id, out T obj, bool onlyInCache)
        {
            //Инициализируем выходные параметры
            obj = default(T);

            //Результат работы
            bool result = false;
            //Признак отсутствия объекта в БД
            bool notFoundInDB = false;

            //Структура описания кешируемого объекта
            CachedObject cachedObj;

            //Цикл поиска объекта
            while (true)
            {
                //Пытаемся найти объект в кэше
                result |= _objects.TryGetValue(id, out cachedObj);

                //Если нашли
                if (result)
                {
                    //Возвращаем объект
                    obj = cachedObj.Object;
                    //Обновляем историю объекта
                    _objectsHistory.Remove(cachedObj.HistoryIndex);
                    cachedObj.HistoryIndex = _objectsHistory.Add(id);

                    //Возвращаем успех
                    return true;
                }

                //Если установлен признак поиска только в кэше
                if (onlyInCache)
                {
                    //Возвращаем результат
                    return result;
                }

                //Пытаемся найти объект в БД
                result |= GetFromDB(id, out obj);

                //Если нашли
                if (result)
                {
                    //Выходим из цикла поиска
                    break;
                }
                //Если не нашли
                else
                {
                    //Устанавлвиаем признак отсутствия объекта в БД
                    notFoundInDB = true;
                }

                //Пытаемся найти объект на сервере
                result |= GetFromServer(id, out obj);

                //Если нашли
                if (result)
                {
                    //Выходим из цикла поиска
                    break;
                }
                //Если не нашли
                else
                {
                    //Возвращаем неуспех
                    return false;
                }
            }

            //Если не нашли в БД
            if (notFoundInDB)
            {
                //Сохраняем объект в БД
                SaveToDB(id, ref obj);
            }

            //Если максимальное количество объектов в кэше задано
            if (_maxCacheCount != 0)
            {
                //Если кэш переполнен
                if (_objectsHistory.Count >= _maxCacheCount)
                {
                    //Идентификатор самого неиспользуемого объекта
                    Int64 localId;

                    //Извлекаем идентификатор самого неиспользуемого объекта из истории
                    if (!_objectsHistory.PopFirst(out localId))
                    {
                        //Если что-то не так
                        //Создаём исключение
                        throw new IndexOutOfRangeException("SingleCacheBase::Get - попытка извлечь из пустой истории!");
                    }

                    //Удаляем объект из кэша
                    _objects.Remove(localId);
                }
            }

            //Размер объекта
            int objectSize = 0;

            //Если максимальный размер кэша или объекта задан
            if (_maxCacheSize != 0 || _maxObjectSize != 0)
            {
                //Получаем размер объекта
                objectSize = GetObjectSize(ref obj);
                //Если размер объекта превышает максимально допустимый
                if (objectSize > _maxObjectSize || objectSize > _maxCacheSize)
                {
                    //Возвращаем успех (при этом не кешируем объект)
                    return true;
                }

                //Пока в кэше нет места для объекта
                while (objectSize + _currentCacheSize > _maxCacheSize)
                {
                    //Идентификатор самого неиспользуемого объекта
                    Int64 localId;

                    //Извлекаем идентификатор самого неиспользуемого объекта из истории
                    if (!_objectsHistory.PopFirst(out localId))
                    {
                        //Если что-то не так
                        //Создаём исключение
                        throw new InvalidOperationException("SingleCacheBase::Get - попытка извлечь из пустой истории!");
                    }

                    //Получаем описание объекта
                    if (!_objects.TryGetValue(localId, out cachedObj))
                    {
                        //Если что-то не так
                        //Создаём исключение
                        throw new InvalidOperationException("SingleCacheBase::Get - объект не найден в кэше!");
                    }

                    //Уменьшаем размер кеша
                    _currentCacheSize -= cachedObj.ObjectSize;

                    //Удаляем объект из кэша
                    _objects.Remove(localId);
                }
            }

            //Создаём новое описание кешируемого объекта
            cachedObj = new CachedObject();
            //Заполняем его
            cachedObj.Object = obj;
            cachedObj.ObjectSize = objectSize;
            //Добавляем объект в список истории использования
            cachedObj.HistoryIndex = _objectsHistory.Add(id);
            //Сохраняем объект в кэш
            _objects.Add(id, cachedObj);

            //Возвращаем успех
            return true;
        }

        /// <summary>
        /// Класс описания кешируемого объекта
        /// </summary>
        protected class CachedObject
        {
            //Сам объект
            public T Object;
            //Индекс объекта в списке истории использования
            public int HistoryIndex;
            //Размер объекта
            public int ObjectSize;
        }
    }
}