<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% if (Request.IsAuthenticated==false) { %>
       
       <%: Html.ActionLink("Home", "Index", "AdminHome", null, new { title = "Back home" })%>
       <%: Html.ActionLink("Login", "Index", "Login", null, new { title = "Login to Dial4Jobz" })%>
       
<% } else { %> 
         <%: Html.ActionLink("Home", "Index", "ChannelHome", null, new { title = "Back home" })%>
         <%: Html.ActionLink("Change Password", "ChangePassword", "Login", null, new { title = "Change Password for Dial4Jobz", @class="ActionLink" })%>
         <%: Html.ActionLink("Logout", "LogOff", "Login")%>          
<% } %>