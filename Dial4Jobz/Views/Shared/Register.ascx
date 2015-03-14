<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.RegisterModel>" %>

<div class="modal">
    <div class="modal-register-column">
        <% var value = string.Empty;
         
           if (Request.QueryString.Count > 0)
           {
               value = Request.QueryString[0];
           } %>
       
       <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
          { %>
            <div class="header">Employer Registration</div>

         <% }
          else if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin"))
          { %>
            <div class="header">Admin Registration</div>
        <% }
          else
          { %>
            <div class="header">Candidate Registration</div>
        <% } %>

        <script type="text/javascript">
            $(document).ready(function () {
//                $("#ConsultantIndustries").hide();
                $("input[name='EmployerType']").change(function () {
                    $("#ConsultantIndustries").hide();
                   $("#Industries").show();
                    var employerType = $('input[name="EmployerType"]:checked').val();
                    // alert(employerType);
                    if (employerType == 1) {
                        $("#ConsultantIndustries").show();
                        $("#Industries").hide();
                    }
                    else {
                        $("#Industries").show();
                        $("#ConsultantIndustries").hide();
                    }
                });

                $("#MobileNumber").keydown(function (e) {
                    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1) {
                        return;
                    }
                    if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                        e.preventDefault();
                    }
                });

            });
    
    </script>
   

        <% using (Html.BeginForm()) { %>
            <%: Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>

             <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
              { %>
                <div class="editor-label">
                    <%: Html.Label("Employer Type") %>
                </div>

                <div class="editor-field">
                    <%:Html.RadioButtonFor(model=> model.EmployerType,1)%>  Individual
                    <%:Html.RadioButtonFor(model=> model.EmployerType,2)%>  Company
                </div>
             <% } %>
               
                <div class="editor-field">
                     <%--<input type="text" name="name" id="name" class="required" maxlength="6" />--%>
                    <%: Html.TextBoxFor(m => m.UserName, new { @title = "Username should contain within 20 characters.Don't enter special characters." })%>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>

                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email, new { @title = "Enter Your EmailId. Eg:xxx@gmail.com,@yahoo.co.in..." })%>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>

                <div class="editor-field">
                   <%: Html.TextBox("MobileNumber", "", new { @title = "Enter the Mobile Number", @maxlength = "10" })%> 
                </div>

                 <div class="editor-field">
                   <%: Html.TextBox("Name", "", new { @title = "Enter the Name" })%> 
                </div>
                                                              
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password, new { @title="Choose a password (minimum 6 characters)."})%>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword, new { @title = "Repeat your password again..." })%>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>     
               
                 <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer")){%>
                  
                    <div class="editor-field">
                        <%: Html.TextBox("ContactPerson", "", new { @title = "Enter the organization's contact person" })%>   
                    </div>

                    <div class="editor-field">
                       <%: Html.DropDownList("Industries", (SelectList) ViewData["Industries"], "Select Industry")%> 
                       <%: Html.DropDownList("ConsultantIndustries", (SelectList)ViewData["ConsultantIndustries"], "Select Industry")%>
                    </div>
                     
                  <% } %>     
               
                   <p>
                        Login / Register to View Resumes, Post Vacancies, Subscribe Value added services.<br />
                       <input type="submit" class="login"  value="Create Account" onclick="javascript:Dial4Jobz.Auth.Register();return false;" />  
                   </p><br/>
             
                    <div class="bottom terms">
                        By creating an account you agree to the <%:Html.ActionLink("Terms of Service","Terms","Home") %> & <%:Html.ActionLink("Privacy Policy","Privacy","Home") %>
                    </div>  
                    <div id="loading">
                        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
                    </div>   
                                    
        <% } %>
        
    </div>
    
</div>


