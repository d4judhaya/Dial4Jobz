<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Earning Opportunities
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h2 style="font-family:Calibri; font-size:20px">Join Our Growing Family of Channel Partners</h2></center>
    <h3 style="font-family:Calibri; font-size:18px">BENEFITS OF JOINING DIAL4JOBZ</h3><br />
    <ul style="font-family:Calibri; font-size:18px">
        <li>Lowest Start-Up Cost with Existing Staff & Infrastructure</li>
        <li>Very High Income Potential</li>
        <li>Immediate Starting</li>
        <li>High Branding</li>
        <li>Grow with us</li>
    </ul>

    <h2 style="font-family:Calibri; font-size:20px"><center><img src="../../Content/Images/hand.png" width="41px" height="20px" /><a class="callpickupcash" href="../../../Home/ChannelEarningLists">Your Various Earning Opportunities</a></center></h2><br />
    <p style="font-family:Calibri; font-size:18px"><a href="http://dial4jobz.com">Dial4Jobz</a> is <b>India’s</b> Only <b>Talking Job Portal</b>, providing services over Both <b>Phone</b> and <b>Online.</b><br />
        <a href="http://dial4jobz.com">Dial4Jobz</a> gets Calls and Online visits, by which we add 100’s of Jobseekers & Vacancies every day.<br />
        Job Seekers & Employers Register With Dial4Jobz through 044 - 44455566 / <a href="http://dial4jobz.com">www.dial4Jobz.com</a><br />
        <a href="http://dial4jobz.com">Dial4Jobz</a> will provide Support in Training, Branding & Development.<br />
        <a href="http://dial4jobz.com">Dial4Jobz</a> is progressing to be the world’s largest Multi Platform Job Search Service.</p>
    <br />
   <center>
        <img src="../../Content/Images/channel_earnings.png" width="480px" height="75px" alt="dial4jobz_channel_earnings" />
   </center><br />
   <p style="font-family:Calibri; font-size:18px"><a href="http://dial4jobz.com">Dial4Jobz</a> Offers Easy Access...Optimal Results…Hot & Fresh Data…
      Candidates & Employers get SMS & Email alerts matching their needs. They contact each other directly…
      Candidates and Vacancies ranging from Driver to Top Management are served by us.</p>

     <center> <div class="confirm_selection" style="overflow-x: hidden; overflow-y: hidden;font-family:Calibri; font-weight:bold; font-size:25px; opacity: 1; color: RGB(255, 105, 0);">Happy Entering !!! Happy Selling !!! Happy Earning !!!</div></center>
   
   <p style="font-family:Calibri; font-size:18px">For further information, please feel free to Contact us.
    Looking forward to your early confirmation<br />
    Yours Sincerely,<br />
    <b>T D Vivekanandan<br />
    Marketing Head</b><br />
   <b> If Interested In Joining Us, Call or SMS @ +91 98413 20010 / <a href="mailto:smc@dial4jobz.com">smc@dial4jobz.com</a></b>
</p>
  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStartedEmployer"); %> <br />
</asp:Content>
