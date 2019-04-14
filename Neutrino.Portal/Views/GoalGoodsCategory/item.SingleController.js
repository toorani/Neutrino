console.log("goalGoodsCategory.ItemSingleController ")

angular.module("neutrinoProject").register.controller('goalGoodsCategory.itemSingleController'
, ['$scope', '$location', '$window', '$http', '$filter', 'ajaxService', 'alertService',
function ($scope, $location, $window, $http, $filter, ajaxService, alertService) {
    "use strict";

    $scope.viewModel = {
        id: 0,
        goodsSelected: null,
        companySelected: null
    }

    $scope.goodsCollection = [];
    $scope.companies = [];

    $scope.initializeController = function () {
        $scope.title = 'دسته بندی تکی دارو';
        getCompanies();
    }

    $scope.submit = function () {

        var vModelPost = dataGathering();

        ajaxService.ajaxPost(vModelPost, "api/goalGoodsCategoryService/add",
           function (response) {
               alertService.showSuccess(response.data.actionResult.returnMessage);
               vModelPost.id = response.data.id;
               $scope.$close(vModelPost);
           },
           function (response) {
               alertService.showError(response);
           });
    }
    $scope.onCompanySelected = function ($item) {
        $scope.viewModel.goodsSelected = null;
        $scope.goodsCollection = [];
        if ($item != null) {
            return ajaxService.ajaxCall({ companyId: $item.id }
                , 'api/GoodsService/getCompanyGoods', 'get',

                function (response) {
                    $scope.goodsCollection = response.data;
                }, function (response) {
                    alertService.showError(response);
                });
        }
    }
    $scope.cancel = function () {
        $window.history.back();
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
    var dataGathering = function () {
        var result = {};
        result.name = $scope.viewModel.goodsSelected.faName;
        result.goodsSelected = [];
        result.goodsSelected.push($scope.viewModel.goodsSelected.id);
        result.goalGoodsCategoryTypeId = 2; // single
        result.goalTypeId = 1; //Distributor
        result.companySelected = [];
        result.companySelected.push($scope.viewModel.companySelected.id);
        return result;
    }
}]);


