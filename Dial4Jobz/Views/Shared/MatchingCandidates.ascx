<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Dial4Jobz.Models.Candidate>>" %>
<% Dial4Jobz.Models.Organization loggedInEmployer = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% Dial4Jobz.Models.Consultante loggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
 <% Dial4Jobz.Models.User loggedInAdmin = (Dial4Jobz.Models.User)ViewData["LoggedInAdmin"]; %>
<% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
<% Dial4Jobz.Models.User user = new Dial4Jobz.Models.User(); %>

<% bool isLoggedIn = loggedInEmployer != null; %>
 <% bool isAdminLoggedIn = loggedInAdmin != null; %>
<% bool isConsultLoggedIn = loggedInConsultant != null; %>
<% bool ActiveEmployers = false; %>
<% Html.BeginForm("Send", "Candidates", FormMethod.Post, new { @id = "Send" }); %>
   
    <% if (isLoggedIn || isConsultLoggedIn == true || isAdminLoggedIn==true)
       { %>
     <a id="SelectAll" href="javascript://">Select All</a>&nbsp;/&nbsp;
     <a id="SelectNone" href="javascript://">Select None</a><br />
    <%} %>

    <%if (Model.Count() > 0) { %>
        <% Html.RenderPartial("Candidates", Model, ViewData); %>
    <%} else { %>
        <span style="color:#324B81; font-size:14px;">Candidate for your search not available, Kindly advance or refine your search for better result!!!</span><br />
    <% } %>
  
  <%if(isLoggedIn==true) { %>
        <%  ActiveEmployers = _vasRepository.GetHORSSubscribed(loggedInEmployer.Id); %>
  <%}
    else if (isConsultLoggedIn == true)
    { %>
        <% ActiveEmployers = _vasRepository.GetHorsSubscribedByConsultant(loggedInConsultant.Id); %>
  <%}
    else
    { %>
    <% ActiveEmployers =false; %>
  <%} %>

        <div class="editor-label">
           <%: Html.Label("Matching Result To")%>
        </div>    
        <div class="editor-field">
             <%: Html.CheckBox("SendToUser", true)%> Candidates
             <%: Html.CheckBox("SendToOrganization", true)%> Organizations
        </div>

    <% 
        if(isLoggedIn==true || isConsultLoggedIn==true || isAdminLoggedIn== true) { %>
            <input type="hidden" value="false" name="sendmethod" id="sendmethod" />
            <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0);return false;" title="Send SMS">Send SMS</a>
             <%if (ActiveEmployers == true)
               { %>
            <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1);return false;" title="Send Email">Send Email</a>
            <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>
   <% } %>
<% } %>

<% Html.EndForm(); %>

