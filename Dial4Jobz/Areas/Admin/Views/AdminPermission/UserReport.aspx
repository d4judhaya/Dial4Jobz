﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dial4jobz :: Manage Admin Users</title>
    <link href="../../Content/adminpermission.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />

<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

<link href="<%= Url.Content("~/Areas/Admin/Content/DataTable/css/demo_table_jui.css") %>" rel="Stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/js/jquery.dataTables.min.js") %>" type="text/javascript"></script>
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
         'sAjaxSource': '/Admin/AdminPermission/ListAdminUserEntries?fromDate=' + fromDate + '&toDate=' + toDate,
         "bFilter": true,
         "aoColumns": [{ 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right'}],
         "fnInitComplete": function () { },
         "fnDrawCallback": function () { },
         "fnPreDrawCallback": function () {
             $('.datatable tbody').html('<tr><td colspan="5" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
             return true;
         }
     });
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
    <h2>Admin Users Report</h2>
        <table width="100%">
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
            </tr>
        </table>
        <br /><br />
        <table cellpadding="0" cellspacing="0" border="0" class="datatable display">
            <thead>
                <tr>
                    <th>
                        User Name
                    </th>
                    <th>
                        Candidate Added
                    </th>
                    <th>
                        Employer Added
                    </th>
                    <th>
                        Consultant Added
                    </th>
                    <th>
                        Job Posted
                    </th>
                    <th>
                        Total
                    </th>                    
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="5" class="dataTables_empty">
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


