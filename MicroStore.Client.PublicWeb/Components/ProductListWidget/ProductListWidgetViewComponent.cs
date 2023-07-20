using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.ProductListWidget
{
    [Widget(
        AutoInitialize = true,
        RefreshUrl = "/Widget/ProductListWidget",
        ScriptFiles =new string[] { "/Pages/Shared/Components/ProductListWidget/product-list-widget.js" }
        )]
    public class ProductListWidgetViewComponent : AbpViewComponent
    {
        private readonly ProductService _productService;

        private readonly IWorkContext _workContext;

        private readonly BasketService _basketService;
        public ProductListWidgetViewComponent(ProductService productService, IWorkContext workContext, BasketService basketService)
        {
            _productService = productService;
            _workContext = workContext;
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int currentPage = 1 ,int pageSize = 24,bool isFeatured = false,string? category = null,string? manufacturer = null,  double? minPrice = null, double? maxPrice = null)
        {
            var requestOptions = new ProductListRequestOptions
            {
                Skip = (currentPage - 1) * pageSize,
                Lenght = pageSize,
                Category = category,
                Manufacturer = manufacturer,
                IsFeatured = isFeatured,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };


            var productResult = await _productService.ListAsync(requestOptions);

            var userBasket = await _basketService.RetrieveAsync(_workContext.TryToGetCurrentUserId());

            var model = new ProductListWidgetModel
            {
                Products = productResult.Items,
                Pager = new PagerModel(productResult.TotalCount, pageSize, productResult.PageNumber, pageSize, "/Products"),
                UserBasket = userBasket
            };

            return View(model);
        }
    }

    public class ProductListWidgetModel
    {
        public List<Product> Products { get; set; }
        public PagerModel Pager { get; set; }
        public MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart.Basket UserBasket { get; set; }
    }
}