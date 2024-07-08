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

        // GET: WardshipRecord/Create
        public ActionResult Create()
        {
            // Prepare dropdown lists for the view
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName");
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName");
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusName");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "RecordName");
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "LapsedName");
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "CWOName");
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "JudgeName");
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "CaseTypeName");
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "CAFCASSName");
            return View();
        }

        // POST: WardshipRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WardshipCaseID,ChildSurname,ChildForenames,ChildDateofBirth,DateOfOS,FileNumber,FileDuplicate,Xreg,TypeID,CourtID,StatusID,GenderID,RecordID,LapsedID,CWOID,DistrictJudgeID,CaseTypeID,CAFCASSID,LapseLetterSent,FirstAppointmentDate,HearingDate,Username")] WardshipRecord wardshipRecord)
        {
            if (ModelState.IsValid)
            {
                db.WardshipRecords.Add(wardshipRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If model state is invalid, reload dropdown lists and return the view
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", wardshipRecord.TypeID);
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName", wardshipRecord.CourtID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusName", wardshipRecord.StatusID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", wardshipRecord.GenderID);
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "RecordName", wardshipRecord.RecordID);
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "LapsedName", wardshipRecord.LapsedID);
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "CWOName", wardshipRecord.CWOID);
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "JudgeName", wardshipRecord.DistrictJudgeID);
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "CaseTypeName", wardshipRecord.CaseTypeID);
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "CAFCASSName", wardshipRecord.CAFCASSID);
            return View(wardshipRecord);
        }
    }
}