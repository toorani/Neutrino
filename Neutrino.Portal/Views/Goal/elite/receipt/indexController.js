console.log("Goal/eilte/receipt/indexController")

angular.module("neutrinoProject").register.controller('receipt.indexController',
['$scope', '$location', '$compile', '$filter', '$routeParams', 'ajaxService', 'dataTableColumns',
    function ($scope, $location, $compile, $filter, $routeParams, ajaxService, dataTableColumns) {

        "use strict";

        var goalGoodsCatType = ($routeParams.id || 'total');

        $scope.initializeController = function () {
            
            dataTableColumns.initialize();

            dataTableColumns.add({ mappingData: 'year', title: 'سال' });
            dataTableColumns.add({ mappingData: 'month', title: 'ماه' });
            if (goalGoodsCatType == 'prgv') {
                dataTableColumns.add({ mappingData: 'goalGoodsCategory.name', title: 'نوع هدف وصول' });
            }
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


        
        if (goalGoodsCatType == 'total') {
            $scope.goalGoodsCatType = '4'; // هدف وصول کل
            $scope.title = 'هدف گذاری وصول کل';
        }
        else if (goalGoodsCatType == 'prgv') {
            $scope.goalGoodsCatType = '5,6'; // هدف وصول خصوصی / هدف وصول دولتی
            $scope.title = 'هدف گذاری وصول خصوصی/دولتی';
        }


        $scope.goalTabs = [{
            title: 'اهداف محاسبه نشده', filter: {
                goalType: 'distributor',
                isUsed: 'false',
                goalGoodsCatType: $scope.goalGoodsCatType 
            }
        }, {
            title: 'اهداف محاسبه شده', filter: {
                goalType: 'distributor',
                isUsed: 'true',
                goalGoodsCatType: $scope.goalGoodsCatType
            }
        }]
        $scope.activeTab = 0;
        $scope.tabSelected = function (idx) {
            $scope.activeTab = idx;
        }
        $scope.newGoal = function () {
            $location.url('goal/elite/receipt/item/param/' + ($routeParams.id || 'total'));
        }
        $scope.viewEntity = function (goalId) {
            $location.url('goal/elite/receipt/item/' + goalId);
        }
    }]);
