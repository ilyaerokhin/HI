using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;
using Microsoft.Phone.Scheduler;
using System.Device.Location;
using System.Globalization;
using System.Threading.Tasks;

namespace EnterPage
{
    public partial class NavigationPage : PhoneApplicationPage
    {
        // Конструктор пейджа:
        public NavigationPage()
        {
            InitializeComponent();            // Инициализация компонентов пейджа
        }

        private void CreateService()
        {
            string ToastAgentName = "Agent-Toast";     // Имя фонового процесса

            PeriodicTask myPeriodicTask = ScheduledActionService.Find(ToastAgentName) as PeriodicTask;

            // Если фоновый процесс уже создан
            if (myPeriodicTask != null)
            {
                try
                {
                    // Удалить фоновый процесс
                    ScheduledActionService.Remove(ToastAgentName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно удалить ранее созданный сервис:" + ex.Message);
                }
            }

            // создание нового фонового процесса
            myPeriodicTask = new PeriodicTask(ToastAgentName);
            myPeriodicTask.Description = "Agent-Toast";
            try
            {
                ScheduledActionService.Add(myPeriodicTask);

#if DEBUG
                ScheduledActionService.LaunchForTest(ToastAgentName, TimeSpan.FromSeconds(10));
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно создать сервис:" + ex.Message);
            }

        }

        // Отправляем координаты пользователя
        private int SetCoordinates()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();  //
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));   // ПОЛУЧАЕМ КООРДИНАТЫ
            GeoCoordinate coord = watcher.Position.Location;            //

            // если координаты известны
            if (coord.IsUnknown != true)
            {
                PublicData.Latitude = coord.Latitude;                   // Сохраняем широту
                PublicData.Longitude = coord.Longitude;                 // Сохраняем долготу

                SocketClient client = new SocketClient();               //
                string rez = client.Connect(PublicData.IP, PublicData.PORT);                  // СОЕДИНЯЕМСЯ С СЕРВЕРОМ
                //
                //если неуспех, то выходим
                if (!rez.Contains("Success"))
                {
                    client.Close();
                    return 1;
                }

                // отправляем на сервер запрос с координатами пользователя
                // структура запроса: <uc/username/широта/долгота>
                // подробнее в документации по запросам

                client.Send("<uc/" + PublicData.Username.ToLower() + "/" + coord.Latitude + "/" + coord.Longitude + ">"); // отправка запроса
                string result = client.Receive();                       // получаем ответ от сервера
                client.Close();                                         // закрываем соединение
                watcher.Stop();
            }
            else
            {
                return 1;
            }
            return 0;
            //NotifyComplete();                                           // что-то нужное
        }

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
                            PublicData.Username = reader.ReadLine();
                            PublicData.Password = reader.ReadLine();
                        }
                    }
                    // если вместо имени и пароля нули, то выйти
                    if (PublicData.Username.CompareTo("0") == 0 && PublicData.Username.CompareTo("0") == 0)
                    {
                        return 2; // 2 = неудача
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

        //действия при загрузке пейджа
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // если пароль и логин сохранены
            int result = SetLoginAndPassword();
            if (result == 0)
            {
                SocketClient client = new SocketClient();                                              // создаём сокет
                string rez = client.Connect(PublicData.IP, PublicData.PORT);                           // подключаемся к серверу

                // Если подключиться невозможно
                if (!rez.Contains("Success"))
                {
                    client.Close();                                                                    // закрываем сокет
                    MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                    IsolatedStorageSettings.ApplicationSettings.Save();                                // сохраняем настройки
                    Application.Current.Terminate();                                                   // выключаем приложение
                }


                // Пытаемся получить короординаты пользователя
                while (SetCoordinates() != 0) ;

                client.Close();                                                                        // закрываем соединение с сервером
                CreateService();                  // Создание сервиса для работы фонового процесса
                NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));             // переходим на другую страницу
            }
            Thread.Sleep(3000);
            // если имя пользователя и пароль не сохранены
            if (result == 2)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));               // переходим на другую страницу
            }
            // если вход выполнен впервые
            if (result == 1)
            {
                NavigationService.Navigate(new Uri("/Resolution.xaml", UriKind.Relative));               // переходим на другую страницу
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
             IsolatedStorageSettings.ApplicationSettings.Save();
             Application.Current.Terminate();
        }
    }
}