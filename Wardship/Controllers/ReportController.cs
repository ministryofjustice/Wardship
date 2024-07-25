using System;
using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using TPLibrary.Logger;

namespace Wardship.Controllers
{
    public class ReportController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public ReportController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                var model = new Report
                {
                    ReportBegin = Convert.ToDateTime("01/01/2013"),
                    ReportFinal = DateTime.Today
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in ReportController in Index method, for user {(User as ICurrentUser).DisplayName}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Report model)
        {
            if (ModelState.IsValid && model.IsValidDateRange())
            {
                var wardshipRecords = db.WardshipsGetAll()
                    .Where(w => w.DateOfOS >= model.ReportBegin && w.DateOfOS <= model.ReportFinal);

                model.WardshipRecordsList = wardshipRecords.ToList();

                return View("Report", model);
            }

            if (!model.IsValidDateRange())
            {
                ModelState.AddModelError("", "End date must be after or equal to Begin date");
            }

            return View(model);
        }

    }
}