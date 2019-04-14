console.log("login")

angular.module("neutrinoProject").controller('loginController',
['$scope', '$location', '$window',  '$http', 'alertService'
, function ($scope, $location, $window,  $http, alertService) {

    "use strict";
    $scope.isLoging = false;
    $scope.vm = {}
    $scope.initializeController = function () {
        $scope.title = 'ورود به سیستم';
    }

    $scope.login = function () {
        $scope.isLoging = true;
        if ($location.search().ReturnUrl !== undefined) {
            $scope.vm.returnUrl = $location.search().ReturnUrl;
        }
        

        $http.post("api/accountService/login", $scope.vm).then(
            function (response, status, headers, config) {
                $scope.isLoging = false;
                $window.location.href = response.data.returnUrl;
                
            }, function (response) {
                $scope.isLoging = false;
                alertService.showError(response);
            });
    }
}
]);