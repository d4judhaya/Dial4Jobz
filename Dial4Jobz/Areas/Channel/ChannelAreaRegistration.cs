using System.Web.Mvc;

namespace Dial4Jobz.Areas.Channel
{
    public class ChannelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Channel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Channel_default",
                "Channel/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Login", id = UrlParameter.Optional }
            );            
        }
    }
}
