console.log("promotion.overview.indexController")

angular.module("neutrinoProject").register.controller('promotion.overview.indexController',
    ['$scope', 'ajaxService', 'alertService', 'persianCalendar', 'exportExcel',
        function ($scope, ajaxService, alertService, persianCalendar, exportExcel) {

            "use strict";
            $scope.viewModel = {
                year: null,
                month: null
            }

            $scope.years = persianCalendar.getYears();
            $scope.months = persianCalendar.getMonthNames();
            $scope.reportData = [];
            $scope.initializeController = function () {
                $scope.title = 'گزارش عملکرد نهایی ';

                var perDate = new persianDate();
                $scope.viewModel.year = perDate.year();
                $scope.viewModel.month = perDate.month();
            }


            $scope.showReport = function () {
                ajaxService.ajaxCall({ year: $scope.viewModel.year, month: $scope.viewModel.month }, '/api/promotionReportService/getOverView', 'get',
                    function (response) {
                        $scope.reportData = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.exportReport = function () {
                ajaxService.ajaxCall({ year: $scope.viewModel.year, month: $scope.viewModel.month }, '/api/promotionReportService/exportExcelOverView', 'get',
                    function (response) {
                        exportExcel.loadfile(response.data);
                    },
                    function (response) {
                        alertService.showError(response);
                    });

            }

        }]);
