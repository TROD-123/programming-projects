package com.thirdarm.testpicasso;

import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;

import info.movito.themoviedbapi.TmdbApi;
import info.movito.themoviedbapi.TmdbMovies;
import info.movito.themoviedbapi.model.MovieDb;
import info.movito.themoviedbapi.model.core.ResultsPage;

public class MainActivity extends AppCompatActivity {

    public final String LOG_TAG = "MY APPPPP";

    public static final String IMAGE_URL0 = "https://cms-assets.tutsplus.com/uploads/users/21/posts/19431/featured_image/CodeFeature.jpg";
    public static final String IMAGE_URL1 = "https://www.shirtpunch.com/imagefly/-c-w465-h650/resources/media/images/designs/8cbc2_design_large.jpg";
    public static final String IMAGE_URL2 = "https://www.shirtpunch.com/imagefly/-c-w465-h650/resources/media/images/designs/546a1_design_large.jpg";
    public static final String IMAGE_URL3 = "https://www.shirtpunch.com/imagefly/-c-w465-h650/resources/media/images/designs/de3db_design_large.jpg";
    public static final String IMAGE_URL4 = "https://www.shirtpunch.com/imagefly/-c-w465-h650/resources/media/images/designs/5c328_design_large.jpg";
    public static final String IMAGE_URL5 = "https://www.google.com";

    public static final int IMAGE_WIDTH = 100;
    public static final int IMAGE_HEIGHT = 100;

    public static final String PLACEHOLDER_URL = "http://taimapedia.org/images/5/5f/Placeholder.jpg";

    int[] movieIds = {550, 671, 100, 150, 200, 350};
    public String[] IMAGE_URLS = {IMAGE_URL0, IMAGE_URL1, IMAGE_URL2, IMAGE_URL3, IMAGE_URL4, IMAGE_URL5};


    public ArrayList<TextView> textViews;
    public ArrayList<MovieDb> movies;

    public final String language = "en";
    public Context mContext;
    public final String POSTER_SIZE = "w500";
    public final int GRID_PADDING = 0;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mContext = this;

        // Fill the gridView with posters
        new FetchMovies().execute(0);

        // have app do something when user clicks on a grid element






        // TODO: Ultimately, transfer all the successful progress done here into the Movies app.
        //  Commit each change one at a time.

        // TODO: Figure out how to populate the movieIds list with ids of those movies rated most
        //  popular. Need to figure out how to access a list of the most popular movies from TMDB.
        //  Also include an indication of how many movies MAX that list should be populated.

        // TODO: Implement a method(s) that would allow extraction of ANY kind of information from each
        //  movie object in the movies ArrayList. Also note that extraction can ONLY be done
        //  once the movies ArrayList has been populated (after the FetchMovies AsyncTask has been
        //  completed.

        // TODO: After implementing the above methods, populate the GridView with movie posters
        //  for each of the movies in the list. This requires populating the IMAGE_URLS list with
        //  urls for each of the movie posters. After changing the IMAGE_URLS list, the grid
        //  should automatically be populated with the movie posters.

