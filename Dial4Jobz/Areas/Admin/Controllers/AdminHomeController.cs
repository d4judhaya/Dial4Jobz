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
using System.Text;
using System.IO.Packaging;
using System.Xml;


namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseController
    {
        Repository _repository = new Repository();
        VasRepository _vasRepository = new VasRepository();
        ChannelRepository _channelrepository = new ChannelRepository();
        DateTime currentdate = Constants.CurrentTime().Date;
        
        const int MAX_ADD_NEW_INPUT = 25;
        const int PAGE_SIZE = 15;
        public int maxLength = int.MaxValue;
        public string[] AllowedFileExtensions;
        public string[] AllowedContentTypes;
        List<string> _filters = new List<string>();

        #region Home
        // GET: /AdminHome/Home/Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region Candidate Summary

        public ActionResult CandidateSummary(string reportType)
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

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction)) || Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {

                    using (var _db = new Dial4JobzEntities())
                    {
                        Session["reportType"] = reportType;

                        if (reportType == "Function")
                        {
                            var totalCount = (from j in _db.Candidates
                                              join i in _db.Functions on j.FunctionId equals i.Id
                                              select i).Count();
                            ViewData["TotalCandidates"] = totalCount;

                            var res = from j in _db.Candidates
                                      join i in _db.Functions on j.FunctionId equals i.Id
                                      group new { i, j } by new
                                      {
                                          i.Name,
                                          i.Id
                                      } into g
                                      orderby g.Key.Name ascending
                                      select new ReportSummary
                                      {
                                          Id = g.Key.Id,
                                          Name = g.Key.Name,
                                          TotalCount = g.Count()
                                      };

                            return View(res.ToList());
                        }
                        else
                        {
                            var totalCount = (from j in _db.Candidates
                                              join i in _db.Industries on j.IndustryId equals i.Id
                                              select i).Count();
                            ViewData["TotalCandidates"] = totalCount;

                            var res = from j in _db.Candidates
                                      join i in _db.Industries on j.IndustryId equals i.Id
                                      group new { i, j } by new
                                      {
                                          i.Name,
                                          i.Id
                                      } into g
                                      orderby g.Key.Name ascending
                                      select new ReportSummary
                                      {
                                          Id = g.Key.Id,
                                          Name = g.Key.Name,
                                          TotalCount = g.Count()
                                      };

                            return View(res.ToList());
                        }


                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        #region CandidateFreshness

        public ActionResult CandidateFreshness(int functionId)
        {
            ViewData["functionId"] = functionId;

            return View();
        }

        [HttpPost]
        public ActionResult CandidateFreshness(FormCollection collection)
        {
            int functionId = 0;

            if (collection["SelectedId"] != null || collection["SelectedId"] != "")
            {
                functionId = Convert.ToInt16(collection["SelectedId"]);
                ViewData["functionId"] = functionId;
            }

            using (var _db = new Dial4JobzEntities())
            {
                string where = collection["where"].ToString();
                string reportDate = collection["Day"].ToString();

                TimeSpan ts;
                DateTime dtStart = Convert.ToDateTime("1900-01-01");

                if (where == "0")
                {
                    dtStart = Convert.ToDateTime("1900-01-01");
                }
                if (where == "30")
                {
                    ts = new TimeSpan(30, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "7")
                {
                    ts = new TimeSpan(7, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "1")
                {
                    ts = new TimeSpan(1, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }

                DateTime dtEnd = DateTime.Now;

                if (reportDate != "" && reportDate.Length == 10)
                {
                    dtStart = Convert.ToDateTime(reportDate).AddHours(00).AddMinutes(00).AddSeconds(00).AddMilliseconds(000);
                    dtEnd = Convert.ToDateTime(reportDate).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                }

                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Functions on j.FunctionId equals i.Id
                                  where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                  group new { i } by new
                                  {
                                      i.Id,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Id = g.Key.Id,
                                      Name = g.Key.Name,
                                      TotalCount = g.Count()
                                  };

                        return View(res.ToList());
                    }
                    else
                    {

                        var res = from j in _db.Candidates
                                  join i in _db.Functions on j.FunctionId equals i.Id
                                  where j.FunctionId == functionId && j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                  group new { i } by new
                                  {
                                      i.Id,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                       {
                                           Id = g.Key.Id,
                                           Name = g.Key.Name,
                                           TotalCount = g.Count()
                                       };

                        return View(res.ToList());
                    }
                }
                else
                {

                    if (functionId == 0)
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Industries on j.IndustryId equals i.Id
                                  where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                  group new { i } by new
                                  {
                                      i.Id,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Id = g.Key.Id,
                                      Name = g.Key.Name,
                                      TotalCount = g.Count()
                                  };

                        return View(res.ToList());
                    }
                    else
                    {

                        var res = from j in _db.Candidates
                                  join i in _db.Industries on j.IndustryId equals i.Id
                                  where j.IndustryId == functionId && j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                  group new { i } by new
                                  {
                                      i.Id,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Id = g.Key.Id,
                                      Name = g.Key.Name,
                                      TotalCount = g.Count()
                                  };

                        return View(res.ToList());
                    }
                }
            }

        }

        #endregion

        #region CandidateGender

        public ActionResult CandidateGender(int functionId)
        {
            ViewData["functionId"] = functionId;

            using (var _db = new Dial4JobzEntities())
            {
                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Functions on j.FunctionId equals i.Id
                                  group new { i } by new
                                  {
                                      j.Gender,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Name = g.Key.Name,
                                      Id = g.Key.Gender,
                                      TotalCount = g.Count()

                                  };

                        return View(res.ToList());
                    }
                    else
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Functions on j.FunctionId equals i.Id
                                  where j.FunctionId == functionId
                                  group new { i } by new
                                  {
                                      j.Gender,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Name = g.Key.Name,
                                      Id = g.Key.Gender,
                                      TotalCount = g.Count()

                                  };

                        return View(res.ToList());
                    }
                }
                else
                {
                    if (functionId == 0)
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Industries on j.IndustryId equals i.Id
                                  group new { i } by new
                                  {
                                      j.Gender,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Name = g.Key.Name,
                                      Id = g.Key.Gender,
                                      TotalCount = g.Count()

                                  };

                        return View(res.ToList());
                    }
                    else
                    {
                        var res = from j in _db.Candidates
                                  join i in _db.Industries on j.IndustryId equals i.Id
                                  where j.IndustryId == functionId
                                  group new { i } by new
                                  {
                                      j.Gender,
                                      i.Name
                                  } into g
                                  orderby g.Key.Name ascending
                                  select new ReportSummary
                                  {
                                      Name = g.Key.Name,
                                      Id = g.Key.Gender,
                                      TotalCount = g.Count()

                                  };

                        return View(res.ToList());
                    }
                }
            }
        }

        #endregion

        #region CandidateLocation

        public ActionResult CandidateCountry(int functionId)
        {
            ViewData["functionId"] = functionId;

            using (var _db = new Dial4JobzEntities())
            {
                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Functions.Id,dbo.Functions.Name,dbo.Countries.Id AS ID1, dbo.Countries.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.Countries ON dbo.Locations.CountryId = dbo.Countries.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Functions.Id, dbo.Functions.Name, dbo.Countries.Name,dbo.Countries.Id", null).ToList();
                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Functions.Id,dbo.Functions.Name, dbo.Countries.Id AS ID1, dbo.Countries.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.Countries ON dbo.Locations.CountryId = dbo.Countries.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Functions.Id, dbo.Functions.Name, dbo.Countries.Name, dbo.Countries.Id  HAVING (dbo.Functions.Id =" + functionId + ")", null).ToList();
                        return View(reports);
                    }
                }
                else
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Industries.Id,dbo.Industries.Name,dbo.Countries.Id AS ID1, dbo.Countries.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.Countries ON dbo.Locations.CountryId = dbo.Countries.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Industries.Id, dbo.Industries.Name, dbo.Countries.Name,dbo.Countries.Id", null).ToList();
                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Industries.Id,dbo.Industries.Name, dbo.Countries.Id AS ID1, dbo.Countries.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.Countries ON dbo.Locations.CountryId = dbo.Countries.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Industries.Id, dbo.Industries.Name, dbo.Countries.Name, dbo.Countries.Id  HAVING (dbo.Industries.Id =" + functionId + ")", null).ToList();
                        return View(reports);
                    }
                }

            }
        }


        public ActionResult CandidateState(int functionId, int countryId)
        {
            ViewData["functionId"] = functionId;
            TempData["countryId"] = countryId;
            TempData.Keep();

            using (var _db = new Dial4JobzEntities())
            {
                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        //Fetching tables data using ExecuteStoreQuery
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Functions.Id,dbo.Functions.Name,dbo.States.Id AS ID1, dbo.States.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.States ON dbo.Locations.StateId = dbo.States.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Functions.Id, dbo.Functions.Name, dbo.States.Name,dbo.States.Id", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Functions.Id,dbo.Functions.Name,dbo.States.Id AS ID1, dbo.States.Name AS Name1, dbo.States.CountryId as ID2, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.States ON dbo.Locations.StateId = dbo.States.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Functions.Id, dbo.Functions.Name, dbo.States.Name,dbo.States.Id,dbo.States.CountryId  HAVING (dbo.Functions.Id =" + functionId + " and  dbo.States.CountryId = " + countryId + ")", null).ToList();

                        return View(reports);
                    }
                }
                else
                {
                    if (functionId == 0)
                    {
                        //Fetching tables data using ExecuteStoreQuery
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Industries.Id,dbo.Industries.Name,dbo.States.Id AS ID1, dbo.States.Name AS Name1, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.States ON dbo.Locations.StateId = dbo.States.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Industries.Id, dbo.Industries.Name, dbo.States.Name,dbo.States.Id", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT  dbo.Industries.Id,dbo.Industries.Name,dbo.States.Id AS ID1, dbo.States.Name AS Name1, dbo.States.CountryId as ID2, COUNT(*) AS TotalCount FROM dbo.Locations INNER JOIN dbo.States ON dbo.Locations.StateId = dbo.States.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY  dbo.Industries.Id, dbo.Industries.Name, dbo.States.Name,dbo.States.Id,dbo.States.CountryId  HAVING (dbo.Industries.Id =" + functionId + " and  dbo.States.CountryId = " + countryId + ")", null).ToList();

                        return View(reports);
                    }
                }

            }
        }

        public ActionResult CandidateCity(int functionId, int stateId)
        {
            ViewData["functionId"] = functionId;
            TempData["stateId"] = stateId;
            TempData.Keep();

            if (stateId > 0)
            {
                TempData["stateId"] = stateId;
            }

            using (var _db = new Dial4JobzEntities())
            {
                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Functions.Id, dbo.Functions.Name, dbo.Cities.Id AS ID1, dbo.Cities.Name AS Name1, COUNT(*) AS TotalCount, dbo.Cities.StateId AS ID2 FROM dbo.Locations INNER JOIN dbo.Cities ON dbo.Locations.CityId = dbo.Cities.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Functions.Id, dbo.Functions.Name, dbo.Cities.Name, dbo.Cities.Id, dbo.Cities.StateId", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Functions.Id, dbo.Functions.Name, dbo.Cities.Id AS ID1, dbo.Cities.Name AS Name1, COUNT(*) AS TotalCount, dbo.Cities.StateId AS ID2 FROM dbo.Locations INNER JOIN dbo.Cities ON dbo.Locations.CityId = dbo.Cities.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Functions.Id, dbo.Functions.Name, dbo.Cities.Name, dbo.Cities.Id, dbo.Cities.StateId  HAVING (dbo.Functions.Id =" + functionId + " and  dbo.Cities.StateId = " + stateId + ")", null).ToList();

                        return View(reports);
                    }
                }
                else
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Industries.Id, dbo.Industries.Name, dbo.Cities.Id AS ID1, dbo.Cities.Name AS Name1, COUNT(*) AS TotalCount, dbo.Cities.StateId AS ID2 FROM dbo.Locations INNER JOIN dbo.Cities ON dbo.Locations.CityId = dbo.Cities.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Industries.Id, dbo.Industries.Name, dbo.Cities.Name, dbo.Cities.Id, dbo.Cities.StateId", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Industries.Id, dbo.Industries.Name, dbo.Cities.Id AS ID1, dbo.Cities.Name AS Name1, COUNT(*) AS TotalCount, dbo.Cities.StateId AS ID2 FROM dbo.Locations INNER JOIN dbo.Cities ON dbo.Locations.CityId = dbo.Cities.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Industries.Id, dbo.Industries.Name, dbo.Cities.Name, dbo.Cities.Id, dbo.Cities.StateId  HAVING (dbo.Industries.Id =" + functionId + " and  dbo.Cities.StateId = " + stateId + ")", null).ToList();

                        return View(reports);
                    }

                }

            }
        }

        public ActionResult CandidateArea(int functionId, int cityId)
        {
            ViewData["functionId"] = functionId;
            TempData["cityId"] = cityId;
            TempData.Keep();

            using (var _db = new Dial4JobzEntities())
            {
                if (Session["reportType"].ToString() == "Function")
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Functions.Id, dbo.Functions.Name, dbo.Regions.Id AS ID1, dbo.Regions.Name AS Name1, COUNT(*) AS TotalCount, dbo.Regions.CityId AS ID2 FROM dbo.Locations INNER JOIN dbo.Regions ON dbo.Locations.RegionId = dbo.Regions.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Functions.Id, dbo.Functions.Name, dbo.Regions.Name, dbo.Regions.Id, dbo.Regions.CityId", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Functions.Id, dbo.Functions.Name, dbo.Regions.Id AS ID1, dbo.Regions.Name AS Name1, COUNT(*) AS TotalCount, dbo.Regions.CityId AS ID2 FROM dbo.Locations INNER JOIN dbo.Regions ON dbo.Locations.RegionId = dbo.Regions.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Functions ON dbo.Candidates.FunctionId = dbo.Functions.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Functions.Id, dbo.Functions.Name, dbo.Regions.Name, dbo.Regions.Id, dbo.Regions.CityId  HAVING (dbo.Functions.Id =" + functionId + " and  dbo.Regions.CityId = " + cityId + ")", null).ToList();

                        return View(reports);
                    }
                }
                else
                {
                    if (functionId == 0)
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Industries.Id, dbo.Industries.Name, dbo.Regions.Id AS ID1, dbo.Regions.Name AS Name1, COUNT(*) AS TotalCount, dbo.Regions.CityId AS ID2 FROM dbo.Locations INNER JOIN dbo.Regions ON dbo.Locations.RegionId = dbo.Regions.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Industries.Id, dbo.Industries.Name, dbo.Regions.Name, dbo.Regions.Id, dbo.Regions.CityId", null).ToList();

                        return View(reports);
                    }
                    else
                    {
                        var reports = _db.ExecuteStoreQuery<GroupReport>("SELECT dbo.Industries.Id, dbo.Industries.Name, dbo.Regions.Id AS ID1, dbo.Regions.Name AS Name1, COUNT(*) AS TotalCount, dbo.Regions.CityId AS ID2 FROM dbo.Locations INNER JOIN dbo.Regions ON dbo.Locations.RegionId = dbo.Regions.Id INNER JOIN dbo.Candidates INNER JOIN dbo.Industries ON dbo.Candidates.IndustryId = dbo.Industries.Id ON dbo.Locations.Id = dbo.Candidates.LocationId GROUP BY dbo.Industries.Id, dbo.Industries.Name, dbo.Regions.Name, dbo.Regions.Id, dbo.Regions.CityId  HAVING (dbo.Industries.Id =" + functionId + " and  dbo.Regions.CityId = " + cityId + ")", null).ToList();

                        return View(reports);
                    }

                }

            }
        }
        #endregion
        #endregion

        #region Data

        public ActionResult ImportData()
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
                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ImportData)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HandleErrorWithAjaxFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportData(HttpPostedFileBase uploadFile)
        {
            string filePath = string.Empty;

            if (uploadFile.ContentLength > 0)
            {
                var fileGUID = Guid.NewGuid();
                filePath = Path.Combine(HttpContext.Server.MapPath("~/Areas/Admin/Content/Data/"),
                                               Path.GetFileName(fileGUID + uploadFile.FileName));

                string folderPath = HttpContext.Server.MapPath("~/Areas/Admin/Content/Data/");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                uploadFile.SaveAs(filePath);
            }

            bool transResult = Insert2SQL(filePath);
            ViewData["ImportStatus"] = transResult;
            return View();
        }





        private bool Insert2SQL(string filePath)
        {

            OleDbConnection oconn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0");
            try
            {
                OleDbCommand ocmd = new OleDbCommand("select * from [Candidates$]", oconn);
                oconn.Open();
                OleDbDataReader odr = ocmd.ExecuteReader();

                // define our transaction scope
                var scope = new TransactionScope(
                    // a new transaction will always be created
                    TransactionScopeOption.RequiresNew,
                    // we will allow volatile data to be read during transaction
                    new TransactionOptions()
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                );

                using (scope)
                {
                    while (odr.Read())
                    {
                        insertdataintosql(odr);
                    }

                    scope.Complete();
                }

                oconn.Close();


            }
            catch (DataException ee)
            {
                throw ee;
            }

            return true;
        }

        private string valid(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
            {
                string[] splitValue = val.ToString().Split('-');
                if (splitValue.Count() == 2)
                    return Convert.ToString(splitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                return Convert.ToString(0);

            }
        }

        private string validLocation(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
            {
                string[] splitValue = val.ToString().Split('-');
                if (splitValue.Count() == 2)
                    return Convert.ToString(splitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                //return Convert.ToString(0);
                return null;
            }
        }

        private string validateValue(string stval)
        {
            object val = stval;
            if (val != DBNull.Value)
            {
                string[] subSplitValue = val.ToString().Split('-');
                if (subSplitValue.Count() == 2)
                    return Convert.ToString(subSplitValue[1]).Trim();
                else
                    return val.ToString();
            }
            else
            {
                return Convert.ToString(0);
                //return null;
            }
        }

        private void insertdataintosql(OleDbDataReader odr)
        {
            //UserRepository _userRepository = new UserRepository();
            Repository _repository = new Repository();
            Candidate candidate = new Candidate();
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            string excelId = valid(odr, 0);

            if (excelId != "0")
            {
                candidate.Name = valid(odr, 1);
                candidate.Email = validLocation(odr, 2);
                candidate.ContactNumber = valid(odr, 4);
                candidate.MobileNumber = valid(odr, 5);
                candidate.Description = valid(odr, 30);

                Candidate candidateEmail = _userRepository.GetCandidateByEmail(candidate.Email);
                Candidate candidatemobile = _userRepository.GetCandidateByMobileNumber(candidate.ContactNumber);

                if (candidateEmail != null)
                {

                    //ViewData["ImportStatus"] += candidate.Email + ",";
                }
                else if (candidatemobile != null)
                {
                    //ViewData["ImportStatus"] += candidate.MobileNumber + ",";
                }
                else
                {
                    candidate.Address = valid(odr, 3);
                    candidate.Pincode = valid(odr, 29);
                    candidate.Description = valid(odr, 30);
                    candidate.LicenseNumber = valid(odr, 22);
                    candidate.PassportNumber = valid(odr, 23);

                    Random randomNo = new Random();
                    candidate.UserName = candidate.Name + randomNo.Next(1000, 9999).ToString();

                    string password = GetRandomPasswordUsingGUID(6);
                    candidate.Password = SecurityHelper.GetMD5Bytes(password);

                    if (candidate.CreatedDate == null)
                    {
                        candidate.CreatedDate = timeZone;
                    }
                    else
                    {
                        candidate.UpdatedDate = timeZone;
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 6)) && valid(odr, 6) != "0")
                        candidate.DOB = Convert.ToDateTime(valid(odr, 6));

                    if (!string.IsNullOrEmpty(valid(odr, 31)) && valid(odr, 31) != "0")
                        candidate.MaritalId = Convert.ToInt32(valid(odr, 31));

                    if (!string.IsNullOrEmpty(valid(odr, 7)) && valid(odr, 7) != null)
                        candidate.Gender = Convert.ToInt32(valid(odr, 7));

                    if (!string.IsNullOrEmpty(valid(odr, 8)) && valid(odr, 8) != "0")
                    {
                        string[] expSplit = valid(odr, 8).Trim().Split('.');
                        long yearsinseconds = 0;
                        long monthsinseconds = 0;

                        if (expSplit.Length > 0)
                            yearsinseconds = Convert.ToInt64(expSplit[0]) * 365 * 24 * 60 * 60;
                        if (expSplit.Length > 1)
                            monthsinseconds = Convert.ToInt64(expSplit[1]) * 31 * 24 * 60 * 60;

                        candidate.TotalExperience = yearsinseconds + monthsinseconds;
                    }

                    candidate.NumberOfCompanies = Convert.ToInt32(valid(odr, 28));
                    candidate.AnnualSalary = (Convert.ToInt32(valid(odr, 9)));
                    candidate.Position = valid(odr, 10);
                    candidate.PresentCompany = valid(odr, 26);
                    candidate.PreviousCompany = valid(odr, 27);

                    if (!string.IsNullOrEmpty(valid(odr, 11)) && valid(odr, 11) != "0")
                        candidate.FunctionId = Convert.ToInt32(valid(odr, 11));

                    if (!string.IsNullOrEmpty(valid(odr, 12)) && valid(odr, 12) != "0")
                        candidate.IndustryId = Convert.ToInt32(valid(odr, 12));

                    if (!string.IsNullOrEmpty(valid(odr, 14)) && valid(odr, 14) != "0")
                    {
                        if (valid(odr, 14) == "0")
                            candidate.PreferredAll = false;
                        else
                            candidate.PreferredAll = Convert.ToBoolean(valid(odr, 14));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 15)))
                    {
                        if (valid(odr, 15) == "0")
                            candidate.PreferredContract = false;
                        else
                            candidate.PreferredContract = Convert.ToBoolean(valid(odr, 15));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 16)))
                    {
                        if (valid(odr, 16) == "0")
                            candidate.PreferredParttime = false;
                        else
                            candidate.PreferredParttime = Convert.ToBoolean(valid(odr, 16));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 17)))
                    {
                        if (valid(odr, 17) == "0")
                            candidate.PreferredWorkFromHome = false;
                        else
                            candidate.PreferredWorkFromHome = Convert.ToBoolean(valid(odr, 17));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 18)))
                    {
                        if (valid(odr, 18) == "0")
                            candidate.PreferredFulltime = false;
                        else
                            candidate.PreferredFulltime = Convert.ToBoolean(valid(odr, 18));
                    }

                    if (!string.IsNullOrEmpty(valid(odr, 19)) && valid(odr, 19) != "0")
                        //candidate.PreferredTimeFrom = Convert.ToInt16(valid(odr, 19));
                        candidate.PreferredTimeFrom = valid(odr, 19);

                    if (!string.IsNullOrEmpty(valid(odr, 20)) && valid(odr, 20) != "0")
                        //candidate.PreferredTimeTo = Convert.ToInt16(valid(odr, 20));
                        candidate.PreferredTimeTo = valid(odr, 20);

                    _repository.AddCandidate(candidate);
                    //_repository.Save();

                    Location location = new Location();
                    if (!string.IsNullOrEmpty(validLocation(odr, 32))) location.CountryId = Convert.ToInt32(validLocation(odr, 32));
                    if (!string.IsNullOrEmpty(validLocation(odr, 33))) location.StateId = Convert.ToInt32(validLocation(odr, 33));
                    if (!string.IsNullOrEmpty(validLocation(odr, 34))) location.CityId = Convert.ToInt32(validLocation(odr, 34));
                    if (!string.IsNullOrEmpty(validLocation(odr, 35))) location.RegionId = Convert.ToInt32(validLocation(odr, 35));

                    if (location.CountryId != 0)
                        candidate.LocationId = _repository.AddLocation(location);

                    int candidateId = candidate.Id;


                    //Candidates skills
                    string[] skills = valid(odr, 36).Split(',');

                    if (skills.Count() != 0)
                        _repository.DeleteCandidateSkills(candidateId);

                    foreach (string skill in skills)
                    {
                        if (!string.IsNullOrEmpty(skill) && skill != "0")
                        {
                            CandidateSkill cs = new CandidateSkill();
                            cs.CandidateId = candidateId;
                            cs.SkillId = Convert.ToInt32(validateValue(skill));

                            _repository.AddCandidateSkill(cs);
                        }
                    }

                    string[] languages = valid(odr, 37).Split(',');
                    if (languages.Count() != null)
                        //if (languages.Count() != 0)
                        _repository.DeleteCandidateLanguages(candidateId);

                    foreach (string lang in languages)
                    {
                        if (!string.IsNullOrEmpty(lang) && lang != "0")
                        {
                            CandidateLanguage cl = new CandidateLanguage();
                            cl.CandidateId = candidateId;
                            cl.LanguageId = Convert.ToInt32(validateValue(lang));
                            _repository.AddCandidateLanguage(cl);
                        }
                    }

                    //preferredFunctions

                    string[] preferredFunctions = valid(odr, 41).Split(',');
                    if (preferredFunctions.Count() != null)
                        _repository.DeleteCandidatePreferredFunctions(candidateId);

                    //then add new ones
                    foreach (string preferredFunction in preferredFunctions)
                    {
                        if ((!string.IsNullOrEmpty(preferredFunction)) && preferredFunction != "0")
                        //if (!string.IsNullOrEmpty(preferredFunction))
                        {
                            CandidatePreferredFunction cpf = new CandidatePreferredFunction();
                            cpf.CandidateId = candidateId;
                            cpf.FunctionId = Convert.ToInt32(preferredFunction);
                            _repository.AddCandidatePreferredFunction(cpf);
                        }
                    }


                    //Roles

                    string[] roles = valid(odr, 38).Split(',');

                    if (roles.Count() != null)
                    //if (roles.Count() != 0)
                    {
                        _repository.DeleteCandidateRoles(candidateId);
                    }

                    foreach (string preferredRole in roles)
                    {
                        if (!string.IsNullOrEmpty(preferredRole) && preferredRole != "0")
                        {
                            CandidatePreferredRole cpr = new CandidatePreferredRole();
                            cpr.CandidateId = candidateId;
                            cpr.RoleId = Convert.ToInt32(validateValue(preferredRole));
                            _repository.AddCandidatePreferredRole(cpr);
                        }
                    }

                    //licence types
                    string[] licenseTypes = { };
                    if (!string.IsNullOrEmpty(valid(odr, 39)))
                        licenseTypes = valid(odr, 39).Split(',');

                    //delete all license types
                    if (licenseTypes.Count() != null)
                        //if (licenseTypes.Count() != 0)
                        _repository.DeleteCandidateLicenseTypes(candidateId);

                    //then add new ones
                    foreach (string licenseType in licenseTypes)
                    {
                        if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
                        {
                            CandidateLicenseType clt = new CandidateLicenseType();
                            clt.CandidateId = candidateId;
                            clt.LicenseTypeId = Convert.ToInt32(validateValue(licenseType));
                            _repository.AddCandidateLicenseType(clt);
                        }
                    }

                    //Add Candidate Qualification (first clear the existing ones)
                    _repository.DeleteCandidateQualifications(candidate.Id);

                    //qualification
                    string[] qualifications = { };
                    if (!string.IsNullOrEmpty(valid(odr, 40)))
                        qualifications = valid(odr, 40).Split(',');

                    //delete all qualification types

                    if (qualifications.Count() != null)
                        //if (qualifications.Count() != 0)
                        _repository.DeleteCandidateQualifications(candidateId);

                    //then add new ones
                    foreach (string qualification in qualifications)
                    {

                        if (!string.IsNullOrEmpty(qualification) && qualification != "0")
                        {
                            CandidateQualification cq = new CandidateQualification();
                            cq.CandidateId = candidateId;
                            cq.DegreeId = Convert.ToInt32(validateValue(qualification));
                            //  cq.Specialization = string.Empty;

                            if (cq.DegreeId > 0)
                                _repository.AddCandidateQualification(cq);
                        }
                    }

                    _repository.Save();

                    if (candidate.Email != null)
                    {
                        EmailHelper.SendEmail(
                           Constants.EmailSender.CandidateSupport,
                           candidate.Email,
                            //candidate.username,
                           Constants.EmailSubject.Registration,
                           Constants.EmailBody.AdminCandidateRegister
                               .Replace("[NAME]", candidate.Name)
                               .Replace("[USERNAME]", candidate.UserName)
                               .Replace("[PASSWORD]", password)
                               .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Home/HomePage")
                               .Replace("[EMAIL]", candidate.Email)
                               );
                    }

                    //if (candidate.ContactNumber != "0")
                    //{
                    //    SmsHelper.Sendsms(
                    //        Constants.SmsSender.UserId,
                    //        Constants.SmsSender.Password,
                    //        Constants.SmsBody.SMSNewCandidate
                    //                        .Replace("[USER_NAME]", candidate.UserName)
                    //                        .Replace("[PASSWORD]", password),
                    //        Constants.SmsSender.Type,
                    //        Constants.SmsSender.senderId,
                    //        candidate.ContactNumber
                    //        );
                    //}
                }

            }

        }

        public string GetRandomPasswordUsingGUID(int length)
        {
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            return guidResult.Substring(0, length);
        }

        #endregion


       
        
        public ActionResult Download(string fileName)
        {
            string pfn = Server.MapPath("~/Content/Resumes/" + fileName);

            if (!System.IO.File.Exists(pfn))
            {
                return Json(new JsonActionResult { Success = false, Message = "Invalid file name or file not exists!" });
            }
            else
            {
                return new BinaryContentResult()
                {
                    FileName = "" + "\"" + fileName + "\"",
                    ContentType = "application/octet-stream",
                    Content = System.IO.File.ReadAllBytes(pfn)
                };
            }

        }


        public ActionResult CandidateReminder(string orderIds, string flag)
        {
            Candidate candidate = null;
            OrderDetail getOrder = null;
            OrderMaster orderMaster = null;
            if (!string.IsNullOrEmpty(orderIds))
            {
                string[] ids = orderIds.Split(',');
                if (ids != null && ids.Length > 0)
                {
                    foreach (string id in ids)
                    {
                        getOrder = _vasRepository.GetOrderDetail(Convert.ToInt32(id));
                        orderMaster = _vasRepository.GetOrderMaster(Convert.ToInt32(id));
                        if (orderMaster != null)
                        {
                            int candidateId = getOrder.OrderMaster.Candidate.Id;
                            candidate = _repository.GetCandidate(candidateId);
                        }

                        if (candidate.Email != "")
                        {
                            EmailHelper.SendEmail(
                                Constants.EmailSender.CandidateSupport,
                                candidate.Email,
                                "Remainder for the Pending Order",
                                Constants.EmailBody.reminderCandidate
                                .Replace("[NAME]", candidate.Name)
                                .Replace("[SERVICE_NAME]", getOrder.VasPlan.Description)
                                .Replace("[LINK_NAME]", "CLICK HERE")
                                .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(getOrder.OrderId.ToString()).ToString())
                                );
                        }

                        if (candidate.ContactNumber != "")
                        {
                            SmsHelper.SendSecondarySms(
                               Constants.SmsSender.SecondaryUserName,
                               Constants.SmsSender.SecondaryPassword,
                               Constants.SmsBody.CandidateRemainder
                                 .Replace("[Name]", candidate.Name)
                                 ,
                               Constants.SmsSender.SecondaryType,
                               Constants.SmsSender.Secondarysource,
                               Constants.SmsSender.Secondarydlr,
                               candidate.ContactNumber
                               );

                        }
                    }
                }
            }

            if (flag == "candidate")
            {
                return RedirectToAction("GetDetail", "AdminHome", new { validateMobile = candidate.ContactNumber });
            }

            else
            {
                var result = new
                {
                    error = "1",
                    message = "Remainder Sent Successfully.",
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            
        }

        public ActionResult EmployerReminder(string orderIds, string flag)
        {
          
            Organization organization = null;
            OrderDetail getOrder = null;
            OrderMaster orderMaster = null;

            if (!string.IsNullOrEmpty(orderIds))
            {
                string[] ids = orderIds.Split(',');
                if (ids != null && ids.Length > 0)
                {
                    foreach (string id in ids)
                    {
                        getOrder = _vasRepository.GetOrderDetail(Convert.ToInt32(id));
                        orderMaster = _vasRepository.GetOrderMaster(Convert.ToInt32(id));
                        if (orderMaster != null)
                        {
                            int organizationId = getOrder.OrderMaster.Organization.Id;
                            organization = _repository.GetOrganization(organizationId);
                        }

                        if (organization.Email != "")
                        {
                            EmailHelper.SendEmail(
                                Constants.EmailSender.EmployerSupport,
                                organization.Email,
                                "Remainder for the Pending Orders",
                                Constants.EmailBody.reminderEmployer
                                .Replace("[NAME]", organization.Name)
                                .Replace("[SERVICE_NAME]", getOrder.VasPlan.Description)
                                .Replace("[LINK_NAME]", "CLICK HERE")
                                .Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/EmployerVas/Payment?orderId=" + Constants.EncryptString(getOrder.OrderId.ToString()).ToString())
                                );
                        }

                        if (organization.MobileNumber != "")
                        {
                            SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.EmployerRemainder
                                    .Replace("[Name]", organization.Name),
                                Constants.SmsSender.SecondaryType,
                                Constants.SmsSender.Secondarysource,
                                Constants.SmsSender.Secondarydlr,
                                organization.MobileNumber
                                );
                        }
                    }
                }
            }

            if (flag=="employer")
            {
                return RedirectToAction("GetCompanyDetail", "AdminHome", new { validateCompany = organization.Name });
            }

            else
            {
                var result = new
                {
                    error = "1",
                    message = "Remainder Sent Successfully.",
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
                      
             
            
        }


        public ActionResult verifyCandidateByAdmin(int candidateId)
        {
            Candidate candidate = _repository.GetCandidate(candidateId);
            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                candidate.VerifiedByAdmin = user.UserName;
                _repository.Save();
            }

            return RedirectToAction("GetDetail", "AdminHome", new { validateEmail = candidate.Email });
        }


        public ActionResult verifyEmployerByAdmin(int organizationId)
        {
            Organization organization = _repository.GetOrganization(organizationId);
            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                organization.VerifiedByAdmin = user.UserName;
                _repository.Save();
            }
            return RedirectToAction("GetCompanyDetail", "AdminHome", new { validateEmail = organization.Email });
        }

        public ActionResult SendVasDetailsToCandidate(int candidateId)
        {
            Candidate candidate = _repository.GetCandidate(candidateId);

            if (candidate.Email != null || candidate.Email != "")
            {
                StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SendVasDetailsCandidate.htm"));
                string table = reader.ReadToEnd();
                reader.Dispose();
                table = table.Replace("[NAME]", candidate.Name);

                EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                         candidate.Email,
                        "Candidate Vas Details",
                        table);
            }
            return RedirectToAction("GetDetail", "AdminHome", new { validateEmail = candidate.Email });
            //ReturnUrl = "/Admin/JobMatches/JobMatch/" + candidateId.ToString()
        }

        //send vas details to employer

        public ActionResult SendVasDetailsToEmployer(int organizationId, int consultantId)
        {
            Organization organization = _repository.GetOrganization(organizationId);
            Consultante consultant = _repository.GetConsulant(consultantId);
            if (organization != null)
            {
                if (organization.Email != null || organization.Email != "")
                {
                    StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SendVasDetailsEmployer.htm"));
                    string table = reader.ReadToEnd();
                    reader.Dispose();
                    table = table.Replace("[NAME]", organization.Name);

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                             organization.Email,
                            "Employer Vas Details",
                            table);
                }
                return RedirectToAction("GetCompanyDetail", "AdminHome", new { validateEmail = organization.Email });
            }
            else
            {
                if (consultant.Email != null || consultant.Email != "")
                {
                    StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SendVasDetailsEmployer.htm"));
                    string table = reader.ReadToEnd();
                    reader.Dispose();
                    table = table.Replace("[NAME]", consultant.Name);

                    EmailHelper.SendEmail(Constants.EmailSender.EmployerSupport,
                             consultant.Email,
                            "Employer Vas Details",
                            table);
                }
                return RedirectToAction("GetConsultantDetail", "ConsultantReport", new { validateMobileNumber = consultant.MobileNumber });
            }
            
        }

        public ActionResult AccountDetails(int candidateId)
        {
            Candidate candidate = _repository.GetCandidate(candidateId);

            if (candidate.ContactNumber != null || candidate.Email != "")
            {
                SmsHelper.SendSecondarySms(
                    Constants.SmsSender.SecondaryUserName,
                    Constants.SmsSender.SecondaryPassword,
                    Constants.SmsBody.SMSAccountDetails
                    .Replace("[NAME]", candidate.Name),
                    Constants.SmsSender.SecondaryType,
                    Constants.SmsSender.Secondarysource,
                    Constants.SmsSender.Secondarydlr,
                    candidate.ContactNumber
                    );
            }

            if (candidate.Email != "")
            {
                EmailHelper.SendEmail(
                    Constants.EmailSender.CandidateSupport,
                    candidate.Email,
                    Constants.EmailSubject.SendBankDetails,
                    Constants.EmailBody.SendBankDetails
                    .Replace("[NAME]", candidate.Name));
            }



            return RedirectToAction("GetDetail", "AdminHome", new { validateMobile = candidate.ContactNumber });
        }

        public ActionResult AccountDetailsEmployer(int EmployerId, int consultantId)
        {
            Organization organization = _repository.GetOrganization(EmployerId);
            Consultante consultant = _repository.GetConsulant(consultantId);

            if (organization != null)
            {
                if (organization.Email != null || organization.Email != "")
                {
                    EmailHelper.SendEmail(
                            Constants.EmailSender.EmployerSupport,
                            organization.Email,
                            Constants.EmailSubject.SendBankDetails,
                            Constants.EmailBody.SendBankDetails
                            .Replace("[NAME]", organization.ContactPerson));
                }
                if (organization.MobileNumber != null)
                {
                    SmsHelper.SendSecondarySms(
                           Constants.SmsSender.SecondaryUserName,
                           Constants.SmsSender.SecondaryPassword,
                           Constants.SmsBody.SMSAccountDetails
                           .Replace("[NAME]", organization.ContactPerson),
                           Constants.SmsSender.SecondaryType,
                           Constants.SmsSender.Secondarysource,
                           Constants.SmsSender.Secondarydlr,
                           organization.MobileNumber
                           );
                }

                return RedirectToAction("GetCompanyDetail", "AdminHome", new { validateMobile = organization.MobileNumber });
            }
            else
            {
                if (consultant.Email != null || consultant.Email != "")
                {
                    EmailHelper.SendEmail(
                            Constants.EmailSender.EmployerSupport,
                            consultant.Email,
                            Constants.EmailSubject.SendBankDetails,
                            Constants.EmailBody.SendBankDetails
                            .Replace("[NAME]", consultant.ContactPerson));
                }
                if (consultant.MobileNumber != null)
                {
                    SmsHelper.SendSecondarySms(
                           Constants.SmsSender.SecondaryUserName,
                           Constants.SmsSender.SecondaryPassword,
                           Constants.SmsBody.SMSAccountDetails
                           .Replace("[NAME]", consultant.ContactPerson),
                           Constants.SmsSender.SecondaryType,
                           Constants.SmsSender.Secondarysource,
                           Constants.SmsSender.Secondarydlr,
                           consultant.MobileNumber
                           );
                }

                return RedirectToAction("GetCompanyDetail", "AdminHome", new { validateMobile = consultant.MobileNumber });
            }
        }


        #region User

        #region Candidate

        public ActionResult AddCandidate()
        {
            string[] userIdentityName = this.User.Identity.Name.Split('|');
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (user != null || (userIdentityName != null && userIdentityName.Length > 1))
            {
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = null;
                Permission adminPermission = new Permission();
                if (user != null)
                {
                    pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                }

                string pageAccess = "";
                string[] Page_Code = null;
                if (pageaccess != null)
                {
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
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if ((userIdentityName != null && userIdentityName.Length > 1) || (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddCandidate)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true))
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

                    //ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
                    //ViewData["LicenseTypeIds"] = candidate.CandidateLicenseTypes.Select(clt => clt.LicenseTypeId);

                    // License Type

                        var license = _repository.GetLicenseTypesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
                        license.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
                        ViewData["License"] = license;

                    //End License Type

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
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

        }


        public ActionResult ValidateCandidate(string validateEmail, string validateMobile)
        {
            //UserRepository _userRepository = new UserRepository();
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
                    return Json("Mobile Number not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Mobile Number exist", JsonRequestBehavior.AllowGet);
            }
            else
            {
               
            }

         
            return RedirectToAction("GetDetail", "AdminHome");

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

            return RedirectToAction("GetCompanyDetail", "AdminHome");
        }


     // Get the details of Candidate
        public ActionResult GetDetail(string validateEmail, string validateMobile)
        {
            //UserRepository _userRepository = new UserRepository();
            Candidate candidate = null;
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

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
                    Session["LoginUserId"] = user != null ? user.UserName : User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];

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
                    Session["LoginUserId"] = user != null ? user.UserName : User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
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

                List<LicenseType> license = new List<LicenseType>();
                license.Add(new LicenseType { Id = 0, Name = "--- Any ---" });
                var result = _repository.GetLicenseTypes();

                foreach (var li in result)
                {
                    license.Add(new LicenseType { Id = li.Id, Name = li.Name });
                }

                ViewData["LicenseTypes"] = license;
                ViewData["LicenseTypeIds"] = candidate.CandidateLicenseTypes.Select(clt => clt.LicenseTypeId);

            //End License Types New

            ViewData["CandidateFunctions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", candidate.FunctionId);
            
            CandidatePreferredRole cpr = _repository.GetRolesById(candidate.Id);
            int functionid = _repository.GetFunctionIdByCandidateId(candidate.Id);

            if (cpr != null)
                ViewData["Roles"] = new SelectList(_repository.GetRolesByFunctionId(functionid), "Id", "Name", cpr.RoleId);
            else
                ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", candidate.FunctionId.HasValue ? candidate.FunctionId.Value : 0);

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", candidate.IndustryId);

            ViewData["MaritalStatus"] = new SelectList(_repository.GetMaritalStatus(), "Id", "Name", candidate.MaritalId);

            //by vignesh

            IEnumerable<Location> locations = _repository.GetLocationsbyCandidateId(candidate.Id);

            if (locations.Count() > 0)
            {
                ViewData["CountryIds"] = String.Join(",", locations.Select(loc => loc.CountryId));
                ViewData["StateIds"] = String.Join(",", locations.Select(loc => loc.StateId).Where(jr => jr != null));
                ViewData["CityIds"] = String.Join(",", locations.Select(loc => loc.CityId).Where(jr => jr != null));
                ViewData["RegionIds"] = String.Join(",", locations.Select(loc => loc.RegionId).Where(jr => jr != null));
            }

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
        public ActionResult UploadResume(IEnumerable<HttpPostedFileBase> FileData, FormCollection collection)
        {
            string candidateId = Request.Form["candId"];
            Candidate candidate = _repository.GetCandidate(Convert.ToInt32(candidateId));
            foreach (var file in FileData)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Resumes/"), fileName);
                    file.SaveAs(path);
                    if (candidate != null)
                    {
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

        public JsonResult getEmployerName(string Name)
        {
            var result = new
            {
                organization = _repository.GetOrganizationByName(Name)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
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
        
        public JsonResult getCompanyNameLists(string term)
        {
            List<string> getValues = _repository.GetOrganizationName(term).Distinct().ToList();
            return Json(getValues, JsonRequestBehavior.AllowGet);
        }



        //[Authorize,HttpPost, HandleErrorWithAjaxFilter]
        [HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult SaveCandidate(FormCollection collection)
        {
            Candidate candidate = null;
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            bool updateOperation = false;
            var contactNumber = collection["ContactNumber"];
            var email = collection["Email"];
            var Name = collection["Name"];
            var internationalnumber = collection["InternationalNumber"];
            var function = collection["CandidateFunctions"];
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

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
                            return Json(new JsonActionResult { Success = false, Message = "Mobile Number is already exists", ReturnUrl = "/Admin/AdminHome/GetDetail?validateMobile=" + mobileValidate.ContactNumber });
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
                            return Json(new JsonActionResult { Success = false, Message = "Email Id is already Exist", ReturnUrl = "/Admin/AdminHome/GetDetail?validateEmail=" + emailValidate.Email });
                        }
                    }
                        if (!string.IsNullOrEmpty(internationalnumber))
                        {
                            //Executes whenever the Candidate gives international number
                        }
                        
                        else if (string.IsNullOrEmpty(contactNumber))
                        {
                            return Json(new JsonActionResult { Success = false, Message = "Mobile Number is required." });
                        }

                    if (collection["Position"] == null || collection["Position"] == "")
                    {
                        return Json(new JsonActionResult { Success = false, Message = "Position is Required" });
                    }


                    else if ((collection["DOB"]) == null || collection["DOB"] == "")
                    {
                        return Json(new JsonActionResult { Success = false, Message = "DOB is required" });
                    }

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


            /****Developer Note: If Update the Details of Email / Mobile, the following details will process****/
            if (updateOperation == true)
            {
                bool updateEmail = false;
                bool updateMobile = false;

                if (collection["Email"] != "")
                {
                    Candidate checkEmailExists = _repository.GetCandidateByEmail(collection["Email"]);
                    if (checkEmailExists == null)
                    {
                        if (candidate.Email != collection["Email"])
                        {
                            updateEmail = true;
                        }
                    }
                   
                }

               // if (candidate.ContactNumber != null || candidate.ContactNumber!="")
                if (collection["ContactNumber"] != "") 
                {
                    Candidate checkMobileExists = _repository.GetCandidateByMobileNumber(collection["ContactNumber"]);

                    if (checkMobileExists == null)
                    {
                        if (candidate.ContactNumber != collection["ContactNumber"])
                        {
                            updateMobile = true;
                        }
                    }
                   
                }

                var registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(candidate.Id, EntryType.Candidate);
                var orderDetail = _vasRepository.GetOrderDetailsbyCandidateId(candidate.Id);
                var subscribedBy = "";
                User subscribedByAdmin = null;

                if (orderDetail.Count() > 0)
                {
                    foreach (var order in orderDetail)
                    {
                        subscribedBy = order.OrderMaster.SubscribedBy;
                    }
                }


                var registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
                var verifiedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(candidate.VerifiedByAdmin).FirstOrDefault();
                if (subscribedBy != "")
                {
                    subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(subscribedBy).FirstOrDefault();
                }

                if (updateEmail == true && user != null)
                {
                    candidate.IsMailVerified = false;

                    if (candidate.Email != "")
                    {
                        EmailHelper.SendEmailSBCC(
                            Constants.EmailSender.CandidateSupport,
                            candidate.Email,
                            (registeredByAdmin != null ? registeredByAdmin.Email : "smo@dial4jobz.com"),
                            Constants.EmailSender.CandidateSupport,
                            "smc@dial4jobz.com",
                           Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.CandidateUpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[EMAIL]", candidate.Email)
                            .Replace("[ID]", candidate.Id.ToString())
                            .Replace("[NEW_EMAIL]", collection["Email"])
                            .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                            .Replace("[REGISTERED_BY]",(registeredByAdmin !=null ? registeredByAdmin.UserName : candidate.Name))
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }
                    else
                    {
                        EmailHelper.SendEmailSBCC(
                          Constants.EmailSender.CandidateSupport,
                          collection["Email"],
                          (registeredByAdmin != null ? registeredByAdmin.Email : "smo@dial4jobz.com"),
                          Constants.EmailSender.CandidateSupport,
                          "smc@dial4jobz.com",
                          Constants.EmailSubject.VerifyUpdateDetails,
                          Constants.EmailBody.CandidateUpdateProfileAlert
                          .Replace("[NAME]", candidate.Name)
                          .Replace("[EMAIL]", candidate.Email)
                          .Replace("[ID]", candidate.Id.ToString())
                          .Replace("[NEW_EMAIL]", collection["Email"])
                          .Replace("[REGISTERED_BY]",(registeredByAdmin !=null ? registeredByAdmin.UserName : candidate.Name))
                          .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                          .Replace("[LINK_NAME]", "Verify your new Email Id")
                          .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                          .Replace("[CHANGED_BY]", user.UserName)
                          );
                    }

                    if (verifiedByAdmin != null)
                    {
                        EmailHelper.SendEmail
                            (
                            Constants.EmailSender.CandidateSupport,
                            verifiedByAdmin.Email,
                            Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.CandidateUpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[EMAIL]", candidate.Email)
                            .Replace("[ID]", candidate.Id.ToString())
                            .Replace("[NEW_EMAIL]", collection["Email"])
                            .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                            .Replace("[REGISTERED_BY]",(registeredByAdmin !=null ? registeredByAdmin.UserName : candidate.Name))
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }

                    else if (subscribedByAdmin != null)
                    {
                        EmailHelper.SendEmail
                            (
                            Constants.EmailSender.CandidateSupport,
                            (subscribedByAdmin.Email!=null ? subscribedByAdmin.Email : "smo@dial4jobz.com"),
                            Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.CandidateUpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[EMAIL]", candidate.Email)
                            .Replace("[NEW_EMAIL]", collection["Email"])
                             .Replace("[REGISTERED_BY]",(registeredByAdmin !=null ? registeredByAdmin.UserName : candidate.Name))
                             .Replace("[ID]", candidate.Id.ToString())
                            .Replace("[CONTACT_NUMBER]", candidate.ContactNumber)
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Candidates/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(candidate.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                                );
                    }

                    if (candidate.ContactNumber != null)
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.EmailUpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[OLD_EMAIL]", candidate.Email)
                            .Replace("[NEW_EMAIL]", collection["Email"]),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            candidate.ContactNumber
                            );
                    }
                }

                if (updateMobile == true && user!=null)
                {
                    candidate.IsPhoneVerified = false;
                    Random randomNo = new Random();
                    string secondVerify = randomNo.Next(1000, 9999).ToString();
                    candidate.PhoneVerificationNo = Convert.ToInt32(secondVerify);

                    if (candidate.ContactNumber != "")
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.UpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                            .Replace("[NEW_MOBILE]", collection["ContactNumber"])
                            .Replace("[VERIFY_CODE]", secondVerify),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            candidate.ContactNumber
                            );
                    }
                    else
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.UpdateProfileAlert
                            .Replace("[NAME]", candidate.Name)
                            .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                            .Replace("[NEW_MOBILE]", collection["ContactNumber"])
                            .Replace("[VERIFY_CODE]", secondVerify),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            collection["ContactNumber"]
                            );
                    }

                    if (candidate.Email != "")
                    {
                        EmailHelper.SendEmailSBCC(
                                 Constants.EmailSender.CandidateSupport,
                                 candidate.Email,
                                 Constants.EmailSender.CandidateSupport,
                                 "smc@dial4jobz.com",
                                 "smo@dial4jobz.com",
                                 Constants.EmailSubject.VerifyUpdateDetails,
                                 Constants.EmailBody.CandidateUpdateMobileAlert
                                     .Replace("[NAME]", candidate.Name)
                                     .Replace("[ID]", candidate.Id.ToString())
                                     .Replace("[EMAIL]", candidate.Email)
                                     .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : candidate.Name))
                                     .Replace("[MOBILE_NUMBER]", candidate.ContactNumber)
                                     .Replace("[NEW_MOBILE]", collection["ContactNumber"])
                                     .Replace("[VERIFY_CODE]", secondVerify)
                                     .Replace("[CHANGED_BY]", user.UserName)
                                 );
                    }
                }

            }
            /*******End Update alert details*****/

            candidate.Name = collection["Name"];
            candidate.Email = collection["Email"];
            candidate.ContactNumber = collection["ContactNumber"];
            candidate.MobileNumber = collection["MobileNumber"];
            candidate.Address = collection["Address"];
            candidate.Pincode = collection["Pincode"];
            candidate.Description = collection["Description"];
            candidate.LicenseNumber = collection["LicenseNumber"];
            candidate.PassportNumber = collection["PassportNumber"];
            candidate.InternationalNumber = collection["InternationalNumber"];

            //candidate.IsPhoneVerified = false;
            if (candidate.IsPhoneVerified != true)
                candidate.IsPhoneVerified = false;

            if (candidate.IsMailVerified != true)
                candidate.IsMailVerified = false;
            

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

            if (roles.Count() > 0)
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
                //if (!string.IsNullOrEmpty(licenseType))
                if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
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
                //string basicQualificationSpecialization = "BasicQualificationSpecialization" + i.ToString();
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
                        //Specialization = collection[basicQualificationSpecialization],
                        //Specialization = string.Empty,
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
                //var postGraduationSpecialization = "PostGraduationSpecialization" + i.ToString();
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
                        //Specialization = collection[postGraduationSpecialization],
                        //Specialization = string.Empty,
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
                //var doctorateSpecialization = "DoctorateSpecialization" + i;
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
                        //Specialization = collection[doctorateSpecialization],
                        //Specialization = string.Empty,
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
            
          //for new entry

            if (updateOperation == false)
            {
                string[] userIdentityName = this.User.Identity.Name.Split('|');     

                if (user != null)
                {
                    AdminUserEntry adminuserentry = new AdminUserEntry();
                    adminuserentry.AdminId = user.Id;
                    adminuserentry.EntryId = candidate.Id;
                    adminuserentry.EntryType = Convert.ToInt32(EntryType.Candidate);
                    adminuserentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    _repository.AddAdminUserEntry(adminuserentry);
                    _repository.Save();
                }

                else if (userIdentityName != null && userIdentityName.Length > 1)
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
                }

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

                    /*Candidate Details send to candidate to verify*/
                    string candidateBasic = string.Empty;
                    string candidatePost = string.Empty;
                    string candidateDoctorate = string.Empty;
                    foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 0))
                    {
                        if (cq != null && cq.Specialization != null)
                        {
                            candidateBasic += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                        }
                        else
                        {

                        }
                    }

                    foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 1))
                    {
                        if (cq != null)
                        {

                            if (cq.Specialization != null && cq.Specialization != null)
                            {
                                candidatePost += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                            }
                            else
                            {
                                candidatePost += cq.Degree.Name + ",";
                            }
                        }
                        else
                        {
                        }
                    }

                    foreach (Dial4Jobz.Models.CandidateQualification cq in candidate.CandidateQualifications.Where(c => c.Degree.Type == 2))
                    {
                        if (cq != null)
                        {

                            if (cq.Specialization != null)
                            {
                                candidateDoctorate += cq.Degree.Name + "(" + cq.Specialization.Name + ")" + ",";
                            }
                            else
                            {
                                candidateDoctorate += cq.Degree.Name + ",";
                            }
                        }

                    }

                    
             
                    string candidateannualsalary = (candidate.AnnualSalary.HasValue && candidate.AnnualSalary != 0) ? Convert.ToInt32(candidate.AnnualSalary.Value).ToString("c0", new System.Globalization.CultureInfo("en-IN")) : "";


                    int candidateexperience;
                    int candidatemonth;

                    candidateexperience = ((int)(candidate.TotalExperience.Value / 31104000));
                    candidatemonth = ((int)((candidate.TotalExperience.Value - ((candidateexperience) * 31536000)) / 2678400));

                    string candidatelocation = string.Empty;
                    string cityName = string.Empty;
                    if (candidate.LocationId != null)
                    {
                        if (candidate.GetLocation(candidate.LocationId.Value).CityId.HasValue && candidate.GetLocation(candidate.LocationId.Value).CityId != 0)
                        {
                            cityName = candidate.GetLocation(candidate.LocationId.Value).City.Name + ",";
                        }
                        if (candidate.GetLocation(candidate.LocationId.Value).CountryId != null && candidate.GetLocation(candidate.LocationId.Value).CountryId != 0)
                        {
                            candidatelocation = candidate.GetLocation(candidate.LocationId.Value).Country.Name;
                        }
                    }

                    string industry = string.Empty;
                    //function = (candidate.FunctionId.HasValue && candidate.FunctionId != 0) ? candidate.FunctionId.Value.ToString() : string.Empty;
                    if (industry == string.Empty)
                    {
                        industry= (candidate.IndustryId==null ? "Any" :candidate.GetIndustry(candidate.IndustryId.Value).Name);
                    }

                    string preffunction = string.Empty;
                    
                    if (candidate.CandidatePreferredFunctions != null)
                    {
                        foreach (CandidatePreferredFunction cpf in candidate.CandidatePreferredFunctions)
                        {
                            if (preffunction == string.Empty)
                            {
                                preffunction = cpf.Function.Name;
                            }
                            else
                            {

                            }
                        }
                    }

                    string role = string.Empty;

                    if (candidate.CandidatePreferredRoles != null)
                    {
                        foreach (CandidatePreferredRole cr in candidate.CandidatePreferredRoles)
                        {
                            if (role == string.Empty)
                            {
                                role = cr.Role.Name;
                            }
                            else
                            {

                            }
                        }
                    }

                    string prefcityName = string.Empty;
                    string prefcountryName = string.Empty;
                    string prefregionName = string.Empty;


                    if (candidate.CandidatePreferredLocations != null)
                    {
                        foreach (CandidatePreferredLocation cpl in candidate.CandidatePreferredLocations)
                        {
                            if (prefcityName == string.Empty)
                            {
                                if (cpl.Location.City != null)
                                {
                                    prefcityName = cpl.Location.City.Name;
                                }
                                else
                                {

                                }
                            }

                            if (prefcountryName == string.Empty)
                            {
                                if (cpl.Location.Country != null)
                                {
                                    prefcountryName = cpl.Location.Country.Name;
                                }
                                else
                                {
                                }
                            }

                            if (prefregionName == string.Empty)
                            {
                                if (cpl.Location.Region != null)
                                {
                                    prefregionName = cpl.Location.Region.Name;
                                }
                                else
                                {
                                }
                            }

                        }
                    }

                    DateTime birth = DateTime.Parse(candidate.DOB.Value.ToShortDateString());
                    DateTime today = DateTime.Today;
                    int age = today.Year - birth.Year;    //people perceive their age in years
                    if (
                       today.Month < birth.Month
                       ||
                       ((today.Month == birth.Month) && (today.Day < birth.Day))
                       )
                    {
                        age--; 
                    }

                    
                     SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.CandidateDetails
                           .Replace("[NAME]", candidate.Name)
                           .Replace("[EMAIL]", (candidate.Email != null ? candidate.Email : ""))
                           .Replace("[MOBILE_NUMBER]", (candidate.ContactNumber != "" ? candidate.ContactNumber : ""))
                           .Replace("[QUALIFICATION]", candidateBasic + "," + (candidatePost != "" ? candidatePost : "") + "," + (candidateDoctorate != "" ? candidateDoctorate : ""))
                           .Replace("[FUNCTION]", candidate.FunctionId == null || candidate.FunctionId == 0 ? "" : candidate.Function.Name)
                           .Replace("[DESIGNATION]", candidate.Position)
                           .Replace("[ANNUAL_SALARY]", (candidateannualsalary != "" ? candidateannualsalary + "Per annum" : "NA"))
                           .Replace("[COUNTRY]", (candidatelocation != "" ? candidatelocation : "NA"))
                           .Replace("[CITY]", (cityName != "" ? cityName.ToString() : "NA"))
                           .Replace("[PREF_COUNTRY]", (prefcountryName != "" ? prefcountryName : "NA"))
                           .Replace("[PREF_CITY]", (prefcityName != "" ? prefcityName : "NA"))
                           .Replace("[PREF_FUNC]", (preffunction != "" ? preffunction.ToString() : "NA"))
                           .Replace("[INDUSTRY]", (industry != "" ? industry : "NA"))
                           .Replace("[DOB]", age.ToString())
                           .Replace("[ROLE]", (role != "" ? role : "NA"))
                           .Replace("[YEARS]", (candidateexperience!=0 ? candidateexperience.ToString() + "Years" : "NM exp yrs"))
                           .Replace("[MONTHS]", (candidatemonth != 0 ? candidatemonth.ToString() + "Months" : "NM exp months"))
                           .Replace("[GENDER]", candidate.Gender == 0 ? "Male" : "Female")
                           ,

                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            candidate.ContactNumber
                            );

                    /*End Candidate Details send*/
                }
            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Profile has been updated",
                //ReturnUrl = "JobMatchesCandidate/JobMatch/" + candidateId.ToString()
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
                                        .Replace("[NAME]",candidate.Name),

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
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/AdminHome/AddCandidate/")

            });
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
            return View("AddCandidate", "AdminHome");

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
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/AdminHome/AddCandidate/")

            });
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
                                .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/AdminHome/CandidateEmailActivation?Id=" + Constants.EncryptString(candidate.Id.ToString()))
                                );
                }
            }

            return RedirectToAction("AddCandidate", "AdminHome");
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
                Session["LoginUserId"]= user.UserName;
                if (candidate == null)
                    candidate = new Candidate();
            }
            else
            {
                if (candidate == null)
                    candidate = new Candidate();
            }
            return View("AddCandidate", "AdminHome");

        }

        
        public ActionResult CandidateEmailActivation(string Id)
        {
            Candidate candidate = null;
            if (!string.IsNullOrEmpty(Id))
            {
                Id = Constants.DecryptString(Id);
                candidate = _userRepository.GetCandidateById(Convert.ToInt32(Id));
                candidate.IsMailVerified = true;
                _userRepository.Save();
            }
            else
            {

            }
            return View();
        }


        #endregion



        #region SendInvoiceBymail For Candidate
        public class GenerateDocument
        {
            public void GenerateWord(OrderDetail orderDetails, string templateDoc, string filename, Candidate candidate)
            {
                // Copy a new file name from template file
                System.IO.File.Copy(templateDoc, filename, true);

                // Open the new Package
                Package pkg = Package.Open(filename, FileMode.Open, FileAccess.ReadWrite);

                // Specify the URI of the part to be read
                Uri uri = new Uri("/word/document.xml", UriKind.Relative);
                PackagePart part = pkg.GetPart(uri);

                XmlDocument xmlMainXMLDoc = new XmlDocument();
                xmlMainXMLDoc.Load(part.GetStream(FileMode.Open, FileAccess.Read));

                xmlMainXMLDoc.InnerXml = ReplacePlaceHoldersInTemplate(orderDetails, xmlMainXMLDoc.InnerXml, candidate);

                // Open the stream to write document
                StreamWriter partWrt = new StreamWriter(part.GetStream(FileMode.Open, FileAccess.Write));
                //doc.Save(partWrt);
                xmlMainXMLDoc.Save(partWrt);

                partWrt.Flush();
                partWrt.Close();
                pkg.Close();
            }
            private string ReplacePlaceHoldersInTemplate(OrderDetail orderDetail, string templateBody, Candidate candidate)
            {
                int actualAmount = Convert.ToInt32((orderDetail.Amount.Value - ((orderDetail.Amount.Value / 112.36) * 12.36)));
                int serviceTax = Convert.ToInt32((orderDetail.Amount.Value / 112.36) * 12.36);
                double discount = 0;

                int total = actualAmount + serviceTax;
                if (orderDetail.DiscountAmount != null)
                {
                    discount = actualAmount * 25 / 100;
                    actualAmount = Convert.ToInt32(actualAmount - discount);
                    serviceTax = Convert.ToInt32(actualAmount * 12.36 / 100);
                }
                templateBody = templateBody.Replace("[Name]", candidate.Name.ToString());
                templateBody = templateBody.Replace("[ContactPerson]", "");
                templateBody = templateBody.Replace("[Address]", candidate.Address.ToString() != null ? candidate.Address.ToString() : "");
                templateBody = templateBody.Replace("[Pincode]", candidate.Pincode);
                templateBody = templateBody.Replace("[Mobile]", candidate.ContactNumber != null ? candidate.ContactNumber : "");
                templateBody = templateBody.Replace("[InvoiceNo]", orderDetail.OrderId.ToString());
                templateBody = templateBody.Replace("[Date]", orderDetail.OrderMaster.OrderDate != null ? orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                templateBody = templateBody.Replace("[ValidityDays]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.Value.ToString() + " days" : "");
                templateBody = templateBody.Replace("[CustomerId]", string.Format("{0}", candidate.Id));
                templateBody = templateBody.Replace("[Plan]", string.Format("{0}", orderDetail.VasPlan.PlanName));


                templateBody = templateBody.Replace("[ActualAmount]", actualAmount.ToString());
                templateBody = templateBody.Replace("[Discount]", discount.ToString());
                templateBody = templateBody.Replace("[Amount]", actualAmount.ToString());
                templateBody = templateBody.Replace("[ServiceTax]", serviceTax.ToString());
                templateBody = templateBody.Replace("[Total]", orderDetail.DiscountAmount != null ? (actualAmount + serviceTax).ToString() : (actualAmount + serviceTax).ToString());
                string words = Dial4Jobz.Helpers.StringHelper.NumbersToWords(orderDetail.Amount != null ? Convert.ToInt32(actualAmount + serviceTax) : 0);


                templateBody = templateBody.Replace("[Words]", words);
                if (orderDetail.OrderPayment != null && orderDetail.OrderPayment.OrderMaster.PaymentStatus == true)
                {
                    templateBody = templateBody.Replace("[PaymentMode]", string.Format("{0}", "Mode of Payment: " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()));
                    templateBody = templateBody.Replace("[PaymentDetails]", string.Format("{0}{1}", ("Received payment through " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()), (" On " + orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"))));
                }
                else
                {
                    templateBody = templateBody.Replace("[PaymentMode]", " ");
                    templateBody = templateBody.Replace("[PaymentDetails]", " ");
                }
                //…
                return (templateBody);
            }
        }

        public ActionResult SendInvoiceBymailToCandidate(int orderId, int Id)
        {
            OrderDetail orderDetails = _vasRepository.GetOrderDetail(orderId);
            GenerateDocument doc = new GenerateDocument();

            Candidate candidate = _repository.GetCandidate(Id);
            
            string TemplateName = "Test.docx";
            string TargetFile = "InvoiceBill.docx";

            //string pfn = Server.MapPath("~/Content/Resumes/" + fileName);
            string docTemplatePath = Server.MapPath("~/Content/Invoice/" + TemplateName);
            string docOutputPath = Server.MapPath("~/Content/Invoice/" + TargetFile);           
            
            doc.GenerateWord(orderDetails, docTemplatePath, docOutputPath, candidate);

            EmailHelper.SendEmailWithAttachment(
                Constants.EmailSender.CandidateSupport,
                candidate.Email,
                Constants.EmailSubject.SendInvoiceBymail,
                "Please check Your invoice in the attachment",
                Constants.EmailSender.CandidateSupport,
                docOutputPath
                );

            return RedirectToAction("SubscriptionBillingForCandidate", "AdminHome", new { candidateId = Id });


        }
        //End Vignesh
        #endregion

        #endregion


        #region Employer


        #region SendInvoiceBymail For Employer
        //by Vignesh
        public class GenerateDocumentForEmployer
        {
            public void GenerateWord(OrderDetail orderDetails, string templateDoc, string filename, Organization organization)
            {
                // Copy a new file name from template file
                System.IO.File.Copy(templateDoc, filename, true);

                // Open the new Package
                Package pkg = Package.Open(filename, FileMode.Open, FileAccess.ReadWrite);

                // Specify the URI of the part to be read
                Uri uri = new Uri("/word/document.xml", UriKind.Relative);
                PackagePart part = pkg.GetPart(uri);

                XmlDocument xmlMainXMLDoc = new XmlDocument();
                xmlMainXMLDoc.Load(part.GetStream(FileMode.Open, FileAccess.Read));

                xmlMainXMLDoc.InnerXml = ReplacePlaceHoldersInTemplate(orderDetails, xmlMainXMLDoc.InnerXml, organization);

                // Open the stream to write document
                StreamWriter partWrt = new StreamWriter(part.GetStream(FileMode.Open, FileAccess.Write));
                //doc.Save(partWrt);
                xmlMainXMLDoc.Save(partWrt);

                partWrt.Flush();
                partWrt.Close();
                pkg.Close();
            }
            private string ReplacePlaceHoldersInTemplate(OrderDetail orderDetail, string templateBody, Organization organization)
            {
                int actualAmount = Convert.ToInt32((orderDetail.Amount.Value - ((orderDetail.Amount.Value / 112.36) * 12.36)));
                int serviceTax = Convert.ToInt32((orderDetail.Amount.Value / 112.36) * 12.36);
                double discount = 0;

                int total = actualAmount + serviceTax;
                if (orderDetail.DiscountAmount != null)
                {
                    discount = actualAmount * 25 / 100;
                    actualAmount = Convert.ToInt32(actualAmount - discount);
                    serviceTax = Convert.ToInt32(actualAmount * 12.36 / 100);
                }
                templateBody = templateBody.Replace("[Name]", organization.Name.ToString());
                templateBody = templateBody.Replace("[ContactPerson]", organization.ContactPerson.ToString() != null ? organization.ContactPerson : "");
                templateBody = templateBody.Replace("[Address]", organization.Address.ToString() != null ? organization.Address.ToString() : "");
                templateBody = templateBody.Replace("[Pincode]", organization.Pincode);
                templateBody = templateBody.Replace("[Mobile]", organization.MobileNumber != null ? organization.MobileNumber : "");
                templateBody = templateBody.Replace("[InvoiceNo]", orderDetail.OrderId.ToString());
                templateBody = templateBody.Replace("[Date]", orderDetail.OrderMaster.OrderDate != null ? orderDetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy") : orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"));
                //templateBody = templateBody.Replace("[OrderId]", string.Format("{0}", orderDetail.OrderId));
                templateBody = templateBody.Replace("[ValidityDays]", orderDetail.VasPlan.ValidityDays != null ? orderDetail.VasPlan.ValidityDays.Value.ToString() + " days" : "");
                templateBody = templateBody.Replace("[CustomerId]", string.Format("{0}", organization.Id));
                templateBody = templateBody.Replace("[Plan]", string.Format("{0}", orderDetail.VasPlan.PlanName));


                templateBody = templateBody.Replace("[ActualAmount]", actualAmount.ToString());
                templateBody = templateBody.Replace("[Discount]", discount.ToString());
                templateBody = templateBody.Replace("[Amount]", actualAmount.ToString());
                templateBody = templateBody.Replace("[ServiceTax]", serviceTax.ToString());
                templateBody = templateBody.Replace("[Total]", orderDetail.DiscountAmount != null ? (actualAmount + serviceTax).ToString() : (actualAmount + serviceTax).ToString());
                string words = Dial4Jobz.Helpers.StringHelper.NumbersToWords(orderDetail.Amount != null ? Convert.ToInt32(actualAmount + serviceTax) : 0);


                templateBody = templateBody.Replace("[Words]", words);
                if (orderDetail.OrderPayment != null && orderDetail.OrderPayment.OrderMaster.PaymentStatus == true)
                {
                    templateBody = templateBody.Replace("[PaymentMode]", string.Format("{0}", "Mode of Payment: " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()));
                    templateBody = templateBody.Replace("[PaymentDetails]", string.Format("{0}{1}", ("Received payment through " + ((Dial4Jobz.Models.Enums.PaymentMode)orderDetail.OrderPayment.PaymentMode).ToString()), (" On " + orderDetail.ActivationDate.Value.ToString("dd-MM-yyyy"))));
                }
                else
                {
                    templateBody = templateBody.Replace("[PaymentMode]", " ");
                    templateBody = templateBody.Replace("[PaymentDetails]", " ");
                }
                //…
                return (templateBody);
            }
        }

        public ActionResult SendInvoiceBymailToEmployer(int orderId, int Id)
        {
            OrderDetail orderDetails = _vasRepository.GetOrderDetail(orderId);
            GenerateDocumentForEmployer doc = new GenerateDocumentForEmployer();

            Organization organization = _repository.GetOrganizationById(Id);

            string docTemplatePath = Server.MapPath("~/Content/Invoice/Test.docx");

            string docOutputPath = Server.MapPath("~/Content/Invoice/InvoiceBill.docx");
            doc.GenerateWord(orderDetails, docTemplatePath, docOutputPath, organization);

            EmailHelper.SendEmailWithAttachment(
                Constants.EmailSender.CandidateSupport,
                organization.Email,
                Constants.EmailSubject.SendInvoiceBymail,
                "Please check Your invoice in the attachment......",
                Constants.EmailSender.CandidateSupport,
                docOutputPath
                );

            return RedirectToAction("EmployerSubscription_Billing", "AdminHome", new { organizationId = Id });


        }
        //End Vigneshe
        #endregion

        public ActionResult ValidateCompany(string validateCompany, string validateEmail, string validateMobileNumber)
        {
            //UserRepository _userRepository = new UserRepository();
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
            //UserRepository _userRepository = new UserRepository();
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
                                       .Replace("[NAME]", organization.Name != null ? organization.Name : "" )
                                       .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

                       Constants.SmsSender.SecondaryType,
                       Constants.SmsSender.Secondarysource,
                       Constants.SmsSender.Secondarydlr,
                       organization.MobileNumber
                       );

                    //SmsHelper.Sendsms(
                    //    Constants.SmsSender.UserId,
                    //    Constants.SmsSender.Password,
                    //    Constants.SmsBody.SMSMobileVerification
                    //                    .Replace("[PIN_NUMBER]", verificationNumber.ToString()),

                    //    Constants.SmsSender.Type,
                    //    Constants.SmsSender.senderId,
                    //    organization.MobileNumber
                    //    );
                }
            }
            return Json(new JsonActionResult
            {
                Success = true,
                Message = "MobileNumber verified successfully",
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/AdminHome/AddEmployer/")

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
                ReturnUrl = VirtualPathUtility.ToAbsolute("~/AdminHome/AddEmployer/")

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
                        //candidate.username,
                            Constants.EmailSubject.EmailVerification,
                            Constants.EmailBody.EmailVerification
                                .Replace("[EMAIL]", organization.Email)
                                .Replace("[LINK_NAME]", "Activate this Link")
                               // .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/AdminHome/Activation?Id=" + organization.Id.ToString())
                               .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Admin/AdminHome/Activation?Id=" + Constants.EncryptString(organization.Id.ToString()))
                                );
                }
            }

            return RedirectToAction("AddEmployer", "AdminHome");
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




        public ActionResult Activation(string Id)
        {
            //Organization organization = null;

            //if (Id != null)
            //{
            //    organization = _userRepository.GetOrganizationById(Id);
            //    organization.IsMailVerified = true;
            //    _userRepository.Save();
            //}
            //else
            //{

            //}
            //return View();

            //BY VIGNESH//
            Organization organization = null;
            if (!string.IsNullOrEmpty(Id))
            {
                Id = Constants.DecryptString(Id);
                organization = _userRepository.GetOrganizationById(Convert.ToInt32(Id));
                organization.IsMailVerified = true;
                _userRepository.Save();
            }
            else
            {

            }
            return View();
            //END VIGNESH//
        }
        #endregion

      

        public ActionResult GetCompanyDetail(string validateCompany, string validateEmail, string validateMobileNumber)
        {

            Organization organization = null;
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (!string.IsNullOrEmpty(validateCompany))
            {
                organization = _userRepository.GetOrganizationByName(validateCompany);
                
                
                if (organization == null)
                    organization = new Organization();

                else
                {
                    Session["LoginAs"] = "EmployerViaAdmin";
                    Session["empId"] = organization.Id;
                    Session["LoginUser"] = user != null ? user.UserName : this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
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
                    Session["LoginUser"] = user != null ? user.UserName : this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
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

            /**Developer Note: To select a particular Industries for Employer Type**/
                List<SelectListItem> consultantindustries = new List<SelectListItem>();
                consultantindustries.Add(new SelectListItem { Text = "Home Needs", Value = "2378" });
                ViewData["ConsultantIndustries"] = new SelectList(consultantindustries, "Value", "Text", (organization.IndustryId != null ? organization.IndustryId : 0));
            /*End*/

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

            ViewData["NumberOfEmployees"] = new SelectList(_repository.GetEmployeesCount(), "Id", "Name", organization.EmployersCount);

            List<SelectListItem> OwnershipType = new List<SelectListItem>();
            OwnershipType.Add(new SelectListItem { Text = "Public Limited Company", Value = "1" });
            OwnershipType.Add(new SelectListItem { Text = "Private Limited Company", Value = "2" });
            OwnershipType.Add(new SelectListItem { Text = "Partnership", Value = "3" });
            OwnershipType.Add(new SelectListItem { Text = "Sole Proprietorship", Value = "4" });
            OwnershipType.Add(new SelectListItem { Text = "Professional Association", Value = "5" });
            ViewData["OwnershipType"] = new SelectList(OwnershipType, "Value", "Text", organization.OwnershipType);
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


        public ActionResult AddEmployer()
        {
            string[] userIdentityName = this.User.Identity.Name.Split('|');
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null || (userIdentityName != null && userIdentityName.Length > 1))
            {
                IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = null;
                Permission adminPermission = new Permission();
                if (user != null)
                {
                    pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
                }

                string pageAccess = "";
                string[] Page_Code = null;
                if (pageaccess != null)
                {
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
                }
                if (!string.IsNullOrEmpty(pageAccess))
                {
                    Page_Code = pageAccess.Split(',');
                }

                if ((userIdentityName != null && userIdentityName.Length > 1) || (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddEmployer)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true))
                {
                   
                    var organization = new Organization();

                    /**Developer Note: To select a particular Industries for Employer Type**/
                        List<SelectListItem> consultantindustries = new List<SelectListItem>();
                        consultantindustries.Add(new SelectListItem { Text = "Home Needs", Value = "2378" });
                        ViewData["ConsultantIndustries"] = new SelectList(consultantindustries, "Value", "Text", (organization.IndustryId != null ? organization.IndustryId : 0));
                    /*End*/
                        
                    ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", organization.IndustryId);

                    ViewData["NumberOfEmployees"] = new SelectList(_repository.GetEmployeesCount(), "Id", "Name", organization.EmployersCount);

                    List<SelectListItem> OwnershipType = new List<SelectListItem>();
                    OwnershipType.Add(new SelectListItem { Text = "Public Limited Company", Value = "1" });
                    OwnershipType.Add(new SelectListItem { Text = "Private Limited Company", Value = "2" });
                    OwnershipType.Add(new SelectListItem { Text = "Partnership", Value = "3" });
                    OwnershipType.Add(new SelectListItem { Text = "Sole Proprietorship", Value = "4" });
                    OwnershipType.Add(new SelectListItem { Text = "Professional Association", Value = "5" });
                    ViewData["OwnershipType"] = new SelectList(OwnershipType, "Value", "Text", organization.OwnershipType);
                

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
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult SaveEmployer(FormCollection collection)
        {
            Organization organization = null;
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            bool updateOperation = false;
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            //Developer Note: Its for admin permissions for add employer wothout mobile number..
            IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = null;
            Permission adminPermission = new Permission();
            if (user != null)
            {
                pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
            }

            var ownershipType = collection["OwnershipType"];
            var numberOfEmployees = collection["NumberOfEmployees"];
            var employerType = collection["EmployerType"];

            if (string.IsNullOrEmpty(employerType))
            {
                return Json(new JsonActionResult { Success = false, Message = "EmployerType is required." });
            }
            else if (!string.IsNullOrEmpty(employerType) && string.IsNullOrEmpty(numberOfEmployees) && employerType != "1")
            {
                return Json(new JsonActionResult { Success = false, Message = "Number Of Employees is required." });
            }
            else if (!string.IsNullOrEmpty(employerType) && string.IsNullOrEmpty(ownershipType) && employerType != "1")
            {
                return Json(new JsonActionResult { Success = false, Message = "Ownership Type is required." });
            }


            var consultIndustryId= collection["Industries"];

            if (!string.IsNullOrEmpty(collection["Id"]))
            {
                int currentId = Convert.ToInt32(collection["Id"]);
                if (currentId == 0)
                {
                    var name = collection["Name"];

                    var organizationName = _userRepository.GetOrganizationByName(collection["Name"]);
                    
                    if (organizationName != null)
                    {
                        return Json(new JsonActionResult { 
                            Success = false, 
                            Message = "Company Name is already Registered.",
                            ReturnUrl="/Admin/AdminHome/GetCompanyDetail?validateCompany="+organizationName.Name });
                    }

                    if (collection["Email"] != "" )
                    {
                        var organizationEmail = _userRepository.GetOrganizationByEmail(collection["Email"]);
                        if (organizationEmail != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Email id is already registered",
                                ReturnUrl="/Admin/AdminHome/GetCompanyDetail?validateEmail="+ organizationEmail.Email
                            });
                            
                        }

                        else
                        {
                            organization = new Organization();
                        }
                    }

                    if (collection["MobileNumber"] != "" )
                    { 
                        var organizationMobile = _userRepository.GetOrganizationByMobileNumber(collection["MobileNumber"]);
                        if (organizationMobile != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Mobile Number is already registered",
                                ReturnUrl = "/Admin/AdminHome/GetCompanyDetail?validateMobileNumber=" + organizationMobile.MobileNumber
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

            #region SendDetailsOfUpdate
            /****Developer Note: If Update the Details of Email / Mobile, the following details will process****/
            if (updateOperation == true)
            {
                bool updateEmail = false;
                bool updateMobile = false;
               // bool updateCompany = false;

                /*Developer Note: First I checked with organization.Email, But if Email field is empty it is throwing an error. So I am going to check with Collection*/
               // if (organization.Email != null || organization.Email!="")
                if(collection["Email"]!= "")
                {
                    Organization checkEmailExists = _repository.GetOrganizationByEmail(collection["Email"]);
                    if (checkEmailExists == null)
                    {
                        if (organization.Email != collection["Email"])
                        {
                            updateEmail = true;
                        }
                    }
                }

                //if (organization.MobileNumber != null)
                if(collection["MobileNumber"] !="")
                {
                    Organization checkMobileExists = _repository.GetOrganizationMobileNumber(collection["MobileNumber"]);

                    if (checkMobileExists==null)
                    {
                        if (organization.MobileNumber != collection["MobileNumber"])
                        {
                            updateMobile = true;
                        }
                    }
                  
                }

                var registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(organization.Id, EntryType.Employer);
                var orderDetail = _vasRepository.GetOrderDetailsbyOrgId(organization.Id);
                var subscribedBy = "";
                User subscribedByAdmin = null;

                if (orderDetail.Count() > 0)
                {
                    foreach (var order in orderDetail)
                    {
                        subscribedBy = order.OrderMaster.SubscribedBy;
                    }
                }

                
                var registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
                var verifiedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(organization.VerifiedByAdmin).FirstOrDefault();
                if (subscribedBy != "")
                {
                    subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(subscribedBy).FirstOrDefault();
                }

                if (updateEmail == true)
                {
                    organization.IsMailVerified = false;

                    if (organization.Email != "")
                    {
                        EmailHelper.SendEmailSBCC(
                            Constants.EmailSender.EmployerSupport,
                            organization.Email,
                            (registeredByAdmin != null ? registeredByAdmin.Email : "smo@dial4jobz.com"),
                            Constants.EmailSender.EmployerSupport,
                            "smc@dial4jobz.com",
                           Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.EmployerUpdateProfileAlert
                            .Replace("[NAME]", organization.Name)
                            .Replace("[ID]", organization.Id.ToString())
                             .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : organization.Name))
                            .Replace("[EMAIL]", organization.Email)
                            .Replace("[NEW_EMAIL]", collection["Email"])
                            .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }
                    else
                    {
                        EmailHelper.SendEmailSBCC(
                            Constants.EmailSender.EmployerSupport,
                            collection["Email"],
                            (registeredByAdmin != null ? registeredByAdmin.Email : "smo@dial4jobz.com"),
                            Constants.EmailSender.EmployerSupport,
                            "smc@dial4jobz.com",
                           Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.EmployerUpdateProfileAlert
                            .Replace("[NAME]", organization.Name)
                            .Replace("[ID]", organization.Id.ToString())
                            .Replace("[EMAIL]", organization.Email)
                            .Replace("[NEW_EMAIL]", collection["Email"])
                             .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : organization.Name))
                            .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }

                    if (verifiedByAdmin != null)
                    {
                        EmailHelper.SendEmail
                            (
                            Constants.EmailSender.EmployerSupport,
                           verifiedByAdmin.Email,
                           Constants.EmailSubject.VerifyUpdateDetails,
                           Constants.EmailBody.EmployerUpdateProfileAlert
                            .Replace("[NAME]", organization.Name)
                            .Replace("[ID]", organization.Id.ToString())
                            .Replace("[EMAIL]", organization.Email)
                             .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : organization.Name))
                            .Replace("[NEW_EMAIL]", collection["Email"])
                            .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }

                    else if (subscribedByAdmin != null)
                    {
                        EmailHelper.SendEmail
                            (
                            Constants.EmailSender.EmployerSupport,
                            (subscribedByAdmin.Email!=null ? subscribedByAdmin.Email : "smo@dial4jobz.com"),
                        Constants.EmailSubject.VerifyUpdateDetails,
                            Constants.EmailBody.EmployerUpdateProfileAlert
                            .Replace("[NAME]", organization.Name)
                            .Replace("[ID]", organization.Id.ToString())
                            .Replace("[EMAIL]", organization.Email)
                              .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : organization.Name))
                            .Replace("[NEW_EMAIL]", collection["Email"])
                            .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                            .Replace("[LINK_NAME]", "Verify your new Email Id")
                            .Replace("[VERIFY_LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(organization.Id.ToString()))
                            .Replace("[CHANGED_BY]", user.UserName)
                            );
                    }

                    if (organization.MobileNumber != null)
                    {
                        SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.EmailUpdateProfileAlert
                            .Replace("[NAME]", organization.Name)
                            .Replace("[OLD_EMAIL]", organization.Email)
                            .Replace("[NEW_EMAIL]", collection["Email"]),
                           // .Replace("[VERIFY_CODE]", secondVerify)
                          //  .Replace("[CHANGED_BY]", user.UserName),
                            Constants.SmsSender.SecondaryType,
                            Constants.SmsSender.Secondarysource,
                            Constants.SmsSender.Secondarydlr,
                            organization.MobileNumber
                            );

                    }
                }

                    if (updateMobile == true)
                    {
                        organization.IsPhoneVerified = false;
                        Random randomNo = new Random();
                        string secondVerify = randomNo.Next(1000, 9999).ToString();
                        organization.PhoneVerificationNo = Convert.ToInt32(secondVerify);

                        if (organization.MobileNumber != null)
                        {
                            SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.UpdateProfileAlert
                                .Replace("[NAME]", organization.Name)
                                .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                                .Replace("[NEW_MOBILE]", collection["MobileNumber"])
                                .Replace("[VERIFY_CODE]", secondVerify),
                                //.Replace("[CHANGED_BY]", user.UserName),
                                Constants.SmsSender.SecondaryType,
                                Constants.SmsSender.Secondarysource,
                                Constants.SmsSender.Secondarydlr,
                                organization.MobileNumber
                                );
                        }
                        if(collection["MobileNumber"]!=null)
                        {
                            SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.UpdateProfileAlert
                               .Replace("[NAME]", organization.Name)
                                .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                                .Replace("[NEW_MOBILE]", collection["MobileNumber"])
                                .Replace("[VERIFY_CODE]", secondVerify),
                                Constants.SmsSender.SecondaryType,
                                Constants.SmsSender.Secondarysource,
                                Constants.SmsSender.Secondarydlr,
                                collection["MobileNumber"]
                                );
                        }

                       
                            if (organization.Email != "")
                            {
                                EmailHelper.SendEmailSBCC(
                                         Constants.EmailSender.EmployerSupport,
                                         organization.Email,
                                         Constants.EmailSender.EmployerSupport,
                                         "smc@dial4jobz.com",
                                         "smo@dial4jobz.com",
                                         Constants.EmailSubject.VerifyUpdateDetails,
                                         Constants.EmailBody.EmployerMobileUpdateAlert
                                             .Replace("[NAME]", organization.Name)
                                             .Replace("[ID]", organization.Id.ToString())
                                             .Replace("[EMAIL]", organization.Email)
                                             .Replace("[REGISTERED_BY]", (registeredByAdmin != null ? registeredByAdmin.UserName : organization.Name))
                                             .Replace("[MOBILE_NUMBER]", organization.MobileNumber)
                                             .Replace("[NEW_MOBILE]", collection["MobileNumber"])
                                             .Replace("[VERIFY_CODE]", secondVerify)
                                             .Replace("[CHANGED_BY]", user.UserName)
                                         );
                            }
                       
                    }
                
            }
            /*End Update alert details*/
            #endregion End UpdateDetails

            var industryId = collection["Industries"];
            if (industryId != "")
            {
                if (!string.IsNullOrEmpty(collection["Industries"]))
                    organization.IndustryId = Convert.ToInt32(collection["Industries"]);
            }
            else
            {

                if (!string.IsNullOrEmpty(collection["ConsultantIndustries"]))
                    organization.IndustryId = Convert.ToInt32(collection["ConsultantIndustries"]);
            }

            if (consultIndustryId == "2412")
            {
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "You are not an Employer.Please Register as a Consultant",
                    ReturnUrl = "/ConsultantReport/AddConsultant"
                });
            }
            
            organization.Name = collection["Name"];
            organization.ContactPerson = collection["ContactPerson"];
            organization.Email = collection["Email"];
            organization.Website = collection["Website"];
            organization.ContactNumber = collection["ContactNumber"];
            organization.MobileNumber = collection["MobileNumber"];
            organization.Address = collection["Address"];
            organization.Pincode = collection["Pincode"];
            organization.EmployerType = Convert.ToInt32(collection["EmployerType"]);
            
            if(!string.IsNullOrEmpty(collection["OwnershipType"]))
                organization.OwnershipType = Convert.ToInt32(collection["OwnershipType"]);
            
            if(!string.IsNullOrEmpty(collection["NumberOfEmployees"]))
                organization.EmployersCount = Convert.ToInt32(collection["NumberOfEmployees"]);


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

            var mobileNumber = collection["MobileNumber"];
            if (mobileNumber == "")
            {
                if (Page_Code.Contains("Add Without Mobile"))
                {
                    _repository.Save();
                }
                else
                {
                    return Json(new JsonActionResult { Success = false, Message = "Mobile Number is required" });
                }
            }
            else
            {
                _repository.Save();
            }

            if (updateOperation == false)
            {
                string[] userIdentityName = this.User.Identity.Name.Split('|');
                if (user != null)
                {
                    AdminUserEntry adminuserentry = new AdminUserEntry();
                    adminuserentry.AdminId = user.Id;
                    adminuserentry.EntryId = organization.Id;
                    adminuserentry.EntryType = Convert.ToInt32(EntryType.Employer);
                    adminuserentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    _repository.AddAdminUserEntry(adminuserentry);
                    _repository.Save();
                }

                else if (userIdentityName != null && userIdentityName.Length > 1)
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
                }


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
                              .Replace("[LINK_NAME]", "Verify your E-mail ID")
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
                ReturnUrl="/Admin/AdminHome/GetCompanyDetail?validateCompany=" + organization.Name });

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

        #region Jobs

        public ActionResult AdminPostedJobs(int organizationId)
        {
            ViewData["OrgnId"] = organizationId;
            var organization = _userRepository.GetOrganizationById(organizationId);

            var vacancy = _vasRepository.GetVacancies(organizationId);
            var status = _vasRepository.GetRatSubscribed(organizationId);
            var orderId = _vasRepository.GetPlanActivatedResultRAT(organizationId);
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(organizationId, orderId);
            
            if (status == true && vacancy != postedJobs.Count())
            {
                PostedJobAlert postedJobalert = new PostedJobAlert();

                foreach (Dial4Jobz.Models.Job job in organization.Jobs)
                {
                    postedJobalert = _vasRepository.GetPostedJobAlert(organizationId, job.Id);

                    if (postedJobalert != null && postedJobalert.Vacancies != 0)
                    {
                        //check condition of posted jobs then update the minimum value into vacancies
                        if (postedJobs.Count() <= vacancy)
                        {
                            var postedJobsCount = _vasRepository.GetRATAlertJobs(organizationId, Convert.ToInt32(postedJobalert.OrderId));
                            if (postedJobsCount != null)
                            {
                                var vacancyCount = postedJobalert.Vacancies - postedJobsCount.Count();
                                postedJobalert.Vacancies = vacancyCount;
                                _vasRepository.Save();
                            }
                        }
                    }

                }

                if (postedJobs.Count() != null)
                {
                    ViewData["Message"] = "Employer have activated  " + postedJobs.Count() + " Vacancies for RAT till now.";
                }

                else if (postedJobalert == null)
                {
                    ViewData["Message"] = "Employer have Subscribed RAT. Now you can activate  " + vacancy.Value + " Vacancies";
                }

                else if (postedJobalert.Vacancies == 0)
                {
                    ViewData["Message"] = "Employer RAT plan is finished now. Please buy RAT again.";
                }

                else
                {
                    ViewData["Message"] = "Employer have Subscribed RAT. Now you can activate  " + vacancy.Value + " Vacancies";
                }


            }
            else
            {
                ViewData["Message"] = "Employer Plan is over.";
            }

            var getOrganization = _userRepository.GetOrganizationById(organizationId);
            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", getOrganization.IndustryId);

            return View(organization);
        }

      


        public ActionResult EmployerSubscription_Billing(int organizationId)
        {

            Organization organization = _repository.GetOrganization(organizationId);
            LoggedInOrganization = _userRepository.GetOrganizationByUserName(organization.UserName);
            if (LoggedInOrganization != null)
                ViewData["LoggedInOrganization"] = organizationId;
            else
                ViewData["LoggedInOrganization"] = 0;

            return View();
        }
        
        public ActionResult AdminViewedCandidatesList(int organizationId, int consultantId)
        {
            Organization organization = _repository.GetOrganization(organizationId);
            Consultante consultant = _repository.GetConsulant(consultantId);
            if (organization != null)
            {
                ViewData["LoggedInOrganization"] = organization.Id;
            }
            else if (consultant != null)
            {
                ViewData["LoggedInConsultant"] = consultant.Id;
            }
            return View();
        }

        public JsonResult ListContactViewedLogs(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate, int organizationId, int consultantId)
        {

            IQueryable<ContactsViewedLog> contactsviewedlog = null;

            if (organizationId != 0)
            {
                contactsviewedlog = _vasRepository.GetViewedLogs().Where(od => od.EmployerId == organizationId);
            }
            else
            {
                contactsviewedlog = _vasRepository.GetViewedLogs().Where(od => od.ConsultantId == consultantId);
            }
            

            Func<IQueryable<ContactsViewedLog>, IOrderedQueryable<ContactsViewedLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.EmployerId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.OrderId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderBy(rslt => rslt.EmployerId);
                    else if (iSortCol_0 == 1)
                        return query.OrderBy(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderBy(rslt => rslt.OrderId);
                    else
                        return query.OrderBy(rslt => rslt.OrderId);

                }

            };

            contactsviewedlog = orderingFunc(contactsviewedlog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                contactsviewedlog = contactsviewedlog.Where(o => o.EmployerId.ToString().Contains(sSearch.Trim()) || o.DateViewed.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                contactsviewedlog = contactsviewedlog.Where(o => o.DateViewed != null && o.DateViewed >= from && o.DateViewed <= to);

            }

            IEnumerable<ContactsViewedLog> contactsviewedlog1 = contactsviewedlog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = contactsviewedlog.Count(),
                iTotalDisplayRecords = contactsviewedlog.Count(),
                aaData = contactsviewedlog1.Select(o => new object[] { (organizationId != 0 ? o.EmployerId : o.ConsultantId), (o.EmployerId != 0 ? (_repository.GetOrganizationNameById(Convert.ToInt32(o.EmployerId))) : (_repository.GetConsultantNameById(Convert.ToInt32(o.ConsultantId)))), o.OrderId, (o.DateViewed != null) ? o.DateViewed.Value.ToString("dd-MM-yyyy") : "", (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))) })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //*********************************//
        //RAT - Alert Sent Details - Admin
        //*********************************//
        public ActionResult AdminAlertSentDetails(int organizationId, int consultantId)
        {
            Organization organization = _repository.GetOrganization(organizationId);
            Consultante consultant = _repository.GetConsulant(consultantId);
            if (organization != null)
            {
                ViewData["LoggedInOrganization"] = organization.Id;
            }
            else if (consultant != null)
            {
                ViewData["LoggedInConsultant"] = consultant.Id;
            }
            return View();
        }

        public JsonResult ListAlertSentDetails(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate, int organizationId, int consultantId)
        {
            IQueryable<AlertsLog> alertsLog = null;

            if (organizationId != 0)
            {
                alertsLog = _vasRepository.GetALertsLogs().Where(od => od.OrganizationId == organizationId && od.OrderMaster.OrderId == od.OrderId);
            }
            else
            {
                alertsLog = _vasRepository.GetALertsLogs().Where(od => od.ConsultantId == consultantId && od.OrderMaster.OrderId == od.OrderId);
            }


            Func<IQueryable<AlertsLog>, IOrderedQueryable<AlertsLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.AlertSentDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.MailSent);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.SmsSent);
                    else
                        return query.OrderByDescending(rslt => rslt.JobId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.OrganizationId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.AlertSentDate);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 4)
                        return query.OrderByDescending(rslt => rslt.MailSent);
                    else if (iSortCol_0 == 5)
                        return query.OrderByDescending(rslt => rslt.SmsSent);
                    else
                        return query.OrderByDescending(rslt => rslt.JobId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.OrderMaster.Organization.Name.ToString().Contains(sSearch.Trim()) || o.AlertSentDate.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.AlertSentDate != null && o.AlertSentDate >= from && o.AlertSentDate <= to);

            }

            IEnumerable<AlertsLog> contactsviewedlog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = alertsLog.Count(),
                iTotalDisplayRecords = alertsLog.Count(),
                aaData = contactsviewedlog1.Select(o => new object[] { o.OrderId, (organizationId != 0 ? (_repository.GetOrganizationNameById(Convert.ToInt32(o.OrganizationId))) : (_repository.GetConsultantNameById(Convert.ToInt32(o.ConsultantId)))), (_repository.GetJobById(Convert.ToInt32(o.JobId))), (o.AlertSentDate != null) ? o.AlertSentDate.Value.ToString("dd-MM-yyyy") : "", (_repository.GetCandidateContactNumberById(Convert.ToInt32(o.CandidateId))), (_repository.GetCandidateNameById(Convert.ToInt32(o.CandidateId))), (o.SmsSent == true ? "Yes" : "No"), (o.MailSent == true ? "Yes" : "No") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //******************************************
        //RAJ- Alert Sent Details
        //******************************************

        public ActionResult AdminRAJAlertSentDetails(int candidateId)
        {
            Candidate candidate = _repository.GetCandidate(candidateId);
            if (candidate != null)
            {
                Session["LoggedInCandidate"] = candidate.Id;
            }
            return View();
            
        }

        public JsonResult ListAlertSentDetailsForRAJ(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate, int candidateId)
        {
            IQueryable<JobsViewedLog> alertsLog = _vasRepository.GetCandidateAlertsLog().Where(od => od.CandidateId == candidateId && od.OrderMaster.OrderId == od.OrderId);

            Func<IQueryable<JobsViewedLog>, IOrderedQueryable<JobsViewedLog>> orderingFunc = query =>
            {
                if ("desc" == sSortDir_0)
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.JobId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);
                }
                else
                {
                    if (iSortCol_0 == 0)
                        return query.OrderByDescending(rslt => rslt.CandidateId);
                    else if (iSortCol_0 == 1)
                        return query.OrderByDescending(rslt => rslt.DateViewed);
                    else if (iSortCol_0 == 2)
                        return query.OrderByDescending(rslt => rslt.JobId);
                    else
                        return query.OrderByDescending(rslt => rslt.OrderId);

                }

            };

            alertsLog = orderingFunc(alertsLog);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                alertsLog = alertsLog.Where(o => o.CandidateId.ToString().Contains(sSearch.Trim()) || o.DateViewed.ToString().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                alertsLog = alertsLog.Where(o => o.DateViewed != null && o.DateViewed >= from && o.DateViewed <= to);

            }

            IEnumerable<JobsViewedLog> alertsLog1 = alertsLog.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            var result = new
            {
                iTotalRecords = alertsLog.Count(),
                iTotalDisplayRecords = alertsLog.Count(),

                aaData = alertsLog1.Select(o => new object[] { o.OrderId, (_repository.getEmployerNameByJobId(Convert.ToInt32(o.JobId))), (o.DateViewed != null) ? o.DateViewed.Value.ToString("dd-MM-yyyy") : "", (_repository.GetJobById(Convert.ToInt32(o.JobId))), (_repository.getEmployerMobileNumber(Convert.ToInt32(o.JobId))), (_repository.getEmployerEmail(Convert.ToInt32(o.JobId))), (o.SmsSent ==true ? "Yes" : "No"), (o.MailSent ==true ? "Yes" : "No") })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //**********Candidate Dashboard**************

        public ActionResult CandidateDashBoard(int candidateId)
        {
            ViewData["candidateId"] = candidateId;
            var candidate = _userRepository.GetCandidateById(candidateId);
            return View(candidate);
        }

        public ActionResult SubscriptionBillingForCandidate(int candidateId)
        {
            Candidate candidate = _repository.GetCandidate(candidateId);

            LoggedInCandidate = User.Identity.IsAuthenticated ? new UserRepository().GetCandidateByUserName(candidate.UserName) : null;

            if (LoggedInCandidate != null)
                ViewData["LoggedInCandidate"] = LoggedInCandidate.Id;
            else
                ViewData["LoggedInCandidate"] = 0;

            return View();
        }

       
        //**********Employer Dashboard**************

        public ActionResult AdminDashBoard(int organizationId)
        {

            ViewData["organizationId"] = organizationId;
            var organization = _userRepository.GetOrganizationById(organizationId);

            var status = _vasRepository.GetRatSubscribed(organizationId);
            var vacancies = _vasRepository.GetVacancies(organizationId);
            var activatedList = _vasRepository.GetPlanActivatedResultRAT(organizationId);
            var orderId = _vasRepository.GetPlanActivatedResultRAT(organizationId);
            var JobsByOrganization = _vasRepository.GetJobsByOrganizationIdAlert(organizationId, orderId);

            if (status == true)
            {
                ViewData["Message"] = "Employer have activated for RAT for" + vacancies + " Vacancies.";
            }

            else if (status == false)
            {
                ViewData["Message"] = "Employer RAT Plan is over. Get Vacancy with Resume Alert Subscribe for more RAT Plan's";
            }
            else if (activatedList != null)
            {
                ViewData["Message"] = "Employer request for the Resume Alert to " + JobsByOrganization.Count() + "  Vacancies is live now. Subscribe Resume Alert for more Alerts";
            }
            else
            {
                ViewData["Message"] = "If Employer want Suitable Resumes for the Vacancy, Subscribe for Resume Alert.";
            }

            var getOrganization = _userRepository.GetOrganizationById(organizationId);
            return View();
        }


        public ActionResult SpecialPlans()
        {
            return View();
        }

        public ActionResult AdminPlans()
        {
            return View();
        }

        /*Spot Interview by candidate(for update the details)*/

        public ActionResult UpdateSIDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSIDetails(FormCollection collection)
        {
            int candidateId = 0;
            currentdate = Constants.CurrentTime().Date;

            if (Session["LoginAs"] == "CandidateViaAdmin")
            {
                candidateId = (int)Session["CandId"];
                Candidate candidate = _repository.GetCandidate(candidateId);
            }

            OrderDetail orderDetail = _vasRepository.GetSIOrderByCandidate(candidateId);

            OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderDetail.OrderId));
            ordermaster.SubscribedBy = User.Identity.Name;
            ordermaster.OrderDate = currentdate;
            _vasRepository.Save();

            var registeredBy = string.Empty;
            User subscribedByAdmin = null;
            User registeredByAdmin = null;
          
            if (orderDetail != null)
            {
                orderDetail.Amount = Convert.ToInt32(collection["Amount"]);
                orderDetail.TeleConference = Convert.ToInt32(collection["TeleConferenceCount"]);
                var ValidityDaysCount = collection["ValidityPeriod"];
                int vasplanDays = Convert.ToInt32(ValidityDaysCount);
                orderDetail.ValidityDays = vasplanDays;
             

                ViewData["OrderNo"] = orderDetail.OrderId;
                _vasRepository.Save();

                if (ordermaster.Candidate != null)
                {
                    registeredBy = _repository.GetAdminUserNamebyEntryIdAndEntryType(ordermaster.Candidate.Id, EntryType.Candidate);
                    subscribedByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(ordermaster.SubscribedBy).FirstOrDefault();
                    registeredByAdmin = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(registeredBy).FirstOrDefault();
                }

                //OrderMaster ordermaster = _vasRepository.GetOrderMaster(Convert.ToInt32(orderDetail.OrderId));
                if (orderDetail.PlanName == "SI")
                {
                    StreamReader reader = new StreamReader(Server.MapPath("~/Views/MailTemplate/SpotInterviewSubscribed.htm"));
                    string table = reader.ReadToEnd();
                    reader.Dispose();
                    table = table.Replace("[NAME]", orderDetail.OrderMaster.Candidate.Name);
                    table = table.Replace("[ID]", orderDetail.OrderMaster.Candidate.Id.ToString());
                    table = table.Replace("[MOBILE]", orderDetail.OrderMaster.Candidate.ContactNumber);
                    table = table.Replace("[EMAILID]", orderDetail.OrderMaster.Candidate.Email);

                    table = table.Replace("[ORDERNO]", orderDetail.OrderId.ToString());
                    table = table.Replace("[PLAN]", orderDetail.PlanName.ToString());
                    table = table.Replace("[VALIDITY]", vasplanDays.ToString());
                    if (orderDetail.TeleConference != null)
                    {
                        table = table.Replace("[INTERVIEW_COUNT1]", "Maximum of " + orderDetail.TeleConference.ToString());
                        table = table.Replace("[INTERVIEW_COUNT]", "Interview Count: " + orderDetail.TeleConference.ToString());
                    }
                    else
                    {
                        table = table.Replace("[INTERVIEW_COUNT1]", "");
                        table = table.Replace("[INTERVIEW_COUNT]","");
                    }

                    table = table.Replace("[AMOUNT]", orderDetail.Amount.ToString());
                    table = table.Replace("[SUBSCRIBED_BY]", ordermaster.SubscribedBy);
                    table = table.Replace("[DATE]", Constants.CurrentTime().ToString("dd-MM-yyyy"));
                    table = table.Replace("[LINK_NAME]", "CLICK THE LINK");
                    table = table.Replace("[PAYMENT_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Candidates/CandidatesVas/Payment?orderId=" + Constants.EncryptString(orderDetail.OrderId.ToString()).ToString());

                    if (ordermaster.Candidate.Email != null)
                    {
                        EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                                Constants.EmailSender.CandidateSupport,
                                Constants.EmailSender.VasEmailId,
                                orderDetail.PlanName + " - subscribed",
                                table);
                    }

                    EmailHelper.SendEmailBCC(Constants.EmailSender.CandidateSupport,
                                Constants.EmailSender.CandidateSupport,
                                Constants.EmailSender.VasEmailId,
                                orderDetail.PlanName + " - subscribed",
                                table);

                    if (subscribedByAdmin != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                           subscribedByAdmin.Email,
                            orderDetail.PlanName + " - subscribed",
                            table);
                    }

                    if (registeredByAdmin != null)
                    {
                        EmailHelper.SendEmail(Constants.EmailSender.CandidateSupport,
                           registeredByAdmin.Email,
                            orderDetail.PlanName + " - subscribed",
                            table);
                    }


                }

                if (orderDetail.OrderMaster.Candidate.ContactNumber != null)
                {
                    SmsHelper.SendSecondarySms(
                        Constants.SmsSender.SecondaryUserName,
                        Constants.SmsSender.SecondaryPassword,
                        Constants.SmsBody.SubscribePlan
                        .Replace("[NAME]",orderDetail.OrderMaster.Candidate.Name)
                        .Replace("[DESCRIPTION]","Spot Interview")
                        .Replace("[PLAN]", orderDetail.PlanName)
                        .Replace("[AMOUNT]", orderDetail.Amount.ToString())
                        ,
                        Constants.SmsSender.SecondaryType,
                        Constants.SmsSender.Secondarysource,
                        Constants.SmsSender.Secondarydlr,
                        orderDetail.OrderMaster.Candidate.ContactNumber
                        );
                        

                }
            }
           
            return RedirectToAction("CandidateDashBoard", "AdminHome", new { candidateId = candidateId });
        }

       public ActionResult UpdateRATVacancy(int jobId, int organizationId)
       {
            
            double vasplanDays = 0;
            OrderDetail orderdetail = null;
            PostedJobAlert getjobalert = _vasRepository.GetPostedJobAlert(organizationId, jobId);
            Job job = _repository.GetJob(jobId);
            if (getjobalert != null)
            {
                orderdetail = _vasRepository.GetOrderDetail(Convert.ToInt32(getjobalert.OrderId));
            }
            if (getjobalert.ActivatedJobStatus == true)
            {
                getjobalert.AlertActivateDate = currentdate;
                vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                getjobalert.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
                if (getjobalert.Vacancies != 0)
                {
                    getjobalert.Vacancies = getjobalert.Vacancies - 1;
                }
                getjobalert.RemainingCount = 25;
                _vasRepository.Save();

                job.UpdatedDate = Constants.CurrentTime();
                _repository.Save();

                EmailHelper.SendEmail(
                    Constants.EmailSender.EmployerSupport,
                    job.EmailAddress,
                    "Job | Re-Activate Your Vacancy",
                    Constants.EmailBody.UpdateVacancy
                    .Replace("[NAME]", job.Organization.Name)
                    .Replace("[POSITION]", job.Position)
                    .Replace("[PLAN]", orderdetail.PlanName)
                    .Replace("[ORDER_NO]", orderdetail.OrderId.ToString())
                    .Replace("[PLAN_NAME]", orderdetail.PlanName.ToString())
                    .Replace("[VALIDITY_COUNT]", orderdetail.ValidityCount.ToString())
                    .Replace("[START_DATE]", getjobalert.AlertActivateDate.Value.ToString("dd-MM-yyyy"))
                    .Replace("[END_DATE]", getjobalert.ValidityTill.Value.ToString("dd-MM-yyyy"))
                    .Replace("[VAC_ALERT]", "25")
                    .Replace("[LINK_NAME]", "Your DashBoard")
                    .Replace("[DASHBOARD_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/MatchCandidates")
                    );
            }

            return RedirectToAction("AdminPostedJobs", "AdminHome", new { organizationId = organizationId });
            
        }
        
        
        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult ActivateRATVacancy(FormCollection collection, int orgnId)
        {
            ViewData["OrgnId"] = orgnId;
            var organization = _userRepository.GetOrganizationById(orgnId);

            var vacancies = _vasRepository.GetVacancies(orgnId);
            var selectedKeyCount = 0;
            var orderId = _vasRepository.GetPlanActivatedResultRAT(orgnId);
            var postedJobs = _vasRepository.GetJobsByOrganizationIdAlert(orgnId,orderId);
            bool isSuccess = false;
            currentdate = Constants.CurrentTime().Date;
            OrderDetail orderdetail = null;
            PostedJobAlert postedJobalert = null;
            double vasplanDays = 0;

            foreach (string key in collection.AllKeys)
            {
                if (key.Contains("Job"))
                {
                    int jobId = Convert.ToInt32(key.Replace("Job", string.Empty));
                    var activatedvacancy = _vasRepository.GetPostedJobAlert(orgnId, jobId);

                    var value = collection.AllKeys.Count();

                    var selectedKeyword = Convert.ToInt32(collection.GetValues(key).Contains("true"));
                    selectedKeyCount = selectedKeyCount + selectedKeyword;

                    if (selectedKeyCount <= vacancies)
                    {
                        if (postedJobs.Count() < vacancies)
                        {
                            if (Convert.ToBoolean(collection.GetValues(key).Contains("true")))
                            {
                                Job job = _repository.GetJob(jobId);
                                PostedJobAlert jobs = _vasRepository.GetJobIdByPostedJobAlert(jobId);
                                if (jobs != null)
                                {
                                    if (jobs.JobId != jobId)
                                    {
                                        Response.Write("<script language=javascript>alert('You have already activated this vacancy');</script>");
                                        
                                    }
                                }
                                else
                                {
                                    OrderDetail validityPlan = _vasRepository.GetValidityRAT(organization.Id);
                                    if (validityPlan != null)
                                    {
                                        _vasRepository.UpdateVASDetails(organization.Id, jobId);

                                        postedJobalert = _vasRepository.GetPostedJobAlert(organization.Id, jobId);
                                        orderdetail = _vasRepository.GetOrderDetailsRATUpdate(Convert.ToInt32(postedJobalert.OrderId));
                                        postedJobalert.Vacancies = vacancies;
                                        postedJobalert.AlertActivateDate = Constants.CurrentTime();
                                        vasplanDays = Convert.ToDouble(orderdetail.VasPlan.ValidityDays);
                                        postedJobalert.ValidityTill = currentdate.AddDays(vasplanDays != null ? vasplanDays - 1 : 0);
                                        postedJobalert.RemainingCount = 25;
                                        _vasRepository.Save();
                                    }
                                    else
                                        
                                        Response.Write("<script language=javascript>alert('Your Plan has been finished. You cannot assign Vacancy');</script>");

                                    
                                    EmailHelper.SendEmail(
                                        Constants.EmailSender.EmployerSupport,
                                        organization.Email,
                                        Constants.EmailSubject.ActivateVacancy,
                                        Constants.EmailBody.ActivateVacancy
                                        .Replace("[NAME]", organization.Name)
                                        .Replace("[POSITION]", job.Position)
                                        .Replace("[PLAN]", orderdetail.PlanName)
                                        .Replace("[ORDER_NO]", orderdetail.OrderId.ToString())
                                        .Replace("[PLAN_NAME]", orderdetail.VasPlan.PlanName)
                                        .Replace("[VALIDITY_COUNT]", (orderdetail.BasicCount != null ? orderdetail.BasicCount.ToString() : orderdetail.ValidityCount.ToString()))
                                        //.Replace("[START_DATE]", orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy"))
                                        .Replace("[START_DATE]", postedJobalert.AlertActivateDate.Value.ToString("dd-MM-yyyy"))
                                        .Replace("[END_DATE]", postedJobalert.ValidityTill.Value.ToString("dd-MM-yyyy"))
                                        .Replace("[VAC_ALERT]","25")
                                        .Replace("[NOTICE]", "Employers")
                                        .Replace("[LINK_NAME]", "Your DashBoard")
                                        .Replace("[DASHBOARD_LINK]", ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Employer/MatchCandidates")
                                        );

                                    SmsHelper.SendSecondarySms(
                                        Constants.SmsSender.SecondaryUserName,
                                        Constants.SmsSender.SecondaryPassword,
                                        Constants.SmsBody.ActivateVacancy
                                        .Replace("[NAME]", organization.Name)
                                        .Replace("[POSITION]", job.Position)
                                        .Replace("[PLAN]", orderdetail.PlanName)
                                        .Replace("[VALIDITY_COUNT]", (orderdetail.BasicCount != null ? orderdetail.BasicCount.ToString() : orderdetail.ValidityCount.ToString()))
                                        .Replace("[VALIDITY_TILL]", vasplanDays.ToString()),
                                        Constants.SmsSender.SecondaryType,
                                        Constants.SmsSender.Secondarysource,
                                        Constants.SmsSender.Secondarydlr,
                                        organization.MobileNumber
                                        );
                                        isSuccess = true;
                                  
                                }

                            }
                        }
                    }

                }                          


            }

            if (selectedKeyCount >= vacancies)
            {
              
                Response.Write("<script language=javascript>alert('As per you have to select limited Vacancies..');</script>");
            }
            else if (postedJobs.Count() >= vacancies)
            {
             
                Response.Write("<script language=javascript>alert('You have already activated all vacancies. Please buy RAT again..');</script>");
            }

            else if (isSuccess == true)
            {
               
                Response.Write("<script language=javascript>alert('Vacancy activated Successfully. Now You will receive alerts..');</script>");
            }

            else
            {
                ViewData["Message"] = "Activation is not successful. Please try again";

            }


            //}
            return RedirectToAction("AdminPostedJobs", "AdminHome", new { organizationId = orgnId });

        }

        public ActionResult AddJob(int organizationId)
        {
            ViewData["OrgnId"] = organizationId;
        
            SetCommonViewData();
            SetAddJobViewData();
            return View();
        }


        public ActionResult EditJob(int id)
        {
            var job = _repository.GetJob(id);

            if (job == null)
                return new FileNotFoundResult();

            SetCommonViewData();
            SetEditJobViewData(job);

            return View(job);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            Job job = _repository.GetJob(id);
            if (job != null)
                return View();
            else
                return RedirectToAction("AdminPostedJobs", "AdminHome");
        }


        [HttpPost]
        public ActionResult Delete(int id, string confirm)
        {
            Job job = _repository.GetJob(id);
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            IEnumerable<AdminUserEntry> getEntries = _userRepository.GetUsersById(user.Id, job.Id, 3);
            if (getEntries.Count() > 0)
            {
                _repository.DeleteAdminUserEntries(user.Id, job.Id, 3);
            }
            if (job != null)
            {
                _repository.DeleteJobLanguages(id);
                _repository.DeleteJobLocations(id);
                _repository.DeleteJobPreferredIndustries(id);
                _repository.DeleteJobRequiredQualifications(id);
                _repository.DeleteJobRoles(id);
                _repository.DeleteJobskills(id);
                _repository.DeleteJobLicenseTypes(id);
                _repository.DeleteJob(id);
            }

            return RedirectToAction("Index", "AdminHome");

        }

        public ActionResult DeleteCompanyDetail(string userName)
        {

            var result = userName.Split('=');
            var orgName = result[0];
            var orgId = Convert.ToInt32(result[1]);

            Job job = _repository.GetJob(orgId);
            if (job.Id != 0)
            {

                _repository.DeleteJobLanguages(job.Id);
                _repository.DeleteJobLocations(job.Id);
                _repository.DeleteJobPreferredIndustries(job.Id);
                _repository.DeleteJobRequiredQualifications(job.Id);
                _repository.DeleteJobRoles(job.Id);
                _repository.DeleteJobskills(job.Id);
                _repository.DeleteJob(job.Id);
            }
            if (!string.IsNullOrEmpty(orgName))
                _repository.DeleteOrgByUserName(orgName);

            return RedirectToAction("AddEmployer");
        }

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult AddJob(FormCollection collection)
        {
            
            Job job = new Job();
            UserRepository _userRespository = new UserRepository();
            int orgnId = Convert.ToInt32(collection["orgnId"]);

            job.CreatedDate = Constants.CurrentTime();
            Organization currentOrganization = _userRespository.GetOrganizationById(orgnId);
            job.OrganizationId = currentOrganization.Id;
             
            if (SetJobDetails(job, collection))
            {
                if (job.ContactPerson == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Contact Person is required" });
                }

                if (job.Position == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Position is required" });
                }

                if (job.MobileNumber == "" && job.EmailAddress == "")
                {
                    return Json(new JsonActionResult { Success = false, Message = "Mobile/Email is required. Enter any one." });
                }


                else
                {
                    _repository.Save();
                    if (job.CommunicateViaEmail == true && job.EmailAddress != null)
                    {
                        EmailHelper.SendEmail(
                        Constants.EmailSender.EmployerSupport,
                        job.EmailAddress,
                        Constants.EmailSubject.ClientPost,
                        Constants.EmailBody.ClientPost
                                  .Replace("[NAME]", job.ContactPerson)
                                  .Replace("[IMAGE_URL]", Url.Content("~/Content/Images/dial4jobz_logo.png"))
                                  );
                    }
                    if (job.CommunicateViaSMS == true && job.MobileNumber != null)
                    {
                        var gender = Convert.ToString(job.Male);
                        if (job.Male == true)
                            gender = "Male";
                        else
                            gender = "Female";

                        string preferredall=string.Empty;
                        string preferredcontract=string.Empty;
                        string preferredparttime=string.Empty;
                        string preferredfulltime=string.Empty;
                        string preferredworkfromhome=string.Empty;

                        if(job.PreferredAll==true)
                        {
                            preferredall="All Type";
                        }

                        else if(job.PreferredContract==true)
                        {
                            preferredcontract= "Contract";
                        }

                        else if(job.PreferredParttime==true)
                        {
                            preferredparttime= "Part Time";
                        }

                        else if(job.PreferredFulltime==true)
                        {
                            preferredfulltime="Full Time";
                        }

                        else 
                        {
                            preferredworkfromhome= "Work From Home";
                        }


                        if (job.MobileNumber != null || job.MobileNumber != "")
                        {
                            /*Job Details send to Employer to verify*/
                            string industry=string.Empty;
                          
                            foreach (JobPreferredIndustry jpi in job.JobPreferredIndustries)
                            {
                                if (industry == string.Empty)
                                    industry = jpi.IndustryId.ToString();
                                else
                                    industry += "," + jpi.IndustryId;
                            }

                            string jobminexp = string.Empty;
                            string jobmaxexp = string.Empty;
                            if ((!job.MinExperience.HasValue || job.MinExperience == 0) && (!job.MaxExperience.HasValue || job.MaxExperience == 0))
                            {

                            }
                            else if (!job.MinExperience.HasValue || job.MinExperience == 0)
                            {
                                jobmaxexp = "Up to" + Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years ";
                            }
                            else if (!job.MaxExperience.HasValue || job.MaxExperience == 0)
                            {
                                jobminexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + "+ Years";
                            }
                            else
                            {
                                jobminexp = Math.Ceiling(job.MinExperience.Value / 33782400.0) + " " +
                                Math.Ceiling(job.MaxExperience.Value / 33782400.0) + " Years";
                            }

                            string minSalary = string.Empty;
                            string maxSalary = string.Empty;
                            if (job.Budget != null)
                            {
                                minSalary = job.Budget.ToString();
                            }
                            if (job.MaxBudget != null)
                            {
                                maxSalary = job.MaxBudget.ToString();
                            }

                            string role = string.Empty;

                            if (job.JobRoles != null)
                            {
                                foreach (JobRole jr in job.JobRoles)
                                {
                                    if (role == string.Empty)
                                    {
                                        role = jr.Role.Name;
                                    }
                                    else
                                    {

                                    }
                                }
                            }


                            string joblanguages = string.Empty;

                            if (job.JobLanguages != null)
                            {
                                foreach (JobLanguage jl in job.JobLanguages)
                                {
                                    if (joblanguages == string.Empty)
                                    {
                                        joblanguages = jl.Language.Name;
                                    }
                                    else
                                    {

                                    }
                                }
                            }

                            string jobCity = string.Empty;
                            string jobRegion = string.Empty;
                            string jobState = string.Empty;
                            string jobCountry = string.Empty;

                            string preferredlocations = string.Empty;
                            if (job.JobLocations != null)
                            {
                                foreach (JobLocation jl in job.JobLocations)
                                {
                                    if (jobCity == string.Empty)
                                    {
                                        if (jl.Location.City != null)
                                        {
                                            jobCity = jl.Location.City.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if(jobState == string.Empty)
                                    {
                                        if (jl.Location.City != null)
                                        {
                                            jobState = jl.Location.City.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (jobRegion == string.Empty)
                                    {
                                        if (jl.Location.Region != null)
                                        {
                                            jobRegion = jl.Location.Region.Name;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    if (jobCountry == string.Empty)
                                    {
                                        if (jl.Location.Country != null)
                                        {
                                            jobCountry = jl.Location.Country.Name;
                                        }
                                        else
                                        {
                                        }
                                    }

                                }
                            }
                            string jobbasicqualification = string.Empty;
                            string jobpostgraduation = string.Empty;
                            string jobdoctrate = string.Empty;
                            foreach (Dial4Jobz.Models.JobRequiredQualification cq in job.JobRequiredQualifications)
                            {
                                if (cq.Degree.Type == 0)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobbasicqualification += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobbasicqualification += cq.Degree.Name + ",";
                                    }
                                }
                                if (cq.Degree.Type == 1)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobpostgraduation += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobpostgraduation += cq.Degree.Name + ",";
                                    }
                                }
                                if (cq.Degree.Type == 2)
                                {
                                    if (cq.Specialization != null)
                                    {
                                        jobdoctrate += cq.Degree.Name + "(" + cq.Specialization + ")" + ",";
                                    }
                                    else
                                    {
                                        jobdoctrate += cq.Degree.Name + ",";
                                    }
                                }
                            }

                            SmsHelper.SendSecondarySms(
                            Constants.SmsSender.SecondaryUserName,
                            Constants.SmsSender.SecondaryPassword,
                            Constants.SmsBody.JobPostingDetails
                            .Replace("[POSITION]", job.Position)
                            .Replace("[BASIC_QUALIFICATION]", (jobbasicqualification!="" ? jobbasicqualification: "" + jobpostgraduation!= "" ? jobpostgraduation : "" + jobdoctrate!="" ? jobdoctrate : ""))
                            .Replace("[FUNCTIONAL_AREA]", job.FunctionId.HasValue && job.FunctionId != 0 ? job.GetFunction(job.FunctionId.Value).Name : "Not Mentioned")
                            .Replace("[ROLE]", (role!=""? role : "Not Mentioned"))
                            .Replace("[PREFERRED_INDUSTRY]", (industry != "" ? industry : "NA"))
                            .Replace("[MINEXP]", jobminexp )
                            .Replace("[MAXEXP]",  " to" + jobmaxexp)
                            .Replace("[ANNUAL_SALARY]", minSalary + "to" + maxSalary)
                            .Replace("[COUNTRY]",(jobCountry!="" ? jobCountry: "")+ ",")
                            .Replace("[STATE]", (jobState != "" ? jobState : "") + ",")
                            .Replace("[CITY]", (jobCity != "" ? jobCity : "") +",")
                            .Replace("[AREA]", (jobRegion != "" ? jobRegion : ""))
                            .Replace("[PREFERRED_LANGUAGES]", (joblanguages.Count() > 0 ? joblanguages + "," : ""))
                            .Replace("[PREFERRED_TYPE]",(preferredall!="" ? preferredall: "" + preferredcontract!="" ? preferredcontract : "" + preferredfulltime !=""? preferredfulltime : "" + preferredparttime!="" ? preferredparttime :"" + preferredworkfromhome!="" ? preferredworkfromhome : ""))
                            .Replace("[GENDER]", gender),
                             Constants.SmsSender.SecondaryType,
                             Constants.SmsSender.Secondarysource,
                             Constants.SmsSender.Secondarydlr,
                             job.MobileNumber
                            );

                        }

                        /*End employer send sms verify*/
                                                
                    }
                }

               
                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Job listing Added",
                    ReturnUrl = "/Admin/CandidateMatches/CandidateMatch/" + job.Id
                });

            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            }
        }

        [Authorize, HttpPost, HandleErrorWithAjaxFilter]
        public ActionResult Save(FormCollection collection, int jobId)
        {
            Job job = _repository.GetJob(jobId);
            DateTime dateTime = DateTime.Now;
            //var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");

            job.UpdatedDate = Constants.CurrentTime();

            if (SetJobDetails(job, collection))
            {
                _repository.Save();
              

                return Json(new JsonActionResult
                {
                    Success = true,
                    Message = "Job listing Updated. You can view the matching candidates..",
                    ReturnUrl = "/Admin/CandidateMatches/CandidateMatch/" + job.Id
                    //ReturnUrl = "CandidateMatchesJob/CandidateMatch/" + job.Id
                });
            }
            else
            {
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });
            }
        }

        private bool SetJobDetails(Job job, FormCollection collection)
        {

            if (!string.IsNullOrEmpty(collection["Functions"]))
                job.FunctionId = Convert.ToInt32(collection["Functions"]);

            job.Position = collection["Position"];

            if (!string.IsNullOrEmpty(collection["Male"]))
                job.Male = Convert.ToBoolean(collection.GetValues("Male").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Female"]))
                job.Female = Convert.ToBoolean(collection.GetValues("Female").Contains("true"));

            long yearsInSecondsFrom = Convert.ToInt64(collection["ddlTotalExperienceYearsFrom"]) * 365 * 24 * 60 * 60;
            long yearsInSecondsTo = Convert.ToInt64(collection["ddlTotalExperienceYearsTo"]) * 365 * 24 * 60 * 60;
            job.MinExperience = yearsInSecondsFrom;
            job.MaxExperience = yearsInSecondsTo;

            long minannualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhsMin"]) * 100000;
            long minannualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousandsMin"]) * 1000;
            job.Budget = minannualSalaryLakhs + minannualSalaryThousands;

            long annualSalaryLakhs = Convert.ToInt32(collection["ddlAnnualSalaryLakhs"]) * 100000;
            long annualSalaryThousands = Convert.ToInt32(collection["ddlAnnualSalaryThousands"]) * 1000;
            job.MaxBudget = annualSalaryLakhs + annualSalaryThousands;

            if (!string.IsNullOrEmpty(collection["Any"]))
                job.PreferredAll = Convert.ToBoolean(collection.GetValues("Any").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Contract"]))
                job.PreferredContract = Convert.ToBoolean(collection.GetValues("Contract").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Parttime"]))
                job.PreferredParttime = Convert.ToBoolean(collection.GetValues("Parttime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["Fulltime"]))
                job.PreferredFulltime = Convert.ToBoolean(collection.GetValues("Fulltime").Contains("true"));

            if (!string.IsNullOrEmpty(collection["WorkFromHome"]))
                job.PreferredWorkFromHome = Convert.ToBoolean(collection.GetValues("WorkFromHome").Contains("true"));

            job.PreferredTimeFrom = collection["ddlPreferredTimeFrom"];
            job.PreferredTimeTo = collection["ddlPreferredTimeto"];

            if (!string.IsNullOrEmpty(collection["GeneralShift"]))
                job.GeneralShift = Convert.ToBoolean(collection.GetValues("GeneralShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["NightShift"]))
                job.NightShift = Convert.ToBoolean(collection.GetValues("NightShift").Contains("true"));

            if (!string.IsNullOrEmpty(collection["TwoWheeler"]))
                job.TwoWheeler = Convert.ToBoolean(collection.GetValues("TwoWheeler").Contains("true"));

            if (!string.IsNullOrEmpty(collection["FourWheeler"]))
                job.FourWheeler = Convert.ToBoolean(collection.GetValues("FourWheeler").Contains("true"));

                     

            job.ContactPerson = collection["RequirementsContactPerson"];
            job.ContactNumber = collection["RequirementsContactNumber"];
            job.MobileNumber = collection["RequirementsMobileNumber"];
            job.EmailAddress = collection["RequirementsEmailAddress"];
            job.InternationalNumber = collection["RequirementsInternationalNumber"];

            job.Description = collection["Description"];

            if (!string.IsNullOrEmpty(collection["CommunicationEmail"]))
                job.CommunicateViaEmail = Convert.ToBoolean(collection.GetValues("CommunicationEmail").Contains("true"));

            if (!string.IsNullOrEmpty(collection["CommunicationSMS"]))
                job.CommunicateViaSMS = Convert.ToBoolean(collection.GetValues("CommunicationSMS").Contains("true"));

            if (!string.IsNullOrEmpty(collection["HideDetails"]))
                job.HideDetails = Convert.ToBoolean(collection.GetValues("HideDetails").Contains("true"));

            if (!TryValidateModel(job))
                return false;

            //add the job and get the job id, so that we can add the foreign tables data.
            int jobId = job.Id > 0 ? job.Id : _repository.AddJob(job);

            // Add skills for job
            string[] skills = collection["Skills"].Split(',');

            if (skills.Count() != 0)
                _repository.DeleteJobskills(jobId);

            foreach (string skill in skills)
            {
                if (!string.IsNullOrEmpty(skill))
                {
                    JobSkill js = new JobSkill();
                    js.JobId = jobId;
                    js.SkillId = Convert.ToInt32(skill);
                    _repository.AddJobSkill(js);
                }
            }

            // Add languages for job
            string[] languages = collection["Languages"].Split(',');

            if (languages.Count() != 0)
                _repository.DeleteJobLanguages(jobId);

            foreach (string language in languages)
            {
                if (!string.IsNullOrEmpty(language))
                {
                    JobLanguage jl = new JobLanguage();
                    jl.JobId = jobId;
                    jl.LanguageId = Convert.ToInt32(language);
                    _repository.AddJobLanguage(jl);
                }
            }

            //licence types
            string[] licenseTypes = { };
            if (!string.IsNullOrEmpty(collection["lbLicenseTypes"]))
                licenseTypes = collection["lbLicenseTypes"].Split(',');

            //delete all license types
            if (licenseTypes.Count() != 0)
                _repository.DeleteJobLicenseTypes(jobId);

            //then add new ones
            foreach (string licenseType in licenseTypes)
            {
                //if (!string.IsNullOrEmpty(licenseType))
                if (!string.IsNullOrEmpty(licenseType) && licenseType != "0")
                {
                    JobLicenseType jlt = new JobLicenseType();
                    jlt.JobId = jobId;
                    jlt.LicenseTypeId = Convert.ToInt32(licenseType);
                    _repository.AddJobLicenseType(jlt);
                }
            }


            string[] preferredIndustries = { };
            if (!string.IsNullOrEmpty(collection["Industries"]))
                preferredIndustries = collection["Industries"].Split(',');

            //delete all preferred industries
            if (preferredIndustries.Count() != 0)
            {
                _repository.DeleteJobPreferredIndustries(jobId);
            }

            //Add new ones

            foreach (string preferredindustry in preferredIndustries)
            {
                if ((!string.IsNullOrEmpty(preferredindustry)) && preferredindustry != "0")
                {
                    JobPreferredIndustry jpi = new JobPreferredIndustry();
                    jpi.JobId = jobId;
                    jpi.IndustryId = Convert.ToInt32(preferredindustry);
                    _repository.AddJobPreferredIndustry(jpi);
                }
            }

            //Roles
            string[] roles = collection["Roles"].Split(',');

            if (roles.Count() != 0)
                _repository.DeleteJobRoles(jobId);

            foreach (string preferredRole in roles)
            {
                //if (!string.IsNullOrEmpty(preferredRole))
                if ((!string.IsNullOrEmpty(preferredRole)) && preferredRole != "0")
                {
                    JobRole jr = new JobRole();
                    jr.JobId = jobId;
                    jr.RoleId = Convert.ToInt32(preferredRole);
                    _repository.AddJobRole(jr);
                }
            }

            //add posting locations for job
            _repository.DeleteJobLocations(jobId);


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
                                        Location location = new Location();

                                        location.CountryId = Convert.ToInt32(countryId);
                                        location.StateId = Convert.ToInt32(stateId);
                                        location.CityId = Convert.ToInt32(cityId);
                                        location.RegionId = Convert.ToInt32(regionId);

                                        int locationId = _repository.AddLocation(location);

                                        JobLocation jl = new JobLocation();
                                        jl.JobId = jobId;
                                        jl.LocationId = locationId;

                                        _repository.AddJobLocation(jl);

                                    }
                                }
                                else
                                {
                                    Location location = new Location();

                                    location.CountryId = Convert.ToInt32(countryId);
                                    location.StateId = Convert.ToInt32(stateId);
                                    location.CityId = Convert.ToInt32(cityId);

                                    int locationId = _repository.AddLocation(location);

                                    JobLocation jl = new JobLocation();
                                    jl.JobId = jobId;
                                    jl.LocationId = locationId;

                                    _repository.AddJobLocation(jl);
                                }

                            }
                        }
                        else
                        {
                            Location location = new Location();

                            location.CountryId = Convert.ToInt32(countryId);
                            location.StateId = Convert.ToInt32(stateId);

                            int locationId = _repository.AddLocation(location);

                            JobLocation jl = new JobLocation();
                            jl.JobId = jobId;
                            jl.LocationId = locationId;

                            _repository.AddJobLocation(jl);
                        }

                    }
                }
                else
                {
                    Location location = new Location();

                    location.CountryId = Convert.ToInt32(countryId);

                    int locationId = _repository.AddLocation(location);

                    JobLocation jl = new JobLocation();
                    jl.JobId = jobId;
                    jl.LocationId = locationId;

                    _repository.AddJobLocation(jl);
                }

            }

            string[] PostingOtherCountries = { };
            if (!string.IsNullOrEmpty(collection["PostingOtherCountry"]))
                PostingOtherCountries = collection["PostingOtherCountry"].Split(',');

            foreach (string countryId in PostingOtherCountries)
            {
                Location location = new Location();

                location.CountryId = Convert.ToInt32(countryId);

                int locationId = _repository.AddLocation(location);

                JobLocation jl = new JobLocation();
                jl.JobId = jobId;
                jl.LocationId = locationId;

                _repository.AddJobLocation(jl);

            }

          
            _repository.DeleteJobRequiredQualifications(job.Id);

            string[] basicQualifications = { };

            if (!string.IsNullOrEmpty(collection["basicQualification"]))
                basicQualifications = collection["basicQualification"].Split(',');

            foreach (string basicQualification in basicQualifications)
            {
                string[] basicspecializations = { };

                if (!string.IsNullOrEmpty(collection["BasicQualificationSpecialization_" + basicQualification]))
                    basicspecializations = collection["BasicQualificationSpecialization_" + basicQualification].Split(',');

                if (basicspecializations != null && basicspecializations.Length > 0)
                {
                    foreach (string specialisation in basicspecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(basicQualification);
                        jrq.SpecializationId = Convert.ToInt32(specialisation);

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(basicQualification);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }

            string[] postGraduations = { };


            if (!string.IsNullOrEmpty(collection["PostGraduation"]))
                postGraduations = collection["PostGraduation"].Split(',');

            foreach (string postgraduate in postGraduations)
            {
                string[] postSpecializations = { };

                if (!string.IsNullOrEmpty(collection["PostGraduationSpecialization_" + postgraduate]))
                    postSpecializations = collection["PostGraduationSpecialization_" + postgraduate].Split(',');

                if (postSpecializations != null && postSpecializations.Length > 0)
                {
                    foreach (string postSpecialization in postSpecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(postgraduate);
                        jrq.SpecializationId = Convert.ToInt32(postSpecialization);

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(postgraduate);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }

            string[] doctrates = { };

            if (!string.IsNullOrEmpty(collection["Doctrate"]))
                doctrates = collection["Doctrate"].Split(',');

            foreach (string doctrate in doctrates)
            {
                string[] doctorateSpecializations = { };

                if (!string.IsNullOrEmpty(collection["DoctrateSpecialization_" + doctrate]))
                    doctorateSpecializations = collection["DoctrateSpecialization_" + doctrate].Split(',');

                if (doctorateSpecializations != null && doctorateSpecializations.Length > 0)
                {
                    foreach (string doctSpecialization in doctorateSpecializations)
                    {
                        JobRequiredQualification jrq = new JobRequiredQualification();
                        jrq.JobId = jobId;
                        jrq.DegreeId = Convert.ToInt32(doctrate);
                        jrq.SpecializationId = Convert.ToInt32(doctSpecialization);

                        if (jrq.DegreeId > 0)
                            _repository.AddJobRequiredQualification(jrq);
                    }
                }
                else
                {
                    JobRequiredQualification jrq = new JobRequiredQualification();
                    jrq.JobId = jobId;
                    jrq.DegreeId = Convert.ToInt32(doctrate);

                    if (jrq.DegreeId > 0)
                        _repository.AddJobRequiredQualification(jrq);
                }
            }

            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                AdminUserEntry adminuserentry = new AdminUserEntry();
                adminuserentry.AdminId = user.Id;
                adminuserentry.EntryId = job.Id;
                adminuserentry.EntryType = Convert.ToInt32(EntryType.Job);
                adminuserentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                _repository.AddAdminUserEntry(adminuserentry);
                _repository.Save();
            }

            return true;
        }

        private void SetCommonViewData()
        {
            ViewData["Countries"] = new SelectList(_repository.GetCountries(), "Id", "Name");
            //to get the role
            ViewData["Roles"] = new SelectList(_repository.GetRoles(), "Id", "Name", 0);
            ViewData["JobBasicQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.BasicQualification);
            ViewData["JobPostQualifications"] = _repository.GetDegreeswithAnyOption(DegreeType.PostGraduation);
            ViewData["JobDoctorate"] = _repository.GetDegreeswithAnyOption(DegreeType.Doctorate);
        }

        private void SetAddJobViewData()
        {
           
            //salary
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(GetBudgetLakhs(), "Value", "Name", 0);
            ViewData["MinAnnualSalaryThousands"] = new SelectList(GetBudgetThousands(), "Value", "Name", 0);

            //maxsalary 
            ViewData["AnnualSalaryLakhs"] = new SelectList(GetMaxBudgetLakhs(), "Value", "Name", 0);
            ViewData["AnnualSalaryThousands"] = new SelectList(GetMaxBudgetThousands(), "Value", "Name", 0);

            //experience          
            ViewData["ddlTotalExperienceYearsFrom"] = new SelectList(GetTotalExperienceMinYears(), "Value", "Name", 0);
            ViewData["ddlTotalExperienceYearsTo"] = new SelectList(GetTotalExperienceMaxYears(), "Value", "Name", 0);

            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", 0);
           // ViewData["LicenseTypes"] = _repository.GetLicenseTypes();

            //new 
                var license = _repository.GetLicenseTypesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
                license.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
                ViewData["License"] = license;
            //end new
           

            //to get the role
            ViewData["Roles"] = new SelectList(_repository.GetRoles(), "Id", "Name", 0);

            var indus = _repository.GetIndustriesEnumerable().Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name }).ToList();
            indus.Insert(0, new SelectListItem { Value = "0", Text = "--- Any ---" });
            ViewData["Industries"] = indus;

        }

        private void SetEditJobViewData(Job job)
        {

            //salary
            int lakhs = job.MaxBudget.HasValue ? (int)(job.MaxBudget / 100000) : 0;
            int thousands = job.MaxBudget.HasValue ? (int)((job.MaxBudget - (lakhs * 100000)) / 1000) : 0;
            ViewData["AnnualSalaryLakhs"] = new SelectList(GetMaxBudgetLakhs(), "Value", "Name", lakhs);
            ViewData["AnnualSalaryThousands"] = new SelectList(GetMaxBudgetThousands(), "Value", "Name", thousands);

            //minsalary
            int minlakhs = job.Budget.HasValue ? (int)(job.Budget / 100000) : 0;
            int minthousands = job.Budget.HasValue ? (int)((job.Budget - (minlakhs * 100000)) / 1000) : 0;
            ViewData["MinAnnualSalaryLakhs"] = new SelectList(GetBudgetLakhs(), "Value", "Name", minlakhs);
            ViewData["MinAnnualSalaryThousands"] = new SelectList(GetBudgetThousands(), "Value", "Name", minthousands);

            //experience
            int minyears = job.MinExperience.HasValue ? (int)job.MinExperience.Value / 31104000 : 0;
            int maxyears = job.MaxExperience.HasValue ? (int)job.MaxExperience.Value / 31104000 : 0;
            ViewData["TotalExperienceYearsFrom"] = new SelectList(GetTotalExperienceMinYears(), "Value", "Name", minyears);
            ViewData["TotalExperienceYearsTo"] = new SelectList(GetTotalExperienceMaxYears(), "Value", "Name", maxyears);

            //ViewData["LicenseTypes"] = _repository.GetLicenseTypes();
            List<LicenseType> license = new List<LicenseType>();
            license.Add(new LicenseType { Id = 0, Name = "--- Any ---" });
            var result = _repository.GetLicenseTypes();
            foreach (var li in result)
            {
                license.Add(new LicenseType { Id = li.Id, Name = li.Name });
            }
            ViewData["LicenseTypes"] = license;
            ViewData["LicenseTypeIds"] = job.JobLicenseTypes.Select(clt => clt.LicenseTypeId);



            ViewData["Functions"] = new SelectList(_repository.GetFunctions(), "Id", "Name", job.FunctionId);
            JobRole jobrole = _repository.GetRolesByJobId(job.Id);
            int functionId = _repository.GetFunctionidByJobId(job.Id);

            if (jobrole != null)
                ViewData["Roles"] = new SelectList(_repository.GetRoleByFunctionId(functionId), "Id", "Name", jobrole.RoleId);
            else
                ViewData["Roles"] = new SelectList(_repository.GetRoles(0), "Id", "Name", job.FunctionId.HasValue ? job.FunctionId.Value : 0);  

            ViewData["JobIndustries"] = _repository.GetIndustries();
            ViewData["JobPreferredIndustryId"] = job.JobPreferredIndustries.Select(jpi => jpi.IndustryId);

            //by vignesh

            IEnumerable<Location> locations = _repository.GetLocationsbyJobId(job.Id);

            if (locations.Count() > 0)
            {
                ViewData["CountryIds"] = String.Join(",", locations.Select(loc => loc.CountryId));
                ViewData["StateIds"] = String.Join(",", locations.Select(loc => loc.StateId).Where(jr => jr != null));
                ViewData["CityIds"] = String.Join(",", locations.Select(loc => loc.CityId).Where(jr => jr != null));
                ViewData["RegionIds"] = String.Join(",", locations.Select(loc => loc.RegionId).Where(jr => jr != null));
            }
            
            IEnumerable<JobRequiredQualification> basiQualifications = _repository.GetJobRequiredQualifications(job.Id, DegreeType.BasicQualification);

            if (basiQualifications.Count() > 0)
            {
                ViewData["basiQualificationIds"] = String.Join(",", basiQualifications.Select(bq => bq.DegreeId));
                ViewData["basiQualificationSpecializationIds"] = String.Join(",", basiQualifications.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }

            IEnumerable<JobRequiredQualification> postGraduations = _repository.GetJobRequiredQualifications(job.Id, DegreeType.PostGraduation);

            if (postGraduations.Count() > 0)
            {
                ViewData["postGraduationIds"] = String.Join(",", postGraduations.Select(bq => bq.DegreeId));
                ViewData["postGraduationSpecializationIds"] = String.Join(",", postGraduations.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }

            IEnumerable<JobRequiredQualification> doctorates = _repository.GetJobRequiredQualifications(job.Id, DegreeType.Doctorate);

            if (doctorates.Count() > 0)
            {
                ViewData["doctorateIds"] = String.Join(",", doctorates.Select(bq => bq.DegreeId));
                ViewData["doctorateSpecializationIds"] = String.Join(",", doctorates.Select(bq => bq.SpecializationId).Where(jr => jr != null));
            }

        }

              

        private List<DropDownItem> GetMaxBudgetLakhs()
        {
            List<DropDownItem> maxBudgetLakhs = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                maxBudgetLakhs.Add(item);
            }

            return maxBudgetLakhs;
        }

        private List<DropDownItem> GetMaxBudgetThousands()
        {
            List<DropDownItem> maxBudgetThousands = new List<DropDownItem>();

            for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                maxBudgetThousands.Add(item);
            }

            return maxBudgetThousands;
        }

        private List<DropDownItem> GetBudgetLakhs()
        {
            List<DropDownItem> BudgetLakhs = new List<DropDownItem>();

            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                BudgetLakhs.Add(item);
            }

            return BudgetLakhs;
        }

        private List<DropDownItem> GetBudgetThousands()
        {
            List<DropDownItem> BudgetThousands = new List<DropDownItem>();

            for (int i = 0; i <= 95; i = i + 5)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                BudgetThousands.Add(item);
            }

            return BudgetThousands;
        }

        private List<DropDownItem> GetTotalExperienceMinYears()
        {
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }

            return totalExperienceYears;
        }

        private List<DropDownItem> GetTotalExperienceMaxYears()
        {
            List<DropDownItem> totalExperienceYears = new List<DropDownItem>();
            for (int i = 0; i <= 50; i++)
            {
                DropDownItem item = new DropDownItem();
                item.Name = i.ToString();
                item.Value = i;
                totalExperienceYears.Add(item);
            }

            return totalExperienceYears;
        }

        #endregion
        #endregion


        #region Employer Summary

        public ActionResult EmployerSummary()
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

                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerSummary)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //Employer Summary
        [HttpPost]
        public ActionResult EmployerSummary(FormCollection collection)
        {
            using (var _db = new Dial4JobzEntities())
            {
                string filter = collection["filter"].ToString();
                string where = collection["where"].ToString();
                string reportDate = collection["Day"].ToString();

                TimeSpan ts;
                DateTime dtStart = Convert.ToDateTime("1900-01-01");

                if (where == "0")
                {
                    dtStart = Convert.ToDateTime("1900-01-01");
                }
                if (where == "30")
                {
                    ts = new TimeSpan(30, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "7")
                {
                    ts = new TimeSpan(7, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "1")
                {
                    ts = new TimeSpan(1, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }

                DateTime dtEnd = DateTime.Now;
                var list = new List<ReportSummary>();

                if (filter == "1")
                {

                    var employers = _db.Organizations.Include("Industry")
                                    .Where(x => x.CreateDate >= dtStart && x.CreateDate <= dtEnd)
                                    .GroupBy(x => x.Industry.Name)
                                    .Select(y => new ReportSummary
                                    {
                                        Name = y.Key,
                                        TotalCount = y.Count()
                                    }).ToList().OrderBy(y => y.Name);

                    return View(employers);
                }

                if (filter == "2")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Countries on i.CountryId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                if (filter == "3")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Cities on i.CityId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                if (filter == "4")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Regions on i.RegionId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                //------------------------------

                return View();
            }

        }

        #endregion

        #region Job Summary

        public ActionResult JobSummary()
        {
            return RedirectToAction("Edit", "Candidates");

            // return View();
        }
        //Employer Summary
        [HttpPost]
        public ActionResult JobSummary(FormCollection collection)
        {
            using (var _db = new Dial4JobzEntities())
            {
                string filter = collection["filter"].ToString();
                string where = collection["where"].ToString();
                string reportDate = collection["Day"].ToString();

                TimeSpan ts;
                DateTime dtStart = Convert.ToDateTime("1900-01-01");

                if (where == "0")
                {
                    dtStart = Convert.ToDateTime("1900-01-01");
                }
                if (where == "30")
                {
                    ts = new TimeSpan(30, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "7")
                {
                    ts = new TimeSpan(7, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }
                if (where == "1")
                {
                    ts = new TimeSpan(1, 0, 0, 0, 0);
                    dtStart = DateTime.Now.Subtract(ts);
                }

                DateTime dtEnd = DateTime.Now;
                var list = new List<ReportSummary>();

                if (filter == "1")
                {

                    var result = _db.Jobs.Include("Organization")
                                    .Where(x => x.CreatedDate >= dtStart && x.CreatedDate <= dtEnd)
                                    .GroupBy(x => x.Organization.Name)
                                    .Select(y => new ReportSummary
                                    {
                                        Name = y.Key,
                                        TotalCount = y.Count()
                                    }).ToList().OrderBy(y => y.Name);

                    return View(result);
                }

                if (filter == "2")
                {

                    var result = from j in _db.Jobs
                                 join i in _db.Functions on j.FunctionId equals i.Id
                                 where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                 group new { i, j } by new
                                 {
                                     i.Name
                                 } into g
                                 orderby g.Key.Name ascending
                                 select new ReportSummary
                                 {
                                     Name = g.Key.Name,
                                     TotalCount = g.Count()
                                 };


                    return View(result.ToList());
                }

                if (filter == "3")
                {

                    var result = from j in _db.Jobs
                                 where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                 group new { j } by new
                                 {
                                     j.Budget
                                 } into g
                                 orderby g.Key.Budget descending
                                 select new ReportSummary
                                 {
                                     NameLng = (long)g.Key.Budget,
                                     TotalCount = g.Count()
                                 };


                    return View(result.ToList());
                }

                if (filter == "4")
                {

                    var result = from j in _db.Jobs
                                 where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                 group new { j } by new
                                 {
                                     j.MaxExperience
                                 } into g
                                 orderby g.Key.MaxExperience descending
                                 select new ReportSummary
                                 {
                                     NameLng = g.Key.MaxExperience,
                                     TotalCount = g.Count()

                                 };


                    return View(result.ToList());
                }

                if (filter == "5")
                {

                    var result = from j in _db.Jobs
                                 where j.CreatedDate >= dtStart && j.CreatedDate <= dtEnd
                                 group new { j } by new
                                 {
                                     j.JobRequiredQualifications
                                 } into g
                                 orderby g.Key.JobRequiredQualifications descending
                                 select new ReportSummary
                                 {
                                     Name = g.Key.JobRequiredQualifications.ToString(),
                                     TotalCount = g.Count()
                                 };


                    return View(result.ToList());
                }


                if (filter == "6")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Countries on i.CountryId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                if (filter == "7")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Cities on i.CityId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                if (filter == "8")
                {

                    var employers = from j in _db.Organizations
                                    join i in _db.Locations on j.LocationId equals i.Id
                                    join c in _db.Regions on i.RegionId equals c.Id
                                    where j.CreateDate >= dtStart && j.CreateDate <= dtEnd
                                    group new { i, c } by new
                                    {
                                        c.Name
                                    } into g
                                    orderby g.Key.Name ascending
                                    select new ReportSummary
                                    {
                                        Name = g.Key.Name,
                                        TotalCount = g.Count()
                                    };

                    return View(employers.ToList());

                }

                //------------------------------

                return View();
            }

        }

        #endregion

        #region Change password

        public ActionResult ChangePassword()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(model.OldPassword)) == SecurityHelper.GetMD5String(user.Password))
            {
                user.Password = SecurityHelper.GetMD5Bytes(model.NewPassword);
                _userRepository.Save();
                return Json(new JsonActionResult { Success = true, Message = "Password Changed Successfully" });

            } else {
                return Json(new JsonActionResult { Success = false, Message = "Old Password is Incorrect" });
            }

        }

        #endregion

       
    }
}
