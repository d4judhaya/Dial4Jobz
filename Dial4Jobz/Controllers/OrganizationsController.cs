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

namespace Dial4Jobz.Controllers
{
    public class OrganizationsController : BaseController
    {
        Repository _repository = new Repository();
        const int MAX_ADD_NEW_INPUT = 25;

        //
        // GET: /Organizations/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Organizations/Add

        public ActionResult Add()
        {
            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name");
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name");
            ViewData["BasicQualificationDegrees"] = new SelectList(_repository.GetDegrees(DegreeType.BasicQualification), "Id", "Name");
            ViewData["PostGraduationDegrees"] = new SelectList(_repository.GetDegrees(DegreeType.PostGraduation), "Id", "Name");
            ViewData["DoctorateDegrees"] = new SelectList(_repository.GetDegrees(DegreeType.Doctorate), "Id", "Name");
            return View();
        }

        //
        // POST: /Organizations/Add

        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Add(FormCollection collection)
        {
            
                // Add new candidate record (primary table info)
                Organization organization = new Organization();
                organization.Name = collection["Name"];

                if (!string.IsNullOrEmpty(collection["Industries"]))
                    organization.IndustryId = Convert.ToInt32(collection["Industries"]);
                
                organization.ContactPerson = collection["ContactPerson"];
                organization.Email = collection["Email"];
                organization.Website = collection["Website"];
                organization.ContactNumber = collection["ContactNumber"];
                organization.MobileNumber = collection["MobileNumber"];

                Location location = new Location();
                if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
                if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
                if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
                if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

                if (location.CountryId != 0)
                    organization.LocationId = _repository.AddLocation(location);

                if (!TryValidateModel(organization))
                    return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

                int organizationId = _repository.AddOrganization(organization);

                Job job = new Job();
                job.OrganizationId = organizationId;
                
                if (!string.IsNullOrEmpty(collection["Functions"]))
                    job.FunctionId = Convert.ToInt32(collection["Functions"]);

                job.Position = collection["Position"];

                if (!string.IsNullOrEmpty(collection["Male"]))
                    job.Male = Convert.ToBoolean(collection.GetValues("Male").Contains("true"));

                if (!string.IsNullOrEmpty(collection["Female"]))
                    job.Female = Convert.ToBoolean(collection.GetValues("Female").Contains("true"));  
         
                //long yearsInSeconds = Convert.ToInt64(collection["TotalExperienceYears"]) * 365 * 24 * 60 * 60;
                //long monthsInSeconds = Convert.ToInt64(collection["TotalExperienceMonths"]) * 31 * 24 * 60 * 60;
                //job.TotalExperience = yearsInSeconds + monthsInSeconds;
                long minExperience = Convert.ToInt64(collection["MinExperienceYears"]) * 365 * 24 * 60 * 60;
                long maxExperience = Convert.ToInt64(collection["MaximumExperienceYears"]) * 365 * 24 * 60 * 60;
                job.MinExperience = minExperience;
                job.MaxExperience = maxExperience;

                job.Budget = (Convert.ToInt32(collection["BudgetLakhs"]) * 100000) + (Convert.ToInt32(collection["BudgetThousands"]) * 1000);
                job.MaxBudget = (Convert.ToInt32(collection["MaximumSalaryLakhs"]) * 100000) + (Convert.ToInt32(collection["MaximumSalaryThousands"]) * 1000);
                            
                if (!string.IsNullOrEmpty(collection["PreferredType"]))
                    job.PreferredType = Convert.ToInt32(collection["PreferredType"]);

                job.ContactPerson = collection["RequirementsContactPerson"];
                job.ContactNumber = collection["RequirementsContactNumber"];
                job.MobileNumber = collection["RequirementsMobileNumber"];
                job.EmailAddress = collection["RequirementsEmailAddress"];

                if (!string.IsNullOrEmpty(collection["CommunicationEmail"]))
                    job.CommunicateViaEmail = Convert.ToBoolean(collection.GetValues("CommunicationEmail").Contains("true"));

                if (!string.IsNullOrEmpty(collection["CommunicationSMS"]))
                    job.CommunicateViaSMS = Convert.ToBoolean(collection.GetValues("CommunicationSMS").Contains("true"));

                if (!TryValidateModel(job))
                    return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

                int jobId = _repository.AddJob(job);

                // Add skills for job
                string[] skills = collection["Skills"].Split(',');
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

                // Add preferred industries for job
                string[] preferredIndustries = collection["PreferredIndustries"].Split(',');
                foreach (string pi in preferredIndustries)
                {
                    if (!string.IsNullOrEmpty(pi))
                    {
                        JobPreferredIndustry jpi = new JobPreferredIndustry();
                        jpi.JobId = jobId;
                        jpi.IndustryId = Convert.ToInt32(pi);
                        _repository.AddJobPreferredIndustry(jpi);
                    }
                }

                //add posting locations for job
                for (int i = 1; i <= MAX_ADD_NEW_INPUT; i++)
                {
                    string countryId = "PostingCountry" + i.ToString();
                    string stateId = "PostingState" + i.ToString();
                    string cityId = "PostingCity" + i.ToString();
                    string regionId = "PostingRegion" + i.ToString();

                    if (!string.IsNullOrEmpty(collection[countryId]))
                    {
                        location = new Location();
                        if (!string.IsNullOrEmpty(collection[countryId])) location.CountryId = Convert.ToInt32(collection[countryId]);
                        if (!string.IsNullOrEmpty(collection[stateId])) location.StateId = Convert.ToInt32(collection[stateId]);
                        if (!string.IsNullOrEmpty(collection[cityId])) location.CityId = Convert.ToInt32(collection[cityId]);
                        if (!string.IsNullOrEmpty(collection[regionId])) location.RegionId = Convert.ToInt32(collection[regionId]);
                        int locationId = _repository.AddLocation(location);

                        JobLocation jl = new JobLocation();
                        jl.JobId = jobId;
                        jl.LocationId = locationId;

                        _repository.AddJobLocation(jl);
                    }
                }

                //add required degrees for job
                for (int i = 1; i <= MAX_ADD_NEW_INPUT; i++)
                {
                    string basicQualificationDegreeId = "BasicQualificationDegree" + i.ToString();
                    string basicQualificationSpecialization = "BasicQualificationSpecialization" + i.ToString();
                    string postGraduationDegreeId = "PostGraduationDegree" + i.ToString();
                    string postGraduationSpecialization = "PostGraduationSpecialization" + i.ToString();
                    string doctorateDegreeId = "DoctorateDegree" + i.ToString();
                    string doctorateSpecialization = "DoctorateSpecialization" + i.ToString();

                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;

                    if (!string.IsNullOrEmpty(collection[basicQualificationDegreeId]))
                    {
                        jrq.DegreeId = Convert.ToInt32(collection[basicQualificationDegreeId]);
                        jrq.Specialization = collection[basicQualificationSpecialization];
                        _repository.AddJobRequiredQualification(jrq);
                    }

                    if (!string.IsNullOrEmpty(collection[postGraduationDegreeId]))
                    {
                        jrq.DegreeId = Convert.ToInt32(collection[postGraduationDegreeId]);
                        jrq.Specialization = collection[postGraduationSpecialization];
                        _repository.AddJobRequiredQualification(jrq);
                    }

                    if (!string.IsNullOrEmpty(collection[doctorateDegreeId]))
                    {
                        jrq.DegreeId = Convert.ToInt32(collection[doctorateDegreeId]);
                        jrq.Specialization = collection[doctorateSpecialization];
                        _repository.AddJobRequiredQualification(jrq);
                    }
                }

                _repository.Save();

                List<Candidate> matchingCandidates = _repository.GetMatchingCandidates(job).ToList();
            
                return Json(new JsonActionResult { Success = true, Html = RenderPartialViewToString("MatchingCandidates", matchingCandidates) });
        }

        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Send(FormCollection collection)
        {
            foreach (string key in collection.AllKeys)
            {
                //process each candidate that is selected.
                if (key.Contains("Candidate"))
                {
                    if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                    {
                        int cadidateId = Convert.ToInt32(key.Replace("Candidate", string.Empty));
                        Candidate candidate = _repository.GetCandidate(cadidateId);

                        if (candidate == null)
                            return new FileNotFoundResult();

                        //send email or sms 
                    }
                }
            }

            return View();
        }       
        
    }
}
