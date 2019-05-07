console.log("promotion.branchsalesoverviewrpt.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchsalesoverviewrpt.indexController',
    ['$scope', 'ajaxService', 'alertService', 'exportExcel',
        function ($scope, ajaxService, alertService, exportExcel) {
            "use strict";
            $scope.viewModel = {
                startDate: null,
                endDate: null
            }

            $scope.reportData = [];

            $scope.initializeController = function () {
                $scope.title = 'گزارش عملکرد کلی اهداف فروش ';
                $scope.viewModel.startDate = '1397/10/01';
                $scope.viewModel.endDate = '1397/10/30';
            }
            $scope.collapse = function (event) {
                $(event.target).toggleClass("glyphicon-chevron-down");
            };

            $scope.showReport = function () {

                if ($scope.viewModel.startDate != null && $scope.viewModel.endDate != null) {
                    ajaxService.ajaxCall($scope.viewModel, '/api/promotionReportService/getBranchPromotionDetail', 'get',
                        function (response) {
                            $scope.reportData = response.data;
                        },
                        function (response) {
                            alertService.showError(response);
                        });
                }
                else {
                    alertService.showWarning('لطفا محدوده تاریخی را مشخص نماید');
                }
            }
            $scope.exportReport = function () {

                var url = '/api/promotionReportService/exportExcelSaleGoals?startDate=' + $scope.viewModel.startDate
                    + '&endDate=' + $scope.viewModel.endDate
                    + '&goalGoodsCategoryId=' + $scope.viewModel.goalGoodsCategoryId;

                exportExcel.loadfile(url);
            }

        }]);
