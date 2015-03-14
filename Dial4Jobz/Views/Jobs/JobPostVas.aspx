<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JobPostVas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table>
    <tr>
    <td rowspan="3" colspan="3">
        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <%:Html.ActionLink("Hot Resumes", "Index", "EmployerVas")%> </h3 >
        <%} else { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>
    </tr>
    </table>

    <h3>Your Job has been posted successfully! Thanks for Post a Job.</h3>

     <%:Html.ActionLink("Post another Job","Add","Jobs") %> 

    <p> We look forward in supporting your business with its staffing needs and strongly believe you will find the staff you need at <a href="http://www.dial4jobz.com">dial4jobz.com</a></p>
      <p>For any queries or assistance, Please email to Sr.Manager – Client Relation <a href="mailto:smc@dial4jobz.com">smc@dial4jobz.com</a>  or call 044 - 44455566 Our Value Added Services for this Position</p>
      <center><b>VAS (Value Added Services)</b></center>
      <b><p>Employers:</p></b>
      <b><p>1. Hot Resumes: (Search Resumes)</p></b>
      <p> You can view all the resumes.  Based on the plan you subscribe for, You will be in position access <b>25</b> or more  contact details of the shortlisted candidates   for communicating with them. </p>

      <b><p> 2. Featured Employer: </p></b>
      <p>•	If you subscribe as a <i><b>Featured Employer</b></i>, Your Vacancy will be sent on priority to all the suitable candidates who call us. You will also get the details of the candidates to whom we have forwarded your vacancies for you to shortlist & proceed further.</p>
      <p>•	Period of Validity for Featured Employer will be <b>1 Month or 25 resumes</b> whichever is earlier.</p>
      <p>•	Period of validity for free job listings will be <b>1 month.</b></p><br />

      <b><p>3.	Spot Selections:</p></b>
      <p>•	We can supplement your recruitment process such as sourcing, filtering & short listing, interviewing & organize Teleconference by our professional Recruiters and submit the final list of candidates to you for selection. This service can be done for any position.</p>

      <b><p>4. Top employers: </p></b>
      <p> banner advt. can be placed in <a href="http://www.dial4jobz.com">dial4jobz.com</a>  home page and the link to your website will be given. </p>

      <b><p>5.	Advertise in emails:</p></b>
      <p>We can insert your banner advertisement in the header & footer of every mail which we send to candidate on vacancy & on registration. For Employer we send mail on Registration & resumes of suitable candidates.	Contact us pricing or mail to smc@dial4jobz.com. (You have option of choosing either category-Employer & Candidate.)</p>

      <b><p>6. Reference Checks: </p></b>
      <p>For a candidates selected by you (even if not from our portal) for recruitment we can conduct a reference check with their previous employers or/and references provided by them.</p>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
  <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
