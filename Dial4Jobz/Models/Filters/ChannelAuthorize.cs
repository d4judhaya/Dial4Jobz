using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Results;

namespace Dial4Jobz.Models.Filters
{
    public class ChannelAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new JsonActionResult
                        {
                            Success = false,
                            ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Channel/Login"
                        }
                    };
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult(System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Channel/Login");
                    return;
                }
            }

            string[] roles = SplitString(Roles);

            if (Roles.Length > 0 && !roles.Any(r => r.Contains(filterContext.HttpContext.User.Identity.Name.Split('|')[Constants.ChannelLoginValues.ChannelRole])))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new JsonActionResult
                        {
                            Success = false,
                            ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Channel/Login"
                        }
                    };
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult(System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() + "/Channel/Login");
                    return;
                }
            }            
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

    }
}