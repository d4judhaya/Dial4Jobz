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
    public class JobMatchesCandidateController : BaseController
    {
        //
        // GET: /JobMatchesCandidate/

        Repository _repository = new Repository();
        //UserRepository _userRepository = new UserRepository();
        VasRepository _vasRepository = new VasRepository();
        bool smsSent;
        bool mailSent;



       
        public ActionResult JobMatch(string Id)
        {
            IQueryable<Job> Jobs;
            if (!string.IsNullOrEmpty(Id))
            {
                Candidate candidate = _userRepository.GetCandidateById(Convert.ToInt32(string.IsNullOrEmpty(Id) ? "0" : Id));

                ViewData["CandidateId"] = candidate.Id;
                if (candidate != null)
                {
                    string basicqualification = string.Empty;
                    string postgraduation = string.Empty;
                    string doctorate = string.Empty;

                    string basicSpec = string.Empty;
                    string postSpec = string.Empty;
                    string doctSpec = string.Empty;
                    foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications)
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
                            if (doctorate == string.Empty)
                                doctorate += cq.Degree.Id;
                            else
                                doctorate += "," + cq.Degree.Id;
                        }
                    }


                    string minExperience = "0";
                    string maxExperience = "0";

                    if (candidate.TotalExperience != null && candidate.TotalExperience != 0)
                    {
                        Int64 year = (candidate.TotalExperience.Value / 31104000);
                        minExperience = (year * 365 * 24 * 60 * 60).ToString();
                        maxExperience = minExperience;
                        //maxExperience = ((year + 1) * 365 * 24 * 60 * 60).ToString();
                    }


                    string minSalary = string.Empty;
                    string maxSalary = string.Empty;
                    if (candidate.AnnualSalary != null)
                    {
                        minSalary = candidate.AnnualSalary.ToString();
                    }
                    if (candidate.AnnualSalary != null)
                    {
                        maxSalary = candidate.AnnualSalary.ToString();
                    }


                    string skills = string.Empty;
                    foreach (CandidateSkill cs in candidate.CandidateSkills)
                    {
                        if (skills == string.Empty)
                            skills = cs.Skill.Name;
                        else
                            skills += "," + cs.Skill.Name;
                    }

                    string languages = string.Empty;

                    foreach (CandidateLanguage cl in candidate.CandidateLanguages)
                    {
                        if (languages == string.Empty)
                            languages = cl.Language.Name;
                        else
                            languages += "," + cl.Language.Name;
                    }

                    string licenseTypes = string.Empty;

                    foreach (CandidateLicenseType cl in candidate.CandidateLicenseTypes)
                    {
                        if (licenseTypes == string.Empty)
                            licenseTypes = cl.LicenseType.Name;
                        else
                            licenseTypes += "," + cl.LicenseType.Name;
                    }

                    string position = string.Empty;
                    string function = string.Empty;

                    if (!string.IsNullOrEmpty(candidate.Position))
                        position = candidate.Position;

                    string industry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.IndustryId.Value.ToString() : string.Empty;


                    string cityName = string.Empty;
                    string countryName = string.Empty;
                    string regionName = string.Empty;


                    if (candidate.CandidatePreferredLocations != null)
                    {
                        foreach (CandidatePreferredLocation cpf in candidate.CandidatePreferredLocations)
                        {
                            if (cityName == string.Empty)
                            {
                                if (cpf.Location.City != null)
                                {
                                    cityName = cpf.Location.City.Name;
                                }
                            }
                            else
                            {
                                if (cpf.Location.City != null)
                                {
                                    cityName += "," + cpf.Location.City.Name;
                                }
                            }

                            if (countryName == string.Empty)
                            {
                                if (cpf.Location.Country != null)
                                {
                                    countryName = cpf.Location.Country.Name;
                                }
                            }
                            else
                            {
                                if (cpf.Location.Country != null)
                                {
                                    countryName += "," + cpf.Location.Country.Name;
                                }
                            }

                        }
                    }

                    string roles = string.Empty;
                    if (candidate.CandidatePreferredFunctions != null)
                    {
                        foreach (CandidatePreferredFunction cp in candidate.CandidatePreferredFunctions)
                        {
                            if (function == string.Empty)
                            {
                                function = cp.FunctionId.ToString();
                            }

                            else
                            {
                                function += "," + cp.Function.Id;
                            }


                        }
                    }

                    if (roles == null || roles == string.Empty)
                    {
                        if (candidate.CandidatePreferredRoles != null)
                        {
                            foreach (CandidatePreferredRole cr in candidate.CandidatePreferredRoles)
                            {
                                if (roles == string.Empty)
                                    roles = cr.Role.Name;

                                else
                                    roles += "," + cr.Role.Name;
                            }
                        }
                    }


                    string gender = string.Empty;
                    if (candidate.Gender == 0)
                        gender = "Male";
                    if (candidate.Gender == 1)
                        gender = "Female";

                    string preferredType = string.Empty, fulltime = string.Empty, parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;

                    if (candidate.PreferredFulltime == true)
                        preferredType = "fulltime";

                    if (candidate.PreferredContract == true)
                        preferredType = "contract";

                    if (candidate.PreferredParttime == true)
                        preferredType = "parttime";

                    if (candidate.PreferredWorkFromHome == true)
                        preferredType = "workfromhome";

                    string generalShift = string.Empty, noonShift = string.Empty, nightShift = string.Empty;

                    if (candidate.GeneralShift == true)
                        generalShift = "generalShift";

                    if (candidate.NightShift == true)
                        nightShift = "NightShift";


                    Jobs = _repository.GetMatchingJobsForCandidate(basicqualification, postgraduation, doctorate, basicSpec, postSpec, doctSpec, position, skills, languages, minExperience, maxExperience, minSalary, maxSalary, industry, function, roles, cityName, countryName, preferredType, parttime, contract, workfromhome, gender, generalShift, nightShift, licenseTypes);

                    //udhaya orderby activated jobs
                    DateTime currentdate = Constants.CurrentTime().Date;
                    List<int> lstOrganizationId = null;
                    var orderOrganization = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderId == od.OrderMaster.OrderId && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.OrganizationId.Value);

                    lstOrganizationId = orderOrganization.ToList();

                    Func<IQueryable<Job>, IOrderedQueryable<Job>> orderingFunc = query =>
                    {
                        if (orderOrganization.Count() > 0)
                            return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => rslt.CreatedDate);
                        else
                            return query.OrderByDescending(rslt => rslt.CreatedDate);
                    };

                    Jobs = orderingFunc(Jobs);


                    // -> Note to Developers: This is to send alert to Employers. When the first time candidate details are update only this block should run

                    List<int> lstJobIds = null;
                    if (Jobs.Count() > 0)
                    {
                        var jobIds = from job in Jobs
                                     orderby job.CreatedDate descending
                                     select job.Id;
                        lstJobIds = jobIds.ToList();

                        IEnumerable<PostedJobAlert> EmployerIds = _vasRepository.GetMatchingOrganizationtoAlert(lstJobIds);

                        bool isAlertSent = false;

                        foreach (var emp in EmployerIds)
                        {
                            int employerId;
                            if (emp.OrganizationId != null)
                            {
                                employerId = (int)emp.OrganizationId;
                            }
                            else
                            {
                                employerId = (int)emp.ConsultantId;
                            }

                            int candidateId = candidate.Id;
                            bool canAlertSent = _vasRepository.CheckForAlertsRemained((int)employerId, (int)emp.JobId);
                            bool VacancyAlertSent = _vasRepository.CheckRATAlertByVacancy((int)employerId, (int)emp.JobId);
                            bool alreadysentornot = _vasRepository.RatAlertsentorNot((int)employerId, (int)candidateId, (int)emp.JobId);

                            if (canAlertSent == true && alreadysentornot == true && VacancyAlertSent == true)
                            {
                                isAlertSent = SendSMSandMail((int)employerId, Id, (int)emp.JobId);

                                if (isAlertSent == true)
                                {
                                    _vasRepository.UpdateAlertsDoneCount((int)employerId, (int)emp.JobId);
                                    _vasRepository.UpdateAlertsDoneRAT((int)employerId, (int)emp.JobId);
                                    _vasRepository.logAlerts((int)employerId, (int)emp.JobId, candidateId, smsSent, mailSent);

                                }
                            }

                        }

                    }

                }
                else
                {
                    Jobs = _repository.GetJobs();
                }

            }
            else
            {
                Jobs = _repository.GetJobs();
            }

            int page = 1;

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = Jobs.Count();

            var JobResults = Jobs.Skip(skip).Take(take);

            ViewData.Add("CandidateIdView", Id == "0" ? "" : Id);
            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(JobResults);
        }

        
        public bool SendSMSandMail(int empId, string HfCandidateId, int jobId)
        {
            try
            {

                Candidate candidate = _repository.GetCandidate(Convert.ToInt32(HfCandidateId));
                Organization organization = _repository.GetOrganization(Convert.ToInt32(empId));
                Consultante consultant = _repository.GetConsulant(Convert.ToInt32(empId));
                Job job = _repository.GetJob(Convert.ToInt32(jobId));

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

                //Send SMS


                string empmobileNo = string.Empty;
                string organizationEmail = string.Empty;

                if (consultant != null)
                {
                    empmobileNo = consultant.MobileNumber;
                    organizationEmail = consultant.Email;
                }

                else
                {
                    empmobileNo = _vasRepository.GetMobileNo(empId);
                    organizationEmail = _vasRepository.GetEmailAddress(empId);
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


                /****************Job Details to send Email******************/

                string jobExp = string.Empty;

                if ((!job.MinExperience.HasValue || job.MinExperience == 0) && (!job.MaxExperience.HasValue || job.MaxExperience == 0))
                {

                }
                else if (!job.MinExperience.HasValue || job.MinExperience == 0)
                {
                    jobExp = "Up to" + Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years ";
                }
                else if (!job.MaxExperience.HasValue || job.MaxExperience == 0)
                {
                    jobExp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + "+ Years";
                }
                else
                {
                    jobExp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + " to " +
                    Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years";
                }

                StringBuilder joblocation = new StringBuilder();

                StringBuilder cityName = new StringBuilder();

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
                            cityName.Append(jl.Location.City.Name + ",");
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

                string jobskills = string.Empty;

                foreach (JobSkill cs in job.JobSkills)
                {
                    jobskills += cs.Skill.Name + ",";
                }

                string jobLicenseType = string.Empty;

                foreach (JobLicenseType jl in job.JobLicenseTypes)
                {
                    jobLicenseType = jl.LicenseType.Name;
                }

                /***************End Job Details*******************/


                if (empmobileNo != null && empmobileNo != "")
                {
                    SmsHelper.SendSecondarySms(
                                   Constants.SmsSender.SecondaryUserName,
                                  Constants.SmsSender.SecondaryPassword,
                                   Constants.SmsBody.SMSResume
                                   .Replace("[VACANCY]", job.Position)
                                   .Replace("[NAME]", candidate.Name)
                                   .Replace("[EMAIL]", candidate.Email)
                                   .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                   .Replace("[QUALIFICATION]", candidatebasic)
                                   .Replace("[FUNCTION]", candidate.FunctionId == null ? "" : candidate.Function.Name)
                                   .Replace("[DESIGNATION]", candidate.Position)
                                   .Replace("[PRESENT_SALARY]", candidateannualsalary)
                                   .Replace("[LOCATION]", candidateLocation + "," + candidateCityName)
                                   .Replace("[DOB]", candidate.DOB.HasValue ? age.ToString() + "years old" : String.Empty)
                                   .Replace("[TOTAL_EXPERIENCE]", candidateexperience)
                                   .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female"),
                                    Constants.SmsSender.SecondaryType,
                                    Constants.SmsSender.Secondarysource,
                                    Constants.SmsSender.Secondarydlr,
                                    empmobileNo.ToString()
                                    );
                }

                if (candidate.ContactNumber != null && candidate.ContactNumber != "")
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
                               .Replace("[EXPERIENCE]", jobExp)
                               .Replace("[LOCATION]", joblocation != null ? joblocation.ToString() : "Not Mentioned")
                               .Replace("[EXP]", jobExp)
                               .Replace("[GENDER]", job.Male == true && job.Female == true ? "Male, Female" : job.Male == true ? "Male" : job.Female == true ? "Female " : "")
                               ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                               candidate.ContactNumber
                                );


                }

                smsSent = true;

                //Send mail


                if (organizationEmail != null && organizationEmail != "")
                {
                    EmailHelper.SendEmailReply(
                                        Constants.EmailSender.EmployerSupport,
                                         organization.Email,
                                         "Job | Response For Your Resume Alert(RAT)",
                                        Constants.EmailBody.MatchingCandidate
                                        .Replace("[ORG_NAME]", organization.Name != null ? organization.Name : "Not Mentioned")
                                        .Replace("[SPOT_TEXT] ", "")
                                        .Replace("[TEXT_VACANCY]", "Reference to your Vacancy submitted for " + job.Position + "& <b>Resume Alert(RAT)</b> assigned ,find the details of the Candidate Matching your Vacancy .The Vacancy is sent to this candidate also.")
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

                    mailSent = true;


                    StringBuilder jobmatchmaincontent = new StringBuilder();
                    if (candidate.Email != null && candidate.Email != "")
                    {
                        jobmatchmaincontent.Append(
                                    Constants.EmailBody.MatchingJobMain
                                    .Replace("[DESCRIPTION]", job.Description != null ? job.Description : "Not Mentioned")
                                    .Replace("[POSITION]", job.Position != null ? job.Position : "Not Mentioned")
                                    .Replace("[COMPANY_NAME]", job.Organization != null ? job.Organization.Name : "Not Mentioned")
                                    .Replace("[INDUSTRY_TYPE]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "Not Mentioned")
                                    .Replace("[EXPERIENCE]", jobExp != "" ? jobExp : "Not Mentioned")
                                    .Replace("[POSTING_LOCATION]", joblocation != null ? joblocation.ToString() : "Not Mentioned")
                                    .Replace("[BASICQUALIFICATION]", jobbasicqualification != "" ? jobbasicqualification : "Not Mentioned")
                                    .Replace("[POSTGRADUATION]", jobpostgraduation != "" ? jobpostgraduation : "Not Mentioned")
                                    .Replace("[DOCTRATE]", jobdoctrate != "" ? jobdoctrate : "Not Mentioned")
                                    .Replace("[LICENSE_TYPE]", jobLicenseType != null ? jobLicenseType : "Not Mentioned")
                                    .Replace("[SKILLS]", jobskills != "" ? jobskills : "Not Mentioned")
                                    .Replace("[CONTACT_PERSON]", job.ContactPerson != null ? job.ContactPerson : "Not Mentioned")
                                    .Replace("[MOBILE]", job.MobileNumber != null ? job.MobileNumber : "Not Mentioned")
                                    .Replace("[LANDLINE]", job.ContactNumber != null ? job.ContactNumber : "Not Mentioned")
                                    .Replace("[EMAIL]", job.EmailAddress != null ? job.EmailAddress : "Not Mentioned")
                                    .Replace("[WEBSITE]", job.Organization != null ? job.Organization.Website : "Not Mentioned")
                                    );
                        EmailHelper.SendEmailReply(
                                    Constants.EmailSender.EmployerSupport,
                                    candidate.Email,
                                    Constants.EmailSubject.JobMatch,
                                    Constants.EmailBody.MatchingJobHeader,
                                    job.Organization.Email
                                    .Replace("[NAME]", candidate.Name)
                                    + jobmatchmaincontent + Constants.EmailBody.MatchingJobFooter
                                    );


                        mailSent = true;
                    }
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

        /**************When click second page in view it will hit the (Post) Method. 
         * So whatever change happen in (GET) method, change
         into (POST) Method also******************/

        [HttpPost]
        public ActionResult JobMatch(string CandidateId, string PageNo)
        {
            IQueryable<Job> Jobs;
            if (!string.IsNullOrEmpty(CandidateId))
            {
                Candidate candidate = _userRepository.GetCandidateById(Convert.ToInt32(string.IsNullOrEmpty(CandidateId) ? "0" : CandidateId));

                if (candidate != null)
                {
                    string basicqualification = string.Empty;
                    string postgraduation = string.Empty;
                    string doctorate = string.Empty;

                    string basicSpec = string.Empty;
                    string postSpec = string.Empty;
                    string doctSpec = string.Empty;
                    foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications)
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
                            if (doctorate == string.Empty)
                                doctorate += cq.Degree.Id;
                            else
                                doctorate += "," + cq.Degree.Id;
                        }
                    }


                    string minExperience = "0";
                    string maxExperience = "0";

                    if (candidate.TotalExperience != null && candidate.TotalExperience != 0)
                    {
                        Int64 year = (candidate.TotalExperience.Value / 31104000);
                        minExperience = (year * 365 * 24 * 60 * 60).ToString();
                        maxExperience = minExperience;
                        //maxExperience = ((year + 1) * 365 * 24 * 60 * 60).ToString();
                    }


                    string minSalary = string.Empty;
                    string maxSalary = string.Empty;
                    if (candidate.AnnualSalary != null)
                    {
                        minSalary = candidate.AnnualSalary.ToString();
                    }
                    if (candidate.AnnualSalary != null)
                    {
                        maxSalary = candidate.AnnualSalary.ToString();
                    }


                    string skills = string.Empty;
                    foreach (CandidateSkill cs in candidate.CandidateSkills)
                    {
                        if (skills == string.Empty)
                            skills = cs.Skill.Name;
                        else
                            skills += "," + cs.Skill.Name;
                    }

                    string languages = string.Empty;

                    foreach (CandidateLanguage cl in candidate.CandidateLanguages)
                    {
                        if (languages == string.Empty)
                            languages = cl.Language.Name;
                        else
                            languages += "," + cl.Language.Name;
                    }

                    string licenseTypes = string.Empty;

                    foreach (CandidateLicenseType cl in candidate.CandidateLicenseTypes)
                    {
                        if (licenseTypes == string.Empty)
                            licenseTypes = cl.LicenseType.Name;
                        else
                            licenseTypes += "," + cl.LicenseType.Name;
                    }

                    string position = string.Empty;
                    string function = string.Empty;

                    if (!string.IsNullOrEmpty(candidate.Position))
                        position = candidate.Position;

                    string industry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.IndustryId.Value.ToString() : string.Empty;


                    string cityName = string.Empty;
                    string countryName = string.Empty;
                    string regionName = string.Empty;


                    if (candidate.CandidatePreferredLocations != null)
                    {
                        foreach (CandidatePreferredLocation cpf in candidate.CandidatePreferredLocations)
                        {
                            if (cityName == string.Empty)
                            {
                                if (cpf.Location.City != null)
                                {
                                    cityName = cpf.Location.City.Name;
                                }
                            }
                            else
                            {
                                if (cpf.Location.City != null)
                                {
                                    cityName += "," + cpf.Location.City.Name;
                                }
                            }

                            if (countryName == string.Empty)
                            {
                                if (cpf.Location.Country != null)
                                {
                                    countryName = cpf.Location.Country.Name;
                                }
                            }
                            else
                            {
                                if (cpf.Location.Country != null)
                                {
                                    countryName += "," + cpf.Location.Country.Name;
                                }
                            }

                        }
                    }

                    string roles = string.Empty;
                    if (candidate.CandidatePreferredFunctions != null)
                    {
                        foreach (CandidatePreferredFunction cp in candidate.CandidatePreferredFunctions)
                        {
                            if (function == string.Empty)
                            {
                                function = cp.FunctionId.ToString();
                            }

                            else
                            {
                                function += "," + cp.Function.Id;
                            }


                        }
                    }

                    if (roles == null || roles == string.Empty)
                    {
                        if (candidate.CandidatePreferredRoles != null)
                        {
                            foreach (CandidatePreferredRole cr in candidate.CandidatePreferredRoles)
                            {
                                if (roles == string.Empty)
                                    roles = cr.Role.Name;

                                else
                                    roles += "," + cr.Role.Name;
                            }
                        }
                    }


                    string gender = string.Empty;
                    if (candidate.Gender == 0)
                        gender = "Male";
                    if (candidate.Gender == 1)
                        gender = "Female";

                    string preferredType = string.Empty, fulltime = string.Empty, parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;

                    if (candidate.PreferredFulltime == true)
                        preferredType = "fulltime";

                    if (candidate.PreferredContract == true)
                        preferredType = "contract";

                    if (candidate.PreferredParttime == true)
                        preferredType = "parttime";

                    if (candidate.PreferredWorkFromHome == true)
                        preferredType = "workfromhome";

                    string generalShift = string.Empty, noonShift = string.Empty, nightShift = string.Empty;

                    if (candidate.GeneralShift == true)
                        generalShift = "generalShift";

                    if (candidate.NightShift == true)
                        nightShift = "NightShift";


                    Jobs = _repository.GetMatchingJobsForCandidate(basicqualification, postgraduation, doctorate, basicSpec, postSpec, doctSpec, position, skills, languages, minExperience, maxExperience, minSalary, maxSalary, industry, function, roles, cityName, countryName, preferredType, parttime, contract, workfromhome, gender, generalShift, nightShift, licenseTypes);

                    //udhaya orderby activated jobs
                    DateTime currentdate = Constants.CurrentTime().Date;
                    List<int> lstOrganizationId = null;
                    var orderOrganization = _vasRepository.GetOrderDetails().Where(od => od.OrderMaster.OrganizationId != null && od.OrderId == od.OrderMaster.OrderId && od.OrderMaster.PaymentStatus == true && od.ValidityTill.Value >= currentdate).OrderByDescending(od => od.ValidityTill).Select(ord => ord.OrderMaster.OrganizationId.Value);

                    lstOrganizationId = orderOrganization.ToList();

                    Func<IQueryable<Job>, IOrderedQueryable<Job>> orderingFunc = query =>
                    {
                        if (orderOrganization.Count() > 0)
                            return query.OrderByDescending(rslt => lstOrganizationId.Contains(rslt.OrganizationId)).ThenByDescending(rslt => rslt.CreatedDate);
                        else
                            return query.OrderByDescending(rslt => rslt.CreatedDate);
                    };

                    Jobs = orderingFunc(Jobs);


                    // -> Note to Developers: This is to send alert to Employers. When the first time candidate details are update only this block should run

                    List<int> lstJobIds = null;
                    if (Jobs.Count() > 0)
                    {
                        var jobIds = from job in Jobs
                                     orderby job.CreatedDate descending
                                     select job.Id;
                        lstJobIds = jobIds.ToList();

                        IEnumerable<PostedJobAlert> EmployerIds = _vasRepository.GetMatchingOrganizationtoAlert(lstJobIds);

                        bool isAlertSent = false;

                        foreach (var emp in EmployerIds)
                        {
                            int employerId;
                            if (emp.OrganizationId != null)
                            {
                                employerId = (int)emp.OrganizationId;
                            }
                            else
                            {
                                employerId = (int)emp.ConsultantId;
                            }

                            int candidateId = candidate.Id;
                            bool canAlertSent = _vasRepository.CheckForAlertsRemained((int)employerId, (int)emp.JobId);
                            bool VacancyAlertSent = _vasRepository.CheckRATAlertByVacancy((int)employerId, (int)emp.JobId);
                            bool alreadysentornot = _vasRepository.RatAlertsentorNot((int)employerId, (int)candidateId, (int)emp.JobId);

                            if (canAlertSent == true && alreadysentornot == true && VacancyAlertSent == true)
                            {
                                isAlertSent = SendSMSandMail((int)employerId, candidate.Id.ToString(), (int)emp.JobId);

                                if (isAlertSent == true)
                                {
                                    _vasRepository.UpdateAlertsDoneCount((int)employerId, (int)emp.JobId);
                                    _vasRepository.UpdateAlertsDoneRAT((int)employerId, (int)emp.JobId);
                                    _vasRepository.logAlerts((int)employerId, (int)emp.JobId, candidateId, smsSent, mailSent);

                                }
                            }

                        }

                    }

                }
                else
                {
                    Jobs = _repository.GetJobs();
                }

            }
            else
            {
                Jobs = _repository.GetJobs();
            }
                      

            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = Jobs.Count();

            var JobResults = Jobs.Skip(skip).Take(take);

            ViewData.Add("CandidateIdView", CandidateId == "0" ? "" : CandidateId);
            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(JobResults);
        }

        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Send(FormCollection collection, SendMethod sendMethod, string Candidate, string Organization, string HfCandidateId)
        {
            Candidate candidate = _repository.GetCandidate(Convert.ToInt32(HfCandidateId));
                      

            string msg = "";

            string candidateexperience = (candidate.TotalExperience.HasValue && candidate.TotalExperience != 0) ? (candidate.TotalExperience.Value / 31104000).ToString() + " Years " + (((candidate.TotalExperience.Value - (candidate.TotalExperience.Value / 31104000) * 31536000)) / 2678400) + " Months" : "";
            string candidateannualsalary = (candidate.AnnualSalary.HasValue && candidate.AnnualSalary != 0) ? Convert.ToInt32(candidate.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN")) : "";
            string candidateindustry = (candidate.IndustryId.HasValue && candidate.IndustryId != 0) ? candidate.GetIndustry(candidate.IndustryId.Value).Name : "";
            string candidatelanguages = string.Empty;

            foreach (Dial4Jobz.Models.CandidateLanguage cla in candidate.CandidateLanguages)
            {
                candidatelanguages += cla.Language.Name + ",";
            }

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

            StringBuilder jobmatchmaincontent = new StringBuilder();
            foreach (string key in collection.AllKeys)
            {
                //process each candidate that is selected.
                if (key.Contains("Job"))
                {
                    if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                    {
                        int jobId = Convert.ToInt32(key.Replace("Job", string.Empty));
                        Job job = _repository.GetJob(jobId);

                        bool planActivated = false;
                        bool RatActivated = false;
                        bool HorsActivated = false;

                        if (job.OrganizationId != -1)
                        {
                            planActivated = _vasRepository.PlanSubscribed(job.OrganizationId);
                            RatActivated = _vasRepository.GetRatSubscribed(job.OrganizationId);
                            HorsActivated = _vasRepository.GetHORSSubscribed(job.OrganizationId);
                        }
                        else if (job.Consultante != null)
                        {
                            planActivated = _vasRepository.PlanSubscribed(Convert.ToInt32(job.ConsultantId));
                            RatActivated = _vasRepository.GetRatSubscribed(Convert.ToInt32(job.ConsultantId));
                            HorsActivated = _vasRepository.GetHORSSubscribed(Convert.ToInt32(job.ConsultantId));
                        }

                        if (job == null)
                            return new FileNotFoundResult();

                        string jobexp = string.Empty;
                        //string maxExp = string.Empty;
                        if ((!job.MinExperience.HasValue || job.MinExperience == 0) && (!job.MaxExperience.HasValue || job.MaxExperience == 0))
                        {

                        }
                        else if (!job.MinExperience.HasValue || job.MinExperience == 0)
                        {
                            jobexp = "Up to" + Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years ";
                        }
                        else if (!job.MaxExperience.HasValue || job.MaxExperience == 0)
                        {
                            jobexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + "+ Years";
                        }
                        else
                        {
                            jobexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + " to " +
                            Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years";
                        }


                        StringBuilder joblocation = new StringBuilder();

                        StringBuilder jobcityName = new StringBuilder();

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
                                    jobcityName.Append(jl.Location.City.Name + ",");
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

                        string candidatelicense = string.Empty;
                        foreach (Dial4Jobz.Models.CandidateLicenseType clt in candidate.CandidateLicenseTypes)
                        {
                            if (clt != null)
                            {
                                candidatelicense = clt.LicenseType.Name;
                            }
                        }

                        string jobskills = string.Empty;

                        foreach (JobSkill cs in job.JobSkills)
                        {
                            jobskills += cs.Skill.Name + ",";
                        }

                        if (sendMethod == SendMethod.SMS || sendMethod == SendMethod.Both)
                        {
                            if (Organization == "true")
                            {

                                if (job.MobileNumber != null || job.MobileNumber != "")
                                {
                                    SmsHelper.SendSecondarySms(
                                    Constants.SmsSender.SecondaryUserName,
                                    Constants.SmsSender.SecondaryPassword,
                                    Constants.SmsBody.SMSResume
                                    .Replace("[VACANCY]", job.Position)
                                    .Replace("[NAME]", candidate.Name)
                                    .Replace("[EMAIL]", candidate.Email)
                                    .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                    .Replace("[QUALIFICATION]", candidateBasic)
                                    .Replace("[FUNCTION]", candidate.FunctionId == null || job.FunctionId == 0 ? "" : candidate.Function.Name)
                                    .Replace("[DESIGNATION]", candidate.Position)
                                    .Replace("[PRESENT_SALARY]", candidateannualsalary)
                                     .Replace("[LOCATION]", candidateLocation + "," + candidateCityName)
                                    .Replace("[DOB]", age.ToString())
                                    .Replace("[TOTAL_EXPERIENCE]", candidateexperience)
                                    .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female")
                                    ,
                                    Constants.SmsSender.SecondaryType,
                                    Constants.SmsSender.Secondarysource,
                                    Constants.SmsSender.Secondarydlr,
                                        job.MobileNumber
                                        );
                                }

                            }

                            if (Candidate == "true")
                            {

                                if (candidate.ContactNumber != null || candidate.ContactNumber != "")
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
                                        .Replace("[BASIC_QUALIFICATION]", jobbasicqualification != null ? jobbasicqualification : "Not Mentioned")
                                        .Replace("[EXP]", jobexp)
                                        .Replace("[LOCATION]", jobcityName.ToString())
                                        .Replace("[GENDER]", job.Male == true && job.Female == true ? "Male, Female" : job.Male == true ? "Male" : job.Female == true ? "Female " : "")
                                        ,
                                        Constants.SmsSender.SecondaryType,
                                        Constants.SmsSender.Secondarysource,
                                        Constants.SmsSender.Secondarydlr,
                                        candidate.ContactNumber
                                        );
                                }


                            }

                            msg = sendMethod == SendMethod.SMS ? "Successfully sent the SMS" : sendMethod == SendMethod.Email ? "Successfully sent the Email" : "SMS / Email";
                        }

                        if (sendMethod == SendMethod.Email || sendMethod == SendMethod.Both)
                        {
                            if (Organization == "true")
                            {
                                if (job.EmailAddress != null || job.EmailAddress != "")
                                {
                                    EmailHelper.SendEmailReply(
                                        Constants.EmailSender.EmployerSupport,
                                        job.EmailAddress,
                                        Constants.EmailSubject.CandidateMatch,
                                        Constants.EmailBody.MatchingCandidate
                                        .Replace("[ORG_NAME]", job.Organization.Name != null ? job.Organization.Name : "Not Mentioned")
                                        .Replace("[SPOT_TEXT] ", "")
                                        .Replace("[TEXT_VACANCY]", "Reference to your Vacancy submitted for " + job.Position + " find the details of the Candidate Matching your Vacancy .The Vacancy is sent to this candidate also.")
                                        .Replace("[TEXT_MATCH]", "")
                                        .Replace("[JOBNAME]", job.Position != null ? job.Position : "Not Mentioned")
                                        .Replace("[CANDIDATENAME]", candidate.Name != null ? candidate.Name : "Not Mentioned")
                                        .Replace("[MOBILE]", candidate.ContactNumber != null ? candidate.ContactNumber : "Not Mentioned")
                                        .Replace("[LANDLINE]", candidate.MobileNumber != null ? candidate.MobileNumber : "Not Mentioned")
                                        .Replace("[EMAIL]", candidate.Email != null ? candidate.Email : "Not Mentioned")
                                        .Replace("[ADDRESS]", candidate.Address != null ? candidate.Address : "Not Mentioned")
                                        .Replace("[BASICQUALIFICATION]", candidateBasic != "" ? candidateBasic : "Not Mentioned")
                                        .Replace("[POSTGRADUATION]", candidatePost != "" ? candidatePost : "Not Mentioned")
                                        .Replace("[DOCTRATE]", candidateDoctorate != "" ? candidateDoctorate : "Not Mentioned")
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

                            }


                            if (Candidate == "true")
                            {
                                if (candidate.Email != null || candidate.Email != "")
                                {
                                    jobmatchmaincontent.Append(
                                    Constants.EmailBody.MatchingJobMain
                                    .Replace("[DESCRIPTION]", job.Description != null ? job.Description : "Not Mentioned")
                                    .Replace("[POSITION]", job.Position != null ? job.Position : "Not Mentioned")
                                    .Replace("[COMPANY_NAME]", job.Organization != null ? job.Organization.Name : "Not Mentioned")
                                    .Replace("[INDUSTRY_TYPE]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "Not Mentioned")
                                    .Replace("[EXPERIENCE]", jobexp)
                                    .Replace("[SPOT_TEXT]", "")
                                    .Replace("[POSTING_LOCATION]", joblocation.ToString())
                                    .Replace("[BASICQUALIFICATION]", jobbasicqualification)
                                    .Replace("[POSTGRADUATION]", jobpostgraduation)
                                    .Replace("[DOCTRATE]", jobdoctrate)
                                    .Replace("[SKILLS]", jobskills)
                                    .Replace("[CONTACT_PERSON]", job.ContactPerson != null ? job.ContactPerson : "Not Mentioned")
                                    .Replace("[MOBILE]", job.MobileNumber)
                                    .Replace("[LANDLINE]", job.ContactNumber != null ? job.ContactNumber : "Not Mentioned")
                                    .Replace("[EMAIL]", job.EmailAddress)
                                    .Replace("[WEBSITE]", job.Organization != null ? job.Organization.Website : "Not Mentioned")
                                    );
                                }
                            }

                            bool alertSent = false;

                            if (RatActivated == true)
                            {
                                if (job.OrganizationId == -1)
                                {
                                    _vasRepository.UpdateAlertsDoneCount((int)job.ConsultantId, (int)job.Id);
                                    _vasRepository.UpdateAlertsDoneRAT((int)job.ConsultantId, (int)job.Id);
                                    _vasRepository.logAlerts((int)job.ConsultantId, (int)job.Id, candidate.Id, smsSent, mailSent);
                                }
                                else
                                {
                                    _vasRepository.UpdateAlertsDoneCount((int)job.ConsultantId, (int)job.Id);
                                    _vasRepository.UpdateAlertsDoneRAT((int)job.ConsultantId, (int)job.Id);
                                    _vasRepository.logAlerts((int)job.ConsultantId, (int)job.Id, candidate.Id, smsSent, mailSent);
                                    alertSent = true;
                                }

                            }

                            if (alertSent == false)
                            {
                                if (HorsActivated == true)
                                {
                                    if (job.OrganizationId == -1)
                                    {
                                        _vasRepository.CheckForHorsRemainingCountConsultant(Convert.ToInt32(job.ConsultantId), candidate.Id);
                                    }
                                    else
                                    {
                                        _vasRepository.CheckForHorsRemainingCount(job.OrganizationId, candidate.Id);
                                    }

                                }
                            }
                            

                            msg = sendMethod == SendMethod.SMS ? "Successfully sent the SMS" : sendMethod == SendMethod.Email ? "Successfully sent the Email" : "Successfully sent the SMS / Email";
                        }
                    }
                }

            }

            //if (Candidate == "true" && candidate.Email != "" && sendMethod == SendMethod.Email)
            //{
            //    EmailHelper.SendEmail(
            //                    Constants.EmailSender.EmployerSupport,
            //                     candidate.Email,
            //                    Constants.EmailSubject.JobMatch,
            //                    Constants.EmailBody.MatchingJobHeader
            //                    .Replace("[NAME]", candidate.Name)
            //                    + jobmatchmaincontent + Constants.EmailBody.MatchingJobFooter
            //                    );
            //    msg = sendMethod == SendMethod.Email ? "Successfully sent the Email" : "Successfully sent the SMS";
            //}

            //msg = sendMethod == SendMethod.SMS ? "SMS" : sendMethod == SendMethod.Email ? "Email" : "SMS / Email";
            //string sentTo = (Candidate == "true" && Organization == "true") ? "Candidate and Organizations" : Candidate == "true" ? "Candidate" : Organization == "true" ? "Organizations" : "";
            //return Json(new JsonActionResult { Success = true, Message = "Successfully sent the " + msg + " to " + sentTo });

            if (Candidate == "true" && candidate.Email != "" && sendMethod == SendMethod.Email)
            {
                EmailHelper.SendEmail(
                                Constants.EmailSender.EmployerSupport,
                                 candidate.Email,
                                Constants.EmailSubject.JobMatch,
                                Constants.EmailBody.MatchingJobHeader
                                .Replace("[NAME]", candidate.Name)
                                + jobmatchmaincontent + Constants.EmailBody.MatchingJobFooter
                                );
                msg = sendMethod == SendMethod.Email ? "Successfully sent the Email" : "Successfully sent the SMS";
            }


            string sentTo = (Candidate == "true" && Organization == "true") ? "Candidate and Organizations" : Candidate == "true" ? "Candidate" : Organization == "true" ? "Organizations" : "";
            return Json(new JsonActionResult { Success = true, Message = msg + " to " + sentTo });
        }

    }

    }

