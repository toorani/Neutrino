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
        dataTableColumns.add({title: 'وضعیت', mappingData: 'status'});
        dataTableColumns.add({
            isKey: true, fnRenderCallBack: function (data, type, promotion) {

                var jsCommission = JSON.stringify(promotion);
                var ctrl = "<button type='button'  class='btn ls-light-blue-btn dropdown-toggle' data-toggle='dropdown' "
                if (promotion.statusId != 4) {
                    ctrl += ' disabled ';
                }

                ctrl += " ng-click='compensantory(" + jsCommission + ")' aria-expanded='false'>"
                         + "<i class='fa fa-magic'></i><span> پورسانت ترمیمی</span>"
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
    
    $scope.compensantory = function (promotion) {
        $location.url('promotion/compensantory/item/' + promotion.id);
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
