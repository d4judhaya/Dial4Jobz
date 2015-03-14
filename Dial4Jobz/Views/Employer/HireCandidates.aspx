<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	HireCandidates
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <% Html.BeginForm("Index", "Candidates", FormMethod.Get, new { }); %>
    <div class="searchForm">
        <input id="what" name="what" type="text"/>
        <input id="where" name="where" type="text"/>
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search candidates"/>
    </div>

  <div class="editor-field" style="float:right">
   <%:Html.ActionLink("Advanced Search", "CandidateSearch", "Search")%>
 </div><br />
<% Html.EndForm(); %>


<% Dial4Jobz.Models.Organization loggedInEmployer = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInEmployer != null; %>

<% Html.BeginForm("Send", "Candidates", FormMethod.Post, new { @id = "Send" }); %>
   
   <div id="candidates">
   <ul id="Candidates-list">
   <h3>Welcome !!!! You are in  Employer Zone.We wish you to get the right candidate for your Vacancy..</h3>
    <% foreach (var item in Model) { %>
        <li class="candidate-list-item">
        
       
    <div class="HireVacancyfirst-line">
        <%: Html.ActionLink(item.DiplayCandidate, "Details", "Candidates", new { id = item.Id }, new { target = "_blank" })%> 
    
    </div>
    <div class="candidate-details">
        <div class="fourth-line">
           <% if (!string.IsNullOrEmpty(item.Position))
              { %>
             Position: <%: item.Position%>
            <% } %>
        </div>
    </div>
     </li>
   <% } %>
   </ul>
    <% if(ViewData["moreUrl"] != null) { %>
         <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>
         <% var url = ViewData["moreUrl"] + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters, true); %>
         <a id="moreLink" href="<%= url %>" title="Click here to see more Candidates">View More Candidates</a>
      <% } %>
   </div>

   <% if (isLoggedIn) { %>
     <p>
        
        <input id="SMS" type ="submit" value="Send SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Candidate.Send(this, 0);return false;" />
        <input id="EMail" type ="submit" value ="Send Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Candidate.Send(this, 1);return false;" />
        <input id="Both" type="submit" value ="Send Email and SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Candidate.Send(this, 2);return false;" />
    </p>
<% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
     <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
