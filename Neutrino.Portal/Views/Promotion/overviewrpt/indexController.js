console.log("promotion.overview.indexController")

angular.module("neutrinoProject").register.controller('promotion.overview.indexController',
    ['$scope', 'ajaxService', 'alertService', 'persianCalendar','$location',
        function ($scope, ajaxService, alertService, persianCalendar, $location) {

            "use strict";
            $scope.viewModel = {
                year: null,
                month: null
            }

            $scope.years = persianCalendar.getYears();
            $scope.months = persianCalendar.getMonthNames();
            $scope.reportData = [];
            $scope.initializeController = function () {
                $scope.title = 'مدیریت محاسبه پورسانت';

                var perDate = new persianDate();
                //$scope.viewModel.year = perDate.year();
                //$scope.viewModel.month = perDate.month();

                $scope.viewModel.year = 1397;
                $scope.viewModel.month = 10;
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

                var url = '/api/promotionReportService/exportExcelOverView?year=' + $scope.viewModel.year + '&month=' + $scope.viewModel.month;
                $location.url(url);

                //return ajaxService.ajaxCall({ year: $scope.viewModel.year, month: $scope.viewModel.month }, '', 'get',
                //    function (response) {

                //    },
                //    function (response) {
                //        alertService.showError(response);
                //    });
            }

        }]);
