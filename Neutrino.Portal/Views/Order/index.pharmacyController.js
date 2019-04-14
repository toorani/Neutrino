console.log("Order\Pharmacy indexController")

angular.module("neutrinoProject").register.controller('index.pharmacyController',
['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns',
    function ($scope, $location, $compile, ajaxService, dataTableColumns) {

        "use strict";


        $scope.initializeController = function () {
            $scope.title = 'لیست هوشمند خرید داروخانه';
            dataTableColumns.initialize();
            dataTableColumns.add({
                searchable: false
                , sortable: false
                , width: "20px"
                , fnRenderCallBack: function (data, type, full) {
                    return $compile("<div><input type='checkbox' style='margin-right:-5px;'  /></div>")($scope);
                }
            });
            dataTableColumns.add({ title: 'نام دارو', mappingData: 'enName', priority: 1 });
            dataTableColumns.add({ title: 'قیمت دارو', mappingData: 'price' });
            dataTableColumns.add({ title: 'جایزه', mappingData: 'promotionValue' });
            dataTableColumns.add({ title: 'سود خرید نقدی', mappingData: 'cashProfit' });
            dataTableColumns.add({ title: 'سود خرید اعتباری', mappingData: 'creditProfit' });
            dataTableColumns.add({ title: 'عدد پیشنهادی', mappingData: 'suggestedQuantity', priority: 3 });
            dataTableColumns.add({
                title: 'عدد نهایی'
                , mappingData: 'finalQuantity.value'
                , fnRenderCallBack: function (data, type, full) {
                    return $compile("<input type='text' class='form-control' value='" + full.finalQuantity + "'/>")($scope);
                }
                , priority: 2
            });

            $scope.dataColumns = dataTableColumns.getItems();
            $scope.serverUrl = '/api/pharmacyOrderService/getDataGrid';
        }
    }]);