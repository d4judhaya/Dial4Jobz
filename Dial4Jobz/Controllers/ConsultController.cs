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
using System.Configuration;
using System.Text;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.IO.Packaging;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Routing;



namespace Dial4Jobz.Controllers
{
   
    public class ConsultController: BaseController
    {
     
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
                  

         //GET: /Consult/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [Authorize]
        public ActionResult MySubscription_Billing()
        {

            if (LoggedInOrganization != null)
                ViewData["LoggedConsultant"] = LoggedInConsultant.Id;
            else
                ViewData["LoggedConsultant"] = 0;

            return View();
        }
       

        [HttpPost]
        public ActionResult Register(FormCollection collection, ConsultantRegisterModel model)
        {
            /**********Developer Note: Start validations in Consultant Model*****************************/
            string consultUsername = collection["UserName"];
            string consultEmail = collection["Email"];

            var usernameExists = _userRepository.GetConsultantUserName(consultUsername);
            var emailidExists = _userRepository.GetConsultantEmail(consultEmail);

            if (usernameExists != null)
                return Json(new JsonActionResult { Success = false, Message = "UserName is already Exists. Pick different Username or Email Id" });

            if (emailidExists != null)
                return Json(new JsonActionResult { Success = false, Message = "Email-Id is already Exists." });
            
            /**********End validations in Consultant Model*****************************/

            Consultante consultants = new Consultante();
            consultants.UserName = model.UserName;
            consultants.Name = collection["Name"];
            consultants.Password = SecurityHelper.GetMD5Bytes(collection["Password"]);
            consultants.Email = model.Email;
            consultants.CreatedDate = Constants.CurrentTime();
            //consultants.MobileNumber = collection["MobileNumber"];
            _userRepository.AddConsultant(consultants);
            _userRepository.Save();

            if (consultants.Email != null)
            {
                EmailHelper.SendEmail(
                   Constants.EmailSender.EmployerSupport,
                   consultants.Email,
                   "REGISTRATION SUCCESSFULL",
                   Constants.EmailBody.ConsultantRegister
                   .Replace("[NAME]", consultants.Name)
                   .Replace("[USERNAME]", consultants.UserName)
                   .Replace("[LINK_NAME]", "Verify your E-mail ID")
                   .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Consult/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(consultants.Id.ToString()))
                   .Replace("[PASSWORD]", collection["Password"])
                   );
            }

            if (consultants.MobileNumber != null)
            {

                SmsHelper.SendSecondarySms(
                       Constants.SmsSender.SecondaryUserName,
                       Constants.SmsSender.SecondaryPassword,
                       Constants.SmsBody.SMSCandidateRegister
                                    .Replace("[USER_NAME]", consultants.UserName)
                                    .Replace("[PASSWORD]", collection["Password"]),
                                    //.Replace("[CODE]", phVerficationNo.ToString()),

                     Constants.SmsSender.SecondaryType,
                       Constants.SmsSender.Secondarysource,
                       Constants.SmsSender.Secondarydlr,
                    consultants.MobileNumber
                    );

            }
            FormsService.SignIn(model.UserName, true /* createPersistentCookie */);

            return Json(new JsonActionResult { Success = true, Message = "Welcome! You have Successfully Registered as a Consultant.", ReturnUrl="/Consult/Profile" });
        }

        public ActionResult Activation(string Id)
        {
            Consultante consultant = null;
            if (!string.IsNullOrEmpty(Id))
            {
                Id = Constants.DecryptString(Id);
                consultant = _userRepository.GetConsultantsById(Convert.ToInt32(Id));
                //consultant.IsMailVerified = true;
                _userRepository.Save();
            }

            if (consultant != null)
            {
                var consultantVerification = _repository.GetConsulant(consultant.Id);
                
            }

            return RedirectToAction("Index", "Consult");

        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult LogOn(ConsultantLogOnModel model, FormCollection collection)
        {
            var consultant = _userRepository.GetConsultantsByUserName(collection["UserName"]);

            bool userValidated = false;

            try
            {
                userValidated = MembershipService.ValidateConsultant(collection["UserName"], collection["Password"]);

                if (!userValidated)
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            catch (UnauthorizedAccessException uae)
            {
                return Json(new JsonActionResult { Success = false, Message = uae.Message });
            }

            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });


            FormsService.SignIn(model.UserName, model.RememberMe);

            return Json(new JsonActionResult { Success = true, Message = "Successfully Logged In", ReturnUrl =  VirtualPathUtility.ToAbsolute("~/Consult/Index") });

        }

        public ActionResult Profile()
        {
            if (LoggedInConsultant == null)
                return new FileNotFoundResult();
            
            var consultant = _userRepository.GetConsultantsById(LoggedInConsultant.Id);

            List<SelectListItem> industries = new List<SelectListItem>();
            industries.Add(new SelectListItem { Text = "Education/Teaching/Training", Value = "2362" });
            industries.Add(new SelectListItem { Text = "Recruitment/Employment Consultants", Value = "2412" });
            industries.Add(new SelectListItem { Text = "NGO/Non Profit", Value = "2398" });

            ViewData["Industries"] = new SelectList(industries, "Value", "Text", (consultant.IndustryId!=null ? consultant.IndustryId: 0));

            Location location = consultant.LocationId.HasValue ? _repository.GetLocationById(consultant.LocationId.Value) : null;

            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);


