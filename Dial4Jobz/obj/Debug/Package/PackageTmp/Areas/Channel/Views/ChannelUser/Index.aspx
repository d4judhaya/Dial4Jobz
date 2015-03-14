<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Channel/Views/Shared/Channel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4jobz :: Manage Channel Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Manage Channel Users</h2>
    <div style="text-align:right;">
    <%= Html.ActionLink("Add New Users", "Create", "", new { @class = "ActionPopup" })%>
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

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
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
             'sAjaxSource': '<%= Url.Action("ListChannelUsers", "ChannelUser") %>',
             "bFilter": true,
             "aaSorting": [[2, 'desc']], 
             "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [0, 2, 5, 6]}],
             "aoColumns": [{ 'sWidth': '5%', 'sClass': 'center' }, { 'sWidth': '25%', 'sClass': 'left' }, { 'sWidth': '10%', 'sClass': 'center' }, { 'sWidth': '25%', 'sClass': 'left' }, { 'sWidth': '25%', 'sClass': 'left' }, { 'sWidth': '5%', 'sClass': 'center' }, { 'sWidth': '5%', 'sClass': 'center'}],
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
                 $('.datatable tbody').html('<tr><td colspan="7" class="dataTables_empty"><img alt="Please Wait..." src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>"  height="50" /></td></tr>');
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
            $("input:checkbox[name=DeleteChannelUserId]").each(function () {
                this.checked = true;
            });
        }
        else {
            $("input:checkbox[name=DeleteChannelUserId]").each(function () {
                this.checked = false;
            });
        }

    }

    function Delete() {
        var userId = "";
        var count = 0;
        $("input:checkbox[name=DeleteChannelUserId]:checked").each(function () {
            if (count == 0) {
                userId = $(this).val();
            }
            else {
                userId += "," + $(this).val();
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
        var data = "userIds=" + userId;
        var url = '<%= Url.Action("Delete", "ChannelUser", "Channel") %>';
    
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("NavChannel"); %>
</asp:Content>
