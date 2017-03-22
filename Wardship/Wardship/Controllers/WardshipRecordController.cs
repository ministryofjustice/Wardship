using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wardship.Models;


using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Security.Principal;



namespace Wardship.Controllers
{
    [Authorize]
    public class WardshipRecordController : Controller
    {
		SourceRepository db = new SQLRepository();
        public WardshipRecordController()
            : this(new SQLRepository())
        { }
        public WardshipRecordController(SourceRepository repository)
        {
            db = repository;
        }



        // GET: /Wardship/ using model to return the whole data so we know the number of records and pageing info 
        public ActionResult Index(WardshipRecordVMlistView model)
        {
            var ListofWardships = db.WardshipsGetAll();
            model.WardshipResults = ListofWardships.OrderByDescending(s => s.WardshipCaseID).ToPagedList(model.page ?? 1, 30);// used with paging 

            return View("Index", model);
        }





        // GET: /WardshipRecord/Details/5
       public ActionResult Details(int id)
        {
            WardshipRecord model = db.GetWardshipRecordByID(id);
            return View(model);
        }





    
    }
}
