<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.User>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create</title>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            $.get("/Admin/AdminPermission/getAdminPermissions/", function (data) {
                //Developer Note: For Exact Match check process.....
                debugger;
                $.each(data.permissions, function (key, value) {
                    if (key == 0) {
                        $("#PageAccessDiv").append("<input type='checkbox' id='All' onclick='checkallPage()' name='All' value='' /> Any <br />");
                        debugger;
                    }
                    else {
                        $("#PageAccessDiv").append("<input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess' value='" + value.Id + "' /> " + value.Name + " <br />");
                    }
                });
            });
        });

        function checkallPage() {

            var check = false;
            $("input:checkbox[name=All]").each(function () {
                if ($(this).is(':checked')) {
                    check = true;
                }
            });

            if (check == true) {
                $("input:checkbox[name=PageAccess]").each(function () {
                    this.checked = true;
                });

            }
            else {
                $("input:checkbox[name=PageAccess]").each(function () {
                    this.checked = false;
                });
            }

        }

        function uncheckallPage(obj) {
            if (!obj.checked) {
                document.getElementById('All').checked = false;
            }

        }

        function Validation() {
            if ($("#UserName").val() == "") {
                alert("User Name is required");
                $("#UserName").focus();
                return false;
            }

            if ($("#Pwd").val() == "") {
                alert("Password is required");
                $("#Pwd").focus();
                return false;
            }

            if ($("#Pwd").val() != "") {
                if ($("#Pwd").val().length < 5) {
                    alert("Password should contain atleast 5 characters");
                    $("#Pwd").focus();
                    return false;
                }
            }

            if ($("#Email").val() != "") {
                var email = $("#Email").val();
                var valid = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
                if (!email.match(valid)) {
                    alert("Please Enter Valid Email Id");
                    $("#Email").focus();
                    return false;
                }
            }

            if ($("#Mobilenumber").val() != "") {
                var Mobilenumber = $("#Mobilenumber").val()
                if (isNaN(Mobilenumber)) {
                    alert("Please Enter Valid Mobile Number");
                    $("#Mobilenumber").focus();
                    return false;
                }
            }

            var g = 0;
            $("input:checkbox[name=PageAccess]:checked").each(function () {
                g = g + 1;
            });

            if (g == 0) {
                alert("Please Select Atleast One Page Access");
                return false;
            }

            CreateAdmin();
        }

        function CreateAdmin() {
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
                success: function (data) {
                    $("#loading").hide();
                    if (data.error != "1") {
                        $("#fancybox-overlay").css("display", "none");
                        $("#fancybox-wrap").css("display", "none");
                        alert(data.message);                        
                        Databind("1"); // this function is in index page
                    }
                    else {
                        alert(data.message);
                    }
                }
            });
            return false;

        }

    </script>
</head>
<body>
    <div style="width:650px;padding-left:20px;">
    <h2> Create Admin User</h2>
    <% using (Html.BeginForm("Create", "AdminPermission", FormMethod.Post))
       { %>
              <div class="editor-label">
                  User Name <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.UserName)%> 
              </div>
              <div class="editor-label">
                  Password <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBox("Pwd")%> 
              </div>
              <div class="editor-label">
                  Email Id 
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.Email)%> 
              </div>
              <div class="editor-label">
                  Mobile 
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.Mobilenumber)%> 
              </div>
              <div class="editor-label">
                  Page Access <span class="red">*</span>   
              </div>
              <div class="editor-field">
                   <div id="PageAccessDiv" class="divscroll">

<%--
                        <input type='checkbox' onclick='checkallPage()' id='All' name='All' value='' /> All <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddAdmin %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddAdmin %> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddCandidate %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddCandidate%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddEmployer %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddEmployer%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddConsultant %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddConsultant%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddJob %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddJob%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.EmployerSummary %>" /> <%= Dial4Jobz.Models.Constants.PageCode.EmployerSummary%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.EmployerReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.EmployerReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ImportData %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ImportData%> <br />                        
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ActivationReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ActivationReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ActivatedReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ActivatedReport%> <br />

                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport%> <br />

                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.VacancyReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.VacancyReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddChannelPartner %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddChannelPartner%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateFullReport %>" /> <%= Dial4Jobz.Models.Constants.PageCode.CandidateFullReport%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans%> <br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile%> <br />
                   --%>
                   </div>
              </div>
              <div>
            <input type="hidden" id="PermissionIds" name="PermissionIdsList[]" value="<%= ViewData["PermissionIds"] != null ? ViewData["PermissionIds"].ToString() : "" %>" />
        </div>
          <% } %>
              <input type="submit" id="contactbtn" value="Save" onclick="javascript:Validation();return false;" />  <br />      
  
    </div>
</body>
</html>
