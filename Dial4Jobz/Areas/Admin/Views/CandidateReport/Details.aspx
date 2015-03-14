<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>

<%@ Import Namespace="Dial4Jobz.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Position %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
        <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>

        <script type="text/javascript">   
            $(document).ready(function () {
             $("#candidatecontact").hide();
                    document.getElementById("counting").value =  '<%= ViewData["ResumesCount"] %>';
                
                $('#butt2').click(function () 
                {
                     count();
                });
            });
           
            function count() {               
                 var tempId = <%=Model.Id%>; 
                 var url = '/Candidates/RemainingCount';
                 $.ajax({
                        type: "GET",
                        url: url,
                        data: { id : tempId}, 
                        dataType: "int",
                        success : function (count) {                            
                            SetCount(count);                            
                        }
                    });                                     
            }
            function SetCount(count)
            {         
                if(count != 0 && count != 9999)
                {                
                    document.getElementById("counting").value = count-1;                    
                     //document.getElementById("counting").value = count;
                    $('#candidatecontact').show();
                }
                else if(count != 9999)
                {
                    document.getElementById("counting").value = 0;
                    alert('Your Plan of Hot Resume Expired or You are yet to subscribe "Hot Resume"..To view Contact details Buy Hot Resumes..');                    
                }
                else
                {                    
                    $('#candidatecontact').show();
                }                               
            }      
                
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%var LoggedInOrganization = Request.QueryString["Id"]; %>
    <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
    <% bool isLoggedIn = LoggedInOrganization != null; %>
    
   <%-- <%if(isLoggedIn){ %>--%>
    <div class="candidate-select">
        <%: Html.CheckBox("Candidate" + Model.Id.ToString(), new { id = Model.Id, @class="candidate"  })%>
    </div>
    <h3 style="color:rgb(122, 162, 170); font-size:20px;"><%: Model.Name %> </h3>
    <h3 style="color:Orange;"><% if (!string.IsNullOrEmpty(Model.Position)) { %>
            (<%: Model.Position%>)
      <% } else { %>
            <%if(!string.IsNullOrEmpty(Model.FunctionId.ToString())) { %>
                (<%:Model.Function.Name %>)
            <%} %>
      <% } %>
   </h3>
   <%--<%} %>--%>


     <%if (Model.LocationId != null)
          { %>
          <span class="location">
          <%if (Model.GetLocation(Model.LocationId.Value).RegionId != null)
            { %>
                   (<%: Html.ActionLink(Model.GetLocation(Model.LocationId.Value).Region.Name, "matchcandidates", "Employer", new { where = Model.GetLocation(Model.LocationId.Value).Region.Name }, new { @class = "location" })%>)
            <%} %>
            <% if (Model.GetLocation(Model.LocationId.Value).CityId != null)
               {%>
                   (<%: Html.ActionLink(Model.GetLocation(Model.LocationId.Value).City.Name, "matchcandidates", "Employer", new { where = Model.GetLocation(Model.LocationId.Value).City.Name }, new { @class = "location" })%>)
            <% } %>
            <% if (Model.GetLocation(Model.LocationId.Value).CountryId != null)
               {%>
                   (<%: Html.ActionLink(Model.GetLocation(Model.LocationId.Value).Country.Name, "matchcandidates", "Employer", new { where = Model.GetLocation(Model.LocationId.Value).Country.Name }, new { @class = "location" })%>)
            <% } %>
             </span>
            <% } %>
      
  <%--  <div class="fourth-line">
       <% foreach (Dial4Jobz.Models.CandidateSkill cs in Model.CandidateSkills){ %>
           <%: Html.ActionLink(cs.Skill.Name, "Skill", "candidates", new { id = cs.Skill.Id }, new { @class = "skill" })%>
       <%} %>
    </div><br />--%>
       
   
    <div class="candidate-desc">
        <h3>Candidate Details</h3>            <%--<%:Html.ActionLink("Delete Current Candidate", "Delete", new { controller = "Candidates" })%>--%>
        
        <p>
             <%if (Model.IsPhoneVerified == true && Model.IsMailVerified == true)
            { %>
            MobileNumber:   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span> | Email:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span>
        <%}
           else if (Model.IsPhoneVerified == false || Model.IsMailVerified == false)
           { %>
            MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
        <%}
           else if (Model.IsPhoneVerified == true || Model.IsMailVerified == false)
           {%>
            MobileNumber:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /> <span class="green">Verified</span> | Email: <span class="red">Pending Verification</span>
        <%}
           else if (Model.IsPhoneVerified == false || Model.IsMailVerified == true)
           { %>
            MobileNumber: <span class="red">Pending Verification</span> | Email:  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /> <span class="green">Verified</span>
        <%} else { %>
            MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
         <%} %>
        </p>
        
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
       
       
                
        <p>
        <strong>Current Location:</strong>

         <%if (Model.LocationId != null)
          { %>

          <%if (Model.GetLocation(Model.LocationId.Value).RegionId != null)
            { %>
                   <%:Model.GetLocation(Model.LocationId.Value).Region.Name%><%:","%>
            <% } %>
            <% if (Model.GetLocation(Model.LocationId.Value).CityId != null)
               {%>
                   <%:Model.GetLocation(Model.LocationId.Value).City.Name%><%:","%>
            <% } %>
            <% if (Model.GetLocation(Model.LocationId.Value).CountryId != null)
               {%>
                   <%:Model.GetLocation(Model.LocationId.Value).Country.Name%>
            <% } %>
            <% } %>
        </p>


    
        <p>
        <strong>Preferred Location:</strong>
        <%if (Model.CandidatePreferredLocations.Count() > 0)
          { %>

            <% List<string> countryIds = new List<string>();
               List<string> stateIds = new List<string>();
               List<string> cityIds = new List<string>();

               foreach (Dial4Jobz.Models.CandidatePreferredLocation cl in Model.CandidatePreferredLocations.OrderBy(c => c.Location.CountryId))
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
          else
          { %>
            <%:"Any"%><%} %>
        </p>
             
          
        <p>  
            <strong>Total Experience:</strong>
            <% if ((!Model.TotalExperience.HasValue || Model.TotalExperience == 0) && (!Model.MaxExperience.HasValue || Model.MaxExperience == 0)){ %>
                <%:Model.TotalExperience %>
            <% } else if (!Model.TotalExperience.HasValue || Model.TotalExperience == 0) { %>
                    Up to <%: Math.Ceiling(Model.TotalExperience.Value / 31104000.0)%> Years 

            <% } else if (!Model.TotalExperience.HasValue || Model.TotalExperience == 0) { %>
                    <%: Math.Ceiling(Model.TotalExperience.Value / 33782400.0) %> + Years
            <% } else {  %>
                    <%:Model.TotalExperience.Value/31104000 %> Years
                    <%:((Model.TotalExperience.Value-(Model.TotalExperience.Value/31104000) * 31536000))/2678400 %> Months
                    <%--   <%: Math.Ceiling(Model.TotalExperience.Value / 33782400.0) %> Years --%>
                    <%-- <%: Math.Ceiling(Model.TotalExperience.Value * 31536000)%> Months--%>
            <% } %>
           
        </p>

         <p>
            <strong>Present / Last Drawn Salary:</strong>
            <% if ((!Model.AnnualSalary.HasValue || Model.AnnualSalary == 0)) { %>
                <%:"Not Mentioned" %>
            <% } else {  %>
                <%: Convert.ToInt32(Model.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>  
            <%} %>
        </p>

        <p>
            <strong>Current Designation:</strong>
            <%if (Model.Position != null)
              { %>
                <%:Model.Position%>
            <% } else { %>
              <%:"Not Mentioned"%>
            <% } %>
        </p>

        <p>
            <strong>Function Area: </strong>
            <%if (Model.FunctionId.HasValue)
              { %>
            <%:Model.Function.Name%>
            <%} else { %>
                <%:Model.FunctionId == null ? "Any" : Model.Function.Name%>
            <%} %>
        </p>

         <p>
            <strong>Current Role:</strong>
           <%foreach (Dial4Jobz.Models.CandidatePreferredRole cr in Model.CandidatePreferredRoles)
             { %>
                <%:cr.Role.Name%>
           <%} %>
        </p>

        <p style="line-height:1.8;">
            <strong>Preferred Function:</strong>
            <%foreach (Dial4Jobz.Models.CandidatePreferredFunction cpf in Model.CandidatePreferredFunctions)
              { %>
                <%:cpf.Function.Name %>
            <%} %>
        </p>


        <p>
         <strong>Industry Type:</strong>
         <% if (Model.IndustryId.HasValue)
          { %>
         <%: Model.GetIndustry(Model.IndustryId.Value).Name%>
         <%} else { %>
           <%:Model.IndustryId==null ? "Any": Model.GetIndustry(Model.IndustryId.Value).Name %>
         <%} %>
        </p>

        <p>
            <strong>Key Skills:</strong>
                <% foreach (Dial4Jobz.Models.CandidateSkill cs in Model.CandidateSkills)
                   { %>
                    <%:cs.Skill.Name%>
                <%} %>
            
        </p>
              

        <p>
            <strong>Language :</strong>
            <% foreach (Dial4Jobz.Models.CandidateLanguage cla in Model.CandidateLanguages){ %>
                <%: cla.Language.Name %><%:"," %>
            <%} %>                    
        </p>

         <p style="line-height:1.8;">
            <strong>Qualification :</strong>
                <% foreach (Dial4Jobz.Models.CandidateQualification cq in Model.CandidateQualifications){ %>
                    <%: cq.Degree.Name %>
                      <% if (cq.SpecializationId.HasValue)
                        { %>
                        (<%: Model.GetSpecialization(cq.SpecializationId.Value).Name%>)
                        <%} else { %>
                            (<%:cq.SpecializationId==null ? "Any": Model.GetSpecialization(cq.SpecializationId.Value).Name %>)
                        <%} %>

                <%} %>
        </p>
            

        <p>
            <strong>DOB:</strong>
                <%:Model.DOB.HasValue ? Model.DOB.Value.ToShortDateString():String.Empty%>
        </p>

        <p>
              <strong>Gender :</strong>
              <%:Model.Gender==1 ? "Female" :"Male" %>
             
              <%--<%: Model.Gender == 0 ? "Male" : "Female" %>--%>
        </p>

        <p>
            <strong>Preferred Type:</strong>
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
            <strong>Timings for Parttime:</strong>
            <%:Model.PreferredTimeFrom + " AM"%><%:" to"%><%:Model.PreferredTimeTo+ " PM"%> </p>
        <%} %>

        <p>
            <strong>Marital Status:</strong>
           <% if (Model.MaritalId.HasValue)
              { %>
             <%: Model.GetMaritalStatus(Model.MaritalId.Value).Name%>
             <%} else { %>
               <%:Model.MaritalId==null ? "": Model.GetMaritalStatus(Model.MaritalId.Value).Name %>
             <%} %>
        </p>
        
         <p style="line-height:1.8;">
            <strong>Description: </strong>
            <p><%:Model.Description %></p>
        </p>

         <% if (new Dial4Jobz.Models.Repositories.VasRepository().CheckDPRValidity(Model.Id))
            {%>    
          <div class="job-desc">
          <h3>Contact Details</h3>
            <p>
                <strong>Candidate Name :</strong>
                <%: Model.Name%>
            </p>

            <p>
                <strong>Mobile Number :</strong>
                <%if (Model.IsPhoneVerified == false)
                  { %>
                <%: Model.ContactNumber%>   <a class="link-tick-frontpage" title="Mobile number verified">Verified</a>
                 
                <% }
                  else
                  { %>
                     <%: Model.ContactNumber%> <span class="red">Not Verified</span>
                <% } %>     
            </p>

            <p>
                <strong>Additional Number: </strong>
                  <%:Model.MobileNumber%>
            </p>

            <%--53, 53, 40, 40, 27, 27--%>
            
            <p>
                <strong>Email Id :</strong>
                 <%if (Model.IsMailVerified == true)
                   { %>
                <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                <a class="link-tick-frontpage" title="Email Id verified">Verified</a>
                                     
                <% }
                   else
                   { %>
                <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                    <span class="red">Not verified</span>
                <% } %>      
            </p>

            <p>
                <strong>Address :</strong>
                <%: Model.Address%><%:","%><%:Model.Pincode%>
            </p>

           <p>
               <strong>Current Company:</strong>
               <%:Model.PresentCompany%>
           </p>

          <p>
               <strong>Previous Company:</strong>
               <%:Model.PreviousCompany%>
          </p>

          <p>
                <strong>No of Companies Worked:</strong>
                <%:Model.NumberOfCompanies%>
          </p>

          
          <% if (!String.IsNullOrEmpty(Model.ResumeFileName))
             { %>
             
                <p>
                    <%: Html.ActionLink("Download Resume", "Download", new { fileName = Model.ResumeFileName })%>
                </p>

         <% } %>
          </div>
          
        <% }
            else
            { %>
        
      <button id="butt2" class="contactbutton">Click to show Contact Details</button>
        <%if (isLoggedIn == true)
          { %>
        <%--<% if (new Dial4Jobz.Models.Repositories.VasRepository().GetHORSSubscribed(LoggedInOrganization) ==true)
           {%>
                <%var remainingHorsCount = _vasRepository.GetRemainingCount(LoggedInOrganization);%>
                <% ViewData["ResumesCount"] = remainingHorsCount; %>
                <%:remainingHorsCount%>
                Contact Details you can View under present HORS plan!!!
                <%:Html.Hidden("counting")%>
              
        <%}
           else
           { %>
        <div style="height: 30px;">
            To view Contact Details <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">
                <b>Login and Subscribe for Hot Resumes</b></a>
        </div>
        <% } %>--%>
     
       
          <div class="job-desc" id="candidatecontact">
            <p>
                <strong>Candidate Name :</strong>
                <%: Model.Name%>
            </p>

            <p>
                <strong>Mobile Number :</strong>
                <%if (Model.IsPhoneVerified == false)
                  { %>
                <%: Model.ContactNumber%>   <a class="link-tick-frontpage" title="Mobile number verified">Verified</a>
                 
                <% }
                  else
                  { %>
                     <%: Model.ContactNumber%> <span class="red">Not Verified</span>
                <% } %>     
            </p>

            <p>
                <strong>Additional Number: </strong>
                  <%:Model.MobileNumber%>
            </p>

            <%--53, 53, 40, 40, 27, 27--%>
            
            <p>
                <strong>Email Id :</strong>
                 <%if (Model.IsMailVerified == true)
                   { %>
                <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                <a class="link-tick-frontpage" title="Email Id verified">Verified</a>
                                     
                <% }
                   else
                   { %>
                <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                    <span class="red">Not verified</span>
                <% } %>      
            </p>
          
            <p>
                <strong>Address :</strong>
                <%: Model.Address%><%:","%><%:Model.Pincode%>
            </p>

           <p>
               <strong>Current Company:</strong>
               <%:Model.PresentCompany%>
           </p>

          <p>
               <strong>Previous Company:</strong>
               <%:Model.PreviousCompany%>
          </p>

          <p>
                <strong>No of Companies Worked:</strong>
                <%:Model.NumberOfCompanies%>
          </p>

          
          <% if (!String.IsNullOrEmpty(Model.ResumeFileName))
             { %>
             
                <p>
                    <%: Html.ActionLink("Download Resume", "Download", new { fileName = Model.ResumeFileName })%>
                </p>

         <% } %>
          </div>
           <%}
          else
          { %>
           <div style="height:30px;">
               <%-- To view Contact Details <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"><b>Login</b></a>--%>
                To view Contact Details <%:Html.ActionLink("Subscribe For Hot Resumes", "Index", "EmployerVas", new { @id = "#HotResumes" })%>
            </div>
            <%}
            }%>
            <br />
            <br />
           <%--  <% if(isLoggedIn){ %>
             <%var ActiveEmployers = _vasRepository.GetHORSSubscribed(LoggedInOrganization); %>--%>
                <input type="hidden" value="false" name="sendmethod" id="sendmethod" />
                <input type="hidden"   name="candidateid" id="candidateid" />
                <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0,<%:Model.Id %>);return false;" title="Send SMS">Send SMS</a>
               <%--  <%if (ActiveEmployers == true)
                   { %>--%>
                <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1,<%:Model.Id %>);return false;" title="Send Email">Send Email</a>
                <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2,<%:Model.Id %>);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>
               <%-- <%} %>

            <% } %>--%>
        </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStartedEmployer"); %> 
    <% Html.RenderPartial("Side/Video"); %> 

</asp:Content>

