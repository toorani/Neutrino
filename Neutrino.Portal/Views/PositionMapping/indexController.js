console.log("positionMapping indexController")

angular.module("neutrinoProject").register.controller('positionMapping.indexController',
    ['$scope', 'alertService', 'ajaxService',
        function ($scope, alertService, ajaxService) {

            "use strict";
            $scope.viewModel = {
                selectedElitePoistions: [],
                positionTypeId: null,
                elitePoistions: []
            }

            $scope.positionTypes = [];

            $scope.initializeController = function () {
                $scope.title = 'معادل سازی پست های سازمانی';
                getPositionTypes();
            }
            $scope.positionType_changed = function () {
                ajaxService.ajaxCall({ positionTypeId: $scope.viewModel.positionTypeId }, "api/positionTypeService/getPositionMapping", 'get',
                    function (response) {
                        $scope.viewModel = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.submit = function () {
                ajaxService.ajaxPost($scope.viewModel, '/api/positionTypeService/addOrEdit',
                    function (response) {
                        alertService.showSuccess(response.data.actionResult.returnMessage);
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

            var getPositionTypes = function () {
                ajaxService.ajaxCall({}, "api/positionTypeService/getValues", 'get',
                    function (response) {
                        $scope.positionTypes = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            var getElitePositions = function () {
                ajaxService.ajaxCall({}, "api/positionTypeService/getElitePositions", 'get',
                    function (response) {
                        $scope.elitePoistions = response.data;
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }

        }]);
