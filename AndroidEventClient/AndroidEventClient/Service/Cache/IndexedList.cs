using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    public class IndexedList<T> : IEnumerable<T>
    {
        //Массив элементов списка
        IndexedListElement[] _elements = null;
        //Индекс первого добавленного элемента
        int _firstElementIndex = -1;
        //Индекс последнего добавленного элемента
        int _lastElementIndex = -1;
        //Индекс первого пустого элемента
        int _firstFreeIndex = -1;
        //Индекс последнего пустого элемента
        int _lastFreeIndex = -1;
        //Количество непустых элементов
        int _count = 0;

        /// <summary>
        /// Количество объектов в списке
        /// </summary>
        public int Count 
        {
            get
            {
                //Возвращаем количество непустых элементов
                return _count;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="initialListSize">Начальный размер списка</param>
        public IndexedList(int initialListSize)
        {
            //Создаём массив элементов списка
            _elements = new IndexedListElement[initialListSize];

            //Инициализируем его
            FillFree(0, _elements.Length - 1, -1);
        }

        /// <summary>
        /// Инициализировать часть массива
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="lastFreeIndex">Индекс последнего пустого элемента в уже имеющемся массиве</param>
        protected void FillFree(int startIndex, int endIndex, int lastFreeIndex)
        {
            //Если индекc последнего пустого задан
            if (lastFreeIndex != -1)
            {
                //Корректируем последний пустой
                _elements[lastFreeIndex].NextIndex = startIndex;
            }
            //Если не задан
            else
            {
                //Устанавливаем новые индекс начала и конца списка пустых элементов
                _lastFreeIndex = endIndex;
                _firstFreeIndex = startIndex;
            }

            //Устанавливаем ссылку на следующий элемент
            _elements[endIndex].NextIndex = -1;
            //Формируем данные
            _elements[endIndex].Data = default(T);
            _elements[endIndex].IsPresent = false;

            //Если количество элементов в массиве больше 1
            if (_elements.Length > 1)
            {
                //Цикл по всем элементам массива
                do
                {
                    //Устанавливаем ссылку на предыдущий элемент
                    _elements[endIndex].PrevIndex = endIndex - 1;

                    //Уменьшаем индекс первого пустого элемента
                    endIndex -= 1;

                    //Устанавливаем ссылку на следующий элемент
                    _elements[endIndex].NextIndex = endIndex + 1;
                    //Формируем данные
                    _elements[endIndex].Data = default(T);
                    _elements[endIndex].IsPresent = false;

                } while (endIndex > startIndex);
            }

            //Устанавливаем ссылку на предыдущий элемент
            _elements[endIndex].PrevIndex = lastFreeIndex;
        }

        /// <summary>
        /// Очистить список
        /// </summary>
        public void Clear()
        {
            //Обнуляем количество непустых элементов
            _count = 0;
            //Устанавливаем индекс первого добавленного элемента
            _firstElementIndex = -1;
            //Устанавливаем индекс последнего добавленного элемента
            _lastElementIndex = -1;

            //Инициализируем массив пустыми элементами
            FillFree(0, _elements.Length, -1);
        }

        /// <summary>
        /// Добавить объект в список
        /// </summary>
        /// <param name="obj">Добавляемый объект</param>
        /// <returns>Индекс добавленного элемента</returns>
        public int Add(T obj)
        {
            //Если массив заполнен
            if (_lastFreeIndex == -1)
            {
                //Создаём новый массив удвоенного размера
                IndexedListElement[] elements = new IndexedListElement[_elements.Length * 2];
                //Копируем данные из старого массива
                Array.Copy(_elements, 0, elements, 0, _elements.Length);
                //Инициализируем добавленную часть
                FillFree(_elements.Length, elements.Length - 1, _lastFreeIndex);
                //Сохраняем вновь созданный массив как рабочий
                _elements = elements;
            }

            //Сохраняем индекс следующего пустого элемента
            int firstFreeIndex = _elements[_firstFreeIndex].NextIndex;
            //Добавляем элемент в список
            _elements[_firstFreeIndex].PrevIndex = _lastElementIndex;
            _elements[_firstFreeIndex].NextIndex = -1;
            _elements[_firstFreeIndex].Data = obj;
            _elements[_firstFreeIndex].IsPresent = true;
            //Корректируем индексы начала и конца списков
            _firstElementIndex = _firstElementIndex == -1 ? _firstFreeIndex : _firstElementIndex;
            _lastElementIndex = _firstFreeIndex;
            _firstFreeIndex = firstFreeIndex;
            _lastFreeIndex = _firstFreeIndex == -1 ? _firstFreeIndex : _lastFreeIndex;

            //Увеличиваем количество непустых элементов
            _count += 1;

            //Возвращаем индекс добавленного элемента
            return _lastElementIndex;
        }

        /// <summary>
        /// Удалить объект из списка
        /// </summary>
        /// <param name="index">Индекс объекта</param>
        public void Remove(int index)
        {
            //Если индекс некорректный
            if (index < 0 || index >= _elements.Length)
            {
                //Создаём исключение
                throw new IndexOutOfRangeException();
            }

            //Если элемент уже пустой
            if (!_elements[index].IsPresent)
            {
                //Создаём исключение
                throw new IndexOutOfRangeException();
            }

            //Уменьшаем количество непустых элементов в списке
            _count -= 1;

            //Если у элемента имеется предыдущий
            if (_elements[index].PrevIndex != -1)
            {
                //Корректируем предыдущий элемент
                _elements[_elements[index].PrevIndex].NextIndex = _elements[index].NextIndex;
            }
            //Если нет
            else
            {
                //Корректируем индекс первого непустого элемента
                _firstElementIndex = _elements[index].NextIndex;
            }

            //Если у элемента имеется следующий
            if (_elements[index].NextIndex != -1)
            {
                //Корректируем следующий элемент
                _elements[_elements[index].NextIndex].PrevIndex = _elements[index].PrevIndex;
            }
            //Если нет
            else
            {
                //Корректируем индекс последнего непустого элемента
                _lastElementIndex = _elements[index].PrevIndex;
            }

            //Корректируем удаляемый элемент
            _elements[index].Data = default(T);
            _elements[index].IsPresent = false;
            _elements[index].NextIndex = -1;
            _elements[index].PrevIndex = _lastFreeIndex;
            //Корректируем индекс последнего непустого элемента
            _lastFreeIndex = index;
        }

        /// <summary>
        /// Извлечь первый элемент в списке
        /// </summary>
        /// <returns></returns>
        public bool PopFirst(out T obj)
        {
            //Инициализируем выходные параметры
            obj = default(T);

            //Если список пуст
            if (_firstElementIndex == -1)
            {
                //Возвращаем неуспех
                return false;
            }

            //Возвращаем объект
            obj = _elements[_firstElementIndex].Data;

            //Удаляем его из списка
            Remove(_firstElementIndex);

            //Возвращаем успех
            return true;
        }

        /// <summary>
        /// Извлечь последний элемент в списке
        /// </summary>
        /// <returns></returns>
        public bool PopLast(out T obj)
        {
            //Инициализируем выходные параметры
            obj = default(T);

            //Если список пуст
            if (_lastElementIndex == -1)
            {
                //Возвращаем неуспех
                return false;
            }

            //Возвращаем объект
            obj = _elements[_lastElementIndex].Data;

            //Удаляем его из списка
            Remove(_lastElementIndex);

            //Возвращаем успех
            return true;
        }

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="index">Индекс объекта</param>
        /// <returns>Объект с заданным индексом</returns>
        public T this[int index] 
        {
            get
            {
                //Если индекс некорректный
                if (index < 0 || index >= _elements.Length)
                {
                    //Создаём исключение
                    throw new IndexOutOfRangeException();
                }

                //Возвращаем объект
                return _elements[index].Data;
            }
        }

        #region Реализация IEnumerable<T>

		IEnumerator<T> IEnumerable<T>.GetEnumerator() 
        { 
            return new IndexedListEnumerator<T>(this); 
		}

		IEnumerator IEnumerable.GetEnumerator()
        {
			return new IndexedListEnumerator<T>(this); 
		}

        protected class IndexedListEnumerator<T> : IEnumerator<T>
        {
            private int _currentIndex;
            IndexedList<T> _list;

            public IndexedListEnumerator(IndexedList<T> list)
            {
                _list = list;
                _currentIndex = -2;
            }

            public T Current
            {
                get
                {
                    if (_currentIndex < 0) return default(T);
                    return _list[_currentIndex];
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return _list[_currentIndex];
                }
            }

            public void Reset() { _currentIndex = -2; }
            public bool MoveNext()
            {
                if (_currentIndex == -2)
                {
                    _currentIndex = _list._firstElementIndex;
                }

                if (_currentIndex == -1)
                {
                    return false;
                }

                _currentIndex = _list._elements[_list._firstElementIndex].NextIndex;

                return true;
            }
            #region IDisposable Members
            public void Dispose() { return; }
            #endregion
        }

        #endregion Реализация IEnumerable<T>

        protected struct IndexedListElement
        {
            //Индекс предыдущего элемента
            public int PrevIndex;
            //Индекс следующего элемента
            public int NextIndex;
            //Сам объект
            public T Data;
            //Признак наличия элемента
            public bool IsPresent;
        }
	}
}