console.log("index")

angular.module("neutrinoProject").register.controller('indexController',
['$location', '$compile', '$scope', 'ajaxService', 'dataTableColumns',
function ($location, $compile, $scope, ajaxService, dataTableColumns) {

    "use strict";

    $scope.initializeController = function () {
        $scope.title = 'صفحه اول';
        //ajaxService.ajaxPost({}, "api/branchService/getBranches",
        //    function (response) {
                
        //    },
        //    function (response) {
        //    });
        
        dataTableColumns.initialize();

        dataTableColumns.add({ mappingData: 'name', title: 'نام ', searchable:true});

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/companyService/getDataGrid';
        $scope.companyExpanded = function () {
            var aData = $scope.ngGrid.fnGetData($scope.rowSelected);
            ajaxService.ajaxCall({ companyId: aData.id }, "api/GoodsService/getCompanyGoods",'get',
            function (response) {
                $scope.ngGrid.fnOpen($scope.rowSelected, $scope.fnFormatDetails(response.data), 'details');
            },
            function (response) {
                console.log(response.data.ReturnMessage);
                //alertService.RenderErrorMessage(response.data.ReturnMessage);
            });

        }
       

        $scope.fnFormatDetails = function (data) {
            var sOut = '<table class="table-bordered table-striped" cellspacing="0" style="width:100%">';
            sOut += "<thead><tr>"

            sOut += "<th>نام فارسی</th>"
            sOut += "<th>نام انگلیسی</th>"
            sOut += "</tr></thead>"

            data.forEach(function (item) {
                sOut += '<tr><td>' + item.faName + '</td><td>' + item.enName + '</td></tr>';
            });


            sOut += '</table>';

            return sOut;
        }
    }

}]);

