console.log("goalNonFulfillment itemController")

angular.module("neutrinoProject").register.controller('goalNonFulfillment.itemController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService', 'modalService',
function ($scope, $location, $filter, $routeParams, ajaxService, alertService, modalService) {

    "use strict";
    $scope.viewModel = {
        items: [],
        goalGoodsCategoryName: null,
        goalGoodsCategoryId : null,
        startGoalDate: null,
        endGoalDate: null,
        isGoalUsed: false,
        goalId: 0,
    }
    
    $scope.branches = [];
    $scope.initializeController = function () {
        $scope.title = 'درصد پاداش مراکز در صورت عدم تحقق هدف';
        $scope.viewModel.goalId = ($routeParams.id || 0)
        getBranches();
        loadData();
    }

    $scope.submit = function (item) {
        if (item.branches == undefined)
            item.branches = [];
        ajaxService.ajaxPost(item, "api/goalNonFulfillmentService/addOrUdate",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                if (item.branches == undefined)
                    item.branches = [];

                if (item.isNewAdded != null)
                    item.isNewAdded = null;
            },
            function (response) {
                alertService.showError(response);

                if (item.branches == undefined)
                    item.branches = [];

                if (item.deselectedBranches == undefined)
                    item.deselectedBranches = [];
                
            });
    }
    $scope.cancel = function () {
        $location.url('goal/elite/groupSingle/item/' + $scope.viewModel.goalId);
    }

    $scope.addBranchPercentItem = function () {
        var maxId = 0;
        $scope.viewModel.items.forEach(function (item) {
            if (item.id > maxId) {
                maxId = item.id;
            }
        })

        var percentItem = {
            percent: null,
            goalId: $scope.viewModel.goalId,
            branches: [],
            isNewAdded: true
        }
        $scope.viewModel.items.push(percentItem);
    }
    $scope.removeBranchPercentItem = function (item) {
        if (item.isNewAdded != null)
            $scope.viewModel.items.splice($scope.viewModel.items.indexOf(item), 1);
        else {

            var modalOptions = {
                actionButtonText: 'حذف درصد مراکز',
                headerText: 'درصد هدف وصول',
                bodyText: 'مطمئن هستید ؟'
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {

                        $scope.branches.forEach(function (bra) {

                            if (item.branches.indexOf(bra.id) === -1)
                                item.branches.push(bra.id);

                            if (item.deselectedBranches.indexOf(bra.id) !== -1)
                                item.deselectedBranches.splice(item.deselectedBranches.indexOf(bra.id), 1);

                        });

                        ajaxService.ajaxPost(item, "api/branchReceiptGoalPercentService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $scope.viewModel.items.splice($scope.viewModel.items.indexOf(item), 1);
                                if ($scope.viewModel.items.length == 0)
                                    $scope.addBranchPercentItem();
                            },
                            function (response) {
                                alertService.showError(response);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });
        }
    }

    var loadData = function () {
        ajaxService.ajaxCall({ goalId: $scope.viewModel.goalId }, "api/goalNonFulfillmentService/getDataInfo", 'get',
            function (response) {
                $scope.viewModel = response.data;
                if ($scope.viewModel.items.length == 0)
                    $scope.addBranchPercentItem();
            },
            function (response) {
                alertService.showError(response);
            });

    }
    var getBranches = function () {
        return ajaxService.ajaxCall({}, "api/branchService/getBranches", 'get',
            function (response) {
                $scope.branches = response.data;
            },
            function (response) {
                $scope.branches = [];
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }

}]);