console.log("fulfillmentPercent itemController")

angular.module("neutrinoProject").register.controller('fulfillmentPercent.itemController',
['$scope', '$filter', 'ajaxService', '$location', 'alertService', '$routeParams', 'persianCalendar',
function ($scope, $filter, ajaxService, $location, alertService, $routeParams, persianCalendar) {

    "use strict";
    $scope.viewModel = {
        items: []
    }
    $scope.years = persianCalendar.getYears();
    $scope.months = persianCalendar.getMonthNames();
    $scope.year = null;
    $scope.month = null;
    $scope.isUsed = false;

    $scope.initializeController = function () {
        $scope.title = 'ضریب تحقق پورسانت ';
    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel.items, "api/fulfillmentPercentService/addFulfillment",
            function (response) {
                alertService.showSuccess(response.data.returnMessage);
                $scope.viewModel.items = response.data.items;
                //loadData(goalId);
            },
            function (response) {

                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }

    $scope.loadData = function () {
        ajaxService.ajaxCall({ year: $scope.year, month: $scope.month }, "api/fulfillmentPercentService/getFulfillmentList", "get",
            function (response) {
                $scope.viewModel.items = response.data;
                $scope.isUsed = $scope.viewModel.items[0].isUsed;
            },
            function (response) {
                alertService.showError(response);
            });
    }
    
}]);