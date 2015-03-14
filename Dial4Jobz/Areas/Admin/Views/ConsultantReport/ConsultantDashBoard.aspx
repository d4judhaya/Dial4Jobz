<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Consultant DashBoard
    <%Session["ConsultantId"] = Request.QueryString["consultantId"]; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <% Html.BeginForm("AdminDashBoard", "AdminHome", FormMethod.Get, new { }); %>
 <% var consultantId = Convert.ToInt32(Session["ConsultantId"]); %>

    <div>
        <p class="heading">
            Jobs<img src="../../Content/Images/Jobs.png" width="24" height="24" alt="Jobs"/>
        </p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
            <a class="dashboard-module" href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Consult/Add?consultantId=<%: consultantId %>" class="dashboard-module" target="_blank" >
                    <img src="../../../../Content/Images/postjob.jpg" width="64" height="64" alt="Post a Job"/>
                    <span>Post a Job</span> </a>

               <%-- <a href="<%:Url.Action("Add","Consult", new { consultantId = consultantId }) %>" class="dashboard-module" target="_blank" >
                    <img src="../../../../Content/Images/postjob.jpg" width="64" height="64" alt="Post a Job"/>
                    <span>Post a Job</span> </a>--%>
                                      
                    
                    <a class="dashboard-module" href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Consult/PostedJobs?consultantId=<%: consultantId %>">
                    <img src="../../../../Content/Images/postedjobs.png" width="64" height="64" alt="My Job Listing"/>
                    <span>Posted Jobs</span> </a>

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
             
                    <img src="../../../../Content/Images/billingdetails.jpg" width="64" height="64" alt="My Credit">
                    <span>Billing details</span> </a><a href="../../../../../employer/employervas/index#HotResumes" class="dashboard-module" target="_blank" >
                        <img src="../../../../Content/Images/buyhotresumes.jpg" width="64" height="64" alt="Buy Hot Resumes">
                        <span>Buy Hot Resumes</span></a><a href="../../../../../employer/employervas/index#smsPurchase" class="dashboard-module" target="_blank">
                            <img src="../../../../Content/Images/buysms1.jpg" width="64" height="64" alt="Buy Sms" >
                            <span>Buy SMS Credit</span> </a><a href="<%:Url.Action("ConsultantSubscription_Billing","ConsultantReport",new { consultantId=consultantId}) %>" class="dashboard-module" target="_blank">
                                <img src="../../../../Content/Images/subscribe.jpg" width="64" height="64" alt="My Subscriptions">
                                <span>Consultant Subscriptions</span> </a>
                               
                                <a href="../../../Admin/AdminHome/AdminPlans" class="dashboard-module" target="_blank">
                                    <img src="../../../../Content/Images/Editprofile.jpg" width="64" height="64" alt="Admin Plans">
                                    <span>Admin Plans</span> </a>

                                    <a href="../../../Consult/Plans" class="dashboard-module" target="_blank">
                                        <img src="../../../../Content/Images/special_plans.png" width="64" height="64" alt="Dial4Jobz_Special_plans" />
                                    <span>Consultant Plans</span> </a>
                                    
                                    <a href="" class="dashboard-module" target="_blank">
                                        <img src="../../../../Content/Images/advertiseemails.jpg" width="64" height="64" alt="Advt Email">
                                        <span>Advertise in emails</span> </a><a href="<%:Url.Action("Index","EmployerVas") %>" class="dashboard-module" target="_blank">
                                            <img src="../../../../Content/Images/referencecheck.jpg" width="64" height="64" alt="Refer Check">
                                            <span>Background checks for candidates</span> </a>

                                            <a href="<%:Url.Action("AdminAlertSentDetails","AdminHome", new {organizationId= 0, consultantId= consultantId}) %>" class="dashboard-module" target="_blank">
                                            <img src="./../../../Content/Images/resume_alerts.png" width="64" height="64" alt="Refer Check"/>
                                            <span>Alert Sent Details</span> </a>

                                              <a href="../../../../employer/employervas/index#ResumeAlert" class="dashboard-module" target="_blank">
                                              <img src="../../Content/Images/resume_alert.jpg" width="64" height="64" alt="Refer Check"/>
                                              <span>Resume Alert</span> </a>

                                            <a href="<%:Url.Action("AdminViewedCandidatesList","AdminHome", new {organizationId= 0, consultantId= consultantId}) %>" class="dashboard-module" target="_blank">
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<%Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
