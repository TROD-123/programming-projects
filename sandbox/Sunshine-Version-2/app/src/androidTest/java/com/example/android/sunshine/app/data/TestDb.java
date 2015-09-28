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

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.test.AndroidTestCase;

import java.util.HashSet;

public class TestDb extends AndroidTestCase {

    public static final String LOG_TAG = TestDb.class.getSimpleName();

    // Since we want each test to start with a clean slate
    void deleteTheDatabase() {
        mContext.deleteDatabase(WeatherDbHelper.DATABASE_NAME);
    }

    /*
        This function gets called before each test is executed to delete the database.  This makes
        sure that we always have a clean test.
     */
    public void setUp() {
        deleteTheDatabase();
    }

    /*
        Students: Uncomment this test once you've written the code to create the Location
        table.  Note that you will have to have chosen the same column names that I did in
        my solution for this test to compile, so if you haven't yet done that, this is
        a good time to change your column names to match mine.

        Note that this only tests that the Location table has the correct columns, since we
        give you the code for the weather table.  This test does not look at the
     */
    public void testCreateDb() throws Throwable {
        // build a HashSet of all of the table names we wish to look for
        // Note that there will be another table in the DB that stores the
        // Android metadata (db version information)
        final HashSet<String> tableNameHashSet = new HashSet<String>();
        tableNameHashSet.add(WeatherContract.LocationEntry.TABLE_NAME);
        tableNameHashSet.add(WeatherContract.WeatherEntry.TABLE_NAME);

        mContext.deleteDatabase(WeatherDbHelper.DATABASE_NAME);
        SQLiteDatabase db = new WeatherDbHelper(
                this.mContext).getWritableDatabase();
        assertEquals(true, db.isOpen());

        // have we created the tables we want?
        Cursor c = db.rawQuery("SELECT name FROM sqlite_master WHERE type='table'", null);

        assertTrue("Error: This means that the database has not been created correctly",
                c.moveToFirst());

        // verify that the tables have been created
        do {
            tableNameHashSet.remove(c.getString(0));
        } while( c.moveToNext() );

        // if this fails, it means that your database doesn't contain both the location entry
        // and weather entry tables
        assertTrue("Error: Your database was created without both the location entry and weather entry tables",
                tableNameHashSet.isEmpty());

        // now, do our tables contain the correct columns?
        c = db.rawQuery("PRAGMA table_info(" + WeatherContract.LocationEntry.TABLE_NAME + ")",
                null);

        assertTrue("Error: This means that we were unable to query the database for table information.",
                c.moveToFirst());

        // Build a HashSet of all of the column names we want to look for
        final HashSet<String> locationColumnHashSet = new HashSet<String>();
        locationColumnHashSet.add(WeatherContract.LocationEntry._ID);
        locationColumnHashSet.add(WeatherContract.LocationEntry.COLUMN_CITY_NAME);
        locationColumnHashSet.add(WeatherContract.LocationEntry.COLUMN_COORD_LAT);
        locationColumnHashSet.add(WeatherContract.LocationEntry.COLUMN_COORD_LONG);
        locationColumnHashSet.add(WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING);

        int columnNameIndex = c.getColumnIndex("name");
        do {
            String columnName = c.getString(columnNameIndex);
            locationColumnHashSet.remove(columnName);
        } while(c.moveToNext());

        // if this fails, it means that your database doesn't contain all of the required location
        // entry columns
        assertTrue("Error: The database doesn't contain all of the required location entry columns",
                locationColumnHashSet.isEmpty());
        db.close();
    }

    /*
        Students:  Here is where you will build code to test that we can insert and query the
        location database.  We've done a lot of work for you.  You'll want to look in TestUtilities
        where you can uncomment out the "createNorthPoleLocationValues" function.  You can
        also make use of the ValidateCurrentRecord function from within TestUtilities.
    */
    public void testLocationTable() {
        insertLocation();
    }

