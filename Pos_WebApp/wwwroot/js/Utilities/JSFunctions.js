// ReSharper disable once NativeTypePrototypeExtending
Number.prototype.roundOff = function (places) {
    const r = +(Math.round(this + "e+" + places) + "e-" + places);
    if (isNaN(r)) {
        return 0;
    }
    return r;
}

function _HeightWidth(seconds = 5) {
    setTimeout(function () {
        const width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
        const height = (window.innerHeight > 0) ? window.innerHeight : screen.height;
        alert('w: ' + width + ", h: " + height);
    }, seconds * 1000);
}
function _SearchListItemByKeyValue(list, value, key = 'id') {
    var item = null;
    var index = null;
    for (let i = 0; i < list.length; i++) {
        if (list[i][key] == value) {
            index = i;
            item = list[i];
            break;
        }
    }
    return { index: index, item: item };
}
function _CopyObject(obj) {
    if (obj != undefined || obj != null) {
        return JSON.parse(JSON.stringify(obj));
    }
    return null;
}


function setButtonBusy(buttonId) {
    $(`#${buttonId}`).attr('disabled', 'disabled');
    $(`#${buttonId}`).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
}
function setButtonFree(buttonId, defaultText) {
    $(`#${buttonId}`).removeAttr('disabled');
    $(`#${buttonId}`).html(defaultText);
}
function bindSubmit(formId, submitBtnId, submitToUrl, resetFormOnSuccess, scrollToTopOnSuccess,callAfterSuccess,callBeforeSubmit,callOnError,validationFunc,callBackUrl) {

    $(`form#${formId}`).submit(function (e) {
        e.preventDefault();
        if (callBeforeSubmit) {
            callBeforeSubmit();
        }
        const formData = new FormData(this);
        var submitButtonText = $(`#${submitBtnId}`).html();
        setButtonBusy(submitBtnId);
        if (validationFunc) {
            if (validationFunc()) {
                submitForm();
            } else {
                setButtonFree(submitBtnId, submitButtonText);
            }
        } else {
            submitForm();
        }
        function submitForm() {
            $.ajax({
                url: submitToUrl,
                type: "POST",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                headers: {
                    RequestVerificationToken:
                        $(`form#${formId} > input[name="__RequestVerificationToken"]`).val()
                },
                success: function (res) {
                    console.log(res);
                    if (!res.errorOccured) {
                        AlertManager.AlertSweetly(res,callBackUrl);
                    } else {
                        sweetAlert.error({ text: res.errorMessage });
                    }

                    if (resetFormOnSuccess && (res.responseCode === CODES.RESPONSE_CODE_OK || res.responseCode === CODES.RESPONSE_CODE_CREATED)) {
                        $(`form#${formId}`).trigger("reset");
                    }
                    if (scrollToTopOnSuccess) {
                        window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
                    }

                    setButtonFree(submitBtnId, submitButtonText);
                    if (callAfterSuccess) {
                        callAfterSuccess(res);
                    }

                },
                error: function (error) {
                    console.log(error);
                    sweetAlert.error({ text: "An Error Occurred." });
                    setButtonFree(submitBtnId, submitButtonText);
                    if (callOnError) {
                        callOnError(error);
                    }
                }
            });
        }
    });   
}


// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.getMonthName = function () {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[this.getMonth()];
}
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.getMonthShortName = function () {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return months[this.getMonth()];
}
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.getDayName = function () {
    var dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return dayNames[this.getDay()];
}
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.getDayShortName = function () {
    var dayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    return dayNames[this.getDay()];
}

// ReSharper disable once NativeTypePrototypeExtending
Number.prototype.toNDecimalPlaces = function (n) {
    var numStr = String(this);
    if (numStr.includes('.')) {
        var newLen = numStr.indexOf('.') + (Number(n) + 1);
        var newNum = numStr.substring(0, newLen);
        return Number(newNum);
    }
    return Number(this);
}

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.toDate = function () {
    var strDate = this; 
    if (strDate.length === 19 && strDate.includes("-") && strDate.includes(":") && strDate.includes("T")) {
        var datetimeParts = strDate.split("-");
        var temp = datetimeParts[2].split("T");
        datetimeParts[2] = temp[0];
        temp = temp[1].split(":");
        datetimeParts.push(temp[0]);
        datetimeParts.push(temp[1]);
        datetimeParts.push(temp[2]);
        return new Date(Number(datetimeParts[0]), Number(datetimeParts[1] - 1), Number(datetimeParts[2]), Number(datetimeParts[2]), Number(datetimeParts[3]), Number(datetimeParts[4]));
    }
    return null;
}