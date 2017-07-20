using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using Wardship.Models;

namespace Wardship.Areas.Admin.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AlertsController : Controller
    {
		SourceRepository db = new SQLRepository();
        public AlertsController()
            : this(new SQLRepository())
        { }
        public AlertsController(SourceRepository repository)
        {
            db = repository;
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
                Trace.TraceError("Admin/AlertsController - Create. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                return PartialView("_Deactivate", model);
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
                Trace.TraceError("Admin/AlertsController - Edit. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                return View(model);
            }
            
        }
      
      
    }
}
