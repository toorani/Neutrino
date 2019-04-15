console.log("fulfillmentPromotionCondition itemController")

angular.module("neutrinoProject").register.controller('fulfillmentPromotionCondition.itemController',
['$scope', '$filter', 'ajaxService', 'alertService', 'modalService', '$timeout',
function ($scope, $filter, ajaxService, alertService, modalService, $timeout) {

    "use strict";
    $scope.totalFulfillPromotions = [];

    $scope.initializeController = function () {
        $scope.title = 'درصد تحقق و پورسانت';
        loadInitalData();
    }

    $scope.submit = function () {
        ajaxService.ajaxPost($scope.totalFulfillPromotions, "api/fulfillmentPromotionConditionService/submitData",
            function (response) {
                alertService.showSuccess(response.data.returnMessage);
            }, function (response) {
                alertService.showError(response);
            });
    }

    $scope.new_totalFulfillPromotion = function () {
        $scope.totalFulfillPromotions.push({ id: 0 });
    }
    $scope.totalFulfillPromotionDelete = function (index) {
        var modalOptions = {
            actionButtonText: 'حذف',
            headerText: $scope.title
        };
        modalService.showModal({}, modalOptions)
           .then(function (result) {
               if (result == 'ok') {
                   let entityDeleting = $scope.totalFulfillPromotions[index];

                   if (entityDeleting.id != 0) {
                       ajaxService.ajaxPost(entityDeleting, "api/fulfillmentPromotionConditionService/delete",
                           function (response) {
                               alertService.showSuccess(response.data.actionResult.returnMessage);
                               $timeout(function () {
                                   $scope.totalFulfillPromotions.splice(index, 1);
                               })
                           }, function (response) {
                               alertService.showError(response);
                           });
                   }
                   else {
                       $timeout(function () {
                           $scope.totalFulfillPromotions.splice(index, 1);
                       })
                   }
               }// end of if
           });
    }
    var loadInitalData = function () {
        ajaxService.ajaxCall({}, "api/fulfillmentPromotionConditionService/getFulfillPromotions", 'get',
        function (response) {
            $scope.totalFulfillPromotions = response.data;

            if ($scope.totalFulfillPromotions.length == 0) {
                $scope.new_totalFulfillPromotion();
            }
        },
        function (response) {
            alertService.showError(response);
        });
    }
}]);