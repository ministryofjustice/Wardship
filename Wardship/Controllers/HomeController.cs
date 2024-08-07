﻿using System.Web.Mvc;
using TPLibrary.Logger;

namespace Wardship.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class HomeController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public HomeController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
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
