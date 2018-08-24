using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using PagedList;
using TPLibrary.Logger;
using System.Security.Principal;

namespace Wardship.Areas.Admin.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CourtsController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public CourtsController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
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
                _logger.LogError(ex, $"Exception in CourtsController in Edit method, for user {User.Identity.Name}");
                
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
                _logger.LogError(ex, $"Exception in CourtsController in Create method, for user {User.Identity.Name}");
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
                return Json(new { success = true, id = model.CourtID, message = model.CourtName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in CourtsController in Deactivate method, for user {User.Identity.Name}");
                return PartialView("_Deactivate", model);
            }

            
        }
		
    }
}
