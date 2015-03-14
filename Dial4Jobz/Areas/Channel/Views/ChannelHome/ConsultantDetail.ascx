<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="modal">
    <div class="modal-login-column">
        <% using (Html.BeginForm()) { %>
            <div class="editor-label">
                <%: Html.Label("Name") %>
            </div>

              <div class="editor-field">
                <%: Html.Label("Name") %>
            </div>
        <%} %>
    </div>
</div>
