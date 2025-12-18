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

        public IActionResult AddressListWidget()
        {
            return ViewComponent("AddressListWidget");
        }

        public IActionResult ProductListWidget(string? category , string? manufacturer , double? minPrice , double? maxPrice , int length = 24)
        {
            return ViewComponent("ProductListWidget", new
            {
                category = category,
                manufacturer = manufacturer,
                minPrice = minPrice,
                maxPrice = maxPrice,
                pageSize = length
            });
        }
    }
}
