using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models;
using Dial4Jobz.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Text;

namespace Dial4Jobz.Controllers
{
    [HandleError]
    public class EmployerController : BaseController
    {
        string smsSent = "No";
        string mailSent = "No";

        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();

        const int PAGE_SIZE = 15;

        public ActionResult Index(int? page)
        {

            List<string> lstRoles = new List<string>();
            lstRoles = _repository.GetRolesForFindJobseekers();

            List<SelectListItem> selectListRoles = new List<SelectListItem>();

            int i = 1;
            foreach (string role in lstRoles)
            {
                selectListRoles.Add(new SelectListItem
                {
                    Text = role,
                    Value = role,
                    Selected = (i == 0)
                });

                i++;
            }
            ViewData["RolesForJobSeekers"] = selectListRoles;
            IQueryable<Candidate> candidates = _repository.GetCandidates();
            return GetPaginatedCandidates(page, candidates);
        }

        public ActionResult SearchCandidates(int? page)
        {
            return View();
        }

      
        public ActionResult Invoice(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    int OrderId;
                    bool ValueIsAnId = int.TryParse(orderId, out OrderId);

                    if (ValueIsAnId)
                    {
                        return View(_vasRepository.GetOrderDetail(OrderId));
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult MySubscription_Billing()
        {

            if (LoggedInOrganization != null)
                ViewData["LoggedOrganization"] = LoggedInOrganization.Id;
            else
                ViewData["LoggedOrganization"] = 0;

            //LoggedInOrganization = User.Identity.IsAuthenticated ? new UserRepository().GetOrganizationByUserName(User.Identity.Name) : null;

            //if (LoggedInOrganization != null)
            //    ViewData["LoggedInOrganization"] = LoggedInOrganization.Id;
            //else
            //    ViewData["LoggedInOrganization"] = 0;

            return View();
        }

        public ActionResult PricingPlans()
        {
            return View();
        }


        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult ActivateRATVacancy(FormCollection collection)
        {
            var vacancies = _vasRepository.GetVacancies(LoggedInOrganization.Id);
            var selectedKeyCount = 0;
            var orderId = _vasRepository.GetPlanActivatedResultRAT(LoggedInOrganization.Id);
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(LoggedInOrganization.Id, orderId);
            bool isSuccess = false;
            DateTime currentdate = Constants.CurrentTime().Date;
            OrderDetail orderdetail = null;
            PostedJobAlert postedJobalert = null;
            double vasplanDays = 0;

            if (LoggedInOrganization != null)
            {
                foreach (string key in collection.AllKeys)
                {
                    if (key.Contains("Job"))
                    {
                        int jobId = Convert.ToInt32(key.Replace("Job", string.Empty));
                        var activatedvacancy = _vasRepository.GetPostedJobAlert(LoggedInOrganization.Id, jobId);

                        var value = collection.AllKeys.Count();

                        var selectedKeyword = Convert.ToInt32(collection.GetValues(key).Contains("true"));
                        selectedKeyCount = selectedKeyCount + selectedKeyword;

                        if (selectedKeyCount <= vacancies)
                        {
                            if (postedJobs.Count() < vacancies)
                            {
                                if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                                {
                                    Job job = _repository.GetJob(jobId);
                                    PostedJobAlert jobs = _vasRepository.GetJobIdByPostedJobAlert(jobId);

                                    //already posted jobs
                                    if (jobs != null)
                                    {
                                        if (jobs.JobId != jobId)
                                        {
                                            Response.Write("<script language=javascript>alert('You have already activated this vacancy');</script>");
                                        }
                                    }

                                    else
                                    {

                                        OrderDetail validityPlan = _vasRepository.GetValidityRAT(LoggedInOrganization.Id);
                                        if (validityPlan != null)
                                        {
                                            _vasRepository.UpdateVASDetails(LoggedInOrganization.Id, jobId);

                                            postedJobalert = _vasRepository.GetPostedJobAlert(LoggedInOrganization.Id, jobId);
                                            orderdetail = _vasRepository.GetOrderDetailsRATUpdate(Convert.ToInt32(postedJobalert.OrderId));
                                            postedJobalert.Vacancies = vacancies;
                                            postedJobalert.AlertActivateDate = Constants.CurrentTime();
                                            vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                                            postedJobalert.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
                                            postedJobalert.RemainingCount = 25;
                                            _vasRepository.Save();
                                        }
                                        else
                                            //return Json(new JsonActionResult { Success = false, Message = "Your Plan has been finished. You cannot assign Vacancy" });
                                            Response.Write("<script language=javascript>alert('Your Plan has been finished. You cannot assign Vacancy');</script>");

                                        //    _vasRepository.Save();
                                        EmailHelper.SendEmail(
                                            Constants.EmailSender.EmployerSupport,
                                            LoggedInOrganization.Email,
                                            Constants.EmailSubject.ActivateVacancy,
                                            Constants.EmailBody.ActivateVacancy
                                            .Replace("[NAME]", LoggedInOrganization.Name)
                                            .Replace("[POSITION]", job.Position)
                                            .Replace("[ORDER_NO]",orderdetail.OrderId.ToString())
                                            .Replace("[PLAN_NAME]", orderdetail.VasPlan.PlanName)
                                            .Replace("[VALIDITY_COUNT]", (orderdetail.BasicCount!=null? orderdetail.BasicCount.ToString() : orderdetail.ValidityCount.ToString()))
                                            .Replace("[START_DATE]", postedJobalert.AlertActivateDate.Value.ToString("dd-MM-yyyy"))
                                            .Replace("[END_DATE]", postedJobalert.ValidityTill.Value.ToString("dd-MM-yyyy"))
                                            .Replace("[VAC_ALERT]", "25")
                                            .Replace("[NOTICE]", "Employers")
                                            .Replace("[LINK_NAME]", "Your DashBoard")
                                            .Replace("[DASHBOARD_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/MatchCandidates")
                                            );

                                        SmsHelper.SendSecondarySms(
                                            Constants.SmsSender.SecondaryUserName,
                                            Constants.SmsSender.SecondaryPassword,
                                            Constants.SmsBody.ActivateVacancy
                                            .Replace("[NAME]", LoggedInOrganization.Name)
                                            .Replace("[POSITION]", job.Position)
                                             .Replace("[PLAN]", orderdetail.VasPlan.PlanName)
                                            .Replace("[VALIDITY_COUNT]", orderdetail.ValidityCount.ToString())
                                            .Replace("[VALIDITY_TILL]", postedJobalert.ValidityTill.Value.ToString("dd-MM-yyyy")),
                                            Constants.SmsSender.SecondaryType,
                                            Constants.SmsSender.Secondarysource,
                                            Constants.SmsSender.Secondarydlr,
                                            LoggedInOrganization.MobileNumber
                                            );
                                        //}
                                        isSuccess = true;

                                    }

                                }
                            }
                        }

                    }


                }



            }
            return RedirectToAction("PostedJobs", "Employer");

        }
        

        public ActionResult MatchCandidates(FormCollection collection, int? page)
        {
            string what = string.Empty;
            string name = string.Empty;
            string where = string.Empty;
            string skill = string.Empty;
            string position = string.Empty;
            string minSalary = string.Empty;
            string maxSalary = string.Empty;
            string minExperience = string.Empty;
            string maxExperience = string.Empty;
            string function = string.Empty;
            string gender = string.Empty;
            string loc = string.Empty;
            string currentloc = string.Empty;
            string annualsalary = string.Empty;
            //string City = string.Empty;

            Dictionary<string, string> filters = new Dictionary<string, string>();

            
            string newRegistration = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["newReg"]))
            {
                newRegistration = Request.QueryString["newReg"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["what"]))
            {
                what = Request.QueryString["what"];
                Session["What"] = what;
                filters.Add("what", what);
            }
            else
            {
                Session["What"] = "Find Candidates";
            }

            if (!string.IsNullOrEmpty(Request.QueryString["where"]))
            {
                where = Request.QueryString["where"];
                filters.Add("where", where);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["loc"]))
            {
                where = Request.QueryString["loc"];
                filters.Add("loc", where);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["currentloc"]))
            {
                currentloc = Request.QueryString["currentloc"];
                filters.Add("currentloc", currentloc);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["annualsalary"]))
            {
                annualsalary = Request.QueryString["annualsalary"];
                filters.Add("annualsalary", annualsalary);

            }


            if (!string.IsNullOrEmpty(Request.QueryString["skill"]))
            {
                skill = Request.QueryString["skill"];
                filters.Add("skill", skill);
            }


            if (!string.IsNullOrEmpty(Request.QueryString["posi"]))
            {
                position = Request.QueryString["posi"];
                filters.Add("position", position);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["func"]))
            {
                function = Request.QueryString["func"];
                Session["Function"] = function;
                filters.Add("function", function);
            }
            else
            {
                Session["Function"] = "Find Candidates";
            }

            if (!string.IsNullOrEmpty(Request.QueryString["gen"]))
            {
                gender = Request.QueryString["gen"];
                filters.Add("gender", gender);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["minsalary"]))
            {
                minSalary = Request.QueryString["minsalary"];
                filters.Add("minsalary", minSalary);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["maxsalary"]))
            {
                maxSalary = Request.QueryString["maxsalary"];
                filters.Add("maxsalary", maxSalary);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["minexperience"]))
            {
                minExperience = Request.QueryString["minexperience"];
                filters.Add("minexperience", minExperience);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["maxexperience"]))
            {
                maxExperience = Request.QueryString["maxexperience"];
                filters.Add("maxexperience", maxExperience);
            }

            // to list out the role names
            List<string> lstRoles = new List<string>();
            //lstRoles = _repository.GetRolesForFindJobseekers();
            lstRoles = _repository.GetTitlesForFindJobseekers();

            // To list out the functions name

            List<string> lstFunctions = new List<string>();
            lstFunctions = _repository.GetFunctionsForRolesFindJobSeekers();


            List<SelectListItem> selectListRoles = new List<SelectListItem>();
            var lstCombined =lstRoles.Zip(lstFunctions, (role, func) => new { Role = role, Function = func }).ToList();
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

            IQueryable<Candidate> candidates = _repository.GetCandidates(what, where, currentloc, skill, position, function, gender, minSalary, maxSalary,annualsalary, minExperience, maxExperience);

            //-------DPR PLan for orderby candidates first----------//

            DateTime currentdate = Constants.CurrentTime();
            DateTime fresh = currentdate;
            //fresh = DateTime.Now.AddDays(-1);
            fresh = DateTime.Now.AddMinutes(-20);

            List<int> lstCandidateId = null;
            List<int> lstUpdatedCandidatesId = null;
            List<int> lstCreatedDatewiseCandidate = null;

            var ordercandidate = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.CandidateId != null && od.PlanName.ToLower().Contains("DPR".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.CandidateId.Value);
            var updatedCandidatesList = _repository.GetCandidates().Where(c => c.UpdatedDate != null && fresh <= c.UpdatedDate).Select(c => c.Id);
            var CandidatesListByCreatedDate = _repository.GetCandidates().Where(c => fresh <= c.CreatedDate).Select(c => c.Id);

            lstCandidateId = ordercandidate.ToList();
            lstUpdatedCandidatesId = updatedCandidatesList.ToList();
            lstCreatedDatewiseCandidate = CandidatesListByCreatedDate.ToList();

            Func<IQueryable<Candidate>, IOrderedQueryable<Candidate>> orderingFunc = query =>
            {
                if (ordercandidate.Count() > 0)
                    return query.OrderByDescending(rslt => lstCandidateId.Contains(rslt.Id)).ThenByDescending(rslt => lstUpdatedCandidatesId.Contains(rslt.Id)).ThenByDescending(rslt => lstCreatedDatewiseCandidate.Contains(rslt.Id));
                else if (updatedCandidatesList.Count() > 0)
                    return query.OrderByDescending(rslt => lstUpdatedCandidatesId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.UpdatedDate);

                else if (CandidatesListByCreatedDate.Count() > 0)
                    return query.OrderByDescending(rslt => lstCreatedDatewiseCandidate.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

                else
                    return query.OrderByDescending(rslt => rslt.CreatedDate);
            };

            candidates = orderingFunc(candidates);
            ViewData["Filters"] = filters;
            return GetPaginatedCandidates(page, candidates);
        }

        
        public bool SendSMSandMail(int candId, int jobId)
        {

            smsSent = "No";
            mailSent = "No";
            try
            {


                Job job = _repository.GetJob(jobId);


                Candidate candidate = _repository.GetCandidate(Convert.ToInt32(candId));


                //Send SMS
                string mobileNo = candidate.ContactNumber.ToString();
                if (mobileNo != null && mobileNo != "")
                {
                    SmsHelper.SendSecondarySms(
                              Constants.SmsSender.SecondaryUserName,
                               Constants.SmsSender.SecondaryPassword,
                               Constants.SmsBody.SMSVacancy
                               .Replace("[ORG_NAME]", job.Organization != null ? job.Organization.Name : "")
                               .Replace("[CONTACT_PERSON]", job.ContactPerson)
                               .Replace("[EMAIL]", job.EmailAddress)
                               .Replace("[MOBILE_NUMBER]", job.MobileNumber)
                               .Replace("[POSITION]", job.Position)
                               .Replace("[BASIC_QUALIFICATION]", null)
                               .Replace("[EXPERIENCE]", null)
                               .Replace("[LOCATION]", null)
                               .Replace("[GENDER]", job.Male == true && job.Female == true ? "Male, Female" : job.Male == true ? "Male" : job.Female == true ? "Female " : "")
                               ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                               mobileNo
                                );

                    smsSent = "Yes";

                }

                //Send mail

                StringBuilder jobmatchmaincontent = new StringBuilder();
                string EmailAddress = candidate.Email;
                if (EmailAddress != null && EmailAddress != "")
                {
                    jobmatchmaincontent.Append(
                                Constants.EmailBody.MatchingJobMain
                                .Replace("[DESCRIPTION]", job.Description)
                                .Replace("[POSITION]", job.Position)
                                .Replace("[COMPANY_NAME]", job.Organization != null ? job.Organization.Name : "")
                                .Replace("[INDUSTRY_TYPE]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "")
                                .Replace("[EXPERIENCE]", null)
                                .Replace("[POSTING_LOCATION]", null)
                                .Replace("[BASICQUALIFICATION]", null)
                                .Replace("[POSTGRADUATION]", null)
                                .Replace("[DOCTRATE]", null)
                                .Replace("[SKILLS]", null)
                                .Replace("[CONTACT_PERSON]", job.ContactPerson)
                                .Replace("[MOBILE]", job.MobileNumber)
                                .Replace("[LANDLINE]", job.ContactNumber)
                                .Replace("[EMAIL]", job.EmailAddress)
                                .Replace("[WEBSITE]", job.Organization != null ? job.Organization.Website : "")
                                );
                    EmailHelper.SendEmail(
                                Constants.EmailSender.EmployerSupport,
                                EmailAddress,
                                Constants.EmailSubject.JobMatch,
                                Constants.EmailBody.MatchingJobHeader
                                .Replace("[NAME]", candidate.Name)
                                + jobmatchmaincontent + Constants.EmailBody.MatchingJobFooter
                                );

                    mailSent = "Yes";
                }

                if (smsSent == "Yes" || mailSent == "Yes")
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                if (smsSent == "Yes" || mailSent == "Yes")
                    return true;
                else
                    return false;
            }

        }


        //For Jobsearch
        public ActionResult MatchingCandidates(int id, int? page)
        {
            var job = _repository.GetJob(id);

            if (job == null)
                return new FileNotFoundResult();

            IQueryable<Candidate> candidates = _repository.GetMatchingCandidates(job);

            return GetPaginatedCandidates(page, candidates);
        }

        //Search by Location
        public ActionResult Location(int id, int? page)
        {
            IQueryable<Candidate> candidates = _repository.GetCandidateByLocation(id);
            return GetPaginatedCandidates(page, candidates);
        }

        //Search By Skill
        public ActionResult Skill(int id, int? page)
        {
            IQueryable<Candidate> candidates = _repository.GetCandidateBySkill(id);
            return GetPaginatedCandidates(page, candidates);
        }

        //Detail of Candidate
        public ActionResult Details(int id)
        {
            Candidate candidate = _repository.GetCandidate(id);
            return View(candidate);
        }

        public ActionResult Profile()
        {
            if (LoggedInOrganization == null)
                //return new FileNotFoundResult();
                return RedirectToRoute("LogOn", "Account");


            var organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);

            /**Developer Note: To select a particular Industries for Employer Type**/
            List<SelectListItem> consultantindustries = new List<SelectListItem>();
            consultantindustries.Add(new SelectListItem { Text = "Home Needs", Value = "2378" });
            //consultantindustries.Add(new SelectListItem { Text = "Recruitment/Employment Consultants", Value = "2412" });
            ViewData["ConsultantIndustries"] = new SelectList(consultantindustries, "Value", "Text", (organization.IndustryId != null ? organization.IndustryId : 0));
            /*End*/

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            ViewData["NumberOfEmployees"] = new SelectList(_repository.GetEmployeesCount(), "Id", "Name", organization.EmployersCount);

            List<SelectListItem> OwnershipType = new List<SelectListItem>();
            OwnershipType.Add(new SelectListItem { Text = "Public Limited Company", Value = "1" });
            OwnershipType.Add(new SelectListItem { Text = "Private Limited Company", Value = "2" });
            OwnershipType.Add(new SelectListItem { Text = "Partnership", Value = "3" });
            OwnershipType.Add(new SelectListItem { Text = "Sole Proprietorship", Value = "4" });
            OwnershipType.Add(new SelectListItem { Text = "Professional Association", Value = "5" });
            ViewData["OwnershipType"] = new SelectList(OwnershipType, "Value", "Text", organization.OwnershipType);

            ///**Developer Note: To select a particular Industries for Employer Type**/
            //List<SelectListItem> consultantindustries = new List<SelectListItem>();
            //consultantindustries.Add(new SelectListItem { Text = "Home Needs", Value = "2378" });
            //if (organization.IndustryId == 2378)
            //{
            //    ViewData["ConsultantIndustries"] = new SelectList(consultantindustries, "Value", "Text", (organization.IndustryId != null ? organization.IndustryId : 0));
            //}
            //else
            //{
            //    /*End*/
            //    ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);
            //}

            Location location = organization.LocationId.HasValue ? _repository.GetLocationById(organization.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);

            return View(organization);
        }

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Save(FormCollection collection)
        {
            Organization organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");

            if (!string.IsNullOrEmpty(collection["Industries"]))
                organization.IndustryId = Convert.ToInt32(collection["Industries"]);
            organization.Name = collection["Name"];
            organization.ContactPerson = collection["ContactPerson"];
            organization.Email = collection["Email"];
            organization.Website = collection["Website"];
            organization.ContactNumber = collection["ContactNumber"];
            organization.MobileNumber = collection["MobileNumber"];
            organization.Address = collection["Address"];
            organization.Pincode = collection["Pincode"];
            organization.IPAddress = Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(collection["OwnershipType"]))
                organization.OwnershipType = Convert.ToInt32(collection["OwnershipType"]);

            if (!string.IsNullOrEmpty(collection["NumberOfEmployees"]))
                organization.EmployersCount = Convert.ToInt32(collection["NumberOfEmployees"]);

           

            if (organization.CreateDate == null)
            {
                organization.CreateDate = timeZone;
            }
            else
            {
                organization.UpdateDate = timeZone;
            }


            Location location = new Location();
            if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
            if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
            if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
            if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

            if (location.CountryId != 0)
                organization.LocationId = _repository.AddLocation(location);

            if (!TryValidateModel(organization))
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            _userRepository.Save();

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Profile has been updated with the IP Address of " + organization.IPAddress
            });
        }

        
        private ActionResult GetPaginatedCandidates(int? page, IQueryable<Candidate> candidates)
        {
            //more button clicked
            if (Request.IsAjaxRequest())
            {
                var paginatedCandidates = new PaginatedList<Candidate>(candidates, page ?? 0, PAGE_SIZE);

                if (paginatedCandidates.HasNextPage)
                    AddMoreUrlToViewData((page ?? 0) + 1);

                return View("Candidates", paginatedCandidates);
            }


            //initial page load
            var page1Candidates = new PaginatedList<Candidate>(candidates, 0, PAGE_SIZE);

            if (page1Candidates.HasNextPage)
                AddMoreUrlToViewData((page ?? 0) + 1);

            return View("Index", page1Candidates);
        }

        private void AddMoreUrlToViewData(int nextPage)
        {
            ViewData["moreUrl"] = Url.Action("Index", "Employer", new { page = nextPage });
        }

        public IQueryable<Candidate> candidates { get; set; }

             

        public ActionResult DashBoard()
        {
            if (LoggedInOrganization == null)
            {
                Response.Write("<script language=javascript>alert('You have logged out. Please login again..');</script>");
            }
            else
            {
                var organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);
            }
            return View();
        }

        //***********Viewed List From HORS*******
        public ActionResult ViewedCandidatesList()
        {
            return View();
        }

        public JsonResult ListContactViewedLogs(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<ContactsViewedLog> contactsviewedlog = null;
            if (LoggedInConsultant != null)
            {
                contactsviewedlog = _vasRepository.GetViewedLogs().Where(od => od.ConsultantId == LoggedInConsultant.Id);
            }
            else
            {
                   contactsviewedlog = _vasRepository.GetViewedLogs().Where(od => od.EmployerId == LoggedInOrganization.Id);
            }
           

            Func<IQueryable<ContactsViewedLog>, IOrderedQueryable<ContactsViewedLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.EmployerId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.ConsultantId);
                    else if (iSortCol_0 == 3)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.EmployerId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.OrderId);
                    else if (iSortCol_0 == 3)
                        return query.OrderByDescending(rslt => rslt.ConsultantId);
                    else
                        return query.OrderBy(rslt => rslt.OrderId);

                }

            };

            contactsviewedlog = orderingFunc(contactsviewedlog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                contactsviewedlog = contactsviewedlog.Where(o => o.EmployerId.ToString().Contains(sSearch.Trim()) || o.DateViewed.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                contactsviewedlog = contactsviewedlog.Where(o => o.DateViewed != null && o.DateViewed >= from && o.DateViewed <= to);

            }

            IEnumerable<ContactsViewedLog> contactsviewedlog1 = contactsviewedlog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            if (LoggedInOrganization!=null)
            {
                var result = new
                {
                    iTotalRecords = contactsviewedlog.Count(),
                    iTotalDisplayRecords = contactsviewedlog.Count(),
                    aaData = contactsviewedlog1.Select(o => new object[] { o.EmployerId, (_repository.GetOrganizationNameById(Convert.ToInt32(o.EmployerId))), (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateEmailById(Convert.ToInt32(o.CandidateId))), o.OrderId, (o.DateViewed != null) ? o.DateViewed.Value.ToString("dd-MM-yyyy") : "",(_repository.GetOrderByOrderId(Convert.ToInt32(o.OrderId)))  })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new
                {
                    iTotalRecords = contactsviewedlog.Count(),
                    iTotalDisplayRecords = contactsviewedlog.Count(),
                    aaData = contactsviewedlog1.Select(o => new object[] { o.ConsultantId, (_repository.GetConsultantNameById(Convert.ToInt32(o.ConsultantId))), (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateEmailById(Convert.ToInt32(o.CandidateId))), o.OrderId, (o.DateViewed != null) ? o.DateViewed.Value.ToString("dd-MM-yyyy") : "", (_repository.GetOrderByOrderId(Convert.ToInt32(o.OrderId))) })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /*Teleconference Count details for SS*/
        public ActionResult TeleConferenceCountForSS()
        {
            return View();
        }



      //  ********Alert Sent Details From RAT*********//

        public ActionResult AlertSentDetails()
        {
            return View();
        }


        public JsonResult ListAlertSentDetails(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<AlertsLog> alertsLog = null;
            if(LoggedInConsultant!=null)
            {
                alertsLog = _vasRepository.GetALertsLogs().Where(od => od.ConsultantId == LoggedInConsultant.Id && od.OrderMaster.OrderId == od.OrderId);
            }
            else
            {
                alertsLog = _vasRepository.GetALertsLogs().Where(od => od.OrganizationId == LoggedInOrganization.Id && od.OrderMaster.OrderId==od.OrderId);
            }

            Func<IQueryable<AlertsLog>, IOrderedQueryable<AlertsLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.AlertSentDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.MailSent);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.SmsSent);
                    else
                        return query.OrderByDescending(rslt => rslt.JobId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.AlertSentDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.MailSent);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.SmsSent);
                    else
                        return query.OrderByDescending(rslt => rslt.JobId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.OrganizationId.ToString().Contains(sSearch.Trim()) || o.AlertSentDate.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.AlertSentDate != null && o.AlertSentDate >= from && o.AlertSentDate <= to);

            }

            IEnumerable<AlertsLog> alertslog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();
            if (LoggedInOrganization != null)
            {
                var result = new
                {
                    iTotalRecords = alertsLog.Count(),
                    iTotalDisplayRecords = alertsLog.Count(),
                    aaData = alertslog1.Select(o => new object[] { o.OrderId, (_repository.GetOrganizationNameById(Convert.ToInt32(o.OrganizationId))), (_repository.GetJobById(Convert.ToInt32(o.JobId))), (o.AlertSentDate != null) ? o.AlertSentDate.Value.ToString("dd-MM-yyyy") : "", (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (o.SmsSent == true ? "Yes" : "No"), (o.MailSent == true ? "Yes" : "No"), ((_vasRepository.GetTeleConferenceCountByOrderId(Convert.ToInt32(o.OrderId))) != null ? (_vasRepository.GetTeleConferenceCountByOrderId(Convert.ToInt32(o.OrderId))) : 0) })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new
                {
                    iTotalRecords = alertsLog.Count(),
                    iTotalDisplayRecords = alertsLog.Count(),
                    aaData = alertslog1.Select(o => new object[] { o.OrderId, (_repository.GetConsultantNameById(Convert.ToInt32(o.ConsultantId))), (_repository.GetJobById(Convert.ToInt32(o.JobId))), (o.AlertSentDate != null) ? o.AlertSentDate.Value.ToString("dd-MM-yyyy") : "", (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (o.SmsSent == true ? "Yes" : "No"), (o.MailSent == true ? "Yes" : "No"), ((_vasRepository.GetTeleConferenceCountByOrderId(Convert.ToInt32(o.OrderId))) != null ? (_vasRepository.GetTeleConferenceCountByOrderId(Convert.ToInt32(o.OrderId))) : 0) })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PostedJobs()
        {
            if (LoggedInOrganization == null)
                return new FileNotFoundResult();

            var vacancy = _vasRepository.GetVacancies(LoggedInOrganization.Id);
            var status = _vasRepository.GetRatSubscribed(LoggedInOrganization.Id);
            var orderId = _vasRepository.GetPlanActivatedResultRAT(LoggedInOrganization.Id);
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(LoggedInOrganization.Id, orderId);

            if (status == true && vacancy != postedJobs.Count())
            {
                PostedJobAlert postedJobalert = new PostedJobAlert();
               
                foreach (Dial4Jobz.Models.Job job in LoggedInOrganization.Jobs)
                {
                    postedJobalert = _vasRepository.GetPostedJobAlert(LoggedInOrganization.Id, job.Id);
                    
                     if (postedJobalert!=null && postedJobalert.Vacancies > 0)
                     {
                         //check condition of posted jobs then update the minimum value into vacancies
                         if (postedJobs.Count() <= vacancy)
                         {
                             var vacancyCount = postedJobalert.Vacancies - postedJobs.Count();
                             postedJobalert.Vacancies = vacancyCount;
                             _vasRepository.Save();
                         }
                     }
                    
                }
                                

            }


            var organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            return View(organization);
        }





        public ActionResult SmsPurchase()
        {

            return View();
            
        }

        

        [HttpPost]
        public ActionResult SmsPurchase(string Plan, string Amount, string VasType)
        {
            //LoggedInOrganization = User.Identity.IsAuthenticated && IsEmployer ? _userRepository.GetOrganizationByUserName(User.Identity.Name) : null;
            

            VasPlan vasplan = _vasRepository.GetVasPlanbyPlanName(Plan);

            OrderMaster ordermaster = new OrderMaster();
            ordermaster.OrderDate = Constants.CurrentTime();

            if (LoggedInOrganization != null)
            {
                ordermaster.OrganizationId = LoggedInOrganization.Id;
            }
            else if (Session["LoginAs"] == "EmployerViaAdmin")
            {
                int adminOrganizationId = (int)Session["empId"];
                ordermaster.OrganizationId = adminOrganizationId;

            }

            ordermaster.Amount = vasplan.Amount;
            ordermaster.PaymentStatus = false;

            _vasRepository.AddOrderMaster(ordermaster);
            _vasRepository.Save();


            System.Web.HttpContext.Current.Session["VasOrderNo"] = ordermaster.OrderId;

            OrderDetail orderdetail = new OrderDetail();

            orderdetail.OrderId = ordermaster.OrderId;
            orderdetail.PlanId = vasplan.PlanId;
            orderdetail.PlanName = vasplan.PlanName;
            orderdetail.Amount = vasplan.Amount;
            orderdetail.ValidityCount = vasplan.ValidityCount;
            orderdetail.RemainingCount = vasplan.ValidityCount;
            orderdetail.Vacancies = vasplan.Vacancies;
            // orderdetail.PostedJobId = postedJobAlert.PostedJobId;

            _vasRepository.AddOrderDetail(orderdetail);
            _vasRepository.Save();

            //if (VasType == "Free Sms Purchase")
            //{
            //    EmailHelper.SendEmailBCC(
            //        Constants.EmailSender.EmployerSupport,
            //        ordermaster.Organization.Email,
            //        Constants.EmailSender.EmployerSupport,
            //        Constants.EmailSubject.SMSSubscription,
            //        Constants.EmailBody.SMSSubscriptionFree
            //        .Replace("[NAME]", ordermaster.Organization.Name)
            //        .Replace("[ID]", ordermaster.Organization.Id.ToString())
            //        .Replace("[EMAIL_ID]", ordermaster.Organization.Email)
            //        .Replace("[MOBILE_NO]", ordermaster.Organization.MobileNumber)
            //        .Replace("[AMOUNT]", "Free Compliment with Hot Resumes")
            //        .Replace("[ORDER_NO]", ordermaster.OrderId.ToString())
            //        .Replace("[PLAN]", vasplan.PlanName)
            //        .Replace("[DATE]", ordermaster.OrderDate.Value.ToString("dd-MM-yyyy"))
            //        );
            //}

            return Json(new JsonActionResult
            {
                Success = true,
                ReturnUrl="Employer/SmsPurchase",
                Message = Plan + " - subscribed"

            });


        }


        public ActionResult HireCandidates(int? page)
        {
            IQueryable<Candidate> candidates = _repository.GetCandidates();
            //more button clicked
            if (Request.IsAjaxRequest())
            {
                var paginatedJobs = new PaginatedList<Candidate>(candidates, page ?? 0, PAGE_SIZE);

                if (paginatedJobs.HasNextPage)
                    AddMoreUrlToViewData((page ?? 0) + 1);

                return View("Candidates", paginatedJobs);
            }

            //initial page load
            var page1Candidates = new PaginatedList<Candidate>(candidates, 0, PAGE_SIZE);

            if (page1Candidates.HasNextPage)
                AddMoreUrlToViewData((page ?? 0) + 1);

            return View(page1Candidates);

        }

        public ActionResult PhoneNoVerification()
        {
            ViewData["OrganizationId"] = LoggedInOrganization.Id;
            return View();
        }

        //Mobile Number Verification
        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult PhoneNoVerification(FormCollection collection)
        {
            int OrganizationId = 0;
            string phoneVerfication = string.Empty;
            if (collection != null && collection.AllKeys.Contains("OrganizationId"))
                OrganizationId = Convert.ToInt32(collection["OrganizationId"].ToString());
            var organization = _userRepository.GetOrganizationById(OrganizationId);
            if (collection != null && collection.AllKeys.Contains("OrganizationId"))
                phoneVerfication = collection["PhVerificationNo"].ToString();

            if (string.IsNullOrEmpty(phoneVerfication))
                return Json(new JsonActionResult { Success = true, Message = LoggedInOrganization.UserName, ReturnUrl = VirtualPathUtility.ToAbsolute("~/Employer/Profile/") });
            else if (!string.Equals(phoneVerfication, LoggedInOrganization.PhoneVerificationNo.ToString()))
                return Json(new JsonActionResult { Success = true, Message = LoggedInOrganization.UserName, ReturnUrl = VirtualPathUtility.ToAbsolute("~/Employer/Profile/") });
            else
                if (!string.IsNullOrEmpty(collection["IsPhoneVerified"]))
                    LoggedInOrganization.IsPhoneVerified = Convert.ToBoolean(collection.GetValues("IsPhoneVerified").Contains("true"));

            _userRepository.Save();


            return Json(new JsonActionResult
            {
                Success = true,
                Message = LoggedInOrganization.UserName,
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/Employer/Profile/")

            });

            //}
        }

        public ActionResult VerifyOrganizationMobile(FormCollection collection)
        {
            Organization organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);
            Random randomNo = new Random();
            string verificationNumber = randomNo.Next(1000, 9999).ToString();
            organization.PhoneVerificationNo = Convert.ToInt32(verificationNumber);
            _userRepository.Save();

            SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSMobileVerification
                                        .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                        organization.MobileNumber
                        );
            //SmsHelper.Sendsms(
            //            Constants.SmsSender.UserId,
            //            Constants.SmsSender.Password,
            //            Constants.SmsBody.SMSMobileVerification
            //                            .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

            //            Constants.SmsSender.Type,
            //            Constants.SmsSender.senderId,
            //            organization.MobileNumber
            //            );

            return RedirectToAction("PhoneNoVerification", "Employer");
        }



        public ActionResult Activation(string Id)
        {
            Organization organization = null;
            if (!string.IsNullOrEmpty(Id))
            {
                Id = Constants.DecryptString(Id);
                organization = _userRepository.GetOrganizationById(Convert.ToInt32(Id));
                organization.IsMailVerified = true;
                _userRepository.Save();
            }

            if (organization != null)
            {
                var employerVerification = _repository.GetOrganization(organization.Id);
                if (employerVerification.IsMailVerified == null)
                {
                    Session["EmployerVerification"] = false;
                }
                else
                {
                    Session["EmployerVerification"] = true;
                }
            }

            return RedirectToAction("MatchCandidates", "Employer");
          
        }

        

        public ActionResult EmailVerification()
        {
            Organization organization = _userRepository.GetOrganizationById(LoggedInOrganization.Id);
            var id = Constants.EncryptString(organization.Id.ToString());
            EmailHelper.SendEmail(
                    Constants.EmailSender.EmployerSupport,
                    organization.Email,
                    Constants.EmailSubject.EmailVerification,
                    Constants.EmailBody.EmailVerification
                        .Replace("[EMAIL]", organization.Email)
                        .Replace("[LINK_NAME]", "Verify Email")                                                
                        .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + id)
                        );

            return RedirectToAction("Profile", "Employer");
        }

    }
}
