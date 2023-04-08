var editCountryObj = editCountryObj || {};

$(document).ready(function () {




    editCountryObj.init = function (obj) {
        editCountryObj.onAjaxSuccess = obj.onAjaxSuccess;
        editCountryObj.onAjaxfailure = obj.onAjaxfailure;
    }


    editCountryObj.createOrUpdateState = function (form, event) {

        event.preventDefault();

        var formData = new FormData(form);

        $.ajax({
            url: form.action,
            method: form.method,
            data: formData,
            contentType: false,
            processData: false,
            success: editCountryObj.onAjaxSuccess,
            error: editCountryObj.onAjaxfailure
        })

    }



}, editCountryObj)



