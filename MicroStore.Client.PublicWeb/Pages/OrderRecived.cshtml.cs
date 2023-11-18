using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;

namespace MicroStore.Client.PublicWeb.Pages
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class OrderRecivedModel : PageModel
    {
        private readonly UserOrderService _userOrderService;

        public Order Order { get; set; }

        public OrderRecivedModel(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public async Task<IActionResult> OnGet(string orderNumber)
        {
            var order = await _userOrderService.GetByOrderNumberAsync(orderNumber);

            Order = order;

            return Page();
        }
    }
}
