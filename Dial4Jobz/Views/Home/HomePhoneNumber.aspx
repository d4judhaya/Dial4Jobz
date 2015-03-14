<%@ Page  Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>--%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ContactCandidates</title>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Home.js")%>" type="text/javascript"></script>
    </head>
    <body>
        <div class="modal">
        <div class="modal-login-column">
            <div class="header"></div>
               <center>
                 <img src="<%=Url.Content("~/Content/Images/Land Line.jpg")%>" alt="Share Landline" title="Share Landline" width="500px"; height="400px" />
                 </center><br />    
                 <center>
                     <a class="homeLandline" href="<%=Url.Content("Home/SendHomePhoneNumber")%>"><img src="<%=Url.Content("~/Content/Images/Click here to sms.jpg")%>" alt="Send Phone Number" title="Dial4Jobz Logo" width=300px; height=50px; /></a> 
                 </center>
                   
        </div>
        </div>
    </body>
    </html>
