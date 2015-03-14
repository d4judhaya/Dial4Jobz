<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Position %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
    <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
    <% bool isLoggedIn = loggedInCandidate != null; %>
    <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
   

    <%if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/employer/jobs/add"))
      {  %>
       <%var paymentstatus = _vasRepository.GetPlanActivatedResultRAT(loggedInOrganization.Id);%>
         <h3>Dear <%:Model.ContactPerson%>, Thanks for Posting the Job as under.</h3>

        <h3><p>Job posting is free and unlimited</p></h3> <%: Html.ActionLink("Post another job", "Add", "Jobs", null, new { @title = "Post a job", id = "podtJob" })%>|
        <%: Html.ActionLink("Get Matching Candidates for your vacancy", "CandidateMatch", "CandidateMatchesJob", new { id = Model.Id }, new { target = "_blank" })%>|
             
        <% if (paymentstatus != null)
            {%>
                <input id="ActivateRATVacancy" type="submit" onclick="javascript:Dial4Jobz.Job.ActivateRatVacancy(this,'<%=Model.Id %>');return false;" value="Active this Vacancy" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
            <%} else  { %>
                
            <% } %>
     
         <h2>Posted Job as displayed in site</h2>
    <%} %>

     <h3 style="font-size:20px; color:orange">
     <%if(Model.Position!=null) { %>
         <%: Html.Encode(Model.Position) %><br />
     <%} %>
     <%var org = Model.Organization; %>
     <%if (org != null) { %>
         <i style="color:rgb(122, 162, 170);">(Posted by
        <%: Model.Organization.Name%>)</i>
      <%} else { %>
        <%:""%>
      <%} %>
      </h3>

        

     <span class="location">
        <%  List<string> citiesId = new List<string>();
            foreach(Dial4Jobz.Models.JobLocation jl in Model.JobLocations.OrderBy(c => c.Location.CountryId)){ 
                if (jl.Location.Country != null)
                        {
                            if (jl.Location.CountryId != 152)
                            {                               
                                %>
                                (<%: Html.ActionLink(jl.Location.ToString(), "Index", "Jobs", new { where = jl.Location.ToString() }, new { @class = "location" })%>)
                        <% }
                        }

                if (jl.Location.City != null)
                       {
                           if (!citiesId.Any(s => s.Contains(jl.Location.CityId.ToString())))
                           {
                               citiesId.Add(jl.Location.CityId.ToString());
                             %>
                                (<%: Html.ActionLink(jl.Location.ToString(), "Index", "Jobs", new { where = jl.Location.ToString() }, new { @class = "location" })%>)
                             <% 
                           }
                       } 
                       
                    
                
         } %>
    </span>  

   
        <% foreach(Dial4Jobz.Models.JobSkill js in Model.JobSkills) { %>
         <div class="fourth-line">
                <%: Html.ActionLink(js.Skill.Name, "Index", "Jobs", new { skill = js.Skill.Id }, new { @class ="skill" } )%> 
          </div>  
        <% } %>

    
    <%--<div class="clear"></div>--%>
    <div class="job-desc">
        <h3>Job Details</h3>

            <p>
            <% if (Model.CreatedDate != null)
               { %>
                <strong>Date Posted:</strong>
                <%:Model.CreatedDate%><%:"," %>
            <% } %>
              
        
            <% if (Model.UpdatedDate != null) { %>
                <strong>Updated Date:</strong>
                    <%:Model.UpdatedDate%>
                <%} else { %>
           </p>
                   <%-- <strong>Date Posted:</strong>
                    <%:Model.CreatedDate%>--%>
            <% } %>
        

          <p style="line-height:1.8;">
            <strong> Job Location: </strong>
            <% List<string> countryIds = new List<string>();
               List<string> stateIds = new List<string>();
               List<string> cityIds = new List<string>();

               foreach (Dial4Jobz.Models.JobLocation jl in Model.JobLocations.OrderBy(c => c.Location.CountryId))
               { %>
                <% if (jl.Location != null)
                   {
                       
                        if (jl.Location.Country != null)
                        {
                            if (!countryIds.Any(s => s.Contains(jl.Location.CountryId.ToString())))
                            {
                                countryIds.Add(jl.Location.CountryId.ToString());
                                %>
                                <%: jl.Location.Country.Name%><%:","%>
                        <% }
                        }

                       if (jl.Location.State != null)
                       {
                            if (!stateIds.Any(s => s.Contains(jl.Location.StateId.ToString())))
                            {
                                stateIds.Add(jl.Location.StateId.ToString());
                            %>
                                <%: jl.Location.State.Name%><%:","%>
                            <%  
                            }
                       }

                       if (jl.Location.City != null)
                       {
                           if (!cityIds.Any(s => s.Contains(jl.Location.CityId.ToString())))
                           {
                               cityIds.Add(jl.Location.CityId.ToString());
                             %>
                                <%: jl.Location.City.Name%><%:","%>
                             <% 
                           }
                       }
                       
                       if (jl.Location.Region != null)
                       { %>
                            <%: jl.Location.Region.Name%><%:","%>
                    <% }
                    
                   }
               } %>

        </p>

         <p>
            <strong>Salary Range:</strong>
            <% if ((!Model.Budget.HasValue || Model.Budget == 0) && (!Model.MaxBudget.HasValue || Model.MaxBudget == 0)) { %>
               <%:"Not Mentioned" %>
            <% } else if (!Model.Budget.HasValue || Model.Budget == 0) { %>
                <%: Convert.ToInt32(Model.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> per annum
            <% } else if (!Model.MaxBudget.HasValue || Model.MaxBudget == 0){ %>
                <%: Convert.ToInt32(Model.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> per annum
            <% } else {  %>
                <%: Convert.ToInt32(Model.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> to 
                <%: Convert.ToInt32(Model.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%> per annum
            <%} %>
        </p>

       
        <p>
            <strong>Experience:</strong>
            <% if ((!Model.MinExperience.HasValue || Model.MinExperience == 0) && (!Model.MaxExperience.HasValue || Model.MaxExperience == 0)){ %>
                <%:Model.MinExperience %>
            <% } else if (!Model.MinExperience.HasValue || Model.MinExperience == 0){ %>
                Up to <%: Math.Ceiling(Model.MaxExperience.Value / 33782400.0) %> Years 
            <% } else if (!Model.MaxExperience.HasValue || Model.MaxExperience == 0){ %>
                <%: Math.Ceiling(Model.MinExperience.Value / 33782400.0) %> + Years
            <% } else {  %>
                <%: Math.Ceiling(Model.MinExperience.Value / 33782400.0) %> to 
                <%: Math.Ceiling(Model.MaxExperience.Value / 33782400.0) %> Years
            <% } %>
        </p>

        
       <p style="line-height:1.8;">
            <strong>Qualification:</strong>
            
           <%if (Model.JobRequiredQualifications.Count() > 0)   { %>
                        
                      <% List<string> degreeIds = new List<string>();

                         foreach (Dial4Jobz.Models.JobRequiredQualification jrq in Model.JobRequiredQualifications)
                           {%>
                            <% if(jrq.Degree!=null)
                               {
                                   if(!degreeIds.Any(s=>s.Contains(jrq.DegreeId.ToString())))
                                   {
                                       degreeIds.Add(jrq.DegreeId.ToString()); %>
                                       <%: jrq.Degree.Name%><%:","%>
                                  <%}
                               }

                               if (jrq.SpecializationId.HasValue)
                               { %>
                                 (<%: Model.GetSpecialization(jrq.SpecializationId.Value).Name%>)<%:","%>
                              <% }
                       }
                       
                  } else { %>
                        <%:"Any"%>
                  <%} %>  


        </p>

        <p>
            <strong>License Types:</strong>
            <%if (Model.TwoWheeler == true || Model.FourWheeler == true)
              { %>
                <%if (Model.TwoWheeler == true)
                  { %>
                    <%:"Two Wheeler"%><%:"," %>
                <%} else { %>
                    <%:"Four Wheeler" %><%:"," %>
                <%} %>
                <%foreach (Dial4Jobz.Models.JobLicenseType jl in Model.JobLicenseTypes)
                  { %>
                    <%:jl.LicenseType.Name %>
                <% } %>
            <%} %>
        </p>

        <% if (Model.FunctionId.HasValue){ %>
            <p>
                <strong>Job Function:</strong>
                <%: Model.GetFunction(Model.FunctionId.Value).Name %>
            </p>
        <% }  else {%>     
            <strong>Job Function:</strong>   
            <%:Model.FunctionId==null? "Any" : Model.GetFunction(Model.FunctionId.Value).Name%><% } %>
            
        <p>
            <strong>Job Role:</strong>
            <% foreach (Dial4Jobz.Models.JobRole jr in Model.JobRoles){ %>
                <%: jr.Role.Name %>
            <% } %>
        </p>


         <p>
            <strong>Preferred Industry:</strong>
            <%if (Model.JobPreferredIndustries.Count()>0) { %>
            <% foreach (Dial4Jobz.Models.JobPreferredIndustry jpi in Model.JobPreferredIndustries) { %>
                 <%:jpi.Industry.Name%>
            <%} %>
           <%} else { %>
                <%:"Any"%>
           <%} %>
        </p>


        <p>
            <strong>Languages Required:</strong>
            <% foreach (Dial4Jobz.Models.JobLanguage jlan in Model.JobLanguages) { %>
                <%: jlan.Language.Name %><%:"," %>
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
            <strong> Gender:</strong>
            <%if (Model.Male == true && Model.Female == true) {%>
                <%:"Male, Female" %>
            <%} else if (Model.Female == true)
              { %>
                <%:"Female"%>
            <%}
              else if (Model.Male == true)
              { %>
                <%:"Male"%>
            <%}
              else { %>
                <%:"Not Mentioned" %>
            <%} %>

        
        </p>

        <p>
            <strong>Job Type:</strong>
             <% if (Model.PreferredAll == true) { %>
                <%:"Any"%><%:","%>
             <% } else { %>
                <%:""%><%} %>

             <% if (Model.PreferredContract == true) {  %>
                <%:"Contract"%><%:","%><%} else { %>
                <%:""%><%} %>

            <%if(Model.PreferredParttime==true) {  %>
               <%:"Part time" %><%:"," %><%} else {%>
               <%:""%><%} %>

            <%if (Model.PreferredFulltime == true)
              { %>
                <%:"Full Time"%><%:","%><%} else { %>
                <%:""%><%} %>

            <%if (Model.PreferredWorkFromHome == true)
              {%>
                <%:"Work from home"%><%:","%><%} else { %>
                <%:""%><%} %>
        
        </p>

        
         <%if (Model.PreferredParttime == true) { %>
         <p>
        <strong>Preferred Time:</strong>
            <%: Model.PreferredTimeFrom %><%:" to"%><%: Model.PreferredTimeTo%> </p>
        <%} %>

        <p>
        <strong>Shift Type:</strong>
        <%if(Model.GeneralShift==true) { %>
            <%:"General Shift" %>
        <%}
          else if (Model.NightShift == true)
          { %>
            <%:"Night Shift"%>
        <%}
          else
          { %>
            <%: "No Shift" %>
        <%} %>
        </p>

        <p style="line-height:1.8;">
            <strong>Job Description:</strong><br/>
            <%var description = Model.Description; %>
            <%if (description != null)
              { %>
                <%:Model.Description%>
            <%}
              else
              { %>
                <%:""%>
            <%} %>
        </p>
        
        <div class="job-desc-details">
            <h3>Contact Details</h3>
        
            <p>
                <strong>Contact Person:</strong>
                <%: Model.ContactPerson%>
            </p>

            <p>
                <strong>Employer Type: </strong>
                <%if (Model.Consultante == null)
                  { %>
                    <%if (Model.Organization.EmployerType != null)
                      { %>
                        <%:((Dial4Jobz.Models.Enums.EmployerType)Model.Organization.EmployerType)%>
                    <%} else { %>

                    <%} %>
                <%} %>
            </p>

            <p>

                <strong>Ownership Type: </strong>
                  <%if (Model.Consultante == null)
                  { %>
                    <%if (Model.Organization.OwnershipType != null)
                      { %>
                    <%:((Dial4Jobz.Models.Enums.OwnershipType)Model.Organization.OwnershipType)%>
                    <%} else { %>
                    <%} %>
                <%} %>
            </p>

            <p>
                <strong>Employer Strength: </strong>
                  <%if (Model.Consultante == null)
                  { %>
                    <%if (Model.Organization.EmployersCount != null)
                      { %>
                          <%: Model.GetEmployeesCount(Model.Organization.EmployersCount.Value).Name%>
                    <%} else { %>
                    <%} %>
                <%} %>
            </p>
             <p>
                <strong>Website:</strong>
                  <%if (org != null) { %>
                   <a href="http://<%: Model.Organization.Website %>"><%: Model.Organization.Website%></a>
                  <%} else { %>
                    <%:""%>
                  <%} %>
            </p>

            <p>
            <%if (Model.HideDetails == true) { %>
                <strong></strong>
                <% } else { %>
                    <strong>Landline Number:</strong>
                <%var contactNumber = Model.ContactNumber; %>
                <%if (contactNumber != null) { %>
                    <%: Model.ContactNumber%>
                <% } else { %>
                    <%:""%>
                <%} %>
            <% }%>
            </p>


          


            <p>
                <%if (Model.HideDetails == true)
                  { %>
                    <strong></strong>
                <% }
                  else
                  { %>
                   <strong>Mobile Number:</strong>
                   <%var mobileNumber = Model.MobileNumber; %>
                   <%if (mobileNumber != null)
                     { %>
                        <%: Model.MobileNumber%>
                   <%} else { %>
                     <%:""%>
                   <% } %>
                <% } %>
            </p>

            <p>
                <%if (Model.Organization != null)
                  { %>
                     <strong>Address:</strong>
                     <%:Model.Organization.Address %>
                <%} %>
               
            </p>

            <p>
                <%if (Model.HideDetails == true)
                  { %>
                    <strong></strong>
                <% } else { %>
                    <strong>Email Id:</strong>
                    <%var email = Model.EmailAddress; %>
                    <%if (email != null)
                       { %>
                        <a href="mailto:<%: Model.EmailAddress %>">
                    <%: Model.EmailAddress%></a>
                <%} else { %>
                    <%:""%>
                <%} %>
                <% } %>
            </p>

    <%if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/employer/jobs/add"))
      {  %>
      <p>
        <strong>To edit the vacancy</strong> <%: Html.ActionLink("Click Here", "Edit", new { id = Model.Id }, new { target = "_blank" })%> |
         <%:Html.ActionLink("Save", "JobPostVas", "Jobs", new { target = "_blank" })%> |
         <%:Html.ActionLink("Post another Job", "Add", "Jobs", new { target = "_blank" })%> |
         <%: Html.ActionLink("Proceed", "CandidateMatch", "CandidateMatchesJob", new { id = Model.Id }, new { target = "_blank" })%>
      </p>

      <%} else { %>
            <% if (isLoggedIn)
                { %>
                <% var smsStatus = _vasRepository.GetCandidateSmsVas(loggedInCandidate.Id); %>
            <% Html.BeginForm("Send", "Jobs", FormMethod.Post, new { @id = "Send" });  %>            
                <p>
                    <% if (smsStatus != null)
                        { %>
                        <input id="Submit1" type ="submit" value="Apply by SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Job.SendApplyJob(this, 0,'<%: Model.Id %>');return false;" />
                        <input id="Both" type="submit" value ="Apply by Email and/or SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Job.SendApplyJob(this, 2,'<%: Model.Id %>');return false;" />
                    <%}
                        else
                        {  %>
                        <a class="sendsmsbtn" style="color:White;" href="../../../../candidates/candidatesvas/index#smsPurchase">Buy SMS</a>
                    <%} %>
                    <input id="Submit2" type ="submit" value ="Apply by Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Job.SendApplyJob(this, 1,'<%: Model.Id %>');return false;" />
                </p>
                <% Html.EndForm(); %>
            <% }
                else
                { %>
                    <p>
                        <a class="login" id="loginapply" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Login to apply</a>
                    </p>
            <% } %>
            <%} %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<%if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/employer/jobs/add"))
     {  %>
         <% Html.RenderPartial("NavEmployer"); %>
    <%}
     else
     { %>
        <% Html.RenderPartial("Nav"); %>
    <%} %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStarted"); %> 
</asp:Content>

