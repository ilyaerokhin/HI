package com.hot_ice.hot_ice;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;


public class ForgotPasswordPage extends ActionBarActivity implements View.OnClickListener {
    EditText NameForgot;
    String parts;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_forgot_password_page);
        final Button recoverButton    = (Button)findViewById(R.id.RecoverButton);

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setIcon(R.drawable.ic72);
        NameForgot =(EditText)findViewById(R.id.NameForgotEdit);
        recoverButton.setOnClickListener(this);

    }

    @Override
    public void onClick(View v) {
        if(NameForgot.getText().toString().equals(""))
            Toast.makeText(this, "Invalid Input - Empty field", Toast.LENGTH_SHORT).show();
        else
        {

            String nameForgot=NameForgot.getText().toString();
            new ConnectTask().execute(UserData.createMessage("gp",nameForgot));

            final Boolean[] flag = {false};
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    if (TCPClient.serverMessage.toString().equals("<gp/bad>"))
                    {
                        Toast.makeText(ForgotPasswordPage.this, "Invalid Input - Invalid name", Toast.LENGTH_SHORT).show();
                    }
                    else
                    {
                        parts = TCPClient.serverMessage.toString();
                        int index = parts.indexOf(">");
                        parts=parts.substring(4,index);
                        Toast.makeText(ForgotPasswordPage.this,"We have sent your password to the address : "+ parts +" \nIf the message has not arrived , please check your spam folder", Toast.LENGTH_SHORT).show();
                        Intent intent = new Intent(getApplicationContext(), MainPage.class);
                        startActivity(intent);
                    }
                }
            }, 2000);
        }
    }
}
