console.log("account/changeassword/item")

angular.module("neutrinoProject").register.controller('account.changeassword.itemController',
    ['$scope','ajaxService', 'alertService',
        function ($scope, ajaxService, alertService) {

            "use strict";
            $scope.user = {
                id: 0,
                newPassword: null,
                confirmPassword: null,
                currentPasswrod: null
            }

            $scope.show_newPassword = false;
            $scope.show_confirmPassword = false;
            $scope.show_currentPassword = false;
            

            $scope.initializeController = function () {
                $scope.title = 'تغییر رمز عبور';
            }
            $scope.submit = function () {
                ajaxService.ajaxPost($scope.user, "api/accountService/changePassword",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.changeIcon = function (event) {
                let showPassword = $(event.currentTarget.nextElementSibling).attr('show-password');
                switch (showPassword) {

                    case 'show_currentPassword':
                        $scope.show_currentPassword = !$scope.show_currentPassword;
                        break;
                    case 'show_newPassword':
                        $scope.show_newPassword = !$scope.show_newPassword;
                        break;
                    case 'show_confirmPassword':
                        $scope.show_confirmPassword = !$scope.show_confirmPassword;
                        break;
                }

                $(event.currentTarget).find('i').remove();
                if ($scope.showPassword) {
                    $(event.currentTarget).html('<i class="fa fa-eye"></i>');
                } else {
                    $(event.currentTarget).html('<i class="fa fa-eye-slash"></i>');
                }
            }
        }]);