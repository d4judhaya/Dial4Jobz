<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CandidateMatch
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <style type="text/css">
.buttondlist {
    border: 1px solid #3278BE;
    border-radius: 5px 5px 5px 5px;
    color: #3399FF;
    display: block;
    float: left;
    font-size: 13px;
    height: 14px;
    margin-right: 1px;
    padding: 10px;
    text-align: center;
    text-decoration: none;
    width: 20px;
}    
</style>

    <script type="text/javascript">
    function Pagination(pageNo) {
        $("#PageNo").val(pageNo);
        $("#PagingForm").submit();
    }    
</script>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Candidate Matches</h2>

   <% Html.BeginForm("Send", "CandidateMatches", FormMethod.Post, new { @id = "Send" }); %>
   <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
   <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
   
    <% if (Model.Count() > 0)
    {
        if (ViewData["JobIdView"] != null && ViewData["JobIdView"].ToString() != "")
        {   %>
            <a id="SelectAll" href="javascript://">Select All</a>&nbsp; /&nbsp;
            <a id="SelectNone" href="javascript://">Select None</a>
     <% }
    } %>

    <%if (Model.Count() > 0) { %>
    <p>
        <div id="candidates">
        <ul id="candidate-list">
        <% foreach (var item in Model)
           { %>
   
            <li class="candidate-list-item">
            <% if (ViewData["JobIdView"] != null && ViewData["JobIdView"].ToString() != "")
               { %>
              
                  <% } %>
                  <div class="first-line">     
                  
                    <div class="candidate-select">
                           <%-- <%: Html.CheckBox("Candidate" + item.Id.ToString(), new { id = item.Id })%>   --%>
                           <%: Html.CheckBox("Candidate" + item.Id.ToString(), new { id = item.Id, @class="candidate"  })%>   
                    </div>           
                        <a href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() %>/employer/candidates/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" ><%: item.DiplayCandidate %></a>

                   <span class="location">
                                    <%if (item.LocationId != null)
                                      { %>
                                            <% if (item.GetLocation(item.LocationId.Value).CityId != null)
                                               {%>
                                                   (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).City.Name, "Location", "Employer", new { id = item.LocationId }, new { @class = "location" })%>)
                                            <% } %>

                                            <% if (item.GetLocation(item.LocationId.Value).RegionId != null)
                                               {%>
                                                   (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).Region.Name, "Location", "Employer", new { id = item.LocationId }, new { @class = "location" })%>)
                                            <% } %>

                                            <% if (item.GetLocation(item.LocationId.Value).CountryId != null)
                                               {%>
                                                (<%: Html.ActionLink(item.GetLocation(item.LocationId.Value).Country.Name, "Location", "Employer", new { id = item.LocationId }, new { @class = "location" })%>)
                                            <% } %>
                                   <% } %>
                                  
                   </span>   
                  </div>

                  <div class="job-details">
                         <div class="second-line">
                          <span class="org-name">
                              <% if (!string.IsNullOrEmpty(item.Position))
                                 { %>
                              Position:
                              <%: item.Position %>
                                  <% var checkDPR = _vasRepository.CheckDPRValidity(item.Id); %>
                                  <% var SIActivated = _vasRepository.PlanActivatedForSI(item.Id); %>
                                  <% var PlanActivated = _vasRepository.PlanActivatedForCandidate(item.Id); %>
                                   <% if (checkDPR == true)
                                     { %>
                                        <a href=""><img src="<%=Url.Content("~/Content/Images/green_star.png")%>" width="21px" height="20px" alt="Paid-Display Resume" title="Display Resume" /></a> 
                                   <%} %>
                                   <% if (SIActivated == true)
                                     { %>
                                        <a href=""><img src="<%=Url.Content("~/Content/Images/SI-candidate.png")%>" width="15px" height="10px" alt="Spot Interview" title="Spot Interview" /></a>
                                   <%} %>

                                   <% if (PlanActivated == true)
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
                                       <a class="salarybutton" style="background-color:Blue; font-weight:bold; border: 1px solid #BBE1EF;border-radius: 5px;display: inline-block; float: none;margin: 0px 5px 4px 0px; padding:3px 23px; color:White;"  href="<%: Url.Action("MatchCandidates", "Employer", new {annualsalary=item.AnnualSalary}) %>"><%:convertsalary%></a>
                                   <% } %>
                              
                              </span></span>
                    </div></div>



                 <div class="candidate-details"> 
                   <div class="fourth-line">
                        <% if(item.FunctionId.HasValue) {  %>
                   
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
                
                        <div class="fourth-line">
                       <%if (item.UpdatedDate != null)
                         { %>
                            <%var updatedDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.UpdatedDate); %>
                            Modified: <%:updatedDate %>
                       <%}
                         else
                         { %>
                            <%var createdDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.CreatedDate); %>
                            Submitted: <%:createdDate%>
                       <%} %>

                        <span class="right">Qualification: <% foreach (Dial4Jobz.Models.CandidateQualification cq in item.CandidateQualifications){ %>
                                        <%: cq.Degree.Name %><%:"," %>
                                 <%} %>
                        </span>
                    </div>


               <div class="fourth-line">
                     
                     <% if (new Dial4Jobz.Models.Repositories.VasRepository().CheckDPRValidity(item.Id))
                       {%>  
                         MobileNumber: <%: item.ContactNumber%> | Email: <%: item.Email%>
                    <% } %>

                     <span class="right">
                        Preferred Location:
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
                      else
                      { %>
                        <%:"Any"%><%} %>
                    </span>  
                                      
                    </span> 
                     </div>

                     <div class="fourth-line">
                        <%if (item.IsPhoneVerified == true && item.IsMailVerified==true)
                          { %>
                            MobileNumber:   <img src="../../../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span> | Email:  <img src="../../../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /><span class="green">Verified</span>
                        <%} else if(item.IsPhoneVerified==false || item.IsMailVerified==false) { %>
                            MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
                        <%} else if(item.IsPhoneVerified==true || item.IsMailVerified==false) {%>
                          MobileNumber:  <img src="../../../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /> <span class="green">Verified</span> | Email: <span class="red">Pending Verification</span>
                        <%} else if(item.IsPhoneVerified==false || item.IsMailVerified==true) { %>
                            MobileNumber: <span class="red">Pending Verification</span> | Email:  <img src="../../../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" /> <span class="green">Verified</span>
                        <%} else { %>
                            MobileNumber: <span class="red">Pending Verification</span> | Email: <span class="red">Pending Verification</span>
                        <%} %>

                 
                    </div>

                     <div class="fourth-line">
                              
                              <%if (item.Description != null)
                              { %>
                            Description: <%:item.Description.IndexOf("\n") > 0 ? String.Format("{0}...", item.Description.Substring(0, item.Description.IndexOf("\n"))) : item.Description%>
                            <% } else{ %>
                                 Description: <%:""%>
                            <% } %>

                            
                           </div>

                    
                    <div class="fourth-line">
                    <% foreach (Dial4Jobz.Models.CandidateSkill cs in item.CandidateSkills){ %>
                       <%: Html.ActionLink(cs.Skill.Name, "Skill", "Employer", new { id = cs.Skill.Id }, new { @class = "skill" })%>
                    <%} %>
                </div>    
            </div>
            </li>
        <%} %>
 </ul>      
