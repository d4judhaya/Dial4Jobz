<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CandidateResults 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>

<!-- for Captcha -->
<link rel="Stylesheet" href="../../Content/popup.css" />

<script src="<%=Url.Content("~/Scripts/popup.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/captcha.js")%>" type="text/javascript"></script>
<!--/ for Captcha -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Dial4Jobz.Models.Consultante loggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
<%if (loggedInConsultant != null)
  { %>
    <% Html.RenderPartial("NavConsultant"); %>
<%}
  else
  { %>
    <% Html.RenderPartial("NavEmployer"); %>
    <%} %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
    <tr>
    <td rowspan="3" colspan="3">
        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <%:Html.ActionLink("Hot Resumes", "Index", "EmployerVas")%> </h3 >
        <%} else { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>
    </tr>
    </table>



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
        $("input[type=radio]:checked").each(function () {
            this.checked = false;
        });
        $("input[type=checkbox]:checked").each(function () {
            this.checked = false;
        });
        $('select').each(function () {
            $(this).val("");
        });

        var g = parseInt($("#HfPageClickcount").val());
       
        if (g > 5) {
            openOffersDialog();
        }
        else {
            //g = (g + 1);
            $("#HfPageClickcount").val(g + 1);
            $(".refine-button").click();
        }
        
    }
</script>

<% if (Model.Count() > 0)
   { %>
   <% int currentPage = Convert.ToInt32(ViewData["PageIndex"].ToString());
      int Recordcount = Convert.ToInt32(ViewData["RecordCount"].ToString());
      double dblPageCount = (double)((decimal)Recordcount / decimal.Parse("15"));
      int pageCount = (int)Math.Ceiling(dblPageCount); %>

        <span style="color:#324B81; font-size:14px;">Number of Candidates for your search is <b><%= Recordcount.ToString()%></b> in <b><%= pageCount%></b> pages</span><br />
        
        <a style="background-color:Blue; font-weight:bold; border: 1px solid #BBE1EF;border-radius: 5px;display: inline-block; float: none;margin: 0px 5px 4px 0px; padding:3px 23px; color:White;" href="/employer/search/candidatesearch">Back To Search</a>
        <p>
            <% Html.RenderPartial("MatchingCandidates", Model);%> 
        </p>



<div style="text-align:center;">
                           
 <% List<ListItem> pages = new List<ListItem>();
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
               
    
    if ((currentPage == 1) || (currentPage == 2) || (currentPage == 3) || (currentPage == 4) || (currentPage == (pageCount - 3)) || (currentPage == (pageCount - 2)) || (currentPage == (pageCount - 1)) || (currentPage == pageCount))
    {
        if ((currentPage == 3))
        {
            
            onclick = (currentPage + 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage + 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
        <%= (currentPage + 1).ToString()%></a>
    <% } if ((currentPage == 4)) {
            
            onclick = (currentPage) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                 <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
            <%= (currentPage).ToString()%></a>
            <% onclick = (currentPage + 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage + 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
            <%= (currentPage + 1).ToString()%></a>
            <% } double avg = ((pageCount - 3) + 3) / 2;
                onclick = "onclick=" + "Pagination('" + avg.ToString() + "')" + "";%>
                <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
            <%if ((currentPage == (pageCount - 2)))
                {
                onclick = (currentPage - 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                <%= (currentPage - 1).ToString()%></a>
                <% } if ((currentPage == (pageCount - 3))) { onclick = (currentPage - 1) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                <%= (currentPage - 1).ToString()%></a>
                <% onclick = (currentPage) != currentPage == true ? "onclick=" + "Pagination('" + (currentPage).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; %>
                    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
                <%= (currentPage).ToString()%></a>
    <%  }
}
    else
    {
        if (currentPage > 5)
        {
            double avgs = ((currentPage - 1) + 3) / 2;
            
            onclick = "onclick=" + "Pagination('" + avgs.ToString() + "')" + ""; 
                   
    %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
    <%
                       
    }

        for (int j = currentPage - 1; j <= currentPage + 1; j++)
        {
            
            onclick = j != currentPage == true ? "onclick=" + "Pagination('" + j.ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'"; 
    %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
        <%= j.ToString()%></a>
    <% 
    }

        if (currentPage < pageCount - 4)
        {
            double avge = ((currentPage + 1) + (pageCount - 2)) / 2;
            
            onclick = "onclick=" + "Pagination('" + avge.ToString() + "')" + ""; 
                   
    %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>....</a>
    <%
    }

    }


    onclick = currentPage != (pageCount - 2) == true ? "onclick=" + "Pagination('" + (pageCount - 2).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
    %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
        <%=(pageCount - 2).ToString()%></a>
    <%
               
    onclick = currentPage != (pageCount - 1) == true ? "onclick=" + "Pagination('" + (pageCount - 1).ToString() + "')" + "" : "style='background-color: #D6EBFF; cursor:auto;'";
    %>
    <a href="javascript:void(0)" class="buttondlist" <%= onclick %>>
        <%= (pageCount - 1).ToString()%></a>
    <%
               
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
   
}
        }

    }
    %>
</div>


<% }
   else
   { %>
   <span style="color:#324B81; font-weight:bold; font-size:14px;">There are no Candidates presently for your search kindly modify your Search & try</span>
   <% } %>
 
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">

    <!-- Captcha -->
 
    <div id="overlay" class="overlay" style="display:none;"></div>

    <div id="boxpopup" class="box">
        <a onclick="closeOffersDialog('boxpopup');" class="boxclose"></a>
        <div id="content">
            <table>
                <tr>
                    <td>
                        Welcome To Dial4jobz<br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="text" id="txtCaptcha" readonly="readonly" style="background-image: url(1.jpg); text-align: center;
                            border: none; font-weight: bold; font-family: Modern; width:300px;" />
                        <input type="button" id="btnrefresh" value="Refresh" onclick="DrawCaptcha();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="text" id="txtInput" style="width:300px;" placeholder="Type the Number shown above"  />
                    </td>
                </tr>
                 <tr>
                    <td>
                        This is in order to prevent misuse of your account please enter the letters as shown in the picture in the text box provided.
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="Button1" type="submit" value="Submit" class="btn" onclick="ValidCaptcha();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <!--/ Captcha -->

