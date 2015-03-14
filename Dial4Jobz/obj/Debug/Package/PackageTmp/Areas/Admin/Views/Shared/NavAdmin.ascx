<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%  string[] userIdentityName =  Context.User.Identity.Name.Split('|');     
    if (Request.IsAuthenticated==false) { %>
       
       <%: Html.ActionLink("Home", "Index", "AdminHome", null, new { title = "Back home" })%>
        <a class="login" href="<%=Url.Content("~/login")%>?value=admin" title="Login to Dial4Jobz">Login</a>
       
<%--        <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz">Sign Up</a>--%>
       
<% } else if(userIdentityName.Length <= 1) { %> 
         <%: Html.ActionLink("Home", "Index", "AdminHome", null, new { title = "Back home" })%>
         <%: Html.ActionLink("Logout", "LogOff", "Account", new { Area = "" }, null)%>
           <a class="login" href="../../../Admin/AdminHome/ChangePassword" title="Change Password for Dial4Jobz">Change Password</a>
<% }
   else if (userIdentityName.Length > 1)
   { %>
       <%: Html.ActionLink("Home", "Index", "ChannelHome", new { area = "Channel" }, new { title = "Back home" })%>
         <%: Html.ActionLink("Change Password", "ChangePassword", "Login", new { area = "Channel" }, new { title = "Change Password for Dial4Jobz", @class = "login" })%>
         <%: Html.ActionLink("Logout", "LogOff", "Login", new { area = "Channel" }, new { title = "Logout" })%>     
       <%  }
    %>