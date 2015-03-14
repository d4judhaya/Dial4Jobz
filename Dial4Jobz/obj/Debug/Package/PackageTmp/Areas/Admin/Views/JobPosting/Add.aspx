<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Add New Job
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.JobAdmin.js")%>" type="text/javascript"></script>
<link href="../../../../Content/jquery.timepicker.css"  rel="Stylesheet" type="text/css"/>
        <script src="<%=Url.Content("~/Scripts/jquery.timepicker.js")%>" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {

                // set Preferred time in timepicker
                $('input[name*="ddlPreferredTimeFrom"]').timepicker({});

                $('input[name*="ddlPreferredTimeTo"]').timepicker({});
            });
        </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("NavAdmin"); %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.BeginForm("Add", "JobPosting", FormMethod.Post, new { @id = "Add" }); %>          
        <% Html.RenderPartial("JobEntry", new Dial4Jobz.Models.Job()); %>
        <input id="AddJob" type="submit" value="Add Job" class="btn" onclick="javascript:Dial4Jobz.Job.AdminAdd(this);return false;" title="Click to add job requirement and search for qualifying candidates" />
    <% Html.EndForm(); %>

    <div id="loading">
        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %>
    <% Html.RenderPartial("Side/GettingStarted"); %>
    <% Html.RenderPartial("Side/Video"); %>
    <%--<input id="Resume" type="file" name="Resume" />--%>
</asp:Content>
