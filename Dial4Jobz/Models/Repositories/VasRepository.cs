using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Enums;
using System.Data.Objects;
using System.Collections;
using Dial4Jobz.Helpers;
using System.Globalization;

namespace Dial4Jobz.Models.Repositories
{
    public class VasRepository : IVasRepository
    {
        private Dial4JobzEntities _db = new Dial4JobzEntities();

        //DateTime currentdate = Constants.CurrentTime().Date;
        DateTime currentdate = Constants.CurrentTime().Date;

        #region IVasRepository Members

        #region Consultant Methods

        public IQueryable<PostedJobAlert> GetRATAlertJobsForConsultant(int consultantId, int orderId)
        {
            return _db.PostedJobAlerts.Where(j => j.ConsultantId == consultantId && j.OrderId == orderId);
        }

        /*Check Consultant Active Plans*/
        public IEnumerable<OrderDetail> GetActivatedPlansByConsultant(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.Contains("RC".ToLower()) && od.OrderMaster.PaymentStatus == true  && od.ValidityTill >= currentdate && od.RemainingCount > 0);
        }
        public bool GetHorsSubscribedByConsultant(int consultantId)
        {
            var HorsSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.ConsultantId == consultantId && od.PlanName.Contains("HORS") && od.OrderId == om.OrderId && om.PaymentStatus == true
                                 //&& od.ValidityTill >= currentdate && od.RemainingCount > 0
                                 select om;

            var horsCount = HorsSubscribed.FirstOrDefault();
            if (horsCount != null)
                return true;
            else
                return false;
        }

        public int UpdateConsultantCount(int consultantId, int candidateId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("RC") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);

            int resumesCount;
            int orderNo;
            if (order.Count() > 0)
                orderNo = (int)order.Select(o => o.OrderId).FirstOrDefault();
            else 
                return 0;

            if (order.Count() > 0)
                resumesCount = (int)order.Select(o => o.RemainingCount).FirstOrDefault();
            else
                return 0;

            if (resumesCount != 0 && resumesCount != 9999)
            {
                resumesCount = resumesCount - 1;
            }

            OrderDetail orderdetail = GetOrderDetails(orderNo);
            orderdetail.RemainingCount = resumesCount;
            Save();

