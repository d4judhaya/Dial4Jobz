﻿<%@ Import Namespace="Dial4Jobz.Models" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Admin - Add Profile
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.timepicker.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/PreventDoubleSubmit.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/JquerySearchIt.js") %>" type="text/javascript"></script>
    
    <link href="../../Content/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Content/jquery-ui.1.8.1.css" rel="Stylesheet" type="text/css" />
    <link href="../../../../Content/jquery.timepicker.css" rel="Stylesheet" type="text/css" />

   <script type="text/javascript">

       $(document).ready(function () {

           /***Confirm Mobile Verification***/
           $('#confirmverify').text('Confirm') // Sets text for company.
                .attr('href', '#');

           $("#confirmverify").click(function () {
               alert("Verified successfully");
               $('#confirmverify').text('Confirm')
                .attr('href', 'GetCandidateNumber?confirmCandidateMobileVerification=' + $('#PhVerificationNo').val());
           });

           //$(document).ready(function () {
           $('#confirmverify').click(function () {
               var name = $('#PhVerificationNo').val();
               var data = 'confirmCode=' + name;
               $.ajax({
                   type: "GET",
                   url: "ConfirmCandidateMobileVerification",
                   data: data,
                   success: function (data) {
                       alert("Verified");
                   }
               });
               return false;
           });
           //});

           /**End Confirm Verification*/


           /*Get the candidate Profile*/
           $('#getCandidate').text('Get Profile') // Sets text for mobile.
                    .attr('href', '#');

           $("#UserName").blur(function () {
               $('#checkUser').trigger('click');
               $('#getCandidate').text('Get Profile')
                         .attr('href', 'GetDetail?userName=' + $('#UserName').val());
           });


           //$(document).ready(function () {
           $('#checkUser').click(function () {
               var name = $('#UserName').val();
               var data = 'userName=' + name;
               $.ajax({
                   type: "GET",
                   url: "ValidateCandidate",
                   data: data,
                   success: function (data) {
                       if (data == "User Id Exists") {
                           window.location = "/Consult/GetDetail?userName=" + name;
                           alert("User Id exists");
                       } else {
                           alert("UserId not exist");
                       }

                   }
               });
               return false;
           });

           /*Start Direct Verification By mail**/


           $('#verifyemail').text('Verify Email') // Sets text for company.
                .attr('href', '#');

           $("#verifyemail").click(function () {
               alert("Verification sent successfully");
               $('#verifyemail').text('Verify Email')
                .attr('href', 'GetCandidateEmail?candidateEmailVerification=' + $('#Email').val());
           });


           //            $(document).ready(function () {
           $('#verifyemail').click(function () {
               var name = $('#Email').val();
               var data = 'email=' + name;
               $.ajax({
                   type: "GET",
                   url: "CandidateEmailVerification",
                   data: data,
                   success: function (data) {
                       //alert("");

                   }
               });
               return false;
           });
           //            });

           /*End Direct Verification By Email*/

           // set Preferred time in timepicker
           $('input[name*="ddlPreferredTimeFrom"]').timepicker({});

           $('input[name*="ddlPreferredTimeTo"]').timepicker({});

           $("#DOB").datepicker({
               //dateFormat: "yy-mm-dd",
               changeMonth: true,
               changeYear: true,
               yearRange: "1930:1995"
               //yearRange: "-100:-16"
           });

           $('#LocalityDiv,#PrefLocalityDiv,#IndustryDiv').click(function (e) {
               e.stopPropagation();
           });

           $(document).click(function () {
               $("#LocalityDiv,#PrefLocalityDiv,#IndustryDiv").css("display", "none");
           });


           $("#CandidateFunctions").change(function () {
               if ($("#PreferredFunctions").val() == "" || $("#PreferredFunctions").val() == null) {
                   $("#PreferredFunctions").val($("#CandidateFunctions").val());
               }
           });

           var country = $("input:radio[name=CountryCheck]:checked").val();
           if (country != "India") {
               $("#Country").html("");
               $("#OtherCountryCity").hide();
               $.get("/Candidates/getotherCountry/152", function (data) {
                   $.each(data.OtherCountry, function (key, value) {
                       if (key == 0) {
                           $("#Country").append($("<option></option>").val("").html("- Select Country -"));
                       }
                       if ($("#HfCountryId").val() == value.Id) {
                           $("#Country").append($("<option selected='selected'></option>").val(value.Id).html(value.Name));
                           $("#OtherCountryCity").show();
                       }
                       else
                           $("#Country").append($("<option></option>").val(value.Id).html(value.Name));
                   });
               });
               $("#Country").show();
               $("#State").hide();
               $("#City").hide();
               $("#Region").hide();
           }
           else {
               $("#Country").hide();
               $("#OtherCountryCity").hide();

               $("#State").show();
               $("#City").show();
               $("#Region").show();
               $("#State").html("");
               $("#State").attr("disabled", false);
               $.get("/Candidates/getstatebyCountryId/152", function (data) {
                   $.each(data.state, function (key, value) {
                       if (key == 0) {
                           $("#State").append($("<option></option>").val("").html("- Select State -"));
                       }
                       if ($("#HfStateId").val() == value.Id) {
                           $("#State").append($("<option selected='selected'></option>").val(value.Id).html(value.Name));
                       }
                       else
                           $("#State").append($("<option></option>").val(value.Id).html(value.Name));

                   });
               });
           }

           $("input:radio[name=CountryCheck]").click(function () {
               var country = $("input:radio[name=CountryCheck]:checked").val();
               if (country != "India") {
                   $("#Country").html("");
                   $.get("/Candidates/getotherCountry/152", function (data) {
                       $.each(data.OtherCountry, function (key, value) {
                           if (key == 0) {
                               $("#Country").append($("<option></option>").val("").html("- Select Country -"));
                           }

                           $("#Country").append($("<option></option>").val(value.Id).html(value.Name));

                       });
                   });
                   $("#Country").show();
                   $("#OtherCountryCity").hide();
                   $("#State").hide();
                   $("#City").hide();
                   $("#Region").hide();
               }
               else {
                   $("#Country").hide();
                   $("#OtherCountryCity").hide();

                   $("#State").show();
                   $("#City").show();
                   $("#Region").show();
                   $("#State").html("");
                   $("#State").attr("disabled", false);
                   $.get("/Candidates/getstatebyCountryId/152", function (data) {
                       $.each(data.state, function (key, value) {
                           if (key == 0) {
                               $("#State").append($("<option></option>").val("").html("- Select State -"));
                           }

                           $("#State").append($("<option></option>").val(value.Id).html(value.Name));

                       });
                   });
               }
           });

       });

       function displayothercity(id) {
           if ($(id).val() != "") {
               $("#OtherCountryCity").show();
           }
           else {
               $("#OtherCountryCity").hide();
           }
       }

  </script>

   <script type="text/javascript">
       $(document).ready(function () {

           var arrCountry = new Array();
           var arrState = new Array();
           var arrCity = new Array();
           var arrRegion = new Array();

           var myForm = document.forms.Save;

           var CountryControls = myForm.elements['HfPreferredCountry[]'];
           for (var i = 0; i < CountryControls.length; i++) {
               arrCountry[i] = CountryControls[i].value;
           }

           var StateControls = myForm.elements['HfPreferredState[]'];
           for (var i = 0; i < StateControls.length; i++) {
               arrState[i] = StateControls[i].value;
           }

           var CityControls = myForm.elements['HfPreferredCity[]'];
           for (var i = 0; i < CityControls.length; i++) {
               arrCity[i] = CityControls[i].value;
           }

           var RegionControls = myForm.elements['HfPreferredRegion[]'];
           for (var i = 0; i < RegionControls.length; i++) {
               arrRegion[i] = RegionControls[i].value;
           }



           if ($.inArray("152", arrCountry) != -1) {
               $("input:checkbox[name=PostingCountry]").attr("checked", "true");
               SelectedCountryCount();
           }

           $("input:checkbox[name=PostingCountry]:checked").each(function () {
               var countryId = $(this).val();
               if ($(this).is(':checked')) {
                   $.get("/Candidates/getstatebyCountryIdwithCountryname/152", function (data) {
                       $.each(data.State, function (key, value) {
                           if (value.Id == "-1") {
                               $("#StateDiv").append("<div id='stateDiv_" + countryId + "' ></div>");
                               $("#stateDiv_" + countryId + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                           }
                           else {
                               if ($.inArray("" + value.Id + "", arrState) != -1) {
                                   $("#stateDiv_" + countryId + "").append("<input type='checkbox' checked='checked' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                               }
                               else {
                                   $("#stateDiv_" + countryId + "").append("<input type='checkbox' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                               }
                           }
                       });
                       setTimeout(SelectedStateCount, 1000);
                   });
               }
           });

           //alert("jhbbhj");
           setTimeout(SelectedCityCount, 5000);

           //alert("jhbbhj");
           setTimeout(SelectedRegionCount, 8000);


           $.get("/Candidates/getotherCountry/152", function (data) {
               $.each(data.OtherCountry, function (key, value) {
                   if (key == 0) {
                       $("#OtherCountryDiv").append("<input type='checkbox' name='checkAllOtherCountry' checked='checked' value='' /> Any <br />");

                   }
                   if ($.inArray("" + value.Id + "", arrCountry) != -1) {
                       $("#OtherCountryDiv").append("<input type='checkbox' checked='checked' id='OtherCountry_" + value.Id + "' name='PostingOtherCountry' onclick='checkOtherCountry()' value='" + value.Id + "' /> " + value.Name + " <br />");
                   }
                   else {
                       $("#OtherCountryDiv").append("<input type='checkbox' id='OtherCountry_" + value.Id + "' name='PostingOtherCountry' onclick='checkOtherCountry()' value='" + value.Id + "' /> " + value.Name + " <br />");
                   }
               });
               checkOtherCountry();
           });


           $('#txtCountry').click(function (e) {
               if (document.getElementById("CountryDiv").style.display == "none") {
                   document.getElementById("CountryDiv").style.display = "block";
                   e.stopPropagation();
               }
               else {
                   document.getElementById("CountryDiv").style.display = "none";
               }
           });

           $('#txtState').click(function (e) {
               if (document.getElementById("StateDiv").style.display == "none") {
                   document.getElementById("StateDiv").style.display = "block";
                   e.stopPropagation();
               }
               else {
                   document.getElementById("StateDiv").style.display = "none";
               }
           });

           $('#txtCity').click(function (e) {
               if (document.getElementById("CityDiv").style.display == "none") {
                   document.getElementById("CityDiv").style.display = "block";
                   e.stopPropagation();
               }
               else {
                   document.getElementById("CityDiv").style.display = "none";
               }
           });

           $('#txtRegion').click(function (e) {
               if (document.getElementById("RegionDiv").style.display == "none") {
                   document.getElementById("RegionDiv").style.display = "block";
                   e.stopPropagation();
               }
               else {
                   document.getElementById("RegionDiv").style.display = "none";
               }
           });

           $('#txtOtherCountry').click(function (e) {
               if (document.getElementById("OtherCountryDiv").style.display == "none") {
                   document.getElementById("OtherCountryDiv").style.display = "block";
                   e.stopPropagation();
               }
               else {
                   document.getElementById("OtherCountryDiv").style.display = "none";
               }
           });

           $('#CountryDiv,#StateDiv,#CityDiv,#RegionDiv,#OtherCountryDiv').click(function (e) {
               e.stopPropagation();
           });

           $(document).click(function () {
               $("#CountryDiv,#StateDiv,#CityDiv,#RegionDiv,#OtherCountryDiv").css("display", "none");
           });

           function SelectedStateCount() {
               var g = 0;
               $("input:checkbox[name=PostingState]:checked").each(function () {
                   g = g + 1;
               });
               $("#txtState").val("- Selected States ( " + g + " ) -");

               $("input:checkbox[name=PostingState]:checked").each(function () {
                   var stateId = $(this).val();
                   if ($(this).is(':checked')) {
                       $.get("/Candidates/getCitybyStateId/" + stateId, function (data) {
                           $.each(data.City, function (key, value) {
                               if (value.Id == "-1") {
                                   $("#CityDiv").append("<div id='cityDiv_" + stateId + "' ></div>");
                                   $("#cityDiv_" + stateId + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                               }
                               else {
                                   if ($.inArray("" + value.Id + "", arrCity) != -1) {
                                       $("#cityDiv_" + stateId + "").append("<input type='checkbox' checked='checked' name='PostingCity" + stateId + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                   }
                                   else {
                                       $("#cityDiv_" + stateId + "").append("<input type='checkbox' name='PostingCity" + stateId + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                   }
                               }
                           });
                       });
                   }

               });
           }

           function SelectedCityCount() {
               var g = 0;
               $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
                   g = g + 1;
               });
               $("#txtCity").val("- Selected Cities ( " + g + " ) -");

               $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {

                   var cityId = $(this).val();
                   if ($(this).is(':checked')) {
                       $.get("/Candidates/getRegionbyCityId/" + cityId, function (data) {
                           $.each(data.Region, function (key, value) {
                               if (value.Id == "-1") {
                                   $("#RegionDiv").append("<div id='regionDiv_" + cityId + "' ></div>");
                                   $("#regionDiv_" + cityId + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                               }
                               else {
                                   if ($.inArray("" + value.Id + "", arrRegion) != -1) {
                                       $("#regionDiv_" + cityId + "").append("<input type='checkbox' checked='checked' name='PostingRegion" + cityId + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                   }
                                   else {
                                       $("#regionDiv_" + cityId + "").append("<input type='checkbox' name='PostingRegion" + cityId + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                   }
                               }
                           });
                       });
                   }

               });
           }

           function SelectedRegionCount() {

               var g = 0;
               $("input:checkbox[class=ddlregioncheckbox]:checked").each(function () {
                   g = g + 1;
               });
               $("#txtRegion").val("- Selected Regions ( " + g + " ) -");
           }

       });


       function checkOtherCountry() {
           var g = 0;
           $("input:checkbox[name=PostingOtherCountry]:checked").each(function () {
               g = g + 1;
           });
           $("#txtOtherCountry").val("- Selected Other Countries ( " + g + " ) -");
           if (g == 0) {
               $("input:checkbox[name=checkAllOtherCountry]").each(function () {
                   this.checked = true;
               });
           }
           else {
               $("input:checkbox[name=checkAllOtherCountry]").each(function () {
                   this.checked = false;
               });
           }
       }

       function checkCountry(val) {
           if (document.getElementById("PostingCountry_" + val + "").checked) {

               $.get("/Candidates/getstatebyCountryIdwithCountryname/" + val, function (data) {
                   $.each(data.State, function (key, value) {
                       if (value.Id == "-1") {
                           $("#StateDiv").append("<div id='stateDiv_" + val + "' ></div>");
                           $("#stateDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                       }
                       else {
                           $("#stateDiv_" + val + "").append("<input type='checkbox' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcitycheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                       }
                   });
               });

           }
           else {

               $("#stateDiv_" + val + "").remove();
           }

           SelectedCountryCount();
       }

       function checkState(val) {
           if (document.getElementById("PostingState_" + val + "").checked) {

               $.get("/Candidates/getCitybyStateId/" + val, function (data) {
                   $.each(data.City, function (key, value) {
                       if (value.Id == "-1") {
                           $("#CityDiv").append("<div id='cityDiv_" + val + "' ></div>");
                           $("#cityDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                       }
                       else {
                           $("#cityDiv_" + val + "").append("<input type='checkbox' name='PostingCity" + val + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                       }
                   });
               });

           }
           else {

               $("#cityDiv_" + val + "").remove();
           }

           SelectedStateCount();
       }

       function checkCity(val) {
           if (document.getElementById("PostingCity_" + val + "").checked) {

               $.get("/Candidates/getRegionbyCityId/" + val, function (data) {

                   $.each(data.Region, function (key, value) {

                       if (value.Id == "-1") {

                           $("#RegionDiv").append("<div id='regionDiv_" + val + "' ></div>");
                           $("#regionDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                       }
                       else {

                           $("#regionDiv_" + val + "").append("<input type='checkbox' name='PostingRegion" + val + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                       }
                   });
               });

           }
           else {

               $("#regionDiv_" + val + "").remove();
           }

           SelectedCityCount();
       }

       function checkRegion(val) {
           SelectedRegionCount();
       }

       function SelectedCountryCount() {
           var g = 0;
           $("input:checkbox[name=PostingCountry]:checked").each(function () {
               g = g + 1;
           });
           $("#txtCountry").val("- Selected Countries ( " + g + " ) -");
           if (g == 0) {
               $("input:checkbox[name=checkAnyCountry]").each(function () {
                   this.checked = true;
               });
           }
           else {
               $("input:checkbox[name=checkAnyCountry]").each(function () {
                   this.checked = false;
               });
           }
       }

       function SelectedStateCount() {
           var g = 0;
           $("input:checkbox[name=PostingState]:checked").each(function () {
               g = g + 1;
           });
           $("#txtState").val("- Selected States ( " + g + " ) -");
       }

       function SelectedCityCount() {
           var g = 0;
           $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
               g = g + 1;
           });
           $("#txtCity").val("- Selected Cities ( " + (g - 1) + " ) -");
       }

       function SelectedRegionCount() {
           var g = 0;
           $("input:checkbox[class=ddlregioncheckbox]:checked").each(function () {
               g = g + 1;
           });
           $("#txtRegion").val("- Selected Regions ( " + g + " ) -");
       }

      </script>

   <style type="text/css">
        .section li
        {
            color: #324B81;
        }
        .lf
        {
            float: left;
        }
        .scrollbox
        {
            overflow: auto;
            width: 260px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
            line-height:20px !important;
            z-index: 60000;
            display: none;
            position: absolute;
            background-color: White;
        }
        
        .scrollboxlongdiv
        {
            overflow: auto;
            width: 540px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
            line-height: 10px !important;
            z-index: 60000;
            display: none;
            position: absolute;
            background-color: White;
        }
        
        .dropleft
        {
            border-right: 1px solid #030CA6;
            float: left;
            width: 250px;
            padding-left: 8px;
        }
        
        .choose
        {
            color: #969696;
            font: bold 13px Arial,Helvetica,sans-serif;
            padding-left: 5px;
        }
        
         .lf
        {
            float: left;           
        }
       .ddlcheckbox
        {
            float: left;
            margin-right: 5px;
            margin-top: 3px;
        }
        
        .scrollbox
        {
            overflow: auto;
            width: 260px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
            line-height:10px !important;
            z-index: 60000;
            display:none;
            position:absolute;
            background-color:White;           
        }    
       
        .text
        {
            background: url(../../Content/Images/dropdown.gif) no-repeat scroll 98% 50% transparent;
        }   
        
       .right
        {
            float: left;
            width: 220px;
            line-height: 18px;
            padding-bottom: 5px;
        }
        
       
    </style>
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.BeginForm("SaveCandidate", "Consult", FormMethod.Post, new { @id = "Save" });{%>

   <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>

   <%: Html.Hidden("Id",Model.Id)%>
   <% int candidateId = Model.Id; %>

   <% Session["candiId"] = candidateId.ToString(); %>
    
    <center>
        <h3>Update Candidate Details</h3>
    </center>

    <div class="mandatoryalign" align="right">
        <span class="red">*</span> Mandatory Fields
    </div>
    
    <a id="directverification"></a>

    <div class="editor-label">
        <%: Html.Label("Candidate Name")%>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("Name", Model.Name, new { @title = "Enter the Candidate's name" })%>
    </div>

    <div class="editor-label">
        <%: Html.Label("User Id")%> <span class="red">*</span>
    </div>

    <div class="editor-field"> Don't Create UserName here. System will generate.
    <%if(Model.UserName!=null) { %>
        <%: Html.TextBox("UserName", Model.UserName, new { @title = "Enter the Candidate's UserName", @readonly = "readonly"})%>
    <%} else { %>
        <%: Html.TextBox("UserName", Model.UserName, new { @title = "Enter the Candidate's UserName" })%>
    <%} %>
        <a id="checkUser" href="#"></a>| <a id="getCandidate"></a>
    </div>
    

    <div class="editor-label">
        <%: Html.Label("Email") %>
    </div>
    <div class="editor-field">

    <% if (LoggedInConsultant != null)
       { %>
        <%: Html.TextBox("Email", LoggedInConsultant.Email, new { @title = "Enter the Candidate Email Id" })%>
    <% }
       else
       { %>
        <%: Html.TextBox("Email", Model.Email, new { @title = "Enter the Candidate Email Id" })%>
    <% } %>

        <%if (Model.IsMailVerified == true)
          { %>
            <img src="../../../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span>
        <% }
          else
          { %>
            <span class="red">Not verified</span><a id="verifyemail"></a>

        <%} %>
        
    </div>
    <div class="editor-label">
        <%: Html.Label("Mobile Number") %>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
    <% if (LoggedInConsultant != null)
       { %>
       <%: Html.TextBox("ContactNumber", LoggedInConsultant.MobileNumber, new { @title = "Don't enter 0 or country code before Mobile Number", @maxlength = "10" })%>
    <% }
       else
       { %>
       <%: Html.TextBox("ContactNumber", Model.ContactNumber, new { @title = "Don't enter 0 or country code before Mobile Number", @maxlength = "10" })%>
    <% } %>
       

        <%if (Model.IsPhoneVerified == true)
          { %>
                 <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
        <% } else { %>
            <span class="red">Not verified</span> <a id="verifymobile"></a>
           
        <% } %>
       
    </div>

    <div class="editor-label">
        <%:Html.Label("International Number") %>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("InternationalNumber", Model.InternationalNumber, new { @title = "Enter Your International Number" })%>
    </div>

    <div class="editor-label">
        <%:Html.Label("Enter Verification Code") %>
    </div>
    <div class="editor-field" style="width: 50px;">
        <%: Html.TextBox("PhVerificationNo", null, new { @title = "Enter the Verification Code" })%>
        <a id="confirmverify"></a>
        
    </div>
    <div class="editor-label">
        <%:Html.Label("Any additional number") %>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("MobileNumber", Model.MobileNumber, new { @title = "Enter Your additional Number"})%>
    </div>
    <div class="editor-label">
        <%: Html.Label("Address") %>
        <span class="textlength">Max 250 characters</span> <span class="red">*</span>
    </div>
    <div class="editor-field">
     <% if (LoggedInConsultant != null)
        { %>
             <%: Html.TextBox("Address", LoggedInConsultant.Address, new { @title = "Enter the Candidate Address", @maxlength = "250" })%>
        <%} else { %>
              <%: Html.TextBox("Address", Model.Address, new { @title = "Enter the Candidate Address", @maxlength = "250" })%>
        <%} %>
    </div>
    <div class="editor-label">
        <%:Html.Label("Pincode")%>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
     <% if (LoggedInConsultant != null)
        { %>
            <%:Html.TextBox("Pincode", LoggedInConsultant.Pincode, new { @title = "Enter your Pincode Number", @maxlength = "6" })%>
     <%} else { %>
            <%:Html.TextBox("Pincode", Model.Pincode, new { @title = "Enter your Pincode Number", @maxlength = "6" })%>
     <%} %>
    </div>

    <div class="editor-label">
        <%:Html.Label("Date of Birth") %>
        <span class="red">*</span> 
    </div>
    <div class="editor-field">
        <%:Html.TextBox("DOB", Model.DOB.HasValue ? Model.DOB.Value.ToShortDateString() : String.Empty) %>
    </div>
    <div class="editor-label">
        <%:Html.Label("Marital Status")%>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("MaritalStatus", "--Select--")%>
    </div>
    <div class="editor-label">
        <%:Html.Label("Gender") %>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <%:Html.RadioButtonFor(model => model.Gender, 0)%>
        Male
        <%:Html.RadioButtonFor(model => model.Gender, 1)%>
        Female
    </div>

    <h2>Professional Details</h2>

     <div class="editor-label">
        <%:Html.Label("Position / Designation") %>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("Position", Model.Position, new { @title = "Enter the Candidate's Position" })%>
    </div>
    <div class="editor-label">
         <%: Html.Label("Functional Area / Department")%>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("CandidateFunctions", "--Any--")%>
    </div>
    <h4 style="color:Blue;">If you are a Fresher select (Any) in Role</h4>
    <div class="editor-label">
        <%: Html.Label("Role") %>
    </div>
    <div class="editor-field">
        <% if (ViewData["Roles"] != null)
           { %>
        <%: Html.DropDownList("Roles", "Any")%>
        <% } else { %>
            <select id="Roles" name="Roles"></select>
        <% } %>
    </div>

    <div class="editor-label">
        <%: Html.Label("Industries") %>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("Industries", "--Any--") %>
    </div>

     <div class="editor-label">
        <%:Html.Label("Preferred Function(s)") %>
   </div>
    
     <div class="editor-field">   
              <% var preferredFunctions = Model.CandidatePreferredFunctions.Where(c => c.FunctionId!=0); %> 
              <%if(preferredFunctions.Count() > 0) { %>
                 <%: Html.ListBox("PreferredFunctions", new MultiSelectList((IEnumerable<Dial4Jobz.Models.Function>)ViewData["Functions"], "Id", "Name", (IEnumerable<int>)ViewData["PreferredFunctionIds"]))%>
              <%} else { %>
                 <%: Html.ListBox("PreferredFunctions", new SelectList((System.Collections.Generic.IEnumerable<SelectListItem>)ViewData["PrefFunctions"], "Value", "Text", (IEnumerable<int>)ViewData["PreferredFunctionIds"]))%>
              <% } %>
     </div>


    <div class="editor-label">
        <%:Html.Label("Total Experience") %>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("ddlTotalExperienceYears", (SelectList)ViewData["TotalExperienceYears"])%>
        Years
        <%: Html.DropDownList("ddlTotalExperienceMonths", (SelectList)ViewData["TotalExperienceMonths"])%>
        Months
    </div>
    <div class="editor-label">
        <%: Html.Label("Annual Salary") %>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("ddlAnnualSalaryLakhs", (SelectList)ViewData["AnnualSalaryLakhs"])%>
        Lakhs
        <%: Html.DropDownList("ddlAnnualSalaryThousands", (SelectList)ViewData["AnnualSalaryThousands"])%>
        Thousands
    </div>

     <div class="editor-label">
        <%:Html.Label("Current Location") %>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
        <script type="text/javascript">
            $(document).ready(function () {
                //populate the candidate function to preferred function
                $("#CandidateFunctions").change(function () {
                    $("#PreferredFunctions").val($(this).val());
                });


                var country = $("input:radio[name=CountryCheck]:checked").val();
                //alert(country);
                if (country != "India") {
                    $("#Country").html("");
                    $.get("/Candidates/getotherCountry/152", function (data) {
                        $.each(data.OtherCountry, function (key, value) {
                            if (key == 0) {
                                $("#Country").append($("<option></option>").val("").html("- Select Country -"));
                            }

                            $("#Country").append($("<option></option>").val(value.Id).html(value.Name));

                        });
                    });
                    $("#Country").show();
                    $("#OtherCountryCity").hide();
                    $("#State").hide();
                    $("#City").hide();
                    $("#Region").hide();
                }
                else {
                    $("#Country").hide();
                    $("#OtherCountryCity").hide();

                    $("#State").show();
                    $("#City").show();
                    $("#Region").show();
                    //$("#State").html("");
                    // $("#State").attr("disabled", false);
                    $.get("/Candidates/getstatebyCountryId/152", function (data) {
                        $.each(data.state, function (key, value) {
                            if (key == 0) {
                                $("#State").append($("<option></option>").val("").html("- Select State -"));
                            }

                            $("#State").append($("<option></option>").val(value.Id).html(value.Name));

                        });
                    });
                }

                $("input:radio[name=CountryCheck]").click(function () {
                    var country = $("input:radio[name=CountryCheck]:checked").val();
                    if (country != "India") {
                        $("#Country").html("");
                        $.get("/Candidates/getotherCountry/152", function (data) {
                            $.each(data.OtherCountry, function (key, value) {
                                if (key == 0) {
                                    $("#Country").append($("<option></option>").val("").html("- Select Country -"));
                                }

                                $("#Country").append($("<option></option>").val(value.Id).html(value.Name));

                            });
                        });
                        $("#Country").show();
                        $("#OtherCountryCity").hide();
                        $("#State").hide();
                        $("#City").hide();
                        $("#Region").hide();
                    }
                    else {
                        $("#Country").hide();
                        $("#OtherCountryCity").hide();

                        $("#State").show();
                        $("#City").show();
                        $("#Region").show();
                        $("#State").html("");
                        $("#State").attr("disabled", false);
                        $.get("/Candidates/getstatebyCountryId/152", function (data) {
                            $.each(data.state, function (key, value) {
                                if (key == 0) {
                                    $("#State").append($("<option></option>").val("").html("- Select State -"));
                                }

                                $("#State").append($("<option></option>").val(value.Id).html(value.Name));

                            });
                        });
                    }
                });

            });

            function displayothercity(id) {
                if ($(id).val() != "") {
                    $("#OtherCountryCity").show();
                }
                else {
                    $("#OtherCountryCity").hide();
                }
            }
        </script>
        <%if (Model.LocationId == null)
          { %>
        <%= Html.RadioButton("CountryCheck", "India", true)%>
        India
        <%= Html.RadioButton("CountryCheck", "Other")%>
        Other Countries
        <br />
        <%}
          else if (ViewData["Countries"].ToString() != "")
          {%>
        <% Dial4Jobz.Models.Location currentlocation = Model.LocationId.HasValue ? new Dial4Jobz.Models.Repositories.Repository().GetLocationById(Model.LocationId.Value) : null;%>
        <%= Html.RadioButton("CountryCheck", "India", currentlocation != null && currentlocation.CountryId == 152)%>
        India
        <%= Html.RadioButton("CountryCheck", "Other", currentlocation != null && currentlocation.CountryId != 152)%>
        Other Countries
        <br />
        <input type="hidden" value="<%= currentlocation != null ? currentlocation.CountryId.ToString() : "" %>"
            id="HfCountryId" />
        <input type="hidden" value="<%= currentlocation != null ? currentlocation.StateId.ToString() : "" %>"
            id="HfStateId" />
        <%} %>
        <%: Html.DropDownList("Country", new[] { new SelectListItem { Text = "-- Select --", Value = "" } }, new { onchange = "displayothercity(this)" })%>
        <%: Html.TextBox("OtherCountryCity", "", new { @class = "specialization", Style = "width:230px;", PlaceHolder = "Enter City" })%>
        <% if (ViewData["State"] != null)
           { %>
        <%: Html.DropDownList("State", "Any")%>
        <%}
           else
           { %>
        <select id="State" name="State">
        </select>
        <%} %>
        <% if (ViewData["City"] != null)
           { %>
        <%: Html.DropDownList("City", "Any")%>
        <%}
           else
           { %>
        <select id="City" name="City">
        </select>
        <% } %>
        <% if (ViewData["Region"] != null)
           { %>
        <%: Html.DropDownList("Region", "Any")%>
        <% }
           else
           { %>
        <select id="Region" name="Region">
        </select>
        <% } %>
    </div>

    <div class="editor-label">
               <%: Html.Label("Preferred Location(s)")%>
           </div>
           <div class="editor-field">
                India :
                <br />
                <input id="txtCountry" class="text" readonly="readonly" value="- Select Country -"
                    type="text" style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="CountryDiv">
                    <%-- country checkbox list --%>
                    <input type='checkbox' name='checkAnyCountry' checked='checked' value='' />
                    Any
                    <br />
                    <input id="PostingCountry_152" type="checkbox" value="152" onclick="checkCountry(152)"
                        name="PostingCountry" />
                    India
                    <br/>
                </div>
                
                <input id="txtState" class="text" readonly="readonly" value="- Select State -" type="text"
                    style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="StateDiv">
                    <%-- state checkbox list --%>
                </div>
                <br />
                <input id="txtCity" class="text" readonly="readonly" value="- Select Cities -" type="text"
                    style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="CityDiv">
                    <%-- city checkbox list --%>
                </div>
               
                <input id="txtRegion" class="text" readonly="readonly" value="- Select Regions -"
                    type="text" style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="RegionDiv">
                    <%-- region checkbox list --%>
                </div>
                <br />
                Other Country :
                <br />
                <input id="txtOtherCountry" class="text" readonly="readonly" value="- Select Other Country -"
                    type="text" style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="OtherCountryDiv">
                    <%-- Industry checkbox list --%>
                </div>
                <div>
                <input type="hidden" name="HfPreferredCountry[]" value="" />   
                <input type="hidden" name="HfPreferredState[]" value="" />
                <input type="hidden" name="HfPreferredCity[]" value="" /> 
                <input type="hidden" name="HfPreferredRegion[]" value="" />                                                                                                                                                             
                </div>
                <% if (Model.CandidatePreferredLocations.Count() > 0)
                   { %>
                            <% for (int i = 1; i <= Model.CandidatePreferredLocations.Count(); i++)
                               { %>
                                <% var location = Model.CandidatePreferredLocations.ElementAt(i - 1).Location; %>                                 
                                   <div>                                         
                                      <input type="hidden" name="HfPreferredCountry[]" value="<%= location.CountryId %>" />                                         

                                      <% if (location.StateId.HasValue)
                                         { %>
                                         <input type="hidden" name="HfPreferredState[]" value="<%= location.StateId %>" />                                        
                                      <% } %>

                                      <% if (location.CityId.HasValue)
                                         { %>
                                         <input type="hidden" name="HfPreferredCity[]" value="<%= location.CityId %>" />                                        
                                      <% } %>                                        

                                      <% if (location.RegionId.HasValue)
                                         { %>
                                         <input type="hidden" name="HfPreferredRegion[]" value="<%= location.RegionId %>" />                                        
                                      <% } %>                                        
                                   </div>
                             <% } %> 
                <% } %>    
                    
           </div>  

    <div class="editor-label">
        <%: Html.Label("Present / Last Employer")%>
        <span class="textlength">Max 70 characters</span>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("PresentCompany", Model.PresentCompany, new { @title = "Enter your Present Company", maxlength="70"})%>
    </div>
    <div class="editor-label">
        <%: Html.Label("Previous Employer")%>
        <span class="textlength">Max 70 characters</span>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("PreviousCompany", Model.PreviousCompany, new { @title = "Enter your Previous Company", maxlength = "70" })%>
    </div>
    <div class="editor-label clear">
        <%: Html.Label("Number of Companies Worked For")%>
    </div>
    <div class="editor-field">
        <%: Html.DropDownList("ddlNumberOfCompanies", (SelectList)ViewData["NumberOfCompanies"])%>
    </div>

     <div class="editor-label">
        <%: Html.Label("Skills") %>
    </div>
    <div class="editor-field">
        <input type="text" id="Skills" name="Skills" />
        <% var skills = string.Empty; %>
        <% foreach (var cs in Model.CandidateSkills)
           { %>
        <% var skill = String.Format("{{\"id\": {0}, \"name\": \"{1}\"}}", cs.Skill.Id.ToString(), cs.Skill.Name); %>
        <% skills = skills == string.Empty ? skill : skills + ", " + skill; %>
        <% } %>
        <%: Html.Hidden("SkillsHidden", "[" + skills + "]") %>
    </div>
    <div class="editor-label">
        <%: Html.Label("Languages") %>
    </div>
    <div class="editor-field">
        <input type="text" id="Languages" name="Languages" />
        <% var languages = string.Empty; %>
        <% foreach (var cl in Model.CandidateLanguages)
           { %>
        <% var language = String.Format("{{\"id\": {0}, \"name\": \"{1}\"}}", cl.Language.Id.ToString(), cl.Language.Name); %>
        <% languages = languages == string.Empty ? language : languages + ", " + language; %>
        <% } %>
        <%: Html.Hidden("LanguagesHidden", "[" + languages + "]")%>
    </div>
    <div class="editor-label">
        <%: Html.Label("Preferred Type")%>
    </div>
    <div id="PreferredType" class="editor-field">
        <%: Html.CheckBox("Any")%>
        Any
        <%: Html.CheckBox("Contract", Model.PreferredContract.HasValue ?  Model.PreferredContract.Value : false)%>
        Contract
        <%: Html.CheckBox("PartTime", Model.PreferredParttime.HasValue ? Model.PreferredParttime.Value : false)%>
        Part Time
        <%: Html.CheckBox("FullTime", Model.PreferredFulltime.HasValue ? Model.PreferredFulltime.Value : false)%>
        Full Time
        <%: Html.CheckBox("WorkFromHome", Model.PreferredWorkFromHome.HasValue ? Model.PreferredWorkFromHome.Value : false)%>
        Work from Home
    </div>
  

    <div id="PreferredTime" class="hidden">
        <div class="editor-label">
            <%: Html.Label("Preferred Time") %>
        </div>
        <div class="editor-field" style="width: 85px;">
            <%:Html.TextBox("ddlPreferredTimeFrom", Model.PreferredTimeFrom, new { @class = "preferredtime" })%>
            to
            <%:Html.TextBox("ddlPreferredTimeTo", Model.PreferredTimeTo, new { @class = "preferredtime" })%>
        </div>
    </div>

    <div class="editor-label">
       <%: Html.Label("Preferred Work Shift") %>
    </div>

    <div class="editor-field">
       <%: Html.CheckBox("GeneralShift", Model.GeneralShift.HasValue ? Model.GeneralShift.Value : false)%> General Shift
       <%: Html.CheckBox("NightShift", Model.NightShift.HasValue ? Model.NightShift.Value :false) %> Night Shift
    </div>

   
  
   
    <div class="editor-label">
        <%: Html.Label("License Types") %>
    </div>
    <div class="editor-field">
        <%: Html.ListBox("lbLicenseTypes", new MultiSelectList((IEnumerable<Dial4Jobz.Models.LicenseType>)ViewData["LicenseTypes"], "Id", "Name", (IEnumerable<int>)ViewData["LicenseTypeIds"]))%>
    </div>
    <br />
   
    <div class="editor-label">
        <%:Html.Label("License Number") %>
    </div>
    <div class="editor-field">
        <%: Html.TextBox("LicenseNumber", Model.LicenseNumber)%>
    </div>
    <div class="editor-label">
        <%:Html.Label("Passport Number") %>
    </div>
    <div class="editor-field">
        <%:Html.TextBox("PassportNumber",Model.PassportNumber)%>
    </div>

     <%: Html.CheckBox("Twowheeler", Model.TwoWheeler)%> Two wheeler
           
    <%: Html.CheckBox("Fourwheeler", Model.FourWheeler)%> Four wheeler

    <h2>
        Educational Details</h2>
    <div class="editor-label">
        <%: Html.Label("Basic Qualification")%>
    </div>
    <div class="editor-field">
        <% var dbRepository = new Dial4Jobz.Models.Repositories.Repository(); %>
        <% var basicQualifications = Model.CandidateQualifications.Where(c => c.Degree.Type == 0);  %>
        <% if (basicQualifications.Count() > 0)
           { %>
        <% for (int i = 1; i <= basicQualifications.Count(); i++)
           { %>
        <% var containerId = "basic-qualification-container" + i; %>
        <div id="<%: containerId %>" style="margin-bottom: 4px;" class="basic-qualification-container left">
            <%: Html.DropDownList("BasicQualificationDegree" + i, new SelectList((IEnumerable<Degree>)ViewData["CandidateBasicQualifications"], "Id", "Name", basicQualifications.ElementAt(i-1).DegreeId), new { @class = "qualification" })%>
            <% var specializationList = dbRepository.GetSpecializationByDegreeId(basicQualifications.ElementAt(i - 1).DegreeId); %>
            <%: Html.DropDownList("BasicQualificationSpecialization" + i, new SelectList(specializationList,"Id","Name", basicQualifications.ElementAt(i - 1).SpecializationId))%>
            <%: Html.DropDownList("BasicQualificationInstitute" + i, new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name", basicQualifications.ElementAt(i - 1).InstituteId))%>
            <%: Html.DropDownList("BasicQualificationPassedOutYear" + i, new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"],"Value", "Text", basicQualifications.ElementAt(i-1).PassedOutYear))%>
        </div>
        <% } %>
        <% }
           else
           { %>
        <div id="basic-qualification-container1" style="margin-bottom: 4px;" class="basic-qualification-container left">
            <%: Html.DropDownList("BasicQualificationDegree1", new SelectList((IEnumerable<Degree>)ViewData["CandidateBasicQualifications"], "Id", "Name"), new { @class = "qualification" })%>
            <select id="BasicQualificationSpecialization1" name="BasicQualificationSpecialization1"
                disabled="disabled">
                <option value="0">--Select Specialization--</option>
            </select>
            <%: Html.DropDownList("BasicQualificationInstitute1", new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name"))%>
            <%: Html.DropDownList("BasicQualificationPassedOutYear1", new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"],"Value", "Text"))%>
        </div>
        <% } %>
        <div class="clear">
            <input type="button" id="btnAddBasicQualification" value="Add Another Degree" />
            <input type="button" id="btnDelBasicQualification" value="Remove" />
        </div>
    </div>
    <div class="editor-label clear">
        <%: Html.Label("Post Graduation")%>
    </div>
    <div class="editor-field">
        <% var postQualifications = Model.CandidateQualifications.Where(c => c.Degree.Type == 1);  %>
        <% if (postQualifications.Count() > 0)
           { %>
        <% for (int i = 1; i <= postQualifications.Count(); i++)
           { %>
        <% var postContainerId = "post-graduation-container" + i.ToString(); %>
        <div id="<%: postContainerId %>" style="margin-bottom: 4px;" class="post-graduation-container left">
            <%: Html.DropDownList("PostGraduationDegree" + i, new SelectList((IEnumerable<Degree>)ViewData["CandidatePostQualifications"], "Id", "Name", postQualifications.ElementAt(i - 1).DegreeId), new { @class = "qualification" })%>
            <% var specializationList = dbRepository.GetSpecializationByDegreeId(postQualifications.ElementAt(i - 1).DegreeId); %>
            <%: Html.DropDownList("PostGraduationSpecialization" + i, new SelectList(specializationList, "Id", "Name", postQualifications.ElementAt(i - 1).SpecializationId))%>
            <%--<%: Html.TextBox("PostGraduationSpecialization" + i.ToString(), postQualifications.ElementAt(i - 1).Specialization, new { @title = "Enter post graduation degree", @class = "specialization" })%>--%>
            <%: Html.DropDownList("PostGraduationInstitute" + i, new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name", postQualifications.ElementAt(i - 1).InstituteId))%>
            <%: Html.DropDownList("PostGraduationPassedOutYear" + i, new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"], "Value", "Text", postQualifications.ElementAt(i - 1).PassedOutYear))%>
        </div>
        <% } %>
        <% }
           else
           { %>
        <div id="post-graduation-container1" style="margin-bottom: 4px;" class="post-graduation-container left">
            <%: Html.DropDownList("PostGraduationDegree1", new SelectList((IEnumerable<Degree>)ViewData["CandidatePostQualifications"], "Id", "Name"), new { @class = "qualification" })%>
            <select id="PostGraduationSpecialization1" name="PostGraduationSpecialization1" disabled="disabled">
                <option value="0">--Select Specialization--</option>
            </select>
            <%: Html.DropDownList("PostGraduationInstitute1", new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name"))%>
            <%: Html.DropDownList("PostGraduationPassedOutYear1", new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"], "Value", "Text"))%>
        </div>
        <% } %>
        <div class="clear">
            <input type="button" id="btnAddPostGraduation" value="Add Another Degree" />
            <input type="button" id="btnDelPostGraduation" value="Remove" />
        </div>
    </div>
    <div class="editor-label clear">
        <%: Html.Label("Doctorate")%>
    </div>
    <div class="editor-field">
        <% var doctorateDegrees = Model.CandidateQualifications.Where(c => c.Degree.Type == 2);  %>
        <% if (doctorateDegrees.Count() > 0)
           { %>
        <% for (int i = 1; i <= doctorateDegrees.Count(); i++)
           { %>
        <% var doctorateContainerId = "doctorate-container" + i.ToString(); %>
        <div id="<%: doctorateContainerId %>" style="margin-bottom: 4px;" class="doctorate-container left">
            <%: Html.DropDownList("DoctorateDegree" + i, new SelectList((IEnumerable<Degree>)ViewData["CandidateDoctorate"], "Id", "Name", doctorateDegrees.ElementAt(i - 1).DegreeId), new { @class = "qualification" })%>
            <% var specializationList = dbRepository.GetSpecializationByDegreeId(doctorateDegrees.ElementAt(i - 1).DegreeId); %>
            <%: Html.DropDownList("DoctorateSpecialization" + i, new SelectList(specializationList, "Id", "Name", doctorateDegrees.ElementAt(i - 1).SpecializationId))%>
            <%--<%: Html.TextBox("DoctorateSpecialization" + i.ToString(), doctorateDegrees.ElementAt(i - 1).Specialization, new { @title = "Enter doctorate degree", @class = "specialization" })%>--%>
            <%: Html.DropDownList("DoctorateInstitute" + i, new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name", doctorateDegrees.ElementAt(i - 1).InstituteId))%>
            <%: Html.DropDownList("DoctoratePassedOutYear" + i, new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"], "Value", "Text", doctorateDegrees.ElementAt(i - 1).PassedOutYear))%>
        </div>
        <% } %>
        <% }
           else
           { %>
        <div id="doctorate-container1" style="margin-bottom: 4px;" class="doctorate-container left">
            <%: Html.DropDownList("DoctorateDegree1", new SelectList((IEnumerable<Degree>)ViewData["CandidateDoctorate"], "Id", "Name"), new { @class = "qualification" })%>
            <select id="DoctorateSpecialization1" name="DoctorateSpecialization1" disabled="disabled">
                <option value="0">--Select Specialization--</option>
            </select>
            <%: Html.DropDownList("DoctorateInstitute1", new SelectList((IEnumerable<Institute>)ViewData["CandidateInstitutes"], "Id", "Name"))%>
            <%: Html.DropDownList("DoctoratePassedOutYear1", new SelectList((IEnumerable<SelectListItem>)ViewData["PassedOutYear"], "Value", "Text"))%>
        </div>
        <% } %>
        <div class="clear">
            <input type="button" id="btnAddDoctorate" value="Add Another Degree" />
            <input type="button" id="btnDelDoctorate" value="Remove" />
        </div>
    </div>
    <div class="editor-label">
        <%:Html.Label("Description") %>
        <span class="textlength">Max 200 characters</span>
    </div>
    <div class="description">
        <%:Html.TextBox("Description",Model.Description,new { @title = "Eg: Mca with 2 yrs exp in system analyst",@maxlength="200"})%>
    </div> <br />
        

  <%--  <h3> Personal Details </h3>--%>

    
    
     <div class="editor-label">
          <%:Html.Label("Resume") %>(You can upload *.pdf. *.doc, *.jpg)
      </div>
           
      <input id="ResumeAdmin" type="file" name="file" value="<%:Model.Id %>" />  
           
       <%if (Model.ResumeFileName != null)
         { %>
            You have uploaded <%: Model.ResumeFileName%>. <% if (!String.IsNullOrEmpty(Model.ResumeFileName)) { %>
       <% } %>
       <%} %>

    
    <div class="editor-field">
        <%: Html.CheckBox("BasicTerms") %>
        <%: Html.ActionLink("I agree Terms and Conditions", "Terms", "Home")%>
    </div>
    <div class="editor-label">
        <%:Html.Label("SMS, Mailer and Privacy Settings") %>
    </div>
    <div id="footerBlocker" style="width: 800px; height: 80px;">
        <div class="fleft cls w200" style="float: left;">
            <div class="dispblo gry font11">
                <input type="checkbox" value="UP" class="cb" name="UP" checked="checked" />
                Job Alerts
            </div>
            <div class="dispblo gry font11">
                <input type="checkbox" value="CS" class="cb" name="CS" checked="checked" />
                Dial4jobz Resume jet E-Mails
                <img src="http://img.naukimg.com/s/1/4/i/help.gif" width="11" height="11" title="(paid service to speed up your job search)" /></div>
            <div class="dispblo gry font11">
                <input type="checkbox" value="CS2" class="cb" name="CS2" checked="checked" />
                Dial4jobz Job on SMS Calls/SMS
                <img src="http://img.naukimg.com/s/1/4/i/help.gif" width="11" height="11" title="(paid service to speed up your job search)" /></div>
        </div>
        <div class="fright w200" style="float: right; text-align: left;">
            <div class="dispblo gry font11">
                <input type="checkbox" value="UP2" class="cb" name="UP2" checked="checked" />
                Important Notifications from Dial4jobz.com
            </div>
            <div class="dispblo gry font11">
                <input type="checkbox" value="VA" class="cb" name="VA" checked="checked" />
                Communication from Clients(Voice/Calls/SMS)
                <img src="http://img.naukimg.com/s/1/4/i/help.gif" width="11" height="11" title="(This includes targeted communication for interview calls, Interest checking & to enhance your Career from Dial4jobz’s clients in the field of Education, Training & Recruitment)" /></div>
            <div class="dispblo gry font11">
                <input type="checkbox" value="PM" class="cb" name="PM" checked="checked" />
                Other Promotions/ Special Offers</div>
        </div>
    </div>
    <div class="gry font11">
        <input type="checkbox" value="PM" id="smsterms" class="cb" name="PM" checked="checked" />I
        have read, understood and agree SMS & Mailer
        <%:Html.ActionLink("Terms and Conditions", "SmsTerms", new { controller = "Home" }, new { target = "_blank" })%>
        governing the use of Dial4jobz.com.</div>
    <input id="Save" type="submit" value="Save Profile" class="btn" name="Save" onclick="javascript:Dial4Jobz.Candidate.Save(this);return false;" />
    <% Html.EndForm(); %>
   <%-- <div id="loading">
        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"
            height="50" alt="Loading..." />
    </div>--%>
    <% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavConsultant"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
   
</asp:Content>
