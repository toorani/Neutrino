console.log("promotion.branchShare.manager.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.manager.indexController',
    ['$scope', 'alertService', 'ajaxService', 'modalService', 'persianCalendar',
        function ($scope, alertService, ajaxService, modalService, persianCalendar) {

            "use strict";
            $scope.branchPromotoinDetail = [];
            $scope.branchMembers = [];
            $scope.branchMemberPromotions = [];

            $scope.viewModel = {
                memberId: null,
                fullName: null,
                member: null,
                branchSalesPromotion: 0,
                sellerPromotion: 0,
                receiptPromotion: 0
            }


            $scope.member_srch = '';

            $scope.initializeController = function () {
                $scope.title = 'تقسیم پورسانت';
                getBranchPromotionDetail();
            }

            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({}, "api/promotionReportService/getBranchPromotionForStep1BranchManager", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;
                        if ($scope.branchPromotoinDetail != null && $scope.branchPromotoinDetail.length != 0) {
                            $scope.year = $scope.branchPromotoinDetail[0].year;
                            $scope.month = $scope.branchPromotoinDetail[0].month;
                            $scope.monthTitle = persianCalendar.getMonthNames()[$scope.month - 1].name;

                            getMemberSharePromotionList();
                        }

                    },
                    function (response) {
                        $scope.branchPromotoinDetail = [];
                        alertService.showError(response);
                    });
            }

            var getMemberSharePromotionList = function () {
                ajaxService.ajaxCall({ month: $scope.month, year: $scope.year }, "api/memberSharePromotionService/getMemberSharePromotionList4Manager", 'get',
                    function (response) {
                        $scope.memberSharePromotionList = response.data;

                        $scope.totalSellerPromotion = 0;
                        $scope.memberSharePromotionList.forEach(x => {
                            $scope.totalSellerPromotion += x.sellerPromotion;
                        });
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {
                ajaxService.ajaxPost($scope.memberSharePromotionList, '/api/memberSharePromotionService/addOrModify',
                    function (response) {
                        let filterResults = $scope.branchMemberPromotions.filter((mp) => mp.memberId == $scope.viewModel.memberId);
                        if (filterResults.length != 0) {
                            filterResults[0].promotion = $scope.viewModel.promotion;
                        }
                        else {
                            $scope.branchMemberPromotions.push($scope.viewModel)
                        }
                        $scope.viewModel = {};
                        alertService.showSuccess(response.data.returnMessage);
                        calculateAssigendPromotion();

                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.releaseManagerStep1 = function () {
                ajaxService.ajaxPost({}, '/api/memberSharePromotionService/releaseManagerStep1',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
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
            $scope.memberFilter = function (record) {
                return ($scope.member_srch == '' || String(record.memberCode).indexOf($scope.member_srch) != -1
                    || String(record.fullName).indexOf($scope.member_srch) != -1 || String(record.positionTitle).indexOf($scope.member_srch) != -1);
            }

            $scope.getRecieptPromotions = function () {
                return $scope.branchPromotoinDetail.filter((prom) => prom.positionPromotions != null);
            }
           
            $scope.getRemained = function () {
                let branchSalesPromotion = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 1)[0];
                let result = {
                    sellerPromotionRemained: 0,
                    salesPromotionRemained: 0,
                    receiptPromotionRemained: 0
                };
                if (branchSalesPromotion != null) {
                    result = {
                        sellerPromotionRemained: $scope.totalSellerPromotion,
                        salesPromotionRemained: branchSalesPromotion.totalFinalPromotion,
                        receiptPromotionRemained: $scope.getTotal() - branchSalesPromotion.totalFinalPromotion
                    };
                    if ($scope.memberSharePromotionList != null) {
                        $scope.memberSharePromotionList.forEach(x => {
                            result.sellerPromotionRemained -= x.sellerPromotion;
                            result.salesPromotionRemained -= x.branchSalesPromotion;
                            result.receiptPromotionRemained -= x.receiptPromotion;
                        });
                    }
                    
                }

                return result;
            }

            $scope.getMemberTotalPromotion = function (record) {
                record.managerPromotion = record.sellerPromotion + record.branchSalesPromotion + record.receiptPromotion;
                $scope.assigendPromotion = 0;
                $scope.memberSharePromotionList.forEach((mp) => {
                    $scope.assigendPromotion += mp.managerPromotion;
                });
                return record.managerPromotion;
            }

            var calculateAssigendPromotion = function () {
                $scope.assigendPromotion = 0;
                $scope.branchMemberPromotions.forEach((mp) => {
                    $scope.assigendPromotion += mp.managerPromotion;
                });
            }

        }]);
