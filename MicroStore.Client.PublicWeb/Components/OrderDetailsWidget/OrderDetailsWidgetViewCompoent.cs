using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.OrderDetailsWidget
{
    [Widget(AutoInitialize = true ,
        ScriptFiles = new string[] { "/Pages/Shared/Components/OrderDetailsWidget/order-details-widget.js" })]
    public class OrderDetailsWidgetViewCompoent : AbpViewComponent
    {
        private readonly UserOrderService _userOrderService;

        public OrderDetailsWidgetViewCompoent(UserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid orderId)
        {
            var order = await _userOrderService.GetAsync(orderId);

            var viewModel = new OrderDetailsWidgetViewModel
            {
                Order = order
            };

            return View(order);
        }

    }

    public class OrderDetailsWidgetViewModel
    {
        public Order Order { get; set; }
    }
}
