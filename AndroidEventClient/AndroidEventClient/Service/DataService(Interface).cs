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
    /// ����� ������� ������ � �������� (����� ���������� ������ � �������)
    /// </summary>
    public partial class DataService : Android.App.Service
    {
        //������ ����������
        static private Int64 currentVersionRootCatID = 30;     //30 = ��������� ���
        static private Int64 currentVersionCatID = 33;         //33 = ���� ��������� ���������� ��� (����� ��������� �������������� �������� ���������)
        //������ � ������ ����������
        static public Int64 whatVersionRootCatID { get { return currentVersionRootCatID; } }
        static public Int64 whatVersionCatID { get { return currentVersionCatID; } }

        //������������� ������� ������ ������� � �������
        public string SessionId = null;

        /// <summary>
        /// ������ ����� ������� � �������
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public bool Login(string login, string password, Captcha captcha)
        {
            //��������� ��������
            bool result = true;

            //������ � �������
            result &= UserAccess.Execute<string>(() => UserAccess.Client.Login("admin", "admin", captcha), out SessionId);

            //���������� ���������
            return result;
        }

        /// <summary>
        /// ��������� ����� ������� � �������
        /// </summary>
        public bool Logout()
        {
            //��������� ��������
            bool result = true;

            //������� �� �������
            result &= UserAccess.Execute(() => UserAccess.Client.Logout(SessionId));

            //���������� ���������
            return result;
        }

        /// <summary>
        /// ��������� ������ ���������
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public bool GetEventsList(int pos, int count, out EventShort[] events)
        {
            //����� ��������� �������� ��������
            events = null;

            //��������� ��������
            bool result = true;

            //�������� ������� � �������
            result &= UserAccess.Execute<EventShort[]>(() => UserAccess.Client.GetEventsList(SessionId, pos, count), out events);
            //���� ������
            if (!result)
            {
                //������� � �������
                return false;
            }

            //������� � �������
            return true;
        }

        /// <summary>
        /// ��������� ������ ���������
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="rootId"></param>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public bool GetCategoriesList(long parentId, long rootId, int pos, int count, out CategoryShort[] categories)
        {
            //����� ��������� �������� ��������
            categories = null;

            //��������� ��������
            bool result = true;

            //�������� ������� � �������
            result &= UserAccess.Execute<CategoryShort[]>(() => UserAccess.Client.GetCategoriesListForCurrentAccount(SessionId, parentId, rootId, pos, count), out categories);
            //���� ������
            if (!result)
            {
                //������� � �������
                return false;
            }

            //������� � �������
            return true;
        }

        /// <summary>
        /// �������� ������ � ��������� �� ID ���������
        /// </summary>
        /// <param name="whatVersionRootCatID">ID ������������ ��������� ������� ���������</param>
        /// <param name="whatVersionCatID">ID ������� ���������</param>
        /// <returns></returns>
        public bool GetRootCategory(Int64 categoryId, out CategoryShort[] categories)
        {
            Int64 parentId = 30;
            Int64 rootId = whatVersionRootCatID;
            int pos = 1;
            int count = 100;

            //����� ��������� �������� ��������
            categories = null;

            //��������� ��������
            bool result = true;

            //�������� ������� � �������
            result &= UserAccess.Execute<CategoryShort[]>(() => UserAccess.Client.GetCategoriesListForCurrentAccount(SessionId, parentId, rootId, pos, count), out categories);
                        //���� ������
            if (!result)
            {
                //������� � �������
                return false;
            }

            //������� � �������
            return true;
        }

        /// <summary>
        /// �������� ���� �� ��� Id
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="photo"></param>
        /// <param name="onlyInCache">������� ������ ������ � ����</param>
        /// <returns></returns>
        public bool GetIcon(long photoId, out Bitmap photo, bool onlyInCache)
        {
            //����� ��������� �������� ��������    
            photo = null;

            //���� � �������
            Photo serverPhoto;

            //�������� ���� � ����
            if (!_iconsCache.Get(photoId, out serverPhoto, onlyInCache))
            {
                //���� �� ����������
                //������� � ���������, ��������� �������� �� ���������

                return false;
            }

            //����������� ���� � �������� ���� � ���������� ���
            photo = Android.Graphics.BitmapFactory.DecodeByteArray(serverPhoto.Data, 0, serverPhoto.Data.Length);

            //������� � �������
            return true;
        }

        /// <summary>
        /// �������� ������ �������� ������� �� ��� Id
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="photo"></param>
        /// <param name="onlyInCache">������� ������ ������ � ����</param>
        /// <returns></returns>
        public bool GetEventFull(long eventId, out EventFullGet eventFull, bool onlyInCache)
        {
            //����� ��������� �������� ��������
            eventFull = null;

            //�������� ���� � ����
            if (!_eventsFullCache.Get(eventId, out eventFull, onlyInCache))
            {
                //���� �� ����������
                //������� � ���������
                return false;
            }

            //������� � �������
            return true;
        }
    }
}