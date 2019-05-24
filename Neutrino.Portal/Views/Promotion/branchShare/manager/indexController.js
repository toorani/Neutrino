console.log("promotion.branchShare.manager.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.manager.indexController',
    ['$scope', 'alertService', 'ajaxService', '$filter',
        function ($scope, alertService, ajaxService, $filter) {

            "use strict";
            $scope.branchPromotoinDetail = [];
            $scope.branchMembers = [];
            $scope.viewModel = {
                memberId: null,
                promotion: null
            }
            $scope.initializeController = function () {
                $scope.title = 'تقسیم پورسانت';
                getData();
                getMemebrs();
            }

            $scope.percent = 25;
            $scope.options = { animate: true, barColor: '#269abc', scaleColor: true, lineWidth: 3, lineCap: 'butt' }

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
            $scope.getTotal = function () {
                var total = 0;
                $scope.branchPromotoinDetail.forEach((prom) => {
                    total += prom.totalFinalPromotion;
                });
                return total;
            }
            $scope.getRecieptPromotions = function () {
                return $scope.branchPromotoinDetail.filter((prom) => prom.positionPromotions != null);
            }


            var getData = function () {
                ajaxService.ajaxCall({}, "api/promotionReportService/getBranchPromotionDetail", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;
                    },
                    function (response) {
                        $scope.branchPromotoinDetail = [];
                        alertService.showError(response);
                    });
            }
            var getMemebrs = function () {
                ajaxService.ajaxCall({}, "api/memberService/getBranchMembers", 'get',
                    function (response) {
                        $scope.branchMembers = response.data;
                    },
                    function (response) {
                        $scope.branchMembers = [];
                        alertService.showError(response);
                    });
            }

        }]);
