package com.hot_ice.hot_ice;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;


public class MainPage extends ActionBarActivity implements OnClickListener {
    SharedPreferences sPref;
    EditText NameEd,PasswordEd;
    TextView forgotPassword;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_page);

        final Button logInButton     = (Button)findViewById(R.id.LogInBut);
        final Button signInButton = (Button)findViewById(R.id.SignInButton);

        NameEd =(EditText)findViewById(R.id.nameEd);
        PasswordEd =(EditText)findViewById(R.id.passwordEd);
        forgotPassword = (TextView)findViewById(R.id.ForgotPasswordView);

        logInButton.setOnClickListener(this);
        signInButton.setOnClickListener(this);
        forgotPassword.setOnClickListener(this);

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setIcon(R.drawable.ic72);
    }

    @Override
    protected void onPause() {
        super.onPause();
        android.os.Process.killProcess(android.os.Process.myPid());
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        android.os.Process.killProcess(android.os.Process.myPid());
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.LogInBut:
                if (NameEd.getText().toString().equals("") || PasswordEd.getText().toString().equals("")) {
                    Toast.makeText(this, "Invalid Input - Empty Fields", Toast.LENGTH_SHORT).show();
                } else {

                    UserData.Name = NameEd.getText().toString();
                    UserData.Name = UserData.Name.toLowerCase();
                    Log.e("Name",UserData.Name);
                    UserData.Password = PasswordEd.getText().toString();
                    UserData.Password = UserData.Password.toLowerCase();
                    Log.e("Name", UserData.Password);

                    new ConnectTask().execute(UserData.createMessage("cu",UserData.Name,UserData.Password));

                    new Handler().postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            if (TCPClient.serverMessage.toString().equals("<cu/ok>"))
                            {
                                saveText();
                                startActivity(new Intent(getApplicationContext(), ActionPage.class));
                            }
                            else
                            {
                                Toast.makeText(MainPage.this, "Invalid Input - Invalid name or password", Toast.LENGTH_SHORT).show();
                                NameEd.setText("");
                                PasswordEd.setText("");
                            }
                        }
                    }, 500);
                }
                break;
            case R.id.SignInButton:
                startActivity(new Intent(getApplicationContext(), RegistrationPage.class));
                break;
            case R.id.ForgotPasswordView:
                startActivity(new Intent(getApplicationContext(), ForgotPasswordPage.class));
                break;
        }
    }
    void saveText() {

        sPref = getSharedPreferences("main",MODE_PRIVATE);
        SharedPreferences.Editor ed = sPref.edit();
        ed.putString("Name", UserData.Name);
        ed.putString("Password",UserData.Password);
        ed.putString("Longitude", UserData.longitude);
        ed.putString("Latitude", UserData.latitude);
        ed.commit();

    }
}
