<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="modal">
    <div class="modal-login-column">
      <div class="header">
    <% Html.BeginForm("Send", "Home", FormMethod.Post, new { @id = "Send" });{ %>
    
        <div class="homephonenumber">
            <center>Share 09381516777 via SMS</center>
        </div>
          <div class="editor-label" style="color:Black";>
          <%:Html.Label("Mobile Number:") %>
            <%:Html.TextBox("MobileNumber") %></div>
          <center><h3><b>SMS shall be sent to your Mobile Number</b></h3></center>
          <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">
            <h3>Save this number to get job or candidate for Any role Any Level from Any Industry...</h3>
            <h3>Forward it to your friends too Its useful.</h3>
          </div>
           <center>
             <input type="submit" class="homeLandline" value="Send Now" onclick="javascript:Dial4Jobz.Home.Send();return false;" />        
           </center>
          </div>
          <% } %>
    </div>

</div>

