function ApplySelect2() {
    $('.select').select2();
}
$(document).ready(function () {

    ApplySelect2();

    $("#TopSalesFilter").bind("change", function (e) {
        if (e.target.value != "") {
            $("#delivery-services-select").val([]).change();
            $("#delivery-services-select").attr("disabled", "disabled");
        }
        else {
            $("#delivery-services-select").removeAttr("disabled"); $('.select').select2();
        }
    })
});

function GenerateReport() {

    if ($("#TopSalesFilter").val() == "") {
        $("#DeliveryServiceIds").val($("#delivery-services-select").val().toString());
    }
    $.ajax({
        url: '/SalesReporting/SalesByDeliveryServicesReport',
        type: 'POST',
        data: $('form').serialize(),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            var report = res.model;
            if (report.reportQuantityValue) {
                $('.th-value-type').html('Items Quantity');
            } else {
                $('.th-value-type').html('Sales Amount');
            }
            if (res.responseCode == CODES.RESPONSE_CODE_OK || res.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
                console.log(res.model);
                var Title = "";
                if (report.dateGroupByFilter != null) {
                    if (report.dateGroupByFilter.toLowerCase() == "day") {
                        if (!(report.reportQuantityValue || report.reportQuantityValue == false)) {
                            Title = report.reportTitle ? report.reportTitle : "Sale Report By Days";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Report By Days";
                        }
                    }
                    else if (report.dateGroupByFilter.toLowerCase() == "month") {
                        if (!(report.reportQuantityValue || report.reportQuantityValue == false)) {
                            Title = report.reportTitle ? report.reportTitle : "Sale Monthly Report";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Monthly Report";
                        }
                    }
                    else if (report.dateGroupByFilter.toLowerCase() == "year") {
                        if (!(report.reportQuantityValue || report.reportQuantityValue == false)) {
                            Title = report.reportTitle ? report.reportTitle : "Sale Yearly Report";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle : "Sold Quantity Yearly Report";
                        }
                    }
                    $('title').html(`${Title} &nbsp;&nbsp; From: ${$("#StartDate").val()} to ${$("#EndDate").val()}`);
                    //var dataSet = [];
                    var table = $('#sales-report-table').DataTable();
                    table.clear().draw();
                    for (var i = 0; i < report.salesDataList.length; i++) {
                        var total = 0;
                        for (var j = 0; j < report.salesDataList[i].length; j++) {
                            var data = report.salesDataList[i][j];
                            if (j == 0) {
                                var currentRow = table.row.add([data.itemName, null]).draw(false);

                            }
                            if (!(report.reportQuantityValue || report.reportQuantityValue == false)) {
                                total += parseFloat(data.totalSalesRounded);
                                table.row.add([data.formattedDate, data.totalSalesRounded]).draw(false);
                            }
                            else {
                                total += parseFloat(data.totalQuantityRounded);
                                table.row.add([data.formattedDate, data.totalQuantityRounded]).draw(false);
                            }
                        }
                        table.row.add(["Total", parseFloat(total).toNDecimalPlaces(2)]).draw(false);
                        total = 0;
                        table.row.add(["", ""]).draw(false);
                    }
                }
                $("#sales-report-table-container").show();
                RenderSalesReportGraph(report, 'sales-report');
            }
            else {
                console.log(res);
                sweetAlert.error({ text: 'An error Occured while generating report.' });
            }
            //$('#sales-report-container').html(res);
        },
        error: function (err) {
            console.log(err);
            sweetAlert.error({ text: 'An error Occured while generating report.' });
        },
    });
}