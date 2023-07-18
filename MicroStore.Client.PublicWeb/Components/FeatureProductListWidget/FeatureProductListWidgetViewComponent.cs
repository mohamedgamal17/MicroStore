using Microsoft.AspNetCore.Mvc;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.FeatureProductListWidget
{
    [Widget(AutoInitialize = true,
        ScriptFiles = new string[] { "/Pages/Shared/Components/FeaturedProductListWidget/featured-product-list-widget.js" })]
    public class FeaturedProductListWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductService _productService;

        public FeaturedProductListWidgetViewComponent(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int lenght = 6)
        {
            var requestOptions = new ProductListRequestOptions
            {
                IsFeatured = true,
                Lenght = lenght,
            };

            var result = await _productService.ListAsync(requestOptions);

            var model = new FeaturedProductListWidgetModel
            {
                Products = result.Items
            };

            return View(model);
        }
    }

    public class FeaturedProductListWidgetModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
