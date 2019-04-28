console.log("BranchGoal itemController")

angular.module("neutrinoProject").register.controller('branchGoal.itemController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService',
function ($scope, $location, $filter, $routeParams, ajaxService, alertService) {

    "use strict";
    $scope.viewModel = {
        items: [],
        goal: {},
    }
    $scope.goalSteps = [];
    $scope.totalPercent = 0;
    $scope.dataEntryMethod = 1;

    $scope.initializeController = function () {
        $scope.title = 'سهم مراکز از هدف';
        $scope.viewModel.goal.id = ($routeParams.id || 0)
        loadData();
    }

    $scope.calculateTotalPercent = function () {
        $scope.totalPercent = 0;
        $scope.viewModel.items.forEach(function (item) {
            if ($scope.dataEntryMethod == 1) // percent
            {
                if (item.percent != undefined) {
                    var firstStep = $scope.goalSteps[0];
                    item.amount = firstStep.computingValue * item.percent / 100;
                    $scope.totalPercent += item.percent;
                }
            }
            else { // amount
                if ((item.amount != undefined && String(item.amount).length >= 3) || $scope.reLoadData) {
                    var firstStep = $scope.goalSteps[0];
                    let percent = item.amount * 100 / firstStep.computingValue;
                    item.percent = parseFloat(percent.toFixed(5));
                    $scope.totalPercent += item.percent;
                }
                else {
                    item.percent = 0;
                    $scope.totalPercent += item.percent;
                }
            }

        });
        $scope.totalPercent = $scope.totalPercent.toFixed(5)
    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/branchGoalService/batchUpdate",
            function (response) {
                alertService.showSuccess(response.data.returnMessage);
                $scope.viewModel.items = response.data.resultValue.items;
                $scope.reLoadData = true;
                $scope.calculateTotalPercent();
                $scope.reLoadData = false;
            },
            function (response) {
                alertService.showError(response);
            });
    }
    $scope.cancel = function () {
        switch ($scope.viewModel.goal.goalGoodsCategoryTypeId) {
            case 1:
            case 2:
                $location.url('goal/elite/groupSingle/item/' + $scope.viewModel.goal.id);
                break;
            case 3: // total sales goal
                $location.url('goal/elite/totalSales/item/' + $scope.viewModel.goal.id);
                break;
            case 4: // total receipt 
            case 5: // private receipt goal
            case 6: // govern receipt goal
                $location.url('goal/elite/receipt/item/' + $scope.viewModel.goal.id);
                break;
        }

    }

    var loadData = function () {
        $scope.totalPercent = 0;
        $scope.goalSteps = [];
        ajaxService.ajaxCall({ goalId: $scope.viewModel.goal.id }, "api/branchGoalService/getbranchGoalDTO", 'get',
            function (response) {
                $scope.viewModel.goal = response.data.goal;
                $scope.goalSteps = $scope.viewModel.goal.goalSteps;

                $scope.viewModel.items = response.data.items;
                //$scope.viewModel.goalGoodsCategoryName = response.data.goalGoodsCategoryName;
                $scope.calculateTotalPercent();
            },
            function (response) {
                alertService.showError(response);
            });

    }

}]);