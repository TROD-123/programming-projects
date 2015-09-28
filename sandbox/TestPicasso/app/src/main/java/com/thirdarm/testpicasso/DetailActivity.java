package com.thirdarm.testpicasso;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import info.movito.themoviedbapi.TmdbApi;
import info.movito.themoviedbapi.TmdbMovies;
import info.movito.themoviedbapi.model.MovieDb;

public class DetailActivity extends AppCompatActivity {

    TextView mTextView;
    ImageView mImageView;
    Intent intent;
    Context mContext;

    public final String POSTER_SIZE = "w500";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_detail);

        mTextView = (TextView) findViewById(R.id.textView);
        mImageView = (ImageView) findViewById(R.id.imageView);
        intent = getIntent();
        mContext = this;

        // make into a method. setText()
        // Currently just retrieves the int id of the movie and then accesses the server once more
        //  to get the information
        if (intent != null && intent.hasExtra("MOVIE_DATA")) {
            Log.d("HAAAAAAA", "WENT THROUGH!!!!!" + intent.getIntExtra("MOVIE_DATA", 0));
            new Thread(new Runnable() {
                MovieDb movie;

                @Override
                public void run() {
                    movie = new TmdbApi(getString(R.string.key))
                            .getMovies()
                            .getMovie(intent.getIntExtra("MOVIE_DATA", 0), "en");
                    mTextView.post(new Runnable() {
                        @Override
                        public void run() {
                            String title = movie.getTitle();
                            String overview = movie.getOverview();
                            String release_date = movie.getReleaseDate();
                            double vote_average = movie.getVoteAverage();
                            int vote_count = movie.getVoteCount();

                            mTextView.setText(title + "\n\n"
                                    + "Overview" + "\n"
                                    + overview + "\n\n"
                                    + "Released: " + release_date + "\n\n"
                                    + "Rating: " + vote_average + " (" + vote_count + " total votes)");

                            String url_base = "http://image.tmdb.org/t/p/" + POSTER_SIZE;

                            Picasso.with(mContext)
                                    .load(url_base + movie.getPosterPath())
                                    .placeholder(R.drawable.piq_76054_400x400)
                                    .error(R.drawable.piq_76054_400x400)
                                    .into(mImageView);
                        }
                    });
                }
            }).start(); // this line is important. without it, whatever happens in thread will not run
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_detail, menu);
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
}
