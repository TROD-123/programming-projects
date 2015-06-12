/** Modern Art UI, written by Teddy Rodriguez
 *
 *  An exercise in creating a dynamic user interface with dialogs, an
 *   app bar, and a seek bar.
 *  UI consists of rectangles of various randomized sizes, positions, and
 *   colors. Seek bar allows user to dynamically manipulate the colors of
 *   each of the rectangles. The rectangle generated with white remains
 *   static.
 *  A dialog presents the user with an advertisement of the Museum of
 *   Modern Art (MOMA) as a source of influence for the UI's design.
 *
 *  Written for Programming Mobile Applications for Android Handheld
 *   Systems: Part 1, a Coursera course hosted by Dr. Adam Porter of the
 *   University of Maryland
 *
 *  NOTE: Contains many blocks of commented code which demonstrate older
 *   procedures used. Saved for future reference.
 */

package com.thirdarm.modernartui;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.app.FragmentManager;
import android.app.FragmentTransaction;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.Color;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.SeekBar;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Random;


public class MainActivity extends Activity {

    // global data
    public static final int SETUI_REQUEST = 0;
    public static final int URLTAG = 0, ABOUTTAG = 1, CUSTOMTAG = 2;
    private static final String NUM_LINEAR_LAYOUTS = "NUM_LINEAR_LAYOUTS";
    private static final String NUM_TEXT_VIEWS = "NUM_TEXT_VIEWS";
    private static final String MOMA_URL = "http://www.moma.org";
    public RelativeLayout globalLayout;
    public LinearLayout mainLayout;

    // UI maximum rectangles
    public static final int MAX_LINEAR_LAYOUTS = 5;
    public static final int MAX_TEXT_VIEWS = 5;
    public static final int MAX_WEIGHT = 5;

    // UI user-set number of rectangles
    public int nLinearLayouts = 0;
    public int nTextViews = 0;

    // one DialogFragment instance for all Dialogs
    private DialogFragment mDialog;

    private SeekBar mSeekBar;

    // ArrayList to store rectangles (custom wrapper class) and each of their properties:
    //  -TextView
    //  -Array of differences of rgb components
    //  -Start color (int)
    //  -End color (int)
    public ArrayList<Rectangle> rectangles = new ArrayList<Rectangle>();

    // ArrayList to store LinearLayouts
    public ArrayList<LinearLayout> linearLayouts = new ArrayList<LinearLayout>();

    // ArrayList to store TextViews programmed in layout xml
    public ArrayList<TextView> textViews = new ArrayList<TextView>();


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // get xml layout for ui
        setContentView(R.layout.activity_main);

        // custom functions for getting ui elements
        initializeUI();
        setRectangleProperties();

        // listener for SeekBar
        mSeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            int pv;
            double max;

            // This method is called each time the SeekBar is changed (meaning, it
            //  is called for every increment that the SeekBar moves)
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                pv = progress;
                max = seekBar.getMax() * 1.0;

