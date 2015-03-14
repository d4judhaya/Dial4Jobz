<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Email Activation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="modal">
    <div class="modal-login-column">
  
        <h2>Your e-mailID has been verified successfully.</h2>

    <div class="editor-field" style="display:none;">
         <%: Html.CheckBox("IsMailVerified")%> 
    </div>
    <%:Html.ActionLink("Go to Home","Index","Employer") %>

    </div>
    </div>
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>--%>
