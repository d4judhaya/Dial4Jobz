<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Dial4Jobz.Models.Job>>" %>
<% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
<% bool isLoggedIn = loggedInCandidate != null; %>

<input type="hidden" name="jobId" id="jobId" value="<%= Html.Encode(ViewData["JobIdView"])%>" />

<% Html.BeginForm("Send", "Jobs", FormMethod.Post, new { @id = "Send" });  %>

<%if (Model.Count() > 0)
  { %>
    <% Html.RenderPartial("Jobs", Model, ViewData); %>      
<%}
  else
  { %>
      <span style="color:#324B81; font-size:14px;">Vacancy for your search is not available!!</span>
<% } %>


 <% if (isLoggedIn) { %>
     <%-- <p>
        <input id="SMS" type ="submit" value="Apply by SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Candidate.Send(this, 0);return false;" />
        <input id="EMail" type ="submit" value ="Apply by Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Candidate.Send(this, 1);return false;" />
        <input id="Both" type="submit" value ="Apply by Email and/or SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Candidate.Send(this, 2);return false;" />
    </p>--%>
<% } %>
<% Html.EndForm(); %>