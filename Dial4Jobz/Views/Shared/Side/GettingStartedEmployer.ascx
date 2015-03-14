<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInOrganization != null; %>

<div class="section">
    <h5>Getting Started</h5>    
    <ul>
        <li><a href="http://www.youtube.com/watch?v=BsjqGoUUboE" target="_blank" class="link-video-frontpage" title="Watch Dial4Jobz videos">Watch Videos</a></li>
        <li>
            <% if (isLoggedIn){ %>
                <%: Html.ActionLink("Post Jobs", "Add", "Jobs", null, new { @class = "link-completeprofile-frontpage", title = "Post a job", id="postJob" })%>

                <%: Html.ActionLink("Posted Jobs", "PostedJobs","Employer", null,new { @class="link-completeprofile-frontpage",title = "Edit this job posting" })%>     
                 <%: Html.ActionLink("VAS(VALUE ADDED SERVICES)", "Index", "Employervas", null, new { @class = "link-completeprofile-frontpage", title = "Vas for per Position" })%>
            <% } else { %>
                <a class="login link-completeprofile-frontpage" href="<%=Url.Content("~/login")%>" title="Post a job">Post Jobs<%--<img src="../../../Content/Images/Free.jpg" />--%></a>
            <% } %>
        </li>
      
        <li><a class="link-officephone-frontpage" title="Call us on the Phone">Call <b>044-44455566/33555777</b></a></li>
        <li><a class="link-phone-frontpage" title="Call us on the Mobile">Call <b>+91 9381516777</b></a></li> <br />
        
        <%if (isLoggedIn == true)
          { %>
          <li>
          <div class="vastext">
            Reach candidates Immediately <a href="../../../../employer/employervas/index#smsPurchase" target="_blank">Buy SMS</a><br />
            Get Matching candidates as soon as they Apply  <a href="../../../../employer/employervas/index#ResumeAlert" target="_blank">Resume Alert</a><br />
            Do  <a href="../../../../employer/employervas/index#backgroundCheck" target="_blank">Background Check</a> before Appointing...<br />
          </div>
         </li>
        <% } else{ %>
        <li>
        <div class="vastext">
            Get Matching candidates as soon as they Apply <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Resume Alert</a><br />
            Reach candidates Immediately <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">buy Sms</a><br />
            Do <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Background Check</a> before Appointing...
        </div>
        </li>
        <% } %>
       </ul>
    
</div>