using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Helpers;
using System.IO;
using System.ComponentModel.DataAnnotations;
using CCA.Util;
using System.Collections.Specialized;
using System.Configuration;
using Dial4Jobz.Areas.Admin.Controllers;


namespace Dial4Jobz.Controllers
{
    public class CandidatesVasController : BaseController
    {
        //avenue access code and encrypted request
        public string strAccessCode = "AVZH01BE45AF56HZFA";// put the access key in the quotes provided here.
        public string strEncRequest = "";

        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        ActivationReportController activationReportController = new ActivationReportController();
        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
        public int maxLength = int.MaxValue;
        public string[] AllowedFileExtensions;
        public string[] AllowedContentTypes;
        List<string> _filters = new List<string>();

        [Authorize]
        public ActionResult Index(string value)
        {
            return View();
        }

        public ActionResult CandidatesPayment(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    value = Constants.DecryptString(value);
                    int OrderId;
                    bool ValueIsAnId = int.TryParse(value, out OrderId);
                    var planDetails = _vasRepository.GetOrderDetail(OrderId);

                    if (ValueIsAnId)
                    {
                        _vasRepository.ActivateVAS(OrderId);
                        //SendActivationMail(OrderId);
                        activationReportController.SendActivationMail(OrderId);
                        ViewData["Success"] = "Thanks, We have received your Payment.Your  " + planDetails.PlanName + "  is acivated.";
                    }
                }
                catch
                {
                    ViewData["Failure"] = "We regret your payment was not successfull.Kindly Contact our Adviser 044 - 44455566  for assistance";
                    return View();
                }
            }
            else
            {
                ViewData["Failure"] = "Activation is not Successfull. Check your Order exists or Not.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Subscribed(string Plan, string VasType, string Amount, string DiscountAmount)
        {
            VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(Plan);
            OrderMaster ordermaster = new OrderMaster();
            OrderDetail orderdetail = new OrderDetail();
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            var registeredBy = string.Empty;      
            User subscribedByAdmin =null;
            User registeredByAdmin = null;

            if (LoggedInCandidate != null)
            {
                ordermaster.CandidateId = LoggedInCandidate.Id;
                ordermaster.SubscribedBy = LoggedInCandidate.Name;
            }

            else if (Session["LoginAs"] == "CandidateViaAdmin")
            {
                int candidateId = (int)Session["CandId"];
                Candidate candidate = _repository.GetCandidate(candidateId);
                ordermaster.CandidateId = candidateId;
            }

            else
            {
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Time Expired. Try again",
                    ReturnUrl = "/Admin/AdminHome/AddCandidate"
                });
            }

            if (Session["LoginUser"] == "UserViaAdmin")
            {
                var subscribedby = Session["LoginUserId"].ToString();
                ordermaster.SubscribedBy = subscribedby;
            }

            else if(user != null)
            {
                ordermaster.SubscribedBy = user.UserName;
            }

            if (Plan == "SI")
            {
                /*In this Plan Mail will go for intimate the user select SI. After update from the admin only mail will go.*/

                ordermaster.OrderDate = Constants.CurrentTime();
                //ordermaster.CandidateId = LoggedInCandidate.Id;
                ordermaster.Amount = vasplan.Amount;
                ordermaster.PaymentStatus = false;
                _vasRepository.AddOrderMaster(ordermaster);
                _vasRepository.Save();

                System.Web.HttpContext.Current.Session["VasOrderNo"] = ordermaster.OrderId;

                //OrderDetail orderdetail = new OrderDetail();
                orderdetail.OrderId = ordermaster.OrderId;
                orderdetail.PlanId = vasplan.PlanId;
                if (Plan == "CRD")
                {
                    orderdetail.PlanName = "CANDPRRAJ";
                }
                else
                {
                    orderdetail.PlanName = vasplan.PlanName;
                }

                if (DiscountAmount != null)
                {
                    orderdetail.DiscountAmount = Convert.ToInt32(DiscountAmount);
                }
                else
                {
                    orderdetail.Amount = vasplan.Amount;
                }
                _vasRepository.AddOrderDetail(orderdetail);
                _vasRepository.Save();

                /*To candidate SI Details*/
                if (ordermaster.Candidate.Email != null || ordermaster.Candidate.Email!="")
                {
                    EmailHelper.SendEmail
                        (
                        Constants.EmailSender.CandidateSupport,
                        ordermaster.Candidate.Email,
                        "SI Details",
                        Constants.EmailBody.SISubscribeForCandidate
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[CONTACT_NUMBER]",ordermaster.Candidate.ContactNumber)
                        .Replace("[EMAIL]",ordermaster.Candidate.Email)
                        .Replace("[SUBSCRIBED_BY]",ordermaster.SubscribedBy)
                        );
                }

                EmailHelper.SendEmailBCC
                        (
                        Constants.EmailSender.CandidateSupport,
                        Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.CandidateSupport,
                        "SI Details",
                        Constants.EmailBody.SISubscribeForCandidate
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[CONTACT_NUMBER]", ordermaster.Candidate.ContactNumber)
                        .Replace("[EMAIL]", ordermaster.Candidate.Email)
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        );

                if (user != null)
                {
                    EmailHelper.SendEmail
                       (
                       Constants.EmailSender.CandidateSupport,
                        user.Email,
                       "SI Details",
                       Constants.EmailBody.SISubscribeForCandidate
                       .Replace("[NAME]", ordermaster.Candidate.Name)
                       .Replace("[CONTACT_NUMBER]", ordermaster.Candidate.ContactNumber)
                        .Replace("[EMAIL]", ordermaster.Candidate.Email)
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                       );
                }

                /*To Employer SI Details*/
                if (ordermaster.Candidate.Email != null)
                {
                    EmailHelper.SendEmail
                       (
                       ordermaster.Candidate.Email,
                       Constants.EmailSender.EmployerSupport,
                       "SI Response",
                       Constants.EmailBody.SISubscribeToAdmin
                       .Replace("[NAME]", ordermaster.Organization.Name)
                       .Replace("[CANDIDATE_NAME]", ordermaster.Candidate.Name)
                       .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                       .Replace("[CONTACT_NUMBER]", ordermaster.Candidate.ContactNumber)
                       .Replace("[EMAIL]", (ordermaster.Candidate.Email!=null ? ordermaster.Candidate.Email: "Not Available"))
                       );

                }
                /*Send SMS for SI Details*/

                SmsHelper.SendSecondarySms(
                    Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.SISubscribe
                    .Replace("[NAME]", ordermaster.Candidate.Name),
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    ordermaster.Candidate.Email);
                    
            }
            else
            {

                ordermaster.OrderDate = Constants.CurrentTime();
                //ordermaster.CandidateId = LoggedInCandidate.Id;
                ordermaster.Amount = vasplan.Amount;
                ordermaster.PaymentStatus = false;
                _vasRepository.AddOrderMaster(ordermaster);
                _vasRepository.Save();

                System.Web.HttpContext.Current.Session["VasOrderNo"] = ordermaster.OrderId;
                orderdetail.OrderId = ordermaster.OrderId;
                orderdetail.PlanId = vasplan.PlanId;
                if (Plan.Contains("CRD"))
                {
                    orderdetail.PlanName = "CANDPRRAJ";
                }
                else
                {
                    orderdetail.PlanName = vasplan.PlanName;
                }
               // orderdetail.PlanName = vasplan.PlanName;
                orderdetail.Amount = vasplan.Amount;
                orderdetail.ValidityCount = vasplan.ValidityCount;
                orderdetail.RemainingCount = vasplan.ValidityCount;
                orderdetail.ValidityDays = vasplan.ValidityDays;

                if (DiscountAmount != null)
                {
                    orderdetail.DiscountAmount = Convert.ToInt32(DiscountAmount);
                }

                _vasRepository.AddOrderDetail(orderdetail);
                _vasRepository.Save();
            }

