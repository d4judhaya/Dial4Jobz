<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dial4Jobz - Employer DashBoard
    <%Session["OrganizationId"] = Request.QueryString["organizationId"]; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm("AdminDashBoard", "AdminHome", FormMethod.Get, new { }); %>
 <% var organizationId = Convert.ToInt32(Session["OrganizationId"]); %>

    <div>
        <p class="heading">
            Jobs<img src="../../Content/Images/Jobs.png" width="24" height="24" alt="Jobs"/>
        </p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
                <a href="<%:Url.Action("AddJob","AdminHome", new { organizationId = organizationId}) %>" class="dashboard-module" target="_blank" >
                    <img src="../../../../Content/Images/postjob.jpg" width="64" height="64" alt="Post a Job"/>
                    <span>Post a Job</span> </a>
                    
                    <a href="<%:Url.Action("AdminPostedJobs","AdminHome", new { organizationId=organizationId}) %>" class="dashboard-module" target="_blank">
                        <img src="../../../../Content/Images/postedjobs.png" width="64" height="64" alt="My Job Listing"/>
                        <span>My Jobs Listing</span> </a>

                         <a href="../../../../../Search/CandidateSearch" class="dashboard-module" target="_blank">
                           <img src="../../../../Content/Images/searchjobseekers.jpg" width="64" height="64" alt="Search Job Seekers"/>
                           <span>Search Job Seekers</span> </a>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <p class="heading">
            Billing<img src="../../Content/Images/Billing.png" width="24" height="24" alt="Billing"/></p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
                <a href="" class="dashboard-module" target="_blank">
                    <%--<img src="../../Content/Images/MyCredit.png" width="64" height="64" alt="My Credit">--%>
                    <img src="../../../../Content/Images/billingdetails.jpg" width="64" height="64" alt="My Credit">
                    <span>Billing details</span> </a><%--<a href="../../../../employer/employervas/index" class="dashboard-module" target="_blank">
                        <img src="../../Content/Images/buyhotresumes.jpg" width="64" height="64" alt="Buy Hot Resumes">
                        <span>Buy Hot Resumes</span> </a>--%><a href="../../../../../employer/employervas/index#HotResumes" class="dashboard-module" target="_blank" >
                        <img src="../../../../Content/Images/buyhotresumes.jpg" width="64" height="64" alt="Buy Hot Resumes">
                        <span>Buy Hot Resumes</span></a><a href="../../../../../employer/employervas/index#smsPurchase" class="dashboard-module" target="_blank">
                            <img src="../../../../Content/Images/buysms1.jpg" width="64" height="64" alt="Buy Sms" >
                            <span>Buy SMS Credit</span> </a><a href="<%:Url.Action("EmployerSubscription_Billing","AdminHome",new { organizationId=organizationId}) %>" class="dashboard-module" target="_blank">
                                <img src="../../../../Content/Images/subscribe.jpg" width="64" height="64" alt="My Subscriptions">
                                <span>Employer Subscriptions</span> </a>
                               
                                <a href="../../../Admin/AdminHome/AdminPlans" class="dashboard-module" target="_blank">
                                    <img src="../../../../Content/Images/Editprofile.jpg" width="64" height="64" alt="Admin Plans">
                                    <span>Admin Plans</span> </a>

                                    <a href="../../../Admin/AdminHome/SpecialPlans" class="dashboard-module" target="_blank">
                                        <img src="../../../../Content/Images/special_plans.png" width="64" height="64" alt="Dial4Jobz_Special_plans" />
                                    <span>Special Plans</span> </a>
                                    
                                    <a href="" class="dashboard-module" target="_blank">
                                        <img src="../../../../Content/Images/advertiseemails.jpg" width="64" height="64" alt="Advt Email">
                                        <span>Advertise in emails</span> </a><a href="<%:Url.Action("Index","EmployerVas") %>" class="dashboard-module" target="_blank">
                                            <img src="../../../../Content/Images/referencecheck.jpg" width="64" height="64" alt="Refer Check">
                                            <span>Background checks for candidates</span> </a>

                                            <a href="<%:Url.Action("AdminAlertSentDetails","AdminHome", new { organizationId= organizationId}) %>" class="dashboard-module" target="_blank">
                                            <img src="./../../../Content/Images/resume_alerts.png" width="64" height="64" alt="Refer Check"/>
                                            <span>Alert Sent Details</span> </a>

                                              <a href="../../../../employer/employervas/index#ResumeAlert" class="dashboard-module" target="_blank">
                                              <img src="../../Content/Images/resume_alert.jpg" width="64" height="64" alt="Refer Check"/>
                                              <span>Resume Alert</span> </a>

                                            <a href="<%:Url.Action("AdminViewedCandidatesList","AdminHome", new { organizationId= organizationId}) %>" class="dashboard-module" target="_blank">
                                            <img src="../../../../Content/Images/opened_list1.jpg" width="64" height="64" alt="Refer Check">
                                            <span>Viewed Candidates List for HORS</span> </a>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            function getTable() {
                
                var tableObj = document.getElementById("backgroundCheck");
                return tableObj;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
   
</asp:Content>
