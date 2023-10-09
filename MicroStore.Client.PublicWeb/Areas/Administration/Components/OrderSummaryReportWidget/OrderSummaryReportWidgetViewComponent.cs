using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Components.OrderSummaryReportWidget
{
    [Widget]
    public class OrderSummaryReportWidgetViewComponent : AbpViewComponent
    {
        private readonly OrderAnalysisService _orderAnaylsisService;
        public OrderSummaryReportWidgetViewComponent(OrderAnalysisService orderAnaylsisService)
        {
            _orderAnaylsisService = orderAnaylsisService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orderSummary = await _orderAnaylsisService.GetOrderSummaryReport();

            var model = new OrderSummaryReportWidgetViewComponentModel
            {
                SummaryReport = orderSummary
            };


            return View(model);
        }
    }

    public class OrderSummaryReportWidgetViewComponentModel
    {
       public OrderSummaryReport  SummaryReport { get; set; }
    }
}