            if (ordermaster.Candidate != null)
            {
                registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Candidate.Id, EntryType.Candidate);
                subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
                registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
            }

            string candidateEmail = null;
            if (ordermaster.Candidate.Email != "")
            {
                candidateEmail = ordermaster.Candidate.Email;
            }
            else
            {
                candidateEmail = null;
            }

            /**************Start Email Templates as per Plans******************/

            if (VasType == "CRDPurchase")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/CRDSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Candidate.Name);
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber!=null? ordermaster.Candidate.ContactNumber:""));
                table = table.Replace("[EMAILID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"));

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName.ToString());
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY_DAYS]", orderdetail.ValidityDays.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.ValidityCount.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                if (candidateEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
                      vasplan.PlanName + " - subscribed",
                      table);

                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.CandidateSupport,
                             vasplan.PlanName + " - subscribed",
                             table);


                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       subscribedByAdmin.Email,
                        vasplan.PlanName + " - subscribed",
                        table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       registeredByAdmin.Email,
                        vasplan.PlanName + " - subscribed",
                        table);
                }

            }

            if (VasType == "WhoIsEmployer")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/WhoIsEmployerSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Candidate.Name);
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAILID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"));
              
                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", Plan);
                table = table.Replace("[AMOUNT]", ordermaster.Amount.ToString());
                table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                table = table.Replace("[VASTYPE]", Plan == "KER" ? "Regular" : "Express");
                table = table.Replace("[DELIVERYDAYS]", Plan == "KER" ? "5" : "2");

                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                if (candidateEmail!=null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
                      Plan + " - subscribed",
                      table);
                    
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.CandidateSupport,
                             Plan + " - subscribed",
                             table);


                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       registeredByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }
            }

            else if (VasType == "JobAlert")
            {
                
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/RAJSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Candidate.Name);
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAILID]", (candidateEmail != null ? candidateEmail : "Not Available"));

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "<b>Special Discount<span style='COLOR:red;'>*</span>@ 25% : </b>" + orderdetail.Amount * 25 / 100 + "<b>(25% Of Above Amount)</b>" : ""));
                table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span style='COLOR:red;'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : vasplan.Amount.ToString()));
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[VACANCIES]", vasplan.Vacancies.ToString());
                table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                table = table.Replace("[DAYS]", vasplan.ValidityDays.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                        candidateEmail,
                        Plan + " - subscribed",
                        table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                            Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.CandidateSupport,
                            Plan + " - subscribed",
                            table);


                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       registeredByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }
            }

            //else if (VasType == "SpotInterview")
            //{

            //}


            else if (VasType == "ResumeWriting" || VasType == "ExpressResumeWriting")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeWritingSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Candidate.Name);
                table = table.Replace("[MOBILE]", ordermaster.Candidate.ContactNumber);
                table = table.Replace("[EMAILID]", (candidateEmail != null ? candidateEmail : "Not Available"));
                
                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", Plan);
                table = table.Replace("[AMOUNT]", vasplan.Amount.ToString());
                
                if (DiscountAmount != null)
                {
                    table = table.Replace("[DISCOUNT_AMOUNT]", "Discount Amount: " + orderdetail.DiscountAmount.ToString());
                }
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", VasType == "ResumeWriting" ? "7" : "4");
                
                table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                        candidateEmail,
                       Plan + " - subscribed",
                       table);

                    
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                             Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.CandidateSupport,
                            Plan + " - subscribed",
                            table);
              

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       registeredByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }
            }


            else if (VasType == "DisplayResume")
            {

                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/DisplayResumeSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Candidate.Name);
                table = table.Replace("[MOBILE]", ordermaster.Candidate.ContactNumber);
                table = table.Replace("[EMAILID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"));

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "<b>Special Discount<span style='COLOR:red;'>*</span>@ 25% :</b>" + orderdetail.Amount * 25 / 100 : ""));
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span style='COLOR:red;'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : vasplan.Amount.ToString()));
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY_DAYS]", orderdetail.ValidityDays.ToString());
                
                
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       candidateEmail,
                       Plan + " - subscribed",
                       table);
                   
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                          Constants.EmailSender.VasEmailId,
                           Constants.EmailSender.CandidateSupport,
                           Plan + " - subscribed",
                           table);

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                       registeredByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }
            }

            else if (VasType == "SMSPurchase")
            {
                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(
                        Constants.EmailSender.CandidateSupport,
                        candidateEmail,
                        Constants.EmailSubject.SMSSubscription,
                        Constants.EmailBody.SMSSubscription
                        .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", ordermaster.Candidate.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Candidate.Name)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[LINK_NAME]", "CLICK THE LINK")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                        );
                }


                EmailHelper.SendEmailBCC(
                        Constants.EmailSender.CandidateSupport,
                        Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.CandidateSupport,
                        Constants.EmailSubject.SMSSubscription,
                        Constants.EmailBody.SMSSubscription
                           .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", ordermaster.Candidate.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Candidate.Name)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[LINK_NAME]", "CLICK THE LINK")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                        );

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(
                   Constants.EmailSender.CandidateSupport,
                   subscribedByAdmin.Email,
                   Constants.EmailSubject.SMSSubscription,
                   Constants.EmailBody.SMSSubscription
                    .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", ordermaster.Candidate.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Candidate.Name)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[LINK_NAME]", "CLICK THE LINK")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                   );
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(
                   Constants.EmailSender.CandidateSupport,
                   registeredByAdmin.Email,
                   Constants.EmailSubject.SMSSubscription,
                   Constants.EmailBody.SMSSubscription
                    .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                        .Replace("[NAME]", ordermaster.Candidate.Name)
                        .Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", ordermaster.Candidate.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Candidate.Name)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[LINK_NAME]", "CLICK THE LINK")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                   );
                }
            }

            if (ordermaster.Candidate.ContactNumber != null)
            {
                SmsHelper.SendSecondarySms(Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.SubscribePlan
                    .Replace("[NAME]", ordermaster.Candidate.Name)
                    .Replace("[DESCRIPTION]", orderdetail.VasPlan.Description)
                    .Replace("[PLAN]", orderdetail.PlanName.ToString())
                    .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? "" : vasplan.Amount.ToString()))
                    .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : ""))
                    .Replace("[DICOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "This Price is valid only for 3 days." : ""))
                    ,
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    ordermaster.Candidate.ContactNumber
                    );
            }


            return Json(new JsonActionResult
            {
                Success = true,
                Message = Plan + " - Subscribed",
                ReturnUrl="/Candidates/CandidatesVas/Payment"
            });
        }


        public void SendActivationMail(int orderId)
        {
            OrderDetail orderdetail = _vasRepository.GetOrderDetail(orderId);
            VasPlan vasplan = _vasRepository.GetVasPlanByPlanId(Convert.ToInt32(orderdetail.PlanId));
            OrderPayment orderpayment = _vasRepository.GetOrderPayment(orderId);
            OrderMaster ordermaster = _vasRepository.GetOrderMaster(orderId);
            
            string paymentmode = "";
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            var registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Candidate.Id, EntryType.Candidate);
            User subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
            User registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();

            if (orderpayment != null)
            {
                paymentmode = orderpayment.PaymentMode.ToString();
                if (paymentmode == "1")
                {
                    paymentmode = "Credit /Debit Card";
                }
                else if (paymentmode == "2")
                {
                    paymentmode = "Electronic Transfer";
                }

                else if (paymentmode == "3")
                {
                    paymentmode = "Cheque";
                }

                else if (paymentmode == "4")
                {
                    paymentmode = "Demand Draft";
                }

                else if (paymentmode == "5")
                {
                    paymentmode = "Pickup Cash";
                }

                else if (paymentmode == "6")
                {
                    paymentmode = "Deposit Cash";
                }

                else if (paymentmode == "7")
                {
                    paymentmode = "InterBank";
                }

                else if (paymentmode == "8")
                {
                    paymentmode = "NEFT";
                }

                else if (paymentmode == "9")
                {
                    paymentmode = "IMPS";
                }

                else if (paymentmode == "10")
                {
                    paymentmode = "Collected at office";
                }
            }


            if (orderdetail.VasPlan.Description.ToLower() == "CRDPurchase".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/CRDActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", orderdetail.OrderMaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != "" ? ordermaster.Candidate.Email : "Not Available"));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : ""));
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));

                table = table.Replace("[VALIDITY]", orderdetail.VasPlan.ValidityDays.ToString());
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[VACANCIES]", orderdetail.ValidityCount.ToString());

                if (ordermaster.Candidate.Email != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    ordermaster.Candidate.Email,
                     vasplan.PlanName + " - Activated",
                     table);
                }

                else
                {
                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                        Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.CandidateSupport,
                          vasplan.PlanName + " - Activated",
                          table);
                }

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       vasplan.PlanName + " - Activated",
                       table);
                }

            }

            if (orderdetail.VasPlan.Description.ToLower() == "WhoIsEmployer".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/WhoIsEmployerActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", (orderdetail.OrderMaster != null ? orderdetail.OrderMaster.CandidateId.ToString() : ""));
                table = table.Replace("[NAME]", (orderdetail.OrderMaster!=null ? orderdetail.OrderMaster.Candidate.Name: ""));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[VASTYPE]", orderdetail.PlanName == "KER" ? "Regular" : "Express");
                table = table.Replace("[DELIVERYDAYS]", orderdetail.PlanName == "KER" ? "5" : "2");
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (ordermaster.Candidate.Email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                   orderdetail.OrderMaster.Candidate.Email,
                      orderdetail.PlanName + " - Activated",
                      table);

                    
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.CandidateSupport,
                             orderdetail.PlanName + " - Activated",
                             table);

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }
                
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }
            }
            else if (orderdetail.VasPlan.Description.ToLower() == "ResumeWriting".ToLower() || orderdetail.VasPlan.Description.ToLower() == "ExpressResumeWriting".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeWritingActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", orderdetail.OrderMaster.CandidateId.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[MOBILE]", (orderdetail.OrderMaster.Candidate.ContactNumber!=null ? orderdetail.OrderMaster.Candidate.ContactNumber: ""));
                table = table.Replace("[EMAILID]", (orderdetail.OrderMaster!=null ? orderdetail.OrderMaster.Candidate.Email: ""));
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", orderdetail.VasPlan.Description == "ResumeWriting" ? "7" : "4");
               
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (ordermaster.Candidate.Email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                  orderdetail.OrderMaster.Candidate.Email,
                      orderdetail.PlanName + " - Activated",
                      table);
                   
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                    Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.CandidateSupport,
                        orderdetail.PlanName + " - Activated",
                        table);

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "Display Resume".ToLower())
            {

                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/DisplayResumeActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", orderdetail.OrderMaster.CandidateId.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[EMAIL_ID]", (ordermaster.Candidate.Email != null ? ordermaster.Candidate.Email : "Not Available"));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));
                
                table =  table.Replace("[PAYMENT_MODE]", (paymentmode!="" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (ordermaster.Candidate.Email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    orderdetail.OrderMaster.Candidate.Email,
                      orderdetail.PlanName + " - Activated",
                      table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                         Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.CandidateSupport,
                            orderdetail.PlanName + " - Activated",
                            table);

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }
            }

            else if (orderdetail.VasPlan.Description.ToLower().Contains("Job Alert".ToLower()))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/RAJActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", orderdetail.OrderMaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAIL_ID]", (ordermaster.Candidate.Email!="" ? ordermaster.Candidate.Email :"Not Available"));
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.ToString());
                table = table.Replace("[VALIDITY]", orderdetail.VasPlan.ValidityDays.ToString());

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (ordermaster.Candidate.Email != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    ordermaster.Candidate.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                else
                {
                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                        Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.CandidateSupport,
                          orderdetail.PlanName + " - Activated",
                          table);
                }

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

            }

            else if (orderdetail.VasPlan.Description.ToLower() == "SMSPurchase".ToLower())
            {
                if (ordermaster.Candidate.Email != null)
                {
                    EmailHelper.SendEmail(
                        Constants.EmailSender.CandidateSupport,
                        LoggedInCandidate.Email,
                        Constants.EmailSubject.SMSActivated,
                        Constants.EmailBody.SMSActivated
                        .Replace("[ID]", LoggedInCandidate.Id.ToString())
                        .Replace("[NAME]", LoggedInCandidate.Name)
                        .Replace("[EMAIL_ID]", (LoggedInCandidate.Email != null ? LoggedInCandidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", (LoggedInCandidate.ContactNumber!=null ? LoggedInCandidate.ContactNumber : ""))

                        .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                        .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                       
                        );
                }
                
                   EmailHelper.SendEmailBCC(
                   Constants.EmailSender.CandidateSupport,
                  Constants.EmailSender.VasEmailId,
                   Constants.EmailSender.CandidateSupport,
                   Constants.EmailSubject.SMSActivated,
                   Constants.EmailBody.SMSActivated
                  .Replace("[ID]", LoggedInCandidate.Id.ToString())
                        .Replace("[NAME]", LoggedInCandidate.Name)
                        .Replace("[EMAIL_ID]", (LoggedInCandidate.Email != null ? LoggedInCandidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", (LoggedInCandidate.ContactNumber != null ? LoggedInCandidate.ContactNumber : ""))

                        .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                        .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                   );
               
                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(
                    Constants.EmailSender.CandidateSupport,
                    subscribedByAdmin.Email,
                    Constants.EmailSubject.SMSActivated,
                    Constants.EmailBody.SMSActivated
                        .Replace("[ID]", LoggedInCandidate.Id.ToString())
                        .Replace("[NAME]", LoggedInCandidate.Name)
                        .Replace("[EMAIL_ID]", (LoggedInCandidate.Email != null ? LoggedInCandidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", (LoggedInCandidate.ContactNumber != null ? LoggedInCandidate.ContactNumber : ""))

                        .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                        .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                    );
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(
                    Constants.EmailSender.CandidateSupport,
                    registeredByAdmin.Email,
                    Constants.EmailSubject.SMSActivated,
                    Constants.EmailBody.SMSActivated
                        .Replace("[ID]", LoggedInCandidate.Id.ToString())
                        .Replace("[NAME]", LoggedInCandidate.Name)
                        .Replace("[EMAIL_ID]", (LoggedInCandidate.Email != null ? LoggedInCandidate.Email : "Not Available"))
                        .Replace("[MOBILE_NO]", (LoggedInCandidate.ContactNumber != null ? LoggedInCandidate.ContactNumber : ""))

                        .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                        .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Candidates")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                    );
                }
            }

            else if (orderdetail.VasPlan.Description.ToLower().Contains("BGC".ToLower()))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BackgroundCheckActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                //table = table.Replace("[CANDIDATE/EMPOYER]", orderdetail.OrderMaster.OrganizationId != null ? "Employer" : "Candidate");
                table = table.Replace("[NAME]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Name : orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[ID]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Id.ToString() : orderdetail.OrderMaster.Candidate.Id.ToString());
                table= table.Replace("[EMAILID]", (ordermaster.Organization!=null ? (ordermaster.Organization.Email!=""?ordermaster.Organization.Email : "") : (ordermaster.Candidate.Email!="" ? ordermaster.Candidate.Email : "")));
                table = table.Replace("[MOBILE]", (ordermaster.Organization != null ? (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "") : (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : "")));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", orderdetail.VasPlan.Description.ToLower() == "Academic record check".ToLower() ? "21" : "14");
               
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if(ordermaster.Candidate.Email!=null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                     orderdetail.OrderMaster.Organization.Email,
                      orderdetail.PlanName + " - Activated",
                      table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                           Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.CandidateSupport,
                             "BC- Activated",
                             table);

                if (subscribedByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribedByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredByAdmin.Email,
                       orderdetail.PlanName + " - Activated",
                       table);
                }
            }

           
            if (ordermaster.Candidate != null)
            {
                if (ordermaster.Candidate.ContactNumber != null)
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSReceiptofPayment
                          .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                          .Replace("[AMOUNT]", ordermaster.Amount.ToString())
                          .Replace("[NAME]", ordermaster.Candidate.Name)
                          .Replace("[PLANNAME]", vasplan.PlanName.ToString()),
                          Constants.SmsSender.SecondaryType,
                          Constants.SmsSender.Secondarysource,
                          Constants.SmsSender.Secondarydlr,
                          ordermaster.Candidate.ContactNumber
                          );
                }

            }
        }


        [HttpGet]
        public ActionResult Payment(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    orderId = Constants.DecryptString(orderId);

                    int OrderId;
                    bool ValueIsAnId = int.TryParse(orderId, out OrderId);

                    if (ValueIsAnId)
                    {
                        System.Web.HttpContext.Current.Session["VasOrderNo"] = OrderId;
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Payment()
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderDetail orderdetail = _vasRepository.GetOrderDetail(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderPayment GetOrderPayment = _vasRepository.GetOrderPayment(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                if (GetOrderPayment != null)
                {
                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(GetOrderPayment.OrderId));
                    OrderPayment orderpayment = new OrderPayment();
                    orderpayment.OrderId = ordermaster.OrderId;
                    orderpayment.PaymentMode = (int)PaymentMode.CreditDebitCard;
                    //orderpayment.Amount = ordermaster.Amount;
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = ordermaster.Amount;
                        }
                    }
                    orderpayment.PaymentDate = Constants.CurrentTime();
                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();
                    orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                }
                else
                {
                    OrderPayment orderpayment = new OrderPayment();
                    orderpayment.OrderId = ordermaster.OrderId;
                    orderpayment.PaymentMode = (int)PaymentMode.CreditDebitCard;
                    //orderpayment.Amount = ordermaster.Amount;
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = ordermaster.Amount;
                        }
                    }
                    orderpayment.PaymentDate = Constants.CurrentTime();

                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                    
                }

                Response.Redirect("http://www.dial4jobz.in//SFAClient//TestSSl.aspx?amount=" + ordermaster.Amount + "&orderid=" + ordermaster.OrderId + "&ResponseUrl=http://www.dial4jobz.in//SFAClient//CandidateResponse.aspx", false);
            }

            return View();
        }

        //**************************************************
        // Avenue Payment Gateway Get Page
        //**************************************************
        public ActionResult CCAVRequest()
        {
            return View();
        }

        //*************************************************
        // Avenue Payment Request Page
        //*************************************************

        [HttpPost]
        public ActionResult CCAVRequest(FormCollection collection)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            string workingKey = "91BE0EE8FE42847C3CA5E923E8D9752B";//put in the 32bit alpha numeric key in the quotes provided here 	
            string ccaRequest = "";

            foreach (string name in Request.Form)
            {
                if (name != null)
                {
                    if (!name.StartsWith("_"))
                    {
                        ccaRequest = ccaRequest + name + "=" + Request.Form[name] + "&";
                    }
                }
            }

            strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);

            ViewData["strEncRequest"] = strEncRequest;
            ViewData["access_code"] = strAccessCode;

            return View();
        }

        //*************************************************
        // Avenue Payment Gateway Response(after payment)
        //**************************************************

        public ActionResult CCAVResponse()
        {
            return View();
        }

        //*************************************************
        // Avenue Payment Gateway Response(Redirect to Activation of Employer)
        //**************************************************

        [HttpPost]
        public ActionResult CCAVResponse(FormCollection collection)
        {
            string workingKey = "91BE0EE8FE42847C3CA5E923E8D9752B";//put in the 32bit alpha numeric key in the quotes provided here
            string orderId = string.Empty;
            string orderid = string.Empty;
            string paymentStatus = string.Empty;
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }

            for (int i = 0; i < Params.Count; i++)
            {
                Response.Write(Params.Keys[i] + " = " + Params[i] + "<br>");
                orderId = Params.GetValues(0).ToString();
                paymentStatus = Params.GetValues(3).ToString();

                orderid = Params["order_id"];
                paymentStatus = Params["order_status"];
            }

            if (paymentStatus == "Success")
            {
                //return RedirectToAction("EmployerPaymentSuccessful", "EmployerVas", new { value = Constants.EncryptString(orderid) });
                //return RedirectToAction("EmployerPaymentSuccessful", "EmployerVas", new { value = orderid });
                Response.Redirect("http://www.dial4jobz.in/candidates/candidatesvas/CandidatesPayment?value=" + Constants.EncryptString(orderid), true);
            }

            return RedirectToAction("DashBoard", "Candidates");
        }


        [HttpGet]
        public ActionResult ElectronicTransfer()
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderDetail orderdetail = _vasRepository.GetOrderDetail(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderPayment GetOrderPayment = _vasRepository.GetOrderPayment(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                if (GetOrderPayment != null)
                {
                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(GetOrderPayment.OrderId));
                    OrderPayment orderpayment = new OrderPayment();

                    orderpayment.OrderId = ordermaster.OrderId;
                    orderpayment.PaymentMode = (int)PaymentMode.ElectronicTransfer;
                    //orderpayment.Amount = ordermaster.Amount;
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = ordermaster.Amount;
                        }
                    }
                    orderpayment.PaymentDate = Constants.CurrentTime();

                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();

                    ViewData["OrderNo"] = ordermaster.OrderId;
                    ViewData["Amount"] = ordermaster.Amount;
                }
                else
                {
                    OrderPayment orderpayment = new OrderPayment();
                    orderpayment.OrderId = ordermaster.OrderId;
                    orderpayment.PaymentMode = (int)PaymentMode.ElectronicTransfer;
                    //orderpayment.Amount = ordermaster.Amount;
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = ordermaster.Amount;
                        }
                    }
                    orderpayment.PaymentDate = Constants.CurrentTime();

                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();

                    ViewData["OrderNo"] = ordermaster.OrderId;
                    ViewData["Amount"] = ordermaster.Amount;
                }
                return View();
            }

            return View();
        }

        [Authorize, HttpPost]
        public ActionResult ElectronicTransfer(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderPayment orderpayment = new OrderPayment();

                orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                orderpayment.PaymentMode = (int)PaymentMode.ElectronicTransfer;
                orderpayment.TransferReference = collection["TransferReference"];
                orderpayment.TransferDate = Convert.ToDateTime(collection["TransferDate"]);
                orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                orderpayment.DrawnOnBank = collection["TransferredBank"];

                _vasRepository.AddOrderPayment(orderpayment);
                _vasRepository.Save();

                OrderDetail orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));

                orderdetails.PaymentId = orderpayment.PaymentId;
                _vasRepository.Save();

                VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetails.PlanName);

                if (LoggedInCandidate != null)
                {
                    if (LoggedInCandidate.Email != null)
                    {
                        EmailHelper.SendEmail(
                       Constants.EmailSender.CandidateSupport,
                       LoggedInCandidate.Email,
                       Constants.EmailSubject.PaymentDetails,
                       Constants.EmailBody.PaymentModeElectronicTransferForCandidate
                               .Replace("[NAME]", LoggedInCandidate.Name)
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                            //.Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                            //.Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                               .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                    }

                    EmailHelper.SendEmailBCC(
                       Constants.EmailSender.CandidateSupport,
                        Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.CandidateSupport,
                       Constants.EmailSubject.PaymentDetails,
                       Constants.EmailBody.PaymentModeElectronicTransferForCandidate
                               .Replace("[NAME]", LoggedInCandidate.Name)
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[BANKNAME]", orderpayment.DrawnOnBank));

                }

                else if (Session["LoginAs"] == "CandidateViaAdmin")
                {
                    int candidateId = (int)Session["CandId"];
                    Candidate candidate = _repository.GetCandidate(candidateId);
                    if (candidate.Email != null)
                    {
                        EmailHelper.SendEmail(
                          Constants.EmailSender.CandidateSupport,
                          candidate.Email,
                          Constants.EmailSubject.PaymentDetails,
                          Constants.EmailBody.PaymentModeElectronicTransferForCandidate
                                  .Replace("[NAME]", candidate.Name)
                                  .Replace("[PLAN]", orderdetails.PlanName)
                                  .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                                  .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                                   .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                            //.Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                            //.Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                                  .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                    }

                    EmailHelper.SendEmailBCC(
                     Constants.EmailSender.CandidateSupport,
                    Constants.EmailSender.VasEmailId,
                    Constants.EmailSender.CandidateSupport,
                     Constants.EmailSubject.PaymentDetails,
                     Constants.EmailBody.PaymentModeElectronicTransferForCandidate
                             .Replace("[NAME]", candidate.Name)
                             .Replace("[PLAN]", orderdetails.PlanName)
                             .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                             .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                             .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                              .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                        //.Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                        //.Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                             .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                }

            }

            return RedirectToAction("Index");

        }

        public ActionResult CallUsForPickupCash()
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                ViewData["OrderNo"] = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                ViewData["Amount"] = ordermaster.Amount;
                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult CallUsForPickupCash(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                
                OrderPayment GetOrderPayment = _vasRepository.GetOrderPayment(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderDetail orderdetails = null;
                OrderPayment orderpayment = null;

                if (GetOrderPayment != null)
                {
                    orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                    orderdetails.PaymentId = null;
                    _vasRepository.Save();

                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                    orderpayment = new OrderPayment();

                    orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                    orderpayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.PaymentMode = (int)PaymentMode.PickupCash;
                    //orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    if (orderdetails != null)
                    {
                        if (orderdetails.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetails.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                        }
                    }
                    orderpayment.Branch = collection["City"];
                    orderpayment.Regions = collection["Region"];

                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    
                    orderdetails.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                }
                else
                {
                    orderpayment = new OrderPayment();
                    orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                    orderpayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.PaymentMode = (int)PaymentMode.PickupCash;
                    //orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    if (orderdetails != null)
                    {
                        if (orderdetails.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetails.DiscountAmount;
                        }
                        else
                        {
                            orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                        }
                    }

                    orderpayment.Branch = collection["City"];
                    orderpayment.Regions = collection["Region"];

                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));
                    orderdetails.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                }

                VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetails.PlanName);


                var paymentmode = orderpayment.PaymentMode.ToString();

                if (paymentmode == "1")
                {
                    paymentmode = "GateWay";
                }
                else if (paymentmode == "3")
                {
                    paymentmode = "Cheque";
                }
                else if (paymentmode == "5")
                {
                    paymentmode = "Pickup Cash";
                }

                if (LoggedInCandidate != null)
                {
                    if (LoggedInCandidate.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                            LoggedInCandidate.Email,
                            Constants.EmailSubject.PaymentDetails,
                            Constants.EmailBody.PaymentModePickupCashCandidate
                               .Replace("[CANDIDATE_NAME]", LoggedInCandidate.Name)
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[EMAIL_ID]", LoggedInCandidate.Email)
                                .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[CONTACT_NUMBER]", LoggedInCandidate.ContactNumber)
                               .Replace("[ALERTS]", (orderdetails.ValidityCount != null ? orderdetails.ValidityCount.ToString() : ""))
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", orderpayment.Branch)
                               .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() + " Days" : "1 Year")
                               .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                               .Replace("[BRANCH]", orderpayment.Branch)
                               .Replace("[AREA]", orderpayment.Regions)
                               );
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                       Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.CandidateSupport,
                        Constants.EmailSubject.PaymentDetails,
                        Constants.EmailBody.PaymentModePickupCashCandidate
                           .Replace("[CANDIDATE_NAME]", LoggedInCandidate.Name)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[EMAIL_ID]", LoggedInCandidate.Email)
                            .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[CONTACT_NUMBER]", LoggedInCandidate.ContactNumber)
                           .Replace("[ALERTS]", (orderdetails.ValidityCount != null ? orderdetails.ValidityCount.ToString() : ""))
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() + " Days" : "1 Year")
                           .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );
                }
                else if (Session["LoginAs"] == "CandidateViaAdmin")
                {
                    int candidateId = (int)Session["CandId"];
                    Candidate candidate = _repository.GetCandidate(candidateId);

                    if (candidate.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                            candidate.Email,
                            Constants.EmailSubject.PaymentDetails,
                            Constants.EmailBody.PaymentModePickupCashCandidate
                               .Replace("[CANDIDATE_NAME]", orderdetails.OrderMaster.Candidate.Name)
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[EMAIL_ID]", candidate.Email)
                               .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                                .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ALERTS]", orderdetails.ValidityCount.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", orderpayment.Branch)
                               .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : "1 Year")
                               .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                               .Replace("[BRANCH]", orderpayment.Branch)
                               .Replace("[AREA]", orderpayment.Regions)
                               );
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                       Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.CandidateSupport,
                        Constants.EmailSubject.PaymentDetails,
                        Constants.EmailBody.PaymentModePickupCashCandidate
                           .Replace("[CANDIDATE_NAME]", orderdetails.OrderMaster.Candidate.Name)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[EMAIL_ID]", candidate.Email)
                           .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                            .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[ALERTS]", orderdetails.ValidityCount.ToString())
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : "1 Year")
                           .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );
                }


            }
            return RedirectToAction("Payment", "CandidatesVas");
        }

        [HttpGet]
        public ActionResult DepositChequeDraft()
        {

            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                ViewData["OrderNo"] = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                ViewData["Amount"] = ordermaster.Amount;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DepositChequeDraft(FormCollection collection)
        {
            
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderPayment orderpayment = new OrderPayment();
                OrderPayment GetOrderPayment = _vasRepository.GetOrderPayment(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                //OrderDetail orderdetails = new OrderDetail();

                OrderDetail orderdetails = _vasRepository.GetOrderDetail(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                /*Order details table (F.K) so first i update with null value.*/
                if (orderdetails != null)
                {
                    orderdetails.PaymentId = null;
                    _vasRepository.Save();
                }
                /*End Update*/
                if (orderdetails.DiscountAmount != null)
                {
                    orderpayment.Amount = orderdetails.DiscountAmount;
                }
                else
                {
                    orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                }
                orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                if (collection["ddlInstrumentType"] == "0")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.Cheque;
                    orderpayment.ChequeNumber = collection["ReferenceNumber"];
                    orderpayment.DepositedOn = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.DrawnOnBank = collection["Drawn"];
                    orderpayment.Branch = collection["BankBranch"];
                }
                else if (collection["ddlInstrumentType"] == "1")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.DemandDraft;
                    orderpayment.ChequeNumber = collection["ReferenceNumber"];
                    orderpayment.DrawnOnBank = collection["Drawn"];
                    orderpayment.DepositedOn = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.Branch = collection["BankBranch"];
                }
                else if (collection["ddlInstrumentType"] == "2")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.CashDeposit;
                    orderpayment.DepositedOn = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.Branch = collection["BankBranch"];

                }
                else if (collection["ddlInstrumentType"] == "3")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.InterBank;
                    orderpayment.TransferDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.TransferReference = collection["ReferenceNumber"];
                }
                else if (collection["ddlInstrumentType"] == "4")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.NEFT;
                    orderpayment.TransferReference = collection["ReferenceNumber"];
                    orderpayment.TransferDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.DrawnOnBank = collection["Drawn"];
                }
                else if (collection["ddlInstrumentType"] == "5")
                {
                    orderpayment.PaymentMode = (int)PaymentMode.IMPS;
                    orderpayment.TransferDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.TransferReference = collection["ReferenceNumber"];
                }

                if (GetOrderPayment != null)
                {
                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                    //orderpayment = new OrderPayment();
                    orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();
                                        

                    if (orderdetails != null)
                    {
                        orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderdetails.OrderId));
                        orderdetails.PaymentId = orderpayment.PaymentId;
                        _vasRepository.Save();
                    }
                }

                else
                {

                    orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    if (orderdetails != null)
                    {
                        orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));
                        orderdetails.PaymentId = orderpayment.PaymentId;
                        _vasRepository.Save();
                    }
                }


                VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetails.PlanName);
               
                var paymentmode = orderpayment.PaymentMode.ToString();
                if (paymentmode == "1")
                {
                    paymentmode = "GateWay";
                }
                else if (paymentmode == "3")
                {
                    paymentmode = "Cheque";
                }
                else if (paymentmode == "4")
                {
                    paymentmode = "Demand Draft";
                }
                else if (paymentmode == "6")
                {
                    paymentmode = "Deposit Cash";
                }

                else if (paymentmode == "7")
                {
                    paymentmode = "Inter Bank";
                }

                else if (paymentmode == "8")
                {
                    paymentmode = "NEFT";
                }

                else if (paymentmode == "9")
                {
                    paymentmode = "IMPS";
                }

                if (LoggedInCandidate != null)
                {
                    if (LoggedInCandidate.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                                LoggedInCandidate.Email,
                                Constants.EmailSubject.PaymentDetails,
                              Constants.EmailBody.PaymentModeDepositForCandidate
                                   .Replace("[NAME]", LoggedInCandidate.Name)
                                   .Replace("[PLAN]", orderdetails.PlanName)
                                   .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                                   .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                   .Replace("[INSTRUMENT_TYPE]", paymentmode)
                                   .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                                   .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                                   .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                                   .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                                   .Replace("[ALERTS]", (vasplan.ValidityCount != null ? vasplan.ValidityCount.ToString() : ""))
                                   .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                                   .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                                   .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                    }


                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                              Constants.EmailSender.VasEmailId,
                              Constants.EmailSender.CandidateSupport,
                              Constants.EmailSubject.PaymentDetails,
                              Constants.EmailBody.PaymentModeDepositForCandidate
                                   .Replace("[NAME]", LoggedInCandidate.Name)
                                   .Replace("[PLAN]", orderdetails.PlanName)
                                   .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                                   .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                   .Replace("[INSTRUMENT_TYPE]", paymentmode)
                                   .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                                   .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                                   .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                                   .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                                   .Replace("[ALERTS]", (vasplan.ValidityCount != null ? vasplan.ValidityCount.ToString() : ""))
                                   .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                                   .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                                   .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                }

                else if (Session["LoginAs"] == "CandidateViaAdmin")
                {
                    int candidateId = (int)Session["CandId"];
                    Candidate candidate = _repository.GetCandidate(candidateId);

                    if (candidate.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                                 candidate.Email,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentModeDepositForCandidate
                                    .Replace("[NAME]", candidate.Name)
                                    .Replace("[PLAN]", orderdetails.PlanName)
                                    .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                                    .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                    .Replace("[INSTRUMENT_TYPE]", paymentmode)
                                    .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                                    .Replace("[BRANCH]", orderpayment.Branch)
                                    .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                                    .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                                    .Replace("[ALERTS]", (vasplan.ValidityCount != null ? vasplan.ValidityCount.ToString() : ""))
                                    .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                                    .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                                    .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                                Constants.EmailSender.VasEmailId,
                                 Constants.EmailSender.CandidateSupport,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentModeDepositForCandidate
                                    .Replace("[NAME]", candidate.Name)
                                    .Replace("[PLAN]", orderdetails.PlanName)
                                    .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                                    .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                    .Replace("[INSTRUMENT_TYPE]", paymentmode)
                                    .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                                    .Replace("[BRANCH]", orderpayment.Branch)
                                    .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                                    .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                                    .Replace("[ALERTS]", (vasplan.ValidityCount != null ? vasplan.ValidityCount.ToString() : ""))
                                    .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                                    .Replace("[BANK_NAME]", (orderpayment.DrawnOnBank != null ? orderpayment.DrawnOnBank : ""))
                                    .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                }
            }

            return RedirectToAction("Payment", "CandidatesVas");
        }
       

        public ActionResult WhoIsEmployer()
        {
            return View();
        }



    }
}
