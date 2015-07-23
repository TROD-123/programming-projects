package com.thirdarm.dayscounter;

import android.app.Activity;
import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;


public class MainActivity extends Activity {

    // global content
    public static String dateString;
    public static TextView dateView;

    private Date mDate;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        // basic initialization
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // get and set date
        dateView = (TextView) findViewById(R.id.todaysDate);
        setDate();

        // add fragment
        if (savedInstanceState == null) {
            getFragmentManager().beginTransaction()
                    .add(R.id.container, new DaysCounterFragment())
                    .commit();
        }
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
            Intent intent = new Intent(this, Settings.class);
            startActivity(intent);
        }

        return super.onOptionsItemSelected(item);
    }

    private void setDate() {

        // create Date object
        mDate = new Date();

        // utilize Calendar to get today's date and create a string for TextView
        Calendar c = Calendar.getInstance();
        dateString = "Today: " + (c.get(Calendar.MONTH)+1) + "/" + c.get(Calendar.DAY_OF_MONTH);
        dateView.setText(dateString);

    }
}
