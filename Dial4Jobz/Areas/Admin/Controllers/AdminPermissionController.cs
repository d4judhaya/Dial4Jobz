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
    public class AdminPermissionController : Controller
    {
        //
        // GET: /Admin/AdminPermission/

        Repository _repository = new Repository();
        UserRepository _userRepository=new UserRepository();

        public ActionResult Index()
        {            
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Permission adminPermission = new Permission();
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                string pageAccess = "";
                string[] Page_Code = null;
                foreach (var page in pageaccess)
                {
                    adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = adminPermission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + adminPermission.Name + ",";
                    }
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddAdmin)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "AdminHome");
                }
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }

        public JsonResult getAdminPermissions()
        {
            var result = new
            {
                permissions = _userRepository.GetAdminPermissions()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserReport()
        {
            
            //****To add the user report menu into Edit*****
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Permission adminPermission = new Permission();
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                string pageAccess = "";
                string[] Page_Code = null;
                foreach (var page in pageaccess)
                {
                    adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = adminPermission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + adminPermission.Name + ",";
                    }
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.UserReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "AdminHome");
                }
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
            //**********End****************

        }

        public JsonResult ListAdminUsers(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            IEnumerable<User> userResult = _repository.GetUsers().Where(u => u.IsSuperAdmin == false);
            var users = _userRepository.GetUserTopId();
            string[] userpagecode = new string[users.Id + 2];
            Permission adminPermission = new Permission();
            foreach (var user in userResult)
            {
                IEnumerable<AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);

                string pageAccess = "";
                foreach (var page in pageaccess)
                {
                    adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = adminPermission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + adminPermission.Name + ",";
                    }
                }
                userpagecode[user.Id] = pageAccess;
            }

            Func<User, object> order = rslt =>
            {
                if (iSortCol_0 == 0)
                    return rslt.UserName;
                else
                    return rslt.Email;
            };

            if ("desc" == sSortDir_0)
                userResult = userResult.OrderByDescending(order);
            else
                userResult = userResult.OrderBy(order);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                userResult = userResult.Where(u => u.UserName.ToLower().Contains(sSearch.ToLower().Trim()) || u.Email != null && u.Email.ToLower().Contains(sSearch.ToLower().Trim()));

            var result = new
            {
                iTotalRecords = userResult.Count(),
                iTotalDisplayRecords = userResult.Count(),
                aaData = userResult.Select(u => new object[] { u.UserName, "<a class='ActionPopup' href='/Admin/AdminPermission/ChangePassword/" + u.Id + "'>Change</a>", u.Email, userpagecode[u.Id], "<a class='ActionPopup' href='/Admin/AdminPermission/Edit/" + u.Id + "'><img alt='Edit' src='/Areas/Admin/Content/Images/icn_edit.png' /></a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='DeleteUserId' value='" + u.Id + "' />" }).Skip(iDisplayStart).Take(iDisplayLength)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAdminUserEntries(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IEnumerable<User> userResult = _repository.GetUsers();

            Func<User, object> order = rslt =>
            {
                if (iSortCol_0 == 0)
                    return rslt.UserName;
                else if (iSortCol_0 == 1)
                    return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Candidate)).Count();
                else if (iSortCol_0 == 2)
                    return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Employer)).Count();
                else if (iSortCol_0 == 2)
                    return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Consultant)).Count();
                else if (iSortCol_0 == 3)
                    return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Job)).Count();
                else
                    return rslt.AdminUserEntries.Count();
            };

            if ("desc" == sSortDir_0)
                userResult = userResult.OrderByDescending(order);
            else
                userResult = userResult.OrderBy(order);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                userResult = userResult.Where(u => u.UserName.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                userResult = userResult.Where(u => u.AdminUserEntries.Any(aue => aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date));

                Func<User, object> order1 = rslt =>
                {
                    if (iSortCol_0 == 0)
                        return rslt.UserName;
                    else if (iSortCol_0 == 1)
                        return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Candidate) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else if (iSortCol_0 == 2)
                        return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Employer) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else if (iSortCol_0 == 2)
                        return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Consultant) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else if (iSortCol_0 == 3)
                        return rslt.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Job) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                    else
                        return rslt.AdminUserEntries.Where(aue => aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count();
                };

                if ("desc" == sSortDir_0)
                    userResult = userResult.OrderByDescending(order1);
                else
                    userResult = userResult.OrderBy(order1);

                var result1 = new
                {
                    iTotalRecords = userResult.Count(),
                    iTotalDisplayRecords = userResult.Count(),
                    aaData = userResult.Select(u => new object[] { u.UserName, 
                        u.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Candidate) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        u.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Employer) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        u.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Consultant) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        u.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Job) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count(), 
                        u.AdminUserEntries.Where(aue => aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count() }).Skip(iDisplayStart).Take(iDisplayLength)
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }


            var result = new
            {
                iTotalRecords = userResult.Count(),
                iTotalDisplayRecords = userResult.Count(),
                aaData = userResult.Select(u => new object[] { u.UserName, u.AdminUserEntries.Where(or => or.EntryType == Convert.ToInt32(EntryType.Candidate)).Count(), u.AdminUserEntries.Where(or => or.EntryType == Convert.ToInt32(EntryType.Employer)).Count(),u.AdminUserEntries.Where(or => or.EntryType == Convert.ToInt32(EntryType.Consultant)).Count(), u.AdminUserEntries.Where(or => or.EntryType == Convert.ToInt32(EntryType.Job)).Count(), u.AdminUserEntries.Count() }).Skip(iDisplayStart).Take(iDisplayLength)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdminReport()
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }

        public JsonResult AdminReportbyId(string UserId, string fromDate, string toDate)
        {
            string CandidateAdded = "0";
            string EmployerAdded = "0";
            string JobPosted = "0";
            string ConsultantAdded = "0";
            string TotalCount = "0";
            User userResult = null;
            if (!string.IsNullOrEmpty(UserId))
            {
                userResult = _repository.GetUserbyId(Convert.ToInt32(UserId));

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    CandidateAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Candidate) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    EmployerAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Employer) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    ConsultantAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Consultant) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    JobPosted = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Job) && aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                    TotalCount = userResult.AdminUserEntries.Where(aue => aue.CreatedOn.Date >= Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date && aue.CreatedOn.Date <= Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date).Count().ToString();
                }
                else
                {
                    CandidateAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Candidate)).Count().ToString();
                    EmployerAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Employer)).Count().ToString();
                    ConsultantAdded = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Consultant)).Count().ToString();
                    JobPosted = userResult.AdminUserEntries.Where(aue => aue.EntryType == Convert.ToInt32(EntryType.Job)).Count().ToString();
                    TotalCount = userResult.AdminUserEntries.Count().ToString();
                }

            }

            var result = new
            {
                CandidateAdded = CandidateAdded,
                EmployerAdded = EmployerAdded,
                ConsultantAdded = ConsultantAdded,
                JobPosted = JobPosted,
                TotalCount = TotalCount
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListEmployerReportbyAdminId(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string UserId, string fromDate, string toDate)
        {
            int AdminId = Convert.ToInt32(UserId);
            int[] orgIds = _repository.GetEntryIdsbyAdminIdAndEntryType(AdminId, EntryType.Employer);
            IQueryable<Organization> organizationResult = _repository.GetOrganizations().Where(o => orgIds.Contains((int)o.Id));

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                organizationResult = organizationResult.OrderByDescending(o => o.Name);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                organizationResult = organizationResult.OrderBy(o => o.Name);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                organizationResult = organizationResult.OrderByDescending(o => o.CreateDate.Value);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                organizationResult = organizationResult.OrderBy(o => o.CreateDate.Value);
            else
                organizationResult = organizationResult.OrderByDescending(o => o.Id);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                organizationResult = organizationResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                organizationResult = organizationResult.Where(o => o.CreateDate != null && o.CreateDate.Value >= from && o.CreateDate.Value <= to);

            }

            IEnumerable<Organization> organizationResult1 = organizationResult.Skip(iDisplayStart).Take(iDisplayLength);

            var result = new
            {
                iTotalRecords = organizationResult.Count(),
                iTotalDisplayRecords = organizationResult.Count(),
                aaData = organizationResult1.Select(o => new object[] { o.Name, (o.CreateDate != null) ? o.CreateDate.Value.Date.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(o.Id, EntryType.Employer), "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/EmployerReport/PostedJobs/?id=" + o.Id + "&name=" + o.Name + "'>View</a>", o.ContactPerson, o.MobileNumber, o.Email })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(User user, string Pwd, string[] PageAccess)
        {
            string message = "";
            string error = "";
            Array pageaccess = PageAccess;
            var users = _userRepository.GetUsersbyUserName(user.UserName).ToList();
            if (users.Count() != 0)
            {
                var result1 = new
                {
                    error = "1",
                    message = "User Name Already Exists"
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            user.Password = SecurityHelper.GetMD5Bytes(Pwd);

            //user.PageCode = String.Join(", ", PageAccess);

            _userRepository.DeleteUserPermissions(user.Id);

            foreach (string page in pageaccess)
            {
                if (!string.IsNullOrEmpty(page))
                {
                    AdminPermission userPermission = new AdminPermission();
                    userPermission.UserId = user.Id;
                    userPermission.PermissionId = Convert.ToInt32(page);

                    _userRepository.AddUserPermissions(userPermission);
                }
            }
            user.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            user.IsSuperAdmin = false;

            _repository.AddAdminUser(user);
            _repository.Save();
            message = "Admin User Added successfully";

            var result = new
            {
                error = error,
                message = message
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int id)
        {
            User user = _repository.GetUserbyId(id);

            if (user == null)
                return new FileNotFoundResult();
            IEnumerable<AdminPermission> userPermissions = _userRepository.GetPermissionsbyUserId(id);
            if (userPermissions.Count() > 0)
            {
                ViewData["PermissionIds"] = String.Join(",", userPermissions.Select(pf => pf.PermissionId));
            }
            return View(user);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(User user, string Pwd, string[] PageAccess)
        {
            string message = "";
            string error = "";
            Array pageaccess = PageAccess;
            var Users = _userRepository.GetUsersbyUserName(user.UserName).ToList();
            var users = Users.Where(u => u.UserName == user.UserName && u.Id != user.Id).ToList();
            if (users.Count() != 0)
            {
                var result1 = new
                {
                    error = "1",
                    message = "User Name Already Exists"
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            User adminuser = _repository.GetUserbyId(user.Id);
            adminuser.UserName = user.UserName;
            adminuser.Email = user.Email;
            adminuser.Mobilenumber = user.Mobilenumber;

            // adminuser.PageCode = String.Join(", ", PageAccess.Where(pa => pa != "false" && !string.IsNullOrEmpty(pa)));

            _userRepository.DeleteUserPermissions(user.Id);

            foreach (string page in pageaccess)
            {
                if (page != "false" && !string.IsNullOrEmpty(page))
                {
                    AdminPermission userPermission = new AdminPermission();
                    userPermission.UserId = user.Id;
                    userPermission.PermissionId = Convert.ToInt32(page);

                    _userRepository.AddUserPermissions(userPermission);
                }
            }
            _repository.Save();
            message = "Admin User Updated Successfully";

            var result = new
            {
                error = error,
                message = message
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[ValidateInput(false)]
        //[HttpPost]
        //public ActionResult Create(User user, string Pwd, string[] PageAccess)
        //{
        //    string message = "";
        //    string error = "";
        //    var users = _userrepository.GetUsersbyUserName(user.UserName).ToList();
        //        if (users.Count() != 0)
        //        {
        //            var result1 = new
        //            {
        //                error = "1",
        //                message = "User Name Already Exists"
        //            };
        //            return Json(result1, JsonRequestBehavior.AllowGet);
        //        }

        //        user.Password = SecurityHelper.GetMD5Bytes(Pwd);
        //        user.PageCode = String.Join(", ", PageAccess);
        //        user.CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
        //        user.IsSuperAdmin = false;

        //        _repository.AddAdminUser(user);
        //        _repository.Save();
        //        message = "Admin User Added successfully";
            
        //    var result = new
        //    {
        //        error = error,
        //        message = message
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        //public ActionResult Edit(int id = 0)
        //{
        //    User user = _repository.GetUserbyId(id);
            
        //    if (user == null)
        //        return new FileNotFoundResult();
            
        //    return View(user);
        //}

        //[ValidateInput(false)]
        //[HttpPost]
        //public ActionResult Edit(User user, string Pwd, string[] PageAccess)
        //{
        //    string message = "";
        //    string error = "";
        //    var Users = _userrepository.GetUsersbyUserName(user.UserName).ToList();
        //    var users = Users.Where(u => u.UserName == user.UserName && u.Id != user.Id).ToList();
        //    if (users.Count() != 0)
        //    {
        //        var result1 = new
        //        {
        //            error = "1",
        //            message = "User Name Already Exists"
        //        };
        //        return Json(result1, JsonRequestBehavior.AllowGet);
        //    }

        //    User adminuser = _repository.GetUserbyId(user.Id);
        //    adminuser.UserName = user.UserName;
        //    adminuser.Email = user.Email;
        //    adminuser.Mobilenumber = user.Mobilenumber;
        //    adminuser.PageCode = String.Join(", ", PageAccess.Where(pa => pa != "false" && !string.IsNullOrEmpty(pa)));
        //    _repository.Save();
        //    message = "Admin User Updated successfully";

        //    var result = new
        //    {
        //        error = error,
        //        message = message
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult ChangePassword(int id = 0)
        {
            User user = _repository.GetUserbyId(id);

            if (user == null)
                return new FileNotFoundResult();

            return View(user);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChangePassword(User user, string Pwd)
        {
            string message = "";
            string error = "";
            
            User adminuser = _repository.GetUserbyId(user.Id);
            adminuser.Password = SecurityHelper.GetMD5Bytes(Pwd);
            _repository.Save();
            message = "Admin User Password Changed successfully";

            var result = new
            {
                error = error,
                message = message
            };
            return Json(result, JsonRequestBehavior.AllowGet);

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
                        _repository.DeleteAdminUserEntry(Convert.ToInt32(id));
                        _repository.DeleteAdminUser(Convert.ToInt32(id));         
                    }
                    _repository.Save();
                }
                else
                {
                    var result1 = new
                    {
                        error = "1",
                        message = "Unable to Delete Admin User"
                    };
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result1 = new
                {
                    error = "1",
                    message = "Unable to Delete Admin User"
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }


            var result = new
            {
                error = "",
                message = "Admin User Deleted Successfully"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        } 

    }
}
