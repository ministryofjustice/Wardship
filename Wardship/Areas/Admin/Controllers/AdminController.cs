using System.Web.Mvc;

namespace Wardship.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lookups()
        {
            return View();
        }
    }
}
