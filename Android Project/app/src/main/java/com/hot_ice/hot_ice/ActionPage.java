package com.hot_ice.hot_ice;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.ActionBarDrawerToggle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;

public class ActionPage extends ActionBarActivity {
    SharedPreferences sPref;
    private String[] mScreenTitles;
    private DrawerLayout mDrawerLayout;
    private ListView mDrawerList;
    private ActionBarDrawerToggle mDrawerToggle;
    private CharSequence mDrawerTitle;
    private CharSequence mTitle;
    public static String[] friendList;
    String parts;
    String[] Datafriend;
    ProgressBar progressBar;
    AlertDialog.Builder ad;
    String addFriend;
    LocationManager locationManager;
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_page);
        loadText();
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);
        locationManager.requestLocationUpdates(
                LocationManager.NETWORK_PROVIDER, 0, 10,
                locationListener);

        ad = new AlertDialog.Builder(this);

        ad.setTitle("Add Friend");
        ad.setMessage("Enter name friend");

        final EditText input = new EditText(this);
        ad.setView(input);

        ad.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                addFriend = input.getText().toString();
                new ConnectTask().execute(UserData.createMessage("af", UserData.Name, UserData.Password,addFriend));
            }
        });

        ad.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                // Если отменили.
            }
        });

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setTitle("Hello, " + UserData.Name + "!");

        progressBar = (ProgressBar) findViewById(R.id.prograssBar);
        progressBar.setVisibility(View.VISIBLE);

        getSupportActionBar().setDisplayHomeAsUpEnabled(false);
        getSupportActionBar().setHomeButtonEnabled(false);



        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {

                new ConnectTask().execute(UserData.createMessage("fl", UserData.Name, UserData.Password));
            }
        }, 0);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                parts = TCPClient.serverMessage.toString();
                int index = parts.indexOf(">");
                parts=parts.substring(4, index);
                String delimiter = "\\/";
                friendList=parts.split(delimiter);
                //arr2 = new Friend[3];
                if (parts.equals("bad")){}
                else
                    {
                        for(int i=0;i<friendList.length;i++)
                        {
                            Datafriend=friendList[i].split("\\|");
                            if(Datafriend[3].equals("@"))
                                Datafriend[3] = "";
                            UserData.ListFriends.add(new Friend(Datafriend[0], Datafriend[3],UserData.DateSearch(Datafriend[4]) ,
                                    UserData.latlng2distance(Double.parseDouble( Datafriend[1].replace(",",".")),Double.parseDouble( Datafriend[2].replace(",",".")),Double.parseDouble( UserData.latitude.replace(",",".")),Double.parseDouble( UserData.longitude.replace(",",".")) )
                                    ,"http://109.120.164.212/photos/"+Datafriend[0]+".jpg"));
                        }

                    }
                }
        }, 3500);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {

                new ConnectTask().execute(UserData.createMessage("pl", UserData.Name, UserData.latitude,UserData.longitude));
            }
        }, 4000);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                parts = TCPClient.serverMessage.toString();
                int index = parts.indexOf(">");
                parts=parts.substring(4, index);
                String delimiter = "\\/";
                friendList=parts.split(delimiter);
                for(int i=0;i<friendList.length;i++)
                {
                    Datafriend=friendList[i].split("\\|");
                    if (Datafriend[0].equals(UserData.Name)) {UserData.Status =Datafriend[3];}
                    else
                    {
                        if(Datafriend[3].equals("@"))
                            Datafriend[3] = "";
                        UserData.ListPeople.add(new People(Datafriend[0], Datafriend[3], UserData.DateSearch(Datafriend[4]),
                                UserData.latlng2distance(Double.parseDouble( Datafriend[1].replace(",",".")),Double.parseDouble( Datafriend[2].replace(",",".")),Double.parseDouble( UserData.latitude.replace(",",".")),Double.parseDouble( UserData.longitude.replace(",",".")) ),
                                "http://109.120.164.212/photos/" + Datafriend[0] + ".jpg"));
                    }
                }

            }
        }, 5000);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {

                new ConnectTask().execute(UserData.createMessage("lp", UserData.Name, UserData.Password));
            }
        }, 6000);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
               parts = TCPClient.serverMessage.toString();
                int index = parts.indexOf(">");
                parts=parts.substring(3, index);

                if (parts.equals("/"))
                {}
                else
                {
                    parts=parts.substring(1);
                    Log.e("parts", parts);
                    if (parts.equals("bad")){}
                    else
                    {
                        String delimiter = "\\/";
                        Datafriend=parts.split(delimiter);
                        for(int i=0;i<Datafriend.length;i++)
                        {
                            Log.e("Name", Datafriend[i]);
                            UserData.ListRequests.add(new Requests(Datafriend[i]));
                        }
                    }
                }
                progressBar.setVisibility(View.INVISIBLE);
                getSupportActionBar().setDisplayHomeAsUpEnabled(true);
                getSupportActionBar().setHomeButtonEnabled(true);


            }
        }, 7500);

        mTitle = mDrawerTitle = getTitle();
        mScreenTitles = getResources().getStringArray(R.array.screen_array);
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.left_drawer);

        // Set the adapter for the list view
        mDrawerList.setAdapter(new ArrayAdapter<String>(this,
                R.layout.drawer_list_item, mScreenTitles));
        // Set the list's click listener
        Log.d("Test","готов");
        mDrawerList.setOnItemClickListener(new DrawerItemClickListener());

        /*getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);*/

        mDrawerToggle = new ActionBarDrawerToggle(
                this, /* host Activity */
                mDrawerLayout, /* DrawerLayout object */
                R.drawable.ic72, /* nav drawer icon to replace 'Up' caret */
                R.string.drawer_open, /* "open drawer" description */
                R.string.drawer_close /* "close drawer" description */
        ) {

            /** Called when a drawer has settled in a completely closed state. */
            public void onDrawerClosed(View view) {
                getSupportActionBar().setTitle(mTitle);
                supportInvalidateOptionsMenu(); // creates call to onPrepareOptionsMenu()
            }

            /** Called when a drawer has settled in a completely open state. */
            public void onDrawerOpened(View drawerView) {
                getSupportActionBar().setTitle(mDrawerTitle);
                supportInvalidateOptionsMenu(); // creates call to onPrepareOptionsMenu()
            }
        };
        Log.d("Test","готов");
        // Set the drawer toggle as the DrawerListener
        mDrawerLayout.setDrawerListener(mDrawerToggle);

        // Initialize the first fragment when the application first loads.
        if (savedInstanceState == null) {
            selectItem(0);
        }

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu;
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.menu_main_page, menu);
        return super.onCreateOptionsMenu(menu);
    }

    /* Called whenever we call invalidateOptionsMenu() */
    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        // If the nav drawer is open, hide action items related to the content view
        boolean drawerOpen = mDrawerLayout.isDrawerOpen(mDrawerList);
        menu.findItem(R.id.action_search).setVisible(!drawerOpen);
        menu.findItem(R.id.menu_overflow).setVisible(!drawerOpen);
        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Pass the event to ActionBarDrawerToggle, if it returns
        // true, then it has handled the app icon touch event
        if (mDrawerToggle.onOptionsItemSelected(item)) {
            return true;
        }
        // Handle action buttons
        switch(item.getItemId()) {
            case R.id.action_search:
                ad.show();
                return true;
            case R.id.action_refresh:
                Intent i = new Intent(ActionPage.this, ActionPage.class);
                startActivity(i);
                return true;
            case R.id.action_exit:
                saveText();
                android.os.Process.killProcess(android.os.Process.myPid());
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    /* The click listener for ListView in the navigation drawer */
    private class DrawerItemClickListener implements ListView.OnItemClickListener {
        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
            selectItem(position);
        }
    }

    /** Swaps fragments in the main content view */
    private void selectItem(int position) {
        // Update the main content by replacing fragments
        Fragment fragment = null;
        switch (position) {
            case 0:
                fragment = new ProfileFragment();
                break;
            case 1:
                fragment = new FriendFragment();
                break;
            case 2:
                fragment = new RequestsFragment();
                break;
            case 3:
                fragment = new PeopleFragment();
                break;
            default:
                break;
        }

        // Insert the fragment by replacing any existing fragment
        if (fragment != null) {
            FragmentManager fragmentManager = getSupportFragmentManager();
            fragmentManager.beginTransaction()
                    .replace(R.id.content_frame, fragment).commit();

            // Highlight the selected item, update the title, and close the drawer
            mDrawerList.setItemChecked(position, true);
            setTitle(mScreenTitles[position]);
            mDrawerLayout.closeDrawer(mDrawerList);
        } else {
            // Error
            Log.e(this.getClass().getName(), "Error. Fragment is not created");
        }
    }

    @Override
    public void setTitle(CharSequence title) {
        mTitle = title;
        getSupportActionBar().setTitle(mTitle);
    }

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);
        // Sync the toggle state after onRestoreInstanceState has occurred.
        mDrawerToggle.syncState();
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);

        mDrawerToggle.onConfigurationChanged(newConfig);
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
            UserData.longitude = Double.toString(location.getLongitude());
            UserData.latitude = Double.toString(location.getLatitude());
        }
        return null;
    }
    void loadText() {
        sPref = getSharedPreferences("main",MODE_PRIVATE);
        UserData.Name     = sPref.getString("Name", "");
        UserData.Password = sPref.getString("Password", "");
        UserData.longitude=sPref.getString("Longitude","");
        UserData.latitude=sPref.getString("Latitude","");
    }

    void saveText() {

        sPref = getSharedPreferences("main",MODE_PRIVATE);
        SharedPreferences.Editor ed = sPref.edit();
        ed.putString("Name", "");
        ed.putString("Password","");
        ed.putString("Longitude","");
        ed.putString("Latitude","");
        ed.commit();

    }


}