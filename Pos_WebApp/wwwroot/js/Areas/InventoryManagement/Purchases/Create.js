function ResetForm() {
    $('#create-purchase-form').trigger("reset");
    $("#purchases-table tbody").find("tr:gt(0)").remove();
    $("#deleted-rows-table tbody").html('<tr></tr>');
    $('.select').trigger('change.select2');
}
function ApplySelect2() {
    $('.select').select2();
}

var FormIsValid = true;
var NonEmptyRowsCount = 0;
$(document).ready(function () {


    ApplySelect2();
    //calculate final sales price here
    //var rowsCount = $("table#purchases-table tbody tr").length;
    //for (var i = 0; i < rowsCount; i++) {
    //    CalculateSalesRateIncTax(i);
    //}

    $('#create-purchase-form').submit(function (e) {

        e.preventDefault();
        FormIsValid = true;

        if ($('#PurchaseDate').val() === "") {
            FormIsValid = false;
            sweetAlert.error({ title: 'Required!', text: 'Purchase Date is Required.' });

        } else {
            NonEmptyRowsCount = 0;
            var rows = $("table#purchases-table tbody tr");
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var itemVal = $(row).find("td:eq(0) select").val();
                if (itemVal !== "") {
                    ValidateAll(row);
                }
                else {
                    //Quantity has value
                    if ($(row).find("td:eq(2) input").val() !== "" && parseFloat($(row).find("td:eq(2) input").val()) !== 0) {
                        ValidateAll(row);
                    }
                    else {
                        $(row).find("td:eq(2) .invalid-span").css("display", "none");
                    }

                    //purchase-rate has value
                    if ($(row).find("td:eq(5) input").val() !== "") {
                        ValidateAll(row);
                    }
                    else {
                        $(row).find("td:eq(5) .invalid-span").css("display", "none");
                    }


                    //Tax has value
                    if ($(row).find("td:eq(7) select").val() !== "") {
                        ValidateAll(row);
                    }
                }
            }

            //if (FormIsValid === false || NonEmptyRowsCount === 0) {

            //}
            if (NonEmptyRowsCount === 0) {
                sweetAlert.error({ title: 'Invalid!', text: 'Purchases Table is empty.' });
            }
            console.log('FormIsValid', FormIsValid);
            console.log('NonEmptyRowsCount > 0', NonEmptyRowsCount);
            console.log(FormIsValid === true && NonEmptyRowsCount > 0);
            if (FormIsValid === true && NonEmptyRowsCount > 0) {
                var submitToUrl = '/Purchases/Create';
                var formId = 'create-purchase-form';
                var submitBtnId = 'btn-create-purchase';
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
        }

        //SaveRole();

    });
});
function ValidateAll(row) {
    NonEmptyRowsCount++;
    var rowIsValid = true;

    //validate Item
    if ($(row).find("td:eq(0) select").val() === "") {
        $(row).find("td:eq(0) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(0) .invalid-span").css("display", "none");
    }

    //validate Quantity
    if ($(row).find("td:eq(2) input").val() === "" || parseFloat($(row).find("td:eq(2) input").val()) === 0) {
        $(row).find("td:eq(2) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(2) .invalid-span").css("display", "none");
    }

    //validate purchase rate
    if ($(row).find("td:eq(5) input").val() === "") {
        $(row).find("td:eq(5) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(5) .invalid-span").css("display", "none");
    }

    if (rowIsValid === false) {
        FormIsValid = false;
    }
    return rowIsValid;
}

function AddRow() {
    var nextRowNo = parseInt($('#purchases-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    $.ajax({
        url: '/Purchases/GetPurchaseDetailRow?rowNo=' + nextRowNo,
        type: 'GET',
        success: function (newRow) {
            //SUCCESS
            $('#purchases-table tbody tr:last').after(newRow);
            ApplySelect2();
        },
        error: function (error) {
            console.log('error while adding row: ', error);
        }
    });
}
function RemoveRow(rowNo) {
    var rowSelector = `table#purchases-table tbody tr.row-no-${rowNo} `;

    if ($("table#purchases-table tbody tr").length > 1) {
        $(rowSelector + " td .row-status").val(STATUSES.DELETED);
        var deletedRow = $(rowSelector).clone();
        $(deletedRow).removeClass('row-no-' + rowNo);
        $(deletedRow).addClass('del-row-no-' + rowNo);
        $('#deleted-rows-table tbody tr:last').after(deletedRow);
        $(rowSelector).remove();
    }
}

var flag = true;
function ItemBarCodeSelect_OnChange(rowNo) {

    if (flag) {
        var itemId = $("#InvPurchaseDetail_" + rowNo + "__BarCodeId option:selected")[0].getAttribute('data-ItemId');
        $('#InvPurchaseDetail_' + rowNo + '__ItemId').val(itemId);
        flag = false;
        $('#InvPurchaseDetail_' + rowNo + '__ItemId').trigger('change.select2');
        flag = true;
    }
}
function ItemSelect_OnChange(rowNo) {
    var purchaseRate = $("#InvPurchaseDetail_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-PurchaseRate');
    var salesRate = $("#InvPurchaseDetail_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-SalesRate');
    $(`#InvPurchaseDetail_${rowNo}__PurchaseRate`).val(purchaseRate);
    $(`#InvPurchaseDetail_${rowNo}__SalesRate`).val(salesRate);
    if (flag) {
        var itemBarCode = $(`#InvPurchaseDetail_${rowNo}__ItemId option:selected`)[0].getAttribute('data-ItemBrCode');
        if (itemBarCode != null) {
            itemBarCode = itemBarCode.trim();
        }
        flag = false;
        if (itemBarCode !== "") {
            var itemBarCodeId = $("#InvPurchaseDetail_" + rowNo + "__BarCodeId option:contains(" + itemBarCode + ")").val();
            $(`#InvPurchaseDetail_${rowNo}__BarCodeId`).val(itemBarCodeId);
            $(`#InvPurchaseDetail_${rowNo}__BarCodeId`).trigger('change.select2');
        }
        else {
            $(`#InvPurchaseDetail_${rowNo}__BarCodeId`).val("");
            $(`#InvPurchaseDetail_${rowNo}__BarCodeId`).trigger('change.select2');
        }
        flag = true;
    }
}