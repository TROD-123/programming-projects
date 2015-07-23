package com.thirdarm.dayscounter;

import android.app.Fragment;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.Calendar;

/**
 * Created by trod-123 on 7/16/15.
 */
public class DaysCounterFragment extends Fragment {


    public static final int NUM_COLUMNS = 3;
    public static int NUM_DATES;
    public static final int NUM_SECTIONS = 3;

    public static final int TITLE_SIZE = 17;
    public static final int DATE_SIZE = 16;

    public static Context mContext;
    public static View rootView;
    public static TableLayout mDateTable;

    public static ArrayList<String> dates;

    public DaysCounterFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        mContext = getActivity();

        rootView = inflater.inflate(R.layout.fragment_main, container, false);
        return rootView;
    }


    // redraw table upon resuming activity
    @Override
    public void onResume() {
        super.onResume();

        initialize();

    }

    // clear table when activity is stopped
    @Override
    public void onStop() {
        super.onStop();

        reset();
    }

    // initialize table elements
    private void initialize() {
        getData();
        generate();
    }

    // get data from preferences file
    private void getData() {

        // Returns the SharedPreferences object containing all key-value pairs associated with
        //  SettingsUIFragment, which extends the PreferenceActivityFragment
        // This is used to get the default values of settings
        SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(mContext);

        NUM_DATES = Integer.parseInt(SP.getString("num_dates", "60"));

    }

    // generate table
    private void generate() {

        mDateTable = (TableLayout) rootView.findViewById(R.id.prospectiveCounter);

        // play with dates
        Calendar c = Calendar.getInstance();
        c.add(Calendar.DAY_OF_MONTH, -1); // so that day 1 includes current date

        // generate title row
        TableRow titleRow = new TableRow(mContext);

        for (int i = 0; i < NUM_COLUMNS; i++) {
            TextView day = new TextView(mContext);
            day.setText("Count");
            day.setTextSize(TITLE_SIZE);
            day.setPadding(20, 10, 20, 10);
            titleRow.addView(day);

            TextView proDate = new TextView(mContext);
            proDate.setText("Date");
            proDate.setTextSize(TITLE_SIZE);
            proDate.setPadding(20, 10, 40, 10);
            titleRow.addView(proDate);
        }

        mDateTable.addView(titleRow);


        //
        // generate prospective date rows
        //

        // generate and store dates in a list
        dates = new ArrayList<>();
        for (int i = 0; i < NUM_DATES; i++) {
            c.add(Calendar.DAY_OF_MONTH, 1);
            dates.add((c.get(Calendar.MONTH)+1) + "/" + c.get(Calendar.DAY_OF_MONTH));
        }

        // print dates into table
        int NUM_ROWS = NUM_DATES/NUM_COLUMNS;
        int NUM_ROWS_PER_SECTION = NUM_ROWS/NUM_SECTIONS;
        int NUM_DATES_PER_SECTION = NUM_DATES/NUM_SECTIONS;

        for (int i = 0; i < NUM_SECTIONS; i++) {

            for (int j = 0; j < NUM_ROWS_PER_SECTION; j++) {

                TableRow row = new TableRow(mContext);

                for (int k = 0; k < NUM_COLUMNS; k++) {
                    TextView counter = new TextView(mContext);
                    counter.setText((i*NUM_DATES_PER_SECTION+1 + j + k*NUM_ROWS_PER_SECTION) + "");
                    counter.setTextSize(DATE_SIZE);
                    counter.setPadding(20, 5, 20, 5);
                    row.addView(counter);

                    TextView pd = new TextView(mContext);
                    pd.setText(dates.get(i*NUM_DATES_PER_SECTION + j + k*NUM_ROWS_PER_SECTION));
                    pd.setTextSize(DATE_SIZE);
                    pd.setPadding(20, 5, 40, 5);
                    row.addView(pd);
                }

                mDateTable.addView(row);

            }

            TextView blankLine = new TextView(mContext);
            blankLine.setText("");
            mDateTable.addView(blankLine);

        }
    }

    // clear tables
    private void reset() {

        if (mDateTable != null) {
            mDateTable.removeAllViews();
            dates.clear();
        }
    }
}