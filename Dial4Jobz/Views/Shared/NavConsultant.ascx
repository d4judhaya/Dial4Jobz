<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
<% bool isLoggedIn = LoggedInConsultant != null; %>
                  
    <% if (isLoggedIn == true)
       { %> 
         <%: Html.ActionLink("Consultant Home", "Index", "Consult", null, new { title = "Back home" })%>
         <%: Html.ActionLink("Update Your Profile", "Profile", "Consult", null, new { title = "Consult Profile" })%>
         <%: Html.ActionLink("Logout", "LogOff", "Account", null, new { title = "Logout of Dial4Jobz" })%>
         <a class="changeorganizationpassword" href="<%=Url.Content("~/ChangeOrganizationPassword")%>" title="Login to Dial4Jobz">Change Password</a><br />   
    <%} else { %>

        <%: Html.ActionLink("Consultant Home", "Index", "Consult", null, new { title = "Back home" })%>
        <%: Html.ActionLink("Login","LogOn","Consult",null,new { title="Login as consultant"}) %>
        <%: Html.ActionLink("Register", "Register", "Consult", null, new { title = "Back home" })%>
        <%: Html.ActionLink("Go to Find Vacancies", "Index", "Home", null, new { @class="nav-candidate", title = "Click here if you are a candidate" })%>
    <%} %>
     


