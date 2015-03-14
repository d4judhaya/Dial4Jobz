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


namespace Dial4Jobz.Areas.Channel.Controllers
{
    public class ChannelHomeController : BaseController
    {
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        ChannelRepository _channelrepository = new ChannelRepository();

        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
        public int maxLength = int.MaxValue;
        public string[] AllowedFileExtensions;
        public string[] AllowedContentTypes;
        List<string> _filters = new List<string>();

        #region Home
        // GET: /ChannelHome/Home/Index

        [ChannelAuthorize(Roles = "1,2")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region User       

        #region Candidate

        public ActionResult ConsultantDetail()
        {
            return View();
        }

        [ChannelAuthorize(Roles = "1,2")]
        public ActionResult AddCandidate()
        {
            Candidate candidate = new Candidate();

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


        public ActionResult ValidateCandidate(string validateEmail, string validateMobile)
        {
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(validateEmail))
            {
                candidate = _userRepository.GetCandidateByEmail(validateEmail);
                if (candidate == null)
                    return Json("Email Id not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Email Id exist", JsonRequestBehavior.AllowGet);

            }


            else if (!string.IsNullOrEmpty(validateMobile))
            {
                candidate = _userRepository.GetCandidateByMobileNumber(validateMobile);
                if (candidate == null)
                    return Json("Mobile number not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Mobile number exist", JsonRequestBehavior.AllowGet);

            }
            else
            {

            }

            return RedirectToAction("GetDetail", "ChannelHome");


        }

        public ActionResult DirectVerification(string mobileNumber)
        {
            Candidate candidate = _repository.GetCandidateByMobileNumber(mobileNumber);

            candidate.IsPhoneVerified = true;
            _repository.Save();

            return RedirectToAction("GetDetail", new { ValidateMobile = mobileNumber });

        }

        public ActionResult DirectEmployerVerification(string mobileNumber)
        {
            Organization organization = _repository.GetOrganizationMobileNumber(mobileNumber);
            organization.IsPhoneVerified = true;
            _repository.Save();

            return RedirectToAction("GetCompanyDetail", "ChannelHome");
        }


        // Get the details of Candidate
        public ActionResult GetDetail(string validateEmail, string validateMobile)
        {
            Candidate candidate = null;
            
            if (!string.IsNullOrEmpty(validateEmail))
            {
                candidate = _userRepository.GetCandidateByEmail(validateEmail);

                if (candidate == null)
                    candidate = new Candidate();

                else
                {
                    Session["LoginAs"] = "CandidateViaAdmin";
                    Session["CandId"] = candidate.Id;
                    Session["LoginUser"] = "UserViaAdmin";
                    Session["LoginUserId"] = User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];

                }

            }
            else if (!string.IsNullOrEmpty(validateMobile))
            {
                candidate = _userRepository.GetCandidateByMobileNumber(validateMobile);
                if (candidate == null)
                    candidate = new Candidate();

                else
                {
                    Session["LoginAs"] = "CandidateViaAdmin";
                    Session["CandId"] = candidate.Id;
                    Session["LoginUser"] = "UserViaAdmin";
                    Session["LoginUserId"] = User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
                }
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

        [HttpPost]
        public ActionResult UploadResume(IEnumerable<HttpPostedFileBase> FileData)
        {

            foreach (var file in FileData)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Resumes/"), fileName);
                    file.SaveAs(path);
                    var timeoutsession = Session.Timeout;
                    if (Session["LoginAs"] == "CandidateViaAdmin")
                    {
                        int candidateId = (int)Session["CandId"];
                        Candidate candidate = _repository.GetCandidate(candidateId);
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


        //[Authorize,HttpPost, HandleErrorWithAjaxFilter]
        [ChannelAuthorize(Roles = "1,2"), HttpPost, HandleErrorWithAjaxFilter]
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


            // Validations checking

            if (!string.IsNullOrEmpty(collection["Id"]))
            {
                int currentId = Convert.ToInt32(collection["Id"]);

                if (currentId == 0)
                {

                    if ((collection["ContactNumber"]) != "")
                    {
                        var mobileValidate = _repository.GetCandidateByMobileNumber(collection["ContactNumber"]);
                        if (mobileValidate != null)
                        {
                            return Json(new JsonActionResult { Success = false, Message = "Mobile Number is already exists", ReturnUrl = "/Channel/ChannelHome/GetDetail?validateMobile=" + mobileValidate.ContactNumber });
                        }
                    }


                    else if (Name == null || Name == "")
                    {
                        return Json(new JsonActionResult { Success = false, Message = "Name is Required. Please Enter the Name" });
                    }


                    else if (collection["Email"] != "")
                    {
                        var emailValidate = _userRepository.GetCandidateByEmail(collection["Email"]);
                        if (emailValidate != null)
                        {
                            return Json(new JsonActionResult { Success = false, Message = "Email Id is already Exist", ReturnUrl = "/Channel/ChannelHome/GetDetail?validateEmail=" + emailValidate.Email });
                        }
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

                    candidate = new Candidate();
                }
                else
                {
                    candidate = _repository.GetCandidate(currentId);
                    updateOperation = true;
                }
            }
            else
            {
                candidate = new Candidate();
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


            if (updateOperation == false)
            {
                ChannelEntry channelentry = new ChannelEntry();

                if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "1")
                    channelentry.ChannelPartnerId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);
                else if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "2")
                    channelentry.ChannelUserId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);

                channelentry.EntryId = candidate.Id;
                channelentry.EntryType = Convert.ToInt32(EntryType.Candidate);
                channelentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                _channelrepository.AddChannelEntry(channelentry);
                _channelrepository.Save();

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
                }
            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Profile has been updated",
                ReturnUrl = "/Admin/JobMatches/JobMatch/" + candidateId.ToString()
            });

        }

