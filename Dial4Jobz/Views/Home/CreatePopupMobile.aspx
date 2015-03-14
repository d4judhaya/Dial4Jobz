<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>ContactCandidates</title>
 <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Home.js")%>" type="text/javascript"></script>
 </head>
 <body>
 <div class="modal">
    <div class="modal-login-column">
        <div class="header">
            <center>
             <img src="<%=Url.Content("~/Content/Images/Mobile.jpg")%>" alt="Share Mobile" title="Share Mobile" width="500px"; height="400px" />
            </center><br />
           <center>
              <a class="homeMobile" href="<%=Url.Content("Home/SendHomeMobileNumber")%>"><img src="<%=Url.Content("~/Content/Images/Click here to sms.jpg")%>" alt="Send Phone Number" title="Dial4Jobz Logo" width=300px; height=50px; /></a> 
           </center>
        
        </div>

    </div>
 </div>

 </body>

 </html>