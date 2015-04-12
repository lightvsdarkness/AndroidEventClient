using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    public class IndexedList<T> : IEnumerable<T>
    {
        //������ ��������� ������
        IndexedListElement[] _elements = null;
        //������ ������� ������������ ��������
        int _firstElementIndex = -1;
        //������ ���������� ������������ ��������
        int _lastElementIndex = -1;
        //������ ������� ������� ��������
        int _firstFreeIndex = -1;
        //������ ���������� ������� ��������
        int _lastFreeIndex = -1;
        //���������� �������� ���������
        int _count = 0;

        /// <summary>
        /// ���������� �������� � ������
        /// </summary>
        public int Count 
        {
            get
            {
                //���������� ���������� �������� ���������
                return _count;
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="initialListSize">��������� ������ ������</param>
        public IndexedList(int initialListSize)
        {
            //������ ������ ��������� ������
            _elements = new IndexedListElement[initialListSize];

            //�������������� ���
            FillFree(0, _elements.Length - 1, -1);
        }

        /// <summary>
        /// ���������������� ����� �������
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="lastFreeIndex">������ ���������� ������� �������� � ��� ��������� �������</param>
        protected void FillFree(int startIndex, int endIndex, int lastFreeIndex)
        {
            //���� �����c ���������� ������� �����
            if (lastFreeIndex != -1)
            {
                //������������ ��������� ������
                _elements[lastFreeIndex].NextIndex = startIndex;
            }
            //���� �� �����
            else
            {
                //������������� ����� ������ ������ � ����� ������ ������ ���������
                _lastFreeIndex = endIndex;
                _firstFreeIndex = startIndex;
            }

            //������������� ������ �� ��������� �������
            _elements[endIndex].NextIndex = -1;
            //��������� ������
            _elements[endIndex].Data = default(T);
            _elements[endIndex].IsPresent = false;

            //���� ���������� ��������� � ������� ������ 1
            if (_elements.Length > 1)
            {
                //���� �� ���� ��������� �������
                do
                {
                    //������������� ������ �� ���������� �������
                    _elements[endIndex].PrevIndex = endIndex - 1;

                    //��������� ������ ������� ������� ��������
                    endIndex -= 1;

                    //������������� ������ �� ��������� �������
                    _elements[endIndex].NextIndex = endIndex + 1;
                    //��������� ������
                    _elements[endIndex].Data = default(T);
                    _elements[endIndex].IsPresent = false;

                } while (endIndex > startIndex);
            }

            //������������� ������ �� ���������� �������
            _elements[endIndex].PrevIndex = lastFreeIndex;
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        public void Clear()
        {
            //�������� ���������� �������� ���������
            _count = 0;
            //������������� ������ ������� ������������ ��������
            _firstElementIndex = -1;
            //������������� ������ ���������� ������������ ��������
            _lastElementIndex = -1;

            //�������������� ������ ������� ����������
            FillFree(0, _elements.Length, -1);
        }

        /// <summary>
        /// �������� ������ � ������
        /// </summary>
        /// <param name="obj">����������� ������</param>
        /// <returns>������ ������������ ��������</returns>
        public int Add(T obj)
        {
            //���� ������ ��������
            if (_lastFreeIndex == -1)
            {
                //������ ����� ������ ���������� �������
                IndexedListElement[] elements = new IndexedListElement[_elements.Length * 2];
                //�������� ������ �� ������� �������
                Array.Copy(_elements, 0, elements, 0, _elements.Length);
                //�������������� ����������� �����
                FillFree(_elements.Length, elements.Length - 1, _lastFreeIndex);
                //��������� ����� ��������� ������ ��� �������
                _elements = elements;
            }

            //��������� ������ ���������� ������� ��������
            int firstFreeIndex = _elements[_firstFreeIndex].NextIndex;
            //��������� ������� � ������
            _elements[_firstFreeIndex].PrevIndex = _lastElementIndex;
            _elements[_firstFreeIndex].NextIndex = -1;
            _elements[_firstFreeIndex].Data = obj;
            _elements[_firstFreeIndex].IsPresent = true;
            //������������ ������� ������ � ����� �������
            _firstElementIndex = _firstElementIndex == -1 ? _firstFreeIndex : _firstElementIndex;
            _lastElementIndex = _firstFreeIndex;
            _firstFreeIndex = firstFreeIndex;
            _lastFreeIndex = _firstFreeIndex == -1 ? _firstFreeIndex : _lastFreeIndex;

            //����������� ���������� �������� ���������
            _count += 1;

            //���������� ������ ������������ ��������
            return _lastElementIndex;
        }

        /// <summary>
        /// ������� ������ �� ������
        /// </summary>
        /// <param name="index">������ �������</param>
        public void Remove(int index)
        {
            //���� ������ ������������
            if (index < 0 || index >= _elements.Length)
            {
                //������ ����������
                throw new IndexOutOfRangeException();
            }

            //���� ������� ��� ������
            if (!_elements[index].IsPresent)
            {
                //������ ����������
                throw new IndexOutOfRangeException();
            }

            //��������� ���������� �������� ��������� � ������
            _count -= 1;

            //���� � �������� ������� ����������
            if (_elements[index].PrevIndex != -1)
            {
                //������������ ���������� �������
                _elements[_elements[index].PrevIndex].NextIndex = _elements[index].NextIndex;
            }
            //���� ���
            else
            {
                //������������ ������ ������� ��������� ��������
                _firstElementIndex = _elements[index].NextIndex;
            }

            //���� � �������� ������� ���������
            if (_elements[index].NextIndex != -1)
            {
                //������������ ��������� �������
                _elements[_elements[index].NextIndex].PrevIndex = _elements[index].PrevIndex;
            }
            //���� ���
            else
            {
                //������������ ������ ���������� ��������� ��������
                _lastElementIndex = _elements[index].PrevIndex;
            }

            //������������ ��������� �������
            _elements[index].Data = default(T);
            _elements[index].IsPresent = false;
            _elements[index].NextIndex = -1;
            _elements[index].PrevIndex = _lastFreeIndex;
            //������������ ������ ���������� ��������� ��������
            _lastFreeIndex = index;
        }

        /// <summary>
        /// ������� ������ ������� � ������
        /// </summary>
        /// <returns></returns>
        public bool PopFirst(out T obj)
        {
            //�������������� �������� ���������
            obj = default(T);

            //���� ������ ����
            if (_firstElementIndex == -1)
            {
                //���������� �������
                return false;
            }

            //���������� ������
            obj = _elements[_firstElementIndex].Data;

            //������� ��� �� ������
            Remove(_firstElementIndex);

            //���������� �����
            return true;
        }

        /// <summary>
        /// ������� ��������� ������� � ������
        /// </summary>
        /// <returns></returns>
        public bool PopLast(out T obj)
        {
            //�������������� �������� ���������
            obj = default(T);

            //���� ������ ����
            if (_lastElementIndex == -1)
            {
                //���������� �������
                return false;
            }

            //���������� ������
            obj = _elements[_lastElementIndex].Data;

            //������� ��� �� ������
            Remove(_lastElementIndex);

            //���������� �����
            return true;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="index">������ �������</param>
        /// <returns>������ � �������� ��������</returns>
        public T this[int index] 
        {
            get
            {
                //���� ������ ������������
                if (index < 0 || index >= _elements.Length)
                {
                    //������ ����������
                    throw new IndexOutOfRangeException();
                }

                //���������� ������
                return _elements[index].Data;
            }
        }

        #region ���������� IEnumerable<T>

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

        #endregion ���������� IEnumerable<T>

        protected struct IndexedListElement
        {
            //������ ����������� ��������
            public int PrevIndex;
            //������ ���������� ��������
            public int NextIndex;
            //��� ������
            public T Data;
            //������� ������� ��������
            public bool IsPresent;
        }
	}
}