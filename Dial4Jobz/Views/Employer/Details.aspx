<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Position %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Dial4Jobz.Models.Organization loggedInCandidates = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <% bool isLoggedIn = loggedInCandidates != null; %>
    
   <h1><%: Model.Name %></h1>

   <b><% if (!string.IsNullOrEmpty(Model.Position)) { %>
            <%: Model.Position%>
      <% } else { %>
        <%:string.IsNullOrEmpty(Model.Function.Name)%>
      <% } %>
    </b>
    <br />

    <span class="location">
     <%if (Model.LocationId != null)
          { %>
            <% if (Model.GetLocation(Model.LocationId.Value).CityId != null)
               {%>
                   (<%: Html.ActionLink(Model.GetLocation(Model.LocationId.Value).City.Name,"Location","Candidate",new {id=Model.LocationId},new{ @class="location"})%>)
            <% } %>
            <% if (Model.GetLocation(Model.LocationId.Value).CountryId != null)
               {%>
                   (<%: Html.ActionLink(Model.GetLocation(Model.LocationId.Value).Country.Name,"Location","Candidate",new {id=Model.LocationId},new{ @class="location"})%>)
            <% } %>
            <% } %>

       <%-- <% foreach(Dial4Jobz.Models.CandidatePreferredLocation cl in Model.CandidatePreferredLocations){  %>
                (<%: Html.ActionLink(cl.Location.ToString(), "Location", "candidate", new { id = cl.Location.Id }, new { @class = "location" })%>)
        <% } %>    --%>                                                       
       
    </span>
    <br />

              
    <div class="fourth-line">
       <% foreach (Dial4Jobz.Models.CandidateSkill cs in Model.CandidateSkills){ %>
           <%: Html.ActionLink(cs.Skill.Name, "Skill", "candidates", new { id = cs.Skill.Id }, new { @class = "skill" })%>
       <%} %>
    </div><br />

   
   
    <div class="candidate-desc">
        <h2>Candidate Details</h2>            <%--<%:Html.ActionLink("Delete Current Candidate", "Delete", new { controller = "Candidates" })%>--%>
        
         <p>
            <% if (Model.CreatedDate != null)
               { %>
                <strong>Date Posted:</strong>
                <%:Model.CreatedDate%>
            <% } %>
        </p>
        
        
            <% if (Model.UpdatedDate != null) { %>
            <p>
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
        <% foreach (Dial4Jobz.Models.CandidatePreferredLocation cl in Model.CandidatePreferredLocations) { %>
            <% if (cl.Location != null)
               { %>
                <% if (cl.Location.Country != null)
                   { %>
                    <%: cl.Location.Country.Name%><%:","%>
                <% } %>
               <%-- <% if (cl.Location.State != null) { %>
                    <%: cl.Location.State.Name %><%:","%>
                <% } %>--%>
                <% if (cl.Location.City != null)
                   { %>
                    <%: cl.Location.City.Name%><%:","%>
                <% } %>
                <% if (cl.Location.Region != null)
                   { %>
                    <%: cl.Location.Region.Name%><%:","%>
                <% } %>
            <% }
               else
               { %>
                <%:"Any"%><%} %>
        <% } %>
        </p>
             
          
        <p>  
            <strong>Total Experience:</strong>
            <% if ((!Model.TotalExperience.HasValue || Model.TotalExperience == 0) && (!Model.MaxExperience.HasValue || Model.MaxExperience == 0)){ %>
                
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
            <% } else {  %>
                <%: Convert.ToInt32(Model.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>  
            <%} %>
        </p>

        <p>
            <strong>Current Designation:</strong>
            <%:Model.Position %>
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

        <p>
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
            <strong>Preferred Time:</strong>
             <% if(Model.PreferredAll==true) %>
                <%:"Any" %>
             <% if(Model.PreferredContract==true)  %>
                <%:"Contract" %>
            <%if(Model.PreferredParttime==true) {  %>
                <strong>Timings:</strong>
                <%:Model.PreferredTimeFromMinutes%><%:","%><%:Model.PreferredTimeToMinutes%><%} %>
            <%if(Model.PreferredFulltime==true) %>
                <%:"Full Time" %>
            <%if(Model.PreferredWorkFromHome==true) %>
                <%:"Work from home" %>
        
        </p>

        <p>
            <strong>Marital Status:</strong>
           <% if (Model.MaritalId.HasValue)
              { %>
             <%: Model.GetMaritalStatus(Model.MaritalId.Value).Name%>
             <%} else { %>
               <%:Model.MaritalId==null ? "": Model.GetMaritalStatus(Model.MaritalId.Value).Name %>
             <%} %>
        </p>
        
        <p>
            <strong>Description: </strong>
            <p><%:Model.Description %></p>
        </p>
        

       <%-- <h2>Contact Details:</h2>--%>
        <%--<button id="butt1">Click to hide Contact Details</button>--%>
        <button id="butt2" class="contactbutton">Click to show Contact Details</button>

        <%if (isLoggedIn == true)
          { %>
            <input id = "counting" type = "text" style="width:20px;"/>
      
       <%-- <%} else { %>
           <div style="height:30px;">
                To view Contact Details <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"><b>Login</b></a>
            </div>
            <%} %>--%>
        
        <script type="text/javascript">        
            $(document).ready(function () {
              $("#candidatecontact").hide();              
              var sess = <%= Session["remainingcount"] %>;
              if(sess != "111")
              {
                document.getElementById("counting").value =  '<%= Session["remainingcount"] %>';
              }
              else
              {
                document.getElementById("counting").value = "0";
              }
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
                        dataType: "string",
                        success : function (count) {                            
                            SetCount(count);                            
                        }
                    });                                     
            }
            function SetCount(count)
            {                
                if(count != "111" && count != "999")
                {                
                    document.getElementById("counting").value = count;                    
                    $('#candidatecontact').show();
                }
                else if(count != "999")
                {
                    document.getElementById("counting").value = "0";
                    alert('Your free resume count has been finished..To view more buy Hot Resumes');                    
                }
                else
                {
                    document.getElementById("counting").value = "X";
                    $('#candidatecontact').show();
                }
                //$("contactbutton").prop('disabled', true);              
                 // $("contactbutton").prop('disabled', false);               
            }      
                
        </script>
      
           
          <div class="job-desc" id="candidatecontact">
            <p>
                <strong>Candidate Name :</strong>
                <%: Model.Name%>
            </p>

            <p>
                <strong>Mobile Number :</strong>
                <%: Model.ContactNumber%>  
                 <%if (Model.IsPhoneVerified == false) { %>
                   <a><img src="../../Content/Images/Tick.png" class="btn" /></a>
                   Verified
                <% } else { %>
                    <span class="red">Not Verified</span>
                <% } %>     
            </p>

            <p>
                <strong>Additional Number: </strong>
                  <%:Model.MobileNumber%>
            </p>


            <p>
                <strong>Email Id :</strong>
                <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                 <%if (Model.IsMailVerified == true) { %>
                    <a><img src="../../Content/Images/Tick.png" class="btn" /></a>
                   Verified
                <% } else { %>
                    <span class="red">Not verified</span>
                <% } %>      
            </p>

            <p>
                <strong>Address :</strong>
                <%: Model.Address%><%:"," %><%:Model.Pincode %>
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
                <%:Model.NumberOfCompanies %>
          </p>

          
          <% if (!String.IsNullOrEmpty(Model.ResumeFileName)) { %>
             
                <p>
                    <%: Html.ActionLink("Download Resume", "Download", new { fileName = Model.ResumeFileName }) %>
                </p>

         <% } %>
          </div>
           <%} else { %>
           <div style="height:30px;">
                To view Contact Details <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"><b>Login</b></a>
            </div>
            <%} %>

            <% if (isLoggedIn) { %>
                <p>
                   <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0);return false;" title="Send SMS">Send SMS</a>
                   <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1);return false;" title="Send Email">Send Email</a>
                   <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>

                </p>
            <% } %>
        </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStarted"); %> 
    <% Html.RenderPartial("Side/Video"); %> 
</asp:Content>

