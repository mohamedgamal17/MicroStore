using Microsoft.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
