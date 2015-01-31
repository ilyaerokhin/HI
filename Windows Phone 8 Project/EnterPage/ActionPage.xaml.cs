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
using System.Threading.Tasks;

namespace EnterPage
{
    public partial class ActionPage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask cameraCaptureTask;
        string photo;
        public ActionPage()
        {
            InitializeComponent();
            Title.Text = "Hello, " + PublicData.Username.ToLower()+"!";

            // для фоток 
            //*********************************************************************************
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Comp);
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Comp);
            //**********************************************************************************
            //установка фотографии
            BitmapImage bmp = new BitmapImage(new Uri("http://109.120.164.212/photos/" + PublicData.Username.ToLower() + ".jpg"  + "?" + Guid.NewGuid().ToString(), UriKind.RelativeOrAbsolute));
            MyPhoto.Source = bmp;
            //MyPhoto.Stretch =System.Windows.Media.Stretch.Uniform;

            //list1.DataContext = PublicData.p1;
        }

        //Функция таймера
        

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SocketClient client = new SocketClient();                                              // создаём сокет
            string rez = client.Connect(PublicData.IP, PublicData.PORT);                           // подключаемся к серверу

            PublicData.p1 = new List<Friend>();                                                    // создаём список друзей

            client.Send("<lf/" + PublicData.Username.ToLower() + "/" + PublicData.Password + ">"); // запрашиваем с сервера список друзей пользователя
            string result = client.Receive();                                                      // получаем ответ

            string[] friends = result.Split(new Char[] { '/', '|', '<', '>' });                    // выдёргиваем из ответа строки между разделителями

            // флажок нужен для пропуска первого захода
            bool flag = true;

            // в цикле заполняем список друзей
            foreach (string s in friends)
            {
                // если строка не пустая
                if (s.Trim() != "")
                {
                    // исключаем первый заход
                    if (flag != true)
                    {
                        list1.DataContext = PublicData.p1;
                        client.Send("<gs/" + s + ">");                                             // запрос на получение статуса друга
                        result = client.Receive();                                                 // получение ответа сервера

                        // !!!!!!
                        // ТУТ НУЖНО ОБРАБОТАТЬ ВАРИАНТЫ ПУСТОГО ОТВЕТА И ОТВЕТА ОБ ОШИБКЕ
                        // !!!!!!

                        string[] status = result.Split(new Char[] { '<', '/', '>' });              // выдёргиваем из ответа статус (status[2])

                        client.Send("<fc/" + s + ">");                                             // запрос на получение других данных пользователя
                        result = client.Receive();                                                 // получение ответа сервера
                        string[] data = result.Split(new Char[] { '/', '<', '>' });                // выдёргиваем строки из ответа
                        double lat = Convert.ToDouble(data[3]);
                        double lng = Convert.ToDouble(data[4]);
                        // если статус пусто;
                        if (status[2].CompareTo("@") == 0)
                        {
                            // добавляем пользователя с его новыми данными с пустым статусом
                            PublicData.p1.Add(new Friend() { name = s, status = "", date = PublicData.DateSearch(data[5]), distance = PublicData.latlng2distance(PublicData.Latitude, PublicData.Longitude, lat, lng), ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() });
                        }
                        // если статус не пустой
                        else
                        {
                            // добавляем пользователя со всеми его новыми данными
                            PublicData.p1.Add(new Friend() { name = s, status = status[2], date = PublicData.DateSearch(data[5]), distance = PublicData.latlng2distance(PublicData.Latitude, PublicData.Longitude, lat, lng), ImagePath = "http://109.120.164.212/photos/" + s + ".jpg" + "?" + Guid.NewGuid().ToString() });
                        }
                    }
                    else
                    {
                        // смена флага
                        flag = false;
                    }
                }
            }
            list1.DataContext = PublicData.p1;
            client.Close();
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

        private void Refresh()
        {
            NavigationService.Navigate(new Uri("/ActionPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }

        public EventHandler<PhotoResult> photoChooserTask_Completed { get; set; }

        //*******************************************************************************
        //Действия пользователя
        //********************************************************************************
        private void Camera_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameraCaptureTask.Show();
        }
        private void Galery_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask.Show();
        }
        private void Delete_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BitmapImage bm = new BitmapImage(new Uri("/Resources/no_photo.jpg", UriKind.RelativeOrAbsolute));
            MyPhoto.Source = bm;

            SocketClient photoclient = new SocketClient();

            string rez = photoclient.Connect(PublicData.IP, 5000);

            photoclient.SendFile(bm, PublicData.Username.ToLower() + ".jpg");
            photoclient.Close();
        }
        
        private void Exit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
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
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Task.Delay(100);
            if (MessageBox.Show("Вы хотите покинуть приложение?", "Выход", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }
        //********************************************************************
        //Добавление друга
        private void add_friend_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Focus();
            SocketClient client = new SocketClient();
            string rez = client.Connect(PublicData.IP, PublicData.PORT);

            if (!rez.Contains("Success"))
            {
                MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                client.Close();
                return;
            }
            client.Send("<af/" + PublicData.Username.ToLower() + "/" + PublicData.Password + "/" + Friend_box.Text.ToLower() + ">");
            string result = client.Receive();
            if (result.Contains("<af/bad>"))
            {
                MessageBox.Show("Пользователь не найден");
            }
            client.Close();

            this.Friend_box.Text = "";
            this.Refresh();
        }
        //Статус
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SocketClient client = new SocketClient();
            string rez = client.Connect(PublicData.IP, PublicData.PORT);

            if (!rez.Contains("Success"))
            {
                MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                client.Close();
                return;
            }
            client.Send("<ss/" + PublicData.Username.ToLower() + "/" + PublicData.Password + "/" + status.Text + ">");
            string result = client.Receive();
            if (result.Contains("<ss/bad>"))
            {
                MessageBox.Show("Статус может содержать не более 256 символов");
            }

            client.Close();
        }

        

        // Метод для вывода верной формы существительного, по склонению


        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Follow_page.xaml", UriKind.Relative));
        }


        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            SocketClient client = new SocketClient();                                              // создаём сокет
            string rez = client.Connect(PublicData.IP, PublicData.PORT);                           // подключаемся к серверу
            client.Send("<df/" + PublicData.Username + "/" + PublicData.Password + "/" +PublicData.Search_friend + ">");                                             // запрос на получение других данных пользователя
            string result = client.Receive();                                                 // получение ответа сервера
            client.Close();

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
               NavigationService.Navigate(new Uri("/Follow_page.xaml", UriKind.Relative));
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
                MessageBox.Show("Загрузка фотографии не требуется, так как Вы не изменяли её");
            }
            else
            {
                SocketClient client = new SocketClient();                          //

                string rez = client.Connect(PublicData.IP, 5000);

                client.SendFile(photo, PublicData.Username.ToLower() + ".jpg");
                client.Close();
                MessageBox.Show("Фотография загружена на сервер");
            }
        }
    }
}