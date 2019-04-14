console.log("angular.dataTabels");

angular.module('neutrinoProject')
.factory('dataTableColumns', function () {
    var columns = {};
    var items = [];
    columns.initialize = function () {
        items = new Array();
    }
    columns.add = function (values) {

        //var direction = 'center';
        var title = '';
        var mappingData = '';
        var searchable = true;
        var sortable = true;
        var isKey = false;
        var name = '';
        var className = 'align-center';
        var width = null;
        var priority = null;


        var fnRenderCallBack = function () { }
        var fnDrawCallback = function () { }

        //
        if (values.isKey != undefined) {
            isKey = values.isKey;
        }

        //if (values.direction != undefined) {
        //    direction = values.direction;
        //}

        if (values.fnRenderCallBack != undefined) {
            fnRenderCallBack = values.fnRenderCallBack;
        }
        else {
            fnRenderCallBack = null;
        }

        if (values.fnDrawCallback != undefined) {
            fnDrawCallback = values.fnDrawCallback;
        }
        else {
            fnDrawCallback = null;
        }

        if (values.className != undefined) {
            className = className + ' ' + values.className;
        }

        if (values.width != undefined) {
            width = values.width;
        }

        if (isKey == false) {

            if (values.mappingData != undefined) {
                mappingData = values.mappingData;
            }

            if (values.title != undefined) {
                title = values.title;
            }
            else {
                title = mappingData;
            }

            if (values.searchable != undefined) {
                searchable = values.searchable;
            }

            if (values.sortable != undefined) {
                sortable = values.sortable;
            }

            if (values.name != undefined) {
                name = values.name
            }
            else {
                name = mappingData;
            }

            if (values.priority != undefined) {
                priority = values.priority
            }
        }
        else {
            mappingData = "Id";
            searchable = false;
            sortable = false;

            if (values.width != undefined) {
                width = values.width;
            }
            else {
                width = '100px';
            }
        }

        items.push({ isKey, title, mappingData, searchable, sortable, fnRenderCallBack, fnDrawCallback, name, className, width, priority });
    };
    columns.getItems = function () {
        return items;
    }
    return columns;
})
.directive('esp.ng.datatable',
function ($http) {
    return {
        restrict: 'E',
        scope: {
            columns: '=columns',
            serverUrl: '@serverUrl',
            searchable: '@searchable',
            orderValue: '@orderValue',
            serverParams: '@serverParams',
            lengthChangeable: '@lengthChangeable',
            enableExpandcolumn: '@enableExpandcolumn',
            fnDrawcallback: '&',
            fnRowcallback: '&',
            fnDetailsexpanded: '&',
            byGrouping: '@byGrouping',
            groupingStartRender: '&'

        },
        templateUrl: '/views/shared/angular.dataTable.tmpl.html',
        link: function (scope, element, attrs, controller) {

            scope.$parent.dataTable = {
                startRenderArgs: {}
            }

            var fnDetailsexpanded = function (ngGrid, trSelected) { }

            //handel server params
            var setServerParams = function () {
                serverParams = '';
                var filter = null;
                if (scope.serverParams != null) {
                    if (scope.$parent[scope.serverParams] != null) {
                        filter = scope.$parent[scope.serverParams];
                        if (filter != null) {
                            var names = Object.keys(filter);
                            for (var i = 0 ; i < names.length; i++) {
                                name = names[i];
                                value = filter[name];
                                if (value != null && Object.keys(filter[name]).length != 0)
                                    //{name:'goalType', value:'supplier'}
                                    serverParams += '{name :\'' + name + '\',value:\'' + value + '\'}&';
                            }
                        }
                    }
                    else {
                        filter = new Array(scope.serverParams);

                        for (var i = 0 ; i < filter.length; i++) {
                            var element = JSON.parse(filter[i]);
                            var keys = Object.keys(JSON.parse(filter[i]));
                            for (var i = 0; i < keys.length; i++) {
                                name = keys[i];
                                value = element[name];
                                if (value != null && Object.keys(element[name]).length != 0)
                                    //{name:'goalType', value:'supplier'}
                                    serverParams += '{name :\'' + name + '\',value:\'' + value + '\'}&';
                            }

                        }
                    }
                }
            }

            var tableElement = element.find('table');
            if (tableElement != undefined) {
                var aoColumns = [];
                var keyIndex = 0;

                //create expand columns
                if (Boolean(scope.enableExpandcolumn)) {
                    scope.columns.splice(0, 0, {
                        mappingData: '',
                        fnRenderCallBack: function (data, type, full) {
                            return "<span id='btnExpand'><i  class='green fa fa-plus-circle fa-lg'></i></span>";
                        },
                        width: '50px',
                        className: 'grid-expandIcon',
                        sortable: false,
                        searchable: false

                    });
                }

                //collect columns
                for (var i = 0; i < scope.columns.length; i++) {
                    var column = scope.columns[i];
                    if (column.isKey) {
                        aoColumns.push({
                            "title": column.title,
                            "mData": column.mappingData,
                            "bSearchable": column.searchable,
                            "bSortable": column.sortable,
                            "mRender": column.fnRenderCallBack,
                            "sWidth": column.width,
                            "className": column.className
                        });
                        if (column.priority != undefined) {
                            aoColumns[i].responsivePriority = column.priority;
                        }
                        keyIndex = i;
                    }
                    else {
                        aoColumns.push({
                            "title": column.title,
                            "mData": column.mappingData,
                            "bSearchable": column.searchable,
                            "bSortable": column.sortable,
                            "mRender": column.fnRenderCallBack,
                            "className": column.className
                        });
                        if (column.width != undefined) {
                            aoColumns[i].sWidth = column.width;
                        }
                        if (column.priority != undefined) {
                            aoColumns[i].responsivePriority = column.priority;
                        }
                    }

                }

                var fnDrawCallback = scope.fnDrawcallback;


                var ajaxSourceUrl = scope.serverUrl;
                var serverParams = '';
                if (scope.serverParams != null) {
                    setServerParams();
                }
                var searchable = true;
                var lengthChangeable = true;
                if (scope.searchable != undefined) {
                    searchable = (scope.searchable == 'true');
                }
                if (scope.lengthChangeable != undefined) {
                    lengthChangeable = scope.lengthChangeable;
                }

                var orderValue = [];
                if (scope.orderValue != undefined) {
                    var orderSplit = scope.orderValue.split('&');
                    orderSplit.forEach(function (orValue) {
                        orderValue.push(eval(orValue))
                    })
                }
                else {
                    orderValue.push([keyIndex, 'asc'])
                }


                var rowGroup = {
                    enable: false
                };
                if (scope.byGrouping != undefined) {
                    rowGroup.enable = true;
                    rowGroup.dataSrc = scope.byGrouping;
                    
                    if (attrs.groupingStartRender) {
                        rowGroup.startRender = function (rows, group, level) {

                            scope.$parent.dataTable.startRenderArgs = {
                                rows: rows,
                                group: group,
                                level: level
                            };
                            return scope.groupingStartRender();
                        }
                    }
                }


                //$.fn.dataTable.TableTools.buttons.download = $.extend(true, {},
                //    $.fn.dataTable.TableTools.buttonBase, {
                //        "sButtonText": "Download",
                //        "sUrl": "",
                //        "sType": "POST",
                //        "fnData": false,
                //        "fnClick": function (button, config) {
                //            var dt = new $.fn.dataTable.Api(this.s.dt);
                //            var data = dt.ajax.params() || {};
                //            // Optional static additional parameters
                //            // data.customParameter = ...;
                //            if (config.fnData) {
                //                config.fnData(data);
                //            }
                //            var iframe = $('<iframe/>', {
                //                id: "RemotingIFrame"
                //            }).css({
                //                border: 'none',
                //                width: 0,
                //                height: 0
                //            }).appendTo('body');
                //            var contentWindow = iframe[0].contentWindow;
                //            contentWindow.document.open();
                //            contentWindow.document.close();
                //            var form = contentWindow.document.createElement('form');
                //            form.setAttribute('method', config.sType);
                //            form.setAttribute('action', config.sUrl);
                //            var input = contentWindow.document.createElement('input');
                //            input.name = 'json';
                //            input.value = JSON.stringify(data);
                //            form.appendChild(input);
                //            contentWindow.document.body.appendChild(form);
                //            form.submit();
                //        }
                //    });

                var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');

                var ngGrid = tableElement.dataTable({
                    "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        if (scope.fnRowcallback != undefined) {
                            var rowDetail = {};
                            rowDetail.tableRow = nRow;
                            rowDetail.data = aData;
                            rowDetail.index = iDisplayIndex;
                            rowDetail.indexFull = iDisplayIndexFull;
                            rowDetail.ngGrid = ngGrid;
                            scope.fnRowcallback({ rowDetail: rowDetail });
                        }
                    }
                    , responsive: { details: { display: $.fn.dataTable.Responsive.display.childRowImmediate, type: '' } }
                    , colReorder: true
                    , "pageLength": 25
                    , pagerSettings: {
                        searchOnEnter: true,
                        language: " ~ از ~ "
                    }
                    //"sScrollX": "100%",
                    //"bScrollCollapse": true,

                    , "bProcessing": true
                    , "bServerSide": true
                    , "sServerMethod": "POST"
                    , "sAjaxSource": ajaxSourceUrl
                    , "fnServerData": function (sSource, aoData, fnCallback, oSettings) {

                        oSettings.jqXHR = $.ajax({
                            "dataType": 'json',
                            "type": "POST",
                            "url": sSource,
                            "data": aoData,
                            "headers": { 'X-XSRF-Token': token },
                            "success": fnCallback
                        })
                    }
                    , "aoColumns": aoColumns
                    , "order": orderValue
                    , "bFilter": searchable
                    , "rowGroup": rowGroup
                    , "bLengthChange": lengthChangeable
                    , "oLanguage": {
                        "oPaginate": {
                            "sFirst": " اول ",
                            "sLast": " آخر ",
                            "sNext": " بعدی ",
                            "sPrevious": " قبلی ",
                            "sEmptyTable": "اطلاعاتی برای نمایش وجود ندارد"

                        },
                        "sInfo": " ردیف _START_ تا _END_  از _TOTAL_ ",
                        "sInfoEmpty": "",
                        "sLengthMenu": "تعداد در صفحه _MENU_",
                        "sLoadingRecords": "  لطفا صبر کنید ...",
                        "sProcessing": " لطفا صبر کنید  ...",
                        "sSearch": "جستجو :",
                        "sZeroRecords": "اطلاعاتی برای نمایش وجود ندارد"
                    }
                    , "bAutoWidth": false
                    , "fnDrawCallback": function (nRow, aData, iDisplayIndex) {
                        var ltrColumns = $('#ngGrid thead tr [datadir]');
                        if (ltrColumns.length > 0) {
                            table_rows = this.fnGetNodes();
                            $.each(table_rows, function (rowIndex) {
                                ltrColumns.map(function (Idx, td) {
                                    colIdx = $(td).parent().children().index(td);
                                    element.find(' tr').eq(rowIndex + 1).find('td').eq(colIdx).css('text-align', $(td).attr('datadir'));

                                })
                            });

                        }

                        $('#ngGrid tbody td span[id="btnExpand"]').on('click', function () {
                            var nTr = $(this).parents('tr')[0];
                            if (ngGrid.fnIsOpen(nTr)) {
                                $(this).html("<i class='green fa fa-plus-circle fa-lg'></i>");
                                ngGrid.fnClose(nTr);
                            }
                            else {
                                /* Open this row */
                                $(this).html("<i class='green fa fa-minus-circle fa-lg'></i>");
                                if (scope.fnDetailsexpanded != undefined) {

                                    scope.$parent.rowSelected = nTr;
                                    scope.fnDetailsexpanded()
                                }
                                //ngGrid.fnOpen(nTr, fnFormatDetails(ngGrid, nTr), 'details');
                            }
                        });




                    }
                    , "fnServerParams": function (aoData) {
                        if (serverParams != null) {

                            //{ "name": "quizid", "value": quizid }&{ "name": "questionid", "value": qid } 
                            var index = 0;
                            serverParams.split('&').forEach(function (item) {
                                if (item != undefined && item != '') {
                                    var param = eval("(" + item + ")");

                                    var externalParamName = {};
                                    externalParamName.name = 'externalParam_' + index + '_name';
                                    externalParamName.value = param.name;
                                    aoData.push(externalParamName);

                                    var externalParamValue = {};
                                    externalParamValue.name = 'externalParam_' + index + '_value';
                                    externalParamValue.value = param.value;
                                    aoData.push(externalParamValue);

                                    index++;
                                    aoData.push(param);
                                }

                            });

                        }

                    }

                });
                scope.$parent.ngGrid = ngGrid;
                if (fnDrawCallback != undefined) {

                    ngGrid.on('draw.dt', function () {
                        fnDrawCallback(ngGrid);
                    });

                }
                ngGrid.fnReload = function () {

                    setServerParams();
                    ngGrid.fnReloadAjax();
                }


                //$(window).bind('resize', function () {
                //    ngGrid.columns.adjust().draw();
                //});


            }


        }

    }
});
