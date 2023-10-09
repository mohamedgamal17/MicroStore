using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Components.BestSellerWidget
{
    [Widget]
    public class BestSellerWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductAnalysisService _productAnalysisService;

        public BestSellerWidgetViewComponent(ProductAnalysisService productAnalysisService)
        {
            _productAnalysisService = productAnalysisService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var bestSellersByAmount = await _productAnalysisService.GetBestSellersReport(new PagingAndSortingRequestOptions
            {
                Length = 10,
                Skip = 0,
                SortBy = "amount",
                Desc = true
            });

            var bestSellersByQuantity = await _productAnalysisService.GetBestSellersReport(new PagingAndSortingRequestOptions
            {
                Length = 10,
                Skip = 0,
                SortBy = "quantity",
                Desc = true
            });


            var model = new BestSellerWidgetModel
            {
                ByAmount = bestSellersByAmount.Items,
                ByQuantity = bestSellersByQuantity.Items,
            };

            return View(model);
        }
    }

    public class BestSellerWidgetModel
    {
        public List<BestSellerReport> ByAmount { get; set; }

        public List<BestSellerReport> ByQuantity { get; set; }
    }
}
