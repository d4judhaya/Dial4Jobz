<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Job Posted Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.BeginForm("ActivateRATVacancy", "Jobs", FormMethod.Post, new { @id = "Add" }); %>   

<% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% var paymentstatus = _vasRepository.GetPlanActivatedResultRAT(loggedInOrganization.Id);%>
<table>
    <tr>
    <td rowspan="3" colspan="3">
        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <a href="../../../../employer/employervas/index#HotResumes" target="_blank">Hot Resumes</a> </h3 >
        <%}
          else
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>
    </tr>
    </table>

 <% if (Request.IsAuthenticated == true)
    { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy
    </div>
    <% }
    else
    { %>
         <div class="identityname">
           Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy
        </div>
    <% } %>

   <br />
        
        <h3>Dear <%:Model.ContactPerson%>, Thanks for Posting the Job as under.</h3>

        <h3><p>Job posting is free and unlimited</p></h3> <%: Html.ActionLink("Post another job", "Add", "Jobs", null, new { @title = "Post a job", id = "podtJob" })%>|
        <%: Html.ActionLink("Get Matching Candidates for your vacancy", "CandidateMatch", "CandidateMatchesJob", new { id = Model.Id }, new { target = "_blank" })%>|
     
        <% if (paymentstatus != null)
         {%>
            <input id="ActivateRATVacancy" type="submit" value="Active this Vacancy" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
         <%} else  { %>

        <% } %>
     
          
         <h2>Posted Job as displayed in site</h2>

         <span class="location">
            <% foreach (Dial4Jobz.Models.JobLocation jl in Model.JobLocations)
               {  %>
                    (<%: Html.ActionLink(jl.Location.ToString(), "Location", "Jobs", new { id = jl.Location.Id }, new { @class = "location" })%>)
            <% } %>
        </span>   

        <p><strong>Your Job Id:</strong>
            <%: Model.Id%>
        </p>
       
        <p>
            <strong>Posted Date:</strong>
            <%: String.Format("{0:g}", Model.CreatedDate)%>
        </p>
        
        <p>
            <strong>Modifid On:</strong>
            <%: String.Format("{0:g}", Model.UpdatedDate)%>
        </p>
        
        <p>
        <strong>Job Function:</strong>
           <% if (Model.FunctionId.HasValue)
              { %>
             <%: Model.GetFunction(Model.FunctionId.Value).Name%>
               
           <% } %>
        </p>
             
        <p>
        <strong>Position: </strong>
             <%: Model.Position%><br />
        </p>

        <p>
       <% if (Model.Male == true)
          { %>
         <strong>Gender:</strong> <%:"Male"%>
         <% }
          else
          { %>
           <strong>Gender:</strong> <%:"Female"%>
         <% } %>
        </p>    
        
        <p>
            <strong> Job Location: </strong>
            <%foreach (Dial4Jobz.Models.JobLocation jl in Model.JobLocations)
              { %>
            <% if (jl.Location != null)
               { %>
                <% if (jl.Location.Country != null)
                   { %>
                    <%: jl.Location.Country.Name%><%:","%>
                <% } %>
                <% if (jl.Location.State != null)
                   { %>
                    <%: jl.Location.State.Name%><%:","%>
                <% } %>
                <% if (jl.Location.City != null)
                   { %>
                    <%: jl.Location.City.Name%><%:","%>
                <% } %>
                <% if (jl.Location.Region != null)
                   { %>
                    <%: jl.Location.Region.Name%><%:","%>
                <% } %>
            <% } %>
            <% } %>
        </p>
       
          
     <p>
            <strong>Salary Range:</strong>
            <% if ((!Model.Budget.HasValue || Model.Budget == 0) && (!Model.MaxBudget.HasValue || Model.MaxBudget == 0))
               { %>
               <%:"Not Mentioned"%>
            <% }
               else if (!Model.Budget.HasValue || Model.Budget == 0)
               { %>
                <%: Convert.ToInt32(Model.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> 
            <% }
               else if (!Model.MaxBudget.HasValue || Model.MaxBudget == 0)
               { %>
                <%: Convert.ToInt32(Model.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
            <% }
               else
               {  %>
                <%: Convert.ToInt32(Model.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> to 
                <%: Convert.ToInt32(Model.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> 
            <%} %>
        </p>

       
        <p>
            <strong>Experience:</strong>
            <% if ((!Model.MinExperience.HasValue || Model.MinExperience == 0) && (!Model.MaxExperience.HasValue || Model.MaxExperience == 0))
               { %>
                <%:Model.MinExperience%>
            <% }
               else if (!Model.MinExperience.HasValue || Model.MinExperience == 0)
               { %>
                Up to <%: Math.Ceiling(Model.MaxExperience.Value / 33782400.0)%> Years 
            <% }
               else if (!Model.MaxExperience.HasValue || Model.MaxExperience == 0)
               { %>
                <%: Math.Ceiling(Model.MinExperience.Value / 33782400.0)%>+ Years
            <% }
               else
               {  %>
                <%: Math.Ceiling(Model.MinExperience.Value / 33782400.0)%> to 
                <%: Math.Ceiling(Model.MaxExperience.Value / 33782400.0)%> Years
            <% } %>
        </p>

          <p>
            <strong>Qualification:</strong>
           
            <% foreach (Dial4Jobz.Models.JobRequiredQualification deg in Model.JobRequiredQualifications)
               { %>
                <%: deg.Degree.Name%>
            <% } %>
        </p>

         <% if (Model.FunctionId.HasValue)
            { %>
            <p>
                <strong>Job Function:</strong>
                <%: Model.GetFunction(Model.FunctionId.Value).Name%>
            </p>
        <% }
            else
            {%>        
            <%:Model.FunctionId == null ? "Any" : Model.GetFunction(Model.FunctionId.Value).Name%><% } %>
            
        <p>
            <strong>Job Role:</strong>
            <% foreach (Dial4Jobz.Models.JobRole jr in Model.JobRoles)
               { %>
                <%: jr.Role.Name%>
            <% } %>
        </p>

        <p>
            <strong>Preferred Industry:</strong>
            <%if (Model.JobPreferredIndustries.Count() > 0)
              { %>
           
            <% foreach (Dial4Jobz.Models.JobPreferredIndustry jpi in Model.JobPreferredIndustries)
               { %>
                 <%:jpi.Industry.Name%>
            <%} %>
           <%}
              else
              { %>
            <%:"Any"%><%} %>
        </p>
              

          <p>
            <strong>Languages Required:</strong>
            <% foreach (Dial4Jobz.Models.JobLanguage jlan in Model.JobLanguages)
               { %>
                <%: jlan.Language.Name%><%:","%>
            <% } %>
        </p>   
        <p>
            <strong>Skills Required:</strong>
            <% foreach (Dial4Jobz.Models.JobSkill jskill in Model.JobSkills)
               { %>
                <%:jskill.Skill.Name%><%:","%>
            <% } %>
        </p>

        <p>
            <strong>Preferred Time:</strong>
             <% if (Model.PreferredAll == true)
                { %>
                <%:"Any"%><%:","%>
             <% }
                else
                { %>
                <%:""%><%} %>

             <% if (Model.PreferredContract == true)
                {  %>
                <%:"Contract"%><%:","%><%}
                else
                { %>
                <%:""%><%} %>

            <%if (Model.PreferredParttime == true)
              {  %>
               <%:"Part time"%><%:","%><%}
              else
              {%>
               <%:""%><%} %>

            <%if (Model.PreferredFulltime == true)
              { %>
                <%:"Full Time"%><%:","%><%}
              else
              { %>
                <%:""%><%} %>

            <%if (Model.PreferredWorkFromHome == true)
              {%>
                <%:"Work from home"%><%:","%><%}
              else
              { %>
                <%:""%><%} %>
        
        </p>

        
         <%if (Model.PreferredParttime == true)
           { %>
         <p>
        <strong>Timings for Parttime:</strong>
            <%:Model.PreferredTimeFromMinutes + " AM"%><%:" to"%><%:Model.PreferredTimeToMinutes + " PM"%> </p>
        <%} %>

        <p>
            <strong>Job Description:</strong><br/>
            <%var description = Model.Description; %>
            <%if (description != null)
              { %>
                <p><%:Model.Description%></p>
            <%}
              else
              { %>
                <%:""%>
            <%} %>
        </p>


       <div class="job-desc-details">
        <h2>Contact Details</h2>
        <p>
        <strong>Contact Person:</strong>
            <%: Model.ContactPerson%>
        </p>
        
        <p>
        <strong>Contact Number:</strong>
            <%: Model.ContactNumber%>
        </p>
        
        <p>
            <strong>Mobile Number:</strong>
            <%: Model.MobileNumber%>
        </p>
        
        <p>
        <strong>Email Address:</strong>
            <%: Model.EmailAddress%>
        </p>

        <p>
        <%if (Model.CommunicateViaSMS == true)
          { %>
            <strong>Communicate Via SMS:</strong>
            <%: "Yes"%>
        <%}
          else
          { %>
            <strong>Communicate Via SMS:</strong>
            <%: "No"%>
        <% } %>
        </p>
        
         <p>
            <%if (Model.CommunicateViaEmail == true)
              { %>
                <strong>Communicate Via Email: </strong> <%: "Yes"%>
            <%}
              else
              { %>
                <strong>Communicate Via Email:</strong>
                    <%: "No"%>
            <% } %>
        </p>
            
       <p>

        <strong>To edit the vacancy</strong> <%: Html.ActionLink("Click Here", "Edit", new { id = Model.Id }, new { target = "_blank" })%> |
         <%:Html.ActionLink("Save", "JobPostVas", "Jobs", new { target = "_blank" })%> |
         <%:Html.ActionLink("Post another Job", "Add", "Jobs", new { target = "_blank" })%> |
         <%: Html.ActionLink("Proceed", "CandidateMatch", "CandidateMatchesJob", new { id = Model.Id }, new { target = "_blank" })%>
      </p>
      </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStarted"); %> 
    <% Html.RenderPartial("Side/Video"); %> 
</asp:Content>

