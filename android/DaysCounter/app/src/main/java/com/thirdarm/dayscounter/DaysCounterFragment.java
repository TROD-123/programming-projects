package com.thirdarm.dayscounter;

import android.app.Fragment;
import android.os.Bundle;
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
    public static final int NUM_DATES = 180;
    public static final int NUM_SECTIONS = 3;

    public static final int TITLE_SIZE = 17;
    public static final int DATE_SIZE = 16;

    public DaysCounterFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_main, container, false);

        TableLayout mDateTable = (TableLayout) rootView.findViewById(R.id.prospectiveCounter);

        // play with dates
        Calendar c = Calendar.getInstance();
        c.add(Calendar.DAY_OF_MONTH, -1); // so that day 1 includes current date

        // generate title row
        TableRow titleRow = new TableRow(getActivity());

        for (int i = 0; i < NUM_COLUMNS; i++) {
            TextView day = new TextView(getActivity());
            day.setText("Count");
            day.setTextSize(TITLE_SIZE);
            day.setPadding(20, 10, 20, 10);
            titleRow.addView(day);

            TextView proDate = new TextView(getActivity());
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
        ArrayList<String> dates = new ArrayList<>();
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

                TableRow row = new TableRow(getActivity());

                for (int k = 0; k < NUM_COLUMNS; k++) {
                    TextView counter = new TextView(getActivity());
                    counter.setText((i*NUM_DATES_PER_SECTION+1 + j + k*NUM_ROWS_PER_SECTION) + "");
                    counter.setTextSize(DATE_SIZE);
                    counter.setPadding(20, 5, 20, 5);
                    row.addView(counter);

                    TextView pd = new TextView(getActivity());
                    pd.setText(dates.get(i*NUM_DATES_PER_SECTION + j + k*NUM_ROWS_PER_SECTION));
                    pd.setTextSize(DATE_SIZE);
                    pd.setPadding(20, 5, 40, 5);
                    row.addView(pd);
                }

                mDateTable.addView(row);

            }

            TextView blankLine = new TextView(getActivity());
            blankLine.setText("");
            mDateTable.addView(blankLine);

        }

        return rootView;
    }
}