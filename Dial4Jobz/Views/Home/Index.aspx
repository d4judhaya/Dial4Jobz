<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dial4Jobz
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.GoogleAnalyticsSite.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.ticker').easyTicker();
        });
   </script>
    <%-- <script src="<%=Url.Content("~/Scripts/jquery.ticker.js")%>" type="text/javascript"></script>
    <link href="../../Content/ticker-style.css" rel="Stylesheet" type="text/css" />--%>
   <%-- <script type="text/javascript">
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

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
 <% bool isLoggedIn = loggedInCandidate != null; %>
  <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
  <% bool isConsultLoggedIn = LoggedInConsultant != null; %>
 <% Html.RenderPartial("Main/CandidatesVas"); %>

  <div class="ticker">
		<ul>
		<li>You May Get Matching Job Alerts For Few Hundred Rupees Onward...</li><%--style="margin: 0px; display: list-item; font-size:13px; font-weight:bold;"--%>
		<li>Stand Out in the Crowd Be Reachable To More Employers /Recruiters </li>
		<li>You May Get Matching Job Alerts For Few Hundred Rupees Onward...</li>
		<li>Right Interviews at The Right Time!!! Our Specialist Can Fix Interviews For You...</li>
		</ul>
	</div>

 <%if (isLoggedIn == false && isConsultLoggedIn == false)
    { %>
        <h3>If you are Consultant <%: Html.ActionLink("Click Here", "Index", "Consult", null, new { target = "_blank" })%></h3>
  <%} %>

 <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
   <% if (isLoggedIn != false)
        {%>
         <% Session["CandidateMobileVerification"] = true; %>
         <% Session["CandidateMailVerification"] = true; %>
         <%var CandidateVerification = _repository.GetCandidate(loggedInCandidate.Id); %>
            <% if (!string.IsNullOrEmpty(loggedInCandidate.ContactNumber))
               {%>                                
                        <%if (CandidateVerification.IsPhoneVerified == true)
                          {%>
                            <% Session["CandidateMobileVerification"] = true; %>
                        <%}
                          else
                          {%>
                            <%  Session["CandidateMobileVerification"] = false;%>
                        <%} %>
            <%} %>

            <% if (!string.IsNullOrEmpty(loggedInCandidate.Email))
               {%>                                
                        <%if (CandidateVerification.IsMailVerified == true)
                          {%>
                            <% Session["CandidateMailVerification"] = true; %>
                        <%}
                          else
                          {%>
                            <%  Session["CandidateMailVerification"] = false;%>
                        <%} %>
            <%} %>

    <%} %>



    <%  if (isLoggedIn != false && (Boolean)Session["CandidateMobileVerification"] == false)
        {%>
            <script type="text/jscript">
                $(function confirmPopupBox() {

                    $('#popup_Verification').fadeIn("slow");
                    $("#Send").css({
                        "opacity": "0.3"
                    });
                });    
            </script>
            <% Session["CandidateMobileVerification"] = true; %>
        <% }
        else
        { %>
        <%} %>

    <%  if (isLoggedIn != false && (Boolean)Session["CandidateMailVerification"] == false && (Boolean)Session["CandidateMobileVerification"] == true)
        {%>
            <script type="text/jscript">
                $(function confirmPopupBox() {

                    $('#popupEmail_Verification').fadeIn("slow");
                    $("#Send").css({
                        "opacity": "0.3"
                    });
                });    
            </script>
            <% Session["CandidateMailVerification"] = true; %>
            <%-- End vignesh--%>
    <% }
        else
        { %>
        <%} %>
    <div id="popup_Verification" class="confirmpopup_box" style="display: none">
        <table style="border-color: White; width: 100%;">
            <tr>
                <td align="center" style="border-color: rgb(39, 199, 199); color: Blue; 

font-family: Calibri;
                    font-size: 26px; font-weight: bold; width: 100%;">
                    Your Mobile Number is not verified.
                </td>
            </tr>
            <tr>
                <td align="center" style="border-color: White; width: 100%;">
                    <a href="<%: Url.Action("VerifyCandidate", "Candidates") %>">
                        <img src="../../Content/Images/VerifyNow.png" class="btn" 

width="100px" alt="Verify"
                            height="25px" /></a> <a>
                                <img src="../../Content/Images/VerifyCancel.png" class="btn" 

width="100px" alt="cancel"
                                    height="25px" onclick="javascript:

$('#popup_Verification').fadeOut('slow');$('#Send').css({'opacity': '1'});" /></a>
                </td>
            </tr>
        </table>
    </div>
    <div id="popupEmail_Verification" class="confirmpopup_box" style="display: 

none">
        <table style="border-color: White; width: 100%;">
            <tr>
                <td align="center" style="border-color: rgb(39, 199, 199); color: Blue; 

font-family: Calibri;
                    font-size: 26px; font-weight: bold; width: 100%;">
                    Your Email-Id is not verified.
                </td>
            </tr>
            <tr>
                <td align="center" style="border-color: White; width: 100%;">
                    <a href="<%: Url.Action("VerifyEmail", "Candidates") %>">
                        <img src="../../Content/Images/VerifyNow.png" class="btn" 

width="100px" alt="Verify"
                            height="25px" /></a> <a>
                                <img src="../../Content/Images/VerifyCancel.png" class="btn" 

width="100px" alt="cancel"
                                    height="25px" onclick="javascript:

$('#popupEmail_Verification').fadeOut('slow');$('#Send').css({'opacity': '1'});" 

/></a>
                </td>
            </tr>
        </table>
    </div>

    <%--END VIGNESH--%>

 
 <%var login = Session["LoginAs"]; %>
    
   <%-- <ul id="js-news" class="js-hidden">
     
        <li class="news-item"><a href="#">You May Get Matching Job Alerts For Few Hundred Rupees Onward...</a></li>
        <li class="news-item"><a href="#">Stand Out in the Crowd Be Reachable To More Employers /Recruiters </a></li>
        <li class="news-item"><a href="#">Right Interviews at The Right Time!!! Our Specialist Can Fix Interviews For You...</a></li>
    </ul>--%>

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

    <% Html.BeginForm("Index", "Jobs",  FormMethod.Get, new { }); %>

        <input id="what" name="what" type="text" />
        <input id="where" name="where" type="text" />
        <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" />
       <%:Html.ActionLink("Advanced Search", "JobSearch", "Search")%>
    <% Html.EndForm(); %>
   
    <p>
        <% Html.RenderPartial("MatchingJobs", Model); %> 
    </p>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStarted"); %> 
    <% Html.RenderPartial("Side/Video"); %> 
</asp:Content>

