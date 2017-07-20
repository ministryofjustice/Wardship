using System;
using System.Web.Mvc;
using Wardship.Models;
using Wardship.Areas.Admin.Models;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;

namespace Wardship.Areas.Admin.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class UsersController : Controller
    {
        string domainID = null;
		SourceRepository db = new SQLRepository();
        public UsersController(): this(new SQLRepository())
        { }
        public UsersController(SourceRepository repository)
        {
            db = repository;
            // domainID = "CDC0821.dom1.infra.int";// uncomment for DOM1/Live use
        }

        //
        // GET: /User/
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
        public ActionResult Index()
        {
            Wardship.Areas.Admin.Models.UserList model = new Models.UserList();
            model.Groups = db.GetAllGroups();
            model.Users = db.GetAllUsers();
            return View(model);
        }

        public ActionResult GetUsers(int id)
        {
            ADGroup grp = db.GetGroupByID(id);
            GroupList model = new GroupList();
            try
            {
                model.Name = grp.Name;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainID))
                {
                    using (GroupPrincipal gp = GroupPrincipal.FindByIdentity(pc, model.Name))
                    {
                        if (gp != null)
                        {
                            foreach (Principal principal in gp.Members)
                            {
                                GroupMember user = new GroupMember(principal.Name);
                                model.Members.Add(user);
                            }
                        }
                        else { model.ErrorMessage = "No Groups could be loaded"; }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Admin/UserController - GetUsers. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
            }
            return PartialView("_ListUsersForGroup", model);
        }

        public ActionResult Create()
        {
            UserAdminVM model = new UserAdminVM();
            model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail");
            //model.User = new User();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserAdminVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UserAdd(model.User);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Admin/UserController - Create. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                    return View(model);
                }
            }
            return View(model);
        }

        
        public ActionResult Edit(int id)
        {
            UserAdminVM model = new UserAdminVM();
            model.User = db.GetUserByID(id);
            model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail",model.User.RoleStrength);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(UserAdminVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UpdateUser(model.User);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Admin/UserController - Edit. User: " + User.Identity.Name + ". When: " + DateTime.Now + ". Exception: " + ex.ToString());
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
