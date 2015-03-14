<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Channel/Views/Shared/Channel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4jobz :: MyReport
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Report</h2>
        
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
                        Job Posted
                    </th>  
                    <th>
                        Plan Subscribed
                    </th>                   
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center"><label id="lblCandidateAdd"> <%: ViewData["CandidateAdded"].ToString() %> </label></td>
                    <td align="center"><label id="lblEmployerAdd"> <%: ViewData["EmployerAdded"].ToString() %> </label></td>
                    <td align="center"><label id="lblJobPosted"> <%: ViewData["JobPosted"].ToString() %> </label></td>
                    <td align="center"><label id="lblPlanSubscribed"> <%: ViewData["PlanSubscribed"].ToString() %> </label></td>
                </tr>
            </tbody>
        </table>
        <br /><br />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<link href="<%= Url.Content("~/Areas/Admin/Content/DataTable/css/demo_table_jui.css") %>" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
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

        $("#lblCandidateAdd").html("0");
        $("#lblEmployerAdd").html("0");
        $("#lblJobPosted").html("0");
        $("#lblPlanSubscribed").html("0");

        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetMyReport", "ChannelUser") %>',
            data: { fromDate: $("#FromDate").val(), toDate: $("#ToDate").val() },
            dataType: "json",
            success: function (response) {
                $("#lblCandidateAdd").html(response.CandidateAdded);
                $("#lblEmployerAdd").html(response.EmployerAdded);
                $("#lblJobPosted").html(response.JobPosted);
                $("#lblPlanSubscribed").html(response.PlanSubscribed);
            }
        });
        return false;
    }
</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("NavChannel"); %>
</asp:Content>
