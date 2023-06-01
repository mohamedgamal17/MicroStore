using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using Volo.Abp.AspNetCore.Mvc;
namespace MicroStore.Client.PublicWeb.Components.CheckoutSummary
{
    public class CheckoutSummaryWidgetViewComponent : AbpViewComponent
    {
        private readonly IWorkContext _workContext;

        private readonly BasketAggregateService _basketAggregateService;

        private readonly PaymentSystemService _paymentSystemService;

        public CheckoutSummaryWidgetViewComponent(IWorkContext workContext, BasketAggregateService basketAggregateService, PaymentSystemService paymentSystemService)
        {
            _workContext = workContext;
            _basketAggregateService = basketAggregateService;
            _paymentSystemService = paymentSystemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Basket = await _basketAggregateService
                .RetriveBasket(_workContext.TryToGetCurrentUserId());

            var paymentSystems = await _paymentSystemService.ListAsync();

            ViewBag.PaymentMethods = paymentSystems.Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.Name

            }).ToList();

            return View();
        }
    }
}
