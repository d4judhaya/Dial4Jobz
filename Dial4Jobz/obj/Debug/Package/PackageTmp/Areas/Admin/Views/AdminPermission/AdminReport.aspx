<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dial4jobz :: Manage Admin Users</title>
    <link href="../../Content/adminpermission.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <link href="../../Content/jquery.fancybox-1.3.1.css" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js" type="text/javascript"></script>

<link href="<%= Url.Content("~/Areas/Admin/Content/DataTable/css/demo_table_jui.css") %>" rel="Stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Areas/Admin/Content/DataTable/js/jquery.dataTables.min.js") %>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.fancybox-1.3.1.pack.js")%>" type="text/javascript"></script>

    </head>
<body>

<% 
    Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.Page.User.Identity.Name).FirstOrDefault();
       string[] Page_Code = null;
       if (!string.IsNullOrEmpty(user.PageCode))
       {
           Page_Code = user.PageCode.Split(',');
       }
%>

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
    <h3>Welcome to admin users..</h3>
    <h2>Admin Report</h2>

        <input type="hidden" id="HfUserId" />
        <table cellpadding="0" cellspacing="0" border="1" width="100%">
            <thead>
                <tr>
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
                    <td align="center"> <label id="lblCandidateAdd"></label> </td>
                    <td align="center"> <label id="lblEmployerAdd"></label> </td>
                    <td align="center"><label id="lblConsultantAdd"></label></td>
                    <td align="center"> <label id="lblJobPosted"></label> </td>
                    <td align="center"> <label id="lblTotalCount"></label> </td>
                </tr>
            </tbody>
        </table>
        <br /><br />

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
                        Employer
                    </th>
                                      
                    <th>
                        Date Of Regn.
                    </th>
                    <th>
                        Created By
                    </th>
                    <th>
                        Posted Jobs
                    </th>
                    <th>
                        Contact Name
                    </th>
                    <th>
                        Mobile
                    </th>
                    <th>
                        Email
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
 

</div>
</div>

<script type="text/javascript">

    $(document).ready(function () {
        var UserId = '<%= user.Id %>';
        $("#HfUserId").val(UserId);

        AdminReportBind(UserId, "", "");

        Databind("0", UserId, "", "");

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

    function AdminReportBind(UserId, fromDate, toDate) {
        $("#lblCandidateAdd").html("0");
        $("#lblEmployerAdd").html("0");
        $("#lblJobPosted").html("0");
        $("#lblTotalCount").html("0");  

        var data = "UserId=" + UserId + "&fromDate=" + fromDate + "&toDate=" + toDate;
        $.ajax({
            type: "POST",
            url: "/Admin/AdminPermission/AdminReportbyId",
            data: data,
            dataType: "json",
            success: function (response) {
                $("#lblCandidateAdd").html(response.CandidateAdded);
                $("#lblEmployerAdd").html(response.EmployerAdded);
                $("#lblJobPosted").html(response.JobPosted);
                $("#lblTotalCount").html(response.TotalCount);                
            }
        });
        return false;
    }

    function Databind(Pdestroy, UserId, fromDate, toDate) {

        var destroy = false;
        if (Pdestroy == "1")
            destroy = true;

        var oTable = $('.datatable').dataTable({
            "bJQueryUI": true,
            'bServerSide': true,
            "bDestroy": destroy,
            "iDisplayLength": 10,
            "sPaginationType": "full_numbers",
            'sAjaxSource': '/Admin/AdminPermission/ListEmployerReportbyAdminId?UserId=' + UserId + '&fromDate=' + fromDate + '&toDate=' + toDate,
            "bFilter": true,
            "aaSorting": [[3, 'desc']],
            "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [2, 3, 4, 5, 6]}],
            "aoColumns": [{ 'sWidth': '15%', 'sClass': 'left' }, { 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '5%', 'sClass': 'center' }, { 'sWidth': '10%', 'sClass': 'left'}],
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

        AdminReportBind($("#HfUserId").val(), $("#FromDate").val(), $("#ToDate").val());

        Databind("1", $("#HfUserId").val(), $("#FromDate").val(), $("#ToDate").val());
    }
 
 </script>
</body>
</html>



