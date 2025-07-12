function PrintReceipt(orderId) {

    if (orderId != null) {
        $.ajax({
            url: '/Orders/PrintReceipt/' + orderId,
            type: 'GET',
            success: function (res) {
                //SUCCESS
                var _response = res;
                if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                    sweetAlert.success({ title: 'Printed!', text: _response.responseMessage });
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
                sweetAlert.error({ text: 'An error Occured while Printing Receipt.' });
            },
        });
    }
}
function CancelOrder(orderId) {
    if (orderId != null) {
        sweetAlert.confirm({ showCancelButton: true }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: '/Orders/Cancel/' + orderId,
                    type: 'GET',
                    success: function (res) {
                        //SUCCESS
                        var _response = res;
                        console.log('response', _response);
                        if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                            $(".order-status").html('<span class="red">Cancelled</span>');
                            $(".btn-print-receipt").remove();
                            $(".btn-cancel-order").remove();
                            sweetAlert.success({ title: 'Cancelled!', text: _response.responseMessage });
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
                        sweetAlert.error({ text: 'An error Occured while Cancelling Order.' });
                    },
                });
            }
        });
    }
}