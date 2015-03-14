<%@ Page Title="" Language="C#" Debug="true" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Employer DashBoard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm("DashBoard", "Employer", FormMethod.Get, new { }); %>

  <%--<% Html.RenderPartial("Main/MainVas"); %>--%>

   <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <div>
        <p class="heading">
            Jobs<img src="../../Content/Images/Jobs.png" width="24" height="24" alt="Jobs"></p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
                <a href="<%:Url.Action("Add","Jobs") %>" class="dashboard-module" target="_blank" >
                    <img src="../../Content/Images/postjob.jpg" width="64" height="64" alt="Post a Job">
                    <span>Post a Job</span> </a>
                    
                    <a href="<%:Url.Action("PostedJobs","Employer") %>" class="dashboard-module" target="_blank">
                        <img src="../../Content/Images/postedjobs.png" width="64" height="64" alt="My Job Listing">
                        <span>My Jobs Listing</span> </a>

                         <a href="<%:Url.Action("CandidateSearch","Search") %>" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/searchjobseekers.jpg" width="64" 
                    height="64" alt="Search Job Seekers">
                                <span>Search Job Seekers</span> </a>

                              <a href="<%:Url.Action("JobsViewedList","Jobs", new {organizationId= LoggedInOrganization.Id}) %>" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/viewed_details.jpg" width="64"  height="64" alt="Search Job Seekers">
                                <span>Viewed Jobs by Candidates</span> </a>

                <div style="clear: both">
                </div>
            </div>
        </div>
        <p class="heading">
            Billing<img src="../../Content/Images/Billing.png" width="24" height="24" alt="Billing"></p>
        <div class="content">
            <!-- Dashboard icons -->
            <div class="grid_7">
               <%-- <a href="" class="dashboard-module" target="_blank">
                    <img src="../../Content/Images/billingdetails.jpg" width="64" height="64" alt="My Credit">
                    <span>Billing details</span> </a>--%><a href="../../../../employer/employervas/index#HotResumes" class="dashboard-module" target="_blank">
                        <img src="../../Content/Images/buyhotresumes.jpg" width="64" height="64" alt="Buy Hot Resumes">
                        <span>Buy Hot Resumes</span> </a><a href="../../../../employer/employervas/index#smsPurchase" class="dashboard-module" target="_blank">
                            <img src="../../Content/Images/buysms1.jpg" width="64" height="64" alt="Buy Sms">
                            <span>Buy SMS Credit</span> </a><a href="MySubscription_Billing" class="dashboard-module" target="_blank">
                                <img src="../../Content/Images/subscribe.jpg" width="64" height="64" alt="My Subscriptions">
                                <span>My Subscriptions</span> </a>
                               
                                <a href="Profile" class="dashboard-module" target="_blank">
                                    <img src="../../Content/Images/Editprofile.jpg" width="64" height="64" alt="Edit Profile">
                                    <span>Edit Profile</span> </a>
                                    
                                    <a href="" class="dashboard-module" target="_blank">
                                        <img src="../../Content/Images/advertiseemails.jpg" width="64" height="64" alt="Advt Email">
                                        <span>Advertise in emails</span> </a><%--<a href="<%:Url.Action("Index","EmployerVas") %>" class="dashboard-module" target="_blank">
                                            <img src="../../Content/Images/referencecheck.jpg" width="64" height="64" alt="Refer Check">
                                            <span>Background checks for candidates</span> </a>--%>
                                            
                                            <a href="<%:Url.Action("Index","Employer") %>" class="dashboard-module" target="_blank">
                                            <img src="../../Content/Images/referencecheck.jpg" width="64" height="64" alt="Refer Check"/>
                                            <span>Background checks for candidates</span> </a>

                                            <%if (new Dial4Jobz.Models.Repositories.VasRepository().GetPlanActivatedResultRAT(LoggedInOrganization.Id) != null)
                                              { %>
                                                 <a href="<%:Url.Action("AlertSentDetails","Employer") %>" class="dashboard-module" target="_blank">
                                                <img src="../../Content/Images/resume_alerts.png" width="64" height="64" alt="Refer Check"/>
                                                <span>Alert Sent Details</span> </a>
                                            <%} %>
                                                  
                                                 <a href="../../../../employer/employervas/index#ResumeAlert" class="dashboard-module" target="_blank">
                                                <img src="../../Content/Images/resume_alert.JPG" width="64" height="64" alt="Refer Check"/>
                                                <span>Resume Alert</span> </a>
                                         

                                            <% if (new Dial4Jobz.Models.Repositories.VasRepository().GetHORSSubscribed(LoggedInOrganization.Id) == true)
                                               {%>
                                                <a href="<%:Url.Action("ViewedCandidatesList","Employer") %>" class="dashboard-module" target="_blank">
                                                <img src="../../Content/Images/viewedlist.png" width="64" height="64" alt="Refer Check"/>
                                                <span>Viewed Candidates List for HORS</span> </a>
                                            <%} %>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
 
<%-- <%var message = ViewData["Message"];
 Response.Write("<script language=javascript>alert('" + message + "');</script>"); %>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
   
       <% var value = string.Empty;
          Session["VASAmount"] = "";
          if (Request.QueryString.Count > 0)
           {
               value = Request.QueryString[0];
               if (value == "Congrats")
               {
                   string orderid = Session["VASOrderId"].ToString();
                   string text = "Congratulations!!! Your plan is activated. Your order id is " + orderid + ". Please note it for future reference!!!";
                   Response.Write("<script language=javascript>alert('" + text + "');</script>");                    

                  // Response.Write("<script language=javascript>alert(' Now post jobs...');</script>");
               }
               else if (value == "activate")
               {

               }
               else
                   Response.Write("<script language=javascript>alert('" + value + "');</script>");
           }
           else
              Session["VASOrderId"] = "0"; %>
</asp:Content>
