<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% Dial4Jobz.Models.Candidate LoggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
 <% bool isLoggedIn = LoggedInCandidate != null; %>
<%var value = Session["LoginAs"]; %>

<%if (value == "Candidate" || isLoggedIn == true)
  { %> 
        <%: Html.ActionLink("Candidate Home", "Index", "Home", null, new { title = "Back home" })%>
        <%: Html.ActionLink("Update Your Resume", "Edit", "Candidates", null, new { title = "Candidate Profile" })%>
        <%: Html.ActionLink("My Account","DashBoard","Candidates",null, new { title="Candidate Dashboard"}) %>
        <%: Html.ActionLink("Logout", "LogOff", "Account", null, new { title = "Logout of Dial4Jobz" })%>
        <a class="changecandidatepassword" href="<%=Url.Content("~/ChangeCandidatePassword")%>" title="Login to Dial4Jobz">Change Password</a><br/>   

<%} else { %>
         <%: Html.ActionLink("Candidate Home", "Index", "Home", null, new { title = "Back home" })%>
        <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Candidate Login</a>
        <%--<a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz">Post Your Resumes</a>--%>
        <a class="signup" href="<%=Url.Content("~/signup")%>?value=candidate">Post Your Resumes</a>
        <%: Html.ActionLink("Go to Employer Zone", "MatchCandidates", "Employer", null, new { @class = "nav-employer", title = "Click here if you are an employer", target = "_blank" })%>
<%} %>



<%--<% if (Request.IsAuthenticated==false) { %>
        <%: Html.ActionLink("Candidate Home", "Index", "Home", null, new { title = "Back home" })%>
        <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Login</a>
        <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz">Post Your Resumes</a>
        <%: Html.ActionLink("Go to Employer Zone", "MatchCandidates", "Employer", null, new { @class = "nav-employer", title = "Click here if you are an employer", target = "_blank" })%>

<% } else if (login == "Candidate" && Request.IsAuthenticated==true) { %> 
        <%: Html.ActionLink("Candidate Home", "Index", "Home", null, new { title = "Back home" })%>
        <%: Html.ActionLink("Update Your Resume", "Edit", "Candidates", null, new { title = "Candidate Profile" })%>
        <%: Html.ActionLink("My Account","DashBoard","Candidates",null, new { title="Candidate Dashboard"}) %>
        <%: Html.ActionLink("Logout", "LogOff", "Account", null, new { title = "Logout of Dial4Jobz" })%>
        <a class="changecandidatepassword" href="<%=Url.Content("~/ChangeCandidatePassword")%>" title="Login to Dial4Jobz">Change Password</a><br/>   
<% } %>--%>