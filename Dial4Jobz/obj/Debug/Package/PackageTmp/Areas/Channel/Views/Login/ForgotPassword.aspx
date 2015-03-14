<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ForgotPassword</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#Email').click(function () {
                $('#MobileNumber').attr("disabled", true);
            });

            $('#MobileNumber').click(function () {
                $('#Email').attr("disabled", true);
            });

        });
    
 </script>
</head>
<body>
    <div class="modal">
    <div class="modal-login-column">
        <div class="header">Forgot Password</div>


        <% using (Html.BeginForm("ForgotPassword", "Login", FormMethod.Post))
           { %>

            <h3><b>Userid & Password will be sent to your Email Id.</b></h3>
            <div class="editor-label">
                <%:Html.Label("Email Id") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("Email", null, new { @title = "Enter your email address." })%> 
            </div>   
            
            <h3>Or</h3>

            <h3><b> Userid & Password will be sent by SMS to your Mobile Number.</b> </h3>
            <div class="editor-label">
               <%:Html.Label("Mobile Number") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("MobileNumber", null, new { @title = "Enter your Mobile Number." })%>
            </div>         
                
            <p>
                <input type="submit" value="Submit" onclick="javascript:Dial4Jobz.ChannelAuth.ForgotPassword();return false;" />        
            </p>      
        <% } %>
    </div>
</div>
</body>
</html>
