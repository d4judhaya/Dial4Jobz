<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Dial4Jobz.Models.Job>>" %>
<% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
<% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
<% bool isLoggedIn = loggedInCandidate != null; %>


<div id="jobs">

    <ul id="job-list">
        <% foreach (var item in Model)
           { %>
        <li class="job-list-item">
            <% if (isLoggedIn)
               { %>
           
            <%  } %>
            <div class="first-line">
                <% if (isLoggedIn == true && loggedInCandidate.Name!=null)
                   { %>
                    <a href="/jobs/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" target="_blank" ><%: item.DisplayPosition %></a>
            <%  } else { %>
                     <a class="login"  href="<%=Url.Content("~/login")%>" title="Viewing Job details is free. Please Login to view or Register"><%:item.DisplayPosition %></a>
            <% } %>
                <span class="location">
                    <%  List<string> citiesId = new List<string>();
                        foreach (Dial4Jobz.Models.JobLocation jl in item.JobLocations.OrderBy(c => c.Location.CountryId))
                        {
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
            </div>
            <div class="job-details">
                <div class="second-line">
                    <span class="org-name">
                        <% var org = item.Organization; %>
                        <% var consultant = item.Consultante; %>
                        <% Dial4Jobz.Models.Repositories.Repository repository = new Dial4Jobz.Models.Repositories.Repository(); %>
                        <% IEnumerable<Dial4Jobz.Models.OrderDetail> getActivePlans = null;%>
                        <%if (org != null)
                          { %>
                               <%: item.Organization.Name%>
                               <% var SSActivated = _vasRepository.PlanActivatedForSS(org.Id); %>
                               <% var planactivated = _vasRepository.PlanSubscribed(org.Id); %>
                            <%if (planactivated == true)
                              { %>
                                 <a href=""><img src="<%=Url.Content("~/Content/Images/star for paidemployers.jpg")%>" width="15px" height="15px" alt="Featured Employer" title="Featured Employer" /></a> 
                             <% } %>

                             <% if (SSActivated == true)
                                { %>
                                   <a href=""><img src="<%=Url.Content("~/Content/Images/SS.png")%>" width="15px" height="10px" alt="Featured Employer" title="Featured Employer" /></a>
                             <%} %>
                            <%} else if(consultant!=null)  { %>
                               <%: consultant.Name %> (Consultant)
                               <% var SSActivated = _vasRepository.PlanActivatedSSConsultant(consultant.Id); %> 
                               <% var planactivated = _vasRepository.PlanSubscribedForConsultant(consultant.Id); %>
                              <% getActivePlans = _vasRepository.GetActivatedPlansByConsultant(consultant.Id); %>
                            <%if (planactivated==true)
                              { %>
                                 <a href=""><img src="<%=Url.Content("~/Content/Images/star for paidemployers.jpg")%>" width="15px" height="15px" alt="Featured Employer" title="Featured Employer" /></a> 
                             <% } %>

                             <% if (SSActivated == true)
                                { %>
                                   <a href=""><img src="<%=Url.Content("~/Content/Images/SS.png")%>" width="15px" height="10px" alt="Featured Employer" title="Featured Employer" /></a>
                             <%} %>
                             <%if(getActivePlans.Count() > 0) { %>
                                <a href=""><img src="<%=Url.Content("~/Content/Images/consult_star.jpg")%>" width="30px" height="26px" alt="Featured Employer" title="Featured Employer" /></a>
                             <%} %>
                            <%} else { %>
                                <%:""%>
                            <%} %>
                    </span>
                </div>
                <div class="fourth-line">
                    <% if (item.FunctionId.HasValue)
                       { %>
                    Job Function:
                        <%: item.GetFunction(item.FunctionId.Value).Name %>
                    <% } %>

                    <span class="right">
                        Role:
                        <%if (item.JobRoles.Count() > 0)
                          { %>
                        <% foreach (Dial4Jobz.Models.JobRole jr in item.JobRoles)
                           { %>
                                 <%: jr.Role.Name%><%:","%>
                        <%} %>
                        <%} else { %>
                             <%:"Any Role" %>
                        <%} %>
                    </span>
                </div>


                <div class="fourth-line">
                   <span style="color:rgba(104, 98, 98, 1);font-weight:bold;">
                        Experience:
                        
                        <% if ((!item.MinExperience.HasValue || item.MinExperience == 0) && (!item.MaxExperience.HasValue || item.MaxExperience == 0))
                           { %>
                        <%:item.MinExperience %>
                        <% }
                           else if (!item.MinExperience.HasValue || item.MinExperience == 0)
                           { %>
                        0 to
                            <%--<%: Math.Ceiling(item.MaxExperience.Value / 33782400.0)%>--%>
                            <%: Math.Ceiling(item.MaxExperience.Value / 31536000.0)%>
                        Years
                        <% }
                           else if (!item.MaxExperience.HasValue || item.MaxExperience == 0)
                           { %>
                        <%: Math.Ceiling(item.MinExperience.Value / 31536000.0)%> + Years
                        <% } else {  %>
                           <%: Math.Ceiling(item.MinExperience.Value / 31536000.0)%>
                        to
                        <%: Math.Ceiling(item.MaxExperience.Value / 31536000.0)%>
                        Years
                        <% } %>
                        
                        <span class="right">Annual Salary:
                        <% if ((!item.Budget.HasValue || item.Budget == 0) && (!item.MaxBudget.HasValue || item.MaxBudget == 0))
                           { %>
                              <%:"Not Mentioned" %>
                        <% }
                           else if (!item.Budget.HasValue || item.Budget == 0)
                           { %>
                        <%: Convert.ToInt32(item.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        <% }
                           else if (!item.MaxBudget.HasValue || item.MaxBudget == 0)
                           { %>
                        <%: Convert.ToInt32(item.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        <% } else
                           {  %>
                        <%: Convert.ToInt32(item.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        to
                        <%: Convert.ToInt32(item.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        <%} %>
                    </span>
                    </span>
                  
                </div>

                <div class="fourth-line">
                 <%if (item.UpdatedDate == null) { %>
                    Posted:
                    <% var date = DateTime.UtcNow; %>
                    <% var createdDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.CreatedDate); %>
                        <%:createdDate%>
                    <%} else { %>
                    Updated:
                      <% var date = DateTime.UtcNow; %>
                      <% var updatedDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.UpdatedDate); %>
                      <%:updatedDate%>
                    <%} %>

                   <span class="right">
                       Qualification:
                    <%if (item.JobRequiredQualifications.Count() > 0)  
                         { %>
                        
                      <% List<string> degreeIds = new List<string>();
                       
                       foreach(Dial4Jobz.Models.JobRequiredQualification jrq in item.JobRequiredQualifications)
                       {%>
                            <% if(jrq.Degree!=null)
                               {
                                   if(!degreeIds.Any(s=>s.Contains(jrq.DegreeId.ToString())))
                                   {
                                       degreeIds.Add(jrq.DegreeId.ToString()); %>
                                       <%: jrq.Degree.Name%><%:","%>
                                  <%}
                               }
                       }
                  } else { %>
                        <%:"Any"%>
                  <%} %>  


                   </span>
                 
                </div>

                <div class="fourth-line">
                    Industry: 
                    <%if (item.JobPreferredIndustries.Count() > 0)
                      { %>
                    <% foreach (Dial4Jobz.Models.JobPreferredIndustry ji in item.JobPreferredIndustries)
                       { %>
                            <%: ji.Industry.Name%>
                            <%:","%>
                    <%} %>
                    <%}
                      else
                      { %>
                    <%:"Any" %>
                    <% } %>

                    <span class="right">
                        <%int viewedCount = _vasRepository.GetCountForJobViews(item.Id); %>
                         <% if( viewedCount== 0 )
                             {%>

                             <%} else { %>
                                <b>Views: <%:viewedCount%> </b><br />
                         <%} %>
                    </span>
                   
                </div>
                
                <div class="fourth-line ellipsis">
                    <%if (item.Description != null)
                      { %>
                        <%if (item.Description.IndexOf("\n") > 0)
                          { %>
                            Description:
                            <%:item.Description.IndexOf("\n") > 0 ? String.Format("{0}...", item.Description.Substring(0, 20)) : item.Description%>
                        <%} else { %>
                            Description:
                            <%:item.Description.Length >20 ? String.Format("{0}...",item.Description.Substring(0,20)) : item.Description%>
                        <%} %>
                    <% } else { %>
                        Job Description:
                            <%:""%>
                    <% } %>
                </div>

                <div class="fourth-line">
                     <% foreach (Dial4Jobz.Models.JobSkill js in item.JobSkills)
                       { %>
                             <%: Html.ActionLink(js.Skill.Name, "Index", "Jobs", new { skill = js.Skill.Id }, new { @class ="skill" })%>
                    <% } %>
                </div>

            </div>
        </li>
        <% } %>
    </ul>
    <% if(ViewData["moreUrl"] != null) { %>
            <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>
            <% var url = ViewData["moreUrl"] + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters, true); %>
            <a id="moreLink" href="<%= url %>" title="Click here to see more jobs">View More Jobs</a>
     <% } %>
 </div>