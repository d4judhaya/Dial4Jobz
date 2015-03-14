using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models
{
    public class Constants
    {
        public struct Path
        {
            //public const string Root = "http://dial4jobz.com";
            public const string Root = "";
            //public const string Root = "http://dial4jobz.com";
        }

        public static DateTime CurrentTime()
        {
            //return DateTime.UtcNow.AddHours(5).AddMinutes(30);

            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            return timeZone;
        }

        // This method is used to Encrypt
        public static string EncryptString(string string_To_Encrypt)
        {
            try
            {
                Byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(string_To_Encrypt);

                string encryptedString = Convert.ToBase64String(b);

                return encryptedString;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        public static string DecryptString(string string_To_Decrypt)
        {
            try
            {
                Byte[] b = Convert.FromBase64String(string_To_Decrypt);

                string decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return decryptedString;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public struct PageCode
        {
            public const string AddAdmin = "Add Admin";
            public const string AddCandidate = "Add Candidate";
            public const string AddEmployer = "Add Employer";
            public const string AddJob = "Add Job";
            public const string ImportData = "Import Data";
            public const string CandidateSummaryFunction = "Candidate Summary Function";
            public const string CandidateSummaryIndustry = "Candidate Summary Industry";
            public const string EmployerSummary = "Employer Summary";
            public const string EmployerReport = "Employer Report";
            public const string CandidateReport = "Candidate Report";
            public const string CandidateFullReport = "Candidate Full Reports";
            public const string UserReport = "User Report";
            public const string ActivationReport = "Activation Report";
            public const string ActivatedReport = "Employer Activated Report";
            public const string CandiateActivatedReport = "Candidate Activated Report";
            public const string CandidateActivationReport = "Candidate Activation Report";
            public const string CandidateRegisteredReport = "Candidate Registered Report";
            public const string ConsultantRegisteredReport = "Consultant Registered Report";
            public const string VacancyReport = "Vacancy Report";
            public const string AddChannelPartner = "Add Channel Partner";
            public const string AllowSeeContactDetails = "Admin Access Contact";
            public const string SendInvoiceBymail = "Invoice Bill";
            public const string ChannelPartnerReport = "Channel Partner Report";
            public const string ConsultantActivationReport = "Consultant Activation Report";
            public const string ConsultantActivatedReport = "Consultant Activated Report";
            public const string AllowSpecialPlans = "Allow Special Plans";
            public const string AddConsultant = "Add Consultant";
            public const string AddWithoutMobile = "Add Without Mobile"; 
                 

        }

        public struct ChannelLoginValues
        {
            public const int ChannelRole = 0;
            public const int ChannelId = 1;
            public const int ChannelName = 2;
            public const int ChannelEmail = 3;
        }
               

        public struct SmsSender
        {
            // ssms api credentials
            public const string UserId = "dialjob";
            public const string Password = "udhaya88";
            public const string senderId = "DLJOBZ";
            public const string Type = "Individual";

            //sms routesms credentials
            public const string SecondaryUserName = "dial4jobzt";
            public const string SecondaryPassword = "wldfslkj";
            public const string Secondarysource = "DLJOBZ";
            public const string SecondaryType = "0";
            public const string Secondarydlr = "1";

            //marketing sms routesms credentials
            public const string MarketingUserName = "dial4jobzp";
            public const string MarketingPassword = "skmskjsk";
            public const string Marketingsource = "DLJOBZ";
            public const string MarketingType = "0";
            public const string Marketingdlr = "1";
            
        }
                

        public struct EmailSender
        {
            public const string EmployerSupport = "Dial4Jobz <resume@dial4jobz.in>";
            public const string CandidateSupport = "Dial4Jobz<vacancy@dial4jobz.in>";
            public const string VasEmailId = "ganesan@dial4jobz.com";
        }

        public struct EmailSubject
        {
            public const string ClientPost = "Job | Vacancy Submission Successfull";
            public const string Registration = "Job | Registration Successfull";
            public const string CandidateMatch = "Job | Response for your Jobposting";
            public const string JobMatch = "Job | Vacancy Details from Dial4Jobz";
            public const string RAJMatch = "Job | Response for your Job Alert";
            public const string CandidateUpdateProfile = "Job | Resume Submission Successfull";
            public const string SendMailtoClientforCandidateApplyJob = "Job | Dear Recruiter";
            public const string EmailVerification = "Job | Email Verification";
            public const string PasswordReset = "Job | Reset Password";
            public const string PaymentDetails = "Job | Payment Details Submitted";
            public const string SubmitOrder = "Job | Order Submitted Successfully";
            public const string receivedPayment = "Job | Payment Received Successfully";
            public const string PaymentRegistration = "Job | Employer Plan Activated!!!";
            public const string EmployerVasRegisterToOrganization = "Job | Reg an Employer Vas Registration";
            public const string RegisterCandidateVasRegistration = "Job | Candidate Registering Vas Details";
            public const string SendBankDetails = "Job | Bank Details";
            public const string ActivateVacancy = "Job | Your RAT vacancy Activated";
            public const string ChannelPartnerRegistration = "Channel Partner Registration Successful";
            public const string SendInvoiceBymail = "Invoice Bill";
            public const string VerifyUpdateDetails = "Job | Verify Your Update Details";
            public const string ChannelUserRegistration = "Channel User Registration Successfull";
            public const string HorsFeedBackByMail = "Thanks For Your Feedback";
            public const string EmployerHorsFeedBack = "Employer HORS FeedBack";
            

            //sms
            public const string SMSSubscription = "SMS Subscribed";
            public const string SMSActivated = "SMS Activated";

            
        }

        public struct EmailBody
        {
            public const string EmployerFeedBackAlert =
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
             @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
             @"Dear <b>[NAME]</b><br/>" +
             @"Greetings from Dial4jobz.com<br/>" +
             @"Thank you for your valuable feedback. Below Feedback given by you will help us to serve you better.<br /><br />" +
             @"<b><u>You Have Stated as Below:</u></b><br /><br />" +
             @"[SHORTLIST]<br /><br />" +
             @"[SELECTED]<br /><br />" +
             @"[ISSUES]<br /><br />" +
             @"[MESSAGE]<br /><br />" +
             @"For any suggestions / queries contact: 044-44455566/33555777 (Between 9.30 AM to 6.00 PM) or e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/>" +
             @"Best Regards,<br/>" +
             @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
             @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
             @"<b>Important Notice for Employers</b><br/>" +
             @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
             @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
             @"<b>::Disclaimer::</b><br/>" +
             @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""LINK"">terms and conditions</a> & <a href=""LINK"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""LINK"">delete this account</a>. 
                (Login details are required to delete the account)" +
             @"</span>";

            public const string EmployerFeedBackToCRM =
                @"<div dir='ltr'><div><img src=""http://www.dial4jobz.in/Content/Images/logo_with_number.jpg"" width='200px' height='75px' alt='' /></div>" +
                @"<br /><div style='font-family: Calibri; font-size:16px; line-height:1.5'><b>Feedback Form From Employer(HORS)</b><br/><br/><b><u>Details of Employer:</u></b><br/></div><br/>" +
                @"<div style='font-family: Calibri; font-size:15px; line-height:1.5'>" +
                @"<table style='background-color:white;font-family: Calibri; font-size:15px; line-height:1.5;'>" +
                @"<tbody>" +
                @"<tr><td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Employer Name: <b>[NAME]</b></span></td>" +
                @"<td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Order No: <b>[ORDERNO]</b></span></td></tr>" +
                @"<tr><td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Employer Id: <b> [ID]</b></span></td> " +
                @"<td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Plan Subscribed: <b>[PLAN]</b></span></td></tr>" +
                @"<tr><td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Email-Id: <b>[EMAILID]</b></span><br></td>" +
                @"<td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Validity: <b>[DURATION] Days</b></span></td></tr>" +
                @"<tr><td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Mobile Number: <b>[MOBILE]</b></span></td>" +
                @"<td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Activation Date: <b>[DATE]</b></span></td></tr>" +
                @"<tr><td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Resumes : <b>[EMAIL_COUNT]</b></span></td>" +
                @"<td width='337'><span style='font-family: Calibri; font-size:15px; line-height:1.5'>Amount Rs: <b>[AMOUNT]</b></span><br /></td></tr>" +
                @"<tr><td width='337'> <span style='font-family: Calibri; font-size:15px; line-height:1.5'>Remaining Views: <b>[REMAINING_COUNT]</b></span><br /> </td></tr>" +
                @"</tbody></table></div><br/>" +
                @"<div style='font-family: Calibri; font-size:16px; line-height:1.5'>" +
                @"<b><u>The following queries posted by the employer,</u></b><br />" +
                @"1. <span style='font-family: Calibri; font-size:15px; line-height:1.5'>Is Employer able to shortlist from this lot anyone? <br />Ans: [SHORTLIST]</span><br /><br />" +
                @"2. <span style='font-family: Calibri; font-size:15px; line-height:1.5'>Have employer selected anyone? <br />Ans: [SELECTED]</span><br /><br />" +
                @"3. <span style='font-family: Calibri; font-size:15px; line-height:1.5'>Did employer encounter any Issues while searching? <br />Ans: [ISSUES]</span><br /><br />" +
                @"</div><span style='FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px'>" +
                @"<b>::Disclaimer::</b><br />By your positive acts of registering on <a target='_blank' href='http://www.dial4jobz.com/'>www.dial4jobz.com</a>, you agree to be bound by the <a href='../Home/Terms'>terms and conditions</a> &amp; <a href='../Home/Privacy'>privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use<a target='_blank' href='http://www.dial4jobz.com/'>www.dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href='mailto:smc@dial4jobz.com'>smc@dial4jobz.com</a>.<br /> In case you do not wish to be registered with <a target='_blank' href='http://www.dial4jobz.com/'>www.dial4jobz.com</a> any longer you can<a href='' link''=''>delete this account</a>. " +
                @"(Login details are required to delete the account)" +
                @"</span><br clear='all'><div><br /></div></div>";


            public const string ConsultantRegister =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5;"">" +
                @"Dear <b>[NAME]</b>,<br/><br/>" +
                @"Welcome to <b>Dial4Jobz</b><br/>" +
                @"<b>India's 1st new age interactive Job Portal.</b><br/>" +
                @"Your Consultant account at <a href=""www.dial4jobz.com"">dial4jobz.com</a> has been created. Thanks for Choosing Dial4jobz.com.<br/>" +
                @"Advertising your Vacancy in <a href=""www.dial4jobz.com"">dial4jobz.com</a> is Free and unlimited<br/>" +
                @"<b>Please click this link to :</b><a href='[LINK]'>[LINK_NAME].</a><br/><br/>" +
                @"Kindly login with details of your account mentioned below.<br/>" +
                @"<b>Your Username :</b> [USERNAME]<br/>" +
                @"<b>Your Password :</b> [PASSWORD]<br/><br/>" +
                @"Keep this information confidential.<br/>" +
                @"Our e-mail validation is intended to verify the ownership of it. Thanks for your registration. We look forward in supporting your business with its staffing needs and strongly believe you will find the staff you need at <a href=""www.dial4jobz.com"">www.dial4jobz.com</a><br/><br/>" +
                @"<b>Value Added Services:</b><br/>" +
                @"<b>Consultant’s Resume:</b><br/>" +
                @"You can submit your Candidate’s Resume without displaying contact details. Only your company name, contact person & contact details are viewable by any employer. Employer cannot reach your candidate directly without your knowledge. They can reach you & you can proceed further without any obligation.<br/>" +
                @"<b>Resume Alert (RAT)</b><br/>" +
                @"Get Resume Alert of any suitable candidates as soon as its submitted (RAT)<br/>" +
                @"<b>Hot Resumes (HORS)</b><br/>" +
                @"View Contact details of all suitable candidates to reach them immediately. Mail to suitable candidate.  SMS all suitable candidates to call for interview or to find their interest to join you or your Client.<br/>" +
                @"<b>Spot Selection (SS)</b><br/>" +
                @"Get connected through D4J with suitable candidates for Spot selection..(Spot Selection). Call 04433555777/04443331101 or <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a> For any queries or assistance, Please email to <b>Sr.Manager- Client Relation <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a></b><br/><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">dial4jobz.com</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Consultant</b><br/>" +
                @"The Information on candidates or Employer Shared/ sent/ displayed to you is as communicated or furnished by the Candidate or Employer over telephone/ Internet and it shall be the sole responsibility of the Consultant before appointing or dealing with them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";


            public const string ComboPlans = 
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear [NAME]<br/>" +
                @"Thanks for Subscribing [PLAN_NAME] On receipt of [AMOUNT] Your Plan will be activated within 1 Working Day.<br/>" +
                @"Employer Name: [NAME]<br/>" +
                @"Employer Id: [ID]<br/>" +
                @"Order No: [ORDER_NO]<br/>" +
                @"Email-Id: [EMAIL]<br/>" +
                @"Mobile No: [MOBILE]<br/>" +
                @"Plan Subscribed: [PLAN_NAME]<br/>" +
                @"Subscribed Date: [SUBSCRIBED_DATE]<br/>" +
                @"Amount Rs: [AMOUNT]<br/>" +
                @"No.of Vacancies: [VACANCIES]<br/>" +
                @"Amount Rs: [AMOUNT]<br/>" +
                @"Duration of Plan: [VALIDITY_DAYS]<br/>" +
                @"Number of Resume Contact Views: [VALIDITY_COUNT]<br/>" +
                @"Number of Email to Candidates: [EMAIL_COUNT] <br/>" +
                @"Subscribed by [SUBSCRIBED_BY].<br/>" +
                @"You can assign your vacancy for resume alert.<br/>" +
                @"You will be intimated by SMS/email as & when Suitable Candidate submits his/her resume either online or over phone & your Vacancy will be sent to those suitable JobSeekers.<br/>" +
                @"You will get Resume alert upto [VALIDITY_COUNT] candidates or for [VALIDITY_DAYS]  Days from the date of Assigning Vacancy for Resume Alert (whichever is earlier).<br/>" +
                @"You can View any number of resume & can view [VALIDITY_COUNT] contact details of Candidates under this Plan.You can send SMS to any number of candidates from Dial4jobz.com while your Combo Basic plan is active..(For sending SMS please buy SMS in your Account). <br/>" +
                
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string CandidateUpdateProfileAlert= 
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>"+
                @"Greetings from Dial4jobz.com<br/>"+
                @"You had initiated for change of following in your profile.If its done with your knowledge no need for any action.If you have not requested or initiated kindly Contact Mr.Mani CRM- Manager, immediately 044 - 44455566 or reply to this mail.<br/>" +
                @"<b><u>Changes Done</u>:</b><br/>" +
                @"<b>Previous  Email:</b> [EMAIL]<br/>" +
                @"<b>New Email id :</b> [NEW_EMAIL]<br/>" +
                @"<b>Registered By:</b> [REGISTERED_BY]<br/>"+
                @"<b>Modified By :</b> [CHANGED_BY]<br/>" +
                @"<b><u>Candidate Details:</u></b><br/>"+
                @"<b>Candidate Id:</b> [ID]<br/>"+
                @"<b>Mobile Number :</b> [CONTACT_NUMBER]<br/>" +
                @"<a href=""[VERIFY_LINK]"">[LINK_NAME]</a><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
               @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string CandidateUpdateMobileAlert =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>" +
                @"Greetings from Dial4jobz.com<br/>" +
                @"You had initiated for change of following in your profile.If its done with your knowledge no need for any action.If you have not requested or initiated kindly Contact Mr.Mani CRM- Manager, immediately 044 - 44455566 or reply to this mail.<br/>" +
                @"<b><u>Changes Done</u>:</b><br/>" +
                @"<b>Previous  Number:</b> [MOBILE_NUMBER]<br/>" +
                @"<b>New Number :</b> [NEW_MOBILE]<br/>" +
                @"<b>Registered By:</b> [REGISTERED_BY]<br/>" +
                @"<b>Modified By :</b> [CHANGED_BY]<br/>" +
                @"<b><u>Candidate Details:</u></b><br/>" +
                @"<b>Candidate Id:</b> [ID]<br/>" +
                @"<b>Email-Id :</b> [EMAIL]<br/>" +
                @"<b>Verification Code:[VERIFY_CODE]</b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string EmployerUpdateProfileAlert =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>" +
                @"Greetings from Dial4jobz.com<br/>" +
                @"You had initiated for change of following in your profile.If its done with your knowledge no need for any action.If you have not requested or initiated kindly Contact Mr.Mani CRM- Manager, immediately 044 - 44455566 or reply to this mail.<br/>" +
                @"<b><u>Changes Done</u>:</b><br/>" +
                @"<b>Previous  Email:</b> [EMAIL]<br/>" +
                @"<b>New Email id : </b>[NEW_EMAIL]<br/>" +
                @"<b>Registered By:</b> [REGISTERED_BY]<br/>" +
                @"<b>Modified By :</b> [CHANGED_BY]<br/>" +
                @"<b>Employer Details:</b><br/>" +
                @"<b>Employer Id:</b> [ID]<br/>" +
                @"<b>Mobile Number :</b> [MOBILE_NUMBER]<br/>" +
                @"<b><a href=""[VERIFY_LINK]"">[LINK_NAME]</a></b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string EmployerMobileUpdateAlert =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>" +
                @"Greetings from Dial4jobz.com<br/>" +
                @"You had initiated for change of following in your profile.If its done with your knowledge no need for any action.If you have not requested or initiated kindly Contact Mr.Mani CRM- Manager, immediately 044 - 44455566 or reply to this mail.<br/>" +
                @"<b><u>Changes Done</u>:</b><br/>" +
                @"<b>Previous  MobileNumber:</b> [MOBILE_NUMBER]<br/>" +
                @"<b>New MobileNumber : </b>[NEW_MOBILE]<br/>" +
                @"<b>Registered By:</b> [REGISTERED_BY]<br/>" +
                @"<b>Modified By :</b> [CHANGED_BY]<br/>" +
                @"<b>Employer Details:</b><br/>" +
                @"<b>Employer Id:</b> [ID]<br/>" +
                @"<b>Email :</b> [EMAIL]<br/>" +
                @"<b>Verification Code:[VERIFY_CODE]</b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";


            public const string reminderCandidate =
                  @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                  @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                  @"Dear [NAME]<br/>" +
                  @"Greetings From Dial4Jobz!!!<br/>" +
                  @"You May Have Missed Exciting Vacancies, since Your Activation of <b>[SERVICE_NAME]</b> is still pending...<br/>" +
                  @"Dial4Jobz is adding 100’s of Fresh Vacancies every day…<br/>" +
                  @"You Can Start Benefitting from our Services by Completing Your Activation very easily…<br/>" +
                  @"You Can Just <a href=[PAYMENT_LINK]>[LINK_NAME]</a> To Complete Your Activation<br/>" +
                  @"For Any Clarifications Call – 044 - 44455566 <br/>" +
                  @"<a href=""www.dial4jobz.com"">Dial4Jobz</a> Wishes You A Great Career Ahead!!!<br/>" +
                  @"Best Regards,<br/>" +
                  @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                  @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                  @"<b>Important Notice for Candidates</b><br/>" +
                  @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                  @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";
            public const string reminderEmployer =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-weight:Normal; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>" +
                @"Greetings From Dial4Jobz!!!<br/>" +
                @"You May Have Missed Suitable Candidates, since Your Activation of <b>[SERVICE_NAME]</b> is still pending...<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a> adding 100’s of Fresh Candidates every day…<br/>" +
                @"You Can Start Benefitting from our Services by Completing Your Activation very easily…<br/>" +
                @"You Can Just <a href=[PAYMENT_LINK]>[LINK_NAME]</a> To Complete Your Activation<br/>" +
                @"For Any Clarifications Call – 044 - 44455566<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a> Makes Sourcing Simplified!!!<br/><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";


            public const string SISubscribeForCandidate=
               @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:16px; font-weight:bold; font-family:Calibri; line-height:1.5"">" +
               @"Dear [NAME],<br/> "+
               @"Contact Number: [CONTACT_NUMBER]<br/>"+
               @"Subscribed By: [SUBSCRIBED_BY]<br/>"+
               @"Email:[EMAIL]<br/>"+
               @"Thanks for Showing interest in Spot Interview. Dial4Jobz Adviser will shortly be in touch with you to proceed further..<br/>" +
               @"You can also call 044 - 44455566 (Between 9.30 AM to 6.00 PM ) & demand for the same,Our Adviser will be happy to serve you.<br/>" +
               @"</span>"+
               @"<b>Important Notice for Candidates</b><br/>" +
               @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
             
              @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string SISubscribeToAdmin=
              @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:16px; font-weight:bold; font-family:Calibri; line-height:1.5"">" +
               @"Dear [NAME], " +
               @"This Candidate is interested in SI Please call him & proceed further. The details are given  below:" +
               @"Candidate Name: [CANDIDATE_NAME]<br/>"+
               @"Candidate ID: [ID]"+
               @"Candidate ContactNumber: [CONTACT_NUMBER]"+
               @"E-Mail:[EMAIL]"+
               @"</span>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
               @"<b>Important Notice for Employers</b><br/>" +
               @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
               @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string UpdateVacancy=
               @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:16px; font-weight:Regular; font-family:Calibri; line-height:1.5"">" +
               @"Dear <b>[NAME]</b><br/>" +
               @"We have resubmitted your vacancy <b>[POSITION]</b> for the plan of <b>[PLAN].</b><br/>" +
               @"Your vacancy will be shown on the top of all Searches." +
               @"We look forward in supporting your business with its staffing needs and strongly believe you will find the staff you need at Dial4Jobz.com<br/>"+
               @"<i><u>Details of Your Order:</u><i><br/>" +
               @"Order No: <b>[ORDER_NO]</b><br/>" +
               @"Plan Name: <b>[PLAN_NAME]</b><br/>" +
               @"Alert total Count: <b>[VALIDITY_COUNT]</b><br/>" +
               @"Per Vacancy Count: <b>[VAC_ALERT]</b><br/>" +
               @"Validity Till: <b>[END_DATE]</b><br/>" +
               @"</span>" +
               @"Best Regards,<br/>" +
               @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
               @"For any queries or assistance," +
               @"Please email to Sr.Manager – Client Relation smc@dial4jobz.com or call 044 - 44455566 <br/> " +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
               @"<b>Important Notice for Employers</b><br/>" +
               @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
               @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string ActivateVacancy=
               @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:16px; font-weight:Regular; font-family:Calibri; line-height:1.5"">" +
               @"Dear <b>[NAME]</b><br/>"+
               @"Your <b>[POSITION]</b> vacancy has been activated for the plan of <b>[PLAN].</b><br/>"+
               @"Now you will receive <b>[VALIDITY_COUNT]</b> alerts from the Date of <b>[START_DATE]</b> to <b>[END_DATE]</b><br />" +
               @"For more Information Login and check in <a href=""[DASHBOARD_LINK]"">[LINK_NAME]</a><br/>"+
               @"<i><u>Details of Your Order:</u><i><br/>"+
               @"Order No: <b>[ORDER_NO]</b><br/>"+
               @"Plan Name: <b>[PLAN_NAME]</b><br/>" +
               @"Alert total Count: <b>[VALIDITY_COUNT]</b><br/>" +
               @"Per Vacancy Count: <b>[VAC_ALERT]</b><br/>" +
               @"Validity Till: <b>[END_DATE]</b><br/>" +
               @"</span>"+
               @"Best Regards,<br/>" +
               @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
               @"<b>Important Notice for [NOTICE]</b><br/>" +
               @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
               @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";
             

            public const string SendBankDetails=
                 @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                 @"<span style=""font-size:14px; font-family:Bookman old style; line-height:1.5"">" +
                 @"Dear [NAME]<br/>"+
                 @"Bank Name: ICICI Bank Ltd <br/><br/>"+
                 @"Account Name: Dial4Jobz India Private Ltd<br/>"+
                 @"Branch: Besant Nagar<br/>" +
                 @"Current Account No: 603305017985<br/> " +
                 @"IFSC Code: ICIC0006033 <br/>" +
                 @"Best Regards,<br/>" +
                 @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                 @"044 - 44455566" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";


            //Change password template
            public const string ChangePassword=
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
             @"<span style=""font-size:14px; font-family:Bookman old style; line-height:1.5"">" +
             @"Dear [NAME]<br/>"+
             @"This is to inform you that the password for your account at Dial4jobz.com has been changed successfully. Please ensure that only an authorized person has accessed the account and made the changes.<br/>" +
             @"[RESETPASSWORD]<br/>"+
             @"If you did not authorize this change, please immediately change your Password & notify our customer support or call us to 044 - 44455566 ." +
             @"Best Regards,<br/>" +
             @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
             @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
             @"<b>Important Notice for Employers</b><br/>" +
             @"[IMPORTANT_NOTICE]<br/><br/>" +
             @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
              @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";
            //sms subscription & activation

            public const string SMSSubscription =
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
             @"<span style=""font-size:14px; font-family:Calibri; line-height:1.5"">" +
               @"Dear <b>[CONTACTPERSON]</b>,<br/>" +
               @"Thanks for Subscribing SMS... Your Subscription Details are below.<br/>" +
               @"<b>Name:</b> [NAME]<br/>" +
               @"<b>Id: </b> [ID]<br/>" +
               @"<b>Mobile Number: </b> [MOBILE_NO]<br/>" +
               @"<b>Email Id: </b>[EMAIL_ID]<br/>" +
               @"<b>Plan Subscribed : </b>[PLAN] <br/>" +
               @"<b>Order Number:</b> [ORDER_NO]<br/>" +
               @"<b>Date of Subscription:</b> [DATE]<br/>" +
               @"<b>Amount: </b>Rs.[AMOUNT] <br/><br/>" +
               @"<b>[DISCOUNT_AMOUNT]</b>" +
               @"<b>Subscribed By [SUBSCRIBED_BY]<br/>"+
               @"After receipt of payment & activation <br/>" +
               @"1.You Can send SMS from Dial4jobz<br/>" +
               "2.No promotional Sms are allowed<br/>" +
               "3.As per TRAI policy only approved messages by TRAI can be sent as Transactional sms.<br/>" +
               "4.Only Customers registered with Dial4jobz can avail this facility & only the registered mobile number & email id will be displayed.<br/>" +
               "5.All the SMS will be sent through Dial4jobz.Sender id will be displayed as DLJOBZ.<br/>" +
               "6.Please note you can send SMS from the available templates only.<br/>" +
               "7.This will be delivered to all on High priority & to DND Numbers also.<br/>" +
               "8.Only name of the contact person,name of the position,date of interview, time & Venue for Interview can be typed as per your preference.<br/>" +
               "9.Upto 160 characters for sms will be counted as 1 sms. if exceeds 160 characters it will counted as 2 Sms and so on..<br/>" +
               @"You can pay by Credit/Debit Card/Internet Banking <a href=""[PAYMENT_LINK]"">[LINK_NAME]</a><br/>" +
               @"Or for other Payment option Kindly Visit the site or feel free to call us at 044 - 44455566  ( Between 9.30 AM to 6.00 PM)<br/>" +
               @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>[NOTICE]</b><br/>" +
                @"[IMPORTANT_NOTICE]<br/><br/>"+
               // @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            
            public const string SMSActivated =
            @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
               @"Dear <b>[NAME]</b>,<br/><br/>" +
               @"Thanks for Your Payment. We have received  <b>Rs.[AMOUNT]</b> through [PAYMENT_MODE]<br/><br/>" +
               @"<b>Name</b>: [NAME] <br/><br/>" +
               @"<b>Id</b>: [ID]<br/><br/>" +
               @"<b>Order No</b>: [ORDER_NO]<br/><br/>" +
               @"<b>Invoice No</b>: [INVOICE_NO]<br/><br/>" +
               @"<b>Date of Subscription</b>: [DATE]<br/><br/>" +
               @"<b>Plan Subscribed</b> : [PLAN]<br/><br/>" +
               @"<b>Validity</b>: No.of  [SMS]<br/><br/>" +
               @"<b>Subscribed By:</b>[SUBSCRIBED_BY]<br/>"+
               @"<b>Your Plan is Activated</b><br/><br/>" +
               @"1.You Can send SMS from Dial4jobz<br/><br/>" +
               "2.No promotional Sms are allowed<br/><br/>" +
               "3.As per TRAI policy only approved messages by TRAI can be sent as Transactional sms.<br/>" +
               "4.Only Customers registered with Dial4jobz can avail this facility & only the registered mobile number & email id will be displayed.<br/>" +
               "5.All the SMS will be sent through Dial4jobz.Sender id will be displayed as DLJOBZ.<br/>" +
               "6.Please note you can send SMS from the available templates only.<br/>" +
               "7.This will be delivered to all on High priority & to DND Numbers also.<br/>" +
               "8.Only name of the contact person,name of the position,date of interview, time & Venue for Interview can be typed as per your preference.<br/>" +
               "9.Upto 160 characters for sms will be counted as 1 sms. if exceeds 160 characters it will counted as 2 Sms and so on..<br/><br/>" +
               @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>[NOTICE]</b><br/>" +
                @"[IMPORTANT_NOTICE]<br/><br/>" +
               // @"<b>Important Notice for Employers</b><br/>" +
                //@"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";


            public const string SMSActivatedFree =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px; font-family:Calibri; line-height:1.5"">" +
                @"Dear <b>[CONTACTPERSON]</b>,<br/>" +
                @"Your complimentary plan of <b>[PLAN]</b> is activated.<br/> <br/>" +
                @"<b>Name:</b> [NAME] <br/>" +
                @"<b>EmployerId:</b> [ID]<br/>" +
                @"<b>order No:</b> [ORDER_NO]<br/>" +
                @"<b>Date:</b> [DATE]<br/>" +
                @"<b>Plan Subscribed:</b> [PLAN]<br/>" +
                @"<b>Validity: No.of</b> [SMS]<br/><br/>" +
                @"<b>Subscribed By:</b> [SUBSCRIBED_BY]<br/>"+
                //@"Your Plan Is Activated<br/>" +
                @"1.You Can send SMS from Dial4jobz<br/>" +
                "2.No promotional Sms are allowed<br/>" +
                "3.As per TRAI policy only approved messages by TRAI can be sent as Transactional sms.<br/>" +
                "4.Only Customers registered with Dial4jobz can avail this facility & only the registered mobile number & email id will be displayed.<br/>" +
                "5.All the SMS will be sent through Dial4jobz.Sender id will be displayed as DLJOBZ.<br/>" +
                "6.Please note you can send SMS from the available templates only.<br/>" +
                "7.This will be delivered to all on High priority & to DND Numbers also.<br/>" +
                "8.Only name of the contact person,name of the position,date of interview, time & Venue for Interview can be typed as per your preference.<br/>" +
                "9.Upto 160 characters for sms will be counted as 1 sms. if exceeds 160 characters it will counted as 2 Sms and so on..<br/><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                   @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

           
                       
            public const string RegisterCandiadteVasRegistration =
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
                   "This is to inform you that, a candidate of id[ID] is registerd for VAS plan[PLANID]<br/>" +
                   "Contact Details are,<br/>" +
                   "Name: [NAME]<br/>" +
                   "UserName: [USERNAME]<br/>" +
                   "Email: [EMAIL_ADDRESS]<br/>" +
                   "Mobile Number: [MOBILE_NUMBER]<br/>" +
                @"</span>";
            
            public const string EmployerVASRegistrationToOrganization =
                 @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
                    "This is to inform you that, an employer of id [EMPID] is registerd for VAS plan[PLANID]" +
                    @"Best Regards,<br/>" +
                    @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                    @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                    @"<b>Important Notice for Employers</b><br/>" +
                    @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                    @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                    @"<b>::Disclaimer::</b><br/>" +
                      @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentRegistration=
                 @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">"+
                    "This is to inform you that, You have registered for VAS plan [PLAN] in your Dial4Jobz account"+
                 @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentSubmitOrder=
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">"+
                @"Name: [NAME]"+
                @"Thanks for ur order No.[ORDERNO] plan [PLANNAME].  On receipt of payment plan will be activated within 1 working day"+
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string ReceiptOfPayment=
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">"+
                "Customer Name: [NAME],"+
                "Received Rs.[AMOUNT] for ur<br/>"+
                "Order Number: [ORDERNO]<br/>"+
                "Plan[PLAN].Service will be activated within 1 working day.<br/>"+
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentModeElectronicTransfer=
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">"+
                @"Dear <b>[NAME]</b><br/><br/>"+
                @"Thanks for Submit the Details for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>"+
                @"<i>Details of Vas Subscribed:</i><br/>"+
                @"<b>You have submitted following details on payment page</b><br/>" +
                @"Plan : <b>[PLAN]</b><br/>" +
                @"Order Number:<b> [ORDER_NUMBER]<br/> </b>Order Date :<b> [ORDER_DATE]</b><br/>" +
              
                @"Validity : <b>[VALIDITY] days</b><br/><br/>" +
                @"Resumes : <b>[RESUMES] Resumes</b><br/><br/>" +
             
                @"Transferred Bank Name:<b>[BANKNAME]</b><br/>" +
                @"Subscribed By: <b>[SUBSCRIBED_BY]</b>" +
                 @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentDetails =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:16px; font-family:Calibri;line-height:1.5"">" +
                @"Dear [NAME]<br/>"+
                @"Thanks for Submitting Payment Details for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
                @"<i><b>Details of Payment:</i></b><br/>" +
                @"Order Number: <b> [ORDER_NUMBER]<br/> </b>Order Date :<b> [ORDER_DATE]</b><br/>" +
                @"Validity : <b>[VALIDITY] </b><br/>" +
                @"Plan : <b>[PLAN]</b><br/>" +
                @"Resumes : <b>[VALIDITY_COUNT] </b><br/>" +
                @"Payment  / Transfer  / Collected On: <b>[PAYMENT_DATE][COLLECTED_DATE]</b><br/>"+
                @"Collected By: [COLLECTED_BY]<br/>"+
                @"Amount: <b>[AMOUNT]</b><br/>" +
                @"<b>[BRANCH]</b>   <b>[AREA]</b><br/>" +
                @"[CHEQUEDRAFT] [TRANSFER_REFERENCE][BANK_NAME]<br/>" +
                              
                @"Subscribed By: <b>[SUBSCRIBED_BY]</b><br/>" +
                @"You have requested for <b>[PAYMENT_MODE]</b><br/>" +

                @"<i><b>[USER] Details</b></i><br/>"+
                @"Name: <b>[NAME]</b><br/>" +
                @"Email-Id: <b>[EMAIL_ID]</b><br/>" +
                @"Mobile Number: <b>[CONTACT_NUMBER]</b><br/>" +
                
                @"<b>Our executive will call you for the same within 1 working day.</b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for [USER]</b><br/>" +
                @"[IMPORTANT_NOTICE]<br/>"+
                //@"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            /*Payment Pickup Cash is for Employers*/
            public const string PaymentModePickupCash =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/><br/>" +
                @"Thanks for Subscribing for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
                @"<i><b>Details of Vas Subscribed:</i></b><br/>" +
                @"Employer Name: <b>[EMPLOYER_NAME]</b><br/>"+
                @"Email-Id: <b>[EMAIL_ID]</b><br/>" +
                @"Mobile Number: <b>[MOBILE_NO]</b><br/>" +
                @"Plan : <b>[PLAN]</b><br/>" +
                @"Order Number: <b> [ORDER_NUMBER]<br/> </b>Order Date :<b> [ORDER_DATE]</b><br/>" +
                @"Validity : <b>[VALIDITY] </b><br/>" +
                @"Resumes : <b>[RESUMES] resumes</b><br/>" +
                @"City and Area : <b>[BRANCH] , [AREA]</b><br/>" +
                @"Subscribed By: <b>[SUBSCRIBED_BY]<br/></b>" +
                @"<b>You have requested for [PAYMENT_MODE]</b><br/>" +
                @"<b>Our executive will call you for the same within 1 working day.</b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentModePickupCashCandidate =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
                @"Dear <b>[CANDIDATE_NAME]</b><br/>" +
                @"Thanks for Subscribing for <b>[PLAN]</b>. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
                @"<i>Details of Vas Subscribed:</i><br/><br/>" +
                @"Candidate Name: <b>[CANDIDATE_NAME]</b><br/>" +
                @"Email Id: <b>[EMAIL_ID]</b><br/>" +
                @"Mobile Number: <b>[CONTACT_NUMBER]</b><br/>" +
                @"Plan : <b>[PLAN]</b><br/>" +
                @"Order Number:<b> [ORDER_NUMBER]<br/> </b>Order Date :<b> [ORDER_DATE]</b><br/>" +
                @"Validity : <b>[VALIDITY] </b><br/>" +
                @"Alerts : <b>[ALERTS]</b><br/>" +
                @"City and Area : <b>[BRANCH] , [AREA]</b><br/>" +
                @"Subscribed By: <b>[SUBSCRIBED_BY]<br/></b>"+
                @"<b>You have requested for cash pickup</b><br/>" +
                @"<b>Our executive will call you for the same within 1 working day.</b><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                 @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string PaymentModeElectronicTransferForCandidate =
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:14px; font-family:Bookman old style;line-height:1.5"">" +
               @"Dear <b>[NAME]</b><br/><br/>" +
               @"Thanks for Subscribing for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
               @"<i>Details of Vas Subscribed:</i><br/>" +
               @"<b>You have submitted following details on payment page</b><br/>" +
               @"Plan : <b>[PLAN]</b><br/>" +
               @"Order Number:<b> [ORDER_NUMBER]<br/> </b>Order Date :<b> [ORDER_DATE]</b><br/>" +
                //@"Validity from: <b>[DATE]</b> to <b>[DATE]</b><br/>" +
               @"Validity : <b>[VALIDITY] days</b><br/><br/>" +
               @"Subscribed By: <b>[SUBSCRIBED_BY]</b>" +
            
              // @"Transfer Reference:<b> [TRANSFER_REFERENCE]</b><br/>" +
             //  @"Transferred Bank Name:<b>[BANKNAME]</b><br/>" +
                @"Best Regards,<br/>" +
               @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentModeDepositChequeDraft =
             @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px; font-family:Bookman old style; line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/><br/>" +
                @"Thanks for Subscribing for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
                @"<i>Details of Vas Subscribed:</i><br/>" +
                @"Employer Name: <b>[EMPLOYER_NAME]</b><br/>"+
                @"Plan: <b>[PLAN]</b><br/>"+
                @"Order Number: <b>[ORDER_NUMBER]</b> <br/> </b>Order Date: <b>[ORDER_DATE]</b><br/>"+
                @"Validity : <b>[VALIDITY] days</b><br/>" +
                @"Resumes : Max <b>[RESUMES] Resumes</b><br/><br/>" +
                @"You have submitted following details on payment page<br/>"+
                @"Mode of Payment: <b>[INSTRUMENT_TYPE]</b><br/>"+
                @"<b> [CHEQUE_NUMBER]</b><br/>" +
                @"Amount: <b>[AMOUNT]</b><br/>" +
                @"Bank Branch: <b>[BRANCH]</b><br/>" +
                 @"<b>[DEPOSITED_ON]</b><br/>" +
                @"Bank Name: <b>[BANK_NAME]</b><br/>" +
                @"Subscribed By: <b>[SUBSCRIBED_BY]</b>" +
                 @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PaymentModeDepositForCandidate =
               @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               @"<span style=""font-size:14px; font-family:Bookman old style; line-height:1.5"">" +
               @"Dear <b>[NAME]</b><br/><br/>" +
               @"Thanks for Subscribing for [PLAN]. On receipt of <b>Rs.[AMOUNT].</b> Your Request will be activated...<br/>" +
               @"<i>Details of Vas Subscribed:</i><br/>" +
               @"Plan: <b>[PLAN]</b><br/>" +
               @"Order Number: <b>[ORDER_NUMBER]</b> <br/> </b>Date: <b>[ORDER_DATE]</b><br/>" +
               @"Validity : <b>[VALIDITY] days</b><br/>" +
               @"Alerts : Max <b>[ALERTS] alerts</b><br/><br/>" +
               @"You have submitted following details on payment page<br/>" +
               @"Mode of Payment: <b>[INSTRUMENT_TYPE]</b><br/>" +
               //@"Cheque/DD Number:<b> [CHEQUE_NUMBER]</b><br/>" +
               @"<b> [CHEQUE_NUMBER]</b><br/>" +
               @"Amount: <b>[AMOUNT]</b><br/>" +
               @"Bank Branch: <b>[BRANCH]</b><br/>" +
               //@"Deposited On: <b>[DEPOSITED_ON]</b><br/>" +
                @"<b>[DEPOSITED_ON]</b><br/>" +
               @"Bank Name: <b>[BANK_NAME]</b><br/>" +
               @"Subscribed By: <b>[SUBSCRIBED_BY]</b><br/>"+
               @"Best Regards,<br/>" +
               @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
               @"<b>Important Notice for Candidates</b><br/>" +
               @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
               @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
               @"<b>::Disclaimer::</b><br/><br/> " +
               @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            public const string AdminCandidateRegister=
                @"<span style=""font-size:14px;line-height:1.5"">" +
                @"Dear <b>[NAME]</b><br/>"+
                @"Greetings from <a href=""www.dial4jobz.com"">dial4jobz.com</a>!!!<br/>" +
                @"Welcome to Dial4jobz,"+
                @"Reference to your Resume with us we have updated the same in Dial4jobz.com.<br/><br/>"+
                @"<b>Your UserId: [USERNAME]</b><br/>"+
                @"<b>Your Password: [PASSWORD]</b><br/><br/>"+
                @"Please click here to login [LINK], update your resume & also upload your word Resume to attract the Employers!!!<br/>" +
                @"You can also Call Dial4jobz 044 - 44455566  (9.30am to 7.00Pm) and update, Our advisors will do the needful…<br/>" +
                @"<li>  Search jobz in Dial4Jobz-Free!!!</li>" +
                @"<li>	Communicate with Employer in Dial4Jobz- Free!!!</li>" +
                @"<li>	Search for Any level, Any Role from Any Industry</li>" +
                @"<li>	Display Your contact in your resume..</li>" +
                @"India’s 1st New age Interactive Job Portal!!!" +
                @"We have upgraded our portal with many user friendly features<br/>" +
                @"For any clarification & assistance email us at smo@dial4jobz.com or call Dial4jobz 044 - 44455566  (9.30am to 7.00Pm),Our advisors will assist you for the same." +
                @"Feel free to call us at 044 - 44455566  we will be happy to serve you.." +
                @"We wish you to get the desired Job you Dream….<br/>" +
                @"Team<br/>" +
                @"<b>Dial4Jobz</b><br/>" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

            
            public const string AdminJobPosting =
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>Greetings from Dial4Jobz !!!!</font> </span></font></p>" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 6.75pt;'>" +
               "<font face='Tahoma'>Viewing your advertisement in one of the media for vacancy, we have posted the same in Dial4jobz.com…</font> </span><br />" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 6.75pt;'>" +
               "<font face='Tahoma'>Its free & you can submit any more requirements you have its free….</font> </span><br /><br />" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 6.75pt;'>" +
               "<font face='Tahoma'>You can also Call Dial4jobz 044 - 44455566 (9.30am to 7.00Pm) and give your requirements  over phone & our advisors will do the needful…</font> </span><br /><br />" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 8.75pt;'><font face='Tahoma'>•	Search candidates in our portal for your requirement Free!!!</font></span><br />" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 8.75pt;'><font face='Tahoma'>•	Communicate with candidates from our portal Free!!!</font></span><br />" +
               "<span style='font-family: Verdana; font-size: 11pt; margin-left: 8.75pt;'><font face='Tahoma'>•	Search for Any level, Any Role from any Industry</font></span><br />" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>India’s 1st New age Job Portal!!!</font></span></font></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>We have upgraded our portal with many user friendly features ….</font></span></font></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>For any reason you don’t want this vacancy to be advertised in dial4jobz.com..email us at smc@dial4jobz.com or  call Dial4jobz 044 - 44455566  (9.30am to 7.00Pm),Our advisors will assist you for the same.</font></span></font></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>Feel free to call us at 044 - 44455566 for any clarification & we will be happy to serve you..</font></span></font></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Verdana; font-size: 11pt'><font>We wish you get you get the desired candidate for your requirement….</font></span></font></p>";


            public const string MatchingJobHeader =
               "<p style='margin-left: 6.75pt;'><span style='font-family: Andalus; font-size: 11pt'><b><font>Dear </font>" +
               "<font size='3' face='Times New Roman'>[NAME]</font> , </b></span></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Andalus; font-size: 11pt'><font>Greetings</font>" +
               "<font>from</font> <font>Dial4Jobz, India’s</font> <font>1 <sup>st</sup> new-age interactive job portal. </font></span></font></p>" +
               "<p style='margin-left: 6.75pt;'><span style='font-family: Andalus; font-size: 11pt'><font face='Tahoma'><span style='font-family: Andalus; font-size: 11pt'>" +
               "<font face='Andalus'>Reference to your Resume submitted find below suitable Vacancy.The said employer has received your resume.They may call you & if you find this vacancy interesting call the employer to fix the interview.</font> </span></font></span></p><br />";

            public const string MatchingJobFooter =
                "<p style='margin-left: 6.75pt;'>You may contact the company directly for more information.</p>" +
               "<p style='margin-left: 6.75pt;'>Please call us at <b>044 - 44455566 </b> for any clarifications. For more jobs visit <b>www.dial4jobz.com </b></p>" +
               "<p style='margin-left: 6.75pt;'>Best wishes<br /><b>Dial4Jobz</b></p>" +
               "<p style='margin-left: 6.75pt;'><font>Disclaimer:</font></p>" +
               "<p style='margin-left: 6.75pt;'><font>You have received this mail because your e-mail ID is registered with Dial4jobz.com. " +
               "This is a system-generated email, please don't reply to this message. The jobs sent in this mail have been posted by the clients of Dial4jobz.com.  " +
               "Dial4Jobz IPL has taken all reasonable steps to ensure that the information in this mailer is authentic. Users are advised to research bonafides of advertisers independently. " +
               "Dial4Jobz IPL shall not have any responsibility in this  regard. We recommend that you visit our Terms & Conditions and the Security Advice for more comprehensive  information.</font></p>";

            

            public const string MatchingCandidateHeader =
               "<p style='margin-left: 6.75pt;'><span style='font-family: Andalus; font-size: 11pt'><b><font>Dear </font>" +
               "<font size='3' face='Times New Roman'>[ORG_NAME]</font> , </b></span></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Andalus; font-size: 11pt'><font>Greetings</font>" +
               "<font>from</font> <font>Dial4Jobz, India’s</font> <font>1 <sup>st</sup> new-age interactive job portal. </font></span></font></p>" +
               "<p style='margin-left: 6.75pt;'><span style='font-family: Andalus; font-size: 11pt'><font face='Andalus'><span style='font-family: Andalus; font-size: 11pt'>" +
               "<font face='Andalus'>Reference to your job - ' <font size='3' face='Times New Roman'> [JOBNAME] '</font> </font></span></font></span>, " +
               "<font face='Andalus'>the details of the candidate matching your requirement is given below with candidate's resume attachment if available.</font></p><br />"+
                "<font face='Calibri' font-weight='bold' font-size='16px'>[SPOT_TEXT] </font></p><br /><br/>" ;

            public const string MatchingCandidateFooter =
                "<p style='margin-left: 6.75pt;'>You can contact the candidate directly and take this forward.</p>" +
                "<p style='margin-left: 6.75pt;'>Please call us at 044 - 44455566 </b> for <font color='black'>any</font> clarifications.For more jobs visit <a target='_blank' href='http://www.dial4jobz.com'>Dial4jobz.com</a></p>" +
                "<p style='margin-left: 6.75pt;'>Regards<br /><b>Dial4Jobz</b></p>" +
                "<p style='margin-left: 6.75pt;'><b><font face='Calibri'>Important Notice for Employers </font></b></p>" +
                "<p style='margin-left: 6.75pt;'><b><span><font face='Calibri'>The Information on candidates Shared or sent to you is" +
                "as communicated or furnished by the them over telephone/Internet and it shall be the sole responsibility of the Employer before appointing them to" +
                "check, authenticate and verify the information/response received . Dial4jobz is not responsible for false information given by the candidate. </font></span></b></p>" +
                "<p style='margin-left: 6.75pt;'><font>Disclaimer:</font></p>" +
                "<p style='margin-left: 6.75pt;'><font>This electronic message and any files attached with it are intended for the recipient(s)" +
                "and may contain confidential and privileged information. If you are not the intended recipient, please notify the sender immediately and destroy all copies of this message" +
                "and any attachments. Any unauthorized usage of this email is strictly prohibited.</font></p>";

            public const string MatchingCandidateMain =
                "<table width='82%' cellspacing='0' cellpadding='0' border='1' style='border-bottom: medium none; border-left: medium none; width: 82.6%; border-collapse: collapse; margin-left: 6.75pt;" +
                "border-top: medium none; margin-right: 6.75pt; border-right: medium none'>" +
                "<tbody><tr><td width='100%' bgcolor='#E0EBED' style='border-bottom: grey 4.5pt double; border-left: grey 4.5pt double;" +
                "padding-bottom: 0in; padding-left: 0in; width: 100%; padding-right: 0in; border-top: grey 4.5pt double;" +
                "border-right: grey 4.5pt double; padding-top: 0in'>" +
               
                "<p><b><span style='color: #727171'>Candidate Name : </span></b>[CANDIDATENAME]</p>" +
                "<p><b><font color='#727171'>Mobile Number : </font></b>[MOBILE]</p>"+
                "<p><b><font color='#727171'>Additional Number:</font></b>[LANDLINE]</p>" +
                "<p><b><font color='#727171'>Email : </font> </b>[EMAIL] </p> " +
                "<p><b><font color='#727171'>Address : </font> </b>[ADDRESS]</p>" +
                "<p><b><font color='#727171'>Age : </font> </b>[DOB] Years</p>" +
                "<p><b><span style='color: #727171'>Basic Qualification : </span> </b> [BASICQUALIFICATION]</p>" +
                "<p><b><span style='color: #727171'>Post Graduation : </span> </b> [POSTGRADUATION]</p>" +
                "<p><b><span style='color: #727171'>Doctorate : </span></b> [DOCTRATE] </p>" +
                "<p><b><span style='color: #727171'>Experience : </span> </b> [EXPERIENCE]</p>" +
                "<p><b><span style='color: #727171'>Industry : </span> </b> [INDUSTRY] </p>" +
                "<p><b><span style='color: #727171'>Function : </span> </b> [FUNCTION] <b></p>" +
                "<p><span style='color: #727171'>Skills : </span> </b> [SKILLS] </p>" +
                "<p><b><span style='color: #727171'>Current Salary/Annum : </span> </b> [ANNUAL_SALARY]</p>" +
                "<p><b><span style='color: #727171'>Current Location : </span> </b> [LOCATION]</p>" +
                "<p><b><span style='color: #727171'>Present Company : </span> </b> [PRESENT_COMPANY]</p>" +
                "<p><b><span style='color: #727171'>Previous Company : </span></b> [PREVIOUS_COMPANY]</p>" +
                "<p><b><span style='color: #727171'>Languages Known : </span> </b> [LANGUAGE]</p>" +
                "<p><b><font color='#727171'>Preferences : </font> </b> [PREFERENCES]</p>" +
                "<p><b><font color='#727171'>Preferred Type : </font> </b> [PREFERRED_TYPE]</p>" +
                "</td></tr></tbody></table><br />";

            public const string MatchingJobMain =
                //"<table width='82%' cellspacing='0' cellpadding='0' border='1' style='border-bottom: medium none; border-left: medium none; width: 82.6%; border-collapse: collapse; margin-left: 6.75pt;" +
                "<table style='border:1px solid #cccccc' width='600' cellpadding='0' cellspacing='0'>" +
                //"border-top: medium none; margin-right: 6.75pt; border-right: medium none'>" +
                "<tbody>" +
                 "<tr>" +
                "<td style='padding-top:2px;padding-bottom:4px;padding-left:5px;border-bottom: 1px #43AC5C dotted';>" +
                "<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br /><p>Matching Vacancy Details for Your Profile</p></td></tr>" +
                "<tr> <td  bgcolor='#E0EBED' style='padding-top:2px;padding-bottom:4px;padding-left:5px;border-bottom: 1px #43AC5C dotted';>" +
                "<font face='Calibri' font-weight='bold' font-size='14pt'>[SPOT_TEXT] </font></p><br />" +
                "<font face='Calibri' font-weight='bold' font-size='14pt'>We do not ask you to pay any fees to any consultant or employer for selection</font></p><br/>" +
                "<p ><b><span style='color: #0E0D0D'>Description : </span></b>[DESCRIPTION]</p>" +
                "<p ><b><font color='#0E0D0D'>Position : </font></b>[POSITION] </p>" +
                "<p ><b><font color='#0E0D0D'>Company Name : </font> </b>[COMPANY_NAME] </p> " +
                "<p ><b><font color='#0E0D0D'>Function Name : </font> </b>[FUNCTION]</p>" +
                "<p><b><span style='color: #0E0D0D'>Experience : </span> </b> [MIN_EXPERIENCE] to [MAX_EXPERIENCE]</p>" +
                "<p><b><span style='color: #0E0D0D'>Location of Country : </span> </b> [POSTING_COUNTRY]</p>" +
                "<p><b><span style='color: #0E0D0D'>Location of State : </span> </b> [POSTING_STATE]</p>" +
                "<p><b><span style='color: #0E0D0D'>Location of City : </span> </b> [POSTING_CITY]</p>" +
                "<p><b><span style='color: #0E0D0D'>Location of Area : </span> </b> [POSTING_AREA]</p>" +
                "<p><b><span style='color: #0E0D0D'>Basic Qualification : </span></b> [BASICQUALIFICATION] </p>" +
                "<p><b><span style='color: #0E0D0D'>Post Graduation : </span> </b> [POSTGRADUATION]</p>" +
                "<p><b><span style='color: #0E0D0D'>Doctorate : </span> </b> [DOCTRATE] </p>" +
                "<p><b><span style='color: #0E0D0D'>Skills Required : </span> </b> [SKILLS] <b></p>" +
                "<p><b><span style='color: #0E0D0D'>License Types Required: [LICENSE_TYPE]</span></b></p>" +
                "<p><b><span style='color: #0E0D0D'>Contact Person : </span> </b> [CONTACT_PERSON]</p>" +
                "<p><b><span style='color: #0E0D0D'>Mobile Number : </span> </b> [MOBILE]</p>" +
                "<p><b><span style='color: #0E0D0D'>Landline Number : </span> </b> [LANDLINE]</p>" +
                "<p><b><span style='color: #0E0D0D'>Email : </span></b> [EMAIL]</p>" +
                "<p><b><span style='color: #0E0D0D'>Website : </span> </b> [WEBSITE]</p>" +
                "<p><b><span style='color: #0E0D0D'>We do not ask you to pay any fees to any consultant or employer for selection</span></p>" +
                "</td></tr></tbody></table><br />";

            public const string MatchingJobForRAJ =
               "<p style='margin-left: 6.75pt;'><span style='font-family: Andalus; font-size: 11pt'><b><font>Dear </font>" +
               "<font size='3' face='Andalus'>[NAME]</font> , </b></span></p>" +
               "<p  style='margin-left: 6.75pt;'><font face='Symbol'><span style='font-family: Andalus; font-size: 11pt'><font>Greetings </font>" +
               "<font>from</font> <font>Dial4Jobz, India’s</font> <font>1 <sup>st</sup> new-age interactive job portal. </font></span></font></p>" +
               "<p style='margin-left: 6.75pt;'><span style='font-family: Verdana; font-size: 11pt'><font face='Andalus'><span style='font-family: Andalus; font-size: 11pt'>" +
               "<font face='Andalus'>As per your Job Alert Plan the following vacancy details is suitable for you.The  employer has received your resume.They may call you or if you find this vacancy interesting call the employer to fix the interview.</font> </span></font></span></p><br />" +
                //"<table width='82%' cellspacing='0' cellpadding='0' border='1' style='border-bottom: medium none; border-left: medium none; width: 82.6%; border-collapse: collapse; margin-left: 6.75pt;" +
                "<table style='border:1px solid #cccccc' width='600' cellpadding='0' cellspacing='0'>" +
                //"border-top: medium none; margin-right: 6.75pt; border-right: medium none'>" +
                "<tbody>" +
                "<tr>" +
                "<td style='padding-top:2px;padding-bottom:4px;padding-left:5px;border-bottom: 1px #43AC5C dotted';>" +
                "<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br /><p>Matching Vacancy Details for Your Profile</p></td></tr>" +
                "<tr> <td  bgcolor='#E0EBED' style='padding-top:2px;padding-bottom:4px;padding-left:5px;border-bottom: 1px #43AC5C dotted';>" +
                "<p ><b><span style='color: #727171'>Description : </span></b>[DESCRIPTION]</p>" +
                "<p ><b><font color='#727171'>Position : </font></b>[POSITION] </p>" +
                "<p ><b><font color='#727171'>Company Name : </font> </b>[COMPANY_NAME] </p> " +
                "<p ><b><font color='#727171'>Function Name : </font> </b>[FUNCTION]</p>" +

                 "<p><b><span style='color:#727171'>Experience : </span> </b> [MIN_EXPERIENCE] to [MAX_EXPERIENCE]</p>" +
                "<p><b><span style='color: #727171'>Location of Country : </span> </b> [POSTING_COUNTRY]</p>" +
                "<p><b><span style='color: #727171'>Location of State : </span> </b> [POSTING_STATE]</p>" +
                "<p><b><span style='color: #727171'>Location of City : </span> </b> [POSTING_CITY]</p>" +
                "<p><b><span style='color: #727171'>Location of Area : </span> </b> [POSTING_AREA]</p>" +
                "<p><b><span style='color: #727171'>Basic Qualification : </span></b> [BASICQUALIFICATION] </p>" +
                "<p><b><span style='color: #727171'>Post Graduation : </span> </b> [POSTGRADUATION]</p>" +
                "<p><b><span style='color: #727171'>Doctorate : </span> </b> [DOCTRATE] </p>" +
                "<p><b><span style='color: #727171'>Skills Required : </span> </b> [SKILLS] <b></p>" +
                "<p><b><span style='color: #727171'>Contact Person : </span> </b> [CONTACT_PERSON]</p>" +
                "<p><b><span style='color: #727171'>Mobile Number : </span> </b> [MOBILE]</p>" +
                "<p><b><span style='color: #727171'>Landline Number : </span> </b> [LANDLINE]</p>" +
                "<p><b><span style='color: #727171'>Email : </span></b> [EMAIL]</p>" +
                "<p><b><span style='color: #727171'>Website : </span> </b> [WEBSITE]</p>" +
                "</td></tr></tbody></table><br />"+
                "<p style='margin-left: 6.75pt;'>You may contact the company directly for more information.</p>" +
               "<p style='margin-left: 6.75pt;'>Please call us at <b>044 - 44455566</b> for any clarifications. For more jobs visit <b>www.dial4jobz.com </b></p>" +
               "<p style='margin-left: 6.75pt;'>Best wishes<br /><b>Dial4Jobz</b></p>" +
               "<p style='margin-left: 6.75pt;'><font>Disclaimer:</font></p>" +
               "<p style='margin-left: 6.75pt;'><font>You have received this mail because your e-mail ID is registered with Dial4jobz.com. " +
               "This is a system-generated email, please don't reply to this message. The jobs sent in this mail have been posted by the clients of Dial4jobz.com.  " +
               "Dial4Jobz IPL has taken all reasonable steps to ensure that the information in this mailer is authentic. Users are advised to research bonafides of advertisers independently. " +
               "Dial4Jobz IPL shall not have any responsibility in this  regard. We recommend that you visit our Terms & Conditions and the Security Advice for more comprehensive  information.</font></p>";


            public const string MatchingCandidate =
                "<p style='margin-left: 6.75pt;'><span style='font-family: Calibri; font-size:16px; color:#727171; line-height:19px'><b><font>Dear </font>" +
                "<font size='3' face='Calibri'>[ORG_NAME]</font> , </b></span></p>" +
                "<p  style='margin-left: 6.75pt;'><font face='Bookman Oldstyle'><span style='font-family: Calibri; font-size: 11pt'><font>Greetings </font>" +
                "<font>from</font> <font>Dial4Jobz, India’s</font> <font>1 <sup>st</sup> new-age interactive job portal. </font></span></font></p>" +
                "<p style='margin-left: 6.75pt;'><span style='font-family: Calibri; font-size: 11pt'><font face='Calibri'><span style='font-family: Calibri; font-size: 14px'>" +
                "<font face='Calibri' font-size='16px'>[TEXT_VACANCY]</font></span></font></span>,<br/> " +
                //"<font face='Calibri' font-size='16px'>Reference to your Vacancy submitted for ' [JOBNAME] ' </font></span></font></span>, " +
                //"<font face='Calibri' font-size='16px'> & resume alert assigned ,find the details of the candidate matching your requirement .The vacancy alert is sent to this candidate also. </font></p><br /><br/>" +
                "<font face='Calibri' font-weight='bold' font-size='16px'>[SPOT_TEXT] </font></p><br /><br/>" +
                "<table style='border:1px solid #cccccc' width='600' cellpadding='0' cellspacing='0'>" +
                "<tbody>" +
                "<tr>" +
                "<td style='padding-top:2px;padding-bottom:4px;padding-left:5px;border-bottom: 1px #43AC5C dotted';>" +
                "<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br /><p><b>[TEXT_MATCH]</b><br/><font face='Monsieur La Doulaise' font-size='35'><b><i>[CANDIDATENAME]</i></font></p></td></tr>" +
                "<tr>" +
                //"<td bgcolor='#eeeeee' style='padding-top:2px;padding-bottom:3px;padding-left:5px;border-bottom:1px #cccccc dotted' 727171>" +
                "<td colspan='4' bgcolor='#E0EBED' style='padding-left:11px;padding-bottom:10px;border-bottom:1px dotted #cccccc'>" +
                "<p><b><span style='color: '#0E0D0D'>Candidate Name : </span></b>[CANDIDATENAME]</p>" +
                "<p><b><font color='#0E0D0D'>Mobile Number : </font></b>[MOBILE]</p>"+
                "<p><b><font color='#0E0D0D'>Additional Number:</font></b>[LANDLINE]</p>" +
                "<p><b><font color='#0E0D0D'>Email : </font> </b>[EMAIL] </p> " +
                "<p><b><font color='#0E0D0D'>Address : </font> </b>[ADDRESS]</p>" +
                "<p><b><font color='#0E0D0D'>Age : </font> </b>[DOB]</p>" +
                "<p><b><span style='color: #0E0D0D'>Basic Qualification : </span> </b> [BASICQUALIFICATION]</p>" +
                "<p><b><span style='color: #0E0D0D'>Post Graduation : </span> </b> [POSTGRADUATION]</p>" +
                "<p><b><span style='color: #0E0D0D'>Doctorate : </span></b> [DOCTRATE] </p>" +
                "<p><b><span style='color: #0E0D0D'>Experience Required : </span> </b> [EXPERIENCE]</p>" +
                //"<p><b><span style='color: #727171'>Experience : </span> </b> [MIN_EXPERIENCE] to [MAX_EXPERIENCE]</p>" +
                "<p><b><span style='color: #0E0D0D'>Industry : </span> </b> [INDUSTRY] </p>" +
                "<p><b><span style='color: #0E0D0D'>Function : </span> </b> [FUNCTION] </p>  <b><span style='color: #0E0D0D'>Skills : </span> </b> [SKILLS] </p>" +
                "<p><b><span style='color: #0E0D0D'>Current Salary/Annum : </span> </b> [ANNUAL_SALARY]</p>" +
                "<p><b><span style='color: #0E0D0D'>Current Location : </span> </b> [LOCATION]</p>" +
                "<p><b><span style='color: #0E0D0D'>Present Company : </span> </b> [PRESENT_COMPANY]</p>" +
                "<p><b><span style='color: #0E0D0D'>Previous Company : </span></b> [PREVIOUS_COMPANY]</p>" +
                "<p><b><span style='color: #0E0D0D'>Languages Known : </span> </b> [LANGUAGE]</p>" +
                "<p><b><font color='#0E0D0D'>Preferences : </font> </b> [PREFERENCES]</p>" +
                "<p><b><font color='#0E0D0D'>Preferred Type : </font> </b> [PREFERRED_TYPE]</p>" +
                "<p><b><font color='#0E0D0D'>License Type: </font></b>[LICENSE_TYPES]</p>"+
                "<p><b><font color='#0E0D0D'>Download Resume: </font> </b>[DOWNLOAD_RESUME]</p" +
                "</td></tr></tbody></table>" +
                 "<p style='margin-left: 6.75pt;'>You can contact the candidate directly and take this forward.</p>" +
                "<p style='margin-left: 6.75pt;'>Please call us at 044 - 44455566  </b> for <font color='black'>any</font> clarifications.For more jobs visit <a target='_blank' href='http://www.dial4jobz.com'>Dial4jobz.com</a></p>" +
                "<p style='margin-left: 6.75pt;'>Regards<br /><b>Dial4Jobz</b></p>" +
                
                "<p style='margin-left: 6.75pt;'><b><font face='Calibri'>Important Notice for Employers </font></b></p>" +
                "<p style='margin-left: 6.75pt;'><b><span><font face='Calibri'>The Information on candidates Shared or sent to you is" +
                "as communicated or furnished by the them over telephone/Internet and it shall be the sole responsibility of the Employer before appointing them to" +
                "check, authenticate and verify the information/response received . Dial4jobz is not responsible for false information given by the candidate. </font></span></b></p>" +
             
                 @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                "<p style='margin-left: 6.75pt;'><font>Disclaimer:</font></p>" +
                "<p style='margin-left: 6.75pt;'><font>This electronic message and any files attached with it are intended for the recipient(s)" +
                "and may contain confidential and privileged information. If you are not the intended recipient, please notify the sender immediately and destroy all copies of this message" +
                "and any attachments. Any unauthorized usage of this email is strictly prohibited.</font></p>";
               

            //public const string ResponseForResume =
            //     "<table style=border:1px solid #cccccc cellpadding=0 cellspacing=0 width=600> " +
            //     "<tbody>" +
            //     "<tr> " +
            //     "<td style=border: 1px solid #f1f1f1> " +
            //     "<span style=font-size: 14px></span>" +
            //     "<table>" +
            //     "<tbody>" +
            //     "<tr><td bgcolor=#FFFFFF valign=top><font color=#000000 face=Arial, Helvetica, sans-serif><div style=margin: 0px; padding: 3px 0px; width= 600';'=><img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' height=75px width=200px><br></div></font></td></tr>" +
            //     "<tr>" +
            //     "<td style=font-size: 11px; border-bottom: 1px solid #f1f1f1 bgcolor=#FFFFFF> " +
            //     "<div style=margin: 0px; padding: 3px 0px><strong>Employer:</strong> [EMPLOYERNAME] </div> " +
            //     "<div style=margin: 0px; padding: 3px 0px><strong>Contact Person:</strong> [NAME] </div>  " +
            //     "<div style=margin: 0px; padding: 3px 0px><strong>Mobile :</strong> [MOBILE_NO] </div> " +
            //     "<div style=margin: 0px; padding: 3px 0px><strong>Email :</strong> [EMAIL]</div><br/> " +
            //     "<div style=margin: 0px; padding: 3px 0px><strong>[MESSAGE]</strong></div> " +
            //     "<p>Your Profile is suitable for our Vacancy. Contact us for more information or forward your updated CV to the same Email-Id.</p> " +
            //     "<p style=clear: both; font-size: 12px><br><strong>Dear [CANDIDATENAME],</strong></p> " +
            //     "<font color=#313131 face=Arial, Helvetica, sans-serif>You are receiving this e-mail because your profile was viewed by [EMPLOYERNAME]</font> " +
            //     "</td></tr>" +
            //     "<tr>" +
            //     "<td bgcolor=#FFFFFF valign=top><font color=#313131 face=Arial, Helvetica, sans-serif> " +
            //     "<div style=border-bottom: 1px solid #888888; line-height: 16px; font-size: 11px;font-weight: bold>Disclaimer:</div> " +
            //     "<div style=padding-top: 5px; font-size: 11px>The sender of this email is registered with <a target=_blank href='http://www.dial4jobz.com'>Dial4jobz.com</a> as [EMPLOYERNAME] as registered " +
            //     "([EMPLOYERMAILID]) using Dial4jobz services. The responsibility of checking the authenticity of offers/correspondence lies with you. <br><br> " +
            //     "If you consider the content of this email inappropriate or spam, you may Report abuse by forwarding this email to: <a target=_blank href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a><br><br> " +
            //     "Advisory: Please do not pay any money to anyone who promises to find you a job. This could be in the form of a registration fee, or document processing fee or visa charges or any other pretext. The money could be asked for" +
            //     "upfront or it could be asked after trust has been built after some correspondence has been exchanged. Also please note that in case you get a job offer or a letter of intent without having been through an interview process it is probably a scam " +
            //     "and you should contact <a target=_blank href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a> for advise.<br><br></div>	</font> " +
            //     "</td></tr></tbody></table></td></tr></tbody></table>";

            public const string ResponseForResume =
                 "<table style=border:1px solid #cccccc cellpadding=0 cellspacing=0 width=600> " +
                 "<tbody>" +
                 "<tr> " +
                 "<td style=border: 1px solid #f1f1f1> " +
                 "<span style=font-size: 14px></span>" +
                 "<table>" +
                 "<tbody>" +
                 "<tr><td bgcolor=#FFFFFF valign=top><font color=#000000 face=Arial, Helvetica, sans-serif><div style=margin: 0px; padding: 3px 0px; width= 600';'=><img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' height=75px width=200px><br></div></font></td></tr>" +
                 "<tr>" +
                 "<td style=font-size: 11px; border-bottom: 1px solid #f1f1f1 bgcolor=#FFFFFF> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Employer:</strong> [EMPLOYERNAME] </div> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Contact Person:</strong> [NAME] </div>  " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Mobile :</strong> [MOBILE_NO] </div> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Email :</strong> [EMAIL]</div><br/> " +
                 //"<div style=margin: 0px; padding: 3px 0px><strong>[MESSAGE]</strong></div> " +
                 "<p>Your vacancy is sent to the following Candidate. The Candidate Details are below</p> " +
                 "<p style=clear: both; font-size: 12px><br><strong>Dear [EMPLOYERNAME],</strong></p> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Candidate Name:</strong> [CANDIDATENAME] </div> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Mobile :</strong> [CAND_MOBILE_NO] </div> " +
                 "<div style=margin: 0px; padding: 3px 0px><strong>Email :</strong> [CAND_EMAIL]</div><br/> " +
                 "<font color=#313131 face=Arial, Helvetica, sans-serif>You are receiving this e-mail because your vacancy was sent to [CANDIDATENAME]</font> " +
                 "</td></tr>" +
                 "<tr>" +
                 "<td bgcolor=#FFFFFF valign=top><font color=#313131 face=Arial, Helvetica, sans-serif> " +
                 "<div style=border-bottom: 1px solid #888888; line-height: 16px; font-size: 11px;font-weight: bold>Disclaimer:</div> " +
                 "<div style=padding-top: 5px; font-size: 11px>The sender of this email is registered with <a target=_blank href='http://www.dial4jobz.com'>Dial4jobz.com</a> as [EMPLOYERNAME] as registered " +
                 "([EMPLOYERMAILID]) using Dial4jobz services. The responsibility of checking the authenticity of offers/correspondence lies with you. <br><br> " +

                 "If you consider the content of this email inappropriate or spam, you may Report abuse by forwarding this email to: <a target=_blank href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a><br><br> " +
                 "Advisory: Please do not pay any money to anyone who promises to find you a job. This could be in the form of a registration fee, or document processing fee or visa charges or any other pretext. The money could be asked for" +
                 "upfront or it could be asked after trust has been built after some correspondence has been exchanged. Also please note that in case you get a job offer or a letter of intent without having been through an interview process it is probably a scam " +
                 "and you should contact <a target=_blank href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a> for advise.<br><br></div>	</font> " +
                 "</td></tr></tbody></table></td></tr></tbody></table>";
           
                      
            public const string ContactCandidate =
               "<table width='100%' cellspacing='1' cellpadding='5' border='0' align='center'>" +
               "<tbody><tr><td></td></tr><tr>" +
               "<td style='border: 1px solid #f1f1f1'>" +
               "<table><tbody><tr><td valign='top' bgcolor='#FFFFFF'>" +
               "<font face='Arial, Helvetica, sans-serif' color='#000000'>" +
               "<span style='font-size: 14px'><div style='margin: 0px; padding: 3px 0px'>" +
               "<font color='#888888' face='Arial, Helvetica, sans-serif'>The sender of this email is registered with Dial4Jobz.com as [EMPLOYERNAME] To respond back directly to the Employer send an email to [EMAIL]</font><br/>" +
                "<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Experience required for the Job:</strong> [EXPERIENCE] years</div>" +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Annual Salary of the Job:</strong> [SALARY] Lacs  [OTHERSALARYDETAILS]</div>" +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Job Location:</strong> [JOBLOCATION]</div></span>" +
               "<p style='clear: both; font-size: 12px'><br /><strong>Dear [CANDIDATENAME],</strong></p>" +
               "<blockquote style='margin: 0px; padding: 10px 0px; font-size: 12px'>" +
               "[MESSAGE]</blockquote><div style='clear: both'>" +

               "<strong>You can reach</strong></div>" +
               "<strong>Employer:</strong> [EMPLOYERNAME] </div>" +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Contact Person:</strong> [NAME] </div> " +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Mobile :</strong> [MOBILE_NO] </div>" +
               "<div style='margin: 0px; padding: 3px 0px'><strong>Email :</strong> [EMAIL]</div></div></font></td></tr><tr>" +

               "<td bgcolor='#FFFFFF' align='center' style='font-size: 11px; border-bottom: 1px solid #f1f1f1'>" +
               "<font face='Arial, Helvetica, sans-serif' color='#313131'>You are receiving this e-mail because your profile was viewed by [EMPLOYERNAME]</font>" +
               "</td></tr><tr><td valign='top' bgcolor='#FFFFFF'><font face='Arial, Helvetica, sans-serif' color='#313131'>" +
               "<div style='border-bottom: 1px solid #888888; line-height: 16px; font-size: 11px;font-weight: bold'>Disclaimer:</div>" +
               "<div style='padding-top: 5px; font-size: 11px'>The sender of this email is registered with <a target='_blank' href='http://www.dial4jobz.com'>Dial4jobz.com</a> as [EMPLOYERNAME] as registered" +
               "([EMPLOYERMAILID]) using Dial4jobz services. The responsibility of checking the authenticity of offers/correspondence lies with you. <br /><br/>" +
               "If you consider the content of this email inappropriate or spam, you may Report abuse by forwarding this email to: <a target='_blank' href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a><br /><br />" +
               "Advisory: Please do not pay any money to anyone who promises to find you a job. This could be in the form of a registration fee, or document processing fee or visa charges or any other pretext. The money could be asked for " +
               "upfront or it could be asked after trust has been built after some correspondence has been exchanged. Also please note that in case you get a job offer or a letter of intent without having been through an interview process it is probably a scam " +
               "and you should contact <a target='_blank' href='mailto:report@dial4jobz.com'>report@dial4jobz.com</a> for advise.<br /><br /></div></font>" +
               "</td></tr></tbody></table></td></tr></tbody></table>";

            public const string CandidateMatch =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                //@"<img src=""~/Content/Images/dial4jobz_logo.png""/>"+
                @"Dear [NAME],<br/><br/>" +
                @"Greetings from <b>Dial4Jobz</b>, India’s 1st new-age interactive job portal." +
                @"Reference to your Job" +
                @"<b>[POSITION]</b>," +
                @" the details of the candidate matching your requirement is given below.<br/><br/> " +
                @"Candidate Name: [CANDIDATE_NAME]<br/><br/>" +
                @"Mobile Number: [MOBILE_NUMBER]<br/><br/>" +
                @"Email: [EMAIL]<br/><br/>" +
                @"Address: [ADDRESS]<br/><br/>" +
                @"Qualification: [QUALIFICATION]<br/><br/>" +
                @"Total Experience: [TOTAL_EXPERIENCE]<br/><br/>" +
                @"Salary: [ANNUAL_SALARY]<br/><br/>" +
                @"Location: [LOCATION]<br/><br/>" +
                @"Present Company: [PRESENT_COMPANY]<br/><br/>" +
                @"Previous Company: [PREVIOUS_COMPANY]<br/><br/>" +
                @"Languages Known: [LANGUAGE]<br/><br/>" +
                @"PREFERRED TYPE: [PREFERRED_TYPE]<br/><br/>" +
                @"You may contact the candidate directly and take this forward.<br/><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.Dial4Jobz.com""></a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:11px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string JobMatch =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                //@"<img src=""[IMAGE_URL]"" alt=""Logo;width:12px;height:12px"" <br/>" +
                @"Dear [NAME]..<br/><br/>" +
                @"Greetings from Dial4 Jobz, India’s 1st new-age interactive job portal <a href=""www.dial4jobz.com""> www.Dial4Jobz.com</a>.  Reference to your call, the list of vacancies suiting your resume is given below.<br/>" +
                @"Name: [ORG_NAME]<br/>" +
                @"Position: [POSITION]<br/>" +
                @"Industry Type: [INDUSTRY_TYPE]<br/>" +
                @"Experience: [EXPERIENCE]<br/>" +
                @"Location of Posting: [LOCATION]<br/>" +
                @"Qualification: [BASIC_QUALIFICATION]<br/>" +
                @"Skills: [SKILLS]<br/>" +
                @"Contact Person: [CONTACT_PERSON]<br/>" +
                @"Mobile Number: [MOBILE_NUMBER]<br/>" +
                @"Email: [EMAIL]<br/>" +
                @"Website: [WEBSITE]<br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.Dial4Jobz.com""></a><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";
                      

            public const string ClientRegister =
                //
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-family:Calibri;font-size:14px;line-height:1.5"">" +
                @"Dear <b>[NAME]</b>,<br/><br/>" +
                @"Welcome to <b>Dial4Jobz</b><br/> " +
                @"<b>India's 1st new age interactive Job Portal.</b><br/>" +
                @"Your Employer account at <a href=""www.dial4jobz.com""> www.Dial4Jobz.com</a>  has been created. Thanks for Choosing  Dial4jobz.com.<br/>" +
                @"Advertising your Vacancy in www.Dial4Jobz.com  is Free and unlimited<br/>" +
                @"<b>Please click this link to :</b> <a href='[LINK]'>[LINK_NAME]</a>.<br/><br/>" +//Link Name should verify your e-mail ID
                @"Kindly login with details of your account mentioned below.<br/>" +
                @"<b>Your Username : [USER_NAME]</b><br/>" +
                @"<b>Your Password : [PASSWORD]<br/><br/></b>" +
                @"Keep this information confidential.<br/>" +
                @"Our e-mail validation is intended to verify the ownership of it. Thanks for your registration. We look forward in supporting your business with its staffing needs and strongly believe you will find the staff you need at www.Dial4Jobz.com<br/>" +
                @"<b><u>Value Added Services:</u></b><br/>" +
                @"<b>Resume Alert (RAT)</b><br/>" +
                @"Get Resume Alert of any suitable candidates as soon as its submitted (RAT)<br/>" +
                @"<b>Hot Resumes (HORS)</b><br/>" +
                @"View Contact details of all suitable candidates to reach them immediately.Mail to suitable candidates...SMS to all suitable candidates to call for interview or to find their interest to join you.<br/>" +
                @"<b>Spot Selection (SS)</b><br/>" +
                @"Get connected through D4J with suitable candidates for Spot selection..(Spot Selection). Call 044 - 44455566   or<a href=""mailto:smc@dial4jobz.com""> smc@dial4jobz.com</a  to avail Value Added Services.<br/>" +
                @"For any queries or assistance,<br/>" +
                @"Please email to<b> Sr.Manager- Client Relation </b><a href=""mailto:smc@dial4jobz.com""> smc@dial4jobz.com</a><br/><br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                  @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string ClientPost =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                @"Dear [NAME]...<br/><br/>" +
                @"Thanks for posting the job</b><br/> " +
                @"<b>Job posting is free and unlimited.</b><br/><br/>" +
                @"We look forward in supporting your business with its staffing needs and strongly believe you will find the staff you need at <a href=""www.dial4jobz.com"">Dial4Jobz.Com</a></br><br/>" +
                @"For any queries or assistance,<br/><br/>" +
                @"Please email to Sr.Manager – Client Relation <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a> or call 044 - 44455566  <br/>" +
                @"<b>Our Value Added Services for this Position</b><br/><br/>" +
                @"<b>Employers:</b><br/><br/>" +

                @"<b>1.Hot Resumes: (Search Resumes)</b><br/>" +
                @" • We have candidates matching your requirement. You can view all the resumes.Based on the plan you subscribe for,You will be in position access 25 or more  contact details of the shortlisted candidates for communicating with them.<br/>" +
                @"<b>All these comes to you at an affordable cost</b><br/><br/>" +

                @"<b>2.Featured Employer:</b><br/>" +
                @"•	If you subscribe as a Featured Employer, Your Vacancy will be sent on priority to all the suitable candidates who call us. You will also get the details of the candidates to whom we have forwarded your vacancies for you to shortlist & proceed further.<br/>" +
                @"•	Period of Validity for Featured Employer will be 1 Month or 25 resumes whichever is earlier.<br/>" +
                @"•	Period of validity for free job listings will be 1 month.<br/><br/>" +

                @"<b>3.Spot Selection:</b><br/>" +
                @"•	We can supplement your recruitment process such as sourcing, filtering & short listing, interviewing & organize Teleconference by our professional Recruiters and submit the final list of candidates to you for selection. This service can be done for any position.<br/><br/>" +

                //@"<b>4. Top employers: </b><br/>" +
                //@"•	A banner advt. can be placed in <a href=""www.Dial4Jobz.com""></a> home page and the link to your website will be given.<br/>" +
                //@"Contact us pricing or mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a><br/><br/>" +

                //@"<b>5. Advertise in emails:</b><br/>" +
                //@"•	We can insert your banner advertisement in the header & footer of every mail which we send to candidate on vacancy & on registration. For Employer we send mail on Registration & resumes of suitable candidates.<br/>" +
                //@"•	Contact us pricing or mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>. (You have option of choosing either category-Employer & Candidate.)<br/><br/>" +

                @"<b>6. Reference Checks: </b><br/>" +
                @"•	For a candidates selected by you for recruitment we can conduct a reference check with their previous employers or/ and references provided by them.Contact us pricing or mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a><br/>" +
                @"Once again, we thank you for choosing Dial4Jobz.We look forward in supporting your business with its staffing needs.<br/><br/>" +
                @"Best Regards, <br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers</b><br/><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            private const string ImportantNoticeForEmployers =
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Employers<b><br/><br/>" +
                @"The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.<br/><br/>" +
                @"</span>";


            public const string CandidateRegister =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                //@"<span style=""FONT-FAMILY:'face=Verdana, Arial, Helvetica, sans-serif';font-size:14px;line-height:1.5"">" +
                @"<span style=""font-family:Calibri;font-size:14px;line-height:1.5"">" +
                @"Dear [NAME]...<br/>" +
                @"Welcome to <b>Dial4Jobz</b><br/>" +
                @"<b>India 's 1st new age interactive Job Portal</b><br/><br/>" +
                @"Thank you for submitting your resume.<br/> " +
                @"You have provided following email-id to dial4jobz.com account [EMAIL].<br/>"+
                @"Please click this to <a href='[LINK]'>[LINK_NAME]</a><br/> " +
                @"Our e-mail validation is intended to verify the ownership of it. Thanks for using Dial4jobz.com.<br/> "+
                @"The details of your account are given below:<br/>" +
                @"<b>Username: [USER_NAME]</b><br/>" +
                @"<b>Password: [PASSWORD]</b><br/>" +
                @"<b><u>Value Added Services:</u></b><br/>"+
                @"<b>Job Alert</b><br/>"+
                @"Get job alert by SMS/Mail as soon as Vacancies are submitted by Any employer.<br/>" +
                @"<b>Spot Interview: </b><br/>" +
                @"Get spot Interview thorough Dial4jobz for suitable Vacancies from any employer.<br/>" +
                @"<b>Display Resume: </b><br/>" +
                @"All employers can view your Contact details & your resume will on the Top of the page on any relevant search. Call 044 - 44455566  or mail to smc@dial4jobz.com  to avail Value Added Services<br/>" +
                @"For any queries or assistance, Please email to Sr.Manager-Operation smo@dial4jobz.com" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";
                      

            public const string CandidateUpdateProfile =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                @"Your details has been updated.<br/>" +
                @"<b>To get the most out of Dial4Jobz: </b><br/><br/> " +
                @"• Remember to log in/Dial 044 - 44455566  regularly, say every week, to check out your matching jobs and apply to the jobs that interests you. The more active you are, the more likely you are to get spotted by the right employer.<br/><br/>" +
                @"<b>Keep your Profile & Resume Updated</b><br/><br/>" +
                @"• Subscribe for Resume Display your Contact details in your resume will be displayed for 120 days and the clients can call you directly.<br/><br/>" +
                @"• <b>Know the Employer: </b>Whenever you receive a call for an interview and if you want to know the details about the company, place of posting <b>“remember”</b> to call us @ 044 - 44455566 <br/><br/>" +
                @"<b>• Resume Services:</b> Dial4Jobz’s resume writing <b>Specialists</b> will design your resume that gets you noticed by the employers. <a href=""www.dial4jobz.com"">Click here to know more now</a>." +
                @"Log in to your account for latest jobs that match your profile.<br/>" +
                @"<a href=""www.dial4jobz.com""> Click Here</a> to go to Dial4Jobz now!<br/><br/>" +
                @"For any queries or assistance,<br/><br/>" +
                @"Please email to Sr.Manager Operation <a href=""mailto:smo@dial4jobz.com"">smo@dial4jobz.com</a><br/>" +
                @"We look forward to help you build a great career! <br/>" +
                @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>Important Notice for Candidates</b><br/>" +
                @"The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.<br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                 @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

  
            public const string SendMailCandidateApplyJob =
                 @"<span style=""font-size:14px;line-height:1.5"">" +
                    @"Dear [NAME]" +
                    @"You have applied for the following jobs" +
                    @"[POSITION_LINK]" +
                    @"[LOGO]" +
                    @"smo@dial4jobz.com" +
                    @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:10px"">" +
                    @"<b>::Disclaimer::</b><br/>" +
                    @"You have received this mail because your e-mail ID is registered with Dial4jobz.com. This is a system-generated email, please don't reply to this message. The jobs sent in this mail have been posted by the clients of Dial4jobz.com.  Dial4Jobz IPL has taken all reasonable steps to ensure that the information in this mailer is authentic. Users are advised to research bonafides of advertisers independently. Dial4Jobz IPL shall not have any responsibility in this  regard. We recommend that you visit our <a href=""[LINK]"">Terms & Conditions</a> and the <a href=""[LINK]"">Security Advice</a>
                     for more comprehensive  information."+
                    @"</span>";

            public const string EmailVerification =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""FONT-FAMILY:'face=Verdana, Arial, Helvetica, sans-serif';font-size:14px;line-height:1.5"">" +
                @"You have provided following email-id <b>[EMAIL]</b> to www.dial4jobz.com  account ."+
                @"Please click on the link given below to validate your e-mail address. (If the link is disabled and non clickable then please copy and paste the link text in the address bar of your browser and press enter.)" +
                @"Our e-mail validation is intended to verify the ownership of it. Thanks for using Dial4jobz.com." +
                @"Please click  link and verify your e-mailID:<br/> " +
                @"<a href='[LINK]'>[LINK_NAME]</a><br/>" +
                 @"Best Regards,<br/>" +
                @"<a href=""www.dial4jobz.com"">Dial4Jobz</a><br/><br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                   @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";
            public const string PasswordReset =
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                @"Dear <b>[NAME]</b>,<br/><br/>" +
                @"You have requested to reset your password, please find the new password following<br/>" +
                @"Username: <b>[USERNAME]</b><br/>"+
                @"Password has been reset.<br/>"+
                @"new password: <b>[PASSWORD]</b><br/>" +
                @"We suggest login the new password and change your password as per your convenience.<br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:black;FONT-SIZE:13px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/home/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/home/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""http://www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smc@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can<a href=""http://www.dial4jobz.com"">delete this account</a>. 
                (Login details are required to delete the account)" +
               @"</span>";

            public const string PasswordResetforCandidate=
                @"<img src='http://www.dial4jobz.in/Content/Images/logo_with_number.jpg' width='200px' height='75px'/><br/>" +
                @"<span style=""font-size:14px;line-height:1.5"">" +
                @"Dear <b>[NAME]</b>,<br/><br/>" +
                @"Your Username: <b>[USERNAME]</b><br/>" +
                @"Your password has been reset.<br/>" +
                @"Your new password: <b>[PASSWORD]</b><br/>" +
                @"We suggest login the new password and change your password as per your convenience.<br/>" +
                @"<span style=""FONT-FAMILY:'Arial','sans-serif';COLOR:gray;FONT-SIZE:13px"">" +
                @"<b>::Disclaimer::</b><br/>" +
                @"By your positive acts of registering on <a href=""www.dial4jobz.com"">dial4jobz.com</a>, you agree to be bound by the <a href=""http://www.dial4jobz.in/terms"">terms and conditions</a> & <a href=""http://www.dial4jobz.in/privacy"">privacy policy </a> and if you do not agree with any of the provisions of these policies, you should not access or use <a href=""www.dial4jobz.com"">
                Dial4jobz.com</a>.If you did not create this account and wish to delete it please send an e-mail to <a href=""mailto:smo@dial4jobz.com"">smc@dial4jobz.com</a>.<br/> In case you do not wish to be registered with <a href=""www.dial4jobz.com"">dial4jobz.com</a> any longer you can <a href='LINK'>delete this account</a>. 
                (Login details are required to delete the account)" +
                @"</span>";

           
        }

        public struct DocumentsDetailsForBC
        {
            public const string Employment =
                "<div>Need the following mandatory information along with supporting documents ( Servicecertificate/ Relieving letter).</div><div><br /></div>" +
                "<div>Name of the Employer:</div><div><br /></div>" +
                "<div>Employee id&nbsp;</div><div><br /></div>" +
                "<div>Designation&nbsp;</div><div><br /></div>" +
                "<div>Exact period of employment</div><div><br /></div>" +
                "<div>Last drawn salary.</div><div><br /></div>";

            public const string Education =
                "<div>Need the following mandatory information along with supporting documents ( Degree&nbsp;certificate/Provisional /Consolidated mark sheet- wherever applicable)&nbsp;</div><div><br /></div>" +
                "<div>Name of the Degree &amp; Discipline&nbsp;</div><div><br /></div>" +
                "<div>Name of the college &amp; University&nbsp;</div><div><br /></div>" +
                "<div>Registration number</div><div><br /></div>" +
                "<div>Study period: From (mm/yyyy) to( mm/yyyy)&nbsp;</div><div><br/></div>" +
                "<div>Year of passing&nbsp;</div><div><br /></div>" +
                "<div>Class obtained</div><div><br /></div>";

            public const string AddressCriminalcheck =
                "<div>• Complete Address of the candidate( Includes Door no, ( Flat name/Floor if Applicable)House Name, Street Name, Nearest Post office, Village Name, Taluk Name, District, State,Pincode) &nbsp;</div><div><br /></div>" +
                "<div>• Nearest Prominent landmark &nbsp;</div><div><br /></div>" +
                "<div>• Mobile Number,landline number if any & email Id&nbsp;</div><div><br /></div>" +
                "<div>• Father’s name of the candidate &nbsp;</div><div><br /></div>" +
                "<div>• Date of Birth</div><div><br /></div>";

            public const string Reference =
                "<div>His/her Resume with full contact details.</div><div><br/></div>" +
                "<div>Referee details( professional reference only If given by candidate)&nbsp;</div><div><br/></div>" +
                "<div>Name of the referee &amp; contact details along with his/her designation.</div><div><br/></div>" +
                "<div>Names of previous employer:</div><div><br/></div>" +
                "<div>Duration of employment:</div><div><br/></div>" +
                "<div>Designation:</div><div><br/></div>" +
                "<div>Place of Posting:</div><div><br/></div>";

        }

        public struct SmsBody
        {

            public const string UpdateProfileAlert =
               "Dear [NAME]\n" +
                "U Asked For Below Modification\n" +
                "Old Number:[MOBILE_NUMBER]\n" +
                "New Number [NEW_MOBILE]\n" +
                "Verification Code: [VERIFY_CODE]\n" +
                "If Not Asked By u\n" +
                "Call Manikandan @ 044 - 44455566\n" +
                "www.dial4jobz.com";
               //"Changed By: [CHANGED_BY]\n" +
         
            public const string EmailUpdateProfileAlert =
                "Dear [NAME]\n" +
                "U Asked For Below Modification\n" +
                "Old Email:[OLD_EMAIL]\n" +
                "New Email [NEW_EMAIL]\n" +
                "If Not Asked By U\n" +
                "Call Manikandan @ 044 - 44455566\n" +
                "www.dial4jobz.com";

            public const string CandidateRemainder =
            "[Name]\n" +
            "Greetings from Dial4Jobz\n" +
            "You Have Missed\n" +
            "Exciting Vacancies since\n" +
            "Your service Activation Pending,\n" + 
            "Pay Now To Activate\n" +
            "044 - 44455566";

            public const string EmployerRemainder =
                "[Name]\n" +
                "Greetings from Dial4Jobz\n" +
                "You May Have Missed\n" +
                "Suitable candidates since\n" +
                "Your service Activation is Pending,\n" +
                "Pay Now To Activate\n" +
                "044 - 44455566";
            
            /*SMS Templates*/

            public const string SIText =
                "[NAME]\n"+
                "Jobz For u!\n" +
                "V can organise Spot Interview immediately with Employer.\n"+
                "V have Matching vacancies For U\n"+
                "Call 044 - 44455566 \n " +
                "Dial4jobz";
            
             public const string JobAlertText =
                 "[NAME]\n"+
                 "Jobz For u!\n" +
                "100's of vacancy added daily in dial4jobz.com.  Get Fresh Matching Jobs on SMS to get Ur job faster.call 044 - 44455566  Dial4jobz";

             public const string DPRText =
                  "[NAME]\n" +
                  "Jobz For u!\n" +
                 "Many employers searching for U in Dial4jobz.com. Display ur Resume on Top With Contact Details.. call 044 - 44455566  Dial4jobz";

            /*End Sms Templates for Plans*/

            public const string SISubscribe =
                "Alert from www.Dial4jobz.com \n" +
                "Dear [NAME]\n" +
                "Thanks for Showing interest in Spot Interview. Dial4Jobz Adviser will shortly be in touch with you to proceed further.\n" +
                "You can also call 044 - 44455566 (Between 9.30 AM to 6.00 PM ) & demand for the same,Our Adviser will be happy to serve you.";

            public const string SubscribePlan =
                "[NAME] thanks for subscribing [DESCRIPTION] [PLAN]\n" +
                "On receipt of [AMOUNT] [DISCOUNT_AMOUNT] your plan will be active.[DICOUNT_TEXT]" +
                "Dial4Jobz.com" +
                "044 - 44455566";

            public const string ActivateVacancy =
                "Alert from www.Dial4jobz.com \n" +
                "Dear [NAME], Your [POSITION] vacancy has been activated for the plan of [PLAN]. Now you will receive [VALIDITY_COUNT] alerts till [VALIDITY_TILL] days.";

            public const string SMSAccountDetails =
                "Alert from www.Dial4jobz.com\n" +
                "Dear [NAME],ICICI Bank,Acct Name: Dial4Jobz India Private Ltd,Branch: Besant Nagar, Current A/C No: 603305017985, IFSC Code:ICIC0006033,Dial4Jobz 044 - 44455566 ";

            public const string SMSInterestCheck =
                 "Dear [CANDIDATENAME] Greetings From  www.dial4jobz.com!!!\n" +
                 "Will you be interested for a vacancy in [SMSLOCATION] For [SMSVACANCY] with [SMSCOMPANYNAME]." +
                 "Contact:\n" +
                 "[SMSCONTACTPERSON]\n" +
                 "[SMSMOBILENO]\n" +
                 "[SMSEMAILID]\n";

            public const string ShareLandlineSMS =
                "Alert from www.Dial4jobz.com\n" +
                "Your No:[MOBILENUMBER]\n"+
                "Save Dial4Jobz No 044 - 44455566  to ur contact list.for Job or Candidates Just call this no.";

            public const string ShareMobileSMS =
                "Alert from www.Dial4jobz.com\n" +
                "Your No:[MOBILENUMBER]\n" +
                "Save Dial4Jobz No 044 - 44455566 to ur contact list.for Job or Candidates just call this no.";
                        

            public const string SMSVacancy =
               "Alert from www.Dial4jobz.com,Vacancy for ur CV\n" +
               "[ORG_NAME],\n" +
               "Contact: [CONTACT_PERSON],\n" +
               "[EMAIL],\n"+
               "[MOBILE_NUMBER],\n" +
               "[POSITION],\n" +
                "Qualification: [BASIC_QUALIFICATION] " +
               "Exp: [EXP]\n" +
               "in  [LOCATION]\n"+
               "Required [GENDER].";

           

            public const string CandidateDetails =
                "Dial4Jobz Verification of CV Details Given By You!!!\n" +
                "Name: [NAME]\n" +
                "Mobile: [MOBILE_NUMBER]\n" +
                "Email: [EMAIL]" +
                "Education: [QUALIFICATION]\n" +
                "Designation: [DESIGNATION]\n" +
                "Function: [FUNCTION]\n" +
                "Role: [ROLE]\n" +
                "Industry: [INDUSTRY]\n" +
                "Salary: [ANNUAL_SALARY] Per Annum\n" +
                "DOB: [DOB]\n" +
                "Exp: [YEARS]  [MONTHS] \n" +
                "Gender: [GENDER]\n" +
                "Current Loc: [COUNTRY], [CITY]\n" +
                "Preferred Location: [PREF_COUNTRY],[PREF_CITY]\n" +
                "Preferred Function: [PREF_FUNC]" +
                "For Any Changes / Clarifications:\n" +
                "044 - 44455566 \n" +
                "www.dial4jobz.com";

            public const string JobPostingDetails =
                "Ur Vacancy submitted as below\n" +
                "Vacancy: [POSITION]\n"+
                "Qualification: [BASIC_QUALIFICATION]\n"+
                "Department: [FUNCTIONAL_AREA]\n"+
                "Role: [ROLE]\n"+
                "Pref Industry: [PREFERRED_INDUSTRY]\n"+
                "Job Location: [COUNTRY][STATE][CITY][AREA]\n"+
                "Gender: [GENDER]\n"+
                "Exp: [MINEXP][MAXEXP]\n"+
                "Salary Budget: [ANNUAL_SALARY]\n" +
                "Languages: [PREFERRED_LANGUAGES]\n"+
                "Type: [PREFERRED_TYPE]\n"+
                "For Any Changes / Clarifications:\n"+
               "044 - 44455566 \n" +
                "www.dial4jobz.com\n";

            public const string VerificationEmployerPersonalData =
                "Dial4Jobz Verification of Details Given By You!!!\n" +
                "Company Name: [NAME]\n" +
                "Contact Person: [CONTACT_PERSON]\n" +
                "Number: [MOBILE_NO]\n" +
                "Email: [EMAIL]\n" +
                "Industry: [INDUSTRY]\n" +
                "Location: [LOCATION]\n" +
                "For Any Changes / Clarifications: \n" +
              "044 - 44455566 \n" +
                "www.dial4jobz.com";
                

            public const string SMSVacancyForSS=
               "Alert from www.Dial4jobz.com for Interview\n" +
               "[ORG_NAME],\n" +
               "Contact: [CONTACT_PERSON],\n" +
               "[EMAIL],\n" +
               "[MOBILE_NUMBER],\n" +
               "[POSITION],\n" +
               "Qualification: [BASIC_QUALIFICATION] " +
               "Exp: [EXP]\n" +
               "in  [LOCATION]\n" +
               "Required [GENDER].";

            public const string SMSVacancyDirect =
              "Alert from www.Dial4jobz.com,CV for ur [POSITION]\n" +
              "[ORG_NAME],\n" +
              "Contact: [CONTACT_PERSON],\n" +
              "[EMAIL],\n" +
              "[MOBILE_NUMBER],\n" +
              //"[ADDRESS]"+
              "for the position of [POSITION],\n" +
                //"Qualification: [BASIC_QUALIFICATION], " +
                //"Exp: [EXP],\n" +
              "in  [LOCATION]\n";
              //"Required [GENDER].";

            public const string SMSResume =
                "Alert from www.Dial4jobz.com,CV for ur [VACANCY]\n" +
                "Candidate Details:\n"+
                "[NAME],\n " +
                "[EMAIL],\n " +
                "[MOBILE_NUMBER],\n " +
                "[QUALIFICATION],\n " +
                "[FUNCTION],\n " +
                "Position: [DESIGNATION],\n " +
                "[PRESENT_SALARY],\n" +
                "[DOB],\n" +
                "Exp: [TOTAL_EXPERIENCE],\n " +
                "[GENDER]," +
                "loc: [LOCATION]\n";

            public const string SMSResumeForSS=
                "Alert from www.Dial4jobz.com for Spot selection\n" +
                "[NAME],\n " +
                "[EMAIL],\n " +
                "[MOBILE_NUMBER],\n " +
                "[QUALIFICATION],\n " +
                "[FUNCTION],\n " +
                "Position: [DESIGNATION],\n " +
                "[PRESENT_SALARY] per annum,\n" +
                "[DOB] years old,\n" +
                "Exp: [TOTAL_EXPERIENCE] yrs,\n " +
                "[GENDER]," +
                "loc: [LOCATION]\n";
            

            public const string SMSPostResume =
             
                "Alert from www.Dial4jobz.com\n" +
                "Dr [NAME]\n" +
                "Email: [EMAIL]\n" +
                "Mobile Number: [MOBILE_NUMBER]\n" +
                //"Qualfication: [QUALIFICATION]\n" +
                "Function: [FUNCTION]\n" +
                "Position: [DESIGNATION]\n" +
                "Present Salary: [PRESENT_SALARY]\n" +
                //"Location: [LOCATION]\n" +
                "DOB: [DOB]\n" +
                "Experience: [TOTAL_EXPERIENCE]\n" +
                "Gender: [GENDER]\n";

           

            public const string ResetPassword =
                "Alert from www.Dial4jobz.com\n" +
                "Dear [NAME]\n" +
                "UserName: [USERNAME]\n" +
                "Password: [PASSWORD]\n";

                       
            public const string AdminSMSPostVacancy =
                "Alert from www.Dial4jobz.com, Dial4jobz has advertised your vacancy in Dial4jobz.com Its free.call 044 - 44455566  or visit www.Dial4jobz.com";

            public const string SMSClientRegister =
                 "Alert from www.Dial4jobz.com\n" +
                 "Tks  for reg.\n" +
                 "UserId:[USER_NAME]\n" +
                 "Pass:[PASSWORD]\n";
               // "Please update your other details..";

            public const string SMSCandidateRegister =
               "Alert from www.Dial4jobz.com\n" +
                "Tks  for reg.\n" +
                "UserId:[USER_NAME]\n" +
                "Pass:[PASSWORD]\n" +
                "Ver.Code:[CODE]\n";

            public const string SMSNewCandidate =
                "Alert from www.Dial4jobz.com\n" +
                "Ref ur Cv with us we have updated ur cv in Dial4jobz.\n"+
                "U can view and update or call us 044 - 44455566 \n" +
                "Username: [USER_NAME]\n" +
                "Password: [PASSWORD]\n";

            public const string SMSMobileVerification =
               "Alert from www.Dial4jobz.com\n" +
                "Dear [NAME],\n" +
                "Greetings from Dial4Jobz.com!\n" +
                "Your PIN Number is:[PIN_NUMBER]\n" +
                "Type/tell this PIN to verify your Mobile Number.";

            public const string SMSCandidateShortlistByEmployer =
                "Alert from www.Dial4jobz.com, [CANDIDATENAME] MSG thru Dial4jobz -044 - 44455566  - Ref ur cv u r shortlisted for Interview on [InterviewDate] at [InterviewLocation], " +
                "for : [InterviewCompany], \n" +
                "meet : [InterviewContactPerson],\n " +
                "[InterviewContactNo],\n " +
                "Position : [InterviewPosition],\n " +
                "Salary : [InterviewJobSalary], \n" +
                "location : [InterviewJobLocation], \n" +
                "Address : [InterviewJobAddress] Pls confirm ur presence";

            public const string SMSPayThroughMobile =
                "Alert from www.Dial4jobz.com,Dear Customer\n" +
                "You can now make payment through a call to us from your mobile or landline if you have a credit card." +
                "Before calling us kindly get “OTP” from your credit card issuing bank." +
                "On getting the same, along with OTP, keep your 16 digit credit card number & CVV number ready and Call us at 044 - 44455566 ";
            
            //not need
            public const string SMSActivated=
                "Alert from www.Dial4jobz.com, [NAME]" +
                "Thanks Received Rs[AMOUNT] Your request for plan[PLAN] is now activated"+
                "044 - 44455566 ";

            public const string SMSSubscribingVas =
              "Alert from www.Dial4jobz.com\n" +
               @"The Information on candidates Shared/ sent/displayed to you is as communicated or furnished by the Candidate over phone/Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/response received . Dial4Jobz IPL is not responsible for false information given by the candidate.";
                       

            public const string SMSReceiptofPayment =
                "Alert from www.Dial4jobz.com\n" +
                "[NAME],Received Rs.[AMOUNT] for ur orderNo.[ORDER_NO],Plan.[PLANNAME].Thanks and your service is activated now.";
        }
    }
}