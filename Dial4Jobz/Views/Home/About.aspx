<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	About Dial4Jobz IPL
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("Nav"); %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>About Us</h2>

    <p>Dial4Jobz IPL is a new venture by a group of professionals with more than 50 man years experience in human resource recruitment and training. The experience gained over these long years enabled the team understand the pain areas in recruitment process. After long hours of brainstorming with the industry experts and HR managers the team got convinced about the imperative need of this service. And they also realized the fact that technology should and will play a pivotal role in the service they want to offer.</p> 

    <p>The team took to its advantage the widespread use of Telephones & Internet and developed this solution for every micro detail.</p> 

    <p>Dial4Jobz IPL aims to serve the work force of India better and effectively. Our mission is to provide an innovative recruitment service for all industries across various segments at an affordable cost and time.</p> 

    <p>Dial4Jobz IPL aims to equally serve the employer and candidate of all financial, cultural, educational and language backgrounds. We have the potential to reach millions of people through telephones and internet.</p> 

    <p>We aim to become the <b>world’s largest</b> phone job search service.</p>
    
      <p>
        <b>Candidate?</b>
        <%: Html.ActionLink("Candidate Home", "Index", "Home", null, new { title = "Back home" })%>
    </p>

    <p>
       <b>Employer?</b>
        <%: Html.ActionLink("Employer Home", "Index", "Employer", null, new { title = "Back home" })%>
    </p>

    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">

</asp:Content>
