using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Components.LatestOrderWidget
{
    [Widget(AutoInitialize = true)]
    public class LatestOrderWidgetViewComponent: AbpViewComponent
    {
        private readonly OrderService _orderService;

        public LatestOrderWidgetViewComponent(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var requestOptions = new OrderListRequestOptions
            {
                Length = 6,
                Skip = 0,
                SortBy = "submission_date",
                Desc = true
            };

            var orderResponse = await _orderService.ListAsync(requestOptions);

            var model = new LatestOrderWidgetViewComponentModel
            {
                Orders = orderResponse.Items
            };

            return View(model);
        }
    }

    public class LatestOrderWidgetViewComponentModel
    {
        public List<Order> Orders { get; set; }
    }
}
