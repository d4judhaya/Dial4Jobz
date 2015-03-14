<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.GroupReport>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Country
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["functionId"] == null || ViewData["functionId"] == "" )
    {
        ViewData["functionId"] = "0";
    }  %>
    <h2>Candidate Country</h2>
          
    <table width="100%">
        <tr>
             <th>
                Name
            </th>           
            <th>
                Country
            </th>
           
            <th>
                Count
            </th>
            
        </tr>

    <%  var total = 0; 
        if (Model != null)
         {
           foreach (var item in Model)
             { %>
    
        <tr>
            
            
            <td>
                <%: item.Name%>
            </td>
           <td>
               
                <%: Html.ActionLink(item.Name1, "CandidateState", "AdminHome", new { functionId = item.Id , countryId = item.Id1}, null)%>
            </td>
            <td align="right">
                <%: item.TotalCount%>
            </td>
           
        </tr>
    
   <%   total = total + item.TotalCount;
             }
             
         } %>
          <tr>
          
          <td  colspan="2"  ><strong>Grand Total</strong></td>
          
          <td  align="right" ><strong><%: total %></strong></td>
                   
         </tr>

    </table>

     <br />
    <p>
        <%: Html.ActionLink("Back to Candidate Summary", "CandidateSummary", new { reportType = Session["reportType"] })%>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Admin.js")%>" type="text/javascript"></script>
</asp:Content>


