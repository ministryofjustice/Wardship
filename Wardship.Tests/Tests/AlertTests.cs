using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using System.Security.Principal;
using Wardship.Models;
using Wardship.Logger;

namespace Wardship.Tests
{
    [TestClass]
    public class AlertTests
    {
        
        [TestMethod]
        public void ShouldBe3LiveAlerts()
        {
            // Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            SourceRepository rep = new MockRepository(new TelemetryLogger());

            // Act
            var result = rep.getCurrentAlerts();

            // Assert
            Assert.AreEqual(3,result.Count());
        }
        
        [TestMethod]
        public void ShouldOnlyBe1HighAlert()
        {
            //Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            SourceRepository rep = new MockRepository(new TelemetryLogger());

            //Act
            IEnumerable<Alert> results = rep.getCurrentAlerts();
            var result = results.Where(x => x.Status == AlertStatus.High);
            //Assert
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void ShouldOnlyBe1WarnAlert()
        {
            //Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            SourceRepository rep = new MockRepository(new TelemetryLogger());

            //Act
            IEnumerable<Alert> results = rep.getCurrentAlerts();
            var result = results.Where(x => x.Status == AlertStatus.Warning);
            //Assert
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void ShouldOnlyBe1OverdueAlert()
        {
            //Arrange
            RequestContext requestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("Nonexistantuser"), new string[] { "no_roles" })), new RouteData());
            SourceRepository rep = new MockRepository(new TelemetryLogger());

            //Act
            IEnumerable<Alert> results = rep.getCurrentAlerts();
            var result = results.Where(x => x.Status == AlertStatus.Overdue);
            //Assert
            Assert.AreEqual(1, result.Count());
        }
      
    }
}
