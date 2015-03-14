<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matching Candidates
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% bool isLoggedIn = loggedInOrganization != null; %>
 <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>

  <% Html.RenderPartial("Main/MainVas"); %>

  <% if (isLoggedIn != false)    { %>
    <%var employerVerification = _repository.GetOrganization(loggedInOrganization.Id); %>
            <% if (loggedInOrganization != null) 
               { %>                                 <%-- By vignesh--%>
                        <%if (employerVerification.IsMailVerified == true)       
                          {%>
                            <% Session["EmployerVerification"] = true; %>
                        <%  } else {%>
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
                  <%-- By vignesh--%>  Your Email-Id is not verified.
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

<%-- <ul id="js-news" class="js-hidden">
        <li class="news-item"><a href="../../../../Home/ChannelsEarningOpportunities">Join and Earn with Dial4Jobz as a Channel Partner...</a></li>
  </ul>--%>
  <div class="ticker">
		<ul>
		<li><a href="../../../../Home/ChannelsEarningOpportunities">Join and Earn with Dial4Jobz as a Channel Partner...</a></li>
		</ul>
	</div>


 <script type="text/jscript">
     $(document).ready(function () {
         var tempDDVal = '<%= Session["Function"] %>';
         $("#ddlRoles option:contains(" + tempDDVal + ")").attr('selected', 'selected');
     });
     function ddlRoleChanged() {
         var selectedValue = $('#ddlRoles').val();
         window.location = '/candidates/Index?func=' + selectedValue.valueOf();
     };        
</script>   

   <% if (Request.IsAuthenticated == true) { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b> , Welcome!You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
    </div>
    <% } else { %>
        <div class="identityname">
           Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
        </div>
    <% } %>
        
    <% Html.BeginForm("Index", "Candidates", FormMethod.Get, new { }); %>
        <input id="what" name="what" type="text" />
        <input id="where" name="where" type="text" />
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" /><br />
        <%--<button id="gbqfba" aria-label="Advanced Search" name="btnK" class="gbqfba"><span id="gbqfsa">--%>
        <label id="lblEastSearch" style="color:InfoText;">Easy Search: </label>
            <%= Html.DropDownList("RolesForJobSeekers", ViewData["RolesForJobSeekers"] as SelectList,"Find Candidates" , new {onchange = "javascript:ddlRoleChanged();", id="ddlRoles",  @class = "dropdownStyle2"})%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(or)       
            <%:Html.ActionLink("Advanced Search", "CandidateSearch", "Search")%><%--</span></button>--%>
    <% Html.EndForm(); %>
    <% Html.RenderPartial("MatchingCandidates", Model);%> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideContent" runat="server">

    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStartedEmployer"); %> <br />
</asp:Content>

