using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
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

        private readonly BasketService _basketService;

        private readonly IWorkContext _workContext;
        public FeaturedProductListWidgetViewComponent(ProductService productService, BasketService basketService, IWorkContext workContext)
        {
            _productService = productService;
            _basketService = basketService;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int lenght = 6)
        {
            var requestOptions = new ProductListRequestOptions
            {
                IsFeatured = true,
                Length = lenght,
                SortBy = "creation",
                Desc = true
            };

            var productResult = await _productService.ListAsync(requestOptions);

            var basket = await _basketService.RetrieveAsync(_workContext.TryToGetCurrentUserId());

            var model = new FeaturedProductListWidgetModel
            {
                Products = productResult.Items,
                UserBasket = basket,
            };

            return View(model);
        }
    }

    public class FeaturedProductListWidgetModel
    {
        public List<Product> Products { get; set; } = new List<Product>();

        public MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart.Basket UserBasket { get; set; }
    }
}
