class AlertManager {
    static AlertSweetly(_res, callbackUrl) {
        
        var res = JSON.parse(JSON.stringify(_res), function (prop, value) {
            var lower = prop.charAt(0).toUpperCase() + prop.slice(1);
            if (prop === lower) return value;
            else this[lower] = value;
        });
        //console.log('response', _res);
        //console.log('AlertSweetly called.');
        if ((res.ResponseCode != 0 || res.ErrorCode != 0) || (res.responseCode != 0 || res.errorCode != 0)) {
            //console.log('AlertSweetly called for message.');
            if (res.ErrorOccured!=undefined) {
                res.errorOccured = res.ErrorOccured;
            }
            else if (res.errorOccured != undefined) {
                res.ErrorOccured = res.errorOccured;
            }
            if (!res.ErrorOccured || !res.errorOccured) {
                //console.log('AlertSweetly called for success message.');
                
                if ((res.ResponseCode == CODES.RESPONSE_CODE_OK || res.ResponseCode == CODES.RESPONSE_CODE_CREATED || res.ResponseCode == CODES.RESPONSE_CODE_UPDATED) || (res.responseCode == CODES.RESPONSE_CODE_CREATED || res.responseCode == CODES.RESPONSE_CODE_UPDATED)) {
                    //console.log('AlertSweetly called for success codes created(201) OR updated(600) OR OK(201).');
                    sweetAlert.success({ text: res.ResponseMessage }, function () {
                        if (callbackUrl) {
                            window.location.href = callbackUrl;
                        }
                    });
                }
                if (res.ResponseCode == CODES.RESPONSE_CODE_NOTFOUND ) {
                    sweetAlert.error({ text: res.ResponseMessage }, function () {
                        if (callbackUrl) {
                            window.location.href = callbackUrl;
                        }
                    });
                }
            }
            else {
                //console.log('AlertSweetly called for error message message.');
                //error occured
                if (res.ErrorCode == CODES.RESPONSE_CODE_ERROR || res.ErrorCode == CODES.RESPONSE_CODE_NOTFOUND) {
                    sweetAlert.error({ text: res.ErrorMessage });
                }
                else if (res.ResponseCode == CODES.RESPONSE_CODE_INVALIDSTATE) {
                    sweetAlert.error({ title: 'Invalid Info!', text: res.ResponseMessage });
                }
            }
        }
    }
}