    /*
        Students:  Here is where you will build code to test that we can insert and query the
        database.  We've done a lot of work for you.  You'll want to look in TestUtilities
        where you can use the "createWeatherValues" function.  You can
        also make use of the validateCurrentRecord function from within TestUtilities.
     */
    public void testWeatherTable() {
        // First insert the location, and then use the locationRowId to insert
        // the weather. Make sure to cover as many failure cases as you can.
        long location_row_id = insertLocation();

        // Instead of rewriting all of the code we've already written in testLocationTable
        // we can move this code to insertLocation and then call insertLocation from both
        // tests. Why move it? We need the code to return the ID of the inserted location
        // and our testLocationTable can only return void because it's a test.

        // First step: Get reference to writable database
        SQLiteDatabase db = new WeatherDbHelper(this.mContext).getWritableDatabase();

        // Create ContentValues of what you want to insert
        // (you can use the createWeatherValues TestUtilities function if you wish)
        ContentValues cv = TestUtilities.createWeatherValues(location_row_id);

        // Insert ContentValues into database and get a row ID back
        long weather_row_id = db.insert(WeatherContract.WeatherEntry.TABLE_NAME, null, cv);

        // Query the database and receive a Cursor back
        Cursor c = db.query(
                WeatherContract.WeatherEntry.TABLE_NAME,
                null,
                null,
                null,
                null,
                null,
                null
        );

        // Move the cursor to a valid database row
        assertTrue("Error: No records returned from weather query ",
                c.moveToFirst());

        // Validate data in resulting Cursor with the original ContentValues
        // (you can use the validateCurrentRecord function in TestUtilities to validate the
        // query if you like)
        TestUtilities.validateCurrentRecord(
                "Error: Weather Query Validation Failed",
                c, cv);

        // Make sure there is only one record in database
        assertFalse(
                "Error: More than one record returned from weather query",
                c.moveToNext()
        );

        // Finally, close the cursor and database
        c.close();
        db.close();
    }


    /*
        Students: This is a helper method for the testWeatherTable quiz. You can move your
        code from testLocationTable to here so that you can call this code from both
        testWeatherTable and testLocationTable.
     */
    public long insertLocation() {

        // First step: Get reference to writable database
        SQLiteDatabase db = new WeatherDbHelper(this.mContext).getWritableDatabase();

        // Create ContentValues of what you want to insert
        // (you can use the createNorthPoleLocationValues if you wish)
        ContentValues cv = TestUtilities.createNorthPoleLocationValues();

//        // Test data we're going to insert into the DB to see if it works.
//        String testLocationSetting = "99705";
//        String testCityName = "North Pole";
//        double testLatitude = 64.7488;
//        double testLongitude = -147.353;
//
//        cv.put(WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING, testLocationSetting);
//        cv.put(WeatherContract.LocationEntry.COLUMN_CITY_NAME, testCityName);
//        cv.put(WeatherContract.LocationEntry.COLUMN_COORD_LAT, testLatitude);
//        cv.put(WeatherContract.LocationEntry.COLUMN_COORD_LONG, testLongitude);

        // Insert ContentValues into database and get a row ID back
        long row_id = db.insert(WeatherContract.LocationEntry.TABLE_NAME, null, cv);
        assertTrue("Error: Failure to insert North Pole Location Values", row_id != -1);

        // Query the database and receive a Cursor back. Make sure that the cursor SELECT
        //  returns all the necessary columns, or else there will be an error with the
        //  validateCurrentRecord() function, as not being able to read missing columns SELECTED
        Cursor c = db.query(
                WeatherContract.LocationEntry.TABLE_NAME,
                new String[]{
                        WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING,
                        WeatherContract.LocationEntry.COLUMN_CITY_NAME,
                        WeatherContract.LocationEntry.COLUMN_COORD_LAT,
                        WeatherContract.LocationEntry.COLUMN_COORD_LONG
                },
                WeatherContract.LocationEntry.COLUMN_LOCATION_SETTING + "= ?",
                new String[]{"99705"},
                null, null, null);

        // OPTIMAL CURSOR CODE BELOW
        // By default, if you want the cursor to include all of the rows and columns, regardless
        //  of their values, fill out the first argument and leave all other arguments null.
//        Cursor c = db.query(
//                WeatherContract.LocationEntry.TABLE_NAME, // Table to query [uri - content uri of the table]]
//                null,  // all columns [projection - the array of columns that should be included for each row retrieved]
//                null,  // columns for the "where" clause [selection - specifies the criteria for selecting rows]
//                null,  // values for the "where" clause
//                null,  // columns to group by
//                null,  // columns to filter by row groups
//                null   // sort order
//        )

        // Move the cursor to a valid database row
        assertTrue("Error: No records returned from location query ",
                c.moveToFirst());

        // Validate data in resulting Cursor with the original ContentValues
        // (you can use the validateCurrentRecord function in TestUtilities to validate the
        // query if you like)
        TestUtilities.validateCurrentRecord(
                "Error: Location Query Validation Failed",
                c, cv);

        // Make sure there is only one record in database
        assertFalse(
                "Error: More than one record returned from location query",
                c.moveToNext()
        );

        // Finally, close the cursor and database
        c.close();
        db.close();

        return row_id;
    }
}
