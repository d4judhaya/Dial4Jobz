<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Channel/Views/Shared/Channel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4jobz :: Channel Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Hi <%: this.User.Identity.Name.Split('|')[Dial4Jobz.Models.Constants.ChannelLoginValues.ChannelName]%>, welcome to channel..</h3><br />

    <ul id="coolMenu" style="width:700px;">
    <li><a href="#">Users</a>
             <ul class="noJS">   
                <li> <%: Html.ActionLink("AddCandidate", "AddCandidate", "AdminHome", new { area = "admin" }, new { target = "_blank" })%></li>
                    
                <li> <%: Html.ActionLink("AddEmployer", "AddEmployer", "AdminHome", new { area = "admin" }, new { target = "_blank" })%></li>

                <% if (Request.IsAuthenticated == true)
                    {
                        if (this.User.Identity.Name.Split('|')[Dial4Jobz.Models.Constants.ChannelLoginValues.ChannelRole] == "1")
                        { %>
                            <li> <%: Html.ActionLink("Add Users", "Index", "ChannelUser", null, new { target = "_blank" })%></li>
					<% }
                    } %>
				</ul>
            </li>
            <li><a href="#">Reports</a>
             <ul class="noJS">                             
                <li> <%: Html.ActionLink("My Report", "MyReport", "ChannelUser", null, new { target = "_blank" })%></li>
                <% if (Request.IsAuthenticated == true)
                    {
                        if (this.User.Identity.Name.Split('|')[Dial4Jobz.Models.Constants.ChannelLoginValues.ChannelRole] == "1")
                        { %>
                            <li> <%: Html.ActionLink("User Report", "UserReport", "ChannelUser", null, new { target = "_blank" })%></li>
					 <% }
                    } %>
				</ul>
            </li>
            </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<link href="<%= Url.Content("~/Content/Admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("NavChannel"); %>
</asp:Content>
