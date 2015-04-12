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
    /// ����� �������� ������ ��� ����������� ������������ �������
    /// </summary>
    /// <typeparam name="ELEMENT">����� ��������</typeparam>
    /// <typeparam name="CONTEXT">����� ��������� ��������� ������</typeparam>
    /// <typeparam name="ELEMENT_DATA">����� ������ ��������</typeparam>
    public abstract class DataAdapter<ELEMENT, CONTEXT, ELEMENT_DATA> : BaseAdapter<ELEMENT>
        where ELEMENT : class
        where CONTEXT : class
        where ELEMENT_DATA : class
    {
        //������ ������������ ���������
        List<ELEMENT> _items = new List<ELEMENT>();
        //������� ����������� ���������
        Dictionary<View, ELEMENT> _itemsToLoad = new Dictionary<View, ELEMENT>();
        //������� ������������ ������
        int _observersCount = 0;
        //������� ������ ������ �������� ��������� ���������
        bool _loadItemsTaskIsRunning = false;

        /// <summary>
        /// ������ ��� ��������� ������
        /// </summary>
        public DataService Service { get; set; }

        /// <summary>
        /// �������� ��������� ������
        /// </summary>
        public CONTEXT DataContext { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        public DataAdapter() : base()
        {
            //��������� ������ �� ������
            Service = (Application.Context as AECApplication).Service;
        }

        /// <summary>
        /// ����� ��������� �������������� ��������
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            //��������� ������
            lock (_items)
            {
                //���������� ������������� ��������
                return (Int64)typeof(ELEMENT).GetProperty("Id").GetValue(_items[position]);
            }
        }

        /// <summary>
        /// ���������� ������ ���������
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override ELEMENT this[int position]
        {
            get
            {
                //��������� ������
                lock (_items)
                {
                    //���������� �������
                    return _items[position];
                }
            }
        }

        /// <summary>
        /// ���������� ��������� �������
        /// </summary>
        public override int Count
        {
            get
            {
                //��������� ������
                lock (_items)
                {
                    //���������� �������
                    return _items.Count;
                }
            }
        }

        /// <summary>
        /// ������� ������� ��������������� � ���������
        /// </summary>
        public override bool HasStableIds
        {
	        get 
	        { 
                //���������� ������� �������
		        return true;
	        }
        }

        /// <summary>
        /// ����� ������������ ���� ��������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //�������������� �������
            ELEMENT item = this[position];

            //������������ ��� ��������
            View itemView = null;

            //������� ������������� ���������� ��������
            bool loadItemExtras;

            //�������� ����� ������������ ���������� ���� ��������
            itemView = GetInitialView(item, convertView, parent, out loadItemExtras);

            //���� ��������� ������� ����������
            if (loadItemExtras)
            {
                //��������� ������
                lock (_itemsToLoad)
                {
                    //������������� ����� ����������� ������� ��� ���������� �����������
                    _itemsToLoad[itemView] = item;

                    //���� ������ �������� ��������� �� ��������
                    if (!_loadItemsTaskIsRunning)
                    {
                        //������������� ������� ������ ������ �������� ��������� ���������
                        _loadItemsTaskIsRunning = true;

                        //��������� ������ �������� ���������
                        ThreadPool.QueueUserWorkItem((a) =>
                        {
                            //��������� �������� ������� ����������� ���������
                            KeyValuePair<View, ELEMENT> pair;

                            while (true)
                            {
                                //��������� ������
                                lock (_itemsToLoad)
                                {
                                    //�������� ��������� ��������� �� �������
                                    pair = _itemsToLoad.FirstOrDefault();

                                    //���� ��������� ��� �������� ���
                                    if (pair.Key == null)
                                    {
                                        //��������� ���� ��������
                                        break;
                                    }

                                    //������� ��������� �� �������
                                    _itemsToLoad.Remove(pair.Key);
                                }

                                //�������������� ������ ��������
                                ELEMENT_DATA extras;

                                //�������� �������� ������ ��������
                                if (LoadItem(pair.Value, out extras))
                                {
                                    //���� ������� ���������
                                    //�������� ��� � �������� ������
                                    Application.SynchronizationContext.Post((state) =>
                                    {
                                        //�������� ����� ������������ �������������� ���� ��������
                                        FormFinalView(((KeyValuePair<View, ELEMENT>)state).Value, extras, ((KeyValuePair<View, ELEMENT>)state).Key);
                                    }, pair);
                                }
                                //���� ���� ������ ��� ��������
                                else
                                {
                                    //������������� ������������ � ������������� ������
                                    NotifyDataSetInvalidated();

                                    //������� �� ����� ��������
                                    break;
                                }
                            };

                            //���������� ������� ������ ������ �������� ��������� ���������
                            _loadItemsTaskIsRunning = false;
                        });
                    }
                }
            }

            //���������� ��������� ��������� ������� ����
            return itemView;
        }

        /// <summary>
        /// ���������� ������ ����������� ������
        /// </summary>
        /// <param name="observer"></param>
        public override void RegisterDataSetObserver(DataSetObserver observer)
        {
            //�������� ����� �������� ������
            base.RegisterDataSetObserver(observer);

            //����������� ������� ������������
            _observersCount += 1;
            //���� ������������ ������ �����������
            if (_observersCount == 1)
            {
                //�������� ���������� �������� ��������� � ��������� ������
                ThreadPool.QueueUserWorkItem((a) =>
                {
                    //������ ����������� ���������
                    List<ELEMENT> items;

                    //�������� ����� �������� ���������
                    if (LoadItems(0, 100, out items))
                    {
                        //���� ������� ���������
                        //��������� ������
                        lock (_items)
                        {
                            //��������� ����� �������� � ����� ������
                            _items.InsertRange(0, items);
                        }

                        //�������� ��� � �������� ������
                        Application.SynchronizationContext.Post((state) =>
                        {
                            //������������� ������������ �� ���������� ������
                            NotifyDataSetChanged();
                        }, null);
                    }
                    //���� ���� ������ ��� ��������
                    else
                    {
                        //�������� ��� � �������� ������
                        Application.SynchronizationContext.Post((state) =>
                        {
                            //������������� ������������ � ������������� ������
                            NotifyDataSetInvalidated();
                        }, null);
                    }
                });
            }
        }

        /// <summary>
        /// �������� ����������� ������
        /// </summary>
        /// <param name="observer"></param>
        public override void UnregisterDataSetObserver(DataSetObserver observer)
        {
            //�������� ����� �������� ������
            base.UnregisterDataSetObserver(observer);

            //��������� ������� ������������
            _observersCount -= 1;
            //���� ��� ����������� �����������
            if (_observersCount == 0)
            {
                //��������� ������
                lock (_items)
                {
                    //��������� ������
                    lock (_itemsToLoad)
                    {
                        //������� ������� ��������� ��� ��������
                        _itemsToLoad.Clear();
                    }

                    //������� ������ ������������ ���������
                    _items.Clear();
                }
            }
        }

        /// <summary>
        /// ���������������� ����� �������� ���������
        /// </summary>
        /// <param name="position">��������� ������� ����������� ��������� � ��������� ������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="items">������������ ������ ���������</param>
        /// <returns>������� ���������� ��������</returns>
        public abstract bool LoadItems(int position, int count, out List<ELEMENT> items);

        /// <summary>
        /// ���������������� ����� �������� ������ ��������
        /// </summary>
        /// <param name="item"></param>
        /// <returns>������, ��������� ����������� ������</returns>
        public virtual bool LoadItem(ELEMENT item, out ELEMENT_DATA extras)
        {
            //���������� ���������� �������������� ������
            extras = null;

            //���������� ����� ��������
            return true;
        }

        /// <summary>
        /// ���������������� ����� ������������ ���������� ���� �������� (����������, ���� LoadEachItem ����������)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <param name="loadItemExtras">������������ ������� ������������� ���������� �������� (����� ������ LoadItem � FormFinalView), ��������, �������� ��� �� ���������, � �� ���� �� ����</param>
        /// <returns></returns>
        public virtual View GetInitialView(ELEMENT item, View convertView, ViewGroup parent, out bool loadItemExtras)
        {
            //�������������� �������� ���������
            loadItemExtras = false;

            //���������� ��������
            return null;
        }

        /// <summary>
        /// ���������������� ����� ������������ �������������� ���� ��������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="extras">������, ������������ ������� LoadItem</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual void FormFinalView(ELEMENT item, ELEMENT_DATA extras, View itemView)
        {
        }
    }
}