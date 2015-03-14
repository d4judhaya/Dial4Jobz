<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.ConsultantLogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LogOn
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <h3>Consultant Login</h3>

      <% using (Html.BeginForm("LogOn","Consult")) { %>
            <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
                 <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName, new { placeholder = "Enter your UserName." })%>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>                    
                </div>

                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password, new { placeholder = "Enter your password." })%>
                    <%: Html.ValidationMessageFor(m => m.Password) %> <%--<a id="retrievepassword" href="<%=Url.Content("~/Account/RetrievePassword")%>">Forgot?</a>--%>
                </div>   
                
                <p>
                    <input type="submit" value="Login" id="btnlogin" onclick="javascript:Dial4Jobz.Consultant.Login();return false;" />        
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>       
                </p>   
                
                
                <div class="bottom">
                    <a class="signup" href="<%=Url.Content("~/Account/forgotpassword")%>?value=consultant"">Forgot Password</a>   
                </div>    
                
        <% } %>

        

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link href="../../Content/bootstrap.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Consultant.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <%: Html.ActionLink("Consultant Home", "Index", "Consult", null, new { title = "Back home" })%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
