<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VasPlans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

         <% Html.BeginForm("Index", "Candidatevas", FormMethod.Post, new { @id = "VasFES" }); { %>

         <table id="JobAlert" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        
        <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="12">
                 <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;"><h3>Job Alert</h3></div>
                <ul>
                    <li>Suitable Vacancies will be sent to you by SMS/Email immediately on posting a Vacancy</li>
                </ul>
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
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('RAJ90','JobAlert','995')"/></a>
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
                <%--<a href="<%: Url.Action("Payment", "CandidatesVas") %> "><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('RAJ180','JobAlert','1800')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('RAJ180','JobAlert','1800')"/></a>
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
                <%--<a href="<%: Url.Action("Payment", "CandidatesVas")%>"><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('RAJ365','JobAlert','3000')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('RAJ365','JobAlert','3000')"/></a>
            </th>
        </tr>

        <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="12">
                    <p>Job Alert will be active for Valid days from the date of activation or till the number of Vacancies as per the plan is sent by the system, whichever is earlier.</p>
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
        
        <%--<tr class="FE">
            <th class="bdrL_blue valignT highlight">                
                 <strong class="font18">DPR30</strong>                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font18">30 Days</span>                
            </th>
            
            <th width="20%" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.360</span>
            </th>

            <th width="20%" class="bdrL_blue valignT highlight">
                <a href="<%: Url.Action("Payment", "CandidatesVas") %> "><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR30','DisplayResume','360')"/></a>                
            </th>
        </tr>--%>
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
                <%--<a href="<%: Url.Action("Payment", "CandidatesVas") %> "><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR90','DisplayResume','900')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR90','DisplayResume','900')"/></a>
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
                <%--<a href="<%: Url.Action("Payment", "CandidatesVas") %> "><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR180','DisplayResume','1500')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR180','DisplayResume','1500')"/></a>
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
               <%-- <a href="<%: Url.Action("Payment", "CandidatesVas")%>"><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR365','DisplayResume','2500')"/></a>--%>
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('DPR365','DisplayResume','2500')"/></a>
            </th>
        </tr>
    </table>

    <br />

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

        <%--<tr>
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
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('CRD195','CRDPurchase','195')" />
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
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('CRD545','CRDPurchase','545')" />
                </a>
            </th>
         </tr>--%>
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
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('CRD1495','CRDPurchase','1495')" />
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
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('CRD2695','CRDPurchase','2695')" />
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
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('CRD4495','CRDPurchase','4495')" />
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

    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Spot Interview</h3>
    </div>
  
    <table id="SpotInterview" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="7">
                 <strong style="font-family:Arial; font-size:12px; font-weight:bold">Right Interviews at The Right Time!!! Our Specialist Can Fix Interviews For You...</strong>
            </th>
        </tr>

        <tr>
            <th colspan="3" class="bdrL_blue valignT highlight">
                <ul>
                    <b>Spot Interview</b>
                </ul>
                
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
             <span class="font18">Contact Us for Pricing</span>              
                <br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a onclick="return SI_confirm('SI','SpotInterview','0')">Click here to Get a call back from Us
                <%--<img src="../../Content/Images/Subscribe now on click.png" alt="subscribe" width="100px" height="25px" class="btn" onclick="return SI_confirm('SI','SpotInterview','0')" />--%></a>
            </th>
        </tr>
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="6">
                   <ul>
                        <li>Dial4Jobz will organise Teleconference with Suitable Vacancies</li>
                        <li>Proceed with Face to face Interview after Teleconference if needed for Selection.</li>
                        <li>To know fee structure Contact Dial4jobz – Call 044 - 44455566</li>
                   </ul>
            </th>
        </tr>
    </table>
    <br />


     <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>SMS Purchase</h3><%--style="text-align:center;"--%>
    </div>

    <table id="smsPurchase" class="Vasprice" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <strong class="font12">SMS100</strong><br />
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 100 + service tax </span>
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                Total Rs. 113/-
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS100','SMSPurchase','113')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <strong class="font12">SMS200</strong><br />
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 195 + service tax </span>
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                Total Rs. 220/-
            </th>
           <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS200','SMSPurchase','220')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <strong class="font12">SMS500</strong><br />
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 450 + service tax </span>
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                Total Rs. 506/-
            </th>
            <th width="20%"  width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS500','SMSPurchase','506')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <strong class="font12">SMS1000</strong><br />
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 875 + service tax </span>
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                Total Rs. 984/-
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS1000','SMSPurchase','984')" /></a>
            </th>
        </tr>
        <tr>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <strong class="font12">SMS5000</strong><br />
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <span class="font18">Rs. 4250 + service tax </span>
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                Total Rs. 4776/-
            </th>
            <th width="20%"  class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px" onclick="return vas_confirm('SMS5000','SMSPurchase','4776')" /></a>
            </th>
        </tr>        
    </table><br />
     

    <table id="ResumeWrite" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="6">
            <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;"><h3>Resume Writing</h3></div>
                <ul>
                      
                    <li>Get your resume developed by experts.</li>
                    <li>We have a team of experts specially for you, who knows what recruiters look for in a resume </li>
                    <li>Our resume writing specialist will design your resume by consulting you regularly</li>.
                    <li>It will take 7 working days to prepare your Resume & deliver</li>
                </ul>
            </th>
        </tr>
        <tr>
             <th colspan="3" class="bdrL_blue valignT highlight">  
                <strong class="font14">Plan </strong>
            </th>

             <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">  
                <strong class="font14">Experience </strong>
            </th>
             <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">  
                <strong class="font14">Price </strong>
            </th>
            <th  class="bdrL_blue valignT highlight">  
               
            </th>
        </tr>

       

        <tr class="FE">
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font18">Fresher (FSR) </strong>
                <br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">0 to 1 year</span>                
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.500</span> 
                <br />                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('FSR','ResumeWriting','500')"/></a>
                <div id="feSubscribe" style="display: none">Featured Employer</div>
            </th>
            
        </tr>

        <tr class="FE">
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Junior & Middle level (JNR)</strong><br />                
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1 to 8 Years</span><br />                
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12"> Rs.1000</span> 
                <br />                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('JNR','ResumeWriting','1000')"/></a>                
            </th>
            

        </tr>

        <tr class="FE">
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Senior level (SNR) </strong>
                <br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">8+ years</span><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.1500</span> <span class="font20"></span>
                <br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
              
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('SNR','ResumeWriting','1500')"/></a>
                
            </th>
        </tr>

         <tr>
            <th class="bdrL_blue valignT highlight" colspan="6">
                <h3>Express Resume Writing</h3>
                <strong style="font-family:Bookman Old Style; font-size:14x; font-weight:bold">Express Resume will be delivered in 4 working days.</strong>
            </th>
        </tr>
        <tr class="FE">
         
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Fresher (FSE)</strong><br />               
                    
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">0 to 1 year</span>
                <br />                
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12"> Rs.800</span> <span class="font20"></span>
                <br />                
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('FSE','ExpressResumeWriting','800')"/></a>
            </th>
        </tr>
        <tr class="FE">
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Junior & Middle level (JNE)</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">1 to 8 years </span>
                <br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.1300</span> <span class="font20"></span>
                <br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('JNE','ExpressResumeWriting','1300')"/></a>
            </th>
        </tr>
        <tr class="FE">
            <th colspan="3" class="bdrL_blue valignT highlight">
                <strong class="font12">Senior Level(SNE)</strong><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">8+ years</span><br />
            </th>
            <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font12">Rs.1800</span> <span class="font20"></span>
                <br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn"  onclick="return vas_confirm('SNE','ExpressResumeWriting','1800')"/></a>
            </th>
        </tr>
    </table><br />

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

    <table id="whoIsEmployer" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th rowspan="2" class="bdrL_blue valignT highlight" colspan="3">
             <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;"><h3>Who is the Employer?</h3></div>
                <%:Html.ActionLink("Get to know about the Prospective Employer", "WhoIsEmployer", "CandidatesVas")%>
            </th>
            <th style="text-align:center;" class="bdrL_blue valignT highlight" colspan="2">
                Regular                
            </th>

            <th style="text-align:center;" class="bdrL_blue valignT highlight" colspan="2">
                Rs. 562
            </th>

            <th class="bdrL_blue valignT highlight" colspan="6">
                <a><img src="../../Content/Images/Subscribe now on click.png" class="btn" onclick="return vas_confirm('KER','WhoIsEmployer','562')" /></a>
            </th>
        </tr>
        <tr><th style="text-align:center;" class="bdrL_blue valignT highlight" colspan="2">
                Express
            </th>

            <th style="text-align:center;" class="bdrL_blue valignT highlight" colspan="2">
                Rs.1124
            </th>

            <th class="bdrL_blue valignT highlight" colspan="6">
                <a>
                    <img src="../../Content/Images/Subscribe now on click.png" class="btn" onclick="return vas_confirm('KEE','WhoIsEmployer','1124')" /></a>
            </th></tr>

       <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="12">
               <strong class="font12">Feedback on the prospective employer will be sent in *5 working days for regular & in 2 working days for express plan.</strong>
            </th>
        </tr>          
    </table><br />

    

   <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:16px;"><h3>Premium Resume</h3></div>
    <table id="PremiumResume" class="Vasprice" cellpadding="0" cellspacing="0" border="0"
        width="100%">
        <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">Your Resume can be displayed with or without contact details</span><br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight" rowspan="3">
                <a href="">Contact us for Price</a>
            </th>
        </tr>
        <tr class="FE">
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font18">Your Resume will be referred on priority to the client of Dial4jobz</span>
            </th>
        </tr>
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font18">Spot selection team & recruiters will check your availability before referring</span>
            </th>
        </tr>
    </table>

 <%--<div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:16px;"><h3>Resume Jet</h3></div>
    <table id="ResumeJet" class="Vasprice" cellpadding="1" cellspacing="1" border="0" width="100%">
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">Your CV reaches top consultants in just 4 working days</span><br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight" rowspan="4">
                <span class="font12">Coming Soon</span>                
            </th>
        </tr>
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">You become eligible for unadvertised vacancies</span><br />
            </th>
        </tr>
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">Saves you the effort of finding & emailing your CV to the right consultants</span><br />
            </th>
        </tr>
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">Reach out to the top consultants from all industries & sectors</span><br />
            </th>
       
        </tr>
        <tr>
            <th colspan="5" class="bdrL_blue valignT highlight">
                <span class="font12">You can choose domestic consultants or Overseas consultants and both.</span><br />
            </th>
            <th width="20%" class="bdrL_blue valignT highlight">
            </th>
        </tr>
    </table>--%>
          <%} %>
          <% Html.EndForm(); %>

    <div id="wait" style="display:none;z-index: 1199;width:69px;height:89px;border:1px solid black;position:absolute;top:50%;left:50%;padding:2px;">
        <img src="<%=Url.Content("~/Areas/Admin/Content/Images/demo_wait.gif")%>" width="64" height="64" alt="Loading" /><br/>Loading..
     </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>

<script type="text/javascript">

    // $(document).ready(function () {
        function vas_confirm(plan, vasType, amount) {

            if (confirm("Confirm the order to buy for Rs. " + amount + "")) {

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
        // });

        /*For SI separte function*/
        function SI_confirm(plan, vasType) {
            if (confirm("Thanks for subscribing SI. our team will contact u within one working day.")) {

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

</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
<div><h5>Sales Enquiries</h5>
    <ul>
 <%--   <li><a title ="contact him">Ganesan(Business Head)</a></li>--%>
    <li>Mobile Number:+91-9381516777 </li>
    <li>Contact Number:044 - 44455566 </li>
    <li>E-Mail:smc@dial4Jobz.com </li>
    </ul>    
    </div>

    <div><h5>Customer Support</h5>
    <ul>
    <li><a title ="contact him">Manikandan</a></li>
    <%--<li>Mobile Number:+91-9841087952 </li>--%>
    <li>Contact Number:044 - 44455566 </li>
    <li>Email: smo@dial4jobz.com</li>
 <%--   <li>E-Mail:mani@jobspoint.com </li>--%>
    </ul>    
    </div>    
</asp:Content>
