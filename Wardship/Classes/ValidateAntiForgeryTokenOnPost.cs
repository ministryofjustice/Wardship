using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using TPLibrary.Logger;

namespace Wardship
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public const string HTTP_HEADER_NAME = "x-RequestVerificationToken";

        public ICloudWatchLogger Logger { get { return new CloudWatchLogger(); } }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            try
            {
                //Ajax Requests
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
            }
            catch (HttpAntiForgeryException ex)
            {
                //Log
                filterContext.Result = new ContentResult()
                {
                    Content =
                        "Forbidden Content.You do not currently have permission to access the page you have requested. <br /> " +
                        "If you feel this is incorrect," +
                        "please contact your local admin.",

                };
                Logger.LogError(ex, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
                throw;
            }
            catch (Exception ex)
            {
                filterContext.Result = new ContentResult()
                {
                    Content =
                        "Forbidden Content.You do not currently have permission to access the page you have requested. <br /> " +
                        "If you feel this is incorrect," +
                        "please contact your local admin.",

                };
                Logger.LogError(ex, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
                throw;
            }


            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {

                var headerTokenValue = request.Headers[HTTP_HEADER_NAME];
                // Ajax POSTs using jquery have a header set that defines the token.
                // However using unobtrusive ajax the token is still submitted normally in the form.
                // if the header is present then use it, else fall back to processing the form like normal
                if (headerTokenValue != null)
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;

                    AntiForgery.Validate(cookieValue, headerTokenValue);
                }
                else if ((!request.FilePath.Contains("ExportToExcel")) && (!request.FilePath.Contains("ListOfCasesExportToExcel")))
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
}


//Original antiforgery token.
//using System;
//using System.Net;
//using System.Web.Helpers;
//using System.Web.Mvc;
//using TPLibrary.Logger;

//namespace Wardship
//{
//    [AttributeUsage(AttributeTargets.Class)]
//    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
//    {
//        public ICloudWatchLogger Logger { get { return new CloudWatchLogger(); } }

//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var request = filterContext.HttpContext.Request;

//            try
//            {
//                string tokenInCookie = string.Empty;
//                string tokenInForm = string.Empty;

//                if (request.HttpMethod == WebRequestMethods.Http.Post)
//                {
//                    if (filterContext.HttpContext.Request.IsAjaxRequest())
//                    {
//                        var antiforgeryToken = request.Headers.Get("AntiForgeryToken");

//                        if (!string.IsNullOrEmpty(antiforgeryToken))
//                        {
//                            tokenInCookie = antiforgeryToken.Split(':')[0].Trim();
//                            tokenInForm = antiforgeryToken.Split(':')[1].Trim();
//                        }

//                        AntiForgery.Validate(tokenInCookie, tokenInForm);
//                        return;
//                    }
//                }

//                if (request.HttpMethod == WebRequestMethods.Http.Post)
//                {
//                    var formTokenValue = request.Form.Get(AntiForgeryConfig.CookieName);
//                    var cookieTokenValue = request.Cookies[AntiForgeryConfig.CookieName];
//                    var cookieValue = cookieTokenValue?.Value;
//                    AntiForgery.Validate(cookieValue, formTokenValue);
//                }
//            }
//            catch (HttpAntiForgeryException e)
//            {
//                //Log
//                filterContext.Result = new ContentResult()
//                {
//                    Content =
//                        "Forbidden Content.You do not currently have permission to access the page you have requested. <br /> " +
//                        "If you feel this is incorrect," +
//                        "please contact your local admin.",
//                };
//                Logger.LogError(e, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError(ex, $"Exception in ValidateAntiForgeryToken for user {filterContext.HttpContext.User.Identity.Name}");
//                throw;
//            }
//        }
//    }
//}