using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using Wardship.Models;
using Wardship.Controllers;
using System.Security.Principal;
using System.Web.Mvc;
using Wardship.Logger;

namespace Wardship.Tests.Controllers
{
    [TestClass]
    public class FAQControllerTest
    {
        [TestMethod]
        public void FAQControllerReturnsIndexPage()
        {
            {
                // Arrange
                FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
                testController.ControllerContext = new ControllerContext()
                {
                    Controller = testController,
                    RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
                };
                // Act
                ViewResult result = (ViewResult)testController.Index();

                // Assert
                Assert.AreEqual("Index", result.ViewName,
                    "Action doesn't return correct view!");
            }
        }
        [TestMethod]
        public void FAQControllerReturnsSingleFAQForLoggedOutUser()
        {
            {
                // Arrange
                FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
                testController.ControllerContext = new ControllerContext()
                {
                    Controller = testController,
                    RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity(""), new string[] { })), new RouteData())
                };
                // Act
                ViewResult result = (ViewResult)testController.Index();

                // Assert
                Assert.AreEqual(1, ((IEnumerable<Wardship.Models.FAQ>)result.Model).Count(),
                    "Action doesn't return correct view!");
            }
        }
        [TestMethod]
        public void FAQControllerReturnsThreeFAQForLoggedInUser()
        {
            {
                // Arrange
                FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
                testController.ControllerContext = new ControllerContext()
                {
                    Controller = testController,
                    RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("User1"), new string[] { "user" })), new RouteData())
                };
                // Act
                ViewResult result = (ViewResult)testController.Index();

                // Assert
                Assert.AreEqual(3, ((IEnumerable<Wardship.Models.FAQ>)result.Model).Count(),
                    "Action doesn't return correct view!");
            }
        }
        [TestMethod]
        public void FAQControllerReturnsForFAQForLoggedInAdminUser()
        {
            {
                // Arrange
                FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
                testController.ControllerContext = new ControllerContext()
                {
                    Controller = testController,
                    RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("AdminUser"), new string[] { "admin" })), new RouteData())
                };
                // Act
                ViewResult result = (ViewResult)testController.Index();

                // Assert
                Assert.AreEqual(4, ((IEnumerable<Wardship.Models.FAQ>)result.Model).Count(),
                    "Action doesn't return correct view!");
            }
        }
        [TestMethod]
        public void FAQControllerCanCreateNewFAQ()
        {
            // Arrange
            FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
            testController.ControllerContext = new ControllerContext()
            {
                Controller = testController,
                RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("AdminUser"), new string[] { "admin" })), new RouteData())
            };
            // Act
            FAQ faq = new FAQ { faqID = 5, loggedInUser = true, question = "When?", answer = "Now" };
            RedirectToRouteResult result = (RedirectToRouteResult)testController.Create(faq);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"], "Action doesn't create new FAQ!");
        }
        [TestMethod]
        public void FAQControllerCanAmendFAQ()
        {
            // Arrange
            FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
            testController.ControllerContext = new ControllerContext()
            {
                Controller = testController,
                RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("AdminUser"), new string[] { "admin" })), new RouteData())
            };
            // Act
            FAQ faq = new FAQ { faqID = 4, loggedInUser = true, question = "When?", answer = "Now" };
            RedirectToRouteResult result = (RedirectToRouteResult)testController.Edit(faq);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"], "Action doesn't amend FAQ!");
        }
        [TestMethod]
        public void FAQControllerShouldRedisplayWithErrorsIfFAQUpdateFails()
        {
            // Arrange
            FAQController testController = new FAQController(new MockRepository(new TelemetryLogger()), new TelemetryLogger());
            testController.ControllerContext = new ControllerContext()
            {
                Controller = testController,
                RequestContext = new RequestContext(new MockHttpContext(new GenericPrincipal(new GenericIdentity("AdminUser"), new string[] { "admin" })), new RouteData())
            };
            // Act
            FAQ faq = new FAQ { loggedInUser = true, question = "When?", answer = "Now" };
            var result = testController.Edit(faq) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected redisplay of view");
        }
    }
}
