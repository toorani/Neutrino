console.log("Goal/elite/groupSingle/item")

angular.module("neutrinoProject").register.controller('receipt.itemController',
['$scope', '$location', '$filter', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$uibModal', 'persianCalendar',
function ($scope, $location, $filter, $routeParams, ajaxService, modalService, alertService, $uibModal, persianCalendar) {

    "use strict";
    $scope.goal = {
        id: 0,
        goalGoodsCategoryTypeId: 4, //هدف وصول
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

    var receiptTypeParam;
    $scope.initializeController = function () {
        $scope.title = 'تعریف هدف وصول';

        var perDate = new persianDate();
        $scope.goal.year = perDate.year();

        $scope.goal.id = ($routeParams.id || 0);
        receiptTypeParam = ($routeParams.param || '');

        if (receiptTypeParam == 'total') {
            $scope.goal.goalGoodsCategoryTypeId = 4;
            arrangeByGoalGoodsCategoryType($scope.goal.goalGoodsCategoryTypeId);
        }
        else if (receiptTypeParam == 'prgv') {
            $scope.goal.goalGoodsCategoryTypeId = 5;
            arrangeByGoalGoodsCategoryType($scope.goal.goalGoodsCategoryTypeId);
        }

        if ($scope.goal.id != 0) {
            loadGoal();
        }


    }

    var loadGoal = function () {
        ajaxService.ajaxCall({ id: $scope.goal.id }, "api/goalService/getDataItem", 'get',
        function (response) {
            $scope.goal = response.data;
            arrangeByGoalGoodsCategoryType($scope.goal.goalGoodsCategoryTypeId);
            if ($scope.goal.goalGoodsCategoryTypeId == 4)
                receiptTypeParam = 'total';
            else
                receiptTypeParam = 'prgv';

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
               $location.path('goal/elite/receipt/item/' + $scope.goal.id).replace();
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
        $location.url('goal/elite/receipt/index/' + receiptTypeParam);
    }
    $scope.onDeleteGoal = function () {
        if ($scope.goal.id != 0) {

            var goal = {};
            //goal.;
            goal = $scope;
            goal.goalSteps = []
            var bodyText = 'مطمئن هستید ؟';
            var goodsCatTitle = $filter('getById')($scope.goalGoodsCategoryTypes, 3).name; // generalGoal
            if (goal.goalGoodsCategoryTypeId != 3) // generalGoal
                goodsCatTitle = $filter('getById')($scope.goalGoodsCategories, goal.goalGoodsCategoryId).name;
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

                        $filter('filter')($scope.goal.goalSteps, { goalId: $scope.goal.id }, true).forEach(function (step) {
                            goal.goalSteps.push(dataGathering(step));
                        });
                        ajaxService.ajaxPost(goal, "api/goalService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $location.url('goal/distributor/index');
                            },
                            function (response) {
                                alertService.showError(response.data.returnMessage);
                                alertService.setValidationErrors($scope, response.data.validationErrors);
                            });
                    }// end of if
                }, function () {
                    // Cancel
                });


        }
    }

    $scope.branchGoal_onlick = function () {
        $location.url('branchGoal/receipt/item/' + $scope.goal.id);
    }
    $scope.branchReceiptGoalPercent_onlick = function () {
        $location.url('branchReceiptGoalPercent/item/' + $scope.goal.id);
    }
    var arrangeByGoalGoodsCategoryType = function (goalGoodsCategoryTypeId) {
        if (goalGoodsCategoryTypeId == 4) {
            $scope.title = 'هدف گذاری وصول کل';
            $scope.receiptTypes = [{ id: 4, title: 'وصول کل' }]
        }
        else if (goalGoodsCategoryTypeId == 5 || goalGoodsCategoryTypeId == 6) {
            $scope.title = 'هدف گذاری وصول خصوصی/دولتی';
            $scope.receiptTypes = [{ id: 5, title: 'وصول خصوصی' }, { id: 6, title: 'وصول دولتی' }]
        }
    }

}]);