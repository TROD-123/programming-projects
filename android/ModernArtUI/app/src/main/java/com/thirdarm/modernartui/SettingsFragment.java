package com.thirdarm.modernartui;

import android.os.Bundle;
import android.preference.PreferenceFragment;

/**
 * PreferenceFragment for Settings Implementation
 */
public class SettingsFragment extends PreferenceFragment {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        addPreferencesFromResource(R.xml.preferences);
    }
}
