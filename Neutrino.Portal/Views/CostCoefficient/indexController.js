console.log("CostCoefficient index Controller")

angular.module("neutrinoProject").register.controller('costCoefficient.indexController',
['$scope', 'ajaxService','alertService',
function ($scope, ajaxService, alertService) {

    "use strict";
    $scope.viewModel = {
        id: 0,
        records: []
    }

    $scope.initializeController = function () {
        $scope.title = 'ضریب هزینه محصول';
        loadData();

    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/costCoefficientService/add",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                loadData();
            },
            function (response) {
                alertService.showError(response.data.actionResult.returnMessage);
                alertService.setValidationErrors($scope, response.data.actionResult.validationErrors);
            });
    }

    var loadData = function () {
        ajaxService.ajaxCall({}, "api/costCoefficientService/getCostCoefficientList",'get',
            function (response) {
                $scope.viewModel.records = response.data;
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
}]);
