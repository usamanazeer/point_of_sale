$('#btnOpenBulkUploadModal').on('click',
    function () {
        $('#bulkUploadModal').on('hidden.bs.modal',
            function () {
                $('#bulk-upload-form').trigger('reset');
                $('.file-format-error').hide();
                $('#btn-close-bulk-upload-modal').removeAttr('disabled');
                $('#btn-top-close-bulk-upload-modal').removeAttr('disabled');
            });
    });


bindSubmit('bulk-upload-form', //formId
    'btn-bulk-upload-submit', //submitBtnId
    '/Items/BulkUpload',//submitToUrl
    true, //resetFormOnSuccess
    null,//scrollToTopOnSuccess
    function (res) { //callAfterSuccess
        console.log(res);
        $('#btn-close-bulk-upload-modal').removeAttr('disabled');
        $('#btn-top-close-bulk-upload-modal').removeAttr('disabled');
        if (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_CREATED) {
            $('#btn-close-bulk-upload-modal').click();
        }
        if (res.model) {
            //$('#btn-bulk-upload-submit').attr('disabled', 'disabled');
            $('#btn-download-bulk-upload-response-file').attr('href', res.model);
            $('a#btn-download-bulk-upload-response-file')[0].click();
        }
    },
    function () { //callBeforeSubmit
        $('#btn-close-bulk-upload-modal').attr('disabled', 'disabled');
        $('#btn-top-close-bulk-upload-modal').attr('disabled', 'disabled');
    },
    function (error) { //callOnError
        $('#btn-close-bulk-upload-modal').removeAttr('disabled');
        $('#btn-top-close-bulk-upload-modal').removeAttr('disabled');
    },
    function () {//validationFunc
        if ($('#items-file').val().split('.')[1] !== 'csv') {
            $('.file-format-error').show();
            return false;
        }
        $('.file-format-error').hide();
        return true;
    },
    "/Items" //callBackUrl
);

function bindItemsDatatable(viewButton, editButton, deleteButton) {
    datatable = $('#tblItems')
        .DataTable({
            "sAjaxSource": "/Items/GetAll",
            "createdRow": function (row, data, dataIndex) {
                $(row).attr('id', `rowid-${data.id}`);
                //console.log(row, data, dataIndex);
            },
            "bServerSide": true,
            "bProcessing": true,
            "bSearchable": true,
            "order": [[1, 'asc']],
            "language": {
                "emptyTable": "No record found.",
                "processing":
                    '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
            },
            "columns": [
                {
                    "data": "itemCode",
                    "autoWidth": true,
                    "searchable": true,
                },
                {
                    "data": "fullName",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    "data": "categoryName",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    "data": "subCategoryName",
                    "autoWidth": true,
                    "searchable": true
                }, {
                    "data": "purchaseRate",
                    "autoWidth": true,
                    "searchable": true
                }, {
                    "data": "finalSalesRate",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        //console.log(data, type, row)
                        return `<img src="${data.imageUrl}" style="background-image: url('/imgs/Default.png');width:100px;" width="100" />`;
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        //console.log(data, type, row);
                        var buttons = '';
                        if (viewButton == 'True') {
                            buttons += `<a href="/Items/Details/${data.id}" class="info p-0" data-original-title="View" title="View">
                                            <i class="ft-eye font-medium-3 mr-2"></i>
                                        </a>`;
                        }
                        if (editButton == 'True') {
                            buttons += `<a class="success p-0 btn-edit-main" href="/Items/Edit/${data.id}" data-original-title="Edit" title="Edit">
                                            <i class="ft-edit-2 font-medium-3 mr-2"></i>
                                        </a>`;
                        }
                        if (deleteButton == 'True') {
                            buttons += `<a class="danger p-0 btn-delete-main" data-id="${data.id}" data-original-title="Delete" title="Delete">
                                            <i class="ft-x font-medium-3 mr-2"></i>
                                        </a>`;
                        }
                        return buttons;
                    }
                }
            ]
        });
    // onclick="DeleteEntity('${data.id}','Items','tblItems')"
    $('#tblItems').on("click", ".btn-delete-main", function () {
        var id = $(this).data("id");
        if (id != null) {
            sweetAlert.confirm({ showCancelButton: true }, function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: `Items/Delete/${id}`,
                        type: 'GET',
                        success: function (res) {
                            //SUCCESS
                            var _response = res;
                            console.log('response', _response);
                            if (_response.responseCode == CODES.RESPONSE_CODE_OK) {
                                sweetAlert.success({ title: 'Deleted!', text: _response.responseMessage });
                                //change row count
                                datatable.row($(this).parents('tr')).remove().draw(false);
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
    });
}