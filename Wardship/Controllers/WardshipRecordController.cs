﻿using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using PagedList;
using TPLibrary.Logger;

namespace Wardship.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    
    public class WardshipRecordController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public WardshipRecordController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
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
