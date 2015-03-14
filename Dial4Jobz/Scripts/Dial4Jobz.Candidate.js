Dial4Jobz.Candidate = {};

var root = "";

$(document).ready(function () {

    $(".callpickupcash").fancybox({
        fixed: false,
        'onComplete': function () {
            $('#UserName').watermark("Username");
        }
    });

    // Fields validations for candidate
    //$("form #FullTime").attr('checked', true);
    //$("form #GeneralShift").attr('checked', true);
    $("#Name").focus();

    $("#Name").blur(function () {
        var name = $('#Name').val();
        if (name.length == 0) {
            $('#Name').next('div.red').remove();
            $('#Name').after('<div class="red">Name is Required</div>');
        } else {
            $(this).next('div.red').remove();
            return true;
        }
    });

    $("#Pincode").keydown(function (e) {
        // Allow: backspace, delete, tab, escape and enter.                      // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

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
        // Allow: backspace, delete, tab, escape and enter.                  // Allow: Ctrl+A                     // Allow: home, end, left, right
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

            //            debugger;
            $('#MobileNumber').next('div.red').remove();
            $('#MobileNumber').after('<div class="red">Please Enter Number Only.</div>');
            e.preventDefault();
        } else {

            $('#MobileNumber').next('div.red').remove();

        }
    });

    //Contact Number validation
//    $("#ContactNumber").blur(function () {
//        var number = $('#ContactNumber').val();
//        if (number.length == 0) {
//            $('#ContactNumber').next('div.red').remove();
//            $('#ContactNumber').after('<div class="red">Mobile No is required</div>');

//        }
//        else {
//            $(this).next('div.red').remove()
//            return true;
//        }
//    });

//    $("#ContactNumber").keydown(function (e) {

//        $('#ContactNumber').next('div.red').remove();
//        // Allow: backspace, delete, tab, escape and enter.                  // Allow: Ctrl+A                     // Allow: home, end, left, right
//        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
//            // let it happen, don't do anything
//            return;
//        }
//        // Ensure that it is a number and stop the keypress
//        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

//            //            debugger;
//            $('#ContactNumber').next('div.red').remove();
//            $('#ContactNumber').after('<div class="red">Please Enter Number Only.</div>');
//            e.preventDefault();
//        } else {

//            $('#ContactNumber').next('div.red').remove();
//            var number = $('#ContactNumber').val();
//            if (number.length >= 10) {
//                $('#ContactNumber').next('div.red').remove();
//                $('#ContactNumber').after('<div class="red">Allowed 10 Digits Only.</div>');
//                e.preventDefault();
//            }
//        }
//    });


    $("#Address").blur(function () {
        var address = $('#Address').val();
        if (address.length == 0) {
            $('#Address').next('div.red').remove();
            $('#Address').after('<div class="red">Address is Required</div>');
        } else {
            $(this).next('div.red').remove();
            return true;
        }
    });

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


    $("#Position").blur(function () {
        var position = $('#Position').val();
        if (position.length == 0) {
            $('#Position').next('div.red').remove();
            $('#Position').after('<div class="red">Position is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $("#ContactNumber").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1) {
            return;
        }
        if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $("#MobileNumber").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1) {
            return;
        }
        if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });


    $('#MaritalStatus').blur(function () {
        var maritalstatus = $('#MaritalStatus').val();
        if (maritalstatus.length == 0) {
            $('#MaritalStatus').next('div.red').remove();
            $('#MaritalStatus').after('<div class="red">Select any one option</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $('#DOB').blur(function () {
        var dob = $('#DOB').val();
        if (dob.length == 0) {
            $('#DOB').next('div.red').remove();
            $('#DOB').after('<div class="red">DOB is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $('#CandidateFunctions').blur(function () {
        var candidateFunctions = $('#CandidateFunctions').val();
        if (candidateFunctions.length == 0) {
            $('#CandidateFunctions').next('div.red').remove();
            $('#CandidateFunctions').after('<div class="red">Function is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    $('#PreferredCountry1').blur(function () {
        alert("If you are not choosen preferred location ...your preferred location will be any in India");
    });

    $('#Industries').blur(function () {
        var industries = $('#Industries').val();
        if (industries.length == 0) {
            $('#Industries').next('div.red').remove();
            $('#Industries').after('<div class="red">Industries is required</div>');

        }
        else {
            $(this).next('div.red').remove()
            return true;
        }
    });

    //forms validations for candidates end

    Dial4Jobz.Candidate.TogglePreferredTimeVisibility();


    $("form #BasicTerms").attr('checked', true);
    $("#Any").click(function () {
        $('#Contract').attr('checked', this.checked);
        $('#PartTime').attr('checked', this.checked);
        $('#FullTime').attr('checked', this.checked);
        $('#WorkFromHome').attr('checked', this.checked);
    });

    $("#PreferredType input").click(function () {
        Dial4Jobz.Candidate.TogglePreferredTimeVisibility();
    });


    Dial4Jobz.Candidate.SetupWatermarks();
    Dial4Jobz.Candidate.SetupAddMultipleInputs();
    Dial4Jobz.Candidate.SetupTokenizerInputs();
    Dial4Jobz.Common.SetupLocationDropdowns('', '');
    Dial4Jobz.Common.SetupLocationDropdowns('Preferred', '1');
    Dial4Jobz.Candidate.SetupTooltips();
    Dial4Jobz.Candidate.AddMoreLinkBehaviour();
    Dial4Jobz.Candidate.AddUploadResumeBehavior();

  
    $("#DOB").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "1930:1997"
    });

   

    $('#CandidateFunctions').change(function () {
        $.ajax({
            url: "/Candidates/Roles",
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


    /*Get Functions by Roles*/

    $('#RolesFunction').change(function () {
        $.ajax({
            url: "/Candidates/GetFunctionsByRoles",
            type: 'GET',
            data: { 'roleId': $(this).val() },
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    $('#CandidateFunctionsRole').children().remove();

                    var results = [];
                    // Converting the JSON object into an array
                    $.each(response.Functions, function (index, func) {
                        results.push(func);
                    });

                    // Sorting the array items by Id
                    results.sort(function (a, b) {
                        return a.Id - b.Id;
                    });

                    $.each(results, function (index, func) {
                        $('#CandidateFunctionsRole').append(
            '<option value="' + func.Id + '">' +
                func.Name +
            '</option>');
                    });
                }
            },
            error: function (xhr, status, error) {

            }
        });
    });


    $('.qualification').live('change', function () {

        var specializationDll = $(this).next();

        if ($(this).val() == 0) {

            //$(this).next().val('');
            $(specializationDll).empty().append($('<option/>', { value: '', html: '--Select Specialization--' })).attr("disabled", "1");
        } else {


            $(specializationDll).empty().removeAttr("disabled"); //.append($('<option/>',{ value: '', html: '--Select Specialization--' }));
            //To Get Specialization 
            $.ajax({
                type: 'POST',
                url: '/Candidates/GetSpecialization',
                data: { degreeId: $(this).val() },
                success: function (data) {
                    if (data != null) {
                        $.each(data, function () {
                            specializationDll.append(
                                   $('<option/>', {
                                       value: this.Value,
                                       html: this.Text
                                   })
                        );
                        });
                    }
                },
                error: function (xhr, status, error) {

                }
            });
        }
    });
});


Dial4Jobz.Candidate.TogglePreferredTimeVisibility = function () {
    if ($('#PartTime').attr('checked') || $('#WorkFromHome').attr('checked')) {
        $("#PreferredTime").show();
    } else {
        $("#PreferredTime").hide();
    }
};


Dial4Jobz.Candidate.SendMatchingCandidate = function (sender, sendMethod, candidateId) {
    var form = $(sender).parent().parent();

    var data = "sendMethod=" + sendMethod + "&candidateid=" + candidateId;
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (response) {
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


Dial4Jobz.Candidate.SetupTooltips = function () {
    $('#Name').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Email').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Address').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#ContactNumber').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#MobileNumber').tipTip({ actiovation: "focus", defaultPosition: "right" });
    $('#Position').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#PresentCompany').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#PreviousCompany').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Pass').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Pincode').tipTip({ activation: "focus", defaultPosition: "right" });
    $('#Description').tipTip({ activation: "focus", defaultPosition: "right" });
};

Dial4Jobz.Candidate.SetupWatermarks = function () {
    Dial4Jobz.Common.AddWatermark($('#BasicQualificationSpecialization1'), "Specialization");
    Dial4Jobz.Common.AddWatermark($('#PostGraduationSpecialization1'), "Specialization");
    Dial4Jobz.Common.AddWatermark($('#DoctorateSpecialization1'), "Specialization");
    Dial4Jobz.Common.AddWatermark($('#what'), "Position, skill, or company...");
    Dial4Jobz.Common.AddWatermark($('#where'), "country,state,city..");
    Dial4Jobz.Common.AddWatermark($('#ContactNumber'), "Eg: 9999999999");
    Dial4Jobz.Common.AddWatermark($('#PassportNumber'), "Eg: ISO/IEC 7501-03:1997.");
    Dial4Jobz.Common.AddWatermark($('#LicenseNumber'), "Enter Your License Number ");
    Dial4Jobz.Common.AddWatermark($('#PresentCompany'), "XXX Pvt Ltd");
    Dial4Jobz.Common.AddWatermark($('#PreviousCompany'), "YYY Pvt Ltd");
    Dial4Jobz.Common.AddWatermark($('#Description'), "Describe your professional background and preferences");

};

Dial4Jobz.Candidate.SetupAddMultipleInputs = function () {
    Dial4Jobz.Common.AddMultipleInput('btnAddLocation', 'btnDelLocation', 'location-container', 'location-container', 'PreferredCountry');
    Dial4Jobz.Common.AddMultipleInput('btnAddBasicQualification', 'btnDelBasicQualification', 'basic-qualification-container', 'basic-qualification-container', 'BasicQualificationDegree');
    Dial4Jobz.Common.AddMultipleInput('btnAddPostGraduation', 'btnDelPostGraduation', 'post-graduation-container', 'post-graduation-container', 'PostGraduationDegree');
    Dial4Jobz.Common.AddMultipleInput('btnAddDoctorate', 'btnDelDoctorate', 'doctorate-container', 'doctorate-container', 'DoctorateDegree');
};


Dial4Jobz.Candidate.SetupTokenizerInputs = function () {
    Dial4Jobz.Common.AddTokenizerInput("Languages", "/Languages/Get", $.parseJSON($("#LanguagesHidden").val()));
    Dial4Jobz.Common.AddTokenizerInput("Skills", "/Skills/Get", $.parseJSON($("#SkillsHidden").val()));
    // Dial4Jobz.Common.AddTokenizerInput("PreferredIndustries", "/Industries/Get");
};


$(document).ready(function () {

    $(".popup").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'yes',
        'onComplete': function () {
            var g = 0;
            $("input:checkbox[class=candidate]:checked").each(function () {
                g = g + 1;
            });

            if (g == 0) {

                $("#ContactCandidateDiv").hide();
            }
            else {

                $("#NotSelectedCandidateDiv").hide();

                if ($("#sendmethod").val() == "0") {
                    $("#contactbtn").val("Send SMS");
                    $("#EmailDiv").hide();
                }
                else if ($("#sendmethod").val() == "1") {
                    $("#contactbtn").val("Send Email");
                    $("#SmsDiv").hide();
                }
                else if ($("#sendmethod").val() == "2") {
                    $("#contactbtn").val("Send SMS / Email");
                }
                Dial4Jobz.Candidate.ContactCandidatesWatermarks();
            }
        }
    });
});

Dial4Jobz.Candidate.ContactCandidatesWatermarks = function () {

    Dial4Jobz.Common.AddWatermark($('#InterviewDate'), "mention Date");
    Dial4Jobz.Common.AddWatermark($('#InterviewLocation'), "Time of Interview");
    Dial4Jobz.Common.AddWatermark($('#SmsVacancy'), "Title of Position or Vacancy");
    Dial4Jobz.Common.AddWatermark($('#SmsLocation'), "Location or area");

};

Dial4Jobz.Candidate.Sendmethod = function (sendMethod, candidateId) {
    
    $("#sendmethod").val(sendMethod);
    $("#candidateid").val(candidateId);
};

Dial4Jobz.Candidate.ViewSmsMsg = function () {
    var title = $("input:radio[name=Title]:checked").val();
    var message = "";
    if (title == "1") {
        alert("MSG thru Dial4jobz -044 - 44455566 - will you be interested for a vacancy in " + $("#SmsLocation").val() + " for " + $("#SmsVacancy").val() + " with " + $("#SmsCompanyName").val() + " contact(" + $("#SmsMobileNo").val() + ") or mailto (" + $("#SmsEmailId").val() + ").(" + $("#SmsContactPerson").val() + ")");
        message = "MSG thru Dial4jobz -044 - 44455566 - will you be interested for a vacancy in " + $("#SmsLocation").val() + " for " + $("#SmsVacancy").val() + " with " + $("#SmsCompanyName").val() + " contact(" + $("#SmsMobileNo").val() + ") or mailto (" + $("#SmsEmailId").val() + ").(" + $("#SmsContactPerson").val() + ")";
        document.getElementById("smsmessage").value = message;
        //document.write(smsmessage);
    }
    else if (title == "2") {
        alert("MSG thru Dial4jobz -044 - 44455566 - Ref ur cv u r shortlisted for Interview on " + $("#InterviewDate").val() + " at " + $("#InterviewLocation").val() + ", " +
                "Company : " + $("#InterviewCompany").val() + ", " +
                "Meet : " + $("#InterviewContactPerson").val() + ", " +
                "Mobile : " + $("#InterviewContactNo").val() + ", " +
                "Position : " + $("#InterviewPosition").val() + ", " +
                "Salary : " + $("#InterviewJobSalary").val() + ", " +
                "location : " + $("#InterviewJobLocation").val() + ", " +
                "Address : " + $("#InterviewJobAddress").val() + " Pls confirm ur presence");

        message = "MSG thru Dial4jobz -044 - 44455566 - Ref ur cv u r shortlisted for Interview on " + $("#InterviewDate").val() + " at " + $("#InterviewLocation").val() + ", " +
                "Company : " + $("#InterviewCompany").val() + ", " +
                "Meet : " + $("#InterviewContactPerson").val() + ", " +
                "Mobile : " + $("#InterviewContactNo").val() + ", " +
                "Position : " + $("#InterviewPosition").val() + ", " +
                "Salary : " + $("#InterviewJobSalary").val() + ", " +
                "location : " + $("#InterviewJobLocation").val() + ", " +
                "Address : " + $("#InterviewJobAddress").val() + " Pls confirm ur presence";
        // document.write(message);
        document.getElementById("smsmessage").value = message;
    }
};

/*Created new function for candidate/details page./By developer vignesh*/
Dial4Jobz.Candidate.SMSEmailForCandidates = function () {
    var candidateId = $("#candidateid").val();
    if ($("#sendmethod").val() == "0" || $("#sendmethod").val() == "2") {
        var title = $("input:radio[name=Title]:checked").val();

        if (title == "1") {

            if ($("#SmsLocation").val() == "") {
                alert("Please Enter Location");
                $("#SmsLocation").focus();
                return false;
            }

            if ($("#SmsVacancy").val() == "") {
                alert("Please Enter Vacancy");
                $("#SmsVacancy").focus();
                return false;
            }

            if ($("#SmsMobileNo").val() == "") {
                alert("Please Enter Mobile No");
                $("#SmsMobileNo").focus();
                return false;
            }


            if ($("#SmsContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#SmsContactPerson").focus();
                return false;
            }
        }
        else if (title == "2") {

            if ($("#InterviewDate").val() == "") {
                alert("Please Enter Interview Date");
                $("#InterviewDate").focus();
                return false;
            }
            if ($("#InterviewLocation").val() == "") {
                alert("Please Enter Time of Interview");
                $("#InterviewLocation").focus();
                return false;
            }
            if ($("#InterviewCompany").val() == "") {
                alert("Please Enter Company");
                $("#InterviewCompany").focus();
                return false;
            }
            if ($("#InterviewContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#InterviewContactPerson").focus();
                return false;
            }
            if ($("#InterviewContactNo").val() == "") {
                alert("Please Enter Contact No");
                $("#InterviewContactNo").focus();
                return false;
            }

            if ($("#InterviewContactNo").val() != "") {
                var InterviewContactNo = $("#InterviewContactNo").val()
                if (isNaN(InterviewContactNo)) {
                    alert("Please Enter Valid Contact No");
                    $("#InterviewContactNo").val("");
                    $("#InterviewContactNo").focus();
                    return false;
                }
            }

            if ($("#InterviewPosition").val() == "") {
                alert("Please Enter Position");
                $("#InterviewPosition").focus();
                return false;
            }
            if ($("#InterviewJobSalary").val() == "") {
                alert("Please Enter Salary Details");
                $("#InterviewJobSalary").focus();
                return false;
            }

            if ($("#InterviewJobLocation").val() == "") {
                alert("Please Enter Job Location");
                $("#InterviewJobLocation").focus();
                return false;
            }
            if ($("#InterviewJobAddress").val() == "") {
                alert("Please Enter Address");
                $("#InterviewJobAddress").focus();
                return false;
            }
        }

    }

    if ($("#sendmethod").val() == "1" || $("#sendmethod").val() == "2") {

        if ($("#EmployerEmail").val() == "") {
            alert("Please Enter Email Id");
            $("#EmployerEmail").focus();
            return false;
        }

        if ($("#EmployerEmail").val() != "") {
            var email = $("#EmployerEmail").val();
            var valid = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            if (!email.match(valid)) {
                alert("Please Enter Valid Email Id");
                $("#EmployerEmail").val("");
                $("#EmployerEmail").focus();
                return false;
            }
        }

        if ($("#Subject").val() == "") {
            alert("Please Enter Subject");
            $("#Subject").focus();
            return false;
        }
        if ($("#MinExperience").val() == "") {
            alert("Please Enter Minimum Experience");
            $("#MinExperience").focus();
            return false;
        }
        if ($("#MinExperience").val() != "") {
            var MinExperience = $("#MinExperience").val()
            if (isNaN(MinExperience)) {
                alert("Please Enter Valid Minimum Experience");
                $("#MinExperience").val("");
                $("#MinExperience").focus();
                return false;
            }
        }

        if ($("#MaxExperience").val() == "") {
            alert("Please Enter Maximum Experience");
            $("#MaxExperience").focus();
            return false;
        }

        if ($("#MaxExperience").val() != "") {
            var MaxExperience = $("#MaxExperience").val()
            if (isNaN(MaxExperience)) {
                alert("Please Enter Valid Maximum Experience");
                $("#MaxExperience").val("");
                $("#MaxExperience").focus();
                return false;
            }
        }

        if ($("#JobLocation").val() == "") {
            alert("Please Enter Job Location");
            $("#JobLocation").focus();
            return false;
        }

        if ($("#Message").val() == "") {
            alert("Please Enter Message");
            $("#Message").focus();
            return false;
        }

    }

    var candt = $("input:checkbox[name=SendToUser]:checked").val();
    var org = $("input:checkbox[name=SendToOrganization]:checked").val();

    $("#fancybox-loading").css("display", "block");
    var $popup = $("#fancybox-outer");
    var sendform = $popup.find("form");

    var senddata = sendform.serialize();
    //var url = form.attr('action');

    var form = $(".popup").parent();
    var data = form.serialize();
    if (data == null || data == "") {
        data = $("#details").find("select, textarea, input").serialize();
    }

    data = data + "&sendMethod=" + $("#sendmethod").val() + "&" + senddata;
        // var url = form.attr('action');

    $.ajax({
        type: "POST",
        url: "/Candidates/Send/" + candidateId,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#fancybox-overlay").css("display", "none");
            $("#fancybox-wrap").css("display", "none");
            $("#fancybox-loading").css("display", "none");
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


Dial4Jobz.Candidate.SendEmailSms = function () {
    if ($("#sendmethod").val() == "0" || $("#sendmethod").val() == "2") {
        var title = $("input:radio[name=Title]:checked").val();

        if (title == "1") {

            if ($("#SmsLocation").val() == "") {
                alert("Please Enter Location");
                $("#SmsLocation").focus();
                return false;
            }

            if ($("#SmsVacancy").val() == "") {
                alert("Please Enter Vacancy");
                $("#SmsVacancy").focus();
                return false;
            }

            if ($("#SmsMobileNo").val() == "") {
                alert("Please Enter Mobile No");
                $("#SmsMobileNo").focus();
                return false;
            }


            if ($("#SmsContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#SmsContactPerson").focus();
                return false;
            }
        }
        else if (title == "2") {

            if ($("#InterviewDate").val() == "") {
                alert("Please Enter Interview Date");
                $("#InterviewDate").focus();
                return false;
            }
            if ($("#InterviewLocation").val() == "") {
                alert("Please Enter Time of Interview");
                $("#InterviewLocation").focus();
                return false;
            }
            if ($("#InterviewCompany").val() == "") {
                alert("Please Enter Company");
                $("#InterviewCompany").focus();
                return false;
            }
            if ($("#InterviewContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#InterviewContactPerson").focus();
                return false;
            }
            if ($("#InterviewContactNo").val() == "") {
                alert("Please Enter Contact No");
                $("#InterviewContactNo").focus();
                return false;
            }

            if ($("#InterviewContactNo").val() != "") {
                var InterviewContactNo = $("#InterviewContactNo").val()
                if (isNaN(InterviewContactNo)) {
                    alert("Please Enter Valid Contact No");
                    $("#InterviewContactNo").val("");
                    $("#InterviewContactNo").focus();
                    return false;
                }
            }

            if ($("#InterviewPosition").val() == "") {
                alert("Please Enter Position");
                $("#InterviewPosition").focus();
                return false;
            }
            if ($("#InterviewJobSalary").val() == "") {
                alert("Please Enter Salary Details");
                $("#InterviewJobSalary").focus();
                return false;
            }

            if ($("#InterviewJobLocation").val() == "") {
                alert("Please Enter Job Location");
                $("#InterviewJobLocation").focus();
                return false;
            }
            if ($("#InterviewJobAddress").val() == "") {
                alert("Please Enter Address");
                $("#InterviewJobAddress").focus();
                return false;
            }
        }

    }

    if ($("#sendmethod").val() == "1" || $("#sendmethod").val() == "2") {
        
        if ($("#EmployerEmail").val() == "") {
            alert("Please Enter Email Id");
            $("#EmployerEmail").focus();
            return false;
        }

        if ($("#EmployerEmail").val() != "") {
            var email = $("#EmployerEmail").val();
            var valid = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            if (!email.match(valid)) {
                alert("Please Enter Valid Email Id");
                $("#EmployerEmail").val("");
                $("#EmployerEmail").focus();
                return false;
            }
        }

        if ($("#Subject").val() == "") {
            alert("Please Enter Subject");
            $("#Subject").focus();
            return false;
        }
        if ($("#MinExperience").val() == "") {
            alert("Please Enter Minimum Experience");
            $("#MinExperience").focus();
            return false;
        }
        if ($("#MinExperience").val() != "") {
            var MinExperience = $("#MinExperience").val()
            if (isNaN(MinExperience)) {
                alert("Please Enter Valid Minimum Experience");
                $("#MinExperience").val("");
                $("#MinExperience").focus();
                return false;
            }
        }

        if ($("#MaxExperience").val() == "") {
            alert("Please Enter Maximum Experience");
            $("#MaxExperience").focus();
            return false;
        }

        if ($("#MaxExperience").val() != "") {
            var MaxExperience = $("#MaxExperience").val()
            if (isNaN(MaxExperience)) {
                alert("Please Enter Valid Maximum Experience");
                $("#MaxExperience").val("");
                $("#MaxExperience").focus();
                return false;
            }
        }

        if ($("#JobLocation").val() == "") {
            alert("Please Enter Job Location");
            $("#JobLocation").focus();
            return false;
        }

        if ($("#Message").val() == "") {
            alert("Please Enter Message");
            $("#Message").focus();
            return false;
        }

    }

    var candt = $("input:checkbox[name=SendToUser]:checked").val();
    var org = $("input:checkbox[name=SendToOrganization]:checked").val();

    //    if (candt != "true" && org != "true") {
    //        //alert("Pls Select Atleast One Matching Result To");
    //        //return false;
    //    }

    $("#fancybox-loading").css("display", "block");
    var $popup = $("#fancybox-outer");
    var sendform = $popup.find("form");

    var senddata = sendform.serialize();
    //var url = form.attr('action');


    var form = $(".popup").parent();
    var data = form.serialize();
    if (data == null || data == "") {
         form = $(".popup").parent().parent();
         data = form.serialize();
    }
    data = data + "&sendMethod=" + $("#sendmethod").val() + "&" + senddata;
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#fancybox-overlay").css("display", "none");
            $("#fancybox-wrap").css("display", "none");
            $("#fancybox-loading").css("display", "none");
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};

Dial4Jobz.Candidate.SendSMSEmailSmsByConsultant = function () {
    
    if ($("#sendmethod").val() == "0" || $("#sendmethod").val() == "2") {
        var title = $("input:radio[name=Title]:checked").val();

        if (title == "1") {

            if ($("#SmsLocation").val() == "") {
                alert("Please Enter Location");
                $("#SmsLocation").focus();
                return false;
            }

            if ($("#SmsVacancy").val() == "") {
                alert("Please Enter Vacancy");
                $("#SmsVacancy").focus();
                return false;
            }

            if ($("#SmsMobileNo").val() == "") {
                alert("Please Enter Mobile No");
                $("#SmsMobileNo").focus();
                return false;
            }


            if ($("#SmsContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#SmsContactPerson").focus();
                return false;
            }
        }
        else if (title == "2") {

            if ($("#InterviewDate").val() == "") {
                alert("Please Enter Interview Date");
                $("#InterviewDate").focus();
                return false;
            }
            if ($("#InterviewLocation").val() == "") {
                alert("Please Enter Time of Interview");
                $("#InterviewLocation").focus();
                return false;
            }
            if ($("#InterviewCompany").val() == "") {
                alert("Please Enter Company");
                $("#InterviewCompany").focus();
                return false;
            }
            if ($("#InterviewContactPerson").val() == "") {
                alert("Please Enter Contact Person");
                $("#InterviewContactPerson").focus();
                return false;
            }
            if ($("#InterviewContactNo").val() == "") {
                alert("Please Enter Contact No");
                $("#InterviewContactNo").focus();
                return false;
            }

            if ($("#InterviewContactNo").val() != "") {
                var InterviewContactNo = $("#InterviewContactNo").val()
                if (isNaN(InterviewContactNo)) {
                    alert("Please Enter Valid Contact No");
                    $("#InterviewContactNo").val("");
                    $("#InterviewContactNo").focus();
                    return false;
                }
            }

            if ($("#InterviewPosition").val() == "") {
                alert("Please Enter Position");
                $("#InterviewPosition").focus();
                return false;
            }
            if ($("#InterviewJobSalary").val() == "") {
                alert("Please Enter Salary Details");
                $("#InterviewJobSalary").focus();
                return false;
            }

            if ($("#InterviewJobLocation").val() == "") {
                alert("Please Enter Job Location");
                $("#InterviewJobLocation").focus();
                return false;
            }
            if ($("#InterviewJobAddress").val() == "") {
                alert("Please Enter Address");
                $("#InterviewJobAddress").focus();
                return false;
            }
        }

    }

    if ($("#sendmethod").val() == "1" || $("#sendmethod").val() == "2") {

        if ($("#EmployerEmail").val() == "") {
            alert("Please Enter Email Id");
            $("#EmployerEmail").focus();
            return false;
        }

        if ($("#EmployerEmail").val() != "") {
            var email = $("#EmployerEmail").val();
            var valid = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            if (!email.match(valid)) {
                alert("Please Enter Valid Email Id");
                $("#EmployerEmail").val("");
                $("#EmployerEmail").focus();
                return false;
            }
        }

        if ($("#Subject").val() == "") {
            alert("Please Enter Subject");
            $("#Subject").focus();
            return false;
        }
        if ($("#MinExperience").val() == "") {
            alert("Please Enter Minimum Experience");
            $("#MinExperience").focus();
            return false;
        }
        if ($("#MinExperience").val() != "") {
            var MinExperience = $("#MinExperience").val()
            if (isNaN(MinExperience)) {
                alert("Please Enter Valid Minimum Experience");
                $("#MinExperience").val("");
                $("#MinExperience").focus();
                return false;
            }
        }

        if ($("#MaxExperience").val() == "") {
            alert("Please Enter Maximum Experience");
            $("#MaxExperience").focus();
            return false;
        }

        if ($("#MaxExperience").val() != "") {
            var MaxExperience = $("#MaxExperience").val()
            if (isNaN(MaxExperience)) {
                alert("Please Enter Valid Maximum Experience");
                $("#MaxExperience").val("");
                $("#MaxExperience").focus();
                return false;
            }
        }

        if ($("#JobLocation").val() == "") {
            alert("Please Enter Job Location");
            $("#JobLocation").focus();
            return false;
        }

        if ($("#Message").val() == "") {
            alert("Please Enter Message");
            $("#Message").focus();
            return false;
        }

    }

    var candt = $("input:checkbox[name=SendToUser]:checked").val();
    var org = $("input:checkbox[name=SendToOrganization]:checked").val();
    $("#fancybox-loading").css("display", "block");
    var $popup = $("#fancybox-outer");
    var sendform = $popup.find("form");

    var senddata = sendform.serialize();
    var form = $(".popup").parent();
    var data = form.serialize();
    if (data == null || data == "") {
        form = $(".popup").parent().parent();
        data = form.serialize();
    }
    data = data + "&sendMethod=" + $("#sendmethod").val() + "&" + senddata;

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: "/Candidates/SendByConsultant/" + candidateId,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#fancybox-overlay").css("display", "none");
            $("#fancybox-wrap").css("display", "none");
            $("#fancybox-loading").css("display", "none");
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


Dial4Jobz.Candidate.SendDetails = function (sender, sendMethod, candidateId) {
    var form = $(sender).parent().parent();
    var data = "sendMethod=" + sendMethod + "&candidateId=" + candidateId;
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: "/Candidates/Send/" + candidateId,
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



Dial4Jobz.Candidate.SendSmsDirect = function () {
    $("#fancybox-loading").css("display", "block");
    var $popup = $("#fancybox-outer");
    var sendform = $popup.find("form");

    var senddata = sendform.serialize();

    var form = $(".popup").parent();
    var data = form.serialize();

    data = data + "&sendMethod=" + $("#sendmethod").val() + "&" + senddata;

    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#fancybox-overlay").css("display", "none");
            $("#fancybox-wrap").css("display", "none");
            $("#fancybox-loading").css("display", "none");
            //$("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


//****************End query candidates details*************************

Dial4Jobz.Candidate.SendMatching = function (sender, sendMethod) {
    var candt = $("input:checkbox[name=Candidate]:checked").val();
    var org = $("input:checkbox[name=Organization]:checked").val();
    var g = 0;
    $("input:checkbox[class=Jobs]:checked").each(function () {
        g = g + 1;
    });
    if (g == 0) {
        alert("Pls Select Atleast One Jobs");
        return false;
    }

    if (candt != "true" && org != "true") {
        alert("Pls Select Atleast One Matching Result To");
        return false;
    }

    $("#loading").show();
    var form = $(sender).parent(); //.parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;

    var url = form.attr('action');

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


//************Direct Matching Jobs while Candidate save the profile

Dial4Jobz.Candidate.SendMatchingToOrganization = function (sender, sendMethod) {
    var candt = $("input:checkbox[name=Candidate]:checked").val();
    var org = $("input:checkbox[name=Organization]:checked").val();
    var g = 0;
    $("input:checkbox[class=Jobs]:checked").each(function () {
        g = g + 1;
    });
    if (g == 0) {
        alert("Pls Select Atleast One Jobs");
        return false;
    }

    if (candt != "true" && org != "true") {
        alert("Pls Select Atleast One Matching Result To");
        return false;
    }

    $("#loading").show();
    var form = $(sender).parent(); //.parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
    var candidateId = $("#HfCandidateId").val();

    var url = form.attr('action');

    $.ajax({
        type: "POST",
        //url: "/CandidateMatchesJob/Send/" + jobId,
        url: "/JobMatchesCandidate/Send/" + candidateId,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};

//************************End ******************



//***********Admin Employer send Matching Candidates****************

Dial4Jobz.Candidate.JobMatchingCandidates = function (sender, sendMethod) {
    
    var count = 0;
    $("input:checkbox[class=candidate]:checked").each(function () {
        count = count + 1;
    });
    if (count == 0) {
        alert("Pls Select Atleast One Candidate");
        return false;
    }

    var candt = $("input:checkbox[name=SendToUser]:checked").val();
    var org = $("input:checkbox[name=SendToOrganization]:checked").val();

    if (candt != "true" && org != "true") {
        alert("Pls Select Atleast One Matching Result To");
        return false;
    }

    $("#loading").show();
    var form = $(sender).parent(); //.parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
    //var jobId = $("#HfJobId").val();
    var url = form.attr('action');

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


//***********Direct Employer send Matching Candidates****************
Dial4Jobz.Candidate.JobMatchingCandidatesForEmployer = function (sender, sendMethod) {
 
    var count = 0;
    $("input:checkbox[class=candidate]:checked").each(function () {
        count = count + 1;
    });
    if (count == 0) {
        alert("Pls Select Atleast One Candidate");
        return false;
    }

    var candt = $("input:checkbox[name=SendToUser]:checked").val();
    var org = $("input:checkbox[name=SendToOrganization]:checked").val();

    if (candt != "true" && org != "true") {
        alert("Pls Select Atleast One Matching Result To");
        return false;
    }

    $("#loading").show();
    var form = $(sender).parent(); //.parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
    var jobId = $("#HfJobId").val();
    var url = form.attr('action');

    $.ajax({
        type: "POST",
        url: "/CandidateMatchesJob/Send/" + jobId,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(response.Message);
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.Common.ShowMessageBar(xhr.statusText);
        }
    });
    return false;
};


Dial4Jobz.Candidate.Send = function (sender, sendMethod) {
    var form = $(sender).parent().parent();

    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
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


Dial4Jobz.Candidate.Save = function (sender) {
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


Dial4Jobz.Candidate.PhVerification = function (sender, sendMethod) {
    var form = $(sender).parent();
    var data = form.serialize();
    data = data + "&sendMethod=" + sendMethod;
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
Dial4Jobz.Candidate.AdminPhoneNoVerification = function (sender) {
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



Dial4Jobz.Candidate.AddAdminUploadResumeBehavior = function () {

    $('#ResumeAdmin').uploadify({
        'swf': root + '/Content/Flash/uploadify.swf',
        'uploader': root + '/Admin/AdminHome/UploadResume',
        'cancelImg': root + '/Content/Images/uploadify-cancel.png',
        'auto': true,
        'multi': true,
        'data-ajax':false,
        'fileDesc': 'Image Files',
        'fileTypeExts': '*.doc;*.docx;*.jpeg;*.pdf',
        'queueSizeLimit': 90,
        'sizeLimit': 4000000,
        'buttonText': 'Upload Resume',
        'width': 200,
        'folder': root + '/uploads',
        'onComplete': function (event, queueID, fileObj, response, data) {
            Dial4Jobz.Common.ShowMessageBar("Resume has been uploaded.");
        },
        'onError': function (event, ID, fileObj, errorObj) {
            var msg;
            if (errorObj.type === "File Size")
                msg = 'File size cannot exceed 4MB';
            else
                msg = "An error occured while attempting to uploading resume."

            Dial4Jobz.Common.ShowMessageBar(msg);
            this.hide();
        }
    });
};


//Dial4Jobz.Candidate.AddUploadResumeBehavior = function () {
//    
//    $('#Resume').uploadify({
//        'swf': root + '/Content/Flash/uploadify.swf',
//        'uploader': root + '/Candidates/UploadResume',
//        'cancelImg': root + '/Content/Images/uploadify-cancel.png',
//        'auto': true,
//        'multi': true,
//        'fileDesc': 'Image Files',
//        'data-ajax': false,
//        //'fileExt': '*.jpg;*.png;*.gif;*.pdf;*.bmp;*.jpeg;*.doc',
//        'fileTypeExts': '*.doc;*.docx;*.jpeg;*.pdf',
//        'queueSizeLimit': 90,
//        'sizeLimit': 4000000,
//        'buttonText': 'Upload Resume',
//        'width': 200,
//        'folder': root + '/uploads',
//        'onComplete': function (event, queueID, fileObj, response, data) {
//            Dial4Jobz.Common.ShowMessageBar("Resume has been uploaded.");
//        },
//        'onError': function (event, ID, fileObj, errorObj) {
//            var msg;
//            if (errorObj.type === "File Size")
//                msg = 'File size cannot exceed 4MB';
//            else
//                msg = "An error occured while attempting to uploading resume."

//            Dial4Jobz.Common.ShowMessageBar(msg);
//            this.hide();
//        }
//    });
//};

Dial4Jobz.Candidate.AddUploadResumeBehavior = function () {

    var canId = $("#Id").val();

    $("#Resume").uploadify({
        swf: root + '/Content/Flash/uploadify.swf',
        uploader: root + '/Admin/AdminHome/UploadResume',
        cancelImg: root + '/Content/Images/uploadify-cancel.png',
        formData: { candId: canId },
        fileTypeDesc: '*.jpg; *.jpeg; *.doc; *.docx; *.pdf',
        fileTypeExts: '*.jpg; *.jpeg; *.doc; *.docx; *.pdf',
        buttonText: "Upload Resume",
        fileSizeLimit: "4MB",
        auto: true,
        onUploadSuccess: function (file) {
            // alert('The resume was successfully uploaded....');
            $('#Resume').next('span').remove();
            $('#Resume').after('<span>You have uploaded ' + file.name + ' successfully.</span>');
            $('#ResumeResult').hide();
        }

    });

};

//paging ajax
Dial4Jobz.Candidate.AddMoreLinkBehaviour = function () {
    $('#moreLink').live("click", function () {
        $(this).html("<img src=\'" + root + "/Content/Images/ajax-loader.gif' border='0' />");
        $.get($(this).attr("href"), function (response) {

            $('#jobs #job-list').append($(response).find('#job-list').html());
            $('#jobs #moreLink').replaceWith($("#moreLink", response));

            $('#candidates #candidate-list').append($(response).find('#candidate-list').html());
            $('#candidates #moreLink').replaceWith($("#moreLink", response));
        });
        return false;
    });
};
