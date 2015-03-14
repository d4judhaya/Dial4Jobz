<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	WhoIsEmployer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div class="orange">“Get to know about the Prospective Employer”</div>
    <p>For the first time in the Recruitment Industry we have initiated to get the details of the companies (Prospective Employer) for the candidates.</p>

    <div class="orange">
        Why should you know about the Employer?
    </div> 

    1.	To know and understand about the company and the work culture.<br />
    2.	The websites of some of the companies are designed highlighting their best qualities. So you will not be able to decide from the information available in site whether that company will suit you.<br />
    3.	Many companies may be credible in all ways but may not have invested time in developing flashy or descriptive website. In some cases even the local citizens may not be aware of its existence. <br />
    4.	Your impression on the company based on their website may go wrong and could affect your decision.<br />
    5.	The Employer or interviewer may or may not reveal both advantage & disadvantages of their company & respective Location.<br />
    6.	To know about the living conditions, cost, security & climate of the location.<br />
    7.	When you are opting for overseas position you may not be aware of hidden costs which might bring down your expected savings or vice versa. <br />
    8.	You would have got convinced with the company, but your parents or Spouse would not have favoured your decision.<br />

    <div class="orange">When & who should know about the Employer?</div>
    <ul>
        <li> You have been called for the interview and have no information about the Company & the location.</li>
        <li>You have received the appointment letter and need to take decision.</li>
        <li>For any overseas employment.</li>
   </ul>

   <div class="orange">What we offer</div>
   Our panel of consultants will discuss with 2 Employees or Ex-Employees of the referred company (Wherever & whatever the size of the company is) and get the details on the following:

     <ul>
         <li>For companies in any location globally </li>
         <li>About the company</li>
         <li>Its work culture</li>
         <li> Work Timings</li>
         <li>Mode of Salary payment. </li>
         <li>Living conditions in the location where you are going to work </li>
         <li>Medical Facilities</li>
         <li>Cost of Living</li>
         <li>Saving potential</li>
         <li>Schools</li>
         <li>Transport</li>
         <li>Security</li>
         <li>All details we get from the employees working with your prospective employer</li>
        </ul>

        <div class="orange">Feedback</div>
        After the discussion our Analysis Report will have:
            •	Name & contact details of Employees or Ex-employees (we discussed with) of the referred company.<br />
            •	Report on the company<br />
       <div class="black">The Analysis report will help you take a wise decision.</div>

       <div class="orange">Our Fees</div>
       <div class="black">Regular:</div>
            All these comes to you at a very affordable cost of Just <b>Rs. 562</b>(inclusive of service tax) (feedback on the employer will be sent in <b>5 working days</b>)<br /> 
       <div class="black">Express Information Report: </div>
            <b>@Rs.1124</b>(inclusive of service tax) .Feedback on the employer will be given within 2 working days.<br />
       If you need more information, we can do at an additional cost.<br /><br />
       PS: For any reason if we are unable to get Information for the company referred the Fee collected will be refunded.
      
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <%: Html.ActionLink("Back To Subscribe", "Index", "CandidatesVas", null, new { title = "Back home" })%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
