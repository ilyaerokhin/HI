package com.hot_ice.hot_ice;

import android.util.Log;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.TimeZone;
import java.util.concurrent.TimeUnit;

public class UserData {
    public static int index;
    public static Friend followFriend;
    public static String Name = null;
    public static String Password = null;
    public static ArrayList<Friend> ListFriends= new ArrayList<Friend>();
    public static ArrayList<People> ListPeople= new ArrayList<People>();
    public static ArrayList<Requests> ListRequests= new ArrayList<Requests>();
    public static String latitude;
    public static String longitude;

    public static People addPeople;
    public static Friend deleteFriend;
    public static Requests request;
    public static String Status;

    public static String createMessage(String... param)
    {
        String message=null;
        switch(param[0])
        {
            case "uc":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
                break;
            case "ad":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
            case "cu":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+">";
                break;
            case "gp":
                message="<"+param[0]+"/"+param[1]+">";
                break;
            case "fl":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+">";
                break;
            case "fd":
                message="<"+param[0]+"/"+param[1]+">";
                break;
            case "pl":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
                break;
            case "lp":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+">";
                break;
            case "af":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
                break;
            case "df":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
                break;
            case "ss":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+"/"+param[3]+">";
                break;
            default:

                break;

        }

        return message;

    }


    static public String latlng2distance(double lat1, double long1, double lat2, double long2)
    {
        //радиус Земли
        double R = 6372795;

        //перевод коордитат в радианы
        lat1 *= Math.PI / 180;
        lat2 *= Math.PI / 180;
        long1 *= Math.PI / 180;
        long2 *= Math.PI / 180;

        //вычисление косинусов и синусов широт и разницы долгот
        double cl1 = Math.cos(lat1);
        double cl2 = Math.cos(lat2);
        double sl1 = Math.sin(lat1);
        double sl2 = Math.sin(lat2);
        double delta = long2 - long1;
        double cdelta = Math.cos(delta);
        double sdelta = Math.sin(delta);

        //вычисления длины большого круга
        double y = Math.sqrt(Math.pow(cl2 * sdelta, 2) + Math.pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
        double x = sl1 * sl2 + cl1 * cl2 * cdelta;
        double ad = Math.atan2(y, x);
        double dist = ad * R; //расстояние между двумя координатами в метрах


        if(dist<1000)
            return (int)dist+" m";
        return (int)(dist/1000)+" km";
    }
    public static String DateSearch(String dateString)// поиск интервала времени
    {
        String result;
        Date dateValue;// тип для расчета
        Date dateValue2 = new Date();
        String format;
        if (dateString.toCharArray()[8] == ' ')
        {
            format = "E MMM  d HH:mm:ss yyyy";// формат даты
        }
        else
        {
            format = "E MMM dd HH:mm:ss yyyy";// формат даты
        }
        TimeZone tz = TimeZone.getDefault();
        long hours = TimeUnit.MILLISECONDS.toHours(tz.getRawOffset());
        Log.e("dateString", dateString);
        try
        {
            Log.e("dateString", "test1");
            DateFormat df = new SimpleDateFormat(format);
            dateValue = df.parse(dateString);
            Log.e("dateString", "test2");
            long diff = dateValue2.getTime() - dateValue.getTime();
            if (hours == 3)
            {
                if ((diff/1000) < 60)// возвращение строки
                {
                    result = " now";
                    return result;
                }
                else
                if ((diff/(1000*60)) < 60)
                {
                    result = (int)(diff/(1000*60))+ " mins ago";
                    return result;
                }
                else if ((diff/(1000*60*60)) < 24)
                {
                    result = (int)(diff/(1000*60*60)) +" hours ago";
                    return result;
                }
                else
                if ((diff/(1000*60*60*24)) < 365)
                {
                    result = (int)(diff/(1000*60*60*24))+" days ago";
                    return result;
                }
            }
            else
            {
                if (hours > 3)
                {
                    if ((diff/1000) - (hours - 3) * 3600 < 60)// возвращение строки
                    {
                        result = " now";
                        return result;
                    }
                    else
                    if ((diff/(1000*60)) - (hours - 3) * 60 < 60)
                    {
                        result = (int)((diff/(1000*60)) - (hours - 4) * 60) + " mins ago";
                        return result;
                    }
                    else if ((diff/(1000*60*60)) - (hours - 3) < 24)
                    {
                        result = (int)((diff/(1000*60*60)) - (hours - 4)) + " hours ago";
                        return result;
                    }
                    else
                    if ((diff/(1000*60*60*24)) - (hours - 3) * 1 / 24 < 365)
                    {
                        result = (int)((diff/(1000*60*60*24)) - (hours - 4) * 1 / 24) + " days ago";
                        return result;
                    }
                }
                else
                {
                    if (hours < 3)
                    {
                        if ((diff/1000) + (-1) * (hours - 3) * 3600 < 60)// возвращение строки
                        {
                            result = "сейчас";
                            return result;
                        }
                        else
                        if ((diff/(1000*60)) + (-1) * (hours - 3) * 60 < 60)
                        {
                            result = (int)((diff/(1000*60)) + (-1) * (hours - 4) * 60) + " mins ago";
                            return result;
                        }
                        else if ((diff/(1000*60*60)) + (-1) * (hours - 3) < 24)
                        {
                            result = (int)((diff/(1000*60*60)) + (-1) * (hours - 4)) + " hours ago";
                            return result;
                        }
                        else
                        if ((diff/(1000*60*60*24)) + (-1) * (hours - 3) * 1 / 24 < 365)
                        {
                            result = (int)((diff/(1000*60*60*24)) + (-1) * (hours - 4) * 1 / 24) + " days ago";
                            return result;
                        }
                    }
                }
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return "";
    }
}

