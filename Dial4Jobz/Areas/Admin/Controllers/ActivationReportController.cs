using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Controllers;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Helpers;
using System.IO;
using System.Net;
using System.Data.Objects;


namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class ActivationReportController : Controller
    {
        //
        // GET: /Admin/ActivationReport/

        VasRepository _vasRepository = new VasRepository();
        Repository _repository = new Repository();
        UserRepository _userRepository = new UserRepository();

        public ActionResult Index()
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Permission permission = new Permission();
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                string pageAccess = "";
                string[] Page_Code = null;
                foreach (var page in pageaccess)
                {
                    permission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = permission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + permission.Name + ",";
                    }
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ActivationReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "AdminHome");
                }
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }

        /*Developer Note: If payment details are already entered it will fetch this action and activate. 
         * Otherwise it will go to the SavePaymentDetails.(Passed via Ajax Call)*/
        //[HttpPost]
        public ActionResult GetOrderDetails(int orderId)
        {
            OrderPayment orderpayment = _vasRepository.GetOrderPayment(orderId);
            OrderMaster successPayment = _vasRepository.GetSuccessPayment(orderId);
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            string ordernos = Session["OrderNumberList"].ToString();

            if (orderpayment != null && successPayment!=null)
            {
                _vasRepository.ActivateVAS(Convert.ToInt32(orderpayment.OrderId));
                SendActivationMail(Convert.ToInt32(orderpayment.OrderId));
                OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderpayment.OrderId));
                ordermaster.ActivatedBy = user.UserName;
                _vasRepository.Save();
                Response.Write("<script language=javascript>alert('Plan activated for the selected Order');</script>");
            }

            return RedirectToAction("SavePaymentDetails", "ActivationReport", new { lstOrderIds = ordernos });
                       
        }

        public ActionResult SavePaymentDetails(string lstOrderIds)
        {
            if (!string.IsNullOrEmpty(lstOrderIds))
            {
                var orders = lstOrderIds.Split(',').Select(n => int.Parse(n)).ToList();
                ViewData["OrderIds"] = new SelectList(orders);
                IEnumerable<OrderDetail> orderAmounts = _vasRepository.GetAmountFromOrders(orders);
                ViewData["Amount"] = new SelectList(orderAmounts.Select(op => op.Amount));
                Session["OrderNumberList"] = lstOrderIds;
                foreach (var orderNo in orders)
                {
                    
                    OrderPayment orderpayment = _vasRepository.GetOrderPayment(Convert.ToInt32(orderNo));
                    if (orderpayment != null)
                    {
                        if (orderpayment.PaymentMode == 5)
                        {
                            ViewData["City"] = orderpayment.Branch;
                            ViewData["Region"] = orderpayment.Regions;
                        }

                        else if (orderpayment.PaymentMode == 1 || orderpayment.PaymentMode==2)
                        {
                            ViewData["DepositedOn"] = Convert.ToDateTime(orderpayment.PaymentDate);
                        }
                    }
                }
                
            }

            return View();
        }


        [HttpPost]
        public ActionResult SavePaymentDetails(FormCollection collection)
        {
            string orderid = collection["OrderIds"];
            string ordernos = Session["OrderNumberList"].ToString();
            string paymentmode = string.Empty;

            OrderDetail orderDetail = _vasRepository.GetOrderDetail(Convert.ToInt32(orderid));
            OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderid));
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            OrderPayment getOrderPayment =_vasRepository.GetOrderPayment(Convert.ToInt32(orderid));
            VasPlan vasplan = _vasRepository.GetVasPlanByPlanId(Convert.ToInt32(orderDetail.PlanId));
            OrderPayment orderPayment = new OrderPayment();

            string candidateEmail = null;

            if (ordermaster.Candidate != null)
            {
                if (ordermaster.Candidate.Email != "")
                {
                    candidateEmail = ordermaster.Candidate.Email;
                }
                else
                {
                    candidateEmail = null;
                }
            }

            /*Order details table (F.K) so first i update with null value.*/
            if (orderDetail != null)
            {
                orderDetail.PaymentId = null;
                _vasRepository.Save();
            }
            /*End Update*/
            
            if (!string.IsNullOrEmpty(collection["OrderIds"]))
                orderPayment.OrderId = Convert.ToInt32(orderid);

            if (orderDetail != null)
            {
                
                if (orderDetail.DiscountAmount != null)
                {
                    orderPayment.Amount = orderDetail.DiscountAmount;
                }
                else
                {
                    orderPayment.Amount = orderDetail.Amount;
                }
            }

            if (collection["ddlInstrumentType"] == "0")
            {
                paymentmode = "Cheque";
                orderPayment.PaymentMode = (int)PaymentMode.Cheque;
                orderPayment.ChequeNumber = collection["ReferenceNumber"];
                //orderPayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                orderPayment.DepositedOn = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.DrawnOnBank = collection["BankName"];
                orderPayment.Branch = collection["BankBranch"];
               
            }

            else if (collection["ddlInstrumentType"] == "1")
            {
                paymentmode = "Demand Draft";
                orderPayment.PaymentMode = (int)PaymentMode.DemandDraft;
                orderPayment.ChequeNumber = collection["ReferenceNumber"];
                orderPayment.DrawnOnBank = collection["BankName"];
                orderPayment.DepositedOn = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.Branch = collection["BankBranch"];
            }

            else if (collection["ddlInstrumentType"] == "2")
            {
                paymentmode = "Cash Deposit";
                orderPayment.PaymentMode = (int)PaymentMode.CashDeposit;
                orderPayment.DepositedOn = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.Branch = collection["BankBranch"];

            }

            else if (collection["ddlInstrumentType"] == "3")
            {
                paymentmode = "Inter Bank";
                orderPayment.PaymentMode = (int)PaymentMode.InterBank;
                orderPayment.TransferDate = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.TransferReference = collection["TransferReference"];
            }

            else if (collection["ddlInstrumentType"] == "4")
            {
                paymentmode = "NEFT";
                orderPayment.PaymentMode = (int)PaymentMode.NEFT;
                orderPayment.TransferReference = collection["TransferReference"];
                orderPayment.TransferDate = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.DrawnOnBank = collection["BankName"];
            }

            else if (collection["ddlInstrumentType"] == "5")
            {
                paymentmode = "IMPS";
                orderPayment.PaymentMode = (int)PaymentMode.IMPS;
                orderPayment.TransferDate = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.TransferReference = collection["TransferReference"];
            }

            else if (collection["ddlInstrumentType"] == "6")
            {
                paymentmode = "Cash Pickup";
                orderPayment.PaymentMode = (int)PaymentMode.PickupCash;
                //orderPayment.DepositedOn = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.CollectedOn = Convert.ToDateTime(collection["CollectedOn"]);
                orderPayment.CollectedBy = collection["CollectedBy"];
                orderPayment.Branch = collection["City"];
                orderPayment.Regions = collection["Region"];
            }

            else if (collection["ddlInstrumentType"] == "7")
            {
                paymentmode = "Cash Collected At our Office";
                orderPayment.PaymentMode = (int)PaymentMode.CollectAtOffice;
                //orderPayment.DepositedOn = Convert.ToDateTime(collection["DepositedOn"]);
                orderPayment.CollectedOn = Convert.ToDateTime(collection["CollectedOn"]);
                orderPayment.CollectedBy = collection["CollectedBy"];
                orderPayment.Branch = collection["City"];
                orderPayment.Regions = collection["Region"];
            }


                if (getOrderPayment != null)
                {
                    //Developer Note: I take orderno using Session. But Because of session timeout it is showing an error. SO i ll take from getorderpayment***/
                    _vasRepository.DeleteOrderPaymentDetails(Convert.ToInt32(getOrderPayment.OrderId));
                    _vasRepository.AddOrderPayment(orderPayment);
                    _vasRepository.Save();

                    if (orderDetail != null)
                    {
                        orderDetail.PaymentId = orderPayment.PaymentId;
                        _vasRepository.Save();
                    }
                }

                else
                {
                    _vasRepository.AddOrderPayment(orderPayment);
                    _vasRepository.Save();

                    if (orderDetail != null)
                    {
                        orderDetail = _vasRepository.GetOrderDetails(Convert.ToInt32(orderPayment.OrderId));
                        orderDetail.PaymentId = orderPayment.PaymentId;
                        _vasRepository.Save();
                    }
                }

                if (orderDetail.OrderMaster.PaymentStatus == false)
                {
                    _vasRepository.ActivateVAS(Convert.ToInt32(orderid));
                    SendActivationMail(Convert.ToInt32(orderid));
                    ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderid));
                    ordermaster.ActivatedBy = user.UserName;
                    _vasRepository.Save();
                }


                if (ordermaster.Candidate != null)
                {
                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                              Constants.EmailSender.VasEmailId,
                               Constants.EmailSender.CandidateSupport,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                  .Replace("[NAME]", ordermaster.Candidate.Name)
                                  .Replace("[PLAN]", vasplan.PlanName)
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                  .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                                  .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))
                                  .Replace("[USER]", "Candidate")
                                  .Replace("[EMAIL_ID]", "mailto:smo@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smo@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  );
                    if (candidateEmail != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                               candidateEmail,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                  .Replace("[NAME]", ordermaster.Candidate.Name)
                                  .Replace("[PLAN]", vasplan.PlanName)
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                   .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                            // .Replace("[BRANCH]", (orderPayment.Branch != null ? orderPayment.Branch + "," : ""))
                                  .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))
                                  .Replace("[USER]", "Candidate")
                                  .Replace("[EMAIL_ID]", "mailto:smo@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smo@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  );
                    }
                }

                else if (ordermaster.Organization != null)
                {
                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                               Constants.EmailSender.VasEmailId,
                               Constants.EmailSender.EmployerSupport,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                 .Replace("[NAME]", ordermaster.Organization.Name)
                                    .Replace("[PLAN]", vasplan.PlanName.ToString())
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                  .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                        //.Replace("[BRANCH]", (orderPayment.Branch != null ? orderPayment.Branch + "," : ""))
                                   .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (ordermaster.Organization.Email != null ? ordermaster.Organization.Email : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Organization.MobileNumber != null ? ordermaster.Organization.MobileNumber : ""))
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  .Replace("[USER]", "Employer")
                                  .Replace("[EMAIL_ID]", "mailto:smc@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smc@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                                  );


                    if (ordermaster.Organization.Email != "")
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                               ordermaster.Organization.Email,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                 .Replace("[NAME]", ordermaster.Organization.Name)
                                .Replace("[PLAN]", vasplan.PlanName.ToString())
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                  .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                            //.Replace("[BRANCH]", (orderPayment.Branch != null ? orderPayment.Branch + "," : ""))
                                   .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (ordermaster.Organization.Email != null ? ordermaster.Organization.Email : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Organization.MobileNumber != null ? ordermaster.Organization.MobileNumber : ""))
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  .Replace("[USER]", "Employer")
                                  .Replace("[EMAIL_ID]", "mailto:smc@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smc@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")

                                  );
                    }

                }

            //By consultants

                else if (ordermaster.Consultante != null)
                {
                    EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                               Constants.EmailSender.VasEmailId,
                               Constants.EmailSender.EmployerSupport,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                  .Replace("[NAME]", ordermaster.Consultante.Name + " (Consultant)")
                                  .Replace("[PLAN]", vasplan.PlanName.ToString())
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                  .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                                  .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (ordermaster.Consultante.Email != null ? ordermaster.Consultante.Email : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Consultante.MobileNumber != null ? ordermaster.Consultante.MobileNumber : ""))
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  .Replace("[USER]", "Employer")
                                  .Replace("[EMAIL_ID]", "mailto:smc@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smc@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                                  );

                    if (ordermaster.Consultante.Email != "")
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                               ordermaster.Consultante.Email,
                               Constants.EmailSubject.PaymentDetails,
                               Constants.EmailBody.PaymentDetails
                                  .Replace("[NAME]", ordermaster.Consultante.Name + " (Consultant)")
                                  .Replace("[PLAN]", vasplan.PlanName.ToString())
                                  .Replace("[ORDER_DATE]", orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"))
                                  .Replace("[VALIDITY]", vasplan.ValidityDays != null ? vasplan.ValidityDays.ToString() + " Days" : "Contact Us")
                                  .Replace("[ORDER_NUMBER]", orderDetail.OrderId.ToString())
                                  .Replace("[VALIDITY_COUNT]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() + " Alerts" : ""))
                                  .Replace("[PAYMENT_DATE]", (orderPayment.DepositedOn != null ? orderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (orderPayment.TransferDate != null ? orderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : "")))
                                  .Replace("[COLLECTED_DATE]", (orderPayment.CollectedOn != null ? orderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : ""))
                                  .Replace("[COLLECTED_BY]", (orderPayment.CollectedBy != null ? orderPayment.CollectedBy : ""))
                                  .Replace("[AMOUNT]", orderPayment.Amount.ToString())
                                  .Replace("[CHEQUEDRAFT]", (orderPayment.PaymentMode == 3 || orderPayment.PaymentMode == 4 ? "Cheque/DD Number " + "<b>" + orderPayment.ChequeNumber + "</b>" : ""))
                                  .Replace("[TRANSFER_REFERENCE]", (orderPayment.TransferReference != null ? "Transfer Reference Number: " + "<b>" + orderPayment.TransferReference + "</b>" : ""))
                            //.Replace("[BRANCH]", (orderPayment.Branch != null ? orderPayment.Branch + "," : ""))
                                  .Replace("[BRANCH]", (orderPayment.Branch != null ? "Branch: " + "<b>" + orderPayment.Branch + "</b>" + "," : ""))
                                  .Replace("[AREA]", (orderPayment.Regions != null ? orderPayment.Regions : ""))
                                  .Replace("[BANK_NAME]", (orderPayment.DrawnOnBank != null ? "Bank Name: " + "<b>" + orderPayment.DrawnOnBank + "</b>" : ""))
                                  .Replace("[SUBSCRIBED_BY]", (orderDetail.OrderMaster.SubscribedBy != null ? orderDetail.OrderMaster.SubscribedBy : "Not Avaliable"))
                                  .Replace("[ALERTS]", (orderDetail.ValidityCount != null ? orderDetail.ValidityCount.ToString() : ""))
                                  .Replace("[EMAIL_ID]", (ordermaster.Consultante.Email != null ? ordermaster.Consultante.Email : ""))
                                  .Replace("[CONTACT_NUMBER]", (ordermaster.Consultante.MobileNumber != null ? ordermaster.Consultante.MobileNumber : ""))
                                  .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                                  .Replace("[USER]", "Employer")
                                  .Replace("[EMAIL_ID]", "mailto:smc@dial4jobz.com")
                                  .Replace("[MAILNAME]", "smc@dial4jobz.com")
                                  .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")

                                  );
                    }
                }


            Response.Write("<script language=javascript>alert('Plan activated for the selected Order');</script>");
            return RedirectToAction("SavePaymentDetails", "ActivationReport", new { lstOrderIds = ordernos });
        }

       
        [HttpPost]
        public JsonResult ActivateVas(string orderIds)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user.UserName == "admin123" || user.UserName == "Vivek2014" || user.UserName == "Mani2014")
            {
                if (!string.IsNullOrEmpty(orderIds))
                {
                    string[] ids = orderIds.Split(',');
                    if (ids != null && ids.Length > 0)
                    {
                        foreach (string id in ids)
                        {
                            var result = new
                            {
                                error = "3",
                                message = "Check the Payment Details Before Activate"
                            };
                            return Json(result, JsonRequestBehavior.AllowGet);

                        }

                    }

                    else
                    {
                        var result1 = new
                        {
                            error = "1",
                            message = "Unable to activate"
                        };
                        return Json(result1, JsonRequestBehavior.AllowGet);
                    }

                }
                var result2 = new
                {
                    error = "2",
                    message = "Plan Activated Successfully"
                };
                return Json(result2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result2 = new
                {
                    error = "4",
                    message = "You are not supposed to do this action"
                };
                return Json(result2, JsonRequestBehavior.AllowGet);
            }
        }

        


        public ActionResult DeleteOrder(int OrderId = 0)
        {
            if (OrderId != 0)
            {
               var orderpayment =_vasRepository.GetOrderPayment(OrderId);
               _vasRepository.DeleteOrderDetails(OrderId);

                if (orderpayment != null)
                {
                    _vasRepository.DeleteOrderPaymentDetails(OrderId);
                }

                _vasRepository.DeleteOrderMaster(OrderId);

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Order has been deleted Successfully"
                });

            }

            else
            {
                return Json(new JsonActionResult
                {
                    Success = false,
                    Message = "Order is not Deleted"
                });
            }
        }

        public void SendActivationMail(int OrderId)
        {
            OrderDetail orderdetail = _vasRepository.GetOrderDetail(OrderId);
            OrderPayment orderpayment = _vasRepository.GetOrderPayment(OrderId);
            OrderMaster ordermaster = _vasRepository.GetOrderMaster(OrderId);
            VasPlan vasplan = _vasRepository.GetVasPlanByPlanId(Convert.ToInt32(orderdetail.PlanId));

            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            var candidateRegisteredBy = "";
            var registeredBy = "";
            string organizationEmail = null;
            string candidateEmail = null;
            string subscribeEmail = null;
            string registeredEmail = null;

            if (ordermaster.Consultante != null)
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


            if (ordermaster.Candidate != null)
            {
                candidateRegisteredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Candidate.Id, EntryType.Candidate);
                if (ordermaster.Candidate.Email != "")
                {
                    candidateEmail = ordermaster.Candidate.Email;
                }
                else
                {
                    candidateEmail = null;
                }
            }

            User subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
            User registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
            User candidateRegByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(candidateRegisteredBy).FirstOrDefault();

            if (subscribedByAdmin != null)
            {
                subscribeEmail = subscribedByAdmin.Email;
            }

            if (registeredByAdmin != null)
            {
                registeredEmail = registeredByAdmin.Email;
            }
            
            string paymentmode="";
           
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

            /**********************COMBO PLANS TEMPLATES START HERE*****************/

            if (orderdetail.VasPlan.Description.ToLower() == "CRDPurchase".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/CRDActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", ordermaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : ""));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
               // table = table.Replace("[AMOUNT]", orderdetail.OrderMaster.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY]", orderdetail.VasPlan.ValidityDays.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.ValidityCount.ToString());
                
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                

                if (candidateEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
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

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

            }

            if (orderdetail.PlanName.Contains("HORS") && orderdetail.Amount == 219 || orderdetail.Amount==336)
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/HotResumesActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                
                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[EMAILID]", organizationEmail != null ? organizationEmail : "");
                table = table.Replace("[MOBILE]", ordermaster.Organization.MobileNumber);
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
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

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.EmployerSupport,
                             orderdetail.PlanName + " - Activated",
                             table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }
            }

            else if (orderdetail.PlanName.Contains("RAT") && orderdetail.Amount == 219 || orderdetail.Amount==336)
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[EMAILID]", organizationEmail != null ? organizationEmail : "");
                table = table.Replace("[MOBILE]", ordermaster.Organization.MobileNumber);

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", "COMBO RAT");
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DURATION]", vasplan.ValidityDays.ToString());
                table = table.Replace("[VALIDITY]", orderdetail.ValidityCount.ToString());
                table = table.Replace("[VACANCIES]", orderdetail.Vacancies.ToString());

                table = table.Replace("[PAYMENT_MODE]", paymentmode!=null ? paymentmode : "");
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

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                            Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.EmployerSupport,
                            orderdetail.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

            }

            /**********************COMBO PLANS TEMPLATES END HERE*****************/


             /**********************COMMON PLANS(EMPLOYER)*****************/

            else if (orderdetail.VasPlan.Description.ToLower() == "Resume Alert".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/ResumeAlertActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[EMAILID]", organizationEmail != null ? organizationEmail : "");
                table = table.Replace("[MOBILE]", ordermaster.Organization.MobileNumber);

               
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
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

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                        Constants.EmailSender.VasEmailId,
                          Constants.EmailSender.EmployerSupport,
                        orderdetail.PlanName + " - Activated",
                        table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
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
                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[EMAILID]", organizationEmail != null ? organizationEmail : "");
                table = table.Replace("[MOBILE]", ordermaster.Organization.MobileNumber);

              
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
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

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                          Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            orderdetail.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
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
                table = table.Replace("[NAME]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Name : orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[ID]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Id.ToString() : orderdetail.OrderMaster.Candidate.Id.ToString());
                table = table.Replace("[EMAILID]", (ordermaster.Organization != null ? (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "") : (ordermaster.Candidate.Email != "" ? ordermaster.Candidate.Email : "")));
                table = table.Replace("[MOBILE]", (ordermaster.Organization != null ? (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "") : (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : "")));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
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

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                     candidateEmail,
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
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
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
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[MOBILE]", orderdetail.OrderMaster.Organization.MobileNumber);
                table = table.Replace("[EMAILID]", organizationEmail != null ? organizationEmail : "");

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
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
                    orderdetail.VasPlan.PlanName + " - Activated",
                     table);

                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                          Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.EmployerSupport,
                            orderdetail.VasPlan.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
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

                table = table.Replace("[ID]", ordermaster.Organization.Id.ToString());
                table = table.Replace("[NAME]", ordermaster.Organization.Name);
                table = table.Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson);
                table = table.Replace("[MOBILE]", ordermaster.Organization.MobileNumber);
                table = table.Replace("[EMAIL_ID]", organizationEmail != null ? organizationEmail : "");
                
                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[FROMDATE]", (orderdetail.ActivationDate!=null ?orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"): ""));
                table = table.Replace("[TODATE]", (orderdetail.ValidityTill!=null? orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"): ""));
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
                
                EmailHelper.SendEmailBCC(Constants.EmailSender.EmployerSupport,
                         Constants.EmailSender.VasEmailId,
                           Constants.EmailSender.EmployerSupport,
                           "SS - Activated",
                           table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }
            }

            else if (orderdetail.VasPlan.Description.ToLower() == "SMSPurchase".ToLower())
            {
                /*Send mail to candidate (SMS Purchase)*/
                if (ordermaster.Candidate != null)
                {
                    if (candidateEmail != null)
                    {
                        EmailHelper.SendEmail(
                            Constants.EmailSender.CandidateSupport,
                            candidateEmail,
                            Constants.EmailSubject.SMSActivated,
                            Constants.EmailBody.SMSActivated
                            .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                            .Replace("[NAME]", ordermaster.Candidate.Name)
                            .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"))
                            .Replace("[MOBILE_NO]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))

                            .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                            .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
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
                         .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                         .Replace("[NAME]", ordermaster.Candidate.Name)
                         .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"))
                         .Replace("[MOBILE_NO]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))

                         .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                         .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                         .Replace("[PLAN]", orderdetail.PlanName)
                         .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                         .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                         .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))

                         .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                         .Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy)

                         .Replace("[NOTICE]", "Important Notice for Candidates")
                         .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                    );

                    if (subscribeEmail != null)
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.CandidateSupport,
                        subscribeEmail,
                        Constants.EmailSubject.SMSActivated,
                        Constants.EmailBody.SMSActivated
                            .Replace("[ID]", ordermaster.Candidate.ToString())
                            .Replace("[NAME]", ordermaster.Candidate.Name)
                            .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"))
                            .Replace("[MOBILE_NO]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))

                            .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                            .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
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

                    if (registeredEmail != null)
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.CandidateSupport,
                        registeredEmail,
                        Constants.EmailSubject.SMSActivated,
                        Constants.EmailBody.SMSActivated
                            .Replace("[ID]", ordermaster.Candidate.Id.ToString())
                            .Replace("[NAME]", ordermaster.Candidate.Name)
                            .Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"))
                            .Replace("[MOBILE_NO]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""))

                            .Replace("[ORDER_NO]", orderdetail.OrderMaster.OrderId.ToString())
                            .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
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

                /***************Sms Activate For Employer*********************/
                if (ordermaster.Organization != null)
                {
                    if (organizationEmail != null )
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                       organizationEmail,
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

                }

            }

              /*Send Free SMS activated mail to Employer.*/

            else if (orderdetail.VasPlan.Description.ToLower() == "Free Sms Purchase".ToLower())
            {
                /*This Mail template is for Organization tracking*/
                EmailHelper.SendEmailBCC(
                       Constants.EmailSender.EmployerSupport,
                        Constants.EmailSender.VasEmailId,
                        Constants.EmailSender.EmployerSupport,
                       Constants.EmailSubject.SMSActivated,
                       Constants.EmailBody.SMSActivatedFree
                        .Replace("[ID]", ordermaster.Organization.Id.ToString())
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", organizationEmail != null ? organizationEmail : "")
                        .Replace("[MOBILE_NO]", ordermaster.Organization.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[DATE]", (orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                       );

                if (organizationEmail != null)
                {
                    EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        organizationEmail,
                        Constants.EmailSubject.SMSActivated,
                        Constants.EmailBody.SMSActivatedFree
                        .Replace("[ID]", ordermaster.Organization.Id.ToString())
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", organizationEmail != null ? organizationEmail : "")
                        .Replace("[MOBILE_NO]", ordermaster.Organization.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[DATE]", (orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                        );
                }

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(
                    Constants.EmailSender.EmployerSupport,
                    subscribeEmail,
                    Constants.EmailSubject.SMSActivated,
                    Constants.EmailBody.SMSActivatedFree
                        .Replace("[ID]", ordermaster.Organization.Id.ToString())
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", organizationEmail != null ? organizationEmail : "")
                        .Replace("[MOBILE_NO]", ordermaster.Organization.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[DATE]", (orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                    );
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(
                    Constants.EmailSender.EmployerSupport,
                    registeredEmail,
                    Constants.EmailSubject.SMSActivated,
                    Constants.EmailBody.SMSActivatedFree
                        .Replace("[ID]", ordermaster.Organization.Id.ToString())
                        .Replace("[NAME]", ordermaster.Organization.Name)
                        .Replace("[EMAIL_ID]", organizationEmail != null ? organizationEmail : "")
                        .Replace("[MOBILE_NO]", ordermaster.Organization.MobileNumber)
                        .Replace("[CONTACTPERSON]", ordermaster.Organization.ContactPerson)

                        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                        .Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null ? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""))
                        .Replace("[PLAN]", orderdetail.PlanName)
                        .Replace("[SMS]", orderdetail.ValidityCount.ToString())
                        .Replace("[AMOUNT]", orderdetail.Amount.ToString())
                        .Replace("[DATE]", (orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""))

                        .Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy)
                        .Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""))
                    );
                }
            }

            //*********** Started Candidates Vas Activation**************//


            if (orderdetail.VasPlan.Description.ToLower() == "CRDPurchase".ToLower())
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/CRDActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();

                table = table.Replace("[ID]", orderdetail.OrderMaster.Candidate.Id.ToString());
                table = table.Replace("[NAME]", orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[EMAIL_ID]", (candidateEmail != "" ? candidateEmail : "Not Available"));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : ""));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", vasplan.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));

                table = table.Replace("[VALIDITY]", orderdetail.VasPlan.ValidityDays.ToString());
                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);
                table = table.Replace("[VACANCIES]", orderdetail.ValidityCount.ToString());

                if (candidateEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
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

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
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
                table = table.Replace("[NAME]", (orderdetail.OrderMaster != null ? orderdetail.OrderMaster.Candidate.Name : ""));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[VASTYPE]", orderdetail.PlanName == "KER" ? "Regular" : "Express");
                table = table.Replace("[DELIVERYDAYS]", orderdetail.PlanName == "KER" ? "5" : "2");
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                     candidateEmail,
                      orderdetail.PlanName + " - Activated",
                      table);


                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                             Constants.EmailSender.VasEmailId,
                             Constants.EmailSender.CandidateSupport,
                             orderdetail.PlanName + " - Activated",
                             table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
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
                table = table.Replace("[MOBILE]", (orderdetail.OrderMaster.Candidate.ContactNumber != null ? orderdetail.OrderMaster.Candidate.ContactNumber : ""));
                table = table.Replace("[EMAILID]", (orderdetail.OrderMaster != null ? candidateEmail : ""));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", orderdetail.VasPlan.Description == "ResumeWriting" ? "7" : "4");

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      candidateEmail,
                      orderdetail.PlanName + " - Activated",
                      table);

                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                    Constants.EmailSender.VasEmailId,
                       Constants.EmailSender.CandidateSupport,
                        orderdetail.PlanName + " - Activated",
                        table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
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
                table = table.Replace("[EMAIL_ID]", (candidateEmail != null ? candidateEmail : "Not Available"));
                table = table.Replace("[MOBILE]", (ordermaster.Candidate.ContactNumber != null ? ordermaster.Candidate.ContactNumber : ""));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[FROMDATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[TODATE]", orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy"));

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (candidateEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
                      orderdetail.PlanName + " - Activated",
                      table);
                }

                EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                         Constants.EmailSender.VasEmailId,
                            Constants.EmailSender.CandidateSupport,
                            orderdetail.PlanName + " - Activated",
                            table);

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
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
                table = table.Replace("[EMAIL_ID]", (candidateEmail != "" ? candidateEmail : "Not Available"));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", orderdetail.PlanName);
                table = table.Replace("[AMOUNT]", (orderdetail.DiscountAmount != null ? orderdetail.DiscountAmount.ToString() : orderdetail.Amount.ToString()));
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[VALIDITY]", orderdetail.VasPlan.ValidityDays.ToString());

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (candidateEmail != null)
                {

                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                    candidateEmail,
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

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

            }

            else if (orderdetail.VasPlan.Description.ToLower().Contains("BGC".ToLower()))
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/BackgroundCheckActivated.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[NAME]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Name : orderdetail.OrderMaster.Candidate.Name);
                table = table.Replace("[ID]", orderdetail.OrderMaster.OrganizationId != null ? orderdetail.OrderMaster.Organization.Id.ToString() : orderdetail.OrderMaster.Candidate.Id.ToString());
                table = table.Replace("[EMAILID]", (ordermaster.Organization != null ? (ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "") : (candidateEmail != "" ? candidateEmail : "")));
                table = table.Replace("[MOBILE]", (ordermaster.Organization != null ? (ordermaster.Organization.MobileNumber != "" ? ordermaster.Organization.MobileNumber : "") : (ordermaster.Candidate.ContactNumber != "" ? ordermaster.Candidate.ContactNumber : "")));

                table = table.Replace("[ORDERNO]", orderdetail.OrderId.ToString());
                table = table.Replace("[INVOICE_NO]", (orderdetail.OrderMaster.Invoice!=null? orderdetail.OrderMaster.Invoice.InvoiceNo.ToString(): ""));
                table = table.Replace("[PLAN]", orderdetail.VasPlan.PlanName);
                table = table.Replace("[AMOUNT]", orderdetail.Amount.ToString());
                table = table.Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                table = table.Replace("[DELIVERYDAYS]", orderdetail.VasPlan.Description.ToLower() == "Academic record check".ToLower() ? "21" : "14");

                table = table.Replace("[PAYMENT_MODE]", (paymentmode != "" ? paymentmode.ToString() : ""));
                table = table.Replace("[SUBSCRIBED_BY]", orderdetail.OrderMaster.SubscribedBy);

                if (candidateEmail != null)
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

                if (subscribeEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      subscribeEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }

                if (registeredEmail != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                      registeredEmail,
                       vasplan.PlanName + " - Activated",
                       table);
                }
            }

            if (ordermaster.Organization != null)
            {
                if (ordermaster.Organization.MobileNumber != null)
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSReceiptofPayment
                          .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                          .Replace("[AMOUNT]", ordermaster.Amount.ToString())
                          .Replace("[NAME]", ordermaster.Organization.Name)
                          .Replace("[PLANNAME]", orderdetail.PlanName),
                          Constants.SmsSender.SecondaryType,
                          Constants.SmsSender.Secondarysource,
                          Constants.SmsSender.Secondarydlr,
                          ordermaster.Organization.MobileNumber
                          );
                }

            }

            if (ordermaster.Candidate != null)
            {
                SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSReceiptofPayment
                          .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
                          .Replace("[AMOUNT]", ordermaster.Amount.ToString())
                          .Replace("[NAME]", ordermaster.Candidate.Name)
                          .Replace("[PLANNAME]", orderdetail.PlanName),
                          Constants.SmsSender.SecondaryType,
                          Constants.SmsSender.Secondarysource,
                          Constants.SmsSender.Secondarydlr,
                          ordermaster.Candidate.ContactNumber
                          );
            }
        }

        /*Employer Activated Report*/
        public ActionResult ActivatedReport()
        {
            //****To add the user report menu into Edit*****
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }

        }

       

        public string GetSumofAmount()
        {
            var order = _vasRepository.GetOrderMasters().Where(od => od.PaymentStatus == false);
            int sum = 0;

            foreach (var amount in order)
            {
                sum += (int)amount.Amount;
            }
            return sum.ToString();
        }
                      

       
        public JsonResult ListActivationEmployerReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderMaster.PaymentStatus == false);


            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Organization.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.OrderMaster.Organization.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Amount);
                    else
                        return query.OrderBy(rslt => rslt.OrderId);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.Organization.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()) );

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrderDate != null && o.OrderMaster.OrderDate >= from && o.OrderMaster.OrderDate <= to);

            }

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { o.OrderMaster.OrganizationId.ToString(), o.OrderMaster.Organization.Name, (o.PlanName.Contains("Basic") ? o.VasPlan.PlanName : o.PlanName), o.OrderId, (o.OrderMaster.OrderDate != null) ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : "", o.Amount, (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), "<a href='JavaScript:void(0)' onclick='DeleteOrder(" + o.OrderId + ")'>Delete</a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='ActivateOrder' value='" + o.OrderId + "' />" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListActivatedEmployers(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderMaster.PaymentStatus == true);
            IQueryable<OrderDetail> orderpayment = _vasRepository.GetOrderDetails().Where(od => od.OrderId == od.OrderPayment.OrderId);


            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Organization.Name);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.ActivationDate);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderPayment.PaymentMode);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else if (iSortCol_0 == 7)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderId);
                    else
                        return query.OrderByDescending(rslt => rslt.ActivationDate);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Organization.Name);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.ActivationDate);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderPayment.PaymentMode);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else if (iSortCol_0 == 7)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.ActivatedBy);
                    else
                        return query.OrderByDescending(rslt => rslt.ActivationDate);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrganizationId.ToString().Contains(sSearch.ToLower().Trim()) || o.OrderMaster.Organization.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.ActivationDate != null && o.ActivationDate >= from && o.ActivationDate <= to);

            }
            

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();
                       

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { (o.OrderMaster.Organization.Name != null) ? o.OrderMaster.Organization.Name.ToString() : "", o.OrderId, (o.OrderMaster.OrderDate != null) ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : "", o.ActivationDate != null ? o.ActivationDate.Value.ToString("dd-MM-yyyy") : "", Enum.GetName(typeof(PaymentMode), o.OrderPayment != null ? ((PaymentMode)o.OrderPayment.PaymentMode) : 0), (o.OrderPayment != null && o.OrderPayment.CollectedBy != null ? o.OrderPayment.CollectedBy : ""), (o.OrderPayment != null && o.OrderPayment.CollectedOn != null ? o.OrderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.DepositedOn != null ? o.OrderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.TransferDate != null ? o.OrderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : ""))), (o.PlanName.Contains("Basic") ? o.VasPlan.PlanName : o.PlanName), o.Amount, (_repository.GetAdminUserNamebyEntryIdAndEntryType(o.OrderMaster.Organization.Id, EntryType.Employer)), (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), o.OrderMaster.ActivatedBy != null ? o.OrderMaster.ActivatedBy : "" })
                
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CandidateActivatedReport()
        {

            //****To add the user report menu into Edit*****
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }


        public JsonResult ListCandidateActivatedReport(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.CandidateId != null && od.OrderMaster.PaymentStatus == true);


            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Candidate.UserName);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderByDescending(rslt => rslt.ActivationDate);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.OrderMaster.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.OrderMaster.Candidate.UserName);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Amount);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderBy(rslt => rslt.ActivationDate);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.Candidate.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrderDate != null && o.OrderMaster.OrderDate >= from && o.OrderMaster.OrderDate <= to);

            }

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { (o.OrderMaster.Candidate.Name != null ? o.OrderMaster.Candidate.Name : ""), (o.OrderId != null ? o.OrderId : 0), (o.OrderMaster.OrderDate != null ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : ""), (o.ActivationDate != null) ? o.ActivationDate.Value.ToString("dd-MM-yyyy") : "", Enum.GetName(typeof(PaymentMode), o.OrderPayment != null ? ((PaymentMode)o.OrderPayment.PaymentMode) : 0), (o.OrderPayment != null && o.OrderPayment.CollectedBy != null ? o.OrderPayment.CollectedBy : ""), (o.OrderPayment != null && o.OrderPayment.CollectedOn != null ? o.OrderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.DepositedOn != null ? o.OrderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.TransferDate != null ? o.OrderPayment.TransferDate.Value.ToString("dd-MM-yyyy"):""))), o.VasPlan.PlanName, o.Amount, (_repository.GetAdminUserNamebyEntryIdAndEntryType(o.OrderMaster.Candidate.Id, EntryType.Candidate)), (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), (o.OrderMaster.ActivatedBy != null ? o.OrderMaster.ActivatedBy : "") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CandidateVasActivation()
        {
            return View();
        }

        public JsonResult ListActivationCandidateReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.CandidateId != null && od.OrderMaster.PaymentStatus == false);

            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Candidate.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.OrderMaster.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.OrderMaster.Candidate.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Amount);
                    else
                        return query.OrderBy(rslt => rslt.OrderId);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.CandidateId.ToString().Contains(sSearch.ToLower().Trim()) || o.OrderMaster.Candidate.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()) || o.OrderId.ToString().Contains(sSearch.ToLower().Trim()) || o.Amount.ToString().ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrderDate != null && o.OrderMaster.OrderDate >= from && o.OrderMaster.OrderDate <= to);

            }

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { o.OrderMaster.CandidateId.ToString(), o.OrderMaster.Candidate.Name, o.VasPlan.PlanName, o.OrderId, (o.OrderMaster.OrderDate != null) ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : "", o.Amount, (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), "<a href='JavaScript:void(0)' onclick='DeleteOrder(" + o.OrderId + ")'>Delete</a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='ActivateOrder' value='" + o.OrderId + "' />" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultantActivation()
        {
            return View();
        }

        public JsonResult ListActivationConsultantReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.ConsultantId != null && od.OrderMaster.PaymentStatus == false);

            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.ConsultantId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Consultante.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.OrderMaster.ConsultantId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.OrderMaster.Consultante.Name);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Amount);
                    else
                        return query.OrderBy(rslt => rslt.OrderId);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.ConsultantId.ToString().Contains(sSearch.ToLower().Trim()) || o.OrderMaster.Consultante.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()) || o.OrderId.ToString().Contains(sSearch.ToLower().Trim()) || o.Amount.ToString().ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrderDate != null && o.OrderMaster.OrderDate >= from && o.OrderMaster.OrderDate <= to);

            }

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { o.OrderMaster.ConsultantId.ToString(), o.OrderMaster.Consultante.Name, o.VasPlan.PlanName, o.OrderId, (o.OrderMaster.OrderDate != null) ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : "", o.Amount, (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), "<a href='JavaScript:void(0)' onclick='DeleteOrder(" + o.OrderId + ")'>Delete</a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='ActivateOrder' value='" + o.OrderId + "' />" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultantActivatedReport()
        {
            //****To add the user report menu into Edit*****
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }


        public JsonResult ListConsultantActivatedReport(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<OrderDetail> ordermasterResult = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.ConsultantId != null && od.OrderMaster.PaymentStatus == true);

            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.ConsultantId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.Consultante.UserName);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Amount);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderByDescending(rslt => rslt.ActivationDate);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.OrderMaster.ConsultantId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.OrderMaster.Consultante.UserName);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.PlanName);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.OrderMaster.OrderDate);
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Amount);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderBy(rslt => rslt.ActivationDate);

                }

            };

            ordermasterResult = orderingFunc(ordermasterResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.Consultante.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || o.PlanName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                ordermasterResult = ordermasterResult.Where(o => o.OrderMaster.OrderDate != null && o.OrderMaster.OrderDate >= from && o.OrderMaster.OrderDate <= to);

            }

            IEnumerable<OrderDetail> ordermasterResult1 = ordermasterResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = ordermasterResult.Count(),
                iTotalDisplayRecords = ordermasterResult.Count(),
                aaData = ordermasterResult1.Select(o => new object[] { (o.OrderMaster.Consultante.Name != null ? o.OrderMaster.Consultante.Name : ""), (o.OrderId != null ? o.OrderId : 0), (o.OrderMaster.OrderDate != null ? o.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : ""), (o.ActivationDate != null) ? o.ActivationDate.Value.ToString("dd-MM-yyyy") : "", Enum.GetName(typeof(PaymentMode), o.OrderPayment != null ? ((PaymentMode)o.OrderPayment.PaymentMode) : 0), (o.OrderPayment != null && o.OrderPayment.CollectedBy != null ? o.OrderPayment.CollectedBy : ""), (o.OrderPayment != null && o.OrderPayment.CollectedOn != null ? o.OrderPayment.CollectedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.DepositedOn != null ? o.OrderPayment.DepositedOn.Value.ToString("dd-MM-yyyy") : (o.OrderPayment != null && o.OrderPayment.TransferDate != null ? o.OrderPayment.TransferDate.Value.ToString("dd-MM-yyyy") : ""))), o.VasPlan.PlanName, o.Amount, (_repository.GetAdminUserNamebyEntryIdAndEntryType(o.OrderMaster.Consultante.Id, EntryType.Candidate)), (o.OrderMaster.SubscribedBy != null ? o.OrderMaster.SubscribedBy : ""), (o.OrderMaster.ActivatedBy != null ? o.OrderMaster.ActivatedBy : "") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}