            return View(consultant);
        }

        [Authorize,HttpPost]
        public ActionResult Profile(FormCollection collection)
        {
            Consultante consultant = _userRepository.GetConsultantsById(LoggedInConsultant.Id);
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            if (!string.IsNullOrEmpty(collection["Industries"]))
                consultant.IndustryId = Convert.ToInt32(collection["Industries"]);
            consultant.Name = collection["Name"];
            consultant.ContactPerson = collection["ContactPerson"];
            consultant.Email = collection["Email"];
            consultant.Website = collection["Website"];
            consultant.ContactNumber = collection["ContactNumber"];
            consultant.MobileNumber = collection["MobileNumber"];
            consultant.Address = collection["Address"];
            consultant.Pincode = collection["Pincode"];
            consultant.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
            consultant.DisplayName = collection["DisplayName"];

            if (consultant.CreatedDate == null)
            {
                consultant.CreatedDate = timeZone;
            }
            else
            {
                consultant.UpdatedDate = timeZone;
            }
            
            Location location = new Location();
            if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
            if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
            if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
            if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

            if (location.CountryId != 0)
                consultant.LocationId = _repository.AddLocation(location);

            if (!TryValidateModel(consultant))
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            _userRepository.Save();

           
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Profile has been updated with the IP Address of " +  consultant.IPAddress
            });
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult Plans()
        {
            return View();
        }


        [Authorize]
        public ActionResult AddCandidate()
        {
                Candidate candidate = new Candidate();
                IEnumerable<OrderDetail> getActivePlans = null;
                if (LoggedInConsultant != null)
                {
                    getActivePlans = _vasRepository.GetActivatedPlansByConsultant(LoggedInConsultant.Id);
                }
                if (getActivePlans.Count() > 0)
                {

                    ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
                    ViewData["RolesFunction"] = new SelectList(_repository.GetRoles(), "Id", "Name");
                    ViewData["CandidateFunctionsRole"] = new SelectList(_repository.GetFunctionsByRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);

                    ViewData["Functions"] = _repository.GetFunctions();

                    var functions = _repository.GetFunctionsEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
                    functions.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
                    ViewData["PrefFunctions"] = functions;
                    
                    IEnumerable<CandidatePreferredFunction> preferredFunctions = _repository.GetCandidatePreferredFunctions(candidate.Id);

                    if (preferredFunctions.Count() > 0)
                    {
                        ViewData["PrefFunctionIds"] = String.Join(",", preferredFunctions.Select(pf => pf.FunctionId));
                        ViewData["PrefRoleIds"] = String.Join(",", preferredFunctions.Select(pf => pf.RoleId).Where(jr => jr != null));
                    }

                    ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
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
                        //int months = candidate.TotalExperience.HasValue ? ((int)candidate.TotalExperience.Value - (years / 31536000)) / 43200 : 0;
                        //int months = candidate.TotalExperience.HasValue ? ((int)candidate.TotalExperience.Value - (years / 2678400)) / 43200 : 0; 
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
                else
                {
                    return RedirectToAction("Index");
                }
        }

        //Consultante consultant = _repository.GetConsulant(LoggedInConsultant.Id);
        //Candidate candidate = _repository.GetCandidateByConsultant(consultant.Id);

        [Authorize,HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult SaveCandidate(FormCollection collection)
        {
            Candidate candidate = null;
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            bool updateOperation = false;
            var contactNumber = collection["ContactNumber"];
            var email = collection["Email"];
            var Name = collection["Name"];
            var function = collection["CandidateFunctions"];
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            // Validations checking

            if (!string.IsNullOrEmpty(collection["Id"]))
            {
                int currentId = Convert.ToInt32(collection["Id"]);

                if (currentId == 0)
                {
                    candidate = new Candidate();
                    
                 }
                else
                {
                    candidate = _repository.GetCandidateByConsultant(LoggedInConsultant.Id,currentId);
                    updateOperation = true;
                }

                    if ((contactNumber == null || contactNumber == "") && (email == null || email == ""))
                    {
                        return Json(new JsonActionResult { Success = false, Message = "Mobile Number/Email is required." });
                    }
                   
                    if (collection["Position"] == null || collection["Position"] == "")
                    {
                        return Json(new JsonActionResult { Success = false, Message = "Position is Required" });
                    }


                    else if ((collection["DOB"]) == null || collection["DOB"] == "")
                    {
                        return Json(new JsonActionResult { Success = false, Message = "DOB is required" });
                    }
                    //end validation checking

                    //candidate = new Candidate();
                }
                else
                {
                    candidate = _repository.GetCandidateByConsultant(LoggedInConsultant.Id, Convert.ToInt32(collection["Id"]));
                    updateOperation = true;
                }
            
            candidate.Name = collection["Name"];
            candidate.Email = collection["Email"];
            candidate.ContactNumber = collection["ContactNumber"];
            candidate.MobileNumber = collection["MobileNumber"];
            candidate.Address = collection["Address"];
            candidate.Pincode = collection["Pincode"];
            candidate.Description = collection["Description"];
            candidate.LicenseNumber = collection["LicenseNumber"];
            candidate.PassportNumber = collection["PassportNumber"];
            candidate.InternationalNumber = collection["InternationalNumber"];
            candidate.ConsultantId = Convert.ToInt32(LoggedInConsultant.Id);

            candidate.IsPhoneVerified = false;


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


            string randomString = string.Empty;
            // Generation of Username and password start
            if (updateOperation == false)
            {
                Random randomNo = new Random();
                string firstname = string.Empty;

                string createUsername = randomNo.Next(250, 350).ToString();
                string fullname = collection["Name"];
                var names = fullname.Split(' ');

                if (names.Count() > 0)
                    firstname = names[0];
                else
                    firstname = fullname;

                if (!string.IsNullOrEmpty(collection["Name"]))
                    candidate.UserName = firstname + createUsername;
                else
                    candidate.UserName = collection["ContactNumber"];

                var usernameExists = _userRepository.GetCandidateByUserName(candidate.UserName);
                if (usernameExists != null)
                {
                    candidate.UserName = collection["ContactNumber"];
                }

                randomString = SecurityHelper.GenerateRandomString(6, true);
                byte[] password = SecurityHelper.GetMD5Bytes(randomString);
                candidate.Password = password;

                string phVerficationNo = randomNo.Next(1000, 9999).ToString();
                candidate.PhoneVerificationNo = Convert.ToInt32(phVerficationNo);
            }


            // Generation of Username and password end

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
            candidate.PreferredTimeFrom = collection["ddlPreferredTimeFrom"];
            candidate.PreferredTimeTo = collection["ddlPreferredTimeto"];

            if (!string.IsNullOrEmpty(collection["CandidateFunctions"]))
                candidate.FunctionId = Convert.ToInt32(collection["CandidateFunctions"]);

            if (!string.IsNullOrEmpty(collection["Industries"]))
                candidate.IndustryId = Convert.ToInt32(collection["Industries"]);
            else
                candidate.IndustryId = null;

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


            if (!TryValidateModel(candidate))
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            if (updateOperation == false)
                _repository.AddCandidate(candidate);

            int candidateId = candidate.Id;

            //Candidates skills

            _repository.DeleteCandidateSkills(candidateId);

            string[] skills = collection["Skills"].Split(',');

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

            string[] roles = collection["Roles"].Split(',');

            if (roles.Count() != 0)
            {
                _repository.DeleteCandidateRoles(candidateId);
            }
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
                if (!string.IsNullOrEmpty(licenseType))
                {
                    CandidateLicenseType clt = new CandidateLicenseType();
                    clt.CandidateId = candidateId;
                    clt.LicenseTypeId = Convert.ToInt32(licenseType);
                    _repository.AddCandidateLicenseType(clt);
                }
            }

            //Add candidate qualification(first clear the existing one's)
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
                    int specializationId;
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

                    if (i == 1 && cq.DegreeId <= 0)
                    {
                        ModelState.AddModelError("BasicQualificationDegreeRequired", "Choose Basic Qualification");
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

            /*Developer Note: After save the New Candidate Details One count will reduce as per plan.*/


            int remainingCount = 0;
            if (updateOperation == false)
            {
               remainingCount=  _vasRepository.UpdateConsultantCount(LoggedInConsultant.Id, candidate.Id);
            }

            //for new entry

            if (updateOperation == false)
            {
                
                if (candidate.Email != "")
                {

                    EmailHelper.SendEmail(
                      Constants.EmailSender.CandidateSupport,
                      candidate.Email,
                      Constants.EmailSubject.Registration,
                      Constants.EmailBody.CandidateRegister
                          .Replace("[NAME]", candidate.Name)
                          .Replace("[USER_NAME]", candidate.UserName)
                          .Replace("[PASSWORD]", randomString)
                          .Replace("[EMAIL]", candidate.Email)
                          .Replace("[LINK_NAME]", "Verify Here")
                          .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                        //.Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + candidate.Id.ToString())
                          );
                }

                if (candidate.ContactNumber != "")
                {

                    SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.SMSCandidateRegister
                                            .Replace("[USER_NAME]", candidate.UserName)
                                            .Replace("[PASSWORD]", randomString)
                                            .Replace("[CODE]", candidate.PhoneVerificationNo.ToString()),

                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            candidate.ContactNumber
                            );

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


                    int candidateexperience;
                    int candidatemonth;

                    //candidateexperience= (candidate.TotalExperience.Value / 33782400.0)  +"Years";
                    //candidatemonth= (candidate.TotalExperience.Value * 31536000)+ "Months";

                    candidateexperience = ((int)(candidate.TotalExperience.Value / 31104000));
                    candidatemonth = ((int)((candidate.TotalExperience.Value - ((candidateexperience) * 31536000)) / 2678400));

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
                          .Replace("[COUNTRY]", (candidatelocation != "" ? candidatelocation : "NA"))
                          .Replace("[CITY]", (cityName != "" ? cityName.ToString() : "NA"))
                          .Replace("[PREF_COUNTRY]", (prefcountryName != "" ? prefcountryName : "NA"))
                          .Replace("[PREF_CITY]", (prefcityName != "" ? prefcityName : "NA"))
                          .Replace("[PREF_FUNC]", (preffunction != "" ? preffunction.ToString() : "NA"))
                          .Replace("[INDUSTRY]", (industry != "" ? industry : "NA"))
                        //.Replace("[PREF_LOC]", prefcountryName + "," + prefcityName)
                          .Replace("[DOB]", age.ToString())
                          .Replace("[ROLE]", (role != "" ? role : "NA"))
                          .Replace("[YEARS]", (candidateexperience != 0 ? candidateexperience.ToString() + "Years" : "NM"))
                          .Replace("[MONTHS]", (candidatemonth != 0 ? candidatemonth.ToString() + "Months" : "NM"))
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
            }

            if (remainingCount != 0)
            {
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Profile has been updated.You can add" + remainingCount + " remaining of Candidates.",
                    ReturnUrl = "/JobMatchesCandidate/JobMatch/" + candidate.Id.ToString()
                });
            }
            else
            {
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Profile has been updated from your IP Address of" + candidate.IPAddress,
                    ReturnUrl = "/JobMatchesCandidate/JobMatch/" + candidate.Id.ToString()
                });
            }
        }


        public ActionResult VacancyReports()
        {
            return View();
        }


        public JsonResult ListVacancyReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Job> jobResult = _repository.GetJobs().Where(j => j.ConsultantId == LoggedInConsultant.Id);
            IEnumerable<Dial4Jobz.Models.OrderDetail> getActivePlans = _vasRepository.GetActivatedPlansByConsultant(LoggedInConsultant.Id);
            //Where(c => c.Consultante.Id ==LoggedInConsultant.Id)

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                jobResult = jobResult.OrderByDescending(o => o.Position);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                jobResult = jobResult.OrderBy(o => o.Position);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                jobResult = jobResult.OrderByDescending(o => o.Organization.Name);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                jobResult = jobResult.OrderBy(o => o.Organization.Name);
            else if (iSortCol_0 == 3 && "desc" == sSortDir_0)
                jobResult = jobResult.OrderByDescending(o => o.FunctionId);
            else if (iSortCol_0 == 3 && "asc" == sSortDir_0)
                jobResult = jobResult.OrderBy(o => o.MinExperience);
            else if (iSortCol_0 == 4 && "desc" == sSortDir_0)
                jobResult = jobResult.OrderByDescending(o => o.Budget);
            //else if (iSortCol_0 == 4 && "asc" == sSortDir_0)
            //    jobResult = jobResult.OrderByDescending(o => o.JobLocations.Where(jl=>jl.Location.Country));
            else
                jobResult = jobResult.OrderByDescending(o => o.Id);


            if (!string.IsNullOrEmpty(sSearch.Trim()))
                jobResult = jobResult.Where(o => o.Position.ToLower().Contains(sSearch.ToLower().Trim()) || o.Organization.Name != null && o.Organization.Name.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                jobResult = jobResult.Where(o => o.CreatedDate != null && o.CreatedDate.Value >= from && o.CreatedDate.Value <= to);

            }

            IEnumerable<Job> jobResult1 = jobResult.Skip(iDisplayStart).Take(iDisplayLength);

            if (getActivePlans.Count() > 0)
            {

                var result = new
                {
                    iTotalRecords = jobResult.Count(),
                    iTotalDisplayRecords = jobResult.Count(),
                    aaData = jobResult1.Select(o => new object[] { o.Position, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString() : "", (o.Consultante != null ? o.Consultante.Name : ""), (o.MobileNumber != null ? o.MobileNumber : ""), (o.EmailAddress != null ? o.EmailAddress : ""), (o.Organization != null ? (o.Organization.IPAddress != null ? o.Organization.IPAddress : "") : ""), "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Consult/Edit?Id=" + Dial4Jobz.Models.Constants.EncryptString(o.Id.ToString()) + "'>Edit Vacancy</a>" })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new
                {
                    iTotalRecords = jobResult.Count(),
                    iTotalDisplayRecords = jobResult.Count(),
                    aaData = jobResult1.Select(o => new object[] { o.Position, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString() : "", (o.Consultante != null ? o.Consultante.Name : ""), (o.MobileNumber != null ? o.MobileNumber : ""), (o.EmailAddress != null ? o.EmailAddress : ""), (o.Organization != null ? (o.Organization.IPAddress != null ? o.Organization.IPAddress : "") : ""), "Subscribe Plans" })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        /*****Consultant's Candidate Reports******/
        public ActionResult CandidateReports()
        {
            return View();
        }


        public JsonResult ListCandidateReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Candidate> candidateResult = _repository.GetCandidates().Where(c => c.Consultante.Id ==LoggedInConsultant.Id);
            IEnumerable<Dial4Jobz.Models.OrderDetail> getActivePlans = _vasRepository.GetActivatedPlansByConsultant(LoggedInConsultant.Id);

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.Name);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.Name);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.CreatedDate.Value);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.CreatedDate.Value);
            else if (iSortCol_0 == 3 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.ContactNumber);
            else if (iSortCol_0 == 3 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.ContactNumber);
            else if (iSortCol_0 == 4 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.Email);
            else if (iSortCol_0 == 4 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.Email);
            else
                candidateResult = candidateResult.OrderByDescending(o => o.Id);


            if (!string.IsNullOrEmpty(sSearch.Trim()))
                candidateResult = candidateResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.ContactNumber != null && o.ContactNumber.ToLower().Contains(sSearch.ToLower().Trim()) || o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()));
            //_repository.GetAdminUserNamebyEntryIdAndEntryType(o.Id, EntryType.Candidate).ToLower().Contains(sSearch.ToLower().Trim()) ||

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                candidateResult = candidateResult.Where(o => o.CreatedDate != null && o.CreatedDate.Value >= from && o.CreatedDate.Value <= to);

            }

            IEnumerable<Candidate> candidateResult1 = candidateResult.Skip(iDisplayStart).Take(iDisplayLength);

            if (getActivePlans.Count() > 0)
            {
                var result = new
                {
                    iTotalRecords = candidateResult.Count(),
                    iTotalDisplayRecords = candidateResult.Count(),
                    aaData = candidateResult1.Select(o => new object[] { (o.Consultante != null ? o.Consultante.Name : ""), o.UserName, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString() : "", o.Name, o.Position, o.Function.Name, (o.TotalExperience.HasValue ? o.TotalExperience / 31104000 + " Years" + ((o.TotalExperience.Value - ((o.TotalExperience / 31104000) * 31536000)) / 2678400) + " Months" : ""), (o.AnnualSalary.HasValue ? o.AnnualSalary.ToString() : ""), "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Consult/GetDetail?userName=" + Dial4Jobz.Models.Constants.EncryptString(o.UserName.ToString()) + "'>Edit CV</a>" })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new
                {
                    iTotalRecords = candidateResult.Count(),
                    iTotalDisplayRecords = candidateResult.Count(),
                    aaData = candidateResult1.Select(o => new object[] { (o.Consultante != null ? o.Consultante.Name : ""), o.UserName, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString() : "", o.Name, o.Position, o.Function.Name, (o.TotalExperience.HasValue ? o.TotalExperience / 31104000 + " Years" + ((o.TotalExperience.Value - ((o.TotalExperience / 31104000) * 31536000)) / 2678400) + " Months" : ""), (o.AnnualSalary.HasValue ? o.AnnualSalary.ToString() : ""),"<a target='_blank' href='"+ System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Consult/Plans"+">Subscribe Plans</a>" })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        // Get the details of Candidate
        public ActionResult GetDetail(string userName)
        {

            Candidate candidate = null;
            if (!string.IsNullOrEmpty(userName))
            {
                userName = Constants.DecryptString(userName);
                candidate = _userRepository.GetCandidateByUserName(userName);
            }
            
            else
            {
                if (candidate == null)
                    candidate = new Candidate();
            }

            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            ViewData["Functions"] = _repository.GetFunctions();

            var functions = _repository.GetFunctionsEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            functions.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["PrefFunctions"] = functions;

            ViewData["RolesFunction"] = new SelectList(_repository.GetRoles(), "Id", "Name");
            ViewData["CandidateFunctionsRole"] = new SelectList(_repository.GetFunctionsByRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);


            ViewData["PreferredFunctionIds"] = candidate.CandidatePreferredFunctions.Select(cpf => cpf.FunctionId);
            IEnumerable<CandidatePreferredFunction> preferredFunctions = _repository.GetCandidatePreferredFunctions(candidate.Id);

            if (preferredFunctions.Count() > 0)
            {
                ViewData["PrefFunctionIds"] = String.Join(",", preferredFunctions.Select(pf => pf.FunctionId));
                ViewData["PrefRoleIds"] = String.Join(",", preferredFunctions.Select(pf => pf.RoleId).Where(jr => jr != null));
            }


            ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            ViewData["LicenseTypeIds"] = candidate.CandidateLicenseTypes.Select(clt => clt.LicenseTypeId);

            ViewData["CandidateFunctions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", candidate.FunctionId);

            //if (candidate.FunctionId != null)
            //    ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);

            CandidatePreferredRole cpr = _repository.GetRolesById(candidate.Id);
            int functionid = _repository.GetFunctionIdByCandidateId(candidate.Id);

            if (cpr != null)
                ViewData["Roles"] = new SelectList(_repository.GetRolesByFunctionId(functionid), "Id", "Name", cpr.RoleId);
            else
                ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", candidate.IndustryId);

            ViewData["MaritalStatus"] = new SelectList(_repository.GetMaritalStatus(), "Id", "Name", candidate.MaritalId);

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


            //part time timings
            List<DropDownItem> preferredTimeFrom = new List<DropDownItem>();
            for (int i = 1; i <= 12; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString() + " AM";
                item.Value = i;
                preferredTimeFrom.Add(item);
            }
            ViewData["PreferredTimeFrom"] = new SelectList(preferredTimeFrom, "Value", "Name", candidate.PreferredTimeFrom);

            List<DropDownItem> MaxPreferredTimeFrom = new List<DropDownItem>();
            for (int i = 1; i <= 12; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString() + " PM";
                item.Value = i;
                MaxPreferredTimeFrom.Add(item);
            }
            ViewData["MaxPreferredTimeFrom"] = new SelectList(MaxPreferredTimeFrom, "Value", "Name", candidate.PreferredTimeFrom);



            List<DropDownItem> preferredTimeTo = new List<DropDownItem>();
            for (int i = 1; i <= 11; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString() + " AM";
                item.Value = i;
                preferredTimeTo.Add(item);
            }
            ViewData["PreferredTimeTo"] = new SelectList(preferredTimeTo, "Value", "Name", candidate.PreferredTimeFrom);

            List<DropDownItem> MaxPreferredTimeTo = new List<DropDownItem>();
            for (int i = 1; i <= 12; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString() + " PM";
                item.Value = i;
                MaxPreferredTimeTo.Add(item);
            }
            ViewData["MaxPreferredTimeTo"] = new SelectList(MaxPreferredTimeTo, "Value", "Name", candidate.PreferredTimeTo);


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

            return View("AddCandidate", candidate);

        }

        public ActionResult ValidateCandidate(string userName)
        {
            //UserRepository _userRepository = new UserRepository();
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(userName))
            {
                candidate = _userRepository.GetCandidateByUserName(userName);
                if (candidate == null)
                    return Json("User Id not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("User Id exist", JsonRequestBehavior.AllowGet);

            }
           
            return RedirectToAction("GetDetail", "Consult");

        }


        public ActionResult Add(int consultantId)
        {
            if (LoggedInConsultant != null)
            {
                ViewData["consultId"] = LoggedInConsultant.Id;
            }
            else if (consultantId != 0)
            {
                ViewData["consultantId"] = consultantId;
            }
            SetCommonViewData();
            SetAddJobViewData();
            return View();
           
        }

        private void SetCommonViewData()
        {
            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name");
            ViewData["JobBasicQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.BasicQualification);
            ViewData["JobPostQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.PostGraduation);
            ViewData["JobDoctorate"] = _repository.GetDegreeswithAnyOption(DegreeType.Doctorate);
        }

        private void SetEditJobViewData(Job job)
        {

            //salary
            int lakhs = (int)(job.MaxBudget / 100000);
            int thousands = (int)((job.MaxBudget - (lakhs * 100000)) / 1000);
            ViewData["AnnualSalaryLakhs"] = new SelectList(GetMaxBudgetLakhs(), "Value", "Name", lakhs);
            ViewData["AnnualSalaryThousands"] = new SelectList(GetMaxBudgetThousands(), "Value", "Name", thousands);

            //minsalary
            int minlakhs = job.Budget.HasValue ? (int)(job.Budget / 100000) : 0;
            int minthousands = job.Budget.HasValue ? (int)((job.Budget - (minlakhs * 100000)) / 1000) : 0;
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(GetBudgetLakhs(), "Value", "Name", minlakhs);
            ViewData["MinAnnualSalaryThousands"] = new SelectList(GetBudgetThousands(), "Value", "Name", minthousands);

            //experience
            int minyears = job.MinExperience.HasValue ? (int)job.MinExperience.Value / 31104000 : 0;
            int maxyears = job.MaxExperience.HasValue ? (int)job.MaxExperience.Value / 31104000 : 0;
            ViewData["TotalExperienceYearsFrom"] = new SelectList(GetTotalExperienceMinYears(), "Value", "Name", minyears);
            ViewData["TotalExperienceYearsTo"] = new SelectList(GetTotalExperienceMaxYears(), "Value", "Name", maxyears);

            ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            ViewData["LicenseTypeIds"] = job.JobLicenseTypes.Select(clt => clt.LicenseTypeId);

            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", job.FunctionId);

            JobRole jobrole = _repository.GetRolesByJobId(job.Id);
            if (jobrole != null)
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

        private void SetAddJobViewData()
        {
            //preferred time
            //ViewData["PreferredTimeFrom"] = new SelectList(GetFromTimes(), "Value", "Name", 0);
            //ViewData["PreferredTimeTo"] = new SelectList(GetToTimes(), "Value", "Name", 0);

            //ViewData["MaxPreferredTimeFrom"] = new SelectList(GetMaxFromTimes(), "Value", "Name", 0);
            //ViewData["MaxPreferredTimeTo"] = new SelectList(GetMaxToTimes(), "Value", "Name", 0);

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

            //var industry = new SelectList(_repository.GetIndustries(), "Id", "Name", 0).ToList();
            //industry.Insert(1, new SelectListItem { Value = "0", Text = "---Any---" });
            ////industry.Insert(0,new SelectListItem { Value = "0", Text = "--- Any ---" });
            //ViewData["Industries"] = industry;

            var indus = _repository.GetIndustriesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            indus.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["Industries"] = indus;

            //ViewData["Industries"] = new MultiSelectList((IEnumerable<Dial4Jobz.Models.Industry>)ViewData["Industries"], "Id", "Name");
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
            //DateTime dateTime = DateTime.Now;
            //var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            job.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            job.OrganizationId = -1;
            if (LoggedInConsultant != null)
            {
                job.ConsultantId = LoggedInConsultant.Id;
            }
            else
            {
               int consultantId = Convert.ToInt32(collection["consultantId"]);
               job.ConsultantId = consultantId;
            }

            if (SetJobDetails(job, collection))
            {
                if (job.EmailAddress == "" && job.MobileNumber == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Please Enter Mobile No or Email Address" });
                }
                else
                {
                    _repository.Save();

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
                    //ReturnUrl = "/CandidateMatches/CandidateMatch/Id=" + job.Id
                    ReturnUrl = "/Admin/CandidateMatches/CandidateMatch/" + job.Id
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
                //if (!string.IsNullOrEmpty(preferredRole))
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

            return true;
        }


        public ActionResult Edit(string id)
        {
            Job job = null;
            if (!string.IsNullOrEmpty(id))
            {
                var jobid = Constants.DecryptString(id);
                job = _repository.GetJob(Convert.ToInt32(jobid));

                if (job == null)
                    return new FileNotFoundResult();

                SetCommonViewData();
                SetEditJobViewData(job);

            }

            return View(job);
        }

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult EditJob(FormCollection collection, int jobId)
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

        public ActionResult PostedJobs(int consultantId)
        {
            if (LoggedInConsultant != null)
            {

            }
            else
            {
                LoggedInConsultant = _userRepository.GetConsultantsById(consultantId);
                ViewData["consultantId"] = consultantId;
            }

            var vacancy = _vasRepository.GetVacanciesConsultant(LoggedInConsultant.Id);
            var status = _vasRepository.GetRatSubscribedForConsultant(LoggedInConsultant.Id);
            var postedJobs = _vasRepository.GetJobsByConsultantIdAlert(LoggedInConsultant.Id);

            if (status == true && vacancy != postedJobs.Count())
            {
                PostedJobAlert postedJobalert = new PostedJobAlert();

                foreach (Dial4Jobz.Models.Job job in LoggedInConsultant.Jobs)
                {
                    postedJobalert = _vasRepository.GetPostedJobAlert(LoggedInConsultant.Id, job.Id);

                    if (postedJobalert != null && postedJobalert.Vacancies > 0)
                    {
                        //check condition of posted jobs then update the minimum value into vacancies
                        if (postedJobs.Count() <= vacancy)
                        {
                            var vacancyCount = postedJobalert.Vacancies - postedJobs.Count();
                            postedJobalert.Vacancies = vacancyCount;
                            _vasRepository.Save();
                        }
                    }

                }

            }

            var consultant = _userRepository.GetConsultantsById(LoggedInConsultant.Id);
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", consultant.IndustryId);

            return View(consultant);
        }

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult ActivateRATVacancy(FormCollection collection)
        {
            var vacancies = _vasRepository.GetVacanciesConsultant(LoggedInConsultant.Id);
            var selectedKeyCount = 0;
            var postedJobs = _vasRepository.GetJobsByConsultantIdAlert(LoggedInConsultant.Id);
            bool isSuccess = false;
            DateTime currentdate = Constants.CurrentTime().Date;
            OrderDetail orderdetail = null;
            PostedJobAlert postedJobalert = null;
            double vasplanDays = 0;

            if (LoggedInConsultant != null)
            {
                foreach (string key in collection.AllKeys)
                {
                    if (key.Contains("Job"))
                    {
                        int jobId = Convert.ToInt32(key.Replace("Job", string.Empty));
                        var activatedvacancy = _vasRepository.GetConsultantPostedAlert(LoggedInConsultant.Id, jobId);

                        var value = collection.AllKeys.Count();

                        var selectedKeyword = Convert.ToInt32(collection.GetValues(key).Contains("true"));
                        selectedKeyCount = selectedKeyCount + selectedKeyword;

                        if (selectedKeyCount <= vacancies)
                        {
                            if (postedJobs.Count() < vacancies)
                            {
                                if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                                {
                                    Job job = _repository.GetJob(jobId);
                                    PostedJobAlert jobs = _vasRepository.GetJobIdByPostedJobAlert(jobId);

                                    //already posted jobs
                                    if (jobs != null)
                                    {
                                        if (jobs.JobId != jobId)
                                        {
                                            Response.Write("<script language=javascript>alert('You have already activated this vacancy');</script>");
                                        }
                                    }

                                    else
                                    {
                                        OrderDetail validityPlan = _vasRepository.GetValidityRATConsultant(LoggedInConsultant.Id);
                                        if (validityPlan != null)
                                        {
                                            _vasRepository.UpdateVASDetailsConsultant(LoggedInConsultant.Id, jobId);

                                            postedJobalert = _vasRepository.GetConsultantPostedAlert(LoggedInConsultant.Id, jobId);
                                            orderdetail = _vasRepository.GetOrderDetailsRATUpdate(Convert.ToInt32(postedJobalert.OrderId));
                                            postedJobalert.Vacancies = vacancies;
                                            postedJobalert.AlertActivateDate = Constants.CurrentTime();
                                            vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                                            postedJobalert.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
                                            postedJobalert.RemainingCount = 25;
                                            _vasRepository.Save();
                                        }
                                        else
                                            Response.Write("<script language=javascript>alert('Your Plan has been finished. You cannot assign Vacancy');</script>");
                                      
                                        EmailHelper.SendEmail(
                                            Constants.EmailSender.EmployerSupport,
                                            LoggedInConsultant.Email,
                                            Constants.EmailSubject.ActivateVacancy,
                                            Constants.EmailBody.ActivateVacancy
                                            .Replace("[NAME]", LoggedInConsultant.Name)
                                            .Replace("[POSITION]", job.Position)
                                            .Replace("[ORDER_NO]", orderdetail.OrderId.ToString())
                                            .Replace("[PLAN]", orderdetail.VasPlan.PlanName)
                                            .Replace("[VALIDITY_COUNT]", (orderdetail.BasicCount != null ? orderdetail.BasicCount.ToString() : orderdetail.ValidityCount.ToString()))
                                            .Replace("[START_DATE]", postedJobalert.AlertActivateDate.Value.ToString("dd-MM-yyyy"))
                                            .Replace("[END_DATE]", postedJobalert.ValidityTill.Value.ToString("dd-MM-yyyy"))
                                            .Replace("[VAC_ALERT]", "25")
                                            .Replace("[NOTICE]", "Consultants")
                                            .Replace("[LINK_NAME]", "Your DashBoard")
                                            .Replace("[DASHBOARD_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/MatchCandidates")
                                            );

                                        SmsHelper.SendSecondarySms(
                                            Constants.SmsSender.SecondaryUserName,
                                            Constants.SmsSender.SecondaryPassword,
                                            Constants.SmsBody.ActivateVacancy
                                            .Replace("[NAME]", LoggedInConsultant.Name)
                                            .Replace("[POSITION]", job.Position)
                                             .Replace("[PLAN]", orderdetail.VasPlan.PlanName)
                                            .Replace("[VALIDITY_COUNT]", orderdetail.ValidityCount.ToString())
                                            .Replace("[VALIDITY_TILL]", postedJobalert.ValidityTill.Value.ToString("dd-MM-yyyy")),
                                            Constants.SmsSender.SecondaryType,
                                            Constants.SmsSender.Secondarysource,
                                            Constants.SmsSender.Secondarydlr,
                                            LoggedInConsultant.MobileNumber
                                            );
                                        //}
                                        isSuccess = true;

                                    }

                                }
                            }
                        }

                    }


                }



            }
            return RedirectToAction("PostedJobs", "Consult");

        }
        

        #region Data

        public ActionResult ImportData()
        {

            return View();
        }

        public ActionResult Download()
        {
            string fileName = "27c73e57-fb0a-405b-8bea-a0f0eaba9f78Candidates.xls";
            string pfn = Server.MapPath("~/Content/Data/" + fileName);
            if (!System.IO.File.Exists(pfn))
            {
                return Json(new JsonActionResult { Success = false, Message = "Invalid file name or file not exists!" });
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


        [HandleErrorWithAjaxFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportData(HttpPostedFileBase uploadFile)
        {
            string filePath = string.Empty;

            if (uploadFile.ContentLength > 0)
            {
                var fileGUID = Guid.NewGuid();
                filePath = Path.Combine(HttpContext.Server.MapPath("~/Areas/Admin/Content/Data/"),
                                               Path.GetFileName(fileGUID + uploadFile.FileName));

                string folderPath = HttpContext.Server.MapPath("~/Areas/Admin/Content/Data/");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                uploadFile.SaveAs(filePath);
            }

            bool transResult = Insert2SQL(filePath);
            ViewData["ImportStatus"] = transResult;
            return View();
        }





        private bool Insert2SQL(string filePath)
        {

            OleDbConnection oconn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0");
            try
            {
                OleDbCommand ocmd = new OleDbCommand("select * from [Candidates$]", oconn);
                oconn.Open();
                OleDbDataReader odr = ocmd.ExecuteReader();

                // define our transaction scope
                var scope = new TransactionScope(
                    // a new transaction will always be created
                    TransactionScopeOption.RequiresNew,
                    // we will allow volatile data to be read during transaction
                    new TransactionOptions()
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                );

                using (scope)
                {
                    while (odr.Read())
                    {
                        insertdataintosql(odr);
                    }

                    scope.Complete();
                }

                oconn.Close();


            }
            catch (DataException ee)
            {
                throw ee;
            }

            return true;
        }

        private string valid(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
            {
                string[] splitValue = val.ToString().Split('-');
                if (splitValue.Count() == 2)
                    return Convert.ToString(splitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                return Convert.ToString(0);

            }
        }

        private string validLocation(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
            {
                string[] splitValue = val.ToString().Split('-');
                if (splitValue.Count() == 2)
                    return Convert.ToString(splitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                //return Convert.ToString(0);
                return null;
            }
        }

        private string validateValue(string stval)
        {
            object val = stval;
            if (val != DBNull.Value)
            {
                string[] subSplitValue = val.ToString().Split('-');
                if (subSplitValue.Count() == 2)
                    return Convert.ToString(subSplitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                return Convert.ToString(0);
                //return null;
            }
        }

        private void insertdataintosql(OleDbDataReader odr)
        {
            //UserRepository _userRepository = new UserRepository();
            Repository _repository = new Repository();
            Candidate candidate = new Candidate();
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            string excelId = valid(odr, 0);

            if (excelId != "0")
            {
                candidate.Name = valid(odr, 1);
                candidate.Email = validLocation(odr, 2);
                candidate.ContactNumber = valid(odr, 4);
                candidate.MobileNumber = valid(odr, 5);
                candidate.Description = valid(odr, 30);

                Candidate candidateEmail = _userRepository.GetCandidateByEmail(candidate.Email);
                Candidate candidatemobile = _userRepository.GetCandidateByMobileNumber(candidate.ContactNumber);

                if (candidateEmail != null)
                {

                    //ViewData["ImportStatus"] += candidate.Email + ",";
                }
                else if (candidatemobile != null)
                {
                    //ViewData["ImportStatus"] += candidate.MobileNumber + ",";
                }
                else
                {
                    candidate.Address = valid(odr, 3);
                    candidate.Pincode = valid(odr, 29);
                    candidate.Description = valid(odr, 30);
                    candidate.LicenseNumber = valid(odr, 22);
                    candidate.PassportNumber = valid(odr, 23);

                    Random randomNo = new Random();
                    candidate.UserName = candidate.Name + randomNo.Next(1000, 9999).ToString();

                    string password = GetRandomPasswordUsingGUID(6);
                    candidate.Password = SecurityHelper.GetMD5Bytes(password);

                    if (candidate.CreatedDate == null)
                    {
                        candidate.CreatedDate = timeZone;
                    }
                    else
                    {
                        candidate.UpdatedDate = timeZone;
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 6)) && valid(odr, 6) != "0")
                        candidate.DOB = Convert.ToDateTime(valid(odr, 6));

                    if (!string.IsNullOrEmpty(valid(odr, 31)) && valid(odr, 31) != "0")
                        candidate.MaritalId = Convert.ToInt32(valid(odr, 31));

                    if (!string.IsNullOrEmpty(valid(odr, 7)) && valid(odr, 7) != null)
                        candidate.Gender = Convert.ToInt32(valid(odr, 7));

                    if (!string.IsNullOrEmpty(valid(odr, 8)) && valid(odr, 8) != "0")
                    {
                        string[] expSplit = valid(odr, 8).Trim().Split('.');
                        long yearsinseconds = 0;
                        long monthsinseconds = 0;

                        if (expSplit.Length > 0)
                            yearsinseconds = Convert.ToInt64(expSplit[0]) * 365 * 24 * 60 * 60;
                        if (expSplit.Length > 1)
                            monthsinseconds = Convert.ToInt64(expSplit[1]) * 31 * 24 * 60 * 60;

                        candidate.TotalExperience = yearsinseconds + monthsinseconds;
                    }

                    candidate.NumberOfCompanies = Convert.ToInt32(valid(odr, 28));
                    candidate.AnnualSalary = (Convert.ToInt32(valid(odr, 9)));
                    candidate.Position = valid(odr, 10);
                    candidate.PresentCompany = valid(odr, 26);
                    candidate.PreviousCompany = valid(odr, 27);

                    if (!string.IsNullOrEmpty(valid(odr, 11)) && valid(odr, 11) != "0")
                        candidate.FunctionId = Convert.ToInt32(valid(odr, 11));

                    if (!string.IsNullOrEmpty(valid(odr, 12)) && valid(odr, 12) != "0")
                        candidate.IndustryId = Convert.ToInt32(valid(odr, 12));

                    if (!string.IsNullOrEmpty(valid(odr, 14)) && valid(odr, 14) != "0")
                    {
                        if (valid(odr, 14) == "0")
                            candidate.PreferredAll = false;
                        else
                            candidate.PreferredAll = Convert.ToBoolean(valid(odr, 14));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 15)))
                    {
                        if (valid(odr, 15) == "0")
                            candidate.PreferredContract = false;
                        else
                            candidate.PreferredContract = Convert.ToBoolean(valid(odr, 15));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 16)))
                    {
                        if (valid(odr, 16) == "0")
                            candidate.PreferredParttime = false;
                        else
                            candidate.PreferredParttime = Convert.ToBoolean(valid(odr, 16));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 17)))
                    {
                        if (valid(odr, 17) == "0")
                            candidate.PreferredWorkFromHome = false;
                        else
                            candidate.PreferredWorkFromHome = Convert.ToBoolean(valid(odr, 17));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 18)))
                    {
                        if (valid(odr, 18) == "0")
                            candidate.PreferredFulltime = false;
                        else
                            candidate.PreferredFulltime = Convert.ToBoolean(valid(odr, 18));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 19)) && valid(odr, 19) != "0")
                        //candidate.PreferredTimeFrom = Convert.ToInt16(valid(odr, 19));
                        candidate.PreferredTimeFrom = valid(odr, 19);

                    if (!string.IsNullOrEmpty(valid(odr, 20)) && valid(odr, 20) != "0")
                        //candidate.PreferredTimeTo = Convert.ToInt16(valid(odr, 20));
                        candidate.PreferredTimeTo = valid(odr, 20);

                    _repository.AddCandidate(candidate);
                    //_repository.Save();

                    Location location = new Location();
                    if (!string.IsNullOrEmpty(validLocation(odr, 32))) location.CountryId = Convert.ToInt32(validLocation(odr, 32));
                    if (!string.IsNullOrEmpty(validLocation(odr, 33))) location.StateId = Convert.ToInt32(validLocation(odr, 33));
                    if (!string.IsNullOrEmpty(validLocation(odr, 34))) location.CityId = Convert.ToInt32(validLocation(odr, 34));
                    if (!string.IsNullOrEmpty(validLocation(odr, 35))) location.RegionId = Convert.ToInt32(validLocation(odr, 35));

                    if (location.CountryId != 0)
                        candidate.LocationId = _repository.AddLocation(location);

                    int candidateId = candidate.Id;


                    //Candidates skills
                    string[] skills = valid(odr, 36).Split(',');

                    if (skills.Count() != 0)
                        _repository.DeleteCandidateSkills(candidateId);

                    foreach (string skill in skills)
                    {
                        if (!string.IsNullOrEmpty(skill) && skill != "0")
                        {
                            CandidateSkill cs = new CandidateSkill();
                            cs.CandidateId = candidateId;
                            cs.SkillId = Convert.ToInt32(validateValue(skill));

                            _repository.AddCandidateSkill(cs);
                        }
                    }

                    string[] languages = valid(odr, 37).Split(',');
                    if (languages.Count() != null)
                        //if (languages.Count() != 0)
                        _repository.DeleteCandidateLanguages(candidateId);

                    foreach (string lang in languages)
                    {
                        if (!string.IsNullOrEmpty(lang) && lang != "0")
                        {
                            CandidateLanguage cl = new CandidateLanguage();
                            cl.CandidateId = candidateId;
                            cl.LanguageId = Convert.ToInt32(validateValue(lang));
                            _repository.AddCandidateLanguage(cl);
                        }
                    }

                    //preferredFunctions

                    string[] preferredFunctions = valid(odr, 41).Split(',');
                    if (preferredFunctions.Count() != null)
                        _repository.DeleteCandidatePreferredFunctions(candidateId);

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


                    //Roles

                    string[] roles = valid(odr, 38).Split(',');

                    if (roles.Count() != null)
                    //if (roles.Count() != 0)
                    {
                        _repository.DeleteCandidateRoles(candidateId);
                    }

                    foreach (string preferredRole in roles)
                    {
                        if (!string.IsNullOrEmpty(preferredRole) && preferredRole != "0")
                        {
                            CandidatePreferredRole cpr = new CandidatePreferredRole();
                            cpr.CandidateId = candidateId;
                            cpr.RoleId = Convert.ToInt32(validateValue(preferredRole));
                            _repository.AddCandidatePreferredRole(cpr);
                        }
                    }

                    //licence types
                    string[] licenseTypes = { };
                    if (!string.IsNullOrEmpty(valid(odr, 39)))
                        licenseTypes = valid(odr, 39).Split(',');

                    //delete all license types
                    if (licenseTypes.Count() != null)
                        //if (licenseTypes.Count() != 0)
                        _repository.DeleteCandidateLicenseTypes(candidateId);

                    //then add new ones
                    foreach (string licenseType in licenseTypes)
                    {
                        if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
                        {
                            CandidateLicenseType clt = new CandidateLicenseType();
                            clt.CandidateId = candidateId;
                            clt.LicenseTypeId = Convert.ToInt32(validateValue(licenseType));
                            _repository.AddCandidateLicenseType(clt);
                        }
                    }

                    //Add Candidate Qualification (first clear the existing ones)
                    _repository.DeleteCandidateQualifications(candidate.Id);

                    //qualification
                    string[] qualifications = { };
                    if (!string.IsNullOrEmpty(valid(odr, 40)))
                        qualifications = valid(odr, 40).Split(',');

                    //delete all qualification types

                    if (qualifications.Count() != null)
                        //if (qualifications.Count() != 0)
                        _repository.DeleteCandidateQualifications(candidateId);

                    //then add new ones
                    foreach (string qualification in qualifications)
                    {

                        if (!string.IsNullOrEmpty(qualification) && qualification != "0")
                        {
                            CandidateQualification cq = new CandidateQualification();
                            cq.CandidateId = candidateId;
                            cq.DegreeId = Convert.ToInt32(validateValue(qualification));
                            //  cq.Specialization = string.Empty;

                            if (cq.DegreeId > 0)
                                _repository.AddCandidateQualification(cq);
                        }
                    }

                    _repository.Save();

                    if (candidate.Email != null)
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.CandidateSupport,
                           candidate.Email,
                            //candidate.username,
                           Constants.EmailSubject.Registration,
                           Constants.EmailBody.AdminCandidateRegister
                               .Replace("[NAME]", candidate.Name)
                               .Replace("[USERNAME]", candidate.UserName)
                               .Replace("[PASSWORD]", password)
                               .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Home/HomePage")
                               .Replace("[EMAIL]", candidate.Email)
                               );
                    }

                    //if (candidate.ContactNumber != "0")
                    //{
                    //    SmsHelper.Sendsms(
                    //        Constants.SmsSender.UserId,
                    //        Constants.SmsSender.Password,
                    //        Constants.SmsBody.SMSNewCandidate
                    //                        .Replace("[USER_NAME]", candidate.UserName)
                    //                        .Replace("[PASSWORD]", password),
                    //        Constants.SmsSender.Type,
                    //        Constants.SmsSender.senderId,
                    //        candidate.ContactNumber
                    //        );
                    //}
                }

            }

        }

        public string GetRandomPasswordUsingGUID(int length)
        {
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            return guidResult.Substring(0, length);
        }

        #endregion

    }

}
