console.log("promotion.branchShare.ceo.itemController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.ceo.itemController',
    ['$scope', '$location', '$routeParams', 'ajaxService', 'alertService',
        function ($scope, $location, $routeParams, ajaxService, alertService) {

            "use strict";

            let branchId = 0;
            $scope.initializeController = function () {
                $scope.title = ' بررسی پورسانت پرسنل مرکز ';

                branchId = ($routeParams.id || 0);
                getBranchPromotionDetail();
                getData();
            }

            var getBranchPromotionDetail = function () {
                ajaxService.ajaxCall({ branchId: branchId, statusId: 2 }, "api/promotionService/getLastBranchPromotion", 'get',
                    function (response) {
                        $scope.branchPromotoinDetail = response.data;
                        
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }



            var getData = function () {
                ajaxService.ajaxCall({ statusId: 2, branchId: branchId }, "api/promotionService/getMemberSharePromotion", 'get',
                    function (response) {
                        $scope.branchMemberPromotions = response.data;
                        //calculateAssigendPromotion();
                    },
                    function (response) {
                        $scope.branchMemberPromotions = [];
                        alertService.showError(response);
                    });
            }

            $scope.submit = function () {

                if ($scope.goal.id == 0) {
                    ajaxService.ajaxPost($scope.goal, "api/goalService/add",
                        function (response) {
                            alertService.showSuccess(response.data.actionResult.returnMessage);
                            $scope.goal.id = response.data.id;
                            $location.path('goal/elite/receipt/item/' + $scope.goal.id).replace();
                            loadGoal();
                        },
                        function (response) {
                            alertService.showError(response);
                            alertService.setValidationErrors($scope, response.data.validationErrors);
                        });
                }
                else {
                    ajaxService.ajaxPost($scope.goal, "api/goalService/edit",
                        function (response) {
                            alertService.showSuccess(response.data.actionResult.returnMessage);
                        },
                        function (response) {
                            alertService.showError(response);
                            alertService.setValidationErrors($scope, response.data.validationErrors);
                        });
                }


            }
            $scope.cancel = function () {
                $location.url('promotion/branchShare/ceo/index');
            }



        }]);