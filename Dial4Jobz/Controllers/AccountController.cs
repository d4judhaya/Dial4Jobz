using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Helpers;
using System.Configuration;

namespace Dial4Jobz.Controllers
{
    [HandleErrorWithAjaxFilter]
    public class AccountController : BaseController
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

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        [HandleNonAjaxRequest]
        public ActionResult LogOn()
        {
            return View("Login");
        }

        //[HttpPost]
        //[ValidateInput(true)]
        ////[ValidateAntiForgeryToken]
        //public ActionResult LogOn(LogOnModel model, string returnUrl)
        //{
            
        //    var candidate = _repository.GetCandidateByUserName(model.UserName);
        //    returnUrl = Request.UrlReferrer.AbsoluteUri.ToLower();

          
        //    Session["PlanId"] = "";
        //    if (model.loginAs == "Employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
        //    {                
        //        Session["LoginAs"] = "Employer";
        //        Session["UserName"] = model.UserName;
        //       // returnUrl = VirtualPathUtility.ToAbsolute("~/employer/matchcandidates");
        //        Session["ShowPopup"] = false;
        //    }
        //    else if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin"))
        //    {
        //        Session["LoginAs"] = "Admin";
        //        Session["UserName"] = model.UserName;
        //        returnUrl = VirtualPathUtility.ToAbsolute("~/admin");
        //    }
        //    else
        //    {
        //        Session["LoginAs"] = "Candidate";
        //        Session["UserName"] = model.UserName;
        //        Session["CandId"] = _repository.GetCandidateId(model.UserName);
        //        returnUrl = VirtualPathUtility.ToAbsolute("/candidate");
        //       // returnUrl = Request.UrlReferrer.AbsoluteUri.ToLower();
                
        //    }

        //    bool userValidated = false;

        //    try
        //    {
        //        if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin"))
        //        {
        //            userValidated = MembershipService.ValidateUser(model.UserName, model.Password);
        //        }
        //        else
        //        {
        //            userValidated = string.IsNullOrEmpty(returnUrl) ? MembershipService.ValidateCandidate(model.UserName, model.Password) :
        //                                                             MembershipService.ValidateOrganization(model.UserName, model.Password);
        //        }
        //        //userValidated = string.IsNullOrEmpty(returnUrl) ? MembershipService.ValidateCandidate(model.UserName, model.Password) :
        //                                                          //MembershipService.ValidateOrganization(model.UserName, model.Password);

        //        if (!userValidated)
        //            ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //    }
        //    catch (UnauthorizedAccessException uae)
        //    {
        //        return Json(new JsonActionResult { Success = false, Message = uae.Message });
        //    }

        //    if (!ModelState.IsValid)
        //        return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            
        //    FormsService.SignIn(model.UserName, model.RememberMe);

        //        return Json(new JsonActionResult { Success = true, Message = "Successfully logged in", ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/Candidates/DashBoard") : returnUrl });

        //}


        // **************************************
        // URL: /Account/LogOff
        // **************************************


        [HttpPost]
        [ValidateInput(true)]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            var candidate = _repository.GetCandidateByUserName(model.UserName);
            returnUrl = Request.UrlReferrer.AbsoluteUri.ToLower();
         
