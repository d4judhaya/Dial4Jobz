<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Find Jobs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <% Html.BeginForm("Index", "Jobs",  FormMethod.Get, new { }); %>

        <input id="what" name="what" type="text" />
        <input id="where" name="where" type="text" />
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" />

        <div class="advancedSearch" style="float:right">
           <%:Html.ActionLink("Advanced Search", "JobSearch", "Search")%>
        </div>

    <% Html.EndForm(); %>

   
        <% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
        <% bool isLoggedIn = loggedInCandidate != null; %>

        <% Html.BeginForm("Send", "Jobs", FormMethod.Post, new { @id = "Send" });  %>

        <div id="jobs">
        <ul id="job-list">    
         <h3><b>Welcome!!! You are in Job seeker's Zone..We wish you to get your Dream job...</b></h3>         
        <% foreach (var item in Model) { %> 
            <li class="job-list-item">        
                   <% if (isLoggedIn){ %>              
                        <div class="job-select">                   
                            <%: Html.CheckBox("Job" + item.Id.ToString(),new { id = item.Id }) %>                         
                        </div>           
                   <%  } %>
      
                    <div class="FindJobsfirst-line">
                       <%: Html.ActionLink(item.DisplayPosition, "Details", "Jobs", new { id = item.Id }, null)%>             
                   </div>

                   <div class="job-details">
                        <div class="fourth-line">
                           <% if (item.FunctionId.HasValue){ %>
                                Job Function:<%: item.GetFunction(item.FunctionId.Value).Name %>
                          <% } %> 
                  </div>
             </div>
             </li> 
         <% } %>   
    </ul>
   <%-- <% if(ViewData["moreUrl"] != null) { %>
            <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>
            <% var url = ViewData["moreUrl"] + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters, true); %>
            <a id="moreLink" href="<%= url %>" title="Click here to see more jobs">View More Jobs</a>
     <% } %>--%>
 </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
      <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
     <%: Html.ActionLink("Go to Employer Zone", "Index", "Employer", null, new { @class="nav-employer", title = "Click here if you are an employer", target="_blank" })%>
    <%--<% Html.RenderPartial("Nav"); %>--%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
      <h3>Post Resume</h3>
     <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Post Your Resume Here</a>
</asp:Content>
