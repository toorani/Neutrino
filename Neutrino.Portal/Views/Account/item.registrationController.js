console.log("account/registration/item")

angular.module("neutrinoProject").register.controller('item.registrationController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService',
function ($scope, $location, $filter, $routeParams, ajaxService, alertService) {

    "use strict";
    $scope.user = {
        id: 0,
        userName: null,
        password: null,
        confirmPassword: null,
        name: null,
        lastName: null,
        email: null,
        roles: []
    }
    $scope.roles = [];

    $scope.initializeController = function () {
        $scope.title = 'تعریف کاربر ';
        $scope.user.id = ($routeParams.id || 0);
        if ($scope.user.id != 0) {
            loadEntity();
        }
        getRoles();
    }
    $scope.submit = function () {
        if ($scope.user.id == 0) {
            ajaxService.ajaxPost($scope.user, "api/accountService/register",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                $scope.user.id = response.data.id;
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }
        else {
            ajaxService.ajaxPost($scope.user, "api/accountService/edit",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }
    }
    $scope.delete = function () {
        if ($scope.user.id != 0) {

            var modalOptions = {
                actionButtonText: 'حذف کاربری',
                headerText: 'حذف کاربر',
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost($scope.user, "api/accountService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $location.url('orgStructure/index');
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
    $scope.cancel = function () {
        $location.url('account/registration/index');
    }
    var getRoles = function () {
        ajaxService.ajaxCall(new Object(), "api/accountService/getRoles",'get',
            function (response) {
                $scope.roles = response.data.items;
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
    }
    var loadEntity = function () {
        ajaxService.ajaxCall({ id: $scope.user.id }, "api/accountService/getDataItem", 'get',
          function (response) {
              $scope.user = response.data;
          },
          function (response) {
              alertService.showError(response);
              alertService.setValidationErrors($scope, response.data.validationErrors);
          });
    }
}]);