console.log("Goal/supplier/item")

angular.module("neutrinoProject").register.controller('item.supplierController',
['$scope', '$location', '$filter', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService', '$uibModal', '$ocLazyLoad', 'envService',
function ($scope, $location, $filter, $timeout, $routeParams, ajaxService, modalService, alertService, $uibModal, $ocLazyLoad, envService) {

    "use strict";
    $scope.goal = {
        id: 0,
        company: null,
        goalTypeId: 2,// supplier
        goodsSelectionList: [],
        goalGoodsCategoryTypeId: 1,
        startDate: null,
        endDate: null,
        goalSteps: []
    }

    $scope.activeTabIndex = 0;
    $scope.computingTypes = [];
    $scope.rewardTypes = [];
    $scope.otherRewardTypes = [];
    $scope.condemnationTypes = [];
    $scope.companies = [];
    $scope.goodsCollection = []
    $scope.isGeneralGoal = false;
    $scope.initializeController = function () {
        $scope.title = 'هدف گذاری تامین کننده';

        getComputingTypes();
        getRewardTypes();
        getOtherRewardTypes();
        getCondemnationType();
        getCompanies();
        $scope.goal.id = ($routeParams.id || 0);

        if ($scope.goal.id != 0) {
            loadGoal();
        }
    }

    var getComputingTypes = function () {
        $scope.computingTypes = [
            { id: 1, name: 'تعداد' },
            { id: 2, name: 'مبلغ' }
        ];
    }
    var getRewardTypes = function () {
        ajaxService.ajaxCall(new Object(), "api/RewardTypeService/getValues", 'get',
            function (response) {
                $scope.rewardTypes = [];
                $scope.rewardTypes = response.data.items;
            },
            function (response) {
                console.log(response.data.ReturnMessage);
                //alertService.RenderErrorMessage(response.data.ReturnMessage);
            });
    }
    var getOtherRewardTypes = function () {
        ajaxService.ajaxCall(new Object(), "api/OtherRewardTypeService/getValues", 'get',
            function (response) {
                $scope.otherRewardTypes = [];
                $scope.otherRewardTypes = response.data.items;
            },
            function (response) {
                console.log(response.data.ReturnMessage);
                //alertService.RenderErrorMessage(response.data.ReturnMessage);
            });
    }
    var getCondemnationType = function () {
        ajaxService.ajaxCall(new Object(), "api/CondemnationTypeService/getValues",'get',
           function (response) {
               $scope.condemnationTypes = [];
               $scope.condemnationTypes = response.data;
           },
           function (response) {
               alertService.showError(response.message);
               //alertService.RenderErrorMessage(response.data.ReturnMessage);
           });
    }
    var getCompanies = function () {
        ajaxService.ajaxCall({}, 'api/CompanyService/getCompany','get',
            function (response) {
                $scope.companies = response.data;
            },
           function (response) {
               alertService.showError(response);
               alertService.setValidationErrors($scope, response.data.validationErrors);
           });
    }
    var loadGoodsCollection = function (companyId) {
        ajaxService.ajaxCall({ companyId: companyId }, 'api/GoodsService/getCompanyGoods','get',
            function (response) {
                //
                if ($scope.goal.goodsSelectionList != undefined) {
                    $scope.isGeneralGoal = true;
                    response.data.forEach(function (goods) {
                        var findItem = $filter('getById')($scope.goal.goodsSelectionList, goods.id)
                        if (findItem == undefined) {
                            $scope.goodsCollection.push(goods);
                            $scope.isGeneralGoal = false;
                        }
                    });
                }
                else {
                    $scope.goodsCollection = response.data;
                }
            },
           function (response) {
               alertService.showError(response);
               alertService.setValidationErrors($scope, response.data.validationErrors);
           });
    }
    var loadGoal = function () {
        ajaxService.ajaxCall($scope.goal.id, "api/goalService/getDataItem",'get',
           function (response) {
               $scope.goal = response.data;
               loadGoodsCollection($scope.goal.company.id);

               $scope.goal.goalSteps.forEach(function (goalStep) {
                   var rewardInfo = {};
                   if (goalStep.rewardInfo != null) {
                       rewardInfo.id = goalStep.rewardInfo.id;
                       if (goalStep.rewardInfo.amount != null && goalStep.rewardInfo.amount != 0) {
                           rewardInfo[goalStep.rewardTypeId + '_amount'] = parseFloat(goalStep.rewardInfo.amount);
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
                   goalStep.rewardItems = rewardInfo;

                   var condemnationInfo = {};
                   if (goalStep.condemnationInfo != null) {
                       condemnationInfo.id = goalStep.condemnationInfo.id;
                       if (goalStep.condemnationInfo.amount != null && goalStep.condemnationInfo.amount != 0) {
                           condemnationInfo[goalStep.condemnationTypeId + '_amount'] = parseFloat(goalStep.condemnationInfo.amount);
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
                   goalStep.condemnationItems = condemnationInfo;
               });

               new_goalSteps();
           },
           function (response) {
               alertService.showError(response);
               alertService.setValidationErrors($scope, response.data.validationErrors);
           });
    }

    $scope.getTitleComputingValue = function (computingTypeId) {
        return ($filter('getById')($scope.computingTypes, computingTypeId)).name + '  پس از افزایش ';
    }
    $scope.calculateComputingValue = function (step) {
        if (step.incrementPercent != undefined && step.rawComputingValue != undefined) {
            step.computingValue = step.rawComputingValue + (step.rawComputingValue * step.incrementPercent / 100)
        }
        else {
            step.computingValue = null;
        }
    }
    $scope.onCompanySelected = function (item) {
        $scope.goal.goodsSelectionList = undefined;
        $scope.goodsCollection = [];
        $scope.isGeneralGoal = false;
        if (item != null) {
            loadGoodsCollection(item.id);
        }

    }
    $scope.onSelectAllChecked = function () {
        if ($scope.isGeneralGoal) {
            $scope.goal.goodsSelectionList = $scope.goodsCollection;
        }
    }
    $scope.submit = function () {
        if ($scope.goal.id == 0) {
            ajaxService.ajaxPost($scope.goal, "api/goalService/add",
           function (response) {
               alertService.showSuccess(response.data.actionResult.returnMessage);
               $scope.goal.id = response.data.id;
               new_goalSteps();
           },
           function (response) {
               alertService.showError(response.data.returnMessage);
               alertService.setValidationErrors($scope, response.data.validationErrors);
           });
        }
        else {
            ajaxService.ajaxPost($scope.goal, "api/goalService/edit",
          function (response) {
              alertService.showSuccess(response.data.returnMessage);
          },
          function (response) {
              alertService.showError(response.data.returnMessage);
              alertService.setValidationErrors($scope, response.data.validationErrors);
          });
        }
    }
    $scope.cancel = function () {
        $location.url('goal/supplier/index');
    }
    $scope.delete_goal = function () {
        if ($scope.goal.id != 0) {

            //var goal = {};
            //goal.goalSteps = [];
            //goal = goalDataGathering();
            var bodyText = 'مطمئن هستید ؟';
            if ($scope.goal.endDate != undefined && $scope.goal.endDate != "") {
                bodyText = ' در صورت حذف  هدفگذاری در محدوده <strong>' + $scope.goal.startDate + '</strong> تا <strong>' + $scope.goal.endDate + '</strong>'
                + 'اطلاعات هدفگذاری برای شرکت <strong>' + $scope.goal.company.name + ' </strong>  وجود نخواهد داشت '
                + '</br>'
                + 'آیا برای حذف مطمئن هستید ؟';
            }
            else {
                bodyText = ' در صورت حذف  هدفگذاری از <strong>' + $scope.goal.startDate + '</strong> '
                + 'اطلاعات هدفگذاری برای شرکت <strong>' + $scope.goal.company.name + ' </strong>  وجود نخواهد داشت '
                + '</br>'
                + 'آیا برای حذف مطمئن هستید ؟';
            }

            var modalOptions = {
                actionButtonText: 'حذف هدف گذاری',
                headerText: 'هدف گذاری',
                bodyText: bodyText
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    if (result == 'ok') {
                        //$filter('filter')($scope.goal.goalSteps, { goalId: $scope.goal.id }, true).forEach(function (step) {
                        //    goal.goalSteps.push(dataGathering(step));
                        //});
                        ajaxService.ajaxPost($scope.goal, "api/goalService/delete",
                            function (response) {
                                alertService.showSuccess(response.data.actionResult.returnMessage);
                                $location.url('goal/supplier/index');
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
    $scope.isDisable = function (id, modelId) {
        return id == modelId;
    }
    $scope.goalStep_submit = function (tabIndex, goalStepId) {
        var findGoalSteps = $filter('filter')($scope.goal.goalSteps, { id: goalStepId }, true);
        if (findGoalSteps.length == 1) {
            var goalStep = findGoalSteps[0];
            if (goalStep.rewardItems != undefined && JSON.stringify(goalStep.rewardItems) !== JSON.stringify({})) {
                setRewardInfo(goalStep);
            }
            if (goalStep.condemnationItems != undefined && JSON.stringify(goalStep.condemnationItems) !== JSON.stringify({})) {
                setCondemnationInfo(goalStep);
            }
            envService.suitablePostingModel(goalStep);
            if (goalStepId == 0) {

                ajaxService.ajaxPost(goalStep, "api/GoalStepService/add",
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
                        alertService.showError(response.data.actionResult.returnMessage);
                        alertService.setValidationErrors($scope, response.data.actionResult.validationErrors);
                        //alertService.RenderErrorMessage(response.data.ReturnMessage);
                    });
            } // end of if 
            else { // update

                ajaxService.ajaxPost(goalStep, "api/GoalStepService/edit",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);

                    },
                    function (response) {
                        alertService.showError(response.data.actionResult.returnMessage);
                        alertService.setValidationErrors($scope, response.data.actionResult.validationErrors);
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
                                    alertService.showError(response.data.actionResult.returnMessage);
                                    alertService.setValidationErrors($scope, response.data.actionResult.validationErrors);
                                });
                        }// end of if
                    });
            }
        }
    }

    var setRewardInfo = function (goalStep) {
        if (goalStep.rewardInfo == undefined)
        {
            goalStep.rewardInfo = {};
        }
        goalStep.rewardInfo.itemTypeId = goalStep.rewardTypeId;
        if (goalStep.rewardItems[goalStep.rewardTypeId + '_amount'] != undefined) {
            goalStep.rewardInfo.amount = goalStep.rewardItems[goalStep.rewardTypeId + '_amount'];
        }

        if (goalStep.rewardItems[goalStep.rewardTypeId + '_computingTypeId'] != undefined) {
            goalStep.rewardInfo.computingTypeId = goalStep.rewardItems[goalStep.rewardTypeId + '_computingTypeId'];
        }

        if (goalStep.rewardItems[goalStep.rewardTypeId + '_eachValue'] != undefined) {
            goalStep.rewardInfo.eachValue = goalStep.rewardItems[goalStep.rewardTypeId + '_eachValue'];
        }

        if (goalStep.rewardItems[goalStep.rewardTypeId + '_choiceValueId'] != undefined) {
            goalStep.rewardInfo.choiceValueId = goalStep.rewardItems[goalStep.rewardTypeId + '_choiceValueId'];
        }

    }
    var setCondemnationInfo = function (goalStep) {

        if (goalStep.condemnationInfo == undefined) {
            goalStep.condemnationInfo = {};
        }
        goalStep.condemnationInfo.itemTypeId = goalStep.rewardTypeId;
        if (goalStep.condemnationItems[goalStep.condemnationTypeId + '_amount'] != undefined) {
            goalStep.condemnationInfo.amount = goalStep.condemnationItems[goalStep.condemnationTypeId + '_amount'];
        }

        if (goalStep.condemnationItems[goalStep.condemnationTypeId + '_computingTypeId'] != undefined) {
            goalStep.condemnationInfo.computingTypeId = goalStep.condemnationItems[goalStep.condemnationTypeId + '_computingTypeId'];
        }

        if (goalStep.condemnationItems[goalStep.condemnationTypeId + '_eachValue'] != undefined) {
            goalStep.condemnationInfo.eachValue = goalStep.condemnationItems[goalStep.condemnationTypeId + '_eachValue'];
        }

        if (goalStep.condemnationItems[goalStep.condemnationTypeId + '_choiceValueId'] != undefined) {
            goalStep.condemnationInfo.choiceValueId = goalStep.condemnationItems[goalStep.condemnationTypeId + '_choiceValueId'];
        }

    }
    var new_goalSteps = function () {
        if ($scope.goal.goalSteps === undefined)
            $scope.goal.goalSteps = [];

        $scope.goal.goalSteps.push({
            id: 0,
            goalId: $scope.goal.id,
            goalTypeId: 2,
            computingTypeId: 1,
            computingValue: null,
            rawComputingValue: null,
            incrementPercent: null,
            rewardTypeId: null,
            condemnationTypeId: null,
            rewardInfo: {},
            condemnationInfo: {}
        });

        $timeout(function () {
            $scope.activeTabIndex = $scope.goal.goalSteps.length - 1;
        })
        window.scrollTo(0, angular.element('goalStepTabs').offsetTop)
    }
}]);


