package com.hot_ice.hot_ice;

import android.content.Context;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

public class BoxAdapter extends BaseAdapter {
    Context ctx;
    LayoutInflater lInflater;
    ArrayList<Friend> objects;
    UserData User;

    BoxAdapter(Context context) {
        ctx = context;
        objects = User.ListFriends;
        lInflater = (LayoutInflater) ctx
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    // кол-во элементов
    @Override
    public int getCount() {
        return objects.size();
    }

    // элемент по позиции
    @Override
    public Object getItem(int position) {
        return objects.get(position);
    }

    // id по позиции
    @Override
    public long getItemId(int position) {
        return position;
    }

    // пункт списка
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // используем созданные, но не используемые view
        View view = convertView;
        if (view == null) {
            view = lInflater.inflate(R.layout.item, parent, false);
        }

        Friend p = getProduct(position);

        // заполняем View в пункте списка данными из товаров: наименование, цена
        // и картинка
        ((TextView) view.findViewById(R.id.tvName)).setText(p.name);
        ((TextView) view.findViewById(R.id.tvDistance)).setText("20 метров" + "");
        ((TextView) view.findViewById(R.id.tvTime)).setText("10 минут назад" + "");
        ((TextView) view.findViewById(R.id.tvStatus)).setText(p.status + "");
        ((ImageView) view.findViewById(R.id.ivImage)).setImageURI(Uri.parse(p.imagePath));
        ((ImageView) view.findViewById(R.id.ivImage)).setImageResource(R.drawable.polly);


        return view;
    }

    // товар по позиции
    Friend getProduct(int position) {
        return ((Friend) getItem(position));
    }

}
