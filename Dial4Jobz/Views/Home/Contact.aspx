<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact Us
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("Nav"); %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Candidates</h2>
        <p>1. For all enquiries mail to <a href="mailto:smo@dial4jobz.com">smo@dial4jobz.com</a> or contact Sr.Manager-Operations</p>
        <p>2. For Clarifications on "Know the Employer" mail to: <a href="mailto:employer@dial4jobz.com">Employer@dial4jobz.com</a></p>

    <h2>Employers</h2>
        <p>1. For all enquiries mail to <a href="mailto:smc@dial4jobz.com">smc@dial4jobz.com</a> or contact Sr.Manager-Client Relations</p>
        <p>2. For Clarifications on "Reference Checks" mail to: <a href="mailto:refcheck@dial4jobz.com">refcheck@dial4jobz.com</a></p>
        <p>3. For Advertising in this site contact Business Head or mail to <a href="mailto:bhd@dial4jobz.com">bhd@dial4jobz.com</a></p>
        
    <h2>Address</h2>
       <p>Dial4Jobz India Private Limited,</p>
       <p>No:32,3rd Cross Street Ext,AGS Colony(Sea Side),</p>
       <p>Kottivakkam,Chennai – 600041.Ph.044 - 44455566</p>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Role.js")%>" type="text/javascript"></script>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideContent" runat="server">
<iframe height="300px" width="300px" src="http://local.google.co.in/maps?hl=en&amp;georestrict=input_srcid:d1c69013aa9631ae&amp;ie=UTF8&amp;view=map&amp;cid=10728518979080230668&amp;q=Millennium's+Dial4Jobz&amp;ved=0CBsQpQY&amp;ei=7MNOTPHDAZCWuAP58qyQAQ&amp;hq=Millennium's+Dial4Jobz&amp;hnear=&amp;ll=12.966713,80.263474&amp;spn=0.006273,0.006437&amp;z=16&amp;iwloc=A&amp;output=embed">
 </iframe>
 <p><a href="http://local.google.co.in/maps?hl=en&amp;georestrict=input_srcid:d1c69013aa9631ae&amp;ie=UTF8&amp;view=map&amp;cid=10728518979080230668&amp;q=Millennium's+Dial4Jobz&amp;ved=0CBsQpQY&amp;ei=7MNOTPHDAZCWuAP58qyQAQ&amp;hq=Millennium's+Dial4Jobz&amp;hnear=&amp;ll=12.966713,80.263474&amp;spn=0.006273,0.006437&amp;z=16&amp;iwloc=A&amp;source=embed">View Large Map</a></p>
</asp:Content>
