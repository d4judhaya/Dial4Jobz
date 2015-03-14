Dial4Jobz.Auth = {};

$(document).ready(function () {

    //forms validation

    $(".login").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
            $('#UserName').watermark("Username");
            $('#Password').watermark("Password");
            $('#Email').watermark("Email");
            $('#ContactNumber').watermark("ContactNumber");
            $('#UserName').tipTip({ activation: "focus", defaultPosition: "left" });
            $('#Password').tipTip({ activation: "focus", defaultPosition: "left" });

            Dial4Jobz.Auth.AddSignupPopupBehavior();
            Dial4Jobz.Auth.AddForgotPasswordPopupBehavior();
            Dial4Jobz.Auth.AddChangeCandidatePasswordPopupBehavior();
            Dial4Jobz.Auth.AddChangeOrganizationPasswordPopupBehavior();

        }
    });

    Dial4Jobz.Auth.AddSignupPopupBehavior();
    Dial4Jobz.Auth.AddForgotPasswordPopupBehavior();
    Dial4Jobz.Auth.AddChangeCandidatePasswordPopupBehavior();
    Dial4Jobz.Auth.AddChangeOrganizationPasswordPopupBehavior();

});

Dial4Jobz.Auth.AddSignupPopupBehavior = function () {
    $(".signup").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
//            if (response.ReturnUrl != null) {
//                window.location = response.ReturnUrl;
//            }
            $('#UserName').watermark("Username / Email Address");
            $('#Email').watermark("Email Address");
            $('#MobileNumber').watermark("Mobile Number");
            $('#Password').watermark("Password");
            $('#ConfirmPassword').watermark("Repeat Password");
            $('#UserName').tipTip({ activation: "focus", defaultPosition: "left" });
            $('#Email').tipTip({ activation: "focus", defaultPosition: "left" });
            $('#Password').tipTip({ activation: "focus", defaultPosition: "right" });
            $('#ConfirmPassword').tipTip({ activation: "focus", defaultPosition: "right" });
            $('#MobileNumber').tipTip({ activation: "focus", defaultPosition: "left" });

            if ($('#Name').length > 0) {
                $('#Name').watermark("Enter the Name");
            }

            if ($('#ContactPerson').length > 0) {
                $('#ContactPerson').watermark("Contact Person");
            }
        }
    });

};

Dial4Jobz.Auth.ChangeAdminPassword = function () {
    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");
    var data = form.serialize();
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            Dial4Jobz.Common.ShowMessageBar(response.Message);

            if (response.Success) {
                $.fancybox.close();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};

Dial4Jobz.Auth.AddForgotPasswordPopupBehavior = function () {
    $(".forgotpassword").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
            $('#Email').watermark("Email Address");

        }
    });

};

Dial4Jobz.Auth.ForgotPassword = function (sender) {
    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");
    var data = form.serialize();
    var url = form.attr('action');
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            Dial4Jobz.Common.ShowMessageBar(response.Message);

            if (response.Success) {
                $.fancybox.close();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};

Dial4Jobz.Auth.AddChangeCandidatePasswordPopupBehavior = function () {
    $(".changecandidatepassword").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
            $('#Email').watermark("Email Address");

        }
    });

};

Dial4Jobz.Auth.ChangeCandidatePassword = function (sender) {
    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");
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
                $("#loading").hide();
                $.fancybox.close();
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message);
                $("#loading").hide();
                $.fancybox.close();
            }
//            Dial4Jobz.Common.ShowMessageBar(response.Message);

//            if (response.Success) {
//                $.fancybox.close();
//            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};

Dial4Jobz.Auth.AddChangeOrganizationPasswordPopupBehavior = function () {
    $(".changeorganizationpassword").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
            $('#Email').watermark("Email Address");

        }
    });

};

Dial4Jobz.Auth.ChangeOrganizationPassword = function (sender) {
   

    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");
    var data = form.serialize();
    var url = form.attr('action');
    $("#loading").show();
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            //Dial4Jobz.Common.ShowMessageBar(response.Message);
            if (response.Success) {
                location.href = response.ReturnUrl;
                $("#loading").hide();
                $.fancybox.close();
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message);
                $("#loading").hide();
                $.fancybox.close();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
            $("#loading").hide();
        }
    });

    return false;
    $("#loading").hide();
};




//Dial4Jobz.Auth.ChangePassword = function () {
//    var $popup = $("#fancybox-outer");
//    var form = $popup.find("form");

//    var data = form.serialize();
//    var url = form.attr('action');

//    $.ajax({
//        type: "POST",
//        url: url,
//        data: data,
//        dataType: "json",
//        success: function (response) {
//            if (response.Success) {
//                location.href = response.ReturnUrl;
//            } else {
//                Dial4Jobz.Common.ShowMessageBar(response.Message, 7000);
//            }
//        },
//        error: function (xhr, status, error) {
//            Dial4Jobz.Common.ShowMessageBar(error);
//        }
//    });

//    return false;
//};

Dial4Jobz.Auth.Login = function () {
    
    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");

    var data = form.serialize();
    var url = form.attr('action');
    $("#loading").show();
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.Success) {
                location.href = response.ReturnUrl;
                $("#loading").hide();
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message);
                $("#loading").hide();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};



Dial4Jobz.Auth.Register = function () {
    var $popup = $("#fancybox-outer");
    var form = $popup.find("form");

    var data = form.serialize();
    var url = form.attr('action');
    $("#loading").show();

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.Success) {
                location.href = response.ReturnUrl;
                $("#loading").hide();
            } else {
                Dial4Jobz.Common.ShowMessageBar(response.Message, 7000);
                $("#loading").hide();
            }
        },
        error: function (xhr, status, error) {
            Dial4Jobz.Common.ShowMessageBar(error);
        }
    });

    return false;
};

