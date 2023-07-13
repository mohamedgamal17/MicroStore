using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;

namespace MicroStore.Client.PublicWeb.Pages
{
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        public string OrderFilter { get; set; }

        private readonly UserOrderService _userOrderService;

        public int CurrentPage { get; set; } = 1;

        public MyOrdersModel(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public IActionResult OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage;

            return Page();
        }
    }
}
