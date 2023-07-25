using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace MicroStore.Client.PublicWeb.Pages
{
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        private readonly UserOrderService _userOrderService;
        public int CurrentPage { get; set; } = 1;
        public List<Order> Orders { get; set; } = new List<Order>();
        public PagerModel Pager { get; set; }
        public MyOrdersModel(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1)
        {
            var requestOptions = new PagingReqeustOptions
            {
                Skip = (currentPage - 1) * 10,
                Lenght = 10
            };

            var response = await _userOrderService.ListAsync(requestOptions);


            Orders = response.Items;

            Pager = new PagerModel(response.TotalCount, response.TotalCount, response.PageNumber, response.PageSize, Url.Page("MyOrders"));

            CurrentPage = currentPage;

            return Page();
        }
    }
}
