

//Admin Candidate Report
$(document).ready(function () {
  $("#Day").datepicker({
        dateFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: "1930:1995",
       
    });
    });

$(document).ready(function () {
    $("#SelectDay").hide();
    $("#where").click(function () {
        if ($('#where').val() == '1' && $('#filter').val() == '1') {
         $("#SelectDay").show();}
         else
         {
         $("#SelectDay").hide();}
        
    });
});


//Admin Menu
$(function(){

	$('#coolMenu').find('> li').hover(function(){
		$(this).find('ul')
		.removeClass('noJS')
		.stop(true, true).slideToggle('fast');
	});
	
});
