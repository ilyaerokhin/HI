using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EnterPage
{
    public partial class Forget : PhoneApplicationPage
    {
        public Forget()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUsername())
            {
                SocketClient client = new SocketClient();
                string rez = client.Connect(PublicData.IP, PublicData.PORT);

                if (!rez.Contains("Success"))
                {
                    MessageBox.Show("Неудаётся подключиться к серверу\nВозможно отсутствует подключение к интернету");
                    client.Close();
                    return;
                }

                client.Send("<gp/" + Username.Text.ToLower() +">");
                string result = client.Receive();
                client.Close();
                if (result.Contains("<gp/bad>"))
                {
                    Username.Text = string.Empty;
                    MessageBox.Show("Не удалось отправить заявку на восстановление пароля\nПроверьте правильность вводимых данных.");
                }
                else
                {
                    string[] email = result.Split(new Char[] { '/', '<', '>' });
                    MessageBox.Show("Мы отправили Ваш пароль по адресу: "+email[2]+"\nЕсли сообщение не пришло, проверьте папку со спамом");
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
    }
}