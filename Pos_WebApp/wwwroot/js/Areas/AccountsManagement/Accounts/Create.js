bindSubmit('create-account-form', 'btn-create-account', '/Accounts/Create', true, true);
$(document).ready(function () {
    filterAccountsList();

    $('#ParentId').change(function () {
        onParentAccChange();
    });

});
$('input[type="checkbox"]').on('click keyup keypress keydown', function (event) {
    if ($(this).is('[readonly]')) { return false; }
});
function onParentAccChange() {
    if ($('#ParentId').val() === "") {
        $('#IsParent').removeAttr("readonly", "readonly");
        return;
    }
    var hasNoChild = $('#ParentId').children('option:selected').data('acc-has-no-child');
    var hasParentChild = $('#ParentId').children('option:selected').data('acc-has-parent-child');
    var hasNonParentChild = $('#ParentId').children('option:selected').data('acc-has-non-parent-child');

    if (hasNoChild === "True") {
        $('#IsParent').removeAttr("readonly", "readonly");
        return;
    }

    if (hasParentChild === "True") {
        $('#IsParent').prop('checked', true);
        //$('#IsParent').attr("checked", "checked");
        $('#IsParent').attr("readonly", "readonly");
        return;
    }

    if (hasNonParentChild === "True") {
        $('#IsParent').prop('checked', false);
        //$('#IsParent').removeAttr("checked");
        $('#IsParent').attr("readonly", "readonly");
        return;
    }
}


function filterAccountsList() {

    $(`#ParentId`).val("");
    //console.log('filterAccountsList called!');
    onParentAccChange();
    var accountTypeId = $("#AccountTypeId").val();
    if (accountTypeId === "") {
        $('#ParentId').attr("disabled", "disabled");
    } else {
        $(`#ParentId > option[data-account-type-id!="${accountTypeId}"]`).css("display", "none");
        $(`#ParentId > option[data-account-type-id="${accountTypeId}"]`).css("display", "");
        $(`#ParentId > option[value=""]`).css("display", "");
        $('#ParentId').removeAttr("disabled");
    }
    onParentAccChange();
}