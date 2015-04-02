package com.hot_ice.hot_ice;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;

public class ProfileFragment extends Fragment {
    public ImageLoader imageLoader;
    EditText editText;
    public ProfileFragment() {

    }



    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        View rootView = inflater.inflate(R.layout.fragment_profile, container,
                false);
        //((ImageView) rootView.findViewById(R.id.icon)).setImageResource(R.drawable.no_photo);
        editText =(EditText)rootView.findViewById(R.id.editText);
        editText.setText(UserData.Status);
        ImageView image=(ImageView)rootView.findViewById(R.id.icon);
        imageLoader=new ImageLoader(getActivity().getApplicationContext());
        imageLoader.DisplayImage("http://109.120.164.212/photos/"+UserData.Name+".jpg", image);
        Button button = (Button)rootView.findViewById(R.id.button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                new ConnectTask().execute(UserData.createMessage("ss", UserData.Name, UserData.Password,editText.getText().toString()));
                new Handler().postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        startActivity(new Intent(getActivity(), ActionPage.class));
                    }
                }, 1000);
                    }
                });
        return rootView;
    }
}