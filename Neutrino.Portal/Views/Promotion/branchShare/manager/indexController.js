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

            $scope.totalValues = {
                totalPromotion() { return this.totalReceiptPromotion + this.totalSellerPromotion + this.totalSalesPromotion },
                totalReceiptPromotion: 0,
                totalSellerPromotion: 0,
                totalSalesPromotion: 0,
                clearValues() {
                    this.totalReceiptPromotion = 0;
                    this.totalSellerPromotion = 0;
                    this.totalSalesPromotion = 0;
                }
            };

            $scope.branchPromotionValues = {
                totalReceiptPromotion: 0,
                totalSalesPromotion: 0,
                totalSellerPromotion: 0,
                totalBranchPromotion() { return this.totalReceiptPromotion + this.totalSalesPromotion + this.totalSellerPromotion }
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

                            let findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 1); //sales
                            if (findRecord.length == 1)
                                $scope.branchPromotionValues.totalSalesPromotion = findRecord[0].totalFinalPromotion;

                            findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 2 || x.goalTypeId == 3); //receipt
                            if (findRecord.length == 2)
                                $scope.branchPromotionValues.totalReceiptPromotion = findRecord[0].totalFinalPromotion + findRecord[1].totalFinalPromotion;


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

                        $scope.memberSharePromotionList.forEach((prom) => {
                            $scope.totalValues.totalReceiptPromotion += prom.receiptPromotion;
                            $scope.totalValues.totalSalesPromotion += prom.branchSalesPromotion;
                            $scope.totalValues.totalSellerPromotion += prom.sellerPromotion;
                        });

                        $scope.branchPromotionValues.totalSellerPromotion = $scope.totalValues.totalSellerPromotion;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {
                ajaxService.ajaxPost($scope.memberSharePromotionList, '/api/memberSharePromotionService/addOrModify',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
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

            $scope.setTotalValues = function () {
                $scope.totalValues.clearValues();

                $scope.memberSharePromotionList.forEach((prom) => {
                    $scope.totalValues.totalReceiptPromotion += prom.receiptPromotion;
                    $scope.totalValues.totalSalesPromotion += prom.branchSalesPromotion;
                    $scope.totalValues.totalSellerPromotion += prom.sellerPromotion;
                });
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

        }]);
