<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PhoneNoVerification
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <table>
    <tr>
   <%-- <td rowspan="3" colspan="3">
        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <%:Html.ActionLink("Hot Resumes", "Index", "EmployerVas")%> </h3 >
        <%} else { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>--%>
    </tr>
    </table>

    <h2>PhoneNoVerification</h2>

    <% Html.BeginForm("PhoneNoVerification", "Employer", FormMethod.Post, new { @id = "Save" });
                {%>
                  <div class="editor-label">
                    <%: Html.Label("Verification Code")%>
                       <%--<span class="red">*</span>--%>
                        <span class="red">You will receive verification code by sms.</span>
                    </div>

                    <div class="editor-field">
                        <%:Html.Hidden("OrganizationId", ViewData["OrganizationId"].ToString())%>
                        <%: Html.TextBox("PhVerificationNo", null, new { @title = "Enter the Verification Code" })%>
                    </div>

                    <div class="editor-field" style="display:none;">
                         <%: Html.CheckBox("IsPhoneVerified")%> 
                    </div>

                  <input id="Save" type="submit" value="Submit" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Employer.PhoneNoVerification(this);return false;"/>
                  <input id="Cancel" type="submit" value="Cancel" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Employer.PhoneNoVerification(this);return false;"/>
                  
        <% } %>
      

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
      <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
      <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Auth.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
      <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">

</asp:Content>
