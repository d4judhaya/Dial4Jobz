<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dial4Jobz - Edit Job
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.DeleteConfirmation.js")%>" type="text/javascript"></script>
    <link href="../../Content/DeleteConfirmation.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
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
    <% Html.BeginForm("EditJob", "Consult", FormMethod.Post, new { @id = "Save" }); %>
        <% Html.RenderPartial("LoggedInEmployerInfo"); %>
        <% Html.RenderPartial("JobEntry", Model); %>
        <% var js = "javascript:Dial4Jobz.Job.Save(this, '" + Model.Id + "');return false;"; %>   
             
        <input id="SaveJob" type="submit" value="Save" class="btn" onclick="<%: js %>" title="Click to add requirement and search for qualifying candidates" />
                     
                    <a href="#" class="topopup">Delete Current Job</a>
                   <%-- <%:Html.ActionLink("See the Job Details","AfterJobPost","Jobs", new { @class = "Edit_AfterJobPosting" })%> --%>
                    <br />
                    <div id="toPopup">
                    <div class="close"></div>
       	            <span class="ecs_tooltip">Press Esc to close <span class="arrow"></span></span>
		            <div id="popup_content" style="display:block">
                      
                        <p><h3>You have requested to delete the position titled as<%:Model.Position %>. Are you sure you want to delete?</h3></p>
                        <p align="center">
                            <%: Html.ActionLink("Delete Current Job", "Delete", "Jobs", new { id = Model.Id }, new { @class = "delete" })%>
                            <%: Html.ActionLink("No", "Edit", "Jobs", new { @class = "Edit_AfterJobPosting" })%>
                        </p>
                       
                    </div> 
                    </div> 

                    <div class="loader"></div>
            	    <div id="backgroundPopup"></div>

    <% Html.EndForm(); %>

     <div id="loading">
        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
     </div>   
</asp:Content>
