console.log("security item.actionController")

angular.module("neutrinoProject").register.controller('item.actionController',
    ['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.viewModel = {
                htmlUrl: null,
                actions: []
            }

            $scope.appMenu = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true } }];
            $scope.allAppActions = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true} }];
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
                $scope.searchAction = '';
                $scope.searchAction_keyup();

                $scope.treeInstance_action.jstree(true).close_all();
                $scope.treeInstance_action.jstree(true).open_node(0);
                $scope.treeInstance_action.jstree(true).deselect_all(true);

                if ($scope.viewModel.htmlUrl !== null) {
                    ajaxService.ajaxCall({ htmlUrl: $scope.viewModel.htmlUrl }, "api/appActionService/getActionsByUrl", 'get',
                        function (response) {
                            response.data.actions.forEach(selAct => {
                                var findItems = $scope.allAppActions.filter((act) => act.text == selAct);
                                if (findItems.length == 1) {
                                    $scope.viewModel.actions.push(findItems[0]);
                                    $scope.treeInstance_action.jstree(true).select_node(findItems[0].id);
                                }
                            });

                        },
                        function (response) {
                            alertService.showError(response);
                        });
                }
               
            };
            $scope.selectActionNode = function (node, selected, event) {

            };

            $scope.submit = function () {
                let selected_actions = $scope.treeInstance_action.jstree(true).get_selected(true);
                $scope.viewModel.actions = [];
                selected_actions.filter((act) => act.parent != 0).forEach(act => {
                    $scope.viewModel.actions.push(act.text);
                })
                ajaxService.ajaxPost($scope.viewModel, "api/appActionService/addOrModify",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            $scope.cancel = function () {
                $location.url('security/action/index');
            }

        }]);