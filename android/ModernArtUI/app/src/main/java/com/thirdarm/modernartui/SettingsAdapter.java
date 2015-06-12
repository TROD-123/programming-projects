package com.thirdarm.modernartui;

import android.content.Context;
import android.content.res.Resources;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.SeekBar;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by trod-123 on 6/9/15.
 *
 * An adapter that holds data for settings.
 * BaseAdapter used because it is commonly used in ListViews
 */
public class SettingsAdapter extends BaseAdapter {

    // Titles of each Setting component
    private static final String NUM_LINEAR_LAYOUTS = "NUM_LINEAR_LAYOUTS";
    private static final String NUM_TEXT_VIEWS = "NUM_TEXT_VIEWS";

    // Adapter stores list of objects to be used in a ListView (example use)
    private final List<SettingsItem> mSettings = new ArrayList<>();

    private final Context mContext;

    // constructor takes in context for easy reference
    public SettingsAdapter(Context context) {
        this.mContext = context;
    }

    // Add a SettingsItem to the adapter and notify observers data set has changed
    public void add(SettingsItem item) {
        mSettings.add(item);
        notifyDataSetChanged();
    }

    public List<SettingsItem> getSettings() {
        return mSettings;
    }

    // Changes in settings value stored in Adapter for saved reference and notify
    // observers that data set has changed
    // TODO: this method, when used by the hosting activity, seems to crash whenever
    // it is used...
    public void changeValue(int pos, int value) {
        mSettings.get(pos).setValue(value);
        notifyDataSetChanged();
    }


    // Default and necessary overridden BaseAdapter methods
    @Override
    public int getCount() {
        return mSettings.size();
    }

    @Override
    public Object getItem(int pos) {
        return mSettings.get(pos);
    }

    @Override
    public long getItemId(int pos) {
        return pos;
    }


    // ViewHolder is a class that contains objects for each setting used in the UI.
    // Objects are referred to as they are named
    private static class ViewHolder {
        TextView messageView;
        TextView valueView;
        SeekBar seekBar;
    }

    // Create a View for the SettingsItem at specified location
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        // Create new ViewHolder
        final ViewHolder holder = new ViewHolder();

        // Get current SettingsItem (getView will explore each of the elements in
        // the Settings list
        final SettingsItem settingsItem = mSettings.get(position);

        // Get value of current SettingsItem
        final int value = settingsItem.getValue();

        // Create a LinearLayout first
        LinearLayout settingsLayout;

        // Check if convertView is not null, and if not, make a new one
        // If null, inflate view
        if (convertView != null) {
            settingsLayout = (LinearLayout) convertView;
        } else {
            LayoutInflater inflater = (LayoutInflater) mContext
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            View view = inflater.inflate(R.layout.settings_item, null);
            settingsLayout = (LinearLayout) view.findViewById(R.id.customizeLayout);
        }


        // Fill in specific SettingsItem data
        holder.messageView = (TextView) settingsLayout.findViewById(R.id.message);
        holder.messageView.setText(settingsItem.getMessage());

        holder.valueView = (TextView) settingsLayout.findViewById(R.id.value);
        if (value == 0) {
            holder.valueView.setText("Random");
        } else {
            holder.valueView.setText("" + value);
        }

        holder.seekBar = (SeekBar) settingsLayout.findViewById(R.id.seekBar2);
        holder.seekBar.setMax(10);
        holder.seekBar.setProgress(value);

        // Set up SetOnSeekBarChangeListener for when user modifies SeekBar, the changed
        // value is reflected on the Adapter
        holder.seekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {

            // This method is called each time the SeekBar is changed (meaning, it
            //  is called for every increment that the SeekBar moves)
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                settingsItem.setValue(progress);
                notifyDataSetChanged();
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

//                //For testing SeekBar
//                Toast.makeText(getApplicationContext(), "Started tracking seekbar",
//                        Toast.LENGTH_SHORT).show();

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {


//                //For testing SeekBar
//                Toast.makeText(getApplicationContext(), "Stopped tracking seekbar",
//                        Toast.LENGTH_SHORT).show();

            }
        });

        settingsLayout.setTag(holder);
        return settingsLayout;
    }
}
