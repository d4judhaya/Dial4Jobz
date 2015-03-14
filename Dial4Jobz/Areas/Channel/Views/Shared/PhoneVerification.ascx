
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

            <% Html.BeginForm("PhoneNoVerification", "AdminHome", FormMethod.Post, new { @id = "Save" });
                {%>

                  <div class="editor-label">
                    <%: Html.Label("Mobile Number Verification Code")%>
                        <span class="red">*</span>
                   </div>

                    <div class="editor-field">
                        <%: Html.TextBox("PhVerificationNo", null, new { @title = "Enter the Verification Code" })%>
                    </div>
                   
                    <div class="editor-field" style="display:none;">
                         <%: Html.CheckBox("IsPhoneVerified")%> 
                    </div>

          <input id="Save" type="submit" value="Submit" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Candidate.AdminPhVerification(this);return false;"/>
          <input id="Cancel" type="submit" value="Proceed wihout Verification" class="btn" name="Submit" onclick="javascript:Dial4Jobz.Candidate.AdminPhVerification(this);return false;"/>
        <% } %>
      
    