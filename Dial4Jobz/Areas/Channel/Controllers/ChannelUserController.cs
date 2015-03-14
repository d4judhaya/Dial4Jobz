using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;
using Dial4Jobz.Models.Filters;
using Dial4Jobz.Helpers;
using System.IO;
using System.Net;

namespace Dial4Jobz.Areas.Channel.Controllers
{
    public class ChannelUserController : Controller
    {
        static readonly IChannelRepository _channelrepository = new ChannelRepository();
        VasRepository _vasRepository = new VasRepository();

        #region Channel User CRUD
        [ChannelAuthorize(Roles="1")]
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult ListChannelUsers(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            IQueryable<ChannelUser> channeluserResult = _channelrepository.GetChannelUsersbyPartner(Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]));

            Func<IQueryable<ChannelUser>, IOrderedQueryable<ChannelUser>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.UserName);
                    else if (iSortCol_0 == 3)
                        return query.OrderByDescending(rslt => rslt.Email);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.ContactNo);                    
                    else
                        return query.OrderByDescending(rslt => rslt.Id);
                }
                else
                {
                    if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.UserName);
                    else if (iSortCol_0 == 3)
                        return query.OrderBy(rslt => rslt.Email);
                    else if (iSortCol_0 == 4)
                        return query.OrderBy(rslt => rslt.ContactNo);
                    else
                        return query.OrderBy(rslt => rslt.Id);

                }

            };

            channeluserResult = orderingFunc(channeluserResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                channeluserResult = channeluserResult.Where(ch => ch.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || ch.Email.ToLower().Contains(sSearch.ToLower().Trim()) || ch.ContactNo.ToLower().Contains(sSearch.ToLower().Trim()));

            IEnumerable<ChannelUser> channeluserResult1 = channeluserResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = channeluserResult.Count(),
                iTotalDisplayRecords = channeluserResult.Count(),
                aaData = channeluserResult1.Select(u => new object[] { "", u.UserName, "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Channel/ChannelUser/ChangePassword/" + u.Id + "'>Change</a>", u.Email, u.ContactNo, "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Channel/ChannelUser/Edit/" + u.Id + "'><img alt='Edit' src='/Areas/Admin/Content/Images/icn_edit.png' /></a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='DeleteChannelUserId' value='" + u.Id + "' />" }).Skip(iDisplayStart).Take(iDisplayLength)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ChannelAuthorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        [ChannelAuthorize(Roles = "1")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(ChannelUser channeluser, string Pwd)
        {
            var channelusers = _channelrepository.GetChannelUsersbyUserName(channeluser.UserName).ToList();
            var channelusersbyemail = _channelrepository.GetChannelUsersbyEmail(channeluser.Email).ToList();

            var channelpartners = _channelrepository.GetChannelPartnersbyEmail(channeluser.Email).ToList();
            
            Random randomNo = new Random();
            string phVerficationNo = randomNo.Next(2000, 2999).ToString();

            if (channelusers.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "User Name Already Exists" });                
            }
            else if(channelusersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }
            else if (channelpartners.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }

            channeluser.ChannelPartnerId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);
            channeluser.Password = SecurityHelper.GetMD5Bytes(Pwd);
            channeluser.PhoneVerificationNo = Convert.ToInt32(randomNo);
            channeluser.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            

            _channelrepository.AddChannelUser(channeluser);
            _channelrepository.Save();

            SendAccountMail(channeluser, Pwd);
            
            return Json(new JsonActionResult { Success = true, Message = "Channel User Added successfully" });

        }

        private void SendAccountMail(ChannelUser channeluser, string Pwd)
        {
            if (channeluser.Email != "")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Areas/Channel/Views/MailTemplate/ChannelUserRegistration.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[CHANNELUSERNAME]", channeluser.UserName);
                table = table.Replace("[EMAILID]", channeluser.Email);
                table = table.Replace("[MOBILE]", channeluser.ContactNo);
                table = table.Replace("[PASSWORD]", Pwd);
                table = table.Replace("[CHANNELPARTNERNAME]", channeluser.ChannelPartner.UserName);

                EmailHelper.SendEmailSBCC(
                      Constants.EmailSender.EmployerSupport,
                      channeluser.Email,
                      channeluser.ChannelPartner.Email,
                      "smc@dial4jobz.com",
                      "ganesan@dial4jobz.com",
                      Constants.EmailSubject.ChannelUserRegistration,
                      table);
            }

            if (channeluser.ContactNo != "")
            {
                SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.SMSCandidateRegister
                                            .Replace("[USER_NAME]", channeluser.Email)
                                        .Replace("[PASSWORD]", Pwd)
                                       .Replace("[CODE]", ""),

                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            channeluser.ContactNo
                            );
            }
        }

        [ChannelAuthorize(Roles = "1")]
        public ActionResult Edit(int id = 0)
        {
            ChannelUser channeluser = _channelrepository.GetChannelUser(id);

            if (channeluser == null)
                return new FileNotFoundResult();

            return View(channeluser);
        }

        [ChannelAuthorize(Roles = "1")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(ChannelUser user)
        {
            var channelusers = _channelrepository.GetChannelUsersbyUserName(user.UserName).Where(u => u.UserName.ToLower() == user.UserName.ToLower() && u.Id != user.Id).ToList();
            var channelusersbyemail = _channelrepository.GetChannelUsersbyEmail(user.Email).Where(u => u.Email.ToLower() == user.Email.ToLower() && u.Id != user.Id).ToList();

            var channelpartners = _channelrepository.GetChannelPartnersbyEmail(user.Email).ToList();

            if (channelusers.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "User Name Already Exists" });
            }
            else if (channelusersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }
            else if (channelpartners.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }            

            ChannelUser channeluser = _channelrepository.GetChannelUser(user.Id);
            channeluser.UserName = user.UserName;
            channeluser.Email = user.Email;
            channeluser.ContactNo = user.ContactNo;
           
            channeluser.ChannelPartnerId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);            

            _channelrepository.Save();            

            return Json(new JsonActionResult { Success = true, Message = "Channel User Updated successfully" });

        }

        [ChannelAuthorize(Roles = "1")]
        public ActionResult ChangePassword(int id = 0)
        {
            ChannelUser channeluser = _channelrepository.GetChannelUser(id);

            if (channeluser == null)
                return new FileNotFoundResult();

            return View(channeluser);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChangePassword(ChannelUser user, string Pwd)
        {
            ChannelUser channeluser = _channelrepository.GetChannelUser(user.Id);
            channeluser.Password = SecurityHelper.GetMD5Bytes(Pwd);
            _channelrepository.Save();            
            
            return Json(new JsonActionResult { Success = true, Message = "Channel User Password Changed successfully" });
        }

        [HttpPost]
        public JsonResult Delete(string userIds)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                string[] ids = userIds.Split(',');
                if (ids != null && ids.Length > 0)
                {
                    foreach (string id in ids)
                    {
                        _channelrepository.DeleteChannelUser(Convert.ToInt32(id));
                    }
                    _channelrepository.Save();
                }
                else
                {                    
                    return Json(new JsonActionResult { Success = false, Message = "Unable to Delete Channel User" });
                }
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = "Unable to Delete Channel User" });
            }

            return Json(new JsonActionResult { Success = true, Message = "Channel User Deleted Successfully" });
        }
        #endregion

        #region Channel User Report
        [ChannelAuthorize(Roles = "1")]
        public ActionResult UserReport()
        {
            return View();
        }

        public JsonResult ListChannelUserEntries(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IEnumerable<ChannelUser> channeluserResult = _channelrepository.GetChannelUsersbyPartnerWithEntries(Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId])).ToList();

            IQueryable<OrderMaster> ordemasterResult = _vasRepository.GetOrderMasters();

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                channeluserResult = channeluserResult.Where(cu => cu.UserName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                channeluserResult = channeluserResult.Where(c => c.ChannelEntries.Any(ce => ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date));

                Func<ChannelUser, object> order1 = rslt =>
                {
                    if (iSortCol_0 == 2)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else if (iSortCol_0 == 3)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else if (iSortCol_0 == 4)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else
                        return rslt.Email;
                };

                if ("desc" == sSortDir_0)
                    channeluserResult = channeluserResult.OrderByDescending(order1);
                else
                    channeluserResult = channeluserResult.OrderBy(order1);

                string fromDate1 = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                string toDate1 = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate1).Date;
                var to = DateTime.Parse(toDate1).Date;

                to = to.AddHours(23.99);

                var result1 = new
                {
                    iTotalRecords = channeluserResult.Count(),
                    iTotalDisplayRecords = channeluserResult.Count(),
                    aaData = channeluserResult.Select(cu => new object[] { "", cu.UserName, 
                        cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        ordemasterResult.Where(or => or.SubscribedBy == cu.Email && or.OrderDate != null && or.OrderDate.Value >= from && or.OrderDate.Value <= to).Count()
                    }).Skip(iDisplayStart).Take(iDisplayLength)
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Func<ChannelUser, object> order = rslt =>
                {
                    if (iSortCol_0 == 2)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count();
                    else if (iSortCol_0 == 3)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count();
                    else if (iSortCol_0 == 4)
                        return rslt.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count();
                    else
                        return rslt.Email;
                };

                if ("desc" == sSortDir_0)
                    channeluserResult = channeluserResult.OrderByDescending(order);
                else
                    channeluserResult = channeluserResult.OrderBy(order);

            }

            var result = new
            {
                iTotalRecords = channeluserResult.Count(),
                iTotalDisplayRecords = channeluserResult.Count(),
                aaData = channeluserResult.Select(cu => new object[] { "", cu.UserName, cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count(), ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count() }).Skip(iDisplayStart).Take(iDisplayLength).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Channel My Report
        [ChannelAuthorize(Roles = "1,2")]
        public ActionResult MyReport()
        {
            IQueryable<OrderMaster> ordemasterResult = _vasRepository.GetOrderMasters();
            int ChannelId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);
            if (Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole]) == (int)ChannelRoles.ChannelPartner)
            {
                var cu = _channelrepository.GetChannelPartnersWithEntries().Where(c => c.Id == ChannelId).FirstOrDefault();
                ViewData["CandidateAdded"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count();
                ViewData["EmployerAdded"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count();
                ViewData["JobPosted"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count();
                ViewData["PlanSubscribed"] = ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count();
            }
            else
            {
                var cu = _channelrepository.GetChannelUsersWithEntries().Where(c => c.Id == ChannelId).FirstOrDefault();
                ViewData["CandidateAdded"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count();
                ViewData["EmployerAdded"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count();
                ViewData["JobPosted"] = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count();
                ViewData["PlanSubscribed"] = ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count();
            }        
            return View();
        }

        public JsonResult GetMyReport(string fromDate, string toDate)
        {
            string CandidateAdded = "0";
            string EmployerAdded = "0";
            string JobPosted = "0";
            string PlanSubscribed = "0";

            string fromDate1 = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
            string toDate1 = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

            var from = DateTime.Parse(fromDate1).Date;
            var to = DateTime.Parse(toDate1).Date;

            to = to.AddHours(23.99);

            IQueryable<OrderMaster> ordemasterResult = _vasRepository.GetOrderMasters();
            int ChannelId = Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelId]);
            if (Convert.ToInt32(this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole]) == (int)ChannelRoles.ChannelPartner)
            {
                var cu = _channelrepository.GetChannelPartnersWithEntries().Where(c => c.Id == ChannelId).FirstOrDefault();
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    CandidateAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    EmployerAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    JobPosted = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    PlanSubscribed = ordemasterResult.Where(or => or.SubscribedBy == cu.Email && or.OrderDate != null && or.OrderDate.Value >= from && or.OrderDate.Value <= to).Count().ToString();
                }
                else
                {
                    CandidateAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count().ToString();
                    EmployerAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count().ToString();
                    JobPosted = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count().ToString();
                    PlanSubscribed = ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count().ToString();
                }

            }
            else
            {
                var cu = _channelrepository.GetChannelUsersWithEntries().Where(c => c.Id == ChannelId).FirstOrDefault();
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    CandidateAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    EmployerAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    JobPosted = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job) && ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    PlanSubscribed = ordemasterResult.Where(or => or.SubscribedBy == cu.Email && or.OrderDate != null && or.OrderDate.Value >= from && or.OrderDate.Value <= to).Count().ToString();
                }
                else
                {
                    CandidateAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count().ToString();
                    EmployerAdded = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count().ToString();
                    JobPosted = cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count().ToString();
                    PlanSubscribed = ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count().ToString();
                }
            }

            var result = new
            {
                CandidateAdded = CandidateAdded,
                EmployerAdded = EmployerAdded,
                JobPosted = JobPosted,
                PlanSubscribed = PlanSubscribed
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
