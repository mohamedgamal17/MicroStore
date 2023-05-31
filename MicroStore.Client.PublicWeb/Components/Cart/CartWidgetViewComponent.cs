using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.Cart
{
    [Widget(
        AutoInitialize = true,
        RefreshUrl = "/Widget/CartWidget"
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

        private BasketAggregate GenerateBasketAggregate()
        {
            return new BasketAggregate
            {
                Items = new List<BasketItemAggregate>
                {
                    new BasketItemAggregate
                    {
                        Name = Guid.NewGuid().ToString(),
                        ShortDescription = Guid.NewGuid().ToString(),
                        Quantity = 2,
                        Price = 50 ,
                        Thumbnail = Guid.NewGuid().ToString(),
                    },new BasketItemAggregate
                    {
                        Name = Guid.NewGuid().ToString(),
                        ShortDescription = Guid.NewGuid().ToString(),
                        Quantity = 2,
                        Price = 50 ,
                        Thumbnail = Guid.NewGuid().ToString(),
                    },new BasketItemAggregate
                    {
                        Name = Guid.NewGuid().ToString(),
                        ShortDescription = Guid.NewGuid().ToString(),
                        Quantity = 2,
                        Price = 50 ,
                        Thumbnail = Guid.NewGuid().ToString(),
                    },new BasketItemAggregate
                    {
                        Name = Guid.NewGuid().ToString(),
                        ShortDescription = Guid.NewGuid().ToString(),
                        Quantity = 2,
                        Price = 50 ,
                        Thumbnail = Guid.NewGuid().ToString(),
                    },
                }
            };
        }


    }
}
