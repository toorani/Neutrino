console.log("OrgStructureShare indexController")

angular.module("neutrinoProject").register.controller('orgStructureShare.indexController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
function ($scope, $location, $compile, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'مدیریت سهم سمت های سازمانی';
        dataTableColumns.initialize();

        dataTableColumns.add({
            title: 'نام مرکز'
            , mappingData: 'branch.name'

        });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                      + " ng-click='funcEntity(" + JSON.stringify(full) + ")' aria-expanded='false'><i class='fa fa-magic'></i><span>جزئیات</span>"
                      + "  </button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/orgStructureShareService/getDataGrid';
        $scope.branchExpanded = function () {
            var aData = $scope.ngGrid.fnGetData($scope.rowSelected);
            $scope.ngGrid.fnOpen($scope.rowSelected, $scope.fnFormatDetails(aData.items), 'details');
        }



    }

    $scope.fnFormatDetails = function (data) {
        var sOut = '<table class="table-bordered table-striped" cellspacing="0" style="width:100%">';
        sOut += "<thead><tr>"

        sOut += "<th class='align-center'>نام پست سازمانی</th>"
        sOut += "<th class='align-center'>سهم فروش</th>"
        sOut += "<th class='align-center'>سهم وصول</th>"
        sOut += "</tr></thead>"

        data.forEach(function (item) {
            var salePercent = item.salePercent != null ? item.salePercent : '';
            var receivablePercent = item.receivablePercent != null ? item.receivablePercent : ''
            sOut += '<tr><td class="align-center">' + item.orgStructure.title
                + '</td><td class="align-center">'
                + salePercent
                + '</td><td class="align-center">'
                + receivablePercent
                + '</td></tr>';
        });


        sOut += '</table>';

        return sOut;
    }

    $scope.newEntity = function () {
        $location.url('orgStructureShare/item')
    }

    $scope.funcEntity = function (dataSelected) {
        $location.url('orgStructureShare/item/' + dataSelected.branch.id);
    }

}]);
