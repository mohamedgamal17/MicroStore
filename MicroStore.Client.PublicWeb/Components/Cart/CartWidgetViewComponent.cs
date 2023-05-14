using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.Cart
{
    [Widget(
        RefreshUrl = "/Widget/CartWidget",
        StyleFiles = new string[] { "/wwwroot/css/components/cart/cart-widget.css" }
        )]
    public class CartWidgetViewComponent :  AbpViewComponent
    {
        private readonly IWorkContext _workContext;

        private readonly BasketAggregateService _basketAggregateService;

        private readonly ILogger<CartWidgetViewComponent> _logger;

        public CartWidgetViewComponent(IWorkContext workContext, BasketAggregateService basketAggregateService, ILogger<CartWidgetViewComponent> logger)
        {
            _workContext = workContext;
            _basketAggregateService = basketAggregateService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _basketAggregateService.RetriveBasket(_workContext.TryToGetCurrentUserId());

            _logger.LogInformation("Cart widget");
            _logger.LogInformation($"Total Count : {model.Items.Count}");
            return View(model);
        } 


    }
}
