using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Host.SystemWeb;

namespace Wardship
{
    public partial class Startup
    {
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        public void ConfigureAuth(IAppBuilder app)
        {
            if (string.IsNullOrEmpty(RedirectUri))
            {
                RedirectUri = "https://localhost:44397/";
            }
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                CookieManager = new SystemWebChunkingCookieManager()
            });
            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = aadInstance + tenantId
            };
            app.UseOpenIdConnectAuthentication(options);
        }

    }
}