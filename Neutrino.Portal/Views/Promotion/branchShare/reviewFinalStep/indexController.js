console.log("promotion.branchShare.reviewFinalStep.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.reviewFinalStep.indexController',
    ['$scope', '$location', 'ajaxService', 'alertService',
        function ($scope, $location, ajaxService, alertService) {

            "use strict";

            $scope.memberPoromotions = [];
            $scope.initializeController = function () {
                $scope.title = ' تایید نهایی پورسانت پرسنل مرکز';
                getBranchPromotionDetail();
                getData();
            }
            

            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({}, "api/branchPromotionService/getBranchPromotionReleasedByCEO", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;

                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            var getData = function () {
                ajaxService.ajaxCall({ statusId: 4 }, "api/memberSharePromotionService/getMemberSharePromotion", 'get',
                    function (response) {
                        $scope.memberPoromotions = response.data;
                    },
                    function (response) {
                        $scope.memberPoromotions = [];
                        alertService.showError(response);
                    });
            }

            $scope.getTotalFinalPromotion = function () {
                let totalPormotion = 0;
                $scope.memberPoromotions.forEach(record => {
                    totalPormotion += record.finalPromotion;
                });
                return totalPormotion;
                
            }
            $scope.submit = function () {
                ajaxService.ajaxPost($scope.memberPoromotions, "api/memberSharePromotionService/addOrModfiyFinalPromotion",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });


            }
            $scope.determinedPromotion = function () {
                ajaxService.ajaxPost($scope.memberPoromotions, '/api/memberSharePromotionService/determinedPromotion',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.branchPromotoinDetail.promotionReviewStatusId = response.data.returnValue;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

        }]);