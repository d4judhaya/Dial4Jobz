using System.Web.Mvc;

namespace Dial4Jobz.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "Admin_default",
                 "Admin/{controller}/{action}/{id}",
                 new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional }

             );

            //context.MapRoute(
            //     "Admin_default",
            //     "Admin/{controller}/{action}/{id}",
            //     new { controller = "AdminAccount", action = "Logon", id = UrlParameter.Optional }

            // );
        }
    }
}
