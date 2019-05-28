console.log("promotion.branchShare.ceo.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.ceo.indexController',
    ['$scope', 'alertService', 'ajaxService', '$location',
        function ($scope, alertService, ajaxService, $location) {

            "use strict";

            $scope.branchPromotions = [];

            $scope.initializeController = function () {
                $scope.title = ' بررسی و اعلام نظر پورسانت پرسنل مراکز';
                getBranchPromotions();
                
            }
            $scope.auditBranchMemebrPromotion = function (dataSelected) {
                $location.url('promotion/branchShare/ceo/item/' + dataSelected.branchId);
            }

            
            var getBranchPromotions = function () {
                ajaxService.ajaxCall({}, "api/promotionService/getBranchPromotionsForCEOReview", 'get',
                    function (response) {
                        $scope.branchPromotions = response.data;
                    },
                    function (response) {
                        $scope.branchPromotions = [];
                        alertService.showError(response);
                    });
            }
        }]);
