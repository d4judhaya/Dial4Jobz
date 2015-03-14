<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMS Activated Successfully
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   

    <% if (ViewData["Success"] != null && ViewData["Success"].ToString() != "")
           { %>
             <h2><%=ViewData["Success"]%></h2><br />

             <%:Html.ActionLink("Go to your Page","MatchCandidates","Employer") %>
                  
        <% }
            if (ViewData["Failure"] != null && ViewData["Failure"].ToString() != "")
           { %>
             <h2><%=ViewData["Failure"]%></h2>        
        <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
