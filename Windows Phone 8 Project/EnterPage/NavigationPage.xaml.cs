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
        Requests request;
        public NavigationPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            request = new Requests();

            if (request.isConnecting() == false)
            {
                MessageBox.Show("Can't connect to the server\nPerhaps there is no internet connection");
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }
        private void Refresh()
        {
            request.Close();
            NavigationService.Navigate(new Uri("/NavigationPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
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
                    MessageBox.Show("Can't remove a previously created service:" + ex.Message);
                }
            }

            // создание нового фонового процесса
            myPeriodicTask = new PeriodicTask(ToastAgentName);
            myPeriodicTask.Description = "Agent-Toast";
            try
            {
                ScheduledActionService.Add(myPeriodicTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create service:" + ex.Message);
            }

        }
        private int SetCoordinates()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();  //
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));   // ПОЛУЧАЕМ КООРДИНАТЫ
            GeoCoordinate coord = watcher.Position.Location;            //

            // если координаты известны
            if (coord.IsUnknown != true)
            {
                User.Latitude = coord.Latitude;                   // Сохраняем широту
                User.Longitude = coord.Longitude;                 // Сохраняем долготу

                int value = request.SetCoordinates(User.Name, User.Latitude, User.Longitude);
                if (value != 0)
                {
                    MessageBox.Show("Error when starting, try to enter again");
                    Refresh();
                }                                         // закрываем соединение
                watcher.Stop();
            }
            else
            {
                return 1;
            }
            return 0;
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
                            User.Name = reader.ReadLine();
                            User.Password = reader.ReadLine();
                        }
                    }
                    // если вместо имени и пароля нули, то выйти
                    if (User.Name.CompareTo("0") == 0 && User.Name.CompareTo("0") == 0)
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
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // если пароль и логин сохранены
            int result = SetLoginAndPassword();
            if (result == 0)
            {
                while (SetCoordinates() != 0) ; // Пытаемся получить короординаты пользователя
                CreateService();                  // Создание сервиса для работы фонового процесса
                request.Close();
                NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));             // переходим на другую страницу
            }
            Thread.Sleep(3000);
            // если имя пользователя и пароль не сохранены
            if (result == 2)
            {
                request.Close();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));               // переходим на другую страницу
            }
            // если вход выполнен впервые
            if (result == 1)
            {
                request.Close();
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