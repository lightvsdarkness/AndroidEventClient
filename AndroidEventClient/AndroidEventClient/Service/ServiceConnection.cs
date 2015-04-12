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
using Android.Support.V7.App;

namespace AEC.Service
{
    /// <summary>
    /// ����� ���������� ������� � Activity
    /// </summary>
    public class ServiceConnection : Java.Lang.Object, IServiceConnection
    {
        //������ ������ ����������
        AECApplication _application = null;
        //������ ��������
        DataServiceBinder _serviceBinder = null;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="activity"></param>
        public ServiceConnection(AECApplication application)
        {
            //��������� ����������
            _application = application;
        }

        /// <summary>
        /// ���������� ������� ���������� ������ � �������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            //��������� ������ �� ������ ��������
            _serviceBinder = service as DataServiceBinder;
            //���� ������ ����������
            if (_serviceBinder != null)
            {
                //��������� ������ �� ������ � ������� ����������
                _application.Service = _serviceBinder.Service;

                //������ ���������
                Intent serviceBoundIntent = new Intent(DataService.AECServiceBoundIntent);
                //�������� ���������� ��������� � ���������� �������
                _application.SendOrderedBroadcast(serviceBoundIntent, null);
            }
        }

        /// <summary>
        /// ���������� ������� ���������� ������ �� �������
        /// </summary>
        /// <param name="name"></param>
        public void OnServiceDisconnected(ComponentName name)
        {
            //������� ������ �� ������ � ������� ����������
            _application.Service = null;
        }
    }
}