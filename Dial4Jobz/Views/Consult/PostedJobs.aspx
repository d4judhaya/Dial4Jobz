<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Consultante>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Posted Jobs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <% Html.BeginForm("ActivateRATVacancy", "Consult", FormMethod.Post, new { @id = "ActivateRATVacancy" });
    { %>

         <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
         <%Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>
         <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
         <% int? balanceVacancy = 0; %>
         <%if (LoggedInConsultant != null)
           { %>
         <%}
           else
           { %>
           <%var consultantId = ViewData["consultantId"]; %>
           <% LoggedInConsultant = _userRepository.GetConsultantsById(Convert.ToInt32(consultantId)); %>
         <%} %>

      <% var postedJobs = _vasRepository.GetJobsByConsultantIdAlert(LoggedInConsultant.Id); %>
     <%-- <% bool planActivated = _vasRepository.GetRatSubscribed(LoggedInConsultant.Id); %>--%>
      <% var planActivatedDetails = _vasRepository.RATPlanActivatedDetails(LoggedInConsultant.Id); %>
      <%if(planActivatedDetails!=null) { %>
        <% balanceVacancy = planActivatedDetails.Vacancies - postedJobs.Count(); %>
      <%} %>
  

<% var planName = string.Empty;%>
  <% if(planActivatedDetails!=null) { %>
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
                <%if (planActivatedDetails!=null)
                  { %>
                  <% if (planActivatedDetails.OrderMaster.PaymentStatus != false)
                    { %>
                    <h3>Your Subscription for RAT, it's Pending activation. On receipt of payment, your plan will be activated within 1 working day.</h3>
                <%}
              //activated for plan and not activate the vacancy
                     else if (planActivatedDetails != null && postedJobs.Count() == 0)
                      { %>
                           <h3>Your <%: planName%> is active. You have not activated any vancancy till now.You can activate <%: planActivatedDetails.Vacancies.ToString() %> vacancies now.</h3>
                     <%}
                    else if (planActivatedDetails != null && postedJobs.Count() != planActivatedDetails.Vacancies)
                      { %> 
                        <h3>Your Plan is active. You have assigned <%: postedJobs.Count()%> vacancy for Resume Alert till now. <%: balanceVacancy.Value%> more vacancies you can assign for Resume Alert.</h3>
                    <% }
                    else if (planActivatedDetails != null && postedJobs.Count() == planActivatedDetails.Vacancies)
                      {%>
                        <h3>You have activated all <%: planActivatedDetails.Vacancies %> Vacancies for Resume Alert under Plan <%: planName%>.To activate more Vacancies for Resume alert upgrade to Rat125 or RAT500</h3>
                    <% }
                    else if (planActivatedDetails==null && postedJobs.Count() != planActivatedDetails.Vacancies)
                      {%>
                        <h3>Your Subscription for <%: planName%> has expired. For more Details go to Subscription Details in <%:Html.ActionLink("DashBoard","DashBoard","Employer") %></h3>
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
                 <%int? remainingCount = _vasRepository.GetRemaingCountVacancy(job.Id, LoggedInConsultant.Id); %>
                   
                   <%  if (planActivatedDetails != null && activatedvacancy != null)
                       { %>
                        <%: Html.CheckBox("Job" + job.Id.ToString(), new { id = job.Id, @class = "Job" })%>
                        <a href="#"><%:job.Position %></a><img src="../../../../Content/Images/star for paidemployers.jpg" alt="paid employers" width="17px" height="15" />(<%:remainingCount%>)
                   <%}
                       else if (job.Position != "" && planActivatedDetails != null)
                       { %>
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
                    <%  if (planActivatedDetails != null && activatedvacancy != null)
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
                    <%  if (planActivatedDetails != null && activatedvacancy != null)
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
                <% Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault(); %>
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                    <%if (user != null)
                     { %>
                        <%-- <%: Html.ActionLink("Get Matching Candidates", "CandidateMatch", "CandidateMatches", new { id = job.Id }, new { target = "_blank" })%>--%>
                         <a href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Admin/CandidateMatches/CandidateMatch?id=<%:job.Id %>">Get Matching Candidates</a>
                    <%} else { %>
                         <%: Html.ActionLink("Get Matching Candidates", "CandidateMatch", "CandidateMatchesJob", new { id = job.Id }, new { target = "_blank" })%>
                    <%} %>
                <br />
                <% } %>
            </td>
                      
        </tr>
             
        

        <tr>
        <%if(planActivatedDetails!=null) { %>
        <% if (planActivatedDetails != null && postedJobs.Count() != planActivatedDetails.Vacancies)
           {%>
            <td colspan="5">
 
                      <input id="ActivateRATVacancy" type="submit" value="Activate Resume Alert" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
                <br />
               
            </td>
            <%}
           else if (postedJobs.Count() == planActivatedDetails.Vacancies)
           { %>
                
            <% }
           else
           { %>
            <%} %>
            <%} %>
        </tr>
    </table>
     <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Consultant.js")%>" type="text/javascript"></script>

   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.DeleteConfirmation.js")%>" type="text/javascript"></script>
   <link href="../../Content/DeleteConfirmation.css" rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavConsultant"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
  <div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
</asp:Content>