        // TODO: Implement an onClickListener for each of the movie posters so that when the user
        //  clicks on a poster, a Toast message should appear, saying the name of the movie
        //  corresponding with the poster. Afterwards, allow clicks to start a new, GENERIC
        //  activity that displays information about that selected movie. Use INTENTS to send data
        //  which would be shown in that activity!!!

    }

    // Class that generates an ArrayList of MovieDb objects (each object contains all information
    //  for each movie). AsyncTask is required for accessing network to grab JSON data.
    // Creates a TmdbMovies object which allows access to the API and retrieval of any Movie object
    // Also creates a list of MovieDb objects to be shown in the gridView.
    // MovieDb objects contain all information about a movie, and information retrieval for each
    //  movie can be done through different methods
    //
    // EDIT: Instead of creating a TmdbMovies object, the doInBackground method creates a ResultsPage<MovieDb>
    //  object that contains a list of movies in some order specified by the user and represented as
    //  an integer code passed into the Asynctask execute() method.
    //  Integer codes:
    //      0 - popular movies
    //      1 - top rated movies
    //      2 - upcoming movies
    //      3 - now playing movies
    public class FetchMovies extends AsyncTask<Integer, Void, ArrayList<MovieDb>> {
        @Override protected ArrayList<MovieDb> doInBackground(Integer... params) {
            TmdbMovies movies = new TmdbApi(getString(R.string.key)).getMovies();
            ResultsPage<MovieDb> moviesResultPage;
            ArrayList<MovieDb> movieList = new ArrayList<>();

            switch (params[0]) {

                // if Integer code is 0, return popular movie list
                case 0 : {
                    moviesResultPage = movies.getPopularMovieList(language, 1);
                    Log.d(LOG_TAG, moviesResultPage.getResults().size() + ""); // 20
                    for (int i = 0; i < moviesResultPage.getResults().size(); i++) {
                        movieList.add(movies.getMovie(moviesResultPage.getResults().get(i).getId(), language));
                        Log.d(LOG_TAG, "movie added to list");
                    }
                    break;
                }
            }
            return movieList;
        }

        // Currently generates and posts a long string consisting of homepage URLs of each
        //  movie collected into the movieList. This will need to be changed into something more
        //  general.
        @Override protected void onPostExecute(ArrayList<MovieDb> result) {
            if (result != null) {

                movies = result;
                // Set images in gridView test
                ArrayList<String> poster_urls = new ArrayList<>();
                String url_base = "http://image.tmdb.org/t/p/" + POSTER_SIZE;
                for (MovieDb movie : movies) {
                    poster_urls.add(url_base + movie.getPosterPath());
                }

                initializeGridView((GridView) findViewById(R.id.gridView), mContext, poster_urls);
            }
        }
    }

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

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    // used to set the posters in the gridView
    public void initializeGridView(GridView gridView, Context c, ArrayList<String> urls) {
        // set posters
        gridView.setAdapter(new ImagesAdapter(c, urls));

        // apply listener
        gridView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            // The int that is passed through onItemClick() is the position of the item in the
            // adapter.
            public void onItemClick(AdapterView<?> parent, View v, int position, long id) {
                Log.d(LOG_TAG, "Item position: " + position);
                //Toast.makeText(mContext, movies.get(position).getTitle(), Toast.LENGTH_SHORT).show();
                Intent intent = new Intent(MainActivity.this, DetailActivity.class);
                intent.putExtra("MOVIE_DATA", movies.get(position).getId());
                startActivity(intent);

            }
        });
    }

    // ArrayAdapter for holding the movie posters. Custom adapter will be the source for all items
    //  to be displayed in the grid.
    // Closely follows BaseAdapter template as outlined in the DAC GridView tutorial
    //  Link here: http://developer.android.com/guide/topics/ui/layout/gridview.html
    public class ImagesAdapter extends BaseAdapter {
        private Context mContext;
        private ArrayList<String> image_urls;

        public ImagesAdapter(Context c, ArrayList<String> images) {
            mContext = c;
            image_urls = images;
        }

        public int getCount() {
            return image_urls.size();
        }

        // returns the actual object at specified position
        public Object getItem(int position) {
            return null;
        }

        // returns the row id of the object at specified position
        public long getItemId(int position) {
            return 0;
        }

        // creates a new view (in this case, ImageView) for each item referenced by the Adapter
        // How it works:
        //  - a view is passed in, which is normally a recycled object
        //  - checks to see if that view is null
        //     - if view is null, a view is initialized and configured with desired properties
        //     - if view is not null, that view is then returned
        public View getView(int position, View convertView, ViewGroup parent) {
            ImageView imageView;
            if (convertView == null) {
                // if it's not recycled, initialize some attributes
                imageView = new ImageView(mContext);
                imageView.setLayoutParams(new GridView.LayoutParams(185, 277));
                imageView.setScaleType(ImageView.ScaleType.CENTER_CROP);
                imageView.setPadding(GRID_PADDING, GRID_PADDING, GRID_PADDING, GRID_PADDING);
            } else {
                imageView = (ImageView) convertView;
            }

            Picasso.with(MainActivity.this)
                    .load(image_urls.get(position))
                    .placeholder(R.drawable.piq_76054_400x400)
                    .error(R.drawable.piq_76054_400x400)
                    .into(imageView);

            // GridView refreshes every time an element goes out of range.
            // TODO: Find a way to cache the images and the data so that data does not get
            //  loaded each and every time.
            Log.d(LOG_TAG, "gridView element refreshed");

            return imageView;
        }
    }
}
