<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Vacancy Reports</title>
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
            'sAjaxSource': '/Admin/VacancyReport/ListVacancyReports?fromDate=' + fromDate + '&toDate=' + toDate,
            "bFilter": true,
            "aaSorting": [[5, 'desc']],
            "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [2, 6]}],
            "aoColumns": [{ 'sWidth': '15%', 'sClass': 'left' }, { 'sWidth': '15%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'left' }],
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
                $('.datatable tbody').html('<tr><td colspan="6" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
                return true;
            }
        });
        $("<div style='width:38px; float:right;'><a style='margin-right:10px;' href='javascript:void(0)' onclick='javascript:Delete();return false;'><img alt='Delete' src='<%=Url.Content("~/Areas/Admin/Content/Images/icn_trash.png")%>' /></a></div>").appendTo('.dataTables_filter');
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

    function Checkall() {

        var check = false;
        $("input:checkbox[name=checkall]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });

        if (check == true) {
            $("input:checkbox[name=deletecandidate]").each(function () {
                this.checked = true;
            });
        }
        else {
            $("input:checkbox[name=deletecandidate]").each(function () {
                this.checked = false;
            });
        }

    }

    function Delete() { 
        var Id = "";
        var count = 0;
        $("input:checkbox[name=deletecandidate]:checked").each(function () {
            if (count == 0) {
                Id = $(this).val();
            }
            else {
                Id += "," + $(this).val();
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
        var data = "Ids=" + Id;
        var url = '<%= Url.Action("Delete", "CandidateReport","Admin") %>';

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            success: function (data) {
                $("#loading").hide();
                if (data.error != "1") {
                    alert(data.message);
                    // Databind("1");
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
    <div id="main" style="width:900px;">    
    <h2>Vacancy Reports</h2>

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
                        Position
                    </th>
                    <th>
                       Created Date
                    </th>
                    <th>
                      Employer
                    </th>
                    <th>
                        Function
                    </th>
                    <th>
                      Role
                    </th>
                    <th>
                     Experience
                    </th>
                    <th>
                      Salary
                    </th>
                    <th>
                      Location
                    </th>
                 
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="6" class="dataTables_empty">
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
