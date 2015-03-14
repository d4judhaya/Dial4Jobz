<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pricing Plans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <% Html.BeginForm("Index", "EmployerVas", FormMethod.Post, new { @id = "VasFES" }); { %>

     <%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>

     <%if (ViewData["LoggedInOrganization"] != null)
       { %>
            <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
            <%var pendingpayment = _vasRepository.GetPendingOrdersEmployers(loggedInOrganization.Id); %>

         <%if (pendingpayment != null)
           { %>
                <h3>You have orders pending: <%:Html.ActionLink("Pay Now & Activate", "MySubscription_Billing", "Employer")%></h3>
         <%} %>

     <%} else { %>
         
     <%} %>
    
    <center>
       <h2 style="font-family:Arial; font-size:19px;font-weight:bold;">Plans to meet your needs</h2>
    </center>

      <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Basic</h3>
    </div>

    <table id="Basic" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
                <strong style="font-family:Arial; font-size:12px; font-weight:bold">Basic Plan</strong>
            </th>
        </tr>

       <tr>
            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Plan</strong>
            </th>

             <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Number of Resumes with Contact Details</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Resumes for Alert</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Vacancies</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Validity / Days</strong>
            </th>

            <th class="bdrL_blue valignT highlight">
               <strong class="font14">Amount</strong>
            </th>

            <th class="bdrL_blue valignT highlight">
               <strong class="font14"></strong>
            </th>
        </tr>
        
         <%--<tr>
           <th width="20%" colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">E- Basic</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">25 + 100 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">25</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">30 Days</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">860</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('E-Basic','basic','860')" /></a>
            </th>
        </tr>--%>

         <tr>
           <th width="20%" colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">E - Basic Plus</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">50 + 200 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">50</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">2</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">30 Days</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">1350</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('E-Basic Plus','basicPlus','1350')" /></a>
            </th>
        </tr>

        <tr>
           <th width="20%" colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">E- Economy</strong><br />
            </th>
            <th width="25%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">100 + 300 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">75</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">3</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">30 Days</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">2250</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('E-Economy','basicEconomy','2250')" /></a>
            </th>
        </tr>

        <tr>
           <th width="20%" colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">E- Ideal</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">500 + 750 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">125</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">5</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">60 Days</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">4200</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('E-Ideal','basicIdeal','4200')" /></a>
            </th>
        </tr>

         <tr>
           <th width="20%" colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">E- Saver</strong><br />
            </th>
            <th width="25%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1000 + 2000 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">500</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">20</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">90 Days</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">8000</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('E-Saver','basicSaver','8000')" /></a>
            </th>
        </tr>
    </table>
    <br />
   

    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Resume Alert</h3>
    </div>

    <table class="Vasprice" id="ResumeAlert" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="11">
                <strong style="font-family:Arial; font-size:12px; font-weight:bold">Get resume alert by SMS & E-Mail of Active Candidates who Register after you subscribe</strong>
            </th>
        </tr>

        <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">Plan</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Validity in Days</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Resumes</strong>
            </th>

             <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Number of Vacancies</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Pricing</strong>
            </th>

            <th class="bdrL_blue valignT highlight"></th>
        </tr>


        <tr>
           <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">RAT75</strong><br />
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">30</span>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">75</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">3</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.1000</span>                
                <br />
                <div id="RAAmount1" style="display: none"> Featured Employer</div>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
               <%-- <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" alt="Featured employer" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT75','Resume Alert','1000')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="Featured employer" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT75','Resume Alert','1000')"/></a>
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">RAT125</strong><br />
                <div id="GV" style="display: none">Featured Employer</div>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">60</span>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">125</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">5</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.1500</span>
                <br />
                
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
               <%-- <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT125','Resume Alert','1500')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT125','Resume Alert','1500')" /></a>
            </th>
        </tr>

        <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RAT500</strong><br />
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">90</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">500</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">20</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.4500</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT500','Resume Alert','4500')" /></a>--%>
                   <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT500','Resume Alert','4500')" /></a>
            </th>
        </tr>

           <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATSILVER</strong><br />
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">180</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">1250</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">50</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.8200</span>
                
             
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>">--%>
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATSILVER','Resume Alert','8200')" /></a>
            </th>
        </tr>

         <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATGOLD</strong><br />
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">365</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">2500</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">100</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.14995</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATGOLD','Resume Alert','14995')" /></a>
            </th>
        </tr>

         <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATCOMBO</strong><br />
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">365</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">3000</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">100</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.24995 + 300 TeleConference</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATCOMBO','Resume Alert','24995')" /></a>
            </th>
        </tr>
  

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="11">
                <ul>
                    <%--<li>Vacancies posted will be displayed on top of all suitable Job search & Alert will be live for 30 days from the date of activation or till 25 Resumes per vacancy is sent whichever is earlier.</li>                    --%>
                    <li>All Vacancies posted will be displayed on top of all suitable Job searches by Employers</li>
                    <li>Alerts by SMS / Email will be live for Valid days from the date of activation or till 25 Resumes per vacancy is sent whichever is earlier</li>
                </ul>
            </th>
        </tr>
    </table>
           
    <br />
   
   
    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Hot Resumes</h3>
    </div>

    <table id="HotResumes" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
                <strong style="font-family:Arial; font-size:12px; font-weight:bold">View Contact Details of Candidates to reach them Immediately.</strong>
            </th>
        </tr>

       <tr>
            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Plan</strong>
            </th>

             <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Number of Resumes with contact Details</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Validity / Days</strong>
            </th>

            <th class="bdrL_blue valignT highlight">
                <strong class="font14">Pricing</strong>
            </th>

            <th class="bdrL_blue valignT highlight">
               
            </th>
        </tr>
             

        <tr>
            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORS100</strong><br />
                
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">100 + 300 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">30</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Rs.1500</span>
                
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
               <%-- <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS100','Hot Resumes','1500')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS100','Hot Resumes','1500')" /></a>
            </th>
        </tr>

        <tr>
            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORS500</strong><br />
            </th>


            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">500 + 750 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">60</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.3000</span>                
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS500','Hot Resumes','3000')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS500','Hot Resumes','3000')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORS1000</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1000 + 2000 Emails </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">90</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.5000</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
               <%-- <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS1000','Hot Resumes','5000')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS1000','Hot Resumes','5000')" /></a>
            </th>
        </tr>

        <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSSILVER </strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">3000 + 7500 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">180</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.12995</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSSILVER','Hot Resumes','12995')" /></a>
            </th>
        </tr>

         <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSGOLD</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">10000 + 25000 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">365</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.24995</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSGOLD','Hot Resumes','24995')" /></a>
            </th>
        </tr>

        
         <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSCOMBO</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">10000 + 25000 Emails</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">365</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.29995 + 300 Teleconference</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSCOMBO','Hot Resumes','29995')" /></a>
            </th>
        </tr>
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
               <ul>
                    <li>All Vacancies posted will be displayed on top of all suitable Job searches by Employers</li>
                    <li>Hot resumes will be active for Valid days from the date of activation or till number of contact details as per the plan is viewed, whichever is earlier.</li>
                    <li>Email Count as per plan can be used within the validity period of the Plan</li>
                </ul>
            </th>
        </tr>
    </table>
    <br />

  
      
  
   <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Background Checks</h3>
    </div>
    <table id="backgroundCheck" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Residence check</strong><br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Current residence/ permanent residence</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.340/-  per verification</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.340</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCRC340','BackgroundChecks','340')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCRC340','BackgroundChecks','340')" /></a>
                
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Academic record check</strong><br />
                
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Latest qualification</span>
                
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.1125/-  per degree or certification</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Rs.1125</span>                
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCARC1125','BackgroundChecks','1125')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCARC1125','BackgroundChecks','1125')" /></a>
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Prior employment check</strong><br />
            </th>


            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Last 5 years of employment subject to 2 employers</span>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
               <span class="font18">Rs.500/- per employment</span>
            </th>


            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.500</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCPEC500','BackgroundChecks','500')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCPEC500','BackgroundChecks','500')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Criminal record check</strong><br />
                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Covering  current residence/ address of longest stay in the last 7 years</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.750/- per verification or address</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.750</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCCRC750','BackgroundChecks','750')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Character ref check</strong><br />
                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">2 referees -  Supervisors in Previous employment</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.500/- </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.500</span>
             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCCREFC500','BackgroundChecks','500')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCCREFC500','BackgroundChecks','500')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Database search</strong><br />
                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">Criminal/ compliance/ regulatory database searches</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.285/- </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.285</span>             
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCDBS285','BackgroundChecks','285')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('BGCDBS285','BackgroundChecks','285')" /></a>
            </th>
        </tr>
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
               <ul>
                    <li>All prices are Inclusive of Service Tax</li>
                    <li>Normal time taken for verification of all the service except Academic record check is 14 working days</li>
                    <li>For Academic record check normally it takes 21 working days is required & for few universities it may take longer time.</li>
                </ul>
            </th>
        </tr>
    </table>
    
    <br />
    
    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>SMS Purchase</h3>
    </div>

    <table id="smsPurchase" class="Vasprice" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">SMS100</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 100 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 113/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS100','SMSPurchase','113')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS100','SMSPurchase','113')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">SMS200</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 195 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 220/-
            </th>
           <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS200','SMSPurchase','220')" /></a>--%>
                 <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS200','SMSPurchase','220')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">SMS500</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 450 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 506/-
            </th>
            <th width="20%" style="text-align:center;" width="20%" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS500','SMSPurchase','506')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS500','SMSPurchase','506')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">SMS1000</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 875 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 984/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS1000','SMSPurchase','984')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS1000','SMSPurchase','984')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">SMS5000</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 4250 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 4776/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <%--<a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS5000','SMSPurchase','4776')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS5000','SMSPurchase','4776')" /></a>
            </th>
        </tr>        
    </table>

    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Spot Selection</h3>
    </div>
  
    <table id="SpotSelection" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
                 <strong style="font-family:Arial; font-size:12px; font-weight:bold">Don't have time or unable to shortlist Suitable Candidates ??? Our Recruiter can help you.</strong>
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <ul>
                    <br />
                    <b>Spot Selection</b>
                </ul>
                <div id="SS" style="display: none">Featured Employer</div>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
             <span class="font18">Rs.500</span>              
                <br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
               <%-- <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('SS','SpotSelection','500')" /></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('SS','SpotSelection','500')" /></a>
            </th>
        </tr>
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="6">
                <strong class="star">*</strong> Rs.500/ Process fee per position<br />
                   <ul>
                    <li>Dial4Jobz will organise Teleconference with Suitable candidates</li>
                    <li>Proceed with Face to face Interview after Teleconference if needed for Selection.</li>
                    <li>Pay Fees once the Candidate Joins</li>
                    <li>To know fee structure Contact Dial4jobz – Call 044 - 44455566</li>
                   </ul>
            </th>
        </tr>
    </table>
    <br />
            
                 
   

   <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Find Candidates</h3>
    </div>
   
    <table class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
                 <strong style="font-family:Arial; font-size:12px; font-weight:bold">Our Recruiters will find availability for your Vacancy & Location</strong>
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <ul>
                    <li>Details of available candidates will be sent to you</li>
                    <li>Cost of Service is as per Vacancy Check with Dial4jobz for the same.</li></ul>
                <br />
                <div id="HR" style="display: none">Hot Resume</div>
            </th>
          
            <th width="20%" class="bdrL_blue valignT highlight">
               <%: Html.ActionLink("Contact Us","Contact","Home") %>
               
            </th>
        </tr>
    </table>

    <%} %>
    <% Html.EndForm(); %>
   
     <div id="wait" style="display:none;z-index: 1199;width:69px;height:89px;position:absolute;top:50%;left:50%;padding:2px;">
        <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/demo_wait.gif")%>" height="50" alt="Loading..." />
     </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
     
   <script type="text/javascript">
       
       function vas_confirm(plan, vasType, amount) {
           if (confirm("Confirm the order to buy for Rs. " + amount + "")) {
               if (plan != undefined) {
                   //$("#txt").load("demo_ajax_load.asp");
                   $("#wait").show();
                   $.ajax({
                       url: '/employer/employervas/Subscribed',
                       type: 'POST',
                       data: { 'Plan': plan, 'VasType': vasType },
                       datatype: 'json',
                       success: function (response) {
                           if (response.Success) {
                               Dial4Jobz.Common.ShowMessageBar(response.Message);
                           }

                           if (response.ReturnUrl != null) {
                               window.location = response.ReturnUrl;
                           }
                       },
                       error: function (xhr, status, error) {
                       }
                   });
               }

           }
           else {
               return false;
           }
       }   

      
</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
                <div><h5>Sales Enquiries</h5>
                <ul>
                <li>Mobile Number:+91-9381516777 </li>
                <li>Contact Number:044 - 44455566 </li>
                <li>E-Mail:smc@dial4Jobz.com </li>
                </ul>    
                </div>

                <div><h5>Customer Support</h5>
                <ul>
                <li><a title ="Contact Me">Manikandan</a></li>
               <%-- <li>Mobile Number:+91-9841087952 </li>--%>
                <li>Contact Number:044 - 44455566 </li>
                <li>E-Mail:smo@dial4jobz.com </li>
                </ul>    
                </div>
</asp:Content>
