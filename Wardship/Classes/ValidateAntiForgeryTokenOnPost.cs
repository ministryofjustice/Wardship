using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using Wardship.Logger;

namespace Wardship
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public ITelemetryLogger Logger { get { return new TelemetryLogger(); } }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            try
            {
                string tokenInCookie = string.Empty;
                string tokenInForm = string.Empty;

                if (request.HttpMethod == WebRequestMethods.Http.Post)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var antiforgeryToken = request.Headers.Get("AntiForgeryToken");

                        if (!string.IsNullOrEmpty(antiforgeryToken))
                        {
                            tokenInCookie = antiforgeryToken.Split(':')[0].Trim();
                            tokenInForm = antiforgeryToken.Split(':')[1].Trim();
                        }

                        AntiForgery.Validate(tokenInCookie, tokenInForm);
                        return;
                    }
                }

                if (request.HttpMethod == WebRequestMethods.Http.Post)
                {
                    var formTokenValue = request.Form.Get(AntiForgeryConfig.CookieName);
                    var cookieTokenValue = request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = cookieTokenValue?.Value;
                    AntiForgery.Validate(cookieValue, formTokenValue);
                }
            }
            catch (HttpAntiForgeryException e)
            {
                //Log
                filterContext.Result = new ContentResult()
                {
                    Content =
                        "Forbidden Content.You do not currently have permission to access the page you have requested. <br /> " +
                        "If you feel this is incorrect," +
                        "please contact your local admin.",
                };
                Logger.LogError(e, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
                throw;
            }
        }
    }
}