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
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Detail");
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName");
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Detail");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Detail");
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "Detail");
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "Detail");
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "Detail");
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "Name");
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "Detail");
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "Detail");
            return View("Create");
        }

        // POST: WardshipRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WardshipCaseID,ChildSurname,ChildForenames,ChildDateofBirth,DateOfOS,FileNumber,FileDuplicate,Xreg,TypeID,CourtID,StatusID,GenderID,RecordID,LapsedID,CWOID,DistrictJudgeID,CaseTypeID,CAFCASSID,LapseLetterSent,FirstAppointmentDate,HearingDate,Username")] WardshipRecord wardshipRecord)
        {
            if (ModelState.IsValid)
            {
                db.AddWardshipRecord(wardshipRecord); // Use the repository to add the record
                db.SaveChanges(); // Save changes
                return RedirectToAction("Index");
            }

            // If model state is invalid, reload dropdown lists and return the view
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", wardshipRecord.TypeID);
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName", wardshipRecord.CourtID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Detail", wardshipRecord.StatusID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Detail", wardshipRecord.GenderID);
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "Detail", wardshipRecord.RecordID);
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "Detail", wardshipRecord.LapsedID);
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "Detail", wardshipRecord.CWOID);
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "Name", wardshipRecord.DistrictJudgeID);
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "Detail", wardshipRecord.CaseTypeID);
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "Detail", wardshipRecord.CAFCASSID);
            return View(wardshipRecord);
        }

        // GET: WardshipRecord/Edit/5
        public ActionResult Edit(int id)
        {
            WardshipRecord wardshipRecord = db.GetWardshipRecordByID(id);
            if (wardshipRecord == null)
            {
                return HttpNotFound();
            }

            // Prepare dropdown lists for the view
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Detail", wardshipRecord.TypeID);
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName", wardshipRecord.CourtID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Detail", wardshipRecord.StatusID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Detail", wardshipRecord.GenderID);
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "Detail", wardshipRecord.RecordID);
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "Detail", wardshipRecord.LapsedID);
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "Detail", wardshipRecord.CWOID);
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "Name", wardshipRecord.DistrictJudgeID);
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "Detail", wardshipRecord.CaseTypeID);
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "Detail", wardshipRecord.CAFCASSID);

            return View(wardshipRecord);
        }

        // POST: WardshipRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WardshipCaseID,ChildSurname,ChildForenames,ChildDateofBirth,DateOfOS,FileNumber,FileDuplicate,Xreg,TypeID,CourtID,StatusID,GenderID,RecordID,LapsedID,CWOID,DistrictJudgeID,CaseTypeID,CAFCASSID,LapseLetterSent,FirstAppointmentDate,HearingDate,Username")] WardshipRecord wardshipRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wardshipRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "WardshipRecord", new { id = wardshipRecord.WardshipCaseID });
            }

            // If model state is invalid, reload dropdown lists and return the view
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "TypeName", wardshipRecord.TypeID);
            ViewBag.CourtID = new SelectList(db.Courts, "CourtID", "CourtName", wardshipRecord.CourtID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Detail", wardshipRecord.StatusID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Detail", wardshipRecord.GenderID);
            ViewBag.RecordID = new SelectList(db.Records, "RecordID", "Detail", wardshipRecord.RecordID);
            ViewBag.LapsedID = new SelectList(db.Lapseds, "LapsedID", "Detail", wardshipRecord.LapsedID);
            ViewBag.CWOID = new SelectList(db.CWOs, "CWOID", "Detail", wardshipRecord.CWOID);
            ViewBag.DistrictJudgeID = new SelectList(db.DistrictJudges, "DistrictJudgeID", "Name", wardshipRecord.DistrictJudgeID);
            ViewBag.CaseTypeID = new SelectList(db.CaseTypes, "CaseTypeID", "Detail", wardshipRecord.CaseTypeID);
            ViewBag.CAFCASSID = new SelectList(db.CAFCASSs, "CAFCASSID", "Detail", wardshipRecord.CAFCASSID);

            return View(wardshipRecord);
        }
    }
}