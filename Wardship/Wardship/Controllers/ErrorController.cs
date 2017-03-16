using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wardship.Models;
using System.Web.UI;

namespace Wardship.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index(int errCode)
        {
            ErrorModel model = new ErrorModel(errCode);
            return View(model);
        }

        public ActionResult IndexByModel(ErrorModel model)
        {
            if (model == null) model = TempData["ErrorModel"] as ErrorModel;
            return View("Index", model);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 0)]
        public ActionResult ClosedFile()
        {
            HttpResponse.RemoveOutputCacheItem("/Error/ClosedFile");
            ErrorModel model = new ErrorModel();
            model.ErrorMessage = string.Format("File {0} has been closed, no further changes or amendments can be made to it, or any of it's associated records", TempData["UID"]);
            return View(model);
        }

        public ActionResult unauthorised()
        {
            return View();
        }
        public ViewResult NotLoggedIn()
        {
            return View();
        }
    }
}
