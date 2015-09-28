package com.thirdarm.testretrofit.API;

import com.thirdarm.testretrofit.model.gitmodel;


import retrofit.Call;
import retrofit.http.GET;
import retrofit.http.Path;

/**
 * Created by TROD on 20150908.
 */
public interface gitapi {

    @GET("users/{user}")
    Call<gitmodel> getFeedSync(@Path("user") String user);


}
