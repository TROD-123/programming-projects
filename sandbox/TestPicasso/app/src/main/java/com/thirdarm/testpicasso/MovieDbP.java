package com.thirdarm.testpicasso;

import android.os.Parcel;
import android.os.Parcelable;

import info.movito.themoviedbapi.model.MovieDb;

/**
 * Created by TROD on 20150908.
 */
public class MovieDbP extends MovieDb implements Parcelable {

    private MovieDbP(Parcel in) {
        return;
    }
    @Override
    public int describeContents() { return 0; }

    @Override
    public void writeToParcel(Parcel out, int flags) {

    }
}
