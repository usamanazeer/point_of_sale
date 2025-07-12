bindSubmit('edit-account-form', 'btn-edit-account', '/Accounts/Edit', false, true);
$(document).ready(function () {
    if ($("#AccountTypeId").val() === "") {
        $('#ParentId').attr("disabled", "disabled");
    }

});

function filterAccountsList() {
    $(`#ParentId`).val("");
    console.log('filterAccountsList called!');
    var accountTypeId = $("#AccountTypeId").val();
    if (accountTypeId === "") {
        $('#ParentId').attr("disabled", "disabled");
        return;
    }
    $(`#ParentId > option[data-account-type-id!="${accountTypeId}"]`).css("display", "none");
    $(`#ParentId > option[data-account-type-id="${accountTypeId}"]`).css("display", "");
    $(`#ParentId > option[value=""]`).css("display", "");
    $('#ParentId').removeAttr("disabled");
}