console.log("Goal/supplier/index")

angular.module("neutrinoProject").register.controller('index.supplierController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
function ($scope, $location, $compile, ajaxService, dataTableColumns) {

    "use strict";
    $scope.initializeController = function () {
        $scope.title = '  هدف گذاری تامین کننده';
        
        dataTableColumns.initialize();
        dataTableColumns.add({ mappingData: 'company.faName', title: 'نام تامین کننده' });
        dataTableColumns.add({ mappingData: 'startDate', title: 'تاریخ شروع' });
        dataTableColumns.add({ mappingData: 'endDate', title: 'تاریخ پایان' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  ng-click='viewEntity("
                    + JSON.stringify(full.id)
                    + ")' class='btn ls-light-blue-btn '><i class='fa fa-magic'></i><span>جزئیات</span></button>")($scope);

            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/GoalService/getDataGrid';

    }

    $scope.goalTabs = [{
        title: 'اهداف فعال', filter: {
            goalType: 'supplier',
            isActive: 'true'
        }
    }, {
        title: 'اهداف غیر فعال', filter: {
            goalType: 'supplier',
            isActive: 'false'
        }
    }]
    $scope.activeTab = 0;
    $scope.tabSelected = function (idx) {
        $scope.activeTab = idx;
    }

    $scope.newGoal = function () {
        $location.url('goal/supplier/item')
    }

    $scope.viewEntity = function (id) {
        $location.url('goal/supplier/item/' + id);
    }

}]);
