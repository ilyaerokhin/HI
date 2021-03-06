﻿using System;
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
using System.Threading.Tasks;

namespace EnterPage
{
    public partial class ActionPage : PhoneApplicationPage
    {
            
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask cameraCaptureTask;
        string photo;
        Requests request;
        public ActionPage()
        {
            InitializeComponent();

            Title.Text = "Hello, " + User.Name.ToLower()+"!";

            // для фоток 
            //*********************************************************************************
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Comp);
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Comp);
            //**********************************************************************************
            //установка фотографии
            BitmapImage bmp = new BitmapImage(new Uri("http://109.120.164.212/photos/" + User.Name.ToLower() + ".jpg"  + "?" + Guid.NewGuid().ToString(), UriKind.RelativeOrAbsolute));
            MyPhoto.Source = bmp;
        }
        private void Refresh()
        {
            request.Close();
            NavigationService.Navigate(new Uri("/ActionPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
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

            User.ListFriends = new List<Friend>();                                                    // создаём список друзей

            string value = request.GetListFriends(User.Name, User.Password);
            string[] friends = value.Split(new Char[] { '/' });                    // выдёргиваем из ответа строки между разделителями

            foreach (string s in friends)
            {
                // если строка не пустая
                if (s.Trim() != "")
                {
                    list1.DataContext = User.ListFriends;
                    string friendstatus = request.GetStatus(s); // получаем статус друга
                    string datafriends = request.GetFrendCoordinates(s); // получаем данные о друге

                    if (status == null || datafriends == null)
                    {
                        MessageBox.Show("Error in geting information about friends, try again");
                        Refresh();
                    }


                    string[] data = datafriends.Split(new Char[] { '/' });                // выдёргиваем строки из ответа
                    double lat = Convert.ToDouble(data[0]);
                    double lng = Convert.ToDouble(data[1]);
                    // если статус пуст;
                    if (friendstatus.CompareTo("@") == 0)
                    {
                        // добавляем пользователя с его новыми данными с пустым статусом
                        User.ListFriends.Add(new Friend() { name = s, status = "", date = PublicData.DateSearch(data[2]), distance = PublicData.latlng2distance(User.Latitude, User.Longitude, lat, lng), ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() });
                    }
                    // если статус не пустой
                    else
                    {
                        // добавляем пользователя со всеми его новыми данными
                        User.ListFriends.Add(new Friend() { name = s, status = friendstatus, date = PublicData.DateSearch(data[2]), distance = PublicData.latlng2distance(User.Latitude, User.Longitude, lat, lng), ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() });
                    }
                }
            }



            User.ListPotential = new List<Potential>();                                                    // создаём список друзей
            string potentialfriends = request.GetListPotential(User.Name, User.Password);
            string[] potential = potentialfriends.Split(new Char[] { '/' });                    // выдёргиваем из ответа строки между разделителями

            // в цикле заполняем список друзей
            foreach (string s in potential)
            {
                // если строка не пустая
                if (s.Trim() != "")
                {
                    list_potential.DataContext = User.ListPotential;
                    User.ListPotential.Add(new Potential() { name = s, ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() });
                }
            }
            list_potential.DataContext = User.ListPotential;


            User.ListPeople = new List<People>();                                                    // создаём список друзей
            string peoplearound = request.GetListPeople(User.Name, User.Latitude, User.Longitude);
            string[] around = peoplearound.Split(new Char[] { '/' });                    // выдёргиваем из ответа строки между разделителями

            foreach (string s in around)
            {
                // если строка не пустая
                if (s.Trim() != "" && !s.Equals(User.Name))
                {
                    list_people.DataContext = User.ListPeople;
                    string datapeople = request.GetFrendCoordinates(s); // получаем данные о друге

                    if (datapeople == null)
                    {
                        MessageBox.Show("Error in geting information about people, try again");
                        Refresh();
                    }


                    string[] data = datapeople.Split(new Char[] { '/' });                // выдёргиваем строки из ответа
                    double lat = Convert.ToDouble(data[0]);
                    double lng = Convert.ToDouble(data[1]);
           
                    User.ListPeople.Add(new People() { name = s, date = PublicData.DateSearch(data[2]), distance = PublicData.latlng2distance(User.Latitude, User.Longitude, lat, lng), ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() }); 
                }
            }
            list_people.DataContext = User.ListPeople;
        }
        
        // Функции к галерее и фотику
        //*********************************************************************************
        void photoChooserTask_Comp(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                MyPhoto.Source = bmp;

                photo = e.OriginalFileName;
            }
        }
        void cameraCaptureTask_Comp(object sender, PhotoResult e)
        {

            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                MyPhoto.Source = bmp;

                photo = e.OriginalFileName;
            }
        }
        public EventHandler<PhotoResult> photoChooserTask_Completed { get; set; }
        private void Camera_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameraCaptureTask.Show();
        }
        private void Galery_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask.Show();
        }
        //********************************************************************

        private void Delete_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BitmapImage bm = new BitmapImage(new Uri("/Resources/no_photo.jpg", UriKind.RelativeOrAbsolute));
            MyPhoto.Source = bm;

            SocketClient photoclient = new SocketClient();
            string rez = photoclient.Connect(request.GetIP(), 5000);
            photoclient.SendFile(bm, User.Name.ToLower() + ".jpg");
            photoclient.Close();
        }
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Task.Delay(100);
            if (MessageBox.Show("You want to leave the application?", "Exit", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }
        private void add_friend_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Focus();
            int value = request.AddFriend(User.Name, User.Password, Friend_box.Text.ToLower());

            if (value == 0)
            {
                MessageBox.Show("To user with the nickname: " + Friend_box.Text.ToLower() + " the request to friends was send");
                this.Refresh();
                return;
            }

            if(value == 1)
            {
                MessageBox.Show("You can't add user to the friends " + Friend_box.Text.ToLower());
                this.Friend_box.Text = "";
                return;
            }

            if (value == -1)
            {
                MessageBox.Show("No connection to the server");
                this.Refresh();
                return;
            }

            if (value == -2)
            {
                MessageBox.Show("Retry the request");
                this.Friend_box.Text = "";
                return;
            }

            if (value == -3)
            {
                MessageBox.Show("Unexpected error");
                this.Refresh();
                return;
            }
        }
        private void SetStatus_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int value = request.SetStatus(User.Name, User.Password, status.Text);
            if (value == 0)
            {
                MessageBox.Show("Status was chenged successfully");
            }
            else
            {
                MessageBox.Show("Unable to change the status");
            }

            status.Text = "";
        }
        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            request.Close();
            NavigationService.Navigate(new Uri("/FollowPage.xaml", UriKind.Relative));
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            int value = request.DeleteFriend(User.Name, User.Password, PublicData.Search_friend);

            if (value != 0)
            {
                MessageBox.Show("User not found");
            }
            else
            {
                MessageBox.Show("User with the nickname: " + PublicData.Search_friend + " successfully removed from friends");
            }

            this.Focus();
            this.Refresh();
        }
        private void Profile_panel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           object wantedNode = ((StackPanel)sender).FindName("name");
           if (wantedNode is TextBlock)
           {
               TextBlock wantedChild = wantedNode as TextBlock;
               PublicData.Search_friend = wantedChild.Text;
               request.Close();
               NavigationService.Navigate(new Uri("/FollowPage.xaml", UriKind.Relative));
           }
            
        }
        private void Profile_panel_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            object wantedNode = ((StackPanel)sender).FindName("name");
            if (wantedNode is TextBlock)
            {
                TextBlock wantedChild = wantedNode as TextBlock;
                PublicData.Search_friend = wantedChild.Text;
            }
        }
        private void Load_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(photo))
            {
                MessageBox.Show("Loading photo is not required");
            }
            else
            {
                SocketClient client = new SocketClient();                          //
                string rez = client.Connect(request.GetIP(), 5000);
                client.SendFile(photo, User.Name.ToLower() + ".jpg");
                client.Close();
                MessageBox.Show("Photo uploaded to the server");
            }
        }
        private void ClickRefresh(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Exit_Tap(object sender, EventArgs e)
        {
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var file = appStorage.OpenFile("LS.txt", FileMode.Create))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine("0");
                        writer.WriteLine("0");
                    }
                }
            }
            request.Close();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int value = request.AddFriend(User.Name, User.Password, PublicData.Search_friend);

            if (value != 0)
            {
                MessageBox.Show("Fail");
            }
            else
            {
                MessageBox.Show("User with the nickname: " + PublicData.Search_friend + " was added to friends");
            }

            this.Refresh();
        }
           
    }
}