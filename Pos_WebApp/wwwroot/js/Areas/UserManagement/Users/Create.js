var formId = 'create-user-form';
var createUserUrl = '/Users/Create';
var submitBtnId = 'btn-create-user';
$(`form#${formId}`).submit(function (e) {
    e.preventDefault();
    setButtonBusy(submitBtnId);
    $.ajax({
        url: createUserUrl,
        type: 'POST',
        data: $(`form#${formId}`).serialize(),
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
            $(`form#${formId}`).trigger("reset");
            setButtonFree(submitBtnId, 'Create');
        },
        error: function (error) {
            console.log(error);
            sweetAlert.error({ text: 'An Error Occured.' });
            setButtonFree(submitBtnId, 'Create');
        }
    });
});