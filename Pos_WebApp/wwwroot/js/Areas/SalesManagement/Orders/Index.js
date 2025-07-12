function LoadData() {
    //var filter = {
    //    StartDate: $('#StartDate').val().toJSON(),
    //    EndDate: $('#StartDate').val().toJSON()
    //}
    $.ajax({
        url: '/Orders/GetOrders',
        type: 'POST',
        data: $('form').serialize(),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            //SUCCESS
            var _response = res;
            console.log('response', _response);
            if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                //add data to table
                var table = $('#sales-report-table').DataTable();
                for (var i = 0; i < _response.model.length; i++) {
                    var row = _response.model[i];
                    table.row.add([row.id, row.orderNo, row.status, row.createdOn, row.modifiedOn,]).draw(false);
                }
            } else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
                sweetAlert.error({ text: _response.errorMessage });
            } else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
                sweetAlert.error({ text: _response.responseMessage });
            }
        },
        error: function (error) {
            console.log(error);
            sweetAlert.error({ text: 'An error Occured while loading Orders.' });
        },
    });
}

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
                } else if (_response.errorCode == CODES.RESPONSE_CODE_ERROR) {
                    sweetAlert.error({ text: _response.errorMessage });
                } else if (_response.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
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