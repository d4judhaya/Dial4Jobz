<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.LogOnModel>" %>

<div class="modal">
    <div class="modal-login-column">

     <% var value = string.Empty;
           if (Request.QueryString.Count > 0)
           {
               value = Request.QueryString[0];
           } %>

    
  <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
     { %>
       <div class="header"> <img src="../../Content/Images/EmploginIcon.png"  width="24px"; height="26px"; />  Employer Login</div>
        <% } else  if (value=="admin" && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin")) { %>
            <div class="header">Admin Login</div>
        <%} else { %>
        <div class="header"> <img src="../../Content/Images/candiloginIcon.png" width="24px"; height="24px"; />Candidate Login</div>
        <h3><b>Viewing Job and Applying Job is free. Please Login to view or Register</b></h3>
        <% } %>

        <% using (Html.BeginForm()) { %>
            <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
                 <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName, new { @title = "Enter your username." })%>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                    <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
                    { %>
                        <%: Html.HiddenFor(m => m.loginAs, new { id = "hiddenEmp", Value="Employer" })%>
                     <% } else  if (value=="admin" && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin")) { %>
                        <%: Html.HiddenFor(m=> m.loginAs, new { id="hiddenAdmi", Value="Admin" }) %>
                    <% } else %>
                        <%: Html.HiddenFor(m => m.loginAs, new { id = "hiddenCand", Value="Candidate" })%>
                </div>

                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password, new { @title = "Enter your password." })%>
                    <%: Html.ValidationMessageFor(m => m.Password) %> <%--<a id="retrievepassword" href="<%=Url.Content("~/Account/RetrievePassword")%>">Forgot?</a>--%>
                </div>   
                
                <p>
                    <input type="submit" value="Login" onclick="javascript:Dial4Jobz.Auth.Login();return false;" />        
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>       
                </p>   
                
                
                <div class="bottom">
                  Don't have an account?<% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
                     { %> <a class="signup" href="<%=Url.Content("~/signup")%>?value=employer">Register |</a><% } else {%> <a class="signup" href="<%=Url.Content("~/signup")%>?value=candidate">Register|</a><% }%> 

                  <% if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer")) { %>  
                        <a class="signup" href="<%=Url.Content("~/forgotpassword")%>?value=employer">Forgot Password</a>
                  <% } else {%>  
                        <a class="signup" href="<%=Url.Content("~/forgotpassword")%>">Forgot Password</a>
                  <% } %> 
                </div>  <br />
        <% } %>

         <div id="loading">
            <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
         </div>
    </div>


    <div class="modal-3rdparty-column">
       
    </div>
</div>