using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wardship.Controllers
{
    public class HomeController : Controller
    {
		SourceRepository db = new SQLRepository();
        public HomeController()
            : this(new SQLRepository())
        { }
        public HomeController(SourceRepository repository)
        {
            db = repository;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {

            string name = (User as ICurrentUser).DisplayName;

            ViewBag.Message = string.Format("{0}, welcome to ASP.NET MVC!", name);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public PartialViewResult DisplayAlerts()
        {
            var alerts = db.getCurrentAlerts();
            if (alerts != null)
            {
                return PartialView("_DisplayAlerts", alerts);
            }
            return null;
        }

        
        public ActionResult Test()
        {
            return View();
        }
      
    }
}
