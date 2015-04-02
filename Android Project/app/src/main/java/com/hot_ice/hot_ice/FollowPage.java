package com.hot_ice.hot_ice;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.widget.ImageView;
import android.widget.TextView;


public class FollowPage extends ActionBarActivity {
    SharedPreferences sPref;
    Friend Followfriend;
    public FollowPage(){
        Followfriend = new Friend("","","","","");
    }

    public ImageLoader imageLoader;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_follow_page);

        loadText();
        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setIcon(R.drawable.ic72);
        actionBar.setTitle(Followfriend.name);

        TextView distance = (TextView)findViewById(R.id.distance);
        distance.setText(Followfriend.distance);
        TextView time = (TextView)findViewById(R.id.time);
        time.setText(Followfriend.time);
        ImageView image=(ImageView)findViewById(R.id.ivImage);
        imageLoader =new ImageLoader(getApplicationContext());
        imageLoader.DisplayImage(Followfriend.imagePath, image);
    }

    void loadText() {
        sPref = getSharedPreferences("main",MODE_PRIVATE);
        Followfriend.name = sPref.getString("FriendName", "");
        Followfriend.status  = sPref.getString("FriendStatus", "");
        Followfriend.distance =sPref.getString("FriendDistance","");
        Followfriend.time      =sPref.getString("FriendTime","");
        Followfriend.imagePath =sPref.getString("FriendImagePath","");
    }

}
