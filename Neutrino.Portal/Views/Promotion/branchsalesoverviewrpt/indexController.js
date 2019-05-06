console.log("promotion.branchsalesoverviewrpt.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchsalesoverviewrpt.indexController',
    ['$scope', 'ajaxService', 'alertService', 'persianCalendar', 'exportExcel',
        function ($scope, ajaxService, alertService, persianCalendar, exportExcel) {
            "use strict";
            $scope.viewModel = {
                startDate: null,
                endDate: null,
                goalGoodsCategoryId: null
            }

            $scope.reportData = [];

            $scope.initializeController = function () {
                $scope.title = 'گزارش عملکرد کلی اهداف فروش ';
            }

            $scope.onGoalGoodsCategoryTypeChanged = function () {
                $scope.viewModel.goalGoodsCategoryId = null;
                getGoalGoodsCategories();
            }
            $scope.isFulfillGoalStep = function (reportRecord) {
                let metCount = 0;
                reportRecord.promotionGoalSteps.forEach((goalStep) => {
                    if (goalStep.fulfilledPercent >= 100)
                        metCount++;
                })
                return metCount != 0;
            }

            $scope.showReport = function () {

                if ($scope.viewModel.startDate != null && $scope.viewModel.endDate != null
                    && $scope.viewModel.goalGoodsCategoryId != null) {
                    ajaxService.ajaxCall($scope.viewModel, '/api/promotionReportService/getBranchSaleGoals', 'get',
                        function (response) {
                            $scope.reportData = response.data;
                        },
                        function (response) {
                            alertService.showError(response);
                        });
                }
            }
            $scope.exportReport = function () {

                var url = '/api/promotionReportService/exportExcelSaleGoals?startDate=' + $scope.viewModel.startDate
                    + '&endDate=' + $scope.viewModel.endDate
                    + '&goalGoodsCategoryId=' + $scope.viewModel.goalGoodsCategoryId;

                exportExcel.loadfile(url);
            }

            var getGoalGoodsCategories = function () {
                return ajaxService.ajaxCall({ goodsCategoryTypeId: $scope.viewModel.goalGoodsCategoryTypeId, isActive: true, iGoalTypeId: 1 }
                    , "api/goalGoodsCategoryService/getDataList", 'get',
                    function (response) {
                        $scope.goalGoodsCategories = response.data;
                    },
                    function (response) {
                        $scope.goodsCategories = [];
                        alertService.showError(response);
                    });
            }
            var getGoalGoodsCategoryTypes = function () {
                $scope.goalGoodsCategoryTypes = [
                    { id: 1, description: 'گروهی' },
                    { id: 2, description: 'تکی' }]
            }

        }]);
