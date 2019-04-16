console.log("goalGoodsCategory.ItemGroupController ")

angular.module("neutrinoProject").register.controller('goalGoodsCategory.ItemGroupController',
    ['$scope', '$location', '$window', '$filter', '$timeout', 'ajaxService', 'alertService',
        function ($scope, $location, $window, $filter, $timeout, ajaxService, alertService) {
            "use strict";

            $scope.viewModel = {
                id: 0,
                name: '',
                goodsSelected: [],
                goalGoodsCategoryTypeId: 1, //Group
                goalTypeId: 1, //Distributor
                companySelected: [],
                isGeneralGoal: false,
                goalCategorySimilarId: 0
            }
            $scope.goodsSelection = {
                company: {},
                goodsList: []
            }
            $scope.treeDataSource = [];
            $scope.companies = [];
            $scope.goodsCollection = []
            $scope.goodsCategoryTypes = [];
            $scope.isSelectAllCompanyGoods = false;
            $scope.titleSelected = {
                item: {
                    id: 0,
                    name: ''
                }

            };
            var treeNodeSelected = null;
            $scope.initializeController = function () {
                $scope.title = 'دسته بندی گروهی دارو';
                getCompanies();
                getGoalGoodsCategories();
                $scope.viewModel.goodsSelected = [];
                $scope.viewModel.companySelected = [];
            }
            $scope.submit = function () {

                if (!$scope.viewModel.isGeneralGoal) {
                    dataGathering();
                    if ($scope.viewModel.goodsSelected.length == 0) {
                        alertService.showError('لطفا دارو(ها) را انتخاب کنید');
                        return;
                    }
                }
                ajaxService.ajaxPost($scope.viewModel, "api/goalGoodsCategoryService/add",
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                        $scope.viewModel.id = response.data.id;
                        $window.history.back();
                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }

            $scope.ontitleSelected_selected = function (item) {
                if (item != null && item.id != 0) {

                    ajaxService.ajaxCall({ id: item.id }, 'api/goalGoodsCategoryService/getDataItem', 'get'
                        , function (response) {
                            var goalCategory = response.data;
                            $scope.treeDataSource = [];
                            $scope.goodsCollection = []
                            goalCategory.companySelected.forEach(function (coId) {
                                let iCoId = parseInt(coId);
                                var lstCompanyGoods = $filter('filter')(goalCategory.goodsCollection, { companyId: iCoId });
                                var goodsItem = lstCompanyGoods[0];
                                var record = {};
                                record.id = iCoId;
                                record.text = goodsItem.companyName;
                                record.parent = '#';
                                record.data = {};
                                record.data.company = { faName: goodsItem.companyName, id: iCoId };
                                record.data.goodsList = lstCompanyGoods;
                                $scope.treeDataSource.push(record);

                                lstCompanyGoods.forEach(function (goods) {
                                    var record = {};
                                    record.id = goods.id;
                                    record.text = goods.faName + ' (' + goods.goodsCode + ')';
                                    record.parent = iCoId;
                                    record.data = goods;

                                    $scope.treeDataSource.push(record);
                                });
                            })

                        }
                        , function (response) {
                        })
                }
            }
            $scope.onSelectAllCompanyGoodsChecked = function () {
                if ($scope.isSelectAllCompanyGoods) {
                    $scope.goodsSelection.goodsList = $scope.goodsCollection;
                }
            }
            $scope.onSelectGeneralGoalChecked = function () {
                if ($scope.viewModel.isGeneralGoal) {

                }
            }
            $scope.onCompanySelected = function (item) {
                $scope.goodsSelection.goodsList = [];
                $scope.goodsCollection = [];
                $scope.isSelectAllCompanyGoods = false;
                if (item != null) {
                    loadGoodsCollection(item.id);
                }
            }
            $scope.onAddGoodsSelected = function () {
                //$scope.viewModel.companySelected.push($scope.goodsSelection.company.id);
                if (!isExistCompany($scope.goodsSelection.company.id)) {
                    if ($scope.goodsSelection.goodsList.length != 0) {
                        var record = {};
                        record.id = parseInt($scope.goodsSelection.company.id);
                        record.text = $scope.goodsSelection.company.faName;
                        record.parent = '#';
                        record.data = {};
                        record.data.company = $scope.goodsSelection.company;
                        record.data.goodsList = $scope.goodsSelection.goodsList;
                        $scope.treeDataSource.push(record);
                        $scope.goodsSelection.goodsList.forEach(function (goods) {
                            var record = {};
                            record.id = goods.id;
                            record.text = goods.faName + ' (' + goods.goodsCode + ')';
                            record.parent = parseInt($scope.goodsSelection.company.id);
                            record.data = goods;
                            $scope.treeDataSource.push(record);
                            //$scope.viewModel.goodsSelected.push(goods.id);
                        });
                        $scope.goodsSelection.company = null;
                        $scope.goodsSelection.goodsList = [];
                        $scope.isSelectAllCompanyGoods = false;
                    }
                    else {
                        alertService.showError('لطفا دارو(ها) را انتخاب کنید');
                    }
                }
                else {
                    alertService.showError('شرکت انتخاب شده تکراری میباشد');
                }


            }
            $scope.onChangedTreeNode = function (e, data) {
                treeNodeSelected = null;
                if (data.node != null && data.node.parent == '#') {
                    treeNodeSelected = data.node;
                }
            }
            $scope.onEditGoods = function () {
                if (treeNodeSelected != null) {
                    var nodeSelected = $filter('filter')($scope.treeDataSource, { id: treeNodeSelected.id })[0];
                    $scope.onCompanySelected(treeNodeSelected.data.company);
                    $scope.goodsSelection = {};
                    $timeout(function () {
                        $scope.goodsSelection.company = treeNodeSelected.data.company;
                        $scope.goodsSelection.goodsList = treeNodeSelected.data.goodsList;
                        var nodeIndex = $scope.treeDataSource.indexOf(nodeSelected);
                        $scope.treeDataSource.splice(nodeIndex, 1);
                        $scope.goodsSelection.goodsList.forEach(function (goods) {
                            nodeSelected = $filter('filter')($scope.treeDataSource, { id: goods.id })[0];
                            nodeIndex = $scope.treeDataSource.indexOf(nodeSelected);
                            $scope.treeDataSource.splice(nodeIndex, 1);
                        });

                        treeNodeSelected = null;
                    }, 100);
                }

            }
            $scope.onRemoveGoods = function () {
                if (treeNodeSelected != null) {
                    var nodeSelected = $filter('filter')($scope.treeDataSource, { id: parseInt(treeNodeSelected.id) })[0];
                    $timeout(function () {
                        var goodsList = treeNodeSelected.data.goodsList;
                        var nodeIndex = $scope.treeDataSource.indexOf(nodeSelected);
                        $scope.treeDataSource.splice(nodeIndex, 1);
                        goodsList.forEach(function (goods) {
                            nodeSelected = $filter('filter')($scope.treeDataSource, { id: goods.id })[0];
                            nodeIndex = $scope.treeDataSource.indexOf(nodeSelected);
                            $scope.treeDataSource.splice(nodeIndex, 1);
                        });
                        treeNodeSelected = null;
                    }, 100);

                }

            }
            $scope.cancel = function () {
                $window.history.back();
            }
            $scope.newCategoryTitle = function ($select) {
                var search = $select.search,
                    list = angular.copy($select.items),
                    FLAG = 0;

                //remove last user input
                list = list.filter(function (item) {
                    return item.id !== FLAG;
                });

                if (!search) {
                    //use the predefined list
                    $select.items = list;
                }
                else {
                    //manually add user input and set selection
                    var userInputItem = {
                        id: FLAG,
                        name: search
                    };
                    $select.items = [userInputItem].concat(list);
                    $select.selected = userInputItem;
                }
            }

            var getGoalGoodsCategories = function () {
                return ajaxService.ajaxCall({ goodsCategoryTypeId: 1, isActive: true, iGoalTypeId: 1 }
                    , "api/goalGoodsCategoryService/getDataList", 'get',
                    function (response) {
                        $scope.goalGoodsCategories = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            var getCompanies = function () {
                ajaxService.ajaxCall({}, 'api/companyService/getCompany', 'get',
                    function (response) {
                        $scope.companies = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
            var loadGoodsCollection = function (companyId) {
                ajaxService.ajaxCall({ companyId: companyId }, 'api/GoodsService/getCompanyGoods', 'get',
                    function (response) {
                        $scope.goodsCollection = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }
            var dataGathering = function () {
                $scope.viewModel.name = $scope.titleSelected.item.name;
                $scope.viewModel.goalCategorySimilarId = $scope.titleSelected.item.id;

                var goodsSelected = $scope.treeDataSource;
                $scope.viewModel.companySelected = [];
                $scope.viewModel.goodsSelected = [];

                for (var i = 0; i < goodsSelected.length; i++) {
                    var nodeSelected = goodsSelected[i];
                    if (nodeSelected.parent == "#") {
                        if ($filter('filter')($scope.viewModel.companySelected, nodeSelected.id, true).length == 0) {
                            $scope.viewModel.companySelected.push(nodeSelected.id);
                        }
                    }
                    else {
                        if ($filter('filter')($scope.viewModel.goodsSelected, nodeSelected.id, true).length == 0) {
                            $scope.viewModel.goodsSelected.push(nodeSelected.id);
                        }
                    }
                }
            }
            var isExistCompany = function (companyId) {
                $filter('filter')($scope.treeDataSource, { parent: '#' }).forEach((company) => {
                    return company.id == companyId;
                });

            }

        }]);


