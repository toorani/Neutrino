console.log("account/resetpass/item")

angular.module("neutrinoProject").register.controller('account.resetpass.itemController',
    ['$scope', '$location', '$routeParams', 'ajaxService', 'alertService',
        function ($scope, $location, $routeParams, ajaxService, alertService) {

            "use strict";
            $scope.user = {
                id: 0,
                userName: null,
                password: null,
                confirmPassword: null,
            }

            $scope.initializeController = function () {
                $scope.title = 'تغییر رمز عبور';
                $scope.user.id = ($routeParams.id || 0);
                if ($scope.user.id != 0) {
                    loadEntity();
                }

            }
            $scope.submit = function () {
                ajaxService.ajaxPost($scope.user, "api/accountService/resetPassword",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                        $location.path("account/registration/index/");
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.cancel = function () {
                $location.url('account/registration/index');
            }
            $scope.changeIcon = function (event) {
                $scope.showPassword = !$scope.showPassword;
                $(event.currentTarget).find('i').remove();
                if ($scope.showPassword) {
                    $(event.currentTarget).html('<i class="fa fa-eye"></i>');
                } else {
                    $(event.currentTarget).html('<i class="fa fa-eye-slash"></i>');  
                }
            }

            var loadEntity = function () {
                ajaxService.ajaxCall({ id: $scope.user.id }, "api/accountService/getDataItem", 'get',
                    function (response) {
                        $scope.user = response.data;
                    },
                    function (response) {
                        alertService.showError(response);

                    });
            }
        }]);