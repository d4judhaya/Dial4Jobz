<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ContactCandidates</title>
    <script type="text/javascript">

        $(document).ready(function () {

            var count = 0;
            var focusCount = 0;
            var totalCount = 0;


            //*******Count the sms content from Interest check**************//

            $('#SmsContactPerson').blur(function () {
                var title = $("input:radio[name=Title]:checked").val();
                var message = "";
                if (title == "1") {
                    message = "MSG thru Dial4jobz -044 - 44455566 - will you be interested for a vacancy in " + $("#SmsLocation").val() + " for " + $("#SmsVacancy").val() + " with " + $("#SmsCompanyName").val() + " contact(" + $("#SmsMobileNo").val() + ") or mailto (" + $("#SmsEmailId").val() + ").(" + $("#SmsContactPerson").val() + ")";
                    $('#smsmessage').focus();
                    document.getElementById("smsmessage").value = message;

                    var len = $('#smsmessage').val().length;
                    var msglength = 0;
                    var $msg = $("#charNum");
                    if (len >= 320) {
                        msglength = 3;
                    }

                    if (len >= 160) {
                        var output = $(this).val().substring(0, 160);
                        msglength = 2;

                        $msg.html(len + "/" + msglength + " sms left");
                    } else {
                        $msg.html(160 - len + " characters left");
                    }

                }

            });

            //*******Count the sms content from Interview call**************//

            $('#InterviewJobAddress').blur(function () {
                var title = $("input:radio[name=Title]:checked").val();
                var message = "";
                if (title == "1") {
                    message = " MSG thru Dial4jobz -044 - 44455566 - Ref ur cv u r shortlisted for Interview on " + $("#InterviewDate").val() + " at " + $("#InterviewLocation").val() + ", " + "Company : " + $("#InterviewCompany").val() + ", " +
                    "Meet : " + $("#InterviewContactPerson").val() + ", " +
                    "Mobile : " + $("#InterviewContactNo").val() + ", " +
                    "Position : " + $("#InterviewPosition").val() + ", " +
                    "Salary : " + $("#InterviewJobSalary").val() + ", " +
                    "location : " + $("#InterviewJobLocation").val() + ", " +
                    "Address : " + $("#InterviewJobAddress").val() + " Pls confirm ur presence";

                    $('#smsmessage').focus();
                    document.getElementById("smsmessage").value = message;

                    var len = $('#smsmessage').val().length;
                    var msglength = 0;
                    var $msg = $("#charNum");

                    if (len >= 320) {
                        msglength = 3;
                    }

                    if (len >= 160) {
                        var output = $(this).val().substring(0, 160);
                        msglength = 2;

                        $msg.html(len + "/" + msglength + " sms left");
                    } else {
                        $msg.html(160 - len + " characters left");
                    }

                }

            });



            //*******Count the sms content from Interest Call**************//

            $("#InterviewJobAddress").blur(function () {
                var title = $("input:radio[name=Title]:checked").val();
                var message = "";
                if (title == "2") {
                    message = " MSG thru Dial4jobz -044 - 44455566 - Ref ur cv u r shortlisted for Interview on " + $("#InterviewDate").val() + " at " + $("#InterviewLocation").val() + ", " +
                    "Company : " + $("#InterviewCompany").val() + ", " +
                    "Meet : " + $("#InterviewContactPerson").val() + ", " +
                    "Mobile : " + $("#InterviewContactNo").val() + ", " +
                    "Position : " + $("#InterviewPosition").val() + ", " +
                    "Salary : " + $("#InterviewJobSalary").val() + ", " +
                    "location : " + $("#InterviewJobLocation").val() + ", " +
                    "Address : " + $("#InterviewJobAddress").val() + " Pls confirm ur presence";
                    $('#smsmessage').focus();
                    document.getElementById("smsmessage").value = message;

                    var len = $('#smsmessage').val().length;
                    var msglength = 0;
                    var $msg = $("#charNum");
                    if (len > 320) {
                        msglength = 3;
                    }

                    if (len > 160) {
                        var output = $(this).val().substring(0, 160);
                        msglength = 2;

                        $msg.html(len + "/" + msglength);
                    } else {
                        $msg.html(160 - len + " characters left");
                    }
                }
            });

            var title = $("input:radio[name=Title]:checked").val();
            if (title == 3) {
                $("#sendsmsdirect").show();
                $("#InterviewDiv").hide();
                $("#InterestDiv").hide();
            }


            var title = $("input:radio[name=Title]:checked").val();

            if (title == "1") {
                $("#InterestDiv").show();
                $("#InterviewDiv").hide();
                $("#sendsmsdirect").hide();
            }
            else {
                $("#InterviewDiv").show();
                $("#InterestDiv").hide();
            }
        });

        $("input:radio[name=Title]").click(function () {
            var title = $("input:radio[name=Title]:checked").val();

            if (title == "1") {
                $("#InterestDiv").show();
                $("#messageArea").show();
                $("#contactbtn").show();
                $("#smsmessage").show();
                $("#InterviewDiv").hide();
                $("#sendsmsdirect").hide();
            }

            else if (title == "3") {
                $("#sendsmsdirect").show();
                $("#InterestDiv").hide();
                $("#InterviewDiv").hide();
                $("#smsmessage").hide();
                $("#contactbtn").hide();
                $("#messageArea").hide();
                $("#SmsDirectPosition").show();

            }
            else {
                $("#InterviewDiv").show();
                $("#sendsmsdirect").hide();
                $("#contactbtn").show();
                $("#InterestDiv").hide();
                $("#smsmessage").show();
                $("#messageArea").show();
            }

        });
    </script>
