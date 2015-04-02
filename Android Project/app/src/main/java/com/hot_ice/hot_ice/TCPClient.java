package com.hot_ice.hot_ice;

import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class TCPClient implements Runnable {
    private SimpleSemaphore semaphore = null;
    public static String serverMessage;
    Socket mSocket;
    public static final String SERVERIP = "109.120.164.212"; //your computer IP address
    public static final int SERVERPORT = 32000;
    boolean Run = true;
    public static UserData User;

    private InputStream in = null;
    private OutputStream out = null;

    public TCPClient(Socket socket, SimpleSemaphore semaphore) {

        this.semaphore = semaphore;

        mSocket = socket;
        InputStream tmpIn = null;
        OutputStream tmpOut = null;


        try {
            tmpIn = socket.getInputStream();
            tmpOut = socket.getOutputStream();
        } catch (IOException e) {
        }

        in = tmpIn;
        out = tmpOut;
    }

    public void sendMessage(String message) throws IOException {
        if (out != null) {
            out.write(message.getBytes());
            out.flush();
            Log.e("Сообщение отправлено...", "УРАААА");
            Log.e("SEND TO SERVER", "S: Send Message: '" + message + "'");

        }
    }

    @Override
    public void run() {
        byte[] buffer = new byte[256]; // buffer store for the stream
        int bytes; // bytes returned from read()
        Log.e("test1", "УРАААА");
        // Keep listening to the InputStream until an exception occurs
        while (Run) {
            try {
                // Read from the InputStream
                bytes = in.read(buffer); // Получаем кол-во байт и само собщение в байтовый массив "buffer"

                serverMessage = new String(buffer, 0, bytes);
                semaphore.take();
                Log.e("Сообщение пришло...", "УРАААА");
                Log.e("RESPONSE FROM SERVER", "S: Received Message: '" + serverMessage + "'");
                semaphore.release();
                if (serverMessage != null)
                    Run=false;

                in.close();
                out.close();

            } catch (IOException e) {
                continue;
            } catch (Exception e) {
                System.exit(-1);
            }
        }
    }



}