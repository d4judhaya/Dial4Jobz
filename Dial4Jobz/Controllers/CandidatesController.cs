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
using System.Configuration;
using System.IO.Packaging;
using System.Xml;
using System.Text.RegularExpressions;


namespace Dial4Jobz.Controllers
{
    public class CandidatesController : BaseController
    {
     
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
        public int maxLength = int.MaxValue;
        public string[] AllowedFileExtensions;
        public string[] AllowedContentTypes;
        List<string> _filters = new List<string>();
        //string smsSent = "No";
        //string mailSent = "No";
        
        List<long> _idLog = new List<long>();
      //  int remainingcount;
       
            

         //GET: /Candidates/

        public ActionResult Index(FormCollection collection, int? page)
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
            string totalSalary = string.Empty;
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


            if (!string.IsNullOrEmpty(Request.QueryString["skill"]))
            {
                skill = Request.QueryString["skill"];
                filters.Add("skill", skill);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["annualsalary"]))
            {
                totalSalary = Request.QueryString["annualsalary"];
                filters.Add("annualsalary", totalSalary);

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
                Session["Function"] = "Find job seekers";
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


            IQueryable<Candidate> candidates = _repository.GetCandidates(what, where, currentloc, skill, position, function, gender, minSalary, maxSalary, totalSalary,minExperience, maxExperience);
            ViewData["Filters"] = filters;


            //-------DPR PLan for orderby candidates first----------//
           
            //DateTime currentdate = Constants.CurrentTime().Date;
            //DateTime fresh = currentdate;
            //fresh = DateTime.Now.AddDays(-1);

            //List<int> lstCandidateId = null;
            //List<int> lstUpdatedCandidatesId = null;
            //List<int> lstCreatedDatewiseCandidate = null;

            //var ordercandidate = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.CandidateId != null && od.PlanName.ToLower().Contains("DPR".ToLower()) && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.CandidateId.Value);
            //var updatedCandidatesList = _repository.GetCandidates().Where(c => c.UpdatedDate != null && c.UpdatedDate.Value >= fresh).Select(c => c.Id);
            //var CandidatesListByCreatedDate = _repository.GetCandidates().Where(c => c.CreatedDate.Value >= fresh).Select(c => c.Id);

            //lstCandidateId = ordercandidate.ToList();
            //lstUpdatedCandidatesId = updatedCandidatesList.ToList();
            //lstCreatedDatewiseCandidate = CandidatesListByCreatedDate.ToList();

            //Func<IQueryable<Candidate>, IOrderedQueryable<Candidate>> orderingFunc = query =>
            //{
            //    if (ordercandidate.Count() > 0)
            //        return query.OrderByDescending(rslt => lstCandidateId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

            //    else if (updatedCandidatesList.Count() > 0)
            //        return query.OrderByDescending(rslt => lstUpdatedCandidatesId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

            //    else if (CandidatesListByCreatedDate.Count() > 0)
            //        return query.OrderByDescending(rslt => lstCreatedDatewiseCandidate.Contains(rslt.Id)).ThenByDescending(rslt => rslt.UpdatedDate);

            //    else
            //        return query.OrderByDescending(rslt => rslt.CreatedDate);
            //};

            DateTime currentdate = Constants.CurrentTime();
            DateTime fresh = currentdate;
            //fresh = DateTime.Now.AddDays(-1);
            fresh = DateTime.Now.AddMinutes(-30);

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
                    return query.OrderByDescending(rslt => lstUpdatedCandidatesId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

                else if (CandidatesListByCreatedDate.Count() > 0)
                    return query.OrderByDescending(rslt => lstCreatedDatewiseCandidate.Contains(rslt.Id)).ThenByDescending(rslt => rslt.UpdatedDate);

                else
                    return query.OrderByDescending(rslt => rslt.CreatedDate);
            };


            candidates = orderingFunc(candidates);
            //DPR plan Ordering End

            return GetPaginatedCandidates(page, candidates);
        }


