<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server"> 

<title>Deposit / Send us a Chque or Draft</title>
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"/>
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/themes/smoothness/jquery-ui-1.8.4.custom.css") %>" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        //hide related select details

        $("#referenceNo").hide();
        $("#transferdate").hide();
        $("#bankname").hide();
        $("#branchname").hide();
        $("#Submit1").hide();

        $('#ddlInstrumentType').change(function () {

            var ddlType = $("#ddlInstrumentType").val();

            if ((ddlType == "0") || (ddlType == "1")) {
                $("#referenceNo").show();
                $("#transferdate").show();
                $("#bankname").show();
                $("#branchname").show();
                $("#Submit1").show();
            }

            else if (ddlType == "2") {
                $("#transferdate").show();
                $("#bankbranch").show();

                //to hide the next select
                $("#referenceNo").hide();
                $("#bankname").hide();

            }

            else if (ddlType == "3") {
                $("#transferdate").show();
                $("#referenceNo").show();
                $("#Submit1").show();

                //to hide the next select
                $("#bankname").hide();
                $("#bankbranch").hide();

            }
            else if (ddlType == "4") {
                $("#referenceNo").show();
                $("#transferdate").show();
                $("#bankname").show();
                $("#Submit1").show();

                //to hide the next select

                $("#bankbranch").hide();

            }

            else if (ddlType == "5") {
                $("#referenceNo").show();
                $("#transferdate").show();
                $("#Submit1").show();

                //to hide the next select

                $("#bankname").hide();
                $("#bankbranch").hide();
            }


        });
        //End hide details of related select

        $("#depositcheque").click(function () {
            alert('Thanks for registering the details. Your plan will be activated within one working day..');
        });

        //date picker
        $("#PaymentDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "mm/dd/yy",
            changeMonth: true,
            changeYear: true
        });

    });
  
</script>


 </head>
 <body>
  <div style="width:650px;padding-left:20px;">    
   <% Html.BeginForm("DepositChequeDraft", "employer/employervas", FormMethod.Post, new { @id = "Save" });
      { %>
        <center> <h3>Deposit /send us a Cheque or Draft</h3></center>
            <p>If your city has a branch of <b>ICICI Bank </b> you can pay us by local cheque. Prepare a cheque drawn in favour of <b>Dial4Jobz India Private Ltd</b> and deposit the same in your city's <b>ICICI Bank</b>
         <p>
         Current Account:
           <b> Our ICICI Bank Ltd </b>
            Account Number:  <b>603305017985</b> 
            Branch:  <b>Besant Nagar</b>
            IFSC Code: <b>ICIC0006033</b>
            Branch Address: <b>36,II Avenue,Besant Nagar,Chennai - 600090</b></p>

            After depositing the Cheque/Cash/Draft, send a email to <b>vas@dial4jobz.com</b> quoting your <b>Name, Address, Cheque number, Cheque amount, ICICI Bank and City</b> where deposited.</p>
     
            <p>You can prepare a Demand Draft / Pay Order drawn in favour of Dial4Jobz India Private Ltd and payable at Chennai and courier it to the following address:<br />
               <b>Dial4Jobz India Private Ltd</b>
                #32,3rd Cross Street Ext.,
                AGS colony (SEA SIDE),
                Kottivakkam,
                Chennai - 600041,
                Phone:044 - 44455566
            </p>
  
        
       <span class="red">Cash deposit is not accepted by courier</span>
       <span class="red">Outstation cheques are not accepted.Please fill up all required form fields accurately and submit.</span>

      <h3>Amount to Pay:Rs. <%:ViewData["Amount"]%> </h3>
       
     <div class="editor-label">
        <%:Html.Label("Instrument Type:") %>
     </div>

     <div class="editor-field">
        <select id="ddlInstrumentType" name="ddlInstrumentType">
            <option value="0">Cheque</option>
            <option value="1">Draft</option>
            <option value="2">Deposit Cash</option>
            <option value="3">Inter Bank</option>
            <option value="4">NEFT</option>
            <option value="5">IMPS</option>
        </select>
     </div>

      <div class="black">
         <%:Html.Label("Order Number:")%> 
      </div>

      <%:Html.Hidden("OrderNo") %>
      <%:ViewData["OrderNo"] %>  


     <div class="editor-label">
        <%:Html.Label("For: Rs") %>
     </div>

     <div class="editor-field">
         <%:Html.TextBox("Amount", ViewData["Amount"])%><br />
     </div>
     
     <div id="referenceNo">
         <div class="editor-label">
            <%:Html.Label("Cheque Number / Draft Number / Transfer Reference:") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("ReferenceNumber")%>
         </div>

     </div>

    <%-- <h3>Deposit / Transfer Date Details</h3>--%>

    <div id="transferdate">
         <div class="editor-label">
            <%:Html.Label("Transfer Date / Deposited Date:") %>
         </div>
         <div class="editor-field">
            <%: Html.TextBox("PaymentDate", "", new { autocomplete = "off", onkeypress = "return false" })%>
         </div>
     </div> 
    <div id="bankname">
         <div class="editor-label">
            <%:Html.Label("Drawn on (Bank Name):") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("Drawn") %> <%--<div class="black">(Cheque should be payable at par in the city of deposit)</div>--%>
         </div>
     </div>

      <div id="branchname">
         <div class="editor-label">
            <%:Html.Label("Our Bank Branch:") %>
         </div>
         <div class="editor-field">
            <%:Html.TextBox("BankBranch") %> <%--<div class="black">(Our Bank branch & city where deposited)</div>--%>
         </div>
     </div>
     <center>
        <input type="submit" id="depositcheque" class="btn" value="Submit" />
     </center>
     <%} %>
            
</div>        
 </body>
 </html>
