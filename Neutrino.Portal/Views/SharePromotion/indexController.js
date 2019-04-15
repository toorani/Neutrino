console.log("sharePromotion indexController")

angular.module("neutrinoProject").register.controller('sharePromotion.indexController',
['$scope', '$location', '$compile', '$interval', 'alertService', 'ajaxService', '$uibModal', 'persianCalendar',
function ($scope, $location, $compile, $interval, alertService, ajaxService, $uibModal, persianCalendar) {

    "use strict";
    $scope.viewModel = {
        branchId: null,
        year: null,
        month: null
    }

    $scope.years = persianCalendar.getYears();
    $scope.months = persianCalendar.getMonthNames();

    $scope.initializeController = function () {
        $scope.title = 'مدیریت محاسبه پورسانت';
        getBranches();
    }
    $scope.getData = function () {

    }
    
    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, '/api/promotionService/add',
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                $scope.modalInstance.close();
            },
           function (response) {
               alertService.showError(response);
           });
    }
    var getBranches = function () {
        $scope.branches = [];
        return ajaxService.ajaxCall({}, "api/branchService/getBranches", 'get',
            function (response) {
                $scope.branches = response.data;
            },
            function (response) {
                $scope.branches = [];
                alertService.showError(response);
            });
    }
}]);
