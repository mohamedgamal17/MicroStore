using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.RecommandedProductListWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/RecommandedProductListWidget/recommanded-product-list-widget.js" })]
    public class RecommandedProductListWidgetViewComponent : AbpViewComponent
    {

        private readonly ProductRecommendationService _productRecommendationService;

        public RecommandedProductListWidgetViewComponent(ProductRecommendationService productRecommendationService)
        {
            _productRecommendationService = productRecommendationService;
        }


        public  async Task<IViewComponentResult> InvokeAsync()
        {
            bool isUserAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

            if (isUserAuthenticated)
            {
                var requestOptions = new PagingReqeustOptions
                {
                    Length = 10
                };


                var response =  await _productRecommendationService.GetProductRecommendation(requestOptions);

                var model = new RecommandedProductListWidgetViewComponentModel
                {
                    Products = response.Items
                };

                return View(model);
            }
            else
            {
                var model = new RecommandedProductListWidgetViewComponentModel()
                {
                    Products = new List<Product>()
                };

                return View(model);
            }
        }
    }



    public class RecommandedProductListWidgetViewComponentModel
    {
        public List<Product> Products { get; set; }
    }
}
