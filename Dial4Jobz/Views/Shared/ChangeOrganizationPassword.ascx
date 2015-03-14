<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.ChangePasswordModel>" %>
<div class="modal">
    <div class="modal-login-column">
        <div class="header">Change Password</div>
       <%-- <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%: ViewData["PasswordLength"] %> characters in length.
    </p>--%>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Password change was unsuccessful. Please correct the errors and try again.") %>
        <div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.OldPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.OldPassword) %>
                    <%: Html.ValidationMessageFor(m => m.OldPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.NewPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.NewPassword) %>
                    <%: Html.ValidationMessageFor(m => m.NewPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
               <p>
                    <input type="submit" value="Submit" onclick="javascript:Dial4Jobz.Auth.ChangeOrganizationPassword();return false;" /> 
               </p>
        </div>
        
    <div id="loading">
       <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
    </div>
        
    <% } %>


   </div>
</div>

