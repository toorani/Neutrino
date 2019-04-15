console.log("fulfillmentPercent indexController")

angular.module("neutrinoProject").register.controller('fulfillmentPercent.indexController',
['$scope', '$location', '$compile', '$filter', '$routeParams', 'ajaxService', 'alertService', 'dataTableColumns',
function ($scope, $location, $compile, $filter, $routeParams, ajaxService, alertService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.goalGoodsCatType = ($routeParams.id || 0);
        setTitle($scope.goalGoodsCatType);
        dataTableColumns.initialize();

        dataTableColumns.add({ title: 'تاریخ', mappingData: 'startDate' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                      + " ng-click='funcEntity(" + JSON.stringify(full) + ")' aria-expanded='false'><i class='fa fa-magic'></i><span> شرط تحقق پورسانت</span>"
                      + "  </button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/goalService/getDataGrid';

    }

    $scope.goalTabs = [{
        title: 'اهداف محاسبه نشده', filter: {
            goalType: 'distributor',
            isUsed: 'false',
            goalGoodsCatType: ($routeParams.id || 0)
        }
    }, {
        title: 'اهداف محاسبه شده', filter: {
            goalType: 'distributor',
            isUsed: 'true',
            goalGoodsCatType: ($routeParams.id || 0)
        }
    }]
    $scope.activeTab = 0;
    $scope.tabSelected = function (idx) {
        $scope.activeTab = idx;
    }

    $scope.funcEntity = function (dataSelected) {
        $location.url('goalFulfillment/item/' + dataSelected.id);
    }

    var setTitle = function (goalGoodsCatType) {
        ajaxService.ajaxCall({}, "api/GoalGoodsCategoryTypeService/getValues", 'get',
            function (response) {
                $scope.title = ' شرط تحقق پورسانت ';

                $scope.title += $filter('filter')(response.data, { name: goalGoodsCatType })[0].description;

            },
            function (response) {
                alertService.showError(response);
            });
    }

}]);