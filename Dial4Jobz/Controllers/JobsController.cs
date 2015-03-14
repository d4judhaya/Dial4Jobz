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
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Configuration;


namespace Dial4Jobz.Controllers
{
    public class JobsController : BaseController
    {
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
        List<string> _filters = new List<string>();
        

        public ActionResult Index(FormCollection collection, int? page)
        {

            if (LoggedInCandidate != null)
            {
                var candidateVerification = _repository.GetCandidate(LoggedInCandidate.Id);
                if (candidateVerification.IsPhoneVerified == null)
                {
                    Session["PhoneVerification"] = true;
                }
                else
                {
                    Session["PhoneVerification"] = false;
                }
            }
           

                string what = string.Empty;
                string where = string.Empty;
                string skill = string.Empty;
                string position = string.Empty;
                string organization = string.Empty;
                string minSalary = string.Empty;
                string maxSalary = string.Empty;
                string minExperience = string.Empty;
                string maxExperience = string.Empty;
                string function = string.Empty;
                string preferredtype = string.Empty;
                string Freshness = string.Empty;

                Dictionary<string, string> filters = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(Request.QueryString["what"]))
                {
                    what = Request.QueryString["what"];
                    filters.Add("what", what);
                }

                string Flag = string.Empty;
                
                if (!string.IsNullOrEmpty(Request.QueryString["Flag"]))
                {
                    Flag = Request.QueryString["Flag"];
                    // filters.Add("Flag", Flag);
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

                if (!string.IsNullOrEmpty(Request.QueryString["skill"]))
                {
                    skill = Request.QueryString["skill"];
                    filters.Add("skill", skill);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["pos"]))
                {
                    position = Request.QueryString["pos"];
                    filters.Add("position", position);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["org"]))
                {
                    organization = Request.QueryString["org"];
                    filters.Add("organization", organization);
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

                if (!string.IsNullOrEmpty(Request.QueryString["function"]))
                {
                    function = Request.QueryString["function"];
                    filters.Add("function", function);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["pretype"]))
                {
                    preferredtype = Request.QueryString["pretype"];
                    filters.Add("preferredtype", preferredtype);
                }

                ViewData["Filters"] = filters;

                IQueryable<Job> jobs = _repository.GetJobs(what, where, skill, position, organization, minSalary, maxSalary, minExperience, maxExperience, function, preferredtype);


