<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SmsTerms
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h5>Sms Terms & conditions</h5><br />
    <p>I agree Terms & conditions ( insert this where ever we ask the client to check the box for sms</p>

<%--<p>Your IP: __________ has been logged on ______ : ________</p>--%>
<p>Signed and accepted following conditions by me </p>
<p>I am approaching your organization to search candidate/search Vacancy/make a sales enquiry and I fully authorize your Company </p>
<p>Representatives to contact me on my given Mobile number or to send SMS on details of available candidate(s) or to send SMS on details of Vacancy(ies) and Email address.</p>
<p>Further I also take responsibility that if my number is registered with NCPR or as a Do Not Call customer then I would not take any legal action as I am permitting you all by myself to contact me on my given contact details.</p>
<p>I also agree that given contact details belong to me and fully adhere to the same.</p>
<p>I also know that while submitting this form, my IP gets logged in your records.</p>
<p><b>Note:</b> By clicking and accepting of the terms and conditions before sending this on the TAB with the option of irrespective of my NCPR Registration, I hereby send this undertaking of indemnity, I accept that this is a valid legal document under the
Information Technology Act, 2000 of the Union of India, with or without valid digital signature.</p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
