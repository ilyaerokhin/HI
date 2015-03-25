package com.hot_ice.hot_ice;

/**
 * Created by shlya_000 on 24.03.2015.
 */
public class Friend {
    public static String name;
    public static String status;
    public static String distance;
    public static String time;
    public static String imagePath;

    Friend(String name, String status, String time,String distance,String imagePath){

        this.name       = name;
        this.status     = status;
        this.time       = time;
        this.distance   = distance;
        this.imagePath  = imagePath;
    }
}
