using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Controllers;
using Dial4Jobz.Models.Filters;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Helpers;
using System.Text;
using System.Configuration;

namespace Dial4Jobz.Controllers
{
    public class CandidateMatchesJobController : BaseController
    {
        //
        // GET: /Admin/CandidateMatches/
         Repository _repository = new Repository();
         VasRepository _vasRepository = new VasRepository();

         bool smsSent;
         bool mailSent;


        public ActionResult CandidateMatch(string Id)
        {
            IQueryable<Candidate> candidates;
            if (!string.IsNullOrEmpty(Id))
            {
                Job job = _repository.GetJob(Convert.ToInt32(Id));

                if (job != null)
                {
                    string basicqualification = string.Empty;
                    string postgraduation = string.Empty;
                    string doctrate = string.Empty;
                    foreach (Dial4Jobz.Models.JobRequiredQualification cq in job.JobRequiredQualifications)
                    {
                        if (cq.Degree.Type == 0)
                        {                            
                            if (basicqualification == string.Empty)
                                basicqualification += cq.Degree.Id;
                            else
                                basicqualification += "," + cq.Degree.Id;
                        }
                        if (cq.Degree.Type == 1)
                        {
                            if (postgraduation == string.Empty)
                                postgraduation += cq.Degree.Id;
                            else
                                postgraduation += "," + cq.Degree.Id;
                        }
                        if (cq.Degree.Type == 2)
                        {
                            if (doctrate == string.Empty)
                                doctrate += cq.Degree.Id;
                            else
                                doctrate += "," + cq.Degree.Id;
                        }
                    }

                    string function = (job.FunctionId.HasValue && job.FunctionId != 0) ? job.FunctionId.Value.ToString() : string.Empty;

                    string roles = string.Empty;
                    foreach (JobRole jr in job.JobRoles)
                    {
                        if (roles == string.Empty)
                            roles = jr.RoleId.ToString();
                        else
                            roles += "," + jr.RoleId;
                    }

                    string preferredindustries = string.Empty;
                    foreach (JobPreferredIndustry jpi in job.JobPreferredIndustries)
                    {
                        if (preferredindustries == string.Empty)
                            preferredindustries = jpi.IndustryId.ToString();
                        else
                            preferredindustries += "," + jpi.IndustryId;
                    }

                    string male = string.Empty;
                    string female = string.Empty;
                    string gender = string.Empty;

                    if (job.Male == true)
                        // male = "Male";
                        gender = "Male";

                    if (job.Female == true)
                        //  female = "Female";
                        gender = "Female";

                    if (job.Female == true && job.Male == true)
                        gender = "";

                    //string minExperience = string.Empty;
                    //string maxExperience = string.Empty;
                    string minExperience = "0";
                    string maxExperience = "0";

                    if (job.MinExperience != null && job.MaxExperience != null)
                    {
                        minExperience = job.MinExperience.ToString();
                        maxExperience = job.MaxExperience.ToString();
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

                    string skills = string.Empty;
                    foreach (JobSkill js in job.JobSkills)
                    {
                        if (skills == string.Empty)
                            skills = js.Skill.Name;
                        else
                            skills += "," + js.Skill.Name;
                    }

                    string licenseTypes = string.Empty;
                    foreach (JobLicenseType jlt in job.JobLicenseTypes)
                    {
                        if (licenseTypes == string.Empty)
                            licenseTypes = jlt.LicenseType.Name;
                        else
                            licenseTypes += "," + jlt.LicenseType.Name;
                    }

                    string languages = string.Empty;

                    foreach (JobLanguage jl in job.JobLanguages)
                    {
                        if (languages == string.Empty)
                            languages = jl.Language.Name;
                        else
                            languages += "," + jl.Language.Name;
                    }

                    string fulltime = string.Empty, parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;
                    
                    if (job.PreferredFulltime == true)
                        fulltime = "fulltime";

                    if (job.PreferredContract == true)
                        contract = "contract";

                    if (job.PreferredParttime == true)
                        parttime = "parttime";

                    if (job.PreferredWorkFromHome == true)
                        workfromhome = "workfromhome";

                    string generalShift = string.Empty, noonShift = string.Empty, nightShift = string.Empty;

                    if (job.GeneralShift == true)
                        generalShift = "GeneralShift";

                    if (job.NightShift == true)
                        nightShift = "NightShift";

                    string cityName = string.Empty;
                    string countryName = string.Empty;

                    string preferredlocations = string.Empty;
                    if (job.JobLocations != null)
                    {
                        foreach (JobLocation jl in job.JobLocations)
                        {
                            if (cityName == string.Empty)
                            {
                                if (jl.Location.City != null)
                                {
                                    cityName = jl.Location.City.Name;
                                }
                                else
                                {

                                }
                            }

                            if (countryName == string.Empty)
                            {
                                if (jl.Location.Country != null)
                                {
                                    countryName = jl.Location.Country.Name;
                                }
                                else
                                {

                                }
                            }

                        }
                    }

                    candidates = _repository.GetMatchingCandiatesForJob(basicqualification, postgraduation, doctrate, function, cityName, countryName, roles, preferredindustries, male, female, gender, minExperience, maxExperience, minSalary, maxSalary, skills, languages, fulltime, parttime, contract, workfromhome, generalShift, nightShift, licenseTypes);

                    DateTime currentdate = Constants.CurrentTime();
                    DateTime fresh = currentdate;
                    //fresh = DateTime.Now.AddDays(-1);
                    fresh = DateTime.Now.AddHours(-1);


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
                            return query.OrderByDescending(rslt => lstCandidateId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

                        else if (updatedCandidatesList.Count() > 0)
                            return query.OrderByDescending(rslt => lstUpdatedCandidatesId.Contains(rslt.Id)).ThenByDescending(rslt => rslt.CreatedDate);

                        else if (CandidatesListByCreatedDate.Count() > 0)
                            return query.OrderByDescending(rslt => lstCreatedDatewiseCandidate.Contains(rslt.Id)).ThenByDescending(rslt => rslt.UpdatedDate);

                        else
                            return query.OrderByDescending(rslt => rslt.CreatedDate);
                    };

                    candidates = orderingFunc(candidates);
                    

                    List<int> lstCandidates = null;
                    if (candidates.Count() > 0)
                    {
                        var matchcandidates = from alertcandidate in candidates
                                           orderby alertcandidate.CreatedDate descending
                                           select alertcandidate.Id;
                        lstCandidates = matchcandidates.ToList();

                        IEnumerable<OrderMaster> CandidateIds = _vasRepository.GetMatchingCandidatestoAlert(lstCandidates);

                        bool isAlertSent = false;

                        foreach (var alertcandidate in CandidateIds)
                        {
                            bool canAlertSent = _vasRepository.CheckForAlertsRemainedRAJ((int)alertcandidate.CandidateId);

                            //********If already this job sent to candidate, again it won't send.******//
                            bool checkalreadysentjob = _vasRepository.AlreadyALertsentorNot((int)alertcandidate.CandidateId, job.Id);

                            if (canAlertSent == true && checkalreadysentjob == true)
                            {
                                isAlertSent = SendSMSandMail((int)alertcandidate.CandidateId, job.Id);
                                var planId = _vasRepository.RAJPlanActivatedDetails((int)alertcandidate.CandidateId);

                                if (isAlertSent == true)
                                {
                                    _vasRepository.UpdateCandAlertsDoneCount((int)alertcandidate.CandidateId);
                                    _vasRepository.logCandlerts((int)alertcandidate.CandidateId,(int)planId,job.Id,smsSent,mailSent);
                                }
                            }
                        }


                    }
                   

                }
                else
                {
                    candidates = _repository.GetCandidates();
                }

            }
            else
            {
                candidates = _repository.GetCandidates();
            }

            int page = 1;

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = candidates.Count();

            var CandidateResults = candidates.Skip(skip).Take(take);

            ViewData.Add("JobIdView", Id == "0" ? "" : Id);
            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(CandidateResults);

        }

        public bool SendSMSandMail(int candId, int jobId)
        {
            smsSent = false;
            mailSent = false;
            try
            {
                Job job = _repository.GetJob(jobId);
                Candidate candidate = _repository.GetCandidate(Convert.ToInt32(candId));
                string basicqualification = string.Empty;
                string basicqualificationName = string.Empty;
                string postgraduation = string.Empty;
                string postgraduationName = string.Empty;
                string doctrate = string.Empty;
                string doctrateName = string.Empty;

                foreach (Dial4Jobz.Models.JobRequiredQualification cq in job.JobRequiredQualifications)
                {
                    if (cq.Degree.Type == 0)
                    {
                        basicqualificationName = cq.Degree.Name;
                        if (basicqualification == string.Empty)
                            basicqualification += cq.Degree.Id;
                        else
                            basicqualification += "," + cq.Degree.Id;
                    }
                    if (cq.Degree.Type == 1)
                    {
                        postgraduationName = cq.Degree.Name;
                        if (postgraduation == string.Empty)
                            postgraduation += cq.Degree.Id;
                        else
                            postgraduation += "," + cq.Degree.Id;
                    }
                    if (cq.Degree.Type == 2)
                    {
                        doctrateName = cq.Degree.Name;
                        if (doctrate == string.Empty)
                            doctrate += cq.Degree.Id;
                        else
                            doctrate += "," + cq.Degree.Id;
                    }
                }

                string function = (job.FunctionId.HasValue && job.FunctionId != 0) ? job.FunctionId.Value.ToString() : string.Empty;

                string roles = string.Empty;
                foreach (JobRole jr in job.JobRoles)
                {
                    if (roles == string.Empty)
                        roles = jr.RoleId.ToString();
                    else
                        roles += "," + jr.RoleId;
                }

                string preferredindustries = string.Empty;
                foreach (JobPreferredIndustry jpi in job.JobPreferredIndustries)
                {
                    if (preferredindustries == string.Empty)
                        preferredindustries = jpi.IndustryId.ToString();
                    else
                        preferredindustries += "," + jpi.IndustryId;
                }

                string male = string.Empty;
                string female = string.Empty;
                if (job.Male == true)
                    male = "Male";

                if (job.Female == true)
                    female = "Female";

                string minExperience = string.Empty;
                string maxExperience = string.Empty;

                if (job.MinExperience != null && job.MaxExperience != null)
                {
                    minExperience = job.MinExperience.ToString();
                    maxExperience = job.MaxExperience.ToString();
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

                string cityName = string.Empty;
                string countryName = string.Empty;
                string stateName = string.Empty;
                string regionName = string.Empty;

                string preferredlocations = string.Empty;
                if (job.JobLocations != null)
                {
                    foreach (JobLocation jl in job.JobLocations)
                    {

                        if (regionName == string.Empty)
                        {
                            if (jl.Location.Region != null)
                            {
                                regionName = jl.Location.Region.Name;
                            }
                            else
                            {

                            }
                        }

                        if (stateName == string.Empty)
                        {
                            if (jl.Location.State != null)
                            {
                                stateName = jl.Location.State.Name;
                            }
                            else
                            {

                            }
                        }


                        if (cityName == string.Empty)
                        {
                            if (jl.Location.City != null)
                            {
                                cityName = jl.Location.City.Name;
                            }
                            else
                            {

                            }
                        }

                        if (countryName == string.Empty)
                        {
                            if (jl.Location.Country != null)
                            {
                                countryName = jl.Location.Country.Name;
                            }
                            else
                            {
                            }
                        }

                    }
                }

                string skills = string.Empty;
                foreach (JobSkill js in job.JobSkills)
                {
                    if (skills == string.Empty)
                        skills = js.Skill.Name;
                    else
                        skills += "," + js.Skill.Name;
                }

                string languages = string.Empty;
                foreach (JobLanguage jl in job.JobLanguages)
                {
                    if (languages == string.Empty)
                        languages = jl.Language.Name;
                    else
                        languages += "," + jl.Language.Name;
                }


                string preferredtime = string.Empty;

                if (job.PreferredFulltime == true)
                    preferredtime = "fulltime";

                if (job.PreferredContract == true)
                    preferredtime = "contract";

                if (job.PreferredParttime == true)
                    preferredtime = "parttime";

                if (job.PreferredWorkFromHome == true)
                    preferredtime = "workfromhome";

                string generalShift = string.Empty, noonShift = string.Empty, nightShift = string.Empty;

                if (job.GeneralShift == true)
                    generalShift = "GeneralShift";

                if (job.NightShift == true)
                    nightShift = "NightShift";


                /**Get Candidate Details***/

                string candidateexperience = (candidate.TotalExperience.HasValue && candidate.TotalExperience != 0) ? (candidate.TotalExperience.Value / 31104000).ToString() + " Years " + (((candidate.TotalExperience.Value - (candidate.TotalExperience.Value / 31104000) * 31536000)) / 2678400) + " Months" : "0";
                string candidateannualsalary = (candidate.AnnualSalary.HasValue && candidate.AnnualSalary != 0) ? Convert.ToInt32(candidate.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN")) : "";
                string candidateindustry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.GetIndustry(candidate.IndustryId.Value).Name : "";
                string candidatelanguages = string.Empty;

                foreach (Dial4Jobz.Models.CandidateLanguage cla in candidate.CandidateLanguages)
                {
                    candidatelanguages += cla.Language.Name + ",";
                }

                string candidatebasic = string.Empty;
                string candidatepost = string.Empty;
                string candidatedoctorate = string.Empty;
                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 0))
                {
                    if (cq != null)
                    {
                        if (cq.Degree != null && cq.Specialization != null)
                        {
                            candidatebasic += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {
                            candidatebasic += cq.Degree.Name;
                        }
                    }
                    else
                    {

                    }
                }

                foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 1))
                {
                    if (cq != null)
                    {

                        if (cq.Specialization != null)
                        {
                            candidatepost += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {
                            candidatepost += cq.Degree.Name + ",";
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
                            candidatedoctorate += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {
                            candidatedoctorate += cq.Degree.Name + ",";
                        }
                    }

                }

                string candidatelicense = string.Empty;
                foreach (Dial4Jobz.Models.CandidateLicenseType clt in candidate.CandidateLicenseTypes)
                {
                    if (clt != null)
                    {
                        candidatelicense = clt.LicenseType.Name;
                    }
                }

                string candidateskills = string.Empty;

                foreach (CandidateSkill cs in candidate.CandidateSkills)
                {
                    candidateskills += cs.Skill.Name + ",";
                }

                string candidatepreferredTypes = string.Empty;

                if (candidate.PreferredAll == true)
                    candidatepreferredTypes = "Any, ";

                if (candidate.PreferredContract == true)
                    candidatepreferredTypes += " Contract,";

                if (candidate.PreferredParttime == true)
                {
                    candidatepreferredTypes += "Part Time, ";
                }

                if (candidate.PreferredFulltime == true)
                    candidatepreferredTypes += "Full Time, ";

                if (candidate.PreferredWorkFromHome == true)
                    candidatepreferredTypes += "Work from home";

                string candgeneralShift = string.Empty, candnoonShift = string.Empty, candnightShift = string.Empty;

                if (candidate.GeneralShift == true)
                    candgeneralShift = "GeneralShift";


                if (candidate.NightShift == true)
                    candnightShift = "NightShift";

                string candidateLocation = string.Empty;
                string candidateCityName = string.Empty;
                if (candidate.LocationId != null)
                {
                    if (candidate.GetLocation(candidate.LocationId.Value).CityId.HasValue && candidate.GetLocation(candidate.LocationId.Value).CityId != 0)
                    {
                        candidateCityName = candidate.GetLocation(candidate.LocationId.Value).City.Name + ",";
                    }
                    if (candidate.GetLocation(candidate.LocationId.Value).CountryId != null && candidate.GetLocation(candidate.LocationId.Value).CountryId != 0)
                    {
                        candidateLocation = candidate.GetLocation(candidate.LocationId.Value).Country.Name;
                    }
                }
               
                //caculating age using dob

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
                
                int years = candidate.TotalExperience.HasValue ? (int)candidate.TotalExperience.Value / 31536000 : 0;
                int months = (int)((candidate.TotalExperience.Value - (years * 31536000)) / 2678400);

                int totalExperienceYears = Convert.ToInt32(minExperience) / 31536000;
                int totalMaxexperience = Convert.ToInt32(maxExperience) / 31536000;
                string industry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.GetIndustry(candidate.IndustryId.Value).Name : "";

              /*End Candidate Details*/

                //Send SMS

                if (candidate.ContactNumber != null && candidate.ContactNumber != "")
                {
                    SmsHelper.SendSecondarySms(
                              Constants.SmsSender.SecondaryUserName,
                               Constants.SmsSender.SecondaryPassword,
                               Constants.SmsBody.SMSVacancy
                               .Replace("[ORG_NAME]", job.Organization != null ? job.Organization.Name : "")
                               //.Replace("[VACANCY]", job.Position !=null ? job.Position :"")
                               .Replace("[CONTACT_PERSON]", job.ContactPerson)
                               .Replace("[EMAIL]", job.EmailAddress)
                               .Replace("[MOBILE_NUMBER]", job.MobileNumber)
                               .Replace("[POSITION]", job.Position)
                               .Replace("[BASIC_QUALIFICATION]", basicqualificationName!="" ? basicqualificationName : "Not Mentioned")
                               .Replace("[EXPERIENCE_YEARS]", years.ToString())
                               .Replace("[EXPERIENCE_MONTHS]", months.ToString())
                               .Replace("[LOCATION]", cityName)
                               .Replace("[GENDER]", job.Male == true && job.Female == true ? "Male, Female" : job.Male == true ? "Male" : job.Female == true ? "Female " : "")
                               ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                               candidate.ContactNumber
                                );


                    if (job.MobileNumber != null && job.MobileNumber != "")
                    {
                        SmsHelper.SendSecondarySms(
                                       Constants.SmsSender.SecondaryUserName,
                                      Constants.SmsSender.SecondaryPassword,
                                       Constants.SmsBody.SMSResume
                                       .Replace("[NAME]", candidate.Name)
                                       .Replace("[EMAIL]", candidate.Email)
                                       .Replace("[VACANCY]", job.Position != null ? job.Position : "")
                                       .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                       .Replace("[QUALIFICATION]", basicqualificationName)
                                       .Replace("[FUNCTION]", candidate.FunctionId == null ? "" : candidate.Function.Name)
                                       .Replace("[DESIGNATION]", candidate.Position)
                                       .Replace("[PRESENT_SALARY]", minSalary)
                                       .Replace("[LOCATION]", cityName)
                                       .Replace("[DOB]", candidate.DOB.HasValue ? age.ToString() : String.Empty)
                                       .Replace("[TOTAL_EXPERIENCE]", (candidate.TotalExperience != null ? (candidate.TotalExperience / 31536000).ToString() : ""))
                                       //.Replace("[EXPERIENCE]", (candidate.TotalExperience != null ? (candidate.TotalExperience / 31536000).ToString() : ""))
                                       .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female"),
                                        Constants.SmsSender.SecondaryType,
                                        Constants.SmsSender.Secondarysource,
                                        Constants.SmsSender.Secondarydlr,
                                       job.MobileNumber
                                        );


                        smsSent = true;
                    }
                }

                //}

                //Send mail for RAJ

                StringBuilder jobmatchmaincontent = new StringBuilder();
                if (candidate.Email != null && candidate.Email != "")
                {
                    //jobmatchmaincontent.Append(
                        EmailHelper.SendEmailReply(
                                Constants.EmailSender.EmployerSupport,
                                candidate.Email,
                                Constants.EmailSubject.RAJMatch,
                                Constants.EmailBody.MatchingJobForRAJ
                                .Replace("[DESCRIPTION]", job.Description)
                                .Replace("[NAME]", candidate.Name)
                                .Replace("[POSITION]", job.Position)
                                .Replace("[COMPANY_NAME]", job.Organization != null ? job.Organization.Name : "Not Mentioned")
                                .Replace("[FUNCTION]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "")
                                .Replace("[MIN_EXPERIENCE]", totalExperienceYears.ToString())
                                .Replace("[MAX_EXPERIENCE]", totalMaxexperience.ToString())
                                .Replace("[POSTING_COUNTRY]", countryName)  
                                .Replace("[POSTING_STATE]", stateName)  
                                .Replace("[POSTING_CITY]", cityName)
                                .Replace("[POSTING_AREA]", regionName)
                                .Replace("[BASICQUALIFICATION]", basicqualificationName)
                                .Replace("[POSTGRADUATION]", postgraduationName!= "" ? postgraduationName : "Not Mentioned")
                                .Replace("[DOCTRATE]", doctrateName != "" ? doctrateName : "Not Mentioned")
                                .Replace("[SKILLS]", skills)
                                .Replace("[LANGUAGES]",languages)
                                .Replace("[CONTACT_PERSON]", job.ContactPerson)
                                .Replace("[MOBILE]", job.MobileNumber)
                                .Replace("[LANDLINE]", job.ContactNumber)
                                .Replace("[EMAIL]", job.EmailAddress)
                                .Replace("[WEBSITE]", job.Organization != null ? job.Organization.Website : "Not Mentioned"),
                                job.Organization.Email
                                );
                    
                    if (job.EmailAddress != null || job.EmailAddress != "")
                    {
                        EmailHelper.SendEmailReply(
                                            Constants.EmailSender.EmployerSupport,
                                            job.EmailAddress,
                                            Constants.EmailSubject.CandidateMatch,
                                            Constants.EmailBody.MatchingCandidate
                                            .Replace("[ORG_NAME]", job.Organization.Name != null ? job.Organization.Name : "Not Mentioned")
                                            .Replace("[SPOT_TEXT] ", "")
                                            .Replace("[TEXT_VACANCY]", "Reference to your Vacancy submitted for " + job.Position + "& <b>Job Alert(RAJ)</b> assigned ,find the details of the Candidate Matching your Vacancy.")
                                            .Replace("[TEXT_MATCH]", "")
                                            .Replace("[JOBNAME]", job.Position != null ? job.Position : "Not Mentioned")
                                            .Replace("[CANDIDATENAME]", candidate.Name != null ? candidate.Name : "Not Mentioned")
                                            .Replace("[MOBILE]", candidate.ContactNumber != null ? candidate.ContactNumber : "Not Mentioned")
                                            .Replace("[LANDLINE]", candidate.MobileNumber != null ? candidate.MobileNumber : "Not Mentioned")
                                            .Replace("[EMAIL]", candidate.Email != null ? candidate.Email : "Not Mentioned")
                                            .Replace("[ADDRESS]", candidate.Address != null ? candidate.Address : "Not Mentioned")
                                            .Replace("[BASICQUALIFICATION]", candidatebasic != "" ? candidatebasic : "Not Mentioned")
                                            .Replace("[POSTGRADUATION]", candidatepost != "" ? candidatepost : "Not Mentioned")
                                            .Replace("[DOCTRATE]", candidatedoctorate != "" ? candidatedoctorate : "Not Mentioned")
                                            .Replace("[EXPERIENCE]", candidateexperience != "" ? candidateexperience : "Not Mentioned")
                                            .Replace("[INDUSTRY]", candidateindustry != "" ? candidateindustry : "Not Mentioned")
                                            .Replace("[FUNCTION]", candidate.FunctionId == null ? "" : candidate.Function.Name)
                                            .Replace("[DOB]", candidate.DOB.HasValue ? age.ToString() + " years old" : String.Empty)
                                            .Replace("[SKILLS]", candidateskills != "" ? candidateskills : "Not Mentioned")
                                            .Replace("[ANNUAL_SALARY]", candidateannualsalary != "" ? candidateannualsalary : "Not Mentioned")
                                            .Replace("[LOCATION]", candidateLocation != "" ? candidateLocation : "Not Mentioned")
                                            .Replace("[PRESENT_COMPANY]", candidate.PresentCompany != null ? candidate.PresentCompany : "Not Mentioned")
                                            .Replace("[PREVIOUS_COMPANY]", candidate.PreviousCompany != null ? candidate.PreviousCompany : "Not Mentioned")
                                            .Replace("[LANGUAGE]", candidatelanguages != "" ? candidatelanguages : "Not Mentioned")
                                            .Replace("[LICENSE_TYPES]", candidatelicense != null ? candidatelicense : "Not Mentioned")
                                            .Replace("[PREFERENCES]", candidate.Description != null ? candidate.Description : "Not Mentioned")
                                            .Replace("[PREFERRED_TYPE]", candidatepreferredTypes != "" ? candidatepreferredTypes : "Not Mentioned")
                                            .Replace("[DOWNLOAD_RESUME]", "<a href='" + ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/DownloadResumeFromJobMatches?fileName=" + candidate.ResumeFileName + "'>Download Resume</a>"),
                                            candidate.Email
                                            );

                       
                    }
                    mailSent = true;
                }

                if (smsSent == true || mailSent == true)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                if (smsSent == true || mailSent == true)
                    return true;
                else
                    return false;
            }

        }

        
        [HttpPost]
        public ActionResult CandidateMatch(string JobId, string PageNo)
        {
            IQueryable<Candidate> candidates;
            if (!string.IsNullOrEmpty(JobId))
            {
                Job job = _repository.GetJob(Convert.ToInt32(JobId));

                if (job != null)
                {
                    string basicqualification = string.Empty;
                    string postgraduation = string.Empty;
                    string doctrate = string.Empty;
                    foreach (Dial4Jobz.Models.JobRequiredQualification cq in job.JobRequiredQualifications)
                    {
                        if (cq.Degree.Type == 0)
                        {
                            if (basicqualification == string.Empty)
                                basicqualification += cq.Degree.Id;
                            else
                                basicqualification += "," + cq.Degree.Id;
                        }
                        if (cq.Degree.Type == 1)
                        {
                            if (postgraduation == string.Empty)
                                postgraduation += cq.Degree.Id;
                            else
                                postgraduation += "," + cq.Degree.Id;
                        }
                        if (cq.Degree.Type == 2)
                        {
                            if (doctrate == string.Empty)
                                doctrate += cq.Degree.Id;
                            else
                                doctrate += "," + cq.Degree.Id;
                        }
                    }

                    string function = (job.FunctionId.HasValue && job.FunctionId != 0) ? job.FunctionId.Value.ToString() : string.Empty;

                    string roles = string.Empty;
                    foreach (JobRole jr in job.JobRoles)
                    {
                        if (roles == string.Empty)
                            roles = jr.RoleId.ToString();
                        else
                            roles += "," + jr.RoleId;
                    }

                    string preferredindustries = string.Empty;
                    foreach (JobPreferredIndustry jpi in job.JobPreferredIndustries)
                    {
                        if (preferredindustries == string.Empty)
                            preferredindustries = jpi.IndustryId.ToString();
                        else
                            preferredindustries += "," + jpi.IndustryId;
                    }

                    string male = string.Empty;
                    string female = string.Empty;
                    string gender = string.Empty;

                    if (job.Male == true)
                        // male = "Male";
                        gender = "Male";

                    if (job.Female == true)
                        //  female = "Female";
                        gender = "Female";

                    if (job.Female == true && job.Male == true)
                        gender = "";

                    string minExperience = "0";
                    string maxExperience = "0";

                    if (job.MinExperience != null && job.MaxExperience != null)
                    {
                        minExperience = job.MinExperience.ToString();
                        maxExperience = job.MaxExperience.ToString();
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

                    string skills = string.Empty;
                    foreach (JobSkill js in job.JobSkills)
                    {
                        if (skills == string.Empty)
                            skills = js.Skill.Name;
                        else
                            skills += "," + js.Skill.Name;
                    }

                    string licenseTypes = string.Empty;
                    foreach (JobLicenseType jlt in job.JobLicenseTypes)
                    {
                        if (licenseTypes == string.Empty)
                            licenseTypes = jlt.LicenseType.Name;
                        else
                            licenseTypes += "," + jlt.LicenseType.Name;
                    }

                    string languages = string.Empty;

                    foreach (JobLanguage jl in job.JobLanguages)
                    {
                        if (languages == string.Empty)
                            languages = jl.Language.Name;
                        else
                            languages += "," + jl.Language.Name;
                    }
                    string fulltime = string.Empty, parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;

                    if (job.PreferredFulltime == true)
                        fulltime = "fulltime";

                    if (job.PreferredContract == true)
                        contract = "contract";

                    if (job.PreferredParttime == true)
                        parttime = "parttime";

                    if (job.PreferredWorkFromHome == true)
                        workfromhome = "workfromhome";

                    string generalShift = string.Empty, nightShift = string.Empty;

                    if (job.GeneralShift == true)
                        generalShift = "GeneralShift";

                    if (job.NightShift == true)
                        nightShift = "NightShift";


                    string cityName = string.Empty;
                    string countryName = string.Empty;

                    string preferredlocations = string.Empty;
                    if (job.JobLocations != null)
                    {
                        foreach (JobLocation jl in job.JobLocations)
                        {
                            if (cityName == string.Empty)
                            {
                                if (jl.Location.City != null)
                                {
                                    cityName = jl.Location.City.Name;
                                }
                                else
                                {

                                }
                            }

                            if (countryName == string.Empty)
                            {
                                if (jl.Location.Country != null)
                                {
                                    countryName = jl.Location.Country.Name;
                                }
                                else
                                {
                                }
                            }

                        }
                    }

                    candidates = _repository.GetMatchingCandiatesForJob(basicqualification, postgraduation, doctrate, function, cityName, countryName, roles, preferredindustries, male, female, gender, minExperience, maxExperience, minSalary, maxSalary, skills, languages, fulltime, parttime, contract, workfromhome, generalShift, nightShift,licenseTypes);
                }
                else
                {
                    candidates = _repository.GetCandidates();
                }

            }
            else
            {
                candidates = _repository.GetCandidates();
            }

            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            int take = pageSize;

            var RecordCount = candidates.Count();

            var CandidateResults = candidates.Skip(skip).Take(take);

            ViewData.Add("JobIdView", JobId == "0" ? "" : JobId);
            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(CandidateResults);

        }

        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Send(FormCollection collection, SendMethod sendMethod, string SendToUser, string SendToOrganization, string HfJobId)
        {
            Job job = _repository.GetJob(Convert.ToInt32(HfJobId));

            if (job == null)
                return new FileNotFoundResult();

            string exp = string.Empty;
            if ((!job.MinExperience.HasValue || job.MinExperience == 0) && (!job.MaxExperience.HasValue || job.MaxExperience == 0))
            {

            }
            else if (!job.MinExperience.HasValue || job.MinExperience == 0)
            {
                exp = "Up to" + Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years ";
            }
            else if (!job.MaxExperience.HasValue || job.MaxExperience == 0)
            {
                exp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + "+ Years";
            }
            else
            {
                exp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + " to " +
                Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years";
            }

            StringBuilder joblocation = new StringBuilder();

            foreach (Dial4Jobz.Models.JobLocation jl in job.JobLocations)
            {
                if (jl.Location != null)
                {
                    if (jl.Location.Country != null)
                    {
                        joblocation.Append(jl.Location.Country.Name + ",");
                    }
                    if (jl.Location.State != null)
                    {
                        joblocation.Append(jl.Location.State.Name + ",");
                    }
                    if (jl.Location.City != null)
                    {
                        joblocation.Append(jl.Location.City.Name + ",");
                    }
                    if (jl.Location.Region != null)
                    {
                        joblocation.Append(jl.Location.Region.Name + ",");
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
                    jobbasicqualification += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                }
                if (cq.Degree.Type == 1)
                {
                    jobpostgraduation += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                }
                if (cq.Degree.Type == 2)
                {
                    jobdoctrate += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                }
            }

            string jobskills = string.Empty;

            foreach (JobSkill cs in job.JobSkills)
            {
                jobskills += cs.Skill.Name + ",";
            }
            

            StringBuilder candidatematchmaincontent = new StringBuilder();
            foreach (string key in collection.AllKeys)
            {
                //process each candidate that is selected.
                if (key.Contains("Candidate"))
                {
                    if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                    {
                        int candidateId = Convert.ToInt32(key.Replace("Candidate", string.Empty));

                        bool isAlertSent = false;
                        bool canAlertSent = false;
                        bool checkalreadysentjob = false;
                        
                        canAlertSent = _vasRepository.CheckForAlertsRemainedRAJ((int)candidateId);
                        checkalreadysentjob = _vasRepository.AlreadyALertsentorNot((int)candidateId, job.Id);

                        Candidate candidate = _repository.GetCandidate(candidateId);

                        if (candidate == null)
                            return new FileNotFoundResult();

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
                        foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 0))
                        {

                            basicqualification += cq.Degree.Name + ",";
                        }

                        foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 1))
                        {

                            postgraduation += cq.Degree.Name + ",";
                        }

                        foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 2))
                        {

                            doctrate += cq.Degree.Name + ",";
                        }

                        string skills = string.Empty;

                        foreach (CandidateSkill cs in candidate.CandidateSkills)
                        {
                            skills += cs.Skill.Name + ",";
                        }

                        string preferredTypes = string.Empty;

                        if (candidate.PreferredAll == true)
                            preferredTypes = "Any, ";

                        if (candidate.PreferredContract == true)
                            preferredTypes += " Contract,";

                        if (candidate.PreferredParttime == true)
                        {
                            preferredTypes += "Part Time, ";
                        }

                        if (candidate.PreferredFulltime == true)
                            preferredTypes += "Full Time, ";

                        if (candidate.PreferredWorkFromHome == true)
                            preferredTypes += "Work from home";

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

                        if (sendMethod == SendMethod.SMS || sendMethod == SendMethod.Both)
                        {
                            if (SendToOrganization == "true")
                            {
                                if (job.CommunicateViaSMS != null && job.CommunicateViaSMS == true)
                                {
                                    SmsHelper.SendSecondarySms(
                                    Constants.SmsSender.SecondaryUserName,
                                    Constants.SmsSender.SecondaryPassword,
                                    Constants.SmsBody.SMSResume
                                    .Replace("[NAME]", candidate.Name)
                                    .Replace("[EMAIL]", candidate.Email)
                                    .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                    .Replace("[QUALIFICATION]", basicqualification)
                                    .Replace("[FUNCTION]", candidate.FunctionId == null || job.FunctionId == 0 ? "" : candidate.Function.Name)
                                    .Replace("[DESIGNATION]", candidate.Position)
                                    .Replace("[PRESENT_SALARY]", annualsalary)
                                    .Replace("[LOCATION]", location)
                                    .Replace("[DOB]", candidate.DOB.HasValue ? candidate.DOB.Value.ToShortDateString() : String.Empty)
                                    .Replace("[TOTAL_EXPERIENCE]", experience)
                                    .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female"),
                                    Constants.SmsSender.SecondaryType,
                                    Constants.SmsSender.Secondarysource,
                                    Constants.SmsSender.Secondarydlr,
                                    job.MobileNumber
                                     );
                                }
                                smsSent = true;
                            }

                            if (SendToUser == "true")
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
                               .Replace("[BASIC_QUALIFICATION]", jobbasicqualification)
                               .Replace("[EXPERIENCE]", exp)
                               .Replace("[LOCATION]", joblocation.ToString())
                               .Replace("[GENDER]", job.Male == true && job.Female == true ? "Male, Female" : job.Male == true ? "Male" : job.Female == true ? "Female " : "")
                               ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                              candidate.ContactNumber
                                );
                               smsSent = true;
                            }
                        }

                        if (sendMethod == SendMethod.Email || sendMethod == SendMethod.Both)
                        {
                            if (SendToOrganization == "true")
                            {
                                
                                candidatematchmaincontent.Append(
                                Constants.EmailBody.MatchingCandidateMain
                                .Replace("[CANDIDATENAME]", candidate.Name)
                                .Replace("[MOBILE]", candidate.ContactNumber)
                                .Replace("[LANDLINE]", candidate.MobileNumber)
                                .Replace("[EMAIL]", candidate.Email)
                                .Replace("[ADDRESS]", candidate.Address)
                                .Replace("[BASICQUALIFICATION]", basicqualification)
                                .Replace("[POSTGRADUATION]", postgraduation)
                                .Replace("[DOCTRATE]", doctrate)
                                .Replace("[EXPERIENCE]", experience)

                                .Replace("[INDUSTRY]", industry)
                                .Replace("[FUNCTION]", candidate.FunctionId == null || job.FunctionId == 0 ? "" : candidate.Function.Name)
                                .Replace("[SKILLS]", skills)
                                .Replace("[ANNUAL_SALARY]", annualsalary)
                                .Replace("[LOCATION]", location)
                                .Replace("[PRESENT_COMPANY]", candidate.PresentCompany)
                                .Replace("[PREVIOUS_COMPANY]", candidate.PreviousCompany)
                                .Replace("[LANGUAGE]", languages)
                                .Replace("[PREFERENCES]", "")
                                .Replace("[PREFERRED_TYPE]", preferredTypes)
                                );

                            }
                            mailSent = true;


                            if (SendToUser == "true")
                            {
                                if (candidate.Email != null)
                                {
                                    EmailHelper.SendEmail(
                                    Constants.EmailSender.EmployerSupport,
                                    candidate.Email,
                                    Constants.EmailSubject.JobMatch,
                                    Constants.EmailBody.MatchingJobHeader
                                    .Replace("[NAME]", candidate.Name) +
                                    Constants.EmailBody.MatchingJobMain
                                    .Replace("[NAME]", candidate.Name)
                                    .Replace("[DESCRIPTION]", job.Description)
                                    .Replace("[POSITION]", job.Position)
                                    .Replace("[COMPANY_NAME]", job.Organization != null ? job.Organization.Name : "")
                                    .Replace("[INDUSTRY_TYPE]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "")
                                    .Replace("[EXPERIENCE]", exp)
                                    .Replace("[POSTING_LOCATION]", joblocation.ToString())
                                    .Replace("[BASICQUALIFICATION]", jobbasicqualification)
                                    .Replace("[POSTGRADUATION]", jobpostgraduation)
                                    .Replace("[DOCTRATE]", jobdoctrate)
                                    .Replace("[SKILLS]", jobskills)
                                    .Replace("[CONTACT_PERSON]", job.ContactPerson)
                                    .Replace("[MOBILE]", job.MobileNumber)
                                    .Replace("[LANDLINE]", job.ContactNumber)
                                    .Replace("[EMAIL]", job.EmailAddress)
                                    .Replace("[WEBSITE]", string.IsNullOrEmpty(job.Organization.Website) ? "" : job.Organization.Website)
                                    + Constants.EmailBody.MatchingJobFooter
                                    );
                                }
                                mailSent = true;
                            }

                            /*********Reduce Count from Send Sms and Email Manual Send**********/

                            if (canAlertSent == true && checkalreadysentjob == true)
                            {
                                isAlertSent = SendSMSandMail((int)candidateId, job.Id);
                                var planId = _vasRepository.RAJPlanActivatedDetails((int)candidateId);

                                if (isAlertSent == true)
                                {
                                    _vasRepository.UpdateCandAlertsDoneCount((int)candidateId);
                                    _vasRepository.logCandlerts((int)candidateId, (int)planId, job.Id, smsSent, mailSent);
                                }
                            }
                        }
                    }
                }

            }

            if (SendToOrganization == "true")
            {
                if (job.EmailAddress != null)
                {
                    EmailHelper.SendEmail(
                                    Constants.EmailSender.EmployerSupport,
                                   job.EmailAddress,
                                    Constants.EmailSubject.CandidateMatch,
                                    Constants.EmailBody.MatchingCandidateHeader
                                    .Replace("[ORG_NAME]", job.Organization != null ? job.Organization.Name : "")
                                    .Replace("[JOBNAME]", job.DisplayPosition)
                                    + candidatematchmaincontent + Constants.EmailBody.MatchingCandidateFooter

                                    );

                }
            }


            

            string msg = sendMethod == SendMethod.SMS ? "SMS" : sendMethod == SendMethod.Email ? "Email" : "SMS / Email";
            string sentTo = (SendToUser == "true" && SendToOrganization == "true") ? "Candidate and Organizations" : SendToUser == "true" ? "Candidate" : SendToOrganization == "true" ? "Organizations" : "";
            return Json(new JsonActionResult { Success = true, Message = "Successfully sent the " + msg + " to " + sentTo });
        }

    }
}
