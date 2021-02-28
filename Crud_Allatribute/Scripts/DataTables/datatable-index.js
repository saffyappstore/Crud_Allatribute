var TableDatatablesAjax = function () {

    var initPickers = function () {
        //init date pickers
        $('.date-picker')
            .datepicker({
                rtl: App.isRTL(),
                autoclose: true
            });
    }

    var handleRecords = function () {

        var grid = new Datatable();

        grid.init({
            src: $("#datatable_ajax"),
            onSuccess: function (grid, response) {
                // grid:        grid object
                // response:    json object of server side ajax response
                // execute some code after table records loaded
                //console.log('----------------onSuccess----------------' + 'grid',
                    //grid,                    'response',                   response);
            },
            onError: function (grid) {
                // execute some code on network or other general error
                //console.log('----------------onError----------------' + 'grid', grid);
            },
            onDataLoad: function (grid) {
                // execute some code on ajax data load
                //console.log('----------------onDataLoad----------------' + 'grid', grid);
            },
            loadingMessage: 'Loading...',
            dataTable: {
                // here you can define a typical datatable settings from http://datatables.net/usage/options

                // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
                // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/scripts/datatable.js).
                // So when dropdowns used the scrollable div should be removed.
                //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r>t<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>",
                //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
                "bDestroy": true,
                "responsive": true,
                // save custom filters to the state
                "fnStateSaveParams": function (oSettings, sValue) {
                    $("#datatable_ajax tr.filter .form-control")
                        .each(function () {
                            sValue[$(this).attr('name')] = $(this).val();
                        });

                    return sValue;
                },

                // read the custom filters from saved state and populate the filter inputs
                "fnStateLoadParams": function (oSettings, oData) {
                    //Load custom filters
                    $("#datatable_ajax tr.filter .form-control")
                        .each(function () {
                            var element = $(this);
                            if (oData[element.attr('name')]) {
                                element.val(oData[element.attr('name')]);
                            }
                        });

                    return true;
                },
                "lengthMenu": [
                    [10, 20, 50, 100, 150, -1],
                    [10, 20, 50, 100, 150, "All"] // change per page values here
                ],
                "pageLength": 10, // default record count per page
                "ajax": {
                    "url": "/DataTable/DataHandler", // ajax source
                    "contentType": 'application/json; charset=utf-8',
                    'data': function (data) {
                        //console.log('data', JSON.stringify(data));
                        return data = JSON.stringify(data);
                    }
                },
                "columns": [
                    //{
                    //    "render": function () {
                    //        return "";
                    //    }
                    //},
                    { "data": "TcsKey" },
                    { "data": "TcsAction" },
                    //{
                    //    "render": function () {
                    //        return "";
                    //    }
                    //},
                    { "data": "TcsSummary" },
                    { "data": "TcsNotes" },
                    { "data": "TcsExecutionDate", "type": "date" },
                    {
                        "render": function (data, display, row, settings) {
                            //console.log(data, display, row, settings);
                            return "<a class='btn btn-primary'>Edit</a>";
                        }
                    },
                    //{ "data": "CreditCard" }
                ],
                "order": [1, "asc"],
                //"order": [
                //    [0, "asc"]
                //] // set first column as a default sort by asc
            }
        });

        // handle group actionsubmit button click
        grid.getTableWrapper()
            .on('click',
                '.table-group-action-submit',
                function (e) {
                    e.preventDefault();
                    var action = $(".table-group-action-input", grid.getTableWrapper());
                    if (action.val() != "" && grid.getSelectedRowsCount() > 0) {
                        grid.setAjaxParam("customActionType", "group_action");
                        grid.setAjaxParam("customActionName", action.val());
                        grid.setAjaxParam("id", grid.getSelectedRows());
                        grid.getDataTable().ajax.reload();
                        grid.clearAjaxParams();
                    } else if (action.val() == "") {
                        App.alert({
                            type: 'danger',
                            icon: 'warning',
                            message: 'Please select an action',
                            container: grid.getTableWrapper(),
                            place: 'prepend'
                        });
                    } else if (grid.getSelectedRowsCount() === 0) {
                        App.alert({
                            type: 'danger',
                            icon: 'warning',
                            message: 'No record selected',
                            container: grid.getTableWrapper(),
                            place: 'prepend'
                        });
                    }
                });
        //console.log(grid.dataTables);
        //grid.dataTables.columns().every(function () {
        //    var that = this;
        //    $('input', this.footer()).on('keyup change', function () {
        //        that
        //            .search(this.value)
        //            .draw();
        //    });
        //});
        //grid.setAjaxParam("customActionType", "group_action");
        //grid.getDataTable().ajax.reload();
        //grid.clearAjaxParams();


    }

    var handleDemo = function () {
        $('#datatable_ajax2 tfoot th').each(function () {
            $(this).html('<input type="text" />');
        });

        var oTable = $('#datatable_ajax2').DataTable({
            "bDestroy": true,
            "serverSide": true,
            "ajax": {
                "type": "POST",
                "url": '/DataTable/DataHandler',
                "contentType": 'application/json; charset=utf-8',
                'data': function (data) { return data = JSON.stringify(data); }
            },
            "dom": 'clftrip',
            //"scrollY": 300,
            "scrollX": true,
            //"scrollCollapse": true,
            //"scroller": {
            //    loadingIndicator: false
            //},
            //"processing": true,
            //"paging": true,
            "lengthMenu": [
                [10, 20, 50, 100, 150, -1],
                [10, 20, 50, 100, 150, "All"] // change per page values here
            ],
            "pageLength": 10, // default record count per page
            //"deferRender": true,
            "columns": [
                     { "data": "TcsKey" },
                     { "data": "TcsAction" },

                     { "data": "TcsSummary" },
                     { "data": "TcsNotes" },
                     { "data": "TcsExecutionDate", "type": "date" },
                     {
                         "render": function (data, display, row, settings) {
                             //console.log(data, display, row, settings);
                             return "<a class='btn btn-primary'>Edit</a>";
                         }
                     },
            ],
            "order": [1, "asc"]

        });

        oTable.columns().every(function () {
            
            var that = this;
            $('input', this.footer()).on('keyup change', function () {
                
                that.search(this.value).draw();
            });
        });
    };

    var handleMyWrapperGridView = function () {

        var gridView = GridView();
        gridView.init({
            src: $("#datatable_ajax"),
            dataTable: {
                "colResize": {
                    "tableWidthFixed": true,
                    "ltr": true
                },
                bDestroy: true,
                "deferRender": true,
                "order": [
                    [1, 'asc']
                ],
                "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                    'orderable': false,
                    'targets': [5]
                }],
                "bAutoWidth": false,
                bServerSide: true,
                processing: true,
                "drawCallback": function (settings) {
                    if ($('.dataTables_filter input').val() != "") {
                        //$('#tbl_Orders td').highlight($('.dataTables_filter input').val());
                    }
                },
                sServerMethod: "POST",
                "ajax": {
                    "url": "/DataTable/DataHandler", // ajax source
                    "contentType": 'application/json; charset=utf-8',
                    'data': function (data) {
                        //console.log('data', JSON.stringify(data));
                        var params = {};
                        $('#datatable_ajax thead tr td input.form-filter').each(function (index) {

                            params[$(this).attr("name")] = $(this).val();
                            //console.log('params', params);
                        });                        
                        $.each(data.columns,
                            function (index, item) {
                                //console.log('data', this);
                                switch (this.data) {
                                    case "TcsKey":
                                        item.search.value = params.id;
                                        break;
                                    case "TcsAction":
                                        item.search.value = params.action;
                                        break;
                                    case "TcsSummary":
                                        item.search.value = params.summary;
                                        break;
                                    case "TcsNotes":
                                        item.search.value = params.notes;
                                        break;
                                    case "TcsExecutionDate":
                                        item.search.value = params.execution_date_from;
                                    case 5:
                                        item.search.value = params.execution_date_to;
                                        break;
                                    default:

                                }
                            });
                        //console.log('data', this);
                        return data = JSON.stringify(data);
                    }
                },
                aoColumns: [
                    { mData: "TcsKey", sTitle: "ID" },
                    { mData: "TcsAction", sTitle: "Action", },
                    { mData: "TcsSummary", sTitle: "Summary", "bSortable": true, },
                    { mData: "TcsNotes", sTitle: "Notes", "bSortable": true, },
                    {
                        "data": "TcsExecutionDate",
                        render: function (data, type, row) {
                            // If display or filter data is requested, format the date
                            if (type === 'display' || type === 'filter') {
                                var rowvalue = row["TcsExecutionDate"];
                                var rowvalueallday = row["TcsExecutionDate"];
                                //console.log(rowvalueallday, rowvalue)
                                if (rowvalue == null) {
                                    return data;
                                }
                                else if (rowvalueallday != '') {
                                    return (moment(data).format("ddd DD/MM/YYYY (HH:mm)"));
                                } else {
                                    if (rowvalue != '')
                                        return (moment(data).format("ddd DD/MM/YYYY"));
                                    else
                                        return '';
                                }
                            }
                            // Otherwise the data type requested (`type`) is type detection or
                            // sorting data, for which we want to use the raw date value, so just return
                            // that, unaltered
                            return data;
                        }
                    },
                    {
                        "render": function (data, display, row, settings) {
                            //console.log(data, display, row, settings);
                            return "<a class='btn btn-primary'>Edit</a>";
                        }
                    }
                ],
                //Assigning rowID to each row
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    //console.log('nRow', nRow, 'aData', aData, 'iDisplayIndex', iDisplayIndex, 'iDisplayIndexFull', iDisplayIndexFull);
                    $(nRow).attr("id", aData["TcsKey"]);
                    return nRow;
                }
            }
        });
        gridView.invokeFilter();
        //console.log('gridView', gridView);
    };

    return {

        //main function to initiate the module
        init: function () {

            initPickers();
            //handleRecords();
            //handleDemo();
            handleMyWrapperGridView();

        }

    };
}();
jQuery(document).ready(function () {
    TableDatatablesAjax.init();
});