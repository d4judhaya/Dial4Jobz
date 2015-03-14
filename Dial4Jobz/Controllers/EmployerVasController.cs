using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;
using System.IO;
using System.ComponentModel.DataAnnotations;
//using SFA;
using System.Web.Routing;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Helpers;
using System.Threading;
using System.Text;
using System.Configuration;
using System.Web.SessionState;
using CCA.Util;
using System.Collections.Specialized;


namespace Dial4Jobz.Controllers
{
    public class EmployerVasController : BaseController
    {

        //private static int orderNumber = 0;

        public string strAccessCode = "AVZH01BE45AF56HZFA";// put the access key in the quotes provided here.
        public string strEncRequest = "";
        //
        // GET: /EmployerVas/

        VasRepository _vasRepository = new VasRepository();
        Repository _repository = new Repository();
        EmployerController employercontroller = new EmployerController();
        private Dial4JobzEntities _db = new Dial4JobzEntities();


       [Authorize]
        public ActionResult Index(string value)
        {
            return View();
        }

        
        //**************After Payment Successfull************//
       public ActionResult EmployerPayment(string value)
       {
           if (!string.IsNullOrEmpty(value))
           {
               try
               {
                   value = Constants.DecryptString(value);
                   int OrderId;
                   bool ValueIsAnId = int.TryParse(value, out OrderId);
                   var planDetails = _vasRepository.GetOrderDetail(OrderId);
                   string planName = string.Empty;
                   if (planDetails.PlanName.Contains("Basic"))
                   {
                       planName = "E-Basic";
                   }
                   else
                   {
                       planName = planDetails.PlanName.ToString();
                   }

                   if (ValueIsAnId)
                   {
                       _vasRepository.ActivateVAS(OrderId);
                       try
                       {
                           SendActivationMail(OrderId);
                       }
                       catch
                       {
                           ViewData["Failure"] = "Your Payment is done. A Technical Error Has Occured. We Apologise For the Inconvenience. Contact Our HR Advisor at 044 44455566 for confirmation ";
                           //Kindly Check Your Bank Account, If Payment is debited, Contact Our HR Advisor at 044 44455566 for confirmation ";
                           return View();
                       }
                       OrderMaster orderSuccess = _vasRepository.GetOrderMaster(OrderId);
                       if (orderSuccess.PaymentStatus == true)
                       {
                           ViewData["Success"] = "Thanks, We have received your Payment Rs. " + planDetails.Amount + ". Your " + planName + "  is acivated. You can now start using this plan.";
                       }
                       else
                       {
                           ViewData["Failure"] = "We regret your order activation was not successfull. Kindly Contact our Adviser 044 - 44455566  for Assistance";
                       }
                   }
               }
               catch
               {
                   ViewData["Failure"] = "We regret your Payment was not Successfull.Kindly Contact our Adviser 044 - 44455566  for Assistance";
                   return View();
               }
           }
           else
           {
               ViewData["Failure"] = "Activation is not Successfull. Check your Order exists or Not. Login and try again later.";
           }
           return View();
       }

        
        public void SendActivationMail(int OrderId)
        {
            OrderDetail orderdetail = _vasRepository.GetOrderDetail(OrderId);
            OrderMaster ordermaster = _vasRepository.GetOrderMaster(OrderId);
            OrderPayment orderpayment = _vasRepository.GetOrderPayment(OrderId);
            VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetail.PlanName);

            var registeredBy = string.Empty;
            User registeredByAdmin = null;
            string subscribeEmail = null;
            string registeredEmail = null;
            if (ordermaster.Organization != null)
            {
                registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Organization.Id, EntryType.Employer);
            }
            User subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
            if (registeredBy != "")
            {
                registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
            }

            if (subscribedByAdmin != null)
            {
                subscribeEmail = subscribedByAdmin.Email;
            }

            if (registeredByAdmin != null)
            {
                registeredEmail = registeredByAdmin.Email;
            }

            string organizationEmail = null;
            string organizationMobile = null;
            

            if (LoggedInConsultant != null)
            {
                organizationEmail = ordermaster.Consultante.Email;
            }

            if (ordermaster.Organization != null)
            {
                if (ordermaster.Organization.Email != null)
                {
                    organizationEmail = ordermaster.Organization.Email;
                }
            }
            else
            {
                organizationEmail = null;
            }

            if (ordermaster.Organization != null)
            {
                if (ordermaster.Organization.MobileNumber != "")
                {
                    organizationMobile = ordermaster.Organization.MobileNumber;
                }
            }
            else
            {
                organizationMobile = null;
            }

            string paymentmode = "";


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


