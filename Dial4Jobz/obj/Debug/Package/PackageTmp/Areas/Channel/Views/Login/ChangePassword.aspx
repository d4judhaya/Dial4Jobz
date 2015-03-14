<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.ChangePasswordModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ChangePassword</title>
    <script type="text/javascript">
        function ChangePassword() {
            $("#loading").show();
            var $popup = $("#fancybox-outer");
            var form = $popup.find("form");
            var data = form.serialize();
            var url = form.attr('action');
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                dataType: "json",
                success: function (response) {
                    $("#loading").hide();
                    if (response.Success == true) {
                        $.fancybox.close();
                        alert(response.Message);
                    }
                    else if (response.Success == false) {
                        if (response.ReturnUrl != null && response.ReturnUrl != "") {
                            location.href = response.ReturnUrl;
                        }
                        else {
                            alert(response.Message);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    $("#loading").hide();
                    alert(error);
                }
            });

            return false;
        };
    </script>
</head>
<body>
    <div>
    <div class="modal">
    <div class="modal-login-column">
        <div class="header">Change Password</div>       

    <% using (Html.BeginForm()) { %>      
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
                    <input type="submit" value="Submit" onclick="javascript:ChangePassword();return false;" />        
                </p>
            
        </div>
    <% } %>
    </div>
</div>
    </div>
</body>
</html>
