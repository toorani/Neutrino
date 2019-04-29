console.log("Goal/elite/groupSingle/index")

angular.module("neutrinoProject").register.controller('groupSingle.indexController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns'
, function ($scope, $location, $compile, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'هدف گذاری گروهی/ تکی';
        dataTableColumns.initialize();

        dataTableColumns.add({ mappingData: 'goalGoodsCategory.name', title: 'نام دسته دارویی' });
        dataTableColumns.add({ mappingData: 'goalGoodsCategoryType.description', title: 'نوع دسته دارویی' });
        dataTableColumns.add({ mappingData: 'computingTypeTitle', title: 'نحوه محاسبه', sortable: false });
        dataTableColumns.add({ mappingData: 'startDate', title: 'تاریخ شروع' });
        dataTableColumns.add({ mappingData: 'endDate', title: 'تاریخ پایان' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, goal) {

                return $compile("<button type='button'  ng-click='viewEntity("
                    + JSON.stringify(goal.id)
                    + ")' class='btn ls-light-blue-btn '><i class='fa fa-magic'></i><span>جزییات</span></button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/GoalService/getDataGrid';

    }
    $scope.goalTabs = [{
        title: 'اهداف محاسبه نشده', filter: {
            goalType: 'distributor',
            isUsed: 'false',
            goalGoodsCatType: '1,2' //group & single goal
        }
    }, {
        title: 'اهداف محاسبه شده', filter: {
            goalType: 'distributor',
            isUsed: 'true',
            goalGoodsCatType: '1,2' //group & single goal
        }
    }]
    $scope.activeTab = 0;
    $scope.tabSelected = function (idx) {
        $scope.activeTab = idx;
    }
    $scope.newGoal = function () {
        $location.url('goal/elite/groupSingle/item')
    }
    $scope.viewEntity = function (goalId) {
        $location.url('goal/elite/groupSingle/item/' + goalId);
    }
}]);
