using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading.Tasks;
using System.Device.Location;
using System.Windows.Media.Imaging;

namespace EnterPage
{
    public partial class RegPage : PhoneApplicationPage
    {

        public RegPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername() && ValidatePassword() && ValidateEmail())
            {
                if (!IsValidEmail(Email.Text))
                {
                    MessageBox.Show("Некорректный E-mail");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                    Email.Text = string.Empty;
                    return;
                }
                SocketClient client = new SocketClient();
                string rez = client.Connect(PublicData.IP, PublicData.PORT);

                if (!rez.Contains("Success"))
                {
                    MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                    client.Close();
                    return;
                }
                client.Send("<ad/" + Username.Text.ToLower() + "/" + Password.Password + "/" + Email.Text.ToLower() + ">");
                string result = client.Receive();
                client.Close();

                if (result.Contains("<ad/ok>"))
                {
                    using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = appStorage.OpenFile("LS.txt", FileMode.Create))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.WriteLine(Username.Text.ToLower());
                                writer.WriteLine(Password.Password);
                                PublicData.Username = Username.Text.ToLower();
                                PublicData.Password = Password.Password;
                            }
                        }
                    }
                    while (SetCoordinates() != 0);

                    BitmapImage bm = new BitmapImage(new Uri("/Resources/no_photo.jpg", UriKind.RelativeOrAbsolute));

                    SocketClient photoclient = new SocketClient();

                    rez = photoclient.Connect(PublicData.IP, 5000);

                    photoclient.SendFile(bm, PublicData.Username.ToLower() + ".jpg");
                    photoclient.Close();

                    NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));
                }


                if (result.Contains("<ad/bad>"))
                {
                    MessageBox.Show("Такой пользователь уже зарегестрирован");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                    Email.Text = string.Empty;
                }

            }
            else
            {
                Username.Text = string.Empty;
                Password.Password = string.Empty;
                Email.Text = string.Empty;
            }
        }

        private bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
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

        private bool ValidateEmail()
        {
            if (String.IsNullOrWhiteSpace(Email.Text))
            {
                MessageBox.Show("Пожалуйста, введите E-mail");
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