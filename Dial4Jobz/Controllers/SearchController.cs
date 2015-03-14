using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Helpers;
using System.IO;

namespace Dial4Jobz.Controllers
{
    public class SearchController : BaseController
    {
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();

        const int PAGE_SIZE = 15;
        List<string> _filters = new List<string>();

        public JsonResult getSkillData(string term)
        {
            var result = _repository.GetSkillList(term).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSkillsAlone(string term)
        {
            var result = _repository.GetSkillsAlone(term).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLanguagesAlone(string term)
        {
            var result = _repository.GetLanguagesAlone(term).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getData(string term)
        {
            var result = _repository.GetCityList(term).ToList();
            
            return Json(result, JsonRequestBehavior.AllowGet);            
        }
        
        public JsonResult getGroupCountryStateCity()
        {
            var result = new
                {
                    location = _repository.GetGroupCountryStateCityList().ToList()
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getCitybyStateId(Int32 Id)
        {
            var result = new
                {
                    location = _repository.GetCitybyStateId(Id).ToList()
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getStatebyCountryId(Int32 Id)
        {
            var result = new
                {
                    State = _repository.GetStatebyCountryId(Id).ToList()
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        } 

        public JsonResult getIndustries()
        {
            var result = new
                {
                    industry = _repository.GetIndustriesEnumerable().ToList()
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
        
        public JsonResult GetSpecialization(Int32 id)
        {
            var result = new
                             {
                                 Specialization =
                                     _repository.GetSpecializationWithDegreeNameByDegreeId(id).Select(
                                         i => new {  i.Id,  i.Name} )
                             };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CandidateSearch()
        {           
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 30; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }


            var minExp = totalExperienceYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minExp.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });

            var maxExp = totalExperienceYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            maxExp.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });

            ViewData["MinExperienceYears"] = new SelectList(minExp, "Value", "Text");
            ViewData["MaxExperienceYears"] = new SelectList(maxExp, "Value", "Text");

            List<DropDownItem> totalAgeYears = new List<DropDownItem>();
            for (int i = 18; i <= 65; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalAgeYears.Add(item);
            }

            var minage = totalAgeYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minage.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });

            var maxage = totalAgeYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            maxage.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });

            ViewData["MinAgeYears"] = new SelectList(minage, "Value", "Text");
            ViewData["MaxAgeYears"] = new SelectList(maxage, "Value", "Text");

            //salary
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

            List<DropDownItem> annualSalaryThousands = new List<DropDownItem>();
            for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                annualSalaryThousands.Add(item);
            }

            var minSalthou = annualSalaryThousands.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minSalthou.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });
            ViewData["MinAnnualSalaryThousands"] = new SelectList(minSalthou, "Value", "Text");

