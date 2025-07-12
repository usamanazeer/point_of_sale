bindSubmit('create-item-form', 'btn-create-item', '/Items/Create', null, null, function (res) {
    if (res.responseCode === CODES.RESPONSE_CODE_CREATED) {
        ResetForm();
    }
});

var IsSubItemsTableInited = false;
var IsModifiersTableInited = false;

function setItemType() {
    setTimeout(function () {
        if ($('#IsRawItem')[0].checked) {
            $('#ItemType').val('1');
        } else if ($('#IsRecipe')[0].checked) {

            $('#ItemType').val('2');
        } else if ($('#IsDeal')[0].checked) {
            $('#ItemType').val('3');
        } else {
            $('#ItemType').val('0');
        }
    },
        100);
}

function ResetForm() {
    $('#create-item-form').trigger("reset");
    $('#table-label').html("");
    $('#PurchaseRate').removeAttr('readonly');
    $('#recipe-table-container').attr('hidden', 'hidden');
    $('#subitems-table tbody').html('<tr></tr>');
    $('#subitems-deleted-rows-table tbody').html('<tr></tr>');
    $('#modifiers-table-container').attr('hidden', 'hidden');
    $('#modifiers-table tbody').html('<tr></tr>');
    $('#modifiers-deleted-rows-table').html('<tr></tr>');
    //$('#IsReturnable')[0].checked = false;
    //$('#DisplayOnPos')[0].checked = true;
    //$('#IsRawItem')[0].checked = false;
    //$('#IsDeal')[0].checked = false;
    //$('#IsRecipe')[0].checked = false;
    //$('#addModifiersSwitch')[0].checked = false;
    setTimeout(function () {
        $('.select').trigger('change.select2');
        $('.select').trigger('change.select2');
    },
        100);

}
//function ResetSubItems()
//{
//    var rowsCount = $("table#subitems-table tbody tr").length;
//    for (var i = 0; i < rowsCount; i++) {
//        $("#InvItemRecipeChild_" + i + "__ItemId").val("-1");
//        $("#InvItemRecipeChild_" + i + "__Quantity").val("0");
//    }
//    setTimeout(function () {
//        $('.select').trigger('change.select2');
//        $('.select').trigger('change.select2');
//    }, 100);
//}

