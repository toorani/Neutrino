console.log("security item.actionController")

angular.module("neutrinoProject").register.controller('item.actionController',
    ['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.viewModel = {
                htmlUrl: null,
                appActions: []
            }

            $scope.appMenu = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true, "disabled": false } }];

            $scope.treeConfig = {
                core: {
                    multiple: false,
                    animation: true,
                    error: function (error) {
                        $log.error('treeCtrl: error from js tree - ' + angular.toJson(error));
                    },
                    check_callback: true,
                    worker: true
                }
            };
            

            $scope.initializeController = function () {
                $scope.title = 'تعریف فعالیت';
                $scope.viewModel.id = ($routeParams.id || 0);
                $timeout(function () {
                    getAppMenu();
                }, 500);
                if ($scope.viewModel.id != 0) {
                    loadEntity();
                }
                else {
                    getTreeActionsList();
                }

            }

            var getAppMenu = function () {
                ajaxService.ajaxCall({}, "api/appMenuService/getTreeAppMenu", 'get',
                    function (response) {
                        response.data.forEach(men => {
                            $scope.appMenu.push(men)
                        });
                    },
                    function (response) {
                        alertService.showError(response);
                    })

            }
            $scope.selectNodeCB = function (node, selected, event) {
                $scope.viewModel.htmlUrl = selected.node.original.extraData;
                $scope.viewModel.appActions = [];
            };
            var getTreeActionsList = function () {

                //return ajaxService.ajaxCall({ selectedId: ($scope.viewModel.parentId || 1) }, "api/appActionService/getTreeActionsList", 'get',
                //    function (response) {
                //        $scope.treeModel = response.data;
                //    },
                //    function (response) {
                //        alertService.showError(response);
                //        alertService.setValidationErrors($scope, response.data.validationErrors);
                //    });

                ajaxService.ajaxCall({}, "api/appActionService/getAllAction", 'get',
                    function (response) {
                        $scope.allAppActions = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            var loadEntity = function () {
                ajaxService.ajaxCall({ id: $scope.viewModel.id }, "api/appActionService/getDataItem", 'get',
                    function (response) {
                        $scope.viewModel = response.data;
                        getTreeActionsList();
                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }

            $scope.submit = function () {
                if ($scope.viewModel.id == 0) {
                    ajaxService.ajaxPost($scope.viewModel, "api/appActionService/add",
                        function (response) {
                            alertService.showSuccess(response.data.actionResult.returnMessage);
                            $scope.viewModel.id = response.data.id;
                        },
                        function (response) {
                            alertService.showError(response);
                            alertService.setValidationErrors($scope, response.data.validationErrors);
                        });
                    getTreeActionsList();
                }
                else {
                    ajaxService.ajaxPost($scope.viewModel, "api/appActionService/edit",
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
                $location.url('security/action/index');
            }

        }]);