            var maxSalthou = annualSalaryThousands.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            maxSalthou.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });
            ViewData["MaxAnnualSalaryThousands"] = new SelectList(maxSalthou, "Value", "Text");  

            return View();            
        }

        public ActionResult CandidateResults(string PageNo,int id)
        {
            var job = _repository.GetJob(id);
            if (job == null)
                return new FileNotFoundResult();
            IQueryable<Candidate> Candidates = _repository.GetMatchingCandidates(job);
            
            //var Candidates = _repository.GetCandidates();
            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = Candidates.Count();

            var CandidateResults = Candidates.Skip(skip).Take(take);

            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(CandidateResults);
        }

        //[HttpPost]
        //public ActionResult CandidateResults(Refine refine, string what, string MinExperienceYears, string MaxExperienceYears, string MinAnnualSalaryLakhs, string MinAnnualSalaryThousands, string MaxAnnualSalaryLakhs, string MaxAnnualSalaryThousands, string[] MetroCity, string[] PrefMetroCity, string[] Locality, string AndOrLocations, string[] PrefLocality, string[] PrefIndustry, string[] FunctionalArea, string[] Role, string[] basicQualification, string[] PostGraduation, string[] Doctrate, string[] BasicQualificationSpecialization, string[] PostGraduationSpecialization, string[] DoctrateSpecialization, string MinAge, string MaxAge, string PageNo)
        //{
        //    if (!string.IsNullOrEmpty(what))
        //    {
        //        what = what.TrimEnd(',');
        //        ViewData.Add("WhatView", what);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfWhat))
        //    {
        //        what = refine.HfWhat;
        //        ViewData.Add("WhatView", what);
        //    }
        //    else
        //    {
        //        what = string.Empty;
        //    }

        //    string minExperience = string.Empty;
        //    string maxExperience = string.Empty;

        //    if (!string.IsNullOrEmpty(refine.RefineExp))
        //    {
        //        string[] RefineExp = refine.RefineExp.Split('-');
        //        if (RefineExp.Length > 1)
        //        {
        //            minExperience = (Convert.ToInt64(RefineExp[0]) * 365 * 24 * 60 * 60).ToString();
        //            maxExperience = (Convert.ToInt64(RefineExp[1]) * 365 * 24 * 60 * 60).ToString();
        //            ViewData.Add("MinExperienceView", minExperience);
        //            ViewData.Add("MaxExperienceView", maxExperience);
        //            ViewData.Add("RefineExperienceView", RefineExp[0]);
        //        }
        //    }
        //    else if (!string.IsNullOrEmpty(MinExperienceYears))
        //    {
        //        minExperience = (Convert.ToInt64(MinExperienceYears) * 365 * 24 * 60 * 60).ToString();
        //        maxExperience = (Convert.ToInt64(MaxExperienceYears) * 365 * 24 * 60 * 60).ToString();
        //        ViewData.Add("MinExperienceView", minExperience);
        //        ViewData.Add("MaxExperienceView", maxExperience);

        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfMinExperience))
        //    {
        //        minExperience = refine.HfMinExperience;
        //        maxExperience = refine.HfMaxExperience;
        //        ViewData.Add("MinExperienceView", minExperience);
        //        ViewData.Add("MaxExperienceView", maxExperience);
        //    }

        //    string minannualSalaryLakhs = string.Empty;
        //    string maxannualSalaryLakhs = string.Empty;

        //    if (!string.IsNullOrEmpty(refine.RefineSalary))
        //    {
        //        string[] RefineSalary = refine.RefineSalary.Split('-');
        //        if (RefineSalary.Length > 1)
        //        {
        //            minannualSalaryLakhs = RefineSalary[0];
        //            maxannualSalaryLakhs = RefineSalary[1];
        //            ViewData.Add("MinSalaryView", minannualSalaryLakhs);
        //            ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
        //            ViewData.Add("RefineSalaryView", minannualSalaryLakhs);
        //        }
        //    }
        //    else if (!string.IsNullOrEmpty(MinAnnualSalaryLakhs) || !string.IsNullOrEmpty(MinAnnualSalaryThousands))
        //    {
        //        minannualSalaryLakhs = (Convert.ToInt32(MinAnnualSalaryLakhs == "" ? "0" : MinAnnualSalaryLakhs) * 100000 + Convert.ToInt32(MinAnnualSalaryThousands == "" ? "0" : MinAnnualSalaryThousands) * 1000).ToString();
        //        maxannualSalaryLakhs = (Convert.ToInt32(MaxAnnualSalaryLakhs == "" ? "0" : MaxAnnualSalaryLakhs) * 100000 + Convert.ToInt32(MaxAnnualSalaryThousands == "" ? "0" : MaxAnnualSalaryThousands) * 1000).ToString();
        //        ViewData.Add("MinSalaryView", minannualSalaryLakhs);
        //        ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfMinSalary))
        //    {
        //        minannualSalaryLakhs = refine.HfMinSalary;
        //        maxannualSalaryLakhs = refine.HfMaxSalary;
        //        ViewData.Add("MinSalaryView", minannualSalaryLakhs);
        //        ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
        //    }

        //    string CurrentLocation = string.Empty;
        //    string metrocities = string.Empty;
        //    if (MetroCity != null && MetroCity.Length > 0)
        //    {
        //        metrocities = String.Join(",", MetroCity);
        //        CurrentLocation = metrocities;
        //        if (Locality != null && Locality.Length > 0)
        //        {
        //        }
        //        else
        //        {
        //            ViewData.Add("CurrentLocationView", CurrentLocation);
        //        }
        //    }

        //    if (Locality != null && Locality.Length > 0)
        //    {
        //        CurrentLocation = String.Join(",", Locality);
        //        if (!string.IsNullOrEmpty(metrocities))
        //        {
        //            CurrentLocation = CurrentLocation + "," + metrocities;
        //        }
        //        ViewData.Add("CurrentLocationView", CurrentLocation);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfCurrentLocation))
        //    {
        //        CurrentLocation = refine.HfCurrentLocation;
        //        ViewData.Add("CurrentLocationView", CurrentLocation);
        //    }


        //    if (!string.IsNullOrEmpty(refine.HfAndOrLocations))
        //    {
        //        AndOrLocations = refine.HfAndOrLocations;
        //        ViewData.Add("AndOrLocationsView", AndOrLocations);
        //    }
        //    else if (!string.IsNullOrEmpty(AndOrLocations))
        //    {
        //        ViewData.Add("AndOrLocationsView", AndOrLocations);
        //    }

        //    string PreferredLocation = string.Empty;
        //    string prefmetrocities = string.Empty;
        //    if (PrefMetroCity != null && PrefMetroCity.Length > 0)
        //    {
        //        prefmetrocities = String.Join(",", PrefMetroCity);
        //        PreferredLocation = prefmetrocities;
        //        if (PrefLocality != null && PrefLocality.Length > 0)
        //        {
        //        }
        //        else
        //        {
        //            ViewData.Add("PreferredLocationView", PreferredLocation);
        //        }
        //    }

        //    if (PrefLocality != null && PrefLocality.Length > 0)
        //    {
        //        PreferredLocation = String.Join(",", PrefLocality);
        //        if (!string.IsNullOrEmpty(prefmetrocities))
        //        {
        //            PreferredLocation = PreferredLocation + "," + prefmetrocities;
        //        }
        //        ViewData.Add("PreferredLocationView", PreferredLocation);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfPrefLocation))
        //    {
        //        PreferredLocation = refine.HfPrefLocation;
        //        ViewData.Add("PreferredLocationView", PreferredLocation);
        //    }

        //    string Function = string.Empty;
        //    if (refine.RefineFunction != null && refine.RefineFunction.Length > 0)
        //    {
        //        Function = String.Join(",", refine.RefineFunction);
        //        ViewData.Add("FunctionView", Function);
        //    }
        //    else if (FunctionalArea != null && FunctionalArea.Length > 0)
        //    {
        //        Function = String.Join(",", FunctionalArea);
        //        ViewData.Add("FunctionView", Function);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfFunction))
        //    {
        //        Function = refine.HfFunction;
        //        ViewData.Add("FunctionView", Function);
        //    }

        //    string roles = string.Empty;
        //    if (Role != null && Role.Length > 0)
        //    {
        //        roles = String.Join(",", Role);
        //        ViewData.Add("RolesView", roles);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfRoles))
        //    {
        //        roles = refine.HfRoles;
        //        ViewData.Add("RolesView", roles);
        //    }

        //    string Industry = string.Empty;
        //    if (PrefIndustry != null && PrefIndustry.Length > 0)
        //    {
        //        Industry = String.Join(",", PrefIndustry);
        //        ViewData.Add("IndustryView", Industry);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfIndustries))
        //    {
        //        Industry = refine.HfIndustries;
        //        ViewData.Add("IndustryView", Industry);
        //    }

        //    string BasicQual = string.Empty;
        //    if (basicQualification != null && basicQualification.Length > 0)
        //    {
        //        BasicQual = String.Join(",", basicQualification);
        //        ViewData.Add("BasicQualView", BasicQual);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfBasicQual))
        //    {
        //        BasicQual = refine.HfBasicQual;
        //        ViewData.Add("BasicQualView", BasicQual);
        //    }

        //    string postGraduations = string.Empty;
        //    if (PostGraduation != null && PostGraduation.Length > 0)
        //    {
        //        postGraduations = String.Join(",", PostGraduation);
        //        ViewData.Add("PostGraduationView", postGraduations);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfPostGraduation))
        //    {
        //        postGraduations = refine.HfPostGraduation;
        //        ViewData.Add("PostGraduationView", postGraduations);
        //    }


        //    string doctrates = string.Empty;
        //    if (Doctrate != null && Doctrate.Length > 0)
        //    {
        //        doctrates = String.Join(",", Doctrate);
        //        ViewData.Add("DoctratesView", doctrates);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfDoctrate))
        //    {
        //        doctrates = refine.HfDoctrate;
        //        ViewData.Add("DoctratesView", doctrates);
        //    }

        //    //for searching specialization
        //    var basicSpec = BasicQualificationSpecialization != null && BasicQualificationSpecialization.Length > 0
        //                        ? string.Join(",", BasicQualificationSpecialization)
        //                        : refine.HfBasicSpecialization;
        //    ViewData.Add("BasicSpecView", basicSpec);

        //    var postSpec = PostGraduationSpecialization != null && PostGraduationSpecialization.Length > 0
        //                       ? string.Join(",", PostGraduationSpecialization)
        //                       : refine.HfPostSpecialization;
        //    ViewData.Add("PostSpecView", postSpec);

        //    var doctrateSpec = DoctrateSpecialization != null && DoctrateSpecialization.Length > 0
        //                           ? string.Join(",", DoctrateSpecialization)
        //                           : refine.HfDoctrateSpecialization;
        //    ViewData.Add("DoctrateSpecView", doctrateSpec);

        //    if (!string.IsNullOrEmpty(MinAge))
        //    {
        //        ViewData.Add("MinAgeView", MinAge);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfMinAge))
        //    {
        //        MinAge = refine.HfMinAge;
        //        ViewData.Add("MinAgeView", MinAge);
        //    }
        //    else
        //    {
        //        MinAge = string.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(MaxAge))
        //    {
        //        ViewData.Add("MaxAgeView", MaxAge);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfMaxAge))
        //    {
        //        MaxAge = refine.HfMaxAge;
        //        ViewData.Add("MaxAgeView", MaxAge);
        //    }
        //    else
        //    {
        //        MaxAge = string.Empty;
        //    }

        //    string position = string.Empty;
        //    if (refine.RefinePosition != null && refine.RefinePosition.Length > 0)
        //    {
        //        position = String.Join(",", refine.RefinePosition);
        //        ViewData.Add("PositionView", position);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfPosition))
        //    {
        //        position = refine.HfPosition;
        //        ViewData.Add("PositionView", position);
        //    }

        //    string gender = string.Empty;
        //    if (!string.IsNullOrEmpty(refine.RefineGender))
        //    {
        //        gender = refine.RefineGender;
        //        ViewData.Add("GenderView", gender);
        //    }
        //    else if (!string.IsNullOrEmpty(refine.HfGender))
        //    {
        //        gender = refine.HfGender;
        //        ViewData.Add("GenderView", gender);
        //    }

        //    var Candidates = _repository.GetCandidateSearch(what, minExperience, maxExperience, minannualSalaryLakhs, maxannualSalaryLakhs, CurrentLocation, AndOrLocations, PreferredLocation, Function, roles, Industry, BasicQual, postGraduations, doctrates,basicSpec, postSpec, doctrateSpec, MinAge, MaxAge, position, gender);

        //    int page;
        //    if (string.IsNullOrEmpty(PageNo))
        //        page = 1;
        //    else
        //        page = Convert.ToInt32(PageNo);

        //    int pageSize = 15;
        //    int skip = (page - 1) * pageSize;

        //    //number of results per page. 
        //    int take = pageSize;

        //    var RecordCount = Candidates.Count();

        //    var CandidateResults = Candidates.Skip(skip).Take(take);

        //    ViewData.Add("PageIndex", page);
        //    ViewData.Add("RecordCount", RecordCount);

        //    return View(CandidateResults);

        //}

        [HttpPost]
        [Authorize]
        public ActionResult CandidateResults(Refine refine, string what, string language, string MinExperienceYears, string MaxExperienceYears, string MinAnnualSalaryLakhs, string MinAnnualSalaryThousands, string MaxAnnualSalaryLakhs, string MaxAnnualSalaryThousands, string[] MetroCity, string[] PrefMetroCity, string[] Locality, string AndOrLocations, string[] PrefLocality, string[] PrefIndustry, string[] FunctionalArea, string[] Role, string[] basicQualification, string[] PostGraduation, string[] Doctrate, string[] BasicQualificationSpecialization, string[] PostGraduationSpecialization, string[] DoctrateSpecialization, string MinAge, string MaxAge, string PageNo, string[] TypeOfVacancy, string[] TypeOfWorkShift)
        {
            if (!string.IsNullOrEmpty(what))
            {
                what = what.TrimEnd(',');
                ViewData.Add("WhatView", what);
            }
            else if (!string.IsNullOrEmpty(refine.HfWhat))
            {
                what = refine.HfWhat;
                ViewData.Add("WhatView", what);
            }
            else
            {
                what = string.Empty;
            }

            //if (!string.IsNullOrEmpty(all))
            //{
            //    all = all.TrimEnd(',');
            //    ViewData.Add("AllView", all);
            //}


            if (!string.IsNullOrEmpty(language))
            {
                language = language.TrimEnd(',');
                ViewData.Add("LanguageView", language);
            }

            else if (!string.IsNullOrEmpty(refine.HfLanguages))
            {
                language = refine.HfLanguages;
                ViewData.Add("LanguageView", language);
            }
            else
            {
                language = string.Empty;
            }

            string minExperience = string.Empty;
            string maxExperience = string.Empty;

            if (!string.IsNullOrEmpty(refine.RefineExp))
            {
                string[] RefineExp = refine.RefineExp.Split('-');
                if (RefineExp.Length > 1)
                {
                    minExperience = (Convert.ToInt64(RefineExp[0]) * 365 * 24 * 60 * 60).ToString();
                    maxExperience = (Convert.ToInt64(RefineExp[1]) * 365 * 24 * 60 * 60).ToString();
                    ViewData.Add("MinExperienceView", minExperience);
                    ViewData.Add("MaxExperienceView", maxExperience);
                    ViewData.Add("RefineExperienceView", RefineExp[0]);
                }
            }
            else if (!string.IsNullOrEmpty(MinExperienceYears))
            {
                minExperience = (Convert.ToInt64(MinExperienceYears) * 365 * 24 * 60 * 60).ToString();
                maxExperience = (Convert.ToInt64(string.IsNullOrEmpty(MaxExperienceYears) ? (Convert.ToInt32(MinExperienceYears) + 1).ToString() : MaxExperienceYears) * 365 * 24 * 60 * 60).ToString();
                ViewData.Add("MinExperienceView", minExperience);
                ViewData.Add("MaxExperienceView", maxExperience);

            }
            else if (!string.IsNullOrEmpty(refine.HfMinExperience))
            {
                minExperience = refine.HfMinExperience;
                maxExperience = refine.HfMaxExperience;
                ViewData.Add("MinExperienceView", minExperience);
                ViewData.Add("MaxExperienceView", maxExperience);
            }

            string minannualSalaryLakhs = string.Empty;
            string maxannualSalaryLakhs = string.Empty;

            if (!string.IsNullOrEmpty(refine.RefineSalary))
            {
                string[] RefineSalary = refine.RefineSalary.Split('-');
                if (RefineSalary.Length > 1)
                {
                    minannualSalaryLakhs = RefineSalary[0];
                    maxannualSalaryLakhs = RefineSalary[1];
                    ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                    ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
                    ViewData.Add("RefineSalaryView", minannualSalaryLakhs);
                }
            }
            else if (!string.IsNullOrEmpty(MinAnnualSalaryLakhs) || !string.IsNullOrEmpty(MinAnnualSalaryThousands))
            {
                minannualSalaryLakhs = (Convert.ToInt32(MinAnnualSalaryLakhs == "" ? "0" : MinAnnualSalaryLakhs) * 100000 + Convert.ToInt32(MinAnnualSalaryThousands == "" ? "0" : MinAnnualSalaryThousands) * 1000).ToString();
                maxannualSalaryLakhs = (Convert.ToInt32(MaxAnnualSalaryLakhs == "" ? "0" : MaxAnnualSalaryLakhs) * 100000 + Convert.ToInt32(MaxAnnualSalaryThousands == "" ? "0" : MaxAnnualSalaryThousands) * 1000).ToString();
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }
            else if (!string.IsNullOrEmpty(refine.HfMinSalary))
            {
                minannualSalaryLakhs = refine.HfMinSalary;
                maxannualSalaryLakhs = refine.HfMaxSalary;
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }

            string CurrentLocation = string.Empty;
            string metrocities = string.Empty;
            if (MetroCity != null && MetroCity.Length > 0)
            {
                metrocities = String.Join(",", MetroCity);
                CurrentLocation = metrocities;
                if (Locality != null && Locality.Length > 0)
                {
                }
                else
                {
                    ViewData.Add("CurrentLocationView", CurrentLocation);
                }
            }

            if (Locality != null && Locality.Length > 0)
            {
                CurrentLocation = String.Join(",", Locality);
                if (!string.IsNullOrEmpty(metrocities))
                {
                    CurrentLocation = CurrentLocation + "," + metrocities;
                }
                ViewData.Add("CurrentLocationView", CurrentLocation);
            }
            else if (!string.IsNullOrEmpty(refine.HfCurrentLocation))
            {
                CurrentLocation = refine.HfCurrentLocation;
                ViewData.Add("CurrentLocationView", CurrentLocation);
            }


            if (!string.IsNullOrEmpty(refine.HfAndOrLocations))
            {
                AndOrLocations = refine.HfAndOrLocations;
                ViewData.Add("AndOrLocationsView", AndOrLocations);
            }
            else if (!string.IsNullOrEmpty(AndOrLocations))
            {
                ViewData.Add("AndOrLocationsView", AndOrLocations);
            }

            string PreferredLocation = string.Empty;
            string prefmetrocities = string.Empty;
            if (PrefMetroCity != null && PrefMetroCity.Length > 0)
            {
                prefmetrocities = String.Join(",", PrefMetroCity);
                PreferredLocation = prefmetrocities;
                if (PrefLocality != null && PrefLocality.Length > 0)
                {
                }
                else
                {
                    ViewData.Add("PreferredLocationView", PreferredLocation);
                }
            }

            if (PrefLocality != null && PrefLocality.Length > 0)
            {
                PreferredLocation = String.Join(",", PrefLocality);
                if (!string.IsNullOrEmpty(prefmetrocities))
                {
                    PreferredLocation = PreferredLocation + "," + prefmetrocities;
                }
                ViewData.Add("PreferredLocationView", PreferredLocation);
            }
            else if (!string.IsNullOrEmpty(refine.HfPrefLocation))
            {
                PreferredLocation = refine.HfPrefLocation;
                ViewData.Add("PreferredLocationView", PreferredLocation);
            }

            string Function = string.Empty;
            if (refine.RefineFunction != null && refine.RefineFunction.Length > 0)
            {
                Function = String.Join(",", refine.RefineFunction);
                ViewData.Add("FunctionView", Function);
            }
            else if (FunctionalArea != null && FunctionalArea.Length > 0)
            {
                Function = String.Join(",", FunctionalArea);
                ViewData.Add("FunctionView", Function);
            }
            else if (!string.IsNullOrEmpty(refine.HfFunction))
            {
                Function = refine.HfFunction;
                ViewData.Add("FunctionView", Function);
            }

            string roles = string.Empty;
            if (Role != null && Role.Length > 0)
            {
                roles = String.Join(",", Role);
                ViewData.Add("RolesView", roles);
            }
            else if (!string.IsNullOrEmpty(refine.HfRoles))
            {
                roles = refine.HfRoles;
                ViewData.Add("RolesView", roles);
            }

            string Industry = string.Empty;
            if (PrefIndustry != null && PrefIndustry.Length > 0)
            {
                Industry = String.Join(",", PrefIndustry);
                ViewData.Add("IndustryView", Industry);
            }
            else if (!string.IsNullOrEmpty(refine.HfIndustries))
            {
                Industry = refine.HfIndustries;
                ViewData.Add("IndustryView", Industry);
            }

            string BasicQual = string.Empty;
            if (basicQualification != null && basicQualification.Length > 0)
            {
                BasicQual = String.Join(",", basicQualification);
                ViewData.Add("BasicQualView", BasicQual);
            }
            else if (!string.IsNullOrEmpty(refine.HfBasicQual))
            {
                BasicQual = refine.HfBasicQual;
                ViewData.Add("BasicQualView", BasicQual);
            }

            string postGraduations = string.Empty;
            if (PostGraduation != null && PostGraduation.Length > 0)
            {
                postGraduations = String.Join(",", PostGraduation);
                ViewData.Add("PostGraduationView", postGraduations);
            }
            else if (!string.IsNullOrEmpty(refine.HfPostGraduation))
            {
                postGraduations = refine.HfPostGraduation;
                ViewData.Add("PostGraduationView", postGraduations);
            }


            string doctrates = string.Empty;
            if (Doctrate != null && Doctrate.Length > 0)
            {
                doctrates = String.Join(",", Doctrate);
                ViewData.Add("DoctratesView", doctrates);
            }
            else if (!string.IsNullOrEmpty(refine.HfDoctrate))
            {
                doctrates = refine.HfDoctrate;
                ViewData.Add("DoctratesView", doctrates);
            }

            //for searching specialization
            var basicSpec = BasicQualificationSpecialization != null && BasicQualificationSpecialization.Length > 0
                                ? string.Join(",", BasicQualificationSpecialization)
                                : refine.HfBasicSpecialization;
            ViewData.Add("BasicSpecView", basicSpec);

            var postSpec = PostGraduationSpecialization != null && PostGraduationSpecialization.Length > 0
                               ? string.Join(",", PostGraduationSpecialization)
                               : refine.HfPostSpecialization;
            ViewData.Add("PostSpecView", postSpec);

            var doctrateSpec = DoctrateSpecialization != null && DoctrateSpecialization.Length > 0
                                   ? string.Join(",", DoctrateSpecialization)
                                   : refine.HfDoctrateSpecialization;
            ViewData.Add("DoctrateSpecView", doctrateSpec);


            //Int32 TypeOfVac = 0;
            string TypeOfVac = string.Empty;
            if (!string.IsNullOrEmpty(refine.HfTypeOfVacancy))
            {
                TypeOfVac = refine.HfTypeOfVacancy;
                ViewData.Add("TypeOfVacancyView", TypeOfVac);
            }
            else if (TypeOfVacancy != null && TypeOfVacancy.Length > 0)
            {
                foreach (string i in TypeOfVacancy)
                {
                    if (i != "")
                    {
                        if (TypeOfVac == "")
                        {
                            TypeOfVac += i;
                        }
                        else
                        {
                            TypeOfVac += "," + i;
                        }
                    }
                }
                ViewData.Add("TypeOfVacancyView", TypeOfVac);
            }


            string TypeOfShift = string.Empty;
            if (!string.IsNullOrEmpty(refine.HfWorkShift))
            {
                TypeOfShift = refine.HfWorkShift;
                ViewData.Add("TypeOfWorkShift", TypeOfShift);
            }
            else if (TypeOfWorkShift != null && TypeOfWorkShift.Length > 0)
            {
                foreach (string i in TypeOfWorkShift)
                {
                    if (i != "")
                    {
                        if (TypeOfShift == "")
                        {
                            TypeOfShift += i;
                        }
                        else
                        {
                            TypeOfShift += "," + i;
                        }
                    }
                }
                ViewData.Add("TypeOfWorkShift", TypeOfShift);
            }


            //Age search

            if (!string.IsNullOrEmpty(MinAge))
            {
                ViewData.Add("MinAgeView", MinAge);
            }
            else if (!string.IsNullOrEmpty(refine.HfMinAge))
            {
                MinAge = refine.HfMinAge;
                ViewData.Add("MinAgeView", MinAge);
            }
            else
            {
                MinAge = string.Empty;
            }

            if (!string.IsNullOrEmpty(MaxAge))
            {
                ViewData.Add("MaxAgeView", MaxAge);
            }
            else if (!string.IsNullOrEmpty(refine.HfMaxAge))
            {
                MaxAge = refine.HfMaxAge;
                ViewData.Add("MaxAgeView", MaxAge);
            }
            else
            {
                MaxAge = string.Empty;
            }

            string position = string.Empty;
            if (refine.RefinePosition != null && refine.RefinePosition.Length > 0)
            {
                position = String.Join(",", refine.RefinePosition);
                ViewData.Add("PositionView", position);
            }
            else if (!string.IsNullOrEmpty(refine.HfPosition))
            {
                position = refine.HfPosition;
                ViewData.Add("PositionView", position);
            }

                               
           
            string gender = string.Empty;
            if (!string.IsNullOrEmpty(refine.RefineGender))
            {
                gender = refine.RefineGender;
                ViewData.Add("GenderView", gender);
            }
            else if (!string.IsNullOrEmpty(refine.HfGender))
            {
                gender = refine.HfGender;
                ViewData.Add("GenderView", gender);
            }

            var Candidates = _repository.GetCandidateSearch(what, language, minExperience, maxExperience, minannualSalaryLakhs, maxannualSalaryLakhs, CurrentLocation, AndOrLocations, PreferredLocation, Function, roles, Industry, BasicQual, postGraduations, doctrates, basicSpec, postSpec, doctrateSpec, MinAge, MaxAge, position, gender, TypeOfVac, TypeOfShift);
            
            /**********Start OrderBy Paid Candidates*************/
            DateTime currentdate = Constants.CurrentTime();
            DateTime fresh = currentdate;
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

            Candidates = orderingFunc(Candidates);

            /**********End OrderBy Paid Candidates*************/

            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = Candidates.Count();

            var CandidateResults = Candidates.Skip(skip).Take(take);

            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(CandidateResults);

        }
        

        private void AddMoreUrlToViewData(int nextPage)
        {
            ViewData["moreUrl"] = Url.Action("Index", "Home", new { page = nextPage });
        }        

        public ActionResult JobSearch(string PageNo)
        {
            ViewData["CandidateFunctions"] = new SelectList(_repository.GetFunctions(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name");

            //ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name");
            var indus = _repository.GetIndustriesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();

            indus.Insert(0, new SelectListItem { Value = "", Text = "--- Any ---" });
            ViewData["Industries"] = indus;

            //experience
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 30; i++)
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

            var minExp = totalExperienceYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minExp.Insert(0, new SelectListItem { Value = "", Text = "--- Years ---" });

            var months = totalExperienceMonths.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            months.Insert(0, new SelectListItem { Value = "", Text = "--- Months ---" });

            ViewData["MinExperienceYears"] = new SelectList(minExp, "Value", "Text");
            ViewData["Months"] = new SelectList(months, "Value", "Text");

            //salary
            List<DropDownItem> annualSalaryLakhs = new List<DropDownItem>();
            for (int i = 0; i <= 51; i++)
            {
                DropDownItem item = new DropDownItem();
                if (i == 0)
                {
                    item.Name = "< .5";
                }
                else if (i == 51)
                {
                    item.Name = "> 50";
                }
                else
                {
                    item.Name = i.ToString();
                }
                item.Value = i;
                annualSalaryLakhs.Add(item);
            }
            var minannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(minannualSal, "Value", "Text");

            var maxannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            maxannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });
            ViewData["MaxAnnualSalaryLakhs"] = new SelectList(maxannualSal, "Value", "Text");

            var jobs = _repository.GetJobs();
            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = jobs.Count();

            var JobResult = jobs.Skip(skip).Take(take);            

            return View(JobResult);
        }

        public ActionResult JobResults(string PageNo)
        {
            var jobs = _repository.GetJobs();
            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = jobs.Count();

            var JobResult = jobs.Skip(skip).Take(take);

            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(JobResult);
        }

        [HttpPost]
        public ActionResult JobResults(Refine refine, string what, string where, string language, string CandidateFunctions, string Roles,string MinExperienceYears, string MaxExperienceYears, string[] PreferredIndustries, string MinAnnualSalaryLakhs, string MaxAnnualSalaryLakhs, string[] TypeOfVacancy,string[] TypeOfShift, string Freshness, string PageNo)
        {
            if (!string.IsNullOrEmpty(what))
            {
                what = what.TrimEnd(',');
                ViewData.Add("WhatView", what);
            }
            else if (!string.IsNullOrEmpty(refine.HfWhat))
            {
                what = refine.HfWhat;
                ViewData.Add("WhatView", what);
            }
            else
            {
                what = string.Empty;
            }

            if (!string.IsNullOrEmpty(language))
            {
                language = language.TrimEnd(',');
                ViewData.Add("LanguageView", language);
            }

            else if (!string.IsNullOrEmpty(refine.HfLanguages))
            {
                language = refine.HfLanguages;
                ViewData.Add("LanguageView", language);
            }
            else
            {
                language = string.Empty;
            }

            if (!string.IsNullOrEmpty(where))
            {
                where = where.TrimEnd(',');
                ViewData.Add("WhereView", where);
            }
            else if (!string.IsNullOrEmpty(refine.HfWhere))
            {
                where = refine.HfWhere;
                ViewData.Add("WhereView", where);
            }
            else
            {
                where = string.Empty;
            }

          

            
            string minannualSalaryLakhs = string.Empty;
            string maxannualSalaryLakhs = string.Empty;
            //ViewData.Add("RefineSalaryView", "No");
            if (!string.IsNullOrEmpty(refine.RefineSalary))
            {
                string[] RefineSalary = refine.RefineSalary.Split('-');
                if (RefineSalary.Length > 1)
                {
                    minannualSalaryLakhs = RefineSalary[0];
                    maxannualSalaryLakhs = RefineSalary[1];
                    ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                    ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
                    ViewData.Add("RefineSalaryView", minannualSalaryLakhs);
                }
            }
            else if (MinAnnualSalaryLakhs == "0" || MaxAnnualSalaryLakhs == "0")
            {
                minannualSalaryLakhs = "1";
                maxannualSalaryLakhs = "50000";
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }
            else if (MinAnnualSalaryLakhs == "51" || MaxAnnualSalaryLakhs == "51")
            {
                minannualSalaryLakhs = "5000000";
                maxannualSalaryLakhs = "500000000";
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }
            else if (!string.IsNullOrEmpty(MinAnnualSalaryLakhs))
            {
                minannualSalaryLakhs = (Convert.ToInt32(MinAnnualSalaryLakhs) * 100000).ToString();
                maxannualSalaryLakhs = (Convert.ToInt32(string.IsNullOrEmpty(MaxAnnualSalaryLakhs) ? MinAnnualSalaryLakhs : MaxAnnualSalaryLakhs) * 100000).ToString();
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }
            else if (!string.IsNullOrEmpty(refine.HfMinSalary))
            {
                minannualSalaryLakhs = refine.HfMinSalary;
                maxannualSalaryLakhs = refine.HfMaxSalary;
                ViewData.Add("MinSalaryView", minannualSalaryLakhs);
                ViewData.Add("MaxSalaryView", maxannualSalaryLakhs);
            }

            string RefineOrganization = string.Empty;
            if (refine.RefineOrganization != null && refine.RefineOrganization.Length > 0)
            {
                foreach (string i in refine.RefineOrganization)
                {
                    if (i != "")
                    {
                        if (RefineOrganization == "")
                        {
                            RefineOrganization += i;
                        }
                        else
                        {
                            RefineOrganization += "," + i;
                        }
                    }
                }
                ViewData.Add("OrganizationView", RefineOrganization);
            }
            else if (!string.IsNullOrEmpty(refine.HfOrganization))
            {
                RefineOrganization = refine.HfOrganization;
                ViewData.Add("OrganizationView", RefineOrganization);
            }

            string minExperience = string.Empty;
            string maxExperience = string.Empty;

            if (MinExperienceYears == "0")
            {
                minExperience = null;
                maxExperience = null;
            }

            if (!string.IsNullOrEmpty(refine.RefineExp))
            {
                string[] RefineExp = refine.RefineExp.Split('-');
                if (RefineExp.Length > 1)
                {
                    minExperience = (Convert.ToInt64(RefineExp[0]) * 365 * 24 * 60 * 60).ToString();
                    maxExperience = (Convert.ToInt64(RefineExp[1]) * 365 * 24 * 60 * 60).ToString();
                    ViewData.Add("MinExperienceView", minExperience);
                    ViewData.Add("MaxExperienceView", maxExperience);
                    ViewData.Add("RefineExperienceView", RefineExp[0]);
                }
            }
            else if (!string.IsNullOrEmpty(MinExperienceYears))
            {
                minExperience = (Convert.ToInt64(MinExperienceYears) * 365 * 24 * 60 * 60).ToString();
                long max = Convert.ToInt64(MinExperienceYears) + 1;
                maxExperience = (max * 365 * 24 * 60 * 60).ToString();
                ViewData.Add("MinExperienceView", minExperience);
                ViewData.Add("MaxExperienceView", maxExperience);

            }
            else if (!string.IsNullOrEmpty(refine.HfMinExperience))
            {
                minExperience = refine.HfMinExperience;
                maxExperience = refine.HfMaxExperience;
                ViewData.Add("MinExperienceView", minExperience);
                ViewData.Add("MaxExperienceView", maxExperience);
            }

            string RefineFunction = "";
            if (refine.RefineFunction != null && refine.RefineFunction.Length > 0)
            {
                foreach (string i in refine.RefineFunction)
                {
                    if (i != "")
                    {
                        if (RefineFunction == "")
                        {
                            RefineFunction += i;
                        }
                        else
                        {
                            RefineFunction += "," + i;
                        }
                    }
                }
                ViewData.Add("FunctionView", RefineFunction);

            }
            else if (!string.IsNullOrEmpty(CandidateFunctions))
            {
                ViewData.Add("FunctionView", CandidateFunctions);
            }
            else if (!string.IsNullOrEmpty(refine.HfFunction))
            {
                RefineFunction = refine.HfFunction;
                ViewData.Add("FunctionView", RefineFunction);
            }

            string RefineRole = string.Empty;

            if (refine.RefineRoles != null && refine.RefineRoles.Length > 0)
            {
                foreach (string i in refine.RefineRoles)
                {
                    if (i != "")
                    {
                        if (RefineRole == "")
                        {
                            RefineRole += i;
                        }
                        else
                        {
                            RefineRole += "," + i;
                        }
                    }
                }
                ViewData.Add("RoleView", RefineRole);

            }
            else if (!string.IsNullOrEmpty(Roles))
            {
                ViewData.Add("RoleView", Roles);
            }
            else if (!string.IsNullOrEmpty(refine.HfRoles))
            {
                RefineRole = refine.HfRoles;
                ViewData.Add("RoleView", RefineRole);
            }

            string industries = string.Empty;
            if (!string.IsNullOrEmpty(refine.HfIndustries))
            {
                industries = refine.HfIndustries;
                ViewData.Add("IndustriesView", industries);
            }
            else if (PreferredIndustries != null && PreferredIndustries.Length > 0)
            {
                foreach (string i in PreferredIndustries)
                {
                    if (i != "")
                    {
                        if (industries == "")
                        {
                            industries += i;
                        }
                        else
                        {
                            industries += "," + i;
                        }
                    }
                }
                ViewData.Add("IndustriesView", industries);
            }


            //Int32 TypeOfVac = 0;
            string TypeOfVac = string.Empty;
            if (!string.IsNullOrEmpty(refine.HfTypeOfVacancy))
            {
                TypeOfVac = refine.HfTypeOfVacancy;
                ViewData.Add("TypeOfVacancyView", TypeOfVac);
            }
            else if (TypeOfVacancy != null && TypeOfVacancy.Length > 0)
            {
                foreach (string i in TypeOfVacancy)
                {
                    if (i != "")
                    {
                        if (TypeOfVac == "")
                        {
                            TypeOfVac += i;
                        }
                        else
                        {
                            TypeOfVac += "," + i;
                        }
                    }
                }
                ViewData.Add("TypeOfVacancyView", TypeOfVac);
            }

           
            string ShiftType = string.Empty;
            if (!string.IsNullOrEmpty(refine.HfWorkShift))
            {
                ShiftType = refine.HfWorkShift;
                ViewData.Add("TypeOfShiftView", ShiftType);
            }
            else if (TypeOfShift != null && TypeOfShift.Length > 0)
            {
                foreach (string i in TypeOfShift)
                {
                    if (i != "")
                    {
                        if (ShiftType == "")
                        {
                            ShiftType += i;
                        }
                        else
                        {
                            ShiftType += "," + i;
                        }
                    }
                }
                ViewData.Add("TypeOfShiftView", ShiftType);
            }

            string fresh = string.Empty;

            if (!string.IsNullOrEmpty(refine.HfFreshness))
            {
                fresh = refine.HfFreshness;
                ViewData.Add("FreshnessView", fresh);
            }
            else if (!string.IsNullOrEmpty(Freshness))
            {
                fresh = Freshness;
                ViewData.Add("FreshnessView", fresh);
            }


            var jobs = _repository.GetJobs(what, where, language, CandidateFunctions,Roles, RefineFunction, RefineOrganization, industries, minExperience, maxExperience, minannualSalaryLakhs, maxannualSalaryLakhs, TypeOfVac,ShiftType, fresh);
            
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

            jobs = orderingFunc(jobs);


            ViewData["CandidateFunctions"] = new SelectList(_repository.GetFunctions(), "Id", "Name");

            ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name");

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name");

            //experience
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 30; i++)
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

            var minExp = totalExperienceYears.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minExp.Insert(0, new SelectListItem { Value = "", Text = "--- Years ---" });

            var months = totalExperienceMonths.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            months.Insert(0, new SelectListItem { Value = "", Text = "--- Months ---" });

            ViewData["MinExperienceYears"] = new SelectList(minExp, "Value", "Text");
            ViewData["Months"] = new SelectList(months, "Value", "Text");

            //salary
            List<DropDownItem> annualSalaryLakhs = new List<DropDownItem>();
            for (int i = 0; i <= 51; i++)
            {
                DropDownItem item = new DropDownItem();
                if (i == 0)
                {
                    item.Name = "< .5";
                }
                else if (i == 51)
                {
                    item.Name = "> 50";
                }
                else
                {
                    item.Name = i.ToString();
                }
                item.Value = i;
                annualSalaryLakhs.Add(item);
            }
            var minannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            minannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Min ---" });
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(minannualSal, "Value", "Text");

            var maxannualSal = annualSalaryLakhs.Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Name }).ToList();
            maxannualSal.Insert(0, new SelectListItem { Value = "", Text = "--- Max ---" });
            ViewData["MaxAnnualSalaryLakhs"] = new SelectList(maxannualSal, "Value", "Text");

            int page;
            if (string.IsNullOrEmpty(PageNo))
                page = 1;
            else
                page = Convert.ToInt32(PageNo);

            int pageSize = 15;
            int skip = (page - 1) * pageSize;

            //number of results per page. 
            int take = pageSize;

            var RecordCount = jobs.Count();

            var JobResult = jobs.Skip(skip).Take(take);

            ViewData.Add("PageIndex", page);
            ViewData.Add("RecordCount", RecordCount);

            return View(JobResult);

        }

    }
}
