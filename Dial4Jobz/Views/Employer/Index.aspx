<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="NavContent" runat="server">
 <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
 <%if (LoggedInConsultant != null)
   { %>
    <% Html.RenderPartial("NavConsultant"); %>
 <%}
   else
   {%>
	<% Html.RenderPartial("NavEmployer"); %>
    <%} %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
 <% bool isConsultLoggedIn = LoggedInConsultant != null; %>
 <% bool isLoggedIn = loggedInOrganization != null; %>

 <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
  
  <% Html.RenderPartial("Main/MainVas"); %>


 <%if (isLoggedIn == false && isConsultLoggedIn == false)
    { %>
        <h3>If you are Consultant <%: Html.ActionLink("Click Here", "Index", "Consult", null, new { target = "_blank" })%></h3>
  <%} %>

  <%--<ul id="js-news" class="js-hidden">
        <li class="news-item"><a href="../../../../Home/ChannelsEarningOpportunities">Join and Earn with Dial4Jobz as a Channel Partner...</a></li>
    </ul>--%>

    <div class="ticker">
		<ul>
		<li><a href="../../../../Home/ChannelsEarningOpportunities">Join and Earn with Dial4Jobz as a Channel Partner...</a></li>
		</ul>
	</div>



   <% if (isLoggedIn != false){ %>
    <% var employerVerification = _repository.GetOrganization(loggedInOrganization.Id); %>
            <% if (loggedInOrganization != null) 
               { %>                                
                        <%if (employerVerification.IsMailVerified == true) {%>
                            <% Session["EmployerVerification"] = true; %>
                        <% } else {%>
                            <%  Session["EmployerVerification"] = false;%>
                        <%} %>
                <%} %>
    <%} %>



 <%  if (isLoggedIn != false && (Boolean)Session["EmployerVerification"] == false)
      {%>    
        <script type="text/jscript">
            $(function confirmPopupBox() {

                $('#popup_Verification').fadeIn("slow");
                $("#Send").css({
                    "opacity": "0.3"
                });
            });    
        </script>          
                <% Session["EmployerVerification"] = true; %>      <%-- End vignesh--%>
        <% }
      else
      { %>
            
        <%} %>
        
       <div id="popup_Verification" class="confirmpopup_box" style="display: none">
        <table style="border-color: White; width: 100%;">
            <tr>
                <td align="center" style="border-color: rgb(39, 199, 199); color:Blue; font-family: Calibri; font-size: 26px; font-weight: bold; width: 100%;">
                   Your Email-Id is not verified.
                </td>
            </tr>
            <tr>
                <td align="center" style="border-color: White; width: 100%;">
                    <a href="<%: Url.Action("EmailVerification", "Employer") %>"><img src="../../Content/Images/VerifyNow.png" class="btn" width="100px" alt="Verify" height="25px" /></a>
                    <a><img src="../../Content/Images/VerifyCancel.png" class="btn" width="100px" alt="cancel" height="25px" onclick="javascript:$('#popup_Verification').fadeOut('slow');$('#Send').css({'opacity': '1'});"/></a>
                 </td>
            </tr>
        </table>
    </div>
       

 <script type="text/jscript">
     $(document).ready(function () {
         var tempDDVal = '<%= Session["Function"] %>';
         $("#ddlRoles option:contains(" + tempDDVal + ")").attr('selected', 'selected');
     });
     function ddlRoleChanged() {
         var selectedValue = $('#ddlRoles option:selected').val();
         window.location = '/employer/MatchCandidates?func=' + selectedValue.valueOf();
     };        
 </script>      

    <% if (isLoggedIn == true || isConsultLoggedIn==true)
       { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates.
    </div>
    <% }
       else
       { %>
         <div class="identityname">
           Welcome!!! You are in Employer Zone.
        </div>
    <% } %>

<% Html.BeginForm("MatchCandidates", "Employer", FormMethod.Get, new { }); %>
<div class="searchForm">
<input id="what" name="what" type="text"/>
<input id="where" name="where" type="text"/>
<input id="Search" type="submit" value="Search" class="btn-search" title="Search candidates"/>
</div><br />

 <label id="lblEastSearch" style="color:InfoText;">Easy Search: </label>
   <%= Html.DropDownList("RolesForJobSeekers", ViewData["RolesForJobSeekers"] as SelectList,"Find Candidates" , new { onchange = "javascript:ddlRoleChanged();", id="ddlRoles",  @class = "dropdownStyle2"})%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(or)       
   <%:Html.ActionLink("Advanced Search", "CandidateSearch", "Search")%>

    <% Html.EndForm(); %>
    <% Html.RenderPartial("MatchingCandidates", Model); %> 
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideContent" runat="server">

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
      
       Get Matching candidates as soon as they Apply <a href="../../../../employer/employervas/index#ResumeAlert" target="_blank">Resume Alert</a><br /><br />

       It is estimated that more than 40% of the resume have incorrect information..Do <a href="../../../../employer/employervas/index#backgroundCheck" target="_blank">Background Check</a> before selection.<br /><br />

       Search"part-time" or "work from home" candidates - <%:Html.ActionLink("Post Your Vacancy", "Add", "Jobs")%> & view Suitable Candidates"
   </div>

   <%} else { %>
       <div class="vastext">
       Get Matching candidates as soon as they Apply <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Resume Alert</a><br /><br />

       It is estimated that more than 40% of the resume have incorrect information..Do <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Reference Check</a> before selection.<br /><br />

       Search"part-time" or "work from home" candidates - <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Post Your Vacancy</a> & view Suitable Candidates"
       </div><br />
   <% } %>

   


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.GoogleAnalyticsSite.js")%>" type="text/javascript"></script>
 <%--<script src="<%=Url.Content("~/Scripts/jquery.ticker.js")%>" type="text/javascript"></script>
    <link href="../../Content/ticker-style.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $('#js-news').ticker({
                speed: 0.10,
                /*htmlFeed: false,*/
                fadeInSpeed: 600,
                titleText: 'Hot & Fresh!!!'
            });
        });
    </script>--%>
    
    <script type="text/javascript">
        $(document).ready(function () {
            var tblID = $("body").find("table").attr("HotResumes");
        });
    </script>
</asp:Content>
