console.log("Benefit index multiController")

angular.module("neutrinoProject").register.controller('index.multiController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns', 
function ($scope, $location, $compile, ajaxService, dataTableColumns ) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'مدیریت درصد سهم مراکز';
        dataTableColumns.initialize();

        dataTableColumns.add({
            title: 'نام دسته(های)دارویی'
            , sortable: false
            , fnRenderCallBack: function (data, type, full) {
                var result = "<div class='ng-ms form-item-container'><ul class='list-inline'>"
                full.goodsCategories.forEach(function (goodsCat) {
                    result += "<li><span><i class='green fa fa-medkit'></i>&nbsp;&nbsp;" + goodsCat.name + "</span></li>"
                });
                result += "</ul></div>";
                return result;
            }
        });
        dataTableColumns.add({ title: 'نام مرکز', mappingData: 'branch.name' });
        dataTableColumns.add({
            title: 'درصد سهم'
            , mappingData: 'percent'
            , fnRenderCallBack: function (data, type, full) {
                var pType = 'success';
                if (full.percent <= 30)
                    pType = 'danger';
                else if (full.percent > 30 && full.percent <= 60)
                    pType = 'warning';
                else if (full.percent > 60 && full.percent <= 90)
                    pType = 'info';

                var result = "<uib-progressbar class='red progress-striped' value='" + full.percent + "' type='" + pType + "'>" + full.percent + "%</uib-progressbar>";
                return $compile(result)($scope);
            }
        });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, full) {
                return $compile("<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                      + " ng-click='funcEntity(" + JSON.stringify(full) + ")' aria-expanded='false'><i class='fa fa-cogs'></i>"
                      + "  </button>")($scope);
            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/branchBenefitService/getDataGrid';

    }

    $scope.newEntity = function () {
        $location.url('branchBenefit/item')
    }

    $scope.funcEntity = function (dataSelected) {
        $location.url('branchBenefit/item/' + dataSelected.id);
    }

}]);
