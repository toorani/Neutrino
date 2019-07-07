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
            $scope.goalGoodsCategories = [];
            $scope.isEmpyData = false;

            $scope.initializeController = function () {
                $scope.title = 'گزارش عملکرد اهداف فروش ';
                getGoalGoodsCategoryTypes();
            }

            $scope.resetGoalGoodsCatgories = function () {
                $scope.goalGoodsCategories = [];
                $scope.reportData = [];
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

                //var url = '/api/promotionReportService/exportExcelSaleGoals?startDate=' + $scope.viewModel.startDate
                //    + '&endDate=' + $scope.viewModel.endDate
                //    + '&goalGoodsCategoryId=' + $scope.viewModel.goalGoodsCategoryId;

                //exportExcel.loadfile(url);


                ajaxService.ajaxCall({ startDate: $scope.viewModel.startDate, endDate: $scope.viewModel.endDate, goalGoodsCategoryId: $scope.viewModel.goalGoodsCategoryId }
                    , "api/promotionReportService/exportExcelSaleGoals", 'get',
                    function (response) {
                        exportExcel.loadfile(response.data);
                    },
                    function (response) {
                        alertService.showError(response);
                    });



            }

            $scope.getGoalGoodsCategories = function () {
                $scope.viewModel.goalGoodsCategoryId = null;
                $scope.isEmpyData = false;
                ajaxService.ajaxCall({ goodsCategoryTypeId: $scope.viewModel.goalGoodsCategoryTypeId, iGoalTypeId: 1, startDate: $scope.viewModel.startDate, endDate: $scope.viewModel.endDate }
                    , "api/goalGoodsCategoryService/getListforReport", 'get',
                    function (response) {
                        $scope.goalGoodsCategories = response.data;
                        $scope.isEmpyData = $scope.goalGoodsCategories.length == 0;
                    },
                    function (response) {
                        alertService.showError(response);
                    });

            }
            var getGoalGoodsCategoryTypes = function () {
                $scope.goalGoodsCategoryTypes = [
                    { id: 1, description: 'گروهی' },
                    { id: 2, description: 'تکی' }]
            }

        }]);
