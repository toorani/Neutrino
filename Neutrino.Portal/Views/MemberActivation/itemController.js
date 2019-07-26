console.log("memberActivation itemController")

angular.module("neutrinoProject").register.controller('memberActivation.itemController',
    ['$scope', '$filter', 'ajaxService', 'alertService',
        function ($scope, $filter, ajaxService, alertService) {

            "use strict";
            $scope.viewModel = {
                branchSelected: null,
            }
            $scope.memberSearch = '';
            $scope.members = [];


            $scope.initializeController = function () {
                $scope.title = 'فعال /غیر فعال سازی پرسنل';
                getBranches();
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
            $scope.memberFilter = function (record) {
                return ($scope.memberSearch == '' || String(record.code).indexOf($scope.memberSearch) != -1
                    || String(record.fullName).indexOf($scope.memberSearch) != -1 || String(record.positionTitle).indexOf($scope.memberSearch) != -1
                    || String(record.positionTitle).indexOf($scope.memberSearch) != -1);
            }
            $scope.onchange_branch = function () {
                if ($scope.viewModel.branchSelected != null) {
                    $scope.members = [];

                    ajaxService.ajaxCall({ branchId: $scope.viewModel.branchSelected.id }, "api/memberService/getMembersByBranchId", 'get',
                        function (response) {
                            $scope.members = response.data;
                            if ($scope.members == 0) {
                                alertService.showWarning('پرسنلی برای مرکز انتخاب شده تعریف نشده است');
                            }
                        },
                        function (response) {
                            alertService.showError(response);
                        });
                }

            }
            $scope.toggleActivation = function (record) {
                ajaxService.ajaxPost(record, "api/memberService/toggleActivation",
                    function (response) {
                        record.isActive = !record.isActive;
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
        }]);