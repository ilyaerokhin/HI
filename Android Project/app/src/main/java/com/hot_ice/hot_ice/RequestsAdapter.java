package com.hot_ice.hot_ice;

import android.content.Context;
import android.graphics.Bitmap;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

public class RequestsAdapter extends BaseAdapter {
    Context ctx;
    LayoutInflater lInflater;
    ArrayList<Requests> objects;
    UserData User;
    Bitmap bitmap;
    ImageView ivImage;
    View view;
    public ImageLoader imageLoader;

    RequestsAdapter(Context context) {
        ctx = context;
        objects = UserData.ListRequests;
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

        Requests p = getProduct(position);

        // заполняем View в пункте списка данными из товаров: наименование, цена
        // и картинка
        ((TextView) view.findViewById(R.id.tvName)).setText(p.name);
        ImageView image=(ImageView)view.findViewById(R.id.ivImage);
        image.setImageResource(R.drawable.no_photo);
        /*Log.e("imagePath",p.imagePath);
        imageLoader.DisplayImage(p.imagePath, image);*/

        // new LoadImage().execute(p.imagePath);

        return view;
    }

    // товар по позиции
    Requests getProduct(int position) {
        return ((Requests) getItem(position));
    }

}
