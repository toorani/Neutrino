console.log("notrionoProjectController")

angular.module('neutrinoProject').controller("layoutController",
    ['$location', '$scope', '$window', '$http', 'blockUI', 'ajaxService', 'permissions',
        function ($location, $scope, $window, $http, blockUI, ajaxService, permissions) {

            "use strict";
            var nextUrl = '';
            $scope.initializeController = function () {
            }

            $scope.$on('$routeChangeStart', function (scope, next, current) {

            });

            $scope.$on("$routeChangeSuccess", function (scope, next, current) {
                $scope.transitionState = "active"
            });
            $scope.logOff = function () {
                //blockUI.start('لطفا صبر کنید ...');
                $http({
                    data: {},
                    method: 'PUT',
                    url: "api/accountService/logOff"
                }).then(
                    function (response, status, headers, config) {
                        $window.location.href = response.data.returnUrl;
                    }, function (response) {

                    });

            }
        }]);
