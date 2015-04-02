package com.hot_ice.hot_ice;

import android.content.Context;
import android.graphics.Bitmap;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

public class FriendsAdapter extends BaseAdapter {
    Context ctx;
    LayoutInflater lInflater;
    ArrayList<Friend> objects;
    UserData User;
    Bitmap bitmap;
    ImageView ivImage;
    View view;
    public ImageLoader imageLoader;
    final int MENU_COLOR_RED = 1;
    final int MENU_COLOR_GREEN = 2;
    final int MENU_COLOR_BLUE = 3;

    FriendsAdapter(Context context) {
        ctx = context;
        objects = UserData.ListFriends;
        lInflater = (LayoutInflater) ctx
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        imageLoader=new ImageLoader(context.getApplicationContext());
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
        view = convertView;
        if (view == null) {
            view = lInflater.inflate(R.layout.item, parent, false);
        }

        Friend p = getProduct(position);

        // заполняем View в пункте списка данными из товаров: наименование, цена
        // и картинка
        ((TextView) view.findViewById(R.id.tvName)).setText(p.name);
        ((TextView) view.findViewById(R.id.tvTime)).setText(p.time);
        ((TextView) view.findViewById(R.id.tvDistance)).setText(p.distance);
        ((TextView) view.findViewById(R.id.tvStatus)).setText(p.status);
        ImageView image=(ImageView)view.findViewById(R.id.ivImage);
        Log.e("imagePath",p.imagePath);
        imageLoader.DisplayImage(p.imagePath, image);

        return view;
    }

    Friend getProduct(int position) {
        return ((Friend) getItem(position));
    }

}
