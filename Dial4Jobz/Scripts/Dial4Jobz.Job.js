Dial4Jobz.Job = {};

var root = "";

$(document).ready(function () {

   
    $("form #GeneralShift").attr('checked', true);

    $(".delete").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
            $('#UserName').watermark("Username");
        }
    });

    $("#RequirementsContactNumber").blur(function () {
        var rnumber = $('#RequirementsContactNumber').val();
        if (rnumber.length == 0) {
            $('#RequirementsContactNumber').next('div.red').remove();
            $('#RequirementsContactNumber').after('<div class="red">Mobile No is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

//    $("#RequirementsContactNumber").keydown(function (e) {

//        $('#RequirementsContactNumber').next('div.red').remove();
//        // Allow: backspace, delete, tab, escape, enter and .                  // Allow: Ctrl+A                     // Allow: home, end, left, right
//        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
//            // let it happen, don't do anything
//            return;
//        }
//        // Ensure that it is a number and stop the keypress
//        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

//            $('#RequirementsContactNumber').next('div.red').remove();
//            $('#RequirementsContactNumber').after('<div class="red">Please Enter Number Only.</div>');
//            e.preventDefault();
//        } else {

//            $('#RequirementsContactNumber').next('div.red').remove();
//            var rnumber = $('#RequirementsMobileNumber').val();
//            if (rnumber.length >= 10) {
//                $('#RequirementsContactNumber').next('div.red').remove();
//                $('#RequirementsContactNumber').after('<div class="red">Allowed 10 Digits Only.</div>');
//                e.preventDefault();
//            }
//        }
//    });

    //Phone Number validation
    $("#RequirementsMobileNumber").blur(function () {
        var rnumber = $('#RequirementsMobileNumber').val();
        if (rnumber.length == 0) {
            $('#RequirementsMobileNumber').next('div.red').remove();
            $('#RequirementsMobileNumber').after('<div class="red">Mobile No is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $("#RequirementsMobileNumber").keydown(function (e) {

        $('#RequirementsMobileNumber').next('div.red').remove();
        // Allow: backspace, delete, tab, escape, enter and .                  // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

            //            debugger;
            $('#RequirementsMobileNumber').next('div.red').remove();
            $('#RequirementsMobileNumber').after('<div class="red">Please Enter Number Only.</div>');
            e.preventDefault();
        } else {

            $('#RequirementsMobileNumber').next('div.red').remove();
            var rnumber = $('#RequirementsMobileNumber').val();
            if (rnumber.length >= 10) {
                $('#RequirementsMobileNumber').next('div.red').remove();
                $('#RequirementsMobileNumber').after('<div class="red">Allowed 10 Digits Only.</div>');
                e.preventDefault();
            }
        }
    });



    $("#Position").focus();

    $("form #BasicTerms").attr('checked', true);
    // Fields validations

    $("#Position").blur(function () {
        var name = $('#Position').val();
        if (name.length == 0) {
            $('#Position').next('div.red').remove();
            $('#Position').after('<div class="red">Position is Required</div>');
        } else {
            $(this).next('div.red').remove();
            return true;
        }
    });

    $('#Functions').blur(function () {
        var maritalstatus = $('#Functions').val();
        if (maritalstatus.length == 0) {
            $('#Functions').next('div.red').remove();
            $('#Functions').after('<div class="red">Function is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });


    $("#RequirementsContactPerson").blur(function () {
        var name = $('#RequirementsContactPerson').val();
        if (name.length == 0) {
            $('#RequirementsContactPerson').next('div.red').remove();
            $('#RequirementsContactPerson').after('<div class="red">ContactPerson Name is Required</div>');
        } else {
            $(this).next('div.red').remove();
            return true;
        }
    });

    $("#RequirementsMobileNumber").blur(function () {
        var pincode = $('#RequirementsMobileNumber').val();
        if (pincode.length != 10) {
            $('#RequirementsMobileNumber').next('div.red').remove();
            $('#RequirementsMobileNumber').after('<div class="red">Mobile Number should have 10 digits only</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });




    Dial4Jobz.Job.TogglePreferredTimeVisibility();

    if ($("#RequirementsMobileNumber") && $('#RequirementsEmailAddress').val() != '') {
        $('#CommunicationEmail').attr('checked', true);

    } else if ($('#RequirementsMobileNumber').val() != '') {
        $('#CommunicationSMS').attr('checked', true);
    }
    else {
        $('#CommunicationEmail').attr('checked', false);
        $('#CommunicationSMS').attr('checked', false);
    }

    $("form #BasicTerms").attr('checked', true);
    $("form #FullTime").attr('checked', true);

    $("#Any").click(function () {
        $('#Contract').attr('checked', this.checked);
        $('#PartTime').attr('checked', this.checked);
        $('#FullTime').attr('checked', this.checked);
        $('#WorkFromHome').attr('checked', this.checked);
    });

    $("#PreferredType input").click(function () {
        Dial4Jobz.Job.TogglePreferredTimeVisibility();
    });

    Dial4Jobz.Job.SetupWatermarks();
    Dial4Jobz.Job.SetupAddMultipleInputs();
    Dial4Jobz.Job.SetupTokenizerInputs();
    Dial4Jobz.Common.SetupLocationDropdowns('', '');
    Dial4Jobz.Common.SetupLocationDropdowns('Posting', '1');
    Dial4Jobz.Job.SetupTooltips();
    Dial4Jobz.Job.AddMoreLinkBehaviour();

    $("#DOB").datepicker();

    $('#Functions').change(function () {
        $("#Roles").show();
        $.ajax({
            url: "/Jobs/Roles",
            type: 'GET',
            data: { 'functionId': $(this).val() },
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    $('#Roles').children().remove();

                    var results = [];
                    // Converting the JSON object into an array
                    $.each(response.Roles, function (index, role) {
                        results.push(role);
                    });

                    // Sorting the array items by Id
                    results.sort(function (a, b) {
                        return a.Id - b.Id;
                    });

                    $.each(results, function (index, role) {
                        $('#Roles').append(
                            '<option value="' + role.Id + '">' +
                                role.Name +
                            '</option>');
                    });
                }
            },
            error: function (xhr, status, error) {

            }
        });
    });
});





$(document).ready(function () {
    $('#RequirementsMobileNumber').blur(function (e) {
        if (validatePhone('RequirementsMobileNumber')) {
            //alert('valid')
            $('#RequirementsMobileNumber').next('div.red').remove();
        }
        else {
            $('#RequirementsMobileNumber').after('<div class="red">Invalid Entry.Please Enter Numbers only.</div>');
            $('#RequirementsMobileNumber').focus();
        }
    });

});

function validatePhone(RequirementsMobileNumber) {
    var a = document.getElementById(RequirementsMobileNumber).value;
    var filter = /^[0-9-+]+$/;
    if (filter.test(a)) {
        return true;
    }
    else {
        return false;
    }
}

Dial4Jobz.Job.TogglePreferredTimeVisibility = function () {
    if ($('#PartTime').attr('checked') || $('#WorkFromHome').attr('checked')) {
        $("#PreferredTime").show();
    } else {
        $("#PreferredTime").hide();
    }
};

Dial4Jobz.Job.SetupWatermarks = function () {
    Dial4Jobz.Common.AddWatermark($('#BasicQualificationSpecialization1'), "Specialization");
    Dial4Jobz.Common.AddWatermark($('#PostGraduationSpecialization1'), "Specialization");
    Dial4Jobz.Common.AddWatermark($('#DoctorateSpecialization1'), "Specialization");
    //    Dial4Jobz.Common.AddWatermark($('#what'), "job title, skill, or company...");
    Dial4Jobz.Common.AddWatermark($('#where'), "city, state, or country...");
    Dial4Jobz.Common.AddWatermark($('#what'), "job title, skill, or company...");
   // Dial4Jobz.Common.AddWatermark($('#where'), "Enter your city here...");
    Dial4Jobz.Common.AddWatermark($('#Position'), "Enter Name of the Position");
    Dial4Jobz.Common.AddWatermark($('#Description'), "Add what you expect from candidate Example:The role requires the candidate to build relationship with customers or add job description & preferrences");
};

Dial4Jobz.Job.SetupTooltips = function () {
    $('#Name').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Position').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#ContactPerson').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#RequirementsContactPerson').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#RequirementsContactNumber').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#RequirementsMobileNumber').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#RequirementsEmailAddress').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Email').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Website').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#ContactNumber').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#MobileNumber').tipTip({ activation: "focus", defaultPosition: "right" });
};

Dial4Jobz.Job.SetupAddMultipleInputs = function () {
    Dial4Jobz.Common.AddMultipleInput('btnAddLocation', 'btnDelLocation', 'location-container', 'location-container', 'PostingCountry');
    Dial4Jobz.Common.AddMultipleInput('btnAddBasicQualification', 'btnDelBasicQualification', 'basic-qualification-container', 'basic-qualification-container', 'BasicQualificationDegree');
    Dial4Jobz.Common.AddMultipleInput('btnAddPostGraduation', 'btnDelPostGraduation', 'post-graduation-container', 'post-graduation-container', 'PostGraduationDegree');
    Dial4Jobz.Common.AddMultipleInput('btnAddDoctorate', 'btnDelDoctorate', 'doctorate-container', 'doctorate-container', 'DoctorateDegree');
};

Dial4Jobz.Job.SetupTokenizerInputs = function () {
    Dial4Jobz.Common.AddTokenizerInput("Languages", "/Languages/Get", $.parseJSON($("#LanguagesHidden").val()));
    Dial4Jobz.Common.AddTokenizerInput("Skills", "/Skills/Get", $.parseJSON($("#SkillsHidden").val()));
    // Dial4Jobz.Common.AddTokenizerInput("PreferredIndustries", "/Industries/Get");
};


Dial4Jobz.Job.Send = function (sender, sendMethod) {
    var form = $(sender).parent().parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
    Date = Date.apply;

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

Dial4Jobz.Job.SendApplyJob = function (sender, sendMethod, jobId) {
    var form = $(sender).parent().parent();

    var data = "sendMethod=" + sendMethod + "&jobId=" + jobId;
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



Dial4Jobz.Job.AdminAdd = function (sender) {
    var form = $(sender).parent();
    var data = form.serialize();
    var terms = $("input:checkbox[name=BasicTerms]:checked").val();
    if (terms != "true") {
        alert("Please agree to the terms and conditions to continue");
        return false;
    }
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


Dial4Jobz.Job.Add = function (sender) {debugger;
    var form = $(sender).parent();
    var data = form.serialize();
    var terms = $("input:checkbox[name=BasicTerms]:checked").val();
    if (terms != "true") {
        alert("Please agree to the terms and conditions to continue");
        return false;
    }
    $("#loading").show();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            Dial4Jobz.Common.ShowMessageBar(response.Message);
            $("#loading").hide();
            if (response.ReturnUrl != null) {
                window.location = response.ReturnUrl;
                $("#loading").hide();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
            $("#loading").hide();
        }
    });
    return false;
};

Dial4Jobz.Job.ActivateRatVacancy = function (sender, jobId) {
    debugger;
    var form = $(sender).parent();
    var data = form.serialize();
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: "/Jobs/ActivateRATVacancy?id=" + jobId,
        data: data + "&jobId=" + jobId,
        dataType: "json",
        success: function (response) {

            Dial4Jobz.Common.ShowMessageBar(response.Message);
            $("#loading").hide();
            if (response.ReturnUrl != null) {
                window.location = response.ReturnUrl;
                $("#loading").hide();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
            $("#loading").hide();
        }
    });
    return false;
};

Dial4Jobz.Job.Save = function (sender, jobId) {
    var form = $(sender).parent();
    var data = form.serialize();
    var terms = $("input:checkbox[name=BasicTerms]:checked").val();
    if (terms != "true") {
        alert("Please agree to the terms and conditions to continue");
        return false;
    }
    $("#loading").show();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data + "&jobId=" + jobId,
        dataType: "json",
        success: function (response) {

            Dial4Jobz.Common.ShowMessageBar(response.Message);
            $("#loading").hide();
            if (response.ReturnUrl != null) {
                window.location = response.ReturnUrl;
                $("#loading").hide();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
            $("#loading").hide();
        }
    });
    return false;
};

Dial4Jobz.Job.Delete = function (sender, jobId) {
    var form = $(sender).parent();
    var data = form.serialize();

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: "jobId=" - jobId,
        success: function (response) {

            $(jobId).remove();
        }
    });

    return false;

    //    $(this).dialog("close");
    //    $.post("", { id: DeleteJob }, DeleteSuccessfull);
};




Dial4Jobz.Job.AddMoreLinkBehaviour = function () {
    $('#moreLink').live("click", function () {
        $(this).html("<img src=\'" + root + "/Content/Images/ajax-loader.gif' border='0' />");
        $.get($(this).attr("href"), function (response) {
            $('#jobs #job-list').append($(response).find('#job-list').html());
            $('#jobs #moreLink').replaceWith($("#moreLink", response));
        });

        return false;
    });
};