                // Set the background colors based on SeekBar thumb location
                for (Rectangle rectangle : rectangles) {
                    for (int i = 0; i < rectangle.getDColors().length; i++) {
                        rectangle.getTextView().setBackgroundColor(Color
                                        .rgb((int) (Color.red(rectangle.getColorStart()) +
                                                        pv / max * rectangle.getDColors()[0]),
                                                (int) (Color.green(rectangle.getColorStart()) +
                                                        pv / max * rectangle.getDColors()[1]),
                                                (int) (Color.blue(rectangle.getColorStart()) +
                                                        pv / max * rectangle.getDColors()[2]))
                        );
                    }
                }
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });
    }

    // TODO: Set up an autoSeek function that allows colors to fluctuate automatically
    //  and so that start and end colors change after every rotation
    // the thing about this is that it needs to constantly be called so that
    //  the seek changes can continuously be done
    // this might need to run continuously in the background, but this does not
    //  need to continue running even after the activity is destroyed
    // haven't learned any of automation procedures yet
    // a service may not be needed, but something else instead
    private void autoSeek() {

        // get value of seekBar to assess whether to increment or decrement
        int value = mSeekBar.getProgress();

        // get values of clock

    }

    // TODO: Preserve layout when switching orientations


    // TODO: Create icons for action bar buttons and minimize entries in action overflow


    // TODO: Allow users to take snapshots of current view configurations and save them
    //  -Save image of configuration
    //  -Save properties of configuration for future load


    // TODO: Allow users to interact with each rectangle by touching them
    //  -Color drop animations
    //  -


    // For getting result from SetUI Activity
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {

            case SETUI_REQUEST:
                if (resultCode == RESULT_OK) {
                    Toast.makeText(getApplicationContext(), "Settings saved",
                            Toast.LENGTH_SHORT).show();
                    nLinearLayouts = data.getExtras().getInt(NUM_LINEAR_LAYOUTS);
                    nTextViews = data.getExtras().getInt(NUM_TEXT_VIEWS);

                } else {
                    Toast.makeText(getApplicationContext(), "Settings not saved",
                            Toast.LENGTH_SHORT).show();
                }
                break;

            default:
                break;
        }
    }

    // Clear all LinearLayouts, TextViews, and Rectangles
    private void resetUI() {
        globalLayout.removeView(mainLayout);
        linearLayouts.clear();
        textViews.clear();
        rectangles.clear();
        mSeekBar.setProgress(0);
    }

    // Initialize UI elements
    private void initializeUI() {

        // Dynamically create LinearLayouts and TextViews (for added
        //  randomness, complexity, and more efficient code)
        Random rand = new Random();

        // Make a main linear layout to hold TextViews and add it to global layout
        globalLayout = (RelativeLayout) findViewById(R.id.globalLayout);
        mainLayout = new LinearLayout(getApplicationContext());

        mainLayout.setLayoutParams(new LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT)
        );
        globalLayout.addView(mainLayout);

        // Create random number of LinearLayouts and add them to linearLayouts list
        int numLinearLayouts = (nLinearLayouts == 0) ? rand.nextInt(MAX_LINEAR_LAYOUTS) + 1 : nLinearLayouts;
        for (int i = 0; i < numLinearLayouts; i++) {
            LinearLayout linearLayout = new LinearLayout(getApplicationContext());
            linearLayout.setOrientation(LinearLayout.VERTICAL);
            linearLayouts.add(linearLayout);}

        for (LinearLayout linearLayout : linearLayouts) {
            mainLayout.addView(linearLayout);
        }

        // Create random number of TextViews for each LinearLayout and add TextViews
        // to textViews list
        for (LinearLayout linearLayout : linearLayouts) {
            int numTextViews = (nTextViews == 0) ? rand.nextInt(MAX_TEXT_VIEWS) + 1 : nTextViews;
            for (int i = 0; i < numTextViews; i++) {
                TextView textView = new TextView(getApplicationContext());
                linearLayout.addView(textView);
                textViews.add(textView);
            }
        }

        // SeekBar
        mSeekBar = (SeekBar) findViewById(R.id.seekBar);
        mSeekBar.setMax(1000);
        mSeekBar.bringToFront();
    }

    // Gives the rectangles random properties (size and start/end colors)
    // and adds them to the ArrayList of rectangles
    private void setRectangleProperties() {

        // random generator
        Random rand = new Random();

        // randomly set weights for LinearLayouts
        for (LinearLayout linearLayout : linearLayouts) {
            linearLayout.setLayoutParams(new LinearLayout.LayoutParams(0,
                    LinearLayout.LayoutParams.MATCH_PARENT, rand.nextInt(MAX_WEIGHT) + 1));
        }

        // for choosing one TextView at random to be white
        int white = rand.nextInt(textViews.size());
        boolean done = false;

        // set TextView properties
        for (TextView textView : textViews) {

            // randomly set weights for TextViews
            textView.setLayoutParams(new LinearLayout.LayoutParams(
                            ViewGroup.LayoutParams.MATCH_PARENT, 0,
                            rand.nextInt(MAX_WEIGHT) + 1)
            );

            int[] colors = new int[2], dColors = new int[3];

            if (!done && white == rectangles.size()) {

                // generate white TextView
                colors[0] = Color.rgb(255, 255, 255); colors[1] = Color.rgb(255, 255, 255);
                done = true;

            } else {

                // generate random start and end colors
                for (int i = 0; i < colors.length; i++) {
                    colors[i] = Color.rgb((int) (255 * rand.nextDouble()),
                            (int) (255 * rand.nextDouble()), (int) (255 * rand.nextDouble()));
                }
            }

            // get differences in rgb color components
            // each value will be 0 if rectangle is white
            dColors[0] = Color.red(colors[1]) - Color.red(colors[0]);
            dColors[1] = Color.green(colors[1]) - Color.green(colors[0]);
            dColors[2] = Color.blue(colors[1]) - Color.blue(colors[0]);

            // create new Rectangle and add to ArrayList of Rectangles
            rectangles.add(new Rectangle(textView, dColors, colors[0], colors[1]));

            // set initial rectangle colors
            textView.setBackgroundColor(colors[0]);
        }
    }


    //
    // The following two methods are for programming the Action Bar menu
    //

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
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
            case R.id.reset:
                resetUI();
                initializeUI();
                setRectangleProperties();

                Toast.makeText(getApplicationContext(), "New rectangles generated",
                        Toast.LENGTH_SHORT).show();

                return true;

            // open MOMA dialog
            case R.id.MOMA_url:
                showDialogFragment(URLTAG);
                return true;

            // open About dialog
            case R.id.About:
                showDialogFragment(ABOUTTAG);
                return true;

            // open Customize dialog
            case R.id.customize:
                showDialogFragment(CUSTOMTAG);
                return true;

            default:
                return super.onOptionsItemSelected(item);
        }
    }

    // Implementation from UIAlertDialog example. Takes in a dialogID and creates and
    // displays the id's corresponding AlertDialog
    void showDialogFragment(int dialogID) {

        switch(dialogID) {

            // show MOMA ad
            case URLTAG:

                // create new AlertDialogFragment
                mDialog = new MOMAFragment();

                // show AlertDialogFragment
                mDialog.show(getFragmentManager(), "moma");

                break;

            // show About
            case ABOUTTAG:

                // create new AlertDialogFragment
                mDialog = new AboutFragment();

                // show AlertDialogFragment
                mDialog.show(getFragmentManager(), "about");

                break;

            // show Customize
            case CUSTOMTAG:

                // start SetUI Activity
                // current values of user-defined numbers of columns and rectangles
                //  are sent to the SetUI activity to illuminate "saving"
                Intent intent = new Intent(MainActivity.this, SetUI.class)
                        .putExtra(NUM_LINEAR_LAYOUTS, nLinearLayouts)
                        .putExtra(NUM_TEXT_VIEWS, nTextViews);
                startActivityForResult(intent, SETUI_REQUEST);
        }
    }

    // Method called by MOMAFragment that determines whether to open the MOMA
    // webpage.
    private void goToMOMA(boolean shouldContinue) {
        if (shouldContinue) {

            // go to MOMA
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setData(Uri.parse(MOMA_URL));
            startActivity(intent);
        } else {

            // dismiss dialog
            mDialog.dismiss();
        }
    }


    //
    // Create dialog fragment for Action Bar buttons
    //

    public static class MOMAFragment extends DialogFragment {

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) {

            return new AlertDialog.Builder(getActivity())

                    .setMessage(R.string.MOMA_msg)
                    .setPositiveButton(R.string.MOMA_yes, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            ((MainActivity) getActivity())
                                    .goToMOMA(true);
                        }
                    })
                    .setNegativeButton(R.string.MOMA_no, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            ((MainActivity) getActivity())
                                    .goToMOMA(false);
                        }
                    }).create();
        }
    }

    public static class AboutFragment extends DialogFragment {

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            return new AlertDialog.Builder(getActivity())
                    .setTitle(R.string.app_about_title)
                    .setMessage(R.string.app_about_msg)
                    .create();
        }
    }

    // Dialog that allows user to manipulate number of ListViews and TextViews per ListView
    public static class SetUIFragment extends DialogFragment {

        // to get DialogFragment layout
        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            return inflater.inflate(R.layout.dialog_custom, container, false);
        }

        // Called only when DialogFragment is shown as Dialog
        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            Dialog dialog = super.onCreateDialog(savedInstanceState);
            dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
            return dialog;
        }
    }


    // Wrapper class

    public class Rectangle {
        private TextView textView;
        private int[] dColors;
        private int colorStart;
        private int colorEnd;

        // Constructor
        public Rectangle(TextView textView, int[] dColors, int colorStart, int colorEnd) {
            this.textView = textView;
            this.dColors = dColors;
            this.colorStart = colorStart;
            this.colorEnd = colorEnd;
        }

        // Getter methods
        public TextView getTextView() { return textView; }

        public int[] getDColors() { return dColors; }

        public int getColorStart() { return colorStart; }

        public int getColorEnd() { return colorEnd; }
    }
}
