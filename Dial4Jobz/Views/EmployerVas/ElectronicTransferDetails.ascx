<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="modal">
    <div class="modal-profile-column">
      <div class="header">ELECTRONIC TRANSFER VIA RTGS/NEFT</div>
      
      <script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/themes/smoothness/jquery-ui-1.8.4.custom.css") %>" type="text/javascript"></script>
      <script type="text/javascript">
         $(document).ready(function () {
              $("#electrans").click(function () {
                  alert('Thanks for registering the details. Your plan will be activated within one working day..');
              });

              $("#TransferDate").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  dateFormat: "mm/dd/yy",
                  changeMonth: true,
                  changeYear: true
                  //yearRange: "1930:1995"
                  //onSelect: function (selected) {
                  // $('#ToDate').datepicker('option', 'minDate', selected)

              });
          });

      </script>
      
    <% Html.BeginForm("ElectronicTransfer", "EmployerVas", FormMethod.Post, new { @id = "ElectronicTransfer" });{ %>
    
          <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">
            You can transfer your payment by RTGS/NEFT  to <%--our ICICI Bank account or--%> ICICI Bank account held at Chennai.
           </div>
                 
         
        <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">ICICI Bank</div>
           <%-- Beneficiary Name: Dial4Jobz India Private Ltd<br />
            Bank Name: ICICI Bank<br />
            Branch Name: Thiruvanmiyur<br />
            Beneficiary Account Number: 782930321 <br />
            Account Type: Current Account<br />
            Account Holder's Name: Dial4Jobz India Private Ltd<br />
            Bank Address : 5,West Tank square Thiruvanmiyur,Chennai-600041<br />
            RTGS/NEFT/ISFC Code: IDIB000T044<br />--%>

             Current Account:
           <b> Our ICICI Bank Ltd </b>
            Account Number:  <b>603305017985</b> 
            Branch:  <b>Besant Nagar</b>
            IFSC Code: <b>ICIC0006033</b>
            Branch Address: <b>36,II Avenue,Besant Nagar,Chennai - 600090</b></p>
        
        <span class="red">Please fill up all required form fields accurately and submit</span>
     
        <h3> Amount to pay: Rs.<%:ViewData["Amount"]%> </h3>

                <%--div class="black">
                    <%:Html.Label("Transfer Reference:")%> 
                </div>

                <%:Html.TextBox("TransferReference")%>  <br />--%>
                <div class="black">
                    <%:Html.Label("Order Number:")%> 
                </div>

                <%:Html.Hidden("OrderNo") %>

                <%:ViewData["OrderNo"] %>       
              <%--  <div class="black">
                    <%:Html.Label("Transfer Date:")%>
                </div>
                <%:Html.TextBox("TransferDate")%><br />--%>
                <div class="black">
                    <%:Html.Label("Amount")%>
                </div>
                <%:Html.TextBox("Amount", ViewData["Amount"].ToString(), new { @readonly = "readonly" })%><br />
                <div class="black">
                    <%:Html.Label("Transferred From(bank)")%>
                </div>
                <%:Html.TextBox("TransferredBank")%><br />
         
           <center>
             <input type="submit" id="electrans" class="btn" value="Submit" />                
           </center>
          
          <% } %>
    </div>

</div>