            Session["PlanId"] = "";
            if (model.loginAs == "Employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
            {
                Session["LoginAs"] = "Employer";
                Session["UserName"] = model.UserName;
                Session["ShowPopup"] = false;
            }

            else if (model.loginAs == "Candidate" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("candidate"))
            {
                Session["LoginAs"] = "Candidate";
                Session["UserName"] = model.UserName;
                Session["CandId"] = _repository.GetCandidateId(model.UserName);
            }
          
            else
            {
                Session["LoginAs"] = "Admin";
                Session["UserName"] = model.UserName;
                returnUrl = VirtualPathUtility.ToAbsolute("~/admin");
            }
          

            bool userValidated = false;

            try
            {
                if (model.loginAs == "Employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
                {
                    userValidated = MembershipService.ValidateOrganization(model.UserName, model.Password);
                }
              
                else if (model.loginAs == "Admin" && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin"))
                {
                    userValidated = MembershipService.ValidateUser(model.UserName, model.Password);
                }

                else
                {
                    userValidated = MembershipService.ValidateCandidate(model.UserName, model.Password);
                }
             

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

            return Json(new JsonActionResult { Success = true, Message = "Successfully logged in", ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/Candidates/DashBoard") : returnUrl });
           
        }


        public ActionResult LogOff()
        {
          
           // string returnUrl = Request.UrlReferrer.AbsoluteUri.ToLower();
            if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
            {
                FormsService.SignOut();
               // return Json(new JsonActionResult { Success = true, Message = "Successfully Logged Out", ReturnUrl = returnUrl });
                return RedirectToAction("MatchCandidates", "Employer");
            }
            else if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer/candidates/details"))
            {
                FormsService.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
            //return Json(new JsonActionResult { Success = true, Message = "Successfully Logged Out", ReturnUrl = returnUrl });
        }


        
        //[HttpPost]
        //public ActionResult RegisterFromFindJobseekers(LogOnModel model, string returnUrl)
        //{
        //    Organization organizationmobile = _userRepository.GetOrganizationByMobileNumber(model.mobileNumber);

        //    Organization organizationEmail=_userRepository.GetOrganizationByEmail(model.emailId);

        //    string username = model.employerName;

        //CheckUserName: Organization organizationName = _userRepository.GetOrganizationByUserName(username);

        //    if (organizationName == null)
        //        goto ValidUser;
        //    else
        //    {
        //        username = username + "9";
        //        goto CheckUserName;
        //    }

        //ValidUser: if (organizationmobile != null)
        //    return Json(new JsonActionResult { Success = false, Message = "Mobile Number is already registered. Please Login.." });

        //         if (organizationEmail != null)
        //             return Json(new JsonActionResult { Success = false, Message = "Email Id is already registered. Please Login.." });

        //         if (model.employerName == null)
        //             return Json(new JsonActionResult { Success = false, Message = "Name is Required" });
        //         else
        //         {
        //             Random randomNo = new Random();
        //             string phVerficationNo = randomNo.Next(1000, 9999).ToString();
        //             string password = "abc" + phVerficationNo;

        //             MembershipCreateStatus createStatus;


        //             createStatus = MembershipService.CreateOrganization(username, password, model.emailId, model.mobileNumber, model.employerName, model.contactpersonName, 2332, phVerficationNo);

        //             if (createStatus != MembershipCreateStatus.Success)
        //                 ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));


        //             FormsService.SignIn(username, false /* createPersistentCookie */);

        //             Session["LoginAs"] = "Employer";
        //             Session["UserName"] = username;

        //             if (model.role != null)
        //             {
        //                 Session["ShowPopup"] = true;
        //             }
        //             else
        //             {
        //                 Session["ShowPopup"] = false;
        //             }

        //             Session["PlanId"] = "";
        //             Session["Validity"] = "";
        //             Session["empId"] = _repository.GetEmployerId(username);
                     
        //             Session["remainingcount"] = "0";


        //             int postJobStatus = PostJobFromRole(model);


        //             returnUrl = null;
        //             return Json(new JsonActionResult
        //             {
        //                 Success = true,
        //                 Message = model.UserName,
        //                 ReturnUrl = returnUrl
        //             });
        //             //}
        //             //else
        //             //{
        //             //    return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
        //             //}
        //         }
        //}

        //Post job
        [HttpPost]
        public int PostJobFromRole(LogOnModel model)
        {
            Organization organizationmobile = _userRepository.GetOrganizationByMobileNumber(model.mobileNumber);

            if (organizationmobile == null)
                return 0;
            else
            {
                Job job = new Job();
                job.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                //job.OrganizationId = -1;
                job.OrganizationId = organizationmobile.Id;
                job.Position = model.role;
                bool status = PostJobonProceed(job, model);
                if (status == true)
                    return 1;
                else
                    return 0;
            }
        }

        
        public bool PostJobonProceed(Job job, LogOnModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(model.minExperience)))
                {
                    long yearsInSecondsFrom = Convert.ToInt64(model.minExperience) * 365 * 24 * 60 * 60;
                    job.MinExperience = yearsInSecondsFrom;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(model.maxExperience)))
                {
                    long yearsInSecondsTo = Convert.ToInt64(model.maxExperience) * 365 * 24 * 60 * 60;
                    job.MaxExperience = yearsInSecondsTo;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(model.minSalary)))
                {
                    job.Budget = (double)model.minSalary * 12;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(model.maxSalary)))
                {
                    job.MaxBudget = (double)model.maxSalary * 12;
                }

                job.ContactPerson = model.contactpersonName;
                job.ContactNumber = model.mobileNumber;
                job.MobileNumber = model.mobileNumber;
                job.EmailAddress = model.emailId;

                //add the job and get the job id, so that we can add the foreign tables data.
                int jobId = job.Id > 0 ? job.Id : _repository.AddJob(job);

                Session["JobId"] = jobId;

                // //Roles
                string role = model.role;

                if (role != null)
                    _repository.DeleteJobRoles(jobId);


                if (!string.IsNullOrEmpty(role))
                {
                    JobRole jr = new JobRole();
                    jr.JobId = jobId;
                    int roleId = _repository.GetRoleId(role);
                    jr.RoleId = Convert.ToInt32(roleId);
                    _repository.AddJobRole(jr);
                    _repository.Save();
                }


                //add posting locations for job
                //  _repository.DeleteJobLocations(jobId)
                return true;
            }
            catch
            {
                return false;
            }


        }

        


        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult SignUp()
        {
            var value = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                value = Request.QueryString[0];
            }

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            if(value=="employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name");

