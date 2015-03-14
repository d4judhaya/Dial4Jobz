<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dial4jobz :: Manage Channel Partners</title>
    <link href="../../Content/adminpermission.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="../../Content/jquery.fancybox-1.3.1.css" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

<link href="<%= Url.Content("~/Areas/Admin/Content/DataTable/css/demo_table_jui.css") %>" rel="Stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/js/jquery.dataTables.min.js") %>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.fancybox-1.3.1.pack.js")%>" type="text/javascript"></script>



<script type="text/javascript">

$(document).ready(function () {
    Databind("0");

     $(".ActionPopup").fancybox({
         'hideOnContentClick': false,
         'titleShow': false,
         'scrolling': 'yes'
     });    
 });

 function Databind(Pdestroy) {

     var destroy = false;
     if (Pdestroy == "1")
         destroy = true;

     var oTable = $('.datatable').dataTable({
         "bJQueryUI": true,
         'bServerSide': true,
         "bDestroy": destroy,
         "iDisplayLength": 10,
         "sPaginationType": "full_numbers",
         'sAjaxSource': '<%= Url.Action("ListChannelPartners", "Channel") %>',
         "bFilter": true,
         "aaSorting": [[2, 'desc']], 
         "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [0, 2, 7, 8]}],
         //"aoColumns": [{ 'sWidth': '15%', 'sClass': 'left' }, { 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '35%', 'sClass': 'left' }, { 'sWidth': '5%', 'sClass': 'center' }, { 'sWidth': '5%', 'sClass': 'center'}],
         "fnRowCallback": function (nRow, aData, iDisplayIndex) {
	                var oSettings = oTable.fnSettings();
	                $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);
	                return nRow;
	            },
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
             $('.datatable tbody').html('<tr><td colspan="9" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
             return true;
         }
     });    

     $("<div style='width:38px; float:right;'><a style='margin-right:10px;' href='javascript:void(0)' onclick='javascript:Delete();return false;'><img alt='Delete' src='<%=Url.Content("~/Areas/Admin/Content/Images/icn_trash.png")%>' /></a></div>").appendTo('.dataTables_filter');
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
        $("input:checkbox[name=DeletePartnerId]").each(function () {
            this.checked = true;
        });
    }
    else {
        $("input:checkbox[name=DeletePartnerId]").each(function () {
            this.checked = false;
        });
    }

}

function Delete() {
    var partnerId = "";
    var count = 0;
    $("input:checkbox[name=DeletePartnerId]:checked").each(function () {
        if (count == 0) {
            partnerId = $(this).val();
        }
        else {
            partnerId += "," + $(this).val();
        }
        count = count + 1;
    });

    if (count == 0) {
        alert("Please select atleast one record to delete");
        return false;
    }

    if (!ConfirmDelete())
        return false;
    

    $("#loading").show();  
    var data = "partnerIds=" + partnerId;
    var url = '<%= Url.Action("Delete", "Channel", "Admin") %>';
    
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            $("#loading").hide();  
            if (response.Success == true) {
                alert(response.Message);
                Databind("1");
                document.getElementById('checkall').checked = false;
            }
            else {
                alert(response.Message);
            }
        },
        error: function (xhr, status, error) {
            $("#loading").hide();
            alert(error);
        }
    });
    return false;

}

function ConfirmDelete() {

    var check = confirm("Do you want to delete ?");
    if (check == true)
        return true;
    else
        return false;

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
    <h2>Manage Channel Partners</h2>
    <div style="text-align:right;">
    <%= Html.ActionLink("Add New Partner", "CreatePartner", "", new { @class = "ActionPopup" })%>
    </div>
        <table cellpadding="0" cellspacing="0" border="0" class="datatable display">
            <thead>
                <tr>
                    <th>
                        S.No
                    </th>
                    <th>
                        User Name
                    </th>
                    <th>
                        Password
                    </th>
                    <th>
                        Email
                    </th>
                    <th>
                        Contact No
                    </th>
                    <th>
                        Address
                    </th>
                    <th>
                        Pincode
                    </th>
                    <th>
                        Edit
                    </th>
                    <th>
                        <input type="checkbox" id="checkall" name="checkall" onclick="javascript:Checkall();" />
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="9" class="dataTables_empty">
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



