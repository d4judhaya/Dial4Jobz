using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Dial4Jobz.Helpers;

namespace Dial4Jobz.Models.Constraints
{
    public class NotNullOrEmptyConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            try
            {
                string userName = values["userName"].ToString().ToLower();

                if (StringHelper.IsReservedWord(userName))
                    return false;

                return !String.IsNullOrEmpty(userName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}