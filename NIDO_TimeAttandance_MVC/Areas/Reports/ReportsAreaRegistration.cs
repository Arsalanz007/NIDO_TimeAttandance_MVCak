using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Reports
{
    public class ReportsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Reports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "NIDO_TimeAttandance_MVC.Areas.Reports.Controllers" }
            );
        }
    }
}