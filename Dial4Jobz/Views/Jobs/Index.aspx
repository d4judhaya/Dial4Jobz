<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matching Jobs
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
 <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
  <% bool isConsultLoggedIn = LoggedInConsultant != null; %>
 <% bool isLoggedIn = loggedInCandidate != null; %>
 <%var login = Session["LoginAs"]; %>

 <% Html.RenderPartial("Main/CandidatesVas"); %>


 <%if (isLoggedIn == false && isConsultLoggedIn == false)
    { %>
        <h3>If you are Consultant <%: Html.ActionLink("Click Here", "Index", "Consult", null, new { target = "_blank" })%></h3>
  <%} %>


 <%-- <ul id="js-news" class="js-hidden">
        <li class="news-item"><a href="#">You May Get Matching Job Alerts For Few Hundred Rupees Onward...</a></li>
        <li class="news-item"><a href="#">Stand Out in the Crowd Be Reachable To More Employers /Recruiters </a></li>
        <li class="news-item"><a href="#">Right Interviews at The Right Time!!! Our Specialist Can Fix Interviews For You...</a></li>
    </ul>--%>

    <div class="ticker">
		<ul>
		<li>You May Get Matching Job Alerts For Few Hundred Rupees Onward...</li><%--style="margin: 0px; display: list-item; font-size:13px; font-weight:bold;"--%>
		<li>Stand Out in the Crowd Be Reachable To More Employers /Recruiters </li>
		<li>You May Get Matching Job Alerts For Few Hundred Rupees Onward...</li>
		<li>Right Interviews at The Right Time!!! Our Specialist Can Fix Interviews For You...</li>
		</ul>
	</div>

 <% if (isLoggedIn && (Boolean)Session["PhoneVerification"])
    {%>    
        <script type="text/jscript">
            $(function confirmPopupBox() {
                       
                $('#popup_Verification').fadeIn("slow");
                $("#Send").css({
                    "opacity": "0.3"
                });
            });    
        </script>          
                <%Session["PhoneVerification"] = false; %>      
        <% } else { %>
            
        <%} %>


      <% if (login == "Candidate" && Request.IsAuthenticated == true)
       { %>
        <div class="identityname">
            Welcome!!! <b><%: this.Page.User.Identity.Name%></b> , You are in Job seeker's Zone..
        </div>
    <% } else if(isConsultLoggedIn==true) { %>
        <div class="identityname">
            Welcome!!! <b><%: this.Page.User.Identity.Name%></b> , You are in Job seeker's Zone..
        </div>
        
    <% } else { %>
     <div class="identityname">
           Welcome!!! You are in Job seeker's Zone..
        </div>
    <%} %>
    
        <% Html.BeginForm("Index", "Jobs", FormMethod.Get, new { }); %>
        <input id="what" name="what" type="text" />
        <input id="where" name="where" type="text" />
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" /><br />
        <%--<button id="gbqfba" aria-label="Advanced Search" name="btnK" class="gbqfba"><span id="gbqfsa">--%>
        <%:Html.ActionLink("Advanced Search", "JobSearch", "Search")%><%--</span></button>--%>
        <% Html.EndForm(); %>
        <p>
        <% Html.RenderPartial("MatchingJobs", Model); %> 
        </p>

    <div id="popup_Verification" class="confirmpopup_box" style="display: none">
        <table style="border-color: White; width: 100%;">
            <tr>
                <td align="left" style="border-color: White; font-family: Calibri; font-size: 14px;
                    font-weight: bold; width: 100%;">
                    Your Mobile Number is not verified.Employers give preference to call the candidates
                    whose Mobile Number is Verified.
                </td>
            </tr>
            <tr>
                <td align="center" style="border-color: White; width: 100%;">
                <a href="<%: Url.Action("VerifyCandidate", "Candidates") %>"><img src="../../Content/Images/VerifyNow.png" class="btn" width="100px" height="25px"/></a>
                <a><img src="../../Content/Images/VerifyCancel.png" class="btn" width="100px" height="25px" onclick="javascript:$('#popup_Verification').fadeOut('slow');$('#Send').css({'opacity': '1'});"/></a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.GoogleAnalyticsSite.js")%>" type="text/javascript"></script>
     <%--<script src="<%=Url.Content("~/Scripts/jquery.ticker.js")%>" type="text/javascript"></script>
    <link href="../../Content/ticker-style.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $('#js-news').ticker({
                speed: 0.10,
                /*htmlFeed: false,*/
                fadeInSpeed: 600,
                titleText: 'Dial4Jobz News'
            });
        });
    </script>--%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideContent" runat="server">    
    <% var minSalary = (int)Math.Round(Model.Min(m => m.Budget).GetValueOrDefault(0)); %>
    <% var maxSalary = 1500000; %>
    <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>

    <% if (maxSalary > 300000 && !filters.ContainsKey("minsalary") && !filters.ContainsKey("maxsalary")) { %>    
            <div class="section">

            <h5>Annual Salary Range</h5> 
             
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

           <%-- <ul>
                <% for (int i = 1500000; i < 2500000; i = i + 1000000) { %>
                       <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "minsalary=" + i.ToString() + "&maxsalary=" + (i + 1000000).ToString(); %>
                       <li><a href="<%: url %>"><%: i.ToString("c0", new System.Globalization.CultureInfo("en-IN")) + " - " + (i + 1000000).ToString("c0", new System.Globalization.CultureInfo("en-IN")) %></a></li>                   
                <% } %>
            </ul>--%>
        </div>
    <% } %> 


   <% if (!filters.ContainsKey("organization")) { %>
        <div class="section">
            <% var topOrganizations = Model.GroupBy(q => (q.Organization!=null ? q.Organization.Name :""))
                                   .OrderByDescending(gp => gp.Count())
                                   .Take(5)
                                   .Select(g => g.Key).ToList(); %>
            <h5>Organization</h5>       

            <ul>
                <% foreach (var organization in topOrganizations)  { %>
                     <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "org=" + organization; %>
                     <li><a href="<%: url %>"><%: organization %></a></li>       
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
                    <%for (int i=0;i<15;i=i+3) {%>
                        <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "minexperience=" + i.ToString() + "&maxexperience=" + (i + 3).ToString(); %>
                        <li><a href="<%: url %>"><%: i.ToString("", new System.Globalization.CultureInfo("")) + " - " +(i + 3) %> Years</a></li>                            
                    <% } %>     
                </ul>
            </div>
     <% } %>  
 
    <% if (!filters.ContainsKey("function")){%>
           <div class="section">
                <% var topFunctionIds = Model.Where(q => q.FunctionId.HasValue)
                                       .GroupBy(q => q.FunctionId.Value)
                                       .OrderByDescending(gp => gp.Count())
                                       .Take(5)
                                       .Select(g => g.Key).ToList(); %>
                <h5>Function</h5>       

                <ul>
                    <% Dial4Jobz.Models.Repositories.Repository repository = new Dial4Jobz.Models.Repositories.Repository();  %>                    
                    <% foreach (var functionId in topFunctionIds)  { %>
                         <% var function = repository.GetFunction(functionId); %>
                         <% var url = "" + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters) + "function=" + function.Name; %>
                         <li><a href="<%:url %>"><%:function.Name%></a></li>
                    <% } %>
                </ul>
            </div>
    <% } %>    
    
                  
   </asp:Content>