            return resumesCount;
                   
        }

        /*****Check Remaining Count for Consultants*******/
        public int CheckForHorsRemainingCountConsultant(int consultantId, int candidateId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);
            var basicOrder = _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("Basic") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);

            int resumescount;
            int basiccount = 0;
            int orderNo;
            if (order.Count() > 0)
                orderNo = (int)order.Select(o => o.OrderId).FirstOrDefault();
            else
                return 0;

            if (order.Count() > 0)
                resumescount = (int)order.Select(o => o.RemainingCount).FirstOrDefault();

            else
                return 0;

            if (basicOrder.Count() > 0)
                basiccount = (int)basicOrder.Select(o => o.BasicCount).FirstOrDefault();

            //candidate contact details seen or not update

            var alreadySeenorNot = from CVL in _db.ContactsViewedLogs
                                   join op in _db.OrderMasters on CVL.ConsultantId equals op.ConsultantId
                                   join od in _db.OrderDetails on op.OrderId equals od.OrderId
                                   //join erd in _db.EmployerRegistrationDetails on CVL.EmployerId equals erd.EmployerId
                                   where CVL.ConsultantId == consultantId && CVL.CandidateId == candidateId && CVL.DateViewed >= od.ActivationDate
                                        && CVL.DateViewed <= od.ValidityTill && od.PlanName.Contains("HORS")
                                   select CVL.CandidateId;

            if (alreadySeenorNot.Count() > 0)
                resumescount = 9999;

            if (resumescount != 0 && resumescount != 9999)
            {
                resumescount = resumescount - 1;

                OrderDetail orderdetail = GetOrderDetails(orderNo);
                if (basicOrder.Count() > 0)
                {
                    orderdetail.BasicCount = orderdetail.BasicCount - 1;
                    Save();
                }
                else
                {
                    orderdetail.RemainingCount = resumescount;
                    Save();
                }

                var contactsViewed = from cvc in _db.ContactsViewedCounts where cvc.ConsultantId == consultantId select cvc;

                if (contactsViewed.Count() > 0)
                {
                    foreach (var cvc in contactsViewed)
                    {
                        cvc.ContactsViewed = cvc.ContactsViewed + 1;
                    }
                }
                else
                {
                    ContactsViewedCount objcvc = new ContactsViewedCount
                    {
                        ConsultantId = consultantId,
                        ContactsViewed = 1
                    };
                    _db.ContactsViewedCounts.AddObject(objcvc);
                }

                ContactsViewedLog cvl = new ContactsViewedLog
                {
                    ConsultantId = consultantId,
                    CandidateId = candidateId,
                    DateViewed = currentdate,
                    OrderId = orderNo
                };
                _db.ContactsViewedLogs.AddObject(cvl);

                Save();
            }
            if (basiccount != 0)
                return basiccount;
            else
                return resumescount;
        }



        public bool PlanSubscribedForConsultant(int consultantId)
        {
            var plansubscribe = from om in _db.OrderMasters
                                join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                where om.ConsultantId == consultantId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate
                                select om;
            var planSubscribeCount = plansubscribe.FirstOrDefault();
            if (planSubscribeCount != null)
                return true;
            else
                return false;
        }

        /*Plan Activated for SS as a Consultant*/
        public bool PlanActivatedSSConsultant(int consultantId)
        {
            var planSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.ConsultantId == consultantId && om.OrderId == od.OrderId && od.ActivationDate != null && om.PaymentStatus == true && od.PlanName.Contains("SS".ToLower())
                                 //&& od.ValidityTill >= currentdate
                                 select om;
            var subscribedCount = planSubscribed.FirstOrDefault();
            if (subscribedCount != null)
                return true;
            else
                return false;
        }


        public OrderDetail RATPlanActivatedDetails(int consultantId)
        {
            var activatedplan = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.ConsultantId == consultantId && od.OrderId == om.OrderId && od.PlanName.Contains("RAT") && om.PaymentStatus == true && od.ValidityTill >= currentdate
                                select od;

            var activate = activatedplan.FirstOrDefault();
            return activate;
        }

        /*HORS Consultant Count**/

        public int? GetConsultantEmailCount(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true).Sum(od => (int?)od.EmailRemainingCount);
        }

        public OrderDetail UpdateConsultantEmailCount(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.EmailRemainingCount > 0 && od.ValidityTill.Value >= currentdate).FirstOrDefault();
        }


        public OrderDetail GetValidityRATConsultant(int consultantId)
        {
            return _db.OrderDetails.FirstOrDefault(o => o.OrderMaster.ConsultantId == consultantId && o.ValidityTill >= currentdate);
        }


        /*By Consultant to get Hors Count*/
        public int? GetHorsConsultantCount(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).Sum(od => (int?)od.RemainingCount);
        }

        public int? GetSmsVasCountConsultant(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.ToLower().Contains("SMS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0).Sum(od => (int?)od.RemainingCount);
        }

        public OrderDetail GetConsultantSmsVas(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId && od.PlanName.ToLower().Contains("SMS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0).FirstOrDefault();
        }

       

        public IEnumerable<PostedJobAlert> GetMatchingOrganizationtoAlert(List<int> lstJobIds)
        {
            List<PostedJobAlert> jobs = new List<PostedJobAlert>();
            List<PostedJobAlert> jobsconsult = new List<PostedJobAlert>();

            var lstEmployers = from lstjobid in lstJobIds
                               join pjd in _db.PostedJobAlerts on lstjobid equals pjd.JobId
                               where pjd.ActivatedJobStatus == true && pjd.OrganizationId != null
                               select pjd;

            var lstConsultants = from lstjobid in lstJobIds
                                 join pjd in _db.PostedJobAlerts on lstjobid equals pjd.JobId
                                 where pjd.ActivatedJobStatus == true && pjd.ConsultantId != null
                                 select pjd;

            foreach (var consultant in lstConsultants)
            {
                jobsconsult.Add(new PostedJobAlert() { ConsultantId = consultant.ConsultantId, JobId = consultant.JobId });
            }

            foreach (var emp in lstEmployers)
            {
                jobs.Add(new PostedJobAlert() { OrganizationId = emp.OrganizationId, JobId = emp.JobId });
            }

            if (jobsconsult.Count() > 0)
                return jobsconsult;
            else
                return jobs;
        }

        public PostedJobAlert GetConsultantPostedAlert(int consultantId, int jobId)
        {
            return _db.PostedJobAlerts.FirstOrDefault(c => c.ConsultantId == consultantId && c.JobId == jobId);
        }

        public void UpdateVASDetailsConsultant(int consultantId, int jobIdPosted)
        {
            var registeredPlan = from erd in _db.OrderMasters
                                 from od in _db.OrderDetails
                                 where erd.ConsultantId.Value == consultantId && erd.OrderId == od.OrderId && od.PlanName.Contains("RAT") && erd.PaymentStatus.Value == true
                                 select erd;

            var orderDetails = registeredPlan.FirstOrDefault();


            PostedJobAlert objPJD = new PostedJobAlert
            {
                ConsultantId = consultantId,
                JobId = jobIdPosted,
                OrderId = orderDetails.OrderId,
                ActivatedJobStatus = true
            };
            _db.PostedJobAlerts.AddObject(objPJD);
            Save();
        }

        /*Get Alert from Posted job alert by ConsultantId*/

        public IQueryable<PostedJobAlert> GetJobsByConsultantIdAlert(int id)
        {
            return _db.PostedJobAlerts.Where(j => j.ConsultantId == id);
        }
                 
        #endregion

        #region VasActivation

        //By Jawahar-----------Sms Purchase-------------

        public IEnumerable<OrderDetail> GetOrderDetailsbyOrgId(int OrgId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == OrgId).OrderByDescending(ord => ord.OrderId);
            //var pendingOrder = _db.OrderMasters.Where(om => om.OrganizationId == organizationId && om.PaymentStatus == false);
        }

        public IEnumerable<OrderDetail> GetOrderDetailsbyConsultantId(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId == consultantId).OrderByDescending(ord => ord.OrderId);
        }

        public IEnumerable<OrderDetail> GetActivatedPlansList(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.OrderMaster.PaymentStatus == true && currentdate >= od.ValidityTill && od.ValidityTill >= currentdate);
        }

        public IEnumerable<OrderDetail> GetOrderDetailsbyCandidateId(int candidateId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == candidateId).OrderByDescending(ord => ord.OrderId);
        }


        public string GetMobileNo(int Id)
        {
            var mobileNo = from emp in _db.Organizations where emp.Id == Id select emp.MobileNumber;
            return mobileNo.FirstOrDefault();
        }

        public string GetEmailAddress(int Id)
        {
            var email = from emp in _db.Organizations where emp.Id == Id select emp.Email;
            return email.FirstOrDefault();
        }

 #region PlanActivation-udhaya

        //Teleconference related methods using by admin

        //update teleconference count

        public OrderDetail UpdateTeleConferenceCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORSCOMBO".ToLower()) && od.OrderMaster.PaymentStatus == true && od.TeleConference > 0 && od.ValidityTill >= currentdate).FirstOrDefault();
        }

        public OrderDetail GetTeleConference(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORSCOMBO".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0 && od.TeleConference > 0 && od.ValidityTill >= currentdate).FirstOrDefault();
        }

        public int? GetTeleConferenceCountByOrderId(int orderId)
        {
            return _db.OrderDetails.SingleOrDefault(od => od.OrderId == orderId).TeleConference;
        }

        public OrderDetail GetSpotSelectionConference(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("SS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.TeleConference >= 0).FirstOrDefault();
        }

        public OrderDetail GetSpotInterviewConference(int candidateId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == candidateId && od.PlanName.Contains("SI".ToLower()) && od.OrderMaster.PaymentStatus == true && od.TeleConference >= 0).FirstOrDefault();
        }

        public int? GetTeleConferenceCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORSCOMBO".ToLower()) && od.OrderMaster.PaymentStatus == true && od.TeleConference > 0).Sum(od => (int?)od.TeleConference);
        }

        public int? GetCountForSS(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("SS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.TeleConference > 0).Sum(od => (int?)od.TeleConference);
        }


        //End teleconference Methods
        
        
        //Hors Activation & related Methods

        //Hors plan orderid details for Employer feedback
        public OrderMaster GetHORSOrderId(int organizationId)
        {

            var HorsSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.OrganizationId == organizationId && od.PlanName.Contains("HORS") && od.OrderId == om.OrderId && om.PaymentStatus == true
                                 //&& od.ValidityTill >= currentdate && od.RemainingCount > 0
                                 select om;

            var horsCount = HorsSubscribed.FirstOrDefault();
            return horsCount;
        }

        public bool GetHORSSubscribed(int organizationId)
        {

            var HorsSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.OrganizationId == organizationId && od.PlanName.Contains("HORS") && od.OrderId == om.OrderId && om.PaymentStatus == true 
                                 select om;

            var horsCount = HorsSubscribed.FirstOrDefault();
            if (horsCount !=null)
                return true;
            else
                return false;
        }

        public OrderDetail GetOrderDetailsForHORS(int organizationId)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.OrderMaster.OrganizationId == organizationId && c.OrderId == c.OrderMaster.OrderId && c.ValidityTill >= currentdate && c.OrderMaster.PaymentStatus == true && c.PlanName.Contains("HORS"));
        }

        public OrderDetail GetConsultantOrderDetailsHors(int consultantId)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.OrderMaster.ConsultantId == consultantId && c.OrderId == c.OrderMaster.OrderId && c.ValidityTill >= currentdate && c.OrderMaster.PaymentStatus == true && c.PlanName.Contains("HORS"));
        }

        //Vignesh: To Get Feedback (getting validity count)

        public int CheckForHorsPlanCount(int organizationId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);
            var basicOrder = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("Basic") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.BasicCount > 0);

            int resumescount = 0;

            if (order.Count() > 0)
                resumescount = (int)order.Select(o => o.ValidityCount).FirstOrDefault();
            else if (basicOrder.Count() > 0)
                resumescount = (int)basicOrder.Select(o => o.ValidityCount).FirstOrDefault();
            else
                return 0;

            if (order.Count() > 0)
                return resumescount;
            else
                return resumescount;
        }

        //Hors-remaining count
        public int GetRemainingCount(int organizationId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);
            var basicOrder = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.BasicCount > 0);
            int resumescount;

            if (basicOrder.Count() > 0)
                resumescount = (int)basicOrder.Select(o => o.BasicCount).FirstOrDefault();
            else if (order.Count() > 0)
                resumescount = (int)order.Select(o => o.RemainingCount).FirstOrDefault();
            else
                return 0;
            return resumescount;
        }

        public int GetRemainingCountForConsultant(int consultantId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);
            int resumescount;
            if (order.Count() > 0)
                resumescount = (int)order.Select(o => o.RemainingCount).FirstOrDefault();
            else
                return 0;
            return resumescount;
        }

      

        /***Viewed Count for DPR***/
        public int GetCountForDPRView(int candidateId)
        {
            var order = _db.DPRLogs.Where(od => od.CandidateId == candidateId);
            if (order.Count() > 0)
                return order.Count();
            else
                return 0;
        }

        public int GetCountForJobViews(int jobId)
        {
            var views = _db.JobsLogs.Where(v => v.JobId == jobId);
            if (views.Count() > 0)
                return views.Count();
            else
                return 0;
        }


        /*E-Basic Plan remainingCount*/

        //HORS activation
        public int CheckForHorsRemainingCount(int empId, int candidateId)
        {

            var order = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == empId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.RemainingCount > 0);
            var basicOrder = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == empId && od.PlanName.Contains("Basic") && od.OrderMaster.PaymentStatus.Value == true && od.ValidityTill >= currentdate && od.BasicCount > 0);

            int resumescount;
            int basiccount = 0;
            int orderNo;

            if (order.Count() > 0)
                orderNo = (int)order.Select(o => o.OrderId).FirstOrDefault();
            else if (basicOrder.Count() > 0)
                orderNo = (int)basicOrder.Select(o => o.OrderId).FirstOrDefault();
            else
                return 0;

            if (order.Count() > 0)
                resumescount = (int)order.Select(o => o.RemainingCount).FirstOrDefault();
            else if (basicOrder.Count() > 0)
                resumescount = (int)basicOrder.Select(o => o.BasicCount).FirstOrDefault();
            else
                return 0;


            //candidate contact details seen or not update

            var alreadySeenorNot = from CVL in _db.ContactsViewedLogs
                                   join op in _db.OrderMasters on CVL.EmployerId equals op.OrganizationId
                                   join od in _db.OrderDetails on op.OrderId equals od.OrderId
                                   //join erd in _db.EmployerRegistrationDetails on CVL.EmployerId equals erd.EmployerId
                                   where CVL.EmployerId == empId && CVL.CandidateId == candidateId && CVL.DateViewed >= od.ActivationDate
                                        && CVL.DateViewed <= od.ValidityTill && od.PlanName.Contains("HORS")
                                   select CVL.CandidateId;

            if (alreadySeenorNot.Count() > 0)
                resumescount = 9999;

            if (resumescount != 0 && resumescount != 9999)
            {
                resumescount = resumescount - 1;

                OrderDetail orderdetail = GetOrderDetails(orderNo);
                if (basicOrder.Count() > 0)
                {
                    orderdetail.BasicCount = orderdetail.BasicCount - 1;
                    Save();
                }
                else
                {
                    orderdetail.RemainingCount = resumescount;
                    Save();
                }

                var contactsViewed = from cvc in _db.ContactsViewedCounts where cvc.EmployerId == empId select cvc;

                if (contactsViewed.Count() > 0)
                {
                    foreach (var cvc in contactsViewed)
                    {
                        cvc.ContactsViewed = cvc.ContactsViewed + 1;
                    }
                }
                else
                {
                    ContactsViewedCount objcvc = new ContactsViewedCount
                    {
                        EmployerId = empId,
                        ContactsViewed = 1
                    };
                    _db.ContactsViewedCounts.AddObject(objcvc);
                }

                ContactsViewedLog cvl = new ContactsViewedLog
                {
                    EmployerId = empId,
                    CandidateId = candidateId,
                    DateViewed = currentdate,
                    OrderId= orderNo
                };
                _db.ContactsViewedLogs.AddObject(cvl);

                Save();
            }
            if (basiccount != 0)
                return basiccount;
            else
                return resumescount;
        }

      
        public IQueryable<ContactsViewedLog> GetViewedList(int organizationId)
        {
            return (from cvl in _db.ContactsViewedLogs
                    where cvl.EmployerId == organizationId
                    select cvl);
        }


        //******End Hors related methods*********//


        //****************DPR PLANS********************//

        //*******DPR Plan Subscribtion details*******//
        public bool PlanSubscribedForDPR(int candidateId)
        {
            var planSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.PlanName.Contains("DPR")
                                 select om;
            var planSubscribedCount = planSubscribed.FirstOrDefault();
            if (planSubscribedCount != null)
                return true;
            else
                return false;

        }

        public OrderDetail GetOrderDetailsForDPR(int candidateId)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.OrderMaster.CandidateId == candidateId && c.OrderId == c.OrderMaster.OrderId && c.OrderMaster.PaymentStatus == true && c.PlanName.Contains("DPR"));
        }


        //****************END DPR*********************//

        
        //****************COMMON METHODS*********************

      
        //calculate per day pending amount for employers in admin
        public double? GetSumAmountActivationForEmployer()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value != null && od.OrderMaster.OrderDate ==currentdate && od.OrderMaster.PaymentStatus.Value == false).Sum(p => (double?)p.Amount);
        }

        //calculate perday collected amount
        public double? GetSumAmountActivatedForEmployer()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value != null && od.ActivationDate == currentdate && od.OrderMaster.PaymentStatus.Value == true).Sum(p => (double?)p.Amount);
        }

        //calculate perday pending amount & collected amount for candidates

        public double? GetSumActivationForCandidate()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId.Value != null && od.OrderMaster.OrderDate == currentdate && od.OrderMaster.PaymentStatus.Value == false).Sum(po => (double?)po.Amount);
        }

        public double? GetSumActivatedForCandidate()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId != null && od.ActivationDate == currentdate && od.OrderMaster.PaymentStatus.Value == true).Sum(po => (double?)po.Amount);
        }

        public double? GetTotalPendingAmountForCandidate()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId != null  && od.OrderMaster.PaymentStatus.Value == false).Sum(po => (double?)po.Amount);
        }

        public double? GetTotalCollectedAmountForCandidate()
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId != null && od.OrderMaster.PaymentStatus.Value == true).Sum(po => (double?)po.Amount);
        }

        public List<AlertsLog> GetOrdersFromAlertsLog(int orderId)
        {
            return _db.AlertsLogs.Where(al => al.OrderId == orderId).ToList();
        }


        //Sum of Activation & Activated Lists.
                
        public double? GetSumOfAmount()
        {
            var amount = from om in _db.OrderMasters
                         //join od in _db.OrderDetails on om.OrderId equals od.OrderId
                         where  om.PaymentStatus == false
                         orderby currentdate
                         select (om.Amount.Value);

            return amount.Sum();
            //return amount.Sum;
        }

        public double? GetSumOfAmountForActivatedList()
        {
            var amount = from om in _db.OrderMasters
                         where om.PaymentStatus == true
                         select (om.Amount.Value);
            return amount.Sum();
        }



        public bool PlanSubscribed(int organizationId)
        {
            var plansubscribe = from om in _db.OrderMasters
                                join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                where om.OrganizationId == organizationId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate
                                select om;
            var planSubscribeCount = plansubscribe.FirstOrDefault();
            if (planSubscribeCount != null)
                return true;
            else
                return false;
        }

       

        public bool PlanSubscribedForCandidate(int candidateId)
        {
            var plansubscribe = from om in _db.OrderMasters
                                join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == false
                                select om;
            var planSubscribeCount = plansubscribe.FirstOrDefault();
            if (planSubscribeCount != null)
                return true;
            else
                return false;
        }

        public bool PlanSubscribedForEmployer(int organizationId)
        {
            var plansubscribe = from om in _db.OrderMasters
                                join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                where om.OrganizationId == organizationId && od.OrderId == om.OrderId && om.PaymentStatus == false
                                select om;
            var planSubscribeCount = plansubscribe.FirstOrDefault();
            if (planSubscribeCount != null)
                return true;
            else
                return false;
        }

        public bool PlanActivatedForCandidate(int candidateId)
        {
            var planSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.CandidateId == candidateId && om.OrderId == od.OrderId && od.ActivationDate != null && (od.PlanName.Contains("RAJ")) && om.PaymentStatus == true
                                 //&& od.ValidityTill >= currentdate
                                 select om;
            var subscribedCount = planSubscribed.FirstOrDefault();
            if (subscribedCount != null)
                return true;
            else
                return false;
        }


        //SS Plan Subscribed or not
        public bool PlanActivatedForSS(int organizationId)
        {
            var planSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.OrganizationId == organizationId && om.OrderId == od.OrderId && od.ActivationDate!=null && om.PaymentStatus == true  && od.PlanName.Contains("SS".ToLower())
                                 //&& od.ValidityTill >= currentdate
                                 select om;
            var subscribedCount = planSubscribed.FirstOrDefault();
            if (subscribedCount != null)
                return true;
            else
                return false;
        }

       
        //SI plan Subscribed or Not
        public bool PlanActivatedForSI(int candidateId)
        {
            var planSubscribed = from om in _db.OrderMasters
                                 join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                 where om.CandidateId == candidateId && om.OrderId == od.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.PlanName.Contains("SI".ToLower())
                                 select om;
            var subscribedCount = planSubscribed.FirstOrDefault();
            if (subscribedCount != null)
                return true;
            else
                return false;
        }

        //List out the active plans

        public List<OrderDetail> GetActivePlans(int organizationId)
        {
            var planActivated = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.OrganizationId == organizationId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.RemainingCount > 0
                                select od;

            return planActivated.ToList();
        }


        public List<int ?> GetCandidatePendingPlans (int candidateId)
        {
            var planActivated = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == false
                                select od.OrderId;

            return planActivated.ToList();
        }

        public List<int?> GetOrganizationPendingPlans(int organizationId)
        {
            var planActivated = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.OrganizationId == organizationId && od.OrderId == om.OrderId && om.PaymentStatus == false
                                select od.OrderId;

            return planActivated.ToList();
        }

        public List<int?> GetCandidateActivePlans(int candidateId)
        {
            var planActivated = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == true
                                select od.OrderId;

            return planActivated.ToList();
        }


    

        public OrderDetail PlanActivatedDetails(int organizationId)
        {
            var activatedplan = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.OrganizationId == organizationId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate 
                                select od;

            var activate = activatedplan.FirstOrDefault();
            return activate;
        }

      
        //hors email count 

        public int? GetEmployerEmailCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true).Sum(od => (int?)od.EmailRemainingCount);   
        }

        public OrderDetail UpdateEmailCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.EmailRemainingCount > 0 && od.ValidityTill.Value >= currentdate).FirstOrDefault();
        }

                  
       //for candidat planactivated details.
        public OrderDetail PlanActivatedDetailsForCandidate(int candidateId)
        {
            var activatedplan = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.RemainingCount > 0
                                select od;

            var activate = activatedplan.FirstOrDefault();
            return activate;
        }

        
        public List<OrderMaster> GetPendingOrdersEmployers(int organizationId)
        {
            var pendingOrder = _db.OrderMasters.Where(om => om.OrganizationId == organizationId && om.PaymentStatus == false);
            return pendingOrder.ToList();
        }


        public void AddOrderMaster(OrderMaster ordermaster)
        {
            _db.OrderMasters.AddObject(ordermaster);
        }

        public void AddInvoice(Invoice invoice)
        {
            _db.Invoices.AddObject(invoice);
        }

        public void AddPostedJobAlert(PostedJobAlert postedJobAlert)
        {
            _db.PostedJobAlerts.AddObject(postedJobAlert);
        }

        internal void AddOrderDetail(OrderDetail orderdetail)
        {
            _db.OrderDetails.AddObject(orderdetail);
        }

        internal void AddOrderPayment(OrderPayment orderpayment)
        {
            _db.OrderPayments.AddObject(orderpayment);
        }

        public OrderDetail GetOrderDetails(int orderId)
        {
            return _db.OrderDetails.SingleOrDefault(o => o.OrderId == orderId);
        }

        public OrderDetail GetOrderDetailsRATUpdate(int orderId)
        {
            return _db.OrderDetails.SingleOrDefault(o => o.OrderId == orderId);
               // && o.ActivationDate == null);
        }

        public OrderDetail GetValidityRAT(int organizationId)
        {
            return _db.OrderDetails.FirstOrDefault(o => o.OrderMaster.OrganizationId == organizationId && o.ValidityTill >= currentdate);
        }

       

        internal IQueryable<OrderDetail> GetOrderDetails()
        {
            return _db.OrderDetails;
        }

        internal IQueryable<ContactsViewedLog> GetViewedLogs()
        {
            return _db.ContactsViewedLogs;
        }

        internal IQueryable<AlertsLog> GetALertsLogs()
        {
            return _db.AlertsLogs;
        }

      
        internal IQueryable<JobsViewedLog> GetCandidateAlertsLog()
        {
            return _db.JobsViewedLogs;
        }

        internal IQueryable<DPRLog> GetDPRLogs()
        {
            return _db.DPRLogs;
        }

        internal IQueryable<JobsLog> GetJobsLogs()
        {
            return _db.JobsLogs;
        }

        internal IQueryable<OrderMaster> GetOrderMasters()
        {
            return _db.OrderMasters;
        }

        /*changed internal into public*/
        public OrderDetail GetOrderDetail(int OrderId)
        {
            return _db.OrderDetails.Where(od => od.OrderId == OrderId).FirstOrDefault();
        }


        internal OrderPayment GetOrderPayment(int OrderId)
        {
            return _db.OrderPayments.Where(op => op.OrderId == OrderId).FirstOrDefault();
        }

        internal OrderMaster GetSuccessPayment(int orderId)
        {
            return _db.OrderMasters.Where(om => om.OrderId == orderId && om.PaymentStatus==false).FirstOrDefault();
        }

      
        internal OrderMaster GetOrderMaster(int orderId)
        {
            return _db.OrderMasters.SingleOrDefault(o => o.OrderId == orderId);
        }

        internal Invoice GetInvoiceId(int invoicId)
        {
            return _db.Invoices.SingleOrDefault(i => i.InvoiceId == invoicId);
        }

        internal PostedJobAlert GetAlertByOrderId(int orderId)
        {
            return _db.PostedJobAlerts.Where(pa => pa.OrderId == orderId).FirstOrDefault();
        }

        internal OrderMaster GetOrderPaymentSuccessOrder(int orderId)
        {
            return _db.OrderMasters.SingleOrDefault(o => o.OrderId == orderId && o.PaymentStatus == true);
        }

        public OrderMaster GetOrderMasterByOrganizationId(int organizationId)
        {
            return _db.OrderMasters.FirstOrDefault(o => o.OrganizationId == organizationId);
        }

        public OrderMaster GetOrderMasterByConsultantId(int consultantId)
        {
            return _db.OrderMasters.FirstOrDefault(o => o.ConsultantId == consultantId);
        }

        //edit
        public OrderDetail GetSIOrderByCandidate(int candidateId)
        {
            return _db.OrderDetails.Where(o => o.OrderMaster.CandidateId == candidateId && o.PlanName.Contains("SI".ToLower())).FirstOrDefault();
        }

      /**below method was in internal. now i changed to public to access all pages including view.**/
        public VasPlan GetVasPlanbyPlanName(string Plan)
        {
            return _db.VasPlans.Where(vp => vp.PlanName.ToLower() == Plan.ToLower()).FirstOrDefault();
        }

        public VasPlan GetVasPlanByPlanId(int planId)
        {
            return _db.VasPlans.SingleOrDefault(vp => vp.PlanId == planId);
        }

        /*Delete Multiple Entries in Order Payment*/

        public void DeleteOrderPaymentDetails(int orderId)
        {
            var order = from od in _db.OrderPayments
                        where od.OrderId == orderId
                        select od;
            foreach (var odp in order)
            {
                _db.OrderPayments.DeleteObject(odp);
            }
            _db.SaveChanges();
        }



        /************Delete Posted Jobs ALert Details by using orderId(Not Use)***********/

        public void DeletePostedJobAlerts(int orderId)
        {
           var order = from pj in _db.PostedJobAlerts
                        where pj.OrderId == orderId
                        select pj;

            foreach (var pja in order)
            {
                _db.PostedJobAlerts.DeleteObject(pja);
            }
            _db.SaveChanges();
        }

        public OrderMaster GetOrderIdByOrganizationId(int organizationId)
        {
            return _db.OrderMasters.FirstOrDefault(om => om.OrganizationId == organizationId);
        }

        

        internal void ActivateVAS(int OrderId)
        {
            OrderMaster ordermaster = GetOrderMaster(OrderId);
            OrderMaster paymentstatus = GetOrderPaymentSuccessOrder(OrderId);
            Invoice invoice = new Invoice();
          
            if (paymentstatus == null)
            {
                ordermaster.PaymentStatus = true;
                Save();
            }

            OrderDetail orderdetail = GetOrderDetails(OrderId);
          
            //Limited period plans activation
            if (orderdetail.PlanName.Contains("HORS") && orderdetail.Amount == 219)
            {
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(7 - 1);
            }

            else if (orderdetail.PlanName.Contains("RAT") && orderdetail.Amount == 219)
            {
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(7 - 1);
            }

            else if (orderdetail.PlanName.Contains("HORS") && orderdetail.Amount == 336)
            {
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(7 - 1);
            }

            else if (orderdetail.PlanName.Contains("RAT") && orderdetail.Amount == 336)
            {
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(7 - 1);
            }

            //else if (orderdetail.PlanName.Contains("SS"))
            //{
            //    orderdetail.ActivationDate = currentdate;
            //}

            else if (orderdetail.PlanName.Contains("SI"))
            {
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(Convert.ToDouble(orderdetail.ValidityDays) != null ? Convert.ToDouble(orderdetail.ValidityDays) - 1 : 0);
            }


            //end limited period plans

            else if (orderdetail.PlanName.ToLower().Contains("DPR".ToLower()))
            {
                OrderDetail dprplanactivealready = _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == orderdetail.OrderMaster.CandidateId && od.PlanName.ToLower().Contains("DPR".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).FirstOrDefault();

                if (dprplanactivealready != null)
                {
                    orderdetail.ActivationDate = dprplanactivealready.ActivationDate.Value.Date.AddDays(1);
                    orderdetail.ValidityTill = orderdetail.ActivationDate.Value.Date.AddDays(Convert.ToInt32(orderdetail.VasPlan.ValidityDays != null ? orderdetail.VasPlan.ValidityDays : 0));
                }
                else
                {
                    orderdetail.ActivationDate = currentdate;
                    double vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                    orderdetail.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);

                }
            }
            
            //Developer Note: For SMS plan validity is taking from orderdetail hors validity days. So i give this separate Loop.
            else if (orderdetail.PlanName.ToLower().Contains("SMS".ToLower()))
            {
                double vasplanDays = Convert.ToDouble(orderdetail.ValidityDays);
                orderdetail.ActivationDate = currentdate;
                orderdetail.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
            }
            else
            {
                orderdetail.ActivationDate = currentdate;
                double vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                orderdetail.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
            }

            /*****Start invoice create**********/
                invoice.OrderNo = OrderId;
                invoice.InvoiceDate = currentdate;
                invoice.InvoiceNo = orderdetail.ActivationDate.Value.ToString("ddMMyyy") + "000";
                AddInvoice(invoice);
                Save();

                Invoice getinvoice = GetInvoiceId(invoice.InvoiceId);
                invoice.InvoiceNo = orderdetail.ActivationDate.Value.ToString("ddMMyyy") + "000" + invoice.InvoiceId;
                Save();

                ordermaster.InvoiceId = getinvoice.InvoiceId;
                Save();
                       
            /*****End invoice create**********/
           

            if (ordermaster.Organization != null)
            {
                ordermaster.ActivatedBy = ordermaster.Organization.Name;
            }
            if (ordermaster.Candidate != null)
            {
                ordermaster.ActivatedBy = ordermaster.Candidate.Name;
            }

            Save();
         
        }

       
        /*End By Consultant to get Hors Count*/

        public bool CheckDPRValidity(int CandidateId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == CandidateId && od.PlanName.ToLower().Contains("DPR".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).Count() != 0;
        }

        //get hot resumes activated count by orgId
        public int? GetHotResumeCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).Sum(od => (int?)od.RemainingCount);
        }

        //get sms activated count by orgId
        public int? GetSmsVasCount(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("SMS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0).Sum(od => (int?)od.RemainingCount);
        }

        //get the first row of sms activated by orgid from orderdetail
        public OrderDetail GetEmployerSmsVas(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId == organizationId && od.PlanName.ToLower().Contains("SMS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0).FirstOrDefault();
        }

        public OrderDetail GetCandidateSmsVas(int candidateId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == candidateId && od.PlanName.Contains("SMS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.RemainingCount > 0).FirstOrDefault();
        }

        //***********End Common Functions for VAS**************



        //******** Employer- RAT Plan Subscriptions & Activations & Send Alerts- start***********

        public int? GetVacancies(int organizationId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("RAT") && od.OrderMaster.PaymentStatus.Value == true && od.OrderMaster.OrderId == od.OrderId).Sum(p => (int?)p.Vacancies);
        }

     
        /*Get Remaining Count of RAT assigned Vacancy*/
        public int? GetRemaingCountVacancy(int jobId, int organizationId)
        {
            return _db.PostedJobAlerts.Where(od => od.OrganizationId == organizationId && od.JobId == jobId).Sum(r => (int?)r.RemainingCount);
        }


        public bool GetRatSubscribed(int organizationId)
        {
            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.OrganizationId == organizationId && od.PlanName.Contains("RAT") && od.OrderId == om.OrderId && om.PaymentStatus == true 
                           //&& od.ValidityTill >= currentdate
                           select om;


            var alertcount = alrCount.FirstOrDefault();

            if (alertcount != null)
                return true;
            else
                return false;

        }

        #region
        public int? GetVacanciesConsultant(int consultantId)
        {
            return _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("RAT") && od.OrderMaster.PaymentStatus.Value == true && od.OrderMaster.OrderId == od.OrderId).Sum(p => (int?)p.Vacancies);
        }


        /*Get Remaining Count of RAT assigned Vacancy*/
        public int? GetRemaingCountVacancyByConsultantId(int jobId, int consultantId)
        {
            return _db.PostedJobAlerts.Where(od => od.ConsultantId == consultantId && od.JobId == jobId).Sum(r => (int?)r.RemainingCount);
        }


        public bool GetRatSubscribedForConsultant(int consultantId)
        {
            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.ConsultantId == consultantId && od.PlanName.Contains("RAT") && od.OrderId == om.OrderId && om.PaymentStatus == true
                           //&& od.ValidityTill >= currentdate
                           select om;


            var alertcount = alrCount.FirstOrDefault();

            if (alertcount != null)
                return true;
            else
                return false;

        }

        public int GetPlanActivatedResultRATForConsult(int consultantId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.ConsultantId.Value == consultantId && od.PlanName.Contains("RAT") && od.OrderMaster.PaymentStatus.Value == true && currentdate <= od.ValidityTill);
            int orderNo;

            if (order.Count() > 0)
                orderNo = (int)order.Select(o => o.OrderId).FirstOrDefault();
            else
                return 0;

            return orderNo;
        }

        public List<PostedJobAlert> GetPostedJobAlertForConsultant(int consultantId)
        {
            var order = _db.PostedJobAlerts.Where(pja => pja.ConsultantId == consultantId && pja.ActivatedJobStatus == true);

            return order.ToList();
        }

        public IQueryable<PostedJobAlert> GetAlertByConsultant(int id, int orderId)
        {
            return _db.PostedJobAlerts.Where(j => j.ConsultantId == id && j.OrderId == orderId);
        }


        public void UpdateVacanciesCountByConsultanId(int consultanId, int jobId)
        {

            var alertscount = _db.OrderDetails.Where(pd => pd.OrderMaster.ConsultantId.Value == consultanId && pd.PlanName.Contains("RAT") && pd.OrderMaster.PaymentStatus.Value == true);
            var orderId = GetPlanActivatedResultRATForConsult(consultanId);
            var postedJobs = GetAlertByConsultant(consultanId, orderId);

            var vacancies = GetVacancies(consultanId);

            if (alertscount.Count() > 0)
            {
                foreach (var al in alertscount)
                {
                    if (postedJobs.Count() <= vacancies)
                    {
                        al.Vacancies = vacancies - postedJobs.Count();
                    }
                }
            }

            Save();
        }
        #endregion

        /*Not in Use*/
        public AlertsLog GetSmsEmailLog(int organizationId, int candidateId, int jobId)
        {
            
            var alreadyseenorNot = from al in _db.AlertsLogs
                                   join om in _db.OrderMasters on al.OrganizationId equals om.OrganizationId
                                   join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                   where al.OrganizationId == organizationId && al.CandidateId == candidateId
                                   && al.JobId == jobId 
                                   select al;

            var alertDetails = alreadyseenorNot.FirstOrDefault();
            return alertDetails;
        }

       

        
        public OrderDetail GetOrderDetailsForRAT(int organizationId)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.OrderMaster.OrganizationId.Value == organizationId && c.OrderId == c.OrderMaster.OrderId);
        }
        
        public PostedJobAlert GetPostedJobAlert(int organizationId, int jobId)
        {
            return _db.PostedJobAlerts.FirstOrDefault(c => c.OrganizationId == organizationId && c.JobId == jobId);
        }

        public void UpdateVASDetails(int empId, int jobIdPosted)
        {
            var registeredPlan = from erd in _db.OrderMasters
                                 from od in _db.OrderDetails
                                 where erd.OrganizationId.Value == empId && erd.OrderId== od.OrderId && od.PlanName.Contains("RAT") && erd.PaymentStatus.Value == true
                                 select erd;

            var orderDetails = registeredPlan.FirstOrDefault();


            PostedJobAlert objPJD = new PostedJobAlert
            {
                OrganizationId = empId,
                JobId = jobIdPosted,
                OrderId = orderDetails.OrderId,
                ActivatedJobStatus = true
            };
            _db.PostedJobAlerts.AddObject(objPJD);
            Save();
        }

        public IQueryable<PostedJobAlert> GetJobsByOrganizationIdAlert(int id, int orderId)
        {
            return _db.PostedJobAlerts.Where(j => j.OrganizationId == id && j.OrderId== orderId );
        }

       

        /*Get Alert from Posted Job alert by OrgId and OrderId.*/
        public IQueryable<PostedJobAlert> GetRATAlertJobs(int organizationId,int orderId)
        {
            return _db.PostedJobAlerts.Where(j => j.OrganizationId == organizationId && j.OrderId == orderId);
        }

        public PostedJobAlert GetJobIdByPostedJobAlert(int jobId)
        {
            return _db.PostedJobAlerts.SingleOrDefault(j => j.JobId == jobId);
        }

        public bool CheckForAlertsRemained(int employerId, int jobId)
        {
            var alrCount = from om in _db.OrderMasters
                           from ad in _db.PostedJobAlerts
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.OrganizationId == employerId && ad.JobId == jobId && od.PlanName.Contains("RAT") && od.RemainingCount > 0
                           select om;

            var alertcountForConsultants = from om in _db.OrderMasters
                           from ad in _db.PostedJobAlerts
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.ConsultantId == employerId && ad.JobId == jobId && od.PlanName.Contains("RAT") && od.RemainingCount > 0
                           select om;

            var consultantalert = alertcountForConsultants.Count();
            var alr = alrCount.Count();

            if (alr == 0 && consultantalert == 0)
                return false;
            else
                return true;

        }

        public bool CheckRATAlertByVacancy(int employerId,int jobId)
        {
            var alrCount = from ad in _db.PostedJobAlerts
                           join od in _db.OrderDetails on ad.OrderId equals od.OrderId
                           join om in _db.OrderMasters on od.OrderId equals om.OrderId
                           where om.OrganizationId == employerId && ad.JobId == jobId && od.OrderId == ad.OrderId && ad.ActivatedJobStatus == true && currentdate <= ad.ValidityTill && ad.RemainingCount > 0
                           select om;

            var consultantCount = from ad in _db.PostedJobAlerts
                                  join od in _db.OrderDetails on ad.OrderId equals od.OrderId
                                  join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                  where om.ConsultantId == employerId && ad.JobId == jobId && od.OrderId == ad.OrderId && ad.ActivatedJobStatus == true && currentdate <= ad.ValidityTill && ad.RemainingCount > 0
                                  select om;

            var alr = alrCount.Count();
            var consultantalert = consultantCount.Count();

            if (alr == 0 && consultantalert == 0)
                return false;
            else
                return true;

        }

        public int GetPlanActivatedResultRAT(int organizationId)
        {
            var order = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == organizationId && od.PlanName.Contains("RAT") && od.OrderMaster.PaymentStatus.Value == true && currentdate <= od.ValidityTill);
            int orderNo;

            if (order.Count() > 0)
                orderNo = (int)order.Select(o => o.OrderId).FirstOrDefault();
            else
                return 0;

            return orderNo;
        }

        public List<PostedJobAlert> GetPostedJobAlert(int organizationId)
        {
            var order = _db.PostedJobAlerts.Where(pja => pja.OrganizationId == organizationId && pja.ActivatedJobStatus == true);

            return order.ToList();
        }


        public void UpdateVacanciesCount(int organizationId, int jobId)
        {

            var alertscount = _db.OrderDetails.Where(pd => pd.OrderMaster.OrganizationId.Value == organizationId && pd.PlanName.Contains("RAT") && pd.OrderMaster.PaymentStatus.Value == true);
            var orderId = GetPlanActivatedResultRAT(organizationId);
            var postedJobs = GetJobsByOrganizationIdAlert(organizationId,orderId);

            var vacancies = GetVacancies(organizationId);

            if (alertscount.Count() > 0)
            {
                foreach (var al in alertscount)
                {
                    if (postedJobs.Count() <= vacancies)
                    {
                        al.Vacancies = vacancies - postedJobs.Count();
                    }
                }
            }

            Save();
        }

        //For RAT
        public void logAlerts(int organizationId, int jobId, int candId, bool smsSent, bool mailSent)
        {
            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.OrganizationId == organizationId && od.PlanName.Contains("RAT")  && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.RemainingCount > 0 
                           select om;

            var consultantalrCount = from om in _db.OrderMasters
                                     join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                     where om.ConsultantId == organizationId && od.PlanName.Contains("RAT") && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.RemainingCount > 0
                                     select om;

            var consultAlertCount = consultantalrCount.FirstOrDefault();
            if (consultAlertCount != null)
            {
                AlertsLog al = new AlertsLog
                {
                    ConsultantId = organizationId,
                    JobId = jobId,
                    AlertSentDate = currentdate,
                    CandidateId = candId,
                    SmsSent = smsSent,
                    MailSent = mailSent,
                    OrderId = consultAlertCount.OrderId

                };
                _db.AlertsLogs.AddObject(al);
                Save();
            }

            var alertcount = alrCount.FirstOrDefault();

            if (alertcount != null)
            {
                AlertsLog al = new AlertsLog
                {
                    OrganizationId = organizationId,
                    JobId = jobId,
                    AlertSentDate = currentdate,
                    CandidateId = candId,
                    SmsSent = smsSent,
                    MailSent = mailSent,
                    OrderId = alertcount.OrderId

                };
                _db.AlertsLogs.AddObject(al);
                Save();
            }
        }

        //checking already sent alert to candidate or not.
        public bool RatAlertsentorNot(int organizationId, int candidateId, int jobId)
        {
            var alreadysentornot = from al in _db.AlertsLogs
                                   //join om in _db.OrderMasters on al.OrganizationId equals om.OrganizationId
                                   join od in _db.OrderDetails on al.OrderId equals od.OrderId
                                   where al.OrganizationId == organizationId  && al.CandidateId == candidateId && al.JobId == jobId && al.AlertSentDate >= od.ActivationDate 
                                   select al;

            var consultantalertsentornot = from al in _db.AlertsLogs
                                           //join om in _db.OrderMasters on al.OrganizationId equals om.OrganizationId
                                           join od in _db.OrderDetails on al.OrderId equals od.OrderId
                                           where al.ConsultantId == organizationId && al.CandidateId == candidateId && al.JobId == jobId && al.AlertSentDate >= od.ActivationDate
                                           select al;

            if (alreadysentornot.Count() > 0 && consultantalertsentornot.Count() > 0)
                return false;
            else
                return true;
        }

                  
        public void UpdateAlertsDoneCount(int employerId, int jobId)
        {

            var alertscount = _db.OrderDetails.Where(pd => pd.OrderMaster.OrganizationId.Value == employerId && pd.PlanName.Contains("RAT")  && pd.OrderMaster.PaymentStatus.Value == true && pd.ValidityTill >= currentdate && pd.RemainingCount > 0);

            var consultantalertscount = _db.OrderDetails.Where(pd => pd.OrderMaster.ConsultantId.Value == employerId && pd.PlanName.Contains("RAT") && pd.OrderMaster.PaymentStatus.Value == true && pd.ValidityTill >= currentdate && pd.RemainingCount > 0);
            //var order = _db.OrderDetails.Where(od => od.OrderMaster.OrganizationId.Value == empId && od.PlanName.Contains("HORS") && od.OrderMaster.PaymentStatus.Value == true);

            if (consultantalertscount.Count() > 0)
            {
                foreach (var al in consultantalertscount)
                {
                    al.RemainingCount = al.RemainingCount - 1;
                }
            }
            Save();

            if (alertscount.Count() > 0)
            {
                foreach (var al in alertscount)
                {
                    al.RemainingCount = al.RemainingCount - 1;
                }
            }
            Save();
        }

        /*Update Posted Job Alert Remaining Count(RAT)*/
        public void UpdateAlertsDoneRAT(int employerId, int jobId)
        {
            var alertcount = _db.PostedJobAlerts.Where(pd => pd.OrderMaster.OrganizationId.Value == employerId && pd.JobId == jobId && pd.ActivatedJobStatus == true && pd.ValidityTill >= currentdate && pd.RemainingCount > 0);

            var consultalertscount = _db.PostedJobAlerts.Where(pd => pd.OrderMaster.ConsultantId.Value == employerId && pd.JobId == jobId && pd.ActivatedJobStatus == true && pd.ValidityTill >= currentdate && pd.RemainingCount > 0);

            if (consultalertscount.Count() > 0)
            {
                foreach (var al in consultalertscount)
                {
                    al.RemainingCount = al.RemainingCount - 1;
                }
            }
            Save();

            if (alertcount.Count() > 0)
            {
                foreach (var al in alertcount)
                {
                    al.RemainingCount = al.RemainingCount - 1;
                }
            }
            Save();
        }

        

        //**********End RAT plan activation & subscription***************



        //********Candidate- RAJ plan Subscription & Activation****************

        public int? RAJPlanActivatedDetails(int candidateId)
        {
            var activatedPlan = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && om.PaymentStatus == true && od.PlanName.Contains("RAJ")
                                select od.PlanId;

            var planId = activatedPlan.FirstOrDefault();
            return planId;
        }

        public OrderDetail GetOrderDetailsForRAJ(int candidateId)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.OrderMaster.CandidateId.Value == candidateId && c.OrderId == c.OrderMaster.OrderId && c.OrderMaster.OrderDate!=null);
            //od.OrderMaster.OrganizationId.Value == empId && od.PlanName.Contains("RAT") && od.OrderMaster.PaymentStatus.Value == true)
        }

        public bool GetRaJSubscribed(int candidateId)
        {
            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.CandidateId == candidateId && od.PlanName.Contains("RAJ") && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate
                           select om;


            var alertcount = alrCount.FirstOrDefault();

            if (alertcount != null)
                return true;
            else
                return false;

        }

        public OrderDetail RAJActivateStatus(int candidateId)
        {
            var activatedplan = from od in _db.OrderDetails
                                join om in _db.OrderMasters on od.OrderId equals om.OrderId
                                where om.CandidateId == candidateId && od.OrderId == om.OrderId && od.PlanName.Contains("RAJ") && om.PaymentStatus == true && od.ValidityTill >= currentdate && od.RemainingCount > 0
                                select od;

            var activate = activatedplan.FirstOrDefault();
            return activate;
        }

       
        public bool CheckForAlertsRemainedRAJ(int candidateId)
        {
            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.CandidateId == candidateId && od.PlanName.Contains("RAJ") && od.RemainingCount <= od.ValidityCount && currentdate <= od.ValidityTill
                           select om;

            var rajAlert = alrCount.Count();
            if (rajAlert == 0)
                return false;
            else
                return true;

        }

        public void logCandlerts(int candId, int planId, int jobId, bool smsSent, bool mailSent)
        {

            var alrCount = from om in _db.OrderMasters
                           join od in _db.OrderDetails on om.OrderId equals od.OrderId
                           where om.CandidateId == candId && od.PlanName.Contains("RAJ") && od.OrderId == om.OrderId && om.PaymentStatus == true && od.ValidityTill >= currentdate
                           select om;


            var alertcount = alrCount.FirstOrDefault();

            JobsViewedLog jvl = new JobsViewedLog
            {
                CandidateId = candId,
                PlanId = planId,
                DateViewed = currentdate,
                JobId = jobId,
                OrderId=alertcount.OrderId,
                SmsSent=smsSent,
                MailSent= mailSent
            };
            _db.JobsViewedLogs.AddObject(jvl);
            Save();
        }

        public bool AlreadyALertsentorNot(int candidateId, int JobId)
        {
            var alreadySeenorNot = from jvl in _db.JobsViewedLogs
                                   join om in _db.OrderMasters on jvl.CandidateId equals om.CandidateId
                                   join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                   where jvl.CandidateId == candidateId && jvl.JobId == JobId && jvl.DateViewed >= od.ActivationDate && 
                                   jvl.DateViewed <=od.ValidityTill && od.PlanName.Contains("RAJ")
                                   select jvl.JobId;
            
            if (alreadySeenorNot.Count() > 0)
                return false;
            else
                return true;
        }

        
        public void UpdateCandAlertsDoneCount(int candidateId)
        {
            var candAlertsCount = _db.OrderDetails.Where(od => od.OrderMaster.CandidateId == candidateId && od.PlanName.Contains("RAJ") && od.OrderMaster.PaymentStatus.Value == true);

            if (candAlertsCount.Count() > 0)
            {
                foreach (var alerts in candAlertsCount)
                {
                    alerts.RemainingCount = alerts.RemainingCount - 1;
                }
            }

            Save();
        }

        // Matching jobs to send candidates-RAJ

        public IEnumerable<OrderMaster> GetMatchingCandidatestoAlert(List<int> lstCandidateIds)
        {
            List<OrderMaster> candidates = new List<OrderMaster>();

            var lstCandidates = from lstCandidateid in lstCandidateIds
                                join om in _db.OrderMasters on lstCandidateid equals om.CandidateId
                                join od in _db.OrderDetails on om.OrderId equals od.OrderId
                                where om.PaymentStatus == true
                                select om;
            foreach (var cand in lstCandidates)
            {
                candidates.Add(new OrderMaster() { CandidateId = cand.CandidateId });
            }
            return candidates;
        }


        public IEnumerable<OrderDetail> GetAmountFromOrders(List<int> lstOrderIds)
        {
            List<OrderDetail> orderAmount = new List<OrderDetail>();

            var lstOrders = from lstorderId in lstOrderIds
                            join od in _db.OrderDetails on lstorderId equals od.OrderId
                            select od.Amount;

            foreach (var amount in lstOrders)
            {
                orderAmount.Add(new OrderDetail() { Amount = amount.Value }); 
            }

            return orderAmount;
        }


        //**********End RAJ plan activation & subscription***************

