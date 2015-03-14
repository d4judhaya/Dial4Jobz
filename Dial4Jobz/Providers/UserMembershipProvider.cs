using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Helpers;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Filters;
using System.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations;



namespace Dial4Jobz.Models
{
    public class UserMembershipProvider : MembershipProvider
    {
        UserRepository _userRepository = new UserRepository();

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            User user = _userRepository.GetByUserName(username);
            //user.Email=
            if (SecurityHelper.GetMD5String(user.Password) != SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(oldPassword)))
                return false;
            else
            {
                user.Password = SecurityHelper.GetMD5Bytes(newPassword);
                _userRepository.Save();
                return true;
            }
        }

        public bool ChangeCandidatePassword(string username, string oldPassword, string newPassword)
        {
            Candidate candidate = _userRepository.GetCandidateByUserName(username);

            if (candidate == null)
                return false;

            if (SecurityHelper.GetMD5String(candidate.Password) != SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(oldPassword)))
                return false;
            else
            {
                candidate.Password = SecurityHelper.GetMD5Bytes(newPassword);
                _userRepository.Save();
                return true;
            }
        }

        public bool ChangeOrganizationPassword(string username, string oldPassword, string newPassword)
        {
            Organization organization = _userRepository.GetOrganizationByUserName(username);

            if (organization == null)
                return false;

            if (SecurityHelper.GetMD5String(organization.Password) != SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(oldPassword)))
                return false;
            else
            {
                organization.Password = SecurityHelper.GetMD5Bytes(newPassword);
                _userRepository.Save();
                return true;
            }
        }


        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                MembershipUser membershipUser = null;
                User user = _userRepository.GetByUserName(username);

                if (user != null)
                    status = MembershipCreateStatus.DuplicateUserName;
                else
                {
                    user = new User();
                    user.UserName = username;
                    user.Email = email;
                    user.Password = SecurityHelper.GetMD5Bytes(password);
                    user.CreatedDate = DateTime.Now;
                    _userRepository.Add(user);
                    _userRepository.Save();
                    membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                    status = MembershipCreateStatus.Success;
                }

                return membershipUser;
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                throw ex;
            }
        }

       
        public MembershipUser CreateCandidate(string username, string password, string email, string name, string mobilenumber, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status, string phVerficationNo)
        {
            try
            {
                MembershipUser membershipUser = null;

                Candidate candidate = _userRepository.GetCandidateByUserName(username);

                Candidate candidateEmail = _userRepository.GetCandidateByEmail(email);

                //Candidate candidateMobile = _userRepository.GetCandidateByMobileNumber(mobilenumber);


                if (candidate != null)
                
                    status = MembershipCreateStatus.DuplicateUserName;
                
                 else if (candidateEmail != null)
                 
                   status = MembershipCreateStatus.DuplicateEmail;

                //else if(candidateMobile!=null)
                    
                   //return Json(new JsonActionResult { Success = true, Message = "Mobile Number is already Exists. Please use different Mobile Number"});

               
                else
                {

                    candidate = new Candidate();
                    candidate.UserName = username;
                    candidate.Password = SecurityHelper.GetMD5Bytes(password);
                    candidate.Email = email;
                    candidate.ContactNumber = mobilenumber;
                    candidate.Name = name;
                    candidate.PhoneVerificationNo = Convert.ToInt32(phVerficationNo);
                    candidate.IsPhoneVerified = false;
                    candidate.IsMailVerified = false;
                    candidate.CreatedDate = Constants.CurrentTime();
                    //candidate.IPAddress = Request.ServerVariables["REMOTE_ADDR"];

                    _userRepository.Add(candidate);
                    _userRepository.Save();
                    membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                    status = MembershipCreateStatus.Success;


                    EmailHelper.SendEmail(
                    Constants.EmailSender.CandidateSupport,
                    candidate.Email,
                        //candidate.username,
                    Constants.EmailSubject.Registration,
                    Constants.EmailBody.CandidateRegister
                        .Replace("[NAME]", candidate.Name)
                        .Replace("[USER_NAME]", username)
                        .Replace("[PASSWORD]", password)
                        .Replace("[EMAIL]", candidate.Email)
                       .Replace("[LINK_NAME]", "Verify your E-mail ID")
                        .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                        );

                    SmsHelper.SendSecondarySms(
                           Constants.SmsSender.SecondaryUserName,
                           Constants.SmsSender.SecondaryPassword,
                           Constants.SmsBody.SMSCandidateRegister
                                       .Replace("[USER_NAME]", username)
                                       .Replace("[PASSWORD]", password)
                                       .Replace("[CODE]", phVerficationNo.ToString()),

                       Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                       candidate.ContactNumber
                       );

                   
                }

                return membershipUser;
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                throw ex;
            }
        }

        public MembershipUser CreateOrganization(string username, string password, string email, string mobilenumber, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status, string name, string contactPerson, int industryId, string phVerficationNo,int employerType)
        {
            try
            {
                MembershipUser membershipUser = null;
                Organization organization = _userRepository.GetOrganizationByUserName(username);
                Organization organizationEmail = _userRepository.GetOrganizationByEmail(email);
                //Organization organizationMobile=_userRepository.getor

                if (organization != null)
                    status = MembershipCreateStatus.DuplicateUserName;

                else if (organizationEmail != null)
                    status = MembershipCreateStatus.DuplicateEmail;
                //else if(

                else
                {
                    organization = new Organization();
                    organization.UserName = username;
                    organization.Email = email;
                    organization.Password = SecurityHelper.GetMD5Bytes(password);
                    organization.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //candidate.CreatedDate = DateTime.Now;
                    organization.Name = name;
                    organization.ContactPerson = contactPerson;
                    organization.MobileNumber = mobilenumber;
                    organization.EmployerType = employerType;
                    organization.IndustryId = industryId;
                    organization.IsPhoneVerified = false;
                    organization.IsMailVerified = false;
                    organization.PhoneVerificationNo = Convert.ToInt32(phVerficationNo);
                    _userRepository.Add(organization);
                    _userRepository.Save();
                    membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                    status = MembershipCreateStatus.Success;


                    EmailHelper.SendEmail(
                    Constants.EmailSender.EmployerSupport,
                    organization.Email,
                    Constants.EmailSubject.Registration,
                    Constants.EmailBody.ClientRegister
                              .Replace("[NAME]", organization.Name)
                              .Replace("[USER_NAME]", username)
                              .Replace("[PASSWORD]", password)
                              .Replace("[LINK_NAME]", "Verify your E-mail ID")
                              .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                              );

                    if (mobilenumber != null)
                    {
                        SmsHelper.SendSecondarySms(
                               Constants.SmsSender.SecondaryUserName,
                               Constants.SmsSender.SecondaryPassword,
                               Constants.SmsBody.SMSCandidateRegister
                                            .Replace("[USER_NAME]", username)
                                            .Replace("[PASSWORD]", password)
                                            .Replace("[CODE]", phVerficationNo.ToString()),

                             Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                            organization.MobileNumber
                            );

                    }
                    else
                    {

                    }


                }

                
                return membershipUser;
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                throw ex;
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = _userRepository.GetByUserName(username);
            MembershipUser membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return membershipUser;
        }

        public MembershipUser GetCandidate(string username, bool userIsOnline)
        {
            Candidate candidate = _userRepository.GetCandidateByUserName(username);
            MembershipUser membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return membershipUser;
        }

        public MembershipUser GetOrganization(string username, bool userIsOnline)
        {
            Organization organization = _userRepository.GetOrganizationByUserName(username);
            MembershipUser membershipUser = new MembershipUser("UserMembershipProvider", username, null, null, String.Empty, String.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return membershipUser;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            User user = _userRepository.GetByUserName(username);

            if (user == null)
                return false;

            if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(user.Password))
                return true;

            return false;
        }

        public bool ValidateCandidate(string username, string password)
        {
            Candidate candidate = _userRepository.GetCandidateByUserName(username);

            if (candidate == null)
                return false;

            if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(candidate.Password))
                return true;

            return false;
        }

        public bool ValidateOrganization(string username, string password)
        {
            Organization organization = _userRepository.GetOrganizationByUserName(username);

            if (organization == null)
                return false;

            if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(organization.Password))
                return true;

            return false;
        }

        public bool ValidateConsultant(string username, string password)
        {
            Consultante consultant = _userRepository.GetConsultantsByUserName(username);

            if (consultant == null)
                return false;

            if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(consultant.Password))
                return true;

            return false;
        }


    }
}