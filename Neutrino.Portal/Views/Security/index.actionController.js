console.log("security index actionController")

angular.module("neutrinoProject").register.controller('index.actionController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns', 
function ($scope, $location, $compile, ajaxService, dataTableColumns ) {

    "use strict";
    $scope.initializeController = function () {
        $scope.title = 'لیست فعالیت ها';
        dataTableColumns.initialize();
        dataTableColumns.add({ title: 'نام فعالیت', mappingData: 'title' });
        dataTableColumns.add({ title: 'نام فارسی فعالیت', mappingData: 'faTitle' });
        dataTableColumns.add({ title: 'Action Url', mappingData: 'actionUrl' });
        dataTableColumns.add({ title: 'Html Url', mappingData: 'htmlUrl' });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                      + " ng-click='funcEntity(" + JSON.stringify(full) + ")' aria-expanded='false'><i class='fa fa-magic'></i><span>جزئیات</span>"
                      + "  </button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/appActionService/getDataGrid';

    }

    $scope.newEntity = function () {
        $location.url('security/action/item')
    }

    $scope.funcEntity = function (dataSelected) {
        $location.url('security/action/item/' + dataSelected.id);
    }

}]);
