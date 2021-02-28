/***
Wrapper/Helper Class for datagrid based on jQuery GridViewClient Plugin
B I L  L
***/
var GridViewClient = function () {

    var tableOptions; // main options
    var dataTable; // datatable object
    var table; // actual table jquery object
    var tableContainer; // actual table container object
    var tableWrapper; // actual table wrapper jquery object
    var tableInitialized = false;
    //var ajaxParams = {}; // set filter mode
    var the;

    //Filter
    var handleFilterEvent = function () {
        $('textarea.form-filter, select.form-filter, input.form-filter:not([type="radio"],[type="checkbox"])').on('keyup', function (event) {
            dataTable.search(this.value).draw();
        });
    };

    //Reload
    var handleReloadGridView = function () {
        dataTable.search("").draw();
        //dataTable.draw();
    };

    return {

        //main function to initiate the module
        init: function (options) {

            if (!$().dataTable) {
                return;
            }

            the = this;

            // default settings
            options = $.extend(true, {
                src: "", // actual table  
                filterApplyAction: "filter",
                filterCancelAction: "filter_cancel",
                resetGroupActionInputOnSuccess: true,
                loadingMessage: 'Loading...',
                dataTable: {
                    destroy: true,
                    "dom": "<'row'<'col-md-10 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-10 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                    "pageLength": 10, // default records per page
                    "lengthMenu": [
                      [10, 25, 50, 100],
                      [10, 25, 50, 100] // change per page values here
                    ],
                    "language": { // language settings
                        // metronic spesific
                        //"metronicGroupActions": "_TOTAL_ records selected:  ",
                        //"metronicAjaxRequestGeneralError": "Could not complete request. Please check your internet connection",

                        // data tables spesific
                        "lengthMenu": "<span class='seperator'>|</span>View _MENU_ records",
                        "info": "<span class='seperator'>|</span>Found total _TOTAL_ records",
                        "infoEmpty": "",
                        "emptyTable": "No matching records found",
                        "zeroRecords": "No matching records found",
                        "paginate": {
                            "previous": "Prev",
                            "next": "Next",
                            "last": "Last",
                            "first": "First",
                            "page": "Page",
                            "pageOf": "of"
                        }
                    },
                    "orderCellsTop": true,
                    "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                        //'orderable': false,
                        //'targets': [0]
                    }],

                    "pagingType": "bootstrap_extended", // pagination type(bootstrap, bootstrap_full_number or bootstrap_extended)
                    "autoWidth": false, // disable fixed width and enable fluid table
                    "processing": false, // enable/disable display message box on record load
                    "serverSide": false, // enable/disable server side ajax loading

                    "drawCallback": function (oSettings) { // run some code on table redraw
                        if (tableInitialized === false) { // check if table has been initialized
                            tableInitialized = true; // set table initialized
                            table.show(); // display table
                        }
                        //App.initUniform($('input[type="checkbox"]', table)); // reinitialize uniform checkboxes on each table reload
                        //countSelectedRecords(); // reset selected records indicator

                        // callback for ajax data load
                        if (tableOptions.onDataLoad) {
                            tableOptions.onDataLoad.call(undefined, the);
                        }
                        var id = $(table).parent().parent().attr('id');
                        //console.log('dataTable', dataTable, table);

                        var count = null;
                        if (dataTable != undefined) {
                            count = dataTable.rows().count();
                        }

                        if ((dataTable == undefined || count == 0) && $(table).find('tbody > tr').length == 1) {
                            //var count = dataTable.rows().count();
                            //console.log('count', count);
                            //if (count == 0) {} 
                            //console.log('id', id);
                            $('#' + id + ' > div:nth-child(1)').hide();
                            $('#' + id + ' > div:nth-child(3)').hide();

                        } else {
                            if ($('#' + id + ' > div:nth-child(1)').get(0).style.display == 'none') {
                                $('#' + id + ' > div:nth-child(1)').show();
                            }
                            if ($('#' + id + ' > div:nth-child(3)').get(0).style.display == 'none') {
                                $('#' + id + ' > div:nth-child(3)').show();
                            }
                        }
                    },
                    //"fnInitComplete": function (oSettings, json) {

                    //}
                }
            }, options);

            tableOptions = options;

            // create table's jquery object
            table = $(options.src);
            tableContainer = table.parents(".table-container");
            //$(options.src).colResizable({ resizeMode: 'overflow' });
            // apply the special class that used to restyle the default datatable
            var tmp = $.fn.dataTableExt.oStdClasses;

            $.fn.dataTableExt.oStdClasses.sWrapper = $.fn.dataTableExt.oStdClasses.sWrapper + " dataTables_extended_wrapper";
            $.fn.dataTableExt.oStdClasses.sFilterInput = "form-control input-xs input-sm input-inline";
            $.fn.dataTableExt.oStdClasses.sLengthSelect = "form-control input-xs input-sm input-inline";
            // initialize a datatable
            dataTable = table.DataTable(options.dataTable);

            // revert back to default
            $.fn.dataTableExt.oStdClasses.sWrapper = tmp.sWrapper;
            $.fn.dataTableExt.oStdClasses.sFilterInput = tmp.sFilterInput;
            $.fn.dataTableExt.oStdClasses.sLengthSelect = tmp.sLengthSelect;        
            // get table wrapper
            $('.form-filter').attr("placeholder", "Filter");
            tableWrapper = table.parents('.dataTables_wrapper');


            //// build table group actions panel from session (if not post back)
            ////console.log('containerActions', containerActions);
            //if (containerActions != null) {
            //    $('.table-group-actions', tableWrapper).html($(containerActions, tableContainer).html()); // place the panel inside the wrapper                
            //}
            // build table group actions panel
            if ($('.table-actions-wrapper', tableContainer).size() === 1) {
                $('.table-group-actions', tableWrapper).html($('.table-actions-wrapper', tableContainer).html()); // place the panel inside the wrapper

                //Cloning Action Before Removing To Call When No Post back event is fired
                //if (keepActionFilterAvailable == true) {
                //    var oldObject = $('.table-actions-wrapper');
                //    containerActions = jQuery.extend(true, {}, oldObject);
                //}

                $('.table-actions-wrapper', tableContainer).hide(); // remove the template container
            }

            //// handle filter submit button click
            //table.on('click', '.filter-submit', function (e) {
            //    e.preventDefault();
            //    the.submitFilter();
            //});

            // handle filter cancel button click
            table.on('click', '.filter-cancel', function (e) {
                e.preventDefault();
                the.resetFilter();
            });

        },

        //submitFilter: function () {
        //    //
        //    the.setAjaxParam("action", tableOptions.filterApplyAction);

        //    // get all typeable inputs
        //    //var summary = $('#datatable_ajax > thead > tr.filter > td:nth-child(3) > input');
        //    //the.setAjaxParam($(summary).attr('name'), $(summary).val());
        //    $('textarea.form-filter, select.form-filter, input.form-filter:not([type="radio"],[type="checkbox"])', table).each(function () {
        //        the.setAjaxParam($(this).attr("name"), $(this).val());
        //    });

        //    // get all checkboxes
        //    $('input.form-filter[type="checkbox"]:checked', table).each(function () {
        //        the.addAjaxParam($(this).attr("name"), $(this).val());
        //    });

        //    // get all radio buttons
        //    $('input.form-filter[type="radio"]:checked', table).each(function () {
        //        the.setAjaxParam($(this).attr("name"), $(this).val());
        //    });

        //    dataTable.ajax.reload();
        //},

        resetFilter: function () {

            $('textarea.form-filter, select.form-filter, input.form-filter', table).each(function () {
                $(this).val("");
            });
            $('input.form-filter[type="checkbox"]', table).each(function () {
                $(this).attr("checked", false);
            });
            //the.clearAjaxParams();
            //the.addAjaxParam("action", tableOptions.filterCancelAction);
            //dataTable.ajax.reload();
            //dataTable.draw();
            dataTable.search("").draw();
        },

        //getSelectedRowsCount: function () {
        //    return $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).size();
        //},

        getSelectedRows: function () {
            var rows = [];
            $('tbody > tr.selected', table).each(function () {
                rows.push($(this).attr('data-row-id'));
            });

            return rows;
        },

        //setAjaxParam: function (name, value) {
        //    ajaxParams[name] = value;
        //},

        //addAjaxParam: function (name, value) {
        //    if (!ajaxParams[name]) {
        //        ajaxParams[name] = [];
        //    }

        //    skip = false;
        //    for (var i = 0; i < (ajaxParams[name]).length; i++) { // check for duplicates
        //        if (ajaxParams[name][i] === value) {
        //            skip = true;
        //        }
        //    }

        //    if (skip === false) {
        //        ajaxParams[name].push(value);
        //    }
        //},

        //clearAjaxParams: function (name, value) {
        //    ajaxParams = {};
        //},
        //getAjaxParams: function () {
        //    return ajaxParams;
        //},
        getDataTable: function () {
            return dataTable;
        },

        getTableWrapper: function () {
            return tableWrapper;
        },

        gettableContainer: function () {
            return tableContainer;
        },

        getTable: function () {
            return table;
        },
        enableColResize: function () {
            //$(table).colResizable({ resizeMode: 'overflow' });
        },
        invokeFilter: function () {
            handleFilterEvent();
        },
        reload: function () {
            handleReloadGridView();
        }

    };

};