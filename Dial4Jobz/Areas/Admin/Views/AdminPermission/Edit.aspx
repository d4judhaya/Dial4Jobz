<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.User>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Update</title>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            $.get("/Admin/AdminPermission/getAdminPermissions/", function (data) {
                //Developer Note: For Exact Match check process.....
                var PermissionIds = $("#PermissionIds").val();
                var PermissionArray = new Array();
                debugger;
                PermissionArray = PermissionIds.split(",");
                debugger;
                $.each(data.permissions, function (key, value) {
                    if (key == 0) {
                        $("#PageAccessDiv").append("<input type='checkbox' id='All' onclick='checkallPage()' name='All' value='' /> Any <br />");
                        debugger;
                    }
                    if ($.inArray("" + value.Id + "", PermissionArray) != -1) {
                        $("#PageAccessDiv").append("<input type='checkbox' checked='checked' onclick='uncheckallPage(this)' name='PageAccess' value='" + value.Id + "' /> " + value.Name + " <br />");
                      
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

            UpdateAdmin();
        }

        function UpdateAdmin() {
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
    <h2> Update Admin User</h2>
    <% Dial4Jobz.Models.AdminPermission permissions=new Dial4Jobz.Models.AdminPermission(); %>
    <% using (Html.BeginForm("Edit", "AdminPermission", FormMethod.Post))
       { %>
              <%: Html.HiddenFor(model => model.Id)%>
              <div class="editor-label">
                  User Name <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.UserName)%> 
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
               </div>
              </div>
        <div>
            <input type="hidden" id="PermissionIds" name="PermissionIdsList[]" value="<%= ViewData["PermissionIds"] != null ? ViewData["PermissionIds"].ToString() : "" %>" />
        </div>
            <%--  <div class="editor-field">.

              <%  if (Model.PageCode != null)
                  {
                      string[] Page_Code = Model.PageCode.Split(','); %>
                   <div id="PageAccessDiv" class="divscroll">
                        <input type='checkbox' onclick='checkallPage()' id='All' name='All' value='' /> All <br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddAdmin)), new { value = Dial4Jobz.Models.Constants.PageCode.AddAdmin, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.AddAdmin%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddCandidate)), new { value = Dial4Jobz.Models.Constants.PageCode.AddCandidate, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.AddCandidate%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddEmployer)), new { value = Dial4Jobz.Models.Constants.PageCode.AddEmployer, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.AddEmployer%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddConsultant)), new { value = Dial4Jobz.Models.Constants.PageCode.AddConsultant, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.AddConsultant%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddJob)), new { value = Dial4Jobz.Models.Constants.PageCode.AddJob, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.AddJob%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry%> <br />                         
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerSummary)), new { value = Dial4Jobz.Models.Constants.PageCode.EmployerSummary, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.EmployerSummary%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerReport)), new { value = Dial4Jobz.Models.Constants.PageCode.EmployerReport, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.EmployerReport%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateReport)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateReport, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.CandidateReport%> <br /> 
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ImportData)), new { value = Dial4Jobz.Models.Constants.PageCode.ImportData, onclick = "uncheckallPage(this)" })%> <%= Dial4Jobz.Models.Constants.PageCode.ImportData%> <br />                         
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s=>s.Contains(Dial4Jobz.Models.Constants.PageCode.UserReport)),new {value=Dial4Jobz.Models.Constants.PageCode.UserReport, onclick="uncheckallpage(this)"}) %><%=Dial4Jobz.Models.Constants.PageCode.UserReport %><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s=>s.Contains(Dial4Jobz.Models.Constants.PageCode.ActivationReport)),new {value=Dial4Jobz.Models.Constants.PageCode.ActivationReport, onclick="uncheckallpage(this)"}) %><%=Dial4Jobz.Models.Constants.PageCode.ActivationReport %><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s=>s.Contains(Dial4Jobz.Models.Constants.PageCode.ActivatedReport)),new {value=Dial4Jobz.Models.Constants.PageCode.ActivatedReport, onclick="uncheckallpage(this)"}) %><%=Dial4Jobz.Models.Constants.PageCode.ActivatedReport %><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport %><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport)), new { value = Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport)), new { value = Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport %><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport)), new { value = Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.VacancyReport)), new { value = Dial4Jobz.Models.Constants.PageCode.VacancyReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.VacancyReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddChannelPartner)), new { value = Dial4Jobz.Models.Constants.PageCode.AddChannelPartner, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.AddChannelPartner%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport)), new { value = Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails)), new { value = Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateFullReport)), new { value = Dial4Jobz.Models.Constants.PageCode.CandidateFullReport, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.CandidateFullReport%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans)), new { value = Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans%><br />
                        <%= Html.CheckBox("PageAccess", Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile)), new { value = Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile, onclick = "uncheckallpage(this)" })%><%=Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile%><br />

                   </div>
               <% }
                  else
                  { %>
                  <div class="divscroll">
                        <input type='checkbox' onclick='checkallPage()' name='All' value='' /> All <br />
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
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.UserReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.UserReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ActivationReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.ActivationReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ActivatedReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.ActivatedReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.VacancyReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.VacancyReport %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddChannelPartner %>" /><%=Dial4Jobz.Models.Constants.PageCode.AddChannelPartner %><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport %><br />
                        <input type='checkbox' onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails %>" /> <%= Dial4Jobz.Models.Constants.PageCode.AllowSeeContactDetails%> <br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.CandidateFullReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.CandidateFullReport%><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport%><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport %>" /><%=Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport%><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans %>" /><%=Dial4Jobz.Models.Constants.PageCode.AllowSpecialPlans%><br />
                        <input type="checkbox" onclick='uncheckallPage(this)' name='PageAccess'  value="<%= Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile %>" /><%=Dial4Jobz.Models.Constants.PageCode.AddWithoutMobile%><br />
                   </div>

               <% } %>
              </div>

              <input type="submit" id="contactbtn" value="Update" onclick="javascript:Validation();return false;" />  <br />      --%>
    <% } %>
    <input type="submit" id="contactbtn" value="Update" onclick="javascript:Validation();return false;" />  <br />
    </div>
</body>
</html>
