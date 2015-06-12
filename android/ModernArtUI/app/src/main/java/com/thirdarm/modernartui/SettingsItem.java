package com.thirdarm.modernartui;

import android.widget.SeekBar;

/**
 * Created by trod-123 on 6/9/15.
 *
 * A class for Settings objects
 */
public class SettingsItem {

    // Fields
    private String mTitle, mMessage;
    private int mValue;
    private SeekBar mSeekBar;

    // Constructor
    SettingsItem(String title, String message, int value) {
        this.mTitle = title;
        this.mMessage = message;
        this.mValue = value;
    }

    // Getter and setter methods
    public String getTitle() {
        return mTitle;
    }

    public String getMessage() {
        return mMessage;
    }

    public int getValue() {
        return mValue;
    }

    public void setTitle(String title) {
        this.mTitle = title;
    }

    public void setMessage(String message) {
        this.mMessage = message;
    }

    public void setValue(int value) {
        this.mValue = value;
    }
}
