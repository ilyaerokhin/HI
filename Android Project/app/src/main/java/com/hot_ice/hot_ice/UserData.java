package com.hot_ice.hot_ice;


import java.util.ArrayList;

public class UserData {
    public static String Name = null;
    public static String Password = null;
    public static String PhotoPath;
    public static ArrayList<Friend> ListFriends= new ArrayList<Friend>();
    public static double Latitude;
    public static double Longitude;
    public static String location;
    public static String[] friendList;
    public static double latitude;
    public static double longitude;

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
            case "lf":
                message="<"+param[0]+"/"+param[1]+"/"+param[2]+">";
                break;
            case "fd":
                message="<"+param[0]+"/"+param[1]+">";
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



        return dist+"м";
    }
    /*public static String DateSearch(String dateString)// поиск интервала времени
    {
        long dateValue;// тип для расчета
        Date dateValue2 = new Date();
        String format;
        if (dateString.toCharArray()[8] == ' ')
        {
            format = "ddd MMM  d HH:mm:ss yyyy";// формат даты
        }
        else
        {
            format = "ddd MMM dd HH:mm:ss yyyy";// формат даты
        }

        dateValue = Date.parse(dateString);//парсим строку в дату(in, )
        //long diff1 = dateValue2.getTime() - dateValue.getTime();

        //String result=(diff1 / (60 * 1000) % 60)+"минут";
       // return result;
    }*/
}

