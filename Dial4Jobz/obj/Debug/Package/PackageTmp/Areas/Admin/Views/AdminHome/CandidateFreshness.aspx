<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.ReportSummary>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Freshness
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["functionId"] == null || ViewData["functionId"] == "" )
    {
        ViewData["functionId"] = "0";
    }  %>
    <h2>Candidate Freshness</h2>
     <% Html.BeginForm("CandidateFreshness", "AdminHome", FormMethod.Post, new { });  %>
    <input id="SelectedId" name="SelectedId"  type ="hidden" value="<%: ViewData["functionId"] %>"  />
    <table border="0" cellpadding ="2"  width="100%">
    <tr>
  
    <td>Freshness : <select id="where"  name="where">
 <option value="0">Total</option>
<option value="30">Last 30 Days</option>
<option selected="selected" value="7">Last 7 Days</option>
<option value="1">Day</option>
</select>
</td>
<td>
<div id="SelectDay">
Day : <input id="Day" name = "Day" type="text" style="width:auto; height :auto"  /> (dd/mm/yyyy)</div>
</td>
    <td align="right" > <input id="view" type="submit" value="View"  /> </td>
    </tr>
    </table>
          
        <% Html.EndForm(); %>
       
    <table width="100%">
        <tr>
                       
            <th>
                Name
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
            
            <% if (item.CreatedDate != null)
               {%>
             <td>
                <%: item.CreatedDate%>
            </td>
            <%}
               else
               { %>
            <td>
                <%: item.Name%>
            </td>
           <%} %>
            <td align="center">
                <%: item.TotalCount%>
            </td>
           
        </tr>
         
    <%   total = total + item.TotalCount;
             }
         } %>
         <tr>
          <td width="90%" ><strong>Grand Total</strong></td>
          <td  width="10%" align="center" ><strong><%: total %></strong></td>
           
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

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>


