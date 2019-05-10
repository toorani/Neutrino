console.log("security index.permissionController")

angular.module("neutrinoProject").register.controller('index.permissionController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns', 
function ($scope, $location, $compile, ajaxService, dataTableColumns ) {

    "use strict";
    $scope.initializeController = function () {
        $scope.title = 'لیست سطوح دسترسی';
        dataTableColumns.initialize();
        dataTableColumns.add({ title: 'عنوان انگلیسی نقش', mappingData: 'name' });
        dataTableColumns.add({ title: 'عنوان فارسی نقش', mappingData: 'faName' });
        dataTableColumns.add({
            width : '220px',
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  class='btn ls-light-blue-btn' "
                      + " ng-click='funcEntity(" + JSON.stringify(full) + ")' aria-expanded='false'>تعریف سطح دسترسی"
                      + "  </button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/accountService/getRoleDataGrid';

    }

    $scope.funcEntity = function (dataSelected) {
        $location.url('security/permission/item/' + dataSelected.id);
    }

}]);
