using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Repositories
{
    public interface IRepository
    {
        IQueryable<Job> GetMatchingJobs(Candidate candidate);
        IQueryable<Candidate> GetMatchingCandidates(Job job);
        //IQueryable<JobSkill> GetJobsBySkill(int skillId);

        IQueryable<Country> GetCountries();
        IQueryable<State> GetStates(int countryId);
        IQueryable<City> GetCities(int stateId);
        IQueryable<Region> GetRegions(int cityId);

        IQueryable<Function> GetFunctions(string q);
        IQueryable<Language> GetLanguages(string q);
        IQueryable<Skill> GetSkills(string q);
        IQueryable<Function> GetFunctions();
        IQueryable<Industry> GetIndustries();
        IQueryable<Industry> GetIndustries(string q);

        IQueryable<Role> GetRoles(int functionId);
        
        int AddCandidate(Candidate candidate);
        int AddLocation(Location location);
        
        void Save();
        

    }   
}