function ResetForm() {
    $('#AddJournalEntry').trigger("reset");
    $("#transaction-details-table tbody").find("tr:gt(0)").remove();
    $('.select').trigger('change.select2');
    $('.form-control.dr').removeAttr('disabled');
    $('.form-control.cr').removeAttr('disabled');
    $('.total-row').removeClass("red");
    $('.total-row').removeClass("green");
    $('#dr-total').html("0");
    $('#cr-total').html("0");
}

function ApplySelect2() {
    $('.select').select2();
}

var FormIsValid = true;
//var NonEmptyRowsCount = 0;
$(document).ready(function () {
    ApplySelect2();
    $('#AddJournalEntry').submit(function (e) {
        e.preventDefault();
        FormIsValid = true;
        var drSum = 0;
        var crSum = 0;
        //if ($('#BillDate').val() === "") {
        //    FormIsValid = false;
        //    sweetAlert.error({ title: 'Required!', text: 'Bill Date is Required.' });

        //}
        const rows = $("table#transaction-details-table tbody tr");
        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const accountId = $(row).find("td:eq(0) select").val();
            const drVal = parseFloat($(row).find("td:eq(2) input").val());
            const crVal = parseFloat($(row).find("td:eq(3) input").val());
            drSum += drVal;
            crSum += crVal;
            if (accountId !== "") {
                $(row).find("td:eq(0) .invalid-span").css("display", "none");
            } else {
                FormIsValid = false;
                $(row).find("td:eq(0) .invalid-span").css("display", "block");
            }

            if (drVal === 0 && crVal === 0) {
                FormIsValid = false;
                $(row).find("td:eq(2) .invalid-span").css("display", "block");
                $(row).find("td:eq(3) .invalid-span").css("display", "block");
            } else {

                $(row).find("td:eq(2) .invalid-span").css("display", "none");
                $(row).find("td:eq(3) .invalid-span").css("display", "none");
            }
        }


        if (drSum !== crSum) {

            sweetAlert.error({
                title: 'Required!',
                text: 'Debit (Dr.) Amount Must be equals to  Credit (Cr.) Amount.'
            });
        }
        if (FormIsValid && drSum === crSum) {
            Submit();
        }
        //SaveRole();

    });
});

function Submit() {
    var submitBtnId = 'btn-add-transaction';
    var formId = 'AddJournalEntry';
    var submitToUrl = '/Journal/Add';

    const formData = new FormData($(`#${formId}`)[0]);
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

            if ((res.responseCode === CODES.RESPONSE_CODE_OK ||
                res.responseCode === CODES.RESPONSE_CODE_CREATED)) {
                ResetForm();
                window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
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

function AddRow() {
    const nextRowNo = parseInt($('#transaction-details-table tbody tr:last')[0].getAttribute("data-row-no")) +
        1;
    $.ajax({
        url: `/Journal/GetJournalEntryRow?rowNo=${nextRowNo}`,
        type: 'GET',
        success: function (newRow) {
            //SUCCESS
            $('#transaction-details-table tbody tr:last').after(newRow);
            ApplySelect2();
        },
        error: function (error) {
            console.log('error while adding row: ', error);
        }
    });
}

function RemoveRow(rowNo) {
    const rowSelector = `table#transaction-details-table tbody tr.row-no-${rowNo} `;

    if ($("table#transaction-details-table tbody tr").length > 2) {
        $(rowSelector + " td .row-status").val(STATUSES.DELETED);
        const deletedRow = $(rowSelector).clone();
        $(deletedRow).removeClass(`row-no-${rowNo}`);
        $(deletedRow).addClass(`del-row-no-${rowNo}`);
        $('#deleted-rows-table tbody tr:last').after(deletedRow);
        $(rowSelector).remove();
    }
}


function AccountSelect_OnChange(rowNo) {
    setStatement(rowNo);
}

function setStatement(rowNo) {
    var accountName = $(`#AccTransactionDetail_${rowNo}__AccountId option:selected`).text();
    if (accountName.trim() !== "") {
        if ($(`#AccTransactionDetail_${rowNo}__Dr`).val() !== "0") {
            $(`#AccTransactionDetail_${rowNo}__Statement`).val(`${accountName} Dr.`);
            return;
        }
        if ($(`#AccTransactionDetail_${rowNo}__Cr`).val() !== "0") {
            $(`#AccTransactionDetail_${rowNo}__Statement`).val(`${accountName} Cr.`);
            return;
        }
        $(`#AccTransactionDetail_${rowNo}__Statement`).val("");
    } else {
        $(`#AccTransactionDetail_${rowNo}__Statement`).val("");
    }
}

function DrChange(rowNo) {
    if ($(`#AccTransactionDetail_${rowNo}__Dr`).val() === "") {
        $(`#AccTransactionDetail_${rowNo}__Dr`).val("0");
    }
    if (parseFloat($(`#AccTransactionDetail_${rowNo}__Dr`).val()) !== 0) {
        $(`#AccTransactionDetail_${rowNo}__Cr`).attr("disabled", "disabled");
    } else {
        $(`#AccTransactionDetail_${rowNo}__Cr`).removeAttr("disabled");
    }
    TotalDrCr();
    setStatement(rowNo);
}

function CrChange(rowNo) {
    if ($(`#AccTransactionDetail_${rowNo}__Cr`).val() === "") {
        $(`#AccTransactionDetail_${rowNo}__Cr`).val("0");
    }
    if (parseFloat($(`#AccTransactionDetail_${rowNo}__Cr`).val()) !== 0) {
        $(`#AccTransactionDetail_${rowNo}__Dr`).attr("disabled", "disabled");
    } else {
        $(`#AccTransactionDetail_${rowNo}__Dr`).removeAttr("disabled");
    }
    TotalDrCr();

    setStatement(rowNo);
}

function TotalDrCr() {
    var drTotal = 0;
    var crTotal = 0;
    const rows = $("table#transaction-details-table tbody tr");
    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const drVal = parseFloat($(row).find("td:eq(2) input").val());
        const crVal = parseFloat($(row).find("td:eq(3) input").val());
        drTotal += drVal;
        crTotal += crVal;
    }
    $("#dr-total").html(`${drTotal}`);
    $("#cr-total").html(`${crTotal}`);

    if (drTotal !== crTotal) {
        $('.total-row').removeClass("green");
        $('.total-row').addClass("red");
        return false;
    } else {
        $('.total-row').removeClass("red");
        $('.total-row').addClass("green");
        return true;
    }

}