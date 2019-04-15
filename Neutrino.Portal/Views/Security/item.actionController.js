console.log("security item.actionController")

angular.module("neutrinoProject").register.controller('item.actionController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService) {

    "use strict";
    $scope.viewModel = {
        id: 0,
        htmlUrl: '',
        actionUrl: '',
        title: '',
        faTitle: '',
        parentId: null,
        actionTypeId:null
    }

    $scope.appActionTypes = [
        { id: 1, title: ' ایجاد اطلاعات',enTitle:'Create' },
        { id: 2, title: 'دیدن اطلاعات', enTitle: 'Read' },
        { id: 3, title: 'ویرایش اطلاعات', enTitle: 'Update' },
        { id: 4, title: 'حذف اطلاعات' ,enTitle:'Delete'}
    ];

    

    $scope.initializeController = function () {
        $scope.title = 'تعریف فعالیت';
        $scope.viewModel.id = ($routeParams.id || 0);

        if ($scope.viewModel.id != 0) {
            loadEntity();
        }
        else {
            getTreeActionsList();
        }
        
    }

    var getTreeActionsList = function () {

        return ajaxService.ajaxCall({ selectedId: ($scope.viewModel.parentId || 1) }, "api/appActionService/getTreeActionsList", 'get',
            function (response) {
                $scope.treeModel = response.data;
            },
            function (response) {
                alertService.showError(response);
                alertService.setValidationErrors($scope, response.data.validationErrors);
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
    $scope.changedActionTreeNode = function (e, data) {
        if (data.node != null) {
            $scope.viewModel.parentId = data.node.id;
            var extraData = data.node.original.extraData;
            if (extraData != null) {
                var splitData = extraData.split(',');

                $timeout(function () {
                    if ($scope.viewModel.actionUrl == null || 
                        $scope.viewModel.actionUrl == '') {
                        $scope.viewModel.actionUrl = splitData[0];
                    }

                    if ($scope.viewModel.htmlUrl == null ||
                        $scope.viewModel.htmlUrl == '') {
                        $scope.viewModel.htmlUrl = splitData[1];
                    }

                    if ($scope.viewModel.title == null ||
                        $scope.viewModel.title == '') {
                        $scope.viewModel.title = data.node.original.enName;
                    }

                    if ($scope.viewModel.faTitle == null ||
                        $scope.viewModel.faTitle == '') {
                        $scope.viewModel.faTitle = data.node.original.text;
                    }

                }, 100);
            }
        }
        
    };
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
    $scope.delete = function () {
        if ($scope.viewModel.id != 0) {

            var modalOptions = {
                actionButtonText: 'حذف فعالیت',
                headerText: 'حذف فعالیت',
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        suitableViewModel();
                        ajaxService.ajaxPost($scope.viewModel, "api/appActionService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.transactionalData.returnMessage);
                                $location.url('security/action/index');
                            },
                            function (response) {
                                alertService.showError(response.data.transactionalData.returnMessage);
                                alertService.setValidationErrors($scope, response.data.validationErrors);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });

        }
    }


}]);