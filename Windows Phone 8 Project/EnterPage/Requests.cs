using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterPage
{
    class Requests
    {
        private SocketClient client;
        private const int PORT = 32000;
        private const string IP = "109.120.164.212";
        private bool IsConnect; // true если соединение удалось
        private string result;  // в переменной хранится результат последнего запроса

        public Requests()
        {
            result = "";
            client = new SocketClient();
            result = client.Connect(IP, PORT);

            if (!result.Contains("Success"))
            {
                IsConnect = false;
                return;
            }
            else
            {
                IsConnect = true;
            }
        }
        public void Close()
        {
            client.Close();
        }
        public int Add(string user, string password, string email)
        {
            if(IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            if(PublicData.IsValidEmail(email) == false)
            {
                return 1; // не валидный email
            }

            client.Send("<ad/" + user.ToLower() + "/" + password + "/" + email.ToLower() + ">");
            result = client.Receive();

            if (result.Contains("<ad/ok>"))
            {
                return 0; // новый пользователь создан
            }

            if (result.Contains("<ad/bad>"))
            {
                return 2; // неудача
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public int CheckUser(string user, string password)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            client.Send("<cu/" + user.ToLower() + "/" + password + ">");
            result = client.Receive();

            if (result.Contains("<cu/ok>"))
            {
                return 0; // логин и пароль верны
            }

            if (result.Contains("<cu/not>"))
            {
                return 1; // пользователь не зарегестрирован
            }

            if (result.Contains("<cu/bad>"))
            {
                return 2; // введён не верный пароль
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public int SetCoordinates(string user, double latitude, double longitude)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            client.Send("<uc/" + user.ToLower() + "/" + latitude.ToString() + "/" + longitude.ToString() + ">");
            result = client.Receive();

            if (result.Contains("<uc/ok>"))
            {
                return 0; // координаты обновлены успешно
            }

            if (result.Contains("<uc/not>"))
            {
                return 1; // пользователь не зарегестрирован
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public int SetCoordinates(string user, string latitude, string longitude)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            client.Send("<uc/" + user.ToLower() + "/" + latitude + "/" + longitude + ">");
            result = client.Receive();

            if (result.Contains("<uc/ok>"))
            {
                return 0; // координаты обновлены успешно
            }

            if (result.Contains("<uc/not>"))
            {
                return 1; // пользователь не зарегестрирован
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public string GetFrendCoordinates(string user)
        {
            if (IsConnect == false)
            {
                return null; // содинения с сервером нет
            }

            client.Send("<fc/" + user.ToLower() + ">");
            result = client.Receive();

            if (result.Contains("<fc/not>"))
            {
                return null; // пользователь не зарегестрирован
            }

            if (result.Contains("<bd>"))
            {
                return null; // некорректный запрос
            }

            string[] data = result.Split(new Char[] { '/', '<', '>' });

            return data[3].Replace('.',',') + "/" + data[4].Replace('.',',') + "/" + data[5]; //      latitude/longitude/date
        }
        public int AddFriend(string user, string password, string friend)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            client.Send("<af/" + user.ToLower() + "/" + password + "/" + friend.ToLower() + ">");
            result = client.Receive();

            if (result.Contains("<af/ok>"))
            {
                return 0; // заявка в друзья отправлена
            }

            if (result.Contains("<af/bad>"))
            {
                return 1; // неудача
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public int DeleteFriend(string user, string password, string friend)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            client.Send("<df/" + user.ToLower() + "/" + password + "/" + friend.ToLower() + ">");
            result = client.Receive();

            if (result.Contains("<df/ok>"))
            {
                return 0; // заявка в друзья отправлена
            }

            if (result.Contains("<df/bad>"))
            {
                return 1; // не верный пароль
            }

            if (result.Contains("<df/not>"))
            {
                return 2; // не верное имя пользователя
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public string GetListFriends(string user, string password)
        {
            if (IsConnect == false)
            {
                return null; // содинения с сервером нет
            }

            client.Send("<lf/" + user.ToLower() + "/" + password + ">");
            result = client.Receive();

            if (result.Contains("<lf/bad>"))
            {
                return null; // не верный пароль
            }

            if (result.Contains("<lf/not>"))
            {
                return null; // не верное имя пользователя
            }

            if (result.Contains("<bd>"))
            {
                return null; // некорректный запрос
            }

            string[] friends = result.Split(new Char[] { '/', '|', '<', '>' });

            // флажок нужен для пропуска первого захода
            bool flag = true;
            string str = "";

            // в цикле заполняем список друзей
            foreach (string s in friends)
            {
                // если строка не пустая
                if (s.Trim() != "")
                {
                    // исключаем первый заход
                    if (flag != true)
                    {
                        str = str + s + "/";
                    }
                    else
                    {
                        // смена флага
                        flag = false;
                    }
                }
            }

            return str; // friend1/friend2/../friendn/
        }
        public string GetListPotential(string user, string password)
        {
            if (IsConnect == false)
            {
                return null; // содинения с сервером нет
            }

            client.Send("<lp/" + user.ToLower() + "/" + password + ">");
            result = client.Receive();

            if (result.Contains("<lp/bad>"))
            {
                return null; // не верный пароль
            }

            if (result.Contains("<lp/not>"))
            {
                return null; // не верное имя пользователя
            }

            if (result.Contains("<bd>"))
            {
                return null; // некорректный запрос
            }

            string[] friends = result.Split(new Char[] { '/', '|', '<', '>' });

            // флажок нужен для пропуска первого захода
            bool flag = true;
            string str = "";

            // в цикле заполняем список друзей
            foreach (string s in friends)
            {
                // если строка не пустая
                if (s.Trim() != "")
                {
                    // исключаем первый заход
                    if (flag != true)
                    {
                        str = str + s + "/";
                    }
                    else
                    {
                        // смена флага
                        flag = false;
                    }
                }
            }

            return str; // friend1/friend2/../friendn/
        }
        public string GetPassword(string user)
        {
            if (IsConnect == false)
            {
                return null; // содинения с сервером нет
            }

            client.Send("<gp/" + user.ToLower() +  ">");
            result = client.Receive();

            if (result.Contains("<gp/bad>"))
            {
                return null; // нет такого пользователя
            }

            string[] data = result.Split(new Char[] { '/', '<', '>' });

            return data[2]; // при удаче, возвращается почта, куда отправился пароль
        }
        public int SetStatus(string user, string password, string status)
        {
            if (IsConnect == false)
            {
                return -1; // содинения с сервером нет
            }

            if (status == string.Empty)
            {
                client.Send("<ss/" + user.ToLower() + "/" + password + "/@>");
            }
            else
            {
                client.Send("<ss/" + user.ToLower() + "/" + password + "/" + status + ">");
            }

            result = client.Receive();

            if (result.Contains("<ss/ok>"))
            {
                return 0; // статус изменён
            }

            if (result.Contains("<bd>"))
            {
                return -2; // некорректный запрос
            }

            return -3; // непредвиденная ошибка
        }
        public string GetStatus(string user)
        {
            if (IsConnect == false)
            {
                return null; // содинения с сервером нет
            }

            client.Send("<gs/" + user.ToLower() + ">");
            result = client.Receive();

            if (result.Contains("<gs/bad>"))
            {
                return null; // нет такого пользователя
            }

            string[] data = result.Split(new Char[] { '/', '<', '>' });

            return data[2]; // при удаче, возвращается статус пользователя
        }
        public bool isConnecting()
        {
            return IsConnect;
        }
        public string GetIP()
        {
            return IP;
        }
    }
}
