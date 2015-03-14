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

namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class ChannelController : Controller
    {
        static readonly IChannelRepository _channelrepository = new ChannelRepository();
        VasRepository _vasRepository = new VasRepository();

        #region Channel Partner CRUD
        public ActionResult ManagePartners()
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                string[] Page_Code = null;
                if (!string.IsNullOrEmpty(user.PageCode))
                {
                    Page_Code = user.PageCode.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddChannelPartner)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return Redirect("/admin");
                }
            }
            else
            {
                return Redirect("/admin");
            }
        }

        public JsonResult ListChannelPartners(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<ChannelPartner> channelResult = _channelrepository.GetChannelPartners();

            Func<IQueryable<ChannelPartner>, IOrderedQueryable<ChannelPartner>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.UserName);
                    else if (iSortCol_0 == 3)
                        return query.OrderByDescending(rslt => rslt.Email);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.ContactNo);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.Address);
                    else if (iSortCol_0 == 6)
                        return query.OrderByDescending(rslt => rslt.Pincode);
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
                    else if (iSortCol_0 == 5)
                        return query.OrderBy(rslt => rslt.Address);
                    else if (iSortCol_0 == 6)
                        return query.OrderBy(rslt => rslt.Pincode);
                    else
                        return query.OrderBy(rslt => rslt.Id);

                }

            };

            channelResult = orderingFunc(channelResult);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                channelResult = channelResult.Where(ch => ch.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || ch.Email.ToLower().Contains(sSearch.ToLower().Trim()) || ch.ContactNo.ToLower().Contains(sSearch.ToLower().Trim()) || ch.Address.ToLower().Contains(sSearch.ToLower().Trim()) || ch.Pincode.ToLower().Contains(sSearch.ToLower().Trim()));

            IEnumerable<ChannelPartner> channelResult1 = channelResult.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = channelResult.Count(),
                iTotalDisplayRecords = channelResult.Count(),
                aaData = channelResult1.Select(u => new object[] { "", 
                                                                   u.UserName, 
                                                                   "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString().Replace("www.","") + "/Admin/Channel/ChangePartnerPassword/" + u.Id + "'>Change</a>",
                                                                   u.Email, 
                                                                   u.ContactNo, 
                                                                   u.Address, 
                                                                   u.Pincode, 
                                                                   "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString().Replace("www.","") + "/Admin/Channel/EditPartner/" + u.Id + "'><img alt='Edit' src='/Areas/Admin/Content/Images/icn_edit.png' /></a>", 
                                                                   "<input type='checkbox' onclick='javascript:Uncheck(this);' name='DeletePartnerId' value='" + u.Id + "' />" }).Skip(iDisplayStart).Take(iDisplayLength)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartner()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreatePartner(ChannelPartner channelpartner, string Pwd)
        {
            var channelpartners = _channelrepository.GetChannelPartnersbyUserName(channelpartner.UserName).ToList();
            var channelpartnersbyemail = _channelrepository.GetChannelPartnersbyEmail(channelpartner.Email).ToList();
            var channelusersbyemail = _channelrepository.GetChannelUsersbyEmail(channelpartner.Email).ToList();

            if (channelpartners.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "User Name Already Exists" });
            }
            else if (channelpartnersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }
            else if (channelusersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists as a Channel User" });
            }

            channelpartner.Password = SecurityHelper.GetMD5Bytes(Pwd);
            channelpartner.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            _channelrepository.AddChannelPartner(channelpartner);
            _channelrepository.Save();

            SendAccountMail(channelpartner, Pwd);

            return Json(new JsonActionResult { Success = true, Message = "Channel Partner Added successfully" });

        }

        private void SendAccountMail(ChannelPartner channelpartner, string Pwd)
        {
            if (channelpartner.Email != "")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Areas/Channel/Views/MailTemplate/ChannelPartnerRegistration.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[CHANNELPARTNERNAME]", channelpartner.UserName);
                table = table.Replace("[MOBILE]", channelpartner.ContactNo);
                table = table.Replace("[EMAILID]", channelpartner.Email);
                table = table.Replace("[PASSWORD]", Pwd);
                table = table.Replace("[ADDRESS]", channelpartner.Address);

                EmailHelper.SendEmailSBCC(
                      Constants.EmailSender.EmployerSupport,
                      channelpartner.Email,
                      "smc@dial4jobz.com",
                      "ganesan@dial4jobz.com",
                      "",
                      Constants.EmailSubject.ChannelPartnerRegistration,
                      table);
            }

            if (channelpartner.ContactNo != "")
            {
                SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.SMSCandidateRegister
                                        .Replace("[USER_NAME]", channelpartner.Email)
                                        .Replace("[PASSWORD]", Pwd)
                                        .Replace("[CODE]", ""),

                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            channelpartner.ContactNo
                            );

            }
        }

        public ActionResult EditPartner(int id = 0)
        {
            ChannelPartner channelpartner = _channelrepository.GetChannelPartner(id);

            if (channelpartner == null)
                return new FileNotFoundResult();

            return View(channelpartner);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditPartner(ChannelPartner partner)
        {
            var channelpartners = _channelrepository.GetChannelPartnersbyUserName(partner.UserName).Where(u => u.UserName.ToLower() == partner.UserName.ToLower() && u.Id != partner.Id).ToList();
            var channelpartnersbyemail = _channelrepository.GetChannelPartnersbyEmail(partner.Email).Where(u => u.Email.ToLower() == partner.Email.ToLower() && u.Id != partner.Id).ToList();
            var channelusersbyemail = _channelrepository.GetChannelUsersbyEmail(partner.Email).ToList();

            if (channelpartners.Count() != 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "User Name Already Exists" });
            }
            else if (channelpartnersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists" });
            }
            else if (channelusersbyemail.Count() > 0)
            {
                return Json(new JsonActionResult { Success = false, Message = "Email Id Already Exists as a Channel User" });
            }

            ChannelPartner channelpartner = _channelrepository.GetChannelPartner(partner.Id);
            channelpartner.UserName = partner.UserName;
            channelpartner.Email = partner.Email;
            channelpartner.ContactNo = partner.ContactNo;
            channelpartner.Address = partner.Address;
            channelpartner.Pincode = partner.Pincode;
            _channelrepository.Save();

            return Json(new JsonActionResult { Success = true, Message = "Channel Partner Updated successfully" });
        }

        public ActionResult ChangePartnerPassword(int id = 0)
        {
            ChannelPartner channelpartner = _channelrepository.GetChannelPartner(id);

            if (channelpartner == null)
                return new FileNotFoundResult();

            return View(channelpartner);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChangePartnerPassword(ChannelPartner partner, string Pwd)
        {
            ChannelPartner channelpartner = _channelrepository.GetChannelPartner(partner.Id);
            channelpartner.Password = SecurityHelper.GetMD5Bytes(Pwd);
            _channelrepository.Save();

            return Json(new JsonActionResult { Success = true, Message = "Channel Partner Password Changed successfully" });

        }

        [HttpPost]
        public JsonResult Delete(string partnerIds)
        {
            if (!string.IsNullOrEmpty(partnerIds))
            {
                string[] ids = partnerIds.Split(',');
                if (ids != null && ids.Length > 0)
                {
                    foreach (string id in ids)
                    {
                        _channelrepository.DeleteChannelPartner(Convert.ToInt32(id));
                    }
                    _channelrepository.Save();
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "Unable to Delete Channel Partner" });
                }
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = "Unable to Delete Channel Partner" });
            }

            return Json(new JsonActionResult { Success = true, Message = "Channel Partner Deleted Successfully" });
        }

        #endregion

        #region Channel Partner Report
        public ActionResult ChannelPartnerReport()
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                string[] Page_Code = null;
                if (!string.IsNullOrEmpty(user.PageCode))
                {
                    Page_Code = user.PageCode.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return Redirect("/admin");
                }
            }
            else
            {
                return Redirect("/admin");
            }
        }

        public JsonResult ListChannelPartnerEntries(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IEnumerable<ChannelPartner> channelResult = _channelrepository.GetChannelPartnersWithEntries();

            IQueryable<OrderMaster> ordemasterResult = _vasRepository.GetOrderMasters();

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                channelResult = channelResult.Where(cu => cu.UserName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                channelResult = channelResult.Where(c => c.ChannelEntries.Any(ce => ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date));

                Func<ChannelPartner, object> order1 = rslt =>
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
                    channelResult = channelResult.OrderByDescending(order1);
                else
                    channelResult = channelResult.OrderBy(order1);

                string fromDate1 = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                string toDate1 = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate1).Date;
                var to = DateTime.Parse(toDate1).Date;

                to = to.AddHours(23.99);

                var result1 = new
                {
                    iTotalRecords = channelResult.Count(),
                    iTotalDisplayRecords = channelResult.Count(),
                    aaData = channelResult.Select(cu => new object[] { "", cu.UserName, 
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
                Func<ChannelPartner, object> order = rslt =>
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
                    channelResult = channelResult.OrderByDescending(order);
                else
                    channelResult = channelResult.OrderBy(order);
            }

            var result = new
            {
                iTotalRecords = channelResult.Count(),
                iTotalDisplayRecords = channelResult.Count(),
                aaData = channelResult.Select(cu => new object[] { "", cu.UserName, cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count(), ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count() }).Skip(iDisplayStart).Take(iDisplayLength).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Channel partner Accounts

        public ActionResult ChannelPartnersAccounts()
        {
            return View();
        }


        public JsonResult ListChannelAccounts(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IEnumerable<ChannelPartner> channelResult = _channelrepository.GetChannelPartnersWithEntries();

            IQueryable<OrderMaster> ordemasterResult = _vasRepository.GetOrderMasters();

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                channelResult = channelResult.Where(cu => cu.UserName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                channelResult = channelResult.Where(c => c.ChannelEntries.Any(ce => ce.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && ce.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date));

                Func<ChannelPartner, object> order1 = rslt =>
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
                    channelResult = channelResult.OrderByDescending(order1);
                else
                    channelResult = channelResult.OrderBy(order1);

                string fromDate1 = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                string toDate1 = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate1).Date;
                var to = DateTime.Parse(toDate1).Date;

                to = to.AddHours(23.99);

                var result1 = new
                {
                    iTotalRecords = channelResult.Count(),
                    iTotalDisplayRecords = channelResult.Count(),
                    aaData = channelResult.Select(cu => new object[] { "", cu.UserName, 
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
                Func<ChannelPartner, object> order = rslt =>
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
                    channelResult = channelResult.OrderByDescending(order);
                else
                    channelResult = channelResult.OrderBy(order);
            }

            var result = new
            {
                iTotalRecords = channelResult.Count(),
                iTotalDisplayRecords = channelResult.Count(),
                aaData = channelResult.Select(cu => new object[] { "", cu.UserName, cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Candidate)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Employer)).Count(), cu.ChannelEntries.Where(ce => ce.EntryType == Convert.ToInt32(EntryType.Job)).Count(), ordemasterResult.Where(or => or.SubscribedBy == cu.Email).Count() }).Skip(iDisplayStart).Take(iDisplayLength).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //end channel partner accounts
        #endregion
    }
}
