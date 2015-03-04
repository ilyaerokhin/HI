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
        Requests request;
        public RegPage()
        {
            InitializeComponent();
            request = new Requests();

            if (request.isConnecting() == false)
            {
                MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }
        private void Refresh()
        {
            NavigationService.Navigate(new Uri("/RegPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername() && ValidatePassword() && ValidateEmail())
            {
                if (!PublicData.IsValidEmail(Email.Text))
                {
                    MessageBox.Show("Некорректный E-mail");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                    Email.Text = string.Empty;
                    return;
                }

                int value = request.Add(Username.Text, Password.Password, Email.Text);

                if (value == 0)
                {
                    using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = appStorage.OpenFile("LS.txt", FileMode.Create))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.WriteLine(Username.Text.ToLower());
                                writer.WriteLine(Password.Password);
                                User.Name = Username.Text.ToLower();
                                User.Password = Password.Password;
                            }
                        }
                    }
                    while (SetCoordinates() != 0);

                    BitmapImage bm = new BitmapImage(new Uri("/Resources/no_photo.jpg", UriKind.RelativeOrAbsolute));
                    SocketClient photoclient = new SocketClient();
                    photoclient.Connect(request.GetIP(), 5000);
                    photoclient.SendFile(bm, User.Name.ToLower() + ".jpg");
                    photoclient.Close();

                    NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));
                }


                if (value != 0)
                {
                    MessageBox.Show("Попробуй ещё раз");
                    Refresh();
                }

            }
            else
            {
                Username.Text = string.Empty;
                Password.Password = string.Empty;
                Email.Text = string.Empty;
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
                User.Latitude = coord.Latitude;                   // Сохраняем широту
                User.Longitude = coord.Longitude;                 // Сохраняем долготу

                int value = request.SetCoordinates(User.Name, User.Latitude, User.Longitude);
                if (value != 0)
                {
                    MessageBox.Show("Ошибка при старте, попробуйте войти ещё раз");
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
    }
}