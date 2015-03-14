using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Enums;
using System.Data.Objects;
using System.Collections;

namespace Dial4Jobz.Models.Repositories
{
    public class Repository : IRepository
    {
        private Dial4JobzEntities _db = new Dial4JobzEntities();

        DateTime currentdate = Constants.CurrentTime().Date;

        VasRepository _vasRepository = new VasRepository();

        #region IRepository Members

        public IQueryable<Candidate> GetCandidates()
        {
            return (from candidate in _db.Candidates
                    orderby candidate.CreatedDate descending
                    where (!String.IsNullOrEmpty(candidate.Name))
                    select candidate);
        }

        internal IQueryable<Candidate> GetFullCandidates()
        {
            return _db.Candidates;
        }
       
        public IQueryable<Consultante> GetConsultantes()
        {
            return (from consultant in _db.Consultantes
                    orderby consultant.CreatedDate descending
                    where (!String.IsNullOrEmpty(consultant.Name))
                    select consultant);
        }



        public IQueryable<Candidate> GetCandidateWithoutAddress()
        {
            return (from candidate in _db.Candidates
                    where candidate.Address == null
                    select candidate);
        }
               
        //original

        public IQueryable<Job> GetJobs()
        {
            return (from job in _db.Jobs
                    orderby job.CreatedDate descending

                    where !string.IsNullOrEmpty(job.Position) //&& job.JobSkills.Count > 1
                    select job);
        }

        public IQueryable<Job> GetJobsByPosition()
        {
            return from job in _db.Jobs
                   orderby job.Position ascending
                   select job;
        }
        

        public IQueryable<Job> GetJobsLatest()
        {
            return _db.Jobs;
        }
                  
             
        public IQueryable<Job> GetJobs(string what, string location)
        {

                    return (from job in _db.Jobs
                    where (String.IsNullOrEmpty(what) ||
                          (!String.IsNullOrEmpty(job.Position) && job.Position.Contains(what)) ||
                          (!String.IsNullOrEmpty(job.Organization.Name) && job.Organization.Name.Contains(what)) ||
                          job.JobSkills.Any(j => j.Skill.Name.Contains(what))) &&

                          (job.JobLocations.Any(j => j.Location.City.Name.Contains(location)) ||
                          job.JobLocations.Any(j => j.Location.Country.Name.Contains(location)) ||
                          job.JobLocations.Any(j => j.Location.State.Name.Contains(location)))
                           
                         orderby job.CreatedDate descending
                    select job);
        }
        
       
        public List<string> GetOrganizationName(string EmployerName)
        {
            List<string> list = new List<string>();
            var first = (from org in _db.Organizations
                         where org.Name.ToLower().StartsWith(EmployerName.ToLower()) || org.Name.ToLower().Contains(EmployerName.ToLower())
                         orderby org.Name ascending
                         select org.Name).Distinct().Take(20);

            foreach (var fir in first)
            {
                list.Add(fir);
            }
            return list;
        }

