<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.ReportSummary>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Job Summary

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Job Summary</h2>
     <% Html.BeginForm("JobSummary", "AdminHome", FormMethod.Post, new { });  %>
    
    <table border="0" cellpadding ="2"  width="100%">
    <tr>
    <td>
     Type : <select id="filter"  name="filter">
        <option selected="selected" value="1">Industry</option>
         <option value="2">Function</option>
          <option value="3">Salary</option>
           <option value="4">Experience</option>
            <option value="5">Education</option>
        <option value="6">Country</option>
    <option value="7">City</option>
    <option value="8">Area</option>
   
</select>
  
    </td>
    <td>Freshness : <select id="where"  name="where">
 <option value="0">Total</option>
<option value="30">Last 30 Days</option>
<option selected="selected" value="7">Last 7 Days</option>
<option value="1">Day</option>
</select>
</td>
<td>
<div id="SelectDay">
Day : <input id="Day" name = "Day" type="text" style="width:auto; height :auto"  /></div>
</td>
    <td> <input id="view" type="submit" value="View"  /> </td>
    </tr>
    </table>
            
        <% Html.EndForm();  %>
        </br>

    <table border="0" cellpadding ="2"  width="100%" >
        <tr>
            
            <th>
               Name
            </th>
            
            <th>
                Total
            </th>
        </tr>

    <%   var grandTotal = 0;
        if (Model != null)
        {
            foreach (var item in Model)
            { %>
    
        <tr>
            
            <td>
                <%: item.Name%>
            </td>
           
            <td>
                <%: item.TotalCount%>
            </td>
        </tr>
        <% grandTotal = grandTotal + item.TotalCount;  %>
    <% }
        } %>
      <tr>
      <td><strong>Grand Total</strong></td>
      <td> <%: grandTotal%></td>
      </tr>
    </table> 
     </br>
    <p>
        <%: Html.ActionLink("Back Admin Home", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Admin.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>

