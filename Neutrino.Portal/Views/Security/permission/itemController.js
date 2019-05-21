console.log("security item.permissionController")

angular.module("neutrinoProject").register.controller('item.permissionController',
    ['$scope', '$location', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $location, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.viewModel = {
                roleId: null,
                urls: []
            }
            $scope.roleName = '';
            $scope.appMenu = [{ "id": "0", "text": "نوترینو", "parent": "#", "state": { "opened": true, "disabled": false } }];
            $scope.roles = [];
            $scope.treeConfig = {
                core: { check_callback: true, worker: true },
                types: { default: { icon: 'fa fa-sitemap' } },
                plugins: ['types', 'checkbox']
            };

            $scope.initializeController = function () {
                $scope.title = 'تعیین سطوح دسترسی';

                getRoles();
                $timeout(function () {
                    getAppMenu();
                }, 500);

            }


            $scope.role_changed = function () {
                $scope.treeInstance.jstree(true).deselect_all(true);
                getRolePermission();
            }
            $scope.submit = function () {
                var selected_nodes = $scope.treeInstance.jstree(true).get_selected(true);
                $scope.viewModel.urls = [];
                selected_nodes.forEach(menuNode => {
                    if (menuNode.original.extraData != null) {
                        $scope.viewModel.urls.push(menuNode.original.extraData);
                    }

                });
                ajaxService.ajaxPost($scope.viewModel, "api/permissionService/addOrModifyPermission",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.cancel = function () {
                $location.url('security/permission/index');
            }

            var getRoles = function () {
                ajaxService.ajaxCall({ roleId: $scope.viewModel.roleId }, "api/accountService/getRoles", 'get',
                    function (response) {
                        $scope.roles = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
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
            var getRolePermission = function () {
                ajaxService.ajaxCall({ roleId: $scope.viewModel.roleId }, "api/permissionService/getRolePermission", 'get',
                    function (response) {
                        $scope.viewModel = response.data;
                        $scope.treeInstance.jstree(true).get_selected(true);
                        $scope.viewModel.urls.forEach(htmlUrl => {
                            var findItems = $scope.appMenu.filter((mni) => mni.extraData == htmlUrl);
                            if (findItems.length == 1) {
                                $scope.treeInstance.jstree(true).select_node(findItems[0].id);
                            }
                        });
                        //$scope.treeInstance.jstree(true).close_all();
                        //$scope.treeInstance.jstree(true).open_node(0);

                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

        }]);