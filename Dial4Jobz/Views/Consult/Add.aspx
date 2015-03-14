<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Add New Job    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavConsultant"); %>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
     <script src="<%=Url.Content("~/Scripts/PreventDoubleSubmit.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
    <tr>
    <td rowspan="3" colspan="3">
        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <a href="../../../../employer/employervas/index#HotResumes" target="_blank">Hot Resumes</a> </h3 >
        <%} else { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>
    </tr>
    </table>

 <% if (Request.IsAuthenticated == true)
       { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy.....
    </div>
    <% }
       else
       { %>
         <div class="identityname">
           Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
        </div>
    <% } %>

    <% Html.BeginForm("Add", "Consult", FormMethod.Post, new { @id = "Add" }); %>   

    <%--<%: Html.Hidden("consultId", ViewData["consultId"])%>  --%>
    <%:Html.Hidden("consultantId",ViewData["ConsultantId"]) %>
        <% Html.RenderPartial("JobEntry", new Dial4Jobz.Models.Job()); %>
        <input id="AddJob" type="submit" value="Add Job" class="btn" onclick="javascript:Dial4Jobz.Job.Add(this);return false;" title="Click to add job requirement and search for qualifying candidates" />
    <% Html.EndForm(); %>

     <div id="loading">
        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
     </div>   

       <% var message = ViewData["Message"];
          Response.Write("<script language=javascript>alert('" + message + "');</script>"); %> 
</asp:Content>


