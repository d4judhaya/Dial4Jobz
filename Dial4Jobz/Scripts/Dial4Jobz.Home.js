Dial4Jobz.Home = {};

$(document).ready(function () {

    $(".homePopup").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
        }
    });

});


$(document).ready(function () {
    $(".homeLandline").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
        }
    });

});

$(document).ready(function () {
    $(".homeMobile").fancybox({
        'hideOnContentClick': false,
        'titleShow': false,
        'scrolling': 'no',
        'onComplete': function () {
        }
    });

});



//Dial4Jobz.Home.MobileNumberPopup = function (sender) {
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
//            Dial4Jobz.Common.ShowMessageBar(response.Message);

//            if (response.Success) {
//                $.fancybox.close();
//            }
//        },
//        error: function (xhr, status, error) {
//            Dial4Jobz.Common.ShowMessageBar(error);
//        }
//    });

//    return false;
//};



Dial4Jobz.Home.Send = function (sender, sendMethod) {
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

