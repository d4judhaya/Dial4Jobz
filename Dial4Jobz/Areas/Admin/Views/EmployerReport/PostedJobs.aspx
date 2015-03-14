<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PostedJobs</title>

<script type="text/javascript">

    $(document).ready(function () {
        var empid = '<%= Request.QueryString["id"] %> ';
        var name = '<%= Request.QueryString["name"] %> ';
        $("#empName").html(name);
        $("#HfempId").val(empid);
        Databind1("0", empid, "", "");

        $("#fromDate").datepicker({ changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            onSelect: function (selected) {
                $('#toDate').datepicker('option', 'minDate', selected)
            }
        });

        $("#toDate").datepicker({ changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy"
        });

        $('#fromDate,#toDate').bind('keypress keydown', function (e) {
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

    function Databind1(Pdestroy, empId, fromDate, toDate) {

        var destroy = false;
        if (Pdestroy == "1")
            destroy = true;

        var oTable = $('.datatable1').dataTable({
            "bJQueryUI": true,
            'bServerSide': true,
            "bDestroy": destroy,
            "iDisplayLength": 10,
            "sPaginationType": "full_numbers",
            'sAjaxSource': '/Admin/EmployerReport/ListPostedJobs?empId=' + empId + '&fromDate=' + fromDate + '&toDate=' + toDate,
            "bFilter": true,
            "aaSorting": [[1, 'desc']],
            "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [2]}],
            "aoColumns": [{ 'sWidth': '60%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }],
            "fnInitComplete": function () { },
            "fnDrawCallback": function () { },
            "fnPreDrawCallback": function () {
                $('.datatable1 tbody').html('<tr><td colspan="3" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
                return true;
            }
        });
    }

    function Search1() {
        if ($("#fromDate").val() != "" && $("#toDate").val() == "") {
            alert("Please Enter To Date");
            $("#toDate").focus();
            return false;
        }

        if ($("#toDate").val() != "" && $("#fromDate").val() == "") {
            alert("Please Enter From Date");
            $("#fromDate").focus();
            return false;
        }

        Databind1("1", $("#HfempId").val(), $("#fromDate").val(), $("#toDate").val());
    }
 </script>
</head>
<body>
    <div>
    <div id="main" style="width:798px;">    
    <h2>Posted Jobs</h2>

    <table width="100%">
            <tr>
                <td style="width:35%;" align="right" valign="middle">
                       <strong> Employer Name :</strong>
                    </td>
                    
                    <td style="width:35%;" align="left" valign="middle">
                        <label id="empName"></label>
                        <input type="hidden" id="HfempId" />
                    </td>
            </tr>
        </table>
        <br /><br />
   <%-- <table width="100%">
            <tr>
                <td style="width:35%;" align="right" valign="middle">
                        From &nbsp; <%: Html.TextBox("fromDate", "", new { autocomplete = "off", onkeypress = "return false" }) %>
                    </td>
                    <td style="width:15%;" align="right" valign="middle" class="editor-field">
                        To &nbsp;
                    </td>
                    <td style="width:25%;" align="left" valign="middle" >
                        <%: Html.TextBox("toDate", "", new { autocomplete = "off", onkeypress = "return false" }) %>
                    </td>
                    <td style="width:35%;" align="center" valign="middle" class="editor-field">
                        <input type="submit" id="searchbtn" value="Search" onclick="javascript:Search1();return false;" />  <br />      
                    </td>
            </tr>
        </table>
        <br /><br />--%>
        <table cellpadding="0" cellspacing="0" border="0" class="datatable1 display">
            <thead>
                <tr>
                    <th>
                        Job Posted
                    </th>                    
                    <th>
                        Date Of Posting
                    </th>   
                    <th>
                        Posted By
                    </th>                                                            
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="3" class="dataTables_empty">
                        <img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"
                            height="50" />
                    </td>
                </tr>
            </tbody>
        </table>
 </div>
    </div>
</body>
</html>
