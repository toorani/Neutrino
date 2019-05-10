console.log("account/registration/index")

angular.module("neutrinoProject").register.controller('index.registrationController',
    ['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
        function ($scope, $location, $compile, ajaxService, dataTableColumns) {

            "use strict";

            $scope.initializeController = function () {
                $scope.title = ' مدیریت کاربران';
                dataTableColumns.initialize();

                dataTableColumns.add({ mappingData: 'fullName', title: 'نام و نام خانوادگی' });
                dataTableColumns.add({ mappingData: 'userName', title: 'نام کاربری' });
                dataTableColumns.add({ mappingData: 'email', title: 'ایمیل' });
                dataTableColumns.add({ mappingData: 'roleName', title: 'پست کاربر' });
                dataTableColumns.add({
                    isKey: true, fnRenderCallBack: function (data, type, full) {
                        return $compile("<div class='btn-group'>"
                            + "<button type='button' class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown'>"
                            + "<i class='glyphicon glyphicon-user'></i> عملیات <span class='caret'></span></button>"
                            + "<ul class='dropdown-menu' role='menu'>"
                            + "<li> <a role='button' ng-click='editUser(" + JSON.stringify(full) + ")'>جزییات</a></li>"
                            + "<li> <a role='button' ng-click='changePassword(" + JSON.stringify(full) + ")'>تغییر رمز عبور</a></li>"
                            + "</ul></div>"
                        )($scope);

                    }
                });

                
                $scope.dataColumns = dataTableColumns.getItems();
                $scope.serverUrl = '/api/accountService/getUsers';

            }

            $scope.newEntity = function () {
                $location.url('account/registration/item')
            }

            $scope.editUser = function (dataSelected) {
                $location.url('account/registration/item/' + dataSelected.id);
            }
            $scope.changePassword = function (dataSelected) {
                $location.url('account/changepassword/item/' + dataSelected.id);
            }
        }]);
