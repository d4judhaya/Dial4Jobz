<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matching Candidates
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% bool isLoggedIn = loggedInOrganization != null; %>
 <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Html.RenderPartial("Main/MainVas"); %>

  
   
   <% if (Request.IsAuthenticated == true) { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b> , Welcome!You are in Employer Zone.
    </div>
    <% } else { %>
        <div class="identityname">
           Welcome!!! You are in Employer Zone.
        </div>
    <% } %>
        
    <% Html.BeginForm("MatchCandidates", "Employer", FormMethod.Get, new { }); %>
        <input id="what" name="what" type="text" />
        <input id="where" name="where" type="text" />
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" /><br />
        <button id="gbqfba" aria-label="Advanced Search" name="btnK" class="gbqfba"><span id="gbqfsa"><%:Html.ActionLink("Advanced Search", "CandidateSearch", "Search")%></span></button>
    <% Html.EndForm(); %>
    <% Html.RenderPartial("MatchingCandidates", Model);%> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% bool isLoggedIn = loggedInOrganization != null; %>

  <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>
  <% var MaxExperience = 15; %>
  <% var minSalary = (int)Math.Round(Model.Min(m => m.AnnualSalary).GetValueOrDefault(0)); %>
  <% var maxSalary = 1500000; %>
  <% if (maxSalary > 300000 && !filters.ContainsKey("minsalary") && !filters.ContainsKey("maxsalary")) { %>    
            <div class="section">
            <h5>Salary Range</h5>       
            <ul>
                <% for (int i = 0; i < 50000; i = i + 50000) { %>
                       <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "minsalary=" + i.ToString() + "&maxsalary=" + (i + 50000).ToString(); %>
                       <li><a href="<%: url %>"><%: i.ToString("c0", new System.Globalization.CultureInfo("en-IN")) + " - " + (i + 50000).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%></a></li>                   
                <% } %>
            </ul>    

            <ul>
                <% for (int i = 50000; i < 300000; i = i + 50000) { %>
                       <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "minsalary=" + i.ToString() + "&maxsalary=" + (i + 50000).ToString(); %>
                       <li><a href="<%: url %>"><%: i.ToString("c0", new System.Globalization.CultureInfo("en-IN")) + " - " + (i + 50000).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%></a></li>                   
                <% } %>
            </ul>
        </div>
    <% } %> 


        <% if (!filters.ContainsKey("minexperience") && !filters.ContainsKey("maxexperience")){%>
            <div class="section">
                <% var topExperience = Model.GroupBy(q => q.MaxExperience / 31536000)
                                       .OrderByDescending(gp => gp.Count())
                                       .Take(5)
                                       .Select(g => g.Key).ToList(); %>
                <h5>Experience</h5>       
                <ul>            
                    <%for (int i=0;i<MaxExperience;i=i+3) { %>
                    <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "minexperience=" + i.ToString() + "&maxexperience=" + (i + 3).ToString(); %>
                    <li><a href="<%: url %>"><%: i.ToString("", new System.Globalization.CultureInfo("")) + " - " +(i + 3) %> Years</a></li>                            
                    <% } %>     
                </ul>
            </div>
     <% } %>  
      


    <% if (!filters.ContainsKey("function")){%>
    <div class="section">
    <% var topFunctions = Model.Where(q => q.Function != null)
                           .GroupBy(q => q.Function.Name)
                           .OrderByDescending(gp => gp.Count())
                           .Take(5)
                           .Select(g => g.Key).ToList(); %>
       <%-- <% var topFunctions = Model.GroupBy(q => q.Function.Name)
                            .OrderByDescending(gp => gp.Count())
                            .Take(5)
                            .Select(g => g.Key).ToList(); %>--%>
        <h5>
            Function</h5>
        <ul>
            <% foreach (var function in topFunctions)  { %>
                 <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "func=" + function; %>
            <li><a href="<%:url %>"><%:function %></a></li>
            <% } %>
        </ul>
    </div>
    <% } %>

    <%if (!filters.ContainsKey("Position")){ %>
    <div class ="section">
        <% var topPosition = Model.GroupBy(q=>q.Position)
                           .OrderByDescending(gp=>gp.Count())
                           .Take (5)
                           .Select(g=>g.Key).ToList(); %>
                              
        <h5>Position</h5>
                
        <ul>
        <%foreach (var position in topPosition)
        { %>
            <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "posi=" + position; %>
            <li><a href="<%: url %>"><%: position %></a></li>       
        <% } %>
        </ul>
    </div>
    <% } %>  

    <%if (!filters.ContainsKey("gender")){ %>
    <div class ="section">
 
        <% var topGender = Model.GroupBy(q => q.Gender == 0 ? "Male" : "Female")
                         .OrderByDescending(gp=>gp.Count())
                         .Take (5)
                         .Select(g=>g.Key).ToList();%>
                            
        <h5>Gender</h5>                        
        <ul>
        <%foreach (var gender in topGender){ %>
        <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "gen=" + gender; %>
        <li><a href="<%: url %>"><%:gender %></a></li>       
        <% } %>
        </ul>
    </div>
   <% } %>

   <%if (isLoggedIn == true)
     { %>
   <div class="vastext">
      
       Get Matching candidates as soon as they Apply <%:Html.ActionLink("Resume Alert", "Index", "EmployerVas")%><br />

       It is estimated that more than 40% of the resume have incorrect information..Do <%:Html.ActionLink("Reference Check", "Index", "EmployerVas")%> before selection.<br />

       Search"part-time" or "work from home" candidates - <%:Html.ActionLink("Post Your Vacancy", "Add", "Jobs")%> & view Suitable Candidates"
   </div>

   <%} else { %>
   <div class="vastext">
          Get Matching candidates as soon as they Apply <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Resume Alert</a><br />

          It is estimated that more than 40% of the resume have incorrect information..Do <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Reference Check</a> before selection.<br />

          Search"part-time" or "work from home" candidates - <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Post Your Vacancy</a> & view Suitable Candidates".
    </div>
   <% } %>



</asp:Content>
