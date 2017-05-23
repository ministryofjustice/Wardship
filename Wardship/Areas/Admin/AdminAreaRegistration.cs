using System.Web.Mvc;

namespace Wardship.Areas.Admin
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
            context.MapRoute("AdminHome", "Admin", new { area = "Admin", controller = "Admin", action = "Index" });
            context.MapRoute("AdminLookup", "Admin/Lookups", new { area = "Admin", controller = "Admin", action = "LookUps" });
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
