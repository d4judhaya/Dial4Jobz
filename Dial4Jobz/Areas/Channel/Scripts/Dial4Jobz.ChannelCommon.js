var Dial4Jobz = {};

Dial4Jobz.ChannelCommon = {};

Dial4Jobz.ChannelCommon.ShowMessageBar = function (message, time) {
    if (!time) {
        time = 4000;
    }

    $("<div />", { 'class': 'messageBar', text: message }).hide().prependTo("body")
      .slideDown('fast').delay(time).slideUp(function () { $(this).remove(); });
}