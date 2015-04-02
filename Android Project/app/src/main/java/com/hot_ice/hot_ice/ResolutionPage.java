package com.hot_ice.hot_ice;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;


public class ResolutionPage extends ActionBarActivity implements View.OnClickListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_resolution_page);

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setIcon(R.drawable.ic72);
        actionBar.setTitle("User Agreement");
        TextView textView = (TextView)findViewById(R.id.Resolution);
        textView.setText("To run this application you require an Internet connection and send information about your location. At the same time, some features may not always be available or not give accurate data.\n" +
                "\n" +
                "We don't request personal information from users, as well as, the track of the way in which the user is moving. The exact location of the user is not given to third parties. During operation, the application uses the approximate values ​​of the coordinates in order to ensure the confidentiality of users. We advise you not to provide personal information when registering that will accurately identify your identity. Add to friends only close friends and acquaintances.\n" +
                "\n" +
                "Enjoy using!\n" );
        final Button ResolutionButton     = (Button)findViewById(R.id.resolutionButton);
        ResolutionButton.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.resolutionButton:
                startActivity(new Intent(getApplicationContext(), MainPage.class));
                break;
        }
    }
}
