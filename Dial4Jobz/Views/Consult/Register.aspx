<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.ConsultantRegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register</h2>

   <% Html.BeginForm("Register", "Consult", FormMethod.Post, new { @id = "Save" });{ %>
        
        <div class="editor-field">
            <%: Html.Label("UserName") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("UserName") %>
        </div>

        <div class="editor-field">
            <%: Html.Label("Name") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Name")%>
        </div>

        <div class="editor-field">
            <%: Html.Label("Password") %>
        </div>

        <div class="editor-field">
           <%: Html.PasswordFor(m => m.Password, new { @title="Choose a password (minimum 6 characters)."})%>
        </div>

         <div class="editor-field">
            <%: Html.Label("Confirm Password") %>
        </div>

        <div class="editor-field">
           <%: Html.PasswordFor(m => m.ConfirmPassword, new { @title = "Choose a password (minimum 6 characters)." })%>
        </div>

        <div class="editor-field">
            <%: Html.Label("Email Address") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Email") %>
        </div>

       <%-- <div class="editor-field">
            <%: Html.Label("Contact Number") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("MobileNumber") %>
        </div>--%>

        <input id="Save" type="submit" value="Register" class="btn" name="Save" onclick="javascript:Dial4Jobz.Consultant.Save(this);return false;" />           
        <% Html.EndForm(); %>

         <%--<div id="loading">
            <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
         </div>--%>
        <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link href="../../Content/bootstrap.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Consultant.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavConsultant"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
