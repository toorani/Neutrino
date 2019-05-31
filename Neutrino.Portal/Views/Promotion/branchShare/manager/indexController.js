﻿console.log("promotion.branchShare.manager.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.manager.indexController',
    ['$scope', 'alertService', 'ajaxService', 'modalService',
        function ($scope, alertService, ajaxService, modalService) {

            "use strict";
            $scope.branchPromotoinDetail = [];
            $scope.branchMembers = [];
            $scope.branchMemberPromotions = [];

            $scope.viewModel = {
                memberId: null,
                managerPromotion: null,
                fullName: null,
                member: null
            }

            $scope.initializeController = function () {
                $scope.title = 'تقسیم پورسانت';
                getBranchPromotionDetail();
                getMemebrs();
                getMemberSharePromotion();

            }

            $scope.submit = function () {
                $scope.viewModel.memberId = $scope.viewModel.member.id;
                ajaxService.ajaxPost($scope.viewModel, '/api/memberSharePromotionService/addOrModify',
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

            $scope.editMemberShare = function (memberSahre) {
                $scope.viewModel = memberSahre;
            }
            $scope.deleteMemberShare = function (memberSahre) {
                var bodyText = 'آیا برای حذف مطمئن هستید ؟'

                var modalOptions = {
                    actionButtonText: 'حذف پورسانت',
                    headerText: 'تقسیم پورسانت',
                    bodyText: bodyText
                };

                modalService.showModal({}, modalOptions)
                    .then(function (result) {
                        if (result == 'ok') {
                            ajaxService.ajaxPost(memberSahre, "api/memberSharePromotionService/delete",
                                function (response) {
                                    alertService.showSuccess(response.data.returnMessage);
                                    $scope.branchMemberPromotions = $scope.branchMemberPromotions.filter(item => item.memberId != memberSahre.memberId);
                                    calculateAssigendPromotion();
                                },
                                function (response) {
                                    alertService.showError(response);
                                });
                        }// end of if
                    }, function () {
                        // Cancel
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
            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({}, "api/promotionReportService/getBranchPromotionForStep1BranchManager", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;
                    },
                    function (response) {
                        $scope.branchPromotoinDetail = [];
                        alertService.showError(response);
                    });
            }
            var getMemberSharePromotion = function () {
                ajaxService.ajaxCall({ statusId: 1 }, "api/memberSharePromotionService/getMemberSharePromotion", 'get',
                    function (response) {
                        $scope.branchMemberPromotions = response.data;
                        calculateAssigendPromotion();
                    },
                    function (response) {
                        $scope.branchMemberPromotions = [];
                        alertService.showError(response);
                    });
            }

            var calculateAssigendPromotion = function () {
                $scope.assigendPromotion = 0;
                $scope.branchMemberPromotions.forEach((mp) => {
                    $scope.assigendPromotion += mp.managerPromotion;
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
