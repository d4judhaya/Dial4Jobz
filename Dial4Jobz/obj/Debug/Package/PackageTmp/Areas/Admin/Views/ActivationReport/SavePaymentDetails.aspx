<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.OrderPayment>" %>


<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
    Payment Details
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <% Html.BeginForm("SavePaymentDetails", "ActivationReport", FormMethod.Post, new { @id = "savedetails" });
     { %>
 
   <center> <h3>(1) Enter Deposit Cheque / Cash Details </h3></center> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; You can check the activated List:  <%:Html.ActionLink("Employer Activated Report", "ActivatedReport", "ActivationReport", new { target = "_blank" })%> &nbsp;&nbsp;| &nbsp;&nbsp;&nbsp; <%:Html.ActionLink("Candidate Activated Report", "CandidateActivatedReport", "ActivationReport", new { target = "_blank" })%> <br />

   <center> <h3> Activation Report: <%: Html.ActionLink("Employer Activation", "Index", "ActivationReport", new { target = "_blank" })%> &nbsp;&nbsp;| &nbsp;&nbsp;&nbsp; <%:Html.ActionLink("Candidate Activation Report", "CandidateVasActivation", "ActivationReport", new { target = "_blank" })%></h3></center>

    <div class="editor-label">
        <%:Html.Label("Instrument Type:") %>
    </div>

    <div class="editor-field">
        <select id="ddlInstrumentType" name="ddlInstrumentType">
            <option value="8">--Select Type--</option>
            <option value="0">Cheque</option>
            <option value="1">Draft</option>
            <option value="2">Cash Deposit</option>
            <option value="3">Inter Bank</option>
            <option value="4">NEFT</option>
            <option value="5">IMPS</option>
            <option value="6">Pickup Cash</option>
            <option value="7">Cash Pickup By office</option>
        </select>
     </div>
     
      <div class="black">
         <%:Html.Label("Order Number:")%> 
      </div>

     <div class="editor-field">
        <%:Html.DropDownList("OrderIds","--Select Order Id--") %>
     </div>

     <div class="editor-label">
        <%:Html.Label("For: ***") %>
     </div>

     <div class="editor-field">
        <%:Html.DropDownList("Amount","--Select Amount--") %>
     </div>

     <div id="referenceNo">
          <div class="editor-label">
            <%:Html.Label("Cheque Number / Draft Number :") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("ReferenceNumber") %>
         </div>
     </div>

     <div id="transferdate">
          <div class="editor-label">
            <%:Html.Label("Transfer Date / Deposited Date:") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("DepositedOn", "", new { autocomplete = "off", onkeypress = "return false" })%>
         </div>
     </div>

     <div id="transferrefernece">
         <div class="editor-label">
            <%:Html.Label("Transfer Reference") %>
         </div>

         <div class="editor-field">
            <%: Html.TextBox("TransferRefernece") %>
         </div>
     </div>

     <div id="bankname">
         <div class="editor-label">
            <%:Html.Label("Bank Name") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("BankName") %> <div class="black">(Cheque should be payable at par in the city of deposit)</div>
         </div>
     </div>

     <div id="bankbranch">
          <div class="editor-label">
            <%:Html.Label("Bank Branch") %>
         </div>

         <div class="editor-field">
            <%:Html.TextBox("BankBranch") %> <div class="black">
            <%--(Cheque should be payable at par in the city of deposit)--%></div>
         </div>
     </div>

               
    

     <div id="cashpickup">
      <center><h3>(2)Cash Pickup Details</h3></center>
        <div class="editor-label">
            <%:Html.Label("City") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("City",ViewData["City"]) %>
        </div>

        <div class="editor-label">
            <%:Html.Label("Area") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Region",ViewData["Region"]) %>
        </div>
    </div>

   
    
    <div id="cashcollect">
     <h3>Enter Cash Collect Details</h3><span class="red">*</span>
        <div class="editor-label">
            <%:Html.Label("Collected On") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("CollectedOn", "", new { autocomplete = "off", onkeypress = "return false" })%>
        </div>

        <div class="editor-label">
            <%:Html.Label("Collected By") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("CollectedBy") %>
        </div>
    </div>

      <input type="submit" id="Submit1" class="btn" value="Submit" />      
  <%--  <input type="submit" id="callpickupcash" class="btn" value="Submit" />       --%>         
  
  <%} %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <%--<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/themes/smoothness/jquery-ui-1.8.4.custom.css") %>" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#depositcheque").click(function () {
                alert('Thanks for Submitting the details. On Receipt of Payment, Your plan will be activated within 1 working day.');
            });

            //date picker

            $("#PaymentDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm/dd/yy",
                changeMonth: true,
                changeYear: true
                //yearRange: "1930:1995"

            });

            $("#CashPickUpDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm/dd/yy",
                changeMonth: true,
                changeYear: true

            });

            $("#DepositedOn").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm/dd/yy",
                changeMonth: true,
                changeYear: true

            });

            $("#CollectedOn").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "mm/dd/yy",
                changeMonth: true,
                changeYear: true

            });
            

        });
  
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#referenceNo").hide();
            $("#transferdate").hide();
            $("#transferrefernece").hide();
            $("#bankname").hide();
            $("#bankbranch").hide();
            $("#cashpickup").hide();
            $("#cashcollect").hide();
            $("#Submit1").hide();
            $("#callpickupcash").hide();

            $('#ddlInstrumentType').change(function () {
                var ddlType = $("#ddlInstrumentType").val();
                if ((ddlType == "0") || (ddlType == "1")) {
                    $("#referenceNo").show();
                    $("#transferdate").show();
                    $("#bankname").show();
                    $("#bankbranch").show();
                    $("#Submit1").show();

                    //to hide the next select

                    $("#transferrefernece").hide();
                    //$("#callpickupcash").hide();
                    $("#cashpickup").hide();
                    $("#cashcollect").hide();
                }

                else if (ddlType == "2") {
                    $("#transferdate").show();
                    $("#bankbranch").show();
                    $("#Submit1").show();
                    //$("#callpickupcash").show();

                    //to hide the next select
                    $("#referenceNo").hide();
                    $("#transferrefernece").hide();
                    $("#bankname").hide();
                    // $("#Submit1").hide();
                    $("#cashpickup").hide();
                    $("#cashcollect").hide();
                }

                else if (ddlType == "3") {
                    $("#transferdate").show();
                    $("#transferrefernece").show();
                    $("#Submit1").show();

                    //to hide the next select
                    $("#referenceNo").hide();
                    $("#bankname").hide();
                    $("#bankbranch").hide();
                    $("#cashpickup").hide();
                    $("#cashcollect").hide();
                }
                else if (ddlType == "4") {
                    $("#transferrefernece").show();
                    $("#transferdate").show();
                    $("#bankname").show();
                     $("#Submit1").show();

                    //to hide the next select
                    $("#referenceNo").hide();
                    $("#bankbranch").hide();
                    $("#cashpickup").hide();
                    $("#cashcollect").hide();

                }

                else if (ddlType == "5") {
                    $("#transferrefernece").show();
                    $("#transferdate").show();
                    $("#Submit1").show();

                    //to hide the next select
                    $("#referenceNo").hide();
                    $("#bankname").hide();
                    $("#bankbranch").hide();
                    $("#cashpickup").hide();
                    $("#cashcollect").hide();
                }

                else if (ddlType == "6") {
                    $("#cashpickup").show();
                    $("#cashcollect").show();
                    $("#callpickupcash").show();
                    $("#Submit1").show();

                    //to hide the next select
                    //$("#Submit1").hide();
                    $("#referenceNo").hide();
                    $("#transferdate").hide();
                    $("#transferrefernece").hide();
                    $("#bankname").hide();
                    $("#bankbranch").hide();

                }

                else if (ddlType == "7") {
                    $("#cashpickup").show();
                    $("#cashcollect").show();
                    $("#callpickupcash").show();
                    $("#Submit1").show();

                    //to hide the next select
                    //$("#Submit1").hide();
                    $("#referenceNo").hide();
                    $("#transferdate").hide();
                    $("#transferrefernece").hide();
                    $("#bankname").hide();
                    $("#bankbranch").hide();

                }
            });
           
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="NavContent" runat="server">
    <%Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>


