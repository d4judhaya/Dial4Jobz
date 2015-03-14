<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>

<%@ Import Namespace="Dial4Jobz.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Position %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>

    <%--<script type="text/javascript">   
            $(document).ready(function () {
             $("#candidatecontact").hide();
                    document.getElementById("counting").value =  '<%= ViewData["ResumesCount"] %>';
                $("#butt2").click(function () 
                {
                     count();
                });
                

                $("input[name='ShortList']").click(function () 
                {
                    var shortList = $("input[name='ShortList']:checked").val();
                    if(shortList == "1")
                    {
                        $("#ShortListText").show();
                        $("#ShortListText").focus();
                    }
                    else
                    {
                        $("#ShortListText").hide();
                    }
                });
                $("input[name='Selected']").click(function () 
                {
                    var selected = $("input[name='Selected']:checked").val();
                    if(selected == "1")
                    {
                        $("#SelectedText").show();
                        $("#SelectedText").focus();
                    }
                    else
                    {
                        $("#SelectedText").hide();
                    }
                });

                $("input[name='Issues']").click(function () {
                    var issues = $("input[name='Issues']:checked").val();
                    if(issues == "0")
                    {
                    $("#IssuesText").show();
                    $("#IssuesText").focus();
                    }
                    else{
                    $("#IssuesText").hide();
                    }
                });
});

            function validation(){
                
                var shortList = $("input[name='ShortList']:checked").val();
                var selected = $("input[name='Selected']:checked").val();
                var issues = $("input[name='Issues']:checked").val();

                if($("#ShortListText").val() == "" && shortList == "1" ){
                alert("Please Enter the reason");
                $("#ShortListText").focus();
                }

            

                if($("#SelectedText").val() == "" && selected == "1")
                {
                alert("Please enter the reason");
                $("#SelectedText").focus();
                }

            
               if($("#IssuesText").val() == "" && issues == "0")
                {
                alert("Please Enter the reason");
                $("#IssuesText").focus();
                }
                return true;
            }
           
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
            
                var planCount= $("#PlanCount").val();
                planCount=planCount - 10;
                count=100;
                planCount=100;
                if (planCount == count)
                {
                    $("#ShortListText").hide();
                    $("#SelectedText").hide();
                    $("#IssuesText").hide();

                    $('#popup_Plancount').fadeIn("slow");
                    $("#Send").css({
                    "opacity": "0.3"
                    });
                    Dial4Jobz.Candidate.SendFeedbackWatermarks();
                }

                if(count != 0 && count != 9999)
                {                
                    document.getElementById("counting").value = count-1;                    
                     //document.getElementById("counting").value = count;
                    $('#candidatecontact').show();
                }
                else if(count != 9999)
                {
                    document.getElementById("counting").value = 0;
                    alert('Your Plan of Hot Resume Expired or You are yet to subscribe "Hot Resumes"..To view Contact details Buy Hot Resumes..');                    
                }
                else
                {                    
                    $('#candidatecontact').show();
                }   
                
            }    
    </script>--%>
    <%--<style type="text/css">
        p.PopupPlan
        {
            padding-left: 14px;
            padding-right: 14px;
        }
        
        div#popup_Plancount
        {
            display: block;
            position: fixed;
            width: 400px;
            top: 86px;
            left: 300px;
        }
        a.feedbackbtn
        {
            font-size: 14px;
            border: 1px solid #BBE1EF;
            border-radius: 5px;
            font-weight: bold;
            padding: 3px 8px;
            background: #3EB3F3;
            color: #fff;
            border: solid 1px #087AB8;
            border-color: #087AB8 #054B72 #054B72 #087AB8;
            cursor: pointer;
            text-decoration: none;
        }
    </style>--%>

    <%--By vignesh--%>
    <script type="text/javascript">   
            $(document).ready(function () {
             $("#candidatecontact").hide();
                    document.getElementById("counting").value =  '<%= ViewData["ResumesCount"] %>';
                $("#butt2").click(function () 
                {
                     count();
                });
                

                $("input[name='ShortList']").click(function () 
                {
                    var shortList = $("input[name='ShortList']:checked").val();
                    if(shortList == "1")
                    {
                        $("#ShortListText").show();
                        $("#ShortListText").focus();
                    }
                    else
                    {
                        $("#ShortListText").hide();
                    }
                });
                $("input[name='Selected']").click(function () 
                {
                    var selected = $("input[name='Selected']:checked").val();
                    if(selected == "1")
                    {
                        $("#SelectedText").show();
                        $("#SelectedText").focus();
                    }
                    else
                    {
                        $("#SelectedText").hide();
                    }
                });

                $("input[name='Issues']").click(function () {
                    var issues = $("input[name='Issues']:checked").val();
                    if(issues == "0")
                    {
                    $("#IssuesText").show();
                    $("#IssuesText").focus();
                    }
                    else{
                    $("#IssuesText").hide();
                    }
                });
});

            function validation(){
                
                var shortList = $("input[name='ShortList']:checked").val();
                var selected = $("input[name='Selected']:checked").val();
                var issues = $("input[name='Issues']:checked").val();

                if($("#ShortListText").val() == "" && shortList == "1" ){
                alert("Please Enter the reason");
                $("#ShortListText").focus();
                }

            

                if($("#SelectedText").val() == "" && selected == "1")
                {
                alert("Please enter the reason");
                $("#SelectedText").focus();
                }

            
               if($("#IssuesText").val() == "" && issues == "0")
                {
                alert("Please Enter the reason");
                $("#IssuesText").focus();
                }
                return true;
            }
           
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
                var planCount= $(".PlanCount").val();
                planCount=planCount - 10;
                if (planCount == count)
                {
                    $("#ShortListText").hide();
                    $("#SelectedText").hide();
                    $("#IssuesText").hide();

                    $('#popup_Plancount').fadeIn("slow");
                    $("#Send").css({
                    "opacity": "0.3"
                    });
                    Dial4Jobz.Candidate.SendFeedbackWatermarks();
                }

                if(count != 0 && count != 9999)
                {                
                    document.getElementById("counting").value = count-1;                    
                     //document.getElementById("counting").value = count;
                    $('#candidatecontact').show();
                }
                else if(count != 9999)
                {
                    document.getElementById("counting").value = 0;
                    alert('Your Plan of Hot Resume Expired or You are yet to subscribe "Hot Resumes"..To view Contact details Buy Hot Resumes..');                    
                }
                else
                {                    
                    $('#candidatecontact').show();
                }   
                
            }    
    </script>
    <style type="text/css">
        p.PopupPlan
        {
            padding-left: 14px;
            padding-right: 14px;
        }
        
        div#popup_Plancount
        {
            display: block;
            position: fixed;
            width: 400px;
            top: 86px;
            left: 300px;
        }
        a.feedbackbtn
        {
            font-size: 14px;
            border: 1px solid #BBE1EF;
            border-radius: 5px;
            font-weight: bold;
            padding: 3px 8px;
            background: #3EB3F3;
            color: #fff;
            border: solid 1px #087AB8;
            border-color: #087AB8 #054B72 #054B72 #087AB8;
            cursor: pointer;
            text-decoration: none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <h3>SMS Candidates to call for interview - <a href="../../../../employer/employervas/index#smsPurchase">Buy Sms</a></h3>
    
    <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
    <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
    <% Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>
    
    <% bool isLoggedIn = LoggedInOrganization != null; %>
    <% bool isConsultLoggedIn = LoggedInConsultant != null; %>
    
      <div id="popup_Plancount" class="confirmpopup_box" style="display: none; width: 400px;
        top: 125px; left: 300px">
        <p class="PopupPlan">
            Thank you for viewing 10 more suitable candidates for your vacancy. Kindly give
            your comments for us to provide more value.
        </p>
        <p class="PopupPlan">
            1. Are you able to shortlist from this lot anyone?<br />
            <%: Html.RadioButton("ShortList", 0, true)%>
            Yes
            <%: Html.RadioButton("ShortList", 1)%>
            No
            <br />
            <%: Html.TextBox("ShortListText", "", new { @Style = "width: 300px;" })%>
            <br />
            <br />
            2. Have you selected anyone?<br />
            <%: Html.RadioButton("Selected", 0, true)%>
            Yes
            <%: Html.RadioButton("Selected", 1)%>
            No
            <br />
            <%: Html.TextBox("SelectedText", "", new { @Style = "width: 300px;" })%>
            <br />
            <br />
            3. Did you encounter any Issues?
            <br />
            <%: Html.RadioButton("Issues", 0)%>
            Yes
            <%: Html.RadioButton("Issues", 1, true)%>
            No
            <br />
            <%: Html.TextBox("IssuesText", "", new { @Style = "width: 300px;" })%>
            <br />
        </p>
        <center>
            <p class="PopupPlan" style="text-align: center;">
                <a href="" onclick="javascript:if(validation()){ if(Dial4Jobz.Candidate.SendFeedback()){ $('#popup_Plancount').fadeOut('slow');}}"
                    title="Submit" class="feedbackbtn">Submit</a>
                <%--    <a href="" onclick="javascript:$('#popup_Plancount').fadeOut('slow');$('#Send').css({'opacity': '1'});"
                          title="Cancel" class="feedbackbtn">Cancel</a>--%>
            </p>
        </center>
    </div>

    <%--by vignesh popup feedback--%>
         <div id="Div1" class="confirmpopup_box" style="display: none; width: 400px;
        top: 125px; left: 300px">
        <p class="PopupPlan">
            Thank you for viewing 10 more suitable candidates for your vacancy. Kindly give
            your comments for us to provide more value.
        </p>
        <p class="PopupPlan">
            1. Are you able to shortlist from this lot anyone?<br />
            <%: Html.RadioButton("ShortList", 0, true)%>
            Yes
            <%: Html.RadioButton("ShortList", 1)%>
            No
            <br />
            <%: Html.TextBox("ShortListText", "", new { @Style = "width: 300px;" })%>
            <br />
            <br />
            2. Have you selected anyone?<br />
            <%: Html.RadioButton("Selected", 0, true)%>
            Yes
            <%: Html.RadioButton("Selected", 1)%>
            No
            <br />
            <%: Html.TextBox("SelectedText", "", new { @Style = "width: 300px;" })%>
            <br />
            <br />
            3. Did you encounter any Issues?
            <br />
            <%: Html.RadioButton("Issues", 0)%>
            Yes
            <%: Html.RadioButton("Issues", 1, true)%>
            No
            <br />
            <%: Html.TextBox("IssuesText", "", new { @Style = "width: 300px;" })%>
            <br />
        </p>
        <center>
            <p class="PopupPlan" style="text-align: center;">
                <a href="" onclick="javascript:if(validation()){ if(Dial4Jobz.Candidate.SendFeedback()){ $('#popup_Plancount').fadeOut('slow');}}"
                    title="Submit" class="feedbackbtn">Submit</a>
                <%--    <a href="" onclick="javascript:$('#popup_Plancount').fadeOut('slow');$('#Send').css({'opacity': '1'});"
                          title="Cancel" class="feedbackbtn">Cancel</a>--%>
            </p>
        </center>
    </div>
    <%--End --%>

    <div id="details">
     <% if(isLoggedIn==true || isConsultLoggedIn==true) { %>
     <div class="candidate-select">
        <%: Html.CheckBox("Candidate" + Model.Id.ToString(), new { id = Model.Id, @class="candidate",@checked="checked" })%>
    </div>
    <% if (isConsultLoggedIn == true) { %>
        <h3 style="color: #0084bc;"><%: Model.Name %> </h3>(No Fee for Recruiting this Job-seeker Its Free!!!)
    <%} else { %>
        <h3 style="color: #0084bc;"><%: Model.Name %> </h3>
    <%} %>
    <h3 style="color:Orange;">
    <% if (!string.IsNullOrEmpty(Model.Position)) { %>
            (<%: Model.Position%>)
      <% } else { %>
            <%if(!string.IsNullOrEmpty(Model.FunctionId.ToString())) { %>
                (<%:Model.Function.Name %>)
            <% } %>
      <% } %>
   </h3>
   <% } %>


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
      
   
    <div class="candidate-desc">
        <h3>Candidate Details</h3>   
        
        <%if (Model.ResumeFileName != null)
            { %>
               <div class="black">(Resume is available)</div>
        <%} %>      
          
        <p>
            <%if (Model.IsPhoneVerified == true && Model.IsMailVerified == true) { %>
                 MobileNumber: <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span> | Email:
            <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span>
            <%} else if (Model.IsPhoneVerified == false && Model.IsMailVerified == false) { %>
                 MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
            <%} else if (Model.IsPhoneVerified == true && Model.IsMailVerified == false) { %>
                 MobileNumber: <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span> | Email: <span class="red">Pending Verification</span>
            <%} else if (Model.IsPhoneVerified == false && Model.IsMailVerified == true) { %>
                 MobileNumber: <span class="red">Pending Verification</span> | Email:
            <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span>
            <%} else if (Model.IsPhoneVerified == null && Model.IsMailVerified == null) { %>
                MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red"> Pending Verification</span>
            <%} else if (Model.IsPhoneVerified == true && Model.IsMailVerified == null) { %>
                MobileNumber: <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px"  alt="verified" /><span class="green">Verified</span> | Email: <span class="red">Pending  Verification</span>
            <% } else if (Model.IsMailVerified == true && Model.IsPhoneVerified == null){  %>
                Email: <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span> | MobileNumber: <span class="red">Pending Verification</span>
            <% } else if (Model.IsPhoneVerified == null && Model.IsMailVerified == null)  { %>
                 MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red"> Pending Verification</span>
            <%} else if (Model.IsPhoneVerified == false && Model.IsMailVerified == null) { %>
                 MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red"> Pending Verification</span>
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
            
        <p style="line-height:1.8;">
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
             
                 <%--<%: Html.ActionLink(cs.Skill.Name, "Skill", "candidates", new { id = cs.Skill.Id }, new { @class = "skill" })%>--%>
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
                <%:cpf.Function.Name %><%:","%>
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
            <strong>License Types:</strong>
            <%if (Model.TwoWheeler == true || Model.FourWheeler == true)
              { %>
                <%if (Model.TwoWheeler == true)
                  { %>
                    <%:"Two Wheeler"%>
                <%} else { %>
                    <%:"Four Wheeler" %>
                <%} %>
                <%foreach (Dial4Jobz.Models.CandidateLicenseType cl in Model.CandidateLicenseTypes)
                  { %>
                    <%:cl.LicenseType.Name %>
                <% } %>
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
                <%:Model.DOB.HasValue ? Model.DOB.Value.ToShortDateString():String.Empty%> <%:"," %> <strong>Gender :</strong>
              <%:Model.Gender==1 ? "Female" :"Male" %>
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

           <%-- <%:"," %>--%> <%if (Model.PreferredParttime == true) { %>
                  
            <%--<% if (Model.PreferredTimeFrom != null || Model.PreferredTimeTo != null)
               { %>
                    <%:Model.PreferredTimeFrom + " to"%>  <%:Model.PreferredTimeTo%> 
            <%} %>--%>

            <%if (Model.PreferredParttime == true) { %>
            <% if (Model.PreferredTimeFrom != null || Model.PreferredTimeTo != null)
               { %>
               <% if (Model.PreferredTimeFrom != "" || Model.PreferredTimeTo != "") {%>
                    <strong>Timings for Parttime:</strong>
                        <%:Model.PreferredTimeFrom + " to"%>  <%:Model.PreferredTimeTo%> 
                <%} %>
            <%} %>
        <%} %>
        <%} %>
        
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
        
        <p style="line-height:1.8;">
            <strong>Description: </strong>
            <%:Model.Description %>
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

                <%if (Model.IsPhoneVerified == true) { %>
                    <%: Model.ContactNumber%>  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span>
                <%} else if (Model.IsPhoneVerified == false) { %>
                     <%: Model.ContactNumber%> <span class="red">Pending Verification</span>
                <%} else if (Model.IsPhoneVerified == null) { %>
                   <%: Model.ContactNumber%> <span class="red">Pending Verification</span>
                <%} %>
                
             
            </p>

            <p>
                <strong>Additional Number: </strong>
                  <%:Model.MobileNumber%>
            </p>
                     
            
            <p>
                <strong>Email Id :</strong>
        
                 <%if (Model.IsMailVerified == true)
                   { %>
                       <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else if(Model.IsMailVerified==false) { %>
                    <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                    <span class="red">Pending Verification</span>
                <%} else if (Model.IsMailVerified == null) { %>
                  <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> <span class="red">Pending Verification</span>
                <%} %>
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
          
         <% } else { %>
        
      <button id="butt2" class="contactbutton">Click to View Contact Details</button>

      <% bool canAccess=false; %>
             <% User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault(); %>
             <%if (user != null)
               { %>
               <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository();
                       Permission adminPermission = new Permission();
                     
                     IEnumerable<AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                     string pageAccess = "";
                     string[] Page_Code = null;
                     foreach (var page in pageaccess)
                     {
                         adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                         if (string.IsNullOrEmpty(pageAccess))
                         {
                             pageAccess = adminPermission.Name + ",";
                         }
                         else
                         {
                             pageAccess = pageAccess + adminPermission.Name + ",";
                         }
                     }

                     if (!string.IsNullOrEmpty(pageAccess))
                     {
                         Page_Code = pageAccess.Split(',');
                     } %>
                    <%if (Page_Code.Contains("Admin Access Contact"))
                      { %>
                        <%canAccess = true; %>
                    <% } %>
             <%} %>

             <%if (canAccess == true)
               { %>
                 Dear <b><%:user.UserName %></b> you can view contact details..
                <%:Html.Hidden("counting")%>
             <%} %>
         <%if (isLoggedIn == true || canAccess == true || isConsultLoggedIn==true)
          { %>

          <%if (isLoggedIn == true)
            { %> 
            <% if (new Dial4Jobz.Models.Repositories.VasRepository().GetHORSSubscribed(LoggedInOrganization.Id) == true)
               {%>
                    <%var remainingHorsCount = _vasRepository.GetRemainingCount(LoggedInOrganization.Id);%>
                    <% var planResumeViewCount = _vasRepository.CheckForHorsPlanCount(LoggedInOrganization.Id); %>
                    <% ViewData["ResumesCount"] = remainingHorsCount; %>
                    <%--By vignesh--%>
                     <% ViewData["PlanCount"] = planResumeViewCount; %>
                     <input type="hidden" class="PlanCount" name="plancount" value="<%= ViewData["PlanCount"] %>" />
                    <%--End Vignesh--%>
                    <%:remainingHorsCount%>
                    Contact Details you can View under present HORS plan!!!
                    <%:Html.Hidden("counting")%>
                    
            <%}
               else
               { %>
                <div style="height: 30px;">
                    To View Contact Details <%:Html.ActionLink("Subscribe For Hot Resumes", "Index", "EmployerVas", new { @id = "#HotResumes" })%>
                </div>
            <% } %>
            <%} else if(isConsultLoggedIn == true) { %>
                 <% if (new Dial4Jobz.Models.Repositories.VasRepository().GetHorsSubscribedByConsultant(LoggedInConsultant.Id) == true)
                    {%>
                        <%var remainingHorsCount = _vasRepository.GetRemainingCount(LoggedInOrganization.Id);%>
                        <% var planResumeViewCount = _vasRepository.CheckForHorsPlanCount(LoggedInOrganization.Id); %>
                        <% ViewData["ResumesCount"] = remainingHorsCount; %>
                        <% ViewData["PlanCount"] = planResumeViewCount; %>
                        <%:remainingHorsCount%>
                        Contact Details you can View under present HORS plan!!!
                        <%:Html.Hidden("counting")%>
                        <input type="hidden" class="PlanCount" name="plancount" value="<%= ViewData["PlanCount"] %>" />
                        <%}
                    else
                    { %>
                <div style="height: 30px;">
                    To View Contact Details <%:Html.ActionLink("Subscribe For Hot Resumes", "Index", "EmployerVas", new { @id = "#HotResumes" })%>
                </div>
            <% } %>
            <%} %>
     
     
          <div class="job-desc" id="candidatecontact">
            <p>
                <strong>Candidate Name :</strong>
                <%: Model.Name%>
            </p>

            <p>
                <strong>Mobile Number :</strong>
                <%if (Model.IsPhoneVerified == true) { %>
                    <%: Model.ContactNumber%>  <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="verified" /><span class="green">Verified</span>
                <%} else if (Model.IsPhoneVerified == false) { %>
                     <%: Model.ContactNumber%> <span class="red">Pending Verification</span>
                <%} else if (Model.IsPhoneVerified == null) { %>
                   <%: Model.ContactNumber%> <span class="red">Pending Verification</span>
                <%} %>
            </p>

            <p>
                <strong>Additional Number: </strong>
                  <%:Model.MobileNumber%>
            </p>

            
            <p>
                <strong>Email Id :</strong>
                <%if (Model.IsMailVerified == true)
                   { %>
                       <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else if(Model.IsMailVerified==false) { %>
                    <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> 
                    <span class="red">Pending Verification</span>
                <%} else if (Model.IsMailVerified == null) { %>
                  <a href="mailto:<%: Model.Email %>"><%: Model.Email%></a> <span class="red">Pending Verification</span>
                <%} %>
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
                To view Contact Details <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"><b>Login</b></a>
           </div>
            <%}
             
            }%>




       <%-- <%if (LoggedInOrganization != null)
            { %>
            <%var ActiveEmployers = _vasRepository.GetHORSSubscribed(LoggedInOrganization.Id); %><br /><br />
                <input type="hidden" value="false" name="sendmethod" id="sendmethod" />
                <%: Html.Hidden("Candidate", Model.Id)%>   
                <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0);return false;" title="Send SMS">Send SMS</a>
            <%if (ActiveEmployers == true)
            { %>
            <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1);return false;" title="Send Email">Send Email</a>
            <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>
      
               
        <% } %>
            <%} %>--%>

        <%if (LoggedInOrganization != null || LoggedInConsultant!=null)
           		 { %>
                 <% bool ActiveEmployers = false; %>
                 <% bool ActiveConsultants = false; %>

                 <% if (LoggedInOrganization != null)
                    { %>
            	    	<%  ActiveEmployers = _vasRepository.GetHORSSubscribed(LoggedInOrganization.Id); %><br />
                 <%} else { %>
                        <%  ActiveConsultants = _vasRepository.GetHorsSubscribedByConsultant(LoggedInConsultant.Id); %>
                 <% } %>
                	<input type="hidden" value="false" name="sendmethod" id="sendmethod" />
                     <input type="hidden"   name="candidateid" id="candidateid" />
                	<%: Html.Hidden("Candidate", Model.Id)%>   
               
            		<%if (ActiveEmployers == true || ActiveConsultants == true)
            		{ %>
             		<a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0,<%:Model.Id %>);return false;" title="Send SMS">Send SMS</a>
            		<a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1,<%:Model.Id %>);return false;" title="Send Email">Send Email</a>
            		<a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2,<%:Model.Id %>);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>
        		<% } %>
           <%} %>
           
         </div>
        </div>
</asp:Content>

  


<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<%--<% Html.RenderPartial("NavEmployer"); %>--%>
 <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
 <% if (LoggedInOrganization != null)
    { %>
        <% Html.RenderPartial("NavEmployer"); %>
 <%} else if(LoggedInConsultant!=null) { %>
     <% Html.RenderPartial("NavConsultant"); %>
 <%} %>

    
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStartedEmployer"); %> 
    <% Html.RenderPartial("Side/Video"); %> 
  <%--  <% Html.RenderPartial("Side/_googleAdds"); %> --%>
</asp:Content>

