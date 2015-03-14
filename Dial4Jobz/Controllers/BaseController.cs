using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models;
using System.IO;
using Dial4Jobz.Models.Repositories;


namespace Dial4Jobz.Controllers
{
    public class BaseController : Controller
    {
        public Candidate LoggedInCandidate;
        public Organization LoggedInOrganization;
        public Consultante LoggedInConsultant;
        public User LoggedInAdmin;
        public UserRepository _userRepository = new UserRepository();
                
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoggedInCandidate = User.Identity.IsAuthenticated && !IsEmployer ? _userRepository.GetCandidateByUserName(User.Identity.Name) : null;
            LoggedInOrganization = User.Identity.IsAuthenticated && IsEmployer ? _userRepository.GetOrganizationByUserName(User.Identity.Name) : null;
            LoggedInAdmin = User.Identity.IsAuthenticated ? _userRepository.GetAdminUsersByUserName(User.Identity.Name) : null;
            LoggedInConsultant = User.Identity.IsAuthenticated ? _userRepository.GetConsultantsByUserName(User.Identity.Name) : null;
            ViewData["LoggedInCandidate"] = LoggedInCandidate;
            ViewData["LoggedInOrganization"] = LoggedInOrganization;
            ViewData["LoggedInAdmin"] = LoggedInAdmin;
            ViewData["LoggedInConsultant"] = LoggedInConsultant;
            ViewData["Root"] = Constants.Path.Root;

            base.OnActionExecuting(filterContext);
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
                        //if (string.IsNullOrEmpty(errorMessage))
                        //    errorMessage = error.ErrorMessage;
                        //else
                        //    errorMessage = string.Format("{0}, {1}", errorMessage, error.ErrorMessage);
                    }
                }

                return errorMessage;
            }
        }

        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        protected bool IsAdmin
        {
            get
            {
                return ((Request.UrlReferrer !=null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("admin")));
            }
        }

        protected bool IsConsultant
        {
            get
            {
                return ((Request.UrlReferrer!=null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("consult")));
            }
        }

        protected bool IsEmployer
        {
            get
            {
               
                //return ((Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer")) || Session["LoginAs"] == "Employer");
                return ((Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("employer")) || Request.RawUrl.Contains("Employer/PhoneNoVerification") || Session["LoginAs"] == "Employer");
            }
        }

    }
}
