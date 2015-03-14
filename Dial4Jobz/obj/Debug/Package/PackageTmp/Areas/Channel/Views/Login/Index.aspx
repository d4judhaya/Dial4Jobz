<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.LogOnModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dial4jobz :: Channel Login</title>
    <link href="<%= Url.Content("~/Areas/Admin/Content/adminpermission.css") %>" rel="stylesheet" type="text/css" />   
    <link href="<%= Url.Content("~/Content/jquery.fancybox-1.3.1.css") %>" rel="stylesheet" type="text/css" /> 
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>           
     <script src="<%=Url.Content("~/Scripts/jquery.fancybox-1.3.1.pack.js")%>" type="text/javascript"></script>  
    <script src="<%= Url.Content("~/Areas/Channel/Scripts/Dial4Jobz.ChannelCommon.js") %>" type="text/javascript"></script>  
    <script src="<%= Url.Content("~/Areas/Channel/Scripts/Dial4Jobz.ChannelAuth.js") %>" type="text/javascript"></script>  
      
</head>
<body>
<div id="wrapper">  
        <div id="page">
            <div id="header">

                <div id="logo">
                    <a href="/"><img src="<%=Url.Content("~/Content/Images/dial4jobz_logo.png")%>" alt="Dial4Jobz logo" title="Dial4Jobz Logo" /></a> 
                </div>
              
                <div id="menu-bar"> 
                
                 </div> 
            </div> 
    <div id="main" style="width:798px;">    
    <h2>Channel Partners & Users Login</h2>
   
        <% using (Html.BeginForm("Login","Login")) { %>
            <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
                 <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName, new { placeholder = "Enter your email." })%>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>                    
                </div>

                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password, new { placeholder = "Enter your password." })%>
                    <%: Html.ValidationMessageFor(m => m.Password) %> 
                </div>   
                
                <p>
                    <input type="submit" value="Login" id="btnlogin" onclick="javascript:Dial4Jobz.ChannelAuth.Login();return false;" />        
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>       
                </p>   
                
                
                <div class="bottom">
                    <a class="ActionPopup" href="<%=Url.Content("~/Channel/Login/ForgotPassword")%>">Forgot Password</a>                
                </div>    
                
        <% } %>
 </div>
</div>
</div>

<div id="loading">
  <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
</div>
</body>
</html>



