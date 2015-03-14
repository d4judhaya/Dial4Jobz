<%@ Page Title="" Language="C#" Debug="true" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Candidate DashBoard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm("DashBoard", "Candidates", FormMethod.Get, new { }); %>

   <% Dial4Jobz.Models.Candidate LoggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
    <div>
        <p class="heading">
            Candidates<img src="../../Content/Images/Jobs.png" width="24" height="24" alt="Jobs"/></p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
                <a href="../../../../Jobs/index" class="dashboard-module" target="_blank"> 
                  <img src="../../Content/Images/cand_home.jpg" width="94" height="84" alt="Buy Sms"/>
                  <span>Candidate Home(Vacancy Lists)</span> </a>

                <a href="<%:Url.Action("Edit", "Candidates") %>" class="dashboard-module" target="_blank" >
                    <img src="../../Content/Images/updateProfile.jpg" width="94" height="84" alt="update profile"/>
                    <span>Update Profile</span> </a>

                         <a href="<%:Url.Action("JobSearch","Search") %>" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/findjobs.jpg" width="94" height="84" alt="Search Job Seekers"/>
                                <span>Advanced Search for Jobs</span> </a>

                                <a href="../../../../candidates/candidatesvas/index#smsPurchase" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/cand.sms.jpg" width="94" height="84" alt="Buy Sms"/>
                                <span>Buy SMS Credit</span> </a>
                                
                <div style="clear: both">
                </div>
            </div>
        </div>
        <p class="heading">
            Billing<img src="../../Content/Images/Billing.png" width="24" height="24" alt="Billing"/>
        </p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
              <a href="../../../../candidates/candidatesvas/index#JobAlert" class="dashboard-module" target="_blank">
                        <img src="../../Content/Images/jobalert.jpg" width="94" height="84" alt="Subscribe Job Alert"/>
                        <span>Subscribe Job Alert</span> </a><a href="../../../../candidates/candidatessubscriptionbilling" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/subscribe.jpg" width="94" height="84" alt="My Subscriptions"/>
                                <span>My Subscriptions</span> </a>

                                <a href="<%:Url.Action("DPRViewedList","Candidates", new{candidateId=LoggedInCandidate.Id}) %>" class="dashboard-module" target="_blank">
                                     <img src="../../Content/Images/viewed_details.jpg" width="94" height="84" alt="My Subscriptions"/>
                                <span>Viewed Employer's List</span> </a>
                                            
                                            <a href="../../../../Candidates/CandidatesVas/Index#backgroundCheck" class="dashboard-module" target="_blank">
                                            <img src="../../Content/Images/referencecheck.jpg" width="94" height="84" alt="Refer Check"/>
                                            <span>Background checks for Candidates</span> </a>
                                            <% if (new Dial4Jobz.Models.Repositories.VasRepository().RAJActivateStatus(LoggedInCandidate.Id) != null) { %>
                                                    <a href="<%:Url.Action("RAJAlertSentDetails","Candidates") %>" class="dashboard-module" target="_blank">
                                                        <img src="../../Content/Images/jobalerts.jpg" width="94" height="84" alt="Refer Check"/>
                                                    <span>Job Alert Details</span> </a>
                                            <% } %>
                                            
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("Nav"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStarted"); %>
   </div> 
</asp:Content>
