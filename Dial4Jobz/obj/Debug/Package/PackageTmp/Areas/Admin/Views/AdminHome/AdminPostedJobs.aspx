<%@ Import Namespace="Dial4Jobz.Models" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AdminPostedJobs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.BeginForm("ActivateRATVacancy", "AdminHome", FormMethod.Post, new { @orgnId = ViewData["OrgnId"] });{%>   

  <%Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>

 <%var orgnId = ViewData["OrgnId"]; %>
 <%var organization = _userRepository.GetOrganizationById(Convert.ToInt32(orgnId)); %>
 
  <%: Html.Hidden("orgnId", ViewData["OrgnId"])%> 
  <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>

  <% var orderId = _vasRepository.GetPlanActivatedResultRAT(Convert.ToInt32(organization.Id));%>
  <% var vacancies = _vasRepository.GetVacancies(Convert.ToInt32(organization.Id)); %>
  <% var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(Convert.ToInt32(organization.Id),orderId); %>
  <% var planActivated = _vasRepository.GetRatSubscribed(Convert.ToInt32(organization.Id)); %>
  <% var planActivatedDetails = _vasRepository.PlanActivatedDetails(Convert.ToInt32(organization.Id)); %>
  <% var balanceVacancy = vacancies - postedJobs.Count(); %>

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
        <td  rowspan="3" colspan="3">
            <%--not activated for plan without activate the vacancy--%>
            <div class="editor-field" style="font-family:Bookman Old Style; font-size:12px; color: Blue;">
            <% if (orderId != 0)
               { %>
            <%if (planActivated == false)
              { %>
                <h3>Your Subscription for <%:planName%>. It's Pending activation. On receipt of payment, your plan will be activated within 1 working day.</h3>
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
                  <%if(planActivatedDetails!=null) { %>
                    <h3>You have activated all <%:planActivatedDetails.Vacancies %> Vacancies for resume alert under Plan <%:planName%>.To activate more Vacancies for Resume alert upgrade to Rat125 or RAT500</h3>
                  <% } %>
            <% } else if (planActivated == false && postedJobs.Count() != vacancies) {%>
                <h3>Your Subscription for <%:planName%> has expired. For more Details go to Subscription Details in <%:Html.ActionLink("DashBoard","DashBoard","Employer") %></h3>
            <%} %>
            </div>
            <%} else { %>
            <% } %>
            
        </td>
    </tr>
    </table>

   <h2>
        You have posted Following jobs</h2>
    
    <table width="150%">
        <tr>
            <td width="26%"><br />
                <h3>Positions</h3>
                <br />
                 <% foreach (Dial4Jobz.Models.Job job in Model.Jobs) { %>
                 <%var activatedvacancy = _vasRepository.GetPostedJobAlert(Model.Id,job.Id); %>
                 <%int? remainingCount = _vasRepository.GetRemaingCountVacancy(job.Id,organization.Id); %>
                   
                  <%  if (planActivated ==true && activatedvacancy != null)
                       { %>
                        <%: Html.CheckBox("Job" + job.Id.ToString(), new { id = job.Id, @class = "Job" })%>
                        
                        <%: Html.ActionLink(job.Position, "EditJob", "AdminHome", new { id = job.Id }, new { target = "_blank" })%><img src="../../../../Content/Images/star for paidemployers.jpg" width="17px" height="15" />(<%:remainingCount %>)
                   <%} else if (job.Position != "" && planActivated ==true)
                       { %>
                        <%: Html.CheckBox("Job" + job.Id.ToString(), new { id = job.Id, @class = "Job" })%> <%: Html.ActionLink(job.Position, "EditJob", "AdminHome", new { id = job.Id }, new { target = "_blank" })%>
                   <%} else { %>
                        <%: Html.ActionLink(job.Position, "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>
                   <%} %>
                <br />
               <% } %>
            </td>


            <td width="18%">
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
                    <%  if (planActivated==true && activatedvacancy != null) { %>

                    <% } else { %>
                     <%: Html.ActionLink("Edit", "EditJob", "AdminHome", new { id = job.Id }, new { target = "_blank" })%>
                   <% } %>
                <br />
                <% } %>
            </td>

            <td width="5%">
                <h3>Delete</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                        
                    <%var activatedvacancy = _vasRepository.GetPostedJobAlert(Model.Id,job.Id); %>
                    <% if (planActivated ==true && activatedvacancy != null)
                       { %>
                            <%: Html.ActionLink("Delete", "Delete", "AdminHome", new { id = job.Id }, new { @class = "deletejob" })%>
                    <% } else { %>
                      <%: Html.ActionLink("Delete", "Delete", "AdminHome", new { id = job.Id }, new { @class = "delete" })%>
                    <%} %>
                                       
                    <br />
               
                <% } %>
            </td>


            <% if (planActivated == true && postedJobs.Count() != vacancies)
                {%>
                <td width="18%">
                    <h3>Update RAT</h3><br />
                    <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                       { %>
                        <%: Html.ActionLink("Update RAT Vacancy", "UpdateRATVacancy", "AdminHome", new { jobId = job.Id, organizationId = job.OrganizationId }, new { target = "_blank" })%>
                    <%} %>
             <%} %>
            </td>

            <td width="43%">
                <h3>Get Candidates</h3>
                <br />
                <% foreach (Dial4Jobz.Models.Job job in Model.Jobs)
                   { %>
                       <%: Html.ActionLink("Get Matching Candidates", "CandidateMatch", "CandidateMatches", new { id = job.Id }, new { target = "_blank" })%>
                <br />
                <% } %>
            </td>

        </tr>

        
       <tr>
        <% if (planActivated ==true && postedJobs.Count() != vacancies)
           {%>
            <td colspan="5">
                      
                      <%--<input id="ActivateRATVacancy" type="submit" value="Activate Resume Alert" class="btn-activate" />--%>
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
   <script type="text/javascript">
       $(document).ready(function () {
           $('.deletejob').click(function () {
               alert("You have activated resume alert or this vacancy. Do you still want to delete?")
           });
       });
   
   </script>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
      <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
