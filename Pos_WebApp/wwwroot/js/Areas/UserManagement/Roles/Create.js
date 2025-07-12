function ApplySelect2() {
    $('#notification-types-select').select2();
}

function firstChild_Onclick(id) {
    if (!$("#parentRight-" + id).prop("checked"))//on unchecked
    {
        $(".child-of-id-" + id).prop("checked", false); //uncheck all childs
    }
}
function secondChild_Onclick(id, parentId) {
    if ($("#right-id-" + id).prop("checked")) //on checked
    {
        $("#right-id-" + parentId).prop("checked", true); //check all parents
    }
    else {
        $(".child-of-id-" + id).prop("checked", false); //else uncheck all childs
    }
}
function thirdChild_Onclick(id, parentId1, parentId2) {
    if ($("#right-id-" + id).prop("checked")) //on checked
    {
        $("#right-id-" + parentId1).prop("checked", true); //check all parents
        $("#right-id-" + parentId2).prop("checked", true);
    }
    else {
        $(".child-of-id-" + id).prop("checked", false); //else uncheck all childs
    }
}
function fourthChild_Onclick(id, parentId1, parentId2, parentId3) {
    if ($("#right-id-" + id).prop("checked")) //on checked
    {
        $("#right-id-" + parentId1).prop("checked", true); //check all parents
        $("#right-id-" + parentId2).prop("checked", true);
        $("#right-id-" + parentId3).prop("checked", true);
    }
}

function SaveRole() {
    var submitBtnId = 'btn-create-role';
    setButtonBusy(submitBtnId);
    $('#alert-container').html("");
    var role = {
        Name: $("#Name").val(),
        Description: $("#Description").val(),
        RoleRights: [],
        NotiRoleNotification: []
    }
    //add rights
    var selectedRights = $(".role-right:checked");
    for (var i = 0; i < selectedRights.length; i++) {
        var right = {
            RightId: selectedRights[i].getAttribute("data-id")
        }
        role.RoleRights.push(right);
    }
    //add notifications

    var notificationTypes = $("#notification-types-select").val();
    for (var i = 0; i < notificationTypes.length; i++) {
        var roleNoti = {
            NotificationTypeId: notificationTypes[i]
        };
        role.NotiRoleNotification.push(roleNoti);
    }
    role.__RequestVerificationToken = $('input[name ="__RequestVerificationToken"]').val();
    //ajax save request

    $.ajax({
        url: '/Roles/Create',
        type: 'POST',
        data: role,
        success: function (res) {
            //SUCCESS
            var _response = res;
            console.log('response', _response);

            if (_response.responseCode == CODES.RESPONSE_CODE_CREATED) {
                sweetAlert.success({ text: _response.responseMessage });
                //ShowNotificaion('success', 'Success', _response.responseMessage, '#alert-container');
            }
            else {
                sweetAlert.error({ text: _response.errorMessage });
                //ShowNotificaion('danger', 'Error', _response.errorMessage, '#alert-container');
            }
            $(`form#createRoleForm`).trigger('reset');
            window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
            setButtonFree(submitBtnId, 'Create');
        },
        error: function (error) {
            console.log(error);
            sweetAlert.error({ text: 'An error occured while creating role.' });
            //ShowNotificaion('danger', 'Error', 'An error occcured while creating role.', '#alert-container');
            setButtonFree(submitBtnId, 'Create');
        },
    });
}

$(document).ready(function () {
    ApplySelect2();

    $('#createRoleForm').submit(function (e) {

        e.preventDefault();
        SaveRole();
    });
});