<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.VASActivationModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VASActivationByAdmin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>VAS Activation </h2>

    <table width="130%">
        <tr>            
            <th style="width:20%; border:1px white solid">
                Organization ID
            </th>
            <th style="width:30%; border:1px white solid">
                User Name
            </th>
            <th style="width:20%; border:1px white solid">
                Plan ID
            </th>
            <th style="width:15%; border:1px white solid">
                Order Number
            </th>

            <th style="width:15%; border:1px white solid">
                Start Date
            </th>
                        
            <th style="width:15%; border:1px white solid">
                End Date
            </th>

            <th style="width:15%; border:1px white solid">
                Activate
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>           
            <td>
                <%: item.orgId %>
            </td>
            <td>
                <%: item.userName %>
            </td>
            <td>
                <%: item.planId %>
            </td>
            <td>
                <%: item.orderId %>
            </td>
            <td>
                <%:item.startDate %>
            </td>

            <td>
                <%:item.endDate %>
            </td>

            <td>
                 <%: Html.ActionLink("Activate", "ActivateVAS", new { orgID = item.orgId, item.orderId,item.planId })%>
            </td>
        </tr>
    
    <% } %>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>

