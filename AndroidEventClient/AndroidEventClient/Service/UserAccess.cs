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
    /// Класс работы с сервером
    /// </summary>
    public class UserAccess : UserAccessClient
    {
        //Количество попыток вызова методов сервера
        protected const int TRY_METHOD_CALLS_MAX_NUM = 3;
        //Таймаут на сетевые операции в секундах
        protected const int NET_OPERATIONS_TIMOUT = 10;

        //Точка потключения к серверу
        protected static EndpointAddress _endPoint = null;
        //Привязка протокола при подключении к серверу
        protected static BasicHttpBinding _binding = null;

        /// <summary>
        /// Делегат события возникновения ошибки при обмене с сервером
        /// </summary>
        /// <param name="errorDescription"></param>
        public delegate void OnErrorDelegate(string errorDescription);

        /// <summary>
        /// Событие возникновения ошибки при обмене с сервером
        /// </summary>
        public static event OnErrorDelegate OnError;

        /// <summary>
        /// Объект доступа к серверу
        /// </summary>
        public static UserAccess Client = null;

        /// <summary>
        /// Инициализация обмена с сервером
        /// </summary>
        /// <param name="client"></param>
        public static void Init()
        {
            //Добавляем обработчик ошибок проверки сертификата сервера
            System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ServerCertificateValidationCallback);

            //Создаём точку подключения
            _endPoint = new EndpointAddress("http://www.project-ai.org/EventService/UserClient.svc");
            //Создаём привязку протокола
            _binding = new BasicHttpBinding();
            //Задаём параметры привязки
            _binding.Name = "AEC";
            _binding.MaxBufferSize = 2147483647;
            _binding.MaxReceivedMessageSize = 2147483647;
            _binding.SendTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
            _binding.OpenTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
            _binding.ReceiveTimeout = new TimeSpan(0, 0, NET_OPERATIONS_TIMOUT);
        }

        /// <summary>
        /// Деинициализация обмена с сервером
        /// </summary>
        public static void Destroy()
        {
            //Если объект клиента создан
            if (Client != null)
            {
                //Освобождаем ресурсы
                (Client as IDisposable).Dispose();
                //Очищаем ссылку
                Client = null;
            }

            //Добавляем обработчик ошибок проверки сертификата сервера
            System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(ServerCertificateValidationCallback);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserAccess() :
            base(_binding, _endPoint)
        {
        }

        /// <summary>
        /// Вызов методов сервера с обработкой ошибок (для методов с возвращаемым значением)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Execute<T>(Expression<Func<T>> method, out T result)
        {
            //Инициализируем возвращаемые значения
            result = default(T);

            //Если объект клиента ещё не создан
            if (Client == null)
            {
                //Создаём его
                Client = new UserAccess();

                //Формируем выражение для вызова метода с новым объектом
                method = Expression.Lambda<Func<T>>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
            }

            //Цикл повторения запроса
            for (int tryNum = 0; tryNum < TRY_METHOD_CALLS_MAX_NUM; tryNum += 1)
            {
                try
                {
                    //Если объект обмена в невменяемом состоянии
                    if (Client.State == CommunicationState.Faulted)
                    {
                        //Закрываем старое подключение
                        Client.Abort();
                        //Освобождаем ресурсы
                        (Client as IDisposable).Dispose();
                        //Создаём новое подключение
                        Client = new UserAccess();

                        //Формируем выражение для вызова метода с новым объектом
                        method = Expression.Lambda<Func<T>>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
                    }

                    //Выполняем запрос
                    result = method.Compile()();
                    //Выходим с успехом
                    return true;
                }
                //Если контролируемое исключение сервера
                catch (FaultException<ESException> ex)
                {
                    //Если обработчик события ошибки определён
                    if (OnError != null)
                    {
                        //Вызываем событие обработки ошибки
                        OnError.Invoke(ex.Detail.Description);
                    }
                    //Выходим с ошибкой
                    return false;
                }
                //Если точка доступа к серверу недоступна
                catch (EndpointNotFoundException)
                {
                    //Ничего не делаем и повторяем попытку
                }
                //Если исключение обмена по сети
                catch (CommunicationException)
                {
                    //Ничего не делаем и повторяем попытку
                }
            }

            //Если обработчик события ошибки определён
            if (OnError != null)
            {
                //Вызываем событие обработки ошибки
                OnError.Invoke("Не удалось связаться с сервером!");
            }

            //Выходим с ошибкой
            return false;
        }

        /// <summary>
        /// Вызов методов сервера с обработкой ошибок (для методов без возвращаемого значения)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Execute(Expression<Action> method)
        {
            //Если объект клиента ещё не создан
            if (Client == null)
            {
                //Создаём его
                Client = new UserAccess();

                //Формируем выражение для вызова метода с новым объектом
                method = Expression.Lambda<Action>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
            }

            //Цикл повторения запроса
            for (int tryNum = 0; tryNum < TRY_METHOD_CALLS_MAX_NUM; tryNum += 1)
            {
                try
                {
                    //Если объект обмена в невменяемом состоянии
                    if (Client.State == CommunicationState.Faulted)
                    {
                        //Закрываем старое подключение
                        Client.Abort();
                        //Освобождаем ресурсы
                        (Client as IDisposable).Dispose();
                        //Создаём новое подключение
                        Client = new UserAccess();

                        //Формируем выражение для вызова метода с новым объектом
                        method = Expression.Lambda<Action>((method.Body as MethodCallExpression).Update(Expression.Constant(Client), (method.Body as MethodCallExpression).Arguments));
                    }

                    //Выполняем запрос
                    method.Compile()();
                    //Выходим с успехом
                    return true;
                }
                //Если контролируемое исключение сервера
                catch (FaultException<ESException> ex)
                {
                    //Если обработчик события ошибки определён
                    if (OnError != null)
                    {
                        //Вызываем событие обработки ошибки
                        OnError.Invoke(ex.Detail.Description);
                    }
                    //Выходим с ошибкой
                    return false;
                }
                //Если точка доступа к серверу недоступна
                catch (EndpointNotFoundException)
                {
                    //Ничего не делаем и повторяем попытку
                }
                //Если исключение обмена по сети
                catch (CommunicationException)
                {
                    //Ничего не делаем и повторяем попытку
                }
            }

            //Если обработчик события ошибки определён
            if (OnError != null)
            {
                //Вызываем событие обработки ошибки
                OnError.Invoke("Не удалось связаться с сервером!");
            }

            //Выходим с ошибкой
            return false;
        }

        //Обработчик ошибок проверки сертификата
        protected static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //Название хоста
            string requestHost = null;

            //Если тип - строка
            if (sender is string)
            {
                //Получаем название хоста
                requestHost = sender.ToString();
            }
            //Если тип другой
            else
            {
                //Формируем указатель на запрос
                HttpWebRequest request = sender as HttpWebRequest;

                //Если это был действительно запрос
                if (request != null)
                {
                    //Получаем название хоста
                    requestHost = request.Host;
                }
            }

            //Если имя хоста определено и это наш хост
            if (!string.IsNullOrEmpty(requestHost) && requestHost == "my_test_machine")
            {
                //Возвращаем успех
                return true;
            }

            //Возвращаем неуспех, если есть какие-то ошибки
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}