#endregion
        
        #endregion

               
        //public void  DeleteOrderPaymentDetails(int orderId)
        //{
        //    var orderpayment = _db.OrderPayments.Where(op => op.OrderId == orderId).FirstOrDefault();
        //    _db.OrderPayments.DeleteObject(orderpayment);
        //    _db.SaveChanges();
        //}

        public IEnumerable<OrderMaster> GetOrderMasterListByOrganizationId(int organizationId)
        {
            List<OrderMaster> OrderMasterList = new List<OrderMaster>();

            var orderMasters = (from orderMaster in _db.OrderMasters
                                where orderMaster.OrganizationId == organizationId
                                select orderMaster);

            foreach (var order in orderMasters)
                OrderMasterList.Add(order);

            return OrderMasterList;
        }

        public void DeleteAlertsLogs(int orderId)
        {
            var order = from od in _db.AlertsLogs
                        where od.OrderId == orderId
                        select od;
            foreach (var odp in order)
            {
                _db.AlertsLogs.DeleteObject(odp);
            }
            _db.SaveChanges();
        }

        public void DeleteOrderDetails(int orderId)
        {
            var orderdetail = _db.OrderDetails.Where(od => od.OrderId == orderId).SingleOrDefault();
            _db.OrderDetails.DeleteObject(orderdetail);
            _db.SaveChanges();
        }

        
        public void DeleteOrderMaster(int orderId)
        {
            var ordermaster = _db.OrderMasters.Where(od => od.OrderId == orderId).SingleOrDefault();
            _db.OrderMasters.DeleteObject(ordermaster);
            _db.SaveChanges();
        }

        public void DeleteAlertsLogOrder(int orderId)
        {
            var alertsLog = _db.AlertsLogs.Where(al => al.OrderId == orderId).FirstOrDefault();
            _db.AlertsLogs.DeleteObject(alertsLog);
            _db.SaveChanges();
        }


        public void Save()
        {
            _db.SaveChanges();
        }
        #endregion

        public IQueryable<VasPlan> VasPlans()
        {
            throw new NotImplementedException();
        }

        public IQueryable<VasPlan> VasPlans(string q)
        {
            throw new NotImplementedException();
        }
        
        public IQueryable<VasPlan> GetVasPlan()
        {
            throw new NotImplementedException();
        }

        public IQueryable<VasPlan> GetVasPlan(string q)
        {
            throw new NotImplementedException();
        }
    }
}