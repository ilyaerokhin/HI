package com.hot_ice.hot_ice;

/**
 * Created by shlya_000 on 24.03.2015.
 */
public  class Friend {
    public  String name="";
    public  String status="";
    public  String distance="";
    public  String time="";
    public  String imagePath="";

    Friend(String name, String status, String time,String distance,String imagePath){

        this.name       = name;
        this.status     = status;
        this.time       = time;
        this.distance   = distance;
        this.imagePath  = imagePath;
    }
}
