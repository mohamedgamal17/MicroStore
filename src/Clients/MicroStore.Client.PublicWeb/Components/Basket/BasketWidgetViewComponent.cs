using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.Basket
{
    [Widget(RefreshUrl = "/Widget/BasketWidget",
        AutoInitialize = true,
        StyleTypes = new[] { typeof(BasketWidgetStyleContributor) },
        ScriptTypes  = new [] {typeof(BasketWidgetScriptContributor) })]
    public class BasketWidgetViewComponent : AbpViewComponent
    {

        private readonly BasketAggregateService _basketAggregateService;

        private readonly IWorkContext _workContext;

        public BasketWidgetViewComponent(BasketAggregateService basketAggregateService, IWorkContext workContext)
        {
            _basketAggregateService = basketAggregateService;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _basketAggregateService.RetriveBasket(_workContext.TryToGetCurrentUserId());

            return View(model);

        }

    }
}
