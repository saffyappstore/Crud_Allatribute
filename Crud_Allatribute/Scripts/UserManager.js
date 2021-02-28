var UserManager = {
    loadGrid: function (Columns) {
        var columnsData = [];
        let grid = "#UserTable";
        let gridId = "UserTable";
        $.each(Columns, function (i, item) {

            if (item == "Action") {
                columnsData.push({
                    "data": item,
                    "name": item,
                    "orderable": false

                });
            }
            else {
                columnsData.push({
                    "data": item,
                    "name": item,

                });
            }
        });
      var tableUser= $("#UserTable").DataTable({
            "dom": "<'row'<'col-md-12 col-sm-12 dompagination'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-12 col-sm-12 dompagination'pli><'col-md-4 col-sm-12'>>", // datatable layout
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "pageLength": 5,
           "bStateSave": true,
            "ajax": {
                "url": "/Home/DynamicGrid",
                "type": "POST",


                "data": function (d) {
                    d.Search = globalGridSetting.getResetState() == true ? "1" : "";
                    d.SearchItems = GetItemsToSearch("");
                    d.columnSortOrder = globalGridSetting.getSortOrder();
                }
            },


            "columns": columnsData,
        });

        

        //$('#UserName').bind('keyup', function (e) {
        //    clearTimeout(timeout);
        //    timeout = setTimeout(function () {
        //        globalGridSettings.setResetState(true);
        //        userGrid.setAjaxParam("Search", "1");
        //        var items = GetItemsToSearch("");

        //        userGrid.setAjaxParam("SearchItems", items);
        //        userGrid.getDataTable().ajax.reload();
        //    }, 800);
        //});
        // function to clear the previous timer and set new timer for filter column keyup event to execute.
        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();


        // script for column filter Keyup event and here I have created half second(500) delay using the timer. You can increase depends on your requirement.
        tableUser.columns().eq(0).each(function (colIdx) {
            $('input', $('.filters td')[colIdx]).bind('keyup', function () {
                var coltext = this.value; // typed value in the search column
                var colindex = colIdx; // column index
               globalGridSetting.setResetState(true);
                var items = GetItemsToSearch("");
                delay(function () {
                  
                    tableUser
                        .column(colindex)
                        .search("SearchItems", items)
                        .draw();
                }, 500);
            });
        })
        function GetItemsToSearch(type) {
            
            var searchItems;
            searchItems = "Username=" + $("#txtUser").val() + "||Email=" + $("#txtEmail").val() + "||StartDate=" + $("#txtDatefrom").val() + "||EndDate=" + $("#txtDateto").val();;
            return searchItems;
        }
    }
}


    //    var columnsData = [];
    //    let grid = "#UserTable";
    //    let gridId = "UserTable";
    //    $.each(Columns, function (i, item) {

    //        columnsData.push({
    //            "data": item,
    //            "name": item
    //        });
    //    });

    //    var userGrid = new Datatable();
    //    userGrid.init({
    //        src: $(grid),
    //        onSuccess: function (grid, response) {
    //        },
    //        onError: function (grid) {
    //        },
    //        onDataLoad: function (grid) {
    //        },

    //        loadingMessage: 'none',
    //        dataTable: {

    //            "dom": "<'row'<'col-md-12 col-sm-12 dompagination'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-12 col-sm-12 dompagination'pli><'col-md-4 col-sm-12'>>", // datatable layout
    //            "pageLength": 10, // default records per page
    //            "lengthMenu": [
    //                [10, 25, 50, 100, -1],
    //                [10, 25, 50, 100, "All"] // change per page values here
    //            ],
    //            "language": {
    //                "lengthMenu": "<span class='seperator'>|</span> View _MENU_ Records",
    //                "info": "<span class='seperator'>| </span>Found total _TOTAL_ records",
    //                "infoEmpty": "<span class='seperator'>|</span>No records found to show",
    //                "emptyTable": "No data available in table",
    //                "zeroRecords": "No matching records found",
    //            },

    //            "pagingType": "bootstrap_extended",
    //            "searching": true,
    //            "ordering": true,
    //            "info": true,
    //            //"stateSave": true,


    //            "ajax": {

    //                "url": '/Home/List',
    //                "data": function (d) {
                      
    //                    d.SearchItems = GetItemsToSearch("");

    //                }
    //            },


    //            "columns": columnsData,


    //        }
    //    });

    //    function GetItemsToSearch(type) {

    //        var searchItems;
    //        searchItems = "ID=" + $("#SerialNum").val() + "||UserName=" + $("#UserName").val() + "||Email=" + $("#EmailUser").val() + "||Name=" + $('#NameUser').val() + "||StartFrom=" + $('#UserJoined').val() + "||StartTo=" + $('#userjoinedTo').val() + "||Status=" + $('#status option:selected').val() + "||Title=" + $('#txtTitle').val();
    //        return searchItems;
    //    }
    //}
  

