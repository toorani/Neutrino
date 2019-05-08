console.log("index")

angular.module("neutrinoProject").register.controller('indexController',
    ['$scope', function ($scope) {

        "use strict";

        $scope.initializeController = function () {
            $scope.title = 'صفحه اول';
            //ajaxService.ajaxCall({ startDate: '1397/10/01', endDate: '1397/10/01', goalGoodsCategoryId: 5094 }
            //    , "api/promotionReportService/getBranchSaleGoals1"
            //    ,'get',
            //    function (response) {
            //        $scope.rpt = response.data;
            //    },
            //    function (response) {
            //    });


        }

    }]);

