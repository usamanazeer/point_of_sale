$('form').submit(function (e) {
    var billAmount = Number($('#total-bill-amount-hidden').val());
    var totalPayment = Number($(e.target).find('#total-payment-hidden').val());
    if (totalPayment > billAmount) {
        e.preventDefault();
        sweetAlert.error({ text: 'Total Payment can not be greater than Bill Amount.' });
    }
});

function ResetPaymentForm(paymentType) {
    var tableId = `${paymentType}-payment-table`;
    $(`.payment-form:not(.${paymentType}-payment-form)`).trigger("reset");
    $(`AccBillPaymentDto_PaymentTypeId`).val(BILL_PAYMENT_TYPE[paymentType.toUpperCase()]);
    $(`table.${tableId}`).find('tr> td > span.cash-payment').html('0.00');
    $(`table.${tableId}`).find('tr> td > span.cheque-payment').html('0.00');
    $(`table.${tableId}`).find('tr> td > span.total-payment').html('0.00');
    var billAmount = Number($('#total-bill-amount-hidden').val());
    $(`table.${tableId}`).find('tr> td > span.remaining-amount').html(billAmount);
}

function AmountInput(paymentType) {
    var tableId = `${paymentType}-payment-table`;
    var billAmount = Number($('#total-bill-amount-hidden').val());
    if (paymentType === 'cash') {
        var cashAmount = Number($($('.cash-payment-cash-amount')[0]).val());
        if (!cashAmount.isNaN) {
            $(`table.${tableId}`).find('tr> td > span.total-payment')
                .html(cashAmount.toNDecimalPlaces(2));
            $(`table.${tableId}`).find('tr> td > span.remaining-amount')
                .html((billAmount - cashAmount).toNDecimalPlaces(2));
            $(`form.${paymentType}-payment-form`).find('#total-payment-hidden').val(cashAmount);
        }
    }
    if (paymentType === 'cheque') {
        var chequeAmount = Number($($('.cheque-payment-cheque-amount')[0]).val());
        if (!chequeAmount.isNaN) {
            $(`table.${tableId}`).find('tr> td > span.total-payment')
                .html(chequeAmount.toNDecimalPlaces(2));
            $(`table.${tableId}`).find('tr> td > span.remaining-amount')
                .html((billAmount - chequeAmount).toNDecimalPlaces(2));
            $(`form.${paymentType}-payment-form`).find('#total-payment-hidden').val(chequeAmount);
        }
    }
    if (paymentType === 'split') {
        var cashAmount = Number($($('.split-payment-cash-amount')[0]).val());
        var chequeAmount = Number($($('.split-payment-cheque-amount')[0]).val());
        var totalAmount = 0;
        if (!cashAmount.isNaN) {
            $(`table.${tableId}`).find('tr> td > span.cash-payment')
                .html(cashAmount.toNDecimalPlaces(2));
            totalAmount += cashAmount;
        }
        if (!chequeAmount.isNaN) {
            $(`table.${tableId}`).find('tr> td > span.cheque-payment')
                .html(chequeAmount.toNDecimalPlaces(2));
            totalAmount += chequeAmount;
        }
        $(`table.${tableId}`).find('tr> td > span.total-payment')
            .html(totalAmount.toNDecimalPlaces(2));
        $(`table.${tableId}`).find('tr> td > span.remaining-amount')
            .html((billAmount - totalAmount).toNDecimalPlaces(2));
        $(`form.${paymentType}-payment-form`).find('#total-payment-hidden').val(totalAmount);
    }
}