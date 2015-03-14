<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Change Password</h2>
    <p>
        Your password has been changed successfully.
    </p>

    <%if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
      { %>
         <%:Html.ActionLink("Go to Your Page", "MatchCandidates", "Employer")%>
    <%}
      else
      { %>
         <%:Html.ActionLink("Go to Your Page", "Index", "Jobs")%>
    <%} %>

</asp:Content>
