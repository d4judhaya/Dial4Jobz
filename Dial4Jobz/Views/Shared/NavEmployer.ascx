
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInOrganization != null; %>
                  
    <% if (isLoggedIn == true)
       { %> 
         <%: Html.ActionLink("Employer Home", "Index", "Candidates", null, new { title = "Back home" })%>
         <%: Html.ActionLink("Update Your Profile", "Profile", "Employer", null, new { title = "Employer Profile" })%>
         <%: Html.ActionLink("My Account", "DashBoard", "Employer", null, new { title = "Employer DashBoard" })%>
         <%: Html.ActionLink("Logout", "LogOff", "Account", null, new { title = "Logout of Dial4Jobz" })%>
         <a class="changeorganizationpassword" href="<%=Url.Content("~/ChangeOrganizationPassword")%>" title="Login to Dial4Jobz">Change Password</a><br />   
    <%} else { %>
        <%: Html.ActionLink("Employer Home", "Index", "Candidates", null, new { title = "Back home" })%>
        <a class="login" href="<%=Url.Content("~/login")%>?value=employer" title="Login to Dial4Jobz">Employer Login</a>
        <a class="signup" href="<%=Url.Content("~/signup")%>?value=employer" title="Create an account on Dial4Jobz">Post Your Vacancies</a>
        <%: Html.ActionLink("Go to Find Vacancies", "Index", "Home", null, new { @class="nav-candidate", title = "Click here if you are a candidate" })%>
    <%} %>
     


