using Microsoft.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Controllers
{
    public class WidgetController :  Controller
    {

        public IActionResult CartWidget()
        {
            return ViewComponent("CartWidget");
        }



        public IActionResult BasketWidget()
        {
            return ViewComponent("BasketWidget");
        }
    }
}
