console.log("OrderSchedule\Pharmacy indexController")

angular.module("neutrinoProject").register.controller('index.pharmacyController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

    "use strict";
    $scope.viewModel = {
        orderDay: null,
        deliveryDay: null
    }
    $scope.weekDays = [{ title: 'شنبه', id: 1 }
        , { title: 'یکشنبه', id: 2 }
        , { title: 'دوشنبه', id: 3 }
        , { title: 'سه شنبه', id: 4 }
        , { title: 'چهار شنبه', id: 5 }
        , { title: 'پنج شنبه', id: 6 }
        , { title: 'جمعه', id: 7 }
    ]
    $scope.initializeController = function () {
        $scope.title = 'زمانبندی سفارش داروخانه';
    }
    $scope.submit = function () {
        ajaxService.ajaxPost($scope.viewModel, "api/personelShareService/add",
            function (response) {
                alertService.showSuccess(response.data.transactionalData.returnMessage);
                $scope.loadData();
            },
            function (response) {

                alertService.showError(response.data.transactionalData.returnMessage);
                alertService.setValidationErrors($scope, response.data.transactionalData.validationErrors);
            });
    }
    $scope.loadData = function () {
        if ($scope.viewModel.branch != null) {
            $scope.viewModel.items = [];
            ajaxService.ajaxPost($scope.viewModel.branch.id, "api/personelShareService/getDataItem",
            function (response) {
                $scope.viewModel.items = response.data.items;
                if ($scope.viewModel.items.length == 0) {
                    alertService.showWarning('پست سازمانی برای مرکز انتخاب شده تعریف نشده است');
                }

            },
            function (response) {
                alertService.showError(response.data.returnMessage);
                alertService.setValidationErrors($scope, response.data.validationErrors);
            });
        }

    }

}]);