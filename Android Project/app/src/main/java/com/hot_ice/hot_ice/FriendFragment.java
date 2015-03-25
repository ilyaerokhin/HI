package com.hot_ice.hot_ice;


import android.support.v4.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

public class FriendFragment extends Fragment {


    public FriendFragment() {
    }
        BoxAdapter boxAdapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_friend, container,
                false);
        ListView lvMain = (ListView)rootView.findViewById(R.id.lvMain);

        boxAdapter = new BoxAdapter(getActivity());

        lvMain.setAdapter(boxAdapter);



        return rootView;
    }

}
