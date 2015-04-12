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
    /// ����� ���� ��������
    /// </summary>
    public class PhotoCache : SingleCacheBase<Photo>
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="service">������ ������� ��������� ������</param>
        /// <param name="maxCacheCount">����������� ���������� ���������� �������� � ���� (0 - �������� �� ��������������)</param>
        /// <param name="maxCacheSize">����������� ���������� ����� ������ ���� (0 - �������� �� ��������������)</param>
        /// <param name="maxObjectSize">����������� ���������� ������ ������ ������� (0 - �������� �� ��������������)</param>
        public PhotoCache(DataService service, int maxCacheCount = 0, int maxCacheSize = 0, int maxObjectSize = 0) :
            base(service, maxCacheCount, maxCacheSize, maxObjectSize)
        {
        }

        /// <summary>
        /// ���������������� ����� ��������� ������� �� ��
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">������������ ������</param>
        /// <returns>������� ��������� ��������� � ��</returns>
        protected override bool GetFromDB(Int64 id, out Photo obj)
        {
            //�������������� �������� ���������
            obj = null;


            //�� ��������� ���������� �������
            return false;
        }

        /// <summary>
        /// ��������� ������ � ��
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">����������� ������</param>
        protected override void SaveToDB(Int64 id, ref Photo obj)
        {
        }

        /// <summary>
        /// ���������������� ����� ��������� ������� � �������
        /// </summary>
        /// <param name="id">������������� �������</param>
        /// <param name="obj">������������ ������</param>
        /// <returns>������� ��������� ��������� �� �������</returns>
        protected override bool GetFromServer(Int64 id, out Photo obj)
        {
            //�������� ���� � �������
            return UserAccess.Execute<Photo>(() => UserAccess.Client.GetPhoto(_service.SessionId, id), out obj);
        }

        /// <summary>
        /// ���������������� ����� ��������� ���������������� ������� ������� 
        /// </summary>
        /// <param name="obj">����������� ������</param>
        /// <returns>��������������� ������</returns>
        protected override int GetObjectSize(ref Photo obj)
        {
            //���������� ������
            return obj.Data.Length;
        }
    }
}