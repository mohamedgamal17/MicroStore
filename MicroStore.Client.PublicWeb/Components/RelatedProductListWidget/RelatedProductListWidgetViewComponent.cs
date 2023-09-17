using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.RelatedProductListWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/RelatedProductListWidget/related-product-list-widget.js" })]
    public class RelatedProductListWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductRecommendationService _productRecommendationService;

        public RelatedProductListWidgetViewComponent(ProductRecommendationService productRecommendationService)
        {
            _productRecommendationService = productRecommendationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId,int length = 6)
        {
            var pagingOptions = new PagingReqeustOptions
            {
                Length = length,
                Skip = 0
            };

            var response =  await _productRecommendationService.GetSimilarItems(productId, pagingOptions);

            var model = new RelatedProductListWidgetModel
            {
                Products = response.Items
            };

            return View(model);
        }

    }

    public class RelatedProductListWidgetModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
