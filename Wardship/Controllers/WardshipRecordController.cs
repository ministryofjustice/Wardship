using System.Linq;
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

        // GET: /WardshipRecord/Create
        public ActionResult Create()
        {
            var model = new WardshipRecord
            {
                CaseTypeList = new SelectList(db.GetCaseTypes(), "CaseTypeID", "Description"),
                CourtList = new SelectList(db.GetCourts(), "CourtID", "Name"),
                TypeList = new SelectList(db.GetTypes(), "TypeID", "Description"),
                GenderList = new SelectList(db.GetGenders(), "GenderID", "Description"),
                DistrictJudgeList = new SelectList(db.GetDistrictJudges(), "JudgeID", "Name"),
                RecordList = new SelectList(db.GetRecords(), "RecordID", "Description"),
                LapsedList = new SelectList(db.GetLapsedStatuses(), "LapsedID", "Detail"),
                StatusList = new SelectList(db.GetStatuses(), "StatusID", "Description"),
                CWOList = new SelectList(db.GetCWOs(), "CWOID", "Name"),
                CAFCASSList = new SelectList(db.GetCAFCASSes(), "CAFCASSID", "Name")
            };
            return View(model);
        }

        // POST: /WardshipRecord/Create
        [HttpPost]
        public ActionResult Create(WardshipRecord model)
        {
            if (ModelState.IsValid)
            {
                db.AddWardshipRecord(model.WardshipRecord);
                return RedirectToAction("Index"); // Redirect to a suitable page after creation
            }

            // Reload dropdown lists if there's a need to return to the form
            model.CaseTypeList = new SelectList(db.GetCaseTypes(), "CaseTypeID", "Description", model.WardshipRecord.CaseTypeID);
            model.CourtList = new SelectList(db.GetCourts(), "CourtID", "Name", model.WardshipRecord.CourtID);
            model.TypeList = new SelectList(db.GetTypes(), "TypeID", "Description", model.WardshipRecord.TypeID);
            model.GenderList = new SelectList(db.GetGenders(), "GenderID", "Description", model.WardshipRecord.GenderID);
            model.DistrictJudgeList = new SelectList(db.GetDistrictJudges(), "JudgeID", "Name", model.WardshipRecord.DistrictJudgeID);
            model.RecordList = new SelectList(db.GetRecords(), "RecordID", "Description", model.WardshipRecord.RecordID);
            model.LapsedList = new SelectList(db.GetLapsedStatuses(), "LapsedID", "Detail", model.WardshipRecord.LapsedID);
            model.StatusList = new SelectList(db.GetStatuses(), "StatusID", "Description", model.WardshipRecord.StatusID);
            model.CWOList = new SelectList(db.GetCWOs(), "CWOID", "Name", model.WardshipRecord.CWOID);
            model.CAFCASSList = new SelectList(db.GetCAFCASSes(), "CAFCASSID", "Name", model.WardshipRecord.CAFCASSID);

            return View(model);
        }

    }
}
