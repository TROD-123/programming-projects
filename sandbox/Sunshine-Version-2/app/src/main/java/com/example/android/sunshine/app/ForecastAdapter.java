package com.example.android.sunshine.app;

import android.content.Context;
import android.database.Cursor;
import android.support.v4.widget.CursorAdapter;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.example.android.sunshine.app.data.WeatherContract;

/**
 * {@link ForecastAdapter} exposes a list of weather forecasts
 * from a {@link android.database.Cursor} to a {@link android.widget.ListView}.
 */
public class ForecastAdapter extends CursorAdapter {

    public ForecastAdapter(Context context, Cursor c, int flags) {
        super(context, c, flags);
    }

    /**
     * Prepare the weather high/lows for presentation.
     */
    private String formatHighLows(double high, double low) {
        boolean isMetric = Utility.isMetric(mContext);
        String highLowStr = Utility.formatTemperature(high, isMetric) + "/" + Utility.formatTemperature(low, isMetric);
        return highLowStr;
    }

    /*
        This is ported from FetchWeatherTask --- but now we go straight from the cursor to the
        string.

        UX = User Experience
        UXD = User Experience Design

        This is for formatting the weather strings into:
            Date - Weather -- High/Low

        Pretty simple method that's just for formatting output strings.
     */
    private String convertCursorRowToUXFormat(Cursor cursor) {
        // Using the constants defined in the ForecastFragment, get the values from the cursor.
        //  cursor.getX(int columnIndex), where columnIndex is the column of data you want.
        //  returns the value that is in that column. you just need to resolve the type
        // This is part of leveraging projections. It is more efficient to do this than to
        //  use cursor.getColumnIndex(String columnName) and then to extract the value from that
        //  column. This would be a 2 step process, but through leveraging projections, it
        //  gets compressed into a single step.C);

//        // OLD CODE: get row indices for our cursor
//        int idx_max_temp = cursor.getColumnIndex(WeatherContract.WeatherEntry.COLUMN_MAX_TEMP);
//        int idx_min_temp = cursor.getColumnIndex(WeatherContract.WeatherEntry.COLUMN_MIN_TEMP);
//        int idx_date = cursor.getColumnIndex(WeatherContract.WeatherEntry.COLUMN_DATE);
//        int idx_short_desc = cursor.getColumnIndex(WeatherContract.WeatherEntry.COLUMN_SHORT_DESC);

        String highAndLow = formatHighLows(
                cursor.getDouble(ForecastFragment.COL_WEATHER_MAX_TEMP),
                cursor.getDouble(ForecastFragment.COL_WEATHER_MIN_TEMP));

        return Utility.formatDate(cursor.getLong(ForecastFragment.COL_WEATHER_DATE)) +
                " - " + cursor.getString(ForecastFragment.COL_WEATHER_DESC) +
                " - " + highAndLow;
    }


    // The following 2 methods are required to be overridden when extending CursorAdapter.
    /*
        Remember that these views are reused as needed. This is where you return what layout is
          going to be duplicated. Each layout is going to be one unit in a ListView, so these
          layouts are going to be reused as needed.

        The layout that is inflated is the list item layout, the layout for each list item, which
          in this case is just a layout containing a single TextView

        That's what this method does. All it does is gets the view on which the items will be placed.
          The view gets populated with the items in the bindView() method below.
     */
    @Override
    public View newView(Context context, Cursor cursor, ViewGroup parent) {
        View view = LayoutInflater.from(context).inflate(R.layout.list_item_forecast, parent, false);

        return view;
    }

    /*
        This is where we fill-in the views with the contents of the cursor. This is where we
          BIND the values in the cursor to the view. This is also where the UX converter is called.
     */
    @Override
    public void bindView(View view, Context context, Cursor cursor) {
        // our view is pretty simple here --- just a text view
        // we'll keep the UI functional with a simple (and slow!) binding.

        TextView tv = (TextView)view;
        tv.setText(convertCursorRowToUXFormat(cursor));
    }
}