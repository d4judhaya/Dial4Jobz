<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Channel/Views/Shared/Channel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4jobz :: Channel Partner Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Channel Partner Report</h2>
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
                        S.No
                    </th>
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
                        Job Posted
                    </th>
                    <th>
                        Plan Subscribed
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

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
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
            'sAjaxSource': '<%= Url.Action("ListChannelPartnerEntries", "Channel") %>' + '?fromDate=' + fromDate + '&toDate=' + toDate,
            "bFilter": true,
            "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [0, 5]}],
            "aoColumns": [{ 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '20%', 'sClass': 'left' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '20%', 'sClass': 'right' }, { 'sWidth': '10%', 'sClass': 'right'}],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                var oSettings = oTable.fnSettings();
                $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);
                return nRow;
            },
            "fnInitComplete": function () { },
            "fnDrawCallback": function () { },
            "fnPreDrawCallback": function () {
                $('.datatable tbody').html('<tr><td colspan="6" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<%--<% Html.RenderPartial("NavChannel"); %>--%>
</asp:Content>
