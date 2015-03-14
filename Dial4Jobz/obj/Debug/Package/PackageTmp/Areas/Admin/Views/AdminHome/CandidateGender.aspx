<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.ReportSummary>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Gender
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["functionId"] == null || ViewData["functionId"] == "" )
    {
        ViewData["functionId"] = "0";
    }  %>
    <h2>Candidate Freshness</h2>
    
       
    <table width="100%">
        <tr>
                       
            <th>
                Functions
            </th>
            <th>
                Gender
            </th>
           
            <th>
                TotalCount
            </th>
            
        </tr>

    <%   var total = 0; 
        if (Model != null)
         {
           foreach (var item in Model)
             { %>
    
        <tr>
            
            
            <td>
                <%: item.Name%>
            </td>
            <td>
                <% 
               string gender = string.Empty ;
               if (item.Id.ToString() == "0")
               {%>
                 <span> Male</span>
               <%}
               else
               {%>
                <span>Female</span>
                <%} %>
            </td>
            <td align="center">
                <%: item.TotalCount%>
            </td>
           
        </tr>
    
    <%   total = total + item.TotalCount;
             }
         } %>
         <tr>
          <td width="90%" ><strong>Grand Total</strong></td>
          <td></td>
          <td  width="10%" align="center" ><strong><%: total %></strong></td>
           
         </tr>
    </table>

     </br>
    <p>
        <%: Html.ActionLink("Back to Candidate Summary", "CandidateSummary", new { reportType = Session["reportType"] })%>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Admin.js")%>" type="text/javascript"></script>
</asp:Content>

