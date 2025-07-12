bindSubmit('create-delivery-service-form', 'btn-create-delivery-service', '/DeliveryServiceVendors/Create', false, true,
    function (res) {
        if (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_CREATED) {
            Reset();
        }
    });

function Reset() {
    $('form#create-delivery-service-form').trigger('reset');
    $('.accountNoRow').css('display', 'block');
    $('#AccountNo').attr('required', 'required');
}

function CheckForIsSelf() {
    if ($('#isSelfSwitch')[0].checked) {
        $('.accountNoRow').css('display', 'none');
        $('#AccountNo').removeAttr('required');
    } else {
        $('.accountNoRow').css('display', 'block');
        $('#AccountNo').attr('required', 'required');
    }
}

CheckForIsSelf();
$('#isSelfSwitch').on('click',
    function (evt) {
        CheckForIsSelf();
    });