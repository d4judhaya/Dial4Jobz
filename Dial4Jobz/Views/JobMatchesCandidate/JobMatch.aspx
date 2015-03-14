<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Job Matches

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

        $(document).ready(function () {
            var id = window.location.href.split('/').pop()
            //var candidateId = document.getElementById(id);
            document.cookie = "candidateId=" + id;
            //alert(session_value); 
        });


</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
     <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
     <% if (LoggedInConsultant != null)
        { %>
             <% Html.RenderPartial("NavConsultant"); %>
     <%} else { %>
        <% Html.RenderPartial("Nav"); %>
    <%} %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Job Matches</h2>

   <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>

    
    <br />
  
 <% Html.BeginForm("Send", "JobMatchesCandidate", FormMethod.Post, new { @id = "Send" });  %>


     <% if (Model.Count() > 0)
        {

            if (ViewData["CandidateIdView"] != null && ViewData["CandidateIdView"].ToString() != "")
            {   %>
                <a id="SelectAll" href="javascript://">Select All</a>&nbsp; /&nbsp;
                <a id="SelectNone" href="javascript://">Select None</a>
         <% }
        } %>

<div id="jobs">
    <ul id="job-list">     
        <% foreach (var item in Model) { %> 

           
            <li class="job-list-item">        
                  <%--  <% var candidateId = ViewData["CandidateId"]; %>
                     <%if (candidateId != null && item.Organization != null)
                     { %>    
                         <%var alertsLog = _vasRepository.GetSmsEmailLog((int)item.Organization.Id, candidateId.ToString(), (int)item.Id); %>
                           <%if (alertsLog != null)
                              { %>
                                <%if (alertsLog.SMSSent == "Yes" && alertsLog.MailSent == "Yes")
                                  { %>
                                    <h3 style="color:rgb(96, 175, 149);"> Sms and Mail successfully sent to the  <b><%: item.ContactPerson%></b> for <b><%: item.Organization.Name%></b> Company on <%:alertsLog.AlertSentDate.Value.ToString("dd-MM-yyyy") %></h3>
                                <% }
                                  else if (alertsLog.SMSSent == "Yes" && alertsLog.MailSent == "No")
                                  { %>
                                <h3 style="color:rgb(96, 175, 149);">Sms is successfully sent to the <%:item.Organization.Name%>  and Mail is not sent on <%:alertsLog.AlertSentDate.Value.ToString("dd-MM-yyyy") %>.</h3>
                                <% }
                                  else
                                  { %>
                                    <h3 style="color:rgb(96, 175, 149);"> Email is sent successfully to the <b><%:item.Organization.Name%></b> and Sms is not sent on <%:alertsLog.AlertSentDate.Value.ToString("dd-MM-yyyy") %>.</h3>
                                <%} %>
                            <%} %>
                      <%} %>--%>

                    <% int? teleConferenceCount = _vasRepository.GetTeleConferenceCount(item.OrganizationId); %>
                    <% int? GetCountForSS=_vasRepository.GetCountForSS(item.OrganizationId); %>
                   <% var SSActivated = _vasRepository.PlanActivatedForSS(item.OrganizationId); %>
                   <div class="first-line">
                     
                     <% if (ViewData["CandidateIdView"] != null && ViewData["CandidateIdView"].ToString() != "")
                          {   %>
                            <div class="job-select">                        
                                <%: Html.CheckBox("Job" + item.Id.ToString(), new { id = item.Id, @class = "Jobs" })%>      
                            </div>   
                       <% } %>
                       
                       <a href="/jobs/details?id=<%: Dial4Jobz.Models.Constants.EncryptString(item.Id.ToString()) %>" target="_blank" ><%: item.DisplayPosition %></a>

                     <span class="location">
                            <% List<string> citiesId = new List<string>();
                            foreach (Dial4Jobz.Models.JobLocation jl in item.JobLocations.OrderBy(c => c.Location.CountryId))
                            {
                                if (jl.Location.Country != null)
                                {
                                    if (jl.Location.CountryId != 152)
                                    { %>
                                          (<%: Html.ActionLink(jl.Location.Country.Name.ToString(), "Index", "Jobs", new { where = jl.Location.ToString() }, new { @class = "location" })%>)
                                     <% }
                                 }

                                 if (jl.Location.City != null)
                                 {
                                    if (!citiesId.Any(s => s.Contains(jl.Location.CityId.ToString())))
                                    {
                                        citiesId.Add(jl.Location.CityId.ToString());
                                    %>
                                    (<%: Html.ActionLink(jl.Location.City.Name.ToString(), "Index", "Jobs", new { where = jl.Location.ToString() }, new { @class = "location" })%>)
                                    <% 
                                      }
                                   }
                                   
                                if (jl.Location.Region != null)
                                 {
                                    if (!citiesId.Any(s => s.Contains(jl.Location.Region.ToString())))
                                    {
                                        citiesId.Add(jl.Location.RegionId.ToString());
                                    %>
                                    (<%: Html.ActionLink(jl.Location.Region.Name.ToString(), "Index", "Jobs", new { where = jl.Location.ToString() }, new { @class = "location" })%>)
                                    <% 
                                      }
                                   }

                              } %>
                     </span>
                   </div>


             <div class="job-details">
                <div class="second-line">
                    <span class="org-name">
                        <%var org = item.Organization; %>
                        <%Dial4Jobz.Models.Repositories.Repository repository = new Dial4Jobz.Models.Repositories.Repository(); %>

                        <%if (org != null)
                          { %>
                            <%: item.Organization.Name%>
                           
                            <%var planactivated = _vasRepository.PlanSubscribed(org.Id); %>
                        

                            <%if (planactivated == true && teleConferenceCount != null || teleConferenceCount > 0)
                              { %>
                                <a href=""><img src="<%=Url.Content("~/Content/Images/star for paidemployers.jpg")%>" width="15px" height="15px" alt="Featured Employer" title="Featured Employer" /></a> 
                                 <a href=""><img src="<%=Url.Content("~/Content/Images/teleconferenceStar.jpg")%>" width="15px" height="15px" alt="Combo Teleconference" title="Combo Teleconference" /></a> (<%:teleConferenceCount%>) 
                            <% }
                              else if (planactivated == true)
                              { %>
                                  <a href=""><img src="<%=Url.Content("~/Content/Images/star for paidemployers.jpg")%>" width="15px" height="15px" alt="Featured Employer" title="Featured Employer" /></a> 
                            <% } %>

                            <% if (SSActivated == true)
                              { %>
                                <a href=""><img src="<%=Url.Content("~/Content/Images/SS.png")%>" width="15px" height="10px" alt="Featured Employer" title="Featured Employer" /></a>
                                <%if (GetCountForSS != 0)
                                  { %>
                                    TC Done(<%:GetCountForSS%>) 
                                <%} %>
                            <%} else { %>
                                
                            <%:""%>

                        <%} %>
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
                        Preferred Role:
                        <%if (item.JobRoles.Count() > 0)
                          { %>
                        <% foreach (Dial4Jobz.Models.JobRole jr in item.JobRoles)
                           { %>
                                    <%: jr.Role.Name%><%:","%>
                        <%} %>
                        <% }
                          else
                          { %>
                            <%:"Any Role"%>
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
                        <% }
                           else
                           {  %>
                        <%: Convert.ToInt32(item.Budget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        to
                        <%: Convert.ToInt32(item.MaxBudget.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN"))%>
                        <%} %>

                        </span>
                    
                    </span>
                  
                </div>

                <div class="fourth-line">
                 <%if (item.CreatedDate != null)
                          { %>
                      Posted:
                    <%var date = DateTime.UtcNow; %>
                    <%var createdDate = Dial4Jobz.Helpers.DateHelper.GetFriendlyDate((DateTime)item.CreatedDate); %>
                        <%:createdDate%>
                    <%} else { %>
                        <%:""%>
                    <%} %>

                   <span class="right">
                       Qualification Required: 
                       <%if(item.JobRequiredQualifications.Count() > 0) { %>
                       <% foreach (Dial4Jobz.Models.JobRequiredQualification cq in item.JobRequiredQualifications){ %>
                                        <%: cq.Degree.Name %><%:"," %>
                                 <% } %>
                      <%} else { %>
                        <%:"Any" %>
                      <% }%>
                   </span>
                 
                </div>

                <div class="fourth-line">
                        Industry: 
                         <%if (item.JobPreferredIndustries.Count() > 0)
                           { %>
                        <% foreach (Dial4Jobz.Models.JobPreferredIndustry ji in item.JobPreferredIndustries)
                           { %>
                                <%: ji.Industry.Name%> <%:","%>
                        <%} %>
                        <%}
                           else
                           { %>
                                <%:"Any" %>
                        <%} %>
                   
                </div>
                
                <div class="fourth-line ellipsis">
                    <%if (item.Description != null)
                      { %>
                    <%if (item.Description.IndexOf("\n") > 0)
                      { %>
                        Description:
                         <%:item.Description.IndexOf("\n") > 0 ? String.Format("{0}...", item.Description.Substring(0, 20)) : item.Description%>
                    <%}
                      else
                      { %>
                    Description:
                    <%:item.Description.Length >20 ? String.Format("{0}...",item.Description.Substring(0,20)) : item.Description%>
                    <%} %>
                    <% } else { %>
                          Job Description: 
                          <%:""%>
                    <% } %>
                    <div class="right">
                         <%if (SSActivated ==true)
                          {%>
                             Mobile Number: <%: item.MobileNumber %> 
                         <% } %>
                    </div>
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
     <div style="clear:both;"></div>   
 </div>


 <% if (Model.Count() > 0) { %>
   <% int currentPage = Convert.ToInt32(ViewData["PageIndex"].ToString());
      int Recordcount = Convert.ToInt32(ViewData["RecordCount"].ToString());
      double dblPageCount = (double)((decimal)Recordcount / decimal.Parse("15"));
      int pageCount = (int)Math.Ceiling(dblPageCount);%>

      <span style="color:#324B81; font-size:14px;">Number of vacancies for your search is <b><%= Recordcount.ToString()%></b> in <b><%= pageCount%></b> pages</span>
        


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

    } else {
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

     <%if (ViewData["CandidateIdView"] != null && ViewData["CandidateIdView"].ToString() != "")
       {   %>
        <div class="editor-label">
                <%: Html.Label("Send Matching Result To")%>
        </div>    
        <div class="editor-field">
             <%: Html.CheckBox("Candidate", true)%>Candidates
             <%: Html.CheckBox("Organization")%>Organizations
        </div>
        
        <input type="hidden" name="HfCandidateId" id="HfCandidateId" value="<%= Html.Encode(ViewData["CandidateIdView"])%>" />
        <input id="SMS" type ="submit" value="Send SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Candidate.SendMatching(this, 0);return false;" />
        <input id="EMail" type ="submit" value ="Send Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Candidate.SendMatching(this, 1);return false;" />
        <input id="Both" type="submit" value ="Send Email and/or SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Candidate.SendMatching(this, 2);return false;" />
        <input id="TeleConference" type="submit" value ="Tele Conference" class ="btn" title ="Tele Conference" onclick ="javascript:Dial4Jobz.Candidate.SendMatching(this, 4);return false;" />

    <% }
   } else { %>
        <span style="color:#324B81; font-weight:bold; font-size:14px;">There are no vacancy Found</span>
   <% } %>    

   <% Html.EndForm(); %>

    <form id="PagingForm" method="post">
        <input type="hidden" name="PageNo" id="PageNo" />
        <input type="hidden" name="CandidateId" id="CandidateId" value="<%= Html.Encode(ViewData["CandidateIdView"])%>" />
    </form>

<div id="loading">
  <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
</div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %>
    <% Html.RenderPartial("Side/Video"); %> 
</asp:Content>
