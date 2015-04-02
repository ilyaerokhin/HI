package com.hot_ice.hot_ice;


import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.ContextMenu;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;

public class FriendFragment extends Fragment {
    SharedPreferences sPref;
    public FriendFragment() {
    }
        FriendsAdapter friendsAdapter;
    ListView lvMain;
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_friend, container,
                false);
        lvMain = (ListView)rootView.findViewById(R.id.lvMain);

        friendsAdapter = new FriendsAdapter(getActivity());

        lvMain.setAdapter(friendsAdapter);

        registerForContextMenu(lvMain);


        return rootView;
    }
    @Override
    public void onCreateContextMenu(ContextMenu menu, View v,
                                    ContextMenu.ContextMenuInfo menuInfo) {
        if (v.getId()==R.id.lvMain) {
            String[] menuItems = getResources().getStringArray(R.array.menu_friend);

            for (int i = 0; i<menuItems.length; i++) {
                menu.add(Menu.NONE, i, i, menuItems[i]);
            }
        }
    }

    @Override
    public boolean onContextItemSelected(MenuItem item) {
        // TODO Auto-generated method stub
        AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo) item.getMenuInfo();
        switch (item.getItemId()) {
            // пункты меню для tvColor
            case 0 :
                UserData.followFriend=UserData.ListFriends.get(info.position);
                Log.e("Friend 1", UserData.followFriend.name.toString());
                UserData.index=info.position;
                saveText1();
                Intent intent = new Intent(getActivity(), FollowPage.class);
                intent.putExtra("index", info.position+"");
                intent.putExtra("fname", UserData.followFriend.name.toString());
                startActivity(intent);
                break;
            case 1:
                UserData.deleteFriend=UserData.ListFriends.get(info.position);
                new ConnectTask().execute(UserData.createMessage("df", UserData.Name, UserData.Password,UserData.deleteFriend.name));
                startActivity(new Intent(getActivity(), ActionPage.class));
                break;
        }
        return super.onContextItemSelected(item);
    }
    void saveText1() {

        sPref = this.getActivity().getSharedPreferences("main", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed = sPref.edit();
        ed.putString("FriendName",UserData.followFriend.name );
        ed.putString("FriendStatus",UserData.followFriend.status);
        ed.putString("FriendDistance",UserData.followFriend.distance );
        ed.putString("FriendImagePath",UserData.followFriend.imagePath);
        ed.putString("FriendTime",UserData.followFriend.time );

        ed.commit();

    }

}
