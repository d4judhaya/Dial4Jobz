<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="main">
<% Dial4Jobz.Models.Candidate LoggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
 <% bool isLoggedIn = LoggedInCandidate != null; %>
 <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <%Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>

 <%var ActivatedList= 0; %>

    <%if (isLoggedIn == true) { %>
        <%ActivatedList = _vasRepository.GetPlanActivatedResultRAT(LoggedInCandidate.Id); %>

 <% } %>

 <table>
        <tr>
            <td width="20%" rowspan="2" colspan="3">
                <% if (isLoggedIn == true)
                  { %>
                     <a  href="<%: Url.Action("Edit", "Candidates")%>"><img src="../../../Content/Images/PostResume1.jpg" alt="Post Resume"/></a>
                    <h3>Be 1<sup>st</sup> to apply for the Vacancy Subscribe <%:Html.ActionLink("Job Alert", "Index", "CandidatesVas")%> </h3>
                    
                <% } else { %>
                    <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz"><img src="../../../Content/Images/PostResume1.jpg"/></a>
                    <h3>Be 1<sup>st</sup> to apply for the Vacancy Subscribe <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Job Alert</a></h3>
                <% } %>
          
                <div class="editor-field" style="font-family: Bookman Old Style; font-size: 12px; color: Blue;">
                    <% if (isLoggedIn == true) { %>
                    <% var status = _vasRepository.GetRaJSubscribed(LoggedInCandidate.Id); %>
                    <% var subscribedplan = _vasRepository.GetOrderDetailsForRAJ(LoggedInCandidate.Id); %>
                    <% var planActivated = _vasRepository.PlanSubscribed(LoggedInCandidate.Id); %>
                    <% var planActivatedDetailsDPR = _vasRepository.GetOrderDetailsForDPR(LoggedInCandidate.Id); %>
                    <% var DPRSubscribed = _vasRepository.PlanSubscribedForDPR(LoggedInCandidate.Id); %>
                    <% var planActivatedDetails = _vasRepository.PlanActivatedDetailsForCandidate(LoggedInCandidate.Id); %>

                    <% if (DPRSubscribed == true)
                      { %>
                        <h3>You have activated for <%: planActivatedDetailsDPR.PlanName%>.</h3>
                    <% }
                      else if (subscribedplan !=null)
                      { %>
                        <h3>You have Subscribed for <%: subscribedplan.PlanName %>. You can activate the plan here <a href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Candidates/CandidatesVas/Payment?orderId=<%: Dial4Jobz.Models.Constants.EncryptString(subscribedplan.OrderId.ToString()) %>" >Pay Now</a> </h3>
                    <% } %>

                    <%if (planActivatedDetails != null)
                      { %>
                        <h3>Your <%: planActivatedDetails.PlanName %> is active. You  could get <%: planActivatedDetails.RemainingCount %> more remaining counts.</h3>
                    <%} %>
                  
                    <%} %>
                   
                </div>
            </td>

            <td width="35%" rowspan="2" colspan="3">
             <a href="../../../../../employer/matchcandidates"  target="_blank"><img src="../../../Content/Images/emp.zone1-Edit.jpg" width="250px" alt="Employer Zone" /></a><br />
                <h3>If you are an Employer <%: Html.ActionLink("Click Here", "MatchCandidates", "Employer", null, new {title = "Click here if you are an employer", target = "_blank" })%></h3>
            </td>
        </tr>
    </table>

</div>