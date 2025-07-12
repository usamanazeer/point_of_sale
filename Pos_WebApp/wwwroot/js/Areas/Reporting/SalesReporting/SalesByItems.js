function ApplySelect2() {
    $('.select').select2();
}
$(document).ready(function () {

    ApplySelect2();

    $("#TopSalesFilter").bind("change", function (e) {
        if (e.target.value != "") {
            $("#item-ids-select").val([]).change();
            $("#item-ids-select").attr("disabled", "disabled");
        }
        else {
            $("#item-ids-select").removeAttr("disabled"); $('.select').select2();
        }
    })
});

function GenerateReport() {

    if ($("#TopSalesFilter").val() === "") {
        $("#ItemIds").val($("#item-ids-select").val().toString());
    }
    if ($("#WaiterId").val() !== "") {
        $("#WaiterName").val($("#WaiterId option:selected").text());
    }
    //$("#WaiterIds").val($("#waiter-ids-select").val().toString());
    $.ajax({
        url: '/SalesReporting/SalesReportByItems',
        type: 'POST',
        data: $('form').serialize(),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {

            var report = res.model;
            var table = $('#sales-report-table').DataTable();
            table.clear().draw();
            $('#sales-report-table > thead > tr#waiter-row').remove();
            if (report.waiterId) {
                $('#sales-report-table > thead').prepend(`<tr id="waiter-row"><th colspan="2">Waiter:&nbsp; ${report.waiterName}</th></tr>`);
            }

            if (report.reportQuantityValue) {
                $('.th-value-type').html('Items Quantity');
            } else {
                $('.th-value-type').html('Sales Amount');
            }

            if (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_NOTFOUND) {
                var Title = "";

                if (report.dateGroupByFilter != null) {
                    if (report.dateGroupByFilter.toLowerCase() === "day") {
                        if (report.reportQuantityValue) {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Report By Days";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sale Report By Days";
                            
                        }
                    }
                    else if (report.dateGroupByFilter.toLowerCase() == "month") {
                        if (report.reportQuantityValue) {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Monthly Report";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sale Monthly Report";
                        }
                    }
                    else if (report.dateGroupByFilter.toLowerCase() == "year") {
                        if (report.reportQuantityValue) {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Yearly Report";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sale Yearly Report";
                        }
                    }
                    $('title').html(`${Title} &nbsp;&nbsp; From: ${$("#StartDate").val()} to ${$("#EndDate").val()}`);
                    //var dataSet = [];

                    for (var i = 0; i < report.salesDataList.length; i++) {
                        var total = 0;
                        for (var j = 0; j < report.salesDataList[i].length; j++) {
                            var data = report.salesDataList[i][j];
                            if (j == 0) {
                                var currentRow = table.row.add([data.itemName, null]).draw(false).nodes().to$().addClass('itemNameRow');
                            }
                            if (report.reportQuantityValue) {
                                total += parseFloat(data.totalQuantityRounded);
                                table.row.add([data.formattedDate, data.totalQuantityRounded]).draw(false);
                            }
                            else {
                                total += parseFloat(data.totalSalesRounded);
                                table.row.add([data.formattedDate, data.totalSalesRounded]).draw(false);
                                
                            }
                        }
                        table.row.add(["Total", parseFloat(total).toNDecimalPlaces(2)]).draw(false).nodes().to$().addClass('totalRow');
                        total = 0;
                        table.row.add(["", ""]).draw(false);
                    }
                }
                $("#sales-report-table-container").show();
                RenderSalesReportGraph(report, 'sales-report', res);
            }
            else {
                sweetAlert.error({ text: 'An error Occured while generating report.' });
            }
            //$('#sales-report-container').html(res);
        },
        error: function (err) {
            sweetAlert.error({ text: 'An error Occured while generating report.' });
        },
    });
}