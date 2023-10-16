using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Helpers;
using System.IdentityModel.Claims;
using System.Net;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Wardship.Infrastructure;
using Microsoft.IdentityModel.Protocols;
using TPLibrary.Logger;
using System.Data.Entity;
using Wardship.Models;

namespace Wardship
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        private ICloudWatchLogger _cloudWatchLogger;

        public MvcApplication()
        {
            _cloudWatchLogger = new CloudWatchLogger();
        }

        private static void BootstrapContainer()
         {
             _container = new WindsorContainer().Install(FromAssembly.This());
             ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(_container.Kernel));
         }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Audit", "{auditType}/Audit/{id}"
                                , new
                                {
                                    controller = "Audit",
                                    action = "Audit",
                                }
                                , new
                                {
                                    auditType = @"\D{1,20}",
                                    id = @"\d{1,6}"
                                });         
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
              "Root",
              "",
              new { controller = "Home", action = "Index", id = ""}
            );
        }
        private class HttpContextDataStore : ServiceLayer.IUnitOfWorkDataStore
        {
            public object this[string key]
            {
                get { return HttpContext.Current.Items[key]; }
                set { HttpContext.Current.Items[key] = value; }
            }
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)12288 | (SecurityProtocolType)48; // only allow TLSV1.2, TLSV1.3 and SSL3
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AreaRegistration.RegisterAllAreas();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //Database.SetInitializer<DataContext>(null);
            Database.SetInitializer(
               new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            BootstrapContainer();
            ServiceLayer.UnitOfWorkHelper.CurrentDataStore = new HttpContextDataStore();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Handle nonce exception
            var ex = Server.GetLastError();
            _cloudWatchLogger.LogError(ex, "Application_Error");

            if ((ex.GetType() == typeof(OpenIdConnectProtocolInvalidNonceException) && User.Identity.IsAuthenticated) && (ex.Message.StartsWith("OICE_20004") || ex.Message.Contains("IDX10311")))
            {
                Server.ClearError();
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}