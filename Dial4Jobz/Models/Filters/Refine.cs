using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Filters
{
    public sealed class Refine
    {
        public string RefineSalary { get; set; }
        public string[] RefineOrganization { get; set; }
        public string RefineExp { get; set; }
        public string[] RefineFunction { get; set; }
        public string[] RefineRoles { get; set; }
        public string[] RefinePosition { get; set; }
        public string RefineGender { get; set; }

        public string HfWhat { get; set; }
        public string HfLanguages { get; set; }
        public string HfWhere { get; set; }
        public string HfMinSalary { get; set; }
        public string HfMaxSalary { get; set; }
        public string HfOrganization { get; set; }
        public string HfMinExperience { get; set; }
        public string HfMaxExperience { get; set; }
        public string HfFunction { get; set; }
        public string HfIndustries { get; set; }
        public string HfTypeOfVacancy { get; set; }
        public string HfWorkShift { get; set; }
        public string HfFreshness { get; set; }

        public string HfCurrentLocation { get; set; }
        public string HfPrefLocation { get; set; }
        public string HfRoles { get; set; }
        public string HfBasicQual { get; set; }
        public string HfPostGraduation { get; set; }
        public string HfDoctrate { get; set; }

        //for tracking specialization
        public string HfBasicSpecialization { get; set; }
        public string HfPostSpecialization { get; set; }
        public string HfDoctrateSpecialization { get; set; }

        public string HfMinAge { get; set; }
        public string HfMaxAge { get; set; }

        public string HfPosition { get; set; }
        public string HfGender { get; set; }
        public string HfAndOrLocations { get; set; }

        ////preferred type

        //public string HfAll { get; set; }
        //public string HfPreferredContract { get; set; }
        //public string HfPreferredPartTime { get; set; }
        //public string HfPreferredFullTime { get; set; }
        //public string HfPreferredWorkFromHome { get; set; }
        
        
    }
}
