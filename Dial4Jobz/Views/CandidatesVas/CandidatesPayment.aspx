<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Payment Status
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Dial4Jobz.Models.Candidate LoggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
 <% bool isLoggedIn = LoggedInCandidate != null; %>
 <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <%Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
<table>
        <tr>
            <td rowspan="2" colspan="3">
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
       </table>

 <% if (Request.IsAuthenticated == true) { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b> , You are in Job seeker's Zone..We wish you to get your Dream job....
    </div>
    <% } else { %>
         <div class="identityname">
           Welcome!!! You are in Job seeker's Zone..We wish you to get your Dream job....
        </div>
    <% } %>

    <% if (ViewData["Success"] != null && ViewData["Success"].ToString() != "")
           { %>
             <h2><%=ViewData["Success"]%></h2><br />

             <%:Html.ActionLink("Go to your Page","Index","Candidates") %>
                  
        <% }
            if (ViewData["Failure"] != null && ViewData["Failure"].ToString() != "")
           { %>
             <h2><%=ViewData["Failure"]%></h2>        
        <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div><h5>Sales Enquiries</h5>
    <ul>
    <li>Mobile Number:+91-9381516777 </li>
    <li>Contact Number:044 - 44455566 </li>
    <li>E-Mail:smc@dial4Jobz.com </li>
    </ul>    
    </div>

    <div><h5>Customer Support</h5>
    <ul>
    <li><a title ="contact him">Manikandan</a></li>
    <li>Contact Number:044 - 44455566</li>
    <li>Email: smo@dial4jobz.com</li>
    </ul>    
    </div>    
</asp:Content>
