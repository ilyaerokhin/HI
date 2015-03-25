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
using System.Text.RegularExpressions;

namespace EnterPage
{

    public class PublicData
    {
        public static string MyImagePath;
        public static string Search_friend;

        public static bool IsValidEmail(string inputEmail)
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

        static public string pluralForm(int n, string form1, string form2, string form5)
        {
            n = Math.Abs(n) % 100;
            int n1 = n % 10;
            if (n > 10 && n < 20) return form5;
            if (n1 > 1 && n1 < 5) return form2;
            if (n1 == 1) return form1;
            return form5;
        }

        static public string latlng2distance(double lat1, double long1, double lat2, double long2)
        {
            
            double dist = latlng2distance_meters(lat1,long1,lat2,long2);

            if ((int)dist >= 1000)
            {
                return (int)dist / 1000 + " " + pluralForm((int)dist / 1000, "km", "kms", "kms");
            }
            else
            {
                return (int)dist + " " + pluralForm((int)dist, "m", "ms", "ms");
            }
        }

        static public double latlng2distance_meters(double lat1, double long1, double lat2, double long2)
        {
            //радиус Земли
            double R = 6372795;

            //перевод коордитат в радианы
            lat1 *= Math.PI / 180;
            lat2 *= Math.PI / 180;
            long1 *= Math.PI / 180;
            long2 *= Math.PI / 180;

            //вычисление косинусов и синусов широт и разницы долгот
            double cl1 = Math.Cos(lat1);
            double cl2 = Math.Cos(lat2);
            double sl1 = Math.Sin(lat1);
            double sl2 = Math.Sin(lat2);
            double delta = long2 - long1;
            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);

            //вычисления длины большого круга
            double y = Math.Sqrt(Math.Pow(cl2 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
            double x = sl1 * sl2 + cl1 * cl2 * cdelta;
            double ad = Math.Atan2(y, x);
            double dist = ad * R; //расстояние между двумя координатами в метрах


            return dist;
        }

        static public string latlng2distance_moreinfo(double lat1, double long1, double lat2, double long2)
        {
            double dist = latlng2distance_meters(lat1,long1,lat2,long2);


            if ((int)dist >= 1000)
            {
                return (dist / 1000).ToString("#.##") + " " + pluralForm((int)dist / 1000, "km", "kms", "kms");
            }
            else
            {
                return (int)dist + " " + pluralForm((int)dist, "m", "ms", "ms");
            }
        }

        public static string DateSearch(string dateString)// поиск интервала времени
        {
            string result;
            DateTime dateValue;// тип для расчета
            DateTime dateValue2 = DateTime.Now;
            string format;
            if (dateString.ToCharArray()[8] == ' ')
            {
                format = "ddd MMM  d HH:mm:ss yyyy";// формат даты
            }
            else
            {
                format = "ddd MMM dd HH:mm:ss yyyy";// формат даты
            }

            try
            {
                dateValue = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);//парсим строку в дату
                System.TimeSpan diff1 = dateValue2.Subtract(dateValue);//считаем разницу

                if (TimeZoneInfo.Local.BaseUtcOffset.Hours == 4)
                {
                    if (diff1.TotalSeconds < 60)// возвращение строки 
                    {
                        result = "now";
                        return result;
                    }
                    else
                        if (diff1.TotalMinutes < 60)
                        {
                            result = (int)diff1.TotalMinutes + " " + PublicData.pluralForm((int)diff1.TotalMinutes, "minute", "minutes", "minutes") + " ago";
                            return result;
                        }
                        else if (diff1.TotalHours < 24)
                        {
                            result = (int)diff1.TotalHours + " " + PublicData.pluralForm((int)diff1.TotalHours, "hour", "hours", "hours") + " ago";
                            return result;
                        }
                        else
                            if (diff1.TotalDays < 365)
                            {
                                result = (int)diff1.TotalDays + " " + PublicData.pluralForm((int)diff1.TotalDays, "day", "days", "days") + " ago";
                                return result;
                            }
                }
                else
                {
                    if (TimeZoneInfo.Local.BaseUtcOffset.Hours > 4)
                    {
                        if (diff1.TotalSeconds - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 3600 < 60)// возвращение строки 
                        {
                            result = "now";
                            return result;
                        }
                        else
                            if (diff1.TotalMinutes - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60 < 60)
                            {
                                result = (int)(diff1.TotalMinutes - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60) + " " + PublicData.pluralForm((int)(diff1.TotalMinutes - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60), "minute", "minutes", "minutes") + " ago";
                                return result;
                            }
                            else if (diff1.TotalHours - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) < 24)
                            {
                                result = (int)(diff1.TotalHours - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4)) + " " + PublicData.pluralForm((int)(diff1.TotalHours - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4)), "hour", "hours", "hours") + " ago";
                                return result;
                            }
                            else
                                if (diff1.TotalDays - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24 < 365)
                                {
                                    result = (int)(diff1.TotalDays - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24) + " " + PublicData.pluralForm((int)(diff1.TotalDays - (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24), "day", "days", "days") + " ago";
                                    return result;
                                }
                    }
                    else
                    {
                        if (TimeZoneInfo.Local.BaseUtcOffset.Hours < 4)
                        {
                            if (diff1.TotalSeconds + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 3600 < 60)// возвращение строки 
                            {
                                result = "сейчас";
                                return result;
                            }
                            else
                                if (diff1.TotalMinutes + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60 < 60)
                                {
                                    result = (int)(diff1.TotalMinutes + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60) + " " + PublicData.pluralForm((int)(diff1.TotalMinutes + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 60), "minute", "minutes", "minutes") + " ago";
                                    return result;
                                }
                                else if (diff1.TotalHours + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) < 24)
                                {
                                    result = (int)(diff1.TotalHours + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4)) + " " + PublicData.pluralForm((int)(diff1.TotalHours + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4)), "hour", "hours", "hours") + " ago";
                                    return result;
                                }
                                else
                                    if (diff1.TotalDays + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24 < 365)
                                    {
                                        result = (int)(diff1.TotalDays + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24) + " " + PublicData.pluralForm((int)(diff1.TotalDays + (-1) * (TimeZoneInfo.Local.BaseUtcOffset.Hours - 4) * 1 / 24), "day", "days", "days") + " ago";
                                        return result;
                                    }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                return "no data";
            }
            return "";
        }
    }
}
