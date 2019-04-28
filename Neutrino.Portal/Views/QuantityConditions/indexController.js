console.log("QuantityConditions indexController")

angular.module("neutrinoProject").register.controller('quantityConditions.indexController',
    ['$scope', '$filter', '$compile', '$location', '$timeout', '$routeParams', 'ajaxService', 'modalService', 'alertService',
        function ($scope, $filter, $compile, $location, $timeout, $routeParams, ajaxService, modalService, alertService) {

            "use strict";
            $scope.quantityCondition = {
                goalId: 0,
                goalCategoryName: '',
                quantity: 0,
                extraEncouragePercent: 0,
                notReachedPercent: 0,
                forthCasePercent: 0,
                quantityConditionTypeId: 1,
                goodsQuantityConditions: [],

            }
            $scope.branches = [];
            $scope.goodsList = [];
            $scope.searchExp = '';

            $scope.goodsSelected = null;
            $scope.selection = "all";
            $scope.isEditMode = false;
            $scope.initializeController = function () {
                $scope.title = ' تعریف هدف تعدادی مشروط';
                var goalIdAndQuntityType = ($routeParams.id || '');
                if (goalIdAndQuntityType != '') {
                    $scope.quantityCondition.goalId = goalIdAndQuntityType.substring(0, goalIdAndQuntityType.indexOf('-'));
                    $scope.quantityCondition.quantityConditionTypeId = goalIdAndQuntityType.substr(goalIdAndQuntityType.indexOf('-') + 1, 1);
                    if ($scope.quantityCondition.quantityConditionTypeId == 2) {
                        $scope.title = 'تعریف هدف تعدادی';
                    }
                    loadData();
                }

            }

            $scope.submit = function () {
                var sbmitData = Object.assign({}, $scope.quantityCondition);
                sbmitData.goodsQuantityConditions = [];
                if ($scope.goodsSelected != null)
                    sbmitData.goodsQuantityConditions.push($scope.goodsSelected);
                ajaxService.ajaxPost(sbmitData, "api/quantityConditionService/addOrUpdate",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                        $scope.quantityCondition.id = response.data.id;
                        if ($scope.goodsQuantityConditions.lenght > 0) {
                            let response_goodsQunatity = response.data.goodsQuantityConditions[0];
                            $scope.goodsQuantityConditions[0].id = response_goodsQunatity.id;
                            $scope.goodsQuantityConditions[0].branchQuantityConditions.forEach((branchQunatity) => {
                                //let response_branchQunatity = $filter('filter')(response.data.goodsQuantityConditions[0].branchQuantityConditions, { branchId: branchQunatity.branchId });
                                let response_branchQunatity = response_goodsQunatity.branchQuantityConditions.filter((branchQ) => branchQ.branchId = branchQunatity.branchId)[0];
                                if (response_branchQunatity != null) {
                                    branchQunatity.id =
                                }
                                branchQunatity.id = 
                            });
                        }

                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
            $scope.cancel = function () {
                $location.url('goal/elite/groupSingle/item/' + $scope.quantityCondition.goalId);
            }

            $scope.goodsListFilter = function (goods) {
                if ($scope.searchExp != '' || $scope.selection == "all") {
                    $scope.selection = "all"
                    return goods.faName.indexOf($scope.searchExp) != -1 || goods.code.indexOf($scope.searchExp) != -1;
                }
                else if ($scope.selection == "withQ")
                    return goods.quantity != 0;
                else if ($scope.selection == "withoutQ")
                    return goods.quantity == 0;
            }
            $scope.getGoodsQuantity = function (goods) {
                goods.quantity = 0;
                goods.branchQuantityConditions.forEach(function (branch) {
                    goods.quantity += branch.quantity;
                });
            }
            $scope.getClassName = function (goods) {
                return goods.quantity == 0 ? "panel panel-primary" : "panel panel-light-green";
            }
            $scope.getGoodsQuantityDeclarationCount = function () {
                var count = 0
                if ($scope.quantityCondition.goodsQuantityConditions != null) {
                    $scope.quantityCondition.goodsQuantityConditions.forEach(function (gdc) {
                        if (gdc.quantity != 0)
                            count++;
                    })
                }
                return count;
            }

            $scope.onclick_goodsSelected = function (goods) {
                $scope.goodsSelected = goods;
            }
            var loadData = function () {
                ajaxService.ajaxCall({ goalId: $scope.quantityCondition.goalId }, "api/quantityConditionService/getQuantityCondition", 'get',
                    function (response) {
                        if (response.data.quantityConditionTypeId == null)
                            response.data.quantityConditionTypeId = $scope.quantityCondition.quantityConditionTypeId;
                        $scope.quantityCondition = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });

            }



        }]);