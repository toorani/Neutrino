console.log("notrionoProjectController")

angular.module('neutrinoProject').controller("layoutController",
    ['$location', '$scope', '$window', '$http', 'blockUI', 'ajaxService', 'permissions',
function ($location, $scope, $window, $http, blockUI, ajaxService, permissions) {

    "use strict";
    var nextUrl = '';
    var userPermissionsChanged = false;
    $scope.initializeController = function () {
        ajaxService.ajaxPost({}, 'api/permissionService/getUserPermission',
            function (response) {
                permissions.setPermissions(response.data);
                if (permissions.checkRoutePermission(nextUrl) == false) {
                    $location.path('account/forbidden');
                }
            },
            function (response) {
            }
            //, 'در حال بارگذاری سطوح دسترسی ...'
            );
    }

    $scope.$on('$routeChangeStart', function (scope, next, current) {
        var originalPath = next.$$route.originalPath;
        nextUrl = '';
        switch (originalPath) {
            case '/:rootSection1/:rootSection2/:section/:tree':
                nextUrl = next.params.rootSection1 + '/' + next.params.rootSection2 + '/' + next.params.section + '/' + next.params.tree;
                break;
            case '/:rootSection/:section/item/:id':
                nextUrl = next.params.rootSection + '/' + next.params.section + '/item';
                break;
            case '/:section/item/:id':
                nextUrl = next.params.section + '/item';
                break;
            case '/:rootSection/:section/:tree':
                nextUrl = next.params.rootSection + '/' + next.params.section + '/' + next.params.tree;
                break;
            case '/:section/:tree':
                nextUrl = next.params.section + '/' + next.params.tree;
                break;
            case '/':
                nextUrl = '/home/index';
                break;
        }
        if (permissions.isLoadPermission() && permissions.checkRoutePermission(nextUrl) == false) {
            $location.path('account/forbidden');
        }
    });

    $scope.$on('$permissionsChanged', function () {

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
        })
            .then(
               function (response, status, headers, config) {
                   $window.location.href = response.data.returnUrl;
               }, function (response) {
                   
               });

    }
}]);
