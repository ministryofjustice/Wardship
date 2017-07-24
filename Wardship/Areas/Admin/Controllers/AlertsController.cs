using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Wardship.Logger;
using Wardship.Models;

namespace Wardship.Areas.Admin.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AlertsController : Controller
    {
        private readonly SourceRepository db;
        private readonly ITelemetryLogger _logger;

        public AlertsController(SQLRepository repository, ITelemetryLogger logger)
        {
            db = repository;
            _logger = logger;
        }

        //
        // GET: /Admin/Alert/

        public ActionResult Index()
        {
            IEnumerable<Alert> model = db.getAllAlerts();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Alert model)
        {
            try
            {
                model.Live = true;
                db.CreateAlert(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in AlertsController in Create method, for user {User.Identity.Name}");
                return View("Error");
            }
            
        }
        
        public ActionResult Edit(int id)
        {
            Alert model = db.getAlertbyID(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Alert model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.updateAlert(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in AlertsController in Create method, for user {User.Identity.Name}");
                return View("Error");
            }
           
        }
      
      
    }
}
