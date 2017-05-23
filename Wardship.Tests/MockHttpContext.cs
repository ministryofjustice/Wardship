using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace Wardship.Tests
{
    public class MockHttpContext : HttpContextBase
    {
        #region orig
        private IPrincipal _user;

        public MockHttpContext()
            : this(new GenericPrincipal(new GenericIdentity("cbruce"), new string[] { "Admin", "Users" }))
        { }
        public MockHttpContext(IPrincipal user)
        { _user = user; }

        public override IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                base.User = value;
            }
        }
        #endregion
    }
}
