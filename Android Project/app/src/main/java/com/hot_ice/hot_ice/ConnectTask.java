package com.hot_ice.hot_ice;

import android.os.AsyncTask;
import android.util.Log;

import java.io.IOException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;

public class ConnectTask extends AsyncTask<String, String, Integer> {
    Socket mSocket;
    private boolean isGoodConnection = false;
    public TCPClient mTcpClient;


    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        Log.e("Идёт попытка подключения...", "Подключение");
    }

    public void getConnectionSocket() {
        Log.e("...getConnectionSocket - попытка соединения...", "Подключение");
        InetAddress addr = null;
        try{
            Log.e("... Попытка создать сокет"+ mTcpClient.SERVERIP,"Подключение");
            mSocket = new Socket();
            mSocket.setSoTimeout(10*1000);

            Log.e("...Соединяемся...","Подключение");
            InetAddress serverAddr = InetAddress.getByName( mTcpClient.SERVERIP);
            mSocket.connect(new InetSocketAddress(serverAddr, 32000), 10*1000);

            if(!mSocket.isConnected())
                return;
            Log.e("...Соединение установлено и готово к передачи данных...","Подключение");
            isGoodConnection = true;
            return;
        } catch(IOException e) {
            try {
                mSocket.close();
                return;
            } catch(IOException e2) {
                Log.e("Fatal Error In getConnectionSocket() and failed to close socket." ,"Подключение" );
                return;
            }
        }
    }


    @Override
    protected Integer doInBackground(String... params) {
        getConnectionSocket();
        if(isGoodConnection) {
            SimpleSemaphore semaphore = new SimpleSemaphore();
            Log.e("Подключение успешно!", "Подключение");
            mTcpClient = new TCPClient(mSocket,semaphore);
            try {

                mTcpClient.sendMessage(params[0]);

            } catch (IOException e) {
                e.printStackTrace();
            }
            mTcpClient.run();

        } else {
            Log.e("Неудалось установить подключение...", "Подключение");
        }
        return 1;
    }

    @Override
    protected void onPostExecute(Integer result) {
        super.onPostExecute(result);
    }

}