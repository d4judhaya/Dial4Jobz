<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

            <% Html.BeginForm("PhoneNoVerification", "Candidates", FormMethod.Post, new { @id = "Save" });
                {%>

                  <div class="editor-label">
                    <%: Html.Label("Mobile Number Verification Code")%><br />
                        <span class="red">You will receive verification code by sms and Verfication Link by Mail. For Mobile Verification, Please type the code below box, Email Verification Check your Email and click the provided link.</span>
                   </div>

                    <div class="editor-field" style="width:200px;">
                        <%:Html.Hidden("CandidateId", ViewData["CandidateId"].ToString())%>
                        <%: Html.TextBox("PhVerificationNo", null, new { @title = "Enter the Verification Code" })%>
                    </div>
                   
                    <div class="editor-field" style="display:none;">
                         <%: Html.CheckBox("IsPhoneVerified")%> 
                    </div>
                    <h3>Its suggested that you verify your mobile number without fail as most of employers give preference to call the candidates only if the mobile number is verified.</h3>

          <input id="Save" type="submit" value="Submit" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Candidate.PhVerification(this);return false;"/>
         <%-- <input id="Cancel" type="submit" value="Proceed wihout Verification" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Candidate.PhVerification(this);return false;"/>--%>
           <input id="Cancel" type="submit" value="Proceed wihout Verification" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Candidate.PhVerification(this, 1);return false;"/>
        <% } %>
      
    