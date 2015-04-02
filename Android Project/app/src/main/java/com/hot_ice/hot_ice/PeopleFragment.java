package com.hot_ice.hot_ice;

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

public class PeopleFragment extends Fragment {

    public PeopleFragment() {
    }

    PeopleAdapter peopleAdapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_people, container,
                false);
        ListView lvMain = (ListView) rootView.findViewById(R.id.lvMain);

        peopleAdapter = new PeopleAdapter(getActivity());

        lvMain.setAdapter(peopleAdapter);

        registerForContextMenu(lvMain);

        return rootView;
    }

    @Override
    public void onCreateContextMenu(ContextMenu menu, View v,
                                    ContextMenu.ContextMenuInfo menuInfo) {
        if (v.getId()==R.id.lvMain) {
            String[] menuItems = getResources().getStringArray(R.array.menu_people);

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
                UserData.addPeople=UserData.ListPeople.get(info.position);
                Log.e("friend", UserData.addPeople.name.toString());
                new ConnectTask().execute(UserData.createMessage("af", UserData.Name, UserData.Password,UserData.addPeople.name.toString()));
                break;
        }
        return super.onContextItemSelected(item);
    }

}
