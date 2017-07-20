using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using PagedList;
using System.Diagnostics;

namespace Wardship.Areas.Admin.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CourtsController : Controller
    {
		SourceRepository db = new SQLRepository();
        public CourtsController()
            : this(new SQLRepository())
        { }
        public CourtsController(SourceRepository repository)
        {
            db = repository;
        }

        //
        // GET: /Admin/Court/

        public ActionResult Index(CourtListView model)
        {
            if (model == null)
            {
                model = new CourtListView();
            }

            IEnumerable<Court> Courts = db.getAllCourts().Where(x=>x.active==true);
            model.TotalRecordCount = Courts.Count();
            if (model.detailContains != "" && model.detailContains != null)
            {
                Courts = Courts.Where(c => c.CourtName.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            switch (model.sortOrder)
            {
                case "DX desc":
                    Courts = Courts.OrderByDescending(x => x.DX);
                    break;
                case "DX asc":
                    Courts = Courts.OrderBy(x => x.DX);
                    break;
                case "Addrss desc":
                    Courts = Courts.OrderByDescending(x => x.AddressLine1);
                    break;
                case "Addrss asc":
                    Courts = Courts.OrderBy(x => x.AddressLine1);
                    break;
                case "CrtNm desc":
                    Courts = Courts.OrderByDescending(x => x.CourtName);
                    break;
                case "CrtNm asc":
                default:
                    model.sortOrder = "CrtNm asc";
                    Courts = Courts.OrderBy(x => x.CourtName);
                    break;
            }
            model.Courts = Courts.ToPagedList(model.page, Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        public ActionResult Details(int id)
        {
            Court model = db.getCourtByID(id);
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            Court model = db.getCourtByID(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Court model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.EditCourt(model);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError("Admin/CourtsController - Edit. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                ModelState.AddModelError("Error", string.Format("Error: {0}", genericFunctions.GetLowestError(ex)));
            }
            return View("Edit", model);
        }
       
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Court model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CreateCourt(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Admin/CourtsController - Create. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                ModelState.AddModelError("Error", string.Format("Error: {0}", genericFunctions.GetLowestError(ex)));
            }
            return View("Edit", model);
        }

        public ActionResult Deactivate(int id)
        {
            Court model = db.getCourtByID(id);
            return PartialView("_Deactivate", model);
        }
        [HttpPost]
        public ActionResult Deactivate(Court model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("error", "uh oh");
                    return PartialView("_Deactivate", model);
                }
                Court crt = db.getCourtByID(model.CourtID);
                //load Court to delete
                crt.active = false;
                crt.deactivatedBy = (User as Wardship.ICurrentUser).DisplayName;
                crt.deactivated = DateTime.Now;
                db.EditCourt(crt);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Admin/CourtsController - Deactivate. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                return PartialView("_Deactivate", model);
            }
            return Json(new { success = true, id = model.CourtID, message=model.CourtName });
        }
		
    }
}
