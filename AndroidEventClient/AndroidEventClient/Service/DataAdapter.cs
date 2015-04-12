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
using Android.Database;
using System.Threading.Tasks;
using System.Threading;
using AEC.Service;

namespace AEC.Service
{
    /// <summary>
    /// Класс адаптера данных для отображения подгружаемых списков
    /// </summary>
    /// <typeparam name="ELEMENT">Класс элемента</typeparam>
    /// <typeparam name="CONTEXT">Класс контекста получения данных</typeparam>
    /// <typeparam name="ELEMENT_DATA">Класс данных элемента</typeparam>
    public abstract class DataAdapter<ELEMENT, CONTEXT, ELEMENT_DATA> : BaseAdapter<ELEMENT>
        where ELEMENT : class
        where CONTEXT : class
        where ELEMENT_DATA : class
    {
        //Список отображаемых элементов
        List<ELEMENT> _items = new List<ELEMENT>();
        //Словарь загружаемых элементов
        Dictionary<View, ELEMENT> _itemsToLoad = new Dictionary<View, ELEMENT>();
        //Счётчик потребителей данных
        int _observersCount = 0;
        //Признак работы задачи загрузки отдельных элементов
        bool _loadItemsTaskIsRunning = false;

        /// <summary>
        /// Сервис для получения данных
        /// </summary>
        public DataService Service { get; set; }

        /// <summary>
        /// Контекст получения данных
        /// </summary>
        public CONTEXT DataContext { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataAdapter() : base()
        {
            //Формируем ссылку на сервис
            Service = (Application.Context as AECApplication).Service;
        }

        /// <summary>
        /// Метод получения идентификатора элемента
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            //Блокируем доступ
            lock (_items)
            {
                //Возвращаем идентификатор элемента
                return (Int64)typeof(ELEMENT).GetProperty("Id").GetValue(_items[position]);
            }
        }

        /// <summary>
        /// Индексатор списка элементов
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override ELEMENT this[int position]
        {
            get
            {
                //Блокируем доступ
                lock (_items)
                {
                    //Возвращаем элемент
                    return _items[position];
                }
            }
        }

        /// <summary>
        /// Количество элементов всписке
        /// </summary>
        public override int Count
        {
            get
            {
                //Блокируем доступ
                lock (_items)
                {
                    //Возвращаем элемент
                    return _items.Count;
                }
            }
        }

        /// <summary>
        /// Признак наличия идентификаторов у элементов
        /// </summary>
        public override bool HasStableIds
        {
	        get 
	        { 
                //Возвращаем признак наличия
		        return true;
	        }
        }

        /// <summary>
        /// Метод формирования вида элемента
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Обрабатываемый элемент
            ELEMENT item = this[position];

            //Возвращаемый вид элемента
            View itemView = null;

            //Признак необходимости дозагрузки элемента
            bool loadItemExtras;

            //Вызываем метод формирования начального вида элемента
            itemView = GetInitialView(item, convertView, parent, out loadItemExtras);

            //Если возвращен признак дозагрузки
            if (loadItemExtras)
            {
                //Блокируем доступ
                lock (_itemsToLoad)
                {
                    //Устанавливаем новый загружаемый элемент для компонента отображения
                    _itemsToLoad[itemView] = item;

                    //Если задача загрузки элементов не запущена
                    if (!_loadItemsTaskIsRunning)
                    {
                        //Устанавливаем признак работы задачи загрузки отдельных элементов
                        _loadItemsTaskIsRunning = true;

                        //Запускаем задачу загрузки элементов
                        ThreadPool.QueueUserWorkItem((a) =>
                        {
                            //Описатель элемента словаря загружаемых элементов
                            KeyValuePair<View, ELEMENT> pair;

                            while (true)
                            {
                                //Блокируем доступ
                                lock (_itemsToLoad)
                                {
                                    //Получаем очередной описатель из словаря
                                    pair = _itemsToLoad.FirstOrDefault();

                                    //Если элементов для загрузки нет
                                    if (pair.Key == null)
                                    {
                                        //Завершаем цикл загрузки
                                        break;
                                    }

                                    //Удаляем описатель из словаря
                                    _itemsToLoad.Remove(pair.Key);
                                }

                                //Дополнительные данные элемента
                                ELEMENT_DATA extras;

                                //Вызываем загрузку данных элемента
                                if (LoadItem(pair.Value, out extras))
                                {
                                    //Если успешно загрузили
                                    //Вызываем код в основном потоке
                                    Application.SynchronizationContext.Post((state) =>
                                    {
                                        //Вызываем метод формирования окончательного вида элемента
                                        FormFinalView(((KeyValuePair<View, ELEMENT>)state).Value, extras, ((KeyValuePair<View, ELEMENT>)state).Key);
                                    }, pair);
                                }
                                //Если была ошибка при загрузке
                                else
                                {
                                    //Сигнализируем потребителям о недоступности данных
                                    NotifyDataSetInvalidated();

                                    //Выходим из цикла загрузки
                                    break;
                                }
                            };

                            //Сбрасываем признак работы задачи загрузки отдельных элементов
                            _loadItemsTaskIsRunning = false;
                        });
                    }
                }
            }

            //Возвращаем созданный компонент пустого вида
            return itemView;
        }

