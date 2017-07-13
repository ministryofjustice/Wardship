
using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Wardship
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public const string HTTP_HEADER_NAME = "x-RequestVerificationToken";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

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