using Microsoft.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            string template = "~/Views/Errors/{0}.cshtml";

            return code switch
            {
                StatusCodes.Status401Unauthorized => View(string.Format(template, code)),
                StatusCodes.Status403Forbidden => View(string.Format(template, code)),
                StatusCodes.Status404NotFound => View(string.Format(template, code)),
                _ => View(string.Format(template, StatusCodes.Status500InternalServerError))
            };
        }
    }
}
