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

public class PeopleAdapter extends BaseAdapter {
    Context ctx;
    LayoutInflater lInflater;
    ArrayList<People> objects;
    UserData User;
    Bitmap bitmap;
    ImageView ivImage;
    View view;
    public ImageLoader imageLoader;

    PeopleAdapter(Context context) {
        ctx = context;
        objects = UserData.ListPeople;
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

        People p = getProduct(position);

        // заполняем View в пункте списка данными из товаров: наименование, цена
        // и картинка
        ((TextView) view.findViewById(R.id.tvName)).setText(p.name);
        ((TextView) view.findViewById(R.id.tvTime)).setText(p.time);
        ((TextView) view.findViewById(R.id.tvDistance)).setText(p.distance);
        ((TextView) view.findViewById(R.id.tvStatus)).setText(p.status);
        ImageView image=(ImageView)view.findViewById(R.id.ivImage);
        Log.e("imagePath",p.imagePath);
        imageLoader.DisplayImage(p.imagePath, image);

        // new LoadImage().execute(p.imagePath);

        return view;
    }

    // товар по позиции
    People getProduct(int position) {
        return ((People) getItem(position));
    }

}