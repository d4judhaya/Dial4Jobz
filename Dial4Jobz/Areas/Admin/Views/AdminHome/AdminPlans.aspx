<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Admin Plans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<center><h2>Admin Candidate Plans</h2></center>

 <table id="CRD" class="Vasprice" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr align="center">
            <th width="18%"  style="text-align:center;" class="bdrL_blue valignT highlight">
               <strong class="font12">Plan</strong><br />
            </th>
            <th width="15%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">Vacancies Count</strong><br />
            </th>
            <th width="15%" style="text-align:center;" class="bdrL_blue valignT highlight">
               <strong class="font12">Incoming Searches</strong><br />
            </th>
            <th width="15%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">Duration Days</strong><br />
            </th>
            <th width="15%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">Gross Rs.</strong><br />
            </th>
            <th width="10%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">Tax @ 12.36%</strong><br />
            </th>
            <th width="15%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">Net Rs.</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight"></th>            
        </tr>

        <tr>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">CRD195</strong><br />
            </th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">10</span>
            </th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">Unlimited</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">7</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">174</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">21</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">195</th>
            <th width="20%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirmcandidates('CRD195','CRDPurchase','195')" />
                </a>
            </th>
         </tr>
        <tr>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><strong class="font12">CRD545</strong><br /></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><span class="font18">50</span></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">Unlimited</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">30</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">485</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">60</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">545</th>
            <th width="20%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirmcandidates('CRD545','CRDPurchase','545')" />
                </a>
            </th>
         </tr>
        <tr>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><strong class="font12">CRD1495</strong><br /></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><span class="font18">90</span></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">Unlimited</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">90</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">1331</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">164</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">1495</th>
            <th width="20%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirmcandidates('CRD1495','CRDPurchase','1495')" />
                </a>
            </th>
         </tr>
        <tr>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><strong class="font12">CRD2695</strong><br /></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><span class="font18">180</span></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">Unlimited</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">180</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">2399</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">296</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">2695</th>
            <th width="20%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirmcandidates('CRD2695','CRDPurchase','2695')" />
                </a>
            </th>
         </tr>
        <tr>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><strong class="font12">CRD4495</strong><br /></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight"><span class="font18">365</span></th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">Unlimited</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">365</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">4001</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">494</th>
            <th width="11%"  style="text-align:center;" class="bdrL_blue valignT highlight">4495</th>
            <th width="20%"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirmcandidates('CRD4495','CRDPurchase','4495')" />
                </a>
            </th>
         </tr>
            <tr>
            <th class="bdrL_blue valignT highlight" colspan="8" style="padding-left:5px;" >
                   <ul>
                        <li>1. Suitable Vacancies Posted by Employer will be sent to you by SMS/Email immediately</li>
                        <li>2. Job Alert will be active for Valid days from the date of activation or till the number of Vacancies as per the plan is sent by the system, Whichever Is Earlier.</li>
                        <li>3. Your Resume will be on top of all relevant Search Result Pages.</li>
                        <li>4. Your Resume along with the contact details, will be displayed for all employers looking for suitable candidates.</li>
                        <li>5. Your Mobile Number & Email Id will be displayed in Search Result page itself, when employer searches for their vacancy.</li>
                        <li>6. Paid and Free Employers can see your contact details in the resume free of cost.</li>
                   </ul>
            </th>
        </tr>
        
    </table><br />

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
                <%--<%:Html.TextBox("Amount","Rs.100") %>--%>
                <span class="font12">Rs.100</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ7','JobAlert','100')"/></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('RAJ30','JobAlert','360')"/></a>
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
               <a><img src="../../content/images/subscribe now on click.png" class="btn"  onclick="return vas_confirmcandidates('DPR30','DisplayResume','360')"/></a>                
            </th>
        </tr>
        </table>

<center><h2>Admin Employer Plans</h2></center>

