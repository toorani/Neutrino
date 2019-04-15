console.log("security item.permissionController")

angular.module("neutrinoProject").register.controller('item.permissionController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

    "use strict";
    $scope.viewModel = {
        roleId : 0,
        role: null,
        actions: []
    }

    $scope.initializeController = function () {
        $scope.title = 'تعیین سطوح دسترسی';
        $scope.viewModel.roleId = $routeParams.id;
        getRoleInfo();
        getActionForPermission();
    }

    var getRoleInfo = function () {
        ajaxService.ajaxCall({ roleId: $scope.viewModel.roleId }, "api/accountService/getRoleInfo", 'get',
            function (response) {
                $scope.viewModel.role = response.data;
            },
            function (response) {
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    var getActionForPermission = function () {

        ajaxService.ajaxCall({ roleId: $scope.viewModel.roleId }, "api/appActionService/getActionForPermission", 'get',
            function (response) {
                $scope.viewModel.actions = response.data;
            },
            function (response) {
                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }

    var loadEntity = function () {
        ajaxService.ajaxPost($scope.viewModel.id, "api/appActionService/getDataItem",
          function (response) {
              $scope.viewModel = response.data;
          },
          function (response) {
              alertService.showError(response.data.transactionalData.returnMessage);
              alertService.setValidationErrors($scope, response.data.validationErrors);
          });
    }
   
    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/permissionService/addPermission",
            function (response) {
                alertService.showSuccess(response.data.returnMessage);
            },
            function (response) {
                alertService.showError(response.data.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }

    $scope.cancel = function () {
        $location.url('security/permission/index');
    }
    $scope.delete = function () {
        var modalOptions = {
                actionButtonText: 'حذف سطوح دسترسی',
                headerText: 'تعیین سطوح دسترسی',
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost($scope.viewModel, "api/permissionService/removePermission",
                            function (response) {
                                alertService.showSuccess(response.data.returnMessage);
                                $location.url('security/permission/index');
                            },
                            function (response) {
                                alertService.showError(response.data.returnMessage);
                                alertService.setValidationErrors($scope, response.data.validationErrors);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });
    }


}]);