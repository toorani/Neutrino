console.log("Goal/eilte/aggregation/indexController")

angular.module("neutrinoProject").register.controller('aggregation.indexController',
['$scope', '$location', '$compile', '$filter', '$routeParams', 'ajaxService', 'dataTableColumns',
    function ($scope, $location, $compile, $filter, $routeParams, ajaxService, dataTableColumns) {

        "use strict";

        var goalGoodsCatType = ($routeParams.id || 'total');

        $scope.initializeController = function () {
            
            dataTableColumns.initialize();

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
                goalGoodsCatType: '7' // هدف تجمیعی
            }
        }, {
            title: 'اهداف محاسبه شده', filter: {
                goalType: 'distributor',
                isUsed: 'true',
                goalGoodsCatType: '7'
            }
        }]
        $scope.activeTab = 0;
        $scope.tabSelected = function (idx) {
            $scope.activeTab = idx;
        }
        $scope.newGoal = function () {
            $location.url('goal/elite/aggregation/item');
        }
        $scope.viewEntity = function (goalId) {
            $location.url('goal/elite/aggregation/item/' + goalId);
        }
    }]);