//var FormIsValid = true;
//var NonEmptyRowsCount = 0;
$(document).ready(function () {
    $('.select').select2();

    if ($("#IsRecipe")[0].checked || $("#IsDeal")[0].checked) {
        IsSubItemsTableInited = true;

        //calculate subitem totals etc
        var rowsCount = $("table#subitems-table tbody tr").length;
        for (var i = 0; i < rowsCount; i++) {
            var SalesRate =
                $("#InvItemRecipeChild_" + i + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
            var Measurement =
                $("#InvItemRecipeChild_" + i + "__ItemId option:selected")[0].getAttribute(
                    'data-itemMeasurement');
            var Unit =
                $("#InvItemRecipeChild_" + i + "__ItemId option:selected")[0].getAttribute('data-itemUnit');
            $("#InvItemRecipeChild_" + i + "__Measurement").val(Measurement + ' ' + Unit);
            $("#InvItemRecipeChild_" + i + "__SalesRate").val(SalesRate);
            CalculateSubItems_Total(i);
        }
    }
    if ($("#addModifiersSwitch")[0].checked) {
        IsModifiersTableInited = true;

        //calculate modifiers totals etc
        var rowsCount = $("table#modifiers-table tbody tr").length;
        for (var i = 0; i < rowsCount; i++) {
            var modifiersCharges =
                $("#InvItemModifiers_" + i + "__ModifierId option:selected")[0].getAttribute(
                    'data-ModifierCharges');
            $("#InvItemModifiers_" + i + "__ModifierCharges").val(modifiersCharges);
            CalculateModifiers_Total(i);
        }
    }
});


function DisplayOnPos_OnClick() {
    if ($("#DisplayOnPos")[0].checked) {

    } else {

    }
}

function IsRawItem_OnClick() {

    //purchase rate enable
    var attr = $('#PurchaseRate').attr('readonly');
    if (typeof attr !== typeof undefined && attr !== false) {
        $('#PurchaseRate').removeAttr('readonly');
        $('#PurchaseRate').val('');
    }

    setItemType();
    if ($("#IsRawItem")[0].checked) {
        IsSubItemsTableInited = false;
        IsModifiersTableInited = false;
        //for deals/recipes
        $('#DisplayOnPos')[0].checked = false;
        $('#DisplayOnPos').attr('disabled', 'disabled');
        $('#IsDeal')[0].checked = false;
        $("#IsRecipe")[0].checked = false;
        $('#table-label').html("");
        $('#recipe-table-container').attr('hidden', 'hidden');
        $('#subitems-table tbody').html('<tr></tr>');
        $('#subitems-deleted-rows-table tbody').html('<tr></tr>');

        //for modifiers
        $('#addModifiersSwitch')[0].checked = false;
        $('#addModifiersSwitch').attr('disabled', 'disabled');
        $('#modifiers-table-container').attr('hidden', 'hidden');
        $('#modifiers-table tbody').html('<tr></tr>');
        $('#modifiers-deleted-rows-table').html('<tr></tr>');
    } else {
        $('#addModifiersSwitch').removeAttr('disabled');
        $('#DisplayOnPos').removeAttr('disabled');
    }
}

function IsDeal_OnClick() {
    setItemType();
    if ($("#IsDeal")[0].checked) {

        //purchase rate disable
        $('#PurchaseRate').attr('readonly', 'readonly');
        $('#PurchaseRate').val('0');

        //show subitems table
        $("div#recipe-table-container").removeAttr('hidden');
        InitSubItemsTable();

        $('#addModifiersSwitch').removeAttr('disabled');
        $('#DisplayOnPos').removeAttr('disabled');
        $('#IsRawItem')[0].checked = false;
        //$('#IsDeal')[0].checked = true;
        $("#IsRecipe")[0].checked = false;
        $('#table-label').html("Deal Items");
    } else {

        //purchase rate enable
        $('#PurchaseRate').removeAttr('readonly');
        $('#PurchaseRate').val('');

        //$("#isRawItemSwitch").removeAttr('disabled');
        if (!$("#IsRecipe")[0].checked) {
            //hide subitems table
            $("div#recipe-table-container").attr('hidden', 'hidden');
        }
        if (!$("#IsRecipe")[0].checked && !$('#IsDeal')[0].checked) {
            IsSubItemsTableInited = false;
        }
    }
}

function AddRecipe_OnClick() {
    setItemType();
    if ($("#IsRecipe")[0].checked) {

        //purchase rate disable
        $('#PurchaseRate').attr('readonly', 'readonly');
        $('#PurchaseRate').val('0');

        //show subitems table
        $("div#recipe-table-container").removeAttr('hidden');
        InitSubItemsTable();
        $('#addModifiersSwitch').removeAttr('disabled');
        $('#DisplayOnPos').removeAttr('disabled');
        $('#IsRawItem')[0].checked = false;
        $("#IsDeal")[0].checked = false;
        $('#table-label').html("Recipe Items");
    } else {
        //purchase rate enable
        $('#PurchaseRate').removeAttr('readonly');
        $('#PurchaseRate').val('');

        if (!$("#IsDeal")[0].checked) {
            //hide subitems table
            $("div#recipe-table-container").attr('hidden', 'hidden');
        }
        if (!$("#IsRecipe")[0].checked && !$('#IsDeal')[0].checked) {
            IsSubItemsTableInited = false;
        }
    }
}

function AddModifiers_OnClick() {
    if ($('#addModifiersSwitch')[0].checked) {
        $('#modifiers-table-container').removeAttr('hidden');
        if (!IsModifiersTableInited) {
            InitModifiersTable();
        }
    } else {
        $('#modifiers-table-container').attr('hidden', 'hidden');
    }
}

function MainCategory_OnChange() {
    var categoryId = $("#CategoryId").val();
    $('#SubCategoryId')
        .find('option')
        .remove()
        .end()
        .append('<option value="">Select Sub-Category</option>')
        .val('');
    if (categoryId != "") {
        $.ajax({
            url: '/SubCategory/GetSelectList/?categoryId=' + categoryId,
            type: 'GET',
            success: function (res) {
                //SUCCESS
                if (res.model != null) {
                    for (var i in res.model) {
                        var subCategory = res.model[i];
                        $('#SubCategoryId').append('<option value="' +
                            subCategory.value +
                            '">' +
                            subCategory.text +
                            '</option>');
                    }
                }
                $('#SubCategoryId').removeAttr('disabled');
                $("#btnAddSubCategory").removeClass("disabled");
                //console.log('response', res);
            },
            error: function () {
                sweetAlert.error({ text: 'An error occured while getting sub-categories.' });
            },
        });
    } else {

        $("#SubCategoryId").attr("disabled", "disabled");
        $("#btnAddSubCategory").addClass("disabled");

    }
}

//function Calculate_SalesIncOthers() {

//    //var result = 0;
//    //var salesRateBefore = 0;
//    //var discountAmount = 0;
//    ////var taxAmount = 0;
//    ////var taxInPercent = false;
//    //if ($("#SalesRate").val() != '') {
//    //    salesRateBefore = parseFloat($("#SalesRate").val());
//    //}
//    //if ($("#DiscountAmount").val() != '') {
//    //    discountAmount = parseFloat($("#DiscountAmount").val());
//    //}
//    ////if ($("#TaxId").val() != "") {
//    ////    taxAmount = parseFloat($("#TaxId option:selected")[0].getAttribute("data-tax-amount"));
//    ////    taxInPercent = ($("#TaxId option:selected")[0].getAttribute("data-is-in-percent") === 'True');; // CONVERT TO BOOL
//    ////}
//    //result = salesRateBefore;
//    //if (!$("#IsDiscountInPercentSwitch")[0].checked) {
//    //    result = result - discountAmount;
//    //} else {
//    //    //calculate discount in percent
//    //    var discountInPercent = (result * discountAmount) / 100;
//    //    result = result - discountInPercent;
//    //}

//    //apply tax
//    //console.log('taxInPercent type: ', typeof (taxInPercent));
//    //if (!taxInPercent) {
//    //    result = result + taxAmount;
//    //    //console.log('result with tax: ', result);
//    //}
//    //else {
//    //    //calculate tax in percentage
//    //    var taxInPercent = (result * taxAmount) / 100;
//    //    result = result + taxInPercent;
//    //}
//    //$("#FinalSalesRate").val(Number(result.toExponential(2)));
//}

function CalculatePurcahseRate() {
    //calculate purchase rate
    var rowsCount = $("table#subitems-table tbody tr").length;
    var purchaseRate = 0;
    for (var i = 0; i < rowsCount; i++) {
        var total = parseFloat($(`#InvItemRecipeChild_${i}__Total`).val());
        if (!isNaN(total)) {
            purchaseRate += total;
        }
    }

    $('#PurchaseRate').val(purchaseRate);
}

function CalculateSubItems_Total(rowNo) {
    var result = 0;
    var quantity = parseFloat($(`#InvItemRecipeChild_${rowNo}__Quantity`).val());
    var unitAmount = parseFloat($(`#InvItemRecipeChild_${rowNo}__SalesRate`).val());
    result = quantity * unitAmount;
    $(`#InvItemRecipeChild_${rowNo}__Total`).val(result);
    CalculatePurcahseRate();
}

function NameAttribute_OnChange() {

    var name = $('#Name').val();
    //name[0] for name
    //name[1] for size
    //name[2] for color
    //name[3] for brand

    if ($('#SizeId').val() != '') {
        name = name + '/' + $('#SizeId option:selected').html().trim();
    }
    if ($('#ColorId').val() != '') {
        name = name + '/' + $('#ColorId option:selected').html().trim();
    }
    if ($('#BrandId').val() != '') {
        name = name + '/' + $('#BrandId option:selected').html().trim();
    }

    $('#FullName').val(name);
}

function InitSubItemsTable() {
    var loader = `<tr id="loader">
                                                                    <td colspan = "7">
                                                                        <div class="d-flex align-items-center">
                                                                            <strong>Loading...</strong>
                                                                            <div class="spinner-border ml-auto" role="status" aria-hidden="true"></div>
                                                                        </div>
                                                                        </td>
                                                                        </tr>`;
    $("#subitems-table tbody").html(loader);
    $('table#subitems-deleted-rows-table tbody').html('<tr></tr>');
    getSubItemRow(0);

}

function getSubItemRow(rowNo) {
    setTimeout(function () {
        $.ajax({
            url: '/Items/GetAddItemRecipeRow?rowNo=' + rowNo + '&itemType=' + $('#ItemType').val(),
            type: 'GET',
            success: function (newRow) {
                $("#subitems-table tbody #loader").hide();
                $('#subitems-table tbody tr:last').after(newRow);
                $('.select').select2();
                $("#subitems-table tbody #loader").remove();
                IsSubItemsTableInited = true;
            },
            error: function (err) {
                //console.log(err);
            },
        });
    },
        150);


}

function AddSubItemsRow() {
    $('#btn-add-subitems-row').attr('disabled', 'disabled');
    var nextRowNo = parseInt($('#subitems-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    if ($("#IsDeal")[0].checked) {
        getSubItemRow(nextRowNo);
    } else {
        getSubItemRow(nextRowNo);
    }

    setTimeout(function () {
        $('#btn-add-subitems-row').removeAttr('disabled');
    },
        300);
}

function RemoveSubItemsRow(rowNo) {
    //console.log(rowNo)
    var rowSlector = 'table#subitems-table tbody tr.row-no-' + rowNo + ' ';


    $(rowSlector + " td .row-status").val(STATUSES.DELETED);
    var deletedRow = $(rowSlector).clone();
    $(deletedRow).removeClass('row-no-' + rowNo);
    $(deletedRow).addClass('del-row-no-' + rowNo);
    $('#subitems-deleted-rows-table tbody tr:last').after(deletedRow);
    $(rowSlector).remove();
    setTimeout(function () {
        CalculatePurcahseRate();
    },
        100);

    if ($("table#subitems-table tbody tr").length == 0) {
        $('#subitems-table tbody').html('<tr></tr>');
    }
}

function InitModifiersTable() {
    var loader = `<tr id="loader">
                                                                    <td colspan = "6">
                                                                        <div class="d-flex align-items-center">
                                                                            <strong>Loading...</strong>
                                                                            <div class="spinner-border ml-auto" role="status" aria-hidden="true"></div>
                                                                        </div>
                                                                        </td>
                                                                        </tr>`;
    $("#modifiers-table tbody").html(loader);
    $('table#modifiers-deleted-rows-table tbody').html('<tr></tr>');
    getModifierRow(0);

}

function getModifierRow(rowNo) {
    $.ajax({
        url: '/Items/GetAddItemModifierRow?rowNo=' + rowNo,
        type: 'GET',
        success: function (newRow) {
            $("#modifiers-table tbody #loader").hide();
            $('#modifiers-table tbody tr:last').after(newRow);
            $('.select').select2();
            $("#modifiers-table tbody #loader").remove();
            IsModifiersTableInited = true;
        },
        error: function (err) {
            //console.log(err);
        }
    });
}

function AddModifiersRow() {
    $('#btn-add-modifiers-row').attr('disabled', 'disabled');
    var nextRowNo = parseInt($('#modifiers-table tbody tr:last')[0].getAttribute("data-row-no")) + 1;
    getModifierRow(nextRowNo);
    setTimeout(function () {
        $('#btn-add-modifiers-row').removeAttr('disabled');
    },
        300);
}

function RemoveModifiersRow(rowNo) {
    //console.log(rowNo)
    var rowSlector = 'table#modifiers-table tbody tr.row-no-' + rowNo + ' ';

    if ($("table#modifiers-table tbody tr").length == 1) {
        $('#modifiers-table tbody tr:last').after('<tr></tr>');
    }
    $(rowSlector + " td .row-status").val(STATUSES.DELETED);
    var deletedRow = $(rowSlector).clone();
    $(deletedRow).removeClass('row-no-' + rowNo);
    $(deletedRow).addClass('del-row-no-' + rowNo);
    $('#modifiers-deleted-rows-table tbody tr:last').after(deletedRow);
    $(rowSlector).remove();
}

function ModifierSelect_OnChange(rowNo) {
    var ModifierCharges =
        $("#InvItemModifiers_" + rowNo + "__ModifierId option:selected")[0]
            .getAttribute('data-ModifierCharges');
    $("#InvItemModifiers_" + rowNo + "__ModifierCharges").val(ModifierCharges);
    CalculateModifiers_Total(rowNo);
}

function CalculateModifiers_Total(rowNo) {
    var result = 0;
    var quantity = parseFloat($(`#InvItemModifiers_${rowNo}__Quantity`).val());
    var modifierCharges = parseFloat($(`#InvItemModifiers_${rowNo}__ModifierCharges`).val());
    result = quantity * modifierCharges;
    $(`#InvItemModifiers_${rowNo}__Total`).val(result);
}


var flag = true;

function ItemBarCodeSelect_OnChange(rowNo) {
    if (flag) {
        var ItemId =
            $("#InvItemRecipeChild_" + rowNo + "__BarCodeId option:selected")[0].getAttribute('data-ItemId');
        $('#InvItemRecipeChild_' + rowNo + '__ItemId').val(ItemId);
        flag = false;
        $('#InvItemRecipeChild_' + rowNo + '__ItemId').trigger('change.select2');
        flag = true;
    }
}

function ItemSelect_OnChange(rowNo) {
    var SalesRate =
        $("#InvItemRecipeChild_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-purchaseRate');
    var Measurement =
        $("#InvItemRecipeChild_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-itemMeasurement');
    var Unit = $("#InvItemRecipeChild_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-itemUnit');
    $("#InvItemRecipeChild_" + rowNo + "__Measurement").val(Measurement + ' ' + Unit);
    $("#InvItemRecipeChild_" + rowNo + "__SalesRate").val(SalesRate);
    CalculateSubItems_Total(rowNo);
    if (flag) {
        var ItemBarCode =
            $("#InvItemRecipeChild_" + rowNo + "__ItemId option:selected")[0].getAttribute('data-ItemBrCode');

        if (ItemBarCode != null) {
            ItemBarCode = ItemBarCode.trim();
        }
        flag = false;
        if (ItemBarCode !== "") {
            var ItemBarCodeId =
                $("#InvItemRecipeChild_" + rowNo + "__BarCodeId option:contains(" + ItemBarCode + ")").val();
            $("#InvItemRecipeChild_" + rowNo + "__BarCodeId").val(ItemBarCodeId);

            $("#InvItemRecipeChild_" + rowNo + "__BarCodeId").trigger('change.select2');
        } else {
            $("#InvItemRecipeChild_" + rowNo + "__BarCodeId").val("");
            $("#InvItemRecipeChild_" + rowNo + "__BarCodeId").trigger('change.select2');
        }
        flag = true;
    }
}