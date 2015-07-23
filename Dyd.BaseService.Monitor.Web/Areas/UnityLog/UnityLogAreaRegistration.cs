using System.Web.Mvc;

namespace Web.Areas.UnityLog
{
    public class UnityLogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UnityLog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UnityLog_default",
                "UnityLog/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
