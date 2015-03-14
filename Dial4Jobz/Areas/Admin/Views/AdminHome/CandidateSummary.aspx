<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.ReportSummary>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Summary
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      

    <h2>Candidate Summary</h2>
   
     <table width="100%" >
      <tr>
          <th width="90%" ><strong>Name</strong> </th>
          <th  width="10%"><strong>Count</strong></th>
           <th  width="10%"><strong></strong></th>
            <th  width="10%"><strong></strong></th>
             <th  width="10%"><strong></strong></th>
         </tr>
         <tr>
         <td>Total number of Candidates registered </td>
         <td> <%: ViewData["TotalCandidates"]%></td>
         <td> <%: Html.ActionLink("Freshness", "CandidateFreshness", "AdminHome", new { functionId =0 },null)%></td>
         <td> <%: Html.ActionLink("Gender", "CandidateGender", "AdminHome", new { functionId = 0 }, null)%></td>
         <td> <%: Html.ActionLink("Place", "CandidateCountry", "AdminHome", new { functionId = 0 }, null)%></td>
                 </tr>

                 <tr>
          <th width="90%" ><strong>Name</strong> </th>
          <th  width="10%"><strong>Count</strong></th>
           <th  width="10%"><strong></strong></th>
            <th  width="10%"><strong></strong></th>
             <th  width="10%"><strong></strong></th>
         </tr>
     <%  
         var total = 0;
         if (Model != null)
         {
           
             foreach (var item in Model)
             { %>  
        <tr>
          <td width="90%" ><% =item.Name%></td>
           <td  width="10%" align="right" ><% =item.TotalCount%></td>
             <td> <%: Html.ActionLink("Freshness", "CandidateFreshness", "AdminHome", new { functionId =item.Id },null)%></td>
         <td> <%: Html.ActionLink("Gender", "CandidateGender", "AdminHome", new { functionId = item.Id }, null)%></td>
         <td> <%: Html.ActionLink("Place", "CandidateCountry", "AdminHome", new { functionId = item.Id }, null)%></td>
         </tr>
        
     <%   total = total + item.TotalCount;
             }
             
         } %>
          <tr>
          <td width="90%" ><strong>Grand Total</strong></td>
          <td  width="10%" align="right" ><strong><%: total %></strong></td>
           <td></td>
           <td></td>
           <td></td>
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

