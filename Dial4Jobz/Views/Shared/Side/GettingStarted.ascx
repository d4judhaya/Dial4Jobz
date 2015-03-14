<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Candidate loggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"]; %>
<% bool isLoggedIn = loggedInCandidate != null; %>

<div class="section">
    <h5>Getting Started</h5>    
     <ul>
        <li><a href="http://www.youtube.com/watch?v=BsjqGoUUboE" target="_blank" class="link-video-frontpage" title="Watch Dial4jobz Videos">Watch Videos</a></li>
        <li>
            <% if (isLoggedIn)
               { %>
                    <%: Html.ActionLink("VAS(VALUE ADDED SERVICES)", "Index", "CandidatesVas", null, new { @class = "link-completeprofile-frontpage", title = "Vas for per Position" })%>
            <%} %>
        </li> 

          <li><a class="link-officephone-frontpage" title="Call us on the Phone">Call <b>044 - 44455566/33555777</b></a></li>

          <li><a class="link-phone-frontpage" title="Call us on the Mobile">Call <b>+91 9381516777</b></a></li><br />

          <%if (isLoggedIn == true)
          { %>
          <li>
          <div class="vastext">
                Stand Out in the Crowd Be Reachable To More Employers /Recruiters <a href="../../../../../Candidates/CandidatesVas/Index#DisplayResume">Display Your Resume</a><br /><br />
                You don’t get a second chance to make a 1st impression. Your Resume is your 1st Impression.<a href="../../../../../Candidates/CandidatesVas/Index#ResumeWrite">Resume Writing Experts at your Service</a> Call us 044 - 44455566 / 044 33555777 <br /><br />
                Get Background check done , Win the Confidence of Employer, Register your resume & order for "background check" 044 - 44455566 / 044 33555777<br /><br />
                Let the employer give 1st preferrence to call you for Interview..get your "certificates verified" & get the seal of Verified Candidate..<br /><br />
                <b>Exploring Overseas Jobs?</b>Want to know more about Employer,Place etc..<b><a href="../../../../../Candidates/CandidatesVas/Index#whoIsEmployer">Know your Employer</a></b><br />
          </div>
         </li>
        <% } else{ %>
        <li>
        <div class="vastext">
            Stand Out in the Crowd Be Reachable To More Employers /Recruiters <a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Display Your Resume</a><br /><br />
            You don’t get a second chance to make a 1st impression. Your Resume is your 1st Impression.<a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Resume Writing Experts at your Service</a>. " Call us - 044 - 44455566 / 044 33555777<br /><br />
            Get Background check done , Win the Confidence of Employer, Register your resume & order for "background check" 044 - 44455566 / 044 33555777<br /><br />
            Let the employer give 1st preferrence to call you for Interview..get your "certificates verified" & get the seal of Verified Candidate..<br /><br />
            <b>Exploring Overseas Jobs?</b>Want to know more about Employer,Place etc..<b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Know the Employer</a></b>
        </div>
        </li>
        <% } %>
    </ul>

</div>



