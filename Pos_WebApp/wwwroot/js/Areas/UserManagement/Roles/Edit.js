function ApplySelect2() {
    $('#notification-types-select').select2();
}

function firstChild_Onclick(id) {
    if (!$("#parentRight-" + id).prop("checked")) {
        $(".child-of-id-" + id).prop("checked", false);
    }
}
function secondChild_Onclick(id, parentId) {
    if ($("#right-id-" + id).prop("checked")) {
        $("#right-id-" + parentId).prop("checked", true);
    }
    else {
        $(".child-of-id-" + id).prop("checked", false);
    }
}
function thirdChild_Onclick(id, parentId1, parentId2) {
    if ($("#right-id-" + id).prop("checked")) {
        $("#right-id-" + parentId1).prop("checked", true);
        $("#right-id-" + parentId2).prop("checked", true);
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
function UpdateRole() {
    var submitBtnId = 'btn-edit-role';
    setButtonBusy(submitBtnId);
    $('#alert-container').html("");
    var role = {
        Id: $("#Id").val(),
        Name: $("#Name").val(),
        Description: $("#Description").val(),
        Status: $("#Status").val(),
        RoleRights: [],
        NotiRoleNotification: []
    }
    //add rights
    var selectedRights = $(".role-right:checked");
    for (var i = 0; i < selectedRights.length; i++) {
        var right = {
            RightId: selectedRights[i].getAttribute("data-id"),
            RoleId: role.Id,
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
        url: '/Roles/Edit/' + role.Id,
        type: 'POST',
        data: role,
        success: function (res) {
            //SUCCESS
            var _response = res;
            if (_response.responseCode == CODES.RESPONSE_CODE_UPDATED) {
                sweetAlert.success({ text: _response.responseMessage });
                //ShowNotificaion('success', 'Success', _response.responseMessage, '#alert-container');
            }
            else {
                sweetAlert.error({ text: _response.errorMessage });
                //ShowNotificaion('danger', 'Error', _response.errorMessage, '#alert-container');
            }
            setButtonFree(submitBtnId, 'Save');
        },
        error: function (err) {
            console.log(err);
            sweetAlert.error({ text: 'An error occured while updating role.' });
            setButtonBusy(submitBtnId, 'Save');
        }
    });
}

$(document).ready(function () {
    ApplySelect2();
    $('#updateRoleForm').submit(function (e) {

        e.preventDefault();
        UpdateRole();

    })
});