</div>
    </p>        
    <% } %>
    

   <% if (Model.Count() > 0)
   { %>
   <% int currentPage = Convert.ToInt32(ViewData["PageIndex"].ToString());
      int Recordcount = Convert.ToInt32(ViewData["RecordCount"].ToString());
      double dblPageCount = (double)((decimal)Recordcount / decimal.Parse("15"));
      int pageCount = (int)Math.Ceiling(dblPageCount);
       %>

       <%-- <span style="color:#324B81; font-size:14px;">Number of vacancies for your search is <b><%= Recordcount.ToString()%></b> in <b><%= pageCount%></b> pages</span>--%>
        


<div style="text-align:center;">
                            <% //int currentPage = Convert.ToInt32(ViewData["PageIndex"].ToString());
    //int Recordcount = Convert.ToInt32(ViewData["RecordCount"].ToString());
    //double dblPageCount = (double)((decimal)Recordcount / decimal.Parse("15"));
    //int pageCount = (int)Math.Ceiling(dblPageCount);
    List<ListItem> pages = new List<ListItem>();
    if (pageCount > 0)
    {
        if (pageCount > 6)
        {
            string onclick = "";
            onclick = currentPage > 1 == true ? "onclick=" + "Pagination('1')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>1</a>
                            <% onclick = currentPage != 2 == true ? "onclick=" + "Pagination('2')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>2</a>
                            <% onclick = currentPage != 3 == true ? "onclick=" + "Pagination('3')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>3</a>
                            <% 
               
    //pages.Add(new ListItem("1", "1", currentPage > 1));
    //pages.Add(new ListItem("2", "2", currentPage != 2));
    //pages.Add(new ListItem("3", "3", currentPage != 3));


    if ((currentPage == 1) || (currentPage == 2) || (currentPage == 3) || (currentPage == 4) || (currentPage == (pageCount - 3)) || (currentPage == (pageCount - 2)) || (currentPage == (pageCount - 1)) || (currentPage == pageCount))
    {
        if ((currentPage == 3))
        {
            //pages.Add(new ListItem((currentPage + 1).ToString(), (currentPage + 1).ToString(), (currentPage + 1) != currentPage));

            onclick = (currentPage + 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage + 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage + 1).ToString()%></a>
                            <%
    }
        if ((currentPage == 4))
        {
            //pages.Add(new ListItem((currentPage).ToString(), (currentPage).ToString(), (currentPage) != currentPage));
            //pages.Add(new ListItem((currentPage + 1).ToString(), (currentPage + 1).ToString(), (currentPage + 1) != currentPage));

            onclick = (currentPage) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage).ToString()%></a>
                            <%
    onclick = (currentPage + 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage + 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage + 1).ToString()%></a>
                            <%
    }

        double avg = ((pageCount - 3) + 3) / 2;
        onclick = "onclick=" + "Pagination('" + avg.ToString() + "')" + "";
        //pages.Add(new ListItem("....", avg.ToString(), true));
                   
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
                            <%

    if ((currentPage == (pageCount - 2)))
    {
        //pages.Add(new ListItem((currentPage - 1).ToString(), (currentPage - 1).ToString(), (currentPage - 1) != currentPage));
        onclick = (currentPage - 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage - 1).ToString()%></a>
                            <% 
    }
    if ((currentPage == (pageCount - 3)))
    {
        //pages.Add(new ListItem((currentPage - 1).ToString(), (currentPage - 1).ToString(), (currentPage - 1) != currentPage));
        onclick = (currentPage - 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage - 1).ToString()%></a>
                            <% 
                       
    //pages.Add(new ListItem((currentPage).ToString(), (currentPage).ToString(), (currentPage) != currentPage));

    onclick = (currentPage) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (currentPage).ToString()%></a>
                            <% 
    }

    }
    else
    {
        if (currentPage > 5)
        {
            double avgs = ((currentPage - 1) + 3) / 2;
            //pages.Add(new ListItem("....", avgs.ToString(), true));

            onclick = "onclick=" + "Pagination('" + avgs.ToString() + "')" + ""; 
                   
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
                            <%
                       
    }

        for (int j = currentPage - 1; j <= currentPage + 1; j++)
        {
            //pages.Add(new ListItem(j.ToString(), j.ToString(), j != currentPage));

            onclick = j != currentPage == true ? "onclick=" + "Pagination('" + j.ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= j.ToString()%></a>
                            <% 
    }

        if (currentPage < pageCount - 4)
        {
            double avge = ((currentPage + 1) + (pageCount - 2)) / 2;
            //pages.Add(new ListItem("....", avge.ToString(), true));

            onclick = "onclick=" + "Pagination('" + avge.ToString() + "')" + ""; 
                   
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
                            <%
    }


    }



    //pages.Add(new ListItem((pageCount - 2).ToString(), (pageCount - 2).ToString(), currentPage != (pageCount - 2)));
    onclick = currentPage != (pageCount - 2) == true ? "onclick=" + "Pagination('" + (pageCount - 2).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%=(pageCount - 2).ToString()%></a>
                            <%
               
    //pages.Add(new ListItem((pageCount - 1).ToString(), (pageCount - 1).ToString(), currentPage != (pageCount - 1)));
    onclick = currentPage != (pageCount - 1) == true ? "onclick=" + "Pagination('" + (pageCount - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= (pageCount - 1).ToString()%></a>
                            <%
               
    //pages.Add(new ListItem(pageCount.ToString(), pageCount.ToString(), currentPage < (pageCount)));
    onclick = currentPage < (pageCount) == true ? "onclick=" + "Pagination('" + pageCount.ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
                            %>
                            <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                                <%= pageCount.ToString()%></a>
                            <%

    }
        else
        {
            if (pageCount > 1)
                for (int i = 1; i <= pageCount; i++)
                {
                    if (i == currentPage)
                    {
                            %>
                            <a href="javascript:void(0)" class="buttondlist" style="background-color: #D6EBFF;
                                cursor: auto;">
                                <%=i.ToString()%></a>
                            <%--<label class="buttondlist" style="color: #3399FF; border-color: #3278BE; background-color: #EBF5FF;
                    margin-right: 0px; cursor: auto;">
                    <%=i.ToString() %>
                </label>--%>
                            <%
    }
                    else
                    {
                            %>
                            <a href="javascript:void(0)" class="buttondlist" onclick="return Pagination('<%=i.ToString() %>')">
                                <%=i.ToString()%></a>
                            <%
    } %>
                            <%
    //pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
}
        }

    }
                            %>
       <div style="clear:both;"></div>                        
                 
</div>

     <%if (ViewData["JobIdView"] != null && ViewData["JobIdView"].ToString() != "")
       {   %>
        
        <div class="editor-label">
                <%: Html.Label("Send Matching Result To")%>
        </div>    
        <div class="editor-field">
             <%: Html.CheckBox("SendToUser")%>Candidates
             <%: Html.CheckBox("SendToOrganization", true)%>Organizations
        </div>
        
        <input type="hidden" name="HfJobId" id="HfJobId" value="<%= Html.Encode(ViewData["JobIdView"])%>" />
        <%Dial4Jobz.Models.Job job = _repository.GetJob(Convert.ToInt32(ViewData["JobIdView"])); %>
        <% int? teleConferenceCount = _vasRepository.GetTeleConferenceCount(job.OrganizationId); %>
            <input id="SMS" type ="submit" value="Send SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidates(this, 0);return false;" />
            <input id="EMail" type ="submit" value ="Send Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidates(this, 1);return false;" />
            <input id="Both" type="submit" value ="Send Email and/or SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidates(this, 2);return false;" />
       <%-- <%if (teleConferenceCount != 0)
          { %>--%>
     <%--   <%if (teleConferenceCount != null)
          { %>--%>
              <input id="TeleConference" type="submit" value ="Assign Conference" class ="btn" title ="Assign Conference" onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidates(this, 4);return false;" />
       <%-- <% } %>--%>
        <%--<% } %>--%>

    <% }
   } else { %>
        <span style="color:#324B81; font-weight:bold; font-size:14px;">There are no Candidate Found</span>
   <% } %>    

   <% Html.EndForm(); %>

    <form id="PagingForm" method="post">
        <input type="hidden" name="PageNo" id="PageNo" />
        <input type="hidden" name="JobId" id="JobId" value="<%= Html.Encode(ViewData["JobIdView"])%>" />
    </form>

<div id="loading">
  <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
</div>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
