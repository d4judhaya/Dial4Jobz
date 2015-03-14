<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

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
<% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Candidate Matches</h2>

 <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
 <% bool isLoggedIn = LoggedInOrganization != null; %>


  <%-- <% Html.BeginForm("Send", "CandidateMatchesJob", FormMethod.Post, new { @id = "Send" }); %>--%>
  <% Html.BeginForm("Send", "Candidates", FormMethod.Post, new { @id = "Send" }); %>

   
   
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
             <% Html.RenderPartial("Candidates", Model, ViewData); %>
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
       { %>
        
        <div class="editor-label">
           <%: Html.Label("Send Matching Result To")%>
        </div>    
        <div class="editor-field">
             <%: Html.CheckBox("SendToUser")%>Candidates
             <%: Html.CheckBox("SendToOrganization", true)%>Organizations
        </div>
              

       <%-- <%var ActiveEmployers = _vasRepository.GetHORSSubscribed(LoggedInOrganization.Id); %>
            <input type="hidden" name="HfJobId" id="HfJobId" value="<%= Html.Encode(ViewData["JobIdView"])%>" />
            <input id="SMS" type ="submit" value="Send SMS" class ="btn" title ="Send SMS"  onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidatesForEmployer(this, 0);return false;" />
         <%if (ActiveEmployers == true)
           { %>
            <input id="EMail" type ="submit" value ="Send Email" class ="btn" title ="Send Email" onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidatesForEmployer(this, 1);return false;" />
            <input id="Both" type="submit" value ="Send Email and/or SMS" class ="btn" title ="Send Email and/or SMS" onclick ="javascript:Dial4Jobz.Candidate.JobMatchingCandidatesForEmployer(this, 2);return false;" />
        <%} %>--%>

    <% if (isLoggedIn == true)
       { %>
    <%var ActiveEmployers = _vasRepository.GetHORSSubscribed(LoggedInOrganization.Id); %>
    <input type="hidden" value="false" name="sendmethod" id="sendmethod" />
    <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(0);return false;" title="Send SMS">Send SMS</a>
     <%if (ActiveEmployers == true)
       { %>
    <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(1);return false;" title="Send Email">Send Email</a>
    <a class="btn popup" href="<%=Url.Content("~/Candidates/ContactCandidates")%>" onclick ="javascript:Dial4Jobz.Candidate.Sendmethod(2);return false;" title="Send Email and/or SMS">Send Email and/or SMS</a>
    <% } %>
    <%} %>

    <% }
   }
   else
   { %>
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
