$('form#company-setup-form').submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    setButtonBusy('btn-company-setup');
    $.ajax({
        url: '/Account/CompanySetup',
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        headers: {
            RequestVerificationToken:
                $(`form#company-setup-form > input[name="__RequestVerificationToken"]`).val()
        },
        success: function (res) {
            if (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_UPDATED) {
                if ($('input:file').val() !== "") {
                    $(`#company-logo`).attr('src', res.model.logo);
                }

                AlertManager.AlertSweetly(res);
            } else {
                sweetAlert.error({ text: res.errorMessage });
            }
            setButtonFree('btn-company-setup', 'Save');
        },
        error: function (error) {
            console.log(error);
            sweetAlert.error({ text: 'An Error Occured.' });
            setButtonFree('btn-company-setup', 'Save');
        }
    });
});
$('form#setup-printers-form').submit(function (e) {
    e.preventDefault();
    setButtonBusy('btn-setup-printers');
    $.ajax({
        url: '/Setup/SetupPrinters',
        type: 'POST',
        data: $('form#setup-printers-form').serialize(),
        headers: {
            RequestVerificationToken:
                $(`form#setup-printers-form > input[name="__RequestVerificationToken"]`).val()
        },
        success: function (res) {
            if (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_UPDATED) {
                AlertManager.AlertSweetly(res);
            } else {
                sweetAlert.error({ text: res.errorMessage });
            }
            setButtonFree('btn-setup-printers', 'Save');
        },
        error: function (error) {
            console.log(error);
            sweetAlert.error({ text: 'An Error Occured.' });
            setButtonFree('btn-setup-printers', 'Save');
        }
    });
});