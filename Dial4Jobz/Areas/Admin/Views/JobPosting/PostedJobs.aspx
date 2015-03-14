<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Posted Jobs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <% if (Request.IsAuthenticated == true)
       { %>
    <div class="identityname">
        Welcome!!! <b>
            <%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy.....
    </div>
    <% }
       else
       { %>
    <div class="identityname">
        Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
    </div>
    <% } %>
    <h2>
        You have posted Following jobs</h2>
    <table>
        <tr>
            <td>
                <h3>Positions</h3>
                <br />
                
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        <%if (job.Position != "")
                          { %>
                        <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                        <%}
                          else
                          { %>
                             <%--<%: Html.ActionLink("", "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>--%>
                            <%} %>
                <br />
                <% } %>
            </td>
            <td>
                <h3>Posted Date</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        <%:job.CreatedDate %>
                <br />
                <% } %>
            </td>
            <td>
                <h3>Edit the Job</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        <%: Html.ActionLink("Edit", "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                <br />
                <% } %>
            </td>
            <td>
                <h3>
                    Delete the Job</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                    <a href="#" class="topopup">Delete</a>
                                       
                    <br />
                    <div id="toPopup">
                    <div class="close"></div>
       	            <span class="ecs_tooltip">Press Esc to close <span class="arrow"></span></span>
		            <div id="popup_content" style="display:block">
                      
                        <p>You have requested to delete the position titled as <b><%:job.Position %></b>. Are you sure you want to delete?</p>
                        <p align="center">
                            <%: Html.ActionLink("Yes", "Delete", "Jobs", new { id = job.Id }, new { @class = "delete" })%>
                            <%: Html.ActionLink("No", "PostedJobs", "Jobs", new { @class = "Edit_AfterJobPosting" })%>
                        </p>
                    </div> 
                    </div> 

                    <div class="loader"></div>
            	    <div id="backgroundPopup"></div>
                <% } %>

            </td>
        </tr>
    </table>

                                
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
  
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.DeleteConfirmation.js")%>" type="text/javascript"></script>
   <link href="../../Content/DeleteConfirmation.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
  <div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
</asp:Content>
