﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Payment
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
    <% Html.BeginForm("Payment", "EmployerVas", FormMethod.Post, new { @id = "Payment" }); { %>

    <% Dial4Jobz.Models.Organization LoggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
    <% bool isLoggedIn = LoggedInOrganization != null; %>
    <% bool isConsultLoggedIn = LoggedInConsultant != null; %>

    <table border="0" cellspacing="0" cellpadding="0" class="dataTable">
        <thead>
        <tr>
            <th class="dataTableHeader"></th>
            <th class="dataTableHeader">Select Payment Mode</th>
            <th class="dataTableHeader" style="white-space: nowrap">Subscribe the Payment</th>
        </tr>
        </thead>
        <tbody>

        <tr class="odd_row">
        <td><input value="0" id="Radio1" name="pmode" type="radio"/></td>
        <td style="white-space: nowrap">Master /Visa Credit / Debit Card through Payment gateway</td>
        <td>     
         <% if (isLoggedIn == true || isConsultLoggedIn == true)
               { %>
                 <input type="image" src="../../Content/Images/credit card.png" class="Featured-plan"  />
            <%} else { %>
                <a class="login"  href="<%=Url.Content("~/login")%>" title="Viewing Job details is free. Please Login to view or Register"><img src="/Content/Images/credit card.png" class="Featured-plan"></a>
            <%} %>
        </td>
        </tr>

        
        <tr class="even_row">
            <td><input value="1" id="masterVisaICICI" name="pmode" type="radio"/></td>
        
            <td>Pay through Internet Banking</td>
            <td>
            <% if (isLoggedIn == true || isConsultLoggedIn== true)
               { %>
                  <a id="electronicTransfer"  href="<%: Url.Action("ElectronicTransfer", "EmployerVas","Employer") %>"><img src="/Content/Images/ebanking.png" class="Featured-plan"></a>                   
            <%} else { %>
                <a class="login"  href="<%=Url.Content("~/login")%>" title="Viewing Job details is free. Please Login to view or Register"><img src="/Content/Images/ebanking.png" class="Featured-plan"></a>
            <% } %>
            </td>
        </tr>

        <tr class="odd_row">
            <td><input value="2" id="CCAvenue" name="pmode" type="radio"/></td>
        
           <%-- <td>Deposit /send us a Cheque/Cash/Draft/</td>--%>
           <td>Deposit /Send /NEFT Transfer</td>
            <td>
                <a class="callpickupcash" href="<%: Url.Action("DepositChequeDraft", "EmployerVas","Employer") %>"><img src="/Content/Images/Cheque Button.jpg" class="Featured-plan"></a>
            </td>
        </tr>

       <tr class="even_row">
        <td><input value="3" id="CCAvenue1" name="pmode" type="radio"/></td>
        <td>Call us to pickup Cash / Cheque</td>
    
       <td>
        <a class="callpickupcash" href="<%: Url.Action("CallUsforPickupCash", "EmployerVas","Employer") %>"><img src="/Content/Images/Locate Us Button.jpg" class="Featured-plan"></a>            
      
       </td>
       </tr>

       <tr class="odd_row">
        <td><input value="4" id="CCAvenue2" name="pmode" type="radio"/></td>
       
        <td>Call us to Pay through Phone from your Credit Card</td>
        <td>
            <a class="callpickupcash" href="<%: Url.Action("PayThroughPhoneCreditCard", "EmployerVas","Employer") %>"><img src="/Content/Images/Pay by Phone Button.jpg" class="Featured-plan"></a>        
    
        </td>
        </tr>
       </tbody>
       </table>
       
   <% } %>
   <% Html.EndForm();%>
   </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">

<div><h5>Sales Enquiries</h5>
    <ul>
        <li>Mobile Number:+91-9381516777 </li>
        <li>Contact Number:044 - 44455566 </li>
        <li>E-Mail:smc@dial4Jobz.com </li>
    </ul>    
    </div>

    <div><h5>Customer Support</h5>
    <ul>
        <li><a title ="contact him">Manikandan</a></li>
        <li>Contact Number:044 - 44455566 </li>
        <li>E-Mail:smo@jobspoint.com </li>
    </ul>    
    </div>
</asp:Content>
