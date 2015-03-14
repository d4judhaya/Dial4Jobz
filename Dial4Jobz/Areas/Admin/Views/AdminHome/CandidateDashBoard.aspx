<%@ Page Title="" Language="C#" Debug="true" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Candidate DashBoard
     <% Session["candidateId"] = Request.QueryString["CandidateId"]; %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm("CandidateDashBoard", "AdminHome", FormMethod.Get, new { }); %>
<% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
   
     <% var candidateId = Convert.ToInt32(Session["candidateId"]); %>
     <% var candidate = _repository.GetCandidate(candidateId); %>

     <%: Html.ActionLink("Send Account Details", "AccountDetails", "AdminHome", new { candidateId = candidateId }, new { title = "Account Details" })%> 

    <div>
        <p class="heading">
            Candidates<img src="../../Content/Images/Jobs.png" width="24" height="24" alt="Jobs"/></p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">

                <a href="../../../../../Jobs/index" class="dashboard-module" target="_blank"> 
                  <img src="../../../../Content/Images/cand_home.jpg" width="94" height="84" alt="Buy Sms"/>
                  <span>Candidate Home(Vacancy Lists)</span> </a>

                   <a href="<%:Url.Action("GetDetail","AdminHome", new { validateEmail = candidate.Email }) %>" class="dashboard-module" target="_blank" >
                    <img src="../../../../Content/Images/updateProfile.jpg" width="94" height="84" alt="update profile"/>
                    <span>Update Profile</span> </a>

                         <a href="../../../../../Search/JobSearch" class="dashboard-module" target="_blank">
                                    <img src="../../../../Content/Images/findjobs.jpg" width="94" height="84" alt="Search Job Seekers"/>
                                <span>Advanced Search for Jobs</span> </a>

                                <a href="../../../../../../candidates/candidatesvas/index#smsPurchase" class="dashboard-module" target="_blank">
                                    <img src="../../../../Content/Images/cand.sms.jpg" width="94" height="84" alt="Buy Sms"/>
                                <span>Buy SMS Credit</span> </a>

                                 <a href="../../../Admin/AdminHome/AdminPlans" class="dashboard-module" target="_blank">
                                    <img src="../../../../Content/Images/Editprofile.jpg" width="64" height="64" alt="Edit Profile"/>
                                    <span>Admin Plans</span> </a>

                                     <a href="../../../Admin/AdminHome/SpecialPlans" class="dashboard-module" target="_blank">
                                        <img src="../../../../Content/Images/special_plans.png" width="64" height="64" alt="Dial4Jobz_Special_plans" />
                                    <span>Special Plans</span> </a>
                                                                       
                                 <a id="login" class="dashboard-module" href="<%: Url.Action("UpdateSIDetails", "AdminHome","Admin") %>"> 
                                     <img src="/Areas/Admin/Content/Images/update_details.jpg" width="64" height="64" alt="Update Profile"/>
                                 <span>Update SI Details</span> </a>
                                
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
              <a href="../../../../../../candidates/candidatesvas/index#JobAlert" class="dashboard-module" target="_blank">
                        <img src="../../../../Content/Images/jobalert.jpg" width="94" height="84" alt="Subscribe Job Alert"/>
                        <span>Subscribe Candidate Plans</span> </a><a href="<%:Url.Action("SubscriptionBillingForCandidate","AdminHome",new { candidateId=candidateId}) %>" class="dashboard-module" target="_blank">
                                <img src="../../../../Content/Images/subscribe.jpg" width="94" height="84" alt="My Subscriptions"/>
                                <span>Candidate Subscriptions</span> </a>
                                            
                                            <a href="../../../../../../Candidates/CandidatesVas/Index#backgroundCheck" class="dashboard-module" target="_blank">
                                            <img src="../../../../Content/Images/referencecheck.jpg" width="94" height="84" alt="Refer Check"/>
                                            <span>Background checks for Candidates</span> </a>
                                          <%--  <% if (new Dial4Jobz.Models.Repositories.VasRepository().RAJActivateStatus(candidateId) != null)
                                               { %>--%>
                                                    <a href="<%:Url.Action("AdminRAJAlertSentDetails","AdminHome", new { candidateId= candidate.Id}) %>" class="dashboard-module" target="_blank">
                                                    <img src="../../../../../../Content/Images/jobalerts.jpg" width="94" height="84" alt="Refer Check"/>
                                                    <span>Job Alert Details</span> </a>
                                           <%-- <% } %>--%>

                                                     <a class="dashboard-module" href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Candidates/DPRViewedList?candidateId=<%:candidate.Id %>" target="_blank">
                                                            <img src="../../Content/Images/viewed_details.jpg" width="64"  height="64" alt="Search Job Seekers">
                                                    <span>Job Alert Details</span> </a>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#login").fancybox({
                'hideOnContentClick': false,
                'titleShow': false,
                'scrolling': 'no',
                'onComplete': function () {
                    $('#Amount').watermark("Enter the Amount");
                    $('#TeleConferenceCount').watermark("Enter the Interviews Count in Number");

                }
            });

        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStarted"); %>
   </div> 
</asp:Content>
