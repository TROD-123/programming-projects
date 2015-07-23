package com.thirdarm.dayscounter;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.res.TypedArray;
import android.os.Bundle;
import android.preference.DialogPreference;
import android.preference.ListPreference;
import android.preference.Preference;
import android.preference.PreferenceFragment;
import android.preference.PreferenceManager;
import android.util.AttributeSet;

import java.util.prefs.Preferences;

/**
 * Created by trod-123 on 7/16/15.
 */
public class Settings extends Activity {

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings_layout);

        // add settings UI fragment
        if (savedInstanceState == null) {
            getFragmentManager().beginTransaction()
                    .add(R.id.settings_container, new SettingsFragment())
                    .commit();
        }

    }

    public static class SettingsFragment extends PreferenceFragment {

        @Override
        public void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);

            // Load preferences from XML
            addPreferencesFromResource(R.xml.preferences);
            //PreferenceManager.setDefaultValues(, R.xml.preferences, false);
        }

        // PreferenceManager.setDefaultValues(mContext, R.xml.preferences, false);

        //    public void onResume(Bundle savedInstanceState) {
        //        super.onResume();
        //        getPreferenceScreen().getSharedPreferences().registerOnSharedPreferenceChangeListener(this);
        //    }
        //
        //    public void onSharedPreferenceChanged(SharedPreferences sharedPreferences, String key) {
        //        Preference pref = findPreference(key);
        //
        //        if (pref instanceof ListPreference) {
        //            ListPreference listPref = (ListPreference) pref;
        //            pref.setSummary(listPref.getEntry());
        //        }
        //    }


        //    public class NumberPickerPreference extends DialogPreference {
        //
        //        public int mNumDates;
        //
        //        // For specifying the user interface
        //        // Constructor declares layout and specifies text for default positive and negative
        //        //  dialog buttons
        //        public NumberPickerPreference(Context context, AttributeSet attrs) {
        //            super(context, attrs);
        //
        //            setDialogLayoutResource(R.layout.numberpicker_dialog);
        //            setPositiveButtonText(android.R.string.ok);
        //            setNegativeButtonText(android.R.string.cancel);
        //
        //            setDialogIcon(null);
        //
        ////            AlertDialog.Builder builder = new AlertDialog.Builder(context) {
        ////                builder.setMessage(R.string.num_dates)
        ////
        ////            }
        //        }
        //
        //        // For saving the setting's value
        //        @Override
        //        protected void onDialogClosed(boolean positiveResult) {
        //
        //            // Save value when user clicks "OK". Calling persistInt() saves its value to
        //            //  the SharedPreferences file (automatically using the key that's specified
        //            //  in the XML file for this preference
        //            if (positiveResult) {
        //                persistInt(mNumDates);
        //            }
        //        }
        //
        //        // For initializing current value
        //        @Override
        //        protected void onSetInitialValue(boolean restorePersistedValue, Object defaultValue) {
        //
        //            // restore existing state. if true, defaultValue is always null
        //            if (restorePersistedValue) {
        //                //mNumDates = this.getPersistedInt(DEFAULT_VALUE);
        //            }
        //
        //            // set default state from the XML attribute
        //            else {
        //                mNumDates = (Integer) defaultValue;
        //                persistInt(mNumDates);
        //            }
        //        }

        // For providing a default value
        //@Override
        //protected Object onGetDefaultValue(TypedArray a, int index) {
        //return a.getInteger(index, DEFAULT_VALUE);
        //}
    }
}


