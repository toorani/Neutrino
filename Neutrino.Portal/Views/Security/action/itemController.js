console.log("security item.actionController")

angular.module("neutrinoProject").register.controller('item.actionController',
    ['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.viewModel = {
                htmlUrl: null,
                actions: []
            }

            $scope.appMenu = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true, "disabled": false } }];
            $scope.allAppActions = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true, "disabled": false } }];
            $scope.treeConfig_menu = {
                core: { check_callback: true, worker: true }
            };
            $scope.treeConfig_action = {
                core: { check_callback: true, worker: true },
                plugins: ['checkbox', "search"]
            };

            $scope.searchAction = '';

            $scope.initializeController = function () {
                $scope.title = 'تعریف فعالیت';
                $scope.viewModel.id = ($routeParams.id || 0);
                $timeout(function () {
                    getAppMenu();
                }, 500);
                $timeout(function () {
                    getTreeActions();
                }, 500);

                //if ($scope.viewModel.id != 0) {
                //    loadEntity();
                //}
                //else {
                //    getTreeActionsList();
                //}

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

            var getTreeActions = function () {
                ajaxService.ajaxCall({}, "api/appActionService/getAllAction", 'get',
                    function (response) {
                        response.data.forEach(act => {
                            $scope.allAppActions.push(act)
                        });

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

            $scope.searchAction_keyup = function () {
                var to = false;
                if (to) {
                    clearTimeout(to);
                }
                to = setTimeout(function () {
                    if ($scope.treeInstance_action) {
                        $scope.treeInstance_action.jstree(true).search($scope.searchAction);
                    }
                }, 250);
            }

            $scope.selectMenuNode = function (node, selected, event) {
                $scope.viewModel.htmlUrl = selected.node.original.extraData;
                $scope.viewModel.actions = [];
            };
            $scope.selectActionNode = function (node, selected, event) {

            };

            $scope.submit = function () {
                let selected_actions = $scope.treeInstance_action.jstree(true).get_selected(true);
                $scope.viewModel.appActions = [];
                selected_actions.filter((act) => act.parent != 0).forEach(act => {
                    $scope.viewModel.actions.push(act.text);
                })

                
                //ajaxService.ajaxPost($scope.viewModel, "api/appActionService/add",
                //    function (response) {
                //        alertService.showSuccess(response.data.actionResult.returnMessage);
                //        $scope.viewModel.id = response.data.id;
                //    },
                //    function (response) {
                //        alertService.showError(response);
                //        alertService.setValidationErrors($scope, response.data.validationErrors);
                //    });
            }

            $scope.cancel = function () {
                $location.url('security/action/index');
            }

        }]);