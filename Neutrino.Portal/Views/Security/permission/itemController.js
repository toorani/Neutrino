console.log("security item.permissionController")

angular.module("neutrinoProject").register.controller('item.permissionController',
    ['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.viewModel = {
                roleId: 0,
                actions: []
            }
            $scope.roleName = '';
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
                },
                types: { default: { icon: 'fa fa-sitemap' } },
                plugins: ['types', 'checkbox']
            };

            $scope.initializeController = function () {
                $scope.title = 'تعیین سطوح دسترسی';
                $scope.viewModel.roleId = $routeParams.id;
                getRoleInfo();
                $timeout(function () {
                    getAppMenu();
                }, 500);

            }

            
            var getRoleInfo = function () {
                ajaxService.ajaxCall({ roleId: $scope.viewModel.roleId }, "api/accountService/getRoleInfo", 'get',
                    function (response) {
                        $scope.roleName = response.data.faName;
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
            
            var loadEntity = function () {
                ajaxService.ajaxPost($scope.viewModel.id, "api/appActionService/getDataItem",
                    function (response) {
                        $scope.viewModel = response.data;
                    },
                    function (response) {
                        alertService.showError(response.data.transactionalData.returnMessage);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }

            $scope.submit = function () {
                var selected_nodes = $scope.treeInstance.jstree(true).get_selected();
                ajaxService.ajaxPost($scope.viewModel, "api/permissionService/addPermission",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response.data.returnMessage);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
            $scope.cancel = function () {
                $location.url('security/permission/index');
            }
            


        }]);