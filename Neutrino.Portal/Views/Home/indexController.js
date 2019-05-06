console.log("index")

angular.module("neutrinoProject").register.controller('indexController',
['$location', '$compile', '$scope', 'ajaxService', 'dataTableColumns',
function ($location, $compile, $scope, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'صفحه اول';
        //ajaxService.ajaxCall({ startDate: '1397/10/01', endDate: '1397/10/01', goalGoodsCategoryId: 5094 }
        //    , "api/promotionReportService/getBranchSaleGoals1"
        //    ,'get',
        //    function (response) {
        //        $scope.rpt = response.data;
        //    },
        //    function (response) {
        //    });
        
        //dataTableColumns.initialize();

        //dataTableColumns.add({ mappingData: 'name', title: 'نام ', searchable:true});

        //$scope.dataColumns = dataTableColumns.getItems();
        //$scope.serverUrl = '/api/companyService/getDataGrid';
        //$scope.companyExpanded = function () {
        //    var aData = $scope.ngGrid.fnGetData($scope.rowSelected);
        //    ajaxService.ajaxCall({ companyId: aData.id }, "api/GoodsService/getCompanyGoods",'get',
        //    function (response) {
        //        $scope.ngGrid.fnOpen($scope.rowSelected, $scope.fnFormatDetails(response.data), 'details');
        //    },
        //    function (response) {
        //        console.log(response.data.ReturnMessage);
        //        //alertService.RenderErrorMessage(response.data.ReturnMessage);
        //    });

        //}
       

        //$scope.fnFormatDetails = function (data) {
        //    var sOut = '<table class="table-bordered table-striped" cellspacing="0" style="width:100%">';
        //    sOut += "<thead><tr>"

        //    sOut += "<th>نام فارسی</th>"
        //    sOut += "<th>نام انگلیسی</th>"
        //    sOut += "</tr></thead>"

        //    data.forEach(function (item) {
        //        sOut += '<tr><td>' + item.faName + '</td><td>' + item.enName + '</td></tr>';
        //    });


        //    sOut += '</table>';

        //    return sOut;
        //}
    }

}]);