            if (orderdetail.PlanName.Contains("HORS") && orderdetail.Amount == 219 || orderdetail.Amount == 336)
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/HotResumesActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }
                               

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", "COMBO HORS");
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[RESUMES]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[DURATION]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());

                table = table.Replace("[PAYMENT_MODE]", paymentmode);
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (orderdetail.PlanName.Contains("HORSCOMBO"))
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + orderdetail.VasPlan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    organizationEmail,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.EmployerSupport,
                             orderdetail.PlanName + " - Activated",
                             table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        

            }

            else if (orderdetail.PlanName.Contains("RAT") && orderdetail.Amount == 219 || orderdetail.Amount == 336)
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", "COMBO RAT");
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DURATION]", vasplan.ValidityDays.ToString());
                table = table.Replace("[VALIDITY]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());

                table = table.Replace("[PAYMENT_MODE]", paymentmode != null ? paymentmode : "");
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (orderdetail.PlanName.Contains("RATCOMBO"))
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + orderdetail.VasPlan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            organizationEmail,
                            orderdetail.PlanName + " - Activated",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

              
                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                       Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.EmployerSupport,
                         orderdetail.PlanName + " - Activated",
                         table);


                    if (subscribeEmail != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           subscribeEmail,
                           vasplan.PlanName + " - Activated",
                           table);
                    }

                    if (registeredEmail != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           registeredEmail,
                           vasplan.PlanName + " - Activated",
                           table);
                    }        

                
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "Consultant Plans".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/RCActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                              
                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                        Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.EmployerSupport,
                        orderdetail.PlanName + " - Activated",
                        table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "Resume Alert".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }


                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[DURATION]", vasplan.ValidityDays.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));

                if (orderdetail.PlanName.Contains("RATCOMBO"))
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + orderdetail.VasPlan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }


                if (organizationEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    organizationEmail,
                    orderdetail.PlanName + " - Activated",
                    table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                        Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.EmployerSupport,
                        orderdetail.PlanName + " - Activated",
                        table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "Hot Resumes".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/HotResumesActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }


                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DURATION]", orderdetail.VasPlan.ValidityDays.ToString());
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());
                table = table.Replace("[RESUMES]", orderdetail.ValidityCount.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));

                if (orderdetail.PlanName.Contains("HORSCOMBO"))
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + orderdetail.VasPlan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    organizationEmail,
                     orderdetail.PlanName + " - Activated",
                     table);

                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                          Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            orderdetail.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

            else if (orderdetail.VasPlan.PlanName.ToLower().Contains("BGC".ToLower()))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BackgroundCheckActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", orderdetail.VasPlan.Description.ToLower() == "Academic record check".ToLower() ? "21" : "14");

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    organizationEmail,
                    orderdetail.PlanName + " - Activated",
                    table);

                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

              

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                           Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            "BC- Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

            if (orderdetail.PlanName.Contains("Basic"))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BasicPlanActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.Amount.ToString()));
                table = table.Replace("[VALIDITY_COUNT]", orderdetail.RemainingCount.ToString());
                table = table.Replace("[RESUMES_COUNT]", orderdetail.BasicCount.ToString());
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());
                table = table.Replace("[DURATION]", orderdetail.VasPlan.ValidityDays.ToString());
                table = table.Replace("[ACTIVATION_DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    organizationEmail,
                    vasplan.PlanName + " - Activated",
                     table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     vasplan.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                          Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            vasplan.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "Spot Selection".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SpotSelectionActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Organization != null ? ordermaster.Organization.Id.ToString() : ordermaster.Consultante.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Organization != null ? ordermaster.Organization.Name : ordermaster.Consultante.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Organization != null ? ordermaster.Organization.ContactPerson : ordermaster.Consultante.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""));
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[FROMDATE]", (orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""));
                table = table.Replace("[TODATE]", (orderdetail.ValidityTill != null ? orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy") : ""));
                table = table.Replace("[VALIDITY]", (orderdetail.ValidityCount != null ? orderdetail.ValidityCount.ToString() : ""));

                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));


                if (organizationEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                     organizationEmail,
                       orderdetail.PlanName + " - Activated",
                       table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    LoggedInConsultant.Email,
                     orderdetail.PlanName + " - Activated",
                     table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                         Constants.EmailSender.VasEmailId,
                           Constants.EmailSender.EmployerSupport,
                           "SS - Activated",
                           table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                       registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }        
            }

           else if (orderdetail.VasPlan.Description.ToLower() == "SMSPurchase".ToLower())
           {
               if (organizationEmail!=null)
               {
                   EmailHelper.SendEmail(
                       Constants.EmailSender.EmployerSupport,
                       organizationEmail,
                       Constants.EmailSubject.SMSActivated,
                       Constants.EmailBody.SMSActivated
                       
                      /***Personal Details***/
                    .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                    .Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name))
                    .Replace("[CONTACTPERSON]", (ordermaster.Consultante!=null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson))
                    .Replace("[EMAILID]", (ordermaster.Consultante!=null ? ordermaster.Consultante.Email : (ordermaster.Organization.Email!="" ?ordermaster.Organization.Email : "")))
                    .Replace("[MOBILE]", (ordermaster.Consultante!=null ? ordermaster.Consultante.MobileNumber : (ordermaster.Organization.MobileNumber!="" ? ordermaster.Organization.MobileNumber :"")))
                       
                    .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                    .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                    .Replace("[PLAN]", orderdetail.PlanName)
                    .Replace("[AMOUNT]", (orderdetail.DiscountAmount!=null? orderdetail.DiscountAmount.ToString(): orderdetail.Amount.ToString()))
                    .Replace("[CONTACTPERSON]", orderdetail.OrderMaster.Organization.ContactPerson)
                    .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                    .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                    .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                    .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                     .Replace("[NOTICE]", "Important Notice for Employers")
                     .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                       
                       );
               }

             
                   EmailHelper.SendEmailBCC(
                       Constants.EmailSender.EmployerSupport,
                       Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.EmployerSupport,
                       Constants.EmailSubject.SMSActivated,
                       Constants.EmailBody.SMSActivated
                       .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                       .Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name))
                       .Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson))
                       .Replace("[EMAILID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Email : (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "")))
                       .Replace("[MOBILE]", (ordermaster.Consultante != null ? ordermaster.Consultante.MobileNumber : (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "")))
                       

                       .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                       .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""))
                       .Replace("[PLAN]", orderdetail.PlanName)
                       .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()))
                       .Replace("[CONTACTPERSON]", orderdetail.OrderMaster.Organization.ContactPerson)
                       .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                       .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                       .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                       .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                       );
              
               /*Mail send to Who subscribed this Employer by admin*/
                   if (subscribeEmail != null)
               {

                   EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        subscribeEmail,
                        Constants.EmailSubject.SMSActivated,
                        Constants.EmailBody.SMSActivated
                       .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                       .Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name))
                       .Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson))
                       .Replace("[EMAILID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Email : (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "")))
                       .Replace("[MOBILE]", (ordermaster.Consultante != null ? ordermaster.Consultante.MobileNumber : (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "")))
                       

                       .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                       .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""))
                       .Replace("[PLAN]", orderdetail.PlanName)
                       .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()))
                       .Replace("[CONTACTPERSON]", orderdetail.OrderMaster.Organization.ContactPerson)
                       .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                       .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                       .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                       .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                        );
               }

               /*Mail send to Who registered this Employer by admin*/
               if (registeredEmail != null)
               {
                   EmailHelper.SendEmail(
                      Constants.EmailSender.EmployerSupport,
                      registeredEmail,
                      Constants.EmailSubject.SMSActivated,
                      Constants.EmailBody.SMSActivated
                       .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                       .Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name))
                       .Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson))
                       .Replace("[EMAILID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Email : (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "")))
                       .Replace("[MOBILE]", (ordermaster.Consultante != null ? ordermaster.Consultante.MobileNumber : (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "")))
                       

                       .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                       .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice != null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString() : ""))
                       .Replace("[PLAN]", orderdetail.PlanName)
                       .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()))
                       .Replace("[CONTACTPERSON]", orderdetail.OrderMaster.Organization.ContactPerson)
                       .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                       .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                       .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                       .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                      );
               }

               
           }

           if (organizationMobile != null)
           {
               SmsHelper.SendSecondarySms(
                   Constants.SmsSender.SecondaryUserName,
                   Constants.SmsSender.SecondaryPassword,
                   Constants.SmsBody.SMSReceiptofPayment
                     .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                     .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()))
                     .Replace("[NAME]", ordermaster.Organization.Name)
                     .Replace("[PLANNAME]", orderdetail.PlanName),
                     Constants.SmsSender.SecondaryType,
                     Constants.SmsSender.Secondarysource,
                     Constants.SmsSender.Secondarydlr,
                     organizationMobile
                     );
           }

           if (LoggedInConsultant != null)
           {
               SmsHelper.SendSecondarySms(
                  Constants.SmsSender.SecondaryUserName,
                  Constants.SmsSender.SecondaryPassword,
                  Constants.SmsBody.SMSReceiptofPayment
                    .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                    .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()))
                    .Replace("[NAME]", ordermaster.Organization.Name)
                    .Replace("[PLANNAME]", orderdetail.PlanName),
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    LoggedInConsultant.MobileNumber
                    );
           }
        }

        public ActionResult PaymentSuccess()
        {
            return View();
        }


        //*******************************
        // Subscribing combo plans (temporary plans)
        //*******************************
        [HttpPost]
        public ActionResult SubscribedComboPlans(string Plan, string Amount)
        {
            string[] plans = Plan.Split(',');

            foreach (string combovasplans in plans)
            {
                //VasPlan comboplan = _vasRepository.GetVasPlanbyPlanName(combovasplans);
                OrderMaster ordermaster = new OrderMaster();
                ordermaster.OrderDate = Constants.CurrentTime();

                User user = _userRepository.GetUsersbyUserName(User.Identity.Name).FirstOrDefault();

                if (LoggedInOrganization != null)
                {
                    ordermaster.OrganizationId = LoggedInOrganization.Id;
                    ordermaster.SubscribedBy = LoggedInOrganization.Name;
                }
                else if (Session["LoginAs"] == "EmployerViaAdmin")
                {
                    int adminOrganizationId = (int)Session["empId"];
                    ordermaster.OrganizationId = adminOrganizationId;
                }

                if (Session["LoginUser"] != null)
                {
                    var subscribedby = Session["LoginUser"].ToString();
                    ordermaster.SubscribedBy = subscribedby;
                }

                if (user != null)
                {
                   ordermaster.SubscribedBy = user.UserName;
                }

                ordermaster.Amount = Convert.ToInt32(Amount);
                ordermaster.PaymentStatus = false;

                _vasRepository.AddOrderMaster(ordermaster);
                _vasRepository.Save();

                System.Web.HttpContext.Current.Session["VasOrderNo"] = ordermaster.OrderId;

                OrderDetail orderdetail = new OrderDetail();
                orderdetail.OrderId = ordermaster.OrderId;
                orderdetail.PlanName = combovasplans;
                orderdetail.Amount = Convert.ToInt32(Amount);
                
                if (combovasplans.Contains("HORS") && Amount=="219")
                {
                    orderdetail.ValidityCount = 25;
                    orderdetail.RemainingCount = 25;
                    orderdetail.EmailRemainingCount = 100;
                }

                if (combovasplans.Contains("HORS") && Amount == "336")
                {
                    orderdetail.ValidityCount = 50;
                    orderdetail.RemainingCount = 50;
                    orderdetail.EmailRemainingCount = 200;
                }

                if (combovasplans.Contains("RAT") && Amount=="219")
                {
                    orderdetail.Vacancies = 1;
                    orderdetail.ValidityCount = 25;
                    orderdetail.RemainingCount = 25;
                }

                else if (combovasplans.Contains("RAT") && Amount == "336")
                {
                    orderdetail.Vacancies = 2;
                    orderdetail.ValidityCount = 50;
                    orderdetail.RemainingCount = 50;
                }

                _vasRepository.AddOrderDetail(orderdetail);
                _vasRepository.Save();

                var registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Organization.Id, EntryType.Employer);
                User subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
                User registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();

                string organizationEmail = null;
                string organizationMobile = null;

                if (ordermaster.Organization.Email != "")
                {
                    organizationEmail = ordermaster.Organization.Email;
                }
                else
                {
                    organizationEmail = null;
                }

                if (ordermaster.Organization.MobileNumber != "")
                {
                    organizationMobile = ordermaster.Organization.MobileNumber;
                }
                else
                {
                    organizationMobile = null;
                }

                if (combovasplans.Contains("HORS"))
                {

                    StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/HotResumesSubscribed.htm"));
                    string table = reader.ReadToEnd();
                    reader.Dispose();
                    table = table.Replace("[NAME]", ordermaster.Organization.Name);
                    table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                    table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                    table = table.Replace("[MOBILE]", organizationMobile);
                    table = table.Replace("[EMAILID]", ordermaster.Organization.Email);
                    table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                    table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                    table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());
                    table = table.Replace("[AMOUNT]", Amount.ToString());
                    table = table.Replace("[PLAN]", "COMBO OFFER - HORS");
                    table = table.Replace("[VALIDITY_COUNT]", orderdetail.ValidityCount.ToString());
                    table = table.Replace("[RESUMES]", orderdetail.ValidityCount.ToString());
                    table = table.Replace("[DURATION]", "7");
                    table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                    table = table.Replace("[COMBO_TEXT]", "");
                    table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                    table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                    if (ordermaster.Organization.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                               ordermaster.Organization.Email,
                                Plan + " - subscribed",
                                table);
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                      Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.EmployerSupport,
                        Plan + " - subscribed",
                        table);

                    /*Mail send to Subscribed User by admin*/
                    if (subscribedByAdmin != null)
                    {

                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            subscribedByAdmin.Email,
                              Plan + " - subscribed",
                            table);
                    }

                    /*Mail send to Who registered this Employer by admin*/
                    if (registeredByAdmin != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            registeredByAdmin.Email,
                            Plan + " - subscribed",
                            table);
                    }


                }
                if (combovasplans.Contains("RAT"))
                {
                    StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertSubscribed.htm"));
                    string table = reader.ReadToEnd();
                    reader.Dispose();
                    table = table.Replace("[NAME]", ordermaster.Organization.Name);
                    table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                    table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                    table = table.Replace("[MOBILE]", organizationMobile);
                    table = table.Replace("[EMAILID]", (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "Not Available"));
                    table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                    table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                    table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                    table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "Special Discount<span style='COLOR:red;'>*</span>@ 25% : " + orderdetail.Amount * 25 / 100 : ""));
                    table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                    table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span style='COLOR:red;'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                    table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                    table = table.Replace("[ACTUAL_AMOUNT]", orderdetail.Amount.ToString());
                    table = table.Replace("[DURATION]", orderdetail.ValidityDays.ToString());
                    table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());
                    table = table.Replace("[ACTUAL_AMOUNT]", orderdetail.Amount.ToString());
                    table = table.Replace("[RESUMES_COUNT]", orderdetail.ValidityCount.ToString());
                    table = table.Replace("[PLAN]", orderdetail.PlanName);
                    table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                    table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                    table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                    if (Plan == "RATCOMBO")
                    {
                        table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + orderdetail.ValidityDays.ToString() + " days.");
                    }
                    else
                    {
                        table = table.Replace("[COMBO_TEXT]", "");
                    }

                    if (ordermaster.Organization.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                               ordermaster.Organization.Email,
                                Plan + " - subscribed",
                                table);
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                       Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.EmployerSupport,
                        Plan + " - subscribed",
                        table);

                    /*Mail send to Subscribed User by admin*/
                    if (subscribedByAdmin != null)
                    {

                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            subscribedByAdmin.Email,
                             Plan + " - subscribed",
                            table);
                    }

                    /*Mail send to Who registered this Employer by admin*/
                    if (registeredByAdmin != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            registeredByAdmin.Email,
                             Plan + " - subscribed",
                            table);
                    }
                }

                if (organizationMobile != null)
                {
                    SmsHelper.SendSecondarySms(Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SubscribePlan
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[DESCRIPTION]", orderdetail.VasPlan.Description)
                        .Replace("[PLAN]", orderdetail.PlanName.ToString())
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString()),
                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        organizationMobile
                        );
                }

            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = Plan + " - subscribed"
            });
        }


        //******************************
        //Subscribing Plans(Saving the Details
        //**********************************

        [HttpPost]
       public ActionResult Subscribed(string Plan, string Amount, string VasType, string DiscountAmount)
       {

            VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(Plan);
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            OrderDetail orderHors = null;
            /**********Create an Order in Order Master Table*************/
            OrderMaster ordermaster = new OrderMaster();
            ordermaster.OrderDate = Constants.CurrentTime();

            if (LoggedInOrganization != null)
            {
                ordermaster.OrganizationId = LoggedInOrganization.Id;
                ordermaster.SubscribedBy = LoggedInOrganization.Name;
            }
            else if (Session["LoginAs"] == "EmployerViaAdmin")
            {
                int adminOrganizationId = (int)Session["empId"];
                ordermaster.OrganizationId = adminOrganizationId;
               
            }
            else if (Session["LoginAs"] == "ConsultantViaAdmin")
            {
                string consultantId = Session["consultantId"].ToString();
                ordermaster.ConsultantId = Convert.ToInt32(consultantId);
            }

            else if (LoggedInConsultant != null)
            {
                ordermaster.ConsultantId = LoggedInConsultant.Id;
                ordermaster.SubscribedBy = LoggedInConsultant.Name;
            }

            else
            {
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Time Expired. Try again",
                    ReturnUrl = "/Admin/AdminHome/AddEmployer"
                });
            }

            if (Session["LoginUser"] != null)
            {
                var subscribedby = Session["LoginUser"].ToString();
                ordermaster.SubscribedBy = subscribedby;
            }

            if (user != null)
            {
                ordermaster.SubscribedBy = user.UserName;
            }
        
            ordermaster.Amount = vasplan.Amount;
            ordermaster.PaymentStatus = false;
            _vasRepository.AddOrderMaster(ordermaster);
            _vasRepository.Save();

           
            /**********End: Create an Order in Order Master Table*************/

            System.Web.HttpContext.Current.Session["VasOrderNo"] = ordermaster.OrderId;

            /**********Start: Create an Order in Order Details Table*************/

            OrderDetail orderdetail = new OrderDetail();
            orderdetail.OrderId = ordermaster.OrderId;
            orderdetail.PlanId = vasplan.PlanId;
            if (VasType.Contains("basic"))
            {
                orderdetail.PlanName = "E-BasicRATHORS";
            }
            else
            {
                orderdetail.PlanName = vasplan.PlanName;
            }
            orderdetail.Amount = Convert.ToInt32(Amount);

            if (DiscountAmount != null)
            {
                orderdetail.DiscountAmount = Convert.ToInt32(DiscountAmount);
            }
            
            orderdetail.Amount = vasplan.Amount;
            orderdetail.ValidityCount = vasplan.ValidityCount;
           
            
            if (VasType.Contains("basic"))
            {
                if (Plan == "E-Basic")
                {
                    orderdetail.RemainingCount = 25;
                    orderdetail.BasicCount = 25;
                }

                else if (Plan == "E-Basic Plus")
                {
                    orderdetail.RemainingCount = 50;
                    orderdetail.BasicCount = 50;
                }

                else if (Plan == "E-Economy")
                {
                    orderdetail.RemainingCount = 75;
                    orderdetail.BasicCount = 100;
                }
                else if (Plan == "E-Ideal")
                {
                    orderdetail.RemainingCount = 125;
                    orderdetail.BasicCount = 500;
                }
                else if (Plan == "E-Saver")
                {
                    orderdetail.RemainingCount = 1000;
                    orderdetail.BasicCount = 500;
                }
            }
            else
            {
                orderdetail.RemainingCount = vasplan.ValidityCount;
            }
            orderdetail.Vacancies = vasplan.Vacancies;
            orderdetail.ValidityDays = vasplan.ValidityDays;
            orderdetail.EmailRemainingCount = vasplan.EmailCount;
            
            if (vasplan.PlanName == "HORSCOMBO")
            {
                orderdetail.TeleConference = 300;
            }

            else if (vasplan.PlanName == "RATCOMBO")
            {
                orderdetail.TeleConference = 300;
            }

            else if (vasplan.PlanName == "SS")
            {
                orderdetail.TeleConference = 0;
            }
            else if (vasplan.PlanName == "E-Basic")
            {
                orderdetail.EmailRemainingCount = 100;
            }

            /*Developer Note: For SMS plan there is no validity in Vasplans table. So while activate the order, the validity till value is same
             as activation date. So i am taking validity of HORS to activate. Check activatevas under _vasrepository
            */
            if (ordermaster.Organization != null)
            {
                orderHors = _vasRepository.GetOrderDetailsForHORS(Convert.ToInt32(ordermaster.OrganizationId));
            }
            else
            {
                orderHors = _vasRepository.GetConsultantOrderDetailsHors(Convert.ToInt32(ordermaster.ConsultantId));
            }

            if (orderHors != null)
            {
                if (orderdetail.PlanName.Contains("SMS"))
                {
                    orderdetail.ValidityDays = orderHors.VasPlan.ValidityDays;
                }
            }

            _vasRepository.AddOrderDetail(orderdetail);
            _vasRepository.Save();

            /**********End: Create an Order in Order Details Table*************/

            string organizationEmail = null;
            string organizationMobile = null;

            if (LoggedInConsultant != null)
            {
                organizationEmail = ordermaster.Consultante.Email;
            }

            if (ordermaster.Organization!=null)
            {
                if (ordermaster.Organization.Email != null)
                {
                    organizationEmail = ordermaster.Organization.Email;
                }
            }
            else
            {
                organizationEmail = null;
            }

            if (ordermaster.Organization != null)
            {
                if (ordermaster.Organization.MobileNumber != "")
                {
                    organizationMobile = ordermaster.Organization.MobileNumber;
                }
            }
            else
            {
                organizationMobile = null;
            }

            var registeredBy = string.Empty;
            User registeredByAdmin = null;
            if (ordermaster.Organization != null)
            {
                registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Organization.Id, EntryType.Employer);
            }
            User subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
            if (registeredBy != "")
            {
                registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
            }

            if (VasType.Contains("Consultant Plans"))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/RCSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : "Not Available"));
                table = table.Replace("[NAME]", (ordermaster.Consultante.Name != null ? ordermaster.Consultante.Name : "Not Available"));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante.ContactPerson != null ? ordermaster.Consultante.ContactPerson : "Not Available"));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }

                /***Order Details***/
                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DURATION]", orderdetail.ValidityDays.ToString());
                table = table.Replace("[RESUMES_COUNT]", orderdetail.ValidityCount.ToString());//for resume alert from validity count
                table = table.Replace("[VALIDITY_COUNT]", orderdetail.BasicCount.ToString()); // for hot resumes from basic count
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);

                /***Payment Details***/
                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);


                /*Mail send to Employer*/

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            LoggedInConsultant.Email,
                             vasplan.PlanName,
                            table);
                }


                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                    Constants.EmailSender.VasEmailId,
                    Constants.EmailSender.EmployerSupport,
                     vasplan.PlanName,
                    table);


                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                         vasplan.PlanName,
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                         vasplan.PlanName,
                        table);
                }     
            }


            if (VasType.Contains("basic"))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BasicPlanSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                /***Personal Details***/
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante!=null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                /***Order Details***/
                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DURATION]", orderdetail.ValidityDays.ToString());
                table = table.Replace("[RESUMES_COUNT]", orderdetail.ValidityCount.ToString());//for resume alert from validity count
                table = table.Replace("[VALIDITY_COUNT]", orderdetail.BasicCount.ToString()); // for hot resumes from basic count
                table = table.Replace("[VACANCIES]",orderdetail.Vacancies.ToString());
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);

                /***Payment Details***/
                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);


                /*Mail send to Employer*/
                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            organizationEmail,
                            vasplan.PlanName + " Subscribed",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            LoggedInConsultant.Email,
                             vasplan.PlanName + " Subscribed",
                            table);
                }


                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                    Constants.EmailSender.VasEmailId,
                    Constants.EmailSender.EmployerSupport,
                      vasplan.PlanName + " Subscribed",
                    table);

                
                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                     vasplan.PlanName + " Subscribed",
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                          vasplan.PlanName + " Subscribed",
                        table);
                }         

            }


            if (VasType == "BackgroundChecks")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BackgroundCheckSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", vasplan.Description.ToLower() == "Academic record check".ToLower() ? "21" : "14");
                if (DiscountAmount != null)
                {
                    table = table.Replace("[DISCOUNT_AMOUNT]", "Discount Amount: "+ orderdetail.DiscountAmount.ToString());
                }
                table = table.Replace("[REQUIREDDOCUMENTS]", vasplan.Description.ToLower() == "Residence check".ToLower() || vasplan.Description.ToLower() == "Criminal record check".ToLower() ? Constants.DocumentsDetailsForBC.AddressCriminalcheck : vasplan.Description.ToLower() == "Prior employment check".ToLower() ? Constants.DocumentsDetailsForBC.Employment : vasplan.Description.ToLower() == "Character ref check".ToLower() ? Constants.DocumentsDetailsForBC.Reference : vasplan.Description.ToLower() == "Academic record check".ToLower() ? Constants.DocumentsDetailsForBC.Education : "");
               
                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);

                /*Mail send to Employer*/
                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            organizationEmail,
                            //Constants.EmailSender.EmployerSupport,
                            //"ganesan@dial4jobz.com",
                            "BC- Subscribed",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            LoggedInConsultant.Email,
                        //Constants.EmailSender.EmployerSupport,
                        //"ganesan@dial4jobz.com",
                            "BC- Subscribed",
                            table);
                }

                
                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                    Constants.EmailSender.VasEmailId,
                    Constants.EmailSender.EmployerSupport,
                    "BC- Subscribed",
                    table);

                

                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                        "BC- Subscribed",
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null) 
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                        "BC- Subscribed",
                        table);
                }

                
            }

            else if (VasType == "Hot Resumes")
            {
              
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/HotResumesSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.ToString());
                table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "Special Discount<span class='red'>*</span>@ 25% : " + orderdetail.Amount * 25 / 100  : ""));
                table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span style='COLOR:red;'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : vasplan.Amount.ToString()));
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[VALIDITY_COUNT]", vasplan.ValidityCount.ToString());
               // table = table.Replace("[RESUMES]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[DURATION]", vasplan.ValidityDays.ToString());
                table = table.Replace("[EMAIL_COUNT]", orderdetail.EmailRemainingCount.Value.ToString());


                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                if (Plan == "HORSCOMBO")
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + vasplan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           organizationEmail,
                            Plan + " - subscribed",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           LoggedInConsultant.Email,
                            Plan + " - subscribed",
                            table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                     Constants.EmailSender.VasEmailId,
                     Constants.EmailSender.EmployerSupport,
                    Plan + " - subscribed",
                     table);


                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                       Plan + " - subscribed",
                        table);
                }

                                
            }

            else if (VasType == "Resume Alert")
            {

                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : vasplan.Amount.ToString()));
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "Special Discount<span style='COLOR:red;'>*</span>@ 25% : " + orderdetail.Amount * 25 / 100 : ""));
                table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span style='COLOR:red;'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                table = table.Replace("[DURATION]", vasplan.ValidityDays.ToString());
                table = table.Replace("[VACANCIES]", vasplan.Vacancies.ToString());
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());
                table = table.Replace("[RESUMES_COUNT]", vasplan.ValidityCount.ToString());

                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());
                if (Plan == "RATCOMBO")
                {
                    table = table.Replace("[COMBO_TEXT]", "Dial4Jobz will initiate Teleconference between you &" + orderdetail.ValidityCount.ToString() + "Suitable candidates in a span of " + vasplan.ValidityDays.ToString() + " days.");
                }
                else
                {
                    table = table.Replace("[COMBO_TEXT]", "");
                }

                if (organizationEmail!=null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           organizationEmail,
                            Plan + " - subscribed",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           LoggedInConsultant.Email,
                            Plan + " - subscribed",
                            table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                      Constants.EmailSender.VasEmailId,
                      Constants.EmailSender.EmployerSupport,
                      Plan + " - subscribed",
                      table);


                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                        Plan + " - subscribed",
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                       Plan + " - subscribed",
                        table);
                }

            }


            else if (VasType == "SpotSelection")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SpotSelectionSubscribed.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()));
                table = table.Replace("[NAME]", (ordermaster.Consultante != null ? ordermaster.Consultante.Name : ordermaster.Organization.Name));
                table = table.Replace("[CONTACTPERSON]", (ordermaster.Consultante != null ? ordermaster.Consultante.ContactPerson : ordermaster.Organization.ContactPerson));
                if (ordermaster.Consultante != null)
                {
                    table = table.Replace("[EMAILID]", ordermaster.Consultante.Email);
                    table = table.Replace("[MOBILE]", ordermaster.Consultante.MobileNumber);
                }
                else
                {
                    table = table.Replace("[EMAILID]", (organizationEmail != null ? organizationEmail : ""));
                    table = table.Replace("[MOBILE]", (organizationMobile != null ? organizationMobile : ""));
                }

              
                table = table.Replace("[ORDERNO]", ordermaster.OrderId.ToString());
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : vasplan.Amount.ToString()));
                table = table.Replace("[SPECIAL_DISCOUNT]", (orderdetail.DiscountAmount != null ? "<b>Special Discount<span class='red'>*</span>@ 25% :</b>" + orderdetail.Amount * 25 / 100 + "<b>(25% Of Above Amount)</b>" : ""));
                table = table.Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Amount after Discount: " + orderdetail.DiscountAmount.ToString() : ""));
                table = table.Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[ACTUAL_AMOUNT]", vasplan.Amount.ToString());


                table = table.Replace("[DISCOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "<span class='red'>*</span> <b>Special Offer Price applicable only on realization of payment within 3 working days from today.</b>" : ""));
                table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                table = table.Replace("[LINK_NAME]", "CLICK HERE TO PAY");
                table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString());

                if (organizationEmail!=null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           organizationEmail,                           
                            "SS - Subscribed",
                            table);
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                           LoggedInConsultant.Email,
                            Plan + " - subscribed",
                            table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                       Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.EmployerSupport,
                       Plan + " - subscribed",
                       table);


                /*Mail send to Subscribed User by admin*/
                if (subscribedByAdmin != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        subscribedByAdmin.Email,
                         "SS - Subscribed",
                        table);
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        registeredByAdmin.Email,
                      "SS - Subscribed",
                        table);
                }

            }
            else if (VasType == "SMSPurchase")
            {
                if (organizationEmail!=null)
                {
                    EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        organizationEmail,
                        Constants.EmailSubject.SMSSubscription,
                        Constants.EmailBody.SMSSubscription

                        .Replace("[ID]", (ordermaster.Consultante!=null ? ordermaster.Consultante.Id.ToString() :ordermaster.Organization.Id.ToString()))
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", (organizationEmail != null ? organizationEmail : "Not Available"))
                        .Replace("[MOBILE_NO]", (organizationMobile!=null? organizationMobile : ""))
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]",  vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]",(orderdetail.DiscountAmount!=null? "Discount Amount: "+orderdetail.DiscountAmount.ToString(): ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                     
                      
                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd (Dial4jobz) is not responsible for false information given by the candidate.")
                        .Replace("[LOGIN LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/EmployerVas/DashBoard")
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                       
                        .Replace("[LINK_NAME]", "CLICK HERE TO PAY")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        );
                }

                if (LoggedInConsultant != null)
                {
                    EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        LoggedInConsultant.Email,
                        Constants.EmailSubject.SMSSubscription,
                        Constants.EmailBody.SMSSubscription

                      .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", (organizationEmail != null ? organizationEmail : "Not Available"))
                        .Replace("[MOBILE_NO]", (organizationMobile != null ? organizationMobile : ""))
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))


                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd (Dial4jobz) is not responsible for false information given by the candidate.")
                        .Replace("[LOGIN LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/EmployerVas/DashBoard")
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)

                        .Replace("[LINK_NAME]", "CLICK HERE TO PAY")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                        );
                }

                
                   EmailHelper.SendEmailBCC(
                   Constants.EmailSender.EmployerSupport,
                   Constants.EmailSender.VasEmailId,
                    Constants.EmailSender.EmployerSupport,
                   Constants.EmailSubject.SMSSubscription,
                   Constants.EmailBody.SMSSubscription
                       .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", (organizationEmail != null ? organizationEmail : "Not Available"))
                        .Replace("[MOBILE_NO]", (organizationMobile != null ? organizationMobile : ""))
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))


                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                        .Replace("[LOGIN LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/EmployerVas/DashBoard")
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)

                        .Replace("[LINK_NAME]", "CLICK HERE TO PAY")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                   );
                

                if (subscribedByAdmin != null)
                {
                   EmailHelper.SendEmail(
                   Constants.EmailSender.EmployerSupport,
                   subscribedByAdmin.Email,
                   Constants.EmailSubject.SMSSubscription,
                   Constants.EmailBody.SMSSubscription
                        .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", (organizationEmail != null ? organizationEmail : "Not Available"))
                        .Replace("[MOBILE_NO]", (organizationMobile != null ? organizationMobile : ""))
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))


                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd (Dial4jobz) is not responsible for false information given by the candidate.")
                        .Replace("[LOGIN LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/EmployerVas/DashBoard")
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)

                        .Replace("[LINK_NAME]", "CLICK HERE TO PAY")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                   );
                   
                }

                /*Mail send to Who registered this Employer by admin*/
                if (registeredByAdmin != null)
                {
                   EmailHelper.SendEmail(
                   Constants.EmailSender.EmployerSupport,
                   registeredByAdmin.Email,
                   Constants.EmailSubject.SMSSubscription,
                   Constants.EmailBody.SMSSubscription
                       .Replace("[ID]", (ordermaster.Consultante != null ? ordermaster.Consultante.Id.ToString() : ordermaster.Organization.Id.ToString()))
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", (organizationEmail != null ? organizationEmail : "Not Available"))
                        .Replace("[MOBILE_NO]", (organizationMobile != null ? organizationMobile : ""))
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[PLAN]", vasplan.PlanName)
                        .Replace("[AMOUNT]", vasplan.Amount.ToString())
                        .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? "Discount Amount: " + orderdetail.DiscountAmount.ToString() : ""))
                        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))


                        .Replace("[NOTICE]", "Important Notice for Employers")
                        .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                        .Replace("[LOGIN LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/EmployerVas/DashBoard")
                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)

                        .Replace("[LINK_NAME]", "CLICK HERE TO PAY")
                        .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(orderdetail.OrderId.ToString()).ToString())
                   );

                }
                

            }

            if (organizationMobile != null)
            {
                SmsHelper.SendSecondarySms(Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.SubscribePlan
                    .Replace("[NAME]", ordermaster.Organization.Name)
                    .Replace("[DESCRIPTION]", orderdetail.VasPlan.Description)
                    .Replace("[PLAN]", orderdetail.PlanName.ToString())
                    //.Replace("[AMOUNT]", orderdetail.Amount.ToString()),
                    .Replace("[AMOUNT]",  (orderdetail.DiscountAmount!=null? "": vasplan.Amount.ToString()))
                    .Replace("[DISCOUNT_AMOUNT]",(orderdetail.DiscountAmount!=null? orderdetail.DiscountAmount.ToString() :""))
                    .Replace("[DICOUNT_TEXT]", (orderdetail.DiscountAmount!=null? "This Price is valid only for 3 days.": ""))
                    ,
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    organizationMobile
                    );
            }

            if(LoggedInConsultant!=null)
            {
                SmsHelper.SendSecondarySms(Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.SubscribePlan
                    .Replace("[NAME]", (ordermaster.Organization!=null ? ordermaster.Organization.Name : ordermaster.Consultante.Name))
                    .Replace("[DESCRIPTION]", orderdetail.VasPlan.Description)
                    .Replace("[PLAN]", orderdetail.PlanName.ToString())
                    //.Replace("[AMOUNT]", orderdetail.Amount.ToString()),
                    .Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? "" : vasplan.Amount.ToString()))
                    .Replace("[DISCOUNT_AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : ""))
                    .Replace("[DICOUNT_TEXT]", (orderdetail.DiscountAmount != null ? "This Price is valid only for 3 days." : ""))
                    ,
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    LoggedInConsultant.MobileNumber
                    );
            }

            

            return Json(new JsonActionResult
            {
                Success = true,
                Message = Plan + " - subscribed",
                ReturnUrl="/Employer/EmployerVas/Payment"
            });
        }

        
        public string getRemoteAddr()
        {
            string UserIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (UserIPAddress == null)
            {
                UserIPAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            return UserIPAddress;
        }

        public string getSecureCookie(HttpRequest Request)
        {

            HttpCookie secureCookie = Request.Cookies["vsc"];
            if (secureCookie != null)
            {
                return secureCookie.ToString();
            }
            else
            {
                return "";
            }

        }

        

        private string getSecureCookie(HttpRequestBase Request)
        {
            throw new NotImplementedException();
        }


     //*************************************************
       // Credit Card, Debit card payment(Get OrderId)
    //**************************************************
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

        
        //*************************************************
        // Credit Card, Debit card payment(ICICI)(Save the details)
        //**************************************************

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
                

                Response.Redirect("http://www.dial4jobz.in//SFAClient//TestSSl.aspx?amount=" + ordermaster.Amount + "&orderid=" + ordermaster.OrderId + "&ResponseUrl=http://www.dial4jobz.in//SFAClient//EmployerResponse.aspx", false);
            }
            return View();
        }



        //*************************************************
        // Electronic Transfer(Save  the Details)
        //**************************************************

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

            }

            else
            {
                return RedirectToAction("Payment", "EmployerVas");
            }

            return View();
        }

        [Authorize, HttpPost,HandleNonAjaxRequest]
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
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderpayment.OrderId));

                orderdetails.PaymentId = orderpayment.PaymentId;
                _vasRepository.Save();

                VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetails.PlanName);

                


                if (LoggedInOrganization != null)
                {
                    if (LoggedInOrganization.Email != null)
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.EmployerSupport,
                           LoggedInOrganization.Email,

                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeElectronicTransfer
                           .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderdetails.Amount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                           .Replace("[RESUMES]", vasplan.ValidityCount.ToString())
                           .Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                           .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                    }

                    EmailHelper.SendEmailBCC(
                       Constants.EmailSender.EmployerSupport,
                       Constants.EmailSender.VasEmailId,
                         Constants.EmailSender.EmployerSupport,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeElectronicTransfer
                           .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderdetails.Amount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                           .Replace("[RESUMES]", vasplan.ValidityCount.ToString())
                           .Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                           .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                }

                else if (Session["LoginAs"] == "EmployerViaAdmin")
                {
                    int adminOrganizationId = (int)Session["empId"];
                    var organization = _repository.GetOrganization(adminOrganizationId);

                    if (organization.Email != null)
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.EmployerSupport,
                           organization.Email,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeElectronicTransfer
                           .Replace("[NAME]", organization.ContactPerson)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderdetails.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                           .Replace("[RESUMES]", vasplan.ValidityCount.ToString())
                           .Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                           .Replace("[BANKNAME]", orderpayment.DrawnOnBank));
                    }

                    EmailHelper.SendEmailBCC(
                           Constants.EmailSender.EmployerSupport,
                          Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.EmployerSupport,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeElectronicTransfer
                           .Replace("[NAME]", organization.ContactPerson)
                           .Replace("[PLAN]", orderdetails.PlanName)
                           .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderdetails.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString())
                           .Replace("[RESUMES]", vasplan.ValidityCount.ToString())
                           .Replace("[TRANSFER_REFERENCE]", orderpayment.TransferReference)
                           .Replace("[BANKNAME]", orderpayment.DrawnOnBank));

                }
                
            }

            return RedirectToAction("Payment", "EmployerVas");
        }

        //*************************************************
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
        // CCAvenue Payment Gateway Response(Redirect to Activation of Employer)
        //**************************************************

        [HttpPost]
        public ActionResult CCAVResponse(FormCollection collection)
        {
            string workingKey = "91BE0EE8FE42847C3CA5E923E8D9752B";//put in the 32bit alpha numeric key in the quotes provided here
            string orderId=string.Empty;
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
                paymentStatus=Params["order_status"];
            }

            if (paymentStatus == "Success")
            {
                //return RedirectToAction("EmployerPaymentSuccessful", "EmployerVas", new { value = Constants.EncryptString(orderid) });
                //return RedirectToAction("EmployerPaymentSuccessful", "EmployerVas", new { value = orderid });
                Response.Redirect("http://www.dial4jobz.in/employer/employervas/EmployerPayment?value=" + Constants.EncryptString(orderid), true);
            }
                       
            return RedirectToAction("DashBoard", "EmployerVas");
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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CallUsForPickupCash(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["VasOrderNo"] != null)
            {
                OrderPayment GetOrderPayment = _vasRepository.GetOrderPayment(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderDetail orderdetail = _vasRepository.GetOrderDetail(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));
                OrderPayment orderpayment = null;
                VasPlan vasplan = null;

                if (GetOrderPayment != null)
                {
                    /*Order details table (F.K) so first i update with null value.*/
                        orderdetail.PaymentId = null;
                       _vasRepository.Save();
                    /*End Update*/

                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                    orderpayment = new OrderPayment();
                    orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                    orderpayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.PaymentMode = (int)PaymentMode.PickupCash;
                    //orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
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

                    /*Update the payment Id into order details table.*/

                    //orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));
                     orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                }
                else
                {

                    orderpayment = new OrderPayment();
                    orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                    orderpayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                    orderpayment.PaymentMode = (int)PaymentMode.PickupCash;
                    //orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                    if (orderdetail != null)
                    {
                        if (orderdetail.DiscountAmount != null)
                        {
                            orderpayment.Amount = orderdetail.DiscountAmount;
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

                    orderdetail = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));
                    orderdetail.PaymentId = orderpayment.PaymentId;
                    _vasRepository.Save();
                }

                if (orderdetail != null)
                {
                    vasplan = _vasRepository.GetVasPlanbyPlanName(orderdetail.PlanName);
                }


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

                if (LoggedInOrganization != null)
                {
                    if (LoggedInOrganization.Email != null)
                    {

                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            LoggedInOrganization.Email,
                            Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModePickupCash
                           .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                           .Replace("[PLAN]", orderdetail.PlanName)
                           .Replace("[EMPLOYER_NAME]", LoggedInOrganization.Name)
                           .Replace("[EMAIL_ID]", LoggedInOrganization.Email)
                           .Replace("[MOBILE_NO]", LoggedInOrganization.MobileNumber)
                           .Replace("[RESUMES]", orderdetail.ValidityCount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetail.OrderMaster.SubscribedBy != null ? orderdetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[MOBILE_NO]", LoggedInOrganization.MobileNumber)
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[ORDER_DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString() + "Days")
                           .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                            Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModePickupCash
                           .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                           .Replace("[PLAN]", orderdetail.PlanName)
                           .Replace("[EMPLOYER_NAME]", LoggedInOrganization.Name)
                           .Replace("[EMAIL_ID]", LoggedInOrganization.Email)
                           .Replace("[MOBILE_NO]", LoggedInOrganization.MobileNumber)
                           .Replace("[RESUMES]", orderdetail.ValidityCount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetail.OrderMaster.SubscribedBy != null ? orderdetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[MOBILE_NO]", LoggedInOrganization.MobileNumber)
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[ORDER_DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString() + "Days")
                           .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );

                }
                else if (Session["LoginAs"] == "EmployerViaAdmin")
                {
                    int adminOrganizationId = (int)Session["empId"];
                    var organization = _repository.GetOrganization(adminOrganizationId);

                    if (organization.Email != "")
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.EmployerSupport,
                           organization.Email,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModePickupCash
                           .Replace("[NAME]", organization.ContactPerson)
                           .Replace("[PLAN]", orderdetail.PlanName)
                           .Replace("[EMPLOYER_NAME]", organization.Name)
                           .Replace("[EMAIL_ID]", organization.Email)
                           .Replace("[MOBILE_NO]", organization.MobileNumber)
                           .Replace("[RESUMES]", orderdetail.ValidityCount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetail.OrderMaster.SubscribedBy != null ? orderdetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[MOBILE_NO]", organization.MobileNumber)
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[ORDER_DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString() + "Days")
                           .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );
                    }

                    EmailHelper.SendEmailBCC(
                           Constants.EmailSender.EmployerSupport,
                           Constants.EmailSender.VasEmailId,
                           Constants.EmailSender.EmployerSupport,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModePickupCash
                           .Replace("[NAME]", organization.ContactPerson)
                           .Replace("[PLAN]", orderdetail.PlanName)
                           .Replace("[EMPLOYER_NAME]", organization.Name)
                           .Replace("[EMAIL_ID]", organization.Email)
                           .Replace("[MOBILE_NO]", organization.MobileNumber)
                           .Replace("[RESUMES]", orderdetail.ValidityCount.ToString())
                           .Replace("[SUBSCRIBED_BY]", (orderdetail.OrderMaster.SubscribedBy != null ? orderdetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                           .Replace("[MOBILE_NO]", organization.MobileNumber)
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[ORDER_DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                           .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                           .Replace("[ORDER_NUMBER]", orderdetail.OrderId.ToString())
                           .Replace("[VALIDITY]", vasplan.ValidityDays.ToString() + "Days")
                           .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                           .Replace("[BRANCH]", orderpayment.Branch)
                           .Replace("[AREA]", orderpayment.Regions)
                           );
                }


            }
            return RedirectToAction("Payment", "EmployerVas");
        }

        public ActionResult PayThroughPhoneCreditCard()
        {
            return View();
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
                OrderDetail orderdetails = _vasRepository.GetOrderDetail(Convert.ToInt32(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString()));

                /*Order details table (F.K) so first i update with null value.*/
                if (orderdetails != null)
                {
                    orderdetails.PaymentId = null;
                    _vasRepository.Save();
                }
                /*End Update*/

                orderpayment.OrderId = int.Parse(System.Web.HttpContext.Current.Session["VasOrderNo"].ToString());
                if (orderdetails.DiscountAmount != null)
                {
                    orderpayment.Amount = orderdetails.DiscountAmount;
                }
                else
                {
                    orderpayment.Amount = Convert.ToDouble(collection["Amount"]);
                }

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
                    _vasRepository.AddOrderPayment(orderpayment);
                    _vasRepository.Save();

                    orderdetails = _vasRepository.GetOrderDetails(Convert.ToInt32(orderpayment.OrderId));
                    if (orderdetails != null)
                    {
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

                if (LoggedInOrganization != null)
                {

                    if (LoggedInOrganization.Email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                            LoggedInOrganization.Email,
                            Constants.EmailSubject.PaymentDetails,
                            Constants.EmailBody.PaymentModeDepositChequeDraft
                               .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                               .Replace("[EMPLOYER_NAME]", LoggedInOrganization.Name)
                               .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[RESUMES]", orderdetails.ValidityCount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[INSTRUMENT_TYPE]", paymentmode)
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                               .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                               .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                               .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                               .Replace("[CHEQUE_NUMBER]",(orderpayment.ChequeNumber!=null? "Cheque/DD Number:"+ orderpayment.ChequeNumber : "Transfer Reference: "+orderpayment.TransferReference)));
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                           Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            Constants.EmailSubject.PaymentDetails,
                            Constants.EmailBody.PaymentModeDepositChequeDraft
                               .Replace("[NAME]", LoggedInOrganization.ContactPerson)
                               .Replace("[EMPLOYER_NAME]", LoggedInOrganization.Name)
                               .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[RESUMES]", orderdetails.ValidityCount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[INSTRUMENT_TYPE]", paymentmode)
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                               .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                               .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                               .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                               .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));

                }
                else if (Session["LoginAs"] == "EmployerViaAdmin")
                {
                    int adminOrganizationId = (int)Session["empId"];
                    var organization = _repository.GetOrganization(adminOrganizationId);

                    if (organization.Email != null)
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.EmployerSupport,
                           organization.Email,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeDepositChequeDraft
                               .Replace("[NAME]", organization.ContactPerson)
                               .Replace("[EMPLOYER_NAME]", organization.Name)
                                .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[RESUMES]", orderdetails.ValidityCount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[INSTRUMENT_TYPE]", paymentmode)
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                               .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                               .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                               .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                               .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                             
                    }

                    EmailHelper.SendEmailBCC(
                           Constants.EmailSender.EmployerSupport,
                           Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                           Constants.EmailSubject.PaymentDetails,
                           Constants.EmailBody.PaymentModeDepositChequeDraft
                               .Replace("[NAME]", organization.ContactPerson)
                               .Replace("[EMPLOYER_NAME]", organization.Name)
                                .Replace("[SUBSCRIBED_BY]", (orderdetails.OrderMaster.SubscribedBy != null ? orderdetails.OrderMaster.SubscribedBy : "Not Avaliable"))
                               .Replace("[PLAN]", orderdetails.PlanName)
                               .Replace("[RESUMES]", orderdetails.ValidityCount.ToString())
                               .Replace("[ORDER_NUMBER]", orderdetails.OrderId.ToString())
                               .Replace("[ORDER_DATE]", orderdetails.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                               .Replace("[INSTRUMENT_TYPE]", paymentmode)
                               .Replace("[AMOUNT]", orderpayment.Amount.ToString())
                               .Replace("[BRANCH]", (orderpayment.Branch != null ? orderpayment.Branch : ""))
                               .Replace("[VALIDITY]", (vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() : ""))
                               .Replace("[DEPOSITED_ON]", (orderpayment.DepositedOn.Value != null ? "Deposited On: " + orderpayment.DepositedOn.Value.ToString("dd-MM-yyyy") : "Transferred On: " + orderpayment.TransferDate.Value.ToString("dd-MM-yyyy")))
                               .Replace("[BANK_NAME]", orderpayment.DrawnOnBank)
                               .Replace("[CHEQUE_NUMBER]", (orderpayment.ChequeNumber != null ? "Cheque/DD Number:" + orderpayment.ChequeNumber : "Transfer Reference: " + orderpayment.TransferReference)));
                }

            }
          
            return RedirectToAction("Payment", "EmployerVas");
        }


        public string button { get; set; }
    }
}
