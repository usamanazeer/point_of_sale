function LoadLastWeekSalesReport(testParam) {
    
    var endDate = new Date();
    var startDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() -6);
    var reportParams = {
        StartDate: startDate.toJSON(),
        EndDate: endDate.toJSON(),
        DateGroupByFilter: "day"
    }; 
    $.ajax({
        url: '/SalesReporting/BasicSalesReport',
        type: 'POST',
        data: reportParams,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            //console.log('report:', res);
            RenderSalesReportGraph(res.model, 'last-week-sales-report');
            //$('#last-week-sales-report').html(res);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function LoadLastSixMonthsSalesReport() {
    var endDate = new Date();
    var startDate = new Date(endDate.getFullYear(), endDate.getMonth() - 5, 1);
    //console.log(startDate);
    var reportParams = {
        StartDate: startDate.toJSON(),
        EndDate: endDate.toJSON(),
        DateGroupByFilter:"month"
    }; 
    $.ajax({
        url: '/SalesReporting/BasicSalesReport',
        type: 'POST',
        data: reportParams,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            RenderSalesReportGraph(res.model, 'last-sixmonths-sales-report');
            //$('#last-sixmonths-sales-report').html(res);
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function LoadSalesAmountInWidget(startDate, endDate, orderType, paymentType, widgetClass) {
    
    var reportParams = {
        OrderType: orderType,
        PaymentType: paymentType,
        StartDate: startDate.toJSON(),
        EndDate: endDate.toJSON()
    };
    $.ajax({
        url: '/SalesReporting/GetSalesAmount',
        type: 'POST',
        data: reportParams,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res.responseCode == CODES.RESPONSE_CODE_OK) {
                //console.log();
                //console.log(`${widgetClass}: ${Number(res.model).toNDecimalPlaces(2)}`);
                $(`.${widgetClass}>h3>span.widget-amount`).html(Number(res.model).toNDecimalPlaces(2));
                //$('.current-month-sales-amount').html(res.model);
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function LoadUpcomingDues(startDate, endDate, tableId,callOnSuccess) {
    var reportParams = {
        FromDueDate: startDate != null? startDate.toJSON(): null,
        ToDueDate: endDate != null ? endDate.toJSON() : null,
        ExcludePaidBills: true
    };
    $.ajax({
        url: '/Bills/GetBillsByFilters',
        type: 'POST',
        data: reportParams,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            //console.log("GetBillsByFilters: ", res);
            $(`#${tableId} > tbody tr`).remove(); 
            if (res.responseCode === CODES.RESPONSE_CODE_NOTFOUND) {
                $(`#${tableId} > tbody`).append(`<tr><td colspan="7">No Upcoming Dues for Next 7 Days.</td></tr>`);
            }
            if (res.responseCode === CODES.RESPONSE_CODE_OK) {
                
                var rowsHtml = ``;
                if (res.model != null) {
                    for (var i = 0; i < res.model.length; i++) {
                        var bill = res.model[i];
                        bill.billDueDate = bill.billDueDate.toDate().toDateString();
                        rowsHtml += `<tr>
                        <td>${(i+1)}</td>
                        <td>${bill.billDueDate}</td>
                        <td>${bill.billNo}</td>
                        <td>${bill.billAmount.toNDecimalPlaces(2)}</td>
                        <td>${bill.remainingAmount.toNDecimalPlaces(2)}</td>
                        <td><span class="btn white btn-round btn-primary">${bill.billStatusText}</span></td>
                        <td><a href="/Bills/PayBill/${bill.id}" target="_blank" class="btn white btn-round btn-success">Pay</a></td>
                    </tr>`;
                    }
                }
                $(`#${tableId} > tbody`).append(rowsHtml);
                
            }
            if (callOnSuccess) {
                callOnSuccess(res);
            }
        },error: function (error) {
            console.error(error);
            $(`#${tableId} > tbody`).append(`<tr><td colspan="7" class="red">An Error Occured.</td></tr>`);
            }
        });
}
//function LoadTaxCollected(startDate, endDate, widgetClass) {
//    var reportParams = {
//        StartDate: startDate.toJSON(),
//        EndDate: endDate.toJSON()
//    };
//    $.ajax({
//        url: '/SalesReporting/GetTaxCollectedAmount',
//        type: 'POST',
//        data: reportParams,
//        headers: {
//            RequestVerificationToken:
//                $('input:hidden[name="__RequestVerificationToken"]').val()
//        },
//        success: function (res) {
//            if (res.responseCode == CODES.RESPONSE_CODE_OK) {
//                $(`.${widgetClass}>h3>span.widget-amount`).html(Number(res.model).toNDecimalPlaces(2));
//                //$('.current-month-sales-amount').html(res.model);
//            }
//        },
//        error: function (error) {
//            console.error(error);
//        }
//    });
//}