            /**Developer Note: To select a particular Industries for Employer Type**/
            List<SelectListItem> consultantindustries = new List<SelectListItem>();
            consultantindustries.Add(new SelectListItem { Text = "Home Needs", Value = "2378" });
            //ViewData["ConsultantIndustries"] = new SelectList(_repository.GetIndustries(), "Id", "Name");
            ViewData["ConsultantIndustries"] = new SelectList(consultantindustries, "Value", "Text");
            /*End*/

            return View("Register");
        }


        [HttpPost, HandleNonAjaxRequest]
        public ActionResult SignUp(RegisterModel model, string returnUrl, FormCollection collection, string candidatemobilenumber)
        {

            var value = string.Empty;
          
            if (Request.QueryString.Count > 0)
            {
                value = Request.QueryString[0];
            }

            candidatemobilenumber = collection["MobileNumber"];

            Organization organizationmobile = null;
            Candidate candidatemobile = null;

            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            var organizationName = _userRepository.GetOrganizationByName(collection["Name"]);

            var industry = collection["Industries"];
            var consultIndustry = collection["ConsultantIndustries"];
            int industryId = 0;
            if (industry != "")
            {
                industryId = Convert.ToInt32(industry);
            }
            else
            {
                industryId = Convert.ToInt32(consultIndustry);
            }
            //Convert.ToInt32(collection["Industries"])

            if (industryId == 2412)
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "You are not an Employer.Please Register as a Consultant",
                    ReturnUrl = "/Consult/Register"
                });

            if (organizationName != null)
                return Json(new JsonActionResult { Success = false, Message = "Company Name is already Registered. Please pick a different Name" });

            if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
            {
                organizationmobile = _userRepository.GetOrganizationByMobileNumber(candidatemobilenumber);
                Session["LoginAs"] = "Employer";
                returnUrl = "/Employer/PhoneNoVerification/";
            }
            else if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin"))
            {
                returnUrl = VirtualPathUtility.ToAbsolute("~/admin");
            }
            else if (value == "candidate" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("candidate"))
            {
                returnUrl = null;
                Session["LoginAs"] = "Candidate";
                candidatemobile = _userRepository.GetCandidateByMobileNumber(candidatemobilenumber);
            }


            if (organizationmobile != null)
                return Json(new JsonActionResult { Success = false, Message = "Mobile.No is already registered as a Employer" });

            else if (candidatemobile != null)
                return Json(new JsonActionResult { Success = false, Message = "Mobile.No is already registered as a Candidate" });

            else
            {
                Random randomNo = new Random();
                string phVerficationNo = randomNo.Next(1000, 9999).ToString();

                MembershipCreateStatus createStatus;
               
                if (value == "employer" || Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer"))
                {
                    createStatus = MembershipService.CreateOrganization(model.UserName, model.Password, model.Email, collection["MobileNumber"], collection["Name"], collection["ContactPerson"], industryId, phVerficationNo,model.EmployerType);
                }
                else
                {
                    createStatus =  MembershipService.CreateCandidate(model.UserName, model.Password, model.Email, collection["Name"], collection["MobileNumber"], phVerficationNo);
                }


                if (createStatus != MembershipCreateStatus.Success)
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));

                if (!ModelState.IsValid)
                    return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

                FormsService.SignIn(model.UserName, true /* createPersistentCookie */);

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = model.UserName,
                    //ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/") : returnUrl
                    ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/Candidates/PhoneNoVerification/") : returnUrl
                });
            }
        }

        
        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View("ChangePassword");
        }

        [Authorize]
        public ActionResult ChangeCandidatePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        public ActionResult ChangeOrganizationPassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        [Authorize,HttpPost, HandleNonAjaxRequest]
        public ActionResult ChangeCandidatePassword(ChangePasswordModel model)
        {
           
            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            else
                MembershipService.ChangeCandidatePassword(User.Identity.Name, model.OldPassword, model.NewPassword);

            EmailHelper.SendEmail(
                Constants.EmailSender.EmployerSupport,
                LoggedInCandidate.Email,
                Constants.EmailSubject.PasswordReset,
                Constants.EmailBody.ChangePassword
                .Replace("[NAME]", LoggedInCandidate.Name)
                .Replace("[USERNAME]", LoggedInCandidate.UserName)
                .Replace("[RESETPASSWORD]","http://www.dial4jobz.in/home/index")
                .Replace("[IMPORTANT_NOTICE]", "The Information on Vacancy & Employer Shared/ sent/ displayed to you is as communicated or furnished by the Employer over telephone/ Internet and it shall be the sole responsibility of the Candidate before attending Interview or joining the Employer to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the Employer.")
                );
           

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            //return View(model);
            return Json(new JsonActionResult { Success = false, Message = "Password has been changed Successfully" });
        }


        [Authorize,HttpPost,HandleNonAjaxRequest]
        public ActionResult ChangeOrganizationPassword(ChangePasswordModel model)
        {
         
            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            else
                MembershipService.ChangeOrganizationPassword(User.Identity.Name, model.OldPassword, model.NewPassword);

            EmailHelper.SendEmail(
                Constants.EmailSender.EmployerSupport,
                LoggedInOrganization.Email,
                Constants.EmailSubject.PasswordReset,
                Constants.EmailBody.ChangePassword
                .Replace("[NAME]", LoggedInOrganization.Name)
                .Replace("[USERNAME]", LoggedInOrganization.UserName)
                .Replace("[RESETPASSWORD]","http://www.dial4jobz.in/Employer/MatchCandidates")
                .Replace("[IMPORTANT_NOTICE]", "The Information on candidates Shared/ sent/ displayed to you is as communicated or furnished by the Candidate over telephone/ Internet and it shall be the sole responsibility of the Employer before appointing them to check, authenticate and verify the information/ response received . Dial4Jobz India Private Ltd is not responsible for false information given by the candidate.")
                );


                 

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            //return View(model);
            return Json(new JsonActionResult { Success = false, Message = "Password has been changed Successfully" });
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************


        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        //*****URL:/Account/ForgotPassword*****
        [HandleNonAjaxRequest]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost, HandleNonAjaxRequest]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            object user;
            string randomString = SecurityHelper.GenerateRandomString(6, true);
            byte[] password = SecurityHelper.GetMD5Bytes(randomString);
            string name = String.Empty;
            string username = String.Empty;
            string email = null;
            string contactnumber = null;
            string mobilenumber = null;
        
            email = collection["Email"];
            contactnumber = collection["ContactNumber"];
            mobilenumber = collection["MobileNumber"];

            //if (mobilenumber != "" && contactnumber != "")
            //    email = null;
            //else if (email!="")
            //    contactnumber = null;
            //    mobilenumber = null;
               
            

            var value = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                value = Request.QueryString[0];
            }
            
            
            if(value!="employer" && !IsEmployer)
            {
                if (value == "consultant")
                {
                    if (mobilenumber != null)
                    {
                        user = (Consultante)_userRepository.GetConsultantMobile(mobilenumber);
                        if (user == null)
                            return Json(new JsonActionResult { Success = false, Message = "Your Mobile Number is not Registered with Us." });
                        ((Consultante)user).Password = password;
                        name = ((Consultante)user).Name;
                        name = ((Consultante)user).UserName;
                    }
                    else
                    {
                        user = (Consultante)_userRepository.GetConsultantEmail(email);

                        if (user == null)
                            return Json(new JsonActionResult { Success = false, Message = "This Email id is not registered with us.." });

                        ((Consultante)user).Password = password;
                        name = ((Consultante)user).Name;
                        username = ((Consultante)user).UserName;
                    }
                }
                else
                {
                    contactnumber= mobilenumber;
                    if (contactnumber != null)
                    {

                        user = (Candidate)_userRepository.GetCandidateByMobileNumber(contactnumber);
                        if (user == null)
                            return Json(new JsonActionResult { Success = false, Message = "This Contact Number is not registered with us.." });

                        ((Candidate)user).Password = password;
                        name = ((Candidate)user).Name;
                        username = ((Candidate)user).UserName;

                    }
                    else
                    {
                        user = (Candidate)_userRepository.GetCandidateByEmail(email);

                        if (user == null)
                            return Json(new JsonActionResult { Success = false, Message = "This Email id is not registered with us.." });

                        ((Candidate)user).Password = password;
                        name = ((Candidate)user).Name;
                        username = ((Candidate)user).UserName;
                    }
                }
                
            }
            
            else
            {
                if (mobilenumber!=null)
                {

                    user = (Organization)_userRepository.GetOrganizationByMobileNumber(mobilenumber);
                    if (user == null)
                        return Json(new JsonActionResult { Success = false, Message = "This Contact Number is not registered with us.." });

                    ((Organization)user).Password = password;
                    name = ((Organization)user).Name;
                    username = ((Organization)user).UserName;
                   
                }

                else
                {
                    user = (Organization)_userRepository.GetOrganizationByEmail(email);

                    if (user == null)
                        return Json(new JsonActionResult { Success = false, Message = "This Email id is not registered with us.." });

                    ((Organization)user).Password = password;
                    name = ((Organization)user).Name;
                    username = ((Organization)user).UserName;
                }
                
            }

           
            

            _userRepository.Save();
            
            if (value != "employer" && !IsEmployer)
            {

                if (value == "consultant")
                {
                    if (email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                        email,
                        Constants.EmailSubject.PasswordReset,
                        Constants.EmailBody.PasswordResetforCandidate.Replace("[NAME]", name)
                                  .Replace("[USERNAME]", username)
                                  .Replace("[PASSWORD]", randomString));
                    }

                    else
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.ResetPassword
                            .Replace("[NAME]", name)
                            .Replace("[USERNAME]", username)
                            .Replace("[PASSWORD]", randomString),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            mobilenumber
                            );
                    }
                }
                else
                {
                    if (email != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                                                email,
                                                Constants.EmailSubject.PasswordReset,
                                                Constants.EmailBody.PasswordResetforCandidate.Replace("[NAME]", name)
                                                                                 .Replace("[USERNAME]", username)
                                                                                 .Replace("[PASSWORD]", randomString));
                    }

                    else
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.ResetPassword
                            .Replace("[NAME]", name)
                            .Replace("[USERNAME]", username)
                            .Replace("[PASSWORD]", randomString),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            contactnumber
                            );
                    }
                }
            }
            
            else
            {
                if (email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                    email,
                    Constants.EmailSubject.PasswordReset,
                    Constants.EmailBody.PasswordReset.Replace("[NAME]", name)
                    .Replace("[USERNAME]", username)
                    .Replace("[PASSWORD]", randomString));
                }
                else
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.ResetPassword
                        .Replace("[NAME]", name)
                        .Replace("[USERNAME]", username)
                        .Replace("[PASSWORD]", randomString),
                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        mobilenumber
                        );
                }
            }


            return Json(new JsonActionResult { Success = true, Message = "Your password has been reset Successfully. Check Your Email / MobileNumber" });
        }      

    }
}
