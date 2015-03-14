Dial4Jobz.ChannelAuth = {};

$(document).ready(function () {
    $(".ActionPopup").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'yes'
    });
});

Dial4Jobz.ChannelAuth.Login = function () {
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
                Dial4Jobz.ChannelCommon.ShowMessageBar(response.Message);
            }
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.ChannelCommon.ShowMessageBar(error);
        }
    });

    return false;
};


Dial4Jobz.ChannelAuth.ForgotPassword = function (sender) {
    $("#loading").show();
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
            $("#loading").hide();
            Dial4Jobz.ChannelCommon.ShowMessageBar(response.Message);

            if (response.Success) {
                $.fancybox.close();
            }
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            Dial4Jobz.ChannelCommon.ShowMessageBar(error);
        }
    });

    return false;
};


