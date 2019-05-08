console.log("account/registration/index")

angular.module("neutrinoProject").register.controller('index.registrationController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
function ($scope, $location, $compile, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = ' مدیریت کاربران';
        dataTableColumns.initialize();

        dataTableColumns.add({
            mappingData: 'firstName'
            , fnRenderCallBack: function (data, type, full) {
                return full.name + ' ' + full.lastName;

            }, title: 'نام و نام خانوادگی'
        });
        dataTableColumns.add({ mappingData: 'userName', title: 'نام کاربری' });
        dataTableColumns.add({ mappingData: 'email', title: 'ایمیل' });
        dataTableColumns.add({ mappingData: 'mobileNumber', title: 'موبایل' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<div class='btn-group'>"
                      + "<button type='button' ng-click='editUser(" + JSON.stringify(full) + ")'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' aria-expanded='false'> <i class='fa fa-magic'></i><span>جزئیات</span></button>"
                      //+ "<ul class='dropdown-menu' role='menu'>"
                      //+ "<li ng-click='editGoal(" + JSON.stringify(full) + ")'><a  class='btn btn-xs'  > <i class='fa fa-pencil'></i><span>ویرایش</span></a></li>"
                      ////+ "<li><a  class='btn btn-xs'  ><i class='fa fa-archive'></i><span>غیرفعال</span></a></li>"
                      //+ "<li class='divider'></li>"
                      //+ "<li><a  class='btn btn-xs'  ><i class='fa fa-info'></i><span>جزئیات</span></a></li>"
                      //+ "</ul></div>"
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



}]);
