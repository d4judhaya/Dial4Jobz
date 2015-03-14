Dial4Jobz.Consultant = {};

var root = "";


$(document).ready(function () {
    $("#Pincode").keydown(function (e) {
        // Allow: backspace, delete, tab, escape and enter                   // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $("#ContactNumber").keydown(function (e) {
        $('#ContactNumber').next('div.red').remove();
        // Allow: backspace, delete, tab, escape, enter and .                  // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

            $('#ContactNumber').next('div.red').remove();
            $('#ContactNumber').after('<div class="red">Please Enter Number Only.</div>');
            e.preventDefault();
        }
    });

    //Phone Number validation
    $("#MobileNumber").blur(function () {
        var number = $('#MobileNumber').val();
        if (number.length == 0) {
            $('#MobileNumber').next('div.red').remove();
            $('#MobileNumber').after('<div class="red">Mobile No is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $("#MobileNumber").keydown(function (e) {

        $('#MobileNumber').next('div.red').remove();
        // Allow: backspace, delete, tab, escape, enter and .                  // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

            $('#MobileNumber').next('div.red').remove();
            $('#MobileNumber').after('<div class="red">Please Enter Number Only.</div>');
            e.preventDefault();
        } else {

            $('#MobileNumber').next('div.red').remove();
            var number = $('#MobileNumber').val();
            if (number.length >= 10) {
                $('#MobileNumber').next('div.red').remove();
                $('#MobileNumber').after('<div class="red">Allowed 10 Digits Only.</div>');
                e.preventDefault();
            }
        }
    });

});


Dial4Jobz.Consultant.Save = function (sender) {
    var form = $(sender).parent();
    var data = form.serialize();
    $("#loading").show();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {

            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
            if (response.ReturnUrl != null) {
                window.location = response.ReturnUrl;
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


Dial4Jobz.Consultant.Login = function () {
    
    $("#loading").show();
    var form = $("form");

    var data = form.serialize();
    var url = form.attr('action');

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();
            if (response.Success) {
                location.href = response.ReturnUrl;
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message);
            }
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};



Dial4Jobz.Consultant.Profile = function (sender) {

    var form = $(sender).parent();
    var data = form.serialize();
    $("#loading").show();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {

            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
            if (response.ReturnUrl != null) {
                window.location = response.ReturnUrl;
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};

