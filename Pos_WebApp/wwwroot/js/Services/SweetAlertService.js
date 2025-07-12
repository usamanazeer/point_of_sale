class sweetAlert {
    static confirm(params = null, thenCall) {
        var confirmButtonColor = '#0CC27E';
        var cancelButtonColor = '#FF586B';
        var confirmButtonText = 'Yes';
        var cancelButtonText = "No, cancel";
        var showCancelButton = false;
        var title = 'Are you sure?';
        var text = "You won't be able to revert this!";
        var type = 'warning';
        if (params == null)
        {
            params = {};
        }
        params.type = type;
        if (params.title == undefined) {
            params.title = title;
        }
        if (params.text == undefined) {
            params.text = text;
        }
        if (params.showCancelButton == undefined) {
            params.showCancelButton = showCancelButton;
        }
        if (params.cancelButtonText == undefined) {
            params.cancelButtonText = cancelButtonText;
        }
        if (params.confirmButtonText == undefined) {
            params.confirmButtonText = confirmButtonText;
        }
        if (params.cancelButtonColor == undefined) {
            params.cancelButtonColor = cancelButtonColor;
        }
        if (params.confirmButtonColor == undefined) {
            params.confirmButtonColor = confirmButtonColor;
        }

        //execute swal
        if (thenCall != undefined) {
            return swal(params).then(thenCall).catch(swal.noop);
        }
        else {
            return swal(params);
        }
    }
    static success(params = null, thenCall) {
        //console.log(typeof (params));
        var title = 'Suceess!';
        var text = "";
        var type = 'success';

        if (params == null) {
            params = {};
        }
        params.type = type;
        if (params.title == undefined) {
            params.title = title;
        }
        if (params.text == undefined) {
            params.text = text;
        }
        var s = swal(params).then(thenCall).catch(swal.noop);
        return s;
    }
    static error(params = null) {
        //console.log(typeof (params));
        var title = 'Error!';
        var text = "An error occured.";
        var type = 'error';

        if (params == null) {
            params = {};
        }
        params.type = type;
        if (params.title == undefined) {
            params.title = title;
        }
        if (params.text == undefined) {
            params.text = text;
        }
        return swal(params);
    }
}