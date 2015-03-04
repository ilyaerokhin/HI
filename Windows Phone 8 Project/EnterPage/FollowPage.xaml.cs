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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using Windows.Devices.Geolocation;
using System.Windows.Threading;
using System.Device.Location;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Windows.Media;

namespace EnterPage
{
    public partial class FollowPage : PhoneApplicationPage
    {
        Geolocator geo = null;
        Geoposition pos = null;

        static double lat;
        static double lng;
        static double priv_dist;
        static double dist;
        static double first;
        string friend_time;

        byte red;
        byte blue;
        byte green;

        DispatcherTimer TimerRed;
        DispatcherTimer TimerGreen;
        DispatcherTimer TimerBlue;
        DispatcherTimer Timer;
        DispatcherTimer TimerNET;

        static Requests request;
        public FollowPage()
        {
            InitializeComponent();
            request = new Requests();

            if (request.isConnecting() == false)
            {
                MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }

            geo = new Geolocator();
            NAME.Text = "WE ARE FOLLOWING FOR "+ PublicData.Search_friend.ToUpper();
            Image_friend.Source = new BitmapImage(new Uri("http://109.120.164.212/photos/" + PublicData.Search_friend.ToLower() + ".jpg" + "?" + Guid.NewGuid().ToString()));

            Colors.red = 204;
            Colors.blue = 119;
            Colors.green = 34;

            string value = request.GetFrendCoordinates(PublicData.Search_friend);                                             // получение ответа сервера
            string[] data = value.Split(new Char[] { '/' });                // выдёргиваем строки из ответа

            lat = Convert.ToDouble(data[0]);
            lng = Convert.ToDouble(data[1]);
            friend_time = PublicData.DateSearch(data[2]);

            TimerGreen = new DispatcherTimer();
            TimerGreen.Interval = TimeSpan.FromMilliseconds(10);
            TimerGreen.Tick += OnTimerGreenTick;

            TimerBlue = new DispatcherTimer();
            TimerBlue.Interval = TimeSpan.FromMilliseconds(10);
            TimerBlue.Tick += OnTimerBlueTick;

            TimerRed = new DispatcherTimer();
            TimerRed.Interval = TimeSpan.FromMilliseconds(10);
            TimerRed.Tick += OnTimerRedTick;

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Tick += TimerTick;

            TimerNET = new DispatcherTimer();
            TimerNET.Interval = TimeSpan.FromSeconds(30);
            TimerNET.Tick += TimerNETTick;

            Timer.Start();
            TimerNET.Start();
            
        }
        private void Refresh()
        {
            NavigationService.Navigate(new Uri("/FollowPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }
        async void TimerTick(Object sender, EventArgs args)
        {
            pos = await geo.GetGeopositionAsync();

            User.Latitude = pos.Coordinate.Point.Position.Latitude;
            User.Longitude = pos.Coordinate.Point.Position.Longitude;

            distance.Text = PublicData.latlng2distance_moreinfo(User.Latitude, User.Longitude, lat, lng);
            time.Text = friend_time;

            if (first == null)
            {
                first = PublicData.latlng2distance_meters(User.Latitude, User.Longitude, lat, lng);
                priv_dist = first;
            }
            else
            {
                dist = PublicData.latlng2distance_meters(User.Latitude, User.Longitude, lat, lng);
                Change_Image(dist, priv_dist, first);
                priv_dist = dist;
            }
        }
        public void TimerNETTick(Object sender, EventArgs args)
        {
            int value = request.SetCoordinates(User.Name, User.Latitude, User.Longitude);
            
            if(value != 0)
            {
                Refresh();
            }

            string value2 = request.GetFrendCoordinates(PublicData.Search_friend);                                             // получение ответа сервера
            string[] data = value2.Split(new Char[] { '/' });                // выдёргиваем строки из ответа

            lat = Convert.ToDouble(data[0]);
            lng = Convert.ToDouble(data[1]);
            friend_time = PublicData.DateSearch(data[2]);                                     // закрываем соединение
        }

        void OnTimerRedTick(Object sender, EventArgs args)
        {
            if (Colors.red == red)
            {
                TimerRed.Stop();
                return;
            }
            else if (Colors.red > red)
            {
                Colors.red--;
            }
            else
            {
                Colors.red++;
            }

            Top.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, Colors.red, Colors.green, Colors.blue));
        }
        void OnTimerBlueTick(Object sender, EventArgs args)
        {
            if (Colors.blue == blue)
            {
                TimerBlue.Stop();
                return;
            }
            else if (Colors.blue > blue)
            {
                Colors.blue--;
            }
            else
            {
                Colors.blue++;
            }

            Top.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, Colors.red, Colors.green, Colors.blue));
        }
        void OnTimerGreenTick(Object sender, EventArgs args)
        {
            if (Colors.green == green)
            {
                TimerGreen.Stop();
                return;
            }
            else if (Colors.green > green)
            {
                Colors.green--;
            }
            else
            {
                Colors.green++;
            }


            Top.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, Colors.red, Colors.green, Colors.blue));
        }
        private void Change_Image(double length, double priv, double first)
        {
            double percent = first/100.0;
            if (length < priv)
            {
                double difference = first - length;
                double per = difference / percent;
                hot(per);
            }
            if (length > priv)
            {
                double difference = length - first;
                double per = difference / percent;
                ice(per);
            }
        }

        private void hot(double condition)
         {
             if (condition < 20)
             {
                 red = 255;
                 green = 0;
                 blue = 0;
                 TimerRed.Start();
                 TimerGreen.Start();
                 TimerBlue.Start();
             }
             else if (condition < 40)
             {
                 red = 146;
                 green = 0;
                 blue = 0;
                 TimerRed.Start();
                 TimerGreen.Start();
                 TimerBlue.Start();
             }
             else if (condition < 60)
             {
                 red = 176;
                 green = 0;
                 blue = 0;
                 TimerRed.Start();
                 TimerGreen.Start();
                 TimerBlue.Start();
             }
             else if (condition < 80)
             {
                 red = 128;
                 green = 0;
                 blue = 0;
                 TimerRed.Start();
                 TimerGreen.Start();
                 TimerBlue.Start();
             }
             else
             {
                 red = 90;
                 green = 0;
                 blue = 0;
                 TimerRed.Start();
                 TimerGreen.Start();
                 TimerBlue.Start();
             }
         }
        private void ice(double condition)
        {
            if (condition < 20)
            {
                red = 0;
                green = 255;
                blue = 255;

                TimerGreen.Start();
                TimerBlue.Start();
                TimerRed.Start();
            }
            else if (condition < 40)
            {
                red = 67;
                green = 110;
                blue = 238;
                TimerRed.Start();
                TimerGreen.Start();
                TimerBlue.Start();
            }
            else if (condition < 60)
            {
                red = 16;
                green = 78;
                blue = 139;
                TimerRed.Start();
                TimerGreen.Start();
                TimerBlue.Start();
            }
            else if (condition < 80)
            {
                red = 25;
                green = 25;
                blue = 112;
                TimerRed.Start();
                TimerGreen.Start();
                TimerBlue.Start();
            }
            else
            {
                red = 0;
                green = 0;
                blue = 139;
                TimerRed.Start();
                TimerGreen.Start();
                TimerBlue.Start();
            }
        }

    }
}