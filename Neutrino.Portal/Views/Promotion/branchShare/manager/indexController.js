console.log("promotion.branchShare.manager.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.manager.indexController',
    ['$scope', 'alertService', 'ajaxService', 'modalService', 'persianCalendar',
        function ($scope, alertService, ajaxService, modalService, persianCalendar) {

            "use strict";
            $scope.branchPromotoinDetail = [];
            $scope.branchMembers = [];
            $scope.branchMemberPromotions = [];
            $scope.promotionReviewStatusId = 1;
            $scope.viewModel = {
                memberId: null,
                fullName: null,
                member: null,
                branchSalesPromotion: 0,
                sellerPromotion: 0,
                receiptPromotion: 0
            }

            $scope.totalValues = {
                totalPromotion() { return this.totalReceiptPromotion + this.totalCompensatoryPromotion + this.totalSalesPromotion + this.totalSupplierPromotion },
                totalReceiptPromotion: 0,
                totalCompensatoryPromotion: 0,
                totalSalesPromotion: 0,
                totalSupplierPromotion: 0,
                clearValues() {
                    this.totalReceiptPromotion = 0;
                    this.totalCompensatoryPromotion = 0;
                    this.totalSalesPromotion = 0;
                    this.totalSupplierPromotion = 0;
                }
            };

            $scope.branchPromotionValues = {
                totalReceiptPromotion: 0,
                totalSalesPromotion: 0,
                totalCompensatoryPromotion: 0,
                totalSupplierPromotion: 0,
                totalBranchPromotion() { return this.totalReceiptPromotion + this.totalSalesPromotion + this.totalCompensatoryPromotion + this.totalSupplierPromotion },
                remindedSupplierPromotion() {
                    return branchPromotionValues.totalSupplierPromotion - totalValues.totalSupplierPromotion;
                },
                remindedReceiptPromotion() {
                    return branchPromotionValues.totalReceiptPromotion - totalValues.totalReceiptPromotion;
                },
                remindedCompensatoryPromotion() {
                    return branchPromotionValues.totalCompensatoryPromotion - totalValues.totalCompensatoryPromotion;
                },
            }

            $scope.member_srch = '';

            $scope.initializeController = function () {
                $scope.title = 'تقسیم پورسانت';
                getBranchPromotionDetail();
            }

            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({}, "api/branchPromotionService/getBranchPromotionForStep1", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;
                        if ($scope.branchPromotoinDetail != null && $scope.branchPromotoinDetail.length != 0) {
                            $scope.year = $scope.branchPromotoinDetail[0].year;
                            $scope.month = $scope.branchPromotoinDetail[0].month;
                            $scope.monthTitle = persianCalendar.getMonthNames()[$scope.month - 1].name;

                            let findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 1); //supplier
                            if (findRecord.length == 1)
                                $scope.branchPromotionValues.totalSupplierPromotion = findRecord[0].totalFinalPromotion;

                            findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 2 || x.goalTypeId == 3); //receipt
                            if (findRecord.length == 2)
                                $scope.branchPromotionValues.totalReceiptPromotion = findRecord[0].totalFinalPromotion + findRecord[1].totalFinalPromotion;

                            findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 4); //compensatory
                            if (findRecord.length == 1)
                                $scope.branchPromotionValues.totalCompensatoryPromotion = findRecord[0].totalFinalPromotion;

                            findRecord = $scope.branchPromotoinDetail.filter(x => x.goalTypeId == 5); //sales
                            if (findRecord.length == 1)
                                $scope.branchPromotionValues.totalSalesPromotion = findRecord[0].totalFinalPromotion;

                            getMemberPromotionList();
                        }
                        else {
                            $scope.promotionReviewStatusId = 0;
                        }

                    },
                    function (response) {
                        $scope.branchPromotoinDetail = [];
                        alertService.showError(response);
                    });
            }

            var getMemberPromotionList = function () {
                ajaxService.ajaxCall({ month: $scope.month, year: $scope.year }, "api/memberPromotionService/getMemberPromotionList4Manager", 'get',
                    function (response) {
                        $scope.memberPromotionList = response.data;
                        $scope.operationEmployeeCount = 0;
                        $scope.memberPromotionList.forEach((prom) => {
                            $scope.totalValues.totalReceiptPromotion += prom.receiptPromotion;
                            $scope.totalValues.totalCompensatoryPromotion += prom.compensatoryPromotion;
                            $scope.totalValues.totalSupplierPromotion += prom.supplierPromotion;
                            $scope.totalValues.totalSalesPromotion += prom.branchSalesPromotion;
                            if (prom.branchSalesPromotion != 0)
                                $scope.operationEmployeeCount++;
                        });
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {
                ajaxService.ajaxPost($scope.memberPromotionList, '/api/memberPromotionService/addOrModify',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.releaseManagerStep1 = function () {

                ajaxService.ajaxPost({}, '/api/memberPromotionService/releaseManagerStep1',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.promotionReviewStatusId = response.data.resultValue;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.setTotalValues = function () {
                $scope.totalValues.clearValues();

                $scope.memberPromotionList.forEach((prom) => {
                    $scope.totalValues.totalReceiptPromotion += prom.receiptPromotion;
                    $scope.totalValues.totalSupplierPromotion += prom.supplierPromotion;
                    $scope.totalValues.totalCompensatoryPromotion += prom.compensatoryPromotion;
                    $scope.totalValues.totalSalesPromotion += prom.branchSalesPromotion;
                });
            }

            $scope.memberFilter = function (record) {
                return ($scope.member_srch == '' || String(record.memberCode).indexOf($scope.member_srch) != -1
                    || String(record.fullName).indexOf($scope.member_srch) != -1 || String(record.positionTitle).indexOf($scope.member_srch) != -1
                    || String(record.positionTitle).indexOf($scope.member_srch) != -1);
            }

            $scope.getRecieptPromotions = function () {
                return $scope.branchPromotoinDetail.filter((prom) => prom.positionPromotions != null);
            }

            $scope.getMemberTotalPromotion = function (record) {
                record.promotion = record.compensatoryPromotion + record.branchSalesPromotion + record.receiptPromotion + record.supplierPromotion;
                $scope.assigendPromotion = 0;
                $scope.memberPromotionList.forEach((mp) => {
                    $scope.assigendPromotion += mp.managerPromotion;
                });
                return record.promotion;
            }

        }]);
