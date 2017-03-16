using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wardship.Models;
using Wardship.Areas.Admin.Models;
using System.DirectoryServices.AccountManagement;
using System.Data;

namespace Wardship.Areas.Admin.Controllers
{
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
                catch (Exception)
                {
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
                db.UpdateUser(model.User);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
