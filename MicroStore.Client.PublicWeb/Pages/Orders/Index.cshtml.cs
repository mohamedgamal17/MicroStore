using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using Microsoft.AspNetCore.Authorization;
using MicroStore.Client.PublicWeb.Infrastructure;

namespace MicroStore.Client.PublicWeb.Pages.Orders
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class IndexModel : PageModel
    {
        private readonly UserOrderService _userOrderService;
        public int CurrentPage { get; set; } = 1;
        public List<Order> Orders { get; set; } = new List<Order>();
        public PagerModel Pager { get; set; }
        public IndexModel(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1)
        {
            var requestOptions = new PagingAndSortingRequestOptions
            {
                Skip = (currentPage - 1) * 10,
                Length = 10,
                SortBy = "submission_date",
                Desc = true              
            };

            var response = await _userOrderService.ListAsync(requestOptions);


            Orders = response.Items;

            Pager = new PagerModel(response.TotalCount, response.TotalCount, response.PageNumber, response.PageSize, Url.Page("/Orders/Index"));

            CurrentPage = currentPage;

            return Page();
        }
    }
}
