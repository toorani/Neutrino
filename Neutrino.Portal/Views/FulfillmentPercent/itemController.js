console.log("fulfillmentPercent itemController")

angular.module("neutrinoProject").register.controller('fulfillmentPercent.itemController',
    ['$scope', '$filter', 'ajaxService', '$location', 'alertService', 'modalService', 'persianCalendar',
        function ($scope, $filter, ajaxService, $location, alertService, modalService, persianCalendar) {

            "use strict";
            $scope.viewModel = {
                items: []
            }
            $scope.years = persianCalendar.getYears();
            $scope.months = persianCalendar.getMonthNames();
            $scope.year = null;
            $scope.month = null;
            $scope.isUsed = false;

            $scope.initializeController = function () {
                $scope.title = 'ضریب تحقق پورسانت ';
            }

            $scope.submit = function () {
                ajaxService.ajaxPost($scope.viewModel.items, "api/fulfillmentPercentService/addFulfillment",
                    function (response) {
                        alertService.showSuccess(response.data.returnMessage);
                        $scope.viewModel.items = response.data.items;
                        //loadData(goalId);
                    },
                    function (response) {

                        alertService.showError(response);
                        alertService.setValidationErrors($scope, response.data.validationErrors);
                    });
            }

            $scope.loadData = function () {
                let postModel = { year: $scope.year, month: $scope.month };

                ajaxService.ajaxCall(postModel, "api/fulfillmentPercentService/getFulfillmentList", "get",
                    function (response) {
                        $scope.viewModel.items = response.data;
                        $scope.isUsed = $scope.viewModel.items[0].isUsed;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.btnApprove_onclick = function () {
                var modalOptions = {
                    actionButtonText: 'تایید ومحاسبه پورسانت',
                    headerText: $scope.title,
                    bodyText: 'آیا مطمئن هستید؟'
                };

                modalService.showModal({}, modalOptions)
                    .then(function (result) {
                        if (result == 'ok') {
                            ajaxService.ajaxPost({ year: $scope.year, month: $scope.month }, "api/fulfillmentPercentService/approveFulfillment",
                                function (response) {
                                    alertService.showSuccess(response.data.returnMessage);
                                    $scope.isUsed = true;
                                },
                                function (response) {
                                    alertService.showError(response);
                                });
                        }// end of if
                    }, function () {
                        // Cancel
                    });


            }
        }]);