console.log("Goal/elite/groupSingle/index")

angular.module("neutrinoProject").register.controller('groupSingle.indexController',
    ['$scope', '$location', '$compile', 'ajaxService', 'dataTableColumns'
        , function ($scope, $location, $compile, ajaxService, dataTableColumns) {

            "use strict";

            $scope.initializeController = function () {
                $scope.title = 'هدف گذاری گروهی/ تکی';
                dataTableColumns.initialize();

                dataTableColumns.add({ mappingData: 'startDate', title: 'تاریخ شروع' });
                dataTableColumns.add({ mappingData: 'endDate', title: 'تاریخ پایان' });

                $scope.dataColumns = dataTableColumns.getItems();
                $scope.serverUrl = '/api/GoalService/getGroupByStartEndDateGoalDataGrid';

            }

            $scope.goalsExpanded = function (dataTable, rowSelected) {
                var aData = dataTable.fnGetData(rowSelected);
                ajaxService.ajaxCall({ startDate: aData.startDate, endDate: aData.endDate, isUsed: aData.isUsed }, "api/GoalService/getGroupSingle", 'get',
                    function (response) {
                        dataTable.fnOpen(rowSelected, $scope.fnFormatDetails(response.data), 'details');
                    },
                    function (response) {
                        alertService.showError(response);
                    });
            }
            $scope.fnFormatDetails = function (data) {
                var sOut = '<table class="table-bordered table-striped" cellspacing="0" style="width:100%">';
                sOut += "<thead><tr >"
                sOut += "<th class='align-center'>#</th>"
                sOut += "<th class='align-center'>نوع هدف</th>"
                sOut += "<th class='align-center'>نام هدف</th>"
                sOut += "<th class='align-center'>نحوه محاسبه</th>"
                sOut += "<th></th>"
                sOut += "</tr></thead>"
                let counter = 1;
                data.forEach(function (item) {
                    sOut += '<tr class="align-center">'
                        + '<td>' + counter + '</td>'
                        + '<td>' + item.goalGoodsCategoryType.description + '</td>'
                        + '<td>' + item.goalGoodsCategory.name + '</td>'
                        + '<td>' + item.computingTypeTitle + '</td>'
                        + '<td><button type="button" ng-click="viewEntity('
                        + item.id
                        + ')" class="btn ls-light-blue-btn"><i class="fa fa-magic"></i><span>جزییات</span></button></td>'
                        + '</tr>';
                    counter++;
                });

                sOut += '</table>';

                return $compile(sOut)($scope);
            }
            $scope.goalTabs = [{
                title: 'اهداف محاسبه نشده', filter: {
                    goalType: 'distributor',
                    isUsed: 'false'
                }
            }, {
                title: 'اهداف محاسبه شده', filter: {
                    goalType: 'distributor',
                    isUsed: 'true'
                }
            }]
            $scope.activeTab = 0;
            $scope.tabSelected = function (idx) {
                $scope.activeTab = idx;
            }
            $scope.newGoal = function () {
                $location.url('goal/elite/groupSingle/item')
            }
            $scope.viewEntity = function (goalId) {
                $location.url('goal/elite/groupSingle/item/' + goalId);
            }
        }]);
