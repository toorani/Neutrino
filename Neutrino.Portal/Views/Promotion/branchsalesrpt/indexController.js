console.log("promotion.branchsalesrpt.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchsalesrpt.indexController',
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
                $scope.title = 'گزارش عملکرد اهداف فروش ';
                getGoalGoodsCategoryTypes();
                $scope.viewModel.startDate = '1397/10/01';
                $scope.viewModel.endDate = '1397/10/30';

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
                ajaxService.ajaxCall($scope.viewModel, '/api/promotionReportService/getBranchSaleGoals', 'get',
                    function (response) {
                        $scope.reportData = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.exportReport = function () {

                var url = '/api/promotionReportService/exportExcelOverView?year=' + $scope.viewModel.year + '&month=' + $scope.viewModel.month;
                exportExcel.loadfile(url);
            }


        }]);
