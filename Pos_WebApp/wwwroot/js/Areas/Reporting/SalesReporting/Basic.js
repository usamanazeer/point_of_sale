function ApplySelect2() {
    $('.select').select2();
}
$(document).ready(function () {
    ApplySelect2();
});

function GenerateReport() {

    $.ajax({
        url: '/SalesReporting/BasicSalesReport',
        type: 'POST',
        data: $('form').serialize(),
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            var report = res.model;
            console.log(res);
            if (res.responseCode == CODES.RESPONSE_CODE_OK || res.responseCode == CODES.RESPONSE_CODE_NOTFOUND) {
                var table = $('#sales-report-table').DataTable();
                table.clear().draw();
                var Title = "";
                if (report.dateGroupByFilter != null) {
                    if (report.dateGroupByFilter.toLowerCase() == "day") {
                        if (!(report.reportQuantityValue || report.reportQuantityValue == false)) {
                            Title = report.reportTitle ? report.reportTitle: "Sale Report By Days";
                        }
                        else {
                            Title = report.reportTitle ? report.reportTitle: "Sold Quantity Report By Days";
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
                    var titleText = `${Title} &nbsp;&nbsp; From: ${$("#StartDate").val()} to ${$("#EndDate").val()}`;
                    if ($('#OrderType').val() !== "") {
                        titleText += ` <br> Order Type: ${$("#OrderType option:selected").text()}`;
                    }
                    if ($('#PaymentType').val() !== "") {
                        titleText += ` <br> Payment Type: ${$("#PaymentType option:selected").text()}`;
                    }
                    $('title').html(titleText);
                    var total = 0;
                    table.clear().draw();
                    for (var i in report.salesDataList) {
                        for (var j in report.salesDataList[i]) {
                            var data = report.salesDataList[i][j];
                            total += parseFloat(data.totalSalesRounded);
                            table.row.add([data.formattedDate, data.totalSalesRounded]).draw(false);
                            //dataSet.push([data.formattedDate, data.totalSalesRounded]);
                        }
                    }
                    table.row.add(["Total", parseFloat(total).toNDecimalPlaces(2)]).draw(false);
                }
                $("#sales-report-table-container").show();
                RenderSalesReportGraph(report, 'sales-report');
            }
            else {
                sweetAlert.error({ text: 'An error Occured while generating report.' });
            }
        },
        error: function (err) {
            console.log(err);
            sweetAlert.error({ text: 'An error Occured while generating report.' });
        },
    });
}