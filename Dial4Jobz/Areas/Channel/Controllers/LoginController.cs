using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Results;
using System.Web.Security;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Helpers;
using Dial4Jobz.Models.Filters;

namespace Dial4Jobz.Areas.Channel.Controllers
{
    public class LoginController : Controller
    {
        ChannelRepository _channelrepository = new ChannelRepository();       

        //
        // GET: /Channel/Login/

        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Login(ChannelLoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            ChannelPartner channelpartner = _channelrepository.GetValidChannelPartner(model.UserName, model.Password);
            ChannelUser channeluser = _channelrepository.GetValidChannelUser(model.UserName, model.Password);
            if (channelpartner != null)
            {
                FormsAuthentication.SetAuthCookie((int)ChannelRoles.ChannelPartner + "|" + channelpartner.Id + "|" + channelpartner.UserName + "|" + channelpartner.Email, model.RememberMe);

                return Json(new JsonActionResult { Success = true, Message = "Successfully logged in", ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/Channel/ChannelHome/Index") : returnUrl });
            }
            else if (channeluser != null)
            {
                FormsAuthentication.SetAuthCookie((int)ChannelRoles.ChannelUser + "|" + channeluser.Id + "|" + channeluser.UserName + "|" + channeluser.Email, model.RememberMe);

                return Json(new JsonActionResult { Success = true, Message = "Successfully logged in", ReturnUrl = returnUrl == null ? VirtualPathUtility.ToAbsolute("~/Channel/ChannelHome/Index") : returnUrl });
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = "The email or password provided is incorrect." });
            }

        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, HandleNonAjaxRequest]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            string randomString = SecurityHelper.GenerateRandomString(6, true);
            byte[] password = SecurityHelper.GetMD5Bytes(randomString);
            string email = null;
            string contactnumber = null;

            email = collection["Email"];
            contactnumber = collection["MobileNumber"];

            if (contactnumber != "" && contactnumber != null)
                email = null;
            else
                contactnumber = null;

            ChannelPartner channelpartner = new ChannelPartner();
            ChannelUser channeluser = new ChannelUser();

            if (contactnumber != null)
            {
                channelpartner = _channelrepository.GetChannelPartnersByMobileNumber(contactnumber).FirstOrDefault();
                channeluser = _channelrepository.GetChannelUsersByMobileNumber(contactnumber).FirstOrDefault();

                if (channelpartner != null)
                {
                    channelpartner.Password = password;
                    
                }
                else if (channeluser != null)
                {
                    channeluser.Password = password;
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "This Contact Number is not registered with us.." });
                }
            }
            else
            {
                channelpartner = _channelrepository.GetChannelPartnersbyEmail(email).FirstOrDefault();
                channeluser = _channelrepository.GetChannelUsersbyEmail(email).FirstOrDefault();

                if (channelpartner != null)
                {
                    channelpartner.Password = password;

                }
                else if (channeluser != null)
                {
                    channeluser.Password = password;
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "This Email id is not registered with us.." });
                }
            }

            _channelrepository.Save(); 
            
            if (channelpartner != null)
            {
                if (email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                                            email,
                                            Constants.EmailSubject.PasswordReset,
                                            Constants.EmailBody.PasswordResetforCandidate.Replace("[NAME]", channelpartner.UserName)
                                                                             .Replace("[USERNAME]", channelpartner.Email)                        
                                                                             .Replace("[PASSWORD]", randomString));
                }
                else
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.ResetPassword
                        .Replace("[NAME]", channelpartner.UserName)
                        .Replace("[USERNAME]", channelpartner.Email)
                        .Replace("[PASSWORD]", randomString),
                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        contactnumber
                        );
                }
            }
            else if (channeluser != null)
            {
                if (email != null)
                {
                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                                        email,
                                        Constants.EmailSubject.PasswordReset,
                                        Constants.EmailBody.PasswordReset.Replace("[NAME]", channeluser.UserName)
                                                                         .Replace("[USERNAME]", channeluser.Email)
                                                                        .Replace("[PASSWORD]", randomString));
                }
                else
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.ResetPassword
                        .Replace("[NAME]", channeluser.UserName)
                        .Replace("[USERNAME]", channeluser.Email)
                        .Replace("[PASSWORD]", randomString),
                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        contactnumber
                        );
                }
            }

            return Json(new JsonActionResult { Success = true, Message = "Your password has been reset Successfully. Check Your Email / MobileNumber" });
        }      


        #region Change password

        [ChannelAuthorize(Roles = "1,2")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [ChannelAuthorize(Roles = "1,2")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "1")
            {
                ChannelPartner channelpartner = _channelrepository.GetChannelPartner(Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]));

                if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(model.OldPassword)) == SecurityHelper.GetMD5String(channelpartner.Password))
                {
                    channelpartner.Password = SecurityHelper.GetMD5Bytes(model.NewPassword);
                    _channelrepository.Save();
                    return Json(new JsonActionResult { Success = true, Message = "Password Changed Successfully" });
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "Old Password is Incorrect" });
                }
            }
            else if (this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole] == "2")
            {
                ChannelUser channeluser = _channelrepository.GetChannelUser(Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]));
                if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(model.OldPassword)) == SecurityHelper.GetMD5String(channeluser.Password))
                {
                    channeluser.Password = SecurityHelper.GetMD5Bytes(model.NewPassword);
                    _channelrepository.Save();
                    return Json(new JsonActionResult { Success = true, Message = "Password Changed Successfully" });
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "Old Password is Incorrect" });
                }
            }

            return Json(new JsonActionResult { Success = false, Message = "Unable to Change Password" });

        }

        #endregion

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

    }
}
