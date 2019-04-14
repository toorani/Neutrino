console.log("Goal/elite/aggregation/item")

angular.module("neutrinoProject").register.controller('aggregation.itemController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$uibModal', 'persianCalendar',
function ($scope, $location, $filter, $routeParams, ajaxService, modalService, alertService, $uibModal, persianCalendar) {

    "use strict";
    $scope.goal = {
        id: 0,
        goalGoodsCategoryTypeId: 7, //هدف تجمیعی
        goalTypeId: 1,
        isUsed: false,
        goalSteps: [],
        computingTypeId: 2,
        amount: null,
        year: null,
        month: null
    }

    $scope.years = persianCalendar.getYears();
    $scope.months = persianCalendar.getMonthNames();
    $scope.aggregationValue = 0;
    $scope.initializeController = function () {
        $scope.title = 'تعریف هدف تجمیعی';

        var perDate = new persianDate();
        $scope.goal.year = perDate.year();

        $scope.goal.id = ($routeParams.id || 0);

        if ($scope.goal.id != 0) {
            loadGoal();
        }
    }

    var loadGoal = function () {
        ajaxService.ajaxCall({ id: $scope.goal.id }, "api/goalService/getDataItem", 'get',
        function (response) {
            $scope.goal = response.data;
        },
        function (response) {
            alertService.showError(response);
        });
    }

    $scope.submit = function () {
        if ($scope.goal.id == 0) {
            ajaxService.ajaxPost($scope.goal, "api/goalService/add",
           function (response) {
               alertService.showSuccess(response.data.actionResult.returnMessage);
               $scope.goal.id = response.data.id;
               $location.path('goal/elite/aggregation/item/' + $scope.goal.id).replace();
               loadGoal();
           },
           function (response) {
               alertService.showError(response);
               alertService.setValidationErrors($scope, response.data.validationErrors);
           });
        }
        else {
            ajaxService.ajaxPost($scope.goal, "api/goalService/edit",
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
        $location.url('goal/elite/aggregation/index/');
    }
    $scope.onDeleteGoal = function () {
        if ($scope.goal.id != 0) {

           
           
            var bodyText = 'آیا برای حذف مطمئن هستید ؟'

            var modalOptions = {
                actionButtonText: 'حذف هدف',
                headerText: 'هدف تجمیعی',
                bodyText: bodyText
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost($scope.goal, "api/goalService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $location.url('goal/elite/aggregation/index/');
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
    $scope.getAggregationValue = function () {
        $scope.aggregationValue = 'در حال بارگذاری ...';
        if ($scope.goal.year != null && $scope.goal.month != null) {
            ajaxService.ajaxCall({ month: $scope.goal.month, year: $scope.goal.year }, "api/goalService/getPreviousAggregationValue", 'get',
                function (response) {
                    $scope.aggregationValue = response.data;
                }, function (response) {
                    $scope.aggregationValue = 0;
                    alertService.showError(response);
                }, '');
        }

    }
    $scope.branchGoal_onlick = function () {
        $location.url('branchGoal/receipt/item/' + $scope.goal.id);
    }

}]);