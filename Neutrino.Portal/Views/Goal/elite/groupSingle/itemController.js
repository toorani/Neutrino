console.log("Goal/elite/groupSingle/item")

angular.module("neutrinoProject").register.controller('groupSingle.itemController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$uibModal', '$ocLazyLoad',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService, $uibModal, $ocLazyLoad) {

    "use strict";
    $scope.goal = {
        id: 0,
        goalGoodsCategoryId: null,
        goalGoodsCategoryTypeId: 1,
        goalTypeId: 1,
        startDate: null,
        endDate: null,
        isUsed: false,
        goalSteps: [],
        computingTypeId: null,
        approvePromotionTypeId : 1
    }

    $scope.activeTabIndex = 0;
    $scope.goodsCategories = [];
    $scope.goodsCategoryTypes = [];
    $scope.computingTypes = [];
    $scope.rewardComputingTypes = [];
    $scope.rewardTypes = [];
    $scope.otherRewardTypes = [];
    $scope.condemnationTypes = [];
    $scope.approvePromotionType = [];

    $scope.initializeController = function () {
        $scope.title = 'هدف گذاری گروهی/ تکی';
        getGoalGoodsCategories();
        getGoalGoodsCategoryTypes();
        getComputingTypes();
        getrewardComputingTypes();  
        getRewardTypes();
        getOtherRewardTypes();
        getCondemnationType();
        getApprovePromotionType();


        $scope.goal.id = ($routeParams.id || 0);

        if ($scope.goal.id != 0) {
            loadGoal();
        }
    }

    var getGoalGoodsCategories = function () {
        return ajaxService.ajaxCall({ goodsCategoryTypeId: $scope.goal.goalGoodsCategoryTypeId, isActive: true, iGoalTypeId: 1 }
            , "api/goalGoodsCategoryService/getDataList", 'get',
            function (response) {
                $scope.goalGoodsCategories = response.data;
            },
            function (response) {
                $scope.goodsCategories = [];
                alertService.showError(response);
                //alertService.RenderErrorMessage(response.data.ReturnMessage);
            });
    }

    var getGoalGoodsCategoryTypes = function () {
        $scope.goalGoodsCategoryTypes = [
            { id: 1, description: 'گروهی' },
            { id: 2, description: 'تکی' }]
    }

    var getComputingTypes = function () {
        $scope.computingTypes = [
            { id: 1, name: 'تعداد' },
            { id: 2, name: 'مبلغ' },
            { id: 3, name: 'درصد' }
        ];
    }
    var getrewardComputingTypes = function () {
        $scope.rewardComputingTypes = [
            { id: 1, name: 'تعداد' },
            { id: 2, name: 'مبلغ' },
        ];
    }

    var getApprovePromotionType = function () {
        $scope.approvePromotionType = [
            { id: 1, name: 'مرکز' },
            { id: 2, name: 'عوامل فروش' }
        ];
    }

    var getRewardTypes = function () {
        ajaxService.ajaxCall(new Object(), "api/RewardTypeService/getValues", 'get',
            function (response) {
                $scope.rewardTypes = [];
                $scope.rewardTypes = response.data;
            },
            function (response) {
                alertService.showError(response);
            });
    }

    var getOtherRewardTypes = function () {
        ajaxService.ajaxCall(new Object(), "api/OtherRewardTypeService/getValues", 'get',
            function (response) {
                $scope.otherRewardTypes = [];
                $scope.otherRewardTypes = response.data;
            },
            function (response) {
                alertService.showError(response);
            });
    }

    var getCondemnationType = function () {
        ajaxService.ajaxCall(new Object(), "api/CondemnationTypeService/getValues", 'get',
           function (response) {
               $scope.condemnationTypes = [];
               $scope.condemnationTypes = response.data;
           },
           function (response) {
               alertService.showError(response);
           });
    }

    var loadGoal = function () {
        ajaxService.ajaxCall({ id: $scope.goal.id }, "api/goalService/getDataItem", 'get',
        function (response) {
            $scope.goal = response.data;

            $scope.goal.goalSteps.forEach(function (goalStep) {
                var rewardInfo = {};
                if (goalStep.rewardInfo != null) {
                    rewardInfo.id = goalStep.rewardInfo.id;
                    if (goalStep.rewardInfo.amount != null && goalStep.rewardInfo.amount != 0) {
                        rewardInfo[goalStep.rewardTypeId + '_amount'] = goalStep.rewardInfo.amount;
                    }
                    else {
                        rewardInfo[goalStep.rewardTypeId + '_amount'] = null;
                    }

                    if (goalStep.rewardInfo.computingTypeId != null && goalStep.rewardInfo.computingTypeId != 0) {
                        rewardInfo[goalStep.rewardTypeId + '_computingTypeId'] = goalStep.rewardInfo.computingTypeId;
                    }
                    else {
                        rewardInfo[goalStep.rewardTypeId + '_computingTypeId'] = null;
                    }

                    if (goalStep.rewardInfo.eachValue != null && goalStep.rewardInfo.eachValue != 0) {
                        rewardInfo[goalStep.rewardTypeId + '_eachValue'] = goalStep.rewardInfo.eachValue;
                    } else {
                        rewardInfo[goalStep.rewardTypeId + '_eachValue'] = null;
                    }

                    if (goalStep.rewardInfo.choiceValueId != null && goalStep.rewardInfo.choiceValueId != 0) {
                        rewardInfo[goalStep.rewardTypeId + '_choiceValueId'] = goalStep.rewardInfo.choiceValueId;
                    } else {
                        rewardInfo[goalStep.rewardTypeId + '_choiceValueId'] = null;
                    }
                }
                goalStep.rewardInfo = rewardInfo;

                var condemnationInfo = {};
                if (goalStep.condemnationInfo != null) {
                    condemnationInfo.id = goalStep.condemnationInfo.id;
                    if (goalStep.condemnationInfo.amount != null && goalStep.condemnationInfo.amount != 0) {
                        condemnationInfo[goalStep.condemnationTypeId + '_amount'] = goalStep.condemnationInfo.amount;
                    }
                    else {
                        condemnationInfo[goalStep.condemnationTypeId + '_amount'] = null;
                    }

                    if (goalStep.condemnationInfo.computingTypeId != null && goalStep.condemnationInfo.computingTypeId != 0) {
                        condemnationInfo[goalStep.condemnationTypeId + '_computingTypeId'] = goalStep.condemnationInfo.computingTypeId;
                    }
                    else {
                        condemnationInfo[goalStep.condemnationTypeId + '_computingTypeId'] = null;
                    }

                    if (goalStep.condemnationInfo.eachValue != null && goalStep.condemnationInfo.eachValue != 0) {
                        condemnationInfo[goalStep.condemnationTypeId + '_eachValue'] = goalStep.condemnationInfo.eachValue;
                    } else {
                        condemnationInfo[goalStep.condemnationTypeId + '_eachValue'] = null;
                    }

                    if (goalStep.condemnationInfo.choiceValueId != null && goalStep.condemnationInfo.choiceValueId != 0) {
                        condemnationInfo[goalStep.condemnationTypeId + '_choiceValueId'] = goalStep.condemnationInfo.choiceValueId;
                    } else {
                        condemnationInfo[goalStep.condemnationTypeId + '_choiceValueId'] = null;
                    }

                }
                goalStep.condemnationInfo = condemnationInfo;
            });
            if ($scope.goal.isUsed == false) {
                new_goalSteps();
            }
            getGoalGoodsCategories();
        },
        function (response) {
            alertService.showError(response);
        });
    }

    $scope.onGoalGoodsCategoryTypeChanged = function () {
        $scope.goal.goalGoodsCategoryId = null;
        getGoalGoodsCategories();
    }

    $scope.addGoalGoodsCategory = function () {
        var goalGoodsCategoryTemplateUrl = "";
        var goalGoodsCategoryControllerUrl = "";
        if ($scope.goal.goalGoodsCategoryTypeId == 1) { // group
            $location.url('goalGoodsCategory/group/item');
        }
        else { // single
            $location.url('goalGoodsCategory/Single/item');
        }

    }

    $scope.submit = function () {

        if ($scope.goal.id == 0) {
            ajaxService.ajaxPost($scope.goal, "api/goalService/add",
           function (response) {
               alertService.showSuccess(response.data.actionResult.returnMessage);
               $scope.goal.id = response.data.id;
               $location.path('goal/elite/groupSingle/item/' + $scope.goal.id).replace();
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
        $location.url('goal/elite/groupSingle/index');
    }
    $scope.onDeleteGoal = function () {
        if ($scope.goal.id != 0) {

            var goal = {};
            //goal.;
            goal = goalDataGathering();
            goal.goalSteps = []
            var bodyText = 'مطمئن هستید ؟';
            var goodsCatTitle = ' هدف ' + $filter('getById')($scope.goalGoodsCategories, $scope.goal.goalGoodsCategoryId).name;
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
                                $location.url('goal/elite/groupSingle/index');
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
  
    $scope.quantityCondition_onclick = function (quantityConditionTypeId) {
        ajaxService.ajaxCall({ goalId: $scope.goal.id }, "api/quantityConditionService/getQuantityConditionType", 'get',
           function (response) {
               if (response.data != null && response.data != quantityConditionTypeId) {
                   let quantityConditionType = response.data == 1 ? 'هدف تعدادی مشروط' : 'هدف تعدادی';
                   let msg = 'برای هدف انتخاب شده ' + quantityConditionType + ' تعریف شده است و امکان ورود اطلاعات جدید وجود ندارد ';
                   alertService.showError(msg)
               }
               else {
                   $location.url('quantityConditions/index/' + $scope.goal.id + '-' + quantityConditionTypeId);
               }
           },
           function (response) {
               alertService.showError(response);
           });
    }

    $scope.branchBenefit_onlick = function () {
        $location.url('branchGoal/item/' + $scope.goal.id);

       
    }
    $scope.goalNonfulfillment_onlick = function () {
        $location.url('goalNonFulfillment/item/' + $scope.goal.id);
    }
    
    $scope.isShowBranchBenefit = function () {
        if ($scope.goal.goalSteps.length == 0)
            return false;
        return $scope.goal.goalSteps[0].id != 0;
    }
    $scope.isDisable = function (id, modelId) {
        return id == modelId;
    }

    $scope.goalStep_submit = function (tabIndex, goalStepId) {
        var findGoalSteps = $filter('filter')($scope.goal.goalSteps, { id: goalStepId }, true);
        if (findGoalSteps.length == 1) {
            var goalStep = findGoalSteps[0];
            if (goalStepId == 0) {
                ajaxService.ajaxPost(dataGathering(goalStep), "api/GoalStepService/add",
                    function (response) {
                        goalStep.id = response.data.id;
                        if (response.data.rewardInfo != undefined) {
                            goalStep.rewardInfo.id = response.data.rewardInfo.id;
                        }
                        if (response.data.condemnationInfo != undefined) {
                            goalStep.condemnationInfo.id = response.data.condemnationInfo.id;
                        }


                        alertService.showSuccess(response.data.actionResult.returnMessage);

                        new_goalSteps();


                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                        //alertService.RenderErrorMessage(response.data.ReturnMessage);
                    });
            } // end of if 
            else { // update

                ajaxService.ajaxPost(dataGathering(goalStep), "api/GoalStepService/edit",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);

                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
        }
        else {
            alertService.showError('اطلاعاتی یافت نشد');
        }

    }
    $scope.goalStepDelete = function (goalStepId, tabIndex) {
        if (goalStepId != 0) {
            var findGoalSteps = $filter('filter')($scope.goal.goalSteps, { id: goalStepId }, true);
            if (findGoalSteps.length == 1) {
                var goalStep = findGoalSteps[0];

                var modalOptions = {
                    actionButtonText: 'حذف پله هدف',
                    headerText: 'پله هدف گذاری'
                };

                modalService.showModal({}, modalOptions)
                    .then(function (result) {
                        if (result == 'ok') {
                            ajaxService.ajaxPost(dataGathering(goalStep), "api/GoalStepService/delete",
                                function (response) {
                                    alertService.showSuccess(response.data.actionResult.returnMessage);

                                    $timeout(function () {
                                        $scope.goal.goalSteps.splice(tabIndex, 1);
                                        if (tabIndex != 0) {
                                            $scope.activeTabIndex = tabIndex - 1;
                                        }
                                        else {
                                            new_goalSteps();
                                        }

                                    })

                                }, function (response) {
                                    alertService.showError(response);
                                    alertService.setValidationErrors($scope, response.data.validationErrors);
                                });
                        }// end of if
                    });
            }
        }
    }


    var goalDataGathering = function () {
        var result = {};
        result.id = $scope.goal.id;
        //result.generalAmount = $scope.goal.generalAmount;
        //result.receiptAmount = $scope.goal.receiptAmount;
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

    var dataGathering = function (goalStep) {
        var result = {};

        result.id = goalStep.id;
        result.goalId = $scope.goal.id;
        result.computingTypeId = goalStep.computingTypeId;
        result.computingValue = goalStep.computingValue;
        result.goalTypeId = 1; //Distributor

        if (JSON.stringify(goalStep.rewardInfo) !== JSON.stringify({})) {
            result.rewardInfo = {};

            if (goalStep.rewardInfo.id != undefined) {
                result.rewardInfo.id = goalStep.rewardInfo.id;
                //result.rewardInfoId = goalStep.rewardInfoId;

            }
            setRewardInfo(result, goalStep);
        }

        if (JSON.stringify(goalStep.condemnationInfo) !== JSON.stringify({}) && goalStep.condemnationInfo != null) {
            result.condemnationInfo = {};

            if (goalStep.condemnationInfo.id != undefined) {
                result.condemnationInfo.id = goalStep.condemnationInfo.id;
                //result.condemnationInfoId = goalStep.condemnationInfoId;
            }

            setCondemnationInfo(result, goalStep);
        }


        if (goalStep.rewardTypeId != null) {
            result.rewardTypeId = goalStep.rewardTypeId;
        }
        if (goalStep.condemnationTypeId != null) {
            result.condemnationTypeId = goalStep.condemnationTypeId;
        }
        return result;
    }

    var setRewardInfo = function (resultData, goalStep) {

        if (goalStep.rewardInfo[goalStep.rewardTypeId + '_amount'] != undefined) {
            resultData.rewardInfo.amount = goalStep.rewardInfo[goalStep.rewardTypeId + '_amount'];
        }

        if (goalStep.rewardInfo[goalStep.rewardTypeId + '_computingTypeId'] != undefined) {
            resultData.rewardInfo.computingTypeId = goalStep.rewardInfo[goalStep.rewardTypeId + '_computingTypeId'];
        }

        if (goalStep.rewardInfo[goalStep.rewardTypeId + '_eachValue'] != undefined) {
            resultData.rewardInfo.eachValue = goalStep.rewardInfo[goalStep.rewardTypeId + '_eachValue'];
        }

        if (goalStep.rewardInfo[goalStep.rewardTypeId + '_choiceValueId'] != undefined) {
            resultData.rewardInfo.choiceValueId = goalStep.rewardInfo[goalStep.rewardTypeId + '_choiceValueId'];
        }

    }

    var setCondemnationInfo = function (resultData, goalStep) {

        if (goalStep.condemnationInfo[goalStep.condemnationTypeId + '_amount'] != undefined) {
            resultData.condemnationInfo.amount = goalStep.condemnationInfo[goalStep.condemnationTypeId + '_amount'];
        }

        if (goalStep.condemnationInfo[goalStep.condemnationTypeId + '_computingTypeId'] != undefined) {
            resultData.condemnationInfo.computingTypeId = goalStep.condemnationInfo[goalStep.condemnationTypeId + '_computingTypeId'];
        }

        if (goalStep.condemnationInfo[goalStep.condemnationTypeId + '_eachValue'] != undefined) {
            resultData.condemnationInfo.eachValue = goalStep.condemnationInfo[goalStep.condemnationTypeId + '_eachValue'];
        }


        if (goalStep.condemnationInfo[goalStep.condemnationTypeId + '_choiceValueId'] != undefined) {
            resultData.condemnationInfo.choiceValueId = goalStep.condemnationInfo[goalStep.condemnationTypeId + '_choiceValueId'];
        }

    }

    var new_goalSteps = function () {

        $scope.goal.goalSteps.push({
            id: 0,
            computingTypeId: $scope.goal.computingTypeId,
            goalTypeId: 1, //Distributor
            computingValue: null,
            rewardTypeId: null,
            condemnationTypeId: null,
            rewardInfo: {},
            condemnationInfo: {}
        });

        $timeout(function () {
            $scope.activeTabIndex = $scope.goal.goalSteps.length - 1;
        })
        //window.scrollTo(0, angular.element('goalStepTabs').offsetTop)
    }

}]);