</head>
<body>
    <div style="width:650px;padding-left:20px;">    
     <h2>Contact Selected Candidate(s)</h2>     
     <div id="NotSelectedCandidateDiv">
     <h3>Please select the candidate(s) you wish to communicate</h3>
     </div>
     
     <div id = "ContactCandidateDiv">
   <% using (Html.BeginForm())
      { %>

      <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
      <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
      <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
     
      <% 
          int? hotresumecount = 0;
          int? smscount = 0;
          if (LoggedInConsultant != null)
          {
              hotresumecount = new Dial4Jobz.Models.Repositories.VasRepository().GetHorsConsultantCount(LoggedInConsultant.Id);
          }
          else if(LoggedInOrganization!=null)
          {
             hotresumecount = new Dial4Jobz.Models.Repositories.VasRepository().GetHotResumeCount(Model.Id);
          }
          
        if (hotresumecount != null && hotresumecount > 0)
        {
            if(LoggedInConsultant!=null)
                smscount = new Dial4Jobz.Models.Repositories.VasRepository().GetSmsVasCountConsultant(LoggedInConsultant.Id);
            else  if(LoggedInOrganization!=null)
                smscount = new Dial4Jobz.Models.Repositories.VasRepository().GetSmsVasCount(Model.Id);
             
        if (smscount != null && smscount > 0)
        { %>
      
     
      <div id="SmsDiv">

      <span style="font-size:18px;">For SMS</span> <br />
      Important Notice for sending SMS...<br />
        <div style="text-align:justify; width:600px;">
        1. As per TRAI policy only approved messages by TRAI can be sent as Transactional sms. 
        2. Only clients registered with Dial4jobz can avail this facility & only the registered mobile number & email id will be displayed.
        3. All the SMS will be be sent through Dial4jobz.Sender id will be displayed as "DLJOBZ".
        4. Please note you can send SMS from the available templates only.
        5. This will be delivered to all on High priority & to DND Numbers also.
        6. Only name of the contact person,name of the position,date of interview, time & Venue for Interview can typed as per your preference.
        <br />
        7. <b>Upto 160 character's for sms will be counted as 1 sms. if exceeds 160 characters it will counted as 2 Sms.</b>
        <b>Currently you can send <%:smscount %> sms.</b>
        </div>
          <div class="editor-label">
              Select Sms Template
          </div>          
          <div class="editor-field">
              <input type="radio" checked="checked" name="Title" value="1" />
              Interest check
              <input type="radio" name="Title" value="2" />
              Interview call   (or)

              
              <input type="radio" name="Title" value="3" />
              Send SMS directly from here 
              <input type="submit" id="Submit1" value="Send" onclick="javascript:Dial4Jobz.Candidate.SendSmsDirect();return false;" />  <br />
          </div>

         
          <div id="sendsmsdirect">
            <div class="editor-field">
            Position:
                <%:Html.TextBox("SmsDirectPosition") %>
            </div>
          </div> 

          <div id="InterestDiv">
              <div class="editor-label">
                  will you be interested for a vacancy in
              </div>
              <div class="editor-field">
                 <%: Html.TextBox("SmsLocation", "")%>
                 <%--<input type="text" name="sms" id="SmsLocation" />--%>
                 
              </div>
              <div class="editor-label">
                  For :
              </div>
              <div class="editor-field">
                   <%: Html.TextBox("SmsVacancy", "")%> 
                <%-- <input type="text" name="sms" id="SmsVacancy" />--%>
              </div>
              <div class="editor-label">
                  With :
              </div>
              <div class="editor-field">
              <% if (LoggedInConsultant != null)
                 { %>
                     <%: Html.TextBox("SmsCompanyName", LoggedInConsultant.Name, new { disabled = "true" })%> 
              <%} else { %>
                   <%: Html.TextBox("SmsCompanyName", Model.Name, new { disabled = "true" })%> 
               <%} %>
              </div>
              <div class="editor-label">
                  Contact Regd Mobile Number :
              </div>
              <div class="editor-field">
              <% if (LoggedInConsultant != null)
                 { %>
                  <%: Html.TextBox("SmsMobileNo", LoggedInConsultant.MobileNumber, new { disabled = "true" })%>
              <%} else { %>
                 <%: Html.TextBox("SmsMobileNo", Model.MobileNumber, new { disabled = "true" })%>
              <%} %>
                  or
              </div>

              <div class="editor-label">
                  Mailto Regd Mail Id :
              </div>
              <div class="editor-field">
               <% if (LoggedInConsultant != null)
                  { %>
                  <%: Html.TextBox("SmsEmailId", LoggedInConsultant.Email, new { disabled = "true" })%>
               <%} else { %>
                      <%: Html.TextBox("SmsEmailId", Model.Email, new { disabled = "true" })%>
               <%} %>
                 
              </div>

              <div class="editor-label">
                  Name of Contact Person :
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("SmsContactPerson","")%>
              </div>

          </div>

          <div id="InterviewDiv">
              <div class="editor-label">
                  Ref ur cv u r shortlisted for Interview on
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("InterviewDate", "")%>
                  at <br /><br />
                  <%: Html.TextBox("InterviewLocation", "")%>
              </div>
              <div class="editor-label">
                  Company
              </div>
              <div class="editor-field">
                 <% if (LoggedInConsultant != null)
                  { %>
                     <%: Html.TextBox("InterviewCompany", LoggedInConsultant.Name, new { disabled = "true" })%>
                <%} else { %>
                     <%: Html.TextBox("InterviewCompany", Model.Name, new { disabled = "true" })%>
               <%} %>
              </div>
              <div class="editor-label">
                  Meet
              </div>
              <div class="editor-field">
               <% if (LoggedInConsultant != null)
                  { %>
                    <%: Html.TextBox("InterviewContactPerson", LoggedInConsultant.ContactPerson)%>
               <%} else { %>
                    <%: Html.TextBox("InterviewContactPerson", Model.ContactPerson)%>
               <%} %>
              </div>
              <div class="editor-label">
                  Mobile 
              </div>
              <div class="editor-field">
               <% if (LoggedInConsultant != null)
                  { %>
                     <%: Html.TextBox("InterviewContactNo", LoggedInConsultant.MobileNumber, new { disabled = "true" })%>
               <%} else { %>
                     <%: Html.TextBox("InterviewContactNo", Model.MobileNumber, new { disabled = "true" })%>
               <%} %>
              </div>
              <div class="editor-label">
                  Position
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("InterviewPosition", "")%>
              </div>
               <div class="editor-label">
                  Salary
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("InterviewJobSalary", "")%> <br />
                  Fill salary offered or excellent salary or any detail to communicate on salary
              </div>
              <div class="editor-label">
                  Location
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("InterviewJobLocation", "")%>
              </div>             
              <div class="editor-label">
                  Address
              </div>
              <div class="editor-field">
                  <%: Html.TextBox("InterviewJobAddress", "")%> <br />
                  Pls confirm ur presence
              </div>
          </div>  
          
                  
        <div id="messageArea">
            <div class="editor-label">
                View Message
            </div>

            <div class="editor-field">
                <%: Html.TextArea("smsmessage", new { disabled = "true" })%> &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;  <div id="charNum"></div>
            </div>
            <a href="javascript:void(0);" onclick="javascript:Dial4Jobz.Candidate.ViewSmsMsg();return false;"> View Message</a>   
       </div>            

      </div>
      
     <% } else {
           %> Currently You don't have sms credit / Login again to Refresh.  Please <%:Html.ActionLink("Click Here", "Index", "Employervas")%> to Buy sms credit 
      <% }
         } 
         else
         {
             
           %> You can send SMS only when Hot Resume Plan is Active.  Please <a href="../../../../employer/employervas/index#HotResumes" target="_blank">Subscribe Hot Resumes</a> to Buy.
           
      <% }%>
        
        <%%>

        <% 
         
          if (hotresumecount != null && hotresumecount > 0)
          {
              int? emailCount = 0;
              if (LoggedInConsultant != null)
              {
                 emailCount = _vasRepository.GetConsultantEmailCount(LoggedInConsultant.Id);
              }
              else if(LoggedInOrganization!=null) 
              {
                  emailCount = _vasRepository.GetEmployerEmailCount(LoggedInOrganization.Id);
              }
              
          if (emailCount != null && emailCount > 0)
           { %>
 
            <div id="EmailDiv">
                <h3> You can send <%: emailCount%> more email's for this plan.</h3>
                <span style="font-size: 18px;">For Email</span><br />
                <div>
                    <input type="checkbox" value="1" class="cb" name="SessionMailTemplate" /><b>Use Saved Template</b></div>
                <div class="editor-label">
                    <%:Html.Label("Your Email Id")%>
                </div>
                <div class="editor-field">
                   <% if (LoggedInConsultant != null)
                      { %>
                          <%: Html.TextBox("EmployerEmail", LoggedInConsultant.Email, new { @title = "Enter Your Email Id" })%>  
                    <%} else { %>           
                          <%: Html.TextBox("EmployerEmail", Model.Email, new { @title = "Enter Your Email Id" })%>    
                    <%} %>
                </div>
                <div class="editor-label">
                    <%:Html.Label("Subject / Job Title")%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("Subject", "", new { @title = "Enter the Subject" })%>
                    <br />
                    For best response, enter designation, experience and location of the job.
                </div>
                <div class="editor-label">
                    <%:Html.Label("Job Details / Message")%>
                </div>
                <div class="editor-field">
                    Please enter details of the job for which you are contacting the candidates.
                    <br />
                    This information is shown upfront in the mail sent to jobseekers, so it is recommended
                    that you fill these fields.
                </div>
                <div class="editor-label">
                    <%:Html.Label("Job Experience")%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("MinExperience", "", new { @title = "Enter the Minimum Experience", Style = "width:65px;" })%>
                    to
                    <%: Html.TextBox("MaxExperience", "", new { @title = "Enter the Maximum Experience", Style = "width:65px;" })%>
                    (in years)
                </div>
                <div class="editor-label">
                    <%:Html.Label("Job CTC")%>
                </div>
                <div class="editor-field">
                    <%: Html.DropDownList("MinAnnualSalaryLakhs", (SelectList)ViewData["MinAnnualSalaryLakhs"])%>
                    -
                    <%: Html.DropDownList("MaxAnnualSalaryLakhs", (SelectList)ViewData["MaxAnnualSalaryLakhs"])%>
                    Lakhs (in INR)
                </div>
                <div class="editor-label">
                    <%:Html.Label("Other Salary Details")%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("OtherSalary", "", new { @title = "Enter the Other Salary" })%>
                    <br />
                    Specify salary details like incentives, reimbursements, breakup of salary, or "Best
                    in the Industry" etc.
                </div>
                <div class="editor-label">
                    <%:Html.Label("Job Location")%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("JobLocation", "", new { @title = "Enter the Job Location" })%>
                </div>
                <div class="editor-label">
                    <%:Html.Label("Message")%>
                </div>
                <div class="editor-field">
                    <%: Html.TextArea("Message", "", new { @title = "Enter the Message" })%>
                    <%--<%: Html.TextArea("Message", "", new { @title = "Enter the Message", @keyup = "countChar(this)" })%>--%>
               
                </div>
                <div class="gry font11">
                    <input type="checkbox" value="1" class="cb" name="SaveMailTemplate" />Save this
                    contact mail as a template (Save this as a mail template which you can use for subsequent
                    searches or session)</div>
                <div class="gry font11">
                    <input type="checkbox" value="1" class="cb" name="SessionMailTemplate" checked="checked" />Use
                    this template for this session (Recommended) (Use this if the same mail has to be
                    sent for other pages of the same search.)</div>
                <div class="gry font11">
                    <input type="checkbox" value="1" class="cb" name="SendApplyButton" checked="checked" />Send
                    'Apply' button in the mail sent to the candidate</div>
                <div class="editor-label">
                    <%:Html.Label("Important Tips")%>
                </div>
                1. Emails should be recruitment related, and specific to a job/role.
                <br />
                2. Do not send emails about training, advertising of any business, or seeking monetary
                transaction from jobseekers.
                <br />
                3. Send targeted emails to a focused set of jobseekers, and do not send bulk mails.
                <br />
                <b>This will ensure that your emails are not treated as spam by email providers and
                    / or jobseekers.</b>
                <br />
            </div>    
        <%} else { %>
                Your Email count for the current plan has been finished. 
        <%} %>

        <%} else { %>
            You can send email when HORS is active. To buy Hot Resumes <%:Html.ActionLink("Buy Hot Resumes","Index","EmployerVas") %>
        <% } %>

        <%var candidateId = ViewData["CandidateId"]; %>

        <% var details = Request.UrlReferrer.AbsoluteUri.Contains("employer/candidates/details");%>
        <% if (details != false)
            { %>
                 <input type="submit" id="Submit2" value="Send" onclick="javascript:Dial4Jobz.Candidate.SMSEmailForCandidates();return false;" />  <br />      
            <%} else { %>
                 <input type="submit" id="contactbtn" value="Send" onclick="javascript:Dial4Jobz.Candidate.SendEmailSms();return false;" />  <br />      
            <%} %>
               

<% } %>
</div>
    </div>
</body>
</html>
