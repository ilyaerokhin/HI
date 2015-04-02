package com.hot_ice.hot_ice;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.text.InputFilter;
import android.text.Spanned;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class RegistrationPage extends ActionBarActivity {
    EditText nameEdit, passwordEdit,emailEdit;
    UserData User;
    Pattern pattern;
    Matcher matcher;
    SharedPreferences sPref;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_registration_page);


        nameEdit =(EditText)findViewById(R.id.NameEdit);
        passwordEdit =(EditText)findViewById(R.id.PasswordEdit);
        emailEdit =(EditText)findViewById(R.id.EmailEdit);

        nameEdit.setFilters(new InputFilter[] {
                new InputFilter() {
                    @Override
                    public CharSequence filter(CharSequence source, int start,
                                               int end, Spanned dest, int dstart, int dend) {
                        if(source.equals("")){ // for backspace
                            return source;
                        }
                        if(source.toString().matches("[a-zA-Zа-яА-Я0-9]+")){
                            return source;
                        }
                        return "";
                    }
                }
        });

        passwordEdit.setFilters(new InputFilter[] {
                new InputFilter() {
                    @Override
                    public CharSequence filter(CharSequence source, int start,
                                               int end, Spanned dest, int dstart, int dend) {
                        if(source.equals("")){ // for backspace
                            return source;
                        }
                        if(source.toString().matches("[a-zA-Zа-яА-Я0-9]+")){
                            return source;
                        }
                        return "";
                    }
                }
        });



        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayOptions(ActionBar.DISPLAY_SHOW_HOME | ActionBar.DISPLAY_SHOW_TITLE);
        actionBar.setIcon(R.drawable.ic72);


    }

    @Override
    protected void onResume() {
        super.onResume();

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
    public void onBackPressed() {

        android.os.Process.killProcess(android.os.Process.myPid());
        // This above line close correctly
    }

    public void onSingInButton(View view)
    {
        if (nameEdit.getText().toString().equals("") && passwordEdit.getText().toString().equals("") &&
                emailEdit.getText().toString().equals(""))
        {
            Toast.makeText(this, "Invalid Input - Empty Fields", Toast.LENGTH_SHORT).show();
        }
        else
        {
            UserData.Name     = nameEdit.getText().toString();    UserData.Name     = UserData.Name.toLowerCase();
            UserData.Password = passwordEdit.getText().toString();UserData.Password = UserData.Password.toLowerCase();
            final String Email      = emailEdit.getText().toString();

            if(isEmailValid(Email))
            {
                saveText();
                new Handler().postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        new ConnectTask().execute(UserData.createMessage("ad",UserData.Name,UserData.Password,Email));
                    }
                }, 0);
                new Handler().postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        startActivity(new Intent(getApplicationContext(), ActionPage.class));
                    }
                }, 1000);
            }
            else {
                Toast.makeText(this, "Invalid Email", Toast.LENGTH_SHORT).show();
                nameEdit.setText("");
                passwordEdit.setText("");
                emailEdit.setText("");
            }
        }
    }

    public boolean isEmailValid(String email)
    {
        String regExpn =
                "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@"
                        +"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?"
                        +"[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\."
                        +"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?"
                        +"[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                        +"([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$";

        CharSequence inputStr = email;

        pattern = Pattern.compile(regExpn, Pattern.CASE_INSENSITIVE);
        matcher = pattern.matcher(inputStr);

        if(matcher.matches())
            return true;
        else
            return false;
    }
    void saveText() {

        sPref = getSharedPreferences("main",MODE_PRIVATE);
        SharedPreferences.Editor ed = sPref.edit();
        ed.putString("Name", UserData.Name);
        ed.putString("Password",UserData.Password);
        ed.commit();
        Log.e("Сохранено", UserData.Name);
        Log.e("Сохранено", UserData.Password);
    }

}
