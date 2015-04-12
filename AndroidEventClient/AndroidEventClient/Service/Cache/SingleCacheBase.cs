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
    /// ������� ����� ���� ��� ��������� ��������
    /// </summary>
    public abstract class SingleCacheBase<T> 
    {
        //����������� ���������� �������� � ����
        protected const int MIN_CACHE_COUNT = 256;

        //������ �������
        protected DataService _service;
        //����������� ���������� ���������� �������� � ����
        protected int _maxCacheCount = 0;
        //����������� ���������� ����� ������ ����
        protected int _maxCacheSize = 0;
        //����������� ���������� ������ ������ �������
        protected int _maxObjectSize = 0;
        //������� ������ ����
        protected int _currentCacheSize = 0;

        //������� ������ �������� �� id � �� �������� (int - ������ � ������ ������� ��������)
        protected Dictionary<Int64, CachedObject> _objects = null;
        //������ ������� ��������
        protected IndexedList<Int64> _objectsHistory = null;

        #region ���������������� ������

        /// <summary>
        /// ���������������� ����� ��������� ������� �� ��
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">������������ ������</param>
        /// <returns>������� ��������� ��������� � ��</returns>
        protected virtual bool GetFromDB(Int64 id, out T obj)
        {
            //�������������� �������� ���������
            obj = default(T);

            //�� ��������� ���������� �������
            return false;
        }

        /// <summary>
        /// ��������� ������ � ��
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">����������� ������</param>
        protected virtual void SaveToDB(Int64 id, ref T obj)
        {
        }

        /// <summary>
        /// ���������������� ����� ��������� ������� � �������
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">������������ ������</param>
        /// <returns>������� ��������� ��������� �� �������</returns>
        protected abstract bool GetFromServer(Int64 id, out T obj);

        /// <summary>
        /// ���������������� ����� ��������� ���������������� ������� ������� 
        /// </summary>
        /// <param name="obj">����������� ������</param>
        /// <returns>��������������� ������</returns>
        protected virtual int GetObjectSize(ref T obj)
        {
            //���������� ��������
            return 0;
        }

        #endregion ���������������� ������

        #region ��������

        /// <summary>
        /// ������� ���������� �������� � ����
        /// </summary>
        public int Count
        {
            get
            {
                //���������� ���������� ��������
                return _objectsHistory.Count;
            }
        }

        /// <summary>
        /// ������� ������ ����
        /// </summary>
        public int Size
        {
            get
            {
                //���������� ������� ������
                return _currentCacheSize;
            }
        }

        #endregion ��������

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="service">������ ������� ��������� ������</param>
        /// <param name="maxCacheCount">����������� ���������� ���������� �������� � ���� (0 - �������� �� ��������������)</param>
        /// <param name="maxCacheSize">����������� ���������� ����� ������ ���� (0 - �������� �� ��������������)</param>
        /// <param name="maxObjectSize">����������� ���������� ������ ������ ������� (0 - �������� �� ��������������)</param>
        public SingleCacheBase(DataService service, int maxCacheCount = 0, int maxCacheSize = 0, int maxObjectSize = 0)
        {
            //��������� ��������� �������������
            _service = service;
            _maxCacheCount = maxCacheCount;
            _maxCacheSize = maxCacheSize;
            _maxObjectSize = maxObjectSize;

            //������� ���
            Clear();
        }

        /// <summary>
        /// �������� ���
        /// </summary>
        public void Clear()
        {
            //�������� ������� ������ ����
            _currentCacheSize = 0;

            //������ ������� ������ �������� �� id � �� ��������
            _objects = new Dictionary<Int64, CachedObject>(MIN_CACHE_COUNT);
            //������ ������ ������� ��������
            _objectsHistory = new IndexedList<Int64>(MIN_CACHE_COUNT);
        }

        /// <summary>
        /// ����� ��������� ������� �� ��� id
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">������������ ������</param>
        /// <param name="onlyInCache">������� ������ ������ � ����</param>
        /// <returns>������� ��������� ���������</returns>
        public bool Get(Int64 id, out T obj, bool onlyInCache)
        {
            //�������������� �������� ���������
            obj = default(T);

            //��������� ������
            bool result = false;
            //������� ���������� ������� � ��
            bool notFoundInDB = false;

            //��������� �������� ����������� �������
            CachedObject cachedObj;

            //���� ������ �������
            while (true)
            {
                //�������� ����� ������ � ����
                result |= _objects.TryGetValue(id, out cachedObj);

                //���� �����
                if (result)
                {
                    //���������� ������
                    obj = cachedObj.Object;
                    //��������� ������� �������
                    _objectsHistory.Remove(cachedObj.HistoryIndex);
                    cachedObj.HistoryIndex = _objectsHistory.Add(id);

                    //���������� �����
                    return true;
                }

                //���� ���������� ������� ������ ������ � ����
                if (onlyInCache)
                {
                    //���������� ���������
                    return result;
                }

                //�������� ����� ������ � ��
                result |= GetFromDB(id, out obj);

                //���� �����
                if (result)
                {
                    //������� �� ����� ������
                    break;
                }
                //���� �� �����
                else
                {
                    //������������� ������� ���������� ������� � ��
                    notFoundInDB = true;
                }

                //�������� ����� ������ �� �������
                result |= GetFromServer(id, out obj);

                //���� �����
                if (result)
                {
                    //������� �� ����� ������
                    break;
                }
                //���� �� �����
                else
                {
                    //���������� �������
                    return false;
                }
            }

            //���� �� ����� � ��
            if (notFoundInDB)
            {
                //��������� ������ � ��
                SaveToDB(id, ref obj);
            }

            //���� ������������ ���������� �������� � ���� ������
            if (_maxCacheCount != 0)
            {
                //���� ��� ����������
                if (_objectsHistory.Count >= _maxCacheCount)
                {
                    //������������� ������ ��������������� �������
                    Int64 localId;

                    //��������� ������������� ������ ��������������� ������� �� �������
                    if (!_objectsHistory.PopFirst(out localId))
                    {
                        //���� ���-�� �� ���
                        //������ ����������
                        throw new IndexOutOfRangeException("SingleCacheBase::Get - ������� ������� �� ������ �������!");
                    }

                    //������� ������ �� ����
                    _objects.Remove(localId);
                }
            }

            //������ �������
            int objectSize = 0;

            //���� ������������ ������ ���� ��� ������� �����
            if (_maxCacheSize != 0 || _maxObjectSize != 0)
            {
                //�������� ������ �������
                objectSize = GetObjectSize(ref obj);
                //���� ������ ������� ��������� ����������� ����������
                if (objectSize > _maxObjectSize || objectSize > _maxCacheSize)
                {
                    //���������� ����� (��� ���� �� �������� ������)
                    return true;
                }

                //���� � ���� ��� ����� ��� �������
                while (objectSize + _currentCacheSize > _maxCacheSize)
                {
                    //������������� ������ ��������������� �������
                    Int64 localId;

                    //��������� ������������� ������ ��������������� ������� �� �������
                    if (!_objectsHistory.PopFirst(out localId))
                    {
                        //���� ���-�� �� ���
                        //������ ����������
                        throw new InvalidOperationException("SingleCacheBase::Get - ������� ������� �� ������ �������!");
                    }

                    //�������� �������� �������
                    if (!_objects.TryGetValue(localId, out cachedObj))
                    {
                        //���� ���-�� �� ���
                        //������ ����������
                        throw new InvalidOperationException("SingleCacheBase::Get - ������ �� ������ � ����!");
                    }

                    //��������� ������ ����
                    _currentCacheSize -= cachedObj.ObjectSize;

                    //������� ������ �� ����
                    _objects.Remove(localId);
                }
            }

            //������ ����� �������� ����������� �������
            cachedObj = new CachedObject();
            //��������� ���
            cachedObj.Object = obj;
            cachedObj.ObjectSize = objectSize;
            //��������� ������ � ������ ������� �������������
            cachedObj.HistoryIndex = _objectsHistory.Add(id);
            //��������� ������ � ���
            _objects.Add(id, cachedObj);

            //���������� �����
            return true;
        }

        /// <summary>
        /// ����� �������� ����������� �������
        /// </summary>
        protected class CachedObject
        {
            //��� ������
            public T Object;
            //������ ������� � ������ ������� �������������
            public int HistoryIndex;
            //������ �������
            public int ObjectSize;
        }
    }
}