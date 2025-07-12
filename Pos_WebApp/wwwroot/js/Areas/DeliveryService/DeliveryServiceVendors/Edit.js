bindSubmit('edit-delivery-service-form', 'btn-edit-delivery-service', '/DeliveryServiceVendors/Edit', false, true);
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
$('#isSelfSwitch').on('click', function (evt) {
    CheckForIsSelf();
});