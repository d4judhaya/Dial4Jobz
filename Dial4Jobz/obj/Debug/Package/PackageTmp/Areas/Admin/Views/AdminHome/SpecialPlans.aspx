<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Special Plans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<center><h2>Special Candidate Plans</h2></center>
<% Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault(); %>

<%if (user.UserName=="admin123" || user.PageCode.Contains("Allow Special Plans"))
  { %>

     <table id="JobAlert" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
     <tr class="FE">
           <h3>Job Alert</h3>
     </tr>      
           <tr class="FE">
            <th colspan="5"  class="bdrL_blue valignT highlight">
                 <strong class="font18">RAJ7</strong>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">7 Days</span>
            </th>
            
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">20 Vacancies</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.100</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ7','JobAlert','100','75')"/></a>
            </th>
          </tr>

        <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">
                <strong class="font18">RAJ30</strong>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">30 Days</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">50 Vacancies</span>
            </th>

           <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.360</span>
            </th>

             <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ30','JobAlert','360','270')"/></a>
            </th>
        </tr>

        <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">
                <strong class="font18">RAJ90</strong>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">90 Days</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">150 Vacancies</span>
            </th>

           <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.995</span>
           </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ90','JobAlert','995','746')"/></a>
            </th>
        </tr>

      <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">
                <strong class="font18">RAJ180</strong>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">180 Days</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">300</span>
            </th>
            
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.1800</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ180','JobAlert','1800','1350')"/></a>
            </th>

        </tr>
        <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">                
                <strong class="font18">RAJ365</strong>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">365 Days</span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">No Limit</span>
            </th>
          
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.3000</span>
            </th>

            <th width="20%"  class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ365','JobAlert','3000','2250')"/></a>
            </th>
        </tr>
    </table><br />
      

     <table id="DisplayResume" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="4">
            <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;"><h3>Display Resume</h3></div>
                <ul>
                    <li>Plan - Display Resume DPR</li>
                </ul>
            </th>
        </tr>       
        
        <tr class="fe">
            <th class="bdrl_blue valignt highlight">                
                 <strong class="font18">DPR30</strong>                
            </th>
            <th width="20%" class="bdrl_blue valignt highlight">
                <span class="font18">30 days</span>                
            </th>
            
            <th width="20%" class="bdrl_blue valignt highlight">
                <span class="font12">Rs.360</span>
            </th>

            <th width="20%" class="bdrl_blue valignt highlight">
               <a><img src="../../content/images/subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('DPR30','DisplayResume','360','270')"/></a>                
            </th>
        </tr>

         <tr class="FE">
            <th class="bdrL_blue valignT highlight">                
                <strong class="font18">DPR90</strong>                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">90 Days</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.900</span>
            </th>

             <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('DPR90','DisplayResume','900','675')"/></a>
            </th>
        </tr>
      <tr class="FE">
            <th class="bdrL_blue valignT highlight">                
                <strong class="font18">DPR180</strong>                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">180 Days</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">Rs. 1500</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('DPR180','DisplayResume','1500','1125')"/></a>
            </th>

        </tr>
        <tr class="FE">
            <th class="bdrL_blue valignT highlight">                
                <strong class="font18">DPR365</strong>
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">365 Days</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.2500</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('DPR365','DisplayResume','2500','1875')"/></a>
            </th>
        </tr>
        </table>

<center><h2>Special Employer Plans</h2></center>
   
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
                <strong class="font12">RAT25</strong><br />
                <div id="RAVacancy" style="display: none">Featured Employer</div>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">30</span>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">25</span>
            </th>

             <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">1</span>
            </th>
            
            <th width="20%" class="bdrL_blue valignT highlight">
                 <span class="font18">Rs.360</span>
                <br />
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('RAT25','Resume Alert','360','270')"/></a>
            </th>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT75','Resume Alert','1000','750')"/></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT125','Resume Alert','1500','1125')" /></a>
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
                 <span class="font18">625</span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">20</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.4500</span>
                
             
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
               <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT500','Resume Alert','4500','3375')" /></a>
            </th>
        </tr>

        <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATSILVER<span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATSILVER','Resume Alert','8200','6150')" /></a>
            </th>
        </tr>

         <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATGOLD<span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATGOLD','Resume Alert','14995','11246')" /></a>
            </th>
        </tr>

         <tr>
             <th colspan="3"  class="bdrL_blue valignT highlight">
                <strong class="font12">RATCOMBO<span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATCOMBO','Resume Alert','24995','18746')" /></a>
            </th>
        </tr>

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="11">
                <ul>
                    <li>Vacancies posted will be displayed on top of all suitable Job search & Alert will be live for 30 days from the date of activation or till 25 Resumes per vacancy is sent whichever is earlier.</li>                    
                </ul>
            </th>
        </tr>
    </table>
           
    <br />

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
                <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('SS','SpotSelection','500','375')" /></a>
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
                <strong class="font12">HORS25</strong><br />
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">25 + 100 Emails</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">30</span> <span class="font20">
                </span>
            </th>

            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
              <span class="font18">Rs.500</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS25','Hot Resumes','500','375')" /></a>
                
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS100','Hot Resumes','1500','1125')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS500','Hot Resumes','3000','2250')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORS1000</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1000 + 2000 Emails</span>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS1000','Hot Resumes','5000','3750')" /></a>
            </th>
        </tr>
        <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSSILVER <span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSSILVER','Hot Resumes','12995','9746')" /></a>
            </th>
        </tr>

         <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSGOLD<span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSGOLD','Hot Resumes','24995','18746')" /></a>
            </th>
        </tr>

         <tr>
           <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font12">HORSCOMBO<span class="red">*</span></strong><br />
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSCOMBO','Hot Resumes','29995','22496')" /></a>
            </th>
        </tr>

        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
               <ul>
                    <li>All Vacancies posted will be displayed on top of all suitable Job search & Hot resumes will be active for Valid days from the date of activation or till number of contact details as per the plan is viewed , whichever is earlier.</li>
                </ul>
            </th>
        </tr>
    </table>
    <br />

       <%} else {  %>
        <h3>You are not Logged in as a admin. Admin have rights to see this page.</h3>
       <%} %>

     <div id="wait" style="display:none;z-index: 1199;width:69px;height:89px;border:1px solid black;position:absolute;top:50%;left:50%;padding:2px;">
        <img src="<%=Url.Content("~/Areas/Admin/Content/Images/demo_wait.gif")%>" width="64" height="64" alt="Loading" /><br/>Loading..
     </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>


    <script type="text/javascript">
        //Candidates vas subscriptions
        function vas_confirmcandidates(plan, vasType, Amount, discountAmount) {

            if (confirm("Confirm the order to buy for Rs. " + discountAmount + "")) {
                if (plan != undefined) {
                    //$("#txt").load("demo_ajax_load.asp");
                    $("#wait").show();
                    $.ajax({
                        url: '/candidates/candidatesvas/Subscribed',
                        type: 'POST',
                        data: { 'Plan': plan, 'VasType': vasType,'Amount':Amount, 'DiscountAmount':discountAmount },
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


        // Employer Vas Subscriptions


        function vas_confirm(plan, vasType,Amount, discountAmount) {

            if (confirm("Confirm the order to buy for Rs. " + discountAmount + "")) {

                if (plan != undefined) {
                    $("#wait").show();
                    $.ajax({
                        url: '/employer/employervas/Subscribed',
                        type: 'POST',
                        data: { 'Plan': plan, 'VasType': vasType, 'DiscountAmount': discountAmount },
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

        function vas_confirmcombo(plan, amount) {

            if (confirm("Confirm the order to buy for Rs. " + amount + "")) {

                if (plan != undefined) {
                    $("#wait").show();
                    $.ajax({
                        url: '/employer/employervas/SubscribedComboPlans',
                        type: 'POST',
                        data: { 'Plan': plan, 'Amount': amount },
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
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">

</asp:Content>

