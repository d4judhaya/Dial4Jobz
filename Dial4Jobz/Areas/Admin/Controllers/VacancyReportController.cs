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
    public class VacancyReportController : Controller
    {
        //
        // GET: /Admin/VacancyReport/
        VasRepository _vasRepository = new VasRepository();
        Repository _repository = new Repository();
        UserRepository _userRepository = new UserRepository();
       

        public ActionResult Index()
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


        public JsonResult ListVacancyReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Job> jobResult = _repository.GetJobs();
                                 
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
           
            var result = new
            {
                iTotalRecords = jobResult.Count(),
                iTotalDisplayRecords = jobResult.Count(),
                aaData = jobResult1.Select(o => new object[] { o.Position, (o.CreatedDate != null) ? o.CreatedDate.Value.ToString() : "", (o.Organization != null ? o.Organization.Name : (o.Consultante.Name!=null ? o.Consultante.Name : "Marketing Jobs")), _repository.GetFunctionNameByFunctionId(Convert.ToInt32(o.FunctionId)), (o.JobRoles.Count() > 0 ? _repository.GetRoleNameByJobId(o.Id) : "Any"), (o.MinExperience.HasValue? o.MinExperience.Value / 31104000 : 0) + " to " +(o.MaxExperience.HasValue? o.MaxExperience.Value / 31104000 : 0)+" Years", o.Budget + " to " + o.MaxBudget, (_repository.GetCountryNameByJobId(o.Id) + "," + _repository.GetCityNameByJobId(o.Id) + "," + _repository.GetRegionNameByJobId(o.Id)) })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
