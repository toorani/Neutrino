console.log("promotion.branchShare.ceo.itemController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.ceo.itemController',
    ['$scope', '$location', '$routeParams', 'ajaxService', 'alertService',
        function ($scope, $location, $routeParams, ajaxService, alertService) {

            "use strict";

            let branchId = 0;
            $scope.memberPenalties = [];
            $scope.initializeController = function () {
                $scope.title = ' بررسی پورسانت پرسنل مرکز ';

                branchId = ($routeParams.id || 0);
                getBranchPromotionDetail();
                getData();
            }

            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({ branchId: branchId }, "api/branchPromotionService/getBranchPromotionReleasedStep1", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;

                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            var getData = function () {
                ajaxService.ajaxCall({ branchId: branchId }, "api/penaltyService/getPenaltiesForPromotion", 'get',
                    function (response) {
                        $scope.memberPenalties = response.data;
                    },
                    function (response) {
                        $scope.memberPenalties = [];
                        alertService.showError(response);
                    });
            }

            $scope.getTotalCEOPromotion = function () {
                let totalCEOPormotion = 0;
                $scope.memberPenalties.forEach(record => {
                    totalCEOPormotion += record.managerPromotion - record.deduction + record.credit;
                });
                return totalCEOPormotion;
                
            }

            $scope.getTotalManagerPromotion = function () {
                let totalManagerPormotion = 0;
                $scope.memberPenalties.forEach(record => {
                    totalManagerPormotion += record.managerPromotion;
                });
                return totalManagerPormotion;
            }

         

            $scope.releaseCEO = function () {
                ajaxService.ajaxPost($scope.memberPenalties, '/api/penaltyService/releaseCEOPromotion',
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.branchPromotoinDetail.promotionReviewStatusId = response.data.returnValue;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {
                ajaxService.ajaxPost($scope.memberPenalties, "api/penaltyService/addOrModify",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.memberPenalties.forEach(vm => {
                            let returnValue = response.data.returnValue.filter(x => x.memberId == vm.memberId)[0];
                            vm.id = returnValue.id;
                        });
                    },
                    function (response) {
                        alertService.showError(response);
                    });


            }
            $scope.cancel = function () {
                $location.url('promotion/branchShare/ceo/index');
            }



        }]);