        #region candidate verification sms & email

        public ActionResult VerifyCandidateMobileNumber(string mobileNumber)
        {
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                candidate = _userRepository.GetCandidateByMobileNumber(mobileNumber);
                if (candidate == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);
                else
                {
                    Random randomNo = new Random();
                    string verificationNumber = randomNo.Next(1111, 9989).ToString();
                    candidate.PhoneVerificationNo = Convert.ToInt32(verificationNumber);
                    _userRepository.Save();

                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSMobileVerification
                                        .Replace("[PIN_NUMBER]", verificationNumber.ToString())
                                        .Replace("[NAME]", candidate.Name),

                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        candidate.ContactNumber
                        );

                }
            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "MobileNumber verified successfully",
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/ChannelHome/AddCandidate/")

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCandidateNumber(string validateMobile)
        {
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(validateMobile))
            {
                User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
                candidate = _userRepository.GetCandidateByMobileNumber(validateMobile);
                Session["LoginAs"] = "CandidateViaAdmin";
                Session["CandId"] = candidate.Id;
                Session["LoginUser"] = "UserViaAdmin";
                Session["LoginUserId"] = user.UserName;

                if (candidate == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (candidate == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);
            }
            return View("AddCandidate", "ChannelHome");

        }

        public ActionResult ConfirmCandidateMobileVerification(int confirmCode)
        {
            Candidate candidate = null;

            if (confirmCode != null)
            {
                candidate = _repository.GetCandidateByMobileCode(confirmCode);
                candidate.PhoneVerificationNo = Convert.ToInt32(confirmCode);
                candidate.IsPhoneVerified = true;
                _repository.Save();
            }
            else
            {

            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "MobileNumber verified successfully",
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/ChannelHome/AddCandidate/")

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CandidateEmailVerification(string email)
        {
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(email))
            {
                candidate = _userRepository.GetCandidateByEmail(email);
                if (candidate == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);
                else
                {
                    EmailHelper.SendEmail(
                            Constants.EmailSender.EmployerSupport,
                            candidate.Email,
                            Constants.EmailSubject.EmailVerification,
                            Constants.EmailBody.EmailVerification
                                .Replace("[EMAIL]", candidate.Email)
                                .Replace("[LINK_NAME]", "Activate this Link")
                                .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Channel/ChannelHome/CandidateEmailActivation?Id=" + candidate.Id.ToString())
                                );
                }
            }

            return RedirectToAction("AddCandidate", "ChannelHome");
        }

        public ActionResult GetCandidateEmail(string validateEmail)
        {
            Candidate candidate = null;

            if (!string.IsNullOrEmpty(validateEmail))
            {
                User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
                candidate = _userRepository.GetCandidateByMobileNumber(validateEmail);
                Session["LoginAs"] = "CandidateViaAdmin";
                Session["CandId"] = candidate.Id;
                Session["LoginUser"] = "UserViaAdmin";
                Session["LoginUserId"] = user.UserName;
                if (candidate == null)
                    candidate = new Candidate();
            }
            else
            {
                if (candidate == null)
                    candidate = new Candidate();
            }
            return View("AddCandidate", "ChannelHome");

        }

        public ActionResult CandidateEmailActivation(int Id)
        {
            Candidate candidate = null;

            if (Id != null)
            {
                candidate = _userRepository.GetCandidateById(Id);
                candidate.IsMailVerified = true;
                _userRepository.Save();
            }
            else
            {

            }
            return View();
        }

        #endregion

        #endregion


        #region Employer

        public ActionResult ValidateCompany(string validateCompany, string validateEmail, string validateMobileNumber)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateCompany))
            {
                organization = _userRepository.GetOrganizationByName(validateCompany);
                if (organization == null)
                    return Json("Company not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Company exist", JsonRequestBehavior.AllowGet);


            }

            else if (!string.IsNullOrEmpty(validateEmail))
            {
                organization = _userRepository.GetOrganizationByEmail(validateEmail);
                if (organization == null)
                    return Json("Email not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Email exist", JsonRequestBehavior.AllowGet);
            }

            else if (!string.IsNullOrEmpty(validateMobileNumber))
            {
                organization = _userRepository.GetOrganizationByMobileNumber(validateMobileNumber);
                if (organization == null)
                    return Json("Mobile not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Mobile exist", JsonRequestBehavior.AllowGet);
            }

            else
            {
                if (organization == null)
                    return Json("Please enter company name", JsonRequestBehavior.AllowGet);
            }

            return Json("Please enter company name", JsonRequestBehavior.AllowGet);
        }



        public ActionResult ValidateEmail(string validateEmail)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateEmail))
            {
                organization = _userRepository.GetOrganizationByEmail(validateEmail);

                if (organization == null)

                    return Json("Email not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Email exist", JsonRequestBehavior.AllowGet);

            }
            else
            {
                if (organization == null)
                    return Json("Please enter EmailId", JsonRequestBehavior.AllowGet);
            }

            return Json("Please enter EmailId", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateMobile(string validateMobile)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateMobile))
            {
                organization = _userRepository.GetOrganizationByMobileNumber(validateMobile);
                if (organization == null)
                    return Json("Mobile not exist", JsonRequestBehavior.AllowGet);
                else
                {


                    return Json("MobileNumber exist", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                if (organization == null)
                    return Json("Please enter Mobile", JsonRequestBehavior.AllowGet);
            }

            return Json("Please enter Mobile", JsonRequestBehavior.AllowGet);
        }


        #region Employer verification email and mobile

        public ActionResult VerifyMobileNumber(string mobileNumber)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                organization = _userRepository.GetOrganizationByMobileNumber(mobileNumber);
                if (organization == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);

                else
                {
                    Random randomNo = new Random();
                    string verificationNumber = randomNo.Next(1000, 9999).ToString();
                    organization.PhoneVerificationNo = Convert.ToInt32(verificationNumber);
                    _userRepository.Save();

                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SMSMobileVerification
                                       .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

                       Constants.SmsSender.SecondaryType,
                       Constants.SmsSender.Secondarysource,
                       Constants.SmsSender.Secondarydlr,
                       organization.MobileNumber
                       );

                }
            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "MobileNumber verified successfully",
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/ChannelHome/AddEmployer/")

            });
        }

        public ActionResult GetMobileNumber(string validateMobile)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateMobile))
            {
                organization = _userRepository.GetOrganizationByMobileNumber(validateMobile);
                User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
                Session["LoginUser"] = user.UserName;
                if (organization == null)
                    organization = new Organization();
                else
                {
                    Session["LoginAs"] = "EmployerViaAdmin";
                    Session["empId"] = organization.Id;
                }
            }
            else
            {
                if (organization == null)
                    organization = new Organization();
            }
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            Location location = organization.LocationId.HasValue ? _repository.GetLocationById(organization.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);
            return View("AddEmployer", organization);
        }

        public ActionResult ConfirmMobileVerification(int confirmCode)
        {
            Organization organization = null;

            if (confirmCode != null)
            {
                organization = _repository.GetOrganizationByMobileCode(confirmCode);
                organization.PhoneVerificationNo = Convert.ToInt32(confirmCode);
                organization.IsPhoneVerified = true;
                _repository.Save();
            }
            else
            {

            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "MobileNumber verified successfully",
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/ChannelHome/AddEmployer/")

            });
        }

        public ActionResult EmailVerification(string email)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(email))
            {
                organization = _userRepository.GetOrganizationByEmail(email);
                if (organization == null)
                    return Json("Profile is not registered", JsonRequestBehavior.AllowGet);
                else
                {
                    EmailHelper.SendEmail(
                            Constants.EmailSender.EmployerSupport,
                            organization.Email,
                            Constants.EmailSubject.EmailVerification,
                            Constants.EmailBody.EmailVerification
                                .Replace("[EMAIL]", organization.Email)
                                .Replace("[LINK_NAME]", "Activate this Link")
                                .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Channel/ChannelHome/Activation?Id=" + organization.Id.ToString())
                                );
                }
            }

            return RedirectToAction("AddEmployer", "ChannelHome");
        }

        public ActionResult GetEmail(string validateEmail)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateEmail))
            {
                organization = _userRepository.GetOrganizationByEmail(validateEmail);
                User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
                Session["LoginUser"] = user.UserName;
                if (organization == null)
                    organization = new Organization();
            }
            else
            {
                if (organization == null)
                    organization = new Organization();
            }

            return View("AddEmployer", organization);

        }




        public ActionResult Activation(int Id)
        {
            Organization organization = null;

            if (Id != null)
            {
                organization = _userRepository.GetOrganizationById(Id);
                organization.IsMailVerified = true;
                _userRepository.Save();
            }
            else
            {

            }
            return View();
        }
        #endregion




        public ActionResult GetCompanyDetail(string validateCompany, string validateEmail, string validateMobileNumber)
        {
            Organization organization = null;

            if (!string.IsNullOrEmpty(validateCompany))
            {
                organization = _userRepository.GetOrganizationByName(validateCompany);
                User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

                if (organization == null)
                    organization = new Organization();

                else
                {
                    Session["LoginAs"] = "EmployerViaAdmin";
                    Session["empId"] = organization.Id;
                    Session["LoginUser"] = user.UserName;
                }
            }

            else if (!string.IsNullOrEmpty(validateEmail))
            {
                organization = _userRepository.GetOrganizationByEmail(validateEmail);

                if (organization == null)
                    organization = new Organization();
                else
                {
                    Session["LoginAs"] = "EmployerViaAdmin";
                    Session["empId"] = organization.Id;
                }
            }

            else if (!string.IsNullOrEmpty(validateMobileNumber))
            {
                organization = _userRepository.GetOrganizationByMobileNumber(validateMobileNumber);

                if (organization == null)
                    organization = new Organization();

                else
                {
                    Session["LoginAs"] = "EmployerViaAdmin";
                    Session["empId"] = organization.Id;
                }
            }

            else
            {
                if (organization == null)
                    organization = new Organization();
            }

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            Location location = organization.LocationId.HasValue ? _repository.GetLocationById(organization.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);

            return View("AddEmployer", organization);

        }

        [ChannelAuthorize(Roles = "1,2")]
        public ActionResult AddEmployer()
        {
            var organization = new Organization();

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            Location location = organization.LocationId.HasValue ? _repository.GetLocationById(organization.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);

            return View(organization);
        }

        [ChannelAuthorize(Roles = "1,2"), HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult SaveEmployer(FormCollection collection)
        {
            Organization organization = null;
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            bool updateOperation = false;

            if (!string.IsNullOrEmpty(collection["Id"]))
            {
                int currentId = Convert.ToInt32(collection["Id"]);
                if (currentId == 0)
                {
                    var name = collection["Name"];

                    var organizationName = _userRepository.GetOrganizationByName(collection["Name"]);

                    if (organizationName != null)
                    {
                        return Json(new JsonActionResult
                        {
                            Success = false,
                            Message = "Company Name is already Registered.",
                            ReturnUrl = "/Channel/ChannelHome/GetCompanyDetail?validateCompany=" + organization.Name
                        });
                    }

                    else if (collection["Email"] != "")
                    {
                        var organizationEmail = _userRepository.GetOrganizationByEmail(collection["Email"]);
                        if (organizationEmail != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Email id is already registered",
                                ReturnUrl = "/Channel/ChannelHome/GetCompanyDetail?validateEmail=" + organizationEmail.Email
                            });

                        }

                        else
                        {
                            organization = new Organization();
                        }
                    }

                    else if (collection["MobileNumber"] != "")
                    {
                        var organizationMobile = _userRepository.GetOrganizationByMobileNumber(collection["MobileNumber"]);
                        if (organizationMobile != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Mobile Number is already registered",
                                ReturnUrl = "/Channel/ChannelHome/GetCompanyDetail?validateMobileNumber=" + organizationMobile.MobileNumber
                            });

                        }
                        else
                        {
                            organization = new Organization();
                        }
                    }

                    else
                    {
                        organization = new Organization();
                    }
                }
                else
                {
                    organization = _repository.GetOrganizationById(currentId);
                    updateOperation = true;
                }
            }
            else
            {
                organization = new Organization();
            }


            if (!string.IsNullOrEmpty(collection["Industries"]))
                organization.IndustryId = Convert.ToInt32(collection["Industries"]);
            organization.Name = collection["Name"];
            organization.ContactPerson = collection["ContactPerson"];
            organization.Email = collection["Email"];
            organization.Website = collection["Website"];
            organization.ContactNumber = collection["ContactNumber"];
            organization.MobileNumber = collection["MobileNumber"];
            organization.Address = collection["Address"];
            organization.Pincode = collection["Pincode"];


            if (organization.CreateDate == null)
            {
                organization.CreateDate = timeZone;
            }
            else
            {
                organization.UpdateDate = timeZone;
            }

            // Generation of Username and password start

            Random randomPassword = new Random();
            string pwdDob = string.Empty;
            string firstname = string.Empty;
            string randomString = string.Empty;

            string username = randomPassword.Next(1111, 2222).ToString();

            string fullname = organization.Name;
            string contactperson = organization.ContactPerson;

            var names = fullname.Split(' ');

            if (names.Count() > 0)
            {
                firstname = names[0];
            }

            else
            {
                firstname = contactperson;
            }


            if (updateOperation == false)
            {
                if (!string.IsNullOrEmpty(collection["Name"]))
                    organization.UserName = firstname + username;

                else
                    organization.UserName = collection["ContactPerson"];

                var usernameExists = _userRepository.GetOrganizationByUserName(organization.UserName);
                if (usernameExists != null)
                {
                    organization.UserName = collection["ContactNumber"] + username;
                }


                randomString = SecurityHelper.GenerateRandomString(6, true);
                byte[] password = SecurityHelper.GetMD5Bytes(randomString);
                organization.Password = password;


                string phVerficationNo = randomPassword.Next(1000, 9999).ToString();
                organization.PhoneVerificationNo = Convert.ToInt32(phVerficationNo);
            }




            Location location = new Location();
            if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
            if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
            if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
            if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

            if (location.CountryId != 0)
                organization.LocationId = _repository.AddLocation(location);

            int organizationId = organization.Id;
            
            if (!TryValidateModel(organization))
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            if (updateOperation == false)
                _repository.AddOrganization(organization);

            _repository.Save();

            if (updateOperation == false)
            {
                ChannelEntry channelentry = new ChannelEntry();

                if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "1")
                    channelentry.ChannelPartnerId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);
                else if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "2")
                    channelentry.ChannelUserId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);

                channelentry.EntryId = organization.Id;
                channelentry.EntryType = Convert.ToInt32(EntryType.Employer);
                channelentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                _channelrepository.AddChannelEntry(channelentry);
                _channelrepository.Save(); 


                if (organization.Email != "")
                {

                    EmailHelper.SendEmail(
                          Constants.EmailSender.EmployerSupport,
                          organization.Email,
                          Constants.EmailSubject.Registration,
                          Constants.EmailBody.ClientRegister
                              .Replace("[NAME]", organization.Name)
                              .Replace("[USER_NAME]", organization.UserName)
                              .Replace("[PASSWORD]", randomString)
                              .Replace("[EMAIL]", organization.Email)
                              .Replace("[LINK_NAME]", "Verify Here")
                              .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                              );
                }


                if (organization.MobileNumber != "")
                {
                    SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.SMSCandidateRegister
                                                .Replace("[USER_NAME]", organization.UserName)
                                            .Replace("[PASSWORD]", randomString)
                                           .Replace("[CODE]", organization.PhoneVerificationNo.ToString()),

                                Constants.SmsSender.SecondaryType,
                                Constants.SmsSender.Secondarysource,
                                Constants.SmsSender.Secondarydlr,
                                organization.MobileNumber
                                );

                }
            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Employer Added Successfully",
                ReturnUrl = "/Channel/ChannelHome/GetCompanyDetail/ " + organization.Name.ToString()
            });

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


        #endregion

        public ActionResult AddCollectionDetails()
        {
            return View();
        }
                
        #endregion


        #region channelAccounts

        #endregion
    }
}
