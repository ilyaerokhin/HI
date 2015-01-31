using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EnterPage.Resources;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading.Tasks;
using System.Device.Location;

namespace EnterPage
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername() && ValidatePassword())
            {
                SocketClient client = new SocketClient();
                string rez = client.Connect(PublicData.IP, PublicData.PORT);

                if (!rez.Contains("Success"))
                {
                    MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                    client.Close();
                    return;
                }

                client.Send("<cu/" + Username.Text.ToLower() + "/" + Password.Password + ">");
                string result = client.Receive();
                client.Close();

                if (result.Contains("<cu/ok>"))
                {
                    using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = appStorage.OpenFile("LS.txt", FileMode.Create))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.WriteLine(Username.Text.ToLower());
                                writer.WriteLine(Password.Password);
                            }
                        }
                    }
                    PublicData.Username = Username.Text.ToLower();
                    PublicData.Password = Password.Password;

                    while (SetCoordinates() != 0) ;
                    NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));
                }

                if (result.Contains("<cu/not>"))
                {
                    MessageBox.Show("Неверное имя пользователя");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                }

                if (result.Contains("<cu/bad>"))
                {
                    MessageBox.Show("Неверный пароль");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                }

            }
            else
            {
                Username.Text = string.Empty;
                Password.Password = string.Empty;
            }
        }

        private bool ValidateUsername()
        {
            if (String.IsNullOrWhiteSpace(Username.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя");
                return false;
            }

            return true;
        }

        private bool ValidatePassword()
        {
            if (String.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Пожалуйста, введите пароль");
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Task.Delay(100);
            if (MessageBox.Show("Вы хотите покинуть приложение?", "Выход", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
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
    }
}