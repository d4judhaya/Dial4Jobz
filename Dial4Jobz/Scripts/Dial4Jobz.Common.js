var Dial4Jobz = {};

Dial4Jobz.Common = {};

$(document).ready(function () {

   // $("#Roles").hide();
    $("#SelectAll").click(function () {
        $('input:checkbox').each(function () {
            this.checked = true;
        });
        return false;
    });

    $("#SelectNone").click(function () {
        $('input:checkbox').each(function () {
            this.checked = false;
        });
        return false;
    });
   
});


Dial4Jobz.Common.AddWatermark = function (textbox, watermarktext) {
    
    textbox.watermark(watermarktext);
    //    textbox.addClass("watermark").val(watermarktext);

    //    textbox.focus(function () {
    //        $(this).filter(function () {
    //            return $(this).val() == "" || $(this).val() == watermarktext
    //        }).removeClass("watermark").val("");
    //    });

    //    textbox.blur(function () {
    //        $(this).filter(function () {
    //            return $(this).val() == ""
    //        }).addClass("watermark").val(watermarktext);
    //    });
};


Dial4Jobz.Common.SetupLocationDropdowns = function (prefix, suffix) {
    
    var countryId = $('#' + prefix + 'Country' + suffix);
    var stateId = $('#' + prefix + 'State' + suffix);
    var cityId = $('#' + prefix + 'City' + suffix);
    var regionId = $('#' + prefix + 'Region' + suffix);

    $(stateId).CascadingDropDown(countryId, '/Location/States',
    {
        //        promptText: 'Any...',
        promptText: '-Select State-',
        postData: function () {
            return { country: $(countryId).val() };
        }
    });
    $(cityId).CascadingDropDown(stateId, '/Location/Cities',
    {
        //        promptText: 'Any...',
        promptText: '-Select Cities-',
        postData: function () {
            return { state: $(stateId).val() };
        }
    });

    $(regionId).CascadingDropDown(cityId, '/Location/Regions',
    {
        //        promptText: 'Any...',
        promptText: '-Select Regions-',
        postData: function () {
            return { city: $(cityId).val() };
        }
    });
};

Dial4Jobz.Common.AddMultipleInput = function (btnAddId, btnDelId, inputContainerIdPrefix, inputContainerCss, firstChildInputIdPrefix) {
    if ($('.' + inputContainerCss).length < 2) {
        $('#' + btnDelId).attr('disabled', 'disabled');
    }

    $('#' + btnAddId).click(function () {
        var num = $('.' + inputContainerCss).length; // how many "duplicatable" input fields we currently have
        var newNum = new Number(num + 1); 	// the numeric ID of the new input field being added

        // create the new element via clone(), and manipulate it's ID using newNum value
        var newElem = $('#' + inputContainerIdPrefix + num).clone().attr('id', inputContainerIdPrefix + newNum);

        // manipulate the name/id values of the input inside the new element
        //newElem.children(':first').attr('id', firstChildInputIdPrefix + newNum).attr('name', firstChildInputIdPrefix + newNum);


        newElem.children().each(function () {
            var idPrefix = $(this).attr('id').substring(0, $(this).attr('id').length - 1);
            var namePrefix = $(this).attr('name').substring(0, $(this).attr('name').length - 1);
            $(this).attr('id', idPrefix + newNum).attr('name', namePrefix + newNum);
        })

        // insert the new element after the last "duplicatable" input field
        $('#' + inputContainerIdPrefix + num).after(newElem);

        // enable the "remove" button
         $('#' + btnDelId).attr('disabled', '');

        // business rule: you can only add 5 names
        if (newNum == 5)
            $('#' + btnAddId).attr('disabled', 'disabled');

        //set up cascading effect for candidate location dropdowns
        if ($(this).attr("id").indexOf('AddLocation') != -1) {
            Dial4Jobz.Common.SetupLocationDropdowns('Preferred', newNum);
            $('#PreferredCountry' + newNum).val('').change();
        }

        //set up cascading effect for job location dropdowns
        if ($(this).attr("id").indexOf('AddLocation') != -1) {
            Dial4Jobz.Common.SetupLocationDropdowns('Posting', newNum);
            $('#PostingCountry' + newNum).val('').change();
        }
    });

    $('#' + btnDelId).click(function () {
        var num = $('.' + inputContainerCss).length;

        $('#' + inputContainerIdPrefix + num).remove();
        $('#' + btnAddId).attr('disabled', '');
        if (num == 2)
            $('#' + btnDelId).attr('disabled', 'disabled');
    });
};

Dial4Jobz.Common.AddTokenizerInput = function (textboxId, url, prePopulateJson) {
    $("#" + textboxId).tokenInput(url, {
        classes: {
            tokenList: "token-input-list-facebook",
            token: "token-input-token-facebook",
            tokenDelete: "token-input-delete-token-facebook",
            selectedToken: "token-input-selected-token-facebook",
            highlightedToken: "token-input-highlighted-token-facebook",
            dropdown: "token-input-dropdown-facebook",
            dropdownItem: "token-input-dropdown-item-facebook",
            dropdownItem2: "token-input-dropdown-item2-facebook",
            selectedDropdownItem: "token-input-selected-dropdown-item-facebook",
            inputToken: "token-input-input-token-facebook"
        },
        prePopulate: prePopulateJson
    });
};

Dial4Jobz.Common.ShowMessageBar = function (message, time) {
    if (!time) {
        time = 4000;
    }

    $("<div />", { 'class': 'messageBar', text: message }).hide().prependTo("body")
      .slideDown('fast').delay(time).slideUp(function () { $(this).remove(); });
}


