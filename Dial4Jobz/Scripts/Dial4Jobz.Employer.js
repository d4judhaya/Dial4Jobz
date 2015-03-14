Dial4Jobz.Employer = {};

var root = "";

$(document).ready(function () {

    /***Note: Start Employer Type***/
    $("#ConsultantIndustries").hide();

    $("input[name='EmployerType']").change(function () {
        var employerType = $('input[name="EmployerType"]:checked').val();
        if (employerType == 1) {
            $("#ConsultantIndustries").show();
            $("#Industries").hide();
            $("#OwnershipType").hide();
            $("#NumberOfEmployees").hide();
        }
        else if (employerType == 2) {
            $("#Industries").show();
            $("#OwnershipType").show();
            $("#NumberOfEmployees").show();
            $("#ConsultantIndustries").hide();
        }
    });

    /***End Employer Type***/

    $(".callpickupcash").fancybox({
            fixed:false,
        'onComplete': function () {
            $('#UserName').watermark("Username");
        }
    });

    //Pincode Number Validation
    $("#Pincode").blur(function () {
        var pincode = $('#Pincode').val();
        if (pincode.length == 0) {
            $('#Pincode').next('div.red').remove();
            $('#Pincode').after('<div class="red">Pincode is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

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

    //datetime picker
    $("#DepositedOn").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "1930:1995"
    });

    $("#Name").focus();
    // Fields validations
    $("#Name").blur(function () {
        var name = $('#Name').val();
        if (name.length == 0) {
            $('#Name').after('<div class="red">Name is Required</div>');
        }
        else {
        }
    });


    Dial4Jobz.Employer.AddMoreLinkBehaviour();

    $('#Plans').change(function () {
        $.ajax({
            url: "/Employer/GetPlans",
            type: 'GET',
            data: { 'planId': $(this).val() },
            datatype: 'json',
            success: function (response) {
                if (response.Success) {
                    $('#Position').children().remove();

                    $.each(response.GetPlans, function (index, featuredEmployer) {
                        $('#Position').append(
                        '<option value="' + featuredEmployer.Id + '">' +
                            featuredEmployer.Position +
                         '</option>');
                    });
                }
            },
            error: function (xhr, status, error) {
            }
        });
    });
});

Dial4Jobz.Employer.Save = function (sender) {
    
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




Dial4Jobz.Employer.SaveAdmin = function (sender) {

    var form = $(sender).parent();
    var data = form.serialize();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};

Dial4Jobz.Employer.PhoneNoVerification = function (sender) {
    var form = $(sender).parent();
    var data = form.serialize();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.Success) {
                location.href = response.ReturnUrl;
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message, 7000);
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};

//Dial4Jobz.Employer.SetupTokenizerInputs = function () {
//    Dial4Jobz.Common.AddTokenizerInput("Name", "/Name/Get", $.parseJSON($("#NameHidden").val()));
//};


Dial4Jobz.Employer.AddMoreLinkBehaviour = function () {
    $('#moreLink').live("click", function () {
        $(this).html("<img src=\'" + root + "/Content/Images/ajax-loader.gif' border='0' />");
        $.get($(this).attr("href"), function (response) {
            $('#candidates #candidate-list').append($(response).find('#candidate-list').html());
            $('#candidates #moreLink').replaceWith($("#moreLink", response));
        });
        return false;
    });
};