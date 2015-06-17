package com.thirdarm.dayscounter;

import android.app.Activity;
import android.app.Fragment;
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
            return true;
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


    public static class DaysCounterFragment extends Fragment {

        public DaysCounterFragment() {
        }

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            View rootView = inflater.inflate(R.layout.fragment_main, container, false);

            TableLayout mDateTable = (TableLayout) rootView.findViewById(R.id.prospectiveCounter);
            final int titleSize = 17;
            final int textSize = 16;

            // play with dates
            Calendar c = Calendar.getInstance();
            c.add(Calendar.DAY_OF_MONTH, -1); // so that day 1 includes current date

            // title row
            TableRow titleRow = new TableRow(getActivity());

            TextView day = new TextView(getActivity());
            day.setText("Count");
            day.setTextSize(titleSize);
            day.setPadding(20, 10, 20, 10);
            titleRow.addView(day);

            TextView proDate = new TextView(getActivity());
            proDate.setText("Date");
            proDate.setTextSize(titleSize);
            proDate.setPadding(20, 10, 40, 10);
            titleRow.addView(proDate);

            TextView day2 = new TextView(getActivity());
            day2.setText("Count");
            day2.setTextSize(titleSize);
            day2.setPadding(40, 10, 20, 10);
            titleRow.addView(day2);

            TextView proDate2 = new TextView(getActivity());
            proDate2.setText("Date");
            proDate2.setTextSize(titleSize);
            proDate2.setPadding(20, 10, 40, 10);
            titleRow.addView(proDate2);

            TextView day3 = new TextView(getActivity());
            day3.setText("Count");
            day3.setTextSize(titleSize);
            day3.setPadding(40, 10, 20, 10);
            titleRow.addView(day3);

            TextView proDate3 = new TextView(getActivity());
            proDate3.setText("Date");
            proDate3.setTextSize(titleSize);
            proDate3.setPadding(20, 10, 40, 10);
            titleRow.addView(proDate3);

            mDateTable.addView(titleRow);

            // make rows
            ArrayList<String> dates = new ArrayList<>();
            for (int i = 0; i < 60; i++) {
                c.add(Calendar.DAY_OF_MONTH, 1);
                dates.add((c.get(Calendar.MONTH)+1) + "/" + c.get(Calendar.DAY_OF_MONTH));
            }

            for (int i = 0; i < 20; i++) {

                TableRow row = new TableRow(getActivity());
                //row.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT));

                TextView counter = new TextView(getActivity());
                counter.setText((i+1) + "");
                counter.setTextSize(textSize);
                counter.setPadding(20, 5, 20, 5);
                row.addView(counter);

                TextView pd = new TextView(getActivity());
                pd.setText(dates.get(i));
                pd.setTextSize(textSize);
                pd.setPadding(20, 5, 40, 5);
                row.addView(pd);

                TextView counter2 = new TextView(getActivity());
                counter2.setText((i+21) + "");
                counter2.setTextSize(textSize);
                counter2.setPadding(40, 5, 20, 5);
                row.addView(counter2);

                TextView pd2 = new TextView(getActivity());
                pd2.setText(dates.get(i+20));
                pd2.setTextSize(textSize);
                pd2.setPadding(20, 5, 40, 5);
                row.addView(pd2);

                TextView counter3 = new TextView(getActivity());
                counter3.setText((i+41) + "");
                counter3.setTextSize(textSize);
                counter3.setPadding(40, 5, 20, 5);
                row.addView(counter3);

                TextView pd3 = new TextView(getActivity());
                pd3.setText(dates.get(i+40));
                pd3.setTextSize(textSize);
                pd3.setPadding(20, 5, 40, 5);
                row.addView(pd3);

                mDateTable.addView(row);
            }

            return rootView;
        }
    }

}
