console.log("promotion.branchShare.manager.indexController")

angular.module("neutrinoProject").register.controller('promotion.branchShare.manager.indexController',
    ['$scope', 'alertService', 'ajaxService', 'persianCalendar',
        function ($scope, alertService, ajaxService, persianCalendar) {

            "use strict";
            $scope.viewModel = {
                year: null,
                month: null
            }

            $scope.years = persianCalendar.getYears();
            $scope.months = persianCalendar.getMonthNames();

            $scope.initializeController = function () {
                $scope.title = 'تقسیم پورسانت';
            }
            $scope.getData = function () {

            }

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
            var getBranches = function () {
                $scope.branches = [];
                return ajaxService.ajaxCall({}, "api/branchService/getBranches", 'get',
                    function (response) {
                        $scope.branches = response.data;
                    },
                    function (response) {
                        $scope.branches = [];
                        alertService.showError(response);
                    });
            }
        }]);
