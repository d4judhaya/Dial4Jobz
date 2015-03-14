<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Add New Job
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavAdmin"); %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.JobAdmin.js")%>" type="text/javascript"></script>
     <script src="<%=Url.Content("~/Scripts/PreventDoubleSubmit.js") %>" type="text/javascript"></script>
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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("AddJob", "AdminHome", FormMethod.Post, new { @orgnId = ViewData["OrgnId"], @ConsultantId= ViewData["ConsultantId"] }); %>   
        <%:Html.Hidden("orgnId", ViewData["OrgnId"])%>  
        <%--<%:Html.Hidden("consultantId",ViewData["ConsultantId"]) %>--%>
        <% Html.RenderPartial("LoggedInEmployerInfo"); %>    
        <% Html.RenderPartial("JobEntry", new Dial4Jobz.Models.Job()); %>
        <input id="AddJob" type="submit" value="Add Job" class="btn" onclick="javascript:Dial4Jobz.Job.Add(this);return false;" title="Click to add job requirement and search for qualifying candidates" />

    <% Html.EndForm(); %>
</asp:Content>

