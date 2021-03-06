﻿using System;
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
        Requests request;
        public MainPage()
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
            NavigationService.Navigate(new Uri("/MainPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername() && ValidatePassword())
            {
                int value = request.CheckUser(Username.Text, Password.Password);

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
                            }
                        }
                    }
                    User.Name = Username.Text.ToLower();
                    User.Password = Password.Password;

                    while (SetCoordinates() != 0) ;
                    request.Close();
                    NavigationService.Navigate(new Uri("/ActionPage.xaml", UriKind.Relative));
                }

                if (value == 1)
                {
                    MessageBox.Show("Incorrect username");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                }

                if (value == 2)
                {
                    MessageBox.Show("Incorrect password");
                    Username.Text = string.Empty;
                    Password.Password = string.Empty;
                }

                if (value < 0)
                {
                    MessageBox.Show("Try again");
                    Refresh();
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
                MessageBox.Show("Please enter nickname");
                return false;
            }

            return true;
        }
        private bool ValidatePassword()
        {
            if (String.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Please enter password");
                return false;
            }

            return true;
        }
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Task.Delay(100);
            if (MessageBox.Show("Do you want to leave the application?", "Exit", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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
                User.Latitude = coord.Latitude;                   // Сохраняем широту
                User.Longitude = coord.Longitude;                 // Сохраняем долготу
                int value = request.SetCoordinates(User.Name,User.Latitude,User.Longitude);
                if (value != 0)
                {
                    MessageBox.Show("Error, try again");
                    Refresh();
                }
                watcher.Stop();
            }
            else
            {
                return 1;
            }
            return 0;
        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            request.Close();
            NavigationService.Navigate(new Uri("/RegPage.xaml", UriKind.Relative));
        }
    }
}