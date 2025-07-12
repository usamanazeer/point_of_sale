function getSubItemRow(rowNo) {
    $.ajax({
        url: '/GoodsReturnNotes/AddGrrn_GrrnDetailsRow?rowNo=' + rowNo,
        type: 'GET',
        success: function (newRow) {
            //$("#grrndetails-table tbody #loader").hide();
            $('#grrndetails-table tbody tr:last').after(newRow);
            $('.select').select2();
            CalculateGrrnDetails_Total(rowNo);
        },
        error: function (err) {
            //console.log(err);
        },
    });
}
function AddRow() {
    $('#btn-add-subitems-row').attr('disabled', 'disabled');
    var nextRowNo = parseInt($('#grrndetails-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    getSubItemRow(nextRowNo);

    setTimeout(function () {
        $('#btn-add-subitems-row').removeAttr('disabled');
    }, 300);
}
function RemoveRow(rowNo) {

    var rowSlector = 'table#grrndetails-table tbody tr.row-no-' + rowNo + ' ';

    if ($("table#grrndetails-table tbody tr").length > 1) {
        $(rowSlector + " td .row-status").val(STATUSES.DELETED);
        var deletedRow = $(rowSlector).clone();
        $(deletedRow).removeClass('row-no-' + rowNo);
        $(deletedRow).addClass('del-row-no-' + rowNo);
        $('#deleted-rows-table tbody tr:last').after(deletedRow);
        $(rowSlector).remove();
    }
}
function CalculateGrrnDetails_Total(rowNo) {
    var result = 0;
    var quantity = parseFloat($(`#InvGrrnDetails_${rowNo}__ReturnQuantity`).val());
    var rate = parseFloat($(`#InvGrrnDetails_${rowNo}__Rate`).val());
    result = quantity * rate;
    result = Number(result.toFixed(2));
    $(`#InvGrrnDetails_${rowNo}__Amount`).val(result);
}
function ItemSelect_OnChange(rowNo) {
    var Rate = $("#InvGrrnDetails_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
    $("#InvGrrnDetails_" + rowNo + "__Rate").val(Rate);
    CalculateGrrnDetails_Total(rowNo);
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
$(document).ready(function () {

    if ($("#GrrnDate").val() === "") {
        var today = ConvertDateForDateInput(new Date());
        $('#GrrnDate').val(today);
    }


    $('.select').select2();



    //calculate subitem totals etc
    var rowsCount = $("table#grrndetails-table tbody tr").length;
    for (var i = 0; i < rowsCount; i++) {
        if ($("#InvGrrnDetails_" + i + "__Rate").val() === "0" || $("#InvGrrnDetails_" + i + "__Rate").val() === "") {
            var Rate = $("#InvGrrnDetails_" + i + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
            $("#InvGrrnDetails_" + i + "__Rate").val(Rate);
        }
        CalculateGrrnDetails_Total(i);
    }

    $('#edit-grrn-form').submit(function (e) {
        e.preventDefault();
        FormIsValid = true;
        NonEmptyRowsCount = 0;
        var rows = $("table#grrndetails-table tbody tr");
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            var ItemVal = $(row).find("td:eq(0) select").val();
            if (ItemVal !== "-1") {
                ValidateAll(row);
            }
            else {
                //Quantity has value
                if ($(row).find("td:eq(2) input").val() !== "") {
                    ValidateAll(row);
                }
                else {
                    $(row).find("td:eq(2) .invalid-span").css("display", "none");
                }

                //Vendor has value
                if ($(row).find("td:eq(3) select").val() !== "") {
                    ValidateAll(row);
                }
                else {
                    $(row).find("td:eq(3) .invalid-span").css("display", "none");
                }
            }
        }
        if (NonEmptyRowsCount === 0) {
            sweetAlert.error({ title: 'Invalid!', text: 'GRRN Details Table is empty.' });
        }
        if (FormIsValid && NonEmptyRowsCount !== 0) {
            var submitBtnId = 'btn-edit-grrn';
            var submitToUrl = '/GoodsReturnNotes/Edit';
            var formId = 'edit-grrn-form';
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
function ValidateAll(row) {
    NonEmptyRowsCount++;
    var rowIsValid = true;
    //validate Item
    if ($(row).find("td:eq(0) select").val() === "-1") {
        $(row).find("td:eq(0) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(0) .invalid-span").css("display", "none");
    }
    //validate ReturnQuantity
    if ($(row).find("td:eq(2) input").val() === "" || parseInt($(row).find("td:eq(2) input").val().trim()) === 0) {
        $(row).find("td:eq(2) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(2) .invalid-span").css("display", "none");
    }
    //validate Rate
    if ($(row).find("td:eq(3) input").val() === "") {
        $(row).find("td:eq(3) .invalid-span").css("display", "block");
        rowIsValid = false;
    }
    else {
        $(row).find("td:eq(3) .invalid-span").css("display", "none");
    }

    if (rowIsValid === false) {
        FormIsValid = false;
    }
    return rowIsValid;
}