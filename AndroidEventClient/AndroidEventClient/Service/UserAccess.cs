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
using System.Web.Services.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Reflection;

namespace AEC.Service
{
    /// <summary>
    /// ����� ������ � ��������
    /// </summary>
    public class UserAccess : UserAccessClient
    {
        //���������� ������� ������ ������� �������
        protected const int TRY_METHOD_CALLS_MAX_NUM = 3;
        //������� �� ������� �������� � ��������
        protected const int NET_OPERATIONS_TIMOUT = 10;

        //����� ����������� � �������
        protected static EndpointAddress _endPoint = null;
        //�������� ��������� ��� ����������� � �������
        protected static BasicHttpBinding _binding = null;

        /// <summary>
        /// ������� ������� ������������� ������ ��� ������ � ��������
        /// </summary>
        /// <param name="errorDescription"></param>
        public delegate void OnErrorDelegate(string errorDescription);

        /// <summary>
        /// ������� ������������� ������ ��� ������ � ��������
        /// </summary>
        public static event OnErrorDelegate OnError;

        /// <summary>
        /// ������ ������� � �������
        /// </summary>
        public static UserAccess Client = null;

        /// <summary>
        /// ������������� ������ � ��������
        /// </summary>
        /// <param name="client"></param>
        public static void Init()
        {
            //��������� ���������� ������ �������� ����������� �������
            System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ServerCertificateValidationCallback);

            //������ ����� �����������
            _endPoint = new EndpointAddress("http://www.project-ai.org/EventService/UserClient.svc");
            //������ �������� ���������
            _binding = new BasicHttpBinding();
            //����� ��������� ��������
            _binding.Name = "AEC";
            _binding.MaxBufferSize = 2147483647;
            _binding.MaxReceivedMessageSize = 2147483647;
            _binding.SendTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
            _binding.OpenTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
            _binding.ReceiveTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
        }

        /// <summary>
        /// ��������������� ������ � ��������
        /// </summary>
        public static void Destroy()
        {
            //���� ������ ������� ������
            if (Client != null)
            {
                //����������� �������
                (Client as IDisposable).Dispose();
                //������� ������
                Client = null;
            }

            //��������� ���������� ������ �������� ����������� �������
            System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(ServerCertificateValidationCallback);
        }

        /// <summary>
        /// �����������
        /// </summary>
        public UserAccess() :
            base(_binding, _endPoint)
        {
        }

        /// <summary>
        /// ����� ������� ������� � ���������� ������ (��� ������� � ������������ ���������)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Execute<T>(Expression<Func<T>> method, out T result)
        {
            //�������������� ������������ ��������
            result = default(T);

            //���� ������ ������� ��� �� ������
            if (Client == null)
            {
                //������ ���
                Client = new UserAccess();

                //��������� ��������� ��� ������ ������ � ����� ��������
                method = Expression.Lambda<Func<T>>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
            }

            //���� ���������� �������
            for (int tryNum = 0; tryNum < TRY_METHOD_CALLS_MAX_NUM; tryNum += 1)
            {
                try
                {
                    //���� ������ ������ � ����������� ���������
                    if (Client.State == CommunicationState.Faulted)
                    {
                        //��������� ������ �����������
                        Client.Abort();
                        //����������� �������
                        (Client as IDisposable).Dispose();
                        //������ ����� �����������
                        Client = new UserAccess();

                        //��������� ��������� ��� ������ ������ � ����� ��������
                        method = Expression.Lambda<Func<T>>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
                    }

                    //��������� ������
                    result = method.Compile()();
                    //������� � �������
                    return true;
                }
                //���� �������������� ���������� �������
                catch (FaultException<ESException> ex)
                {
                    //���� ���������� ������� ������ ��������
                    if (OnError != null)
                    {
                        //�������� ������� ��������� ������
                        OnError.Invoke(ex.Detail.Description);
                    }
                    //������� � �������
                    return false;
                }
                //���� ����� ������� � ������� ����������
                catch (EndpointNotFoundException)
                {
                    //������ �� ������ � ��������� �������
                }
                //���� ���������� ������ �� ����
                catch (CommunicationException)
                {
                    //������ �� ������ � ��������� �������
                }
            }

            //���� ���������� ������� ������ ��������
            if (OnError != null)
            {
                //�������� ������� ��������� ������
                OnError.Invoke("�� ������� ��������� � ��������!");
            }

            //������� � �������
            return false;
        }

        /// <summary>
        /// ����� ������� ������� � ���������� ������ (��� ������� ��� ������������� ��������)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Execute(Expression<Action> method)
        {
            //���� ������ ������� ��� �� ������
            if (Client == null)
            {
                //������ ���
                Client = new UserAccess();

                //��������� ��������� ��� ������ ������ � ����� ��������
                method = Expression.Lambda<Action>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
            }

            //���� ���������� �������
            for (int tryNum = 0; tryNum < TRY_METHOD_CALLS_MAX_NUM; tryNum += 1)
            {
                try
                {
                    //���� ������ ������ � ����������� ���������
                    if (Client.State == CommunicationState.Faulted)
                    {
                        //��������� ������ �����������
                        Client.Abort();
                        //����������� �������
                        (Client as IDisposable).Dispose();
                        //������ ����� �����������
                        Client = new UserAccess();

                        //��������� ��������� ��� ������ ������ � ����� ��������
                        method = Expression.Lambda<Action>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
                    }

                    //��������� ������
                    method.Compile()();
                    //������� � �������
                    return true;
                }
                //���� �������������� ���������� �������
                catch (FaultException<ESException> ex)
                {
                    //���� ���������� ������� ������ ��������
                    if (OnError != null)
                    {
                        //�������� ������� ��������� ������
                        OnError.Invoke(ex.Detail.Description);
                    }
                    //������� � �������
                    return false;
                }
                //���� ����� ������� � ������� ����������
                catch (EndpointNotFoundException)
                {
                    //������ �� ������ � ��������� �������
                }
                //���� ���������� ������ �� ����
                catch (CommunicationException)
                {
                    //������ �� ������ � ��������� �������
                }
            }

            //���� ���������� ������� ������ ��������
            if (OnError != null)
            {
                //�������� ������� ��������� ������
                OnError.Invoke("�� ������� ��������� � ��������!");
            }

            //������� � �������
            return false;
        }

        //���������� ������ �������� �����������
        protected static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //�������� �����
            string requestHost = null;

            //���� ��� - ������
            if (sender is string)
            {
                //�������� �������� �����
                requestHost = sender.ToString();
            }
            //���� ��� ������
            else
            {
                //��������� ��������� �� ������
                HttpWebRequest request = sender as HttpWebRequest;

                //���� ��� ��� ������������� ������
                if (request != null)
                {
                    //�������� �������� �����
                    requestHost = request.Host;
                }
            }

            //���� ��� ����� ���������� � ��� ��� ����
            if (!string.IsNullOrEmpty(requestHost) && requestHost == "my_test_machine")
            {
                //���������� �����
                return true;
            }

            //���������� �������, ���� ���� �����-�� ������
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}