<style type="text/css">
        .section li
        {
            color: #324B81;
        }
    </style>

     <form  method="post">
                <!-- this for captcha -->
                <% string clickcount = string.Empty;
                   if (ViewData["PageClickcountView"] != null && ViewData["PageClickcountView"].ToString() != "")
                   {
                       clickcount = ViewData["PageClickcountView"].ToString();
                   }
                   else
                   {
                       clickcount = "1";
                   }
                      %>
              <input type="hidden" id="HfPageClickcount" name="HfPageClickcount" value="<%= clickcount %>" />
                <!--/ this for captcha -->
                
                <input type="hidden" name="PageNo" id="PageNo" />  
                <input type="hidden" name="HfWhat" id="HfWhat" value="<%= Html.Encode(ViewData["WhatView"])%>" />
                <input type="hidden" name="HfLanguages" id="HfLanguages" value="<%= Html.Encode(ViewData["LanguageView"])%>" />
                <input type="hidden" name="HfMinSalary" id="HfMinSalary" value="<%= Html.Encode(ViewData["MinSalaryView"])%>" />                
                <input type="hidden" name="HfMaxSalary" id="HfMaxSalary" value="<%= Html.Encode(ViewData["MaxSalaryView"])%>" />
                <input type="hidden" name="HfMinExperience" id="HfMinExperience" value="<%= Html.Encode(ViewData["MinExperienceView"])%>" />
                <input type="hidden" name="HfMaxExperience" id="HfMaxExperience" value="<%= Html.Encode(ViewData["MaxExperienceView"])%>" />

                <input type="hidden" name="HfCurrentLocation" id="HfCurrentLocation" value="<%= Html.Encode(ViewData["CurrentLocationView"])%>" /> 
                <input type="hidden" name="HfAndOrLocations" id="HfAndOrLocations" value="<%= Html.Encode(ViewData["AndOrLocationsView"])%>" />               
                <input type="hidden" name="HfPrefLocation" id="HfPrefLocation" value="<%= Html.Encode(ViewData["PreferredLocationView"])%>" />
                <input type="hidden" name="HfFunction" id="HfFunction" value="<%= Html.Encode(ViewData["FunctionView"])%>" />
                <input type="hidden" name="HfRoles" id="HfRoles" value="<%= Html.Encode(ViewData["RolesView"])%>" />

                <input type="hidden" name="HfIndustries" id="HfIndustries" value="<%= Html.Encode(ViewData["IndustryView"])%>" />                
                <input type="hidden" name="HfBasicQual" id="HfBasicQual" value="<%= Html.Encode(ViewData["BasicQualView"])%>" />
                <input type="hidden" name="HfPostGraduation" id="HfPostGraduation" value="<%= Html.Encode(ViewData["PostGraduationView"])%>" />
                <input type="hidden" name="HfDoctrate" id="HfDoctrate" value="<%= Html.Encode(ViewData["DoctratesView"])%>" />

                <input type="hidden" name="HfBasicSpecialization" id="HfBasicSpecialization" value="<%= Html.Encode(ViewData["BasicSpecView"])%>" />
                <input type="hidden" name="HfPostSpecialization" id="HfPostSpecialization" value="<%= Html.Encode(ViewData["PostSpecView"])%>" />
                <input type="hidden" name="HfDoctrateSpecialization" id="HfDoctrateSpecialization" value="<%= Html.Encode(ViewData["DoctrateSpecView"])%>" />

                <input type="hidden" name="HfMinAge" id="HfMinAge" value="<%= Html.Encode(ViewData["MinAgeView"])%>" />                
                <input type="hidden" name="HfMaxAge" id="HfMaxAge" value="<%= Html.Encode(ViewData["MaxAgeView"])%>" />
                
                <input type="hidden" name="HfPosition" id="HfPosition" value="<%= Html.Encode(ViewData["PositionView"])%>" />                
                <input type="hidden" name="HfGender" id="HfGender" value="<%= Html.Encode(ViewData["GenderView"])%>" />  
                <input type="hidden" name="HfTypeOfVacancy" id="HfTypeOfVacancy" value="<%= Html.Encode(ViewData["TypeOfVacancyView"])%>" /> 
              <%--  <input type="hidden" name="HfAll" id="HfAll" value="<%= Html.Encode(ViewData["AllView"])%>" />  --%>
                

                
                              
                <% if (Model.Count() > 0)
     { %>
             
                   
 <% string RefineSalary = Html.Encode(ViewData["RefineSalaryView"]);
    if (string.IsNullOrEmpty(RefineSalary))
    { %>
    <div class="section">
        <h5>
            Annual Salary Range</h5>
        <ul>
            <li>
                <input type='radio' name='RefineSalary' value='1-100000' />
                0-1 Lakh </li>
            <li>
                <input type='radio' name='RefineSalary' value='100000-200000' />
                1-2 Lakhs </li>
            <li>
                <input type='radio' name='RefineSalary' value='200000-300000' />
                2-3 Lakhs </li>
            <li>
                <input type='radio' name='RefineSalary' value='300000-600000' />
                3-6 Lakhs </li>
            <li>
                <input type='radio' name='RefineSalary' value='600000-1000000' />
                6-10 Lakhs </li>
            <li>
                <input type='radio' name='RefineSalary' value='1000000-1000001' />
                > 10 Lakhs </li>
        </ul>
        <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit1" />
    </div>
    <% }
    else
    { %>
    <div class="section">
        <h5>
            Annual Salary Range</h5>
        <ul>
       <% if (RefineSalary == "1")
          { %> 
            <li>
                <input type='radio' name='RefineSalary' value='1-100000' checked="checked" />
                0-1 Lakh </li>
        <% }
          if (RefineSalary == "100000")
          { %> 
         <li>
                <input type='radio' name='RefineSalary' value='100000-200000' checked="checked" />
                1-2 Lakhs </li>
       <% }
          if (RefineSalary == "200000")
          { %> 
            <li>
                <input type='radio' name='RefineSalary' value='200000-300000' checked="checked" />
                2-3 Lakhs </li>
       <% }
          if (RefineSalary == "300000")
          { %> 
            <li>
                <input type='radio' name='RefineSalary' value='300000-600000' checked="checked" />
                3-6 Lakhs </li>
       <% }
          if (RefineSalary == "600000")
          { %> 
            <li>
                <input type='radio' name='RefineSalary' value='600000-1000000' checked="checked" />
                6-10 Lakhs </li>
       <% }
          if (RefineSalary == "1000000")
          { %> 
            <li>
                <input type='radio' name='RefineSalary' value='1000000-1000001' checked="checked" />
                > 10 Lakhs </li>
        <% } %>
        </ul>
         <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit5" />
     </div>

 <% } %>

 <div class="section">
        <h5>
            Experience</h5>
        <ul>
        <% string RefineExperience = Html.Encode(ViewData["RefineExperienceView"]);
           if (string.IsNullOrEmpty(RefineExperience))
           { %>
            <li>
                <input type='radio' name='RefineExp' value='0-3' />
                0-3 Years </li>
            <li>
                <input type='radio' name='RefineExp' value='3-6' />
                3-6 Years </li>
            <li>
                <input type='radio' name='RefineExp' value='6-9' />
                6-9 Years </li>
            <li>
                <input type='radio' name='RefineExp' value='9-12' />
                9-12 Years </li>
            <li>
                <input type='radio' name='RefineExp' value='12-13' />
                > 12 Years </li>    
        <% }
           else
           { %>

           <% if (RefineExperience == "0")
              { %>
              <li>
                <input type='radio' name='RefineExp' value='0-3' checked="checked" />
                0-3 Years </li>
           <% }
              if (RefineExperience == "3")
              { %>
            <li>
                <input type='radio' name='RefineExp' value='3-6' checked="checked" />
                3-6 Years </li>
           <% }
              if (RefineExperience == "6")
              { %>
            <li>
                <input type='radio' name='RefineExp' value='6-9' checked="checked" />
                6-9 Years </li>
           <% }
              if (RefineExperience == "9")
              { %>
            <li>
                <input type='radio' name='RefineExp' value='9-12' checked="checked" />
                9-12 Years </li>
           <% }
              if (RefineExperience == "12")
              { %>
            <li>
                <input type='radio' name='RefineExp' value='12-13' checked="checked" />
                > 12 Years </li>  
           <% }
           } %>
                   
        </ul>
        <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit3" />
    </div>

  <%--  <div class="section">
      
    </div>--%>

     <div class="section">
     <% var topFunctionIds = Model.Where(q => q.FunctionId.HasValue)
                                       .GroupBy(q => q.FunctionId.Value)
                                       .OrderByDescending(gp => gp.Count())
                                       .Take(5)
                                       .Select(g => g.Key).ToList(); %>       
        <h5>
            Function</h5>
        <ul>
            <% Dial4Jobz.Models.Repositories.Repository repository = new Dial4Jobz.Models.Repositories.Repository();  %>                    
            <% foreach (var functionId in topFunctionIds)
               { %>
                    <% var function = repository.GetFunction(functionId); %>                         
                        <li>
                        <input type="checkbox" name="RefineFunction" value="<%: functionId.ToString() %>" />
                        <%: function.Name%> 
                    </li>                
            <% } %>
        </ul>
        <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit2" />
    </div>

     <div class ="section">
        <% var topPosition = Model.GroupBy(q=>q.Position)
                           .OrderByDescending(gp=>gp.Count())
                           .Take (5)
                           .Select(g=>g.Key).ToList(); %>
                              
        <h5>Position</h5>
                
        <ul>
        <%foreach (var position in topPosition)
        { %>
            <li>
            <input type="checkbox" name="RefinePosition" value="<%: position %>" />
                        <%: position %> 
            </li>       
        <% } %>
        </ul>
        <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit4" />
    </div>

    <div class ="section">
 
        <% var topGender = Model.GroupBy(q => q.Gender == 0 ? "Male" : "Female")
                         .OrderByDescending(gp=>gp.Count())
                         .Take (5)
                         .Select(g=>g.Key).ToList();%>
                            
        <h5>Gender</h5>                        
        <ul>
        <%foreach (var gender in topGender){ %>
       <li>
            <input type='radio' name='RefineGender' value="<%: gender %>" />            
                        <%: gender %> 
            </li>   
        <% } %>
        </ul>
        <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit6" />
    </div>

    <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <% bool isLoggedIn = loggedInOrganization != null; %>

     <%if (isLoggedIn == true)
     { %>
   <div class="vastext">
      
       <h3>Get Matching candidates as soon as they Apply <%:Html.ActionLink("Resume Alert", "Index", "EmployerVas")%></h3><br />

       <h3>It is estimated that more than 40% of the resume have incorrect information..Do <%:Html.ActionLink("Reference Check", "Index", "EmployerVas")%> before selection.</h3><br />

       <h3>Search"part-time" or "work from home" candidates - <%:Html.ActionLink("Post Your Vacancy", "Add", "Jobs")%> & view Suitable Candidates"</h3>
   </div>

   <%} else { %>
       <div class="vastext">
       <h3>Get Matching candidates as soon as they Apply <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Resume Alert</a></h3><br />

       <h3>It is estimated that more than 40% of the resume have incorrect information..Do <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Reference Check</a> before selection.</h3><br />

       <h3>Search"part-time" or "work from home" candidates - <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Post Your Vacancy</a> & view Suitable Candidates"</h3>
       </div>
   <% }%>
    </form>

    <% } %>

</asp:Content>
