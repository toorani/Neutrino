console.log("Goal/eilte/totalSales/indexController")

angular.module("neutrinoProject").register.controller('totalSales.indexController',
['$scope', '$location', '$compile', '$filter', 'ajaxService', 'dataTableColumns',
    function ($scope, $location, $compile, $filter, ajaxService, dataTableColumns) {

        "use strict";

        $scope.initializeController = function () {
            $scope.title = 'هدف گذاری کل';
            dataTableColumns.initialize();

            //dataTableColumns.add({  title: '#' });
            
            dataTableColumns.add({ mappingData: 'year', title: 'سال' });
            dataTableColumns.add({ mappingData: 'month', title: 'ماه' });
            dataTableColumns.add({
                mappingData: 'amount', title: 'مبلغ'
                , className: 'dir-ltr'
                , fnRenderCallBack: function (data, type, goal) {
                    return $filter('number')(data, 0);
                }

            });
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
                goalGoodsCatType: '3' //total sales goal
            }
        }, {
            title: 'اهداف محاسبه شده', filter: {
                goalType: 'distributor',
                isUsed: 'true',
                goalGoodsCatType : '3' //total sales goal
            }
        }]
        $scope.activeTab = 0;
        $scope.tabSelected = function (idx) {
            $scope.activeTab = idx;
        }
        $scope.newGoal = function () {
            $location.url('goal/elite/totalSales/item')
        }
        $scope.viewEntity = function (goalId) {
            $location.url('goal/elite/totalSales/item/' + goalId);
        }
    }]);
