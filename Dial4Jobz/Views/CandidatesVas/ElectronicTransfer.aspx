<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Internet Banking
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   

      <% Html.BeginForm("CCAVRequest", "candidates/candidatesvas", FormMethod.Post, new { @id = "CCAVRequest" });
       { %>

        <%--<% Dial4Jobz.Models.Organization LoggedInCandidate = (Dial4Jobz.Models.Organization)ViewData["LoggedInCandidate"]; %>--%>

        <% Dial4Jobz.Models.Candidate LoggedInCandidate = (Dial4Jobz.Models.Candidate)ViewData["LoggedInCandidate"];%>
    
      <%--    <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">
            You can transfer your payment by RTGS/NEFT  to our ICICI Bank account or INDIAN Bank account held at Chennai.
           </div>
          <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">ICICI Bank</div>
            Beneficiary Name: Dial4Jobz India Private Ltd<br />
            Bank Name: ICICI BANK<br />
            Branch Name: Cenotaph Road (Teynampet)<br />
            Beneficiary Account Number: 000105018492 <br />
            Account Type: Current Account<br />
            Account Holder's Name: Dial4Jobz India Private Ltd<br />
            Bank Address : No.1, Cenotaph Road, Chennai – 600018<br />
            RTGS/NEFT/ISFC Code: ICIC0000001<br />
         
        <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">INDIAN Bank</div>
            Beneficiary Name: Dial4Jobz India Private Ltd<br />
            Bank Name: INDIAN Bank<br />
            Branch Name: Thiruvanmiyur<br />
            Beneficiary Account Number: 782930321 <br />
            Account Type: Current Account<br />
            Account Holder's Name: Dial4Jobz India Private Ltd<br />
            Bank Address : 5,West Tank square Thiruvanmiyur,Chennai-600041<br />
            RTGS/NEFT/ISFC Code: IDIB000T044<br />--%>
        
        <%--<span class="red">Please fill up all required form fields accurately and submit</span>--%>
     
        <h3>Transfer Amount : Rs.<%:ViewData["amount"]%> </h3>

                  
         <%--<table width="40%" height="100" border='1' align="center"></table>--%>
        <table width="40%" height="100" border='1' align="center">

        <caption><font size="4" color="blue"><b>Payment Details</b></font></caption>
             <tr> 
                <td>Parameter Name:</td>
                <td>Parameter Value:</td>
             </tr>
             <tr> 
                <td colspan="2"> <b>Compulsory information</b></td>
             </tr>
             <tr>
            <%--    <td>Merchant Id</td>--%>
                <td><input type="hidden" name="merchant_id" id="merchant_id" value="34702"/></td>
             </tr>
             <tr>
                <td>Order Id</td>
            
               <td><%:Html.TextBox("order_id", ViewData["OrderNo"].ToString(), new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Amount</td>
                <td><%:Html.TextBox("amount", ViewData["Amount"].ToString(), new { @readonly = "readonly" })%></td>
                <%--<td><%:Html.TextBox("amount", "1.00")%></td>--%>
             </tr>
             <tr>
                <td>Currency</td>
                <td><input type="text" name="currency" value="INR" readonly="readonly"/></td>
             </tr>
             <tr>
                <%--<td>Redirect URL</td>--%>
                    <td><input type="hidden"  name="redirect_url" value="http://www.dial4jobz.in/Employer/EmployerVas/CCAVResponse"/></td>
                    <%--<td><input type="text" name="redirect_url" value="http://192.168.0.89/MCPG.ASP.net.2.0.kit/ccavResponseHandler.aspx"/></td>--%>
             </tr>
	         <tr>
               <%-- <td>Cancel URL</td>--%>
               <%-- <td><input type="hidden" name="cancel_url" value="http://192.168.0.96/mcpg_new/iframe/ccavResponseHandler.php" /></td>--%>
                  <td><input type="hidden" name="cancel_url" value="http://www.dial4jobz.in/employer/matchcandidates" /></td>
             </tr>
             <tr>
                <td colspan="2"><b>Billing information(optional):</b></td>
             </tr>
             <tr>
                <td>Billing Name</td>
             
                <td><%:Html.TextBox("billing_name", LoggedInCandidate.Name, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing Address:</td>
         
               <td><%:Html.TextBox("billing_address", LoggedInCandidate.Address, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing City:</td>
           
              <td><%:Html.TextBox("billing_city", "Chennai", new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing State:</td>
          
                <td><%:Html.TextBox("billing_state", "TamilNadu", new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing Zip:</td>
               
                <td><%:Html.TextBox("billing_zip", LoggedInCandidate.Pincode, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing Country:</td>
              
                <td><%:Html.TextBox("billing_country", "India", new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing Tel:</td>
            
                <td><%:Html.TextBox("billing_tel", LoggedInCandidate.ContactNumber)%></td>
             </tr>
             <tr>
                <td>Billing Email:</td>
                <td><%:Html.TextBox("billing_email", LoggedInCandidate.Email)%></td>
             </tr>
             <tr>
               <td colspan="2"><b>Shipping Information(optional):</b></td>
             </tr>
             <tr>
                <td>Shipping Name</td>
             
                <td><%:Html.TextBox("delivery_name", LoggedInCandidate.Name, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Shipping Address:</td><br />
                <td><%:Html.TextBox("delivery_address", LoggedInCandidate.Address, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>shipping City:</td>
           
               <td><%:Html.TextBox("delivery_city","Chennai") %></td>
             </tr>
             <tr>
                <td>shipping State:</td>
                 <td><%:Html.TextBox("delivery_state","TamilNadu") %></td>
             </tr>
             <tr>
                <td>shipping Zip:</td>
                <td><%:Html.TextBox("delivery_zip", LoggedInCandidate.Pincode, new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>shipping Country:</td>
                <td><input type="text" name="delivery_country" value="India"/></td>
             </tr>
             <tr>
                <td>Shipping Tel:</td>
                
                <td><%:Html.TextBox("delivery_tel",LoggedInCandidate.ContactNumber) %></td>
             </tr>

             <tr>
              <td>Customer Id:</td>
              <td><%:Html.TextBox("customer_identifier", LoggedInCandidate.Id, new { @readonly = "readonly" })%></td>
             </tr>

             <tr>
          	    <td></td>
			    <td><input type="submit" id="electrans" value="Submit" /></td>
               <%-- <td><input type="submit" id="electrans" class="btn" value="Submit" /></td>--%>
             </tr>

             <tr>
                <%--<td>Merchant Param1</td>--%>
                <td><input type="hidden" name="merchant_param1" value="additional Info."/></td>
             </tr>
             <tr>
                <%--<td>Merchant Param2</td>--%>
                <td><input type="hidden" name="merchant_param2" value="additional Info."/></td>
             </tr>
	        <tr>
                <%--<td>Merchant Param3</td>--%>
                <td><input type="hidden" name="merchant_param3" value="additional Info."/></td>
             </tr>
	        <tr>
                <%--<td>Merchant Param4</td>--%>
                <td><input type="hidden" name="merchant_param4" value="additional Info."/></td>
             </tr>
	        <tr>
                <%--<td>Merchant Param5</td>--%>
                <td><input type="hidden" name="merchant_param5" value="additional Info."/></td>
            </tr>
             <tr>
                <%--<td>Promo Code</td>--%>
                <td><input type="hidden" name="promo_code" /></td>
             </tr>
                      
              <% } %>
        </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/themes/smoothness/jquery-ui-1.8.4.custom.css") %>" type="text/javascript"></script>
      <script type="text/javascript">
          $(document).ready(function () {
              $("#electrans").click(function () {
                  //alert('your plan will be activated on successful receipt of payment within maximum of one working day');
                  alert('Your Plan details has been updated. Your plan will be activated on successful receipt of payment within maximum of one working day');
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
      
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
     <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div><h5>Sales Enquiries</h5>
    <ul>
    <li><a title ="contact him">Vivek</a></li>
    <%--<li>Mobile Number:+91-9381516777 </li>--%>
    <li>Contact Number:044 - 44455566 </li>
 <%--   <li>E-Mail:ganesan@dial4Jobz.com </li>--%>
    </ul>    
    </div>

    <div><h5>Customer Support</h5>
    <ul>
    <li><a title ="contact him">Manikandan</a></li>
    <%--<li>Mobile Number:+91-9841087952 </li>--%>
    <li>Contact Number:044 - 44455566 </li>
    <%--<li>E-Mail:mani@jobspoint.com </li>--%>
    </ul>    
    </div>
</asp:Content>
