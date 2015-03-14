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
using System.Text;


namespace Dial4Jobz.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        const int PAGE_SIZE = 15;

        
        public ActionResult Index(int ? page)
        {
            IQueryable<Job> jobs = _repository.GetJobs();

            // ******ordering paid employers jobs********

            DateTime currentdate = Constants.CurrentTime().Date;
            DateTime updateFresh = currentdate;
            DateTime freshConsultant = currentdate;
            updateFresh = DateTime.Now.AddMinutes(-30);
            freshConsultant = DateTime.Now.AddDays(-15);

            List<int> lstOrganizationId = null;
            List<int> lstUpdatedJobsListId = null;
            List<int> lstConsultantListId=null;

            var orderOrganization = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderId == od.OrderMaster.OrderId && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.OrganizationId.Value);
            var updatedVacancyList = _repository.GetJobs().Where(c => c.UpdatedDate != null && updateFresh <= c.UpdatedDate).Select(c => c.Id);
            var orderLatestConsultant = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.ConsultantId != null && od.OrderMaster.PaymentStatus == true && freshConsultant <= od.ActivationDate && od.ValidityTill.Value >= currentdate).Select(c => c.OrderMaster.Consultante.Id);
            //&& freshConsultant <= od.ActivationDate 

            if (orderOrganization.Count() > 0)
            {
                lstOrganizationId = orderOrganization.ToList();
                lstUpdatedJobsListId = updatedVacancyList.ToList();
                lstConsultantListId= orderLatestConsultant.ToList();

                Func<IQueryable<Job>, IOrderedQueryable<Job>> orderingFunc = query => 
                {
                    if (orderOrganization.Count() > 0)
                        //return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => rslt.CreatedDate);
                        //return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => lstUpdatedJobsListId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);
                        return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt=>lstConsultantListId.Contains(rslt.Consultante.Id)).ThenByDescending(rslt => lstUpdatedJobsListId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);
                        //return query.OrderByDescending(rslt => lstConsultantListId.Contains(rslt.Consultante.Id)).ThenByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => lstUpdatedJobsListId.Contains(rslt.Id));

                    if (updatedVacancyList.Count() > 0)
                        return query.OrderByDescending(rslt => lstUpdatedJobsListId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.UpdatedDate);

                    if (orderLatestConsultant.Count() > 0)
                        return query.OrderByDescending(rslt => lstConsultantListId.Contains(rslt.Consultante.Id)).ThenByDescending(rslt => rslt.UpdatedDate);
                    else
                        return query.OrderByDescending(rslt => rslt.CreatedDate);
                };

                jobs = orderingFunc(jobs);
            }
            //***************END*************************
           
           
            //more button clicked
            if (Request.IsAjaxRequest())
            {                
                var paginatedJobs = new PaginatedList<Job>(jobs, page ?? 0, PAGE_SIZE);

                if (paginatedJobs.HasNextPage)              
                    AddMoreUrlToViewData((page ?? 0) + 1);                

                return View("Jobs", paginatedJobs);
            }

            //initial page load
            var page1Jobs = new PaginatedList<Job>(jobs, 0, PAGE_SIZE);

            if (page1Jobs.HasNextPage)            
                AddMoreUrlToViewData((page ?? 0) + 1);

            return View(page1Jobs);
        }

        public ActionResult About()
        {
            return View();           
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult HomePageTest()
        {
            return View();
        }

        public ActionResult ChannelsEarningOpportunities()
        {
            return View();
        }

        public ActionResult ChannelEarningLists()
        {
            return View();
        }

        public ActionResult CreatePopupMobile()
        {
            return View();
        }

        //create popup for phone numbers
        public ActionResult HomePhoneNumber()
        {
            return View();
        }

        public ActionResult SendHomePhoneNumber()
        {
            return View();
        }

        public ActionResult SendHomeMobileNumber()
        {
            return View();
        }


        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Send(FormCollection collection, string LandlineNumber, string MobileNumber)
        {
            if (LandlineNumber != null)
            {
                
                SmsHelper.SendSecondarySms(
                    Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.ShareLandlineSMS,
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                   LandlineNumber
                   );

                //SmsHelper.Sendsms(
                //    Constants.SmsSender.UserId,
                //    Constants.SmsSender.Password,
                //    Constants.SmsBody.ShareLandlineSMS,
                //    Constants.SmsSender.Type,
                //    Constants.SmsSender.senderId,
                //    LandlineNumber
                //    );
            }
            else
            {

                SmsHelper.SendSecondarySms(
                   Constants.SmsSender.SecondaryUserName,
                   Constants.SmsSender.SecondaryPassword,
                   Constants.SmsBody.ShareMobileSMS,
                   Constants.SmsSender.SecondaryType,
                   Constants.SmsSender.Secondarysource,
                   Constants.SmsSender.Secondarydlr,
                   MobileNumber
                   );

                //SmsHelper.Sendsms(
                //   Constants.SmsSender.UserId,
                //   Constants.SmsSender.Password,
                //   Constants.SmsBody.ShareMobileSMS,
                //   Constants.SmsSender.Type,
                //   Constants.SmsSender.senderId,
                //   MobileNumber
                //   );
            }

            return RedirectToAction("HomePage", "Home");
        }


       

        public ActionResult HomePage()
        {

            // to list out the role names
            List<string> lstRoles = new List<string>();
            //lstRoles = _repository.GetRolesForFindJobseekers();
            lstRoles = _repository.GetTitlesForFindJobseekers();

            // To list out the functions name

            List<string> lstFunctions = new List<string>();
            lstFunctions = _repository.GetFunctionsForRolesFindJobSeekers();


            List<SelectListItem> selectListRoles = new List<SelectListItem>();
            var lstCombined = lstRoles.Zip(lstFunctions, (role, func) => new { Role = role, Function = func }).ToList();
            int i = 1;

            foreach (var item in lstCombined)
            {
                selectListRoles.Add(new SelectListItem
                {
                    Text = item.Role,
                    Value = item.Function,
                    Selected = (i == 0)
                });

                i++;
            }

            ViewData["RolesForJobSeekers"] = selectListRoles;

            return View();
        }

        

        public ActionResult Terms()
        {
            return View("Terms");
        }

        public ActionResult Privacy()
        {
            return View("Privacy");
        }

        public ActionResult Contact()
        {
            return View("Contact");
        }

        public ActionResult Security()
        {
            return View("Security");
        }

        public ActionResult SmsTerms()
        {
            return View("SmsTerms");
        }

        private void AddMoreUrlToViewData(int nextPage)
        {
            ViewData["moreUrl"] = Url.Action("Index", "Home", new { page = nextPage });            
        }
        
    }
}
