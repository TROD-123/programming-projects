package com.thirdarm.testretrofit;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.TextView;

import com.squareup.picasso.Picasso;
import com.thirdarm.testretrofit.API.gitapi;
import com.thirdarm.testretrofit.model.gitmodel;

import java.io.IOException;
import java.util.List;

import retrofit.Call;
import retrofit.Callback;
import retrofit.Response;
import retrofit.Retrofit;
import retrofit.GsonConverterFactory;

public class MainActivity extends AppCompatActivity {

    public final String LOG_TAG = "Retrofit Test App";

    // BASE URL for accessing JSON data
    public final String API = "https://api.github.com/";
    public gitapi git;
    public Call<gitmodel> response;
    public TextView mTextView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mTextView = (TextView) findViewById(R.id.textView);

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(API)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        // Create instance of github API interface
        git = retrofit.create(gitapi.class);

        // Create call instance for looking up trod-123 in github
        response = git.getFeedSync("trod-123");
        response.enqueue(new Callback<gitmodel>() {
            @Override
            public void onResponse(Response<gitmodel> response) {
                // Get result from response.body()
                Log.v(LOG_TAG, "In onResponse");
                String text = response.body().getName();
                Log.v(LOG_TAG, "The string is: " + text);
                mTextView.setText(text);
            }

            @Override
            public void onFailure(Throwable t) {
                Log.d(LOG_TAG, "In onFailure" + t);
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
