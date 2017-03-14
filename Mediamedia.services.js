sabio.services.media = sabio.services.media || {};




sabio.services.media.apiPostMedia = function (data, onSuccess, onError) {
    var url = sabio.services.apiPrefix + "api/media"

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

sabio.services.media.apiPostSinglePlaceMedia = function (data, onSuccess, onError) {
    var url = sabio.services.apiPrefix + "api/media/video/placesUpload"

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

sabio.services.media.apiGetAllMedia = function (onSuccess, onError) {

    var url = sabio.services.apiPrefix + "api/media"

    var settings = {
       
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",
        dataType: "json",
        success: onSuccess,
        error: onError,     
    }

    $.ajax(url, settings)
}

sabio.services.media.apiDeleteById = function (id, onSuccess, onError) {

    var url = sabio.services.apiPrefix + "api/media/" + id

    var settings = {
        
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "DELETE",
        dataType: "json",
        success: onSuccess,
        error: onError,
    }
    $.ajax(url, settings)
}

sabio.services.media.apiUpdateById = function (id, data, onSuccess, onError)
{
    var url = sabio.services.apiPrefix + "api/media/" + id

    var settings = {

        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "PUT",
        data: data,
        dataType: "json",
        success: onSuccess,
        error: onError,
    }
    $.ajax(url, settings)
}

sabio.services.media.apiGetMediaByUserId = function (onSuccess, onError) {
    var url = sabio.services.apiPrefix + "api/media/UserId"

    var settings = {

        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",    
        dataType: "json",
        success: onSuccess,
        error: onError,
    }
    $.ajax(url, settings)

}
