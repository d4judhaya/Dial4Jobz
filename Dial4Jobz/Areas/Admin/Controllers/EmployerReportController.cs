using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Enums;
using Dial4Jobz.Models.Results;

namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class EmployerReportController : Controller
    {   

        Repository _repository = new Repository();
        UserRepository _userRepository = new UserRepository();
        VasRepository _vasRepository = new VasRepository();

        public ActionResult Index()
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Permission permission = new Permission();
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                string pageAccess = "";
                string[] Page_Code = null;
                foreach (var page in pageaccess)
                {
                    permission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
                    if (string.IsNullOrEmpty(pageAccess))
                    {
                        pageAccess = permission.Name + ",";
                    }
                    else
                    {
                        pageAccess = pageAccess + permission.Name + ",";
                    }
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
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

        public ActionResult PostedJobs()
        {
            return View();
        }

               
        public ActionResult Delete(int id)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            Organization organization = _repository.GetOrganization(id);

            IEnumerable<OrderMaster> orderMaster = _vasRepository.GetOrderMasterListByOrganizationId(id);
            IEnumerable<Job> jobs = _repository.GetJobs(id);

            if (user.UserName == "admin123" || user.UserName == "Mani2014")
            {
                if (jobs != null)
                {
                    return Json(new JsonActionResult
                    {
                        Success = true,
                        Message = "The Employer Posted a job, Can't delete this employer"
                    });
                }

                if (orderMaster != null)
                {
                    foreach (var order in orderMaster)
                    {
                        if (order.PaymentStatus == true)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "The Employer have Paid order's, Can't delete this employer"
                            });
                        }
                    }
                    foreach (var order in orderMaster)
                    {
                        _vasRepository.DeleteOrderDetails(order.OrderId);
                        _vasRepository.DeleteOrderPaymentDetails(order.OrderId);
                        _vasRepository.DeleteAlertsLogs(order.OrderId);
                    }
                    _repository.DeleteOrganizationOrder(id);
                }
                _repository.DeleteOrganization(id);

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Employer has been deleted Successfully"
                });
            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "You don't have permission to delete"
            });
        }

        [HttpPost]
        public ActionResult DeleteOrganization(string ids)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (user.UserName == "admin123" || user.UserName == "Mani2014")
            {

                if (!string.IsNullOrEmpty(ids))
                {
                    string[] employerIds = ids.Split(',');
                    if (employerIds != null && employerIds.Length > 0)
                    {
                        foreach (string id in employerIds)
                        {
                            Organization organization = _repository.GetOrganization(Convert.ToInt32(id));
                            
                            if (organization != null)
                            {
                                _repository.DeleteJobs(Convert.ToInt32(id));
                                _repository.DeleteOrganizationOrder(Convert.ToInt32(id));
                                _repository.DeleteOrganization(Convert.ToInt32(id));
                               
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
                message = "Organization Detleted Successfully"
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListEmployerReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Organization> organizationResult = _repository.GetOrganizations();

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

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
                organizationResult = organizationResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()) || o.MobileNumber != null && o.MobileNumber.ToLower().Contains(sSearch.ToLower().Trim()));

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
                aaData = organizationResult1.Select(o => new object[] { o.Name, (o.CreateDate != null) ? o.CreateDate.Value.Date.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(o.Id, EntryType.Employer), o.VerifiedByAdmin, "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Admin/EmployerReport/PostedJobs/?id=" + o.Id + "&name=" + o.Name + "'>View</a>", o.ContactPerson, o.MobileNumber, o.Email, (o.IPAddress != null ? o.IPAddress : ""), "<a href='JavaScript:void(0)' onclick='Delete(" + o.Id + ")'>Delete</a>" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ListPostedJobs(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string empId, string fromDate, string toDate)
        {
            IQueryable<Job> jobpostedResult = null;

            if(!string.IsNullOrEmpty(empId))
                jobpostedResult = _repository.GetJobsByOrganizationId(Convert.ToInt32(empId));
            else
                jobpostedResult = _repository.GetJobsByOrganizationId(-0);

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                jobpostedResult = jobpostedResult.OrderByDescending(j => j.Position);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                jobpostedResult = jobpostedResult.OrderBy(j => j.Position);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                jobpostedResult = jobpostedResult.OrderByDescending(j => j.CreatedDate.Value);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                jobpostedResult = jobpostedResult.OrderBy(j => j.CreatedDate.Value);
            else
                jobpostedResult = jobpostedResult.OrderByDescending(j => j.Id);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                jobpostedResult = jobpostedResult.Where(j => j.Position.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                var from = Convert.ToDateTime(DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date;
                var to = Convert.ToDateTime(DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy")).Date;

                jobpostedResult = jobpostedResult.Where(j => (j.CreatedDate != null) && (j.CreatedDate.Value >= from && j.CreatedDate.Value <= to));

            }

            IEnumerable<Job> jobpostedResult1 = jobpostedResult.Skip(iDisplayStart).Take(iDisplayLength);

            var result = new
            {
                iTotalRecords = jobpostedResult.Count(),
                iTotalDisplayRecords = jobpostedResult.Count(),
                aaData = jobpostedResult1.Select(j => new object[] { "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Jobs/Details/" + Constants.EncryptString(j.Id.ToString()) + "'>"+ j.Position +"</a>", (j.CreatedDate != null) ? j.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(j.Id, EntryType.Job) != "" ? _repository.GetAdminUserNamebyEntryIdAndEntryType(j.Id, EntryType.Job) : j.Organization.Name }) 
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
