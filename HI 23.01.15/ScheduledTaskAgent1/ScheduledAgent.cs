using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System;
using System.IO.IsolatedStorage;
using System.IO;
using EnterPage;

namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        string Username;
        string Password;
        static int PORT = 32000;
        static string IP = "109.120.164.212";
        /// <remarks>
        /// Конструктор ScheduledAgent, инициализирует обработчик UnhandledException
        /// </remarks>
        static ScheduledAgent()
        {

            // Подпишитесь на обработчик управляемых исключений
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Код для выполнения на необработанных исключениях
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Произошло необработанное исключение; перейти в отладчик
                Debugger.Break();
            }
        }

        // Получаем логин и пароль из внутреннего хранилища 
        private int SetLoginAndPassword()
        {
            // ПОЛУЧЕНИЕ ДАННЫХ ИЗ ЛОКАЛЬНОГО ХРАНИЛИЩА
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // проверка на существование файла
                if (appStorage.FileExists("LS.txt")) 
                {
                    // открытие файла
                    using (var file = appStorage.OpenFile("LS.txt", FileMode.Open))
                    {
                        // чтение файла
                        using (var reader = new StreamReader(file))
                        {
                            Username = reader.ReadLine();
                            Password = reader.ReadLine();
                        }
                    }
                    // если вместо имени и пароля нули, то выйти
                    if (Username.CompareTo("0") == 0 && Username.CompareTo("0") == 0)
                    {
                        return 1; // 1 = неудача
                    }
                }
                // если файл не существует, то выйти
                else
                {
                    return 1; // 1 = неудача
                }
            }
            return 0; // 0 = удача
        }

        // Отправляем координаты пользователя
        void SetCoordinates()
        {
            //ShellToast toast = new ShellToast();
            //toast.Title = "Service";

            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();  //
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));   // ПОЛУЧАЕМ КООРДИНАТЫ
            GeoCoordinate coord = watcher.Position.Location;            //

            // если координаты известны
            if (coord.IsUnknown != true)
            {

                SocketClient client = new SocketClient();               //
                string rez = client.Connect(IP, PORT);                  // СОЕДИНЯЕМСЯ С СЕРВЕРОМ
                                                                        //
                //если неуспех, то выходим
                if (!rez.Contains("Success"))
                {
                    client.Close();
                    return;
                }

                // отправляем на сервер запрос с координатами пользователя
                // структура запроса: <uc/username/широта/долгота>
                // подробнее в документации по запросам

                client.Send("<uc/" + Username.ToLower() + "/" + coord.Latitude + "/" + coord.Longitude + ">"); // отправка запроса
                string result = client.Receive();                       // получаем ответ от сервера
                client.Close();                                         // закрываем соединение

                //toast.Content = coord.Latitude + "/" + coord.Longitude;
                //toast.Show();
            }
            NotifyComplete();                                           // что-то нужное
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                             //
        //      метод, вызываемый при включении приложения в фоне                                      //
        protected override void OnInvoke(ScheduledTask task)                                           //
        {
            SocketClient client = new SocketClient();               //
            string rez = client.Connect(IP, PORT);                  // СОЕДИНЯЕМСЯ С СЕРВЕРОМ
            if (!rez.Contains("Success"))
            {
                client.Close();
                return;
            }
            client.Close();

            if(SetLoginAndPassword() == 1)                                                             //
            {                                                                                          //
                return;                                                                                //
            }                                                                                          //
                                                                                                       //
            SetCoordinates();                                                                          //
                                                                                                       //
            #if DEBUG                                                                                  //
            ScheduledActionService.LaunchForTest(task.Name, System.TimeSpan.FromSeconds(1));           //
            #endif                                                                                     //
        }                                                                                              //
        /////////////////////////////////////////////////////////////////////////////////////////////////
    }                                                                                                  

}