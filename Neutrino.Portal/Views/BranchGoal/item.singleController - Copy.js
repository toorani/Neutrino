console.log("BranchBenefit item.singleController")

angular.module("neutrinoProject").register.controller('item.singleController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

    "use strict";
    $scope.viewModel = {
        goalGoodsCategory: null,
        items: []
    }
    $scope.isEditMode = false;
    $scope.goalSteps = [];
    $scope.totalPercent = 0;

    $scope.initializeController = function () {
        $scope.title = 'سهم مراکز از هدف';
        getGoalGoodsCategories();
    }
    $scope.onChangeGoalGoodsCategory = function () {
        $scope.totalPercent = 0;
        $scope.goalSteps = [];
        if ($scope.viewModel.goalGoodsCategory != null) {
            ajaxService.ajaxCall({ goodsCategoryId: $scope.viewModel.goalGoodsCategory.id}, "api/branchBenefitService/getBatchData",'get',
            function (response) {
                $scope.goalSteps = response.data.goalSteps;
                $scope.viewModel.items = response.data.items;
                $scope.calculateTotalPercent();
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }

    }
    $scope.calculateTotalPercent = function () {
        $scope.totalPercent = 0;
        $scope.viewModel.items.forEach(function (item) {
            if (item.percent != undefined) {
                $scope.totalPercent += item.percent;
            }
            
        });
        $scope.totalPercent = $scope.totalPercent.toFixed(3)
           
    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/branchBenefitService/batchUpdate",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                $scope.onChangeGoalGoodsCategory();
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    $scope.cancel = function () {
        $location.url('branchBenefit/single/index');
    }
    $scope.delete = function () {
        if ($scope.viewModel.id != 0) {

            var modalOptions = {
                actionButtonText: 'حذف سهم مرکز',
                headerText: 'سهم مراکز/دسته دارویی',
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost($scope.viewModel, "api/branchBenefitService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $location.url('branchBenefit/single/index');
                            },
                            function (response) {
                                alertService.showError(response);
                                alertService.setValidationErrors($scope, response.data.validationErrors);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });

        }
    }

    var getGoalGoodsCategories = function () {
        ajaxService.ajaxCall({ goodsCategoryTypeId: 0, isActive: true }, "api/goalGoodsCategoryService/getDataList",'get',
            function (response) {
                $scope.goalGoodsCategories = response.data;

                if ($routeParams.id != undefined && $routeParams.id != 0) {
                    $scope.viewModel.goalGoodsCategory = $filter('getById')($scope.goalGoodsCategories, $routeParams.id);
                    $scope.onChangeGoalGoodsCategory();
                    $scope.isEditMode = true;
                }
            },
            function (response) {
                $scope.goodsCategories = [];
                alertService.showError(response);
            });
    }

}]);