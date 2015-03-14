<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Vacancy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <% Html.BeginForm("Save", "AdminHome", FormMethod.Post, new { @id = "Save" }); %>
        <% Html.RenderPartial("LoggedInEmployerInfo"); %>
        <% Html.RenderPartial("JobEntry", Model); %>
        <% var js = "javascript:Dial4Jobz.Job.Save(this, '" + Model.Id + "');return false;"; %>   
             
        <input id="SaveJob" type="submit" value="Save" class="btn" onclick="<%: js %>" title="Click to add requirement and search for qualifying candidates" />
                     

    <% Html.EndForm(); %>

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
        <style type="text/css">
             .text
             {
               background: url("../../Areas/Admin/Content/Images/dropdown.gif") no-repeat scroll 98% 50% transparent;
             }    
            
        </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
