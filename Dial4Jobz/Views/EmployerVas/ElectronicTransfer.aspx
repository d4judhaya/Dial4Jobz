<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Internet Banking
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   

      <% Html.BeginForm("CCAVRequest", "EmployerVas", FormMethod.Post, new { @id = "CCAVRequest" });
       { %>

         <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
         <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
         <% bool isLoggedIn = LoggedInConsultant != null; %>
     
        <h3>Transfer Amount : Rs.<%:ViewData["amount"]%> </h3>

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
                <td><input type="hidden" name="cancel_url" value="http://www.dial4jobz.in/employer/matchcandidates" /></td>
             </tr>
             <tr>
                <td colspan="2"><b>Billing information(optional):</b></td>
             </tr>
             <tr>
                <td>Billing Name</td>
                <% if (LoggedInOrganization != null)
                   { %>
                        <td><%:Html.TextBox("billing_name", LoggedInOrganization.Name, new { @readonly = "readonly" })%></td>
                <%} else if(LoggedInConsultant!=null) { %>
                        <td><%:Html.TextBox("billing_name", LoggedInConsultant.Name, new { @readonly = "readonly" })%></td>
                <% } %>
             </tr>
             <tr>
                <td>Billing Address:</td>
                <% if (LoggedInOrganization != null)
                   { %>
                    <td><%:Html.TextBox("billing_address", LoggedInOrganization.Address, new { @readonly = "readonly" })%></td>
               <%} else if(LoggedInConsultant!=null) { %>
                    <td><%:Html.TextBox("billing_address", LoggedInConsultant.Address, new { @readonly = "readonly" })%></td>
               <%} %>
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
                <% if (LoggedInOrganization != null)
                   { %>
                       <td><%:Html.TextBox("billing_zip", LoggedInOrganization.Pincode, new { @readonly = "readonly" })%></td>
                <%} else if(LoggedInConsultant!=null) { %>
                    <td><%:Html.TextBox("billing_zip", LoggedInConsultant.Pincode, new { @readonly = "readonly" })%></td>
                <%} %>
             </tr>
             <tr>
                <td>Billing Country:</td>
                   <td><%:Html.TextBox("billing_country", "India", new { @readonly = "readonly" })%></td>
             </tr>
             <tr>
                <td>Billing Tel:</td>
             <% if (LoggedInOrganization != null)
                { %>
                <td><%:Html.TextBox("billing_tel", LoggedInOrganization.MobileNumber, new { @readonly = "readonly" })%></td>
            <%} else if(LoggedInConsultant!=null) { %>
                <td><%:Html.TextBox("billing_tel", LoggedInConsultant.MobileNumber, new { @readonly = "readonly" })%></td>
            <%} %>
             </tr>
             <tr>
                <td>Billing Email:</td>
                <% if (LoggedInOrganization != null)
                    { %>
                    <td><%:Html.TextBox("billing_email", LoggedInOrganization.Email, new { @readonly = "readonly" })%></td>
                <%} else if(LoggedInConsultant!=null) { %>
                     <td><%:Html.TextBox("billing_email", LoggedInConsultant.Email, new { @readonly = "readonly" })%></td>
                <%} %>
             </tr>
             <tr>
               <td colspan="2"><b>Shipping Information(optional):</b></td>
             </tr>
             <tr>
                <td>Shipping Name</td>
              <% if (LoggedInOrganization != null)
                { %>
                    <td><%:Html.TextBox("delivery_name", LoggedInOrganization.Name, new { @readonly = "readonly" })%></td>
              <%} else if(LoggedInConsultant!=null) { %>
                     <td><%:Html.TextBox("delivery_name", LoggedInConsultant.Name, new { @readonly = "readonly" })%></td>
              <%} %>
             </tr>
             <tr>
                <td>Shipping Address:</td><br />
                    <% if (LoggedInOrganization != null)
                    { %>
                         <td><%:Html.TextBox("delivery_address", LoggedInOrganization.Address, new { @readonly = "readonly" })%></td>
                    <%} else if(LoggedInConsultant!=null) { %>
                        <td><%:Html.TextBox("delivery_address", LoggedInConsultant.Address, new { @readonly = "readonly" })%></td>
                    <%} %>
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
                 <% if (LoggedInOrganization != null)
                { %>
                    <td><%:Html.TextBox("delivery_zip", LoggedInOrganization.Pincode, new { @readonly = "readonly" })%></td>
                <%} else if(LoggedInConsultant!=null) { %>
                    <td><%:Html.TextBox("delivery_zip", LoggedInConsultant.Pincode, new { @readonly = "readonly" })%></td>
                <%} %>
             </tr>
             <tr>
                <td>shipping Country:</td>
                <td><input type="text" name="delivery_country" value="India"/></td>
             </tr>
             <tr>
                <td>Shipping Tel:</td>
                 <% if (LoggedInOrganization != null)
                 { %>
                     <td><%:Html.TextBox("delivery_tel",LoggedInOrganization.MobileNumber) %></td>
                <%} else if(LoggedInConsultant!=null) { %>
                     <td><%:Html.TextBox("delivery_tel",LoggedInConsultant.MobileNumber) %></td>
                <%} %>
             </tr>

             <tr>
              <td>Customer Id:</td>
               <% if (LoggedInOrganization != null)
                { %>
                    <td><%:Html.TextBox("customer_identifier", LoggedInOrganization.Id, new { @readonly = "readonly" })%></td>
                <%} else if(LoggedInConsultant!=null) { %>
                    <td><%:Html.TextBox("customer_identifier", LoggedInConsultant.Id, new { @readonly = "readonly" })%></td>
                <%} %>
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
    <li><a title ="contact him">Ganesan(Business Head)</a></li>
    <li>Mobile Number:+91-9381516777 </li>
    <li>Contact Number:044 - 44455566 </li>
  <%--  <li>E-Mail:ganesan@dial4Jobz.com </li>--%>
    </ul>    
    </div>

    <div><h5>Customer Support</h5>
    <ul>
    <li><a title ="contact him">Manikandan</a></li>
   <%-- <li>Mobile Number:+91-9841087952 </li>--%>
    <li>Contact Number:044 - 44455566 </li>
    <%--<li>E-Mail:mani@jobspoint.com </li>--%>
    </ul>    
    </div>
</asp:Content>
