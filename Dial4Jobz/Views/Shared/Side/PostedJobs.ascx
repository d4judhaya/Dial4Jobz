<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInOrganization != null; %>

<div class="section">
    <h5>Posted Jobs</h5>    
    <ul>
        <li>
            <% if (isLoggedIn){ %>
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                     { %>
                     <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { title = "Edit this job posting" })%> <br/>       
                 <% } %>

            <% } else { %>
                <a class="login link-completeprofile-frontpage" href="<%=Url.Content("~/login")%>" title="Post a job">Post a job</a>
            <% } %>
        </li>
        <li>
            <% if (isLoggedIn){ %>
                
                     <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { title = "Edit this job posting" })%> <br/>       
                

            <% }%>               
            
        </li>
       </ul>
</div>

