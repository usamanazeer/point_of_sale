function UpdateTableOccupyStatus(tableId) {
    if (tableId != null) {
        $.ajax({
            url: '/DiningTables/ReleaseOrOccupy',
            type: 'POST',
            data: { Id: tableId, IsOccupied: $("#isInPercentSwitch:checked").prop("checked") },
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (res) {
                //SUCCESS
                var _response = res;
                console.log('response', _response);
                if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                    sweetAlert.success({ title: 'Success!', text: _response.responseMessage });
                }
                else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
                    sweetAlert.error({ text: _response.errorMessage });
                }
                else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
                    sweetAlert.error({ text: _response.responseMessage });
                }
            },
            error: function (error) {
                console.log(error);
                sweetAlert.error({ text: 'An error Occured while Releasing/Occupying Dining Table.' });
            }
        });
    }
}