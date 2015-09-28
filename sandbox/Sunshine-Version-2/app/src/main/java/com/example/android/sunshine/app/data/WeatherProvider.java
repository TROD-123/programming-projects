/*
 * Copyright (C) 2014 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package com.example.android.sunshine.app.data;

import android.annotation.TargetApi;
import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteQueryBuilder;
import android.net.Uri;

public class WeatherProvider extends ContentProvider {

    // The URI Matcher used by this content provider.
    private static final UriMatcher sUriMatcher = buildUriMatcher();
    private WeatherDbHelper mOpenHelper;

    static final int WEATHER = 100;
    static final int WEATHER_WITH_LOCATION = 101;
    static final int WEATHER_WITH_LOCATION_AND_DATE = 102;
    static final int LOCATION = 300;

    // this will contain the conjoined location and weather tables from which we will grab
    //  our data and present it in the UI
    private static final SQLiteQueryBuilder sWeatherByLocationSettingQueryBuilder;

    static{
        sWeatherByLocationSettingQueryBuilder = new SQLiteQueryBuilder();

        //This is an inner join which looks like
        //weather INNER JOIN location ON weather.location_id = location._id
        sWeatherByLocationSettingQueryBuilder.setTables(
                WeatherContract.WeatherEntry.TABLE_NAME + " INNER JOIN " +
                        WeatherContract.LocationEntry.TABLE_NAME +
                        " ON " + WeatherContract.WeatherEntry.TABLE_NAME +
                        "." + WeatherContract.WeatherEntry.COLUMN_LOC_KEY +
                        " = " + WeatherContract.LocationEntry.TABLE_NAME +
                        "." + WeatherContract.LocationEntry._ID);
    }

    // This is for defining the selection. Uses question mark replacement syntax. Selection
    //  parameters (selectionArgs) will replace the ?'s. All of this is part of SQLite SELECT
    //  structure, translated from query() arguments

    //location.location_setting = ?
    private static final String sLocationSettingSelection =
            WeatherContract.LocationEntry.TABLE_NAME+
                    "." + WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING + " = ? ";

    //location.location_setting = ? AND date >= ?
    private static final String sLocationSettingWithStartDateSelection =
            WeatherContract.LocationEntry.TABLE_NAME+
                    "." + WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING + " = ? AND " +
                    WeatherContract.WeatherEntry.COLUMN_DATE + " >= ? ";

    //location.location_setting = ? AND date = ?
    private static final String sLocationSettingAndDaySelection =
            WeatherContract.LocationEntry.TABLE_NAME +
                    "." + WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING + " = ? AND " +
                    WeatherContract.WeatherEntry.COLUMN_DATE + " = ? ";

    private Cursor getWeatherByLocationSetting(Uri uri, String[] projection, String sortOrder) {
        String locationSetting = WeatherContract.WeatherEntry.getLocationSettingFromUri(uri);
        long startDate = WeatherContract.WeatherEntry.getStartDateFromUri(uri);

        // get selection parameters from the provided uri to be used in the query
        String[] selectionArgs;
        String selection;

        if (startDate == 0) {
            selection = sLocationSettingSelection;
            selectionArgs = new String[]{locationSetting};
        } else {
            selectionArgs = new String[]{locationSetting, Long.toString(startDate)};
            selection = sLocationSettingWithStartDateSelection;
        }

        return sWeatherByLocationSettingQueryBuilder.query(mOpenHelper.getReadableDatabase(),
                projection,
                selection,
                selectionArgs,
                null,
                null,
                sortOrder
        );
    }

    private Cursor getWeatherByLocationSettingAndDate(
            Uri uri, String[] projection, String sortOrder) {
        String locationSetting = WeatherContract.WeatherEntry.getLocationSettingFromUri(uri);
        long date = WeatherContract.WeatherEntry.getDateFromUri(uri);

        return sWeatherByLocationSettingQueryBuilder.query(mOpenHelper.getReadableDatabase(),
                projection,
                sLocationSettingAndDaySelection,
                new String[]{locationSetting, Long.toString(date)},
                null,
                null,
                sortOrder
        );
    }

    /*
        Students: Here is where you need to create the UriMatcher. This UriMatcher will
        match each URI to the WEATHER, WEATHER_WITH_LOCATION, WEATHER_WITH_LOCATION_AND_DATE,
        and LOCATION integer constants defined above.  You can test this by uncommenting the
        testUriMatcher test within TestUriMatcher.
     */
    static UriMatcher buildUriMatcher() {
        // 1) The code passed into the constructor represents the code to return for the root
        // URI.  It's common to use NO_MATCH as the code for this case. You don't want the root
        // note to match anything. Add the constructor below.
        UriMatcher um = new UriMatcher(UriMatcher.NO_MATCH);

        // shortcut to refer to CONTENT_AUTHORITY
        String authority = WeatherContract.CONTENT_AUTHORITY;

        // 2) Use the addURI function to match each of the types.  Use the constants from
        // WeatherContract to help define the types to the UriMatcher.
        // /* is for a path string
        // /# is for a number string
        //
        // CONJECTURE FOR HOW URIMATCHER WORKS:
        // UriMatcher takes in an incoming uri that is going to access the Content Provider and
        //  and outputs an integer corresponding with the uri form that has been matched
        // This integer would then be used in a switch statement to retrieve the appropriate
        //  piece of data.
        um.addURI(authority, WeatherContract.PATH_WEATHER, WEATHER);
        um.addURI(authority, WeatherContract.PATH_WEATHER + "/*", WEATHER_WITH_LOCATION);
        um.addURI(authority, WeatherContract.PATH_WEATHER + "/*/#", WEATHER_WITH_LOCATION_AND_DATE);
        um.addURI(authority, WeatherContract.PATH_LOCATION, LOCATION);

        // 3) Return the new matcher!
        return um;
    }

    /*
        Students: We've coded this for you.  We just create a new WeatherDbHelper for later use
        here.
     */
    @Override
    public boolean onCreate() {
        mOpenHelper = new WeatherDbHelper(getContext());
        return true;
    }

    /*
        Students: Here's where you'll code the getType function that uses the UriMatcher.  You can
        test this by uncommenting testGetType in TestProvider.

     */
    @Override
    public String getType(Uri uri) {

        // Use the Uri Matcher to determine what kind of URI this is.
        final int match = sUriMatcher.match(uri);

        switch (match) {
            // Student: Uncomment and fill out these two cases. All that has to be done is to
            //  return whether the content is a single item or a directory
            case WEATHER_WITH_LOCATION_AND_DATE:
                return WeatherContract.WeatherEntry.CONTENT_ITEM_TYPE;
            case WEATHER_WITH_LOCATION:
                return WeatherContract.WeatherEntry.CONTENT_TYPE;
            case WEATHER:
                return WeatherContract.WeatherEntry.CONTENT_TYPE;
            case LOCATION:
                return WeatherContract.LocationEntry.CONTENT_TYPE;
            default:
                throw new UnsupportedOperationException("Unknown uri: " + uri);
        }
    }

    // query() returns a cursor that points to the table of data created using the specifications
    //  provided in the arguments to query(). The cursor could then be used to access data of each
    //  individual row of the given table. Briefly, this table is the same table that would be
    //  outputted from the string of SQLite commands.
    @Override
    public Cursor query(Uri uri, String[] projection, String selection, String[] selectionArgs,
                        String sortOrder) {
        // Here's the switch statement that, given a URI, will determine what kind of request it is,
        // and query the database accordingly.
        Cursor retCursor;
        switch (sUriMatcher.match(uri)) {
            // "weather/*/#"
            case WEATHER_WITH_LOCATION_AND_DATE:
            {
                retCursor = getWeatherByLocationSettingAndDate(uri, projection, sortOrder);
                break;
            }
            // "weather/*"
            case WEATHER_WITH_LOCATION: {
                retCursor = getWeatherByLocationSetting(uri, projection, sortOrder);
                break;
            }
            // "weather"
            case WEATHER: {
                retCursor = mOpenHelper.getWritableDatabase().query(
                        WeatherContract.PATH_WEATHER,
                        projection,
                        selection,
                        selectionArgs,
                        null, null,
                        sortOrder
                );
                break;
            }
            // "location"
            case LOCATION: {
                retCursor = mOpenHelper.getWritableDatabase().query(
                        WeatherContract.PATH_LOCATION,
                        projection,
                        selection,
                        selectionArgs,
                        null, null,
                        sortOrder
                );
                break;
            }

            default:
                throw new UnsupportedOperationException("Unknown uri: " + uri);
        }
        // Registers a content observer to watch for changes in that uri, which allows content provider
        //  to tell UI when cursor changes
        retCursor.setNotificationUri(getContext().getContentResolver(), uri);
        return retCursor;
    }

    /*
        Student: Add the ability to insert Locations to the implementation of this function.
        Only accepts WEATHER and LOCATION uri requests
        insert() inserts a new row into the provider (i.e. database). The uri specifies the
          database to be the destination table and the content values represents the column
          values to use. The returned uri is the content uri for the newly-inserted row.
        insert() also notifies all Content Observers that might have data is modified by the insert(),
          through Content Resolver. Content Resolver would notify Content Observers
        Notifying root uri also notifies all decendents of that uri, ones that contain additional
          path information
          (e.g. content://com.example.android.sunshine.app/weather and
                content://com.example.android.sunshine.app/weather/[date] )
        Notifying on anything other than root uri means cursor listening on root uri won't get
          notified of change
     */
    @Override
    public Uri insert(Uri uri, ContentValues values) {
        final SQLiteDatabase db = mOpenHelper.getWritableDatabase();

        // Only requires the base URIs (/weather and /location). .insert() is simple in that it just
        //  makes sure that the right record ends up at the right table. paths and queries to
        //  location and date will be provided through the ContentValues object
        final int match = sUriMatcher.match(uri);
        Uri returnUri;

        switch (match) {
            case WEATHER: {
                normalizeDate(values);

                // when inserting a row into the db, a long is returned specifying the _id of
                //  the row that has been inserted. if there is an error, it returns -1, at which
                //  case you throw an exception
                long _id = db.insert(WeatherContract.WeatherEntry.TABLE_NAME, null, values);
                if ( _id > 0 )
                    returnUri = WeatherContract.WeatherEntry.buildWeatherUri(_id);
                else
                    throw new android.database.SQLException("Failed to insert row into " + uri);
                break;
            }
            case LOCATION: {
                long _id = db.insert(WeatherContract.LocationEntry.TABLE_NAME, null, values);
                if (_id > 0)
                    returnUri = WeatherContract.LocationEntry.buildLocationUri(_id);
                else
                    throw new android.database.SQLException("Failed to insert row into " + uri);
                break;
            }
            default:
                throw new UnsupportedOperationException("Unknown uri: " + uri);
        }

        // Notifies any registered ContentObserver of a change so as to reflect the change in the UI
        // Make sure you pass in the passed in uri and not the return uri, or else it would not
        //  correctly notify cursors of the change
        getContext().getContentResolver().notifyChange(uri, null);

        // DO NOT HAVE DB.CLOSE() STATEMENTS IN CONTENT PROVIDER
        return returnUri;
    }

    @Override
    public int delete(Uri uri, String selection, String[] selectionArgs) {
        // Student: Start by getting a writable database
        final SQLiteDatabase db = mOpenHelper.getWritableDatabase();
        int rowsDeleted;

        // To remove all rows and get a count of the number of rows removed, pass "1" as the whereClause
        if (selection == null)
            selection = "1";

        // Student: Use the uriMatcher to match the WEATHER and LOCATION URI's we are going to
        // handle.  If it doesn't match these, throw an UnsupportedOperationException.
        switch (sUriMatcher.match(uri)) {
            case WEATHER: {

                // SQLiteDatabase.delete(table, whereClause, whereArgs) is a convenience method for
                //  deleting rows in database.
                // If whereClause (selection) is null, all rows will be deleted
                // All ?'s in whereClause (selection) is replaced by values in whereArgs (selectionArgs)
                // The returned value is the number of rows deleted, if a whereClause is passed in,
                //  otherwise 0.
                rowsDeleted = db.delete(WeatherContract.WeatherEntry.TABLE_NAME, selection, selectionArgs);
                break;
            }
            case LOCATION: {
                rowsDeleted = db.delete(WeatherContract.LocationEntry.TABLE_NAME, selection, selectionArgs);
                break;
            }
            default:
                throw new UnsupportedOperationException("Unknown uri: " + uri);
        }

        // Student: A null selection value deletes all rows.  In my implementation of this, I only notified
        // the uri listeners (using the content resolver) if the rowsDeleted != 0 or the selection
        // is null.
        // Oh, and you should notify the listeners here.
        if (rowsDeleted != 0)
            getContext().getContentResolver().notifyChange(uri, null);

        // Student: return the actual rows deleted
        return rowsDeleted;
    }

    private void normalizeDate(ContentValues values) {
        // normalize the date value
        if (values.containsKey(WeatherContract.WeatherEntry.COLUMN_DATE)) {
            long dateValue = values.getAsLong(WeatherContract.WeatherEntry.COLUMN_DATE);
            values.put(WeatherContract.WeatherEntry.COLUMN_DATE, WeatherContract.normalizeDate(dateValue));
        }
    }

    @Override
    public int update(
            Uri uri, ContentValues values, String selection, String[] selectionArgs) {
        // Student: This is a lot like the delete function.  We return the number of rows impacted
        // by the update.
        final SQLiteDatabase db = mOpenHelper.getWritableDatabase();
        int rowsUpdated;

        switch (sUriMatcher.match(uri)) {
            case WEATHER: {

                // SQLiteDatabase.update(table, values, whereClause, whereArgs) is a convenience
                //  method for updating rows in database
                // Values is a mapping of column names and new column values. If values is null,
                //  then NULL will be passed as the new value of the column
                // whereClause (selection) indicates which rows to update. Null will update
                //  all rows.
                rowsUpdated = db.update(WeatherContract.WeatherEntry.TABLE_NAME, values, selection, selectionArgs);
                break;
            }
            case LOCATION: {
                rowsUpdated = db.update(WeatherContract.LocationEntry.TABLE_NAME, values, selection, selectionArgs);
                break;
            }
            default:
                throw new UnsupportedOperationException("Unknown uri: " + uri);
        }

        // Only notify ContentResolver if at least one row has been updated. This is if
        //  rowsUpdated is not 0 or if the selection is null (meaning all rows have been updated).
        if (rowsUpdated != 0)
            getContext().getContentResolver().notifyChange(uri, null);

        // Student: return the actual rows deleted
        return rowsUpdated;
    }

    // This is an optional method that allows putting a bunch of inserts into a single transaction
    //  instead of inserting rows individually. Looking closely, it contains the major snippet
    //  used in the .update() method implemented above
    // This performs much faster than doing additions individually.
    @Override
    public int bulkInsert(Uri uri, ContentValues[] values) {
        final SQLiteDatabase db = mOpenHelper.getWritableDatabase();
        final int match = sUriMatcher.match(uri);
        switch (match) {

            // Only weather transactions are done in bulk
            case WEATHER:
                // Start with beginTransaction()
                db.beginTransaction();
                // Increment returnCount for each record inserted
                int returnCount = 0;
                try {
                    // Each iteration looks a lot like the .insert() written above
                    for (ContentValues value : values) {
                        normalizeDate(value);
                        long _id = db.insert(WeatherContract.WeatherEntry.TABLE_NAME, null, value);
                        if (_id != -1) {
                            returnCount++;
                        }
                    }
                    // When done, set transaction to be successful
                    db.setTransactionSuccessful();
                } finally {
                    // When done, end the transaction. This will commit all records. If
                    //  .setTransactionSuccessful() is not called, then records will NOT be commited
                    //  when .endTransaction() is called
                    db.endTransaction();
                }
                getContext().getContentResolver().notifyChange(uri, null);
                return returnCount;
            default:
                return super.bulkInsert(uri, values);
        }
    }

    // You do not need to call this method. This is a method specifically to assist the testing
    // framework in running smoothly. You can read more at:
    // http://developer.android.com/reference/android/content/ContentProvider.html#shutdown()
    @Override
    @TargetApi(11)
    public void shutdown() {
        mOpenHelper.close();
        super.shutdown();
    }
}