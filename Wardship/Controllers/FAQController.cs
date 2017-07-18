using System;
using System.Web.Mvc;
using Wardship.Models;

namespace Wardship.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class FAQController : Controller
    {
        SourceRepository db = new SQLRepository();
        public FAQController()
            : this(new SQLRepository())
        { }
        public FAQController(SourceRepository repository)
        {
            db = repository;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var faqs = db.FAQsGetAll();
                return View("Index", faqs);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var faqs = db.FAQsGetOnline();
                    return View("Index", faqs);
                }
                else
                {
                    var faqs = db.FAQsGetOffline();
                    return View("Index", faqs);
                }
            }
        }
        [AuthorizeRedirect(MinimumRequiredAccessLevel=AccessLevel.Manager)]
        public ActionResult Edit(int id)
        {
            FAQ faq = db.FAQGetbyID(id);
            return View(faq);
        }

        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(FAQ faq)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.FAQUpdate(faq);

                    return RedirectToAction("Index");
                }
                return View(faq);
            }
            catch
            {
                return View(faq);
            }
        }
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BusinessArea/Create

        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
        [HttpPost]
        public ActionResult Create(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.FAQAdd(faq);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(faq);
                }
            }
            return View(faq);
        }

    }
}
