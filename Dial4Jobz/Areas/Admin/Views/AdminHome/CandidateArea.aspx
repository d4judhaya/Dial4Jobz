<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.GroupReport>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Area
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Candidate Area</h2>

    <table width="100%">
        <tr>
            
            <th>
                Name
            </th>
           
            <th>
                Area
            </th>
            <th>
                Count
            </th>
        </tr>

    <%  var total = 0; 
        if (Model != null)
         {
    foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Name %>
            </td>
           
            <td>
                <%: item.Name1 %>
            </td>
            <td  align="right">
                <%: item.TotalCount %>
            </td>
        </tr>
    
    <%   total = total + item.TotalCount;
             }
             
         } %>
          <tr>
          
          <td   colspan="2"><strong>Grand Total</strong></td>
         
          <td  align="right" ><strong><%: total %></strong></td>
                   
         </tr>

    </table>

   </br>
    <p>
        <%: Html.ActionLink("Back to Candidate City", "CandidateCity", new { functionId = ViewData["functionId"], stateId = TempData["stateId"] })%>
    </p>
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>

