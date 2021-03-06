﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dial4jobz :: Consultants Activation</title>
    <link href="../../Content/adminpermission.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="../../Content/jquery.fancybox-1.3.1.css" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js" type="text/javascript"></script>

<link href="<%= Url.Content("~/Areas/Admin/Content/DataTable/css/demo_table_jui.css") %>" rel="Stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/js/jquery.dataTables.min.js") %>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.fancybox-1.3.1.pack.js")%>" type="text/javascript"></script>

<script type="text/javascript">

    $(document).ready(function () {
        Databind("0", "", "");

        $("#FromDate").datepicker({ changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            onSelect: function (selected) {
                $('#ToDate').datepicker('option', 'minDate', selected)
            }
        });

        $("#ToDate").datepicker({ changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy"
        });

        $('#FromDate,#ToDate').bind('keypress keydown', function (e) {
            var tag = e.target.tagName;
            if (e.which === 8 || e.which === 46) {
                $(this).val('');
                $(this).datepicker("hide");
                $(this).blur();
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
        });

        $(".ActionPopup").fancybox({
            'hideOnContentClick': false,
            'titleShow': false,
            'scrolling': 'yes'
        });
    });

    function Databind(Pdestroy, fromDate, toDate) {

        var destroy = false;
        if (Pdestroy == "1")
            destroy = true;

        var oTable = $('.datatable').dataTable({
            "bJQueryUI": true,
            'bServerSide': true,
            "bDestroy": destroy,
            "iDisplayLength": 10,
            "sPaginationType": "full_numbers",
            'sAjaxSource': '/Admin/ActivationReport/ListActivationCandidateReports?fromDate=' + fromDate + '&toDate=' + toDate,
            "bFilter": true,
            "aaSorting": [[3, 'desc']],
            "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [8]}],
            "aoColumns": [{ 'sWidth': '10%', 'sClass': 'left' }, { 'sWidth': '15%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '15%', 'sClass': 'center' }, { 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '10%', 'sClass': 'left'}],
            "fnInitComplete": function () { },
            "fnDrawCallback": function () {

                $(".ActionPopup").fancybox({
                    'hideOnContentClick': false,
                    'titleShow': false,
                    'scrolling': 'yes',
                    'onComplete': function () { }
                });
                return false;

            },
            "fnPreDrawCallback": function () {
                $('.datatable tbody').html('<tr><td colspan="7" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
                return true;
            }
        });
             
          $("<div style='width:38px; float:right;'><a style='margin-right:10px;' href='javascript:void(0)' onclick='javascript:SendRemainder();return false;'><img  alt='activate' src='<%=Url.Content("~/Areas/Admin/Content/Images/send_email.jpg")%>' /></a></div>").appendTo('.dataTables_filter'); 
          $("<div style='width:38px; float:right;'><a style='margin-right:10px;' href='javascript:void(0)' onclick='javascript:Activate();return false;'><img  alt='activate' src='<%=Url.Content("~/Areas/Admin/Content/Images/activate1.jpg")%>' /></a></div>").appendTo('.dataTables_filter');
    }

    function Search() {
        if ($("#FromDate").val() != "" && $("#ToDate").val() == "") {
            alert("Please Enter To Date");
            $("#ToDate").focus();
            return false;
        }

        if ($("#ToDate").val() != "" && $("#FromDate").val() == "") {
            alert("Please Enter From Date");
            $("#FromDate").focus();
            return false;
        }

        Databind("1", $("#FromDate").val(), $("#ToDate").val());
    }

    function Uncheck(obj) {
        if (!obj.checked) {
            document.getElementById('checkall').checked = false;
        }

    }

    function Checkall() {

        var check = false;
        $("input:checkbox[name=checkall]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });

        if (check == true) {
            $("input:checkbox[name=ActivateOrder]").each(function () {
                this.checked = true;
            });
        }
        else {
            $("input:checkbox[name=ActivateOrder]").each(function () {
                this.checked = false;
            });
        }

    }

    function Activate() {
        var orderId = "";
        var count = 0;
        $("input:checkbox[name=ActivateOrder]:checked").each(function () {
            if (count == 0) {
                orderId = $(this).val();
            }
            else {
                orderId += "," + $(this).val();
            }
            count = count + 1;
        });

        if (count == 0) {
            alert("Please select atleast one record to delete");
            return false;
        }

        if (!ConfirmActivate())
            return false;

        $("#loading").show();
        var data = "orderIds=" + orderId;
        var url = '<%= Url.Action("ActivateVAS", "ActivationReport","Admin") %>';

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            success: function (data) {
                $("#loading").hide();
                if(data.error=="3")
                {
                    alert(data.message);
                    //Databind("3", $("#FromDate").val(), $("#ToDate").val());
                    document.getElementById('checkall').checked=false;
                     window.location = "/Admin/ActivationReport/SavePaymentDetails?lstOrderIds=" + orderId;
                }
                else if (data.error != "1") {
                    alert(data.message);
                    Databind("1", $("#FromDate").val(), $("#ToDate").val());
                    document.getElementById('checkall').checked = false;
                }   
                else {
                    alert(data.message);
                }
            }
        });
        return false;

    }

     function ConfirmSend() {

        var check = confirm("Do you want to send Remainder for selected Orders ?");
        if (check == true)
            return true;
        else
            return false;

    }

     function SendRemainder() {
        var orderId = "";
        var count = 0;
        $("input:checkbox[name=ActivateOrder]:checked").each(function () {
            if (count == 0) {
                orderId = $(this).val();
            }
            else {
                orderId += "," + $(this).val();
            }

            count = count + 1;
        });

        if (count == 0) {
            alert("Please select atleast one record to delete");
            return false;
        }

        if (!ConfirmSend())
            return false;

        $("#loading").show();
        var data = "OrderIds=" + orderId;
        var url = '<%= Url.Action("CandidateReminder", "AdminHome","Admin") %>';

        $.ajax({
            type: "GET",
            url: url,
            data: data,
            dataType: "json",
            success: function (data) { 
                $("#loading").hide();
                alert(data.message);
                document.getElementById('checkall').checked=false;
                Databind("1", $("#FromDate").val(), $("#ToDate").val());
                
            },
            error: function (xhr, status, error) {
                alert(response.Message);
            }
        });
        return false;

    }

    function ConfirmActivate() {
        var check = confirm("Do you want to activate ?");
        if (check == true)
            return true;
        else
            return false;

    }
    

    function ActivateVAS(OrderId) {

        $.ajax({
            url: '/Admin/ActivationReport/ActivateVAS',
            type: 'POST',
            data: { 'OrderId': OrderId },
            datatype: 'json',
            success: function (response) {
                if (response.Success) {
                    alert(response.Message);
                    Databind("1", $("#FromDate").val(), $("#ToDate").val());
                }
            },
            error: function (xhr, status, error) {
                alert(response.Message);
            }
        });
    }

    function ConfirmDelete() {

        var check = confirm("Do you want to delete ?");
        if (check == true)
            return true;
        else
            return false;

    }

    function DeleteOrder(OrderId) {
    
    if (!ConfirmDelete())
        return false;

        $.ajax({
            url: '/Admin/ActivationReport/DeleteOrder',
            type: 'POST',
            data: { 'OrderId': OrderId },
            datatype: 'json',
            success: function (response) {
                if (response.Success) {
                    alert(response.Message);
                    Databind("1", $("#FromDate").val(), $("#ToDate").val());
                }
            },
            error: function (xhr, status, error) {
                alert(response.Message);
            }
        });
    }


 
 </script>

</head>
<body>
<div id="wrapper">  
        <div id="page">
            <div id="header">

                <div id="logo">
                    <a href="/"><img src="<%=Url.Content("~/Content/Images/dial4jobz_logo.png")%>" alt="Dial4Jobz logo" title="Dial4Jobz Logo" /></a> 
                </div>
              
                <div id="menu-bar"> 
                <% Html.RenderPartial("NavAdmin"); %>
                 </div> 
            </div> 
    <div id="main" style="width:798px;">   

    <h2>Candidate Vas Activation</h2>

     <table width="100%">
        <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
            <tr>
                <td style="width:35%;" align="right" valign="middle">
                        From &nbsp; <%: Html.TextBox("FromDate", "", new { autocomplete = "off", onkeypress = "return false" }) %>
                    </td>
                    <td style="width:15%;" align="right" valign="middle" class="editor-field">
                        To &nbsp;
                    </td>
                    <td style="width:25%;" align="left" valign="middle" >
                        <%: Html.TextBox("ToDate", "", new { autocomplete = "off", onkeypress = "return false" }) %>
                    </td>
                    <td style="width:35%;" align="center" valign="middle" class="editor-field">
                        <input type="submit" id="contactbtn" value="Search" onclick="javascript:Search();return false;" />  <br />      
                    </td>

                       <%var sumAmount = _vasRepository.GetTotalPendingAmountForCandidate();%>
                     <%var datewiseAmount = _vasRepository.GetSumActivationForCandidate(); %>

                     <h3>Total Pending Amount:Rs. </h3><b><%:sumAmount%></b><br />

                     <h3>Today Pending Amount: Rs. <b><%:datewiseAmount %></b></h3>
            </tr>
        </table>
        <br /><br />
        <table cellpadding="0" cellspacing="0" border="0" class="datatable display">
            <thead>
                <tr>
                    <th>
                        Candidate ID
                    </th>
                    <th>
                        Name
                    </th>
                    <th>
                        PlanId
                    </th>
                    <th>
                        Order Number
                    </th>
                    <th>
                        Subscribed Date
                    </th>
                    <th>
                        Amount
                    </th>
                  
                    <th class="ui-state-default left" role="columnheader">
                       Subs.By
                    </th>

                     <th class="ui-state-default left" role="columnheader">
                        DeleteOrder
                    </th>
                      <th>
                        <input type="checkbox" id="checkall" name="checkall" onclick="javascript:Checkall();" />
                    </th>

                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="7" class="dataTables_empty">
                        <img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"
                            height="50" />
                    </td>
                </tr>
            </tbody>
        </table>
 </div>
 <div id="loading">
  <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
</div>

</div>
</div>
</body>
</html>



