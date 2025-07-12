$(document).ready(function () {

    //calculate final sales price here
    var rowsCount = $("table#add-stock-table tbody tr").length;
    for (var i = 0; i < rowsCount; i++) {
        CalculateSalesRateIncTax(i);
    }
});
function CalculateSalesRateIncTax(rowNo) {
    var salesRate = 0;
    var finalSalesRate = 0;
    var taxAmt = 0;
    var isTaxInPercent = false;
    if ($(".row-no-" + rowNo + " td .SalesRate").val() != "") {
        salesRate = parseFloat($(".row-no-" + rowNo + " td .SalesRate").val());
        finalSalesRate = salesRate;
    }
    if ($(".row-no-" + rowNo + " td .TaxAmount").val() != null) {
        taxAmt = parseFloat($(".row-no-" + rowNo + " td .TaxAmount").val());
        isTaxInPercent = $(".row-no-" + rowNo + " td .TaxIsInPercent")[0].checked;

        if (!isTaxInPercent) {
            finalSalesRate = finalSalesRate + taxAmt;
        }
        else {
            var taxAmtPercent = (taxAmt * salesRate) / 100;
            finalSalesRate = finalSalesRate + taxAmtPercent;
        }
    }
    if (finalSalesRate != null) {
        finalSalesRate = Number(finalSalesRate.toFixed(2));
    }
    console.log('sales rate: ', salesRate);
    console.log('Tax Amount: ', taxAmt);
    console.log('TaxIsInPercent: ', isTaxInPercent);
    console.log('SalesRateFinal: ', finalSalesRate);
    $(".row-no-" + rowNo + " td .SalesRateFinal").html(finalSalesRate);
}