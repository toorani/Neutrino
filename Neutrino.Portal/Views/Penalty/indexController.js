console.log("penalty indexController")

angular.module("neutrinoProject").register.controller('penalty.indexController',
['$scope', '$location', '$compile', '$interval', 'alertService', 'ajaxService', '$uibModal', 'persianCalendar',
function ($scope, $location, $compile, $interval, alertService, ajaxService, $uibModal, persianCalendar) {

    "use strict";
    $scope.viewModel = {
        branch: null,
        year: null,
        month: null,
        penalties: []
    }

    $scope.years = persianCalendar.getYears();
    $scope.months = persianCalendar.getMonthNames();

    $scope.initializeController = function () {
        $scope.title = 'مدیریت پرداخت و کسورات';
        getBranches();
    }
    $scope.getData = function () {
        if ($scope.viewModel.year != null && $scope.viewModel.month != null
        && $scope.viewModel.branch != null) {
            ajaxService.ajaxCall({ year: $scope.viewModel.year, month: $scope.viewModel.month, branchId: $scope.viewModel.branch.id }
                , "api/penaltyService/getData", 'get',
            function (response) {
                $scope.viewModel.penalties = response.data;
                //$scope.positionTypes = 
            },
            function (response) {
                alertService.showError(response);
            });
        }
    }

    $scope.submit = function () {
        //ajaxService.ajaxPost($scope.viewModel, '/api/promotionService/add',
        //    function (response) {
        //        alertService.showSuccess(response.data.actionResult.returnMessage);
        //        $scope.modalInstance.close();
        //    },
        //   function (response) {
        //       alertService.showError(response);
        //   });
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