        public ActionResult MatchingCandidates(int id, int? page)
        {
            var job = _repository.GetJob(id);

            if (job == null)
                return new FileNotFoundResult();

            IQueryable<Candidate> candidates = _repository.GetMatchingCandidates(job);

            return GetPaginatedCandidates(page, candidates);
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
                state = _repository.GetStatebyCountryId(Id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFunctions()
        {
            var result = new
            {
                functionalArea = _repository.GetFunctionsEnumerable().ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRole(Int32 Id)
        {
            var result = new
            {
                Role = _repository.GetRolesEnumerable(Id).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //public ActionResult EmployerFeedback(FormCollection collection)
        //{

        //    OrderMaster orderMaster = _vasRepository.GetHORSOrderId(LoggedInOrganization.Id);
        //    var orderId = Convert.ToInt32(orderMaster.OrderId);
        //    OrderDetail orderdetail = _vasRepository.GetOrderDetail(orderId);
        //    OrderPayment orderpayment = _vasRepository.GetOrderPayment(orderId);
        //    OrderMaster ordermaster = _vasRepository.GetOrderMaster(orderId);
        //    VasPlan vasplan = _vasRepository.GetVasPlanByPlanId(Convert.ToInt32(orderdetail.PlanId));

        //    string ShortList = collection["ShortList"];
        //    string Selected = collection["Selected"];
        //    string Issues = collection["Issues"];
        //    string ShortListText = collection["ShortListText"];
        //    string SelectedText = collection["SelectedText"];
        //    string IssuesText = collection["IssuesText"];


        //    var shortlistText = "";
        //    var selectedText = "";
        //    var issuesText = "";

        //    var EmployershortlistText = "";
        //    var EmployerselectedText = "";
        //    var EmployerissuesText = "";

        //    //Developer Note: "0" Means Yes, "1" Means No checked in option button
        //    if (ShortList == "0")
        //    {
        //        //NO problem #07DB12
        //        shortlistText = "<span style='color: #07DB12;'>" + "Yes, I was able to shortlist Candidates through Dial4Jobz service easily</span>";
        //        EmployershortlistText = "I was able to shortlist Candidates through Dial4Jobz service easily";
        //    }
        //    else
        //    {
        //        shortlistText = "<span style='color: #FF0000;'>" + "No, I was not able to shortlist Candidates thorough Dial4Jobz service, because of <b>" + ShortListText + "</b></span>";
        //        EmployershortlistText = "I was not able to shortlist Candidates thorough Dial4Jobz service, because of <b>" + ShortListText + "</b>";
        //    }

        //    if (Selected == "0")
        //    {
        //        //NO Problem
        //        selectedText = "<span style='color: #07DB12;'>" + "Yes, I selected Candidates thorough Dial4Jobz service easily</span>";
        //        EmployerselectedText = "I selected Candidates thorough Dial4Jobz service easily";
        //    }
        //    else
        //    {
        //        selectedText = "<span style='color: #FF0000;'>" + "No, I was not able to select Candidates thorough Dial4Jobz service, because of <b>" + SelectedText + "</b></span>";
        //        EmployerselectedText = "I was not able to select Candidates thorough Dial4Jobz service, because of <b>" + SelectedText + "</b>";
        //    }

        //    if (Issues == "1")
        //    {
        //        //NO Problem
        //        issuesText = "<span style='color: #07DB12;'>" + "Easy to use Dial4Jobz services</span>";
        //        EmployerissuesText = "Easy to use Dial4Jobz services";
        //    }
        //    else
        //    {
        //        issuesText = "<span style='color: #FF0000;'>" + "I have an issue/Suggestion for You - <b>" + IssuesText + "</b></span>";
        //        EmployerissuesText = "I have an issue/Suggestion for You - <b>" + IssuesText + "</b>";
        //    }

        //    if (ShortList == "0" && Selected == "0" && Issues == "1")
        //    {
        //        if (LoggedInOrganization.Email != "")
        //        {
        //            EmailHelper.SendEmail(
        //                   Constants.EmailSender.EmployerSupport,
        //                   LoggedInOrganization.Email,
        //                   Constants.EmailSubject.HorsFeedBackByMail,
        //                   Constants.EmailBody.EmployerFeedBackAlert
        //                       .Replace("[NAME]", LoggedInOrganization.Name)
        //                       .Replace("[SHORTLIST]", EmployershortlistText)
        //                       .Replace("[SELECTED]", EmployerselectedText)
        //                       .Replace("[ISSUES]", EmployerissuesText)
        //                       .Replace("[MESSAGE]", "Looking forward for your continued patronage…")
        //                   );
        //        }
        //        //NO Problem
        //    }
        //    else
        //    {
        //        if (LoggedInOrganization.Email != "")
        //        {
        //            EmailHelper.SendEmail(
        //                   Constants.EmailSender.EmployerSupport,
        //                   LoggedInOrganization.Email,
        //                   Constants.EmailSubject.HorsFeedBackByMail,
        //                   Constants.EmailBody.EmployerFeedBackAlert
        //                       .Replace("[NAME]", LoggedInOrganization.Name)
        //                       .Replace("[SHORTLIST]", EmployershortlistText)
        //                       .Replace("[SELECTED]", EmployerselectedText)
        //                       .Replace("[ISSUES]", EmployerissuesText)
        //                       .Replace("[MESSAGE]", "Our Customer Support will contact you at the earliest, for resolving your query.")
        //                   );
        //        }

        //    }


        //    //"smo@dial4jobz.com",
        //    //"ganesan@dial4jobz.com",
        //    //Constants.EmailSender.EmployerSupport,
        //    //"smc@dial4jobz.com",
        //    //Constants.EmailSubject.EmployerHorsFeedBack,

        //    EmailHelper.SendEmailSBCC(Constants.EmailSender.EmployerSupport,
        //        "smo@dial4jobz.com",
        //        "ganesan@dial4jobz.com",
        //        Constants.EmailSender.EmployerSupport,
        //        "smc@dial4jobz.com",
        //        Constants.EmailSubject.EmployerHorsFeedBack,
        //        Constants.EmailBody.EmployerFeedBackToCRM
        //           .Replace("[NAME]", ordermaster.Organization.Name)
        //           .Replace("[ORDERNO]", orderMaster.OrderId.ToString())
        //           .Replace("[ID]", orderMaster.OrganizationId.ToString())
        //           .Replace("[PLAN]", orderdetail.PlanName)
        //           .Replace("[EMAILID]", ordermaster.Organization.Email != "" ? ordermaster.Organization.Email : "Not Mentioned")
        //           .Replace("[DURATION]", vasplan.ValidityDays.ToString())
        //           .Replace("[MOBILE]", LoggedInOrganization.MobileNumber != null ? LoggedInOrganization.MobileNumber : "Not Available")
        //           .Replace("[DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))
        //           .Replace("[EMAIL_COUNT]", vasplan.ValidityCount.ToString())
        //           .Replace("[AMOUNT]", orderdetail.Amount.ToString())
        //           .Replace("[REMAINING_COUNT]", orderdetail.RemainingCount.ToString())
        //           .Replace("[SHORTLIST]", shortlistText)
        //           .Replace("[SELECTED]", selectedText)
        //           .Replace("[ISSUES]", issuesText)
        //           );


        //    return Json(new JsonActionResult { Message = "Done Successfully" });
        //}

        //Edit


        [HttpGet]
        public ActionResult Edit()
        {
            if (LoggedInCandidate == null)
                return new FileNotFoundResult();

            var candidate = _userRepository.GetCandidateById(LoggedInCandidate.Id);
                       
            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            ViewData["Functions"] = _repository.GetFunctions();

            var functions = _repository.GetFunctionsEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            functions.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["PrefFunctions"] = functions;

            ViewData["PreferredFunctionIds"] = candidate.CandidatePreferredFunctions.Select(cpf => cpf.FunctionId);

           // IEnumerable<CandidatePreferredFunction> preferredFunctions = _repository.GetCandidatePreferredFunctions(candidate.Id);

            //if (preferredFunctions.Count() > 0)
            //{
            //    ViewData["PreferredFunctionIds"] = String.Join(",", preferredFunctions.Select(pf => pf.FunctionId));
            //    ViewData["PreferredRoleIds"] = String.Join(",", preferredFunctions.Select(pf => pf.RoleId).Where(jr => jr != null));
            //}

           // ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            var licenses = _repository.GetLicenseTypesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            licenses.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["License"] = licenses;

            List<LicenseType> license = new List<LicenseType>();
            license.Add(new LicenseType { Id = 0, Name = "--- Any ---" });
            var result = _repository.GetLicenseTypes();

            foreach (var li in result)
            {

                license.Add(new LicenseType { Id = li.Id, Name = li.Name });

            }

            ViewData["LicenseTypes"] = license;
            ViewData["LicenseTypeIds"] = candidate.CandidateLicenseTypes.Select(clt => clt.LicenseTypeId);

            ViewData["CandidateFunctions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", candidate.FunctionId);
        

            CandidatePreferredRole cpr = _repository.GetRolesById(candidate.Id);
            int functionid = _repository.GetFunctionIdByCandidateId(candidate.Id);

            if (cpr != null)
                ViewData["Roles"] = new SelectList(_repository.GetRolesByFunctionId(functionid), "Id", "Name", cpr.RoleId);
            else
                ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", candidate.IndustryId);

            ViewData["MaritalStatus"] = new SelectList(_repository.GetMaritalStatus(), "Id", "Name", candidate.MaritalId);

            IEnumerable<Location> locations = _repository.GetLocationsbyCandidateId(candidate.Id);

            if (locations.Count() > 0)
            {
                ViewData["CountryIds"] = String.Join(",", locations.Select(loc => loc.CountryId));
                ViewData["StateIds"] = String.Join(",", locations.Select(loc => loc.StateId).Where(jr => jr != null));
                ViewData["CityIds"] = String.Join(",", locations.Select(loc => loc.CityId).Where(jr => jr != null));
                ViewData["RegionIds"] = String.Join(",", locations.Select(loc => loc.RegionId).Where(jr => jr != null));
            }


            Location location = candidate.LocationId.HasValue ? _repository.GetLocationById(candidate.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);
            
            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);
                     

            ViewData["CandidateBasicQualifications"] = _repository.GetDegreesWithNoneOption(DegreeType.BasicQualification);
            ViewData["CandidatePostQualifications"] = _repository.GetDegreesWithNoneOption(DegreeType.PostGraduation);
            ViewData["CandidateDoctorate"] = _repository.GetDegreesWithNoneOption(DegreeType.Doctorate);
            ViewData["CandidateInstitutes"] = _repository.GetInstitutesWithSelectOption();
            ViewData["PassedOutYear"] = _repository.GetPassedOutYearWithSelectOption();

            ViewData["CandidateSkills"] = new SelectList(_repository.GetSkills(), "Id", "Name", candidate.CandidateSkills.Select(cs => cs.SkillId));

            //number of companies
            List<DropDownItem> numberOfCompanies = new List<DropDownItem>();
            for (int i = 0; i <= 10; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                numberOfCompanies.Add(item);
            }
            ViewData["NumberOfCompanies"] = new SelectList(numberOfCompanies, "Value", "Name", candidate.NumberOfCompanies);


            //salary
            List<DropDownItem> annualSalaryLakhs = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                annualSalaryLakhs.Add(item);
            }

            List<DropDownItem> annualSalaryThousands = new List<DropDownItem>();
            for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                annualSalaryThousands.Add(item);
            }

            if (candidate.AnnualSalary != null)
            {
                int lakhs = (int)(candidate.AnnualSalary / 100000);
                int thousands = (int)((candidate.AnnualSalary - (lakhs * 100000)) / 1000);
                ViewData["AnnualSalaryLakhs"] = new SelectList(annualSalaryLakhs, "Value", "Name", lakhs);
                ViewData["AnnualSalaryThousands"] = new SelectList(annualSalaryThousands, "Value", "Name", thousands);
            }
            else
            {
                ViewData["AnnualSalaryLakhs"] = new SelectList(annualSalaryLakhs, "Value", "Name");
                ViewData["AnnualSalaryThousands"] = new SelectList(annualSalaryThousands, "Value", "Name");
            }

            
            //experience
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }

            List<DropDownItem> totalExperienceMonths = new List<DropDownItem>();
            for (int i = 0; i <= 12; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceMonths.Add(item);
            }

            if (candidate.TotalExperience != null)
            {
                int years = candidate.TotalExperience.HasValue ? (int)candidate.TotalExperience.Value / 31104000 : 0;
                int months = (int)((candidate.TotalExperience.Value - (years * 31536000)) / 2678400);
                ViewData["TotalExperienceYears"] = new SelectList(totalExperienceYears, "Value", "Name", years);
                ViewData["TotalExperienceMonths"] = new SelectList(totalExperienceMonths, "Value", "Name", months);
            }
            else
            {
                ViewData["TotalExperienceYears"] = new SelectList(totalExperienceYears, "Value", "Name");
                ViewData["TotalExperienceMonths"] = new SelectList(totalExperienceMonths, "Value", "Name");
            }

            return View(candidate);
           
        }


        public ActionResult DashBoard()
        {
            if (LoggedInCandidate == null)
            {
                //return FileNotFoundResult;
            }
            if (LoggedInCandidate != null)
            {
                var candidate = _userRepository.GetCandidateById(LoggedInCandidate.Id);
            }
            return View();

        }

        public ActionResult CandidatesSubscriptionBilling()
        {
            LoggedInCandidate = User.Identity.IsAuthenticated ? new UserRepository().GetCandidateByUserName(User.Identity.Name) : null;

            if (LoggedInCandidate != null)
                ViewData["CandidateId"] = LoggedInCandidate.Id;
            else
                ViewData["CandidateId"] = 0;

            return View();
        }

        public ActionResult CandidatesInvoice(string orderId)
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

        #region SendInvoiceBymail

        public class GenerateDocument
        {
            public void GenerateWord(OrderDetail orderDetails, string templateDoc, string filename, Candidate candidate)
            {
                // Copy a new file name from template file
                System.IO.File.Copy(templateDoc, filename, true);

                // Open the new Package
                Package pkg = Package.Open(filename, FileMode.Open, FileAccess.ReadWrite);

                // Specify the URI of the part to be read
                Uri uri = new Uri("/word/document.xml", UriKind.Relative);
                PackagePart part = pkg.GetPart(uri);

                XmlDocument xmlMainXMLDoc = new XmlDocument();
                xmlMainXMLDoc.Load(part.GetStream(FileMode.Open, FileAccess.Read));

                xmlMainXMLDoc.InnerXml = ReplacePlaceHoldersInTemplate(orderDetails, xmlMainXMLDoc.InnerXml, candidate);

                // Open the stream to write document
                StreamWriter partWrt = new StreamWriter(part.GetStream(FileMode.Open, FileAccess.Write));
                //doc.Save(partWrt);
                xmlMainXMLDoc.Save(partWrt);

                partWrt.Flush();
                partWrt.Close();
                pkg.Close();
            }

            private string ReplacePlaceHoldersInTemplate(OrderDetail orderDetail, string templateBody, Candidate candidate)
             {
                 int actualAmount = Convert.ToInt32((orderDetail.Amount.Value - ((orderDetail.Amount.Value / 112.36) * 12.36)));
                 int serviceTax = Convert.ToInt32((orderDetail.Amount.Value / 112.36) * 12.36);
                 double discount = 0;
                // int Amount= actualAmount; 

                 int total = actualAmount + serviceTax;
                 if (orderDetail.DiscountAmount != null)
                 {
                     discount = actualAmount * 25 / 100;
                     actualAmount =Convert.ToInt32(actualAmount - discount);
                     serviceTax = Convert.ToInt32(actualAmount * 12.36 / 100);
                 } 
                 templateBody = templateBody.Replace("[Name]", candidate.Name.ToString());
                 templateBody = templateBody.Replace("[Address]", candidate.Address.ToString());
                 templateBody = templateBody.Replace("[Pincode]", candidate.Pincode);
                 templateBody = templateBody.Replace("[Mobile]", candidate.ContactNumber);
                 templateBody = templateBody.Replace("[InvoiceNo]", orderDetail.OrderId.ToString());
                 templateBody = templateBody.Replace("[Date]", orderDetail.OrderMaster.OrderDate != null ? orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                 //templateBody = templateBody.Replace("[OrderId]", string.Format("{0}", orderDetail.OrderId));
                 templateBody = templateBody.Replace("[ValidityDays]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.Value.ToString() + " days" : "");
                 templateBody = templateBody.Replace("[CustomerId]", string.Format("{0}", candidate.Id));
                 templateBody = templateBody.Replace("[Plan]", string.Format("{0}", orderDetail.VasPlan.PlanName));


                 templateBody = templateBody.Replace("[ActualAmount]", actualAmount.ToString());
                 templateBody = templateBody.Replace("[Discount]", discount.ToString());
                 templateBody = templateBody.Replace("[Amount]", actualAmount.ToString());
                 templateBody = templateBody.Replace("[ServiceTax]", serviceTax.ToString());
                 templateBody = templateBody.Replace("[Total]", orderDetail.DiscountAmount != null ? (actualAmount + serviceTax).ToString() : (actualAmount + serviceTax).ToString());
                 string words = Dial4Jobz.Helpers.StringHelper.NumbersToWords(orderDetail.Amount != null ? Convert.ToInt32(actualAmount + serviceTax) : 0);


                 templateBody = templateBody.Replace("[Words]", words);
                 if (orderDetail.OrderPayment != null && orderDetail.OrderPayment.OrderMaster.PaymentStatus == true)
                 {
                     templateBody = templateBody.Replace("[PaymentMode]", string.Format("{0}", "Mode of Payment: " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()));
                     templateBody = templateBody.Replace("[PaymentDetails]", string.Format("{0}{1}", ("Received payment through " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()), (" On " + orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"))));
                 }
                 else
                 {
                     templateBody = templateBody.Replace("[PaymentMode]", " ");
                     templateBody = templateBody.Replace("[PaymentDetails]", " ");
                 }
                 //…
                 return (templateBody);
             }
        }

        public ActionResult SendInvoiceByMail(int orderId)
        {
            OrderDetail orderDetails = _vasRepository.GetOrderDetail(orderId);
            GenerateDocument doc = new GenerateDocument();

            string docTemplatePath = Server.MapPath("~/Content/Invoice/Test.docx");
            Candidate candidate = _repository.GetCandidate(LoggedInCandidate.Id);
            string docOutputPath = Server.MapPath("~/Content/Invoice/InvoiceBill.docx");
            doc.GenerateWord(orderDetails, docTemplatePath, docOutputPath, candidate);

            EmailHelper.SendEmailWithAttachment(
                Constants.EmailSender.CandidateSupport,
                candidate.Email,
                Constants.EmailSubject.SendInvoiceBymail,
                "Please check Your invoice in the attachment",
                Constants.EmailSender.CandidateSupport,
                docOutputPath
                );

            return RedirectToAction("CandidatesSubscriptionBilling", "Candidates");


        }
        
        #endregion


         // ********Alert Sent Details From RAJ*********//

        public ActionResult RAJAlertSentDetails()
        {
            return View();
        }


        public JsonResult ListAlertSentDetails(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {

            IQueryable<JobsViewedLog> alertsLog = _vasRepository.GetCandidateAlertsLog().Where(od => od.CandidateId == LoggedInCandidate.Id && od.OrderMaster.OrderId == od.OrderId);

            Func<IQueryable<JobsViewedLog>, IOrderedQueryable<JobsViewedLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.JobId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.JobId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.CandidateId.ToString().Contains(sSearch.Trim()) || o.DateViewed.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.DateViewed != null && o.DateViewed >= from && o.DateViewed <= to);

            }

            IEnumerable<JobsViewedLog> alertsLog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = alertsLog.Count(),
                iTotalDisplayRecords = alertsLog.Count(),

                aaData = alertsLog1.Select(o => new object[] { o.OrderId, (_repository.getEmployerNameByJobId(Convert.ToInt32(o.JobId))), (o.DateViewed != null) ? o.DateViewed.Value.ToString("dd-MM-yyyy") : "", (_repository.GetJobById(Convert.ToInt32(o.JobId))), (_repository.getEmployerMobileNumber(Convert.ToInt32(o.JobId))), (_repository.getEmployerEmail(Convert.ToInt32(o.JobId))), (o.SmsSent == true ? "Yes" : "No"), (o.MailSent ==true ? "Yes" : "No") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Save the Profile after the Editing
        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Save(FormCollection collection)
        {
            Candidate candidate = _repository.GetCandidate(LoggedInCandidate.Id);
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");

            /**********Developer Note: Validations By vignesh*************/
            var contactNumber = collection["ContactNumber"];
            var internationalNumber = collection["InternationalNumber"];
            var email = collection["Email"];

            bool EmailValidation = false;
            bool MobileValidation = false;

            if (string.IsNullOrEmpty(internationalNumber) && string.IsNullOrEmpty(contactNumber) && string.IsNullOrEmpty(email))
            {
                return Json(new JsonActionResult { Message = "Mobile Number or International Number or Email is Required." });
            }
            else if (string.IsNullOrEmpty(internationalNumber) && string.IsNullOrEmpty(contactNumber))
            {
                return Json(new JsonActionResult { Message = "Mobile Number or International Number is Required" });
            }

            if (internationalNumber != "")
            {
                if (email != "")
                {
                    EmailValidation = true;
                }
                if (contactNumber != "")
                {
                    MobileValidation = true;
                }
            }
            else if (contactNumber != "")
            {
                 MobileValidation = true;
            }
            else if (contactNumber == "")
            {
                 return Json(new JsonActionResult { Success = false, Message = "Mobile Number is Required." });
            }

            if (email != "")
            {
                EmailValidation = true;
            }
            
           
            if (EmailValidation == true)
            {
                var emailValidate = _userRepository.GetCandidateByEmail(email);
                if (emailValidate != null && !(emailValidate.Id == LoggedInCandidate.Id))
                {
                    return Json(new JsonActionResult { Success = false, Message = "Email Id is already exists" });
                }
                else if (emailValidate != null && emailValidate.Id == LoggedInCandidate.Id)
                {

                }
                else
                {
                    if (candidate.IsMailVerified != false)
                        candidate.IsMailVerified = false;
                }
            }

            if (MobileValidation == true)
            {
                var mobileValidate = _repository.GetCandidateByMobileNumber(contactNumber);
                if (mobileValidate != null && !(mobileValidate.Id == LoggedInCandidate.Id))
                {
                    return Json(new JsonActionResult { Success = false, Message = "Mobile Number is already exists" });
                }
                else if (mobileValidate != null && mobileValidate.Id == LoggedInCandidate.Id)
                {

                }
                else
                {
                    if (candidate.IsPhoneVerified != false)
                        candidate.IsPhoneVerified = false;
                }
            }

            /***************End*****************************/

            candidate.Name = collection["Name"];
            candidate.Email = collection["Email"];
            candidate.ContactNumber = collection["ContactNumber"];
            candidate.MobileNumber = collection["MobileNumber"];
            candidate.Address = collection["Address"];
            candidate.Pincode = collection["Pincode"];
            candidate.Description = collection["Description"];
            candidate.LicenseNumber = collection["LicenseNumber"];
            candidate.PassportNumber = collection["PassportNumber"];
           // candidate.IsPhoneVerified = false;
            candidate.InternationalNumber = collection["InternationalNumber"];

            if (candidate.IsPhoneVerified != true)
                candidate.IsPhoneVerified = false;

            if (candidate.IsMailVerified != true)
                candidate.IsMailVerified = false;

            var function = collection["CandidateFunctions"];
            
            if (candidate.CreatedDate == null)
            {
                candidate.CreatedDate = timeZone;
            }
            else
            {
                candidate.UpdatedDate = timeZone;
            }
        
            if (!string.IsNullOrEmpty(collection["DOB"]))
                candidate.DOB = Convert.ToDateTime(collection["DOB"]);

            if (!string.IsNullOrEmpty(collection["MaritalStatus"]))
                candidate.MaritalId = Convert.ToInt32(collection["MaritalStatus"]);

            if (!string.IsNullOrEmpty(collection["Gender"]))
                candidate.Gender = Convert.ToInt32(collection["Gender"]);

            long yearsinseconds = Convert.ToInt64(collection["ddlTotalExperienceYears"]) * 365 * 24 * 60 * 60;
            long monthsinseconds = Convert.ToInt64(collection["ddlTotalExperienceMonths"]) * 31 * 24 * 60 * 60;
            candidate.TotalExperience = yearsinseconds + monthsinseconds;
            candidate.NumberOfCompanies = Convert.ToInt32(collection["ddlNumberOfCompanies"]);
            candidate.AnnualSalary = (Convert.ToInt32(collection["ddlAnnualSalaryLakhs"]) * 100000 + (Convert.ToInt32(collection["ddlAnnualSalaryThousands"]) * 1000));
            candidate.Position = collection["Position"];
            candidate.PresentCompany = collection["PresentCompany"];
            candidate.PreviousCompany = collection["PreviousCompany"];
            candidate.IPAddress = Request.ServerVariables["REMOTE_ADDR"];

            candidate.PreferredTimeFrom = collection["ddlPreferredTimeFrom"];
            candidate.PreferredTimeTo = collection["ddlPreferredTimeto"];

            if (!string.IsNullOrEmpty(collection["CandidateFunctions"]))
                candidate.FunctionId = Convert.ToInt32(collection["CandidateFunctions"]);

            if (!string.IsNullOrEmpty(collection["Industries"]))
                candidate.IndustryId = Convert.ToInt32(collection["Industries"]);
  
            if (!string.IsNullOrEmpty(collection["Any"]))
                candidate.PreferredAll = Convert.ToBoolean(collection.GetValues("Any").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Contract"]))
                candidate.PreferredContract = Convert.ToBoolean(collection.GetValues("Contract").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Parttime"]))
                candidate.PreferredParttime = Convert.ToBoolean(collection.GetValues("Parttime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Fulltime"]))
                candidate.PreferredFulltime = Convert.ToBoolean(collection.GetValues("Fulltime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["WorkFromHome"]))
                candidate.PreferredWorkFromHome = Convert.ToBoolean(collection.GetValues("WorkFromHome").Contains("true"));

            if (!string.IsNullOrEmpty(collection["GeneralShift"]))
                candidate.GeneralShift = Convert.ToBoolean(collection.GetValues("GeneralShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["NightShift"]))
                candidate.NightShift = Convert.ToBoolean(collection.GetValues("NightShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["TwoWheeler"]))
                candidate.TwoWheeler = Convert.ToBoolean(collection.GetValues("TwoWheeler").Contains("true"));

            if (!string.IsNullOrEmpty(collection["FourWheeler"]))
                candidate.FourWheeler = Convert.ToBoolean(collection.GetValues("FourWheeler").Contains("true"));
                  
            Location location = new Location();
            if (!string.IsNullOrEmpty(collection["CountryCheck"]) && collection["CountryCheck"] == "India")
            {
                location.CountryId = 152;
            }
            else
            {
                if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
            }
            if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
            if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
            if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

            if (location.CountryId != 0)
                candidate.LocationId = _repository.AddLocation(location);

            //if (function == null || function != "")
            //{
            //    return Json(new JsonActionResult { Success = false, Message = "Function is Required" });
            //}
            
           
            if (!TryValidateModel(candidate))
            return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            int candidateId = candidate.Id;


            //Candidates skills

            _repository.DeleteCandidateSkills(candidateId);         
            
            string[] skills = collection["Skills"].Split(',');
            //if (skills.Count() != 0) 
             //_repository.DeleteCandidateSkills(candidateId);         

            foreach (string skill in skills)
            {
                if (!string.IsNullOrEmpty(skill))
                {
                    CandidateSkill cs = new CandidateSkill();
                    cs.CandidateId = candidateId;
                    cs.SkillId = Convert.ToInt32(skill);

                    _repository.AddCandidateSkill(cs);
                }
            }



            // Candidate Languages

            _repository.DeleteCandidateLanguages(candidateId);

            string[] languages = collection["Languages"].Split(',');

            //if (languages.Count() != 0)          
            //    _repository.DeleteCandidateLanguages(candidateId);
           
            foreach (string lang in languages)
            {
                if (!string.IsNullOrEmpty(lang))
                {
                    CandidateLanguage cl = new CandidateLanguage();
                    cl.CandidateId = candidateId;
                    cl.LanguageId = Convert.ToInt32(lang);
                    _repository.AddCandidateLanguage(cl);
                }
            }


            //Roles
            _repository.DeleteCandidateRoles(candidateId);

            string[] roles = collection["Roles"].Split(',');

            //if (roles.Count() != 0)
            //{
            //    _repository.DeleteCandidateRoles(candidateId);
            //}
            foreach (string preferredRole in roles)
            {
                if (!string.IsNullOrEmpty(preferredRole))
                {
                    if (preferredRole != "-1" && preferredRole != "0")
                    {
                        CandidatePreferredRole cpr = new CandidatePreferredRole();
                        cpr.CandidateId = candidateId;
                        cpr.RoleId = Convert.ToInt32(preferredRole);
                        _repository.AddCandidatePreferredRole(cpr);
                    }
                }
            }

            //Preferred Functions
            string[] preferredFunctions = { };
            if (!string.IsNullOrEmpty(collection["PreferredFunctions"]))
                preferredFunctions = collection["PreferredFunctions"].Split(',');

            //delete all preferred functions
            if (preferredFunctions.Count() != 0)
            {
                _repository.DeleteCandidatePreferredFunctions(candidateId);
            }
            //then add new ones
            foreach (string preferredFunction in preferredFunctions)
            {
                if ((!string.IsNullOrEmpty(preferredFunction)) && preferredFunction != "0")
                //if (!string.IsNullOrEmpty(preferredFunction))
                {
                    CandidatePreferredFunction cpf = new CandidatePreferredFunction();
                    cpf.CandidateId = candidateId;
                    cpf.FunctionId = Convert.ToInt32(preferredFunction);
                    _repository.AddCandidatePreferredFunction(cpf);
                }
            }
            
            
            //preferred locations for candidate (delete them first, we'll re-add them)
            _repository.DeleteCandidatePreferredLocation(candidateId);

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
                                        location = new Location();

                                        location.CountryId = Convert.ToInt32(countryId);
                                        location.StateId = Convert.ToInt32(stateId);
                                        location.CityId = Convert.ToInt32(cityId);
                                        location.RegionId = Convert.ToInt32(regionId);

                                        int locationId = _repository.AddLocation(location);

                                        CandidatePreferredLocation cpl = new CandidatePreferredLocation();
                                        cpl.CandidateId = candidateId;
                                        cpl.LocationId = locationId;

                                        _repository.AddCandidatePreferredLocation(cpl);

                                    }
                                }
                                else
                                {
                                    location = new Location();

                                    location.CountryId = Convert.ToInt32(countryId);
                                    location.StateId = Convert.ToInt32(stateId);
                                    location.CityId = Convert.ToInt32(cityId);

                                    int locationId = _repository.AddLocation(location);

                                    CandidatePreferredLocation cpl = new CandidatePreferredLocation();
                                    cpl.CandidateId = candidateId;
                                    cpl.LocationId = locationId;

                                    _repository.AddCandidatePreferredLocation(cpl);
                                }

                            }
                        }
                        else
                        {
                            location = new Location();

                            location.CountryId = Convert.ToInt32(countryId);
                            location.StateId = Convert.ToInt32(stateId);

                            int locationId = _repository.AddLocation(location);

                            CandidatePreferredLocation cpl = new CandidatePreferredLocation();
                            cpl.CandidateId = candidateId;
                            cpl.LocationId = locationId;

                            _repository.AddCandidatePreferredLocation(cpl);
                        }

                    }
                }
                else
                {
                    location = new Location();

                    location.CountryId = Convert.ToInt32(countryId);

                    int locationId = _repository.AddLocation(location);

                    CandidatePreferredLocation cpl = new CandidatePreferredLocation();
                    cpl.CandidateId = candidateId;
                    cpl.LocationId = locationId;

                    _repository.AddCandidatePreferredLocation(cpl);
                }

            }

            string[] PostingOtherCountries = { };
            if (!string.IsNullOrEmpty(collection["PostingOtherCountry"]))
                PostingOtherCountries = collection["PostingOtherCountry"].Split(',');

            foreach (string countryId in PostingOtherCountries)
            {
                location = new Location();

                location.CountryId = Convert.ToInt32(countryId);

                int locationId = _repository.AddLocation(location);

                CandidatePreferredLocation cpl = new CandidatePreferredLocation();
                cpl.CandidateId = candidateId;
                cpl.LocationId = locationId;

                _repository.AddCandidatePreferredLocation(cpl);

            }


            //licence types
            string[] licenseTypes = { };
            if (!string.IsNullOrEmpty(collection["lbLicenseTypes"]))
                licenseTypes = collection["lbLicenseTypes"].Split(',');

            //delete all license types
            if (licenseTypes.Count() != 0)          
                _repository.DeleteCandidateLicenseTypes(candidateId);
         
            //then add new ones
            foreach (string licenseType in licenseTypes)
            {
                //if (!string.IsNullOrEmpty(licenseType))
                if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
                {
                    CandidateLicenseType clt = new CandidateLicenseType();
                    clt.CandidateId = candidateId;
                    clt.LicenseTypeId = Convert.ToInt32(licenseType);
                    _repository.AddCandidateLicenseType(clt);
                }
            }

            //Add Candidate Qualification (first clear the existing ones)
            _repository.DeleteCandidateQualifications(candidate.Id);

            for (int i = 1; i <= MAX_ADD_NEW_INPUT; i++)
            {
                #region Basic Qualification Insert

                var basicQualificationDegreeId = "BasicQualificationDegree" + i;
                //string basicQualificationSpecialization = "BasicQualificationSpecialization" + i.ToString();
                var basicQualificationSpecializationId = "BasicQualificationSpecialization" + i;
                var basicQualificationInstituteId = "BasicQualificationInstitute" + i;
                var basicQualificationPassedOutYear = "BasicQualificationPassedOutYear" + i;

                if (!string.IsNullOrEmpty(collection[basicQualificationDegreeId]))
                {
                    int specializationId ;
                    int instituteId;
                    int passedOutYear;
                    int.TryParse(collection[basicQualificationSpecializationId], out specializationId);
                    int.TryParse(collection[basicQualificationInstituteId], out instituteId);
                    int.TryParse(collection[basicQualificationPassedOutYear], out passedOutYear);

                    
                    var cq = new CandidateQualification
                    {
                        CandidateId = candidateId,
                        DegreeId = Convert.ToInt32(collection[basicQualificationDegreeId]),
                        //Specialization = collection[basicQualificationSpecialization],
                        //Specialization = string.Empty,
                        SpecializationId = specializationId == 0 ? null : (int?)specializationId,
                        InstituteId = instituteId == 0 ? null : (int?)instituteId,
                        PassedOutYear = passedOutYear == 0 ? null : (int?)passedOutYear
                    };

                    if(i == 1 && cq.DegreeId <= 0)
                    {
                        ModelState.AddModelError("BasicQualificationDegreeRequired","Choose Basic Qualification");
                        return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
                    }

                    if (i == 1 && cq.SpecializationId == null)
                    {
                        ModelState.AddModelError("BasicQualificationSpecializationRequired", "Choose Specialization");
                        return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
                    }

                    if (cq.DegreeId > 0)
                        _repository.AddCandidateQualification(cq);
                }

                #endregion

                #region Post Graduation Insert

                var postGraduationDegreeId = "PostGraduationDegree" + i;
                //var postGraduationSpecialization = "PostGraduationSpecialization" + i.ToString();
                var postGraduationSpecializationId = "PostGraduationSpecialization" + i;
                var postGraduationInstituteId = "PostGraduationInstitute" + i;
                var postGraduationPassedOutYear = "PostGraduationPassedOutYear" + i;

                if (!string.IsNullOrEmpty(collection[postGraduationDegreeId]))
                {
                    int specializationId;
                    int instituteId;
                    int passedOutYear;
                    int.TryParse(collection[postGraduationSpecializationId], out specializationId);
                    int.TryParse(collection[postGraduationInstituteId], out instituteId);
                    int.TryParse(collection[postGraduationPassedOutYear], out passedOutYear);

                    var cq = new CandidateQualification
                                 {
                                     CandidateId = candidateId,
                                     DegreeId = Convert.ToInt32(collection[postGraduationDegreeId]),
                                     //Specialization = collection[postGraduationSpecialization],
                                     //Specialization = string.Empty,
                                     SpecializationId = specializationId == 0 ? null : (int?)specializationId,
                                     InstituteId = instituteId == 0 ? null : (int?)instituteId,
                                     PassedOutYear = passedOutYear == 0 ? null : (int?)passedOutYear
                                 };
                    
                    if (cq.DegreeId > 0)
                        _repository.AddCandidateQualification(cq);
                }

                #endregion

                #region Doctorate Insert

                var doctorateDegreeId = "DoctorateDegree" + i;
                //var doctorateSpecialization = "DoctorateSpecialization" + i;
                var doctorateSpecializationId = "DoctorateSpecialization" + i;
                var doctorateInstituteId = "DoctorateInstitute" + i;
                var doctoratePassedOutYear = "DoctoratePassedOutYear" + i;

                               


                if (!string.IsNullOrEmpty(collection[doctorateDegreeId]))
                {
                    int specializationId;
                    int instituteId;
                    int passedOutYear;
                    int.TryParse(collection[doctorateSpecializationId], out specializationId);
                    int.TryParse(collection[doctorateInstituteId], out instituteId);
                    int.TryParse(collection[doctoratePassedOutYear], out passedOutYear);

                    var cq = new CandidateQualification
                    {
                        CandidateId = candidateId,
                        DegreeId = Convert.ToInt32(collection[doctorateDegreeId]),
                        //Specialization = collection[doctorateSpecialization],
                        //Specialization = string.Empty,
                        SpecializationId = specializationId == 0 ? null : (int?)specializationId,
                        InstituteId = instituteId == 0 ? null : (int?)instituteId,
                        PassedOutYear = passedOutYear == 0 ? null : (int?)passedOutYear
                    };


                    if (cq.DegreeId > 0)
                        _repository.AddCandidateQualification(cq);
                }

                #endregion
            }


            
            _repository.Save();

            if (candidate.ContactNumber != null)
            {

                /*Candidate Details send to candidate to verify*/
                string candidateBasic = string.Empty;
                string candidatePost = string.Empty;
                string candidateDoctorate = string.Empty;
                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 0))
                {
                    if (cq != null && cq.Specialization != null)
                    {
                        candidateBasic += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                    }
                    else
                    {

                    }
                }

                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 1))
                {
                    if (cq != null)
                    {

                        if (cq.Specialization != null && cq.Specialization != null)
                        {
                            candidatePost += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {
                            candidatePost += cq.Degree.Name + ",";
                        }
                    }
                    else
                    {
                    }
                }

                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 2))
                {
                    if (cq != null)
                    {

                        if (cq.Specialization != null)
                        {
                            candidateDoctorate += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {
                            candidateDoctorate += cq.Degree.Name + ",";
                        }
                    }

                }



                string candidateannualsalary = (candidate.AnnualSalary.HasValue && candidate.AnnualSalary != 0) ? Convert.ToInt32(candidate.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN")) : "";


                string candidateexperience = string.Empty;
                string candidatemonth = string.Empty;

                candidateexperience = (candidate.TotalExperience.Value / 33782400.0) + "Years";
                candidatemonth = (candidate.TotalExperience.Value * 31536000) + "Months";

                string candidatelocation = string.Empty;
                string cityName = string.Empty;
                if (candidate.LocationId != null)
                {
                    if (candidate.GetLocation(candidate.LocationId.Value).CityId.HasValue && candidate.GetLocation(candidate.LocationId.Value).CityId != 0)
                    {
                        cityName = candidate.GetLocation(candidate.LocationId.Value).City.Name + ",";
                    }
                    if (candidate.GetLocation(candidate.LocationId.Value).CountryId != null && candidate.GetLocation(candidate.LocationId.Value).CountryId != 0)
                    {
                        candidatelocation = candidate.GetLocation(candidate.LocationId.Value).Country.Name;
                    }
                }

                string industry = string.Empty;
                //function = (candidate.FunctionId.HasValue && candidate.FunctionId != 0) ? candidate.FunctionId.Value.ToString() : string.Empty;
                if (industry == string.Empty)
                {
                    industry = (candidate.IndustryId == null ? "Any" : candidate.GetIndustry(candidate.IndustryId.Value).Name);
                }

                string preffunction = string.Empty;

                if (candidate.CandidatePreferredFunctions != null)
                {
                    foreach (CandidatePreferredFunction cpf in candidate.CandidatePreferredFunctions)
                    {
                        if (preffunction == string.Empty)
                        {
                            preffunction = cpf.Function.Name;
                        }
                        else
                        {

                        }
                    }
                }

                string role = string.Empty;

                if (candidate.CandidatePreferredRoles != null)
                {
                    foreach (CandidatePreferredRole cr in candidate.CandidatePreferredRoles)
                    {
                        if (role == string.Empty)
                        {
                            role = cr.Role.Name;
                        }
                        else
                        {

                        }
                    }
                }

                string prefcityName = string.Empty;
                string prefcountryName = string.Empty;
                string prefregionName = string.Empty;


                if (candidate.CandidatePreferredLocations != null)
                {
                    foreach (CandidatePreferredLocation cpl in candidate.CandidatePreferredLocations)
                    {
                        if (prefcityName == string.Empty)
                        {
                            if (cpl.Location.City != null)
                            {
                                prefcityName = cpl.Location.City.Name;
                            }
                            else
                            {

                            }
                        }

                        if (prefcountryName == string.Empty)
                        {
                            if (cpl.Location.Country != null)
                            {
                                prefcountryName = cpl.Location.Country.Name;
                            }
                            else
                            {
                            }
                        }

                        if (prefregionName == string.Empty)
                        {
                            if (cpl.Location.Region != null)
                            {
                                prefregionName = cpl.Location.Region.Name;
                            }
                            else
                            {
                            }
                        }

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
                    age--;
                }


                SmsHelper.SendSecondarySms(
                   Constants.SmsSender.SecondaryUserName,
                   Constants.SmsSender.SecondaryPassword,
                   Constants.SmsBody.CandidateDetails
                           .Replace("[NAME]", candidate.Name)
                           .Replace("[EMAIL]", (candidate.Email != null ? candidate.Email : ""))
                           .Replace("[MOBILE_NUMBER]", (candidate.ContactNumber != "" ? candidate.ContactNumber : ""))
                           .Replace("[QUALIFICATION]", candidateBasic + "," + (candidatePost != "" ? candidatePost : "") + "," + (candidateDoctorate != "" ? candidateDoctorate : ""))
                           .Replace("[FUNCTION]", candidate.FunctionId == null || candidate.FunctionId == 0 ? "" : candidate.Function.Name)
                           .Replace("[DESIGNATION]", candidate.Position)
                           .Replace("[ANNUAL_SALARY]", (candidateannualsalary != "" ? candidateannualsalary + "Per annum" : "NA"))
                           .Replace("[COUNTRY]", (candidatelocation!="" ? candidatelocation :"NA"))
                           .Replace("[CITY]", (cityName!=""?cityName.ToString(): "NA"))
                           .Replace("[PREF_COUNTRY]", (prefcountryName!="" ? prefcountryName : "NA"))
                           .Replace("[PREF_CITY]", (prefcityName!=""? prefcityName : "NA"))
                           .Replace("[PREF_FUNC]", (preffunction!="" ? preffunction.ToString() : "NA"))
                           .Replace("[INDUSTRY]", (industry!="" ? industry:"NA"))
                    //.Replace("[PREF_LOC]", prefcountryName + "," + prefcityName)
                           .Replace("[DOB]", age.ToString())
                           .Replace("[ROLE]", (role!="" ? role: "NA"))
                           .Replace("[YEARS]", (candidateexperience!="" ? candidateexperience :"NM"))
                           .Replace("[MONTHS]", (candidatemonth!="" ? candidatemonth: "NA"))
                    //.Replace("[TOTAL_EXPERIENCE]", (candidateexperience!=""? candidateexperience +" Yrs" : "NA"))
                           .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female")
                      ,

                       Constants.SmsSender.SecondaryType,
                       Constants.SmsSender.Secondarysource,
                       Constants.SmsSender.Secondarydlr,
                       candidate.ContactNumber
                       );

                /*End Candidate Details send*/

            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Profile has been updated from your IP Address of" + candidate.IPAddress,
                ReturnUrl = "/JobMatchesCandidate/JobMatch/" + candidate.Id.ToString()
            });
        }


        public ActionResult GetSpecialization(int degreeId)
        {
            return Json(new SelectList(_repository.GetSpecializationByDegreeId(degreeId), "Id", "Name"));
        }


        
         public ActionResult ContactCandidates()
         {
            
             List<DropDownItem> annualSalaryLakhs = new List<DropDownItem>();
             for (int i = 0; i <= 50; i++)
             {
                 DropDownItem item = new DropDownItem();
                 item.Name = i.ToString();
                 item.Value = i;
                 annualSalaryLakhs.Add(item);
             }

             var minannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
             minannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });
             ViewData["MinAnnualSalaryLakhs"] = new SelectList(minannualSal, "Value", "Text");

             var maxannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
             maxannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });
             ViewData["MaxAnnualSalaryLakhs"] = new SelectList(maxannualSal, "Value", "Text");

             LoggedInOrganization = User.Identity.IsAuthenticated && IsEmployer ? _userRepository.GetOrganizationByUserName(User.Identity.Name) : null;

             return View(LoggedInOrganization);
         }

        //Get Roles by functionId
         public JsonResult getrolebyFunctionIdwithFunctionname(Int32 Id)
         {
             var result = new
             {
                 role = _repository.GetRoleIdByFunctionId(Id)
             };
             return Json(result, JsonRequestBehavior.AllowGet);
         }

         public JsonResult getstatebyCountryIdwithCountryname(Int32 Id)
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

        //by vignesh
         [HttpPost]
         public ActionResult UploadResume(IEnumerable<HttpPostedFileBase> FileData, FormCollection collection)
         {
             string candidateId = Request.Form["candId"];
             Candidate candidate = _repository.GetCandidate(Convert.ToInt32(candidateId));
             foreach (var file in FileData)
             {
                 if (file.ContentLength > 0)
                 {
                     var fileName = Path.GetFileName(file.FileName);
                     var path = Path.Combine(Server.MapPath("~/Content/Resumes/"), fileName);
                     file.SaveAs(path);
                     //var timeoutsession = Session.Timeout;
                     //var candidate1 = Session["candiId"];
                     //if (candidate1 != null)
                     //{

                     //}
                     if (candidate != null)
                     {
                         candidate.ResumeFileName = fileName;
                         _repository.Save();
                     }

                 }
             }
             return Json(new JsonActionResult
             {
                 Success = true,
                 Message = "Resume has been uploaded."
             });
         }
                   

         //[HttpPost]
         //public ActionResult UploadResume (IEnumerable<HttpPostedFileBase> FileData)
         //{
         //    foreach (var file in FileData)
         //    {
         //        if (file.ContentLength > 0)
         //        {
         //            var fileName = Path.GetFileName(file.FileName);
         //            var path = Path.Combine(Server.MapPath("~/Content/Resumes/"), fileName);
         //            file.SaveAs(path);
         //            var candidateId = ViewData["Id"];
         //            if (candidateId != null)
         //            {
         //                Candidate candidatedetails = _repository.GetCandidate(Convert.ToInt32(candidateId));
         //                candidatedetails.ResumeFileName = fileName;
         //                _repository.Save();
         //            }
         //            else if (LoggedInCandidate != null)
         //            {

         //                var candidate = _repository.GetCandidate(LoggedInCandidate.Id);
         //                candidate.ResumeFileName = fileName;
         //                _repository.Save();
         //            }
         //        }
         //    }

         //    //return RedirectToAction("Index");
         //    return Json(new JsonActionResult
         //    {
         //        Success = true,
         //        Message = "Resume has been uploaded."
         //    });
         //}

       
         public ActionResult DownloadResumeFromJobMatches(string fileName)
          {
             string pfn = Server.MapPath("~/Content/Resumes/" + fileName);
             if (!System.IO.File.Exists(pfn))
             {
                 throw new ArgumentException("Invalid file name or file not exists!");
                 //return Json(new JsonActionResult { Success = false, Message = "Invalid file name or file not exists!" });
             }
             else
             {

                 return new BinaryContentResult()
                 {
                     FileName = fileName,
                     ContentType = "application/octet-stream",
                     Content = System.IO.File.ReadAllBytes(pfn)
                 };
             }

           
         }



         //[Authorize, HttpGet]
         //public ActionResult Download(string fileName)
         //{
         //    string pfn = Server.MapPath("~/Content/Resumes/" + fileName);

         //    if (!System.IO.File.Exists(pfn))
         //    {
         //        //throw new ArgumentException("Invalid file name or file not exists!");

         //        return Json(new JsonActionResult { Success = false, Message = "Invalid file name or file not exists!" });
         //    }
         //    else
         //    {
                    
         //        return new BinaryContentResult()
         //        {
         //            FileName = fileName,
         //            ContentType = "application/octet-stream",
         //            Content = System.IO.File.ReadAllBytes(pfn)
         //        };
         //    }

         //}

         [Authorize, HttpGet]
         public ActionResult Download(string fileName)
         {
             string pfn = Server.MapPath("~/Content/Resumes/" + fileName);

             if (!System.IO.File.Exists(pfn))
             {
                 //throw new ArgumentException("Invalid file name or file not exists!");

                 return Json(new JsonActionResult { Success = false, Message = "Invalid file name or file not exists!" });
             }
             else
             {

                 return new BinaryContentResult()
                 {
                     FileName = "" + "\"" + fileName + "\"",
                     ContentType = "application/octet-stream",
                     Content = System.IO.File.ReadAllBytes(pfn)
                 };
             }

         }

       

         [HttpPost, HandleErrorWithAjaxFilter]
         public ActionResult Send(FormCollection collection, SendMethod sendMethod, ContactCandidates _contactCandidates, string SendToUser, string SendToOrganization, string candidateId)
         {
             
             string message = "";
             int? emailCount=0;
             int smslength = 0;
             double smssent = 0;
             int? smscount =0;
             OrderDetail orderdetail = null;
             Location orglocation = null;

             if (_contactCandidates.Message != null)
             {
                 Regex space = new Regex(" ", RegexOptions.Compiled | RegexOptions.Singleline);
                 message = space.Replace(_contactCandidates.Message, "&nbsp;");
                 Regex matchNewLine = new Regex("\r\n|\r|\n", RegexOptions.Compiled | RegexOptions.Singleline);
                 message = matchNewLine.Replace(message, "<br />");
             }
             if (LoggedInOrganization != null)
             {
                 emailCount = _vasRepository.GetEmployerEmailCount(LoggedInOrganization.Id);
                 smscount = _vasRepository.GetSmsVasCount(LoggedInOrganization.Id);
                 orderdetail = _vasRepository.GetEmployerSmsVas(LoggedInOrganization.Id);
                 orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInOrganization.LocationId));
             }

             else
             {
                 emailCount = _vasRepository.GetConsultantEmailCount(LoggedInConsultant.Id);
                 smscount = _vasRepository.GetSmsVasCountConsultant(LoggedInConsultant.Id);
                 orderdetail = _vasRepository.GetConsultantSmsVas(LoggedInConsultant.Id);
                 orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInConsultant.LocationId));  
             }
             
             orglocation = null;
             LoggedInOrganization = User.Identity.IsAuthenticated && IsEmployer ? _userRepository.GetOrganizationByUserName(User.Identity.Name) : null;


             /********Developer Note: This method will run when send sms and email from details page.**********/
             if(LoggedInOrganization!=null || LoggedInConsultant!=null)
             {
                 if ((LoggedInOrganization != null || LoggedInConsultant!=null) && candidateId != null)
                 {
                     // if (candidateId != null)
                     // {
                     Candidate getCandidate = _repository.GetCandidate(Convert.ToInt32(candidateId));

                     /*Both Method(SMS and EMAIL)*/
                     if (sendMethod == SendMethod.SMS || sendMethod == SendMethod.Both)
                     {
                         if (smscount != null && smscount > 0)
                         {
                           
                             //if (LoggedInOrganization != null)
                             //{
                             //    orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInOrganization.LocationId));
                             //}
                             //else
                             //{
                             //    orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInConsultant.LocationId));
                             //}
                             SmsHelper.SendSecondarySms(
                             Constants.SmsSender.SecondaryUserName,
                             Constants.SmsSender.SecondaryPassword,
                             Constants.SmsBody.SMSVacancyDirect
                             .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                             .Replace("[CONTACT_PERSON]", (LoggedInOrganization!=null ? LoggedInOrganization.ContactPerson: LoggedInConsultant.ContactPerson))
                             .Replace("[EMAIL]", (LoggedInOrganization!=null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                             .Replace("[MOBILE_NUMBER]", (LoggedInOrganization!=null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                             .Replace("[POSITION]", getCandidate.Position != null ? getCandidate.Position : "")
                             .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name)
                             ,
                             Constants.SmsSender.SecondaryType,
                             Constants.SmsSender.Secondarysource,
                             Constants.SmsSender.Secondarydlr,
                             getCandidate.ContactNumber
                              );

                             string smsbody = Constants.SmsBody.SMSVacancyDirect
                              .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                             .Replace("[CONTACT_PERSON]", (LoggedInOrganization!=null ? LoggedInOrganization.ContactPerson: LoggedInConsultant.ContactPerson))
                             .Replace("[EMAIL]", (LoggedInOrganization!=null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                             .Replace("[MOBILE_NUMBER]", (LoggedInOrganization!=null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                             .Replace("[POSITION]", getCandidate.Position != null ? getCandidate.Position : "")
                             .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name);

                             smslength = smsbody.Length;
                             smssent = Math.Ceiling((double)smslength / 160);
                                                          
                             if (orderdetail != null)
                             {
                                 orderdetail.RemainingCount = orderdetail.RemainingCount - Convert.ToInt32(smssent);
                                 _vasRepository.Save();
                             }
                         }
                        
                     }


                     else if (sendMethod == SendMethod.Email || sendMethod == SendMethod.Both)
                     {
                         if (getCandidate.Email != null)
                         {
                             if (getCandidate.Email != "")
                             {

                                 if (emailCount != null || emailCount != 0)
                                 {
                                     EmailHelper.SendEmailReply(
                                         Constants.EmailSender.EmployerSupport,
                                         getCandidate.Email,
                                         "Response for your Resume",
                                         Constants.EmailBody.ResponseForResume
                                         .Replace("[CANDIDATENAME]", getCandidate.Name)
                                         .Replace("[MOBILE_NO]", (LoggedInOrganization!=null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                         .Replace("[NAME]", (LoggedInOrganization!=null ?LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson))
                                         .Replace("[EMAIL]", (LoggedInOrganization!=null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                         .Replace("[EXPERIENCE]", _contactCandidates.MinExperience + " - " + _contactCandidates.MaxExperience)
                                         .Replace("[SALARY]", _contactCandidates.MinAnnualSalaryLakhs + " - " + _contactCandidates.MaxAnnualSalaryLakhs)
                                          .Replace("[OTHERSALARYDETAILS]", _contactCandidates.OtherSalary)
                                         .Replace("[JOBLOCATION]", _contactCandidates.JobLocation)
                                          .Replace("[MESSAGE]", message)
                                         .Replace("[EMPLOYERNAME]", LoggedInOrganization.Name)
                                         .Replace("[EMPLOYERMAILID]", _contactCandidates.EmployerEmail),
                                            _contactCandidates.EmployerEmail
                                         );



                                     var balanceCount = emailCount - 1;
                                     if(LoggedInOrganization!=null)
                                     {
                                         orderdetail = _vasRepository.UpdateEmailCount(LoggedInOrganization.Id);
                                     }
                                     else
                                     {
                                          orderdetail = _vasRepository.UpdateConsultantEmailCount(LoggedInConsultant.Id);
                                     }

                                    
                                     if (orderdetail != null)
                                     {
                                         orderdetail.EmailRemainingCount = balanceCount;
                                         _vasRepository.Save();
                                     }
                                 }


                                 /*For SMS (Both)*/
                                 if (smscount != null && smscount > 0)
                                 {
                                     SmsHelper.SendSecondarySms(
                                     Constants.SmsSender.SecondaryUserName,
                                     Constants.SmsSender.SecondaryPassword,
                                     Constants.SmsBody.SMSVacancyDirect
                                     .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                                     .Replace("[CONTACT_PERSON]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson)
                                     .Replace("[EMAIL]", (LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                     .Replace("[MOBILE_NUMBER]", (LoggedInOrganization != null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                     .Replace("[POSITION]", getCandidate.Position != null ? getCandidate.Position : "")
                                     .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name)
                                     ,
                                     Constants.SmsSender.SecondaryType,
                                     Constants.SmsSender.Secondarysource,
                                     Constants.SmsSender.Secondarydlr,
                                     getCandidate.ContactNumber
                                      );

                                     string smsbody = Constants.SmsBody.SMSVacancyDirect
                                     .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                                     .Replace("[CONTACT_PERSON]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson)
                                     .Replace("[EMAIL]", (LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                     .Replace("[MOBILE_NUMBER]", (LoggedInOrganization != null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                     .Replace("[POSITION]", getCandidate.Position != null ? getCandidate.Position : "")
                                     .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name);

                                     smslength = smsbody.Length;
                                     smssent = Math.Ceiling((double)smslength / 160);

                                    //if(LoggedInOrganization!=null)
                                    //{
                                    //    orderdetail = _vasRepository.GetEmployerSmsVas(LoggedInOrganization.Id);
                                    //}
                                    //else 
                                    //{
                                    //     orderdetail = _vasRepository.GetConsultantSmsVas(LoggedInConsultant.Id);
                                    //}

                                     if (orderdetail != null)
                                     {
                                         orderdetail.RemainingCount = orderdetail.RemainingCount - Convert.ToInt32(smssent);
                                         _vasRepository.Save();
                                     }
                                 }

                                 /*End SMS*/
                             }

                         }
                     }
                 }

                 //}

                 /**********Note: Multiple Candidate Select SMS and Email Send******/
                 foreach (string key in collection.AllKeys)
                 {
                     //process each candidate that is selected.
                     if (key.Contains("Candidate"))
                     {
                         if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                         {
                             int candiId = Convert.ToInt32(key.Replace("Candidate", string.Empty));
                             Candidate candidate = _repository.GetCandidate(candiId);

                             if (candidate == null)
                                 return new FileNotFoundResult();

                             if (sendMethod == SendMethod.SMS || sendMethod == SendMethod.Both)
                             {
                                //smscount = _vasRepository.GetSmsVasCount(LoggedInOrganization.Id);
                                smslength = 0;
                                smssent = 0;
                                 
                                 if (smscount != null && smscount > 0)
                                 {
                                     if (_contactCandidates.Title == "1")
                                     {

                                         if (SendToOrganization == "true")
                                         {
                                            SmsHelper.SendSecondarySms(
                                            Constants.SmsSender.SecondaryUserName,
                                            Constants.SmsSender.SecondaryPassword,
                                            Constants.SmsBody.SMSResume
                                            .Replace("[NAME]", candidate.Name)
                                            .Replace("[EMAIL]", candidate.Email)
                                            .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                           // .Replace("[QUALIFICATION]", basicqualification)
                                            .Replace("[FUNCTION]", candidate.FunctionId != null ? candidate.Function.Name:"")
                                            .Replace("[DESIGNATION]", candidate.Position)
                                            .Replace("[PRESENT_SALARY]", candidate.AnnualSalary.ToString())
                                            //.Replace("[LOCATION]", location)
                                            .Replace("[DOB]", candidate.DOB.HasValue ? candidate.DOB.Value.ToShortDateString() : String.Empty)
                                            .Replace("[TOTAL_EXPERIENCE]", candidate.TotalExperience.ToString())
                                            .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female"),
                                            Constants.SmsSender.SecondaryType,
                                            Constants.SmsSender.Secondarysource,
                                            Constants.SmsSender.Secondarydlr,
                                            (LoggedInOrganization!=null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber)
                                             );
                                         }

                                         if(SendToUser=="true")
                                         {
                                             if (candidate.ContactNumber != null)
                                             {
                                                 SmsHelper.SendSecondarySms(
                                                 Constants.SmsSender.SecondaryUserName,
                                                 Constants.SmsSender.SecondaryPassword,
                                                 Constants.SmsBody.SMSInterestCheck
                                                 .Replace("[CANDIDATENAME]", candidate.Name)
                                                 .Replace("[SMSVACANCY]", _contactCandidates.SmsVacancy)
                                                 .Replace("[SMSLOCATION]", _contactCandidates.SmsLocation)
                                                 .Replace("[SMSCOMPANYNAME]", (LoggedInOrganization!=null? LoggedInOrganization.Name : LoggedInConsultant.Name))
                                                 .Replace("[SMSMOBILENO]", (LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                                 .Replace("[SMSEMAILID]", (LoggedInOrganization!=null? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                                 .Replace("[SMSCONTACTPERSON]", _contactCandidates.SmsContactPerson),
                                                Constants.SmsSender.SecondaryType,
                                                Constants.SmsSender.Secondarysource,
                                                Constants.SmsSender.Secondarydlr,
                                                candidate.ContactNumber
                                                );
                                             }

                                             string smsbody = Constants.SmsBody.SMSInterestCheck
                                             .Replace("[CANDIDATENAME]", candidate.Name)
                                             .Replace("[SMSVACANCY]", _contactCandidates.SmsVacancy)
                                             .Replace("[SMSLOCATION]", _contactCandidates.SmsLocation)
                                             .Replace("[SMSCOMPANYNAME]", _contactCandidates.SmsCompanyName)
                                             .Replace("[SMSMOBILENO]", _contactCandidates.SmsMobileNo)
                                             .Replace("[SMSEMAILID]", _contactCandidates.SmsEmailId)
                                             .Replace("[SMSCONTACTPERSON]", _contactCandidates.SmsContactPerson);

                                             smslength = smsbody.Length;
                                             smssent = Math.Ceiling((double)smslength / 160);

                                             if(LoggedInOrganization!=null)
                                             {
                                                orderdetail = _vasRepository.GetEmployerSmsVas(LoggedInOrganization.Id);
                                             }
                                             else
                                             {
                                                 orderdetail = _vasRepository.GetConsultantSmsVas(LoggedInConsultant.Id);
                                             }

                                             if (orderdetail != null)
                                             {
                                                 orderdetail.RemainingCount = orderdetail.RemainingCount - Convert.ToInt32(smssent);
                                                 _vasRepository.Save();
                                             }
                                         }

                                     }
                                                                       

                                     if (_contactCandidates.Title == "2")
                                     {

                                         if (SendToOrganization == "true")
                                         {
                                             SmsHelper.SendSecondarySms(
                                            Constants.SmsSender.SecondaryUserName,
                                            Constants.SmsSender.SecondaryPassword,
                                            Constants.SmsBody.SMSInterestCheck
                                            .Replace("[CANDIDATENAME]", candidate.Name)
                                            .Replace("[SMSVACANCY]", _contactCandidates.SmsVacancy)
                                            .Replace("[SMSLOCATION]", _contactCandidates.SmsLocation)
                                            .Replace("[SMSCOMPANYNAME]", _contactCandidates.SmsCompanyName)
                                            .Replace("[SMSMOBILENO]", _contactCandidates.SmsMobileNo)
                                            .Replace("[SMSEMAILID]", _contactCandidates.SmsEmailId)
                                            .Replace("[SMSCONTACTPERSON]", _contactCandidates.SmsContactPerson)
                                            ,
                                           Constants.SmsSender.SecondaryType,
                                           Constants.SmsSender.Secondarysource,
                                           Constants.SmsSender.Secondarydlr,
                                           (LoggedInOrganization!=null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber)
                                           );

                                         }
                                         else if(SendToUser=="true")
                                         {
                                             if (candidate.ContactNumber != null)
                                             {
                                                 SmsHelper.SendSecondarySms(
                                                 Constants.SmsSender.SecondaryUserName,
                                                 Constants.SmsSender.SecondaryPassword,
                                                 Constants.SmsBody.SMSCandidateShortlistByEmployer
                                                 .Replace("[CANDIDATENAME]", candidate.Name)
                                                 .Replace("[InterviewDate]", _contactCandidates.InterviewDate)
                                                 .Replace("[InterviewLocation]", _contactCandidates.InterviewLocation)
                                                     //.Replace("[InterviewCompany]", _contactCandidates.InterviewCompany)
                                                 .Replace("[InterviewCompany]", (LoggedInOrganization!=null? LoggedInOrganization.Name : LoggedInConsultant.Name))
                                                 .Replace("[InterviewContactPerson]", _contactCandidates.InterviewContactPerson)
                                                     //.Replace("[InterviewContactNo]", _contactCandidates.InterviewContactNo)
                                                 .Replace("[InterviewContactNo]", (LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                                 .Replace("[InterviewPosition]", _contactCandidates.InterviewPosition)
                                                 .Replace("[InterviewJobSalary]", _contactCandidates.InterviewJobSalary)
                                                 .Replace("[InterviewJobLocation]", _contactCandidates.InterviewJobLocation)
                                                 .Replace("[InterviewJobAddress]", _contactCandidates.InterviewJobAddress),
                                                    Constants.SmsSender.SecondaryType,
                                                    Constants.SmsSender.Secondarysource,
                                                    Constants.SmsSender.Secondarydlr,
                                                    candidate.ContactNumber
                                                  );
                                             }



                                             string smsbody = Constants.SmsBody.SMSCandidateShortlistByEmployer
                                             .Replace("[CANDIDATENAME]", candidate.Name)
                                             .Replace("[InterviewDate]", _contactCandidates.InterviewDate)
                                             .Replace("[InterviewLocation]", _contactCandidates.InterviewLocation)
                                             .Replace("[InterviewCompany]", _contactCandidates.InterviewCompany)
                                             .Replace("[InterviewContactPerson]", _contactCandidates.InterviewContactPerson)
                                             .Replace("[InterviewContactNo]", _contactCandidates.InterviewContactNo)
                                             .Replace("[InterviewPosition]", _contactCandidates.InterviewPosition)
                                             .Replace("[InterviewJobSalary]", _contactCandidates.InterviewJobSalary)
                                             .Replace("[InterviewJobLocation]", _contactCandidates.InterviewJobLocation)
                                             .Replace("[InterviewJobAddress]", _contactCandidates.InterviewJobAddress);

                                             smslength = smsbody.Length;
                                             smssent = Math.Ceiling((double)smslength / 160);

                                             //if(LoggedInOrganization!=null)
                                             //{
                                             //   orderdetail = _vasRepository.GetEmployerSmsVas(LoggedInOrganization.Id);
                                             //}
                                             //else
                                             //{
                                             //    orderdetail = _vasRepository.GetConsultantSmsVas(LoggedInConsultant.Id);
                                             //}
                                             if (orderdetail != null)
                                             {
                                                 orderdetail.RemainingCount = orderdetail.RemainingCount - Convert.ToInt32(smssent);
                                                 _vasRepository.Save();
                                             }
                                         }

                                     }

                                     if (_contactCandidates.Title == "3")
                                     {

                                         if (SendToOrganization == "true")
                                         {
                                             //Location orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInOrganization.LocationId));
                                             SmsHelper.SendSecondarySms(
                                             Constants.SmsSender.SecondaryUserName,
                                             Constants.SmsSender.SecondaryPassword,
                                             Constants.SmsBody.SMSVacancyDirect
                                             .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                                             .Replace("[CONTACT_PERSON]",LoggedInOrganization != null ? LoggedInOrganization.ContactPerson:LoggedInConsultant.ContactPerson)
                                             .Replace("[EMAIL]", (LoggedInOrganization!=null? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                             .Replace("[MOBILE_NUMBER]",(LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                             .Replace("[POSITION]", _contactCandidates.SmsDirectPosition)
                                             .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name)
                                             ,
                                             Constants.SmsSender.SecondaryType,
                                             Constants.SmsSender.Secondarysource,
                                             Constants.SmsSender.Secondarydlr,
                                                LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber
                                              );

                                             string smsbody = Constants.SmsBody.SMSVacancyDirect
                                              .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : "")
                                              .Replace("[CONTACT_PERSON]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson:LoggedInConsultant.ContactPerson)
                                              .Replace("[EMAIL]", (LoggedInOrganization!=null? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                              .Replace("[MOBILE_NUMBER]", (LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                              .Replace("[POSITION]", _contactCandidates.SmsDirectPosition)
                                              .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name);
                                         }

                                         if(SendToUser=="true")
                                         {
                                            // Location orglocation = _repository.GetLocation(Convert.ToInt32(LoggedInOrganization.LocationId));
                                             SmsHelper.SendSecondarySms(
                                             Constants.SmsSender.SecondaryUserName,
                                             Constants.SmsSender.SecondaryPassword,
                                             Constants.SmsBody.SMSVacancyDirect
                                             .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                                             .Replace("[CONTACT_PERSON]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson:LoggedInConsultant.ContactPerson)
                                             .Replace("[EMAIL]", (LoggedInOrganization!=null? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                             .Replace("[MOBILE_NUMBER]", (LoggedInOrganization!=null? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                             .Replace("[POSITION]", _contactCandidates.SmsDirectPosition)
                                             .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name)
                                             ,
                                             Constants.SmsSender.SecondaryType,
                                             Constants.SmsSender.Secondarysource,
                                             Constants.SmsSender.Secondarydlr,
                                             candidate.ContactNumber
                                              );

                                             string smsbody = Constants.SmsBody.SMSVacancyDirect
                                            .Replace("[ORG_NAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name)
                                             .Replace("[CONTACT_PERSON]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson)
                                             .Replace("[EMAIL]", (LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.Email))
                                             .Replace("[MOBILE_NUMBER]", (LoggedInOrganization != null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                             .Replace("[POSITION]", _contactCandidates.SmsDirectPosition)
                                             .Replace("[LOCATION]", orglocation.City != null ? orglocation.City.Name : orglocation.Country.Name);

                                             smslength = smsbody.Length;
                                             smssent = Math.Ceiling((double)smslength / 160);

                                             if (orderdetail != null)
                                             {
                                                 orderdetail.RemainingCount = orderdetail.RemainingCount - Convert.ToInt32(smssent);
                                                 _vasRepository.Save();
                                             }
                                         }

                                     }

                                     
                                 }
                                 //send SMS
                             }

                             if (sendMethod == SendMethod.Email || sendMethod == SendMethod.Both)
                             {
                                 //if (candidate.Email != null)
                                 //{
                                     //if (candidate.Email != "")
                                     //{
                                         var selectedKeyCount = 0;
                                         if (LoggedInOrganization != null)
                                         {
                                             emailCount = _vasRepository.GetEmployerEmailCount(LoggedInOrganization.Id);
                                         }
                                         else
                                         {
                                             emailCount = _vasRepository.GetConsultantEmailCount(LoggedInConsultant.Id);
                                         }

                                         var selectedKeyword = Convert.ToInt32(collection.GetValues(key).Contains("true"));
                                         selectedKeyCount = selectedKeyCount + selectedKeyword;

                                         if (emailCount != null || emailCount != 0)
                                         {
                                             //var keycount = Convert.ToInt32(collection.GetValues(key));
                                             foreach (var email in selectedKeyCount.ToString())
                                             {

                                                 if (SendToOrganization == "true")
                                                 {
                                                     if (emailCount != null || emailCount != 0)
                                                     {
                                                         if (_contactCandidates.EmployerEmail != null)
                                                         {
                                                             EmailHelper.SendEmailReply(
                                                                   Constants.EmailSender.EmployerSupport,
                                                                   _contactCandidates.EmployerEmail,
                                                                      "Response for Your Vacancy",
                                                                   Constants.EmailBody.ResponseForResume
                                                                   .Replace("[CANDIDATENAME]", candidate.Name)
                                                                   .Replace("[CAND_MOBILE_NO]",(candidate.ContactNumber!=null ? candidate.ContactNumber : "NM"))
                                                                   .Replace("[CAND_EMAIL]",(candidate.Email!=null ? candidate.Email : "NM"))
                                                                   .Replace("[MOBILE_NO]", (LoggedInOrganization != null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber))
                                                                   .Replace("[NAME]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson)
                                                                   .Replace("[EMAIL]", LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.ContactPerson)
                                                                   .Replace("[EXPERIENCE]", _contactCandidates.MinExperience + " - " + _contactCandidates.MaxExperience)
                                                                   .Replace("[SALARY]", _contactCandidates.MinAnnualSalaryLakhs + " - " + _contactCandidates.MaxAnnualSalaryLakhs)
                                                                   .Replace("[OTHERSALARYDETAILS]", _contactCandidates.OtherSalary)
                                                                   .Replace("[JOBLOCATION]", _contactCandidates.JobLocation)
                                                                   .Replace("[MESSAGE]", message)
                                                                   .Replace("[EMPLOYERNAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name + "(Consultant)")
                                                                   .Replace("[EMPLOYERMAILID]", _contactCandidates.EmployerEmail),
                                                                   candidate.Email
                                                                   );
                                                         }
                                                        
                                                     }
                                                 }
                                                 if (SendToUser == "true")
                                                 {
                                                     if (candidate.Email != null)
                                                     {
                                                         EmailHelper.SendEmailReply(
                                                         Constants.EmailSender.CandidateSupport,
                                                          candidate.Email,
                                                         _contactCandidates.Subject,
                                                         Constants.EmailBody.ContactCandidate
                                                         .Replace("[CANDIDATENAME]", candidate.Name)
                                                         .Replace("[MOBILE_NO]", LoggedInOrganization != null ? LoggedInOrganization.MobileNumber : LoggedInConsultant.MobileNumber)
                                                         .Replace("[NAME]", LoggedInOrganization != null ? LoggedInOrganization.ContactPerson : LoggedInConsultant.ContactPerson)
                                                         .Replace("[EMAIL]", LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.Email)
                                                         .Replace("[EXPERIENCE]", _contactCandidates.MinExperience + " - " + _contactCandidates.MaxExperience)
                                                         .Replace("[SALARY]", _contactCandidates.MinAnnualSalaryLakhs + " - " + _contactCandidates.MaxAnnualSalaryLakhs)
                                                         .Replace("[OTHERSALARYDETAILS]", _contactCandidates.OtherSalary)
                                                         .Replace("[JOBLOCATION]", _contactCandidates.JobLocation)
                                                         .Replace("[MESSAGE]", message)
                                                         .Replace("[EMPLOYERNAME]", LoggedInOrganization != null ? LoggedInOrganization.Name : LoggedInConsultant.Name + "(Consultant)")
                                                         .Replace("[EMPLOYERMAILID]", _contactCandidates.EmployerEmail),
                                                         LoggedInOrganization != null ? LoggedInOrganization.Email : LoggedInConsultant.Email
                                                         );
                                                        
                                                     }
                                                 }

                                             }

                                             var balanceCount = emailCount - 1;
                                             if (LoggedInOrganization != null)
                                             {
                                                 orderdetail = _vasRepository.UpdateEmailCount(LoggedInOrganization.Id);
                                             }
                                             else
                                             {
                                                 orderdetail = _vasRepository.UpdateConsultantEmailCount(LoggedInConsultant.Id);
                                             }

                                             if (orderdetail != null)
                                             {
                                                 orderdetail.EmailRemainingCount = balanceCount;
                                                 _vasRepository.Save();
                                             }
                                         }
                                    
                             }
                             

                         }
                     }
                 }
             }

             string msg = sendMethod == SendMethod.SMS ? "SMS" : sendMethod == SendMethod.Email ? "Email" : "SMS / Email";

             return Json(new JsonActionResult { Success = true, Message = "Successfully sent the " + msg + " to Selected candidates" });
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

   
        [Authorize]
        // [ValidateAntiForgeryToken]
        public ActionResult Details(string id)
        {
            if (ModelState.IsValid)
            {
                id = Constants.DecryptString(id.ToString());
                int Id;
                bool ValueIsAnId = int.TryParse(id, out Id);
                Candidate candidate = _repository.GetCandidate(Id);
                bool dprActivatedOrNot = _vasRepository.PlanSubscribedForDPR(Id);

                if (LoggedInOrganization != null)//dprActivatedOrNot==true)
                {
                    _repository.GetLogForCandidateViewByEmployer(Convert.ToInt32(candidate.Id), Convert.ToInt32(LoggedInOrganization.Id),0);
                }

                else if(LoggedInConsultant!=null)
                {
                    _repository.GetLogForCandidateViewByEmployer(Convert.ToInt32(candidate.Id), 0, Convert.ToInt32(LoggedInConsultant.Id));
                }

                return View(candidate);
            }
            return View("MatchCandidates", "Employer");

        }

        public ActionResult DPRViewedList(int candidateId)
        {
            /*Developer Note: When Candidate gets logged in they can view the reports in dashboard which employer have viewed their profiles.*/
            if (LoggedInCandidate != null)
            {
                ViewData["CandidateId"] = LoggedInCandidate.Id;
            }
            else
            {
                ViewData["CandidateId"] = candidateId;
            }
            
            return View();
        }

        public JsonResult ListDPRViewedList(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate, int candidateId)
        {
            IQueryable<DPRLog> alertsLog = _vasRepository.GetDPRLogs().Where(od => od.CandidateId == candidateId);

            Func<IQueryable<DPRLog>, IOrderedQueryable<DPRLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.ViewedDate);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else //if  (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    //else
                    //    return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.ViewedDate);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else //if  (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.CandidateId.ToString().Contains(sSearch.Trim()) || o.ViewedDate.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.ViewedDate != null && o.ViewedDate >= from && o.ViewedDate <= to);

            }

            IEnumerable<DPRLog> alertsLog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = alertsLog.Count(),
                iTotalDisplayRecords = alertsLog.Count(),

                aaData = alertsLog1.Select(o => new object[] { o.CandidateId, (_repository.GetCandidateNameById(o.CandidateId)), (o.ViewedDate.Value.ToString("dd-MM-yyyy")), (o.OrganizationId!=null ? (_repository.GetOrganizationNameById(Convert.ToInt32(o.OrganizationId))):(_repository.GetConsultantNameById(Convert.ToInt32(o.ConsultantId)))+ "(Consultant)" ) })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
               

        public ActionResult Roles(int functionId)
        {
           // var roles = _repository.GetRoles(functionId).ToList();
            var candidateRoles = _repository.GetRoleByFunctionId(functionId);
            var jsonRoles = new List<JsonRole>();
            foreach (Role role in candidateRoles)
            {
                JsonRole jsonRole = new JsonRole();
                jsonRole.Id = role.Id;
                jsonRole.Name = role.Name;
                jsonRole.FunctionId = role.FunctionId;
                jsonRoles.Add(jsonRole);
            }

            return Json(new JsonRolesResult { Success = true, Roles = jsonRoles }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFunctionsByRoles(int roleId)
        {

            int functionId = _repository.GetFunctionIdbyRole(roleId);
            var functionName = _repository.GetFunctionNameByFunctionId(functionId);
            var jsonFunctions = new List<JsonFunction>();
            //foreach (var func in functionName)
            //{
                JsonFunction jsonfunction = new JsonFunction();
                jsonfunction.Name = functionName.ToString();
                jsonFunctions.Add(jsonfunction);
            //}
            return Json(new JsonRolesResult { Success = true, Functions= jsonFunctions }, JsonRequestBehavior.AllowGet);

        }


        //public ActionResult DprLogCount(string candidateId, string organizationId)
        //{
        //    if (LoggedInOrganization != null)
        //    {
        //        _vasRepository.GetLogForDPR(Convert.ToInt32(candidateId), Convert.ToInt32(organizationId));
        //    }
        //    return RedirectToAction("Details", "Candidates", new { id = Constants.EncryptString(candidateId) });
        //}


        public int RemainingCount(int id)
        {
            
            try
            {
                int resumesCanView = 0;
                if (LoggedInOrganization != null)
                {
                    resumesCanView = _vasRepository.CheckForHorsRemainingCount(LoggedInOrganization.Id, id);
                } else {
                    resumesCanView = _vasRepository.CheckForHorsRemainingCountConsultant(LoggedInConsultant.Id, id);
                }

                //int basicPlanResumeView= 

                if (resumesCanView != 0 && resumesCanView != 9999)
                    ViewData["ResumesCount"] = resumesCanView;

                return resumesCanView;
            }
            catch (Exception ex)
            {
                return 9999;
            }
        }


        public ActionResult ResetCount()
        {
            return View();
        }



        public ActionResult PhoneNoVerification()
        {
            ViewData["CandidateId"] = LoggedInCandidate.Id;
            return View();
        }
        //BY VIGNESH
        //public ActionResult PhoneNoVerification(FormCollection collection, string sendMethod)
        //{
        //    int candidateId = 0;

        //    if (sendMethod == "1")
        //    {
        //        return Json(new JsonActionResult
        //        {
        //            Success = true,
        //            //Message = candidate.UserName,
        //            ReturnUrl = VirtualPathUtility.ToAbsolute("~/Candidates/Edit/")

        //        });
        //    }
        //}

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult PhoneNoVerification(FormCollection collection, string sendMethod)
        {
            int candidateId = 0;

            //Developer Note: Assigned without verification as 1 in ajax call. Here i am checking the return type.
            if (sendMethod == "1")
            {
                return Json(new JsonActionResult
                {
                    Success = false,
                    ReturnUrl = VirtualPathUtility.ToAbsolute("~/Candidates/Edit/")

                });
            }

            string phoneVerfication = string.Empty;
            if (collection != null && collection.AllKeys.Contains("CandidateId"))
                candidateId = Convert.ToInt32(collection["CandidateId"].ToString());

            var candidate = _userRepository.GetCandidateById(candidateId);
            if (collection != null && collection.AllKeys.Contains("CandidateId"))
                phoneVerfication = collection["PhVerificationNo"].ToString();

            if (string.IsNullOrEmpty(phoneVerfication))
                return Json(new JsonActionResult { Message = "Please Enter Verification Code." });
            else if (!string.Equals(phoneVerfication, candidate.PhoneVerificationNo.ToString()))
                return Json(new JsonActionResult { Message = "Verification code is not Match. Please Verify again." });
            else if (string.Equals(phoneVerfication, candidate.PhoneVerificationNo.ToString()))
                candidate.IsPhoneVerified = true;
            else
                if (!string.IsNullOrEmpty(collection["IsPhoneVerified"]))
                    candidate.IsPhoneVerified = Convert.ToBoolean(collection.GetValues("IsPhoneVerified").Contains("true"));

             _userRepository.Save();

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = candidate.UserName,
                    ReturnUrl = VirtualPathUtility.ToAbsolute("~/Candidates/Edit/")

                });

            //}
        }

        public ActionResult VerifyCandidate(FormCollection collection)
        {
            Candidate candidate = _userRepository.GetCandidateById(LoggedInCandidate.Id);
            //Candidate candidate = new Candidate();
            Random randomNo = new Random();
            string verificationNumber = randomNo.Next(1000, 9999).ToString();
            candidate.PhoneVerificationNo = Convert.ToInt32(verificationNumber);
            _userRepository.Save();

            SmsHelper.SendSecondarySms(
                       Constants.SmsSender.SecondaryUserName,
                       Constants.SmsSender.SecondaryPassword,
                       Constants.SmsBody.SMSMobileVerification
                                       .Replace("[NAME]",candidate.Name != null ? candidate.Name : "")
                                       .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                       candidate.ContactNumber
                       );
          
            //SmsHelper.Sendsms(
            //            Constants.SmsSender.UserId,
            //            Constants.SmsSender.Password,
            //            Constants.SmsBody.SMSMobileVerification
            //                            .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

            //            Constants.SmsSender.Type,
            //            Constants.SmsSender.senderId,
            //            candidate.ContactNumber
            //            );

            return RedirectToAction("PhoneNoVerification", "Candidates");
        }

      
        public ActionResult Activation(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                Id = Constants.DecryptString(Id);
                var candidate = _userRepository.GetCandidateById(Convert.ToInt32(Id));
                candidate.IsMailVerified = true;
                _userRepository.Save();
            }
            //return RedirectToAction("Edit", "Candidates");
            return RedirectToAction("Index", "Jobs");

          
        }

        public ActionResult VerifyEmail()
        {
            Candidate candidate = _userRepository.GetCandidateById(LoggedInCandidate.Id);
            EmailHelper.SendEmail(
                    Constants.EmailSender.CandidateSupport,
                    candidate.Email,
                    Constants.EmailSubject.EmailVerification,
                    Constants.EmailBody.EmailVerification
                        .Replace("[EMAIL]", candidate.Email)
                        .Replace("[LINK_NAME]","Verify Email")
                        .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Constants.EncryptString(candidate.Id.ToString()))
                        );
            return RedirectToAction("Edit", "Candidates");
        }

        public ActionResult GetDetail(string validateEmail, string validateMobile)
        {
            UserRepository _userRepository = new UserRepository();
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(validateEmail))
            {
                candidate = _userRepository.GetCandidateByEmail(validateEmail);
                if (candidate == null)
                    return RedirectToAction("VerifyEmail", "Candidates");
            }
            else if (!string.IsNullOrEmpty(validateMobile))
            {
                candidate = _userRepository.GetCandidateByMobileNumber(validateMobile);
                if (candidate == null)
                    return RedirectToAction("VerifyEmail", "Candidates");
            }
            return View("Edit", "Candidates");
        }

        
    }

}
