function RemoveVarified(transactionId) {
    $(`table#unverified-entries-table tr#unverified-entries-row-${transactionId}`).remove();
    var newCount = parseInt($(".unverified-entries-count").html()) - 1;
    $(".unverified-entries-count").html(newCount);
}
function VerifyJournalEntry(transactionId) {

    if (transactionId != null) {
        sweetAlert.confirm({ showCancelButton: true }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: `/Journal/VerifyJournalEntry/${transactionId}`,
                    type: 'GET',
                    success: function (res) {
                        //SUCCESS
                        var _response = res;
                        console.log('response', _response);
                        if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                            sweetAlert.success({ title: 'Success!', text: _response.responseMessage });
                            RemoveVarified(transactionId);
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
                        sweetAlert.error({ text: 'An error Occured while verifying journal Entry.' });
                    }
                });
            }
        });
    }
}