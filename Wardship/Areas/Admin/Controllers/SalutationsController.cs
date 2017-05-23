using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wardship.Models;
using System.Configuration;
using PagedList;
using System.Data;

//namespace Wardship.Areas.Admin.Controllers
//{
//    [AuthorizeRedirect(Roles = "Admin")]
//    public class SalutationsController : Controller
//    {
//        SourceRepository db = new SQLRepository();
//        public SalutationsController()
//            : this(new SQLRepository())
//        { }
//        public SalutationsController(SourceRepository repository)
//        {
//            db = repository;
//        }


//        // GET: /Admin/Salutation/Index
//        public ActionResult Index(SalutationListView model)
//        {
//            if (model.page < 1)
//            {
//                model.page = 1;
//            }

//            IEnumerable<Salutation> Salutations = db.GetListofSalutations();

//            if (model.onlyActive == true)//showing active by default
//            {
//                Salutations = Salutations.Where(c => c.active == true);
//            }
//            model.Salutations = Salutations.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));

//            return View("Index", model);
//        }

//        // GET: /Admin/Salutation/Details/5
//        public ActionResult Details(int id)
//        {
//            var salutations = db.GetSalutationByID(id);
//            return View("Details", salutations);
//        }

//        // GET: /Admin/Gender/Create
//        public ActionResult Create()
//        {
//            return View("Create");
//        }


//        // POST: /Admin/Salutation/Create
//        [HttpPost]
//        public ActionResult Create(Salutation model)
//        {
//            if (ModelState.IsValid)
//            {
//                model.active = true;
//                db.CreateSalutation(model);

//                return RedirectToAction("Index");
//            }

//            return View(model);
//        }


//        public ActionResult Edit(int id)
//        {
//            var salutations = db.GetSalutationByID(id);
//            return View("Edit", salutations);
//        }

//        [HttpPost]
//        public ActionResult Edit(Salutation model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    db.SalutationEditByID(model);
//                    return RedirectToAction("Index");
//                }
//                return View("Edit", model);
//            }
//            catch
//            {
//                return View("Edit", model);
//            }
//        }


//        // GET: /Admin/Salutation/Deactivate/5
//        public ActionResult Deactivate(int id)
//        {
//            Salutation salutations = db.SalutationDeleteByID(id);
//            return View("Deactivate", salutations);
//        }


//        // POST: /Admin/Salutation/Deactivate/5
//        [HttpPost, ActionName("Deactivate")]
//        public ActionResult DeactivateConfirmed(int id)
//        {
//            Salutation model = db.SalutationDeleteByID(id);

//            model.active = false;
//            model.deactivated = DateTime.Now;
//            model.deactivatedBy = User.Identity.Name;
//            db.SalutationDeactivateByID(model);


//            return RedirectToAction("Index");
//        }

//    }


//}
