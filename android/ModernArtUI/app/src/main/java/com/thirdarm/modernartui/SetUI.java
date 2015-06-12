package com.thirdarm.modernartui;

import android.app.ActionBar;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.app.ListActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.SeekBar;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by trod-123 on 6/8/15.
 */
public class SetUI extends ListActivity {

    // TODO: Implement Preferences to create a different Settings UI

    // data to be sent back to MainActivity
    private static final String NUM_LINEAR_LAYOUTS = "NUM_LINEAR_LAYOUTS";
    private static final String NUM_TEXT_VIEWS = "NUM_TEXT_VIEWS";

    private int numLinearLayouts = 0;
    private int numTextViews = 0;

    // store Settings items (not needed if an Adapter is used)
    //private final List<SettingsItem> mSettings = new ArrayList<SettingsItem>();

    // Initialize adapter
    SettingsAdapter mAdapter;

    // one DialogFragment instance for all Dialogs
    private DialogFragment mDialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Create a new SettingsAdapter
        mAdapter = new SettingsAdapter(getApplicationContext());

        // Put dividers between settings items
        getListView().setFooterDividersEnabled(true);

        // Attach adapter
        getListView().setAdapter(mAdapter);

        // Enable up button in action bar to return to MainActivity
        // But why display it if you just want Back button behavior?
//        ActionBar actionBar = getActionBar();
//        actionBar.setDisplayHomeAsUpEnabled(true);

    }

    // Populate adapter view with SettingsItems
    @Override
    protected void onResume() {
        super.onResume();
        createSettingsItems();
    }

    // Create list of settings items
    public void createSettingsItems() {
        String[] titles = {NUM_LINEAR_LAYOUTS, NUM_TEXT_VIEWS};

        // access string resources through getResources().getString(resource)
        String[] messages = {getResources().getString(R.string.columns),
                getResources().getString(R.string.rectangles)};

        // get current values of number of columns and rectangles per column from
        // MainActivity
        for (int i = 0; i < titles.length; i++) {
            mAdapter.add(new SettingsItem(titles[i], messages[i],
                    getIntent().getExtras().getInt(titles[i])));

        }
    }

    //
    // The following two methods are for programming the Action Bar menu
    //

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_settings, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        // check to see which button was clicked
        switch (id) {

            // restore removed textViews, generate new rectangles with different properties,
            // and reset SeekBar
            case R.id.save:
                List<SettingsItem> settings = mAdapter.getSettings();
                numLinearLayouts = settings.get(0).getValue();
                numTextViews = settings.get(1).getValue();
                Intent data = new Intent();
                data.putExtra(NUM_LINEAR_LAYOUTS, numLinearLayouts);
                data.putExtra(NUM_TEXT_VIEWS, numTextViews);
                setResult(RESULT_OK, data);
                finish();

            default:
                return super.onOptionsItemSelected(item);
        }
    }
}

