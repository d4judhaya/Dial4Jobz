<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>


 <script type="text/javascript">
     $(document).ready(function () {
         //         $("#Email").change(function () {
         //             //alert('I am pretty sure the text box changed');
         //             $("#MobileNumber").prop('disabled', true);
         //             event.preventDefault();
         //         });

         $('#Email').click(function () {
             $('#MobileNumber').attr("disabled", true);
         });

         $('#MobileNumber').click(function () {
             $('#Email').attr("disabled", true);
         });

     });
    
 </script>

<div class="modal">
    <div class="modal-login-column">
        <div class="header">Forgot Password</div>


        <% using (Html.BeginForm()) { %>

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
                <input type="submit" value="Submit" onclick="javascript:Dial4Jobz.Auth.ForgotPassword();return false;" />        
            </p>      
        <% } %>
    </div>
</div>

