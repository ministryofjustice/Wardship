using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wardship.Models;

namespace Wardship.Areas.Admin.Controllers
{
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
            model.Live = true;
            db.CreateAlert(model);
            return RedirectToAction("Index");
        }
        
        public ActionResult Edit(int id)
        {
            Alert model = db.getAlertbyID(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Alert model)
        {
            if (ModelState.IsValid)
            {
                db.updateAlert(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
      
      
    }
}
