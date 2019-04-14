console.log("BranchBenefit item.multiController")

angular.module("neutrinoProject").register.controller('item.multiController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

    "use strict";
    $scope.viewModel = {
        id: 0,
        goodsCategories: [],
        branch: null,
        percent: 0.1
    }

    $scope.initializeController = function () {
        $scope.title = 'سهم مراکز/چند دسته دارویی';
        getGoodsCategories();
        getBranches();
        $scope.viewModel.id = ($routeParams.id || 0);

        if ($scope.viewModel.id != 0) {
            loadEntity();
        }

    }
    $scope.percentSlider = {
        options: {
            floor: 0.1,
            ceil: 100,
            step: 0.1,
            precision: 2,
            showSelectionBar: true,
            translate: function (value) {
                if (isNaN(value)) {
                    value = 1;
                }
                return '%' + value;
            },
            getSelectionBarColor: function (value) {
                if (value <= 30)
                    return 'red';
                if (value <= 60)
                    return 'orange';
                if (value <= 90)
                    return 'yellow';
                return '#2AE02A';
            }
        }
    };
    var getGoodsCategories = function () {
        return ajaxService.ajaxPost({ goodsCategoryTypeId: 0, isActive: true }, "api/GoodsCategoryService/getDataList",
            function (response) {
                $scope.goodsCategories = response.data.items;
            },
            function (response) {
                $scope.goodsCategories = [];
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    var getBranches = function () {
        return ajaxService.ajaxPost({}, "api/branchService/getBranches",
            function (response) {
                $scope.branches = response.data.items;
            },
            function (response) {
                $scope.branches = [];
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    var loadEntity = function () {
        ajaxService.ajaxPost($scope.viewModel.id, "api/branchBenefitService/getDataItem",
          function (response) {
              $scope.viewModel = response.data;
          },
          function (response) {
              alertService.showError(response.data.transactionalData.returnMessage);
              alertService.setValidationErrors($scope, response.data.validationErrors);
          });
    }
    $scope.submit = function () {
        suitableViewModel();
        if ($scope.viewModel.id == 0) {

            ajaxService.ajaxPost($scope.viewModel, "api/branchBenefitService/add",
            function (response) {
                alertService.showSuccess(response.data.transactionalData.returnMessage);
                $scope.viewModel.id = response.data.id;
            },
            function (response) {
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }
        else {
            ajaxService.ajaxPost($scope.viewModel, "api/branchBenefitService/edit",
            function (response) {
                alertService.showSuccess(response.data.transactionalData.returnMessage);
            },
            function (response) {
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }


    }
    $scope.cancel = function () {
        $location.url('branchBenefit/index');
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
                        suitableViewModel();
                        ajaxService.ajaxPost($scope.viewModel, "api/branchBenefitService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.transactionalData.returnMessage);
                                $location.url('branchBenefit/index');
                            },
                            function (response) {
                                alertService.showError(response.data.transactionalData.returnMessage);
                                alertService.setValidationErrors($scope, response.data.validationErrors);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });

        }
    }

    var suitableViewModel = function () {
        delete $scope.viewModel.transactionalData;
        delete $scope.viewModel.branch.transactionalData;
        $scope.viewModel.goodsCategories.forEach(function (en) {
            delete en.transactionalData;
        });
    }


}]);