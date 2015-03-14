<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Security
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Security</h2>
    
    <h5>Dial4Jobz.com Security Advice</h5><br />
    <p>Dial4Jobz.com is a platform that brings together Jobseekers and Employers. We are neither a recruitment firm nor do we act as labor consultants to, or employment partners of, any employer. The portal is used by numerous corporate, SME, MSME, Home employers, placement companies, and recruitment firms. The usage of our site is bound by a set of <a href="/terms">terms and conditions</a>  that the employers must agree and adhere to. One of the primary objectives of these Terms and Conditions is to discourage and prevent misuse and fraud.</p>
    <p>Regrettably, sometimes, false job postings are listed online, and non-existing job offers are sent via email to illegally collect personal information and/or money from unsuspecting job seekers. To tackle such cases, we need your help to remain vigilant every day - when we receive complaints from jobseekers about fraudulent or suspicious emails, we promptly notify the concerned employer and, if necessary, block them from using our services.</p>
    <h5>We encourage you to be wary of emails that</h5>
    <ul>
    <li>ask your personal, non-work related information such as Credit Card numbers or Bank information over phone or email</li>
    <li>do not provide valid contact information</li>
    <li>ask for monetary transactions, money-transfers, or payment for any employment/recruitment related purpose</li>
    <li>promise emigration and ask for money to process visa, etc</li>
    </ul>
    <p>Before you respond to such emails, <b>we suggest you to do a discreet enquiry and be sure to verify the legitimacy of the employer</b> with whom you are interacting. Please note that Dial4Jobz.com does not approve of, or represent any employer or recruiter sending such fraudulent communication, which in fact are a violation of the Terms and Conditions. We value the trust you place on Dial4Jobz.com and are committed to making your job search a safe and fraud-free experience on our site.</p>
   <p>If you have received (or receive) any such suspicious email communication from a possible Dial4Jobz client, do report it to us at report@Dial4Jobz.com.</p>

   <h5>Educate yourself against Fraud/Scams</h5>
   <p>We encourage you to read the following and make yourself aware of the warning signs of the most common kinds of Internet and Email frauds.</p>
   <p>There are two types of email scams – 'phishing' and 'spoofing'. In both the cases, the 'from address' is forged to make it appear as if it came from a source that it actually did not come from.</p>
   
   <h5>What is Phishing?</h5>
   <p>Phishing is an attempt by fraudsters to 'fish' for your personal details. A phishing attempt is usually in the form of an e-mail, which encourages you to click a link that takes you to a fraudulent log-on page designed to capture your account/password/personal details. These emails can also be used to lure the recipient into downloading harmful software. Please note that Dial4Jobz.com will never ask you to download software in order to access your account.</p>
   <p>More information about Phishing scams and how you can protect yourself against them is available here  <a href="http://www.onguardonline.gov/phishing">Phishing</a></p>
   <h5>What is Spoofing?</h5>
   <p>Spoof emails usually include a fraudulent offer of employment and/or an invitation to perform a monetary transaction. Such email scams are, unfortunately, common across the world and could target anyone - including unsuspecting jobseekers who have registered with Dial4Jobz.com. The sender's address is often disguised and/or the sender may not have provided the entire contact information, such as, the correct physical address, phone numbers and email ID.</p>
   <p>The precautionary measures jobseekers could take to protect themselves against suspected spoof emails have been mentioned above.</p>
   <h5>How can you check if an employer or a recruiter is a fraud or genuine?</h5>
   <p>Below are a few things we can suggest you could try (you are encouraged to exhaust all possible ways, not just be limited to the following) to ascertain the legitimacy of the employers who contact you:</p>
   <ul>
   <li>1. Have you heard the name of the company before?</li>
   <li>2. Do you remember applying to this particular company or did the offer come unsolicited?</li>
   <li>3. Search on Yahoo, Google or MSN to see if the company has a genuine website. Also check if other people have reported that the employer has been involved in job scams previously.</li>
   <li>4. Note the country that company claims to be working in, and offering you a job in. Most fake companies offer scam jobs in Africa, Middle East, Asia, South America, and to a much smaller extent, US or Western Europe.</li>
   <li>5. Find out if you need to pay the company for anything. If they ask you to pay - at first or even after some time - for visa, work permit or for travel, chances are it's a fake company.</li>
   </ul>
   <p>Remember, If you have received (or receive) any such suspicious email communication from a possible Dial4Jobz client, do not hesitate to report it to us at report@Dial4Jobz.com. You can also block the employer from contacting you in future, from your Account Settings.</p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
  <% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
