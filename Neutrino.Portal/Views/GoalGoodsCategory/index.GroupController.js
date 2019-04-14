console.log("GoalGoodsCategory/Distributor/index")

angular.module("neutrinoProject").register.controller('index.distributorController',
['$scope', '$location', 'ajaxService', 'dataTableColumns', function ($scope, $location, ajaxService, dataTableColumns) {

    "use strict";
    
    $scope.initializeController = function () {
        $scope.title = 'مدیریت هدف';
        dataTableColumns.initialize();
       
        dataTableColumns.add({ mappingData: 'نام دسته دارویی' });
        dataTableColumns.add({ mappingData: 'نوع دسته دارویی' });
        dataTableColumns.add({ mappingData: 'تاریخ شروع' });
        dataTableColumns.add({ mappingData: 'تاریخ پایان' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return "<button type='button' id='btnEdit' data-whatever=" + data + " class='btn ls-red-btn' data-toggle='modal' onclick='alert(" + data + ");'   ><i class='glyphicon glyphicon-edit'></i> </button>";
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/GoalService/getDataGrid';

    }

    $scope.newGoal = function () {
        $location.url('goal/distributor/item')
    }

   

}]);