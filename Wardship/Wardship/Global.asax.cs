using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Wardship.Models;
using System.Web.Security;

namespace Wardship
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new Filters.UserActivityAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
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


            //routes.MapRoute("Print"
            //                           , "{controller}/{action}/{WardshipCaseID}"
            //                           , new
            //                           {
            //                               controller = "QuickSearch"
            //                             ,
            //                               action = "Print"
            //                           }
            //                           , new
            //                           {
            //                               WardshipCaseID = UrlParameter.Optional     //UrlParameter.Optional // @"\d{1,9}"
                                     
            //                           }
            //                           );
            
            
            
            
            
            
            
            
            
            
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
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

           // Database.SetInitializer(new DBInitializer());
            System.Configuration.ConfigurationManager.AppSettings["CurServer"] = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString.Split(';').First().Split('=').Last();
            ServiceLayer.UnitOfWorkHelper.CurrentDataStore = new HttpContextDataStore();
        }
    }
}