using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Exceptions;

namespace Dial4Jobz.Models.Filters
{
    public class HandleErrorWithAjaxFilter : HandleErrorAttribute
    {
        public bool ShowStackTraceIfNotDebug { get; set; }
        public string ErrorMessage { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var content = ShowStackTraceIfNotDebug || filterContext.HttpContext.IsDebuggingEnabled ? filterContext.Exception.StackTrace : string.Empty;
                filterContext.Result = new ContentResult
                {
                    ContentType = "text/plain",
                    Content = content
                };

                string message = string.Empty;
                if (!filterContext.Controller.ViewData.ModelState.IsValid)
                    message = GetModeStateErrorMessage(filterContext);
                else if (filterContext.Exception is ClientException)
                    message = filterContext.Exception.Message.Replace("\r", " ").Replace("\n", " ");
                else if (!string.IsNullOrEmpty(ErrorMessage))
                    message = ErrorMessage;
                else
                    message = "An error occured while attemting to perform the last action.  Sorry for the inconvenience.";
                    //message = "Please enter correct values";

                filterContext.HttpContext.Response.Status = "500 " + message;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            else
            {
                base.OnException(filterContext);
            }
        }        

        private string GetModeStateErrorMessage(ExceptionContext filterContext)
        {
            string errorMessage = string.Empty;
            foreach (var key in filterContext.Controller.ViewData.ModelState.Keys)
            {
                var error = filterContext.Controller.ViewData.ModelState[key].Errors.FirstOrDefault();
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
}