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
using System.Web.Routing;
using System.Configuration;

namespace Dial4Jobz.Areas.Admin.Controllers
{
    public class ConsultantReportController : Controller
    {
        //
        // GET: /Admin/ConsultantReport/

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

        [Authorize]
        public ActionResult ConsultantSubscription_Billing(int consultantId)
        {

            if (consultantId != null)
                ViewData["LoggedConsultant"] = consultantId;
            else
                ViewData["LoggedConsultant"] = 0;

            return View();
        }

        public JsonResult getConsultantNameLists(string term)
        {
            List<string> getValues = _repository.GetConsultantName(term).Distinct().ToList();
            return Json(getValues, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Invoice(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    int OrderId;
                    bool ValueIsAnId = int.TryParse(orderId, out OrderId);

                    if (ValueIsAnId)
                    {
                        return View(_vasRepository.GetOrderDetail(OrderId));
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }


        public JsonResult ListConsultantsReports(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string fromDate, string toDate)
        {
            IQueryable<Consultante> consultantResult = _repository.GetConsultantes();

            User user = _userRepository.GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (iSortCol_0 == 0 && "desc" == sSortDir_0)
                consultantResult = consultantResult.OrderByDescending(o => o.Name);
            else if (iSortCol_0 == 0 && "asc" == sSortDir_0)
                consultantResult = consultantResult.OrderBy(o => o.Name);
            else if (iSortCol_0 == 1 && "desc" == sSortDir_0)
                consultantResult = consultantResult.OrderByDescending(o => o.CreatedDate.Value);
            else if (iSortCol_0 == 1 && "asc" == sSortDir_0)
                consultantResult = consultantResult.OrderBy(o => o.CreatedDate.Value);
            else
                consultantResult = consultantResult.OrderByDescending(o => o.Id);

            if (!string.IsNullOrEmpty(sSearch.Trim()))
                consultantResult = consultantResult.Where(o => o.Name.ToLower().Contains(sSearch.ToLower().Trim()) || o.Email != null && o.Email.ToLower().Contains(sSearch.ToLower().Trim()) || o.MobileNumber != null && o.MobileNumber.ToLower().Contains(sSearch.ToLower().Trim()));

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");
                toDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", null).ToString("MM/dd/yyyy");

                var from = DateTime.Parse(fromDate).Date;
                var to = DateTime.Parse(toDate).Date;

                to = to.AddHours(23.99);
                consultantResult = consultantResult.Where(o => o.CreatedDate != null && o.CreatedDate.Value >= from && o.CreatedDate.Value <= to);

            }

            IEnumerable<Consultante> consultantResult1 = consultantResult.Skip(iDisplayStart).Take(iDisplayLength);

            var result = new
            {
                iTotalRecords = consultantResult.Count(),
                iTotalDisplayRecords = consultantResult.Count(),
                aaData = consultantResult1.Select(o => new object[] { o.Name, (o.CreatedDate != null) ? o.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(o.Id, EntryType.Consultant), "<a class='ActionPopup' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Admin/ConsultantReport/PostedJobs/?id=" + o.Id + "&name=" + o.Name + "'>View</a>", o.ContactPerson, o.MobileNumber, o.Email, (o.IPAddress != null ? o.IPAddress : ""), "<a href='JavaScript:void(0)' onclick='Delete(" + o.Id + ")'>Delete</a>" })
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostedJobs()
        {
            return View();
        }

        public JsonResult ListPostedJobs(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string empId, string fromDate, string toDate)
        {
            IQueryable<Job> jobpostedResult = null;

            if (!string.IsNullOrEmpty(empId))
                jobpostedResult = _repository.GetJobsByConsultantId(Convert.ToInt32(empId));
            else
                jobpostedResult = _repository.GetJobsByConsultantId(-0);

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
                aaData = jobpostedResult1.Select(j => new object[] { "<a target='_blank' href='" + System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Jobs/Details/" + Constants.EncryptString(j.Id.ToString()) + "'>" + j.Position + "</a>", (j.CreatedDate != null) ? j.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : "", _repository.GetAdminUserNamebyEntryIdAndEntryType(j.Id, EntryType.Job) != "" ? _repository.GetAdminUserNamebyEntryIdAndEntryType(j.Id, EntryType.Job) : j.Consultante.Name+" (Consultant)" })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            Consultante organization = _repository.GetConsulant(id);
            OrderMaster ordermaster = _vasRepository.GetOrderMasterByConsultantId(id);
            OrderMaster GetOrderId = _vasRepository.GetOrderIdByOrganizationId(id);

            Job job = _repository.GetJob(id);
            if (user.UserName == "admin123")
            {
                if (job != null)
                {
                    _repository.DeleteJobLocations(job.Id);
                    _repository.DeleteJobLocations(job.Id);
                    _repository.DeleteJobPreferredIndustries(job.Id);
                    _repository.DeleteJobRequiredQualifications(job.Id);
                    _repository.DeleteJobRoles(job.Id);
                    _repository.DeleteJobskills(job.Id);
                    _repository.DeleteJob(job.Id);
                }

                if (GetOrderId != null)
                {
                    _vasRepository.DeleteOrderDetails(GetOrderId.OrderId);
                    _vasRepository.DeleteOrderPaymentDetails(GetOrderId.OrderId);
                    _vasRepository.DeletePostedJobAlerts(GetOrderId.OrderId);
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
                Success = false,
                Message = "You don't have permission to delete"
            });
        }


        [HttpPost]
        public ActionResult DeleteOrganization(string ids)
        {
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (user.UserName == "admin123")
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


        /**************Profile save start***********************************/
        public ActionResult AddConsultant()
        {
            string[] userIdentityName = this.User.Identity.Name.Split('|');
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (user != null || (userIdentityName != null && userIdentityName.Length > 1))
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
                if ((userIdentityName != null && userIdentityName.Length > 1) || (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddEmployer)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true))
                {

                    var consultant = new Consultante();
                    
                    List<SelectListItem> industries = new List<SelectListItem>();
                    industries.Add(new SelectListItem { Text = "Education/Teaching/Training", Value = "2362" });
                    industries.Add(new SelectListItem { Text = "Recruitment/Employment Consultants", Value = "2412" });
                    industries.Add(new SelectListItem { Text = "NGO/Non Profit", Value = "2398" });

                    ViewData["Industries"] = new SelectList(industries, "Value", "Text", (consultant.IndustryId != null ? consultant.IndustryId : 0));

                    Location location = consultant.LocationId.HasValue ? _repository.GetLocationById(consultant.LocationId.Value) : null;
                    ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);

                    if (location != null)
                        ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

                    if (location != null && location.StateId.HasValue)
                        ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

                    if (location != null && location.CityId.HasValue)
                        ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);

                    return View(consultant);
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

        public ActionResult ValidateConsultant(string validateConsultant, string validateEmail, string validateMobileNumber)
        {
            Consultante consultant = null;

            if (!string.IsNullOrEmpty(validateConsultant))
            {
                consultant = _userRepository.GetConsultantByName(validateConsultant);
                if (consultant == null)
                    return Json("Consultant not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Consultant exist", JsonRequestBehavior.AllowGet);


            }

            else if (!string.IsNullOrEmpty(validateEmail))
            {
                consultant = _userRepository.GetConsultantEmail(validateEmail);
                if (consultant == null)
                    return Json("Email not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Email exist", JsonRequestBehavior.AllowGet);
            }

            else if (!string.IsNullOrEmpty(validateMobileNumber))
            {
                consultant = _userRepository.GetConsultantMobile(validateMobileNumber);
                if (consultant == null)
                    return Json("Mobile not exist", JsonRequestBehavior.AllowGet);
                else
                    return Json("Mobile exist", JsonRequestBehavior.AllowGet);
            }

            else
            {
                if (consultant == null)
                    return Json("Please enter Consultant name", JsonRequestBehavior.AllowGet);
            }

            return Json("Please enter Consultant name", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConsultantDetail(string validateConsultant, string validateEmail, string validateMobileNumber)
        {
            Consultante consultant = null;
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();
            if (!string.IsNullOrEmpty(validateConsultant))
            {
                consultant = _userRepository.GetConsultantByName(validateConsultant);
                
                if (consultant == null)
                    consultant = new Consultante();

                else
                {
                    Session["LoginAs"] = "ConsultantViaAdmin";
                    Session["consultantId"] = consultant.Id;
                    Session["LoginUser"] = user != null ? user.UserName : this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
                }
            }

            else if (!string.IsNullOrEmpty(validateEmail))
            {
                consultant = _userRepository.GetConsultantEmail(validateEmail);

                if (consultant == null)
                    consultant = new Consultante();
                else
                {
                    Session["LoginAs"] = "ConsultantViaAdmin";
                    Session["consultantId"] = consultant.Id;
                    Session["LoginUser"] = user != null ? user.UserName : this.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelEmail];
                }
            }

            else if (!string.IsNullOrEmpty(validateMobileNumber))
            {
                consultant = _userRepository.GetConsultantMobile(validateMobileNumber);

                if (consultant == null)
                    consultant = new Consultante();

                else
                {
                    Session["LoginAs"] = "ConsultantViaAdmin";
                    Session["consultantId"] = consultant.Id;
                }
            }

            else
            {
                if (consultant == null)
                    consultant = new Consultante();
            }

            ViewData["Industries"] = new SelectList(_repository.GetIndustries(), "Id", "Name", consultant.IndustryId);

              Location location = consultant.LocationId.HasValue ? _repository.GetLocationById(consultant.LocationId.Value) : null;
            ViewData["Country"] = new SelectList(_repository.GetCountries(), "Id", "Name", location != null ? location.CountryId : 0);


            if (location != null)
                ViewData["State"] = new SelectList(_repository.GetStates(location.CountryId), "Id", "Name", location.StateId.HasValue ? location.StateId.Value : 0);

            if (location != null && location.StateId.HasValue)
                ViewData["City"] = new SelectList(_repository.GetCities(location.StateId.Value), "Id", "Name", location.CityId.HasValue ? location.CityId.Value : 0);

            if (location != null && location.CityId.HasValue)
                ViewData["Region"] = new SelectList(_repository.GetRegions(location.CityId.Value), "Id", "Name", location.RegionId.HasValue ? location.RegionId.Value : 0);

            return View("AddConsultant", consultant);

        }


        public ActionResult SaveConsultant(FormCollection collection)
        {
            Consultante consultant = null;
            DateTime dateTime = DateTime.Now;
            var timeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, "India Standard Time");
            bool updateOperation = false;
            User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.User.Identity.Name).FirstOrDefault();

            if (!string.IsNullOrEmpty(collection["Id"]))
            {
                int currentId = Convert.ToInt32(collection["Id"]);
                if (currentId == 0)
                {
                    var name = collection["Name"];

                    var consultantName = _userRepository.GetConsultantByName(collection["Name"]);

                    if (consultantName != null)
                    {
                        return Json(new JsonActionResult
                        {
                            Success = false,
                            Message = "Consultant is already Registered.",
                            ReturnUrl = "/Admin/ConsultantReport/GetConsultantDetail?validateConsultant=" + consultantName.Name 
                        });
                    }

                    if (collection["Email"] != "")
                    {
                        var consultantEmail = _userRepository.GetConsultantEmail(collection["Email"]);
                        if (consultantEmail != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Email id is already registered",
                                ReturnUrl = "/Admin/ConsultantReport/GetConsultantDetail?validateEmail=" + consultantEmail.Email 
                            });

                        }

                        else
                        {
                            consultant = new Consultante();
                        }
                    }

                    if (collection["MobileNumber"] != "")
                    {
                        var consultantMobile = _userRepository.GetConsultantMobile(collection["MobileNumber"]);
                        if (consultantMobile != null)
                        {
                            return Json(new JsonActionResult
                            {
                                Success = true,
                                Message = "Mobile Number is already registered",
                                ReturnUrl = "/Admin/ConsultantReport/GetConsultantDetail?validateMobileNumber=" + consultantMobile.MobileNumber 
                            });

                        }
                        else
                        {
                            consultant = new Consultante();
                        }
                    }

                    else
                    {
                        consultant = new Consultante();
                    }
                }
                else
                {
                    consultant = _userRepository.GetConsultantsById(currentId);
                    updateOperation = true;
                }
            }
            else
            {
                consultant = new Consultante();
            }
            

            if (!string.IsNullOrEmpty(collection["Industries"]))
                consultant.IndustryId = Convert.ToInt32(collection["Industries"]);
            consultant.Name = collection["Name"];
            consultant.ContactPerson = collection["ContactPerson"];
            consultant.Email = collection["Email"];
            consultant.Website = collection["Website"];
            consultant.ContactNumber = collection["ContactNumber"];
            consultant.MobileNumber = collection["MobileNumber"];
            consultant.Address = collection["Address"];
            consultant.Pincode = collection["Pincode"];


            if (consultant.CreatedDate == null)
            {
                consultant.CreatedDate = timeZone;
            }
            else
            {
                consultant.UpdatedDate = timeZone;
            }

            // Generation of Username and password start

            Random randomPassword = new Random();
            string pwdDob = string.Empty;
            string firstname = string.Empty;
            string randomString = string.Empty;

            string username = randomPassword.Next(1111, 2222).ToString();

            string fullname = consultant.Name;
            string contactperson = consultant.ContactPerson;

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
                    consultant.UserName = firstname + username;

                else
                    consultant.UserName = collection["ContactPerson"];

                var usernameExists = _userRepository.GetOrganizationByUserName(consultant.UserName);
                if (usernameExists != null)
                {
                    consultant.UserName = collection["ContactNumber"] + username;
                }
                
                randomString = SecurityHelper.GenerateRandomString(6, true);
                byte[] password = SecurityHelper.GetMD5Bytes(randomString);
                consultant.Password = password;

                string phVerficationNo = randomPassword.Next(1000, 9999).ToString();
                consultant.PhoneVerificationNo = Convert.ToInt32(phVerficationNo);
            }

            Location location = new Location();
            if (!string.IsNullOrEmpty(collection["Country"])) location.CountryId = Convert.ToInt32(collection["Country"]);
            if (!string.IsNullOrEmpty(collection["State"])) location.StateId = Convert.ToInt32(collection["State"]);
            if (!string.IsNullOrEmpty(collection["City"])) location.CityId = Convert.ToInt32(collection["City"]);
            if (!string.IsNullOrEmpty(collection["Region"])) location.RegionId = Convert.ToInt32(collection["Region"]);

            if (location.CountryId != 0)
                consultant.LocationId = _repository.AddLocation(location);

            int consultantId = consultant.Id;


            if (!TryValidateModel(consultant))
                return Json(new JsonActionResult { Success = false, Message = ModelStateErrorMessage });


            if (updateOperation == false)
                _userRepository.AddConsultant(consultant);

            _userRepository.Save();


            if (updateOperation == false)
            {
                string[] userIdentityName = this.User.Identity.Name.Split('|');
                if (user != null)
                {
                    AdminUserEntry adminuserentry = new AdminUserEntry();
                    adminuserentry.AdminId = user.Id;
                    adminuserentry.EntryId = consultant.Id;
                    adminuserentry.EntryType = Convert.ToInt32(EntryType.Consultant);
                    //}

                    adminuserentry.CreatedOn = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    _repository.AddAdminUserEntry(adminuserentry);
                    _repository.Save();
                }

                if (consultant.Email != "")
                {

                    EmailHelper.SendEmail(
                          Constants.EmailSender.EmployerSupport,
                          consultant.Email,
                          Constants.EmailSubject.Registration,
                          Constants.EmailBody.ConsultantRegister
                              .Replace("[NAME]", consultant.Name)
                              .Replace("[USERNAME]", consultant.UserName)
                              .Replace("[PASSWORD]", randomString)
                              .Replace("[EMAIL]", consultant.Email)
                              .Replace("[LINK_NAME]", "Verify your E-mail ID")
                              .Replace("[LINK]", ConfigurationManager.AppSettings["SiteURL"].ToString() + "/Employer/Activation?Id=" + Dial4Jobz.Models.Constants.EncryptString(consultant.Id.ToString()))
                              );
                }


                if (consultant.MobileNumber != "")
                {
                    SmsHelper.SendSecondarySms(
                                Constants.SmsSender.SecondaryUserName,
                                Constants.SmsSender.SecondaryPassword,
                                Constants.SmsBody.SMSCandidateRegister
                                                .Replace("[USER_NAME]", consultant.UserName)
                                            .Replace("[PASSWORD]", randomString)
                                           .Replace("[CODE]", consultant.PhoneVerificationNo.ToString()),

                                Constants.SmsSender.SecondaryType,
                                Constants.SmsSender.Secondarysource,
                                Constants.SmsSender.Secondarydlr,
                                consultant.MobileNumber
                                );

                }
            }

            return Json(new JsonActionResult
            {
                Success = true,
                Message = "Consultant Added Successfully",
                ReturnUrl = "/Admin/ConsultantReport/GetConsultantDetail?validateConsultant=" + consultant.Name
            });
        }

        public ActionResult ConsultantDashBoard(int consultantId)
        {
            ViewData["consultantId"] = consultantId;
            var consultant = _userRepository.GetConsultantsById(consultantId);
            return View();
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



    }
}
