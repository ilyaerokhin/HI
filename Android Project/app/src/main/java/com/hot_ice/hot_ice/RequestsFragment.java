package com.hot_ice.hot_ice;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.ContextMenu;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;

public class RequestsFragment extends Fragment {

    public RequestsFragment() {

    }

    RequestsAdapter requestsAdapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_friend, container,
                false);
        ListView lvMain = (ListView) rootView.findViewById(R.id.lvMain);

        requestsAdapter = new RequestsAdapter(getActivity());

        lvMain.setAdapter(requestsAdapter);
        registerForContextMenu(lvMain);

        return rootView;
    }

    @Override
    public void onCreateContextMenu(ContextMenu menu, View v,
                                    ContextMenu.ContextMenuInfo menuInfo) {
        if (v.getId()==R.id.lvMain) {
            String[] menuItems = getResources().getStringArray(R.array.menu_request);

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
                UserData.request=UserData.ListRequests.get(info.position);
                new ConnectTask().execute(UserData.createMessage("af", UserData.Name, UserData.Password,UserData.request.name));
                break;
            case 1 :
                UserData.request=UserData.ListRequests.get(info.position);
                new ConnectTask().execute(UserData.createMessage("df", UserData.Name, UserData.Password,UserData.request.name));
                break;
        }
        return super.onContextItemSelected(item);
    }
}