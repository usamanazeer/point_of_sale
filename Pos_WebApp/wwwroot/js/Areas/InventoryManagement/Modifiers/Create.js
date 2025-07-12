bindSubmit('create-modifier-form', 'btn-create-modifier', '/Modifiers/Create', null, null, function (res) {
    if (res.responseCode === CODES.RESPONSE_CODE_CREATED) {
        ResetForm();
    }
});

function ResetForm() {
    $('#create-modifier-form').trigger("reset");
    $("#subitems-table tbody").find("tr:gt(0)").remove();
    $("#deleted-rows-table tbody").html('<tr></tr>');
    setTimeout(function () {
        $('.select').trigger('change.select2');
        $('.select').trigger('change.select2');
    }, 100);
}

function getSubItemRow(rowNo) {
    $.ajax({
        url: '/Modifiers/GetAddModifier_ItemsRow?rowNo=' + rowNo,
        type: 'GET',
        success: function (newRow) {
            //$("#subitems-table tbody #loader").hide();
            $('#subitems-table tbody tr:last').after(newRow);
            $('.select').select2();
            //$("#subitems-table tbody #loader").remove();
            //IsSubItemsTableInited = true;
        },
        error: function (err) {
            //console.log(err);
        }
    });
}
function AddRow() {
    $('#btn-add-subitems-row').attr('disabled', 'disabled');
    var nextRowNo = parseInt($('#subitems-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    getSubItemRow(nextRowNo);

    setTimeout(function () {
        $('#btn-add-subitems-row').removeAttr('disabled');
    }, 300);
}
function RemoveRow(rowNo) {
    //console.log(rowNo)
    var rowSlector = 'table#subitems-table tbody tr.row-no-' + rowNo + ' ';

    if ($("table#subitems-table tbody tr").length > 1) {
        $(rowSlector + " td .row-status").val(STATUSES.DELETED);
        var deletedRow = $(rowSlector).clone();
        $(deletedRow).removeClass('row-no-' + rowNo);
        $(deletedRow).addClass('del-row-no-' + rowNo);
        $('#deleted-rows-table tbody tr:last').after(deletedRow);
        $(rowSlector).remove();
    }
}
function CalculateSubItems_Total(rowNo) {
    var result = 0;
    var quantity = parseFloat($(`#InvModifierItems_${rowNo}__Quantity`).val());
    var unitAmount = parseFloat($(`#InvModifierItems_${rowNo}__SalesRate`).val());
    //console.log('unitAmount: ', unitAmount);
    //console.log('quantity: ', quantity);
    result = quantity * unitAmount;
    $(`#InvModifierItems_${rowNo}__Total`).val(result);
}
var flag = true;
function ItemBarCodeSelect_OnChange(rowNo) {
    if (flag) {
        var ItemId = null;
        if ($("#InvModifierItems_" + rowNo + "__BarCodeId option:selected")[0]) {
            ItemId = $("#InvModifierItems_" + rowNo + "__BarCodeId option:selected")[0].getAttribute('data-ItemId');
            $('#InvModifierItems_' + rowNo + '__ItemId').val(ItemId);
            flag = false;
            $('#InvModifierItems_' + rowNo + '__ItemId').trigger('change.select2');
            flag = true;
        }
    }
}
function ItemSelect_OnChange(rowNo) {
    var SalesRate = $("#InvModifierItems_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
    var Measurement = $("#InvModifierItems_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-itemMeasurement');
    var Unit = $("#InvModifierItems_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-itemUnit');
    $("#InvModifierItems_" + rowNo + "__Measurement").val(Measurement + ' ' + Unit);
    $("#InvModifierItems_" + rowNo + "__SalesRate").val(SalesRate);
    CalculateSubItems_Total(rowNo);
    if (flag) {
        var ItemBarCode = $("#InvModifierItems_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-ItemBrCode');

        if (ItemBarCode != null) {
            ItemBarCode = ItemBarCode.trim();
        }
        flag = false;
        if (ItemBarCode != "") {
            var ItemBarCodeId = $("#InvModifierItems_" + rowNo + "__BarCodeId option:contains(" + ItemBarCode + ")").val();
            $("#InvModifierItems_" + rowNo + "__BarCodeId").val(ItemBarCodeId);

            $("#InvModifierItems_" + rowNo + "__BarCodeId").trigger('change.select2');
        }
        else {
            $("#InvModifierItems_" + rowNo + "__BarCodeId").val("");
            $("#InvModifierItems_" + rowNo + "__BarCodeId").trigger('change.select2');
        }
        flag = true;
    }
}
$(document).ready(function () {
    $('.select').select2();

    //if ($("#IsRecipe")[0].checked || $("#IsDeal")[0].checked) {
    //    IsSubItemsTableInited = true;

    //calculate subitem totals etc
    var rowsCount = $("table#subitems-table tbody tr").length;
    for (var i = 0; i < rowsCount; i++) {
        var SalesRate = $("#InvModifierItems_" + i + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
        var Measurement = $("#InvModifierItems_" + i + "__ItemId option:selected")[0].getAttribute('data-itemMeasurement');
        var Unit = $("#InvModifierItems_" + i + "__ItemId option:selected")[0].getAttribute('data-itemUnit');
        $("#InvModifierItems_" + i + "__Measurement").val(Measurement + ' ' + Unit);
        $("#InvModifierItems_" + i + "__SalesRate").val(SalesRate);
        CalculateSubItems_Total(i);
    }
    //}
});