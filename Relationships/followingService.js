
(function () {
    "use strict",


    angular.module(APPNAME)
    .factory('$followingService', followingServiceFactory);


    followingServiceFactory.$inject = ['$baseService', '$sabio'];


    function followingServiceFactory($baseService, $sabio) {
        
        var aSabioServiceObject = sabio.followersNg.services.followers; 

        var newService = $baseService.merge(true, {}, aSabioServiceObject, $baseService)

        return newService;

    }


})();