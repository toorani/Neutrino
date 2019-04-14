console.log("branchReceiptGoalPercent itemController")

angular.module("neutrinoProject").register.controller('branchReceiptGoalPercent.itemController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService', '$uibModal', 'modalService',
function ($scope, $location, $filter, $routeParams, ajaxService, alertService, $uibModal, modalService) {

    "use strict";
    $scope.viewModel = {
        items: [],
        goalGoodsCategoryName: null,
        goalDate: null,
        isGoalUsed: false,
        goalId: 0,
        amount: 0
    }
    var percentItem = {
        reachedPercent: 0,
        notReachedPercent: 0,
        goalId: 0,
        branches: [],
        deselectedBranches: []
    }
    $scope.branches = [];
    $scope.initializeController = function () {
        $scope.title = 'درصد هدف وصول';
        $scope.viewModel.goalId = ($routeParams.id || 0)
        getBranches();
        loadData();
    }
    $scope.submit = function (item) {
        if (item.branches == undefined)
            item.branches = [];
        ajaxService.ajaxPost(item, "api/branchReceiptGoalPercentService/addOrUdate",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                if (item.branches == undefined)
                    item.branches = [];

                if (item.deselectedBranches == undefined)
                    item.deselectedBranches = [];

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
        $location.url('goal/elite/receipt/item/' + $scope.viewModel.goalId);
    }
    $scope.toggleCheck = function (percentItem, branch) {
        if (percentItem.branches.indexOf(branch.id) === -1) {
            percentItem.branches.push(branch.id);
            if (percentItem.deselectedBranches.indexOf(branch.id) !== -1) {
                percentItem.deselectedBranches.splice(percentItem.deselectedBranches.indexOf(branch.id), 1);
            }
        } else {
            percentItem.branches.splice(percentItem.branches.indexOf(branch.id), 1);
            if (percentItem.deselectedBranches.indexOf(branch.id) === -1) {
                percentItem.deselectedBranches.push(branch.id);
            }
        }
    };
    $scope.batchToggleCheck = function (item, action) {
        if (action == 'selectAll') {
            $scope.branches.forEach(function (bra) {

                if (item.branches.indexOf(bra.id) === -1)
                    item.branches.push(bra.id);

                if (item.deselectedBranches.indexOf(bra.id) !== -1)
                    item.deselectedBranches.splice(item.deselectedBranches.indexOf(bra.id), 1);

            });
        }
        else {
            $scope.branches.forEach(function (bra) {

                if (item.deselectedBranches.indexOf(bra.id) === -1)
                    item.deselectedBranches.push(bra.id);

                if (item.branches.indexOf(bra.id) !== -1)
                    item.branches.splice(item.branches.indexOf(bra.id), 1);
            });
        }

    };
    $scope.addBranchPercentItem = function () {
        var maxId = 0;
        $scope.viewModel.items.forEach(function (item) {
            if (item.id > maxId) {
                maxId = item.id;
            }
        })

        var percentItem = {
            reachedPercent: 0,
            notReachedPercent: 0,
            goalId: $scope.viewModel.goalId,
            branches: [],
            deselectedBranches: [],
            id: maxId + 1,
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
        ajaxService.ajaxCall({ goalId: $scope.viewModel.goalId }, "api/branchReceiptGoalPercentService/getGoalPercent", 'get',
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