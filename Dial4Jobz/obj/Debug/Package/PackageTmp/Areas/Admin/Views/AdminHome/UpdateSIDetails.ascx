<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.OrderDetail>" %>

<div class="modal">
    <div class="modal-login-column">
      <center><div class="black">UPDATE DETAILS OF SPOT INTERVIEW</div></center>
      <script type="text/javascript">
          $(document).ready(function () {
              $("#electrans").click(function () {
                  alert('Thanks for registering the details. Your plan will be activated within one working day..');
              });

//              $("#OrderDate").datepicker({
//                  changeMonth: true,
//                  changeYear: true,
//                  dateFormat: "mm/dd/yy",
//                  changeMonth: true,
//                  changeYear: true
//                  //yearRange: "1930:1995"
//                  //onSelect: function (selected) {
//                  // $('#ToDate').datepicker('option', 'minDate', selected)

//              });
          });

      </script>
    <% Html.BeginForm("UpdateSIDetails", "AdminHome", FormMethod.Post, new { @id = "SI" });{ %>
  
        <div class="editor-label">
            <%:Html.Label("Amount") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Amount") %>
        </div>

       <div class="editor-label">
            <%:Html.Label("Validity Period (Days)") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("ValidityPeriod")%>
        </div>

        <div class="editor-label">
            <%:Html.Label("Number of Interviews")%>
        </div>

        <div>
            <%:Html.TextBox("TeleConferenceCount")%>
        </div>
         
        <center>
            <input type="submit" id="SI" class="btn" value="Submit" />                
        </center>
          
          <% } %>
   

</div>

