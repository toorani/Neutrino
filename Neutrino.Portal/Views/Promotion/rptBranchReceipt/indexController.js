console.log("promotion.branchreceiptrpt.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchreceiptrpt.indexController',
    ['$scope', 'ajaxService', 'alertService', 'persianCalendar', 'exportExcel',
        function ($scope, ajaxService, alertService, persianCalendar, exportExcel) {

            "use strict";
            $scope.viewModel = {
                year: null,
                month: null,
                goalGoodsCategoryTypeId: null
            }

            $scope.years = persianCalendar.getYears();
            $scope.months = persianCalendar.getMonthNames();
            $scope.reportData = [];
            $scope.receiptTypes = [{ id: 4, title: 'وصول کل' },{ id: 5, title: 'وصول خصوصی' }]

            $scope.initializeController = function () {
                $scope.title = 'گزارش اهداف وصول';

                var perDate = new persianDate();
                $scope.viewModel.year = perDate.year();
                $scope.viewModel.month = perDate.month();

                //$scope.viewModel.year = 1397;
                //$scope.viewModel.month = 10;
            }


            $scope.showReport = function () {
                if ($scope.viewModel.month != null && $scope.viewModel.year != null
                    && $scope.viewModel.goalGoodsCategoryTypeId != null) {
                    ajaxService.ajaxCall($scope.viewModel, '/api/promotionReportService/getBranchReceiptGoals', 'get',
                        function (response) {
                            $scope.reportData = response.data;
                        },
                        function (response) {
                            alertService.showError(response);
                        });
                }
            }

            $scope.exportReport = function () {

                var url = '/api/promotionReportService/exportExcelBranchReceiptGoals?year=' + $scope.viewModel.year + '&month=' + $scope.viewModel.month + '&goalGoodsCategoryTypeId=' + $scope.viewModel.goalGoodsCategoryTypeId;
                exportExcel.loadfile(url);
            }

        }]);
