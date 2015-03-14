<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Home.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="HomeLogo">
        <img src="../../Content/Images/Home page Logo D4J.jpg"/>
    </div>
    <div id="menuBarHolder">
        <ul id="menuBar">
            <li class="firstchild">
                <%:Html.ActionLink("Find Jobs","FindJobs","Jobs") %>
                <div class="menuInfo">
                    Click here to search job of your preference.</div>
            </li>
            <li>
                <%:Html.ActionLink("Post Jobs Free","Add","Jobs")%>
                <div class="menuInfo">
                    Post your vacancy its free.</div>
            </li>
            <li>
                <%:Html.ActionLink("Hire Candidates","HireCandidates","Employer")%>
                <div class="menuInfo">
                    Find suitable candidates for your Vacancy</div>
            </li>
        </ul>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">

<script type="text/javascript">
    $(document).ready(function () {

        $('#menuBar li').click(function () {
            var url = $(this).find('a').attr('href');
            document.location.href = url;

        });

        $('#menuBar li').hover(function () {
            $(this).find('.menuInfo').slideDown();
        },
function () {
    $(this).find('.menuInfo').slideUp();

});

    });
</script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <h4>Dial4Jobz Member?</h4><a class="login" href="/login" title="Login to Dial4Jobz">Login</a><br />
    <h4>New?</h4><a class="signup" href="/signup" title="Create an account on Dial4Jobz">Sign Up</a><br />
    <h4>Employer Login</h4><%: Html.ActionLink("Go to Employer Zone", "Index", "Employer", null, new { @class="nav-employer", title = "Click here if you are an employer", target="_blank" })%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
