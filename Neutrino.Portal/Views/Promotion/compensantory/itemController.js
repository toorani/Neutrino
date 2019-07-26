console.log("promotion.compensantory.itemController")

angular.module("neutrinoProject").register.controller('promotion.compensantory.itemController',
    ['$scope', '$location', 'alertService', 'ajaxService', '$routeParams',
        function ($scope, $location, alertService, ajaxService, $routeParams) {

            "use strict";
            let promotionId = 0;
            $scope.branchPromotionStatusId = 7;

            $scope.initializeController = function () {
                $scope.title = 'پورسانت ترمیمی';
                promotionId = ($routeParams.id || 0);
                if (promotionId != 0) {
                    getBranchPromotions();
                }
            }

            var getBranchPromotions = function () {
                ajaxService.ajaxCall({ promotionId: promotionId }, '/api/branchPromotionService/getDataList', 'get',
                    function (response) {
                        $scope.branchPromotions = response.data;
                        $scope.branchPromotionStatusId = $scope.branchPromotions[0].promotionReviewStatusId;
                            
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {

                ajaxService.ajaxPost($scope.branchPromotions, "api/branchPromotionService/addOrModify",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });


            }

            $scope.confirmPromotion = function () {
                ajaxService.ajaxPost($scope.branchPromotions, "api/branchPromotionService/confirmCompensatory",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.branchPromotionStatusId = response.data.resultValue;
                    },
                    function (response) {
                        alertService.showError(response);
                    });


            }

            $scope.cancel = function () {
                $location.url('promotion/index/');
            }
        }]);
