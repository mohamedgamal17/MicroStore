using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public PagedList<OrderList> Orders { get; set; }


        private readonly OrderService _orderService;

        public IndexModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGet()
        {
            Orders = await _orderService.ListAsync(new PagingReqeustOptions
            {
                PageNumber = 1,
                PageSize = 100
            });


            return Page();
        }
    }
}