        /// <summary>
        /// Добавление нового потребителя данных
        /// </summary>
        /// <param name="observer"></param>
        public override void RegisterDataSetObserver(DataSetObserver observer)
        {
            //Вызываем метод базового класса
            base.RegisterDataSetObserver(observer);

            //Увеличиваем счётчик потребителей
            _observersCount += 1;
            //Если подключается первый потребитель
            if (_observersCount == 1)
            {
                //Вызываем выполнение загрузки элементов в отдельном потоке
                ThreadPool.QueueUserWorkItem((a) =>
                {
                    //Список загруженных элементов
                    List<ELEMENT> items;

                    //Вызываем метод загрузки элементов
                    if (LoadItems(0, 100, out items))
                    {
                        //Если успешно загрузили
                        //Блокируем доступ
                        lock (_items)
                        {
                            //Добавляем новые элементы в общий список
                            _items.InsertRange(0, items);
                        }

                        //Вызываем код в основном потоке
                        Application.SynchronizationContext.Post((state) =>
                        {
                            //Сигнализируем потребителям об обновлении данных
                            NotifyDataSetChanged();
                        }, null);
                    }
                    //Если была ошибка при загрузке
                    else
                    {
                        //Вызываем код в основном потоке
                        Application.SynchronizationContext.Post((state) =>
                        {
                            //Сигнализируем потребителям о недоступности данных
                            NotifyDataSetInvalidated();
                        }, null);
                    }
                });
            }
        }

        /// <summary>
        /// Удаление потребителя данных
        /// </summary>
        /// <param name="observer"></param>
        public override void UnregisterDataSetObserver(DataSetObserver observer)
        {
            //Вызываем метод базового класса
            base.UnregisterDataSetObserver(observer);

            //Уменьшаем счётчик потребителей
            _observersCount -= 1;
            //Если все потребители отключились
            if (_observersCount == 0)
            {
                //Блокируем доступ
                lock (_items)
                {
                    //Блокируем доступ
                    lock (_itemsToLoad)
                    {
                        //Очищаем словарь элементов для загрузки
                        _itemsToLoad.Clear();
                    }

                    //Очищаем список отображаемых элементов
                    _items.Clear();
                }
            }
        }

        /// <summary>
        /// Переопределяемый метод загрузки элементов
        /// </summary>
        /// <param name="position">Начальная позиция загружаемых элементов в источнике данных</param>
        /// <param name="count">Количество загружаемых элементов</param>
        /// <param name="items">Возвращаемый список элементов</param>
        /// <returns>Признак успешности загрузки</returns>
        public abstract bool LoadItems(int position, int count, out List<ELEMENT> items);

        /// <summary>
        /// Переопределяемый метод загрузки одного элемента
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Объект, содержащиё загруженные данные</returns>
        public virtual bool LoadItem(ELEMENT item, out ELEMENT_DATA extras)
        {
            //Возвращаем отсутствие дополнительных данных
            extras = null;

            //Возвращаем успех загрузки
            return true;
        }

        /// <summary>
        /// Переопределяемый метод формирования начального вида элемента (вызывается, если LoadEachItem установлен)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <param name="loadItemExtras">Возвращаемый признак необходимости дозагрузки элемента (будет вызван LoadItem и FormFinalView), например, загрузки его из интернета, а не лишь из кэша</param>
        /// <returns></returns>
        public virtual View GetInitialView(ELEMENT item, View convertView, ViewGroup parent, out bool loadItemExtras)
        {
            //Инициализируем выходные параметры
            loadItemExtras = false;

            //Возвращаем пустышку
            return null;
        }

        /// <summary>
        /// Переопределяемый метод формирования окончательного вида элемента
        /// </summary>
        /// <param name="item"></param>
        /// <param name="extras">Объект, возвращаемый методом LoadItem</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual void FormFinalView(ELEMENT item, ELEMENT_DATA extras, View itemView)
        {
        }
    }
}