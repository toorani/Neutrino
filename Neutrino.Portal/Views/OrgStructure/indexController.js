console.log("OrgStructure index Controller")

angular.module("neutrinoProject").register.controller('orgStructure.indexController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
function ($scope, $location, $compile, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'تعریف ساختار سازمانی';
        dataTableColumns.initialize();

        dataTableColumns.add({ title: 'نام مرکز', mappingData: 'branch.name', sortable: false });
        dataTableColumns.add({ title: 'پست سازمانی', mappingData: 'positionType.description' });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/orgStructureService/getDataGrid';

    }

    $scope.groupingStartOrder = function () {
        let group = $scope.dataTable.startRenderArgs.group;
        let positionTypeId = $scope.dataTable.startRenderArgs.rows.data()[0].positionType.eId;

        return $compile(
                    $('<tr/>')
                        .append('<td>' + group + '</td>')
                        .append("<td  class='align-left'> <button type='button' class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                      + " ng-click='funcEntity(" + positionTypeId + ")' aria-expanded='false'><i class='fa fa-magic'></i><span>جزئیات</span>"
                      + "  </button> </td>")
                    )($scope);
    }

    $scope.newEntity = function () {
        $location.url('OrgStructure/item')
    }

    $scope.funcEntity = function (positionTypeId) {
        $location.url('orgStructure/item/' + positionTypeId);
    }

}]);
