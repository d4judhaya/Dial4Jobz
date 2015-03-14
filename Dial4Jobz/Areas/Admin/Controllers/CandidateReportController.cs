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
using System.Data.Objects;

namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class CandidateReportController : Controller
    {
        //
        // GET: /Admin/CandidateReport/

        Repository _repository = new Repository();
        UserRepository _userRepository = new UserRepository();

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

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
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

             

        [HttpPost]
        public ActionResult Delete(string ids)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if ((user.UserName == "admin123") || (user.UserName=="Mani2014"))
            {

                if (!string.IsNullOrEmpty(ids))
                {
                    string[] candidateIds = ids.Split(',');
                    if (candidateIds != null && candidateIds.Length > 0)
                    {
                        foreach (string id in candidateIds)
                        {
                            Candidate candidate = _repository.GetCandidate(Convert.ToInt32(id));
                            if (candidate != null)
                            {
                                _repository.DeleteCandidateLanguages(Convert.ToInt32(id));
                                _repository.DeleteCandidatePreferredLocation(Convert.ToInt32(id));
                                _repository.DeleteCandidateQualifications(Convert.ToInt32(id));
                                _repository.DeleteCandidateSkills(Convert.ToInt32(id));
                                _repository.DeleteCandidateRoles(Convert.ToInt32(id));
                                _repository.DeleteCandidateLicenseTypes(Convert.ToInt32(id));
                                _repository.DeleteCandidatePreferredFunctions(Convert.ToInt32(id));
                                _repository.DeleteCandidate(Convert.ToInt32(id));
                            }
                        }
                    }

                    else
                    {
                        var result1 = new
                        {
                            error = "1",
                            message = "Unable to delete"

                        };
                        return Json(result1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                var result2 = new
                {
                    error = "",
                    message = "Sorry! You don't have permission to delete.Contact Admin!!"
                };
                return Json(result2, JsonRequestBehavior.AllowGet);
            }

            var result = new
            {
                error = "",
                message = "Candidate Deleted Successfully"
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CandidateFullReports()
        {
            return View();
        }


        public JsonResult ListCandidateFullReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Candidate> candidateResult = _repository.GetCandidates();

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.Position);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.Position);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.Name);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.Function.Name);
            else if (iSortCol_0 == 3 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.TotalExperience);
            else if (iSortCol_0 == 4 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.AnnualSalary);
            else
                candidateResult = candidateResult.OrderByDescending(o => o.Id);


            if (!string.IsNullOrEmpty(sSearch.Trim()))
                candidateResult = candidateResult.Where(o => o.Position.ToLower().Contains(sSearch.ToLower().Trim()) || o.Name != null && o.Function.Name.ToLower().Contains(sSearch.ToLower().Trim()));

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

            var result = new
            {
                iTotalRecords = candidateResult.Count(),
                iTotalDisplayRecords = candidateResult.Count(),
                aaData = candidateResult1.Select(o => new object[] { o.Position, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString("dd-MM-yyyy") : "", (o.Name != null ? o.Name : ""), (o.Function != null ? o.Function.Name : "Any"), (o.CandidatePreferredRoles.Count() > 0 ? _repository.GetRoleByCandiateId(o.Id) : "Any"), (o.TotalExperience.HasValue ? o.TotalExperience / 31104000 + " Years" + ((o.TotalExperience.Value - ((o.TotalExperience / 31104000) * 31536000)) / 2678400) + " Months" : ""), (o.AnnualSalary.HasValue ? o.AnnualSalary.ToString() : ""), (_repository.GetCountryNameByCandidateId(o.Id) + "," + _repository.GetCityNameByCandidateId(o.Id) + "," + _repository.GetRegionNameByCandidateId(o.Id)) })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NonUpdateCandidateReports()
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



        public JsonResult ListNonUpdateCandidateReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Candidate> candidateResult = _repository.GetCandidateWithoutAddress();
            

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.UserName);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.UserName);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.Email);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.Email);
            else if (iSortCol_0 == 3 && "desc" == sSortDir_0)
                candidateResult = candidateResult.OrderByDescending(o => o.ContactNumber);
            else if (iSortCol_0 == 3 && "asc" == sSortDir_0)
                candidateResult = candidateResult.OrderBy(o => o.ContactNumber);
            else
                candidateResult = candidateResult.OrderByDescending(o => o.Id);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                candidateResult = candidateResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.ContactNumber != null && o.ContactNumber.ToLower().Contains(sSearch.ToLower().Trim()) || o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()));

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

            var result = new
            {
                iTotalRecords = candidateResult.Count(),
                iTotalDisplayRecords = candidateResult.Count(),
                aaData = candidateResult1.Select(o => new object[] { o.Id, (o.CreatedDate != null ? o.CreatedDate.Value.ToString("dd-MM-yyyy") : ""), o.UserName, o.ContactNumber, o.Email, (o.IPAddress != null ? o.IPAddress : ""), "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/AdminHome/GetDetail?validateEmail=" + o.Email + "'>Edit</a>" })
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Details(string id)
        {
            if (ModelState.IsValid)
            {
                id = Constants.DecryptString(id.ToString());
                int Id;
                bool ValueIsAnId = int.TryParse(id, out Id);
                Candidate candidate = _repository.GetCandidate(Id);
                return View(candidate);
            }
            return View("MatchCandidates", "Employer");
        }

        public JsonResult ListCandidateReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Candidate> candidateResult = _repository.GetCandidates();

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
                candidateResult = candidateResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.ContactNumber != null && o.ContactNumber.ToLower().Contains(sSearch.ToLower().Trim()) ||  o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()));
           
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


            var result = new
            {
                iTotalRecords = candidateResult.Count(),
                iTotalDisplayRecords = candidateResult.Count(),
                aaData = candidateResult1.Select(o => new object[] { o.Name, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(o.Id, EntryType.Candidate), o.VerifiedByAdmin, o.ContactNumber, o.Email, (o.IPAddress != null ? o.IPAddress : ""), "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/CandidateReport/Details/" + Dial4Jobz.Models.Constants.EncryptString(o.Id.ToString()) + "'>View</a>", "<input type='checkbox' onclick='javascript:Uncheck(this);' name='deletecandidate' value='" + o.Id + "' />" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
