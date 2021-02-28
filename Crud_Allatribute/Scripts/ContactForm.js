var ContactForm = function () {
    var SaveContactValues = function () {
        debugger
        populateFormElementArray('ContactForm');
        BacklogFormElements = getFormElements();
        $('#DynamicFieldsData').val(JSON.stringify(BacklogFormElements));
        return true;
    };
    var populateFormElementArray = function (divID) {
        FormElements = [];
        $("#" + divID + " .text-field").each(function () {
            if ($(this).parents('.form-group').css('display') != 'none') {
                if ($(this).val() != "") {
                    var FormElementModel = {
                        "elementId": $(this).attr('data-id'),
                        "FieldValue": $(this).val(),
                        "attrType": "Text",
                        "OptName": "",
                        "ColumnName": $(this).attr('data-columnname'),
                    }
                    FormElements.push(FormElementModel);
                }
            }
        });

        //$("#" + divID + " .editor-field").each(function () {
        //    var FormElementModel = {
        //        "elementId": $(this).attr('data-id'),
        //        "FieldValue": encodeURIComponent($(this).summernote('code')),
        //        "attrType": "Editor",
        //        "OptName": "",
        //        "ColumnName": $(this).attr('data-columnname'),
        //    }
        //    FormElements.push(FormElementModel);
        //});
        $("#" + divID + " .dropdown-field").each(function () {
            if ($(this).parents('.form-group').css('display') != 'none') {
                var id = $(this).attr('id');
                var name = $('#' + id + ' option:selected').text();
                if (name == "Select") {
                    name = "";
                }
                if ($(this).val() != "") {
                    var FormElementModel = {
                        "elementId": $(this).attr('data-id'),
                        "FieldValue": $(this).val(),
                        "attrType": "Dropdown",
                        "OptName": name,
                        "ColumnName": $(this).attr('data-columnname'),
                    }
                    FormElements.push(FormElementModel);
                }
                else {
                    var FormElementModel = {
                        "elementId": $(this).attr('data-id'),
                        "FieldValue": "",
                        "attrType": "Dropdown",
                        "OptName": name,
                        "ColumnName": $(this).attr('data-columnname'),
                    }
                    FormElements.push(FormElementModel);
                }
            }
        });

    };
    var EditContactField = function (id, type, Name, tabType) {
        var Url = "/Home/openEditContactFieldPopup";
        $.ajax({
            type: "GET",
            data: { id: id, type: type, Name: Name },
            contentType: "application/json; charset=utf-8",
            url: Url,
            dataType: "html",
            success: function (response) {
                $("#showData").html(response);
                $("#exampleModal").modal({
                    backdrop: 'static'
                });
            },
            error: function (response) {
                showToaster({
                    msg: "Something went wrong!",
                    priority: "error"
                });
            },
        });
    };

    var getFormElements = function () {
        return FormElements
    };

    var handleAddUpdateTemplateElement = function () {
        
        var $form = $('#AddEditFormElement');
        var model = {
            itemName: $form.find("#FieldName").val(),
            itemId: $form.find("#hdnElementID").val()
        };

        var dataTable = $("#CustomOptionstbl").dataTable();
        var OptionsListData = [];
        $(dataTable.fnGetNodes()).each(function () {
            var row = $(this);
            var OptionId = row.attr("id");
            var Option = stripHtml(row.children('td:eq(1)').text());
            if ($.trim(Option) != "") {
                OptionsListData.push({
                    "OptionId": escape(OptionId),
                    "Option": Option,
                });
            }
        });

        if (OptionsListData.length == 0 && $('#CustomOptionstbl').length != 0) {
            showToaster({
                msg: "Please Add Options",
                priority: "error"
            });
            return;
        }
        var elementId = $('#hdnElementID').val();
        var attrType = $('#hdnattrType').val();
        var order = $('#hdnElementID').val();
        var FieldName = $('#FieldName').val();
        var FieldInfo = $('#FieldInfo').val();
        var FieldLabel = $('#FieldLabel').val();
        var buttonText = $('#buttonText').val();

        if ($("#hdnLabelEnum").val() == $("#hdnattrType").val()) {
            FieldInfo = encodeURIComponent($("#LabelEditor").summernote('code'));
        }


        var tabType = $('#hdntabType').val();
        //`````````````                   
        var Colspan = $('#Colspan').val();
        if (Colspan == undefined) {
            Colspan = 2;
        }
        var DropdownType = $('#DropdownType').val();
        if (DropdownType == undefined) {
            DropdownType = 0;
        }
        var ListType = $('#hdnListType').val();
        if (ListType == undefined) {
            ListType = "User";
        }
        //else if (attrType == "DropdownCustomList") {
        //    ListType = "Custom";
        //}
        let FieldPlaceholder = $("#FieldPlaceholder").val();

        var OptionsList = JSON.stringify(OptionsListData);

        var IsRequired = false;
        if ($('#hdnIsRootField').val() == "Root") {
            IsRequired = true;
        }
        else {
            IsRequired = $('#IsRequired').is(":checked");
        }
        $.ajax({
            url: '/Home/AddUpdateContactTemplateElement',
            type: 'POST',
            data: { elemId: elementId, attrType: attrType, name: FieldName, info: FieldInfo, FieldLabel: FieldLabel, colspan: Colspan, ddlType: DropdownType, ddlListType: ListType, isReq: IsRequired, OptionsList: OptionsList, tabType: tabType, FieldPlaceholder: FieldPlaceholder, FieldErrorMessage: $('#FieldErrorMessage').val() },
            success: function (response) {
                if (response.success != false) {
                    if ($("#hdnElementID").val() != "0") {
                        showToaster({ msg: "Field updated successfully", priority: "success" });
                        $("#divEditElementPopup").modal('hide');
                    }
                    else {
                        showToaster({ msg: "Field added successfully", priority: "success" });
                        $("#divEditElementPopup").modal('hide');
                    }
                    ContactFormTemplate.GetTemplate();
                }
                else {
                    showToaster({ msg: "Multiselect dropdown types cannot be updated to other types.", priority: "error" });
                }
            },
            error: function (error) {
                $("#divEditElementPopup").modal('hide');
                showToaster({ title: "Error:", msg: "Unable to process your request. Please contact your admin or admin@calibermatrix.com", priority: "error" });
            }
        });


        //caliberSys.makeRequest("/Franchise/CheckExistingContactElementName", "POST", model,
        //    function (result) {
        //        if ($form.valid() && result) {
        //            if ($('#IsRequired').prop('checked') && $('#FieldErrorMessage').val() == 0) {
        //                showToaster({ msg: "Please enter Field error message!", priority: "error" });
        //                return;
        //            }
        //            var elementId = $('#hdnElementID').val();
        //            var attrType = $('#hdnattrType').val();
        //            var order = $('#hdnElementID').val();
        //            var FieldName = $('#FieldName').val();
        //            var FieldInfo = $('#FieldInfo').val();
        //            var FieldLabel = $('#FieldLabel').val();
        //            var buttonText = $('#buttonText').val();

        //            if ($("#hdnLabelEnum").val() == $("#hdnattrType").val()) {
        //                FieldInfo = encodeURIComponent($("#LabelEditor").summernote('code'));
        //            }

        //            let FieldPlaceholder = $("#FieldPlaceholder").val();

        //            var tabType = $('#hdntabType').val();
        //            //`````````````                   
        //            var Colspan = $('#Colspan').val();
        //            if (Colspan == undefined) {
        //                Colspan = 2;
        //            }
        //            var DropdownType = $('#DropdownType').val();
        //            if (DropdownType == undefined) {
        //                DropdownType = 0;
        //            }
        //            var ListType = $('#hdnListType').val();
        //            if (ListType == undefined) {
        //                ListType = "User";
        //            }
        //            //else if (attrType == "DropdownCustomList") {
        //            //    ListType = "Custom";
        //            //}

        //            var OptionsList = JSON.stringify(OptionsListData);

        //            var IsRequired = false;
        //            if ($('#hdnIsRootField').val() == "Root") {
        //                IsRequired = true;
        //            }
        //            else {
        //                IsRequired = $('#IsRequired').is(":checked");
        //            }
        //            $.ajax({
        //                url: '/Franchise/AddUpdateContactTemplateElement',
        //                global: true,
        //                type: 'POST',
        //                data: { elemId: elementId, attrType: attrType, name: FieldName, info: FieldInfo, FieldLabel: FieldLabel, colspan: Colspan, ddlType: DropdownType, ddlListType: ListType, isReq: IsRequired, OptionsList: OptionsList, tabType: tabType, FieldPlaceholder: FieldPlaceholder, FieldErrorMessage: $('#FieldErrorMessage').val(), buttonText: buttonText },
        //                success: function (response) {
        //                    if (response.success != false) {
        //                        if ($("#hdnElementID").val() != "0") {
        //                            showToaster({ msg: "Field updated successfully", priority: "success" });
        //                            $("#divEditElementPopup").modal('hide');
        //                        }
        //                        else {
        //                            showToaster({ msg: "Field added successfully", priority: "success" });
        //                            $("#divEditElementPopup").modal('hide');
        //                        }
        //                        ContactFormTemplate.GetTemplate();
        //                    }
        //                    else {
        //                        showToaster({ msg: "Multiselect dropdown types cannot be updated to other types.", priority: "error" });
        //                    }
        //                },
        //                error: function (error) {
        //                    $("#divEditElementPopup").modal('hide');
        //                    showToaster({ title: "Error:", msg: "Unable to process your request. Please contact your admin or admin@calibermatrix.com", priority: "error" });
        //                }
        //            });
        //        }
        //        if (!result) {
        //            showToaster({
        //                msg: "Field Name already exists!",
        //                priority: "error"
        //            });
        //            //$form.validate().showErrors({
        //            //    "FieldName": errorMessage
        //            //});
        //        }
        //    },
        //    function () {
        //    },
        //    null, true);
    };

    return {
        ContactValues:function () {
            return SaveContactValues();
        },
        EditContactField: function (id, type, Name, tabType) {
            
            EditContactField(id, type, Name, tabType);
        },
        AddUpdateTemplateElement() {
            handleAddUpdateTemplateElement();
        },
    }
}();
