<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>ContactCandidates</title>
 <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"/>
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/themes/smoothness/jquery-ui-1.8.4.custom.css") %>" type="text/javascript"></script>
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

        $("#DepositedOn").datepicker({
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
 <div class="modal">
    <div class="modal-login-column">
        <div class="header">Call Us to Pickup Cash/Cheque</div>
            
            <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">This Facility is available only in the radius of 6 Km from our Office. Kindly call us at 044 - 44455566</div>
        <%--     <div class="editor-label" style="color:Black;font-family:Times New Roman; font-weight:bold;">Or Speak to Mr. Manikandan at 044 - 44455566</div>--%>

             <% Html.BeginForm("CallUsForPickupCash", "CandidatesVas", FormMethod.Post, new { @id = "pickupcash" });
                { %>
                     <div class="editor-label">
                        <%:Html.Label("Amount") %>
                     </div>
                     <%:Html.TextBox("Amount")%>

                     <div class="editor-label">
                        <%:Html.Label("Dated:") %>
                    </div>

                    <div class="editor-field">
                        <%: Html.TextBox("PaymentDate", "", new { autocomplete = "off", onkeypress = "return false" })%>
                    </div>

                    <div class="editor-label">
                        <%:Html.Label("City") %>
                    </div>

                    <div class="editor-field">
                        <%:Html.TextBox("City") %>
                    </div>

                    <div class="editor-label">
                        <%:Html.Label("Area") %>
                    </div>

                    <div class="editor-field">
                        <%:Html.TextBox("Region") %>
                    </div>

                 <center>
                    <input type="submit" id="callpickupcash" class="btn" value="Submit" />                
                 </center>

             <%} %>

            Dial4Jobz India Private Ltd.<br />
            #32,3rd Cross Street Ext.,<br />
            AGS colony (SEA SIDE),<br />
            Kottivakkam<br />
            CHENNAI - 600041.<br />
            Phone: 044 - 44455566<br />
        
    </div>
 </div>
 </body>
 </html>