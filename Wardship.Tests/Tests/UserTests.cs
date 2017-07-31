using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.Web.Routing;
using Wardship.Logger;

namespace Wardship.Tests
{
    [TestClass]
    public class UserTests
    {
        
        [TestMethod]
        public void AdminUserSeeAdminInRoleList()
        {
            //Arrange
            RequestContext requestContext1 = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Admin"), new string[] { "Admin" })), new RouteData());
            ISQLRepository rep = new MockRepository(requestContext1, new TelemetryLogger());

            //Act
            var Roles = rep.GetAllRoles();
            
            //Assert
            Assert.AreEqual(7, Roles.Count());
        }
        
        [TestMethod]
        public void ManagerCannotSeeAdminInRoleList()
        {
            //Arrange
            MockHttpContext bob = new MockHttpContext();
            
            RequestContext requestContext1 = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Manager"), new string[] { "Manager" })), new RouteData());
            ISQLRepository rep = new MockRepository(requestContext1, new TelemetryLogger());

            //Act
            var Roles = rep.GetAllRoles();

            //Assert
            Assert.AreEqual(6, Roles.Count());
        }
      
        [TestMethod]
        public void MutipleContextDifferentName()
        {
            // Arrange

            RequestContext requestContext1 = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("cbruce"), new string[] { "no_roles" })), new RouteData());
            RequestContext requestContext2 = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("dpenny"), new string[] { "no_roles" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());
           
            //Act
            string name1 = rep.GetUserByName(requestContext1.HttpContext.User.Identity.Name).DisplayName;
            string name2 = rep.GetUserByName(requestContext2.HttpContext.User.Identity.Name).DisplayName;

            // Assert
            Assert.AreNotEqual(name1,name2);

        }
        [TestMethod]
        public void ADUserNotInGroupAndNotExplicitlyNamedIsDeniedAccess()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual(AccessLevel.Denied,result);
        }
        [TestMethod]
        public void ExplicitlyNamedUserisAllowedAccess()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("cbruce"), new string[] { "no_roles" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreNotEqual(AccessLevel.Denied,result);
        }
        [TestMethod]
        public void UserCanbeManagerByGroupWithoutBeingNamedExplicitly()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "soldev\\gg_ssg_developer" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual(AccessLevel.Manager,result);
        }
        [TestMethod]
        public void UserWithExplicitPermissionsOverruleGroupPermissions()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("dpenny"), new string[] { "soldev\\gg_ssg_developer" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual(AccessLevel.ReadOnly,result);
            // Group permissions make this user Manager
            // user permissions make this user read-only
        }
        [TestMethod]
        public void UserCanBeDeniedByExplicitPermissions()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("ijones"), new string[] { "soldev\\gg_ssg_developer","soldev\\SSGDeveloper" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual(AccessLevel.Denied,result);
            // Group permissions make this user Admin
            // user permissions deny the user access
        }
        [TestMethod]
        public void UserNameWillBeDisplayedForNamedUser()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("ijones"), new string[] { "soldev\\gg_ssg_developer","soldev\\SSGDeveloper" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            string result = rep.curUserDisplay(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual("Ian Jones",result);           
        }
        [TestMethod]
        public void GroupNameWillBeDisplayedForUnknownUserInGroup()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("amason"), new string[] { "soldev\\gg_ssg_developer"})), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            string result = rep.curUserDisplay(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual("soldev\\gg_ssg_developer", result);
        }
        [TestMethod]
        public void ADUserNotInGroupAndNotExplicitlyNamedShowsADLoginName()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("soldev\\Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            string result = rep.curUserDisplay(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual("soldev\\Nonexistantuser", result);
        }
        [TestMethod]
        public void OldUsersMarkedAsDeactivated()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("oUser"), new string[] { "no_roles" })), new RouteData());
            ISQLRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            AccessLevel result = rep.UserAccessLevel(requestContext.HttpContext.User);

            // Assert
            Assert.AreEqual(AccessLevel.Deactivated,result);
            // Group permissions make this user Admin
            // user permissions deny the user access
            
        }
    }
}
