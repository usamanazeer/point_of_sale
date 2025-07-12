(function () {
    //LoadNotifications();
})();

function LoadNotifications() {
    $.ajax({
        url: '/Notifications/GetUserNotifications',
        type: 'GET',
        success: function (res) {
            //SUCCESS
            $("#notification-container").html(res);
        },
        error: function (error) {
            console.log(error);
        },
    });
}

function SetNotificationToSeen(notiId) {
    $.ajax({
        url: '/Notifications/SetNotificationToSeen/'+notiId,
        type: 'GET',
        success: function (res) {
            if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                $("#noti-id-" + notiId).remove();
            }
        },
        error: function (error) {
            console.log(error);
        },
    });
}
function OnOpenNotifications() {
    $("#notification-base-tab").click();
}