        public List<string> GetConsultantName(string consultantName)
        {
            List<string> list = new List<string>();
            var first = (from consultant in _db.Consultantes
                         where consultant.Name.ToLower().StartsWith(consultantName.ToLower()) || consultant.Name.ToLower().Contains(consultantName.ToLower())
                         orderby consultant.Name ascending
                         select consultant.Name).Distinct().Take(20);

            foreach (var fir in first)
            {
                list.Add(fir);
            }
            return list;
        }

       
        public IQueryable<Job> GetJobs(string what, string location, string skill, string position, string organization, string minSalary, string maxSalary, string minExperience, string maxExperience, string function, string preferredtype)
        {
            int skillId;
            bool skillSpecified = int.TryParse(skill, out skillId);

            int locationId;
            bool locationIsAnId = int.TryParse(location, out locationId);
                                                            
            int minimumSalary;
            bool minSalarySpecified = int.TryParse(minSalary, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(maxSalary, out maximumSalary);

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(minExperience, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(maxExperience, out maximumExperience);

            int funcId = GetFunctionId(function);
            
           // int candidateFunctions;
            //bool FunctionsSpecified = int.TryParse(CandidateFunctions, out candidateFunctions);

            //DateTime fresh = DateTime.Now;
            DateTime fresh = currentdate;
                        
                   return (from job in _db.Jobs
                   where  (String.IsNullOrEmpty(what) ||
                          (!String.IsNullOrEmpty(job.Position) && job.Position.Contains(what)) ||
                          (!String.IsNullOrEmpty(job.Organization.Name) && job.Organization.Name.Contains(what)) ||
                          job.JobSkills.Any(j => j.Skill.Name.Contains(what))) &&
                                                                                                      
                          ((!locationIsAnId &&
                          (String.IsNullOrEmpty(location) ||
                          job.JobLocations.Any(j => j.Location.City.Name.Contains(location)) ||
                          job.JobLocations.Any(j => j.Location.Country.Name.Contains(location)) ||
                          job.JobLocations.Any(j => j.Location.State.Name.Contains(location)))) ||

                          (locationIsAnId &&
                          job.JobLocations.Select(j => j.LocationId).Contains(locationId))) &&

                           // (String.IsNullOrEmpty(function) ||
                         //candidate.Function.Name.Contains(function)) &&
                         (string.IsNullOrEmpty(function) ||
                          job.FunctionId==funcId) && 

                          (string.IsNullOrEmpty(skill) ||
                          job.JobSkills.Select(j => j.SkillId).Contains(skillId)) &&

                          (string.IsNullOrEmpty(position) ||
                          job.Position.Contains(position)) &&

                          (string.IsNullOrEmpty(organization) ||
                          job.Organization.Name.Contains(organization)) &&

                          (string.IsNullOrEmpty(minSalary) ||
                          ((!job.Budget.HasValue || (job.Budget >= minimumSalary && job.Budget <= maximumSalary)) && 
                          (!job.MaxBudget.HasValue || (job.MaxBudget >= minimumSalary && job.MaxBudget <= maximumSalary)))) &&
                           
                          //(string.IsNullOrEmpty(minExperience) ||
                          //(((job.MinExperience >= minimumExperience && job.MinExperience <= maximumExperience)) && 
                          //((job.MaxExperience >= minimumExperience && job.MaxExperience <= maximumExperience))))

                          (string.IsNullOrEmpty(minExperience) ||
                          (((job.MinExperience >= minimumExperience && job.MaxExperience < maximumExperience))))

                           //orderby job.OrganizationId!=-1 descending,job.OrganizationId!=null descending, job.CreatedDate descending 
                           orderby job.CreatedDate descending
                           select job);
                          }


        public IQueryable<Job> GetJobs(string what, string location, string language, string CandidateFunctions, string Roles, string RefineFunction, string RefineOrganization, string Industries, string MinExperienceYears, string MaxExperienceYears, string MinAnnualSalaryLakhs, string MaxAnnualSalaryLakhs, string TypeOfVacancy, string TypeOfShift, string Freshness)
        {
            var whats = what.Split(',');

            if (whats[0] == "")
            {
                whats[0] = "1";
            }
            var whatArray = Array.ConvertAll(whats, s => s.ToString());

            var languages = language.Split(',');
            if (languages[0] == "")
            {
                languages[0] = "1";
            }
            var languageArray = Array.ConvertAll(languages, s => s.ToString());
            

            var locations = location.Split(',');

            if (locations[0] == "")
            {
                locations[0] = "1";
            }
            var locationArray = Array.ConvertAll(locations, s => s.ToString());

            int locationId;
            bool locationIsAnId = int.TryParse(location, out locationId);

            int candidateFunctions;
            bool FunctionsSpecified = int.TryParse(CandidateFunctions, out candidateFunctions);

            int candidateRoles;
            bool RolesSpecified = int.TryParse(Roles, out candidateRoles);

            int minimumSalary;
            bool minSalarySpecified = int.TryParse(MinAnnualSalaryLakhs, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(MaxAnnualSalaryLakhs, out maximumSalary);

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(MinExperienceYears, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(MaxExperienceYears, out maximumExperience);

            var functions = RefineFunction.Split(',');

            if (functions[0] == "")
            {
                functions[0] = "1";
            }
            var functionArray = Array.ConvertAll(functions, s => int.Parse(s));

           
            var organizations = RefineOrganization.Split(',');

            if (organizations[0] == "")
            {
                organizations[0] = "1";
            }
            var organizationArray = Array.ConvertAll(organizations, s => s.ToString());

            var preferredIndusties = Industries.Split(',');

            if (preferredIndusties[0] == "")
                preferredIndusties[0] = "1";

            var industriesArray = Array.ConvertAll(preferredIndusties, p => int.Parse(p));

            var candidateroles = Roles.Split(',');
            if (candidateroles[0] == "")
            {
                candidateroles[0] = "1";
            }
            var roleArray = Array.ConvertAll(candidateroles, s => int.Parse(s));

            string Fulltime = string.Empty, Parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;
            string[] vacancy = TypeOfVacancy.Split(',');
            foreach (string vac in vacancy)
            {
                if (vac == "1")
                    Fulltime = "1";

                if (vac == "2")
                    Parttime = "2";

                if (vac == "3")
                    contract = "3";

                if (vac == "4")
                    workfromhome = "4";
            }

            string GeneralShift = string.Empty, NightShift = string.Empty;
            string[] shift = TypeOfShift.Split(',');
            foreach (string ws in shift)
            {
                if (ws == "1")
                    GeneralShift = "1";

                if (ws == "2")
                    NightShift = "2";
            }

            
            DateTime fresh = currentdate;

            if (Freshness == "1")
                fresh = DateTime.Now.AddDays(-1);

            if (Freshness == "2")
                fresh = DateTime.Now.AddDays(-7);

            if (Freshness == "3")
                fresh = DateTime.Now.AddDays(-30);

            //var freshen = fresh;

            return (from job in _db.Jobs
                    join pfInd in _db.JobPreferredIndustries on job.Id equals pfInd.JobId into t
                    from rt in t.DefaultIfEmpty()
                    //orderby job.Position ascending
                    where (String.IsNullOrEmpty(what) ||
                          (!String.IsNullOrEmpty(job.Position) && whatArray.Contains(job.Position)) ||
                          (!String.IsNullOrEmpty(job.Organization.Name) && whatArray.Contains(job.Organization.Name)) ||
                          job.JobSkills.Any(j => whatArray.Contains(j.Skill.Name))) &&

                          (String.IsNullOrEmpty(language) ||
                          job.JobLanguages.Any(jl=> languageArray.Contains(jl.Language.Name))) &&

                          (String.IsNullOrEmpty(location) ||
                          job.JobLocations.Any(j => locationArray.Contains(j.Location.City.Name))) &&

                          //job.JobLocations.Any(j => j.Location.Country.Name.Contains(location)) ||
                        //job.JobLocations.Any(j => j.Location.State.Name.Contains(location)))) ||

                          //(locationIsAnId &&
                        //job.JobLocations.Select(j => j.LocationId).Contains(locationId))) &&

                          (string.IsNullOrEmpty(RefineOrganization) || (organizationArray.Contains(job.Organization.Name))) &&

                          (string.IsNullOrEmpty(RefineFunction) || (functionArray.Contains((int)job.FunctionId))) &&

                          (String.IsNullOrEmpty(Roles) ||
                          job.JobRoles.Any(r => roleArray.Contains((int)r.RoleId))) &&

                          (string.IsNullOrEmpty(CandidateFunctions) ||
                          job.FunctionId == candidateFunctions) &&
                         

                          (string.IsNullOrEmpty(Industries) || (industriesArray.Contains((int)rt.IndustryId))) &&

                          (string.IsNullOrEmpty(Fulltime) || job.PreferredFulltime == true) &&

                          (string.IsNullOrEmpty(Parttime) || job.PreferredParttime == true) &&

                          (string.IsNullOrEmpty(contract) || job.PreferredContract == true) &&

                          (string.IsNullOrEmpty(workfromhome) || job.PreferredWorkFromHome == true) &&

                          // Match by General Shift
                        (string.IsNullOrEmpty(GeneralShift) || job.GeneralShift == true) &&
                        //Match by Night Shift
                        (string.IsNullOrEmpty(NightShift) || job.NightShift == true) &&

                          (string.IsNullOrEmpty(MinAnnualSalaryLakhs) ||
                          ((!job.Budget.HasValue || (job.Budget >= minimumSalary && job.Budget <= maximumSalary)) &&
                          (!job.MaxBudget.HasValue || (job.MaxBudget >= minimumSalary && job.MaxBudget <= maximumSalary)))) &&

                          (string.IsNullOrEmpty(MinExperienceYears) ||
                          (((job.MinExperience >= minimumExperience && job.MinExperience < maximumExperience)))) &&
                        // &&((job.MaxExperience >= minimumExperience && job.MaxExperience <= maximumExperience))))

                          (string.IsNullOrEmpty(Freshness) ||
                          (((job.CreatedDate.Value >= fresh))))

                    orderby job.CreatedDate descending 
                    select job);
        } 


         //get candidates
        public IQueryable<Candidate> GetCandidates(string what, string location, string CurrentLocation, string skill, string position, string function, string gender, string minSalary, string maxSalary, string TotalSalary, string minExperience, string maxExperience)
        {
            int skillId;
            bool skillSpecified = int.TryParse(skill, out skillId);

            int locationId;
            bool locationIsAnId = int.TryParse(location, out locationId);

            int minimumSalary;
            bool minSalarySpecified = int.TryParse(minSalary, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(maxSalary, out maximumSalary);

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(minExperience, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(maxExperience, out maximumExperience);

            int annualSalary;
            bool annualSalarySpecified = int.TryParse(TotalSalary, out annualSalary);

            DateTime currentdate = Constants.CurrentTime();
            DateTime fresh = currentdate;
            //fresh = DateTime.Now.AddDays(-1);
            fresh = DateTime.Now.AddMinutes(-40);
                                
            //int Position;
            //bool positionSpecified = int.TryParse(position, out Position);
           
            return (from candidate in _db.Candidates
                    join locat in _db.Locations on candidate.LocationId equals locat.Id into l
                    from locations in l.DefaultIfEmpty()
                   
                    where (String.IsNullOrEmpty(what) ||
                    (!String.IsNullOrEmpty(candidate.Position) && candidate.Position.Contains(what)) ||
                    (!String.IsNullOrEmpty(candidate.Function.Name) && candidate.Function.Name.Contains(what)) &&
                    candidate.CandidateSkills.Any(j => j.Skill.Name.Contains(what))) &&

                     (string.IsNullOrEmpty(CurrentLocation) ||
                    locations.City.Name.Contains(CurrentLocation) ||
                    locations.Region.Name.Contains(CurrentLocation) ||
                    locations.Country.Name.Contains(CurrentLocation)) &&

                    ((!locationIsAnId &&
                    (String.IsNullOrEmpty(location) ||
                    candidate.CandidatePreferredLocations.Any(j => j.Location.City.Name.Contains(location)) ||
                    candidate.CandidatePreferredLocations.Any(j => j.Location.Country.Name.Contains(location)) ||
                    candidate.CandidatePreferredLocations.Any(j => j.Location.State.Name.Contains(location)))) ||

                    (locationIsAnId &&
                     candidate.CandidatePreferredLocations.Select(j => j.LocationId).Contains(locationId))) &&
                                        

                    (string.IsNullOrEmpty(skill) ||
                    candidate.CandidateSkills.Select(j => j.SkillId).Contains(skillId)) &&
           
                    (string.IsNullOrEmpty(position) ||
                    candidate.Position.Contains(position)) &&

                    (String.IsNullOrEmpty(function)||
                    //(string.IsNullOrEmpty(function) ||
                    candidate.Function.Name.Contains(function)) &&

                    (string.IsNullOrEmpty(gender) ||
                    (gender == "Male" && candidate.Gender == (int)Gender.Male) ||
                    (gender == "Female" && candidate.Gender == (int)Gender.Female)) &&

                    (string.IsNullOrEmpty(TotalSalary) ||
                    (candidate.AnnualSalary == annualSalary)) &&


                    (string.IsNullOrEmpty(minSalary) ||
                    ((!candidate.AnnualSalary.HasValue || (candidate.AnnualSalary >= minimumSalary && candidate.AnnualSalary <= maximumSalary)))) &&
                    //(!candidate.MaxSalary.HasValue || (candidate.MaxSalary >= minimumSalary && candidate .MaxSalary <= maximumSalary)))) &&


                    (string.IsNullOrEmpty(minExperience) ||
                    ((!candidate.MinExperience.HasValue || (candidate.MinExperience >= minimumExperience && candidate.MinExperience <= maximumExperience)) &&
                    (!candidate.MaxExperience.HasValue || (candidate.MaxExperience >= minimumExperience && candidate.MaxExperience <= maximumExperience))))
                    
                    orderby fresh <= candidate.UpdatedDate descending
                   // c => fresh <= c.CreatedDate
                    //orderby candidate.UpdatedDate <=DateTime.UtcNow descending
                    select candidate);                    
        }




        public IQueryable<Job> GetMatchingJobsForCandidate(string BasicQualification, string postGraduation, string doctorate, string basicSpecialization, string postSpecialization, string doctrateSpecialization, string position, string skills, string languages, string minExperience, string maxExperience, string minSalary, string maxSalary, string industry, string function, string role, string cityName, string countryName, string preferredType, string parttime, string contract, string workfromhome, string gender, string generalshift, string nightshift, string licenseType)
        {

            var basicQualifications = BasicQualification.Split(',');
                            
            if (basicQualifications[0] == "")
            {
                basicQualifications[0] = "1";
            }
            
            var basicQualificationArray = Array.ConvertAll(basicQualifications, s => int.Parse(s));

            var postGraduations = postGraduation.Split(',');
            if (postGraduations[0] == "")
            {
                postGraduations[0] = "1";
            }
            var postGraduationArray = Array.ConvertAll(postGraduations, s => int.Parse(s));

            var doctorates = doctorate.Split(',');
            if (doctorates[0] == "")
            {
                doctorates[0] = "1";
            }
            var doctorateArray = Array.ConvertAll(doctorates, s => int.Parse(s));

            var basicSpecializations = basicSpecialization == null ? new[] { "0" } : basicSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var basicSpecializationArray = Array.ConvertAll(basicSpecializations, int.Parse);

            var postSpecializations = postSpecialization == null ? new[] { "0" } : postSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var postSpecializationArray = Array.ConvertAll(postSpecializations, int.Parse);

            var doctrateSpecializations = doctrateSpecialization == null ? new[] { "0" } : doctrateSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var doctrateSpecializationArray = Array.ConvertAll(doctrateSpecializations, int.Parse);

            var CityName = cityName.Split(',');
            var CityNameArray = Array.ConvertAll(CityName, c => c.ToString());

            var CountryName = countryName.Split(',');
            var CountryNameArray = Array.ConvertAll(CountryName, c => c.ToString());
            
            var Skills = skills.Split(',');

            if (Skills[0] == "")
            {
                Skills[0] = "1";
            }
            var skillsArray = Array.ConvertAll(Skills, s => s.ToString());

            var Languages = languages.Split(',');

            if (Languages[0] == "")
            {
                Languages[0] = "1";
            }
            var languageArray = Array.ConvertAll(Languages, l => l.ToString());

            var Roles = role.Split(',');

            if (Roles[0] == "")
            {
                Roles[0] = "1";
            }
            var roleArray = Array.ConvertAll(Roles, r => r.ToString());

            var Functions = function.Split(',');
            if (Functions[0] == "")
            {
                Functions[0] = "1";
            }
            var functionArray = Array.ConvertAll(Functions, f => int.Parse(f));

            var LicenseTypes = licenseType.Split(',');
            if (LicenseTypes[0] == "")
            {
                LicenseTypes[0] = "1";
            }
            var licenseTypeArray = Array.ConvertAll(LicenseTypes, l => l.ToString());
                      

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(minExperience, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(maxExperience, out maximumExperience);

            int minimumSalary;
            bool minSalarySpecified = int.TryParse(minSalary, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(maxSalary, out maximumSalary);

            int functionId;
            bool functionSpecified = int.TryParse(function, out functionId);
                       

            int industryId;
            bool industrySpecified = int.TryParse(industry, out industryId);


            return (from job in _db.Jobs
                    
                    where
                        // Match by function
                    
                        (String.IsNullOrEmpty(function) ||
                       functionArray.Contains((int)job.FunctionId)) &&

                       //Match by Preferred Roles
                        (string.IsNullOrEmpty(role) ||
                        job.JobRoles.Any(j => roleArray.Contains(j.Role.Name)) ||
                        (job.JobRoles.Count() == 0)) &&
                        
                        // Match by Position
                       // (string.IsNullOrEmpty(position) ||
                       // (!string.IsNullOrEmpty(job.Position) && job.Position.Contains(position))) &&

                         // Match by Skills
                        (string.IsNullOrEmpty(skills) || job.JobSkills.Any(j => skillsArray.Contains(j.Skill.Name)) ||
                        (job.JobSkills.Count()==0)) &&

                         //Match by Languages
                        (string.IsNullOrEmpty(languages) || job.JobLanguages.Any(j => languageArray.Contains(j.Language.Name))||
                        (job.JobLanguages.Count() == 0)) &&
                       
                                                 
                         //Match by LicenseTypes
                         (string.IsNullOrEmpty(licenseType) ||
                         job.JobLicenseTypes.Any(j=> licenseTypeArray.Contains(j.LicenseType.Name))) &&
                         
                           // Match by city
                        // (string.IsNullOrEmpty(cityName) ||
                        //    job.JobLocations.Any(j => j.Location.City.Name.Contains(cityName))) &&

                        // // Match by country
                        //(string.IsNullOrEmpty(countryName) ||
                        //    job.JobLocations.Any(j => j.Location.Country.Name.Contains(countryName))) &&

                        // Match by city
                         (string.IsNullOrEmpty(cityName) ||
                            job.JobLocations.Any(j => CityNameArray.Contains(j.Location.City.Name))) &&

                        // Match by country
                        (string.IsNullOrEmpty(countryName) ||
                            job.JobLocations.Any(j => CountryNameArray.Contains(j.Location.Country.Name))) &&



                         // Match by industry
                        //(string.IsNullOrEmpty(industry) ||
                        //job.JobPreferredIndustries.Any(j => j.IndustryId == industryId)) &&

                        ////Match by Gender
                        (string.IsNullOrEmpty(gender) ||
                            (gender=="Male" && job.Male==true) ||
                            (gender=="Female" && job.Female==true)) &&


                        (string.IsNullOrEmpty(preferredType) ||
                        (preferredType == "fulltime" && job.PreferredFulltime == true) ||
                        (preferredType == "contract" && job.PreferredContract == true) ||
                        (preferredType == "parttime" && job.PreferredParttime == true) ||
                        (preferredType == "workfromhome" && job.PreferredWorkFromHome == true)) &&
                                                                   

                         //Match by Shift Type
                         (string.IsNullOrEmpty(generalshift) || job.GeneralShift == true) &&
                         (string.IsNullOrEmpty(nightshift) || job.NightShift == true) &&
                         
                         //Match by Qualification

                        (String.IsNullOrEmpty(BasicQualification) ||
                        (job.JobRequiredQualifications.Any(r => basicQualificationArray.Contains((int)r.DegreeId))) ||
                        (job.JobRequiredQualifications.Count() == 0)) &&

                         (string.IsNullOrEmpty(postGraduation) ||
                          (job.JobRequiredQualifications.Any(r => postGraduationArray.Contains((int)r.DegreeId))) ||
                          (job.JobRequiredQualifications.Count() == 0)) &&

                         (string.IsNullOrEmpty(doctorate) ||
                          (job.JobRequiredQualifications.Any(r => doctorateArray.Contains((int)r.DegreeId))) ||
                          (job.JobRequiredQualifications.Count() == 0)) &&

                        (string.IsNullOrEmpty(basicSpecialization) ||
                          (job.JobRequiredQualifications.Any(r => basicSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) ||
                          (job.JobRequiredQualifications.Count()==0)) &&

                        (string.IsNullOrEmpty(postSpecialization) ||
                           (job.JobRequiredQualifications.Any(r => postSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) ||
                           (job.JobRequiredQualifications.Count()==0)) &&

                        (string.IsNullOrEmpty(doctrateSpecialization) ||
                           (job.JobRequiredQualifications.Any(r => doctrateSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) ||
                           (job.JobRequiredQualifications.Count()==0)) &&


                       // match by salary  
                        (string.IsNullOrEmpty(maxSalary) ||
                         ((job.MaxBudget >= maximumSalary))) &&

                      // Match by experience(correct one)
                       (string.IsNullOrEmpty(minExperience) ||
                       (job.MinExperience <= minimumExperience && job.MaxExperience >= maximumExperience)) 

                        orderby job.CreatedDate descending
                    select job);
        }

        

        public IQueryable<Candidate> GetMatchingCandiatesForJob(string BasicQualification, string postGraduation, string doctorate, string function, string cityName, string countryName, string roles, string preferredindustries, string male, string female, string gender, string minExperience, string maxExperience, string minSalary, string maxSalary, string skills, string languages, string preferredType, string parttime, string contract, string workfromhome, string generalshift, string nightshift, string licenseType)
        {
            var basicQualifications = BasicQualification.Split(',');
            
            if (basicQualifications[0] == "")
            {
                basicQualifications[0] = "1";
            }
            
            var basicQualificationArray = Array.ConvertAll(basicQualifications, s => int.Parse(s));

            var postGraduations = postGraduation.Split(',');
            if (postGraduations[0] == "")
            {
                postGraduations[0] = "1";
            }
            var postGraduationArray = Array.ConvertAll(postGraduations, s => int.Parse(s));

            var doctorates = doctorate.Split(',');
            if (doctorates[0] == "")
            {
                doctorates[0] = "1";
            }
            var doctorateArray = Array.ConvertAll(doctorates, s => int.Parse(s));

            int functionId;
            bool functionSpecified = int.TryParse(function, out functionId);

            //int male1;
            //bool maleSpecified = int.TryParse(male, out male1);

            //int female1;
            //bool femaleSpecified = int.TryParse(female, out female1);

            var Roles = roles.Split(',');
            if (Roles[0] == "")
            {
                Roles[0] = "1";
            }
            var rolesArray = Array.ConvertAll(Roles, s => int.Parse(s));

            var preferindustries = preferredindustries.Split(',');
            if (preferindustries[0] == "")
            {
                preferindustries[0] = "1";
            }
            var preferindustriesArray = Array.ConvertAll(preferindustries, s => int.Parse(s));

            var LicenseTypes = licenseType.Split(',');
            if (LicenseTypes[0] == "")
            {
                LicenseTypes[0] = "1";
            }
            var licenseTypeArray = Array.ConvertAll(LicenseTypes, l => l.ToString());

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(minExperience, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(maxExperience, out maximumExperience);

            int minimumSalary;
            bool minSalarySpecified = int.TryParse(minSalary, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(maxSalary, out maximumSalary);

            var Skills = skills.Split(',');

            if (Skills[0] == "")
            {
                Skills[0] = "1";
            }
            var skillsArray = Array.ConvertAll(Skills, s => s.ToString());
            

            var Languages = languages.Split(',');

            if (Languages[0] == "")
            {
                Languages[0] = "1";
            }
            var languageArray = Array.ConvertAll(Languages, s => s.ToString());


            return (from candidate in _db.Candidates
                    // join location in _db.Locations on candidate.LocationId equals location.Id into l
                     //from locations in l.DefaultIfEmpty()
                   
                    where
                    (String.IsNullOrEmpty(function) || candidate.FunctionId == functionId) &&

                    (String.IsNullOrEmpty(cityName) ||
                    (candidate.CandidatePreferredLocations.Any(j=>j.Location.City.Name.Contains(cityName))) ||
                    (candidate.CandidatePreferredLocations.Count()==0))&&

                    (String.IsNullOrEmpty(countryName) ||
                      (candidate.CandidatePreferredLocations.Any(j => j.Location.Country.Name.Contains(countryName))) ||
                      (candidate.CandidatePreferredLocations.Count() == 0)) &&

                    (String.IsNullOrEmpty(roles) ||
                     candidate.CandidatePreferredRoles.Any(r => rolesArray.Contains((int)r.RoleId))) &&

                    //(String.IsNullOrEmpty(preferredindustries) || preferindustriesArray.Contains((int)candidate.IndustryId)) &&

                   (string.IsNullOrEmpty(gender) ||
                    (gender == "Male" && candidate.Gender == (int)Gender.Male) ||
                    (gender == "Female" && candidate.Gender == (int)Gender.Female)) &&

                    //Match by LicenseTypes
                    (string.IsNullOrEmpty(licenseType) ||
                        candidate.CandidateLicenseTypes.Any(c=>licenseTypeArray.Contains(c.LicenseType.Name))) &&

                // Match by Skills                        
                    (String.IsNullOrEmpty(skills) ||
                        candidate.CandidateSkills.Any(j => skillsArray.Contains(j.Skill.Name))) &&

                    (String.IsNullOrEmpty(languages) ||
                        candidate.CandidateLanguages.Any(j => languageArray.Contains(j.Language.Name))) &&

                    (string.IsNullOrEmpty(preferredType) ||
                        (preferredType == "fulltime" && candidate.PreferredFulltime == true) ||
                        (preferredType == "contract" && candidate.PreferredContract == true) ||
                        (preferredType == "parttime" && candidate.PreferredParttime == true) ||
                        (preferredType == "workfromhome" && candidate.PreferredWorkFromHome == true)) &&
                    
                        
                    //((string.IsNullOrEmpty(preferredType) && (string.IsNullOrEmpty(parttime)) ||
                    //    (candidate.PreferredFulltime== true || candidate.PreferredParttime==true))) &&
                    
                    //// Match by fulltime
                    //    (string.IsNullOrEmpty(fulltime) || candidate.PreferredFulltime == true) &&

                    ////  Match by part time
                    //    (string.IsNullOrEmpty(parttime) || candidate.PreferredParttime == true) &&

                    //// Match by contract
                    //    (string.IsNullOrEmpty(contract) || candidate.PreferredContract == true) &&

                    //// Match by work from home
                    //    (string.IsNullOrEmpty(workfromhome) || candidate.PreferredWorkFromHome == true) &&
                    
                    // Match by morning shift
                       (string.IsNullOrEmpty(generalshift) || candidate.GeneralShift == true) &&

                    // Match by night shift
                        (string.IsNullOrEmpty(nightshift) || candidate.NightShift == true) &&

                        (String.IsNullOrEmpty(BasicQualification) ||
                            candidate.CandidateQualifications.Any(r => basicQualificationArray.Contains((int)r.DegreeId))) &&

                        (string.IsNullOrEmpty(postGraduation) ||
                            candidate.CandidateQualifications.Any(r => postGraduationArray.Contains((int)r.DegreeId))) &&

                        (string.IsNullOrEmpty(doctorate) ||
                            candidate.CandidateQualifications.Any(r => doctorateArray.Contains((int)r.DegreeId))) &&


                     (string.IsNullOrEmpty(maxSalary) ||
                     (candidate.AnnualSalary <= maximumSalary)) &&

                    (string.IsNullOrEmpty(minExperience) ||
                    ((candidate.TotalExperience >= minimumExperience && candidate.TotalExperience <= maximumExperience)))

                    orderby candidate.CreatedDate descending
                    select candidate);
        }


        public IQueryable<Candidate> GetCandidateSearch(string what, string language, string minExperience, string maxExperience, string minannualSalaryLakhs, string maxannualSalaryLakhs, string CurrentLocation, string AndOrLocations, string PreferredLocation, string Function, string role, string Industry, string BasicQualification, string postGraduation, string doctorate, string basicSpecialization, string postSpecialization, string doctrateSpecialization, string MinAge, string MaxAge, string position, string gender, string TypeOfVacancy, string workshift)
        {
            int minimumSalary;
            bool minSalarySpecified = int.TryParse(minannualSalaryLakhs, out minimumSalary);

            int maximumSalary;
            bool maxSalarySpecified = int.TryParse(maxannualSalaryLakhs, out maximumSalary);

            int minimumExperience;
            bool minExperienceSpecified = int.TryParse(minExperience, out minimumExperience);

            int maximumExperience;
            bool maxExperienceSpecified = int.TryParse(maxExperience, out maximumExperience);

            var what1 = what.Split(',');

            if (what1[0] == "")
            {
                what1[0] = "1";
            }
            var whatArray = Array.ConvertAll(what1, s => s.ToString());

            var language1 = language.Split(',');

            if (language1[0] == "")
            {
                language1[0] = "1";
            }
            var languageArray = Array.ConvertAll(language1, s => s.ToString());

            var currentLocations = CurrentLocation.Split(',');

            if (currentLocations[0] == "")
            {
                currentLocations[0] = "1";
            }
            var currentLocationArray = Array.ConvertAll(currentLocations, s => int.Parse(s));

            var preferredLocations = PreferredLocation.Split(',');

            if (preferredLocations[0] == "")
            {
                preferredLocations[0] = "1";
            }

            var preferredLocationsArray = Array.ConvertAll(preferredLocations, s => int.Parse(s));

            var functions = Function.Split(',');
            if (functions[0] == "")
            {
                functions[0] = "1";
            }
            var functionArray = Array.ConvertAll(functions, s => int.Parse(s));

            var roles = role.Split(',');
            if (roles[0] == "")
            {
                roles[0] = "1";
            }
            var roleArray = Array.ConvertAll(roles, s => int.Parse(s));

            var industries = Industry.Split(',');
            if (industries[0] == "")
            {
                industries[0] = "1";
            }
            var industriesArray = Array.ConvertAll(industries, s => int.Parse(s));

            var basicQualifications = BasicQualification.Split(',');
            if (basicQualifications[0] == "")
            {
                basicQualifications[0] = "1";
            }
            var basicQualificationArray = Array.ConvertAll(basicQualifications, s => int.Parse(s));

            var postGraduations = postGraduation.Split(',');
            if (postGraduations[0] == "")
            {
                postGraduations[0] = "1";
            }
            var postGraduationArray = Array.ConvertAll(postGraduations, s => int.Parse(s));

            var doctorates = doctorate.Split(',');
            if (doctorates[0] == "")
            {
                doctorates[0] = "1";
            }
            var doctorateArray = Array.ConvertAll(doctorates, s => int.Parse(s));

            var basicSpecializations = basicSpecialization == null ? new[] { "0" } : basicSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var basicSpecializationArray = Array.ConvertAll(basicSpecializations, int.Parse);

            var postSpecializations = postSpecialization == null ? new[] { "0" } : postSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var postSpecializationArray = Array.ConvertAll(postSpecializations, int.Parse);

            var doctrateSpecializations = doctrateSpecialization == null ? new[] { "0" } : doctrateSpecialization.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var doctrateSpecializationArray = Array.ConvertAll(doctrateSpecializations, int.Parse);

            var positions = position.Split(',');

            if (positions[0] == "")
            {
                positions[0] = "1";
            }
            var positionArray = Array.ConvertAll(positions, s => s.ToString());

            string GeneralShift = string.Empty, NightShift = string.Empty;
            string[] shift = workshift.Split(',');
            foreach (string ws in shift)
            {
                if (ws == "1")
                    GeneralShift = "1";

                if (ws == "2")
                    NightShift = "2";
            }
            

            string Fulltime = string.Empty, Parttime = string.Empty, contract = string.Empty, workfromhome = string.Empty;
            string[] vacancy = TypeOfVacancy.Split(',');
            foreach (string vac in vacancy)
            {
                if (vac == "1")
                    Fulltime = "1";

                if (vac == "2")
                    Parttime = "2";

                if (vac == "3")
                    contract = "3";

                if (vac == "4")
                    workfromhome = "4";
            }

            DateTime minage = DateTime.Now;
            DateTime maxage = DateTime.Now;
            if (!string.IsNullOrEmpty(MinAge))
            {
                minage = DateTime.Now.AddYears(-Convert.ToInt32(MinAge));
                maxage = DateTime.Now.AddYears(-Convert.ToInt32(MaxAge));
            }

            return (from candidate in _db.Candidates
                    join location in _db.Locations on candidate.LocationId equals location.Id into l
                    from locations in l.DefaultIfEmpty()
                    where
                    (String.IsNullOrEmpty(what) ||
                    candidate.CandidateSkills.Any(j => whatArray.Contains(j.Skill.Name))) &&

                    (String.IsNullOrEmpty(language) ||
                    candidate.CandidateLanguages.Any(j => languageArray.Contains(j.Language.Name))) &&


                     // Match by fulltime
                        (string.IsNullOrEmpty(Fulltime) || candidate.PreferredFulltime == true) &&

                    //  Match by part time
                        (string.IsNullOrEmpty(Parttime) || candidate.PreferredParttime == true) &&

                    // Match by contract
                        (string.IsNullOrEmpty(contract) || candidate.PreferredContract == true) &&

                    // Match by work from home
                        (string.IsNullOrEmpty(workfromhome) || candidate.PreferredWorkFromHome == true) &&

                        // Match by General Shift
                        (string.IsNullOrEmpty(GeneralShift) || candidate.GeneralShift== true) &&

                        //Match by Night Shift

                        (string.IsNullOrEmpty(NightShift) || candidate.NightShift==true) &&


                    //(string.IsNullOrEmpty(CurrentLocation) || CurrentLoc.Contains((int)locations.CityId)) &&

                    //(String.IsNullOrEmpty(PreferredLocation) ||
                        //candidate.CandidatePreferredLocations.Any(j => PreferredLoc.Contains((int)j.Location.CityId))) &&

                    (string.IsNullOrEmpty(AndOrLocations) ? 
                    ((string.IsNullOrEmpty(CurrentLocation) || currentLocationArray.Contains((int)locations.CityId)) && 
                    
                    (String.IsNullOrEmpty(PreferredLocation) || 
                    candidate.CandidatePreferredLocations.Any(j => preferredLocationsArray.Contains((int)j.Location.CityId)))) : ((string.IsNullOrEmpty(CurrentLocation) || 
                    currentLocationArray.Contains((int)locations.CityId)) || (String.IsNullOrEmpty(PreferredLocation) || 
                    candidate.CandidatePreferredLocations.Any(j => preferredLocationsArray.Contains((int)j.Location.CityId))))) &&

                    (String.IsNullOrEmpty(Function) || functionArray.Contains((int)candidate.FunctionId)) &&

                    (String.IsNullOrEmpty(role) ||
                    candidate.CandidatePreferredRoles.Any(r => roleArray.Contains((int)r.RoleId))) &&

                    (String.IsNullOrEmpty(Industry) || industriesArray.Contains((int)candidate.IndustryId)) &&

                    (String.IsNullOrEmpty(BasicQualification) ||
                    candidate.CandidateQualifications.Any(r => basicQualificationArray.Contains((int)r.DegreeId))) &&

                    (string.IsNullOrEmpty(postGraduation) ||
                    candidate.CandidateQualifications.Any(r => postGraduationArray.Contains((int)r.DegreeId))) &&

                    (string.IsNullOrEmpty(doctorate) ||
                    candidate.CandidateQualifications.Any(r => doctorateArray.Contains((int)r.DegreeId))) &&

                    //(string.IsNullOrEmpty(basicSpecialization) ||
                    //candidate.CandidateQualifications.Any(cq=> basicSpecializationArray.Contains((int)cq.SpecializationId)))&&

                    (string.IsNullOrEmpty(basicSpecialization) ||
                       candidate.CandidateQualifications.Any(r => basicSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) &&

                    (string.IsNullOrEmpty(postSpecialization) ||
                        candidate.CandidateQualifications.Any(r => postSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) &&

                    (string.IsNullOrEmpty(doctrateSpecialization) ||
                        candidate.CandidateQualifications.Any(r => doctrateSpecializationArray.Contains(r.SpecializationId.HasValue ? r.SpecializationId.Value : 0))) &&

                    (string.IsNullOrEmpty(position) || (positionArray.Contains(candidate.Position))) &&

                    (string.IsNullOrEmpty(gender) ||
                    (gender == "Male" && candidate.Gender == (int)Gender.Male) ||
                    (gender == "Female" && candidate.Gender == (int)Gender.Female)) &&

                    (string.IsNullOrEmpty(MinAge) || (candidate.DOB.Value >= maxage && candidate.DOB.Value <= minage)) &&

                    (string.IsNullOrEmpty(minannualSalaryLakhs) ||
                    ((candidate.AnnualSalary >= minimumSalary && candidate.AnnualSalary <= maximumSalary))) &&

                    (string.IsNullOrEmpty(minExperience) ||
                    ((candidate.TotalExperience >= minimumExperience && candidate.TotalExperience <= maximumExperience)))

                    orderby candidate.CreatedDate descending
                    select candidate);

        }
              


        public IQueryable<Job> GetJobsBySkill(int skillId)
        {
            return (from job in _db.Jobs
                    where job.JobSkills.Select(j => j.SkillId).Contains(skillId)
                    //orderby job.Position descending
                    orderby job.CreatedDate descending
                    select job);
        }             
        
        //Location jobs
        public IQueryable<Job> GetJobsByLocation(int locationId)
        {
            return (from job in _db.Jobs
                    where job.JobLocations.Select(j => j.LocationId).Contains(locationId)
                    orderby job.CreatedDate descending
                    //orderby job.CreatedDate descending
                    select job);
        }

        public IQueryable<Job> GetMatchingJobs(Candidate candidate)
        {
            IEnumerable<int> candidateSkillIds = candidate.CandidateSkills.Select(c => c.SkillId);
            IEnumerable<int> candidatePreferedFunctionIds = candidate.CandidatePreferredFunctions.Select(c => c.FunctionId);
            IEnumerable<int> candidateQualificationIds = candidate.CandidateQualifications.Select(c => c.DegreeId);
            //IEnumerable<int> candidateLangageIds = candidate.CandidateLanguages.Select(c => c.LanguageId);

            return (from job in _db.Jobs
                    where job.Budget >= candidate.AnnualSalary &&
                          job.MaxBudget >= candidate.AnnualSalary &&
                          job.MinExperience <= candidate.TotalExperience &&
                          job.MaxExperience >= candidate.TotalExperience &&
                          (!candidateSkillIds.Any() || candidateSkillIds.Any(c => job.JobSkills.Select(j => j.SkillId).Contains(c))) &&
                          ((!job.FunctionId.HasValue || !candidatePreferedFunctionIds.Any()) || candidatePreferedFunctionIds.Any(c => c == job.FunctionId)) &&
                          (!candidate.IndustryId.HasValue || job.JobPreferredIndustries.Any(j => candidate.IndustryId == j.IndustryId)) &&
                          (!candidateQualificationIds.Any() || candidateQualificationIds.Any(c => job.JobRequiredQualifications.Select(j => j.DegreeId).Contains(c))) &&
                          !string.IsNullOrEmpty(job.Position)
                    //orderby job.Position ascending
                    orderby job.CreatedDate descending
                    select job);
        }

        public IQueryable<Candidate> GetMatchingCandidates(Job job)
        {
            IEnumerable<int> jobSkillIds = job.JobSkills.Select(j => j.SkillId);
            IEnumerable<int> jobPreferredIndustryIds = job.JobPreferredIndustries.Select(j => j.IndustryId);
            IEnumerable<int> jobRequireQualifications = job.JobRequiredQualifications.Select(j => j.DegreeId);
            //IEnumerable<int> joblocations = job.JobLocations.Select(j => j.LocationId);

            return from candidate in _db.Candidates
                   where candidate.AnnualSalary <= job.Budget &&
                         candidate.AnnualSalary <= job.MaxBudget &&
                         candidate.TotalExperience >= job.MinExperience &&
                         candidate.TotalExperience <= job.MaxExperience &&
                         
                         (!jobSkillIds.Any() || jobSkillIds.Any(j => candidate.CandidateSkills.Select(c => c.SkillId).Contains(j))) &&
                         (!jobRequireQualifications.Any() || jobRequireQualifications.Any(j => candidate.CandidateQualifications.Select(c => c.DegreeId).Contains(j))) &&
                         (!job.FunctionId.HasValue || candidate.CandidatePreferredFunctions.Count(c => job.FunctionId == c.FunctionId) < 0) &&
                         ((!candidate.IndustryId.HasValue || !jobPreferredIndustryIds.Any()) || jobPreferredIndustryIds.Any(j => j == candidate.IndustryId))
                    orderby candidate.CreatedDate descending
                   //orderby candidate.Name 
                   select candidate;
        }

        public IEnumerable<Job> GetJobs(int id)
        {
            List<Job> job = new List<Job>();

            var jobs = (from jo in _db.Jobs
                        //where location.Id ==  location.JobLocations.Where(jbl => jbl.JobId == JobId).Select(jl => jl.LocationId) 
                        where jo.OrganizationId == id
                        select jo);

            foreach (var list in jobs)
                job.Add(list);

            return job;
        }


        //get city by list
        public IEnumerable<string> GetCityList(string term)
        {
            var citylist = from city in _db.Cities where city.Name.ToLower().StartsWith(term.ToLower()) orderby city.Name ascending select city.Name;

            return citylist;
        }


        public IEnumerable<City> GetGroupCountryStateCityList()
        {
            List<City> citylist = new List<City>();

            
            var country = from coun in _db.Countries orderby coun.Name == "India" ? 1 : 0 descending select coun;
            int count = 0;

            foreach (var cn in country)
            {
                citylist.Add(new City() { Id = cn.Id, Name = cn.Name, StateId = -1 });
                if (count == 0)
                {
                    citylist.Add(new City() { Id = 0, Name = "Major Metro Cities", StateId = -4 });

                    int[] topmetrocities = new[] { 21, 28, 797, 717, 237, 264, 1584, 1009, 1010 };

                    var metrocity = from cit in _db.Cities where topmetrocities.Contains(cit.Id) orderby cit.Name ascending select cit;

                    foreach (var ct in metrocity)
                    {
                        citylist.Add(new City() { Id = ct.Id, Name = ct.Name, StateId = -5 });
                    }
                    count = count + 1;

                }
                var state = from stat in _db.States where stat.CountryId.Equals(cn.Id) orderby stat.Name ascending select stat;
                foreach (var st in state)
                {
                    citylist.Add(new City() { Id = st.Id, Name = st.Name, StateId = -2 });

                    var city = from cit in _db.Cities where cit.StateId.Equals(st.Id) orderby cit.Name ascending select cit;
                    foreach (var ct in city)
                    {
                        citylist.Add(new City() { Id = ct.Id, Name = ct.Name, StateId = -3 });
                    }
                }
            }

            return citylist;
        }

      
        public IEnumerable<City> GetCitybyStateId(Int32 Id)
        {
            List<City> citylist = new List<City>();
            var city = from cit in _db.Cities where cit.StateId.Equals(Id) orderby cit.Name ascending select cit;


            foreach (var ct in city)
            {
                citylist.Add(new City() { Id = ct.Id, Name = ct.Name });
            }
            return citylist;
        }

        public IEnumerable<State> GetStatebyCountryId(Int32 Id)
        {
            List<State> statelist = new List<State>();
            var state = from stat in _db.States where stat.CountryId.Equals(Id) orderby stat.Name ascending select stat;
            foreach (var st in state)
            {
                statelist.Add(new State() { Id = st.Id, Name = st.Name });
            }
            return statelist;
        }



        public IEnumerable<string> GetSkillList(string term)
        {
            var skillList = (from skill in _db.Skills where skill.Name.ToLower().StartsWith(term.ToLower()) orderby skill.Name ascending select skill.Name)
                           .Union
                           (from job in _db.Jobs where job.Position.ToLower().StartsWith(term.ToLower()) orderby job.Position ascending select job.Position)
                           .Union
                           (from job in _db.Jobs where job.Organization.Name.ToLower().StartsWith(term.ToLower()) orderby job.Organization.Name ascending select job.Organization.Name);

            return skillList;
        }

        public IEnumerable<string> GetSkillsAlone(string term)
        {
            var skillList = (from skill in _db.Skills where skill.Name.ToLower().StartsWith(term.ToLower()) orderby skill.Name ascending select skill.Name);

            return skillList;
        }

        public IEnumerable<string> GetLanguagesAlone(string term)
        {
            var languageList = (from language in _db.Languages where language.Name.ToLower().StartsWith(term.ToLower()) orderby language.Name ascending select language.Name);

            return languageList;
        }

        //Location Candidate
        public IQueryable<Candidate> GetCandidateByLocation(int locationId)
        {
            return (from candidate in _db.Candidates
                    where candidate.CandidatePreferredLocations.Select(c => c.LocationId).Contains(locationId)

                     orderby candidate.Name descending
                    //orderby candidate.CreatedDate descending
                    select candidate);
        }

        //by vignesh
        public IEnumerable<Location> GetLocationsbyCandidateId(int candidateId)
        {
            List<Location> locationlist = new List<Location>();

            var locations = (from location in _db.Locations
                             where _db.CandidatePreferredLocations.Where(jl => jl.CandidateId == candidateId).Select(j => j.LocationId).Contains(location.Id)
                             select location);

            foreach (var loc in locations)
                locationlist.Add(new Location() { Id = loc.Id, CountryId = loc.CountryId, StateId = loc.StateId, CityId = loc.CityId, RegionId = loc.RegionId });

            return locationlist;
        }

                 
        //skill Candidate
        public IQueryable<Candidate> GetCandidateBySkill(int skillId)
        {
            return (from candidate in _db.Candidates
                    where candidate.CandidateSkills.Select(cs => cs.SkillId).Contains(skillId)

                    orderby candidate.Name descending
                    select candidate);
        }

        public Candidate GetCandidateByUserName(string userName)
        {
            return _db.Candidates.SingleOrDefault(c => c.UserName == userName);
        }

       

        public Candidate GetCandidateByMobileCode(int mobileCode)
        {
            return _db.Candidates.SingleOrDefault(o => o.PhoneVerificationNo == mobileCode);
        }

        public IQueryable<Function> GetFunctions()
        {
            return from function in _db.Functions
                   orderby function.Name ascending
                   select function;
        }
                
        public IQueryable<Function> GetFunctions(string q)
        {
            return from function in _db.Functions
                   where function.Name.Contains(q)
                   orderby function.Name ascending
                   select function;
        }

        public IEnumerable<Function> GetFunctionsEnumerable()
        {
            List<Function> funclist = new List<Function>();

            var func = (from function in _db.Functions
                        orderby function.Name ascending
                        select function);

            foreach (var f in func)
                funclist.Add(new Function() { Id = f.Id, Name = f.Name });

            return funclist;
        }


        public Function GetFunction(int functionId)
        {
            return _db.Functions.SingleOrDefault(u => u.Id == functionId);
        }

        public EmployeesCount GetEmployeesCount(int id)
        {
            return _db.EmployeesCounts.SingleOrDefault(u => u.Id == id);
        }
       
        public int GetFunctionId(string function)
        {
            var funcid = (from fun in _db.Functions
                          where fun.Name == function
                          select fun.Id);

            return funcid.SingleOrDefault();
        }

        public IQueryable<Job> GetJobsByFuncId(int id)
        {

            return (from job in _db.Jobs
                    where job.FunctionId == id
                    orderby job.OrganizationId != -1 descending, job.OrganizationId != null descending
                    select job);

        }

        /***Developer Note: For Candidates click by employer/consultant tracking the below method is created**/
        public void GetLogForCandidateViewByEmployer(int candidateId, int organizationId, int consultantId)
        {
            if (organizationId != 0)
            {
                bool existsorNot = DPRExistsOrNot(candidateId, organizationId, 0);
                if (existsorNot == true)
                {
                    DPRLog objcvc = new DPRLog
                    {
                        OrganizationId = organizationId,
                        CandidateId = candidateId,
                        ViewedDate = currentdate
                    };
                    _db.DPRLogs.AddObject(objcvc);
                    Save();
                }
            }
            else if (consultantId != 0)
            {
                bool existsorNot = DPRExistsOrNot(0, candidateId, consultantId);
                if (existsorNot == true)
                {
                    DPRLog objcvc = new DPRLog
                    {
                        ConsultantId = consultantId,
                        CandidateId = candidateId,
                        ViewedDate = currentdate
                    };
                    _db.DPRLogs.AddObject(objcvc);
                    Save();
                }
            }
            //}

        }

        public bool DPRExistsOrNot(int organizationId, int candidateId, int consultantId)
        {
            var alreadysentornot = from al in _db.DPRLogs where al.OrganizationId == organizationId && al.CandidateId == candidateId select al;
            var consultantsentornot = from al in _db.DPRLogs where al.ConsultantId == consultantId && al.CandidateId == candidateId select al;

            if (alreadysentornot.Count() > 0)
                return false;
            else if (consultantsentornot.Count() > 0)
                return false;
            else
                return true;

        }

        ///***Developer Note: For Jobs click by candidates/consultant tracking the below method is created**/
        public void GetLogForJobsViewByCandidate(int candidateId, int jobId, int consultantId)
        {
            var job = GetJob(jobId);

            if (candidateId != 0)
            {
                bool existsorNot = JobLogExistsOrNot(candidateId, 0, jobId);
                if (existsorNot == true)
                {
                    JobsLog objcvc = new JobsLog
                    {
                        OrganizationId = (job.ConsultantId == null ? job.OrganizationId : 0),
                        ConsultantId = (job.ConsultantId != null ? job.ConsultantId : 0),
                        JobId = jobId,
                        CandidateId = candidateId,
                        JobViewedDate = currentdate
                    };
                    _db.JobsLogs.AddObject(objcvc);
                    Save();
                }
            }
            else if (consultantId != 0)
            {
                bool existsorNot = JobLogExistsOrNot(0, consultantId, jobId);
                if (existsorNot == true)
                {
                    JobsLog objcvc = new JobsLog
                    {
                        OrganizationId = (job.ConsultantId == null ? job.OrganizationId : 0),
                        JobId = jobId,
                        ConsultantId = consultantId,
                        CandidateId = candidateId,
                        JobViewedDate = currentdate
                    };
                    _db.JobsLogs.AddObject(objcvc);
                    Save();
                }
            }

        }

        public bool JobLogExistsOrNot(int candidateId, int consultantId, int jobId)
        {
            var alreadysentornot = from al in _db.JobsLogs where al.CandidateId == candidateId && al.JobId == jobId select al;
            var consultantsentornot = from al in _db.JobsLogs where al.ConsultantId == consultantId && al.JobId == jobId select al;

            if (alreadysentornot.Count() > 0)
                return false;
            else if (consultantsentornot.Count() > 0)
                return false;
            else
                return true;

        }


        public IQueryable<Role> GetRolesByFunctionId(int functionId)
        {
            return _db.Roles.Where(ro => ro.FunctionId == functionId);
        }
              
        public Industry GetIndustry(int industryId)
        {
            return _db.Industries.SingleOrDefault(u => u.Id == industryId);
        }

        public MaritalStatus GetMaritalStatus(int maritalId)
        {
            return _db.MaritalStatuses.SingleOrDefault(u => u.Id == maritalId);
        }

        public Specialization GetSpecialization(int specializationId)
        {
            return _db.Specializations.SingleOrDefault(u => u.Id == specializationId);
        }

        public Location GetLocation(int locationId)
        {
            return _db.Locations.SingleOrDefault(l=>l.Id==locationId);
        }

        public Skill GetSkill(int skillId)
        {
            return _db.Skills.SingleOrDefault(s => s.Id == skillId);
        }

      
        public IEnumerable<City> GetCitybyStateIdwithStatename(int stateId)
        {
            List<City> citylist = new List<City>();

            var state = from stat in _db.States where stat.Id.Equals(stateId) orderby stat.Name ascending select stat;

            foreach (var st in state)
            {
                citylist.Add(new City() { Id = -1, Name = st.Name });

                var cities = from city in _db.Cities where city.StateId == stateId orderby city.Name ascending select city;

                foreach (var ct in cities)
                {
                    citylist.Add(new City() { Id = ct.Id, Name = ct.Name });
                }
            }

            return citylist;
        }


        
        ////City

        public IQueryable<City> GetCities()
        {
            return from city in _db.Cities
                   orderby city.Name ascending
                   select city;
        }

        public IQueryable<City> GetCities(string q)
        {
            return from city in _db.Cities
                   where city.Name.Contains(q)
                   orderby city.Name ascending
                   select city;
        }

        public IQueryable<LicenseType> GetLicenseTypes()
        {
            return from licenseType in _db.LicenseTypes
                   orderby licenseType.Name ascending
                   select licenseType;
        }

        public IEnumerable<LicenseType> GetLicenseTypesEnumerable()
        {
            List<LicenseType> license = new List<LicenseType>();

            var licenses = (from licenese in _db.LicenseTypes
                            orderby licenese.Name ascending
                            select licenese);

            foreach (var lic in licenses)
                license.Add(new LicenseType() { Id = lic.Id, Name = lic.Name });

            return license;
        }

        /*search employer Name*/
        public IQueryable<Organization> GetEmployerNames()
        {
            return from organization in _db.Organizations
                   orderby organization.Name ascending
                   select organization;
        }


        public IQueryable<Role> GetRoles()
        {
            return from role in _db.Roles
                   orderby role.Name ascending
                   select role;
        }


        public IQueryable<Role> GetRoles(int functionId)
        {
            return from role in _db.Roles
                   where role.FunctionId == functionId
                   orderby role.Name ascending
                   select role;
        }

        

        public IQueryable<Role> GetFunctionsByRoles(int roleId)
        {
            return from role in _db.Roles
                   where role.Id == roleId
                   orderby role.Function.Name ascending
                   select role;
        }
      
       
        public IEnumerable<Role> GetRolesEnumerable(Int32 Id)
        {
            List<Role> rolelist = new List<Role>();

            var Func = from function in _db.Functions
                       where function.Id == Id
                       orderby function.Name ascending
                       select function;

            foreach (var q in Func)
                rolelist.Add(new Role() { Id = -1, Name = q.Name });

            var Ro = (from role in _db.Roles
                      where role.FunctionId == Id
                     // orderby role.Name ascending
                      select role);

            foreach (var r in Ro)
                rolelist.Add(new Role() { Id = r.Id, Name = r.Name });

            return rolelist;
        }

      
       

        public void AddJobRole(JobRole role)
        {
            _db.JobRoles.AddObject(role);
        }

       

        public IEnumerable GetSkills()
        {
            return from skill in _db.Skills
                   orderby skill.Name ascending
                   select skill;
        }

        public IQueryable<Skill> GetSkills(string q)
        {
            return from skill in _db.Skills
                   where skill.Name.Contains(q)
                   orderby skill.Name ascending
                   select skill;
        }
        
        public IQueryable<Language> GetLanguages(string q)
        {
            return from language in _db.Languages
                   where language.Name.Contains(q)
                   orderby language.Name ascending
                   select language;
        }

        public IQueryable<Industry> GetIndustries()
        {
            return from industry in _db.Industries
                   orderby industry.Name ascending
                   select industry;
        }

        public IQueryable<EmployeesCount> GetEmployeesCount()
        {
            return from employees in _db.EmployeesCounts
                   orderby employees.Id ascending
                   select employees;
        }


        public IEnumerable<Industry> GetIndustriesEnumerable()
        {
            List<Industry> induslist = new List<Industry>();

            var indus = (from industry in _db.Industries
                         orderby industry.Name ascending
                         select industry);

            foreach (var q in indus)
                induslist.Add(new Industry() { Id = q.Id, Name = q.Name });

            return induslist;
        }


        public IQueryable<Industry> GetIndustries(string q)
        {
            return from industry in _db.Industries
                   where industry.Name.Contains(q)
                   orderby industry.Name ascending
                   select industry;
        }

        public IQueryable<MaritalStatus> GetMaritalStatus()
        {
            return from maritalstatus in _db.MaritalStatuses
                   orderby maritalstatus.Name ascending
                   select maritalstatus;
        }

        public IQueryable<MaritalStatus> GetMaritalStatus(string q)
        {
            return from maritalstatus in _db.MaritalStatuses
                   where maritalstatus.Name.Contains(q)
                   orderby maritalstatus.Name ascending
                   select maritalstatus;
        }


        public IQueryable<Degree> GetDegrees(DegreeType type)
        {
            return from degree in _db.Degrees
                   where degree.Type == (int)type
                   orderby degree.Name ascending
                   select degree;
        }

        public List<Degree> GetDegreesWithNoneOption(DegreeType type)
        {
            var degrees =  (from degree in _db.Degrees
                           where degree.Type == (int)type
                           orderby degree.Name ascending
                           select degree).ToList();

            Degree noDegree = new Degree();
            noDegree.Id = 0;
            //noDegree.Name = "None";
            noDegree.Name = "Select Degree";

            degrees.Insert(0, noDegree);
            return degrees;
        }


        public IEnumerable<Degree> GetDegreesEnumerable(DegreeType type)
        {
            List<Degree> degreelist = new List<Degree>();

            var degrees = (from degree in _db.Degrees
                           where degree.Type == (int)type
                           orderby degree.Name ascending
                           select degree);

            foreach (var d in degrees)
                degreelist.Add(new Degree() { Id = d.Id, Name = d.Name });

            return degreelist;
        }

        //Job qualification to show "Any course" option
        public List<Degree> GetDegreeswithAnyOption(DegreeType type)
        {
            var degrees = (from degree in _db.Degrees
                           where degree.Type == (int)type
                           orderby degree.Name ascending
                           select degree).ToList();

            Degree noDegree = new Degree();
            noDegree.Id = 0;
            noDegree.Name = "Any Course";
           
            degrees.Insert(0, noDegree);
            return degrees;
        }

        public IEnumerable<JobRequiredQualification> GetJobRequiredQualifications(int JobId, DegreeType degreeType)
        {
            List<JobRequiredQualification> jobRequiredQualificationlist = new List<JobRequiredQualification>();

            var jobRequiredQualification = (from reqQualification in _db.JobRequiredQualifications
                                            where reqQualification.JobId == JobId && reqQualification.Degree.Type == (int)degreeType
                                            select reqQualification);

            foreach (var jrq in jobRequiredQualification)
                jobRequiredQualificationlist.Add(new JobRequiredQualification() { Id = jrq.Id, DegreeId = jrq.DegreeId, SpecializationId = jrq.SpecializationId });

            return jobRequiredQualificationlist;
        }
        
        // new one for populating mulitsielect function and roles
        public IEnumerable<CandidatePreferredFunction> GetCandidatePreferredFunctions(int candidateId)
        {
            List<CandidatePreferredFunction> candidatePreferredFunctionlist = new List<CandidatePreferredFunction>();
            var candidatepreferredFunction = (from candpreferredfunction in _db.CandidatePreferredFunctions
                                              where candpreferredfunction.CandidateId == candidateId
                                              select candpreferredfunction);
            foreach (var cpf in candidatepreferredFunction)
                candidatePreferredFunctionlist.Add(new CandidatePreferredFunction() { Id = cpf.Id, FunctionId = cpf.FunctionId, RoleId = cpf.RoleId });
            return candidatePreferredFunctionlist;
        }

        public CandidatePreferredFunction GetcandidateFunction(int candidateId, int functionId)
        {
            //return _db.ChannelPartners.Where(ch => ch.Id == id).FirstOrDefault();
            return _db.CandidatePreferredFunctions.Where(cp => cp.CandidateId == candidateId && cp.FunctionId==functionId).FirstOrDefault();
        }


        //Specialization
        public List<Specialization> GetSpecializationByDegreeId(int degreeId)
        {
            return new[] { new Specialization { Id = 0, Name = "--Select Specialization--" } }.Concat(
                (from specialization in _db.Specializations
                 where specialization.DegreeId == degreeId
                 orderby specialization.Name ascending
                 select specialization)).ToList();
        }

        //Get Roles

        public List<Role> GetRoleByFunctionId(int functionId)
        {

            var result = new List<Role> {
             new Role { Id = -1, Name = "--Select Roles--" },
             new Role { Id = 0, Name = "Any" }
            };

            var roles = from role in _db.Roles
                        where role.FunctionId == functionId
                        orderby role.Name ascending
                        select role;

            result.AddRange(roles);

            return result;
        }

        public List<Role> GetFunctionsByRoleId(int roleId)
        {
            var result= new List<Role>{

        };
            var functions = from func in _db.Roles
                            where func.Id == roleId
                            orderby func.Function.Name ascending
                            select func;
            result.AddRange(functions);
            return result;
            
        }

        public int GetFunctionIdbyRole(int roleId)
        {
            return _db.Roles.FirstOrDefault(r => r.Id == roleId).FunctionId;
        }

        public string GetFunctionNameById(int functionId)
        {
            return _db.Functions.SingleOrDefault(f => f.Id == functionId).Name;
        }


        public List<Specialization> GetSpecializationWithDegreeNameByDegreeId(int degreeId)
        {

            var list = new List<Specialization>();

            var degreeQuery = (from degree in _db.Degrees
                               where degree.Id == degreeId
                               select degree);

            foreach (var d in degreeQuery)
                list.Add(new Specialization { Id = -1, Name = d.Name });

            var specQuery = from specialization in _db.Specializations
                            where specialization.DegreeId == degreeId
                            orderby specialization.Name ascending
                            select specialization;

            foreach (var s in specQuery)
                list.Add(new Specialization { Id = s.Id, Name = s.Name });

            return list;
        }
        

        //Institutes
        public List<Institute> GetInstitutesWithSelectOption()
        {
            return new[] { new Institute { Id = 0, Name = "--Select Institute--" } }.Concat(
                            from institute in _db.Institutes
                            orderby institute.Name ascending
                            select institute).ToList();
        }
        //Passed Out Year
        public List<SelectListItem> GetPassedOutYearWithSelectOption()
        {
            return new[] { new SelectListItem { Value = "0", Text = "--Select Year--" } }.Concat(
                Enumerable.Range(1950, DateTime.Now.Year - 1945).Select(i =>
                                                                        new SelectListItem { Value = i.ToString(), Text = i.ToString() }))
                .ToList();


        }

        public IQueryable<OrderDetail> GetOrderDetail()
        {
            return _db.OrderDetails;
        }

        public IQueryable<OrderMaster> GetOrderMaster()
        {
            return _db.OrderMasters;
        }

        public OrderDetail GetOrderDetail(int orderid)
        {
            return _db.OrderDetails.SingleOrDefault(o => o.OrderId == orderid);
        }

       

       
        public IQueryable<Job> GetJobsByOrganization()
        {
            return _db.Jobs;
        }

       
        #region Job

        public string GetJobById(int Id)
        {
            var jobname = from job in _db.Jobs
                               where job.Id == Id
                               select job.Position;
            var position = jobname.FirstOrDefault();
            return position;
        }


        public Job GetJob(int id)
        {
            return _db.Jobs.SingleOrDefault(j => j.Id == id);
        }

        public Job GetJobByOrganizationId(int id)
        {
            return _db.Jobs.SingleOrDefault(j => j.OrganizationId == id);
        }
        
        
        public IQueryable<Job> GetJobsByOrganizationId(int id)
        {
            return _db.Jobs.Where(j => j.OrganizationId == id);
        }

        public IQueryable<Job> GetJobsByConsultantId(int id)
        {
            return _db.Jobs.Where(j => j.ConsultantId == id);
        }

        public int AddJob(Job job)
        { 
            _db.Jobs.AddObject(job);
            Save();
            return job.Id;
        }

        public void AddJobSkill(JobSkill jobSkill)
        {
            _db.JobSkills.AddObject(jobSkill);
        }

        public void AddJobLanguage(JobLanguage jobLanguage)
        {
            _db.JobLanguages.AddObject(jobLanguage);
        }

        public void AddJobLicenseType(JobLicenseType jobLicenseType)
        {
            _db.JobLicenseTypes.AddObject(jobLicenseType);
        }

        public void AddJobPreferredIndustry(JobPreferredIndustry jobPreferredIndustry)
        {
            _db.JobPreferredIndustries.AddObject(jobPreferredIndustry);
        }

        public void AddJobLocation(JobLocation jobLocation)
        {
            _db.JobLocations.AddObject(jobLocation);
        }

        public void AddJobRequiredQualification(JobRequiredQualification jobRequiredQualification)
        {
            _db.JobRequiredQualifications.AddObject(jobRequiredQualification);
        }
        #endregion


        #region Consultant

        public Consultante GetConsulant(int id)
        {
            return _db.Consultantes.SingleOrDefault(c => c.Id == id);
        }

        public Candidate GetCandidateByConsultant(int consultantId, int candidateId)
        {
            return _db.Candidates.SingleOrDefault(c => c.ConsultantId == consultantId && c.Id == candidateId);
        }

        #endregion

        #region Candidate
        public Candidate GetCandidate(int id)
        {
            return _db.Candidates.SingleOrDefault(c => c.Id == id);
        }

        public Candidate GetCandidateByResumeFileName(string resumeFileName)
        {
            return _db.Candidates.SingleOrDefault(c => c.ResumeFileName == resumeFileName);
        }

        public Candidate GetCandidateByMobileNumber(string mobileNumber)
        {
            return _db.Candidates.SingleOrDefault(c => c.ContactNumber == mobileNumber);
        }

        public Candidate GetCandidateByEmail(string email)
        {
            return _db.Candidates.SingleOrDefault(c => c.Email == email);
        }

           

        public int AddCandidate(Candidate candidate)
        {
            _db.Candidates.AddObject(candidate);
            Save();
            return candidate.Id;
        }
             

        public void AddCandidateSkill(CandidateSkill candidateSkill)
        {
            _db.CandidateSkills.AddObject(candidateSkill);
        }

        public void AddCandidateLanguage(CandidateLanguage candidateLanguage)
        {
            _db.CandidateLanguages.AddObject(candidateLanguage);
        }


        public void AddCandidateCertification(CandidateCertification candidatecertification)
        {
            _db.CandidateCertifications.AddObject(candidatecertification);
        }
        
        public void AddCandidatePreferredFunction(CandidatePreferredFunction candidatePreferredFunction)
        {
            _db.CandidatePreferredFunctions.AddObject(candidatePreferredFunction);
        }

        //Role
        public void AddCandidatePreferredRole(CandidatePreferredRole candidatePreferredRole)
        {
            _db.CandidatePreferredRoles.AddObject(candidatePreferredRole);
        }

        public void AddCandidatePreferredLocation(CandidatePreferredLocation candidatePreferredLocation)
        {
            _db.CandidatePreferredLocations.AddObject(candidatePreferredLocation);
        }

        public void AddCandidateLicenseType(CandidateLicenseType candidateLicenseType)
        {
            _db.CandidateLicenseTypes.AddObject(candidateLicenseType);
        }

        public void AddCandidateQualification(CandidateQualification candidateQualification)
        {
            _db.CandidateQualifications.AddObject(candidateQualification);
        }

        public string GetCandidateNameById(int Id)
        {
            var candidate = from cand in _db.Candidates
                               where cand.Id == Id
                               select cand.Name;
            var candidateName = candidate.FirstOrDefault();
            return candidateName;
        }

        public string getEmployerNameByJobId(int Id)
        {
            var job= GetJob(Id);
            var employer = from emp in _db.Organizations
                           where emp.Id == job.OrganizationId
                           select emp.Name;

            var employerName = employer.FirstOrDefault();
            return employerName;
        }

        /*Get Employer Details*/
        public string getEmployerEmail(int Id)
        {
           // var job = GetJob(Id);
            var employer = from job in _db.Jobs
                           where job.Id == Id
                           select job.EmailAddress;

            var employerMobileNumber = employer.FirstOrDefault();
            return employerMobileNumber;
        }

        public string getEmployerMobileNumber(int Id)
        {
            //var job = GetJob(Id);
            var employer = from job in _db.Jobs
                           where job.Id == Id
                           select job.MobileNumber;

            var employerMobileNumber = employer.FirstOrDefault();
            return employerMobileNumber;
        }

        public string GetCandidateContactNumberById(int Id)
        {
            var candidate = from cand in _db.Candidates
                            where cand.Id == Id
                            select cand.ContactNumber;
            var candidateNumber = candidate.FirstOrDefault();
            return candidateNumber;
        }

        public string GetCandidateEmailById(int Id)
        {
            var candidate = from cand in _db.Candidates
                            where cand.Id == Id
                            select cand.Email;
            var candidateEmail = candidate.FirstOrDefault();
            return candidateEmail;
        }
        #endregion

        #region Consultant

        public string GetOrderByOrderId(int orderId)
        {
            var Orderdetail = from order in _db.OrderDetails
                            where order.OrderId == orderId
                            select order.RemainingCount;
            var remainingcount = Orderdetail.FirstOrDefault();
            return remainingcount.ToString();
        }

        #endregion
        public string GetConsultantNameById(int Id)
        {
            var consultant = from cons in _db.Consultantes
                               where cons.Id == Id
                               select cons.Name;
            var consultantName = consultant.FirstOrDefault();
            return consultantName;
        }
        #region organization

        public string GetOrganizationNameById(int Id)
        {
            var organization = from org in _db.Organizations
                               where org.Id == Id
                               select org.Name;
            var orgName = organization.FirstOrDefault();
            return orgName;
        }

        public Organization GetOrganizationById(int id)
        {
            return _db.Organizations.SingleOrDefault(c => c.Id == id);
        }

        public Organization GetOrganizationMobileNumber(string mobileNumber)
        {
            return _db.Organizations.SingleOrDefault(c => c.MobileNumber == mobileNumber);
        }

        public Organization GetOrganizationByUserName(string userName)
        {
            return _db.Organizations.SingleOrDefault(o => o.UserName == userName);
        }

        public Organization GetOrganizationByEmail(string email)
        {
            return _db.Organizations.SingleOrDefault(o => o.Email == email);
        }

        public Organization GetOrganizationByName(string Name)
        {
            return _db.Organizations.SingleOrDefault(o => o.Name == Name);
        }

        public Organization GetOrganizationByMobileCode(int mobileCode)
        {
            return _db.Organizations.SingleOrDefault(o => o.PhoneVerificationNo == mobileCode);
        }

        public int AddOrganization(Organization organization)
        {
            _db.Organizations.AddObject(organization);
            Save();
            return organization.Id;
        }

        public void DeleteOrganization(int organizationId)
        {
            var organization = from organizations in _db.Organizations
                               where organizations.Id == organizationId
                               select organizations;
            foreach (var organizationdelete in organization)
            {
                _db.Organizations.DeleteObject(organizationdelete);
            }
            _db.SaveChanges();
        }

        public Organization GetOrganization(int id)
        {
            return _db.Organizations.SingleOrDefault(o => o.Id == id);
        }
        public IQueryable<Organization> GetOrganizations()
        {
            return _db.Organizations;
        }

        #endregion

        #region Admin User

        public IEnumerable<User> GetUsers()
        {
            return _db.Users;
        }
        public User GetUserbyId(int id)
        {
            return _db.Users.Where(u => u.Id == id).SingleOrDefault();
        }
        

        public string GetAdminUserNamebyEntryIdAndEntryType(int entryId, EntryType entryType)
        {

            int adminId = _db.AdminUserEntries.Where(aue => aue.EntryId == entryId && aue.EntryType == (int)entryType).Select(au => au.AdminId).FirstOrDefault();
            if (adminId != null && adminId != 0)
            {
                return _db.Users.Where(u => u.Id == adminId).Select(us => us.UserName).FirstOrDefault();
            }
            else
                return "";
        }

        public int[] GetEntryIdsbyAdminIdAndEntryType(int adminId, EntryType entryType)
        {

            var EntryId = _db.AdminUserEntries.Where(aue => aue.AdminId == adminId && aue.EntryType == (int)entryType).Select(au => au.EntryId).ToList();

            int[] orgIds = new int[EntryId.Count()];

            for (int i = 0; i < EntryId.Count(); i++)
            {
                orgIds[i] = EntryId[i];
            }

            return orgIds;
        }
               

        public void AddAdminUser(User user)
        {
            _db.Users.AddObject(user);
        }

        public void DeleteAdminUser(int id)
        {
            var adminuser = _db.Users.Where(u => u.Id == id).SingleOrDefault();

            _db.Users.DeleteObject(adminuser);

            _db.SaveChanges();
        }

        public void DeleteAdminUserEntry(int adminId)
        {
            var adminUser = from adminUserEntry in _db.AdminUserEntries
                               where adminUserEntry.AdminId == adminId
                               select adminUserEntry;

            foreach (var adminuserEntry in adminUser)
            {
                _db.AdminUserEntries.DeleteObject(adminuserEntry);
            }
            _db.SaveChanges();
        }

        

        public void AddAdminUserEntry(AdminUserEntry adminuserentry)
        {
            _db.AdminUserEntries.AddObject(adminuserentry);
        }


        public void DeleteOrgByUserName(string userName)
        {
            var empUserName = _db.Organizations.Where(u => u.Name == userName).SingleOrDefault();

            _db.Organizations.DeleteObject(empUserName);
            _db.SaveChanges();
        }

       

        #endregion

        #region Delete

    
        public void DeleteOrganizationOrder(int organizationid)
        {

            var d = from om in _db.OrderMasters
                    where om.OrganizationId == organizationid
                    select om;

            foreach (var omdelete in d)
            {
                //_db.OrderDetails.DeleteObject(omdelete);
                _db.OrderMasters.DeleteObject(omdelete);
            }
            _db.SaveChanges();

        }


        public void DeleteJobs(int organizationId)
        {
            var job = from j in _db.Jobs
                      where j.OrganizationId == organizationId
                      select j.Id.ToString();

            if (job != null)
            {
                foreach (string id in job)
                {
                    DeleteJobLanguages(Convert.ToInt32(id));
                    DeleteJobLocations(Convert.ToInt32(id));
                    DeleteJobPreferredIndustries(Convert.ToInt32(id));
                    DeleteJobRequiredQualifications(Convert.ToInt32(id));
                    DeleteJobRoles(Convert.ToInt32(id));
                    DeleteJobskills(Convert.ToInt32(id));
                    DeleteJob(Convert.ToInt32(id));
                }
            }
            
            _db.SaveChanges();
        }

       
        public void DeleteCandidateQualifications(int candidateId)
        {
            var candidateQualifications = from cq in _db.CandidateQualifications
                                          where cq.CandidateId == candidateId
                                          select cq;
            if (candidateQualifications.Count() > 0)
            {
                foreach (var cq in candidateQualifications)
                {
                    _db.CandidateQualifications.DeleteObject(cq);
                }
                _db.SaveChanges();
            }
        }

       

        public void DeleteCandidatePreferredLocation(int locId)
        {
            var loc = from canloc in _db.CandidatePreferredLocations
                      where canloc.CandidateId == locId
                      select canloc;
            if (loc.Count() > 0)
            {
                foreach (var conlocDelete in loc)
                {
                    _db.CandidatePreferredLocations.DeleteObject(conlocDelete);
                }
                _db.SaveChanges();
            }
        }        


        public void DeleteCandidateLicenseTypes(int candidateId)
        {
            var candidateLicenseTypes = from clt in _db.CandidateLicenseTypes
                                              where clt.CandidateId == candidateId
                                              select clt;

            foreach (var clt in candidateLicenseTypes)
            {
                _db.CandidateLicenseTypes.DeleteObject(clt);
            }

            _db.SaveChanges();
        }

        public void DeleteCandidateSkills(int candidateId)
        {
            var c = from cs in _db.CandidateSkills
                    where cs.CandidateId == candidateId
                    select cs;
            if (c.Count() > 0)
            {
                foreach (var cskdelete in c)
                {
                    _db.CandidateSkills.DeleteObject(cskdelete);
                }
                _db.SaveChanges();
            }
        }

        public void DeleteCandidatePreferredFunctions(int cpfId)
        {
            var p = from cpf in _db.CandidatePreferredFunctions
                    where cpf.CandidateId == cpfId
                    select cpf;
            foreach (var cpfDelete in p)
            {
                _db.CandidatePreferredFunctions.DeleteObject(cpfDelete);
            }
            _db.SaveChanges();
        }

        public void DeleteCandidateOrder(int candidateId)
        {
            var d = from om in _db.OrderMasters
                    where om.CandidateId == candidateId
                    select om;

            foreach (var omdelete in d)
            {
                _db.OrderMasters.DeleteObject(omdelete);
            }
            _db.SaveChanges();
            
        }



        public void DeleteCandidateLanguages(int clId)
        {
            var l = from cpl in _db.CandidateLanguages
                    where cpl.CandidateId == clId
                    select cpl;
            if (l.Count() > 0)
            {
                foreach (var cplDelete in l)
                {
                    _db.CandidateLanguages.DeleteObject(cplDelete);
                }
                _db.SaveChanges();
            }
        }

        public void DeleteCandidateRoles(int rId)
        {
            var r = from cpr in _db.CandidatePreferredRoles
                    where cpr.CandidateId == rId
                    select cpr;
            if (r.Count() > 0)
            {
                foreach (var cprDelete in r)
                {
                    _db.CandidatePreferredRoles.DeleteObject(cprDelete);
                }
                _db.SaveChanges();
            }
        }

        public void DeleteCandidate(int candidateId)
        {
            var candidate = from candidates in _db.Candidates
                            where candidates.Id == candidateId
                            select candidates;
            foreach (var candidatedelete in candidate)
            {
                _db.Candidates.DeleteObject(candidatedelete);
            }
            _db.SaveChanges();
        }
             

        public void DeleteJob(int jobId)
        {
            var job = from jobs in _db.Jobs
                      where jobs.Id == jobId
                      select jobs;
            foreach (var jobdelete in job)
            {
                _db.Jobs.DeleteObject(jobdelete);
            }
            Save();
        }

        public void DeleteJobskills(int skillId)
        {
            var jobSkills = from jobSkill in _db.JobSkills
                            where jobSkill.JobId == skillId
                            select jobSkill;

            foreach (var jobSkill in jobSkills)
            {
                _db.JobSkills.DeleteObject(jobSkill);
            }

            _db.SaveChanges();
        }

        public void DeleteJobLanguages(int jobId)
        {
            var jobLanguages = from jobLanguage in _db.JobLanguages
                                where jobLanguage.JobId == jobId
                                select jobLanguage;

            foreach (var jobLanguage in jobLanguages)
            {
                _db.JobLanguages.DeleteObject(jobLanguage);
            }
            _db.SaveChanges();
        }

        public void DeleteAdminUserEntries(int userId, int entryId, int entryType)
        {
            var userEntries = from userEntry in _db.AdminUserEntries
                               where userEntry.EntryId == entryId && userEntry.AdminId==userId && userEntry.EntryType==entryType
                               select userEntry;

            foreach (var userentry in userEntries)
            {
                _db.AdminUserEntries.DeleteObject(userentry);
            }
            _db.SaveChanges();
        }

        public void DeleteJobPreferredIndustries(int jobId)
        {
            var industry = from jobindustry in _db.JobPreferredIndustries
                           where jobindustry.JobId == jobId
                           select jobindustry;
            foreach (var jobPreferredIndustryDelete in industry)
            {
                _db.JobPreferredIndustries.DeleteObject(jobPreferredIndustryDelete);
            }
            _db.SaveChanges();
        }

        public void DeleteJobLicenseTypes(int jobId)
        {

            var jobLicenseTypes = from jlt in _db.JobLicenseTypes
                                  where jlt.JobId == jobId
                                  select jlt;

            foreach (var jlt in jobLicenseTypes)
            {
                _db.JobLicenseTypes.DeleteObject(jlt);
            }
           
            _db.SaveChanges();
        }

        public void DeleteJobRoles(int jobId)
        {
            var role = from jobroles in _db.JobRoles
                       where jobroles.JobId == jobId
                       select jobroles;
            foreach (var jobPreferredRoleDelete in role)
            {
                _db.JobRoles.DeleteObject(jobPreferredRoleDelete);
            }
            _db.SaveChanges();
        }


        public void DeleteJobRequiredQualifications(int jobId)
        {

            var jobRequiredQualifications = from jq in _db.JobRequiredQualifications
                                            where jq.JobId == jobId
                                            select jq;

            foreach (var jq in jobRequiredQualifications)
            {
                _db.JobRequiredQualifications.DeleteObject(jq);
            }
            _db.SaveChanges();
        }

        public void DeleteJobLocations(int jobId)
        {
            var jobLocations = from jl in _db.JobLocations
                               where jl.JobId == jobId
                               select jl;

            foreach (var jl in jobLocations)
            {
                try
                {
                    _db.JobLocations.DeleteObject(jl);
                }
                catch(Exception e)
                {
                    
                }
            }

            _db.SaveChanges();
        }

        #endregion

        //Get Roles by functionid

        public IEnumerable<Role> GetRoleIdByFunctionId(int functionId)
        {
            List<Role> roleList = new List<Role>();
            var roles = from role in _db.Roles where role.FunctionId == functionId orderby role.Name ascending select role;

            foreach (var ct in roles)
            {
                roleList.Add(new Role() { Id = ct.Id });
            }

            return roleList;
        }


        #region Location
        public IQueryable<Country> GetCountries()
        {
            return from country in _db.Countries
                   orderby country.Name ascending
                   select country;
        }

        public IEnumerable<Country> GetCountriesList()
        {
            List<Country> countrylist = new List<Country>();

            var coun = (from country in _db.Countries
                        orderby country.Name ascending
                        select country);

            foreach (var d in coun)
                countrylist.Add(new Country() { Id = d.Id, Name = d.Name });

            return countrylist;
        }

        public IEnumerable<Country> GetOtherCountries(int Id)
        {
            List<Country> countrylist = new List<Country>();

            var coun = (from country in _db.Countries
                        where country.Id != Id
                        orderby country.Name ascending
                        select country);

            foreach (var d in coun)
                countrylist.Add(new Country() { Id = d.Id, Name = d.Name });

            return countrylist;

        }

        public IEnumerable<State> GetStateIdbyCountryId(int countryId)
        {
            List<State> statelist = new List<State>();
            var state = from stat in _db.States where stat.CountryId.Equals(countryId) orderby stat.Name ascending select stat;
            foreach (var st in state)
            {
                statelist.Add(new State() { Id = st.Id });
            }

            return statelist;
        }

        public IEnumerable<State> GetStatebyCountryIdwithCountryname(int countryId)
        {
            List<State> statelist = new List<State>();

            var countries = (from country in _db.Countries
                             where country.Id.Equals(countryId)
                             orderby country.Name ascending
                             select country);

            foreach (var cn in countries)
            {
                statelist.Add(new State() { Id = -1, Name = cn.Name });

                var state = from stat in _db.States where stat.CountryId.Equals(cn.Id) orderby stat.Name ascending select stat;
                foreach (var st in state)
                {
                    statelist.Add(new State() { Id = st.Id, Name = st.Name });
                }
            }

            return statelist;
        }

        public IEnumerable<City> GetCityIdbyStateId(int stateId)
        {
            List<City> citylist = new List<City>();
            var cities = from city in _db.Cities where city.StateId == stateId orderby city.Name ascending select city;

            foreach (var ct in cities)
            {
                citylist.Add(new City() { Id = ct.Id });
            }

            return citylist;
        }


       

        public IEnumerable<Region> GetRegionIdbyCityId(int cityId)
        {
            List<Region> regionlist = new List<Region>();

            var regions = from region in _db.Regions where region.CityId == cityId orderby region.Name ascending select region;

            foreach (var rg in regions)
            {
                regionlist.Add(new Region() { Id = rg.Id, Name = rg.Name });
            }

            return regionlist;
        }



        public IEnumerable<Region> GetRegionbyCityIdwithCityname(int cityId)
        {
            List<Region> regionlist = new List<Region>();

            var cities = from city in _db.Cities where city.Id == cityId orderby city.Name ascending select city;

            foreach (var ct in cities)
            {
                regionlist.Add(new Region() { Id = -1, Name = ct.Name });

                var regions = from region in _db.Regions where region.CityId == cityId orderby region.Name ascending select region;

                foreach (var rg in regions)
                {
                    regionlist.Add(new Region() { Id = rg.Id, Name = rg.Name });
                }
            }

            return regionlist;
        }

        public IQueryable<State> GetStates(int countryId)
        {
            return from state in _db.States
                   where state.CountryId == countryId
                   orderby state.Name ascending
                   select state;
        }

        

        public IQueryable<City> GetCities(int stateId)
        {
            return from city in _db.Cities
                   where city.StateId == stateId
                   orderby city.Name ascending
                   select city;
        }

        public IQueryable<Region> GetRegions(int cityId)
        {
            return from region in _db.Regions
                   where region.CityId == cityId
                   orderby region.Name ascending
                   select region;
        }

       
        public int AddLocation(Location location)
        {
                        
            //check if location already exists
            var loc = from dbLocation in _db.Locations
                      where dbLocation.CountryId == location.CountryId &&
                            dbLocation.StateId == location.StateId &&
                            dbLocation.CityId == location.CityId &&
                           dbLocation.RegionId == location.RegionId
                      select dbLocation;

            //add location if it doesn't exist
            if (loc.Count() == 0)
            {
                _db.Locations.AddObject(location);
                Save();
                return location.Id;
            }

            //return location.Id;
            return loc.First().Id;
        }

       
        public Location GetLocationById(int id)
        {
            return _db.Locations.SingleOrDefault(l => l.Id == id);
        }

        public CandidatePreferredRole GetRolesById(int candidateId)
        {
            return _db.CandidatePreferredRoles.SingleOrDefault(cpr => cpr.CandidateId == candidateId);
        }

        // To get the Exact function while entering the update profile.
        public int GetFunctionIdByCandidateId(int candidateId)
        {
            var function = _db.Candidates.Where(c => c.Id == candidateId && c.FunctionId !=null);
            int functionId = 0;

            if (function.Count() > 0)
                functionId = (int)function.Select(f => f.FunctionId).FirstOrDefault();
            else
                return 0;

            return functionId;
          
        }

        // To get the Exact function from Job while entering the update profile.
        public int GetFunctionidByJobId(int jobId)
        {
            var function = _db.Jobs.Where(c => c.Id == jobId && c.FunctionId != null);
            int functionId = 0;

            if (function.Count() > 0)
                functionId = (int)function.Select(f => f.FunctionId).FirstOrDefault();
            else
                return 0;

            return functionId;

        }

        public string GetCountryNameByJobId(int jobId)
        {
            var location = _db.JobLocations.Where(c => c.JobId == jobId);
            string countryName = "Any";

            if (location.Count() > 0)
            {
                countryName = location.Select(f => f.Location.Country.Name).FirstOrDefault();
                if (countryName != null)
                    return countryName;
                else
                    return "Any";
            }
            else
                return "Any";
            //return countryName.ToString();
        }

        public string GetCityNameByJobId(int jobId)
        {
            var location = _db.JobLocations.Where(c => c.JobId == jobId);
            string cityName = "";

            if (location.Count() > 0)
            {
                cityName = location.Select(f => f.Location.City.Name).FirstOrDefault();
                if (cityName != null)
                    return cityName.ToString();
                else
                    return "";

            }
            else
                return "";

           // return cityName.ToString();
        }

        public string GetRegionNameByJobId(int jobId)
        {
            var location = _db.JobLocations.Where(c => c.JobId == jobId);
            string regionName = "";

            if (location.Count() > 0)
            {
                regionName = location.Select(f => f.Location.Region.Name).FirstOrDefault();
                if (regionName != null)
                    return regionName.ToString();
                else
                    return "";
            }

            else
                return "";

            //return regionName.ToString();
        }


        /***************Get Name by Models (Candidate Advanced Reports)*****************/
        public string GetRoleByCandiateId(int candidateId)
        {
            return _db.CandidatePreferredRoles.FirstOrDefault(jr => jr.CandidateId == candidateId).Role.Name;
        }

        public string GetCountryNameByCandidateId(int candidateId)
        {
            var location = _db.CandidatePreferredLocations.Where(c => c.CandidateId == candidateId);
            string countryName = "Any";

            if (location.Count() > 0)
            {
                countryName = location.Select(f => f.Location.Country.Name).FirstOrDefault();
                if (countryName != null)
                    return countryName;
                else
                    return "Any";
            }
            else
                return "Any";
        }

        public string GetCityNameByCandidateId(int candidateId)
        {
            var location = _db.CandidatePreferredLocations.Where(c => c.CandidateId == candidateId);
            string cityName = "";

            if (location.Count() > 0)
            {
                cityName = location.Select(f => f.Location.City.Name).FirstOrDefault();
                if (cityName != null)
                    return cityName.ToString();
                else
                    return "";

            }
            else
                return "";

            // return cityName.ToString();
        }

        public string GetRegionNameByCandidateId(int candidateId)
        {
            var location = _db.CandidatePreferredLocations.Where(c => c.CandidateId == candidateId);
            string regionName = "";

            if (location.Count() > 0)
            {
                regionName = location.Select(f => f.Location.Region.Name).FirstOrDefault();
                if (regionName != null)
                    return regionName.ToString();
                else
                    return "";
            }

            else
                return "";

            //return regionName.ToString();
        }

        /***************End*****************/
        

        /*Get Function Name by FunctionId(For VacancyReports)*/
      

        public string GetFunctionNameByFunctionId(int functionId)
        {
            return _db.Functions.FirstOrDefault(f => f.Id == functionId).Name;
        }

        public string GetRoleNameByJobId(int jobId)
        {
            return _db.JobRoles.FirstOrDefault(jr => jr.JobId == jobId).Role.Name;
        }


        public JobRole GetRolesByJobId(int jobId)
        {
            return _db.JobRoles.SingleOrDefault(jr => jr.JobId == jobId);
        }

        public IEnumerable<Location> GetLocationsbyJobId(int JobId)
        {
            List<Location> locationlist = new List<Location>();

            var locations = (from location in _db.Locations
                             //where location.Id ==  location.JobLocations.Where(jbl => jbl.JobId == JobId).Select(jl => jl.LocationId) 
                             where _db.JobLocations.Where(jl => jl.JobId == JobId).Select(j => j.LocationId).Contains(location.Id)
                             select location);

            foreach (var loc in locations)
                locationlist.Add(new Location() { Id = loc.Id, CountryId = loc.CountryId, StateId = loc.StateId, CityId = loc.CityId, RegionId = loc.RegionId });

            return locationlist;
        }


        public JobSkill GetJobSkillById(int id)
        {
            return _db.JobSkills.SingleOrDefault(js => js.Id == id);
        }

      
        public int GetEmployerId(string userName)
        {
            try
            {
                var empId = from emp in _db.Organizations where emp.UserName == userName select emp.Id;

                return empId.FirstOrDefault();
            }
            catch
            {
                return 0;
            }

        }

        public int GetCandidateId(string userName)
        {
            try
            {
                var candId = from cand in _db.Candidates where cand.UserName == userName select cand.Id;

                return candId.FirstOrDefault();
            }
            catch
            {
                return 0;
            }

        }

      
        
        public int GetRoleId(string role) 
        {
            var roleId = from roles in _db.Roles where roles.Name == role select roles.Id;
            return roleId.FirstOrDefault();
        }

        //Titles
        public List<string> GetTitlesForFindJobs()
        {
            var functions = from funcs in _db.FunctionsForFindJobs orderby funcs.Title ascending select funcs.Title;
            return functions.ToList();
        }
        //Functions
        public List<string> GetFunctionsForFindJobs()
        {
            var functions = from funcs in _db.FunctionsForFindJobs orderby funcs.Title ascending select funcs.Functions;
            return functions.ToList();
        }

        //Roles
        public List<string> GetRolesForFindJobseekers()
        {
            var lstRoles = from roles in _db.Roles orderby roles.Name ascending where roles.HomeFlag == "Home" select roles.Name;
            return lstRoles.ToList();
        }

  

        //Get titles for roles
        public List<string> GetTitlesForFindJobseekers()
        {
            var roles = from role in _db.RolesForFindJobseekers orderby role.RoleName ascending select role.RoleName;
            return roles.ToList();
        }


        //functions from roles
        public List<string> GetFunctionsForRolesFindJobSeekers()
        {
            var lstRoles= from roles in _db.RolesForFindJobseekers orderby roles.RoleName ascending  select roles.Functions;
            return lstRoles.ToList();
        }

          
        public int GetExistsLocations(int locationId)
        {
            
                var count = from erv in _db.CandidatePreferredLocations where erv.LocationId == locationId select erv.LocationId;

                if (count.Count() > 0)
                {
                    return count.First();
                }
                return locationId;
            
        }

       
       public IEnumerable<int> Series(int k = 0, int n = 1, int c = 1)
        {
            while (true)
            {
                yield return k;
                k = (c * k) + n;
            }
        }

        public void ResetCounts()
        {
            try
            {
                _db.ExecuteStoreCommand("TRUNCATE TABLE EmployerResumeViews");
            }
            catch
            {
                return;
            }
        }

        

        #endregion

        public void Save()
        {
            _db.SaveChanges();
        }

        #endregion

        #region manage channel partner

        public IQueryable<ChannelPartner> GetChannelPartners()
        {
            return _db.ChannelPartners;
        }

        public IEnumerable<ChannelPartner> GetChannelPartnersbyUserName(string UserName)
        {
            return _db.ChannelPartners.Where(u => u.UserName == UserName);
        }

        public void AddChannelPartner(ChannelPartner channelpartner)
        {
            _db.ChannelPartners.AddObject(channelpartner);
        }

        public ChannelPartner GetChannelPartner(int id)
        {
            return _db.ChannelPartners.Where(ch => ch.Id == id).FirstOrDefault();
        }

        public void DeleteChannelPartner(int id)
        {
            var channelpartner = _db.ChannelPartners.Where(u => u.Id == id).SingleOrDefault();

            _db.ChannelPartners.DeleteObject(channelpartner);

            _db.SaveChanges();
        }

        #endregion manage channel partner

    }
}