console.log("BranchGoal item.receiptController")

angular.module("neutrinoProject").register.controller('branchGoal.item.receiptController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService',
function ($scope, $location, $filter, $routeParams, ajaxService, alertService) {

    "use strict";
    $scope.viewModel = {
        items: [],
        goal: {},
    }
    $scope.totalAmount = 0;


    $scope.initializeController = function () {
        
        $scope.viewModel.goal.id = ($routeParams.id || 0)
        loadData();
    }

    $scope.calculateTotalAmount = function () {
        $scope.totalAmount = 0;
        $scope.viewModel.items.forEach(function (item) {
            if (item.amount != undefined) {
                $scope.totalAmount += parseInt(item.amount);
            }

        });
    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/branchGoalService/batchUpdate",
            function (response) {
                alertService.showSuccess(response.data.returnMessage);
                $scope.viewModel.items = response.data.resultValue.items;
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    $scope.cancel = function () {
        if ($scope.viewModel.goal.goalGoodsCategoryTypeId >= 4 && $scope.viewModel.goal.goalGoodsCategoryTypeId <= 6)
            $location.url('goal/elite/receipt/item/' + $scope.viewModel.goal.id);
        else
            $location.url('goal/elite/aggregation/item/' + $scope.viewModel.goal.id);
    }

    var loadData = function () {
        $scope.totalAmount = 0;
        $scope.computingValue = 0;
        ajaxService.ajaxCall({ goalId: $scope.viewModel.goal.id }, "api/branchGoalService/getbranchGoalDTO", 'get',
            function (response) {
                $scope.viewModel.goal = response.data.goal;
                if ($scope.viewModel.goal.goalSteps != null && $scope.viewModel.goal.goalSteps.length == 1)
                    $scope.computingValue = $scope.viewModel.goal.goalSteps[0].computingValue;

                $scope.viewModel.items = response.data.items;
                $scope.calculateTotalAmount();
                $scope.title = ' سهم مراکز از ' + $scope.viewModel.goal.goalGoodsCategory.name;
            },
            function (response) {
                alertService.showError(response);
            });

    }

}]);