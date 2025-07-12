function DeleteEntity(id, controllerName) {

    if (id != null) {
        sweetAlert.confirm({ showCancelButton: true }, function (isConfirm) {
            if (isConfirm) {

                $.ajax({
                    url: `${controllerName}/Delete/${id}`,
                    type: 'GET',
                    success: function (res) {
                        //SUCCESS
                        var _response = res;
                        if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                            sweetAlert.success({ title: 'Deleted!', text: _response.responseMessage });

                            //change row count
                            $("table#main-table tr#main-table-row-" + id).remove();
                            var newCount = parseInt($(".main-count").html()) - 1;
                            $(".main-count").html(newCount);

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
                        sweetAlert.error({ text: `An error Occured while deleting.` });
                    },
                });
            }
        });
    }
}