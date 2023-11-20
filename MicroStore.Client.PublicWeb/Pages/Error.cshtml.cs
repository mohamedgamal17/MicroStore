using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using System.Net;

namespace MicroStore.Client.PublicWeb.Pages
{
    public class ErrorModel : PageModel
    {

        public HttpStatusCode HttpStatusCode { get; set; }
        public string Info { get; set; }
        public void OnGet(int httpStatusCode)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = exceptionFeature?.Error ?? new Exception("Unhandled Exception");

            if (httpStatusCode == 0)
            {
                httpStatusCode = FindStatusCode(exception);
            }

            HttpStatusCode = (HttpStatusCode)httpStatusCode;

            Info = ConvertExceptionToError(exception);

            HttpContext.Response.StatusCode = httpStatusCode;

        }


        private string ConvertExceptionToError(Exception exception)
        {
            if (exception is MicroStoreClientException clientException)
            {
                return clientException.Error.Detail;
            }

            return exception.Message;
        }

        private int FindStatusCode(Exception exception)
        {

            if (exception is MicroStoreClientException clientException)
            {
                return (int)clientException.StatusCode;
            }

            return StatusCodes.Status500InternalServerError;
        }

        public string GetErrorPage(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status401Unauthorized => "~/Views/Errors/401.cshtml",
                StatusCodes.Status403Forbidden => "~/Views/Errors/403.cshtml",
                StatusCodes.Status404NotFound => "~/Views/Errors/404.cshtml",
                StatusCodes.Status502BadGateway => "~/Views/Errors/502.cshtml",
                _ => "~/Views/Errors/500.cshtml"
            };
        }
    }
}
