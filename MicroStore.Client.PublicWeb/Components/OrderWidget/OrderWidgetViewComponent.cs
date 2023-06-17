using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.OrderWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/OrderWidget/order-widget.js" })
     ]
    public class OrderWidgetViewComponent : AbpViewComponent  
    {
        private const int PAGE_SIZE = 5;

        private readonly UserOrderService _userOrderService;

        public OrderWidgetViewComponent(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int currentPage = 1)
        {
            var requestOptions = new PagingReqeustOptions
            {
                Skip = (currentPage - 1) * PAGE_SIZE,
                Lenght = PAGE_SIZE
            };

            var response = await _userOrderService.ListAsync(requestOptions);

            var model = new OrderWidgetViewModel
            {
                Orders = response.Items,
                Pager = new PagerModel(response.TotalCount, response.TotalCount, response.PageNumber, response.PageSize, "/myorders")
            };

            return View(model);
        }
    }

    public class OrderWidgetViewModel
    {
        public List<Order> Orders { get; set; }
        public PagerModel Pager { get; set; }
    }


}
