


    gwig.followersNg.services.followers.apiSelectById = function (data, selectByIdOnSuccess, selectByIdOnError) {
        var url =  "/api/followers/GetById";

        var settings = {
            cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            ,data: data
            , success: selectByIdOnSuccess
            , error: selectByIdOnError
            , type: "GET"
        };
        $.ajax(url, settings);

    }

    gwig.followersNg.services.followers.apiAddFollower = function (data, onSuccess, onError) {
        var url =  "/api/followers"

        var settings = {

            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: data,
            type: "POST",
            dataType: "json",
            success: onSuccess,
            error: onError,

        };

        $.ajax(url, settings)
    }



    gwig.followersNg.services.followers.apiDeleteById = function (id, onSuccess, onError) {

        var url =  "/api/followers/" + id

        var settings = {

            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            type: "DELETE",
            dataType: "json",
            success: onSuccess,
            error: onError,
        }
        $.ajax(url, settings)
    }

    gwig.followersNg.services.followers.getFollowers = function (data, selectByIdOnSuccess, selectByIdOnError) {
        var url = "/api/following/GetById";

        var settings = {
            cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            ,data: data
            , success: selectByIdOnSuccess
            , error: selectByIdOnError
            , type: "GET"
        };
        $.ajax(url, settings);

    }

    gwig.followersNg.services.followers.getFriends= function (data, selectByIdOnSuccess, selectByIdOnError) {
        var url = "/api/following/GetFriends";

        var settings = {
            cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , data: data
            , success: selectByIdOnSuccess
            , error: selectByIdOnError
            , type: "GET"
        };
        $.ajax(url, settings);

    }

    gwig.followersNg.services.followers.unfollow = function (id, onSuccess, onError) {

        var url = "/api/following/" + id

        var settings = {

            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            type: "DELETE",
            dataType: "json",
            success: onSuccess,
            error: onError,
        }
        $.ajax(url, settings)
    }
    gwig.followersNg.services.followers.getCurrentUser = function (onAjaxSuccess, onAjaxError) {
        var url = sabio.services.apiPrefix + "api/userprofile/current";
        var settings = {
            cache: false
                , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
                , dataType: "json"
                , success: onAjaxSuccess
                , error: onAjaxError
                , type: "GET"
        };

        $.ajax(url, settings);

    }
}
