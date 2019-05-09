console.log("account/registration/item")

angular.module("neutrinoProject").register.controller('item.registrationController',
    ['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'alertService', '$timeout',
        function ($scope, $location, $filter, $routeParams, ajaxService, alertService, $timeout) {

            "use strict";
            $scope.user = {
                id: 0,
                userName: null,
                password: null,
                confirmPassword: null,
                name: null,
                lastName: null,
                email: null,
                roleId: null,
                branchesUnderControl: []
            }

            $scope.roleSelected = null;
            $scope.selectAll = false;
            $scope.branchChecked = []

            $scope.initializeController = function () {
                $scope.title = 'تعریف کاربر ';
                $scope.user.id = ($routeParams.id || 0);
                getRoles();
                getBranches();
                if ($scope.user.id != 0) {
                    loadEntity();
                }
               
            }
            $scope.submit = function () {
                $scope.user.branchesUnderControl = [];

                for (let branchId = $scope.branchChecked.length - $scope.branches.length; branchId < $scope.branchChecked.length; branchId++) {
                    if ($scope.branchChecked[branchId])
                        $scope.user.branchesUnderControl.push(branchId);
                }

                ajaxService.ajaxPost($scope.user, "api/accountService/registerOrModify",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                        if ($scope.user.id == 0) {
                            $scope.user.id = response.data.id;
                            $location.path("account/registration/item/" + $scope.user.id,false);
                        }
                        
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.delete = function () {
                if ($scope.user.id != 0) {

                    var modalOptions = {
                        actionButtonText: 'حذف کاربری',
                        headerText: 'حذف کاربر',
                    };

                    modalService.showModal({}, modalOptions)
                        .then(function (result) {
                            if (result == 'ok') {
                                ajaxService.ajaxPost($scope.user, "api/accountService/delete",
                                    function (response) {
                                        alertService.showSuccess(response.data.actionResult.returnMessage);
                                        $location.url('orgStructure/index');
                                    },
                                    function (response) {
                                        alertService.showError(response);
                                        alertService.setValidationErrors($scope, response.data.validationErrors);
                                    });
                            }// end of if
                        }, function () {
                            // Cancel
                        });

                }
            }
            $scope.cancel = function () {
                $location.url('account/registration/index');
            }

            var getBranches = function () {
                return ajaxService.ajaxCall({}, "api/branchService/getBranches", 'get',
                    function (response) {
                        $scope.branches = response.data;
                        setCheckedDefaluValue();
                    },
                    function (response) {
                        $scope.branches = [];
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
            var setCheckedDefaluValue = function () {
                $scope.branchChecked = [];
                $scope.branches.forEach(function (bra) {
                    $scope.branchChecked[bra.id] = false;
                });
            }
            $scope.onclick_selectAll = function () {
                $timeout(function () {
                    $scope.branches.forEach(function (bra) {
                        $scope.branchChecked[bra.id] = $scope.selectAll;
                    });

                }, 100);
            };
            var getRoles = function () {
                ajaxService.ajaxCall(new Object(), "api/accountService/getRoles", 'get',
                    function (response) {
                        $scope.roles = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            var loadEntity = function () {
                ajaxService.ajaxCall({ id: $scope.user.id }, "api/accountService/getDataItem", 'get',
                    function (response) {
                        $scope.user = response.data;
                        $scope.selectAll = false;
                        //setCheckedDefaluValue();
                        $timeout(function () {
                            $scope.user.branchesUnderControl.forEach(function (braId) {
                                $scope.branchChecked[braId] = true;
                            });
                        }, 100);
                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
        }]);