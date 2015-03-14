<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Posted Jobs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <% Html.BeginForm("ActivateRATVacancy", "Employer", FormMethod.Post, new { @id = "ActivateRATVacancy" });
    { %>

 <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>

  <%var orderId = _vasRepository.GetPlanActivatedResultRAT(loggedInOrganization.Id);%>
  <%var vacancies = _vasRepository.GetVacancies(loggedInOrganization.Id); %>
  <%var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(loggedInOrganization.Id, orderId); %>
  <%var planActivated = _vasRepository.GetRatSubscribed(loggedInOrganization.Id); %>
  <%var planActivatedDetails=_vasRepository.PlanActivatedDetails(loggedInOrganization.Id); %>
  <%var balanceVacancy = vacancies - postedJobs.Count(); %>

<% var planName = string.Empty;%>
  <%if(planActivatedDetails!=null) { %>
    <%if (planActivatedDetails.PlanName == "BasicRATHORS")
      { %>
       <% planName = "E-Basic"; %>
    <%} else { %>
        <% planName = planActivatedDetails.PlanName; %>
    <%} %>
  <%} %>        

    <table>
    <tr>
        <td rowspan="3" colspan="3">
            <%--not activated for plan without activate the vacancy--%>
            <div class="editor-field" style="font-family:Bookman Old Style; font-size:12px; color: Blue;">
            <% if (orderId != 0) { %>
            <%if (planActivated == false)
              { %>
                <h3>Your Subscription for RAT, it's Pending activation. On receipt of payment, your plan will be activated within 1 working day.</h3>
            <%}
              //activated for plan and not activate the vacancy
              else if (planActivated == true && postedJobs.Count() == 0)
              { %>
                   <h3>Your <%: planName%> is active. You have not activated any vancancy till now.You can activate <%:vacancies%> vacancies now.</h3>
             <%}
              else if (planActivated == true && postedJobs.Count() != vacancies)
              { %> 
                <h3>Your Plan is active. You have assigned <%:postedJobs.Count()%> vacancy for Resume Alert till now. <%:balanceVacancy.Value%> more vacancies you can assign for Resume Alert.</h3>
            <% }
              else if (planActivated == true && postedJobs.Count() == vacancies)
              {%>
                <h3>You have activated all <%:planActivatedDetails.Vacancies %> Vacancies for Resume Alert under Plan <%:planName%>.To activate more Vacancies for Resume alert upgrade to Rat125 or RAT500</h3>
            <% }
              else if (planActivated == false && postedJobs.Count() != vacancies)
              {%>
                <h3>Your Subscription for <%:planName%> has expired. For more Details go to Subscription Details in <%:Html.ActionLink("DashBoard","DashBoard","Employer") %></h3>
            <%} %>
            </div>
            <%} else { %>
            <% } %>
            
        </td>
    </tr>
    </table>
    
    <% if (Request.IsAuthenticated == true)
       { %>
    <div class="identityname">
        Welcome!!! <b>
            <%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy.....
    </div>
    <% }
       else
       { %>
    <div class="identityname">
        Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
    </div>
    <% } %>
    <h2>
        You have posted Following jobs</h2>
    <table>
        <tr>
            <td width="25%">
                <h3>Positions</h3>
                <br />
                
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs) { %>
                 <%var activatedvacancy = _vasRepository.GetPostedJobAlert(Model.Id,job.Id); %>
                 <%int? remainingCount = _vasRepository.GetRemaingCountVacancy(job.Id, loggedInOrganization.Id); %>
                   
                   <%  if (planActivated ==true && activatedvacancy != null)
                       { %>
                        <%: Html.CheckBox("Job" + job.Id.ToString(), new { id = job.Id, @class = "Job" })%>
                        <a href="#"><%:job.Position %></a><img src="../../../../Content/Images/star for paidemployers.jpg" alt="paid employers" width="17px" height="15" />(<%:remainingCount%>)
                   <%} else if (job.Position != "" && planActivated ==true) { %>
                        <%: Html.CheckBox("Job" + job.Id.ToString(), new { id = job.Id, @class = "Job" })%> <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                   <% } else { %>
                         <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                   <%} %>
                <br />
               <% } %>
            </td>
           <td width="24%">
                <h3>Posted Date</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        <%:job.CreatedDate %>
                <br />
                <% } %>
            </td>
             <td width="9%">
                <h3>Edit</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                    <%var activatedvacancy = _vasRepository.GetPostedJobAlert(Model.Id,job.Id); %>
                    <%  if (planActivated ==true && activatedvacancy != null)
                        { %>

                    <% } else { %>
                     <%: Html.ActionLink("Edit", "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                   <% } %>
                <br />
                <% } %>
            </td>
           
               <td width="16%">
                <h3>
                    Delete Job</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        <%var activatedvacancy = _vasRepository.GetPostedJobAlert(Model.Id,job.Id); %>
                    <%  if (planActivated ==true && activatedvacancy != null)
                        { %>
                            <%: Html.ActionLink("Delete", "Delete", "Jobs", new { id = job.Id }, new { @class = "delete" })%>
                    <% } else { %>
                            <%: Html.ActionLink("Delete", "Delete", "Jobs", new { id = job.Id }, new { @class = "delete" })%>
                    <%} %>
                                       
                    <br />
                   
                <% } %>

            </td>

          <td width="42%">
                <h3>Get Candidates</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                       <%: Html.ActionLink("Get Matching Candidates", "CandidateMatch", "CandidateMatchesJob", new { id = job.Id }, new { target = "_blank" })%>
                <br />
                <% } %>
            </td>
                      
        </tr>
             
        

        <tr>
        <% if (planActivated ==true && postedJobs.Count() != vacancies)
           {%>
            <td colspan="5">
 
                      <input id="ActivateRATVacancy" type="submit" value="Activate Resume Alert" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
                <br />
               
            </td>
            <%} else if(postedJobs.Count()==vacancies) { %>
                
            <% } %>
        </tr>
    </table>
     <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
  
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.DeleteConfirmation.js")%>" type="text/javascript"></script>
   <link href="../../Content/DeleteConfirmation.css" rel="Stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
  <div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
</asp:Content>
