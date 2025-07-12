function Vendor_OnChange() {
    var vendorId = $("#VendorId").val();
    $('#PoId')
        .find('option')
        .remove()
        .end()
        .append('<option value=""></option>')
        .val('');
    if (vendorId !== "") {
        $.ajax({
            url: '/PurchaseOrders/GetSelectList/?vendorId=' + vendorId,
            type: 'GET',
            success: function (res) {
                //SUCCESS
                if (res.responseCode === CODES.RESPONSE_CODE_OK) {
                    for (var i in res.model) {
                        var po = res.model[i];
                        $('#PoId').append('<option value="' + po.value + '">' + po.text + '</option>');
                    }
                }
                else {
                    console.error('error occured while getting POs.');
                }
                $('#PoId').removeAttr('disabled');
                $("#btn-load-po").removeAttr("disabled");
            },
            error: function () {
                //sweetAlert.error({ text: 'An error occcured while getting POs.' });
                console.error('error occured while getting POs.');
            },
        });
    }
    else {

        $("#PoId").attr("disabled", "disabled");
        $("#btn-load-po").attr("disabled", "disabled");
    }
}
function LoadPO() {
    if ($('#PoId').val() !== "") {
        var poId = $('#PoId').val();
        //load po here
        $.ajax({
            url: '/PurchaseOrders/GetDetails/' + poId,
            type: 'GET',
            success: function (res) {
                if (res.responseCode === CODES.RESPONSE_CODE_OK) {
                    var po = res.model;
                    if (po.invPoDetails.length > 0) {
                        $('#btn-load-po').attr('disabled', 'disabled');
                        $('#PoId').attr('disabled', 'disabled');
                    }
                    var nextRowNo = parseInt($('#grndetails-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
                    for (var i = 0; i < po.invPoDetails.length; i++) {
                        var poDet = po.invPoDetails[i];
                        var grnDetObj = {
                            PoId: po.id, ItemId: poDet.itemId, ReceivedQuantity: poDet.requestedQuantity, Rate: poDet.rate,
                            Po: { Id: po.id, PoNo: po.poNo }
                        };
                        var isLast = false;
                        if (i === po.invPoDetails.length - 1) {
                            isLast = true;
                        }
                        getSubItemRow(nextRowNo, grnDetObj, isLast);
                        nextRowNo++;
                    }
                }
                //$('#grndetails-table tbody tr:last').after(newRow);
                //$('.select').select2();
                //CalculateGrnDetails_Total(rowNo);
                //$("#grndetails-table tbody #loader").remove();
                //IsSubItemsTableInited = true;
            },
            error: function (err) {
                console.error(err);
            },
        });
    }
}
function ResetForm() {
    $('#create-grn-form').trigger("reset");
    AddRow(null, true);
    //$("#grndetails-table tbody").find("tr:gt(0)").remove();
    $("#deleted-rows-table tbody").html('<tr></tr>');
    setTimeout(function () {
        $('.select').trigger('change.select2');
        $('.select').trigger('change.select2');
    }, 100);
}

function getSubItemRow(rowNo, model, isLastCheck, deleteFirst) {
    if (model) {
        model.Id = rowNo;
    }
    else {
        model = { Id: rowNo };
    }
    $.ajax({
        url: '/GoodsReceivedNotes/AddGrn_GrnDetailsRow',
        type: 'POST',
        data: model,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (newRow) {
            //$("#grndetails-table tbody #loader").hide();
            $('#grndetails-table tbody tr:last').after(newRow);
            $('.select').select2();

            CalculateGrnDetails_Total(rowNo);
            if (isLastCheck) {
                setTimeout(function () {
                    $('#btn-load-po').removeAttr('disabled');
                    $('#PoId').removeAttr('disabled');
                }, 300);
            }
            if (deleteFirst) {
                $("#grndetails-table tbody").find("tr:first").remove();
            }
            //$("#grndetails-table tbody #loader").remove();
            //IsSubItemsTableInited = true;
        },
        error: function (err) {
            //console.log(err);
        }
    });
}
function AddRow(model, deleteFirst) {
    $('#btn-add-subitems-row').attr('disabled', 'disabled');
    var nextRowNo = parseInt($('#grndetails-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    getSubItemRow(nextRowNo, model, null, deleteFirst);

    setTimeout(function () {
        $('#btn-add-subitems-row').removeAttr('disabled');
    }, 300);
}
function RemoveRow(rowNo) {

    var rowSlector = 'table#grndetails-table tbody tr.row-no-' + rowNo + ' ';

    if ($("table#grndetails-table tbody tr").length > 1) {
        $(rowSlector + " td .row-status").val(STATUSES.DELETED);
        var deletedRow = $(rowSlector).clone();
        $(deletedRow).removeClass('row-no-' + rowNo);
        $(deletedRow).addClass('del-row-no-' + rowNo);
        $('#deleted-rows-table tbody tr:last').after(deletedRow);
        $(rowSlector).remove();
    }
}
function CalculateGrnDetails_Total(rowNo) {
    var result = 0;
    var quantity = parseFloat($(`#InvGrnDetails_${rowNo}__ReceivedQuantity`).val());
    var rate = parseFloat($(`#InvGrnDetails_${rowNo}__Rate`).val());
    result = quantity * rate;
    result = Number(result.toFixed(2));
    $(`#InvGrnDetails_${rowNo}__Amount`).val(result);
}
function ItemSelect_OnChange(rowNo) {
    var Rate = $("#InvGrnDetails_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
    $("#InvGrnDetails_" + rowNo + "__Rate").val(Rate);
    CalculateGrnDetails_Total(rowNo);
}




var FormIsValid = true;
var NonEmptyRowsCount = 0;
function ConvertDateForDateInput(date) {
    if (date instanceof Date) {
        var month = (date.getMonth() + 1);
        var day = date.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = date.getFullYear() + '-' + month + '-' + day;
        return today;
    }

}

function ValidateAll(row) {
    NonEmptyRowsCount++;
    var rowIsValid = true;
    //validate Item
    if ($(row).find("td:eq(1) select").val() == "-1") {
        $(row).find("td:eq(1) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(1) .invalid-span").css("display", "none");
    }
    //validate ReceivedQuantity
    if ($(row).find("td:eq(3) input").val() == "" || parseInt($(row).find("td:eq(3) input").val().trim()) == 0) {
        $(row).find("td:eq(3) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(3) .invalid-span").css("display", "none");
    }
    //validate Rate
    if ($(row).find("td:eq(4) input").val() == "") {
        $(row).find("td:eq(4) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(4) .invalid-span").css("display", "none");
    }

    if (rowIsValid == false) {
        FormIsValid = false;
    }
    return rowIsValid;
}
$(document).ready(function () {
    if ($("#GrnDate").val() === "") {
        var today = ConvertDateForDateInput(new Date());
        $('#GrnDate').val(today);
    }


    $('.select').select2();



    //calculate subitem totals etc
    var rowsCount = $("table#grndetails-table tbody tr").length;
    for (var i = 0; i < rowsCount; i++) {
        if ($("#InvGrnDetails_" + i + "__Rate").val() === "0" || $("#InvGrnDetails_" + i + "__Rate").val() === "") {
            var Rate = $("#InvGrnDetails_" + i + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
            $("#InvGrnDetails_" + i + "__Rate").val(Rate);
        }
        CalculateGrnDetails_Total(i);
    }

    $('#create-grn-form').submit(function (e) {
        e.preventDefault();
        FormIsValid = true;
        NonEmptyRowsCount = 0;
        var rows = $("table#grndetails-table tbody tr");
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            var ItemVal = $(row).find("td:eq(1) select").val();
            if (ItemVal !== "-1") {
                ValidateAll(row);
            }
            else {
                //Quantity has value
                if ($(row).find("td:eq(3) input").val() !== "") {
                    ValidateAll(row);
                }
                else {
                    $(row).find("td:eq(3) .invalid-span").css("display", "none");
                }

                //Vendor has value
                if ($(row).find("td:eq(4) select").val() !== "") {
                    ValidateAll(row);
                }
                else {
                    $(row).find("td:eq(4) .invalid-span").css("display", "none");
                }
            }
        }
        if (NonEmptyRowsCount === 0) {
            sweetAlert.error({ title: 'Invalid!', text: 'GRN Details Table is empty.' });
        }
        if (FormIsValid && NonEmptyRowsCount !== 0) {
            var submitBtnId = 'btn-create-grn';
            var submitToUrl = '/GoodsReceivedNotes/Create';
            var formId = 'create-grn-form';
            const formData = new FormData(this);
            var submitButtonText = $(`#${submitBtnId}`).html();
            setButtonBusy(submitBtnId);
            $.ajax({
                url: submitToUrl,
                type: "POST",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                headers: {
                    RequestVerificationToken:
                        $(`form#${formId} > input[name="__RequestVerificationToken"]`).val()
                },
                success: function (res) {
                    console.log(res);
                    if (!res.errorOccured) {
                        AlertManager.AlertSweetly(res);
                    } else {
                        sweetAlert.error({ text: res.errorMessage });
                    }
                    if ((res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_CREATED)) {
                        ResetForm();
                    }
                    window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
                    setButtonFree(submitBtnId, submitButtonText);
                },
                error: function (error) {
                    console.log(error);
                    sweetAlert.error({ text: "An Error Occured." });
                    setButtonFree(submitBtnId, submitButtonText);
                }
            });
        }
    });
});