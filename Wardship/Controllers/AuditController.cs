using System.Web.Mvc;
using Wardship.Models;
using TPLibrary.Logger;

namespace Wardship.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AuditController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public AuditController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
        }

        public ActionResult Audit(string auditType, int id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            AuditEventViewModel model = new AuditEventViewModel();

            string stringID = id.ToString();
            string AuditName = string.Format("{0} ", auditType);
           // var auditEvents = db.AuditEventsGetAll().Where(s => s.RecordAddedTo == id).Union(db.AuditEventsGetAll().Where(s => s.auditEventDescription.AuditDescription.StartsWith(AuditName) && s.RecordChanged == stringID));
            model.auditType = auditType;
            model.itemID = stringID;
           // model.AuditEvents = auditEvents.OrderByDescending(s => s.EventDate).ToPagedList(page ?? 1, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
    }
}