<div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Basic Plan</h3>
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
        
         <tr>
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
        </tr>
     </table>


   <h3>Free SMS Purchase</h3>
          
    <table id="smsPurchase" class="Vasprice" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">100 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 100 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 113/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('FREESMS100','SMSPurchase','113')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">200 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 195 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 220/-
            </th>
           <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('FREESMS200','SMSPurchase','220')" /></a>
            </th>
        </tr>

        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">300 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs.290 + service tax  </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 325/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
               <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('FREESMS300','SMSPurchase','325')" /></a>
            </th>
        </tr>

        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">500 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 450 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 506/-
            </th>
            <th width="20%" style="text-align:center;" width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('FREESMS500','SMSPurchase','506')" /></a>
            </th>
        </tr>
       <%-- <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">1000 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 875 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 984/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS1000','SMSPurchase','984')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font12">5000 sms</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 4250 + service tax </span>
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                Total Rs. 4776/-
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS5000','SMSPurchase','4776')" /></a>
            </th>
        </tr>        --%>
    </table>   
    
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" class="btn" width="100px" height="25px" onclick="return vas_confirm('RAT25','Resume Alert','360')"/></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT75','Resume Alert','1000')"/></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT125','Resume Alert','1500')" /></a>
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
               <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT500','Resume Alert','4500')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATSILVER','Resume Alert','8200')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATGOLD','Resume Alert','14995')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RATCOMBO','Resume Alert','24995')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS25','Hot Resumes','500')" /></a>
                
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS100','Hot Resumes','1500')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS500','Hot Resumes','3000')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORS1000','Hot Resumes','5000')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSSILVER','Hot Resumes','12995')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSGOLD','Hot Resumes','24995')" /></a>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('HORSCOMBO','Hot Resumes','29995')" /></a>
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

    <h3>COMBO NEW OFFER</h3><br />

    <table id="ComboServices" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
             <th width="20%" colspan="2" class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red"> Rs. 99 + TAX Each </span><br />
                 Resume Alert Services (OR) Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates  </span><br />
                 <span class="font18">(OR)</span><br />
                 <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                 <span class="font18">Access Database</span><br />
                 <span class="maintext">Send 100 Emails To Suitable Candidates</span>

            </th>

             <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18"><b>7 Days / 25 Alerts</b>- Whichever Is Earlier </span><br />
                 <span class="font18">(OR)</span><br />
                 <span class="font18"><b>7 Days / 25 Contacts Viewed</b>- Whichever Is Earlier</span>
            </th>
                  
         
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px"/></a>
            </th>
         
        </tr>
               

        <tr>
            <th width="20%" colspan="2" class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red">Rs. 195/- + TAX COMBO</span> <br />
                  Resume Alert Services (&) Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                 <span class="font18">Access Database<br /></span>
                 <span class="maintext"> Send 100 Emails To Suitable Candidates</span>
             </th>

            <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">7 Days / 25 Alerts -  Whichever Is Earlier</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18">7 Days / 25 Contacts Viewed - Whichever Is Earlier</span>
            </th>

                     
            <th width="24%" class="bdrL_blue valignT highlight">
               <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirmcombo('RAT,HORS','219')" /></a>
            </th>
        </tr>

        <tr>
             <th width="20%" colspan="2"  class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red">Rs. 299/- + TAX SUPER COMBO </span><br />
                Resume Alert Services & Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates </span><br />
                  <span class="font18">(AND)</span><br />
                  <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                  <span class="font18">Access Database</span><br />
                  <span class="maintext">Send 200 Emails To Suitable Candidates</span><br />
            </th>

            <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">7 Days / 50 Alerts - Whichever Is Earlier</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18"> 7 Days / 50 Contacts Viewed - Whichever Is Earlier</span>
            </th>
         
             
                <th width="24%" class="bdrL_blue valignT highlight">
                    <a><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirmcombo('RAT,HORS','336')" /></a>
                </th>
        </tr>
      
    </table>

 

     <div id="wait" style="display:none;z-index: 1199;width:69px;height:89px;border:1px solid black;position:absolute;top:50%;left:50%;padding:2px;">
        <img src="<%=Url.Content("~/Areas/Admin/Content/Images/demo_wait.gif")%>" width="64" height="64" alt="Loading" /><br/>Loading..
     </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>


    <script type="text/javascript">
     //Candidates vas subscriptions
     function vas_confirmcandidates(plan, vasType, amount) {

         if (confirm("Confirm the order to buy for Rs. " + amount + "")) {debugger;

             if (plan != undefined) {
                 //$("#txt").load("demo_ajax_load.asp");
                 $("#wait").show();
                 $.ajax({
                     url: '/candidates/candidatesvas/Subscribed',
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


     // Employer Vas Subscriptions


     function vas_confirm(plan, vasType, amount) {

         if (confirm("Confirm the order to buy for Rs. " + amount + "")) {

             if (plan != undefined) {
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

     /*Combo plans subscribe*/

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

