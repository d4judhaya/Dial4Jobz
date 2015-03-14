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
using System.Net;

namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class JobPostingController : Controller
    {
        //
        // GET: /Admin/JobPosting/

        Repository _repository = new Repository();
        UserRepository _userRepository = new UserRepository();

        public ActionResult Add()
        {
            string[] userIdentityName = this.User.Identity.Name.Split('|');
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null || (userIdentityName != null && userIdentityName.Length > 1))
            {
                Permission adminPermission = new Permission();
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                string pageAccess = "";
                string[] Page_Code = null;
                foreach (var page in pageaccess)
                {
                    adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = adminPermission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + adminPermission.Name + ",";
                    }
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if ((userIdentityName != null && userIdentityName.Length > 1) || (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddJob)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true))
                {
                    SetCommonViewData();
                    SetAddJobViewData();
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
            //preferred time

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

            ViewData["LicenseTypes"] = _repository.GetLicenseTypes();


            var indus = _repository.GetIndustriesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            indus.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["Industries"] = indus;

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

        [Authorize, HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            Job job = new Job();
            job.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            job.OrganizationId = -1;

            if (SetJobDetails(job, collection))
            {
                if (job.EmailAddress == "" && job.MobileNumber == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Please Enter Mobile No or Email Address" });
                }
                else
                {
                    _repository.Save();
                    if (job.CommunicateViaEmail == true && job.EmailAddress != null)
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        job.EmailAddress,
                        Constants.EmailSubject.ClientPost,
                           Constants.EmailBody.AdminJobPosting
                        );
                    }

                    if (job.CommunicateViaSMS == true && job.MobileNumber != null)
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.MarketingUserName,
                            Constants.SmsSender.MarketingPassword,
                            Constants.SmsBody.AdminSMSPostVacancy,
                            Constants.SmsSender.MarketingType,
                            Constants.SmsSender.Marketingsource,
                            Constants.SmsSender.Marketingdlr,
                            job.MobileNumber
                            );
                    }

                }

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Job has been Posted Successfully",
                    ReturnUrl = "/Admin/JobPosting/Add"
                });

            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            }
        }

        public string ModelStateErrorMessage
        {
            get
            {
                string errorMessage = string.Empty;
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        return error.ErrorMessage;
                    }
                }

                return errorMessage;
            }
        }

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

            if (!string.IsNullOrEmpty(collection["GeneralShift"]))
                job.GeneralShift = Convert.ToBoolean(collection.GetValues("GeneralShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["NightShift"]))
                job.NightShift = Convert.ToBoolean(collection.GetValues("NightShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["TwoWheeler"]))
                job.TwoWheeler = Convert.ToBoolean(collection.GetValues("TwoWheeler").Contains("true"));

            if (!string.IsNullOrEmpty(collection["FourWheeler"]))
                job.FourWheeler = Convert.ToBoolean(collection.GetValues("FourWheeler").Contains("true"));


            job.PreferredTimeFrom = collection["ddlPreferredTimeFrom"];
            job.PreferredTimeTo = collection["ddlPreferredTimeto"];

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
                if (!string.IsNullOrEmpty(licenseType))
                {
                    JobLicenseType jlt = new JobLicenseType();
                    jlt.JobId = jobId;
                    jlt.LicenseTypeId = Convert.ToInt32(licenseType);
                    _repository.AddJobLicenseType(jlt);
                }
            }


            //Add preferred industries for job

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
                if ((!string.IsNullOrEmpty(preferredindustry)) && preferredindustry != "0")
                {
                    JobPreferredIndustry jpi = new JobPreferredIndustry();
                    jpi.JobId = jobId;
                    jpi.IndustryId = Convert.ToInt32(preferredindustry);
                    _repository.AddJobPreferredIndustry(jpi);
                }
            }


            //Roles
            string[] roles = collection["Roles"].Split(',');

            if (roles.Count() != 0 || roles.Count() != -1)
                _repository.DeleteJobRoles(jobId);

            foreach (string preferredRole in roles)
            {
                if ((!string.IsNullOrEmpty(preferredRole)) && preferredRole != "-1" && preferredRole != "0")
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

            //add required degrees for job
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

            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                AdminUserEntry adminuserentry = new AdminUserEntry();
                adminuserentry.AdminId = user.Id;
                adminuserentry.EntryId = job.Id;
                adminuserentry.EntryType = Convert.ToInt32(EntryType.Job);
                adminuserentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                _repository.AddAdminUserEntry(adminuserentry);
                _repository.Save();
            }

            return true;
        }
    }
}
