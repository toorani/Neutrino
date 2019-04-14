console.log("orgStructureShare itemController")

angular.module("neutrinoProject").register.controller('orgStructureShare.itemController',
['$scope', '$filter', 'ajaxService', 'alertService',
function ($scope, $filter, ajaxService, alertService) {

    "use strict";
    $scope.viewModel = {
        branch: null,
        items: []
    }
    $scope.isEditMode = false;
    $scope.initializeController = function () {
        $scope.title = 'سهم سمت های سازمانی';
        getBranches();
    }


    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/orgStructureShareService/add",
            function (response) {
                response.data.items.forEach(function (orgStruct) {
                    if (orgStruct.id != 0) {
                        let local_orgStruct = $filter('filter')($scope.viewModel.items, { orgStructureId: orgStruct.orgStructureId }, false)[0];
                        local_orgStruct.id = orgStruct.id;
                    }
                });

                alertService.showSuccess(response.data.actionResult.returnMessage);

            },
            function (response) {
                alertService.showError(response);
            });
    }

    $scope.cancel = function () {
        $location.url('orgStructureShare/index');
    }

    $scope.onchange_branch = function () {
        if ($scope.viewModel.branch != null) {
            $scope.viewModel.items = [];
            ajaxService.ajaxCall({ id: $scope.viewModel.branch.id }, "api/orgStructureShareService/getDataItem", 'get',
            function (response) {
                $scope.viewModel.items = response.data.items;
                if ($scope.viewModel.items.length == 0) {
                    alertService.showWarning('پست سازمانی برای مرکز انتخاب شده تعریف نشده است');
                }
            },
            function (response) {
                alertService.showError(response);
            });
        }

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