console.log("promotion indexController")

angular.module("neutrinoProject").register.controller('promotion.indexController',
['$scope', '$location', '$compile', '$interval', 'alertService', 'ajaxService', 'dataTableColumns', '$uibModal', 'persianCalendar',
function ($scope, $location, $compile, $interval, alertService, ajaxService, dataTableColumns, $uibModal, persianCalendar) {

    "use strict";
    $scope.viewModel = {
        year: null,
        month: null
    }


    //$scope.$on('$destroy', function () {
    //    $scope.stop();
    //});

    $scope.years = persianCalendar.getYears();
    $scope.months = persianCalendar.getMonthNames();

    $scope.initializeController = function () {
        var promise;

        $scope.title = 'مدیریت محاسبه پورسانت';
        dataTableColumns.initialize();
        dataTableColumns.add({
            title: 'تاریخ'
            , mappingData: 'displayDate'

        });
        dataTableColumns.add({
            title: 'وضعیت'
            , mappingData: 'status'
            //, fnRenderCallBack: function (data, type, commission) {
            //    $interval(function () {
            //        if (commission.statusId == 3) // inserted
            //        {
            //            ajaxService.ajaxCall({ id: commission.id }, '/api/commissionService/getDataItem', 'get',
            //                function (response) {
            //                    $scope.tableAPI.rows().eq(0).filter(function (rowIdx) {
            //                        if ($scope.tableAPI.data()[rowIdx].id === commission.id)
            //                        {
            //                            var cell = $scope.tableAPI.rows(rowIdx).cells("td:eq(1)");
            //                            $scope.tableAPI.data()[rowIdx].status = response.data.status;
            //                            cell.data(response.data.status).draw(false);
            //                        }
            //                    });
            //                    commission.statusId = response.data.statusId;
            //                    commission.status = response.data.status;
            //                    //return response.data.status;
            //                },
            //                function (response) {
            //                });
            //        }
            //        else {
            //            $interval.cancel(promise);
            //        }
            //    }, 10000)
            //    return commission.status;
            //}

        });
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, promotion) {

                var jsCommission = JSON.stringify(promotion);
                var ctrl = "<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                if (promotion.statusId != 4) {
                    ctrl += ' disabled ';
                }

                ctrl += " ng-click='confirmFulfillment(" + jsCommission + ")' aria-expanded='false'>"
                         + "<i class='fa fa-magic'></i><span> ثبت نهایی شرط تحقق پورسانت</span>"
                         + "  </button>";
                return $compile(ctrl)($scope);

            }
        });

        $scope.dataColumns = dataTableColumns.getItems();
        $scope.serverUrl = '/api/promotionService/getDataGrid';



        var perDate = new persianDate();
        $scope.viewModel.year = perDate.year();
        $scope.viewModel.month = perDate.month();
    }

    //$scope.dataTableRowCallback = function (rowDetail) {
    //    var promise;
    //    var tableAPI = rowDetail.ngGrid.dataTable().api();
    //    $interval(function () {
    //        var cell = tableAPI.rows(rowDetail.index).cells("td:eq(1)");
    //        //var cell = tableAPI.cell(tableAPI..find("td:eq(2)"));
    //        //cell.data(totalQunatiy).draw();
    //        if (rowDetail.data.statusId == 3) // inserted
    //        {
    //            ajaxService.ajaxCall({ id: rowDetail.data.id }, '/api/commissionService/getDataItem', 'get',
    //                function (response) {
    //                    var cell = tableAPI.rows(rowDetail.index).cells("td:eq(1)");
    //                    rowDetail.data.statusId = response.data.statusId;
    //                    rowDetail.data.status = response.data.status;
    //                    cell.data(response.data.status).draw();
    //                    //
    //                },
    //                function (response) {
    //                });
    //        }
    //        else {
    //            $interval.cancel(promise);
    //        }
    //    }, 10000)
    //}
    //$scope.dataTableDrawCallback = function () {
    //    $scope.tableAPI = $scope.ngGrid.dataTable().api();
    //}

    $scope.confirmFulfillment = function (promotion) {
        ajaxService.ajaxPost(promotion, '/api/promotionService/startCalculation',
                function (response) {
                    alertService.showSuccess(response.data.actionResult.returnMessage);
                    $scope.ngGrid.fnReload();
                },
               function (response) {
                   alertService.showError(response);
               });
    }
    $scope.newEntity = function () {

        $scope.modalInstance = $uibModal.open({
            templateUrl: 'addPromotion.html',
            scope: $scope,
            backdrop: 'static',
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',

        });

        $scope.modalInstance.result.then(function (promotionAdded) {
            $scope.ngGrid.fnReload();
        }, function () { });

    }
    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, '/api/promotionService/add',
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                $scope.modalInstance.close();
            },
           function (response) {
               alertService.showError(response);
           });
    }
}]);
