using Microsoft.AspNetCore.Mvc;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Controllers;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Controllers
{
    public class DashboardController : BackEndController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
