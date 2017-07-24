using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Linq;
using Wardship.Models;
using System.Web;
using Wardship.Logger;

namespace Wardship
{
    public enum AccessLevel
    {
        Denied = -1,
        Deactivated = 0,
        ReadOnly = 25,
        User = 50,
        Manager = 75,
        Admin = 100
    }
    public class ICurrentUser: IPrincipal
    {
        private DateTime lastCheck { get; set; }
        private User SystemUser { get; set; }
        private DateTime lastActive { get; set; }
        public IIdentity Identity { get; private set; }

        private SourceRepository db { get; set; }

        public ICurrentUser(SourceRepository repository)
        {
            db = repository;
        }
        public ICurrentUser(IIdentity identity): this(new SQLRepository( new TelemetryLogger()))
        { 
            this.Identity = identity;
            SystemUser = db.GetUserByName(Identity.Name.Split('\\').Last());
        }

        public ICurrentUser(IIdentity identity, SourceRepository rep)
        {
            db = rep;
            this.Identity = identity;
            Update();
        }
        public DateTime LastActive
        {
            get
            {
                return lastActive;
            }
        }
        public AccessLevel AccessLevel
        {
            get
            {
                Update();
                return (AccessLevel)SystemUser.Role.strength;
            }
        }
        public string DisplayName
        {
            get
            {
                Update();
                return SystemUser.DisplayName;
            }
        }
        private void Update()
        {
            if (lastCheck < DateTime.Now.AddMinutes(-10))
            {
                SystemUser = db.GetUserByName(Identity.Name.Split('\\').Last());
                lastCheck = DateTime.Now;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRedirect : AuthorizeAttribute
    {
        private bool _isAuthorized;
        private AccessLevel UserAccessLevel;
        public string RedirectPrivateUrl = "~/Private/Index";
        public string RedirectUnAuthUrl = "~/Error/Unauthorised";
        public AccessLevel MinimumRequiredAccessLevel { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && UserAccessLevel == AccessLevel.Denied)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectPrivateUrl);
            }
            else if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && (UserAccessLevel < MinimumRequiredAccessLevel))
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUnAuthUrl);
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _isAuthorized = false;
            UserAccessLevel = AccessLevel.Denied;
            //check groups (strart with them for a bigger group target!)
            using (SourceRepository db = new SQLRepository(new TelemetryLogger()))
            {
                UserAccessLevel = (AccessLevel)db.UserAccessLevel(httpContext.User);
            }
            _isAuthorized = (UserAccessLevel > AccessLevel.Denied && UserAccessLevel >= MinimumRequiredAccessLevel);


            IIdentity user = httpContext.User.Identity;
            ICurrentUser cPrincipal = new ICurrentUser(user);
            httpContext.User = cPrincipal;

            return _isAuthorized;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }

    public sealed class LogonAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            //if (!skipAuthorization)
            //{
                base.OnAuthorization(filterContext);
            //}
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IIdentity user = httpContext.User.Identity;
            ICurrentUser cPrincipal = new ICurrentUser(user);
            httpContext.User = cPrincipal;

            return true; // always true as anonymous allowed
        }
    }
}