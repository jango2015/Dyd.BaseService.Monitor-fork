using System.Web.Mvc;

namespace Web.Areas.TimeWatchCustomer
{
    public class TimeWatchCustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TimeWatchCustomer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TimeWatchCustomer_default",
                "TimeWatchCustomer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