                //udhaya orderby activated jobs
                DateTime currentdate = Constants.CurrentTime().Date;
                List<int> lstOrganizationId = null;
                var orderOrganization = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderId == od.OrderMaster.OrderId && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.OrganizationId.Value);
                //var orderOrganization = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.PlanName.ToLower().Contains("HORS".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.OrganizationId.Value);

                lstOrganizationId = orderOrganization.ToList();

                Func<IQueryable<Job>, IOrderedQueryable<Job>> orderingFunc = query =>
                {
                    if (orderOrganization.Count() > 0)
                        return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => rslt.CreatedDate);
                    else
                        return query.OrderByDescending(rslt => rslt.CreatedDate);
                };

                jobs = orderingFunc(jobs);
                            
                return GetPaginatedJobs(page, jobs);
            
        }

        public ActionResult MatchingJobsForCandidate(int id, int? page)
        {
            var candidate = _repository.GetCandidate(id);
            var job = _repository.GetJob(id);
            if (candidate == null)
                return new FileNotFoundResult();

            IQueryable<Job> jobs = _repository.GetMatchingJobs(candidate);
            EmailHelper.SendEmail(
              Constants.EmailSender.EmployerSupport,
              job.EmailAddress,
              Constants.EmailSubject.CandidateMatch,
              Constants.EmailBody.CandidateMatch
                //.Replace("[NAME]", candidate.Name)
                        .Replace("[POSITION]", job.Position)
                        .Replace("[CANDIDATE_NAME]", candidate.Name)
                        .Replace("[MOBILE_NUMBER]", candidate.MobileNumber)
                        .Replace("[EMAIL]", candidate.Email)
                        .Replace("[ADDRESS]", candidate.Address)
                        .Replace("[QUALIFICATION]", candidate.CandidateQualifications.Select(cq => cq.Degree.Id).ToString())
                        .Replace("[TOTAL_EXPERIENCE]", candidate.TotalExperience.ToString())
                        .Replace("[ANNUAL_SALARY]", candidate.AnnualSalary.ToString())
                        .Replace("[LOCATION]", candidate.LocationId.ToString())
                        .Replace("[PRESENT_COMPANY]", candidate.PresentCompany)
                        .Replace("[PREVIOUS_COMPANY]", candidate.PresentCompany)
                        .Replace("[LANGUAGE]", candidate.CandidateLanguages.Select(cl => cl.Language.Id).ToString())
                        );

            return GetPaginatedJobs(page, jobs);
        }

        public ActionResult JobPostVas()
        {
            return View();
        }


        public JsonResult getBasicQualification()
        {
            var result = new
            {
                BasicQualification = _repository.GetDegreesEnumerable(DegreeType.BasicQualification)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPostGraduation()
        {
            var result = new
            {
                PostGraduation = _repository.GetDegreesEnumerable(DegreeType.PostGraduation)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDoctorate()
        {
            var result = new
            {
                Doctorate = _repository.GetDegreesEnumerable(DegreeType.Doctorate)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCountry()
        {
            var result = new
            {
                Country = _repository.GetCountriesList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getotherCountry(Int32 Id)
        {
            var result = new
            {
                OtherCountry = _repository.GetOtherCountries(Id)
                
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getstatebyCountryId(Int32 Id)
        {
            var result = new
            {
                State = _repository.GetStatebyCountryIdwithCountryname(Id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCitybyStateId(Int32 Id)
        {
            var result = new
            {
                City = _repository.GetCitybyStateIdwithStatename(Id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRegionbyCityId(Int32 Id)
        {
            var result = new
            {
                Region = _repository.GetRegionbyCityIdwithCityname(Id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            
            int empId = LoggedInOrganization.Id;

            var vacancies = _vasRepository.GetVacancies(LoggedInOrganization.Id);
            var planActivated = _vasRepository.PlanSubscribed(LoggedInOrganization.Id);
            var planActivatedDetails = _vasRepository.PlanActivatedDetails(LoggedInOrganization.Id);
            var orderId = _vasRepository.GetPlanActivatedResultRAT(LoggedInOrganization.Id);
            
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(LoggedInOrganization.Id,orderId);

            //var expired = _vasRepository.GetRATExpiredByDate(LoggedInOrganization.Id);

            if (planActivated == false && orderId!=0)
            {
                ViewData["Message"] = "Your Subscription for" + planActivatedDetails.PlanName + "It's Pending activation. On receipt of payment, your plan will be activated within 1 working day.";
            }

            else if (orderId == 0)
            {
                ViewData["Message"] = "If you want Suitable Resumes for the Vacancy, Subscribe for Resume Alert.";
            }

            else if (planActivated == true)
            {
                ViewData["Message"] = "Your " + planActivatedDetails.PlanName + " is active. You can activate" + vacancies + " vacancies now.";
            }
            else
            {
                ViewData["Message"] = "If you want Suitable Resumes for the Vacancy, Subscribe for Resume Alert.";
            }


            SetCommonViewData();
            SetAddJobViewData();
            return View();
        }

       

        
        public ActionResult Edit(int id)
        {
            var job = _repository.GetJob(id);

            if (job == null)
                return new FileNotFoundResult();

            SetCommonViewData();
            SetEditJobViewData(job);

            return View(job);
        }

        [Authorize,HttpPost]
        public ActionResult ActivateRATVacancy(int id)
        {
            var job = _repository.GetJob(id);
            var vacancies = _vasRepository.GetVacancies(LoggedInOrganization.Id);
            var orderId = _vasRepository.GetPlanActivatedResultRAT(LoggedInOrganization.Id);
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(LoggedInOrganization.Id, orderId);
            bool isSuccess = false;
            if (LoggedInOrganization != null && postedJobs.Count()==0)
            {
                   _vasRepository.UpdateVASDetails(LoggedInOrganization.Id, job.Id);
                   PostedJobAlert postedJobalert = _vasRepository.GetPostedJobAlert(LoggedInOrganization.Id, job.Id);
                    postedJobalert.Vacancies = vacancies;
                    _vasRepository.Save();
                    isSuccess = true;
                
            }
            else if (LoggedInOrganization != null && postedJobs.Count() <= vacancies)
            {
                _vasRepository.UpdateVASDetails(LoggedInOrganization.Id, job.Id);
                PostedJobAlert postedJobalert = _vasRepository.GetPostedJobAlert(LoggedInOrganization.Id, job.Id);
                postedJobalert.Vacancies = vacancies;
                _vasRepository.Save();
                isSuccess = true;

            }

            else
            {
                ViewData["Message"] = "Your Subscription for RAT plan has expired. For Details Please check Subscription Details in your Account";
                //Response.Write("<script language=javascript>alert('Your Subscription for RAT plan has expired. For Details Please check Subscription Details in your Account');</script>");
            }

            if (postedJobs.Count() >= vacancies)
            {
                ViewData["Message"] = "You have already activated all vacancies. Please buy RAT again..";
                return RedirectToAction("PostedJobs", "Employer");
            }

            else if (isSuccess == true)
            {
                ViewData["Message"] = "Vacancy activated Successfully. Now You will receive alerts..";
                return RedirectToAction("PostedJobs", "Employer");
            }

            else
            {
                ViewData["Message"] = "Activation is not successful. Please try again";

            }

            return RedirectToAction("PostedJobs", "Employer");
        }
             

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Add(FormCollection collection)
        {
            Job job = new Job();
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");

            job.CreatedDate = timeZone;
            job.OrganizationId = LoggedInOrganization.Id;

            long minannualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhsMin"]) * 100000;
            long minannualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousandsMin"]) * 1000;

            long annualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhs"]) * 100000;
            long annualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousands"]) * 1000;
                                    
            
            if (SetJobDetails(job, collection))
            {
                if (job.ContactPerson == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Contact Person is required" });
                }

                if (job.Position == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Position is required" });
                }

                if (job.MobileNumber == "" && job.EmailAddress == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Mobile/Email is required. Enter any one." });
                }
                else if (job.JobLocations.Count() == 0)
                {
                    return Json(new JsonActionResult { Success = false, Message = "Location is Required." });
                }

                if (minannualSalaryLakhs > annualSalaryLakhs)
                    return Json(new JsonActionResult { Success = false, Message = "Minimum Salary Should not greater than Maximum" });


                else
                {
                    _repository.Save();


                    if (job.CommunicateViaEmail == true && job.EmailAddress != null)
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        job.EmailAddress,
                        Constants.EmailSubject.ClientPost,
                           Constants.EmailBody.ClientPost
                                  .Replace("[NAME]", job.ContactPerson)
                                  .Replace("[IMAGE_URL]", Url.Content("~/Content/Images/dial4jobz_logo.png"))
                                  );
                    }
                                       

                    if (job.CommunicateViaSMS == true && job.MobileNumber != null)
                    {
                        var gender = Convert.ToString(job.Male);
                        if (job.Male == true)
                            gender = "Male";
                        else
                            gender = "Female";

                        string preferredall = string.Empty;
                        string preferredcontract = string.Empty;
                        string preferredparttime = string.Empty;
                        string preferredfulltime = string.Empty;
                        string preferredworkfromhome = string.Empty;

                        if (job.PreferredAll == true)
                        {
                            preferredall = "All Type";
                        }

                        else if (job.PreferredContract == true)
                        {
                            preferredcontract = "Contract";
                        }

                        else if (job.PreferredParttime == true)
                        {
                            preferredparttime = "Part Time";
                        }

                        else if (job.PreferredFulltime == true)
                        {
                            preferredfulltime = "Full Time";
                        }

                        else
                        {
                            preferredworkfromhome = "Work From Home";
                        }


                        if (LoggedInOrganization.MobileNumber != null || LoggedInOrganization.MobileNumber != "")
                        {
                            /*Job Details send to Employer to verify*/
                            string industry = string.Empty;

                            foreach (JobPreferredIndustry jpi in job.JobPreferredIndustries)
                            {
                                if (industry == string.Empty)
                                    industry = jpi.IndustryId.ToString();
                                else
                                    industry += "," + jpi.IndustryId;
                            }

                            string jobminexp = string.Empty;
                            string jobmaxexp = string.Empty;
                            //string maxExp = string.Empty;
                            if ((!job.MinExperience.HasValue || job.MinExperience == 0) && (!job.MaxExperience.HasValue || job.MaxExperience == 0))
                            {

                            }
                            else if (!job.MinExperience.HasValue || job.MinExperience == 0)
                            {
                                jobmaxexp = "Up to" + Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years ";
                            }
                            else if (!job.MaxExperience.HasValue || job.MaxExperience == 0)
                            {
                                jobminexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + "+ Years";
                            }
                            else
                            {
                                jobminexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + " " +
                                Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years";
                            }

                            string minSalary = string.Empty;
                            string maxSalary = string.Empty;
                            if (job.Budget != null)
                            {
                                minSalary = job.Budget.ToString();
                            }
                            if (job.MaxBudget != null)
                            {
                                maxSalary = job.MaxBudget.ToString();
                            }

                            string role = string.Empty;

                            if (job.JobRoles != null)
                            {
                                foreach (JobRole jr in job.JobRoles)
                                {
                                    if (role == string.Empty)
                                    {
                                        role = jr.Role.Name;
                                    }
                                    else
                                    {

                                    }
                                }
                            }


                            string joblanguages = string.Empty;

                            if (job.JobLanguages != null)
                            {
                                foreach (JobLanguage jl in job.JobLanguages)
                                {
                                    if (joblanguages == string.Empty)
                                    {
                                        joblanguages = jl.Language.Name;
                                    }
                                    else
                                    {

                                    }
                                }
                            }

                            string jobCity = string.Empty;
                            string jobRegion = string.Empty;
                            string jobState = string.Empty;
                            string jobCountry = string.Empty;

                            string preferredlocations = string.Empty;
                            if (job.JobLocations != null)
                            {
                                foreach (JobLocation jl in job.JobLocations)
                                {
                                    if (jobCity == string.Empty)
                                    {
                                        if (jl.Location.City != null)
                                        {
                                            jobCity = jl.Location.City.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (jobState == string.Empty)
                                    {
                                        if (jl.Location.City != null)
                                        {
                                            jobState = jl.Location.City.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (jobRegion == string.Empty)
                                    {
                                        if (jl.Location.Region != null)
                                        {
                                            jobRegion = jl.Location.Region.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (jobCountry == string.Empty)
                                    {
                                        if (jl.Location.Country != null)
                                        {
                                            jobCountry = jl.Location.Country.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                }
                            }
                            string jobbasicqualification = string.Empty;
                            string jobpostgraduation = string.Empty;
                            string jobdoctrate = string.Empty;
                            foreach (Dial4Jobz.Models.JobRequiredQualification cq in job.JobRequiredQualifications)
                            {
                                if (cq.Degree.Type == 0)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobbasicqualification += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobbasicqualification += cq.Degree.Name + ",";
                                    }
                                }
                                if (cq.Degree.Type == 1)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobpostgraduation += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobpostgraduation += cq.Degree.Name + ",";
                                    }
                                }
                                if (cq.Degree.Type == 2)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobdoctrate += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobdoctrate += cq.Degree.Name + ",";
                                    }
                                }
                            }

                            SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.JobPostingDetails
                            .Replace("[POSITION]", job.Position)
                            .Replace("[BASIC_QUALIFICATION]", (jobbasicqualification != "" ? jobbasicqualification : "" + jobpostgraduation != "" ? jobpostgraduation : "" + jobdoctrate != "" ? jobdoctrate : ""))
                            .Replace("[FUNCTIONAL_AREA]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "Not Mentioned")
                            .Replace("[ROLE]", (role != "" ? role : "Not Mentioned"))
                            .Replace("[PREFERRED_INDUSTRY]", (industry != "" ? industry : "NA"))
                            .Replace("[MINEXP]", jobminexp)
                            .Replace("[MAXEXP]", " to" + jobmaxexp)
                            .Replace("[ANNUAL_SALARY]", minSalary + "to" + maxSalary)
                            .Replace("[COUNTRY]", (jobCountry != "" ? jobCountry : "") + ",")
                            .Replace("[STATE]", (jobState != "" ? jobState : "") + ",")
                            .Replace("[CITY]", (jobCity != "" ? jobCity : "") + ",")
                            .Replace("[AREA]", (jobRegion != "" ? jobRegion : ""))
                            .Replace("[PREFERRED_LANGUAGES]", (joblanguages.Count() > 0 ? joblanguages + "," : ""))
                            .Replace("[PREFERRED_TYPE]", (preferredall != "" ? preferredall : "" + preferredcontract != "" ? preferredcontract : "" + preferredfulltime != "" ? preferredfulltime : "" + preferredparttime != "" ? preferredparttime : "" + preferredworkfromhome != "" ? preferredworkfromhome : ""))
                            .Replace("[GENDER]", gender),
                             Constants.SmsSender.SecondaryType,
                             Constants.SmsSender.Secondarysource,
                             Constants.SmsSender.Secondarydlr,
                             job.MobileNumber
                            );

                        }

                        /*End employer send sms verify*/
                    }

                       
                }


                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Vacancy has been updated",
                    ReturnUrl = "/Jobs/Details/" + Constants.EncryptString(job.Id.ToString())
                });
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            }
        }




        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Save(FormCollection collection, int jobId)
        {
            Job job = _repository.GetJob(jobId);
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");

            job.UpdatedDate = timeZone;

            if (SetJobDetails(job, collection))
            {
                _repository.Save();
               
                
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Vacancy has been updated",
                    ReturnUrl = "/Employer/CandidateMatchesJob/CandidateMatch/" + job.Id.ToString()
                });
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            }
        }

        // [ValidateAntiForgeryToken]
        public ActionResult Details(string id)
        {
            if (ModelState.IsValid)
            {
                id = Constants.DecryptString(id.ToString());
                int Id;
                bool ValueIsAnId = int.TryParse(id, out Id);
                Job job = _repository.GetJob(Id);
                if (LoggedInCandidate != null)
                {
                    _repository.GetLogForJobsViewByCandidate(LoggedInCandidate.Id, job.Id, 0);
                }
                if (LoggedInConsultant != null)
                {
                    _repository.GetLogForJobsViewByCandidate(0, job.Id, LoggedInConsultant.Id);//doubt 
                }
                return View(job);
            }
            return View("Index", "Jobs");

        }

        public ActionResult JobsViewedList(int organizationId)
        {
            if (organizationId != null)
            {
                ViewData["OrganizationId"] = organizationId;
            }
            else
            {
                ViewData["OrganizationId"] = LoggedInOrganization.Id;
            }
            return View();
        }

        public JsonResult ListJobsViewedList(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate, int organizationId)
        {
            IQueryable<JobsLog> alertsLog = _vasRepository.GetJobsLogs().Where(od => od.OrganizationId == organizationId);
            OrderDetail getOrderDetails = _vasRepository.GetOrderDetailsForHORS(organizationId);

            Func<IQueryable<JobsLog>, IOrderedQueryable<JobsLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.JobViewedDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    //else if (iSortCol_0 == 3)
                       // return query.OrderByDescending(rslt => rslt.ConsultantId);
                    else
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.JobViewedDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    //else if (iSortCol_0 == 3)
                       //return query.OrderByDescending(rslt => rslt.ConsultantId);
                    else
                        return query.OrderByDescending(rslt => rslt.CandidateId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.CandidateId.ToString().Contains(sSearch.Trim()) || o.JobViewedDate.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.JobViewedDate != null && o.JobViewedDate >= from && o.JobViewedDate <= to);

            }

            IEnumerable<JobsLog> alertsLog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = alertsLog.Count(),
                iTotalDisplayRecords = alertsLog.Count(),

                aaData = alertsLog1.Select(o => new object[] { (_repository.GetJobById(Convert.ToInt32(o.JobId))), o.CandidateId, (_repository.GetCandidateNameById(o.CandidateId)), (o.JobViewedDate.ToString("dd-MM-yyyy")), (getOrderDetails!=null ? (_repository.GetCandidateEmailById(o.CandidateId)): "Subscribe Plans"), (getOrderDetails!=null ? (_repository.GetCandidateContactNumberById(o.CandidateId)): "") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
               


        //public ActionResult AfterJobPost(int id)
        //{
        //    Job job = _repository.GetJob(id);
        //    return View(job);
            
        //}

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Job job = _repository.GetJob(id);
            if (job != null)
                return View();
            else
                return RedirectToAction("PostedJobs", "Employer");
        }

             
        [HttpPost,Authorize]
        public ActionResult Delete(int id, string confirm)
        {
            Job job = _repository.GetJob(id);
            

                if (job != null)
                {
                    _repository.DeleteJobLanguages(id);
                    _repository.DeleteJobLocations(id);
                    _repository.DeleteJobPreferredIndustries(id);
                    _repository.DeleteJobRequiredQualifications(id);
                    _repository.DeleteJobRoles(id);
                    _repository.DeleteJobskills(id);
                    _repository.DeleteJobLicenseTypes(id);
                    _repository.DeleteJob(id);
                }
            //}

                return RedirectToAction("PostedJobs", "Employer");

        }

       


        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Send(SendMethod sendMethod, string jobId)
        {
            LoggedInCandidate = User.Identity.IsAuthenticated && !IsEmployer ? _userRepository.GetCandidateByUserName(User.Identity.Name) : null;

            if (LoggedInCandidate != null)
            {
                Candidate candidate = _repository.GetCandidate(Convert.ToInt32(LoggedInCandidate.Id));
                string experience = (candidate.TotalExperience.HasValue && candidate.TotalExperience != 0) ? (candidate.TotalExperience.Value / 31104000).ToString() + " Years " + (((candidate.TotalExperience.Value - (candidate.TotalExperience.Value / 31104000) * 31536000)) / 2678400) + " Months" : "";
                string annualsalary = (candidate.AnnualSalary.HasValue && candidate.AnnualSalary != 0) ? Convert.ToInt32(candidate.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN")) : "";
                string industry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.GetIndustry(candidate.IndustryId.Value).Name : "";
                string languages = string.Empty;
                foreach (Dial4Jobz.Models.CandidateLanguage cla in candidate.CandidateLanguages)
                {
                    languages += cla.Language.Name + ",";
                }

                string basicqualification = string.Empty;
                string postgraduation = string.Empty;
                string doctrate = string.Empty;
                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications)
                {
                    if (cq.Degree.Type == 0)
                    {
                        basicqualification += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                    }
                    if (cq.Degree.Type == 1)
                    {
                        postgraduation += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                    }
                    if (cq.Degree.Type == 2)
                    {
                        doctrate += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                    }
                }

                string skills = string.Empty;

                foreach (CandidateSkill cs in candidate.CandidateSkills)
                {
                    skills += cs.Skill.Name + ",";
                }

                string licensetypes = string.Empty;

                foreach (CandidateLicenseType clt in candidate.CandidateLicenseTypes)
                {
                    licensetypes  =clt.LicenseType.Name;
                }

                string preferredTypes = string.Empty;

                if (candidate.PreferredAll == true)
                    preferredTypes = "Any, ";

                if (candidate.PreferredContract == true)
                    preferredTypes += " Contract, ";

                if (candidate.PreferredParttime == true)
                {
                    preferredTypes += "Part Time, ";
                }

                if (candidate.PreferredFulltime == true)
                    preferredTypes += "Full Time, ";

                if (candidate.PreferredWorkFromHome == true)
                    preferredTypes += "Work from home, ";

                string location = string.Empty;
                if (candidate.LocationId != null)
                {
                    if (candidate.GetLocation(candidate.LocationId.Value).CityId.HasValue && candidate.GetLocation(candidate.LocationId.Value).CityId != 0)
                    {
                        location = candidate.GetLocation(candidate.LocationId.Value).City.Name + ",";
                    }
                    if (candidate.GetLocation(candidate.LocationId.Value).CountryId != null && candidate.GetLocation(candidate.LocationId.Value).CountryId != 0)
                    {
                        location = candidate.GetLocation(candidate.LocationId.Value).Country.Name;
                    }
                }

                DateTime birth = DateTime.Parse(candidate.DOB.Value.ToShortDateString());
                DateTime today = DateTime.Today;
                int age = today.Year - birth.Year;    //people perceive their age in years
                if (
                   today.Month < birth.Month
                   ||
                   ((today.Month == birth.Month) && (today.Day < birth.Day))
                   )
                {
                    age--;  //birthday in current year not yet reached, we are 1 year younger ;)
                    //+ no birthday for 29.2. guys ... sorry, just wrong date for birth
                }


                if (!string.IsNullOrEmpty(jobId))
                {
                    Job job = _repository.GetJob(Convert.ToInt32(jobId));

                    if (job == null)
                        return new FileNotFoundResult();

                    if (sendMethod == SendMethod.SMS || sendMethod == SendMethod.Both)
                    {
                        //send SMS

                        var smsStatus = _vasRepository.GetCandidateSmsVas(LoggedInCandidate.Id);

                        if (smsStatus != null)
                        {
                            SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.SMSResume
                                .Replace("[NAME]", candidate.Name)
                                .Replace("[VACANCY]",job.Position)
                                .Replace("[EMAIL]", candidate.Email)
                                .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                .Replace("[QUALIFICATION]", basicqualification)
                                .Replace("[FUNCTION]", candidate.FunctionId == null || job.FunctionId == 0 ? "" : candidate.Function.Name)
                                .Replace("[DESIGNATION]", candidate.Position)
                                .Replace("[PRESENT_SALARY]", annualsalary+" per annum")
                                .Replace("[LOCATION]", location != "" ? location : "Any where in India")
                                .Replace("[DOB]", candidate.DOB.HasValue ? age.ToString() + " years old" : String.Empty)
                                .Replace("[TOTAL_EXPERIENCE]", experience)
                                .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female")
                                ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                                job.MobileNumber
                             );
                        }

                    }

                    if (sendMethod == SendMethod.Email || sendMethod == SendMethod.Both)
                    {
                        //send Email

                        EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        job.EmailAddress,
                        Constants.EmailSubject.CandidateMatch,
                        Constants.EmailBody.MatchingCandidate
                        .Replace("[ORG_NAME]", job.Organization != null ? job.Organization.Name : job.ContactPerson)
                        .Replace("[JOBNAME]", job.DisplayPosition != null ? job.DisplayPosition : "Not Mentioned")
                        .Replace("[CANDIDATENAME]", candidate.Name != null ? candidate.Name : "Not Mentioned")
                        .Replace("[MOBILE]", candidate.ContactNumber != null ? candidate.ContactNumber : "Not Mentioned")
                        .Replace("[LANDLINE]", candidate.MobileNumber != null ? candidate.MobileNumber : "Not Mentioned")
                        .Replace("[EMAIL]", candidate.Email != null ? candidate.Email : "Not Mentioned")
                        .Replace("[ADDRESS]", candidate.Address != null ? candidate.Address : "Not Mentioned")
                        .Replace("[BASICQUALIFICATION]", basicqualification != "" ? basicqualification : "Not Mentioned")
                        .Replace("[POSTGRADUATION]", postgraduation != "" ? postgraduation : "Not Mentioned")
                        .Replace("[DOCTRATE]", doctrate != "" ? doctrate : "Not Mentioned")
                        .Replace("[EXPERIENCE]", experience !="" ? experience + "Years" : "Not Mentioned")
                        .Replace("[SPOT_TEXT] ","")
                        .Replace("[TEXT_VACANCY]", "")
                        .Replace("[TEXT_MATCH]", "Find my resume for " + job.Position)
                        .Replace("[DOB]", candidate.DOB.HasValue ? age.ToString() + " years old" : String.Empty)
                        .Replace("[INDUSTRY]", industry==null ? "Not Mentioned" : industry)
                        .Replace("[FUNCTION]", candidate.FunctionId == null || job.FunctionId == 0 ? "" : candidate.Function.Name)
                        .Replace("[SKILLS]", skills !="" ? skills : "Not Mentioned")
                        .Replace("[ANNUAL_SALARY]", annualsalary != "" ? annualsalary : "Not Mentioned")
                        .Replace("[LOCATION]", location=="" ? "Not Mentioned": location)
                        .Replace("[PRESENT_COMPANY]", candidate.PresentCompany)
                        .Replace("[PREVIOUS_COMPANY]", candidate.PreviousCompany)
                        .Replace("[LICENSE_TYPES]", licensetypes)
                        .Replace("[LANGUAGE]", languages!="" ? languages : "Not Mentioned")
                        .Replace("[PREFERENCES]", "Not Mentioned")
                        .Replace("[DOWNLOAD_RESUME]", "<a href='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/DownloadResumeFromJobMatches?fileName=" + candidate.ResumeFileName + "'>Download Resume</a>")
                        .Replace("[PREFERRED_TYPE]", preferredTypes)
                        );

                    }
                }

            }

            string msg = sendMethod == SendMethod.SMS ? "SMS" : sendMethod == SendMethod.Email ? "Email" : "SMS / Email";

            return Json(new JsonActionResult { Success = true, Message = "Successfully Applied for the Vacancy by " + msg });

        }
        public ActionResult Roles(int functionId)
        {
            var roles = _repository.GetRoles(functionId).ToList();
            var jobRoles = _repository.GetRoleByFunctionId(functionId);
            var jsonRoles = new List<JsonRole>();
            foreach (Role role in jobRoles)
            {
                JsonRole jsonRole = new JsonRole();
                jsonRole.Id = role.Id;
                jsonRole.Name = role.Name;
                jsonRole.FunctionId = role.FunctionId;
                jsonRoles.Add(jsonRole);
            }

            return Json(new JsonRolesResult { Success = true, Roles = jsonRoles }, JsonRequestBehavior.AllowGet);
        }        

        #region Helper Methods     

        private bool SetJobDetails(Job job, FormCollection collection)
        {


            if (!string.IsNullOrEmpty(collection["Functions"]))
                job.FunctionId = Convert.ToInt32(collection["Functions"]);

            job.Position = collection["Position"];

            if (!string.IsNullOrEmpty(collection["Male"]))
                job.Male = Convert.ToBoolean(collection.GetValues("Male").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Female"]))
                job.Female = Convert.ToBoolean(collection.GetValues("Female").Contains("true"));

            long yearsInSecondsFrom = Convert.ToInt64(collection["ddlTotalExperienceYearsFrom"]) * 365 * 24 * 60 * 60;
            long yearsInSecondsTo = Convert.ToInt64(collection["ddlTotalExperienceYearsTo"]) * 365 * 24 * 60 * 60;
            job.MinExperience = yearsInSecondsFrom;
            job.MaxExperience = yearsInSecondsTo;

            long minannualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhsMin"]) * 100000;
            long minannualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousandsMin"]) * 1000;
            job.Budget = minannualSalaryLakhs + minannualSalaryThousands;

            long annualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhs"]) * 100000;
            long annualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousands"]) * 1000;
            job.MaxBudget = annualSalaryLakhs + annualSalaryThousands;

          
            if (!string.IsNullOrEmpty(collection["Any"]))
                job.PreferredAll = Convert.ToBoolean(collection.GetValues("Any").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Contract"]))
                job.PreferredContract = Convert.ToBoolean(collection.GetValues("Contract").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Parttime"]))
                job.PreferredParttime = Convert.ToBoolean(collection.GetValues("Parttime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Fulltime"]))
                job.PreferredFulltime = Convert.ToBoolean(collection.GetValues("Fulltime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["WorkFromHome"]))
                job.PreferredWorkFromHome = Convert.ToBoolean(collection.GetValues("WorkFromHome").Contains("true"));

            if (!string.IsNullOrEmpty(collection["TwoWheeler"]))
                job.TwoWheeler = Convert.ToBoolean(collection.GetValues("TwoWheeler").Contains("true"));

            if (!string.IsNullOrEmpty(collection["FourWheeler"]))
                job.FourWheeler = Convert.ToBoolean(collection.GetValues("FourWheeler").Contains("true"));

                job.PreferredTimeFrom = collection["ddlPreferredTimeFrom"];
                job.PreferredTimeTo = collection["ddlPreferredTimeto"];

            if (!string.IsNullOrEmpty(collection["GeneralShift"]))
                job.GeneralShift = Convert.ToBoolean(collection.GetValues("GeneralShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["NightShift"]))
                job.NightShift = Convert.ToBoolean(collection.GetValues("NightShift").Contains("true"));

                     

            job.ContactPerson = collection["RequirementsContactPerson"];
            job.ContactNumber = collection["RequirementsContactNumber"];
            job.MobileNumber = collection["RequirementsMobileNumber"];
            job.EmailAddress = collection["RequirementsEmailAddress"];
            job.InternationalNumber = collection["RequirementsInternationalNumber"];

            job.Description = collection["Description"];

            if (!string.IsNullOrEmpty(collection["CommunicationEmail"]))
                job.CommunicateViaEmail = Convert.ToBoolean(collection.GetValues("CommunicationEmail").Contains("true"));

            if (!string.IsNullOrEmpty(collection["CommunicationSMS"]))
                job.CommunicateViaSMS = Convert.ToBoolean(collection.GetValues("CommunicationSMS").Contains("true"));

            if (!string.IsNullOrEmpty(collection["HideDetails"]))
                job.HideDetails = Convert.ToBoolean(collection.GetValues("HideDetails").Contains("true"));

            if (!TryValidateModel(job))
                return false;

            //add the job and get the job id, so that we can add the foreign tables data.
            int jobId = job.Id > 0 ? job.Id : _repository.AddJob(job);

            // Add skills for job
            string[] skills = collection["Skills"].Split(',');

            if (skills.Count() != 0)
                _repository.DeleteJobskills(jobId);

            foreach (string skill in skills)
            {
                if (!string.IsNullOrEmpty(skill))
                {
                    JobSkill js = new JobSkill();
                    js.JobId = jobId;
                    js.SkillId = Convert.ToInt32(skill);
                    _repository.AddJobSkill(js);
                }
            }

            // Add languages for job
            string[] languages = collection["Languages"].Split(',');

            if (languages.Count() != 0)
                _repository.DeleteJobLanguages(jobId);

            foreach (string language in languages)
            {
                if (!string.IsNullOrEmpty(language))
                {
                    JobLanguage jl = new JobLanguage();
                    jl.JobId = jobId;
                    jl.LanguageId = Convert.ToInt32(language);
                    _repository.AddJobLanguage(jl);
                }
            }

            //licence types
            string[] licenseTypes = { };
            if (!string.IsNullOrEmpty(collection["lbLicenseTypes"]))
                licenseTypes = collection["lbLicenseTypes"].Split(',');

            //delete all license types
            if (licenseTypes.Count() != 0)
                _repository.DeleteJobLicenseTypes(jobId);

            //then add new ones
            foreach (string licenseType in licenseTypes)
            {
                //if (!string.IsNullOrEmpty(licenseType))
                if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
                {
                    JobLicenseType jlt = new JobLicenseType();
                    jlt.JobId = jobId;
                    jlt.LicenseTypeId = Convert.ToInt32(licenseType);
                    _repository.AddJobLicenseType(jlt);
                }
            }

                      

            string[] preferredIndustries = { };
            if (!string.IsNullOrEmpty(collection["PreferredIndustries"]))
                preferredIndustries = collection["PreferredIndustries"].Split(',');

            //delete all preferred industries
            if (preferredIndustries.Count() != 0)
            {
                _repository.DeleteJobPreferredIndustries(jobId);
            }
            //Add new ones


            foreach (string preferredindustry in preferredIndustries)
            {

                if ((!string.IsNullOrEmpty(preferredindustry)) && preferredindustry!="0")
                {
                    JobPreferredIndustry jpi = new JobPreferredIndustry();
                    jpi.JobId = jobId;
                    jpi.IndustryId = Convert.ToInt32(preferredindustry);
                    _repository.AddJobPreferredIndustry(jpi);
                }
            }


            //Roles
            string[] roles = collection["Roles"].Split(',');

            if (roles.Count() != 0 || roles.Count()!= -1)
                _repository.DeleteJobRoles(jobId);

            foreach (string preferredRole in roles)
            {
                if ((!string.IsNullOrEmpty(preferredRole)) && preferredRole != "-1" && preferredRole!="0")
                {
                    JobRole jr = new JobRole();
                    jr.JobId = jobId;
                    jr.RoleId = Convert.ToInt32(preferredRole);
                    _repository.AddJobRole(jr);
                }
            }

            //add posting locations for job
            _repository.DeleteJobLocations(jobId);


            string[] countries = { };
            if (!string.IsNullOrEmpty(collection["PostingCountry"]))
                countries = collection["PostingCountry"].Split(',');

            string[] states = { };
            if (!string.IsNullOrEmpty(collection["PostingState"]))
                states = collection["PostingState"].Split(',');



            foreach (string countryId in countries)
            {
                if (states.Count() > 0)
                {
                    foreach (string stateId in states)
                    {
                        string[] cities = { };
                        if (!string.IsNullOrEmpty(collection["PostingCity" + stateId.ToString()]))
                            cities = collection["PostingCity" + stateId.ToString()].Split(',');

                        if (cities.Count() > 0)
                        {
                            foreach (string cityId in cities)
                            {
                                string[] regions = { };
                                if (!string.IsNullOrEmpty(collection["PostingRegion" + cityId.ToString()]))
                                    regions = collection["PostingRegion" + cityId.ToString()].Split(',');

                                if (regions.Count() > 0)
                                {
                                    foreach (string regionId in regions)
                                    {
                                        Location location = new Location();

                                        location.CountryId = Convert.ToInt32(countryId);
                                        location.StateId = Convert.ToInt32(stateId);
                                        location.CityId = Convert.ToInt32(cityId);
                                        location.RegionId = Convert.ToInt32(regionId);

                                        int locationId = _repository.AddLocation(location);

                                        JobLocation jl = new JobLocation();
                                        jl.JobId = jobId;
                                        jl.LocationId = locationId;

                                        _repository.AddJobLocation(jl);

                                    }
                                }
                                else
                                {
                                    Location location = new Location();

                                    location.CountryId = Convert.ToInt32(countryId);
                                    location.StateId = Convert.ToInt32(stateId);
                                    location.CityId = Convert.ToInt32(cityId);

                                    int locationId = _repository.AddLocation(location);

                                    JobLocation jl = new JobLocation();
                                    jl.JobId = jobId;
                                    jl.LocationId = locationId;

                                    _repository.AddJobLocation(jl);
                                }

                            }
                        }
                        else
                        {
                            Location location = new Location();

                            location.CountryId = Convert.ToInt32(countryId);
                            location.StateId = Convert.ToInt32(stateId);

                            int locationId = _repository.AddLocation(location);

                            JobLocation jl = new JobLocation();
                            jl.JobId = jobId;
                            jl.LocationId = locationId;

                            _repository.AddJobLocation(jl);
                        }

                    }
                }
                else
                {
                    Location location = new Location();

                    location.CountryId = Convert.ToInt32(countryId);

                    int locationId = _repository.AddLocation(location);

                    JobLocation jl = new JobLocation();
                    jl.JobId = jobId;
                    jl.LocationId = locationId;

                    _repository.AddJobLocation(jl);
                }

            }

            string[] PostingOtherCountries = { };
            if (!string.IsNullOrEmpty(collection["PostingOtherCountry"]))
                PostingOtherCountries = collection["PostingOtherCountry"].Split(',');

            foreach (string countryId in PostingOtherCountries)
            {
                Location location = new Location();

                location.CountryId = Convert.ToInt32(countryId);

                int locationId = _repository.AddLocation(location);

                JobLocation jl = new JobLocation();
                jl.JobId = jobId;
                jl.LocationId = locationId;

                _repository.AddJobLocation(jl);

            }

                     

      

            _repository.DeleteJobRequiredQualifications(job.Id);

            string[] basicQualifications = { };

            if (!string.IsNullOrEmpty(collection["basicQualification"]))
                basicQualifications = collection["basicQualification"].Split(',');

            foreach (string basicQualification in basicQualifications)
            {
                string[] basicspecializations = { };

                if (!string.IsNullOrEmpty(collection["BasicQualificationSpecialization_" + basicQualification]))
                    basicspecializations = collection["BasicQualificationSpecialization_" + basicQualification].Split(',');

                if (basicspecializations != null && basicspecializations.Length > 0)
                {
                    foreach (string specialisation in basicspecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(basicQualification);
                        jrq.SpecializationId = Convert.ToInt32(specialisation);

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(basicQualification);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }

            string[] postGraduations = { };


            if (!string.IsNullOrEmpty(collection["PostGraduation"]))
                postGraduations = collection["PostGraduation"].Split(',');


            foreach (string postgraduate in postGraduations)
            {
                string[] postSpecializations = { };

                if (!string.IsNullOrEmpty(collection["PostGraduationSpecialization_" + postgraduate]))
                    postSpecializations = collection["PostGraduationSpecialization_" + postgraduate].Split(',');

                if (postSpecializations != null && postSpecializations.Length > 0)
                {
                    foreach (string postSpecialization in postSpecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(postgraduate);
                        jrq.SpecializationId = Convert.ToInt32(postSpecialization); 

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(postgraduate);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }

            string[] doctrates = { };

            if (!string.IsNullOrEmpty(collection["Doctrate"]))
                doctrates = collection["Doctrate"].Split(',');

            foreach (string doctrate in doctrates)
            {
                string[] doctorateSpecializations = { };

                if (!string.IsNullOrEmpty(collection["DoctrateSpecialization_" + doctrate]))
                    doctorateSpecializations = collection["DoctrateSpecialization_" + doctrate].Split(',');

                if (doctorateSpecializations != null && doctorateSpecializations.Length > 0)
                {
                    foreach (string doctSpecialization in doctorateSpecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(doctrate);
                        jrq.SpecializationId = Convert.ToInt32(doctSpecialization);

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(doctrate);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }
            
            Session["JobIdPosted"] = jobId;
            return true;
        }

        private void SetCommonViewData()
        {
            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name");
            ViewData["JobBasicQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.BasicQualification);
            ViewData["JobPostQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.PostGraduation);
            ViewData["JobDoctorate"] = _repository.GetDegreeswithAnyOption(DegreeType.Doctorate);

        }

        private void SetAddJobViewData()
        {
            
            //salary
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(GetBudgetLakhs(), "Value", "Name", 0);
            ViewData["MinAnnualSalaryThousands"] = new SelectList(GetBudgetThousands(), "Value", "Name", 0);

            //maxsalary         
            ViewData["AnnualSalaryLakhs"] = new SelectList(GetMaxBudgetLakhs(), "Value", "Name", 0);
            ViewData["AnnualSalaryThousands"] = new SelectList(GetMaxBudgetThousands(), "Value", "Name", 0);

            //experience          
            ViewData["ddlTotalExperienceYearsFrom"] = new SelectList(GetTotalExperienceMinYears(), "Value", "Name", 0);
            ViewData["ddlTotalExperienceYearsTo"] = new SelectList(GetTotalExperienceMaxYears(), "Value", "Name", 0);

            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", 0);
            //ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            var license = _repository.GetLicenseTypesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            license.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["License"] = license;

            //to get the role
            ViewData["Roles"] = new SelectList(_repository.GetRoles(), "Id", "Name", 0);

            
            var indus = _repository.GetIndustriesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            indus.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["Industries"] = indus;


            //ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name",0);
            //ViewData["Industries"] = new MultiSelectList((IEnumerable<Dial4Jobz.Models.Industry>)ViewData["Industries"], "Id", "Name");
        }

        private void SetEditJobViewData(Job job)
        {
                    
            //salary
            int lakhs = (int)(job.MaxBudget / 100000);
            int thousands = (int)((job.MaxBudget - (lakhs * 100000)) / 1000);
            ViewData["AnnualSalaryLakhs"] = new SelectList(GetMaxBudgetLakhs(), "Value", "Name", lakhs);
            ViewData["AnnualSalaryThousands"] = new SelectList(GetMaxBudgetThousands(), "Value", "Name", thousands);

            //minsalary
            int minlakhs = job.Budget.HasValue? (int)(job.Budget / 100000) : 0;
            int minthousands = job.Budget.HasValue ? (int)((job.Budget - (minlakhs * 100000)) / 1000) : 0;
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(GetBudgetLakhs(), "Value", "Name", minlakhs);
            ViewData["MinAnnualSalaryThousands"] = new SelectList(GetBudgetThousands(), "Value", "Name", minthousands);
            
            //experience
            int minyears = job.MinExperience.HasValue ? (int)job.MinExperience.Value / 31104000 : 0;
            int maxyears = job.MaxExperience.HasValue ? (int)job.MaxExperience.Value / 31104000 : 0;
            ViewData["TotalExperienceYearsFrom"] = new SelectList(GetTotalExperienceMinYears(), "Value", "Name", minyears);
            ViewData["TotalExperienceYearsTo"] = new SelectList(GetTotalExperienceMaxYears(), "Value", "Name", maxyears);

            //ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            ViewData["LicenseTypeIds"] = job.JobLicenseTypes.Select(clt => clt.LicenseTypeId);
            List<LicenseType> license = new List<LicenseType>();
            license.Add(new LicenseType { Id = 0, Name = "--- Any ---" });
            var result = _repository.GetLicenseTypes();
            foreach (var li in result)
            {
                license.Add(new LicenseType { Id = li.Id, Name = li.Name });
            }
            ViewData["LicenseTypes"] = license;
           
            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", job.FunctionId);

            JobRole jobrole= _repository.GetRolesByJobId(job.Id);
            if(jobrole != null)
                ViewData["Roles"] = new SelectList(_repository.GetRoles(), "Id", "Name", jobrole.RoleId);
            else
                ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", job.FunctionId.HasValue ? job.FunctionId.Value : 0);
            
            ViewData["JobIndustries"] = _repository.GetIndustries();
            ViewData["JobPreferredIndustryId"] = job.JobPreferredIndustries.Select(jpi => jpi.IndustryId);

            IEnumerable<Location> locations = _repository.GetLocationsbyJobId(job.Id);

            if (locations.Count() > 0)
            {
                ViewData["CountryIds"] = String.Join(",", locations.Select(loc => loc.CountryId));
                ViewData["StateIds"] = String.Join(",", locations.Select(loc => loc.StateId).Where(jr => jr != null));
                ViewData["CityIds"] = String.Join(",", locations.Select(loc => loc.CityId).Where(jr => jr != null));
                ViewData["RegionIds"] = String.Join(",", locations.Select(loc => loc.RegionId).Where(jr => jr != null));
            }

            IEnumerable<JobRequiredQualification> basiQualifications = _repository.GetJobRequiredQualifications(job.Id, DegreeType.BasicQualification);

            if (basiQualifications.Count() > 0)
            {
                ViewData["basiQualificationIds"] = String.Join(",", basiQualifications.Select(bq => bq.DegreeId));
                ViewData["basiQualificationSpecializationIds"] = String.Join(",", basiQualifications.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }

            IEnumerable<JobRequiredQualification> postGraduations = _repository.GetJobRequiredQualifications(job.Id, DegreeType.PostGraduation);

            if (postGraduations.Count() > 0)
            {
                ViewData["postGraduationIds"] = String.Join(",", postGraduations.Select(bq => bq.DegreeId));
                ViewData["postGraduationSpecializationIds"] = String.Join(",", postGraduations.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }

            IEnumerable<JobRequiredQualification> doctorates = _repository.GetJobRequiredQualifications(job.Id, DegreeType.Doctorate);

            if (doctorates.Count() > 0)
            {
                ViewData["doctorateIds"] = String.Join(",", doctorates.Select(bq => bq.DegreeId));
                ViewData["doctorateSpecializationIds"] = String.Join(",", doctorates.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }
            
            // ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", job.JobPreferredIndustries.Select(jpi => jpi.IndustryId));
        }
         
       
        private List<DropDownItem> GetMaxBudgetLakhs()
        {
            List<DropDownItem> maxBudgetLakhs = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                maxBudgetLakhs.Add(item);
            }

            return maxBudgetLakhs;
        }

        private List<DropDownItem> GetMaxBudgetThousands()
        {
            List<DropDownItem> maxBudgetThousands = new List<DropDownItem>();

           for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                maxBudgetThousands.Add(item);
            }

            return maxBudgetThousands;
        }


        private List<DropDownItem> GetBudgetLakhs()
        {
            List<DropDownItem> BudgetLakhs = new List<DropDownItem>();

            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                BudgetLakhs.Add(item);
            }

            return BudgetLakhs;
        }

        private List<DropDownItem> GetBudgetThousands()
        {
            List<DropDownItem> BudgetThousands = new List<DropDownItem>();

            for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                BudgetThousands.Add(item);
            }

            return BudgetThousands;
        }

        private List<DropDownItem> GetTotalExperienceMinYears()
        {
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }

            return totalExperienceYears;
        }

        private List<DropDownItem> GetTotalExperienceMaxYears()
        {
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }

            return totalExperienceYears;
        }

        private ActionResult GetPaginatedJobs(int? page, IQueryable<Job> jobs)
        {
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

            return View("Index", page1Jobs);
        }

        private void AddMoreUrlToViewData(int nextPage)
        {
            ViewData["moreUrl"] = Url.Action("Index", "Jobs", new { page = nextPage });
        } 
        #endregion

        #region Find Jobs

        public ActionResult FindJobs(int? page)
        {
            IQueryable<Job> jobs = _repository.GetJobs();

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


        #endregion

    }
}
