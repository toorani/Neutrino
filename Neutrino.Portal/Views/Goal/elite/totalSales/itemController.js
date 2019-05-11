console.log("Goal/elite / totalSales / itemController")

angular.module("neutrinoProject").register.controller('totalSales.itemController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$uibModal', 'persianCalendar',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService, $uibModal, persianCalendar) {

    "use strict";
    $scope.goal = {
        id: 0,
        goalGoodsCategoryId: null,
        goalGoodsCategoryTypeId: 3, // هدف کل
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

    $scope.initializeController = function () {
        $scope.title = 'تعریف هدف کل';

        var perDate = new persianDate();
        $scope.goal.year = perDate.year();

        $scope.goal.id = ($routeParams.id || 0);

        if ($scope.goal.id != 0) {
            loadGoal();
        }
    }

    $scope.submit = function () {

        if ($scope.goal.id == 0) {
            ajaxService.ajaxPost($scope.goal, "api/goalService/add",
           function (response) {
               alertService.showSuccess(response.data.actionResult.returnMessage);
               $scope.goal.id = response.data.id;
               $location.path('goal/elite/totalSales/item/' + $scope.goal.id).replace();
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
        $location.url('goal/elite/totalSales/index');
    }
    $scope.onDeleteGoal = function () {
        if ($scope.goal.id != 0) {

            var goal = {};
            //goal.;
            goal = goalDataGathering();
            goal.goalSteps = []
            var bodyText = 'مطمئن هستید ؟';
            var goodsCatTitle = 'هدف کل'
            bodyText = ' در صورت حذف در محدوده <strong>' + goal.startDate + '</strong> ';
            if (goal.endDate != undefined) {
                bodyText += 'تا <strong>' + goal.endDate + '</strong>'
            }

            bodyText += 'هدفگذاری برای دسته دارویی <strong>' + goodsCatTitle + '</strong> وجود نخواهد داشت'
            + '</br>'
            + 'آیا برای حذف مطمئن هستید ؟';


            var modalOptions = {
                actionButtonText: 'حذف هدف گذاری',
                headerText: 'هدف گذاری',
                bodyText: bodyText
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        ajaxService.ajaxPost(goal, "api/goalService/delete",
                            function (response) {
                                var totalFulfillPromotion = {
                                    month: $scope.goal.month,
                                    year: $scope.goal.year,
                                };
                                ajaxService.ajaxPost(totalFulfillPromotion, "api/fulfillmentPromotionConditionService/delete",
                                function (response) {
                                    alertService.showSuccess(response.data.returnMessage);
                                    $location.url('goal/elite/totalSales/index');

                                }, function (response) {
                                    alertService.showError(response);
                                });
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
    $scope.quantityCondition_onlick = function () {
        $location.url('quantityConditions/index/' + $scope.goal.id);
    }
    $scope.branchGoal_onlick = function () {
        $location.url('branchGoal/item/' + $scope.goal.id);
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
    var goalDataGathering = function () {
        var result = {};
        result.id = $scope.goal.id;
        result.goalTypeId = 1; // distributor
        if ($scope.goal.goalGoodsCategoryId != null) {
            result.goalGoodsCategoryId = $scope.goal.goalGoodsCategoryId;
        }

        if ($scope.goal.goalGoodsCategoryTypeId != null) {
            result.goalGoodsCategoryTypeId = $scope.goal.goalGoodsCategoryTypeId;
        }

        if ($scope.goal.startDate != null) {
            result.startDate = $scope.goal.startDate;
        }

        if ($scope.goal.endDate != '' && $scope.goal.endDate != undefined) {
            result.endDate = $scope.goal.endDate;
        }
        //result.goalSteps = [];
        result.computingTypeId = $scope.goal.computingTypeId;
        return result;
    }

}]);