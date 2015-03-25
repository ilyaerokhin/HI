package com.hot_ice.hot_ice;

import android.content.Intent;
import android.content.SharedPreferences;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.widget.ImageView;
import android.widget.ProgressBar;


public class NavigationPage extends ActionBarActivity {

    public static UserData User;

    ProgressBar myProgressBar;
    String longitude;
    String latitude;
    LocationManager locationManager;
    Location location;
    SharedPreferences sPref;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_navigation_page);


        ActionBar actionBar = getSupportActionBar();
        actionBar.hide();

        ImageView imgView = (ImageView) findViewById(R.id.icon);
        imgView.setImageResource(R.drawable.ic);

        myProgressBar = (ProgressBar) findViewById(R.id.progressBar1);
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);
    }



    @Override
    protected void onResume() {
        super.onResume();
        SimpleSemaphore semaphore = new SimpleSemaphore();
        locationManager.requestLocationUpdates(
                LocationManager.NETWORK_PROVIDER, 0, 10,
                locationListener);
        loadText();
        if(User.Name.equals("") || User.Password.equals(""))
        {
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    Intent intent = new Intent(getApplicationContext(), ResolutionPage.class);
                    startActivity(intent);
                }
            }, 3000);
        }
        else {
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {

                    new ConnectTask().execute(User.createMessage("uc",User.Name,latitude,longitude));
                }
            }, 3000);

            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    Intent intent = new Intent(getApplicationContext(), ActionPage.class);
                    startActivity(intent);
                }
            }, 7000);
        }


    }

    @Override
    protected void onPause() {
        super.onPause();
        locationManager.removeUpdates(locationListener);
        android.os.Process.killProcess(android.os.Process.myPid());
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        android.os.Process.killProcess(android.os.Process.myPid());
    }

    private LocationListener locationListener = new LocationListener() {

        @Override
        public void onLocationChanged(Location location) {
            receiveLocation(location);
        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {

        }

        @Override
        public void onProviderDisabled(String provider) {
        }

        @Override
        public void onProviderEnabled(String provider) {
            receiveLocation(locationManager.getLastKnownLocation(provider));
        }

    };

    private Void receiveLocation(Location location) {
	
        if (location != null) {
            longitude = Double.toString(location.getLongitude());
            latitude = Double.toString(location.getLatitude());
        }
        return null;
    }

    void loadText() {
        sPref = getSharedPreferences("main",MODE_PRIVATE);
        User.Name     = sPref.getString("Name", "");
        User.Password = sPref.getString("Password", "");
    }

}
