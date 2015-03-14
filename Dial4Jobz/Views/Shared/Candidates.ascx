<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Dial4Jobz.Models.Candidate>>" %>
 <% Dial4Jobz.Models.Organization loggedInEmployer = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
 <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% Dial4Jobz.Models.User loggedInAdmin = (Dial4Jobz.Models.User)ViewData["LoggedInAdmin"]; %>
 <% bool isLoggedIn = loggedInEmployer != null; %>
 <% bool isAdminLoggedIn = loggedInAdmin != null; %>
 <% bool isConsultLoggedIn = LoggedInConsultant != null; %>

 


<div id="candidates">
    <ul id="candidate-list">
        <% foreach (var item in Model)
           { %>
            <% bool dprActivatedOrNot = _vasRepository.PlanSubscribedForDPR(item.Id); %>
            <%var checkDPR = _vasRepository.CheckDPRValidity(item.Id); %>
            <%var SIActivated = _vasRepository.PlanActivatedForSI(item.Id); %>
            <% var PlanActivated = _vasRepository.PlanActivatedForCandidate(item.Id); %>
           <% if (item.Name != null) { %>  
   
            <li class="candidate-list-item">
             <% if (isLoggedIn)
               { %>
           
                  <% } %>
                  <div class="first-line">     
                        <%if (isLoggedIn == true || isAdminLoggedIn == true || isConsultLoggedIn== true)
                          { %>    
                               <div class="candidate-select">
                                   <%: Html.CheckBox("Candidate" +  item.Id.ToString(), new { id = item.Id, @class="candidate"  })%>   
                               </div>  
                                    <a href="/employer/candidates/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" target="_blank"><%: item.DiplayCandidate%></a>
                        <%} else { %>
                            <a class="login"  href="<%=Url.Content("~/login")%>" title="Viewing candidate details is free. Please Login to view or Create One"><%:item.DiplayCandidate %></a>
                        <% } %>

                   <span class="location">
                       <%if (item.LocationId != null) { %>
                        <% if (item.GetLocation(item.LocationId.Value).RegionId != null) {%>
                            (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).Region.Name, "matchcandidates", "Employer", new { currentloc = item.GetLocation(item.LocationId.Value).Region.Name }, new { @class = "location" })%>)
                       <% } %>

                       <% if (item.GetLocation(item.LocationId.Value).CityId != null)
                        {%>
                            (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).City.Name, "matchcandidates", "Employer", new { currentloc = item.GetLocation(item.LocationId.Value).City.Name }, new { @class = "location" })%>)
                       <% } %>
                       <% if (item.GetLocation(item.LocationId.Value).CountryId != null)
                       {%>
                            (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).Country.Name, "matchcandidates", "Employer", new { currentloc = item.GetLocation(item.LocationId.Value).Country.Name }, new { @class = "location" })%>)
                       <% } %>
                        
                       <% } %> 
                   </span>   
                  </div>

          
                  <div class="job-details">
                      <div class="second-line">
                          <span class="org-name">
                              <% if (!string.IsNullOrEmpty(item.Position))
                                 { %>
                                    <%if (dprActivatedOrNot == true)
                                      { %>
                                          <b style="color:#4BCCE1;"> Featured Candidate Its Free!!!</b>  Click to View Resume with contact details<br />
                                    <%}%>
                                     Position: <%: item.Position %> 
                                  <%if (item.Consultante != null)
                                    { %>
                                    <span style="color:Green;">(By <%: item.Consultante.Name %> Consultant)</span>
                                <%} %>  
                                  
                                  <%if (checkDPR == true)
                                     { %>
                                        <a href=""><img src="<%=Url.Content("~/Content/Images/green_star.png")%>" width="21px" height="20px" alt="Paid-Display Resume" title="Display Resume" /></a> 
                                   <%} %>
                                   <%if (SIActivated == true)
                                     { %>
                                        <a href=""><img src="<%=Url.Content("~/Content/Images/SI-candidate.png")%>" width="15px" height="10px" alt="Spot Interview" title="Spot Interview" /></a>
                                   <%} %>

                                   <%if (PlanActivated == true)
                                     { %>
                                        <a href=""><img src="<%=Url.Content("~/Content/Images/star for paidemployers.jpg")%>" width="15px" height="15px" alt="Job Alert" title="Job Alert" /></a> 
                                   <%} %>

                              <% } %>   
                              <span class="right">
                              <% if ((!item.AnnualSalary.HasValue || item.AnnualSalary == 0))
                                      { %>
                                         <a class="salarybutton" style="background-color:Blue; font-weight:bold; border: 1px solid #BBE1EF;border-radius: 5px;display: inline-block; float: none;margin: 0px 5px 4px 0px; padding:3px 23px; color:White;" href="<%:Url.Action("MatchCandidates","Employer", new {annualsalary=item.AnnualSalary }) %>">Not Mentioned</a>

                                       <% } else {  %>
                                       <%var Monthlysalary = item.AnnualSalary / 12; %>

                                       <%var convertsalary = Convert.ToInt32(Monthlysalary).ToString("c0", new System.Globalization.CultureInfo("en-IN"));%>
                                       <a class="salarybutton" style="background-color:Blue; font-weight:bold; border: 1px solid #BBE1EF;border-radius: 5px;display: inline-block; float: none;margin: 0px 5px 4px 0px; padding:3px 23px; color:White;"  href="<%: Url.Action("MatchCandidates", "Employer", new {annualsalary=item.AnnualSalary}) %>"><%:convertsalary%> / PM</a>
                                   <% } %>
                              
                              </span></span>
                      </div>
                    </div>

                 <div class="candidate-details"> 
                   <div class="fourth-line">
                        <% if(item.FunctionId.HasValue) {  %>
                      <%--  <% if (!string.IsNullOrEmpty(item.Function))
                           {%>--%>
                              Function Area: <%:item.Function.Name%>
                            <%} else {%>
                                  Function Area: <%:""%>
                              <% } %>
                             
                             <span class="right">

                              Industry Type:  <% if (item.IndustryId.HasValue) {%>
                                     <%: item.GetIndustry(item.IndustryId.Value).Name%>
                             <%} else { %>
                                     <%:item.IndustryId==null ? "": item.GetIndustry(item.IndustryId.Value).Name %>
                             <%} %>

                                
                             </span>
                  </div> 

                  <div class="fourth-line">

                   Annual Salary: <% if ((!item.AnnualSalary.HasValue || item.AnnualSalary == 0)) { %>
                                    <%:"Not Mentioned" %>
                                 <% } else {  %>
                                        <%: Convert.ToInt32(item.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>  
                                         
                                 <%} %>


                  <span class="right" style="color:Gray;">
                  <%-- Experience: <%: item.TotalExperience / 33782400%> Years--%>
                                  <% if ((!item.TotalExperience.HasValue || item.TotalExperience == 0) && (!item.MaxExperience.HasValue || item.MaxExperience == 0))
                                     { %>
                                        <%:"Experience: Not Mentioned" %>
                                  <% }
                                     else if (!item.TotalExperience.HasValue || item.TotalExperience == 0)
                                     { %>
                                  Up to
                                  <%: Math.Ceiling(item.TotalExperience.Value / 31104000.0)%>
                                  Years
                                  <% }
                                     else if (!item.TotalExperience.HasValue || item.TotalExperience == 0)
                                     { %>
                                  <%: Math.Ceiling(item.TotalExperience.Value / 33782400.0) %>
                                  + Years
                                  <% }
                                     else
                                     {  %>
                                  <%:item.TotalExperience.Value/31104000 %>
                                  Years
                                  <%:((item.TotalExperience.Value-(item.TotalExperience.Value/31104000) *31536000))/2678400 %>
                                  Months
                                  <% } %>
               
                                 
                  </span>
                  </div>
                   
                   
                    <%-- Industry Type:  <% if (item.IndustryId.HasValue) {%>
                                     <%: item.GetIndustry(item.IndustryId.Value).Name%>
                             <%} else { %>
                                     <%:item.IndustryId==null ? "": item.GetIndustry(item.IndustryId.Value).Name %>
                             <%} %>--%>

                      

                   <div class="fourth-line">
                   <%--Modified on:<%:item.UpdatedDate %>--%>
                   <%if (item.UpdatedDate != null)
                     { %>
                        <%var updatedDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.UpdatedDate); %>
                        <%--<%var updatedDate1 = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime.UtcNow.AddHours(5).AddMinutes(30)) item.UpdatedDate); %>--%>
                        Modified: <%:updatedDate %>
                   <%}
                     else
                     { %>
                        <%var createdDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.CreatedDate); %>
                        Submitted: <%:createdDate%>
                   <%} %>

                    <span class="right">Qualification: <% foreach (Dial4Jobz.Models.CandidateQualification cq in item.CandidateQualifications){ %>
                                    <%: cq.Degree.Name %><%:"," %>
                             <% } %>
                    </span>
                    </div>


                     <div class="fourth-line">
                     <%int viewedCount = _vasRepository.GetCountForDPRView(item.Id); %>
                         <% if( viewedCount== 0)
                             {%>

                             <%} else { %>
                                <b>Views: <%:viewedCount%> </b> <br />
                         <%} %>
                     
                     <% if (new Dial4Jobz.Models.Repositories.VasRepository().CheckDPRValidity(item.Id)) {%>  
                         <%--MobileNumber: <%: item.ContactNumber%> | Email: <%: item.Email%>--%>
                         
                    <% } else { %>
                           <%if (item.IsPhoneVerified == true && item.IsMailVerified == true)
                             { %>
                                MobileNumber:   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span> | Email:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" alt="dial4jobz_verified" width="14px" height="12px" /><span class="green">Verified</span>
                            <%}
                             else if (item.IsPhoneVerified == false && item.IsMailVerified == false)
                             { %>
                                 MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
                            <%}
                             else if (item.IsPhoneVerified == true && item.IsMailVerified == false)
                             { %>
                                MobileNumber:   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span> | Email: <span class="red">Pending Verification</span>
                            <%}
                             else if (item.IsPhoneVerified == false && item.IsMailVerified == true)
                             { %>
                                 MobileNumber: <span class="red">Pending Verification</span> | Email:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span>

                            <%} else if (item.IsPhoneVerified == null && item.IsMailVerified == null) { %>
                                MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>

                            <%} else if (item.IsPhoneVerified == true && item.IsMailVerified == null) { %>
                                MobileNumber: <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span> | Email: <span class="red">Pending Verification</span>

                            <% } else if (item.IsMailVerified == true && item.IsPhoneVerified == null) {  %>
                                 Email:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span> | MobileNumber: <span class="red">Pending Verification</span>

                            <% } else if(item.IsPhoneVerified==null && item.IsMailVerified==null) { %>
                                MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>

                            <%} else if(item.IsPhoneVerified==false && item.IsMailVerified==null) { %>
                                MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>

                            <%} %>
                          
                    <%} %>

         <%--  <span class="right"> Preferred Location:
             <span class="location">
              <%if (item.CandidatePreferredLocations.Count() > 0)
                  { %>
                    <% List<string> countryIds = new List<string>();
                       List<string> stateIds = new List<string>();
                       List<string> cityIds = new List<string>();
                           foreach (Dial4Jobz.Models.CandidatePreferredLocation cl in item.CandidatePreferredLocations.OrderBy(c => c.Location.CountryId))
                           { %>
                        <% if (cl.Location != null)
                           {

                               if (cl.Location.Country != null)
                               {
                                   if (!countryIds.Any(s => s.Contains(cl.Location.CountryId.ToString())))
                                   {
                                       countryIds.Add(cl.Location.CountryId.ToString());
                                        %>
                                        <%: cl.Location.Country.Name%><%:","%>
                                <% }
                               }


                               if (cl.Location.State != null)
                               {
                                   if (!stateIds.Any(s => s.Contains(cl.Location.StateId.ToString())))
                                   {
                                       stateIds.Add(cl.Location.StateId.ToString());
                                    %>
                                        <%: cl.Location.State.Name%><%:","%>
                                    <%  
                                   }
                               }

                               if (cl.Location.City != null)
                               {
                                   if (!cityIds.Any(s => s.Contains(cl.Location.CityId.ToString())))
                                   {
                                       cityIds.Add(cl.Location.CityId.ToString());
                                     %>
                                        <%: cl.Location.City.Name%><%:","%>
                                     <% 
                                   }
                               }

                               if (cl.Location.Region != null)
                               { %>
                                    <%: cl.Location.Region.Name%><%:","%>
                            <% }

                           }
                           }
                       }
                       
                  }
                  else
                  { %>
                    <%:"Any"%><%} %>
                </span>  
                                      
                    </span> --%>

                    
          <span class="right"> Preferred Location:
             <span class="location">
              <%if (item.CandidatePreferredLocations.Count() > 0)
                { %>
                  <%var Count = 0; %>
                    <% List<string> countryIds = new List<string>();
                       List<string> stateIds = new List<string>();
                       List<string> cityIds = new List<string>();
                       List<string> regionIds = new List<string>();

                       foreach (Dial4Jobz.Models.CandidatePreferredLocation cl in item.CandidatePreferredLocations.OrderBy(c => c.Location.CountryId))
                       { %>
                       <% if (Count < 5)
                          { %>
                        <% if (cl.Location != null)
                           {

                               if (cl.Location.Country != null && Count < 5)
                               {
                                   if (!countryIds.Any(s => s.Contains(cl.Location.CountryId.ToString())))
                                   {
                                       countryIds.Add(cl.Location.CountryId.ToString());
                                        %>
                                        <%: cl.Location.Country.Name%><%:","%>
                                <% }
                                   Count = Count + 1;
                               }


                               if (cl.Location.State != null && Count < 5)
                               {
                                   if (!stateIds.Any(s => s.Contains(cl.Location.StateId.ToString())))
                                   {
                                       stateIds.Add(cl.Location.StateId.ToString());
                                    %>
                                        <%: cl.Location.State.Name%><%:","%>
                                    <%  
                                   }
                                   Count = Count + 1;
                               }

                               if (cl.Location.City != null && Count < 5)
                               {
                                   if (!cityIds.Any(s => s.Contains(cl.Location.CityId.ToString())))
                                   {
                                       cityIds.Add(cl.Location.CityId.ToString());
                                     %>
                                        <%: cl.Location.City.Name%><%:","%>
                                     <% 
                                   }
                                   Count = Count + 1;
                               }

                               if (cl.Location.Region != null && Count < 5)
                               {
                                   if (!regionIds.Any(r => r.Contains(cl.Location.RegionId.ToString())))
                                   {
                                   
                                    %>
                                        <%: cl.Location.Region.Name%><%:","%>
                                    <%
                                   }
                                   Count = Count + 1;
                               }

                           }
                          }
                       }
                       if (Count > 4)
                       {%>
                       <%if (isLoggedIn == true)
                         {%>
                         <a href="/employer/candidates/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" target="_blank">See More....</a>
                      <% }
                         else
                         {%>
                          <a class="login" href="<%=Url.Content("~/login")%>" title="Login">See More...</a>
                          <%} %>
                      <%}
                           
                }
                else
                { %>
                    <%:"Any"%><%} %>
                    
                </span>  
                                      
                    </span> 
                     </div>


                    <div class="fourth-line">

                      <span class="right">
                                <%if (isLoggedIn == true)
                                  { %>
                                    <%var viewedList = _vasRepository.GetViewedList(loggedInEmployer.Id); %>
                                    <%var tick = viewedList.Select(vl => vl.CandidateId == item.Id); %>
                                    <%if (tick.Contains(Convert.ToBoolean(item.Id)))
                                      { %>
                                        <h3><b>Viewed</b></h3>
                                    <%} else { %>
                                  
                                    <% } %>
                                <%} else { %>
                                 
                                <%} %>
                      </span>
                    </div>


                           <div class="fourth-line">
                                  <%if (item.Description != null)
                                  { %>
                                Description: <%:item.Description.IndexOf("\n") > 0 ? String.Format("{0}...", item.Description.Substring(0, item.Description.IndexOf("\n"))) : item.Description%>
                                <% } else{ %>
                                     Description: <%:""%>
                                <% } %>
                           </div>
                        <% if (!String.IsNullOrEmpty(item.ResumeFileName)) 
                                { %>
                     		<%if (isLoggedIn == true)                       { %>                     
                            <p>
                         		<a href="<%:Url.Action("Download","Candidates",new {fileName=item.ResumeFileName}) %>"> Download Resume</a>     
                            </p>                     
                     		<% } else {%>
                     	        <a class="login" href="<%=Url.Content("~/login")%>" title="Login or Create an account on Dial4Jobz to Download"> Download Resume</a>
                     	    <%} %>
                     <%}%>

                    
                    <div class="fourth-line">
                       <%-- <% foreach (Dial4Jobz.Models.CandidateSkill cs in item.CandidateSkills){ %>
                            <%: Html.ActionLink(cs.Skill.Name, "MatchCandidates", "Employer", new { skill = cs.Skill.Id }, new { @class = "skill" })%>
                        <% } %>--%>
                        <%var skillcount = 0; %>
                        <%if (item.CandidateSkills.Count() > 0)
                          { %>
                             <% foreach (Dial4Jobz.Models.CandidateSkill cs in item.CandidateSkills){ %>
                             <%if (skillcount < 5)
                               { %>
                                <%if (cs.Skill != null && skillcount < 5)
                                  { %>
                                    <%: Html.ActionLink(cs.Skill.Name, "MatchCandidates", "Employer", new { skill = cs.Skill.Id }, new { @class = "skill" })%>
                                <%} %>
                             <%} %>
                              <% skillcount = skillcount + 1; %>
                            <% } %>

                       <%if (skillcount > 4)
                         {%>
                         <%if(isLoggedIn==true) {%>
                             <a href="/employer/candidates/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" target="_blank">See More....</a>
                         <%} else { %>
                             <a class="login" href="<%=Url.Content("~/login")%>" title="Login">See More...</a>
                         <%} %>
                      <% } %>
                    <%} %>

                </div>    
            </div>
            </li>
        <%} %>
         <%} %>
 </ul>
      <% if(ViewData["moreUrl"] != null) { %>
         <% var filters = (Dictionary<string, string>) ViewData["Filters"]; %>
         <% var url = ViewData["moreUrl"] + Dial4Jobz.Helpers.StringHelper.AssembleQueryStrings(filters, true); %>
         <a id="moreLink" href="<%= url %>" title="Click here to see more Candidates">View More Candidates</a>
      <% } %>
</div>