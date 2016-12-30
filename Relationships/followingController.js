(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('followingController', FollowingController)
    .filter('capitalize', function () {
        return function (input) {
            return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : '';
        }
    });


    FollowingController.$inject = ['$scope', '$baseController', "$followingService", "$filter"];

    function FollowingController(
         $scope
        , $baseController
        , $followingService
        , $filter) {

        var vm = this;
        $baseController.merge(vm, $baseController);

        vm.items = null;

        vm.currentbuttontoshow = null;
       
        vm.pageChanged = _pageChanged;
        vm.currentPage = 1;
        vm.itemsPerPage = 4;
        vm.maxSize = 10;
        vm.totalItemCount = null;


        vm.$followingService = $followingService;
        vm.$scope = $scope;


        vm.receiveItems = _receiveItems;
        vm.onFolError = _onFolError;
        vm.unFollow = _unFollow;
        vm.addFollower = _addFollower;

        vm.header = vm.$routeParams.type;

       


        

        vm.notify = vm.$followingService.getNotifier($scope);



        vm.tabs = [
        { link: '#friends/friends', label: 'Friends' },
        { link: '#friends/following', label: 'Following' },
        { link: '#friends/followers', label: 'Followers' }
        ];

        vm.selectedTab = vm.tabs[0];

        vm.tabClass = _tabClass;
        vm.setSelectedTab = _setSelectedTab;

        render();




        function render() {

            vm.paginatedModel =
              {
                  "UserId": vm.followerUserId,
                  "CurrentPage": vm.currentPage,
                  "ItemsPerPage": vm.itemsPerPage
              };

            if (vm.$routeParams.type == "friends") {

                vm.$followingService.getFriends(vm.paginatedModel, vm.receiveItems, vm.onFolError);
                vm.selectedTab = vm.tabs[0];

            }

            else if (vm.$routeParams.type == "following") {


                vm.$followingService.apiSelectById(vm.paginatedModel, vm.receiveItems, vm.onFolError)
                vm.selectedTab = vm.tabs[1];
            }

            else if (vm.$routeParams.type == "followers") {


                vm.$followingService.getFollowers(vm.paginatedModel, vm.receiveItems, vm.onFolError)
                vm.selectedTab = vm.tabs[2];
            };


        }



        function _receiveItems(data) {
            console.log("data", data);
            vm.notify(function () {

             
                vm.totalItemCount = data.totalItemCount;

                vm.items = data.items;
                vm.followerUserid = data.items.follwerUserId;
                vm.userName = data.items.userName;
                vm.currentbuttontoshow = vm.$routeParams.type;

            });



        }




        function _onFolError(jqXhr, Error) {
            console.error(error);
        }


        function _unFollow(fol) {
            console.log("in unfollow")


            if (confirm("Are you sure you want to delete " + fol.userName + "?")) {

                vm.$followingService.unfollow(fol.followingUserId, render, vm.onFolError);
                vm.$alertService.error(fol.userName + " has been unfollowed!", "Delete!");

            }

            else {

                vm.$alertService.warning(fol.userName + " was not unfollowed!", "Warning!");


            };



        }

        function _addFollower(fol) {
            console.log("in addfollower")

            var followingUsers =
        {
            "FollowingUserId": fol.followingUserId,
            "FollowerUserId": "",
        };
            vm.$followingService.apiAddFollower(followingUsers, render, vm.onFolError);
            vm.$alertService.success("You are now following "+ "" + fol.userName, "Success!");
        }



        function _tabClass(tab) {

            if (vm.selectedTab == tab) {
                console.log("selectedTab", vm.selectedTab)
                console.log("tab", vm.tabs)
                return "active";
            } else {
                return "";
            }
        }

        function _setSelectedTab(tab) {
            console.log("set selected tab", tab);
            vm.selectedTab = tab;

        }


     


        function _pageChanged() {

            render();

        };




    }
})();
