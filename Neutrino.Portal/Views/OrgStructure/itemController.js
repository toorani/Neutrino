console.log("OrgStructure itemController")

angular.module("neutrinoProject").register.controller('orgStructure.itemController',
['$scope', '$location', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$timeout',
function ($scope, $location, $routeParams, ajaxService, modalService, alertService,  $timeout) {

    "use strict";
    $scope.viewModel = {
        positionTypeId: null,
        branches: []
        
    }
    $scope.selectAll = false;
    $scope.branchChecked = []

    $scope.initializeController = function () {
        $scope.title = 'تعریف ساختار سازمانی';
        getPositionTypes();
        getBranches();
        if ($routeParams.id) {
            $scope.editMode = true;
            $scope.viewModel.positionTypeId = $routeParams.id;
            loadEntity();
        }
    }

    $scope.submit = function () {
        
        $scope.viewModel.branches = [];
        
        for (let branchId = $scope.branchChecked.length - $scope.branches.length; branchId < $scope.branchChecked.length; branchId++) {
            if ($scope.branchChecked[branchId])
                $scope.viewModel.branches.push(branchId);
        }
        ajaxService.ajaxPost($scope.viewModel, "api/orgStructureService/addOrEdit",
            function (response) {
                alertService.showSuccess(response.data.actionResult.returnMessage);
                $scope.viewModel.id = response.data.id;

            },
            function (response) {
                alertService.showError(response);
            });
    }
    $scope.cancel = function () {
        $location.url('orgStructure/index');
    }
    $scope.delete = function () {
        if ($scope.viewModel.id != 0) {

            var modalOptions = {
                actionButtonText: 'حذف ساختار سازمانی',
                headerText: 'تعریف ساختار سازمانی',
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost($scope.viewModel, "api/orgStructureService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.returnMessage);
                                $location.url('orgStructure/index');
                            },
                            function (response) {
                                alertService.showError(response);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });

        }
    }

    $scope.onclick_selectAll = function () {
        $timeout(function () {
            $scope.branches.forEach(function (bra) {
                $scope.branchChecked[bra.id] = $scope.selectAll;
            });

        }, 100);
    };
    $scope.onchange_positionTypeId = function () {
        if ($scope.viewModel.positionTypeId != null) {
            loadEntity();
        }
        else {
            $timeout(function () {
                $scope.viewModel.branches.forEach(function (braId) {
                    $scope.branchChecked[braId] = false;
                });
            }, 100);
        }

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
    var getPositionTypes = function () {
        $scope.positionTypes = [];
        ajaxService.ajaxCall({}, "api/positionTypeService/getValues", 'get',
            function (response) {
                $scope.positionTypes = response.data;
            },
            function (response) {
                alertService.showError(response);
            });
    }
    var loadEntity = function () {

        ajaxService.ajaxCall({ positionTypeId: $scope.viewModel.positionTypeId }, "api/orgStructureService/getDataItem", 'get',
          function (response) {
              $scope.viewModel = response.data;
              $scope.viewModel.positionTypeId = response.data.id;
              $scope.selectAll = false;
              setCheckedDefaluValue();
              $timeout(function () {
                  $scope.viewModel.branches.forEach(function (braId) {
                      $scope.branchChecked[braId] = true;
                  });
              }, 100);



          },
          function (response) {
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
}]);