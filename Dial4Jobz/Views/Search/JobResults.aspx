<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JobResults
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
  <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<%--<%: Html.ActionLink("Back To Search", "JobSearch", "Search", null, new { title = "Back To Search" })%>--%>
<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
        $(".refine-button").click();
    }
</script>

<% if (Model.Count() > 0)
   { %>
   <% int currentPage = Convert.ToInt32(ViewData["PageIndex"].ToString());
      int Recordcount = Convert.ToInt32(ViewData["RecordCount"].ToString());
      double dblPageCount = (double)((decimal)Recordcount / decimal.Parse("15"));
      int pageCount = (int)Math.Ceiling(dblPageCount);
       %>
        <span style="color:#324B81; font-size:14px;">Number of vacancies for your search is <b><%= Recordcount.ToString()%></b> in <b><%= pageCount%></b> pages</span>
        <p>
        <% Html.RenderPartial("MatchingJobs", Model); %> 
        </p>


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
                            
                      
</div>


<% }
   else
   { %>
   <span style="color:#324B81; font-weight:bold; font-size:14px;">There are no vacancy presently for your search kindly modify your Search & try</span>
   <% } %>
 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">

 <style type="text/css">
        .section li
        {
            color: #324B81;
        }
    </style>

    <form method="post">
                <input type="hidden" name="PageNo" id="PageNo" />  
                <input type="hidden" name="HfWhat" id="HfWhat" value="<%= Html.Encode(ViewData["WhatView"])%>" />
                <input type="hidden" name="HfWhere" id="HfWhere" value="<%= Html.Encode(ViewData["WhereView"])%>" />                                
                <input type="hidden" name="HfLanguages" id="HfLanguages" value="<%= Html.Encode(ViewData["LanguageView"])%>" />
                <input type="hidden" name="HfMinSalary" id="HfMinSalary" value="<%= Html.Encode(ViewData["MinSalaryView"])%>" />                
                <input type="hidden" name="HfMaxSalary" id="HfMaxSalary" value="<%= Html.Encode(ViewData["MaxSalaryView"])%>" />
                <input type="hidden" name="HfOrganization" id="HfOrganization" value="<%= Html.Encode(ViewData["OrganizationView"])%>" />
                
                <input type="hidden" name="HfMinExperience" id="HfMinExperience" value="<%= Html.Encode(ViewData["MinExperienceView"])%>" />
                <input type="hidden" name="HfMaxExperience" id="HfMaxExperience" value="<%= Html.Encode(ViewData["MaxExperienceView"])%>" />
                <input type="hidden" name="HfFunction" id="HfFunction" value="<%= Html.Encode(ViewData["FunctionView"])%>" />
                <input type="hidden" name="HfRole" id="HfRole" value="<%= Html.Encode(ViewData["RoleView"])%>" />

                <input type="hidden" name="HfIndustries" id="HfIndustries" value="<%= Html.Encode(ViewData["IndustriesView"])%>" />
                <input type="hidden" name="HfTypeOfVacancy" id="HfTypeOfVacancy" value="<%= Html.Encode(ViewData["TypeOfVacancyView"])%>" /> 
                <input type="hidden" name="HfTypeOfShift" id="HfTypeOfShift" value="<%= Html.Encode(ViewData["TypeOfShiftView"])%>" /> 
                <input type="hidden" name="HfFreshness" id="HfFreshness" value="<%= Html.Encode(ViewData["FreshnessView"])%>" />
  
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
            <% var topOrganizations = Model.GroupBy(q => (q.Organization != null ? q.Organization.Name : ""))
                                   .OrderByDescending(gp => gp.Count())
                                   .Take(5)
                                   .Select(g => g.Key).ToList(); %>
            <h5>Organization</h5>       

            <ul>
                <% foreach (var organization in topOrganizations)
                   { %>
                     <li>
                        <input type="checkbox" name="RefineOrganization" value="<%: organization.ToString() %>" />
                        <%: organization%>                            
                     </li>   
                <% } %>
            </ul>
            <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit2" />
        </div>


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

     <div class ="section">
 
        <% var topGender = Model.GroupBy(q => q.Male == true ? "Male" : "Female")
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

     <div class="section">
                <% var topFunctionIds = Model.Where(q => q.FunctionId.HasValue)
                                       .GroupBy(q => q.FunctionId.Value)
                                       .OrderByDescending(gp => gp.Count())
                                       .Take(5)
                                       .Select(g => g.Key).ToList(); %>
                <h5>Function</h5>       

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
                <input type="submit" title="Search jobs" class="refine-button" style="padding: 0 0 0 0; font-size:10px;" value="Refine" id="Submit4" />
            </div>
    </form>

    <% } %>
</asp:Content>
