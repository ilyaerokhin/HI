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

namespace EnterPage
{
    public partial class ForgetPage : PhoneApplicationPage
    {
        Requests request;
        public ForgetPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
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
            request.Close();
            NavigationService.Navigate(new Uri("/ForgetPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername())
            {
                string value = request.GetPassword(Username.Text);

                if (value == null)
                {
                    Username.Text = string.Empty;
                    MessageBox.Show("Не удалось отправить заявку на восстановление пароля\nПроверьте правильность вводимых данных.");
                }
                else
                {
                    MessageBox.Show("Мы отправили Ваш пароль по адресу: "+value+"\nЕсли сообщение не пришло, проверьте папку со спамом");
                    Username.Text = string.Empty;
                }
            }
            else
            {
                Username.Text = string.Empty;
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
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            request.Close();
            NavigationService.Navigate(new Uri("/MainPage.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
        }
    }
}